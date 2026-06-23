namespace Application.Commands.Org.Transactions.TransactionType.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;

    public sealed class UpdateTransactionTypeCommand : Application.DTOs.TransactionTypeDto, ICommand, IUpdateCommand<Result>;
    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Domain.Entities.TransactionType> _Repository , IMapper mapper, IServiceProvider _provider) : UpdateCommandHandler<UpdateTransactionTypeCommand, Domain.Entities.TransactionType>(_UnitOfWork, _Repository , mapper , _provider)
    {
       
    }
}