namespace Application.Commands.Org.Financials.Integration.JournalTransaction;

using Domain.Abstraction;
using Domain.Entities;
using Microsoft.Extensions.DependencyInjection;

internal sealed class TransactionJournalIntegration(IServiceProvider provider)
{
    private const string ReferenceTable = "transaction";

    public async Task SyncAsync(Transaction transaction, long? sourceInvoiceTypeId = null, bool force = false)
    {
        var journalRepository = provider.GetRequiredService<IRepository<Journal>>();
        var journalItemRepository = provider.GetRequiredService<IRepository<JournalItem>>();
        var preferenceRepository = provider.GetRequiredService<IRepository<Preference>>();

        var journal = await journalRepository.GetByFilterAsync(
            e => e.RefranceTable == ReferenceTable
                && e.RefranceId == transaction.Id
                && e.RefranceTypeId == transaction.TypeId,
            "JournalItems");

        var preferenceTypeId = transaction.TypeId switch
        {
            5 => 1,
            6 => 2,
            _ => transaction.TypeId
        };
        var preferences = (await preferenceRepository.GetListByFilterAsync(
            e => e.Reference == "Transaction" && e.TypeId == preferenceTypeId))?.ToList() ?? [];

        var accountsIntegrationEnabled = preferences.FirstOrDefault(e => e.Key == "AccountsIntegration")?.Value == "1";
        var autoCreateJournalEnabled = preferences.FirstOrDefault(e => e.Key == "AutoCreateJournalEntry")?.Value == "1";
        var enabled = force
            || autoCreateJournalEnabled
            || (accountsIntegrationEnabled && journal != null);

        if (!enabled)
        {
            await DeleteAsync(journal, journalRepository, journalItemRepository);
            transaction.HasJournal = false;
            return;
        }

        var (debitAccountId, creditAccountId) = await ResolveAccountsAsync(transaction, preferences, sourceInvoiceTypeId);
        if (debitAccountId <= 0 || creditAccountId <= 0)
        {
            if (force)
                throw new InvalidOperationException("The transaction accounts are not configured in transaction preferences or warehouse settings.");

            await DeleteAsync(journal, journalRepository, journalItemRepository);
            transaction.HasJournal = false;
            return;
        }

        var currencyRepository = provider.GetRequiredService<IRepository<Currency>>();
        var currency = await currencyRepository.GetByFilterAsync(e => e.IsDefault, "")
            ?? throw new InvalidOperationException("A default currency is required to create the transaction journal entry.");

        var items = new List<JournalItem>
        {
            new() { AccountId = debitAccountId, Debit = transaction.Total, Credit = 0, Note = transaction.Notes },
            new() { AccountId = creditAccountId, Debit = 0, Credit = transaction.Total, Note = transaction.Notes }
        };

        if (journal is null)
        {
            var journalTypeId = transaction.TypeId == 7 ? 1 : 2;
            var codeNumber = await journalRepository.AnyAsync(e => e.TypeId == journalTypeId)
                ? await journalRepository.GetMaxByFilterAsync(e => e.TypeId == journalTypeId, e => e.CodeNumber) + 1
                : 1;

            journal = new Journal
            {
                JournalTypeId = journalTypeId,
                TypeId = journalTypeId,
                CodeNumber = codeNumber,
                Code = codeNumber.ToString(),
                Date = transaction.Date,
                CreateDate = transaction.CreateDate,
                CreateUserId = transaction.CreateUserId,
                BranchId = transaction.BranchId,
                ShiftId = transaction.ShiftId,
                CurrencyId = currency.Id,
                Rate = currency.Rate,
                RefranceId = transaction.Id,
                RefranceCode = transaction.Code,
                RefranceTypeId = transaction.TypeId,
                RefranceTable = ReferenceTable,
                Note = transaction.Notes,
                JournalItems = items
            };
            await journalRepository.CreateAsync(journal);
        }
        else
        {
            await journalItemRepository.ShiftDeleteAsync(e => e.JournalId == journal.Id);
            foreach (var item in items)
                item.JournalId = journal.Id;
            await journalItemRepository.CreateAsync(items);

            journal.Date = transaction.Date;
            journal.ModifyDate = transaction.ModifyDate;
            journal.ModifyUserId = transaction.ModifyUserId;
            journal.BranchId = transaction.BranchId;
            journal.ShiftId = transaction.ShiftId;
            journal.CurrencyId = currency.Id;
            journal.Rate = currency.Rate;
            journal.RefranceCode = transaction.Code;
            journal.Note = transaction.Notes;
            await journalRepository.UpdateAsync(journal);
        }

        transaction.HasJournal = true;
    }

    public async Task DeleteByTransactionIdAsync(long transactionId)
    {
        var journalRepository = provider.GetRequiredService<IRepository<Journal>>();
        var journalItemRepository = provider.GetRequiredService<IRepository<JournalItem>>();
        var journals = await journalRepository.GetListByFilterAsync(
            e => e.RefranceTable == ReferenceTable && e.RefranceId == transactionId);

        foreach (var journal in journals ?? [])
            await DeleteAsync(journal, journalRepository, journalItemRepository);
    }

