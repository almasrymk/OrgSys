namespace Accounting.Application;

using AutoMapper;

public partial class MappingProfile : Profile
{
    public void FiscalPeriodMappingProfile()
    {
        #region FiscalPeriod
        CreateMap<FiscalPeriod, FiscalPeriodDto>();
        CreateMap<FiscalPeriodDto, FiscalPeriod>()
        .ForMember(d => d.FiscalYear, o => o.Ignore());
        #endregion
    }
}
