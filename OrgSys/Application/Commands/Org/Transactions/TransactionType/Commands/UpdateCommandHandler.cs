namespace Application.Commands.Org.Transactions.TransactionType.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.ModelView;

    public sealed class UpdateTransactionTypeCommand : Entity.ModelView.TransactionTypeModelView, ICommand, IUpdateCommand<Result>;
    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.TransactionType> _Repository , IMapper mapper, IServiceProvider _provider) : UpdateCommandHandler<UpdateTransactionTypeCommand, Entity.Model.TransactionType>(_UnitOfWork, _Repository , mapper , _provider)
    {
       
    }
}