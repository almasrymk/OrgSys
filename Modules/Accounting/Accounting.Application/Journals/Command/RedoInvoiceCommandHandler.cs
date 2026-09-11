using OrgSys.SharedKernel;
using OrgSys.SharedKernel;
using OrgSys.SharedKernel;
using AutoMapper;
using System.Net;

namespace Accounting.Application.Journals.Commands
{
    public record RedoJournalCommand(long Id) : ICommand, IUpdateCommand<Result>;

    public class RedoJournalCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Accounting.Domain.Journal> _Repository,   IMapper mapper, IServiceProvider _provider
        ) : UpdateCommandHandler<RedoJournalCommand, Accounting.Domain.Journal>(_UnitOfWork, _Repository, mapper, _provider)
    {
        public override async Task<Result> Handle(RedoJournalCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var journal = await _Repository.GetByFilterAsync(x => x.Id == request.Id, await CreateInclude());


                if (journal is null)
                    return new Result(HttpStatusCode.NotFound, new List<Error> { new Error("Journal not found") });

                if (!string.IsNullOrEmpty(journal.RefranceTable))
                    return new Result(HttpStatusCode.Forbidden, new List<Error> { new Error("A journal created from a resource is controlled by that resource") });

                // Once a journal has been Posted, its accounting history is immutable — Redo can only
                // reopen a Cancelled Draft (never posted). A Posted journal is corrected by reversing it,
                // not by reopening it, and a Reversed journal's original accounting effect must stand.
                if (journal.Posted || journal.Status == OrgSys.SharedKernel.Status.Reversed)
                    return new Result(HttpStatusCode.Forbidden, new List<Error> { new Error("A posted or reversed journal entry cannot be redone.") });

                if (journal.Status != OrgSys.SharedKernel.Status.Cancel)
                    return new Result(HttpStatusCode.OK, null);

                journal.Status = OrgSys.SharedKernel.Status.New;

                var saved = await _UnitOfWork.SaveChangeAsync(cancellationToken);

                return saved > 0 ? new Result(HttpStatusCode.OK, null) : new Result(HttpStatusCode.InternalServerError, new List<Error> { new Error("Error saving changes") });
            }
            catch (Exception ex)
            {

                return new Result(HttpStatusCode.InternalServerError, new List<Error> { new Error("Error") });

            }

        }
        
    }
}
