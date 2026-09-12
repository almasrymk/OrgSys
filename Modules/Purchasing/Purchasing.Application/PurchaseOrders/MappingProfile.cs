namespace Purchasing.Application;

using AutoMapper;
using Purchasing.Application.PurchaseOrders.Commands;

public partial class MappingProfile : Profile
{
    public void PurchaseOrderMappingProfile()
    {
        #region PurchaseOrder
        CreateMap<PurchaseOrder, PurchaseOrderDto>()
        .ForMember(dest => dest.DealerName, opt => opt.MapFrom(src => src.Dealer!.Name))
        .ForMember(dest => dest.PurchaseOrderProductList,
        opt => opt.MapFrom(src => src.PurchaseOrderProducts));
        CreateMap<PurchaseOrderDto, PurchaseOrder>();

        CreateMap<PurchaseOrderProduct, PurchaseOrderProductDto>();
        CreateMap<PurchaseOrderProductDto, PurchaseOrderProduct>();

        CreateMap<CreatePurchaseOrderCommand, PurchaseOrder>()
        .ForMember(dest => dest.PurchaseOrderProducts,
        opt => opt.MapFrom(src => src.PurchaseOrderProductList));

        CreateMap<UpdatePurchaseOrderCommand, PurchaseOrder>()
        .ForMember(dest => dest.PurchaseOrderProducts,
        opt => opt.MapFrom(src => src.PurchaseOrderProductList));

        CreateMap<PurchaseOrderDto, CreatePurchaseOrderCommand>()
        .ForMember(dest => dest.PurchaseOrderProducts,
        opt => opt.MapFrom(src => src.PurchaseOrderProductList));

        CreateMap<PurchaseOrderDto, UpdatePurchaseOrderCommand>()
        .ForMember(dest => dest.PurchaseOrderProducts,
        opt => opt.MapFrom(src => src.PurchaseOrderProductList));
        #endregion
    }
}
