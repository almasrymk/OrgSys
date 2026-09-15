namespace Parties.Application;

using AutoMapper;

public partial class MappingProfile : Profile
{
    public void CustomerProfileMappingProfile()
    {
        CreateMap<Parties.Domain.CustomerProfile, CustomerProfileDto>();
        CreateMap<CustomerProfileDto, Parties.Domain.CustomerProfile>();
    }
}
