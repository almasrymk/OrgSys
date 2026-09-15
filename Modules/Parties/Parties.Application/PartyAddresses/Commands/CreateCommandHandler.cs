namespace Parties.Application.PartyAddresses.Commands
{
    using OrgSys.SharedKernel;
    using AutoMapper;

    public sealed class CreatePartyAddressCommand : PartyAddressDto, ICommand, ICreateCommand<Result>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Parties.Domain.PartyAddress> _Repository, IMapper mapper) : CreateCommandHandler<CreatePartyAddressCommand, Parties.Domain.PartyAddress>(_UnitOfWork, _Repository, mapper)
    {

    }
}
