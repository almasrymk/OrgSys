using Application.Commands.Org.Transactions.Inventory.Commands;
using AutoMapper;
using Entity.Model;
using Entity.ModelView;

public partial class MappingProfile : Profile
{
    public void InventoryMappingProfile()
    {
        CreateMap<InventoryProduct, InventoryProductModelView>();

        CreateMap<InventoryProductModelView, InventoryProduct>();

        CreateMap<InventoryProductModelView, TransactionProductModelView>();

        CreateMap<TransactionProductModelView, InventoryProduct>();


        CreateMap<Entity.Model.Inventory, InventoryModelView>()
            .ForMember(
                dest => dest.InventoryProductList,
                opt => opt.MapFrom(src => src.InventoryProducts)
            );

        CreateMap<InventoryModelView, Entity.Model.Inventory>()
            .ForMember(
                dest => dest.InventoryProducts,
                opt => opt.MapFrom(src => src.InventoryProducts)
            );

        CreateMap<Entity.Model.Inventory, CreateInventoryCommand>();

        CreateMap<CreateInventoryCommand, Entity.Model.Inventory>()
            .ForMember(dest => dest.Stock, opt => opt.Ignore())
            .ForMember(
                dest => dest.InventoryProducts,
                opt => opt.MapFrom(src => src.InventoryProductList)
            );

        CreateMap<Entity.Model.Inventory, UpdateInventoryCommand>();

        CreateMap<UpdateInventoryCommand, Entity.Model.Inventory>()
            .ForMember(dest => dest.Stock, opt => opt.Ignore())
            .ForMember(
                dest => dest.InventoryProducts,
                opt => opt.MapFrom(src => src.InventoryProductList)
            );


        CreateMap<Entity.Model.Inventory, DeleteInventoryCommand>();

        CreateMap<DeleteInventoryCommand, Entity.Model.Inventory>();

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