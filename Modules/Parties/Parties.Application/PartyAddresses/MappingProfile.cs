namespace Parties.Application;

using Parties.Application.PartyAddresses.Commands;
using AutoMapper;

public partial class MappingProfile : Profile
{
    public void PartyAddressMappingProfile()
    {
        #region PartyAddress
        CreateMap<Parties.Domain.PartyAddress, PartyAddressDto>()
        .ForMember(dest => dest.CountryName, opt => opt.Ignore())
        .ForMember(dest => dest.CityName, opt => opt.Ignore())
        .ForMember(dest => dest.DistrictName, opt => opt.Ignore());
        CreateMap<PartyAddressDto, Parties.Domain.PartyAddress>();

        CreateMap<Parties.Domain.PartyAddress, CreatePartyAddressCommand>();
        CreateMap<CreatePartyAddressCommand, Parties.Domain.PartyAddress>();
        CreateMap<Parties.Domain.PartyAddress, UpdatePartyAddressCommand>();
        CreateMap<UpdatePartyAddressCommand, Parties.Domain.PartyAddress>();
        CreateMap<Parties.Domain.PartyAddress, DeletePartyAddressCommand>();
        CreateMap<DeletePartyAddressCommand, Parties.Domain.PartyAddress>();

        CreateMap<PartyAddressDto, CreatePartyAddressCommand>();
        CreateMap<CreatePartyAddressCommand, PartyAddressDto>();
        CreateMap<PartyAddressDto, UpdatePartyAddressCommand>();
        CreateMap<UpdatePartyAddressCommand, PartyAddressDto>();
        #endregion
    }
}
