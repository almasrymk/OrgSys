namespace Parties.Application.PartyContacts.Commands
{
    using OrgSys.SharedKernel;
    using System.Linq.Expressions;

    public sealed record DeleteListPartyContactCommand(List<long> Ids) : ICommand, IDeleteListCommand<Result>;

    public sealed class DeleteListCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Parties.Domain.PartyContact> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteListPartyContactCommand, Parties.Domain.PartyContact>(_UnitOfWork, _Repository, _provider)
    {
        public override Expression<Func<Parties.Domain.PartyContact, bool>> CreateFilter(DeleteListPartyContactCommand request)
        {
            return e => request.Ids.Contains(e.Id) && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}
