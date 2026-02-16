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

        // 1-3- Products
        ProductUnitMappingProfile();
        ProductMappingProfile();
        ClassificationMappingProfile();
        UnitMappingProfile();

        // 1-4- Dealers
        DealerGroupMappingProfile();
        DealerMappingProfile();

        // 1-5- Financial
        AccountMappingProfile();
        SafeMappingProfile();
        BankBranchMappingProfile();
        BankMappingProfile();
        CurrencyMappingProfile();

        CityMappingProfile();
        CountryMappingProfile();
    }
}