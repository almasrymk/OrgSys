using Accounting.Contracts.IntegrationEvents;
using Accounting.Domain.Events;
using Accounting.Domain.Exceptions;
using Accounting.Domain.Repositories;
using OrgSys.SharedKernel;
using System.Net;

namespace Accounting.Application.Journals.Commands
{
    public record CancelJournalCommand(long Id) : ICommand, IUpdateCommand<Result>;

    public class CancelJournalCommandHandler(
        IUnitOfWork unitOfWork,
        IJournalRepository journalRepository,
        IIntegrationEventPublisher integrationEventPublisher) : ICommandHandler<CancelJournalCommand>
    {
        public async Task<Result> Handle(CancelJournalCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var journal = await journalRepository.GetByIdAsync(request.Id, cancellationToken);
                if (journal is null)
                    return new Result(HttpStatusCode.NotFound, [new Error("Journal not found")]);

                journal.Cancel();

                var saved = await unitOfWork.SaveChangeAsync(cancellationToken);
                if (saved <= 0 && journal.DomainEvents.Count > 0)
                    return new Result(HttpStatusCode.InternalServerError, [new Error("Error saving changes")]);

                foreach (var domainEvent in journal.DomainEvents.OfType<JournalCancelledDomainEvent>())
                    await integrationEventPublisher.PublishAsync(new JournalCancelledIntegrationEvent(domainEvent.JournalId), cancellationToken);
                journal.ClearDomainEvents();

                return new Result(HttpStatusCode.OK, null);
            }
            catch (AccountingDomainException ex)
            {
                return new Result(HttpStatusCode.Forbidden, [new Error(ex.Message)]);
            }
            catch (Exception)
            {
                return new Result(HttpStatusCode.InternalServerError, [new Error("Error")]);
            }
        }
    }
}
