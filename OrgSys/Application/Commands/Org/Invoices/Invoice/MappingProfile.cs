using AutoMapper;
using Entity.Model;
using Entity.ModelView;
using Application.Commands.Org.Invoices.Invoice.Commands;

public partial class MappingProfile : Profile
{
    public void InvoiceMappingProfile()
    {
        #region Invoice
        CreateMap<Invoice, InvoiceModelView>();
            //.ForMember(dest => dest.InvoiceProductList.Select(e => e.UnitList).ToList(), opt => opt.MapFrom(src => src.InvoiceProducts.Select(e => e.Product.ProductUnits.Select(e => e.Unit))));
        CreateMap<InvoiceModelView, Invoice>();
        CreateMap<Invoice, CreateInvoiceCommand>();
        CreateMap<CreateInvoiceCommand, Invoice>();
        CreateMap<Invoice, UpdateInvoiceCommand>();
        CreateMap<UpdateInvoiceCommand, Invoice>();
        CreateMap<Invoice, DeleteInvoiceCommand>();
        CreateMap<DeleteInvoiceCommand, Invoice>();

        CreateMap<InvoiceProduct, InvoiceProductModelView>();
        CreateMap<InvoiceProductModelView, InvoiceProduct>();




        CreateMap<Product, ProductModelView>();
        CreateMap<ProductModelView, Product>();

        CreateMap<ProductUnit, ProductUnitModelView>();
        CreateMap<ProductUnitModelView, ProductUnit>();

        CreateMap<Unit, UnitModelView>();           
        CreateMap<UnitModelView, Unit>();           
        #endregion
    }
}