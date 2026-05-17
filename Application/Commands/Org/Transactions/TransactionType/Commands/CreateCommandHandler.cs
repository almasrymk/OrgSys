namespace Application.Commands.Org.Transactions.TransactionType.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.ModelView;

    public sealed class CreateTransactionTypeCommand : Entity.ModelView.TransactionTypeModelView, ICommand , ICreateCommand<Result>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.TransactionType> _Repository , IMapper mapper) : CreateCommandHandler<CreateTransactionTypeCommand, Entity.Model.TransactionType>(_UnitOfWork, _Repository , mapper)
    {
       
    }
}