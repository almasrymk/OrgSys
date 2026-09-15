namespace Parties.Application.PartyContacts.Commands
{
    using OrgSys.SharedKernel;
    using System.Linq.Expressions;

    public sealed record DeletePartyContactCommand(long Id) : ICommand, IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Parties.Domain.PartyContact> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeletePartyContactCommand, Parties.Domain.PartyContact>(_UnitOfWork, _Repository, _provider)
    {
        public override Expression<Func<Parties.Domain.PartyContact, bool>> CreateFilter(DeletePartyContactCommand request)
        {
            return e => e.Id == request.Id && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}
