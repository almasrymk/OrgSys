using Application.Commands.Org.Financials.Journal.Commands;
using AutoMapper;
using Domain.Entities;
using Application.DTOs;

public partial class MappingProfile : Profile
{

    public void JournalMappingProfile()
    {
        CreateMap<Journal, JournalDto>()
            .ForMember(dest => dest.JournalTypeName, opt => opt.MapFrom(src => src.JournalType!.Name))
            .ForMember(dest => dest.JournalItems, opt => opt.MapFrom(src => src.JournalItems));
        CreateMap<JournalDto, Journal>()
            .ForMember(dest => dest.FiscalYearId, opt => opt.Ignore())
            .ForMember(dest => dest.FiscalPeriodId, opt => opt.Ignore())
            .ForMember(dest => dest.Posted, opt => opt.Ignore());

        CreateMap<JournalType, JournalTypeDto>();
        CreateMap<JournalTypeDto, JournalType>();

        CreateMap<Journal, CreateJournalCommand>();

        CreateMap<Journal, UpdateJournalCommand>();

        CreateMap<Journal, DeleteJournalCommand>();
        CreateMap<DeleteJournalCommand, Journal>();

        CreateMap<Invoice, Journal>();
        CreateMap<Journal, Invoice>();

        CreateMap<JournalItem, JournalItemDto>();
        CreateMap<JournalItemDto, JournalItem>();

        CreateMap<CreateJournalCommand, Journal>()
            .ForMember(dest => dest.JournalItems, opt => opt.MapFrom(src => src.JournalItems))
            .ForMember(dest => dest.FiscalYearId, opt => opt.Ignore())
            .ForMember(dest => dest.FiscalPeriodId, opt => opt.Ignore())
            .ForMember(dest => dest.Posted, opt => opt.Ignore());
        CreateMap<UpdateJournalCommand, Journal>()
            .ForMember(dest => dest.JournalItems, opt => opt.MapFrom(src => src.JournalItems))
            .ForMember(dest => dest.FiscalYearId, opt => opt.Ignore())
            .ForMember(dest => dest.FiscalPeriodId, opt => opt.Ignore())
            .ForMember(dest => dest.Posted, opt => opt.Ignore());

        CreateMap<JournalDto, UpdateJournalCommand>()
            .ForMember(dest => dest.JournalItems, opt => opt.MapFrom(src => src.JournalItems));

        CreateMap<CreateJournalCommand, JournalDto>();
        CreateMap<JournalDto, CreateJournalCommand>()
            .ForMember(dest => dest.JournalItems, opt => opt.MapFrom(src => src.JournalItems));
    }
}
