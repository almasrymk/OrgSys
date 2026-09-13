namespace CommercialDocuments.Application;

using AutoMapper;

public partial class MappingProfile : Profile
{
    public MappingProfile()
    {
        InvoiceTypeMappingProfile();
        InvoiceMappingProfile();
    }
}
