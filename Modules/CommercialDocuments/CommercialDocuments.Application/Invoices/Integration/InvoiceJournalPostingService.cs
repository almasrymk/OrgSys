namespace CommercialDocuments.Application.Invoices.Integration;

using Accounting.Contracts.Postings;
using Administration.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Parties.Domain;

/// <summary>
/// Invoice -> Journal posting bridge. Determines posting intent (which accounts, what amounts)
/// from Invoice/Preference/Dealer data — the same cross-module Application reads this bridge
/// always made (Preference: Administration, Dealer: Parties, see docs/dependency-rules.md); Journal
/// creation/update/deletion is owned by Accounting.Application and reached only through
/// Accounting.Contracts. Replaces (behavior preserved) the legacy root Application project's
/// Application.Commands.Org.Financials.Integration.JournalInvoice.InvoiceJournalIntegration — see
/// the legacy-Application-elimination report.
/// </summary>
public sealed class InvoiceJournalPostingService(IServiceProvider provider)
{
    private const string ReferenceTable = "invoice";

    public async Task SyncAsync(CommercialDocuments.Domain.Invoice invoice, bool force = false)
    {
        var sender = provider.GetRequiredService<ISender>();
        var preferenceRepository = provider.GetRequiredService<IRepository<Preference>>();

        var existingJournal = (await sender.Send(
            new GetAccountingDocumentJournalQuery(ReferenceTable, invoice.Id, invoice.TypeId))).Response;

        var preferences = (await preferenceRepository.GetListByFilterAsync(
            e => e.Reference == "Invoice" && e.TypeId == invoice.TypeId))?.ToList() ?? [];

        var enabled = preferences.FirstOrDefault(e => e.Key == "AccountsIntegration")?.Value == "1"
            && (force
                || existingJournal != null
                || preferences.FirstOrDefault(e => e.Key == "AutoCreateJournalEntry")?.Value == "1");

        if (!enabled)
        {
            await sender.Send(new DeleteAccountingDocumentJournalCommand(ReferenceTable, invoice.Id));
            invoice.HasJournal = false;
            return;
        }

        var invoiceAccountKey = invoice.TypeId is 2 or 4 ? "PurchaseAccount" : "SalesAccount";
        var invoiceAccountId = ParseAccountId(preferences, invoiceAccountKey);
        var dealerRepository = provider.GetRequiredService<IRepository<Dealer>>();
        var dealer = await dealerRepository.GetByFilterAsync(e => e.Id == invoice.DealerId, "");
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

        var dealerIsDebit = invoice.TypeId is 1 or 4;
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
        var sender = provider.GetRequiredService<ISender>();
        await sender.Send(new DeleteAccountingDocumentJournalCommand(ReferenceTable, invoiceId));
    }

    public async Task SetStatusByInvoiceIdAsync(long invoiceId, OrgSys.SharedKernel.Status status)
    {
        var sender = provider.GetRequiredService<ISender>();
        await sender.Send(new SetAccountingDocumentJournalStatusCommand(ReferenceTable, invoiceId, status));
    }

    private static long ParseAccountId(IEnumerable<Preference> preferences, string key) =>
        long.TryParse(preferences.FirstOrDefault(e => e.Key == key)?.Value, out var id) ? id : 0;

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
