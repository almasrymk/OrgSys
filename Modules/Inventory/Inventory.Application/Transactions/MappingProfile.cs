namespace Inventory.Application;

using Inventory.Application.Transactions.Commands;
using Inventory.Application.TransactionTypes.Commands;
using Inventory.Contracts.Transactions;
using AutoMapper;

public partial class MappingProfile : Profile
{
    public void TransactionMappingProfile()
    {
        #region Transaction
        CreateMap<Transaction, DeleteTransactionCommand>();
        CreateMap<DeleteTransactionCommand, Transaction>();
        CreateMap<TransactionProduct, TransactionProductDto>();
        CreateMap<TransactionProductDto, TransactionProduct>();

        CreateMap<Transaction, CreateTransactionByInvoiceCommand>();
        CreateMap<CreateTransactionByInvoiceCommand, Transaction>();



        CreateMap<Transaction, CreateTransactionCommand>()
            .ForMember(des => des.TransactionProductList, src => src.MapFrom(s => s.TransactionProducts));
        CreateMap<CreateTransactionCommand, Transaction>()
            .ForMember(des => des.TransactionProducts, src => src.MapFrom(s => s.TransactionProductList));


        CreateMap<TransactionDto, CreateTransactionCommand>()
        .ForMember(des => des.TransactionProductList, src => src.MapFrom(s => s.TransactionProductList));
        CreateMap<CreateTransactionCommand, TransactionDto>();


        CreateMap<Transaction, TransactionDto>()
        .ForMember(dest => dest.TransactionProductList, src => src.MapFrom(s => s.TransactionProducts));
        CreateMap<TransactionDto, Transaction>();

        CreateMap<Transaction, UpdateTransactionCommand>();
        CreateMap<UpdateTransactionCommand, Transaction>();

        CreateMap<TransactionDto, UpdateTransactionCommand>()
        .ForMember(des => des.TransactionProductList, src => src.MapFrom(s => s.TransactionProductList));
        CreateMap<UpdateTransactionCommand, TransactionDto>();


        CreateMap<UpdateTransactionCommand, Transaction>()
            .ForMember(dest => dest.TransactionProducts, src => src.MapFrom(s => s.TransactionProductList));
        CreateMap<Transaction, UpdateTransactionCommand>();
        #endregion
    }
}
