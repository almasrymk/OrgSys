namespace Application.Commands.Org.Financials.Journal.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using Domain.Abstraction;
    using Domain.Shared;
    using Domain.Entities;
    using System.Linq.Expressions;

    public sealed record DeleteJournalCommand(long Id) : ICommand, IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork,IRepository<Journal> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteJournalCommand, Journal>(_UnitOfWork, _Repository, _provider)
    {

        public override Expression<Func<Domain.Entities.Journal, bool>> CreateFilter(DeleteJournalCommand request)
        {
            return e => e.Id == request.Id && e.Status != Domain.Enums.Status.Deleted && e.Hide != true;
        }

        public override async Task<bool> RemoveDetails(DeleteJournalCommand request)
        {
            var journal = await _Repository.GetByFilterAsync(i => i.Id == request.Id, "JournalItems");
            if (journal == null)
                return false;

            if (!string.IsNullOrEmpty(journal.RefranceTable))
                throw new InvalidOperationException("A journal created from a resource cannot be deleted");

            journal.JournalItems!.Clear();

            return await _UnitOfWork.SaveChangeAsync() > 0;
        }
    }
}
