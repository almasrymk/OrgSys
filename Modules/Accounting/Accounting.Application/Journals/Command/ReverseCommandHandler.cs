using OrgSys.SharedKernel;
using OrgSys.SharedKernel;
using Accounting.Application;
using OrgSys.SharedKernel;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Accounting.Application.Journals.Commands
{
    public record ReverseJournalCommand(long Id) : ICommand, IUpdateCommand<Result>;

    /// <summary>
    /// Books a proper accounting reversal for a Posted journal: the original entry and its lines are
    /// never touched — a brand new Posted journal is created with every line's Debit/Credit swapped,
    /// the two entries are linked, and the original's Status flips to Reversed. Everything happens in
    /// one transaction. This is the only supported way to undo a Posted journal's accounting effect;
    /// Cancel is reserved for voiding an unposted Draft.
    /// </summary>
    public class ReverseJournalCommandHandler(
        IUnitOfWork _UnitOfWork,
        IRepository<Accounting.Domain.Journal> _Repository,
        IAccountingPeriodService _AccountingPeriodService,
        IMapper mapper,
        IServiceProvider _provider,
        ILogger<ReverseJournalCommandHandler> logger) :
        UpdateCommandHandler<ReverseJournalCommand, Accounting.Domain.Journal>(_UnitOfWork, _Repository, mapper, _provider)
    {
        public override async Task<Result> Handle(ReverseJournalCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var original = await _Repository.GetByFilterAsync(x => x.Id == request.Id, "JournalItems");

                if (original is null)
                    return new Result(HttpStatusCode.NotFound, [new Error("Journal not found")]);

                if (!original.Posted)
                    return new Result(HttpStatusCode.BadRequest, [new Error("Only a Posted journal entry can be reversed.")]);

                // Check both the Status flag and the relationship itself, so a duplicate reversal can't
                // slip through even if one of the two were ever left inconsistent (e.g. by a direct DB edit).
                if (original.Status == Status.Reversed || original.ReversalJournal is not null)
                    return new Result(HttpStatusCode.BadRequest, [new Error("This journal entry has already been reversed.")]);

                if (!string.IsNullOrEmpty(original.RefranceTable))
                    return new Result(HttpStatusCode.Forbidden, [new Error("A journal created from a resource is controlled by that resource")]);

                if (original.JournalItems is null || original.JournalItems.Count == 0)
                    return new Result(HttpStatusCode.BadRequest, [new Error("Journal entry has no lines to reverse.")]);

                await _UnitOfWork.BeginTransactionAsync();
                try
                {
                    // The reversal is dated today, not backdated to the original's date — the original's
                    // period may since be closed, and a reversal must land in a currently Open period.
                    var reversalDate = DateTime.Now.Date;
                    var resolution = await _AccountingPeriodService.ResolveAndValidateAsync(reversalDate, cancellationToken);
                    if (!resolution.Success)
                    {
                        await _UnitOfWork.RollbackAsync();
                        return new Result(HttpStatusCode.BadRequest, resolution.Errors);
                    }

                    var maxCodeNumber = await _Repository.GetMaxByFilterAsync(e => e.TypeId == original.TypeId, e => e.CodeNumber);
                    var codeNumber = maxCodeNumber + 1;

                    var reversal = new Accounting.Domain.Journal
                    {
                        JournalTypeId = original.JournalTypeId,
                        CurrencyId = original.CurrencyId,
                        Rate = original.Rate,
                        Date = reversalDate,
                        FiscalYearId = resolution.FiscalYear!.Id,
                        FiscalPeriodId = resolution.FiscalPeriod!.Id,
                        Note = $"Reversal of Journal Entry {original.Code}",
                        TypeId = original.TypeId,
                        ParentId = original.ParentId,
                        CodeNumber = codeNumber,
                        Code = codeNumber.ToString(),
                        CreateUserId = original.CreateUserId,
                        CreateDate = DateTime.Now,
                        Posted = true,
                        Status = Status.New,
                        OriginalJournalId = original.Id,
                        JournalItems = original.JournalItems.Select(line => new JournalItem
                        {
                            AccountId = line.AccountId,
                            // The whole point of a reversal: swap Debit and Credit on every line.
                            Debit = line.Credit,
                            Credit = line.Debit,
                            Note = line.Note,
                            Status = Status.New
                        }).ToList()
                    };

                    await _Repository.CreateAsync(reversal);

                    original.Status = Status.Reversed;
                    await _Repository.UpdateAsync(original);

                    if (await _UnitOfWork.SaveChangeAsync(cancellationToken) <= 0)
                    {
                        await _UnitOfWork.RollbackAsync();
                        return new Result(HttpStatusCode.InternalServerError, [new Error("Error saving changes")]);
                    }

                    await _UnitOfWork.CommitAsync();
                    logger.LogInformation("Journal {OriginalId} reversed by new Journal {ReversalId}", original.Id, reversal.Id);
                    return new Result(HttpStatusCode.OK, null);
                }
                catch
                {
                    await _UnitOfWork.RollbackAsync();
                    throw;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to reverse Journal {Id}", request.Id);
                return new Result(HttpStatusCode.InternalServerError, [new Error(ex.Message)]);
            }
        }
    }
}
