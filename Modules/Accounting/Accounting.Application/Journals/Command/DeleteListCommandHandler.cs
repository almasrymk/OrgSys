namespace Accounting.Application.Journals.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using System.Linq.Expressions;

    public sealed record DeleteListJournalCommand(List<long> Ids) : ICommand, IDeleteListCommand<Result>;   

    public sealed class DeleteListCommandHandler(IUnitOfWork _UnitOfWork, 
        IRepository<Accounting.Domain.Journal> _Repository,  
        IServiceProvider _provider) : DeleteCommandHandler<DeleteListJournalCommand, Accounting.Domain.Journal>(_UnitOfWork, _Repository , _provider)
    {

        public override async Task<bool> RemoveDetails(DeleteListJournalCommand request)
        {
            foreach (var journalId in request.Ids)
            {
                var journal = await _Repository.GetByFilterAsync(i => i.Id == journalId, "JournalItems");

                if (journal == null)
                    continue;

                journal.PrepareForDeletion();
            }

            return await _UnitOfWork.SaveChangeAsync() > 0;
        }

        public override Expression<Func<Accounting.Domain.Journal, bool>> CreateFilter(DeleteListJournalCommand request)
        {
            return e => request.Ids.Contains(e.Id) && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }

    }
}
