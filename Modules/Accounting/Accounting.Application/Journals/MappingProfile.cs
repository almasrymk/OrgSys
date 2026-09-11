namespace Accounting.Application;

﻿using Accounting.Application.Journals.Commands;
using AutoMapper;

public partial class MappingProfile : Profile
{

    public void JournalMappingProfile()
    {
        CreateMap<Journal, JournalDto>()
            .ForMember(dest => dest.JournalTypeName, opt => opt.MapFrom(src => src.JournalType!.Name))
            .ForMember(dest => dest.FiscalYearName, opt => opt.MapFrom(src => src.FiscalYear!.Name))
            .ForMember(dest => dest.FiscalPeriodName, opt => opt.MapFrom(src => src.FiscalPeriod!.Name))
            .ForMember(dest => dest.OriginalJournalCode, opt => opt.MapFrom(src => src.OriginalJournal!.Code))
            .ForMember(dest => dest.ReversalJournalId, opt => opt.MapFrom(src => src.ReversalJournal!.Id))
            .ForMember(dest => dest.ReversalJournalCode, opt => opt.MapFrom(src => src.ReversalJournal!.Code))
            .ForMember(dest => dest.JournalItems, opt => opt.MapFrom(src => src.JournalItems));
        CreateMap<JournalDto, Journal>()
            .ForMember(dest => dest.FiscalYearId, opt => opt.Ignore())
            .ForMember(dest => dest.FiscalPeriodId, opt => opt.Ignore())
            .ForMember(dest => dest.Posted, opt => opt.Ignore())
            // Never trust these from the client — only the Reverse handler is allowed to set them.
            .ForMember(dest => dest.OriginalJournalId, opt => opt.Ignore());

        CreateMap<JournalType, JournalTypeDto>();
        CreateMap<JournalTypeDto, JournalType>();

        CreateMap<Journal, CreateJournalCommand>();

        CreateMap<Journal, UpdateJournalCommand>();

        CreateMap<Journal, DeleteJournalCommand>();
        CreateMap<DeleteJournalCommand, Journal>();

        CreateMap<JournalItem, JournalItemDto>();
        CreateMap<JournalItemDto, JournalItem>();

        CreateMap<CreateJournalCommand, Journal>()
            .ForMember(dest => dest.JournalItems, opt => opt.MapFrom(src => src.JournalItems))
            .ForMember(dest => dest.FiscalYearId, opt => opt.Ignore())
            .ForMember(dest => dest.FiscalPeriodId, opt => opt.Ignore())
            .ForMember(dest => dest.Posted, opt => opt.Ignore())
            .ForMember(dest => dest.OriginalJournalId, opt => opt.Ignore());
        CreateMap<UpdateJournalCommand, Journal>()
            .ForMember(dest => dest.JournalItems, opt => opt.MapFrom(src => src.JournalItems))
            .ForMember(dest => dest.FiscalYearId, opt => opt.Ignore())
            .ForMember(dest => dest.FiscalPeriodId, opt => opt.Ignore())
            .ForMember(dest => dest.Posted, opt => opt.Ignore())
            .ForMember(dest => dest.OriginalJournalId, opt => opt.Ignore());

        CreateMap<JournalDto, UpdateJournalCommand>()
            .ForMember(dest => dest.JournalItems, opt => opt.MapFrom(src => src.JournalItems));

        CreateMap<CreateJournalCommand, JournalDto>();
        CreateMap<JournalDto, CreateJournalCommand>()
            .ForMember(dest => dest.JournalItems, opt => opt.MapFrom(src => src.JournalItems));
    }
}
