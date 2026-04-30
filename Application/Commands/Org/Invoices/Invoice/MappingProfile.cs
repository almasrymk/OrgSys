using AutoMapper;
using Entity.Model;
using Entity.ModelView;
using Application.Commands.Org.Invoices.Invoice.Commands;

public partial class MappingProfile : Profile
{
    public void InvoiceMappingProfile()
    {
        #region Invoice
        CreateMap<Invoice, InvoiceModelView>()
            .ForMember(dest => dest.TransactionCode, opt => opt.MapFrom(src => src.Transaction.Code))
            .ForMember(dest => dest.DealerName, opt => opt.MapFrom(src => src.Dealer.Name))
            .ForMember(dest => dest.ShiftName, opt => opt.MapFrom(src => src.Shift.Name))        
            .ForMember(dest => dest.StockName, opt => opt.MapFrom(src => src.Stock.Name))
            .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.Branch.Name))
            .ForMember(dest => dest.PaymentTypeName, opt => opt.MapFrom(src => src.PaymentType.Name))
            .ForMember(dest => dest.CurrencyName, opt => opt.MapFrom(src => src.Currency.Name))
            .ForMember(dest => dest.InvoiceProducts , opt => opt.MapFrom(src => src.InvoiceProducts));
        CreateMap<InvoiceModelView, Invoice>()
            .ForMember(dest => dest.InvoiceProducts , opt => opt.MapFrom(src => src.InvoiceProducts));

        CreateMap<Invoice, CreateInvoiceCommand>()
            .ForMember(dest => dest.InvoiceProducts, opt => opt.MapFrom(src => src.InvoiceProducts));
        CreateMap<CreateInvoiceCommand, Invoice>()
            .ForMember(dest => dest.InvoiceProducts, opt => opt.MapFrom(src => src.InvoiceProducts));

        CreateMap<InvoiceModelView, CreateInvoiceCommand>()
            .ForMember(dest => dest.InvoiceProducts, opt => opt.MapFrom(src => src.InvoiceProducts));
        CreateMap<CreateInvoiceCommand, InvoiceModelView>()
            .ForMember(dest => dest.InvoiceProducts, opt => opt.MapFrom(src => src.InvoiceProducts));

        CreateMap<Invoice, UpdateInvoiceCommand>()
            .ForMember(dest => dest.InvoiceProducts, opt => opt.MapFrom(src => src.InvoiceProducts));
        CreateMap<UpdateInvoiceCommand, Invoice>()
            .ForMember(dest => dest.InvoiceProducts, opt => opt.MapFrom(src => src.InvoiceProducts));

        CreateMap<InvoiceModelView, UpdateInvoiceCommand>()
            .ForMember(dest => dest.InvoiceProducts, opt => opt.MapFrom(src => src.InvoiceProducts));
        CreateMap<UpdateInvoiceCommand, InvoiceModelView>()
            .ForMember(dest => dest.InvoiceProducts, opt => opt.MapFrom(src => src.InvoiceProducts));

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