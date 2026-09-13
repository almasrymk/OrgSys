namespace Inventory.Application.Transactions.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using Microsoft.Extensions.DependencyInjection;
    using System.Linq.Expressions;
    using Inventory.Application.Transactions.Integration;

    public sealed record DeleteTransactionCommand(long Id) : ICommand, IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork,
        IRepository<Inventory.Domain.Transaction> _Repository,
        IRepository<TransactionProduct> _TransactionProductRepository,
        IServiceProvider _provider) : DeleteCommandHandler<DeleteTransactionCommand, Inventory.Domain.Transaction>(_UnitOfWork, _Repository, _provider)
    {
        public override Expression<Func<Inventory.Domain.Transaction, bool>> CreateFilter(DeleteTransactionCommand request)
        {
            return e => e.Id == request.Id && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }

        public override async Task<bool> RemoveDetails(DeleteTransactionCommand request)
        {
            var sourceTransaction = await _Repository.GetByFilterAsync(e => e.Id == request.Id, string.Empty);
            if (sourceTransaction?.InventoryId is > 0)
                throw new InvalidOperationException("A transaction created from an inventory cannot be deleted");

            var sourceInvoice = await _provider.GetRequiredService<IRepository<CommercialDocuments.Domain.Invoice>>()
                .GetByFilterAsync(e => e.TransactionId == request.Id, string.Empty);
            if (sourceInvoice != null)
                throw new InvalidOperationException("A transaction created from an invoice cannot be deleted");

          var transactionProducts =  await _Repository.GetByFilterAsync(t => t.Id == request.Id, "TransactionProducts");

            if (transactionProducts == null)
                return false;

            await new TransferReceivedIntegration(_provider).DeleteReceivedAsync(transactionProducts);
            await new TransactionJournalPostingService(_provider).DeleteByTransactionIdAsync(request.Id);
            return await _TransactionProductRepository.ShiftDeleteAsync(e => e.TransactionId == request.Id);
        }
    }
}
