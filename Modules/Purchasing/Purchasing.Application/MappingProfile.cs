namespace Purchasing.Application;

using AutoMapper;

public partial class MappingProfile : Profile
{
    public MappingProfile()
    {
        PurchaseRequisitionMappingProfile();
        PurchaseOrderMappingProfile();
    }
}
