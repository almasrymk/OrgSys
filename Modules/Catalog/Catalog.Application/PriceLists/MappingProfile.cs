namespace Catalog.Application;

﻿using Catalog.Application.PriceLists.Commands;
using AutoMapper;

public partial class MappingProfile : Profile
{
    public void PriceListMappingProfile()
    {
        #region PriceList
        CreateMap<PriceListEntry, PriceListEntryDto>()
        .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product!.Name))
        .ForMember(dest => dest.UnitName, opt => opt.MapFrom(src => src.Unit!.Name));
        CreateMap<PriceListEntryDto, PriceListEntry>()
        .ForMember(d => d.Product, o => o.Ignore())
        .ForMember(d => d.Unit, o => o.Ignore());

        CreateMap<PriceList, PriceListDto>()
        .ForMember(dest => dest.Entries, opt => opt.MapFrom(src => src.Entries));
        CreateMap<PriceListDto, PriceList>()
        .ForMember(dest => dest.Entries, opt => opt.MapFrom(src => src.Entries));

        CreateMap<PriceList, CreatePriceListCommand>();
        CreateMap<CreatePriceListCommand, PriceList>()
        .ForMember(dest => dest.Entries, opt => opt.MapFrom(src => src.Entries));
        CreateMap<PriceList, UpdatePriceListCommand>()
        .ForMember(dest => dest.Entries, opt => opt.MapFrom(src => src.Entries));
        CreateMap<UpdatePriceListCommand, PriceList>()
        .ForMember(dest => dest.Entries, opt => opt.Ignore());
        CreateMap<PriceList, DeletePriceListCommand>();
        CreateMap<DeletePriceListCommand, PriceList>();

        CreateMap<PriceListDto, CreatePriceListCommand>();
        CreateMap<CreatePriceListCommand, PriceListDto>();
        CreateMap<PriceListDto, UpdatePriceListCommand>();
        CreateMap<UpdatePriceListCommand, PriceListDto>();
        #endregion
    }
}
