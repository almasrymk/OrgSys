namespace Application.Commands.Org.Transactions.Transaction.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;

    public sealed class CreateTransactionCommand : Application.DTOs.TransactionModelView, ICommand , ICreateCommand<Result>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Domain.Entities.Transaction> _Repository , IMapper mapper) : CreateCommandHandler<CreateTransactionCommand, Domain.Entities.Transaction>(_UnitOfWork, _Repository , mapper)
    {
       
    }
}