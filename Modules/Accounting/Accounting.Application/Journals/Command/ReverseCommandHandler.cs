using Accounting.Contracts.IntegrationEvents;
using Accounting.Domain.Exceptions;
using Accounting.Domain.Repositories;
using OrgSys.SharedKernel;
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
        IUnitOfWork unitOfWork,
        IJournalRepository journalRepository,
        IAccountingPeriodService accountingPeriodService,
        IIntegrationEventPublisher integrationEventPublisher,
        ILogger<ReverseJournalCommandHandler> logger) : ICommandHandler<ReverseJournalCommand>
    {
        public async Task<Result> Handle(ReverseJournalCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var original = await journalRepository.GetByIdAsync(request.Id, cancellationToken);
                if (original is null)
                    return new Result(HttpStatusCode.NotFound, [new Error("Journal not found")]);

                // Fail fast on the cheap, transaction-free checks before ever touching
                // AccountingPeriodService or opening a transaction — Journal.CreateReversal enforces
                // the exact same rules again inside the transaction below (defense in depth against a
                // race), so nothing here is a rule Application owns; it is only an early-exit optimization.
                if (!original.Posted)
                    return new Result(HttpStatusCode.BadRequest, [new Error("Only a Posted journal entry can be reversed.")]);

                if (original.Status == Status.Reversed || original.ReversalJournal is not null)
                    return new Result(HttpStatusCode.BadRequest, [new Error("This journal entry has already been reversed.")]);

                if (!string.IsNullOrEmpty(original.RefranceTable))
                    return new Result(HttpStatusCode.Forbidden, [new Error("A journal created from a resource is controlled by that resource")]);

                if (original.JournalItems.Count == 0)
                    return new Result(HttpStatusCode.BadRequest, [new Error("Journal entry has no lines to reverse.")]);

                await unitOfWork.BeginTransactionAsync();
                try
                {
                    // The reversal is dated today, not backdated to the original's date — the original's
                    // period may since be closed, and a reversal must land in a currently Open period.
                    var reversalDate = DateTime.Now.Date;
                    var resolution = await accountingPeriodService.ResolveAndValidateAsync(reversalDate, cancellationToken);
                    if (!resolution.Success)
                    {
                        await unitOfWork.RollbackAsync();
                        return new Result(HttpStatusCode.BadRequest, resolution.Errors);
                    }

                    var codeNumber = await journalRepository.GetNextCodeNumberAsync(original.TypeId, cancellationToken);

                    var reversal = original.CreateReversal(codeNumber, reversalDate, resolution.FiscalYear!, resolution.FiscalPeriod!);

                    await journalRepository.AddAsync(reversal, cancellationToken);

                    if (await unitOfWork.SaveChangeAsync(cancellationToken) <= 0)
                    {
                        await unitOfWork.RollbackAsync();
                        return new Result(HttpStatusCode.InternalServerError, [new Error("Error saving changes")]);
                    }

                    await unitOfWork.CommitAsync();

                    // reversal.Id only exists after SaveChangeAsync (DB-generated identity), so the
                    // domain fact is published directly here rather than collected on the aggregate —
                    // see Journal's own DomainEvents doc comment.
                    await integrationEventPublisher.PublishAsync(
                        new JournalReversedIntegrationEvent(original.Id, reversal.Id),
                        cancellationToken);

                    original.ClearDomainEvents();
                    logger.LogInformation("Journal {OriginalId} reversed by new Journal {ReversalId}", original.Id, reversal.Id);
                    return new Result(HttpStatusCode.OK, null);
                }
                catch
                {
                    await unitOfWork.RollbackAsync();
                    throw;
                }
            }
            catch (JournalControlledByResourceException ex)
            {
                return new Result(HttpStatusCode.Forbidden, [new Error(ex.Message)]);
            }
            catch (AccountingDomainException ex)
            {
                return new Result(HttpStatusCode.BadRequest, [new Error(ex.Message)]);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to reverse Journal {Id}", request.Id);
                return new Result(HttpStatusCode.InternalServerError, [new Error(ex.Message)]);
            }
        }
    }
}
