namespace Purchasing.Application;

using AutoMapper;

public partial class MappingProfile : Profile
{
    public void PurchaseOrderMappingProfile()
    {
        #region PurchaseOrder
        // Read direction only — Create/Update now go through PurchaseOrder's own domain methods
        // (see Purchasing.Application.PurchaseOrders.Commands.{Create,Update}CommandHandler), not
        // AutoMapper, since PurchaseOrder has private setters. PurchaseOrderDto/
        // PurchaseOrderProductDto are standalone classes (no longer inherit from the entity), so
        // these are ordinary property-name-convention maps.
        CreateMap<PurchaseOrder, PurchaseOrderDto>()
        .ForMember(dest => dest.DealerName, opt => opt.Ignore())
        .ForMember(dest => dest.PurchaseOrderProductList,
        opt => opt.MapFrom(src => src.PurchaseOrderProducts));

        CreateMap<PurchaseOrderProduct, PurchaseOrderProductDto>();
        #endregion
    }
}
