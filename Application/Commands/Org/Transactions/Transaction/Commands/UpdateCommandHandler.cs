namespace Application.Commands.Org.Transactions.Transaction.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.ModelView;

    public sealed class UpdateTransactionCommand : Entity.ModelView.TransactionModelView , ICommand, IUpdateCommand<Result>;
    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.Transaction> _Repository , IMapper mapper, IServiceProvider _provider) : UpdateCommandHandler<UpdateTransactionCommand, Entity.Model.Transaction>(_UnitOfWork, _Repository , mapper , _provider)
    {
       
    }
}