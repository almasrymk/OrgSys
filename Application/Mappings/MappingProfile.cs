using AutoMapper;

public partial class MappingProfile : Profile
{
    public MappingProfile()
    {
        // 1- Data
        // 1-1- Organization
        BranchMappingProfile();
        StockMappingProfile();

        // 1-2- Security
        RoleMappingProfile();
        UserMappingProfile();

        // 1-3- Product
        ClassificationMappingProfile();
        UnitMappingProfile();

        // 1-4- Dealer
        DealerGroupMappingProfile();
        DealerMappingProfile();

        CityMappingProfile();
        CountryMappingProfile();
    }
}