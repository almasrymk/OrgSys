namespace Inventory.Application.Transactions.Integration;

using Accounting.Contracts.Postings;
using Administration.Contracts.Preferences;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Transaction -> Journal posting bridge. Determines posting intent (which stock/dealer/transit
/// accounts, which currency) from Transaction/Preference/Stock/Currency/Invoice data —
/// Inventory.Application already read these across module boundaries before this move (documented
/// accepted exceptions, see docs/dependency-rules.md); Journal creation/update/deletion is owned by
/// Accounting.Application and reached only through Accounting.Contracts. Replaces (behavior
/// preserved) the legacy root Application project's Application.Commands.Org.Financials.Integration.
/// JournalTransaction.TransactionJournalIntegration — see the legacy-Application-elimination report.
/// </summary>
public sealed class TransactionJournalPostingService(IServiceProvider provider)
{
    private const string ReferenceTable = "transaction";

    public async Task SyncAsync(Transaction transaction, long? sourceInvoiceTypeId = null, bool force = false)
    {
        var sender = provider.GetRequiredService<ISender>();

        var existingJournal = (await sender.Send(
            new GetAccountingDocumentJournalQuery(ReferenceTable, transaction.Id, transaction.TypeId))).Response;

        var preferenceTypeId = transaction.TypeId switch
        {
            5 => 1,
            6 => 2,
            _ => transaction.TypeId
        };
        var preferences = (await sender.Send(new GetPreferenceValuesQuery("Transaction", preferenceTypeId))).Response
            ?? new Dictionary<string, string?>();

        var accountsIntegrationEnabled = PreferenceEquals(preferences, "AccountsIntegration", "1");
        var autoCreateJournalEnabled = PreferenceEquals(preferences, "AutoCreateJournalEntry", "1");
        var enabled = force
            || autoCreateJournalEnabled
            || (accountsIntegrationEnabled && existingJournal != null);

        if (!enabled)
        {
            await sender.Send(new DeleteAccountingDocumentJournalCommand(ReferenceTable, transaction.Id));
            transaction.HasJournal = false;
            return;
        }

        var (debitAccountId, creditAccountId) = await ResolveAccountsAsync(transaction, preferences, sourceInvoiceTypeId);
        if (debitAccountId <= 0 || creditAccountId <= 0)
        {
            if (force)
                throw new InvalidOperationException("The transaction accounts are not configured in transaction preferences or warehouse settings.");

            await sender.Send(new DeleteAccountingDocumentJournalCommand(ReferenceTable, transaction.Id));
            transaction.HasJournal = false;
            return;
        }

        var currencyRepository = provider.GetRequiredService<IRepository<Currency>>();
        var currency = await currencyRepository.GetByFilterAsync(e => e.IsDefault, "")
            ?? throw new InvalidOperationException("A default currency is required to create the transaction journal entry.");

        var lines = new List<AccountingPostingLine>
        {
            new(debitAccountId, transaction.Total, 0, transaction.Notes),
            new(creditAccountId, 0, transaction.Total, transaction.Notes)
        };

        var journalTypeId = transaction.TypeId == 7 ? 1 : 2;

        var result = await sender.Send(new PostAccountingDocumentCommand(
            ReferenceTable: ReferenceTable,
            SourceDocumentId: transaction.Id,
            SourceDocumentTypeId: transaction.TypeId,
            SourceDocumentCode: transaction.Code,
            JournalTypeId: journalTypeId,
            Date: transaction.Date,
            CreateDate: transaction.CreateDate,
            CreateUserId: transaction.CreateUserId,
            ModifyDate: transaction.ModifyDate,
            ModifyUserId: transaction.ModifyUserId,
            BranchId: transaction.BranchId,
            ShiftId: transaction.ShiftId,
            CurrencyId: currency.Id,
            Rate: currency.Rate,
            Note: transaction.Notes,
            Lines: lines));

        transaction.HasJournal = result.Response?.HasJournal ?? false;
    }

    public async Task DeleteByTransactionIdAsync(long transactionId)
    {
        var sender = provider.GetRequiredService<ISender>();
        await sender.Send(new DeleteAccountingDocumentJournalCommand(ReferenceTable, transactionId));
    }

    public async Task SetStatusByTransactionIdAsync(long transactionId, OrgSys.SharedKernel.Status status)
    {
        var sender = provider.GetRequiredService<ISender>();
        await sender.Send(new SetAccountingDocumentJournalStatusCommand(ReferenceTable, transactionId, status));
    }

    private async Task<(long DebitAccountId, long CreditAccountId)> ResolveAccountsAsync(
        Transaction transaction,
        IReadOnlyDictionary<string, string?> preferences,
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
        IReadOnlyDictionary<string, string?> preferences) =>
        (ParseAccountId(preferences, "StockAccount"), ParseAccountId(preferences, "OpeningBalanceAccount"));

    private static (long DebitAccountId, long CreditAccountId) ResolveInventoryDamageAccounts(
        IReadOnlyDictionary<string, string?> preferences) =>
        (ParseAccountId(preferences, "InventoryDamageExpenseAccount"), ParseAccountId(preferences, "StockAccount"));

    private async Task<(long DebitAccountId, long CreditAccountId)> ResolveAdditionAccountsAsync(
        Transaction transaction,
        IReadOnlyDictionary<string, string?> preferences,
        long? sourceInvoiceTypeId)
    {
        var counterKey = sourceInvoiceTypeId == 3
            ? "SalesReturnAccount"
            : await ResolveInvoiceCounterKeyAsync(transaction.Id, 3, "SalesReturnAccount", "PurchaseAccount");
        return (ParseAccountId(preferences, "StockAccount"), ParseAccountId(preferences, counterKey));
    }

    private async Task<(long DebitAccountId, long CreditAccountId)> ResolveIssueAccountsAsync(
        Transaction transaction,
        IReadOnlyDictionary<string, string?> preferences,
        long? sourceInvoiceTypeId)
    {
        var counterKey = sourceInvoiceTypeId == 4
            ? "PurchaseReturnAccount"
            : await ResolveInvoiceCounterKeyAsync(transaction.Id, 4, "PurchaseReturnAccount", "SalesAccount");
        return (ParseAccountId(preferences, counterKey), ParseAccountId(preferences, "StockAccount"));
    }

    private async Task<(long DebitAccountId, long CreditAccountId)> ResolveTransferAccountsAsync(
        Transaction transaction,
        IReadOnlyDictionary<string, string?> preferences)
    {
        var sourceAccountId = await GetStockAccountIdAsync(transaction.StockId);
        return (
            ParseAccountId(preferences, "TransitAccount"),
            sourceAccountId > 0 ? sourceAccountId : ParseAccountId(preferences, "SourceInventoryAccount"));
    }

    private async Task<(long DebitAccountId, long CreditAccountId)> ResolveReceivedAccountsAsync(
        Transaction transaction,
        IReadOnlyDictionary<string, string?> preferences)
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

    private static bool PreferenceEquals(IReadOnlyDictionary<string, string?> preferences, string key, string expected) =>
        preferences.TryGetValue(key, out var value) && value == expected;

    private static long ParseAccountId(IReadOnlyDictionary<string, string?> preferences, string key) =>
        preferences.TryGetValue(key, out var value) && long.TryParse(value, out var id) ? id : 0;
}
