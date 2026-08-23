using AutoMapper;
using Domain.Entities;
using Application.DTOs;

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
