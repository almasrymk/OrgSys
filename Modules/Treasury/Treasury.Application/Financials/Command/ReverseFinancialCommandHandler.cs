namespace Treasury.Application.Financials.Commands
{
    using Accounting.Contracts.Postings;
    using MediatR;
    using OrgSys.SharedKernel;
    using Microsoft.Extensions.Logging;
    using System.Net;

    public sealed record ReverseFinancialCommand(long Id) : ICommand, IUpdateCommand<Result>;

    /// <summary>
    /// Reverse for a single-leg Posted <see cref="Financial"/> row (Receipt/Payment/Deposit/.../Opening
    /// Balance): delegates the Journal reversal itself to Accounting.Contracts.Postings.
    /// ReverseAccountingDocumentJournalCommand (the original Journal and its lines are never touched;
    /// a new Posted Journal is created with every line's Debit/Credit swapped, and the original's
    /// Status flips to Reversed — see Journal.CreateReversalForSourceDocument) and only flips this
    /// Financial row's own Status here. A Transfer leg (which shares ONE Journal between TWO
    /// Financial rows) is explicitly rejected here — use ReverseFinancialTransferCommand for that.
    /// </summary>
    public sealed class ReverseFinancialCommandHandler(
        IUnitOfWork unitOfWork,
        ISender sender,
        IRepository<Financial> financialRepository,
        ILogger<ReverseFinancialCommandHandler> logger) : ICommandHandler<ReverseFinancialCommand>
    {
        public async Task<Result> Handle(ReverseFinancialCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var financial = await financialRepository.GetByFilterAsync(e => e.Id == request.Id, string.Empty);
                if (financial is null)
                    return new Result(HttpStatusCode.NotFound, [new Error("Financial transaction not found.")]);

                if (!financial.Posted)
                    return new Result(HttpStatusCode.BadRequest, [new Error("Only a Posted financial transaction can be reversed.")]);

                if (financial.Status == Status.Reversed)
                    return new Result(HttpStatusCode.BadRequest, [new Error("This financial transaction has already been reversed.")]);

                if (financial.FinancialTransferId is not null)
                    return new Result(HttpStatusCode.BadRequest, [new Error("This is one leg of a Transfer — reverse the Transfer instead.")]);

                if (financial.JournalId is not > 0)
                    return new Result(HttpStatusCode.BadRequest, [new Error("This financial transaction has no linked Journal Entry to reverse.")]);

                await unitOfWork.BeginTransactionAsync();
                try
                {
                    var reverseResult = await sender.Send(new ReverseAccountingDocumentJournalCommand("financialtransaction", financial.Id), cancellationToken);
                    if (reverseResult.Response is null)
                    {
                        await unitOfWork.RollbackAsync();
                        return new Result(reverseResult.StatusCode, reverseResult.Errors);
                    }

                    financial.Status = Status.Reversed;
                    await financialRepository.UpdateAsync(financial);

                    if (await unitOfWork.SaveChangeAsync(cancellationToken) <= 0)
                    {
                        await unitOfWork.RollbackAsync();
                        return new Result(HttpStatusCode.InternalServerError, [new Error("Error saving changes")]);
                    }

                    await unitOfWork.CommitAsync();
                    logger.LogInformation("Financial {FinancialId} reversed by new Journal {ReversalId}", financial.Id, reverseResult.Response.ReversalJournalId);
                    return new Result(HttpStatusCode.OK, null);
                }
                catch
                {
                    await unitOfWork.RollbackAsync();
                    throw;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to reverse Financial {Id}", request.Id);
                return new Result(HttpStatusCode.InternalServerError, [new Error(ex.Message)]);
            }
        }
    }
}
