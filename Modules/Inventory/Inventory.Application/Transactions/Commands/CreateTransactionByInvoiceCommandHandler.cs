namespace Inventory.Application.Transactions.Commands
{
    using Administration.Contracts.Preferences;
    using AutoMapper;
    using CommercialDocuments.Contracts.Invoices;
    using MediatR;
    using System.Net;
    using Inventory.Application.Transactions.Integration;
    using Inventory.Contracts.Transactions;
    using Inventory.Domain.Enums;
    using Inventory.Domain.Repositories;

    public sealed class CreateTransactionByInvoiceCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Transaction> _Repository,
        ISender sender, IMapper mapper,
        IServiceProvider provider, IInventoryBalanceRepository balanceRepository) : CreateCommandHandler<CreateTransactionByInvoiceCommand, Transaction>(_UnitOfWork, _Repository, mapper)
    {
        public override async Task<Result> Handle(CreateTransactionByInvoiceCommand request, CancellationToken cancellationToken)
        {
            await _UnitOfWork.BeginTransactionAsync();
            try
            {
                var invoice = (await sender.Send(new GetInvoiceInventoryImpactQuery(request.Id), cancellationToken)).Response;

                if (invoice == null)
                {
                    await _UnitOfWork.RollbackAsync();
                    return new Result(HttpStatusCode.NotFound, new List<Error> { new Error("Invoice not found") });
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
                    transaction = (await _Repository.GetByFilterAsync(e => e.Id == invoice.TransactionId, "TransactionProducts"))!;

                    if (transaction == null)
                    {
                        await _UnitOfWork.RollbackAsync();
                        return new Result(HttpStatusCode.NotFound, new List<Error> { new Error("Transaction not found") });
                    }

                    var originalTransactionTypeId = transaction.TypeId;
                    transaction.TransactionProducts?.Clear();
                    ApplyInvoice(invoice, transaction);
                    transaction.TypeId = expectedTransactionTypeId;
                    transactionTypeChanged = originalTransactionTypeId != expectedTransactionTypeId;
                }
                else
                {
                    transaction = ApplyInvoice(invoice, new Transaction());
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
                    await sender.Send(new SetInvoiceLinkedTransactionCommand(invoice.Id, transaction.Id), cancellationToken);

                    if (transactionTypeChanged)
                        await new TransactionJournalPostingService(provider).DeleteByTransactionIdAsync(transaction.Id);
                    await new TransactionJournalPostingService(provider).SyncAsync(transaction, invoice.TypeId);
                    await _Repository.UpdateAsync(transaction);

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
                return new Result(HttpStatusCode.InternalServerError, new List<Error> { new Error("Error while saving") });
            }
            catch (Exception ex)
            {
                await _UnitOfWork.RollbackAsync();
                return new Result(HttpStatusCode.InternalServerError, new List<Error> { new Error(ex.Message) });
            }
        }

        private static Transaction ApplyInvoice(InvoiceInventoryImpactDto invoice, Transaction transaction)
        {
            transaction.Date = invoice.Date;
            transaction.DealerId = invoice.DealerId;
            transaction.StockId = invoice.StockId;
            transaction.CreateUserId = invoice.CreateUserId;
            transaction.CreateDate = invoice.CreateDate;
            transaction.ShiftId = invoice.ShiftId;
            transaction.BranchId = invoice.BranchId;
            transaction.Notes = invoice.Notes;
            transaction.Posted = invoice.Posted;
            transaction.TransactionProducts = invoice.Lines.Select(line => new TransactionProduct
            {
                ProductId = line.ProductId,
                UnitId = line.UnitId,
                StockId = line.StockId,
                Quantity = line.Quantity,
                Cost = line.Price,
                Total = line.Quantity * line.Price,
                Notes = line.Notes
            }).ToList();
            return transaction;
        }
    }
}
