namespace Parties.Application.PartyAddresses.Commands
{
    using OrgSys.SharedKernel;
    using AutoMapper;

    public sealed class UpdatePartyAddressCommand : PartyAddressDto, ICommand, IUpdateCommand<Result>;

    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Parties.Domain.PartyAddress> _Repository, IMapper mapper, IServiceProvider _provider) : UpdateCommandHandler<UpdatePartyAddressCommand, Parties.Domain.PartyAddress>(_UnitOfWork, _Repository, mapper, _provider)
    {

    }
}
