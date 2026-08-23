namespace Application.Commands.Org.Financials.FinancialTransfer.Commands;

using Application.Abstraction.Command;
using Application.Common.Services;
using Application.Interfaces.CQRS;
using Domain.Abstraction;
using Domain.Entities;
using Domain.Enums;
using Domain.Shared;
using Microsoft.Extensions.Logging;
using System.Net;

/// <summary>Correction path for a Posted Transfer. A Transfer shares ONE Journal between its TWO
/// linked <c>Financial</c> legs (both carry the same <see cref="Financial.JournalId"/> and the same
/// <see cref="Financial.FinancialTransferId"/>). The generic <c>ReverseCustomerReceiptCommand</c>
/// reverses by a single Financial row's Id — pointed at one transfer leg it would correctly reverse
/// the shared Journal once, but leave the OTHER leg's Status stale (still Approved), corrupting that
/// account's transaction history. This command reverses the shared Journal exactly once (mirroring
/// <c>Journal/Command/ReverseCommandHandler.cs</c>/<c>ReverseCustomerReceiptCommandHandler.cs</c>) and
/// flips BOTH legs to Reversed.</summary>
public sealed record ReverseFinancialTransferCommand(long FinancialTransferId) : ICommand, IUpdateCommand<Result>;

public sealed class ReverseFinancialTransferCommandHandler(
    IUnitOfWork _UnitOfWork,
    IRepository<Domain.Entities.FinancialTransfer> _TransferRepository,
    IRepository<Financial> _FinancialRepository,
    IRepository<Journal> _JournalRepository,
    IAccountingPeriodService _AccountingPeriodService,
    ILogger<ReverseFinancialTransferCommandHandler> logger) : ICommandHandler<ReverseFinancialTransferCommand>
{
    public async Task<Result> Handle(ReverseFinancialTransferCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var transfer = await _TransferRepository.GetByFilterAsync(e => e.Id == request.FinancialTransferId, string.Empty);
            if (transfer is null)
                return new Result(HttpStatusCode.NotFound, [new Error("Transfer not found.")]);

            if (!transfer.Posted)
                return new Result(HttpStatusCode.BadRequest, [new Error("Only a Posted transfer can be reversed.")]);

            if (transfer.Status == Status.Reversed)
                return new Result(HttpStatusCode.BadRequest, [new Error("This transfer has already been reversed.")]);

            var legs = (await _FinancialRepository.GetListByFilterAsync(e => e.FinancialTransferId == transfer.Id))?.ToList() ?? [];
            if (legs.Count != 2)
                return new Result(HttpStatusCode.BadRequest, [new Error("This transfer does not have exactly two linked financial movements to reverse.")]);

            var journalId = legs[0].JournalId;
            if (journalId is not > 0 || legs.Any(e => e.JournalId != journalId))
                return new Result(HttpStatusCode.BadRequest, [new Error("This transfer's financial movements do not share a single Journal Entry.")]);

            var original = await _JournalRepository.GetByFilterAsync(e => e.Id == journalId, "JournalItems");
            if (original is null || original.JournalItems is null || original.JournalItems.Count == 0)
                return new Result(HttpStatusCode.BadRequest, [new Error("The transfer's Journal Entry has no lines to reverse.")]);

            if (original.Status == Status.Reversed || original.ReversalJournal is not null)
                return new Result(HttpStatusCode.BadRequest, [new Error("This transfer's Journal Entry has already been reversed.")]);

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
                    Note = $"Reversal of Transfer Journal {original.Code}",
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

                foreach (var leg in legs)
                {
                    leg.Status = Status.Reversed;
                    await _FinancialRepository.UpdateAsync(leg);
                }

                transfer.Status = Status.Reversed;
                await _TransferRepository.UpdateAsync(transfer);

                if (await _UnitOfWork.SaveChangeAsync(cancellationToken) <= 0)
                {
                    await _UnitOfWork.RollbackAsync();
                    return new Result(HttpStatusCode.InternalServerError, [new Error("Error saving changes")]);
                }

                await _UnitOfWork.CommitAsync();
                logger.LogInformation("Transfer {TransferId} reversed by new Journal {ReversalId}", transfer.Id, reversal.Id);
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
            logger.LogError(ex, "Failed to reverse Transfer {Id}", request.FinancialTransferId);
            return new Result(HttpStatusCode.InternalServerError, [new Error(ex.Message)]);
        }
    }
}
