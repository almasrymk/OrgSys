using Application.Commands.Org.Transactions.Inventory.Commands;
using AutoMapper;
using Domain.Entities;
using Application.DTOs;

public partial class MappingProfile : Profile
{
    public void InventoryMappingProfile()
    {
        CreateMap<InventoryProduct, InventoryProductDto>();

        CreateMap<InventoryProductDto, InventoryProduct>();

        CreateMap<InventoryProductDto, TransactionProductDto>();

        CreateMap<TransactionProductDto, InventoryProduct>();


        CreateMap<Domain.Entities.Inventory, InventoryDto>()
            .ForMember(
                dest => dest.InventoryProductList,
                opt => opt.MapFrom(src => src.InventoryProducts)
            );

        CreateMap<InventoryDto, Domain.Entities.Inventory>()
            .ForMember(
                dest => dest.InventoryProducts,
                opt => opt.MapFrom(src => src.InventoryProducts)
            );

        CreateMap<Domain.Entities.Inventory, CreateInventoryCommand>();

        CreateMap<CreateInventoryCommand, Domain.Entities.Inventory>()
            .ForMember(dest => dest.Stock, opt => opt.Ignore())
            .ForMember(
                dest => dest.InventoryProducts,
                opt => opt.MapFrom(src => src.InventoryProductList)
            );

        CreateMap<Domain.Entities.Inventory, UpdateInventoryCommand>();

        CreateMap<UpdateInventoryCommand, Domain.Entities.Inventory>()
            .ForMember(dest => dest.Stock, opt => opt.Ignore())
            .ForMember(
                dest => dest.InventoryProducts,
                opt => opt.MapFrom(src => src.InventoryProductList)
            );


        CreateMap<Domain.Entities.Inventory, DeleteInventoryCommand>();

        CreateMap<DeleteInventoryCommand, Domain.Entities.Inventory>();

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