namespace Application.Commands.Org.Financials.Receivable.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Services;
    using Application.Interfaces.CQRS;
    using Domain.Abstraction;
    using Domain.Entities;
    using Domain.Enums;
    using Domain.Shared;
    using Microsoft.Extensions.Logging;
    using System.Net;

    /// <summary>Correction path for a Posted Customer Receipt: the original <c>Financial</c> row and its
    /// Journal are never touched — a brand new Posted Journal is created with every line's Debit/Credit
    /// swapped (mirroring <c>Journal/Command/ReverseCommandHandler.cs</c>), linked via
    /// <see cref="Journal.OriginalJournalId"/>, and both the original Journal's and the Financial's
    /// Status flip to Reversed. The generic <c>ReverseJournalCommand</c> cannot be used here — it
    /// explicitly rejects any journal with <c>RefranceTable</c> set ("controlled by that resource").</summary>
    public sealed record ReverseCustomerReceiptCommand(long FinancialId) : ICommand, IUpdateCommand<Result>;

    public sealed class ReverseCustomerReceiptCommandHandler(
        IUnitOfWork _UnitOfWork,
        IRepository<Financial> _FinancialRepository,
        IRepository<Journal> _JournalRepository,
        IAccountingPeriodService _AccountingPeriodService,
        ILogger<ReverseCustomerReceiptCommandHandler> logger) : ICommandHandler<ReverseCustomerReceiptCommand>
    {
        public async Task<Result> Handle(ReverseCustomerReceiptCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var financial = await _FinancialRepository.GetByFilterAsync(e => e.Id == request.FinancialId, string.Empty);
                if (financial is null)
                    return new Result(HttpStatusCode.NotFound, [new Error("Receipt not found.")]);

                if (!financial.Posted)
                    return new Result(HttpStatusCode.BadRequest, [new Error("Only a Posted receipt can be reversed.")]);

                if (financial.Status == Status.Reversed)
                    return new Result(HttpStatusCode.BadRequest, [new Error("This receipt has already been reversed.")]);

                if (financial.JournalId is not > 0)
                    return new Result(HttpStatusCode.BadRequest, [new Error("This receipt has no Journal Entry to reverse.")]);

                var original = await _JournalRepository.GetByFilterAsync(e => e.Id == financial.JournalId, "JournalItems");
                if (original is null || original.JournalItems is null || original.JournalItems.Count == 0)
                    return new Result(HttpStatusCode.BadRequest, [new Error("The receipt's Journal Entry has no lines to reverse.")]);

                if (original.Status == Status.Reversed || original.ReversalJournal is not null)
                    return new Result(HttpStatusCode.BadRequest, [new Error("This receipt's Journal Entry has already been reversed.")]);

                await _UnitOfWork.BeginTransactionAsync();
                try
                {
                    var reversalDate = DateTime.Now.Date;
                    var resolution = await _AccountingPeriodService.ResolveAndValidateAsync(reversalDate, cancellationToken);
                    if (!resolution.Success)
                    {
                        await _UnitOfWork.RollbackAsync();
                        return new Result(HttpStatusCode.BadRequest, resolution.Errors);
                    }

                    var maxCodeNumber = await _JournalRepository.GetMaxByFilterAsync(e => e.TypeId == original.TypeId, e => e.CodeNumber);
                    var codeNumber = maxCodeNumber + 1;

                    var reversal = new Journal
                    {
                        JournalTypeId = original.JournalTypeId,
                        CurrencyId = original.CurrencyId,
                        Rate = original.Rate,
                        Date = reversalDate,
                        FiscalYearId = resolution.FiscalYear!.Id,
                        FiscalPeriodId = resolution.FiscalPeriod!.Id,
                        Note = $"Reversal of Customer Receipt Journal {original.Code}",
                        TypeId = original.TypeId,
                        CodeNumber = codeNumber,
                        Code = codeNumber.ToString(),
                        CreateUserId = original.CreateUserId,
                        CreateDate = DateTime.Now,
                        Posted = true,
                        Status = Status.New,
                        OriginalJournalId = original.Id,
                        // Kept resource-controlled, like the original, so it can't be re-reversed or
                        // manually Posted/Cancelled through the generic Journal endpoints.
                        RefranceTable = original.RefranceTable,
                        RefranceId = original.RefranceId,
                        RefranceCode = original.RefranceCode,
                        RefranceTypeId = original.RefranceTypeId,
                        JournalItems = original.JournalItems.Select(line => new JournalItem
                        {
                            AccountId = line.AccountId,
                            Debit = line.Credit,
                            Credit = line.Debit,
                            Note = line.Note,
                            Status = Status.New
                        }).ToList()
                    };

                    await _JournalRepository.CreateAsync(reversal);

                    original.Status = Status.Reversed;
                    await _JournalRepository.UpdateAsync(original);

                    financial.Status = Status.Reversed;
                    await _FinancialRepository.UpdateAsync(financial);

                    if (await _UnitOfWork.SaveChangeAsync(cancellationToken) <= 0)
                    {
                        await _UnitOfWork.RollbackAsync();
                        return new Result(HttpStatusCode.InternalServerError, [new Error("Error saving changes")]);
                    }

                    await _UnitOfWork.CommitAsync();
                    logger.LogInformation("Customer Receipt {FinancialId} reversed by new Journal {ReversalId}", financial.Id, reversal.Id);
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
                logger.LogError(ex, "Failed to reverse Customer Receipt {Id}", request.FinancialId);
                return new Result(HttpStatusCode.InternalServerError, [new Error(ex.Message)]);
            }
        }
    }
}
