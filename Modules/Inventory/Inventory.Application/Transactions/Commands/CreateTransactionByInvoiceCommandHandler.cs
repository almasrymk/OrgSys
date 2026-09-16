namespace Inventory.Application.Transactions.Commands
{
    using Administration.Contracts.Preferences;
    using AutoMapper;
    using MediatR;
    using System.Net;
    using Inventory.Application.Transactions.Integration;
    using Inventory.Contracts.Transactions;
    using Inventory.Domain.Enums;
    using Inventory.Domain.Repositories;

    /// <summary>
    /// Note on InventoryBalance (docs/ddd/inventory-target-architecture.md §6): only the first-time
    /// "create" branch below updates InventoryBalance. The "re-sync an already-linked transaction"
    /// branch (invoice.TransactionId > 0) clears and rebuilds TransactionProducts from the invoice's
    /// current lines without keeping the old quantities, so correctly reversing the old balance
    /// effect before applying the new one needs its own dedicated pass — deliberately not attempted
    /// here to avoid guessing at a balance delta from data that's already been overwritten by the
    /// time this handler runs. Flagged as known follow-up work (Phase 11/12), not silently dropped.
    /// </summary>
    public sealed class CreateTransactionByInvoiceCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Transaction> _Repository,
        IRepository<Invoice> _InvoiceRepository, ISender sender, IMapper mapper,
        IServiceProvider provider, IInventoryBalanceRepository balanceRepository) : CreateCommandHandler<CreateTransactionByInvoiceCommand, Transaction>(_UnitOfWork, _Repository, mapper)
    {
        public override async Task<Result> Handle(CreateTransactionByInvoiceCommand request, CancellationToken cancellationToken)
        {
            await _UnitOfWork.BeginTransactionAsync();
            try
            {
                var invoice = await _InvoiceRepository.GetByFilterAsync(e => e.Id == request.Id,"InvoiceProducts");

                if (invoice == null)
                {
                    await _UnitOfWork.RollbackAsync();
                    return new Result(HttpStatusCode.NotFound,new List<Error>{new Error("Invoice not found")});
                }

                if (request.RespectAutoCreatePreference && invoice.TransactionId is not > 0)
                {
                    var autoCreateTransaction = (await sender.Send(
                        new GetPreferenceValueQuery("Invoice", invoice.TypeId, "AutoCreateTransaction"),
                        cancellationToken)).Response;

                    if (autoCreateTransaction != "1")
                    {
                        await _UnitOfWork.RollbackAsync();
                        return new Result(HttpStatusCode.OK, null);
                    }
                }
                
                Transaction transaction;
                var expectedTransactionTypeId = invoice.TypeId == 2 || invoice.TypeId == 3 ? 1L : 2L;
                var transactionTypeChanged = false;
                var isFirstTimeCreate = invoice.TransactionId is not > 0;

                if (invoice.TransactionId > 0)
                {
                    transaction = (await _Repository.GetByFilterAsync(e => e.Id == invoice.TransactionId,"TransactionProducts"))!;

                    if (transaction == null)
                    {
                        await _UnitOfWork.RollbackAsync();
                        return new Result(HttpStatusCode.NotFound,new List<Error>{new Error("Transaction not found")});
                    }

                    var originalTransactionTypeId = transaction.TypeId;
                    transaction.TransactionProducts?.Clear();

                    mapper.Map(invoice, transaction);
                    transaction.TypeId = expectedTransactionTypeId;
                    transactionTypeChanged = originalTransactionTypeId != expectedTransactionTypeId;

                }
                else
                {
                    transaction = mapper.Map<Transaction>(invoice);

                    transaction.TypeId = expectedTransactionTypeId;

                    transaction.CodeNumber = await _Repository.AnyAsync(e =>
                            e.TypeId == transaction.TypeId
                            || (transaction.TypeId == 1 && e.TypeId == 5)
                            || (transaction.TypeId == 2 && e.TypeId == 6))
                        ? await _Repository.GetMaxByFilterAsync(
                            e => e.TypeId == transaction.TypeId
                                || (transaction.TypeId == 1 && e.TypeId == 5)
                                || (transaction.TypeId == 2 && e.TypeId == 6),
                            e => e.CodeNumber) + 1
                        : 1;

                    transaction.Code = transaction.CodeNumber.ToString();
                }


                if (invoice.TransactionId == null || invoice.TransactionId == 0)
                    await _Repository.CreateAsync(transaction);
                

                if (await _UnitOfWork.SaveChangeAsync() > 0)
                {
                    invoice.TransactionId = transaction.Id;

                    if (transactionTypeChanged)
                        await new TransactionJournalPostingService(provider).DeleteByTransactionIdAsync(transaction.Id);
                    await new TransactionJournalPostingService(provider).SyncAsync(transaction, invoice.TypeId);
                    await _Repository.UpdateAsync(transaction);

                    // Keep InventoryBalance in sync for the common (first-time) case — see this
                    // class's doc comment for why the re-sync branch is deliberately not covered yet.
                    if (isFirstTimeCreate)
                    {
                        var movementType = MovementTypeExtensions.FromTransactionTypeId(transaction.TypeId);
                        var allowNegativeStock = false;
                        foreach (var product in transaction.TransactionProducts ?? [])
                        {
                            var targetStockId = product.StockId ?? transaction.StockId;
                            if (targetStockId is not > 0)
                                continue;

                            var balance = await balanceRepository.GetOrCreateTrackedAsync(product.ProductId, targetStockId.Value, null, null, cancellationToken);
                            if (movementType.Direction() == MovementDirection.In)
                                balance.Receive(product.Quantity, product.Cost);
                            else
                                balance.IssueOut(product.Quantity, allowNegativeStock);
                        }
                    }

                    await _UnitOfWork.SaveChangeAsync(cancellationToken);
                    await _UnitOfWork.CommitAsync();

                    return new Result(HttpStatusCode.OK, null);
                }

                await _UnitOfWork.RollbackAsync();
                return new Result(HttpStatusCode.InternalServerError,new List<Error>{new Error("Error while saving")});
            }
            catch (Exception ex)
            {
                await _UnitOfWork.RollbackAsync();
                return new Result(HttpStatusCode.InternalServerError,new List<Error>{new Error(ex.Message)});
            }
        }
    }
}
