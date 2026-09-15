namespace MasterData.Application;

using AutoMapper;

public partial class MappingProfile : Profile
{
    public MappingProfile()
    {
        CountryMappingProfile();
        CityMappingProfile();
        DistrictMappingProfile();
        CurrencyMappingProfile();
        ReferenceTypeMappingProfile();
        PaymentTypeMappingProfile();
    }
}
