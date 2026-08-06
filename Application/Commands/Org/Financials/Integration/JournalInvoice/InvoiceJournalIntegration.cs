namespace Application.Commands.Org.Financials.Integration.JournalInvoice;

using Domain.Abstraction;
using Domain.Entities;
using Microsoft.Extensions.DependencyInjection;

internal sealed class InvoiceJournalIntegration(IServiceProvider provider)
{
    private const string ReferenceTable = "invoice";

    public async Task SyncAsync(Domain.Entities.Invoice invoice)
    {
        var journalRepository = provider.GetRequiredService<IRepository<Journal>>();
        var journalItemRepository = provider.GetRequiredService<IRepository<JournalItem>>();
        var preferenceRepository = provider.GetRequiredService<IRepository<Preference>>();

        var journal = await journalRepository.GetByFilterAsync(
            e => e.RefranceTable == ReferenceTable
                && e.RefranceId == invoice.Id
                && e.RefranceTypeId == invoice.TypeId,
            "JournalItems");

        var preferences = (await preferenceRepository.GetListByFilterAsync(
            e => e.Reference == "Invoice" && e.TypeId == invoice.TypeId))?.ToList() ?? [];

        var enabled = preferences.FirstOrDefault(e => e.Key == "AccountsIntegration")?.Value == "1"
            && preferences.FirstOrDefault(e => e.Key == "AutoCreateJournalEntry")?.Value == "1";

        if (!enabled)
        {
            await DeleteAsync(journal, journalRepository, journalItemRepository);
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
        var items = new List<JournalItem>
        {
            new()
            {
                AccountId = dealerAccountId,
                Debit = dealerIsDebit ? amount : 0,
                Credit = dealerIsDebit ? 0 : amount,
                Note = invoice.Notes
            },
            new()
            {
                AccountId = invoiceAccountId,
                Debit = dealerIsDebit ? 0 : invoiceAmount,
                Credit = dealerIsDebit ? invoiceAmount : 0,
                Note = invoice.Notes
            }
        };

        if (taxAmount != 0)
        {
            items.Add(new JournalItem
            {
                AccountId = taxAccountId,
                Debit = dealerIsDebit ? 0 : taxAmount,
                Credit = dealerIsDebit ? taxAmount : 0,
                Note = invoice.Notes
            });
        }

        if (journal is null)
        {
            long codeNumber = 1;
            if(await journalRepository.AnyAsync(e => e.TypeId == 2))
                codeNumber = await journalRepository.GetMaxByFilterAsync(e => e.TypeId == 2 , e=>e.CodeNumber) + 1;
            //var codeNumber = (journals?.Select(e => e.CodeNumber).DefaultIfEmpty(0).Max() ?? 0) + 1;
            journal = new Journal
            {
                JournalTypeId = 2,
                TypeId = 2,
                CodeNumber = codeNumber,
                Code = codeNumber.ToString(),
                Date = invoice.Date,
                CreateDate = invoice.CreateDate,
                CreateUserId = invoice.CreateUserId,
                BranchId = invoice.BranchId,
                ShiftId = invoice.ShiftId,
                CurrencyId = invoice.CurrencyId,
                Rate = invoice.Rate,
                RefranceId = invoice.Id,
                RefranceCode = invoice.Code,
                RefranceTypeId = invoice.TypeId,
                RefranceTable = ReferenceTable,
                Note = invoice.Notes,
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

            journal.Date = invoice.Date;
            journal.ModifyDate = invoice.ModifyDate;
            journal.ModifyUserId = invoice.ModifyUserId;
            journal.BranchId = invoice.BranchId;
            journal.ShiftId = invoice.ShiftId;
            journal.CurrencyId = invoice.CurrencyId;
            journal.Rate = invoice.Rate;
            journal.RefranceCode = invoice.Code;
            journal.Note = invoice.Notes;
            await journalRepository.UpdateAsync(journal);
        }

        invoice.HasJournal = true;
    }

    public async Task DeleteByInvoiceIdAsync(long invoiceId)
    {
        var journalRepository = provider.GetRequiredService<IRepository<Journal>>();
        var journalItemRepository = provider.GetRequiredService<IRepository<JournalItem>>();
        var journals = await journalRepository.GetListByFilterAsync(
            e => e.RefranceTable == ReferenceTable && e.RefranceId == invoiceId);

        foreach (var journal in journals ?? [])
            await DeleteAsync(journal, journalRepository, journalItemRepository);
    }

    public async Task SetStatusByInvoiceIdAsync(long invoiceId, Domain.Enums.Status status)
    {
        var journalRepository = provider.GetRequiredService<IRepository<Journal>>();
        var journals = await journalRepository.GetListByFilterAsync(
            e => e.RefranceTable == ReferenceTable && e.RefranceId == invoiceId);

        foreach (var journal in journals ?? [])
        {
            journal.Status = status;
            await journalRepository.UpdateAsync(journal);
        }
    }

    private static long ParseAccountId(IEnumerable<Preference> preferences, string key) =>
        long.TryParse(preferences.FirstOrDefault(e => e.Key == key)?.Value, out var id) ? id : 0;

    private static decimal CalculateTaxAmount(Domain.Entities.Invoice invoice)
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
