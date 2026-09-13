namespace Accounting.Application;

﻿using Accounting.Application.Journals.Commands;
using AutoMapper;

public partial class MappingProfile : Profile
{

    public void JournalMappingProfile()
    {
        // Journal -> JournalDto is the only direction AutoMapper handles for the aggregate itself.
        // The opposite direction (DTO/Command -> Journal) no longer goes through AutoMapper at all:
        // Create/Update build/mutate the aggregate explicitly via Journal.CreateDraft/UpdateHeader/
        // AddLine/UpdateLine/RemoveLine (see CreateCommandHandler.cs/UpdateCommandHandler.cs) — a
        // blind mapper.Map<Journal>(request) can no longer construct a valid aggregate now that
        // JournalItems and the lifecycle/period fields are private-set. See the Accounting DDD
        // cleanup report.
        CreateMap<Journal, JournalDto>()
            .ForMember(dest => dest.CurrencyName, opt => opt.Ignore())
            .ForMember(dest => dest.JournalTypeName, opt => opt.MapFrom(src => src.JournalType!.Name))
            .ForMember(dest => dest.FiscalYearName, opt => opt.MapFrom(src => src.FiscalYear!.Name))
            .ForMember(dest => dest.FiscalPeriodName, opt => opt.MapFrom(src => src.FiscalPeriod!.Name))
            .ForMember(dest => dest.OriginalJournalCode, opt => opt.MapFrom(src => src.OriginalJournal!.Code))
            .ForMember(dest => dest.ReversalJournalId, opt => opt.MapFrom(src => src.ReversalJournal!.Id))
            .ForMember(dest => dest.ReversalJournalCode, opt => opt.MapFrom(src => src.ReversalJournal!.Code))
            .ForMember(dest => dest.JournalItems, opt => opt.MapFrom(src => src.JournalItems));

        CreateMap<JournalType, JournalTypeDto>();
        CreateMap<JournalTypeDto, JournalType>();

        CreateMap<Journal, CreateJournalCommand>();
        CreateMap<Journal, UpdateJournalCommand>();
        CreateMap<Journal, DeleteJournalCommand>();

        CreateMap<JournalItem, JournalItemDto>();

        CreateMap<JournalDto, CreateJournalCommand>();
        CreateMap<CreateJournalCommand, JournalDto>();
        CreateMap<JournalDto, UpdateJournalCommand>();
        CreateMap<UpdateJournalCommand, JournalDto>();
        CreateMap<CreateJournalCommand, JournalDto>();
    }
}
