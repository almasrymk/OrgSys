namespace Inventory.Application.Transactions.Integration;

using global::Application.Commands.Org.Financials.Integration.JournalTransaction;
using Microsoft.Extensions.DependencyInjection;

internal sealed class TransferReceivedIntegration(IServiceProvider provider)
{
    public async Task DeleteReceivedAsync(Transaction transfer)
    {
        if (transfer.TypeId != 3)
            return;

        var transactionRepository = provider.GetRequiredService<IRepository<Transaction>>();
        var received = transfer.ParentId > 0
            ? await transactionRepository.GetByFilterAsync(
                e => e.Id == transfer.ParentId && e.TypeId == 4,
                string.Empty)
            : await transactionRepository.GetByFilterAsync(
                e => e.TypeId == 4 && e.ParentId == transfer.Id,
                string.Empty);

        if (received is null)
            return;

        await new TransactionJournalIntegration(provider).DeleteByTransactionIdAsync(received.Id);
        var productRepository = provider.GetRequiredService<IRepository<TransactionProduct>>();
        await productRepository.ShiftDeleteAsync(e => e.TransactionId == received.Id);
        await transactionRepository.ShiftDeleteAsync(e => e.Id == received.Id);
    }

    public async Task SyncAsync(Transaction transfer, CancellationToken cancellationToken = default, bool force = false)
    {
        if (transfer.TypeId != 3)
            return;

        var preferenceRepository = provider.GetRequiredService<IRepository<Preference>>();
        var autoReceived = await preferenceRepository.GetByFilterAsync(
            e => e.Reference == "Transaction" && e.TypeId == 3 && e.Key == "AutoReceived",
            string.Empty);
        if (!force && autoReceived?.Value != "1")
            return;

        var transactionRepository = provider.GetRequiredService<IRepository<Transaction>>();
        var productRepository = provider.GetRequiredService<IRepository<TransactionProduct>>();
        var received = transfer.ParentId > 0
            ? await transactionRepository.GetByFilterAsync(e => e.Id == transfer.ParentId && e.TypeId == 4, "TransactionProducts")
            : await transactionRepository.GetByFilterAsync(e => e.TypeId == 4 && e.ParentId == transfer.Id, "TransactionProducts");

        if (received == null)
        {
            var codeNumber = await transactionRepository.AnyAsync(e => e.TypeId == 4)
                ? await transactionRepository.GetMaxByFilterAsync(e => e.TypeId == 4, e => e.CodeNumber) + 1
                : 1;
            received = new Transaction
            {
                TypeId = 4,
                ParentId = transfer.Id,
                CodeNumber = codeNumber,
                Code = codeNumber.ToString(),
                CreateDate = transfer.CreateDate,
                CreateUserId = transfer.CreateUserId
            };
            await transactionRepository.CreateAsync(received);
        }
        else
        {
            await productRepository.ShiftDeleteAsync(e => e.TransactionId == received.Id);
        }

        received.Date = transfer.Date;
        received.ModifyDate = transfer.ModifyDate;
        received.ModifyUserId = transfer.ModifyUserId;
        received.BranchId = transfer.BranchId;
        received.ShiftId = transfer.ShiftId;
        received.StockId = transfer.ToStockId;
        received.ToStockId = null;
        received.DealerId = transfer.DealerId;
        var receivedNotes = $"TransferId: {transfer.Id}"
            + (string.IsNullOrWhiteSpace(transfer.Notes) ? string.Empty : $" - {transfer.Notes}");
        received.Notes = receivedNotes.Length <= 500 ? receivedNotes : receivedNotes[..500];
        received.Total = transfer.Total;
        received.Status = transfer.Status;

        var rowNumber = 1L;
        received.TransactionProducts = (transfer.TransactionProducts ?? [])
            .Select(e => new TransactionProduct
            {
                Transaction = received,
                TransactionId = received.Id,
                RowNumber = rowNumber++,
                ProductId = e.ProductId,
                UnitId = e.UnitId,
                StockId = transfer.ToStockId,
                Quantity = e.Quantity,
                Cost = e.Cost,
                Total = e.Total,
                Notes = e.Notes
            }).ToList();

        await transactionRepository.UpdateAsync(received);
        await provider.GetRequiredService<IUnitOfWork>().SaveChangeAsync(cancellationToken);

        transfer.ParentId = received.Id;
        received.ParentId = transfer.Id;
        await transactionRepository.UpdateAsync(transfer);
        await transactionRepository.UpdateAsync(received);

        var autoCreateReceivedJournal = await preferenceRepository.GetByFilterAsync(
            e => e.Reference == "Transaction"
                && e.TypeId == 4
                && e.Key == "AutoCreateJournalEntry",
            string.Empty);
        if (autoCreateReceivedJournal?.Value == "1")
        {
            // Keep this non-forced so incomplete account configuration does not roll back
            // the Transfer and its automatically generated Received transaction.
            await new TransactionJournalIntegration(provider).SyncAsync(received);
            await provider.GetRequiredService<IUnitOfWork>().SaveChangeAsync(cancellationToken);
        }
    }
}
