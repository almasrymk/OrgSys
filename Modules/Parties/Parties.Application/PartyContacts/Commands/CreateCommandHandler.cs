namespace Parties.Application.PartyContacts.Commands
{
    using OrgSys.SharedKernel;
    using AutoMapper;

    public sealed class CreatePartyContactCommand : PartyContactDto, ICommand, ICreateCommand<Result>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Parties.Domain.PartyContact> _Repository, IMapper mapper) : CreateCommandHandler<CreatePartyContactCommand, Parties.Domain.PartyContact>(_UnitOfWork, _Repository, mapper)
    {

    }
}
