using Accounting.Domain.Exceptions;
using Accounting.Domain.Repositories;
using OrgSys.SharedKernel;
using System.Net;

namespace Accounting.Application.Journals.Commands
{
    public record RedoJournalCommand(long Id) : ICommand, IUpdateCommand<Result>;

    public class RedoJournalCommandHandler(
        IUnitOfWork unitOfWork,
        IJournalRepository journalRepository) : ICommandHandler<RedoJournalCommand>
    {
        public async Task<Result> Handle(RedoJournalCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var journal = await journalRepository.GetByIdAsync(request.Id, cancellationToken);
                if (journal is null)
                    return new Result(HttpStatusCode.NotFound, [new Error("Journal not found")]);

                var willTransition = journal.Status == Status.Cancel;
                journal.Redo();

                if (!willTransition)
                    return new Result(HttpStatusCode.OK, null);

                var saved = await unitOfWork.SaveChangeAsync(cancellationToken);
                return saved > 0
                    ? new Result(HttpStatusCode.OK, null)
                    : new Result(HttpStatusCode.InternalServerError, [new Error("Error saving changes")]);
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
