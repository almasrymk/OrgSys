namespace Parties.Application.PartyContacts.Commands
{
    using OrgSys.SharedKernel;
    using AutoMapper;

    public sealed class UpdatePartyContactCommand : PartyContactDto, ICommand, IUpdateCommand<Result>;

    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Parties.Domain.PartyContact> _Repository, IMapper mapper, IServiceProvider _provider) : UpdateCommandHandler<UpdatePartyContactCommand, Parties.Domain.PartyContact>(_UnitOfWork, _Repository, mapper, _provider)
    {

    }
}
