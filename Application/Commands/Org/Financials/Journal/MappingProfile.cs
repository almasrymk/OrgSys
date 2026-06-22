using Application.Commands.Org.Financials.Journal.Commands;
using AutoMapper;
using Domain.Entities;
using Application.DTOs;

public partial class MappingProfile : Profile
{

    public void JournalMappingProfile()
    {
        CreateMap<Journal, JournalModelView>()
            .ForMember(dest => dest.JournalItems, opt => opt.MapFrom(src => src.JournalItems));
        CreateMap<JournalModelView, Journal>();

        CreateMap<Journal, CreateJournalCommand>();
        CreateMap<CreateJournalCommand, Journal>();

        CreateMap<Journal, UpdateJournalCommand>();
        CreateMap<UpdateJournalCommand, Journal>();

        CreateMap<Journal, DeleteJournalCommand>();
        CreateMap<DeleteJournalCommand, Journal>();

        CreateMap<Invoice, Journal>();
        CreateMap<Journal, Invoice>();

        CreateMap<JournalItem, JournalItemModelView>();
        CreateMap<JournalItemModelView, JournalItem>();

        CreateMap<CreateJournalCommand, Journal>().ForMember(dest => dest.JournalItems,opt => opt.MapFrom(src => src.JournalItems));  
        CreateMap<UpdateJournalCommand, Journal>().ForMember(dest => dest.JournalItems, opt => opt.MapFrom(src => src.JournalItems));
    }
}