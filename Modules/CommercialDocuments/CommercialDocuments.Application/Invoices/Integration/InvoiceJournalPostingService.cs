namespace CommercialDocuments.Application.Invoices.Integration;

using Accounting.Contracts.Postings;
using Administration.Contracts.Preferences;
using CommercialDocuments.Contracts.Invoices;
using MediatR;
using Parties.Contracts.Dealers;

/// <summary>
/// Invoice → Journal posting bridge. Determines posting intent (which accounts, what amounts)
/// from Invoice facts plus Contracts lookups (Administration preferences, Parties dealer account);
/// Journal creation/update/deletion is owned by Accounting.Application and reached only through
/// Accounting.Contracts. Replaces the legacy root Application project's InvoiceJournalIntegration.
/// </summary>
public sealed class InvoiceJournalPostingService(ISender sender)
{
    private const string ReferenceTable = "invoice";

    public async Task SyncAsync(CommercialDocuments.Domain.Invoice invoice, bool force = false)
    {
        var existingJournal = (await sender.Send(
            new GetAccountingDocumentJournalQuery(ReferenceTable, invoice.Id, invoice.TypeId))).Response;

        var preferences = (await sender.Send(
            new GetPreferenceValuesQuery("Invoice", invoice.TypeId))).Response
            ?? new Dictionary<string, string?>();

        var enabled = PreferenceEquals(preferences, "AccountsIntegration", "1")
            && (force
                || existingJournal != null
                || PreferenceEquals(preferences, "AutoCreateJournalEntry", "1"));

        if (!enabled)
        {
            await sender.Send(new DeleteAccountingDocumentJournalCommand(ReferenceTable, invoice.Id));
            invoice.HasJournal = false;
            return;
        }

        var invoiceAccountKey = invoice.TypeId is (long)InvoiceTypeId.Purchase or (long)InvoiceTypeId.PurchaseReturn
            ? "PurchaseAccount"
            : "SalesAccount";
        var invoiceAccountId = ParseAccountId(preferences, invoiceAccountKey);
        var dealer = (await sender.Send(new GetDealerByIdQuery(invoice.DealerId))).Response;
        var dealerAccountId = dealer?.AccountId is > 0
            ? dealer.AccountId.Value
            : ParseAccountId(preferences, "DealerAccount");
        var taxAccountId = ParseAccountId(preferences, "TaxAccount");

        if (invoiceAccountId <= 0)
            throw new InvalidOperationException("The invoice account is not configured in invoice preferences.");
        if (dealerAccountId <= 0)
            throw new InvalidOperationException("The selected dealer does not have an account and DealerAccount is not configured in invoice preferences.");
        var taxAmount = CalculateTaxAmount(invoice);
        if (taxAmount != 0 && taxAccountId <= 0)
            throw new InvalidOperationException("The tax account is not configured in invoice preferences.");

        var dealerIsDebit = invoice.TypeId is (long)InvoiceTypeId.Sales or (long)InvoiceTypeId.PurchaseReturn;
        var amount = invoice.Net;
        var invoiceAmount = amount - taxAmount;
        var lines = new List<AccountingPostingLine>
        {
            new(dealerAccountId, dealerIsDebit ? amount : 0, dealerIsDebit ? 0 : amount, invoice.Notes),
            new(invoiceAccountId, dealerIsDebit ? 0 : invoiceAmount, dealerIsDebit ? invoiceAmount : 0, invoice.Notes)
        };

        if (taxAmount != 0)
            lines.Add(new AccountingPostingLine(taxAccountId, dealerIsDebit ? 0 : taxAmount, dealerIsDebit ? taxAmount : 0, invoice.Notes));

        var result = await sender.Send(new PostAccountingDocumentCommand(
            ReferenceTable: ReferenceTable,
            SourceDocumentId: invoice.Id,
            SourceDocumentTypeId: invoice.TypeId,
            SourceDocumentCode: invoice.Code,
            JournalTypeId: 2,
            Date: invoice.Date,
            CreateDate: invoice.CreateDate,
            CreateUserId: invoice.CreateUserId,
            ModifyDate: invoice.ModifyDate,
            ModifyUserId: invoice.ModifyUserId,
            BranchId: invoice.BranchId,
            ShiftId: invoice.ShiftId,
            CurrencyId: invoice.CurrencyId,
            Rate: invoice.Rate,
            Note: invoice.Notes,
            Lines: lines));

        invoice.HasJournal = result.Response?.HasJournal ?? false;
    }

    public async Task DeleteByInvoiceIdAsync(long invoiceId)
    {
        await sender.Send(new DeleteAccountingDocumentJournalCommand(ReferenceTable, invoiceId));
    }

    public async Task SetStatusByInvoiceIdAsync(long invoiceId, OrgSys.SharedKernel.Status status)
    {
        await sender.Send(new SetAccountingDocumentJournalStatusCommand(ReferenceTable, invoiceId, status));
    }

    private static bool PreferenceEquals(IReadOnlyDictionary<string, string?> preferences, string key, string expected) =>
        preferences.TryGetValue(key, out var value) && value == expected;

    private static long ParseAccountId(IReadOnlyDictionary<string, string?> preferences, string key) =>
        preferences.TryGetValue(key, out var value) && long.TryParse(value, out var id) ? id : 0;

    private static decimal CalculateTaxAmount(CommercialDocuments.Domain.Invoice invoice)
    {
        if (invoice.Tax == 0)
            return 0;

        if (invoice.TaxType == 1)
            return invoice.Tax;

        if (invoice.TaxType != 2)
            return 0;

        var discountAmount = invoice.DiscountType == 2
            ? invoice.Total * invoice.Discount / 100
            : invoice.DiscountType == 1 ? invoice.Discount : 0;
        var taxableAmount = invoice.Total - discountAmount;

        return taxableAmount * invoice.Tax / 100;
    }
}
