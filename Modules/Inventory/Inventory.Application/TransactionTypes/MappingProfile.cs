namespace Inventory.Application;

using Inventory.Application.Transactions.Commands;
using Inventory.Application.TransactionTypes.Commands;
using AutoMapper;

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