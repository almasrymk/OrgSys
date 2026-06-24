using Application.Commands.Org.Financials.Journal.Commands;
using AutoMapper;
using Domain.Entities;
using Application.DTOs;

public partial class MappingProfile : Profile
{

    public void JournalMappingProfile()
    {
        CreateMap<Journal, JournalDto>()
            .ForMember(dest => dest.JournalItems, opt => opt.MapFrom(src => src.JournalItems));
        CreateMap<JournalDto, Journal>();

        CreateMap<Journal, CreateJournalCommand>();
        CreateMap<CreateJournalCommand, Journal>();

        CreateMap<Journal, UpdateJournalCommand>();
        CreateMap<UpdateJournalCommand, Journal>();

        CreateMap<Journal, DeleteJournalCommand>();
        CreateMap<DeleteJournalCommand, Journal>();

        CreateMap<Invoice, Journal>();
        CreateMap<Journal, Invoice>();

        CreateMap<JournalItem, JournalItemDto>();
        CreateMap<JournalItemDto, JournalItem>();

        CreateMap<CreateJournalCommand, Journal>().ForMember(dest => dest.JournalItems,opt => opt.MapFrom(src => src.JournalItems));  
        CreateMap<UpdateJournalCommand, Journal>().ForMember(dest => dest.JournalItems, opt => opt.MapFrom(src => src.JournalItems));
    }
}