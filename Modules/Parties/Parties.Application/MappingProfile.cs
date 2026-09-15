namespace Parties.Application;

using AutoMapper;

public partial class MappingProfile : Profile
{
    public MappingProfile()
    {
        DealerGroupMappingProfile();
        DealerMappingProfile();
        CustomerProfileMappingProfile();
        SupplierProfileMappingProfile();
        PartyContactMappingProfile();
        PartyAddressMappingProfile();
    }
}