    public async Task SetStatusByTransactionIdAsync(long transactionId, Domain.Enums.Status status)
    {
        var journalRepository = provider.GetRequiredService<IRepository<Journal>>();
        var journals = await journalRepository.GetListByFilterAsync(
            e => e.RefranceTable == ReferenceTable && e.RefranceId == transactionId);

        foreach (var journal in journals ?? [])
        {
            journal.Status = status;
            await journalRepository.UpdateAsync(journal);
        }
    }

    private async Task<(long DebitAccountId, long CreditAccountId)> ResolveAccountsAsync(
        Transaction transaction,
        IEnumerable<Preference> preferences,
        long? sourceInvoiceTypeId)
    {
        return transaction.TypeId switch
        {
            1 => await ResolveAdditionAccountsAsync(transaction, preferences, sourceInvoiceTypeId),
            2 => await ResolveIssueAccountsAsync(transaction, preferences, sourceInvoiceTypeId),
            3 => await ResolveTransferAccountsAsync(transaction, preferences),
            4 => await ResolveReceivedAccountsAsync(transaction, preferences),
            5 => await ResolveAdditionAccountsAsync(transaction, preferences, sourceInvoiceTypeId),
            6 => await ResolveIssueAccountsAsync(transaction, preferences, sourceInvoiceTypeId),
            7 => ResolveOpeningBalanceAccounts(preferences),
            8 => ResolveInventoryDamageAccounts(preferences),
            _ => throw new InvalidOperationException($"Transaction type {transaction.TypeId} does not support journal integration.")
        };
    }

    private static (long DebitAccountId, long CreditAccountId) ResolveOpeningBalanceAccounts(
        IEnumerable<Preference> preferences) =>
        (ParseAccountId(preferences, "StockAccount"), ParseAccountId(preferences, "OpeningBalanceAccount"));

    private static (long DebitAccountId, long CreditAccountId) ResolveInventoryDamageAccounts(
        IEnumerable<Preference> preferences) =>
        (ParseAccountId(preferences, "InventoryDamageExpenseAccount"), ParseAccountId(preferences, "StockAccount"));

    private async Task<(long DebitAccountId, long CreditAccountId)> ResolveAdditionAccountsAsync(
        Transaction transaction,
        IEnumerable<Preference> preferences,
        long? sourceInvoiceTypeId)
    {
        var counterKey = sourceInvoiceTypeId == 3
            ? "SalesReturnAccount"
            : await ResolveInvoiceCounterKeyAsync(transaction.Id, 3, "SalesReturnAccount", "PurchaseAccount");
        return (ParseAccountId(preferences, "StockAccount"), ParseAccountId(preferences, counterKey));
    }

    private async Task<(long DebitAccountId, long CreditAccountId)> ResolveIssueAccountsAsync(
        Transaction transaction,
        IEnumerable<Preference> preferences,
        long? sourceInvoiceTypeId)
    {
        var counterKey = sourceInvoiceTypeId == 4
            ? "PurchaseReturnAccount"
            : await ResolveInvoiceCounterKeyAsync(transaction.Id, 4, "PurchaseReturnAccount", "SalesAccount");
        return (ParseAccountId(preferences, counterKey), ParseAccountId(preferences, "StockAccount"));
    }

    private async Task<(long DebitAccountId, long CreditAccountId)> ResolveTransferAccountsAsync(
        Transaction transaction,
        IEnumerable<Preference> preferences)
    {
        var sourceAccountId = await GetStockAccountIdAsync(transaction.StockId);
        return (
            ParseAccountId(preferences, "TransitAccount"),
            sourceAccountId > 0 ? sourceAccountId : ParseAccountId(preferences, "SourceInventoryAccount"));
    }

    private async Task<(long DebitAccountId, long CreditAccountId)> ResolveReceivedAccountsAsync(
        Transaction transaction,
        IEnumerable<Preference> preferences)
    {
        var destinationAccountId = await GetStockAccountIdAsync(transaction.StockId);
        return (
            destinationAccountId > 0 ? destinationAccountId : ParseAccountId(preferences, "DestinationInventoryAccount"),
            ParseAccountId(preferences, "TransitAccount"));
    }

    private async Task<string> ResolveInvoiceCounterKeyAsync(
        long transactionId,
        long returnInvoiceTypeId,
        string returnAccountKey,
        string defaultAccountKey)
    {
        var invoiceRepository = provider.GetRequiredService<IRepository<Invoice>>();
        var invoice = await invoiceRepository.GetByFilterAsync(e => e.TransactionId == transactionId, "");
        return invoice?.TypeId == returnInvoiceTypeId ? returnAccountKey : defaultAccountKey;
    }

    private async Task<long> GetStockAccountIdAsync(long? stockId)
    {
        if (stockId is not > 0)
            return 0;

        var stockRepository = provider.GetRequiredService<IRepository<Stock>>();
        var stock = await stockRepository.GetByFilterAsync(e => e.Id == stockId.Value, "");
        return stock?.AccountId ?? 0;
    }

    private static long ParseAccountId(IEnumerable<Preference> preferences, string key) =>
        long.TryParse(preferences.FirstOrDefault(e => e.Key == key)?.Value, out var id) ? id : 0;

    private static async Task DeleteAsync(
        Journal? journal,
        IRepository<Journal> journalRepository,
        IRepository<JournalItem> journalItemRepository)
    {
        if (journal is null)
            return;

        await journalItemRepository.ShiftDeleteAsync(e => e.JournalId == journal.Id);
        await journalRepository.ShiftDeleteAsync(e => e.Id == journal.Id);
    }
}
