namespace Purchasing.Application;

using AutoMapper;

public partial class MappingProfile : Profile
{
    public void PurchaseRequisitionMappingProfile()
    {
        #region PurchaseRequisition
        // Read direction only — see PurchaseOrderMappingProfile's identical remark.
        CreateMap<PurchaseRequisition, PurchaseRequisitionDto>()
        .ForMember(dest => dest.PurchaseRequisitionProductList,
        opt => opt.MapFrom(src => src.PurchaseRequisitionProducts));

        CreateMap<PurchaseRequisitionProduct, PurchaseRequisitionProductDto>();
        #endregion
    }
}
