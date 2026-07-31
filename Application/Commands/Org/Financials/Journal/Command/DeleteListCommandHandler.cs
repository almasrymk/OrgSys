namespace Application.Commands.Org.Financials.Journal.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using Domain.Abstraction;
    using Domain.Shared;
    using System.Linq.Expressions;

    public sealed record DeleteListJournalCommand(List<long> Ids) : ICommand, IDeleteListCommand<Result>;   

    public sealed class DeleteListCommandHandler(IUnitOfWork _UnitOfWork, 
        IRepository<Domain.Entities.Journal> _Repository,  
        IServiceProvider _provider) : DeleteCommandHandler<DeleteListJournalCommand, Domain.Entities.Journal>(_UnitOfWork, _Repository , _provider)
    {

        public override async Task<bool> RemoveDetails(DeleteListJournalCommand request)
        {
            foreach (var journalId in request.Ids)
            {
                var journal = await _Repository.GetByFilterAsync(i => i.Id == journalId, "JournalItems");

                if (journal == null)
                    continue;

                journal.JournalItems!.Clear();
            }

            return await _UnitOfWork.SaveChangeAsync() > 0;
        }

        public override Expression<Func<Domain.Entities.Journal, bool>> CreateFilter(DeleteListJournalCommand request)
        {
            return e => request.Ids.Contains(e.Id) && e.Status != Domain.Enums.Status.Deleted && e.Hide != true;
        }

    }
}