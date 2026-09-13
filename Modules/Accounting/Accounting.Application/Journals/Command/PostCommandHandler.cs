using Accounting.Contracts.IntegrationEvents;
using Accounting.Domain.Events;
using Accounting.Domain.Exceptions;
using Accounting.Domain.Repositories;
using OrgSys.SharedKernel;
using System.Net;

namespace Accounting.Application.Journals.Commands
{
    public record PostJournalCommand(long Id) : ICommand, IUpdateCommand<Result>;

    public class PostJournalCommandHandler(
        IUnitOfWork unitOfWork,
        IJournalRepository journalRepository,
        IAccountRepository accountRepository,
        IAccountingPeriodService accountingPeriodService,
        IIntegrationEventPublisher integrationEventPublisher) : ICommandHandler<PostJournalCommand>
    {
        public async Task<Result> Handle(PostJournalCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var journal = await journalRepository.GetByIdAsync(request.Id, cancellationToken);
                if (journal is null)
                    return new Result(HttpStatusCode.NotFound, [new Error("Journal not found")]);

                // Fail fast on the cheap, transaction-free checks before ever touching
                // AccountingPeriodService or opening a transaction — Journal.Post enforces the exact
                // same rules again inside the transaction below (defense in depth against a race), so
                // nothing here is a rule Application owns; it is only an early-exit optimization.
                if (journal.Posted)
                    return new Result(HttpStatusCode.BadRequest, [new Error("Journal entry is already posted.")]);

                if (!string.IsNullOrEmpty(journal.RefranceTable))
                    return new Result(HttpStatusCode.Forbidden, [new Error("A journal created from a resource is controlled by that resource")]);

                if (!journal.IsBalanced)
                    return new Result(HttpStatusCode.BadRequest, [new Error("Total Debit must equal total Credit before posting.")]);

                await unitOfWork.BeginTransactionAsync();
                try
                {
                    // Re-resolve against current DB state inside the posting transaction — the entry
                    // may have been created while the period was open and closed/locked since.
                    var resolution = await accountingPeriodService.ResolveAndValidateAsync(journal.Date, cancellationToken);
                    if (!resolution.Success)
                    {
                        await unitOfWork.RollbackAsync();
                        return new Result(HttpStatusCode.BadRequest, resolution.Errors);
                    }

                    var openingBalanceErrors = await accountingPeriodService.ValidateOpeningBalanceAsync(journal.JournalTypeId, journal.Date, resolution.FiscalYear!, journal.Id, cancellationToken);
                    if (openingBalanceErrors is { Count: > 0 })
                    {
                        await unitOfWork.RollbackAsync();
                        return new Result(HttpStatusCode.BadRequest, openingBalanceErrors);
                    }

                    var accountIds = journal.JournalItems.Select(i => i.AccountId).Distinct().ToList();
                    var accounts = await accountRepository.GetByIdsAsync(accountIds, cancellationToken);

                    journal.Post(resolution.FiscalYear!, resolution.FiscalPeriod!, accounts);

                    if (await unitOfWork.SaveChangeAsync(cancellationToken) <= 0)
                    {
                        await unitOfWork.RollbackAsync();
                        return new Result(HttpStatusCode.InternalServerError, [new Error("Error saving changes")]);
                    }

                    await unitOfWork.CommitAsync();

                    foreach (var domainEvent in journal.DomainEvents.OfType<JournalPostedDomainEvent>())
                        await integrationEventPublisher.PublishAsync(
                            new JournalPostedIntegrationEvent(domainEvent.JournalId, domainEvent.FiscalYearId, domainEvent.FiscalPeriodId),
                            cancellationToken);
                    journal.ClearDomainEvents();

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
                return new Result(HttpStatusCode.InternalServerError, [new Error(ex.Message)]);
            }
        }
    }
}
