using Application.Commands.Org.Invoices.Invoice.Commands;
using Application.Commands.Org.Transactions.Transaction.Commands;
using AutoMapper;
using Entity.Model;
using Entity.ModelView;

public partial class MappingProfile : Profile
{
    public void TransactionMappingProfile()
    {
        #region Transaction
        CreateMap<Transaction, TransactionModelView>();        
        CreateMap<TransactionModelView, Transaction>();
        CreateMap<Transaction, CreateTransactionCommand>();
        CreateMap<CreateTransactionCommand, Transaction>();
        CreateMap<Transaction, UpdateTransactionCommand>();
        CreateMap<UpdateTransactionCommand, Transaction>();
        CreateMap<Transaction, DeleteTransactionCommand>();
        CreateMap<DeleteTransactionCommand, Transaction>();
        CreateMap<TransactionProduct, TransactionProductModelView>();
        CreateMap<TransactionProductModelView, TransactionProduct>();
        CreateMap<Product, ProductModelView>();
        CreateMap<ProductModelView, Product>();
        CreateMap<ProductUnit, ProductUnitModelView>();
        CreateMap<ProductUnitModelView, ProductUnit>();
        CreateMap<Unit, UnitModelView>();           
        CreateMap<UnitModelView, Unit>();
        CreateMap<Transaction, CreateTransactionByInvoiceCommand>();
        CreateMap<CreateTransactionByInvoiceCommand, Transaction>();

        CreateMap<Invoice, Transaction>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Code, opt => opt.Ignore())
            .ForMember(dest => dest.CodeNumber, opt => opt.Ignore())
            .ForMember(dest => dest.TransactionProducts,
                opt => opt.MapFrom(src => src.InvoiceProducts));

        CreateMap<InvoiceProduct, TransactionProduct>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
        #endregion
    }
}