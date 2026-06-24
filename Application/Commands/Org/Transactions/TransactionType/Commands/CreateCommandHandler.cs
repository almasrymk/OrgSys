namespace Application.Commands.Org.Transactions.TransactionType.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;

    public sealed class CreateTransactionTypeCommand : Application.DTOs.TransactionTypeDto, ICommand , ICreateCommand<Result>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Domain.Entities.TransactionType> _Repository , IMapper mapper) : CreateCommandHandler<CreateTransactionTypeCommand, Domain.Entities.TransactionType>(_UnitOfWork, _Repository , mapper)
    {
       
    }
}