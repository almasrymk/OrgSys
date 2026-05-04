namespace Application.Commands.Org.Transactions.Transaction.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.ModelView;

    public sealed class CreateTransactionCommand : Entity.ModelView.TransactionModelView, ICommand , ICreateCommand<Result>;

    public sealed class CreateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.Transaction> _Repository , IMapper mapper) : CreateCommandHandler<CreateTransactionCommand, Entity.Model.Transaction>(_UnitOfWork, _Repository , mapper)
    {
       
    }
}