namespace Parties.Application.PartyAddresses.Commands
{
    using OrgSys.SharedKernel;
    using System.Linq.Expressions;

    public sealed record DeleteListPartyAddressCommand(List<long> Ids) : ICommand, IDeleteListCommand<Result>;

    public sealed class DeleteListCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Parties.Domain.PartyAddress> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteListPartyAddressCommand, Parties.Domain.PartyAddress>(_UnitOfWork, _Repository, _provider)
    {
        public override Expression<Func<Parties.Domain.PartyAddress, bool>> CreateFilter(DeleteListPartyAddressCommand request)
        {
            return e => request.Ids.Contains(e.Id) && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}
