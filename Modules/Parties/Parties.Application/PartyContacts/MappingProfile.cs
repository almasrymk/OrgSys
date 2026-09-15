namespace Parties.Application;

using Parties.Application.PartyContacts.Commands;
using AutoMapper;

public partial class MappingProfile : Profile
{
    public void PartyContactMappingProfile()
    {
        #region PartyContact
        CreateMap<Parties.Domain.PartyContact, PartyContactDto>();
        CreateMap<PartyContactDto, Parties.Domain.PartyContact>();

        CreateMap<Parties.Domain.PartyContact, CreatePartyContactCommand>();
        CreateMap<CreatePartyContactCommand, Parties.Domain.PartyContact>();
        CreateMap<Parties.Domain.PartyContact, UpdatePartyContactCommand>();
        CreateMap<UpdatePartyContactCommand, Parties.Domain.PartyContact>();
        CreateMap<Parties.Domain.PartyContact, DeletePartyContactCommand>();
        CreateMap<DeletePartyContactCommand, Parties.Domain.PartyContact>();

        CreateMap<PartyContactDto, CreatePartyContactCommand>();
        CreateMap<CreatePartyContactCommand, PartyContactDto>();
        CreateMap<PartyContactDto, UpdatePartyContactCommand>();
        CreateMap<UpdatePartyContactCommand, PartyContactDto>();
        #endregion
    }
}
