namespace Application.Commands.Org.Transactions.Inventory.Integration;

using Application.Commands.Org.Financials.Integration.JournalTransaction;
using Domain.Abstraction;
using Domain.Entities;
using Microsoft.Extensions.DependencyInjection;

internal sealed class InventoryAdjustmentIntegration(IServiceProvider provider)
{
    private const long AdjustmentInTypeId = 5;
    private const long AdjustmentOutTypeId = 6;

    public async Task SyncAsync(Inventory inventory, CancellationToken cancellationToken = default, bool force = false)
    {
        var preferenceRepository = provider.GetRequiredService<IRepository<Preference>>();
        var preference = await preferenceRepository.GetByFilterAsync(
            e => e.Reference == "Inventory"
                && e.Key == "AutoCreateAdjustment"
                && (e.TypeId == inventory.TypeId || e.TypeId == 0),
            string.Empty);

        if (!force && preference?.Value != "1")
            return;

        var transactionRepository = provider.GetRequiredService<IRepository<Transaction>>();
        var transactionProductRepository = provider.GetRequiredService<IRepository<TransactionProduct>>();
        var existing = (await transactionRepository.GetListByFilterAsync(
            e => e.InventoryId == inventory.Id
                && e.Status != Domain.Enums.Status.Deleted
                && e.Hide != true
                && (e.TypeId == AdjustmentInTypeId || e.TypeId == AdjustmentOutTypeId),
            "TransactionProducts"))?.ToList() ?? [];

        await SyncTypeAsync(inventory, AdjustmentInTypeId, inventory.InventoryProducts?.Where(e => e.DiffQuantity > 0) ?? [],
            existing.FirstOrDefault(e => e.TypeId == AdjustmentInTypeId), transactionRepository, transactionProductRepository);
        await SyncTypeAsync(inventory, AdjustmentOutTypeId, inventory.InventoryProducts?.Where(e => e.DiffQuantity < 0) ?? [],
            existing.FirstOrDefault(e => e.TypeId == AdjustmentOutTypeId), transactionRepository, transactionProductRepository);
        inventory.HasAdjustment = inventory.InventoryProducts?.Any(e => e.DiffQuantity != 0) == true;
    }

    public async Task SetStatusAsync(long inventoryId, Domain.Enums.Status status)
    {
        var transactionRepository = provider.GetRequiredService<IRepository<Transaction>>();
        var transactions = await transactionRepository.GetListByFilterAsync(e => e.InventoryId == inventoryId);
        foreach (var transaction in transactions ?? [])
        {
            transaction.Status = status;
            await transactionRepository.UpdateAsync(transaction);
        }
    }

    public async Task DeleteAsync(long inventoryId)
    {
        var transactionRepository = provider.GetRequiredService<IRepository<Transaction>>();
        var transactionProductRepository = provider.GetRequiredService<IRepository<TransactionProduct>>();
        var transactions = await transactionRepository.GetListByFilterAsync(e => e.InventoryId == inventoryId);
        foreach (var transaction in transactions ?? [])
        {
            await new TransactionJournalIntegration(provider).DeleteByTransactionIdAsync(transaction.Id);
            await transactionProductRepository.ShiftDeleteAsync(e => e.TransactionId == transaction.Id);
            await transactionRepository.ShiftDeleteAsync(e => e.Id == transaction.Id);
        }
    }

    private async Task SyncTypeAsync(
        Inventory inventory,
        long typeId,
        IEnumerable<InventoryProduct> sourceProducts,
        Transaction? transaction,
        IRepository<Transaction> transactionRepository,
        IRepository<TransactionProduct> transactionProductRepository)
    {
        var products = sourceProducts.Where(e => e.DiffQuantity != 0).ToList();
        if (products.Count == 0)
        {
            if (transaction != null)
            {
                await new TransactionJournalIntegration(provider).DeleteByTransactionIdAsync(transaction.Id);
                await transactionProductRepository.ShiftDeleteAsync(e => e.TransactionId == transaction.Id);
                await transactionRepository.ShiftDeleteAsync(e => e.Id == transaction.Id);
            }
            return;
        }

        var notes = string.IsNullOrWhiteSpace(inventory.Notes)
            ? $"InventoryId: {inventory.Id}"
            : $"InventoryId: {inventory.Id} - {inventory.Notes}";

        if (transaction == null)
        {
            var serialTypeId = typeId == AdjustmentInTypeId ? 1L : 2L;
            var codeNumber = await transactionRepository.AnyAsync(e => e.TypeId == serialTypeId || e.TypeId == typeId)
                ? await transactionRepository.GetMaxByFilterAsync(e => e.TypeId == serialTypeId || e.TypeId == typeId, e => e.CodeNumber) + 1
                : 1;
            transaction = new Transaction
            {
                InventoryId = inventory.Id,
                TypeId = typeId,
                CodeNumber = codeNumber,
                Code = codeNumber.ToString(),
                Date = inventory.Date,
                CreateDate = inventory.CreateDate,
                CreateUserId = inventory.CreateUserId,
                BranchId = inventory.BranchId,
                ShiftId = inventory.ShiftId,
                StockId = inventory.StockId,
                Notes = notes,
                Total = 0,
                Status = inventory.Status
            };
            await transactionRepository.CreateAsync(transaction);
        }
        else
        {
            await transactionProductRepository.ShiftDeleteAsync(e => e.TransactionId == transaction.Id);
            transaction.Date = inventory.Date;
            transaction.ModifyDate = inventory.ModifyDate;
            transaction.ModifyUserId = inventory.ModifyUserId;
            transaction.BranchId = inventory.BranchId;
            transaction.ShiftId = inventory.ShiftId;
            transaction.StockId = inventory.StockId;
            transaction.Notes = notes;
            transaction.Status = inventory.Status;
            await transactionRepository.UpdateAsync(transaction);
        }

        var rowNumber = 1L;
        var transactionProducts = products.Select(e => new TransactionProduct
        {
            Transaction = transaction,
            TransactionId = transaction.Id,
            RowNumber = rowNumber++,
            ProductId = e.ProductId,
            UnitId = e.UnitId,
            StockId = inventory.StockId,
            Quantity = Math.Abs(e.DiffQuantity),
            Cost = 0,
            Total = 0,
            Notes = $"InventoryId: {inventory.Id}"
        }).ToList();
        await transactionProductRepository.CreateAsync(transactionProducts);
    }
}
