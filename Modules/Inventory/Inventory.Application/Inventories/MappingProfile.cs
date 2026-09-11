namespace Inventory.Application;

﻿using Inventory.Application.Inventories.Commands;
using AutoMapper;

public partial class MappingProfile : Profile
{
    public void InventoryMappingProfile()
    {
        CreateMap<InventoryProduct, InventoryProductDto>();

        CreateMap<InventoryProductDto, InventoryProduct>();

        CreateMap<InventoryProductDto, TransactionProductDto>();

        CreateMap<TransactionProductDto, InventoryProduct>();


        CreateMap<Inventory.Domain.Inventory, InventoryDto>()
            .ForMember(
                dest => dest.InventoryProductList,
                opt => opt.MapFrom(src => src.InventoryProducts)
            );

        CreateMap<InventoryDto, Inventory.Domain.Inventory>()
            .ForMember(
                dest => dest.InventoryProducts,
                opt => opt.MapFrom(src => src.InventoryProducts)
            );

        CreateMap<Inventory.Domain.Inventory, CreateInventoryCommand>();

        CreateMap<CreateInventoryCommand, Inventory.Domain.Inventory>()
            .ForMember(dest => dest.Stock, opt => opt.Ignore())
            .ForMember(
                dest => dest.InventoryProducts,
                opt => opt.MapFrom(src => src.InventoryProductList)
            );

        CreateMap<Inventory.Domain.Inventory, UpdateInventoryCommand>();

        CreateMap<UpdateInventoryCommand, Inventory.Domain.Inventory>()
            .ForMember(dest => dest.Stock, opt => opt.Ignore())
            .ForMember(
                dest => dest.InventoryProducts,
                opt => opt.MapFrom(src => src.InventoryProductList)
            );


        CreateMap<Inventory.Domain.Inventory, DeleteInventoryCommand>();

        CreateMap<DeleteInventoryCommand, Inventory.Domain.Inventory>();

        CreateMap<CreateInventoryCommand, InventoryDto>();

        CreateMap<InventoryDto, CreateInventoryCommand>()
            .ForMember(
                dest => dest.InventoryProducts,
                opt => opt.MapFrom(src => src.InventoryProducts)
            );


        CreateMap<UpdateInventoryCommand, InventoryDto>();

        CreateMap<InventoryDto, UpdateInventoryCommand>();

        CreateMap<DeleteInventoryCommand, InventoryDto>();

        CreateMap<InventoryDto, DeleteInventoryCommand>();
    }
}