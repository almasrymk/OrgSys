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
        #endregion
    }
}