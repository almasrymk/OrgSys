using AutoMapper;

public partial class MappingProfile : Profile
{
    public MappingProfile()
    {
        // 1- Data
        // 1-1- Organization
        BranchMappingProfile();
        StockMappingProfile();

        CityMappingProfile();
        CountryMappingProfile();
    }
}