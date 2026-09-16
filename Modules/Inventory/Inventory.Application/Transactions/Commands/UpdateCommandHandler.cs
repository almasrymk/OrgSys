namespace Inventory.Application.Transactions.Commands
{
    using AutoMapper;
    using CommercialDocuments.Contracts.Invoices;
    using MediatR;
    using Microsoft.Extensions.DependencyInjection;
    using Inventory.Application.Transactions.Integration;

    public sealed class UpdateTransactionCommand : TransactionDto , ICommand, IUpdateCommand<Result>;
    public sealed class UpdateCommandHandler(IUnitOfWork _UnitOfWork,
        IRepository<Inventory.Domain.Transaction> _Repository , 
        IRepository<TransactionProduct> _TransactionRepository,
        IMapper mapper, IServiceProvider _provider) : UpdateCommandHandler<UpdateTransactionCommand, Inventory.Domain.Transaction>(_UnitOfWork, _Repository , mapper , _provider)
    {
        public override async Task<Result> Handle(UpdateTransactionCommand request, CancellationToken cancellationToken)
        {
            var transaction = await _Repository.GetByFilterAsync(e => e.Id == request.Id, string.Empty);
            if (transaction?.InventoryId is > 0)
                return new Result(System.Net.HttpStatusCode.Forbidden, [new Error("A transaction created from an inventory is read-only")]);

            var sourceInvoice = await _provider.GetRequiredService<ISender>()
                .Send(new GetInvoiceByLinkedTransactionQuery(request.Id), cancellationToken);
            if (sourceInvoice.Response != null)
                return new Result(System.Net.HttpStatusCode.Forbidden, [new Error("A transaction created from an invoice is read-only")]);

            var result = await base.Handle(request, cancellationToken);
            if (result.StatusCode != System.Net.HttpStatusCode.OK || request.TypeId != 3)
                return result;

            var transfer = await _Repository.GetByFilterAsync(e => e.Id == request.Id, "TransactionProducts");
            if (transfer == null)
                return new Result(System.Net.HttpStatusCode.NotFound, [new Error("Transfer not found")]);

            await new TransferReceivedIntegration(_provider).SyncAsync(transfer, cancellationToken);
            await _UnitOfWork.SaveChangeAsync(cancellationToken);
            return result;
        }

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

            if (res)
            {
                await new TransactionJournalPostingService(_provider).SyncAsync(request);
                res = await _Repository.UpdateAsync(request);
            }

            return res;
        }
    }
}
