using Application.Commands.Org.Invoices.Invoice.Commands;
using Application.Commands.Org.Transactions.Transaction.Commands;
using Application.Commands.Org.Transactions.TransactionType.Commands;
using AutoMapper;
using Domain.Entities;
using Application.DTOs;

public partial class MappingProfile : Profile
{
    public void TransactionTypeMappingProfile()
    {
        #region Transaction
        CreateMap<TransactionType, TransactionTypeDto>();
        CreateMap<TransactionTypeDto, TransactionType>();
        CreateMap<TransactionType, CreateTransactionTypeCommand>();
        CreateMap<CreateTransactionTypeCommand, TransactionType>();

        CreateMap<TransactionType, UpdateTransactionTypeCommand>();
        CreateMap<UpdateTransactionTypeCommand, TransactionType>();
        CreateMap<TransactionType, DeleteTransactionTypeCommand>();
        CreateMap<DeleteTransactionTypeCommand, TransactionType>();

        #endregion
    }
}