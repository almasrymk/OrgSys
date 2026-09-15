namespace Parties.Application;

using AutoMapper;

public partial class MappingProfile : Profile
{
    public void SupplierProfileMappingProfile()
    {
        CreateMap<Parties.Domain.SupplierProfile, SupplierProfileDto>();
        CreateMap<SupplierProfileDto, Parties.Domain.SupplierProfile>();
    }
}
