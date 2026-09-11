using OrgSys.SharedKernel;
using OrgSys.SharedKernel;
using Accounting.Application;
using OrgSys.SharedKernel;
using AutoMapper;
using System.Linq;
using System.Net;

namespace Accounting.Application.Journals.Commands
{
    public record PostJournalCommand(long Id) : ICommand, IUpdateCommand<Result>;

    public class PostJournalCommandHandler(
        IUnitOfWork _UnitOfWork,
        IRepository<Accounting.Domain.Journal> _Repository,
        IAccountingPeriodService _AccountingPeriodService,
        IMapper mapper, IServiceProvider _provider) :
        UpdateCommandHandler<PostJournalCommand, Accounting.Domain.Journal>(_UnitOfWork, _Repository, mapper, _provider)
    {
        public override async Task<Result> Handle(PostJournalCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var journal = await _Repository.GetByFilterAsync(x => x.Id == request.Id, "JournalItems");

                if (journal is null)
                    return new Result(HttpStatusCode.NotFound, [new Error("Journal not found")]);

                if (journal.Posted)
                    return new Result(HttpStatusCode.BadRequest, [new Error("Journal entry is already posted.")]);

                if (!string.IsNullOrEmpty(journal.RefranceTable))
                    return new Result(HttpStatusCode.Forbidden, [new Error("A journal created from a resource is controlled by that resource")]);

                var totalDebit = journal.JournalItems?.Sum(i => i.Debit) ?? 0;
                var totalCredit = journal.JournalItems?.Sum(i => i.Credit) ?? 0;
                if (totalDebit != totalCredit)
                    return new Result(HttpStatusCode.BadRequest, [new Error("Total Debit must equal total Credit before posting.")]);

                await _UnitOfWork.BeginTransactionAsync();
                try
                {
                    // Re-resolve against current DB state inside the posting transaction — the entry
                    // may have been created while the period was open and closed/locked since.
                    var resolution = await _AccountingPeriodService.ResolveAndValidateAsync(journal.Date, cancellationToken);
                    if (!resolution.Success)
                    {
                        await _UnitOfWork.RollbackAsync();
                        return new Result(HttpStatusCode.BadRequest, resolution.Errors);
                    }

                    var openingBalanceErrors = await _AccountingPeriodService.ValidateOpeningBalanceAsync(journal.JournalTypeId, journal.Date, resolution.FiscalYear!, journal.Id, cancellationToken);
                    if (openingBalanceErrors is { Count: > 0 })
                    {
                        await _UnitOfWork.RollbackAsync();
                        return new Result(HttpStatusCode.BadRequest, openingBalanceErrors);
                    }

                    journal.FiscalYearId = resolution.FiscalYear!.Id;
                    journal.FiscalPeriodId = resolution.FiscalPeriod!.Id;
                    journal.Posted = true;

                    await _Repository.UpdateAsync(journal);

                    if (await _UnitOfWork.SaveChangeAsync(cancellationToken) <= 0)
                    {
                        await _UnitOfWork.RollbackAsync();
                        return new Result(HttpStatusCode.InternalServerError, [new Error("Error saving changes")]);
                    }

                    await _UnitOfWork.CommitAsync();
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
                return new Result(HttpStatusCode.InternalServerError, [new Error(ex.Message)]);
            }
        }
    }
}
