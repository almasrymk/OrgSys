namespace Accounting.Application.Journals.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using System.Linq.Expressions;

    public sealed record DeleteJournalCommand(long Id) : ICommand, IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork,IRepository<Journal> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteJournalCommand, Journal>(_UnitOfWork, _Repository, _provider)
    {

        public override Expression<Func<Accounting.Domain.Journal, bool>> CreateFilter(DeleteJournalCommand request)
        {
            return e => e.Id == request.Id && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }

        public override async Task<bool> RemoveDetails(DeleteJournalCommand request)
        {
            var journal = await _Repository.GetByFilterAsync(i => i.Id == request.Id, "JournalItems");
            if (journal == null)
                return false;

            journal.PrepareForDeletion();

            return await _UnitOfWork.SaveChangeAsync() > 0;
        }
    }
}
