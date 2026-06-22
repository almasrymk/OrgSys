using Application.Commands.Org.Transactions.Inventory.Commands;
using AutoMapper;
using Domain.Entities;
using Application.DTOs;

public partial class MappingProfile : Profile
{
    public void InventoryMappingProfile()
    {
        CreateMap<InventoryProduct, InventoryProductModelView>();

        CreateMap<InventoryProductModelView, InventoryProduct>();

        CreateMap<InventoryProductModelView, TransactionProductModelView>();

        CreateMap<TransactionProductModelView, InventoryProduct>();


        CreateMap<Domain.Entities.Inventory, InventoryModelView>()
            .ForMember(
                dest => dest.InventoryProductList,
                opt => opt.MapFrom(src => src.InventoryProducts)
            );

        CreateMap<InventoryModelView, Domain.Entities.Inventory>()
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

        CreateMap<CreateInventoryCommand, InventoryModelView>();

        CreateMap<InventoryModelView, CreateInventoryCommand>()
            .ForMember(
                dest => dest.InventoryProducts,
                opt => opt.MapFrom(src => src.InventoryProducts)
            );


        CreateMap<UpdateInventoryCommand, InventoryModelView>();

        CreateMap<InventoryModelView, UpdateInventoryCommand>();

        CreateMap<DeleteInventoryCommand, InventoryModelView>();

        CreateMap<InventoryModelView, DeleteInventoryCommand>();
    }
}