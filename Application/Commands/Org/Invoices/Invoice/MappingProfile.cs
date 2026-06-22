using AutoMapper;
using Domain.Entities;
using Application.DTOs;
using Application.Commands.Org.Invoices.Invoice.Commands;

public partial class MappingProfile : Profile
{
    public void InvoiceMappingProfile()
    {
        #region Invoice
        CreateMap<Invoice, InvoiceModelView>()
        .ForMember(dest => dest.InvoiceProductList,
        opt => opt.MapFrom(src => src.InvoiceProducts));
        CreateMap<InvoiceModelView, Invoice>();
        CreateMap<Invoice, CreateInvoiceCommand>();
        CreateMap<CreateInvoiceCommand, Invoice>();
        CreateMap<Invoice, UpdateInvoiceCommand>();
        CreateMap<UpdateInvoiceCommand, Invoice>();
        CreateMap<Invoice, DeleteInvoiceCommand>();
        CreateMap<DeleteInvoiceCommand, Invoice>();

        CreateMap<InvoiceProduct, InvoiceProductModelView>();
        CreateMap<InvoiceProductModelView, InvoiceProduct>();

        CreateMap<CreateInvoiceCommand, Invoice>()
        .ForMember(dest => dest.InvoiceProducts,
        opt => opt.MapFrom(src => src.InvoiceProductList));


        CreateMap<UpdateInvoiceCommand, Invoice>()
       .ForMember(dest => dest.InvoiceProducts,
        opt => opt.MapFrom(src => src.InvoiceProductList));

        CreateMap<InvoiceModelView, UpdateInvoiceCommand>()
        .ForMember(dest => dest.InvoiceProducts,
        opt => opt.MapFrom(src => src.InvoiceProductList));

        CreateMap<CreateInvoiceCommand, InvoiceModelView>();
        CreateMap<InvoiceModelView, CreateInvoiceCommand>()
        .ForMember(dest => dest.InvoiceProducts,
        opt => opt.MapFrom(src => src.InvoiceProductList)); 

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