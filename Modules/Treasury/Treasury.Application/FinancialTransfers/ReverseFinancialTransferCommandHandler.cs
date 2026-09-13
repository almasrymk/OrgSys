namespace Treasury.Application.FinancialTransfers.Commands;

using Accounting.Contracts.Postings;
using MediatR;
using OrgSys.SharedKernel;
using Microsoft.Extensions.Logging;
using System.Net;

/// <summary>Correction path for a Posted Transfer. A Transfer shares ONE Journal between its TWO
/// linked <c>Financial</c> legs (both carry the same <see cref="Financial.JournalId"/> and the same
/// <see cref="Financial.FinancialTransferId"/>). A single-Financial-row reversal — pointed at one
/// transfer leg — would correctly reverse the shared Journal once, but leave the OTHER leg's Status
/// stale (still Approved), corrupting that account's transaction history. This command reverses the
/// shared Journal exactly once, via Accounting.Contracts.Postings.
/// ReverseAccountingDocumentJournalCommand (see Journal.CreateReversalForSourceDocument), and flips
/// BOTH legs to Reversed.</summary>
public sealed record ReverseFinancialTransferCommand(long FinancialTransferId) : ICommand, IUpdateCommand<Result>;

public sealed class ReverseFinancialTransferCommandHandler(
    IUnitOfWork _UnitOfWork,
    ISender _Sender,
    IRepository<Treasury.Domain.FinancialTransfer> _TransferRepository,
    IRepository<Financial> _FinancialRepository,
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

            await _UnitOfWork.BeginTransactionAsync();
            try
            {
                var reverseResult = await _Sender.Send(new ReverseAccountingDocumentJournalCommand("financialtransfer", transfer.Id), cancellationToken);
                if (reverseResult.Response is null)
                {
                    await _UnitOfWork.RollbackAsync();
                    return new Result(reverseResult.StatusCode, reverseResult.Errors);
                }

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
                logger.LogInformation("Transfer {TransferId} reversed by new Journal {ReversalId}", transfer.Id, reverseResult.Response.ReversalJournalId);
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
