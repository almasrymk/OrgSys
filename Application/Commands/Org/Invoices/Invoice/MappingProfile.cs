using AutoMapper;
using Domain.Entities;
using Application.DTOs;
using Application.Commands.Org.Invoices.Invoice.Commands;

public partial class MappingProfile : Profile
{
    public void InvoiceMappingProfile()
    {
        #region Invoice
        CreateMap<Invoice, InvoiceDto>()
        .ForMember(dest => dest.InvoiceProductList,
        opt => opt.MapFrom(src => src.InvoiceProducts));
        CreateMap<InvoiceDto, Invoice>();
        CreateMap<Invoice, CreateInvoiceCommand>();
        CreateMap<CreateInvoiceCommand, Invoice>();
        CreateMap<Invoice, UpdateInvoiceCommand>();
        CreateMap<UpdateInvoiceCommand, Invoice>();
        CreateMap<Invoice, DeleteInvoiceCommand>();
        CreateMap<DeleteInvoiceCommand, Invoice>();

        CreateMap<InvoiceProduct, InvoiceProductDto>();
        CreateMap<InvoiceProductDto, InvoiceProduct>();

        CreateMap<CreateInvoiceCommand, Invoice>()
        .ForMember(dest => dest.InvoiceProducts,
        opt => opt.MapFrom(src => src.InvoiceProductList));


        CreateMap<UpdateInvoiceCommand, Invoice>()
       .ForMember(dest => dest.InvoiceProducts,
        opt => opt.MapFrom(src => src.InvoiceProductList));

        CreateMap<InvoiceDto, UpdateInvoiceCommand>()
        .ForMember(dest => dest.InvoiceProducts,
        opt => opt.MapFrom(src => src.InvoiceProductList));

        CreateMap<CreateInvoiceCommand, InvoiceDto>();
        CreateMap<InvoiceDto, CreateInvoiceCommand>()
        .ForMember(dest => dest.InvoiceProducts,
        opt => opt.MapFrom(src => src.InvoiceProductList)); 

        CreateMap<InvoiceProductDto, InvoiceProduct>();


        CreateMap<Product, ProductDto>();
        CreateMap<ProductDto, Product>();

        CreateMap<ProductUnit, ProductUnitDto>();
        CreateMap<ProductUnitDto, ProductUnit>();

        CreateMap<Unit, UnitDto>();           
        CreateMap<UnitDto, Unit>();           
        #endregion
    }
}