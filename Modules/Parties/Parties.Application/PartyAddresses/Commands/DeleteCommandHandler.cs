namespace Parties.Application.PartyAddresses.Commands
{
    using OrgSys.SharedKernel;
    using System.Linq.Expressions;

    public sealed record DeletePartyAddressCommand(long Id) : ICommand, IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Parties.Domain.PartyAddress> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeletePartyAddressCommand, Parties.Domain.PartyAddress>(_UnitOfWork, _Repository, _provider)
    {
        public override Expression<Func<Parties.Domain.PartyAddress, bool>> CreateFilter(DeletePartyAddressCommand request)
        {
            return e => e.Id == request.Id && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}
