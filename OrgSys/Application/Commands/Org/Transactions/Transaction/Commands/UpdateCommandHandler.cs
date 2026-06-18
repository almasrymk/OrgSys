namespace Application.Commands.Org.Transactions.Transaction.Commands
{
    using Application.Abstraction.Command;
    using Application.Commands.Org.Invoices.Invoice.Commands;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.Model;
    using Entity.ModelView;

    public sealed class UpdateTransactionCommand : Entity.ModelView.TransactionModelView , ICommand, IUpdateCommand<Result>;
    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork,
        IRepository<Entity.Model.Transaction> _Repository , 
        IRepository<TransactionProduct> _TransactionRepository,
        IMapper mapper, IServiceProvider _provider) : UpdateCommandHandler<UpdateTransactionCommand, Entity.Model.Transaction>(_UnitOfWork, _Repository , mapper , _provider)
    {
        override public async Task<bool> SaveDetials(UpdateTransactionCommand request)
        {
            #region UpdateProduct
            var ids = request.TransactionProductList.Select(e => e.Id);
            var removeList = await _TransactionRepository.GetListByFilterAsync(e => e.TransactionId == request.Id && !ids.Contains(e.Id));

            var res = await RemoveDetails<TransactionProduct>(removeList!);
            if (!res) return false;
            var ob = mapper.Map<List<TransactionProduct>>(request.TransactionProductList);
            res = await UpdateDetails<TransactionProduct>(ob);
            #endregion 

            return res;
        }
    }
}