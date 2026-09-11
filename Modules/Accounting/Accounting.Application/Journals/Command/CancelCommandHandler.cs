using OrgSys.SharedKernel;
using OrgSys.SharedKernel;
using OrgSys.SharedKernel;
using AutoMapper;
using System.Net;

namespace Accounting.Application.Journals.Commands
{
    public record CancelJournalCommand(long Id) : ICommand, IUpdateCommand<Result>;

    public class CancelJournalCommandHandler(IUnitOfWork _UnitOfWork,   IRepository<Accounting.Domain.Journal> _Repository,  IMapper mapper, IServiceProvider _provider) :
        UpdateCommandHandler<CancelJournalCommand, Accounting.Domain.Journal>(_UnitOfWork, _Repository, mapper, _provider)
    {

        public override async Task<Result> Handle(CancelJournalCommand request, CancellationToken cancellationToken)
        {

            try
            {
                var journal = await _Repository.GetByFilterAsync(x => x.Id == request.Id, await CreateInclude());


                if (journal is null)
                    return new Result(HttpStatusCode.NotFound, new List<Error> { new Error("Journal not found") });

                if (!string.IsNullOrEmpty(journal.RefranceTable))
                    return new Result(HttpStatusCode.Forbidden, new List<Error> { new Error("A journal created from a resource is controlled by that resource") });

                // Cancel only voids an unposted Draft. A Posted journal's accounting effect must never be
                // altered directly — use ReverseJournalCommand instead, which books a proper reversing entry.
                if (journal.Posted)
                    return new Result(HttpStatusCode.Forbidden, new List<Error> { new Error("A posted journal entry cannot be cancelled. Use Reverse instead.") });

                if (journal.Status == OrgSys.SharedKernel.Status.Cancel)
                    return new Result(HttpStatusCode.OK, null);

                journal.Status = OrgSys.SharedKernel.Status.Cancel;
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
