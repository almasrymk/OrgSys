namespace Inventory.Application.Postings;

using Inventory.Application.Transactions.Integration;
using Inventory.Domain.Enums;
using Inventory.Domain.Repositories;

/// <summary>
/// The single place that turns a posted document (InventoryReceipt/InventoryIssue/StockTransfer/
/// StockAdjustment) into the canonical InventoryMovement rows (Transaction/TransactionProduct) plus
/// the corresponding InventoryBalance update — brief §5/§70's "one canonical stock ledger" rule.
/// Every document's Post command handler calls exactly one of these methods after the aggregate's
/// own Post()/Ship()/Receive() transitions its status; this class never decides *whether* to post
/// (that's the aggregate's job), only *how* a decided post becomes ledger + balance facts.
/// Availability (InsufficientStockException) is enforced here, at the balance, not earlier — brief
/// §10: never silently allow negative inventory unless the warehouse explicitly opts in.
/// </summary>
public sealed class InventoryLedgerPoster(
    IRepository<Transaction> transactionRepository,
    IRepository<TransactionProduct> transactionProductRepository,
    IRepository<Stock> stockRepository,
    IInventoryBalanceRepository balanceRepository,
    IUnitOfWork unitOfWork,
    IServiceProvider serviceProvider)
{
    public async Task PostReceiptAsync(InventoryReceipt receipt, CancellationToken cancellationToken)
    {
        var stock = await stockRepository.GetByFilterAsync(s => s.Id == receipt.StockId, "");
        stock?.EnsureActiveForPosting();

        foreach (var line in receipt.Lines)
        {
            var transaction = await CreateMovementAsync(
                MovementType.Receipt, receipt.StockId, toStockId: null, receipt.DealerId,
                receipt.Date, receipt.CreateUserId, receipt.CreateDate, receipt.BranchId,
                line.Quantity * line.UnitCost, receipt.Notes,
                SourceDocumentType.InventoryReceipt, receipt.Id, line.Id);

            await CreateMovementLineAsync(transaction, line.ProductId, line.UnitId, receipt.StockId, line.Quantity, line.UnitCost, line.RowNumber, line.Notes);

            var balance = await balanceRepository.GetOrCreateTrackedAsync(line.ProductId, receipt.StockId, receipt.LocationId, line.BatchId, cancellationToken);
            balance.Receive(line.Quantity, line.UnitCost);
        }
    }

    public async Task PostIssueAsync(InventoryIssue issue, CancellationToken cancellationToken)
    {
        var stock = await stockRepository.GetByFilterAsync(s => s.Id == issue.StockId, "");
        stock?.EnsureActiveForPosting();
        var allowNegativeStock = stock?.AllowNegativeStock ?? false;

        foreach (var line in issue.Lines)
        {
            var balance = await balanceRepository.GetOrCreateTrackedAsync(line.ProductId, issue.StockId, issue.LocationId, line.BatchId, cancellationToken);
            balance.IssueOut(line.Quantity, allowNegativeStock);

            var transaction = await CreateMovementAsync(
                MovementType.Issue, issue.StockId, toStockId: null, issue.DealerId,
                issue.Date, issue.CreateUserId, issue.CreateDate, issue.BranchId,
                line.Quantity * balance.AverageCost, issue.Notes,
                SourceDocumentType.InventoryIssue, issue.Id, line.Id);

            await CreateMovementLineAsync(transaction, line.ProductId, line.UnitId, issue.StockId, line.Quantity, balance.AverageCost, line.RowNumber, line.Notes);
        }
    }

    /// <summary>Posts the TransferIssue side at FromStockId — brief §11: a transfer never edits a
    /// movement's WarehouseId, it always produces a separate issue-side movement.</summary>
    public async Task PostTransferShipmentAsync(StockTransfer transfer, CancellationToken cancellationToken)
    {
        var sourceStock = await stockRepository.GetByFilterAsync(s => s.Id == transfer.FromStockId, "");
        sourceStock?.EnsureActiveForPosting();

        foreach (var line in transfer.Lines)
        {
            var sourceBalance = await balanceRepository.GetOrCreateTrackedAsync(line.ProductId, transfer.FromStockId, null, line.BatchId, cancellationToken);
            sourceBalance.IssueOut(line.Quantity, sourceStock?.AllowNegativeStock ?? false);

            var transaction = await CreateMovementAsync(
                MovementType.TransferIssue, transfer.FromStockId, transfer.ToStockId, dealerId: null,
                transfer.Date, transfer.CreateUserId, transfer.CreateDate, transfer.BranchId,
                line.Quantity * sourceBalance.AverageCost, transfer.Notes,
                SourceDocumentType.StockTransfer, transfer.Id, line.Id);

            await CreateMovementLineAsync(transaction, line.ProductId, line.UnitId, transfer.FromStockId, line.Quantity, sourceBalance.AverageCost, line.RowNumber, line.Notes);
        }
    }

    /// <summary>Posts the TransferReceipt side at ToStockId, carrying forward the cost recorded at
    /// shipment time (never re-priced at the destination — brief §21: a movement's cost is fixed at
    /// posting time).</summary>
    public async Task PostTransferReceiptAsync(StockTransfer transfer, CancellationToken cancellationToken)
    {
        var destinationStock = await stockRepository.GetByFilterAsync(s => s.Id == transfer.ToStockId, "");
        destinationStock?.EnsureActiveForPosting();

        foreach (var line in transfer.Lines)
        {
            var sourceBalance = await balanceRepository.GetReadOnlyAsync(line.ProductId, transfer.FromStockId, null, line.BatchId, cancellationToken);
            var unitCost = sourceBalance?.AverageCost ?? 0;

            var destinationBalance = await balanceRepository.GetOrCreateTrackedAsync(line.ProductId, transfer.ToStockId, null, line.BatchId, cancellationToken);
            destinationBalance.Receive(line.Quantity, unitCost);

            var transaction = await CreateMovementAsync(
                MovementType.TransferReceipt, transfer.ToStockId, transfer.FromStockId, dealerId: null,
                transfer.Date, transfer.CreateUserId, transfer.CreateDate, transfer.BranchId,
                line.Quantity * unitCost, transfer.Notes,
                SourceDocumentType.StockTransfer, transfer.Id, line.Id);

            await CreateMovementLineAsync(transaction, line.ProductId, line.UnitId, transfer.ToStockId, line.Quantity, unitCost, line.RowNumber, line.Notes);
        }
    }

    public async Task PostAdjustmentAsync(StockAdjustment adjustment, CancellationToken cancellationToken)
    {
        var stock = await stockRepository.GetByFilterAsync(s => s.Id == adjustment.StockId, "");
        stock?.EnsureActiveForPosting();

        foreach (var line in adjustment.Lines)
        {
            var balance = await balanceRepository.GetOrCreateTrackedAsync(line.ProductId, adjustment.StockId, null, line.BatchId, cancellationToken);
            var movementType = line.Direction == MovementDirection.In ? MovementType.AdjustmentIncrease : MovementType.AdjustmentDecrease;
            var unitCost = balance.AverageCost;

            if (line.Direction == MovementDirection.In)
                balance.Receive(line.Quantity, unitCost);
            else
                balance.IssueOut(line.Quantity, stock?.AllowNegativeStock ?? false);

            var transaction = await CreateMovementAsync(
                movementType, adjustment.StockId, toStockId: null, dealerId: null,
                adjustment.Date, adjustment.CreateUserId, adjustment.CreateDate, adjustment.BranchId,
                line.Quantity * unitCost, adjustment.Notes,
                SourceDocumentType.StockAdjustment, adjustment.Id, line.Id);

            await CreateMovementLineAsync(transaction, line.ProductId, line.UnitId, adjustment.StockId, line.Quantity, unitCost, line.RowNumber, line.Notes);
        }
    }

    private async Task<Transaction> CreateMovementAsync(
        MovementType movementType, long stockId, long? toStockId, long? dealerId,
        DateTime date, long createUserId, DateTime createDate, long? branchId,
        decimal total, string? notes, SourceDocumentType sourceType, long sourceId, long sourceLineId)
    {
        var transaction = new Transaction
        {
            TypeId = (long)movementType,
            StockId = stockId,
            ToStockId = toStockId,
            DealerId = dealerId,
            Date = date,
            CreateUserId = createUserId,
            CreateDate = createDate,
            BranchId = branchId,
            Total = total,
            Notes = notes,
            SourceDocumentType = sourceType,
            SourceDocumentId = sourceId,
            SourceDocumentLineId = sourceLineId,
            Status = OrgSys.SharedKernel.Status.Approved
        };
        await transactionRepository.CreateAsync(transaction);
        // TransactionJournalPostingService.SyncAsync needs transaction.Id already assigned (it's
        // used as SourceDocumentId for the journal linkage) — same ordering the original
        // Transactions/Commands/CreateCommandHandler.cs uses: create, save (assigns Id), then sync.
        await unitOfWork.SaveChangeAsync(CancellationToken.None);

        // Reuses the existing Transaction -> Journal posting bridge unchanged (brief §24) — every
        // new document type posts a Transaction with the same TypeId semantics
        // (MovementType.Receipt=1 .. DamageLoss=8) that TransactionJournalPostingService already
        // resolves accounts for via Preference/Stock.AccountId, so no new accounting-integration
        // code is needed for InventoryReceipt/Issue/StockTransfer/StockAdjustment.
        await new TransactionJournalPostingService(serviceProvider).SyncAsync(transaction);

        return transaction;
    }

    private async Task CreateMovementLineAsync(Transaction transaction, long productId, long unitId, long stockId, decimal quantity, decimal cost, long rowNumber, string? notes)
    {
        var line = new TransactionProduct
        {
            Transaction = transaction,
            ProductId = productId,
            UnitId = unitId,
            StockId = stockId,
            Quantity = quantity,
            Cost = cost,
            Total = quantity * cost,
            RowNumber = rowNumber,
            Notes = notes
        };
        await transactionProductRepository.CreateAsync(line);
    }
}
