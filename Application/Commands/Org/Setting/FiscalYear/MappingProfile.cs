using Application.Commands.Org.Setting.FiscalYear.Commands;
using AutoMapper;
using Domain.Entities;
using Application.DTOs;

public partial class MappingProfile : Profile
{
    public void FiscalYearMappingProfile()
    {
        #region FiscalYear
        CreateMap<FiscalYear, FiscalYearDto>()
        .ForMember(dest => dest.Periods, opt => opt.MapFrom(src => src.Periods));
        CreateMap<FiscalYearDto, FiscalYear>()
        .ForMember(dest => dest.Periods, opt => opt.MapFrom(src => src.Periods));

        CreateMap<FiscalYear, CreateFiscalYearCommand>();
        CreateMap<CreateFiscalYearCommand, FiscalYear>()
        .ForMember(dest => dest.Periods, opt => opt.MapFrom(src => src.Periods));
        CreateMap<FiscalYear, UpdateFiscalYearCommand>()
        .ForMember(dest => dest.Periods, opt => opt.MapFrom(src => src.Periods));
        CreateMap<UpdateFiscalYearCommand, FiscalYear>()
        .ForMember(dest => dest.Periods, opt => opt.MapFrom(src => src.Periods));
        CreateMap<FiscalYear, DeleteFiscalYearCommand>();
        CreateMap<DeleteFiscalYearCommand, FiscalYear>();

        CreateMap<FiscalYearDto, CreateFiscalYearCommand>();
        CreateMap<CreateFiscalYearCommand, FiscalYearDto>();
        CreateMap<FiscalYearDto, UpdateFiscalYearCommand>();
        CreateMap<UpdateFiscalYearCommand, FiscalYearDto>();
        #endregion
    }
}
