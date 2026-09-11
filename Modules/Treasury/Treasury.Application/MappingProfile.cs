namespace Treasury.Application;

using AutoMapper;

public partial class MappingProfile : Profile
{
    public MappingProfile()
    {
        BankMappingProfile();
        BankBranchMappingProfile();
        CashBoxMappingProfile();
        FinancialAccountMappingProfile();
        FinancialTypeMappingProfile();
        OutlayMappingProfile();
        FinancialMappingProfile();
    }
}
