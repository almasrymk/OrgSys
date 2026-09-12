namespace Purchasing.Application;

using AutoMapper;
using Purchasing.Application.PurchaseRequisitions.Commands;

public partial class MappingProfile : Profile
{
    public void PurchaseRequisitionMappingProfile()
    {
        #region PurchaseRequisition
        CreateMap<PurchaseRequisition, PurchaseRequisitionDto>()
        .ForMember(dest => dest.PurchaseRequisitionProductList,
        opt => opt.MapFrom(src => src.PurchaseRequisitionProducts));
        CreateMap<PurchaseRequisitionDto, PurchaseRequisition>();

        CreateMap<PurchaseRequisitionProduct, PurchaseRequisitionProductDto>();
        CreateMap<PurchaseRequisitionProductDto, PurchaseRequisitionProduct>();

        CreateMap<CreatePurchaseRequisitionCommand, PurchaseRequisition>()
        .ForMember(dest => dest.PurchaseRequisitionProducts,
        opt => opt.MapFrom(src => src.PurchaseRequisitionProductList));

        CreateMap<UpdatePurchaseRequisitionCommand, PurchaseRequisition>()
        .ForMember(dest => dest.PurchaseRequisitionProducts,
        opt => opt.MapFrom(src => src.PurchaseRequisitionProductList));

        CreateMap<PurchaseRequisitionDto, CreatePurchaseRequisitionCommand>()
        .ForMember(dest => dest.PurchaseRequisitionProducts,
        opt => opt.MapFrom(src => src.PurchaseRequisitionProductList));

        CreateMap<PurchaseRequisitionDto, UpdatePurchaseRequisitionCommand>()
        .ForMember(dest => dest.PurchaseRequisitionProducts,
        opt => opt.MapFrom(src => src.PurchaseRequisitionProductList));
        #endregion
    }
}
