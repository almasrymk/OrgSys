namespace Accounting.Application;

using AutoMapper;

public partial class MappingProfile : Profile
{
    public MappingProfile()
    {
        AccountTypeMappingProfile();
        AccountMappingProfile();
        FiscalPeriodMappingProfile();
        FiscalYearMappingProfile();
        JournalMappingProfile();
    }
}
