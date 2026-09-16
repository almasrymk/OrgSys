namespace Accounting.Integration.Tests;

using Accounting.Application.Postings;
using Accounting.Domain;
using Accounting.Domain.Repositories;
using Accounting.Infrastructure.Persistence;
using Administration.Application.Preferences.Queries;
using Administration.Domain;
using CommercialDocuments.Application.Invoices.Integration;
using CommercialDocuments.Contracts.Invoices;
using CommercialDocuments.Domain;
using MediatR;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OrgSys.Infrastructure.Persistence;
using OrgSys.SharedKernel;
using Parties.Application.Dealers.Queries;
using Parties.Domain;
using Xunit;

/// <summary>
/// Database-backed coverage of the live invoice GL-posting path: InvoiceJournalPostingService
/// (CommercialDocuments) → Accounting.Contracts → PostAccountingDocumentCommandHandler, through a
/// real EF SQLite DbContext (not mocked repositories). Cancel/Redo currently mirror Status onto
/// the posted journal (SetAccountingDocumentJournalStatusCommand), which this test asserts.
/// </summary>
public sealed class InvoiceJournalPostingTests : IDisposable
{
    private readonly string _dbPath = Path.Combine(Path.GetTempPath(), $"orgsys-invoice-journal-{Guid.NewGuid():N}.db");
    private readonly SqliteConnection _keepAliveConnection;

    public InvoiceJournalPostingTests()
    {
        _keepAliveConnection = new SqliteConnection($"Data Source={_dbPath}");
        _keepAliveConnection.Open();

        using var setup = NewContext();
        setup.Database.EnsureCreated();
    }

    public void Dispose()
    {
        _keepAliveConnection.Dispose();
        SqliteConnection.ClearAllPools();
        try
        {
            if (File.Exists(_dbPath))
                File.Delete(_dbPath);
        }
        catch (IOException)
        {
        }
    }

    [Fact]
    public async Task Sync_SalesInvoice_CreatesBalancedJournalLines()
    {
        await SeedChartAndPreferencesAsync();
        var invoice = await SeedInvoiceAsync(net: 1000m);

        await using var context = NewContext();
        var sender = BuildSender(context);
        var service = new InvoiceJournalPostingService(sender);

        await service.SyncAsync(invoice);
        await context.SaveChangesAsync();

        Assert.True(invoice.HasJournal);

        var journal = await context.Set<Journal>()
            .Include("JournalItems")
            .SingleAsync(j => j.RefranceTable == "invoice" && j.RefranceId == invoice.Id);

        var lines = journal.JournalItems.OrderBy(l => l.AccountId).ToList();
        Assert.Equal(2, lines.Count);

        var dealerLine = lines.Single(l => l.AccountId == 200);
        Assert.Equal(1000m, dealerLine.Debit);
        Assert.Equal(0m, dealerLine.Credit);

        var salesLine = lines.Single(l => l.AccountId == 100);
        Assert.Equal(0m, salesLine.Debit);
        Assert.Equal(1000m, salesLine.Credit);

        Assert.Equal(journal.JournalItems.Sum(l => l.Debit), journal.JournalItems.Sum(l => l.Credit));
    }

    [Fact]
    public async Task CancelThenRedo_MirrorsJournalStatus()
    {
        await SeedChartAndPreferencesAsync();
        var invoice = await SeedInvoiceAsync(net: 250m);

        await using var context = NewContext();
        var sender = BuildSender(context);
        var service = new InvoiceJournalPostingService(sender);

        await service.SyncAsync(invoice);
        await context.SaveChangesAsync();

        await service.SetStatusByInvoiceIdAsync(invoice.Id, Status.Cancel);
        await context.SaveChangesAsync();

        var cancelled = await context.Set<Journal>()
            .SingleAsync(j => j.RefranceTable == "invoice" && j.RefranceId == invoice.Id);
        Assert.Equal(Status.Cancel, cancelled.Status);

        await service.SetStatusByInvoiceIdAsync(invoice.Id, Status.New);
        await context.SaveChangesAsync();

        var restored = await context.Set<Journal>()
            .SingleAsync(j => j.RefranceTable == "invoice" && j.RefranceId == invoice.Id);
        Assert.Equal(Status.New, restored.Status);
    }

    private TestOrgContext NewContext()
    {
        var options = new DbContextOptionsBuilder<TestOrgContext>()
            .UseSqlite($"Data Source={_dbPath}")
            .Options;
        return new TestOrgContext(options);
    }

    private ISender BuildSender(TestOrgContext context)
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddSingleton<IOrgContext>(context);
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IJournalRepository, JournalRepository>();
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(PostAccountingDocumentCommandHandler).Assembly);
            cfg.RegisterServicesFromAssembly(typeof(GetPreferenceValuesQueryHandler).Assembly);
            cfg.RegisterServicesFromAssembly(typeof(GetDealerByIdContractQueryHandler).Assembly);
        });
        return services.BuildServiceProvider().GetRequiredService<ISender>();
    }

    private async Task SeedChartAndPreferencesAsync()
    {
        await using var context = NewContext();
        context.Add(new Account { Id = 100, Name = "Sales", IsPostable = true, AccountTypeId = 1 });
        context.Add(new Account { Id = 200, Name = "AR", IsPostable = true, AccountTypeId = 1 });
        context.Add(new Dealer { Id = 1, Name = "Customer", TypeId = 1, AccountId = 200 });
        context.Add(new Preference { Id = 1, Reference = "Invoice", TypeId = (long)InvoiceTypeId.Sales, Key = "AccountsIntegration", Value = "1" });
        context.Add(new Preference { Id = 2, Reference = "Invoice", TypeId = (long)InvoiceTypeId.Sales, Key = "AutoCreateJournalEntry", Value = "1" });
        context.Add(new Preference { Id = 3, Reference = "Invoice", TypeId = (long)InvoiceTypeId.Sales, Key = "SalesAccount", Value = "100" });
        await context.SaveChangesAsync();
    }

    private async Task<Invoice> SeedInvoiceAsync(decimal net)
    {
        await using var context = NewContext();
        var invoice = new Invoice
        {
            DealerId = 1,
            TypeId = (long)InvoiceTypeId.Sales,
            Code = "INV-1",
            Date = new DateTime(2026, 9, 16),
            CreateDate = new DateTime(2026, 9, 16),
            CreateUserId = 1,
            CurrencyId = 1,
            Rate = 1,
            Net = net,
            Total = net,
            PaymentTypeId = 1,
        };
        context.Add(invoice);
        await context.SaveChangesAsync();
        return invoice;
    }
}
