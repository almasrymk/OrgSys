namespace OrgSys.DatabaseMigrator.Persistence
{
    using System.Reflection;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.EntityFrameworkCore.Migrations;
    using Microsoft.EntityFrameworkCore.Diagnostics;
    using Microsoft.EntityFrameworkCore.Infrastructure;
    using Microsoft.EntityFrameworkCore.Migrations.Internal;
    using OrgSys.DatabaseMigrator.Seeding;
    using Administration.Infrastructure.Seeding;
    using Sales.Infrastructure.Seeding;
    using Accounting.Infrastructure.Seeding;
    using CommercialDocuments.Infrastructure.Seeding;
    using Inventory.Infrastructure.Seeding;
    using MasterData.Infrastructure.Seeding;
    using Organization.Infrastructure.Seeding;
    using Parties.Infrastructure.Seeding;
    using Treasury.Infrastructure.Seeding;
    using SaaS.Infrastructure.Seeding;

    public class OrgContext : DbContext , IOrgContext
    {
        //public string Schema { get; set; } = "org";

        public OrgContext(DbContextOptions<OrgContext> options) : base(options)
        {

        }
         
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
                .AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: false)
                .AddEnvironmentVariables();
            IConfigurationRoot config = builder.Build();
            string assemblyName = "" + typeof(OrgContext).Namespace;
            optionsBuilder
                .UseSqlServer(
                    config.GetConnectionString("OrgConnection"),
                    e => e.MigrationsHistoryTable("__MigrationsHistory"))
                //.ReplaceService<IModelCacheKeyFactory, DbSchemaAwareModelCacheKeyFactory>()
                //.ReplaceService<IMigrationsAssembly, DbSchemaAwareMigrationAssembly>()
                .UseSeeding((context, _) =>
                {
                    CreateSeederCoordinator().Seed(context);
                })
                .UseAsyncSeeding((context, _, _) =>
                {
                    CreateSeederCoordinator().Seed(context);
                    return Task.CompletedTask;
                });
        }

        // Matches the legacy `new InitialData()` pattern exactly: EF's UseSeeding/UseAsyncSeeding
        // hooks run before the DI container is available, so the coordinator and every module
        // seeder it calls are plain, dependency-free classes constructed directly here.
        private static IDataSeederCoordinator CreateSeederCoordinator() =>
            new DataSeederCoordinator(
                new AdministrationDataSeeder(),
                new SalesDataSeeder(),
                new AccountingDataSeeder(),
                new CommercialDocumentsDataSeeder(),
                new InventoryDataSeeder(),
                new MasterDataDataSeeder(),
                new OrganizationDataSeeder(),
                new PartiesDataSeeder(),
                new TreasuryDataSeeder(),
                new SaaSDataSeeder());

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Journal>()
                .Property(journal => journal.Rate)
                .HasPrecision(18, 2);

            // Accounting.Domain must not reference MasterData.Domain (see the Accounting DDD
            // cleanup report) — Journal.Currency was dropped in favor of the plain CurrencyId
            // scalar. Fluent "no navigation" relationship, same pattern as MovementModel's
            // CreateUser/ModifyUser/Shift/Branch, keeps the exact same FK column/constraint.
            // No explicit OnDelete: CurrencyId is a required (non-nullable) FK, so EF's default
            // (Cascade) matches what the dropped [ForeignKey("Currency")] attribute + convention
            // produced before — verified with `dotnet ef migrations has-pending-model-changes`.
            modelBuilder.Entity<Journal>()
                .HasOne(typeof(Currency)).WithMany()
                .HasForeignKey("CurrencyId");

            modelBuilder.Entity<CashBox>()
                .HasOne(e => e.FinancialAccount).WithOne(e => e.CashBox)
                .HasForeignKey<CashBox>(e => e.FinancialAccountId).OnDelete(DeleteBehavior.Restrict);
            // CashBox.KeeperUser navigation dropped (Treasury.Domain must not reference
            // Administration.Domain). Fluent "no navigation" FK keeps the same column/constraint.
            modelBuilder.Entity<CashBox>()
                .HasOne(typeof(User)).WithMany()
                .HasForeignKey("KeeperUserId");
            modelBuilder.Entity<CashBox>()
                .HasOne(typeof(Branch)).WithMany()
                .HasForeignKey("BranchId");
            modelBuilder.Entity<BankAccount>()
                .HasOne(typeof(Branch)).WithMany()
                .HasForeignKey("BranchId");
            modelBuilder.Entity<BankAccount>()
                .HasOne(e => e.FinancialAccount).WithOne(e => e.BankAccount)
                .HasForeignKey<BankAccount>(e => e.FinancialAccountId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Financial>()
                .HasOne(e => e.FinancialAccount).WithMany()
                .HasForeignKey(e => e.FinancialAccountId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Financial>()
                .HasOne(e => e.ContraFinancialAccount).WithMany()
                .HasForeignKey(e => e.ContraFinancialAccountId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<FinancialTransfer>()
                .HasOne(e => e.FromFinancialAccount).WithMany()
                .HasForeignKey(e => e.FromFinancialAccountId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<FinancialTransfer>()
                .HasOne(e => e.ToFinancialAccount).WithMany()
                .HasForeignKey(e => e.ToFinancialAccountId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Journal>()
                .HasOne(e => e.FiscalYear).WithMany()
                .HasForeignKey(e => e.FiscalYearId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Journal>()
                .HasOne(e => e.FiscalPeriod).WithMany()
                .HasForeignKey(e => e.FiscalPeriodId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Journal>()
                .HasIndex(e => e.Date);
            modelBuilder.Entity<Journal>()
                .HasOne(e => e.OriginalJournal).WithOne(e => e.ReversalJournal)
                .HasForeignKey<Journal>(e => e.OriginalJournalId).OnDelete(DeleteBehavior.Restrict);
            // At most one reversing entry per original journal — enforced at the DB level too,
            // not just in the handler, so a race between two concurrent Reverse calls can't slip through.
            modelBuilder.Entity<Journal>()
                .HasIndex(e => e.OriginalJournalId)
                .IsUnique()
                .HasFilter("[OriginalJournalId] IS NOT NULL");

            // Idempotency guard for the inbound Invoice-posted/OpeningBalance integrations (brief
            // §8/§32) — the same source document must never create two Receivables for the same
            // customer, enforced at the DB level, not just in the handler. CustomerId is included
            // because OpeningBalance's SourceDocumentId is a FiscalYearId, shared across every
            // customer's opening balance in that year (unlike SalesInvoice's SourceDocumentId, an
            // InvoiceId that is already globally unique on its own).
            modelBuilder.Entity<Receivable>()
                .HasIndex(e => new { e.SourceDocumentType, e.SourceDocumentId, e.CustomerId })
                .IsUnique();

            // Idempotency guard for the inbound customer-payment integration (brief §32) — the same
            // posted Financial must never be FIFO-applied twice.
            modelBuilder.Entity<PaymentApplication>()
                .HasIndex(e => e.SourceFinancialId)
                .IsUnique();
            modelBuilder.Entity<PaymentApplicationLine>()
                .HasOne(l => l.PaymentApplication).WithMany(p => p.Lines)
                .HasForeignKey(l => l.PaymentApplicationId).OnDelete(DeleteBehavior.Cascade);

            // Payables — mirrors the Receivable/PaymentApplication configuration above exactly.
            modelBuilder.Entity<Payable>()
                .HasIndex(e => new { e.SourceDocumentType, e.SourceDocumentId, e.SupplierId })
                .IsUnique();
            modelBuilder.Entity<SupplierPaymentApplication>()
                .HasIndex(e => e.SourceFinancialId)
                .IsUnique();
            modelBuilder.Entity<SupplierPaymentApplicationLine>()
                .HasOne(l => l.SupplierPaymentApplication).WithMany(p => p.Lines)
                .HasForeignKey(l => l.SupplierPaymentApplicationId).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ApprovalRequest>()
                .HasMany(r => r.Decisions).WithOne(d => d.ApprovalRequest)
                .HasForeignKey(d => d.ApprovalRequestId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<ApprovalRequest>()
                .Navigation(r => r.Decisions).UsePropertyAccessMode(PropertyAccessMode.Field);

            modelBuilder.Entity<Budget>()
                .HasMany(b => b.Lines).WithOne(l => l.Budget)
                .HasForeignKey(l => l.BudgetId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Budget>()
                .Navigation(b => b.Lines).UsePropertyAccessMode(PropertyAccessMode.Field);
            modelBuilder.Entity<Budget>()
                .HasOne(typeof(FiscalYear)).WithMany()
                .HasForeignKey("FiscalYearId");
            modelBuilder.Entity<Budget>()
                .HasOne(typeof(Department)).WithMany()
                .HasForeignKey("DepartmentId");
            modelBuilder.Entity<BudgetLine>()
                .HasOne(typeof(Account)).WithMany()
                .HasForeignKey("AccountId");

            modelBuilder.Entity<InvoiceTaxSnapshot>()
                .HasMany(s => s.Lines).WithOne(l => l.Snapshot)
                .HasForeignKey(l => l.SnapshotId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<InvoiceTaxSnapshot>()
                .Navigation(s => s.Lines).UsePropertyAccessMode(PropertyAccessMode.Field);
            modelBuilder.Entity<InvoiceTaxSnapshot>()
                .HasIndex(s => s.InvoiceId).IsUnique();
            modelBuilder.Entity<InvoiceTaxSnapshot>()
                .HasOne(typeof(Invoice)).WithMany()
                .HasForeignKey("InvoiceId");
            modelBuilder.Entity<InvoiceTaxSnapshot>()
                .HasOne(typeof(Currency)).WithMany()
                .HasForeignKey("CurrencyId");

            modelBuilder.Entity<FixedAsset>()
                .HasMany(a => a.Entries).WithOne(e => e.Asset)
                .HasForeignKey(e => e.AssetId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<FixedAsset>()
                .Navigation(a => a.Entries).UsePropertyAccessMode(PropertyAccessMode.Field);
            modelBuilder.Entity<FixedAsset>()
                .HasOne(typeof(FixedAssetCategory)).WithMany()
                .HasForeignKey("CategoryId");
            modelBuilder.Entity<FixedAsset>()
                .HasOne(typeof(Currency)).WithMany()
                .HasForeignKey("CurrencyId");
            modelBuilder.Entity<DepreciationEntry>()
                .HasIndex(e => new { e.AssetId, e.PeriodYear, e.PeriodMonth }).IsUnique();
            modelBuilder.Entity<FixedAssetCategory>()
                .HasOne(typeof(Account)).WithMany().HasForeignKey("AssetAccountId")
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<FixedAssetCategory>()
                .HasOne(typeof(Account)).WithMany().HasForeignKey("AccumulatedDepreciationAccountId")
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<FixedAssetCategory>()
                .HasOne(typeof(Account)).WithMany().HasForeignKey("DepreciationExpenseAccountId")
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<InboxMessage>()
                .HasIndex(e => new { e.EventId, e.HandlerName }).IsUnique();
            modelBuilder.Entity<Reporting.Infrastructure.Projections.CustomerAgingReadModel>()
                .HasIndex(e => e.InvoiceId).IsUnique();
            modelBuilder.Entity<Reporting.Infrastructure.Projections.SalesSummaryReadModel>()
                .HasIndex(e => new { e.SummaryDate, e.BranchId });

            modelBuilder.Entity<Custody>()
                .HasMany(c => c.Handovers).WithOne(h => h.Custody)
                .HasForeignKey(h => h.CustodyId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Custody>()
                .HasOne(typeof(Currency)).WithMany()
                .HasForeignKey("CurrencyId");

            modelBuilder.Entity<Quotation>()
                .HasMany(q => q.Lines).WithOne(l => l.Quotation)
                .HasForeignKey(l => l.QuotationId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Quotation>()
                .HasOne(typeof(Currency)).WithMany()
                .HasForeignKey("CurrencyId");
            modelBuilder.Entity<QuotationLine>()
                .HasOne(typeof(Product)).WithMany()
                .HasForeignKey("ProductId");
            modelBuilder.Entity<QuotationLine>()
                .HasOne(typeof(Catalog.Domain.Unit)).WithMany()
                .HasForeignKey("UnitId");

            modelBuilder.Entity<SalesOrder>()
                .HasMany(o => o.Lines).WithOne(l => l.SalesOrder)
                .HasForeignKey(l => l.SalesOrderId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<SalesOrder>()
                .HasOne(typeof(Currency)).WithMany()
                .HasForeignKey("CurrencyId");
            modelBuilder.Entity<SalesOrderLine>()
                .HasOne(typeof(Product)).WithMany()
                .HasForeignKey("ProductId");
            modelBuilder.Entity<SalesOrderLine>()
                .HasOne(typeof(Catalog.Domain.Unit)).WithMany()
                .HasForeignKey("UnitId");

            // GeneralLedger bounded-context isolation: Dealer/Stock/BankAccount/CashBox/
            // FinancialAccount.AccountId and Financial.JournalId dropped their navigations to
            // Accounting.Domain.Account/Journal (Parties.Domain/Inventory.Domain/Treasury.Domain
            // must not reference Accounting.Domain — see the GeneralLedger migration report).
            // These Fluent "no navigation" relationships keep the exact same FK columns and
            // constraints, verified with `dotnet ef migrations has-pending-model-changes`.
            modelBuilder.Entity<Dealer>()
                .HasOne(typeof(Account)).WithMany()
                .HasForeignKey("AccountId");
            modelBuilder.Entity<Stock>()
                .HasOne(typeof(Account)).WithMany()
                .HasForeignKey("AccountId");
            modelBuilder.Entity<BankAccount>()
                .HasOne(typeof(Account)).WithMany()
                .HasForeignKey("AccountId");
            modelBuilder.Entity<CashBox>()
                .HasOne(typeof(Account)).WithMany()
                .HasForeignKey("AccountId");
            modelBuilder.Entity<Treasury.Domain.FinancialAccount>()
                .HasOne(typeof(Account)).WithMany()
                .HasForeignKey("AccountId");
            modelBuilder.Entity<Financial>()
                .HasOne(typeof(Journal)).WithMany()
                .HasForeignKey("JournalId");

            // MovementModel dropped its CreateUser/ModifyUser/Shift/Branch navigation properties
            // when it moved to SharedKernel (see BuildingBlocks/OrgSys.SharedKernel/MovementModel.cs
            // for why) — these Fluent "no navigation" relationships keep the exact same FK columns
            // and constraints every MovementModel-derived entity had before, verified with
            // `dotnet ef migrations has-pending-model-changes`.
            foreach (var movementEntityType in new[]
                     {
                         typeof(Financial), typeof(FinancialTransfer), typeof(Invoice),
                         typeof(Journal), typeof(Transaction), typeof(global::Inventory.Domain.Inventory)
                     })
            {
                modelBuilder.Entity(movementEntityType)
                    .HasOne(typeof(User)).WithMany()
                    .HasForeignKey("CreateUserId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
                modelBuilder.Entity(movementEntityType)
                    .HasOne(typeof(User)).WithMany()
                    .HasForeignKey("ModifyUserId");
                modelBuilder.Entity(movementEntityType)
                    .HasOne(typeof(Shift)).WithMany()
                    .HasForeignKey("ShiftId");
                modelBuilder.Entity(movementEntityType)
                    .HasOne(typeof(Branch)).WithMany()
                    .HasForeignKey("BranchId");
            }

            // Invoice/InvoiceProduct dropped their Stock/Product navigation properties for the
            // same Sales/Inventory module-boundary reason (Invoice.Transaction too, confirmed
            // dead) — these Fluent "no navigation" relationships keep the exact same FK columns
            // and constraints, verified with `dotnet ef migrations has-pending-model-changes`.
            // See docs/modular-monolith-analysis.md §21.
            modelBuilder.Entity<Invoice>()
                .HasOne(typeof(Stock)).WithMany()
                .HasForeignKey("StockId");
            modelBuilder.Entity<Invoice>()
                .HasOne(typeof(Transaction)).WithMany()
                .HasForeignKey("TransactionId");
            modelBuilder.Entity<InvoiceProduct>()
                .HasOne(typeof(Product)).WithMany()
                .HasForeignKey("ProductId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
            modelBuilder.Entity<InvoiceProduct>()
                .HasOne(typeof(Stock)).WithMany()
                .HasForeignKey("StockId");
            modelBuilder.Entity<InvoiceProduct>()
                .HasOne(typeof(Unit)).WithMany()
                .HasForeignKey("UnitId")
                .IsRequired();
            modelBuilder.Entity<Invoice>()
                .HasOne(typeof(PaymentType)).WithMany()
                .HasForeignKey("PaymentTypeId")
                .IsRequired();
            modelBuilder.Entity<Invoice>()
                .HasOne(typeof(Currency)).WithMany()
                .HasForeignKey("CurrencyId")
                .IsRequired();
            modelBuilder.Entity<Bank>()
                .HasOne(typeof(Country)).WithMany()
                .HasForeignKey("CountryId");
            modelBuilder.Entity<BankBranch>()
                .HasOne(typeof(Country)).WithMany()
                .HasForeignKey("CountryId")
                .IsRequired();
            modelBuilder.Entity<BankBranch>()
                .HasOne(typeof(City)).WithMany()
                .HasForeignKey("CityId")
                .IsRequired();
            modelBuilder.Entity<BankBranch>()
                .HasOne(typeof(District)).WithMany()
                .HasForeignKey("DistrictId")
                .IsRequired();
            modelBuilder.Entity<Treasury.Domain.FinancialAccount>()
                .HasOne(typeof(Currency)).WithMany()
                .HasForeignKey("CurrencyId");
            modelBuilder.Entity<Financial>()
                .HasOne(typeof(PaymentType)).WithMany()
                .HasForeignKey("PaymentTypeId")
                .IsRequired();
            modelBuilder.Entity<Financial>()
                .HasOne(typeof(Currency)).WithMany()
                .HasForeignKey("CurrencyId")
                .IsRequired();
            modelBuilder.Entity<FinancialTransfer>()
                .HasOne(typeof(Currency)).WithMany()
                .HasForeignKey("CurrencyId")
                .IsRequired();
            modelBuilder.Entity<Dealer>()
                .HasOne(typeof(Country)).WithMany()
                .HasForeignKey("CountryId");
            modelBuilder.Entity<Dealer>()
                .HasOne(typeof(City)).WithMany()
                .HasForeignKey("CityId");
            modelBuilder.Entity<Dealer>()
                .HasOne(typeof(District)).WithMany()
                .HasForeignKey("DistrictId");
            modelBuilder.Entity<PartyAddress>()
                .HasOne(typeof(Country)).WithMany()
                .HasForeignKey("CountryId");
            modelBuilder.Entity<PartyAddress>()
                .HasOne(typeof(City)).WithMany()
                .HasForeignKey("CityId");
            modelBuilder.Entity<PartyAddress>()
                .HasOne(typeof(District)).WithMany()
                .HasForeignKey("DistrictId");
            modelBuilder.Entity<Purchasing.Domain.PurchaseRequisitionProduct>()
                .HasOne(typeof(Unit)).WithMany()
                .HasForeignKey("UnitId")
                .IsRequired();
            modelBuilder.Entity<Purchasing.Domain.PurchaseOrderProduct>()
                .HasOne(typeof(Unit)).WithMany()
                .HasForeignKey("UnitId")
                .IsRequired();
            modelBuilder.Entity<TransactionProduct>()
                .HasOne(typeof(Product)).WithMany()
                .HasForeignKey("ProductId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
            modelBuilder.Entity<TransactionProduct>()
                .HasOne(typeof(Unit)).WithMany()
                .HasForeignKey("UnitId")
                .IsRequired();
            modelBuilder.Entity<InventoryProduct>()
                .HasOne(typeof(Product)).WithMany()
                .HasForeignKey("ProductId")
                .IsRequired();
            modelBuilder.Entity<InventoryProduct>()
                .HasOne(typeof(Unit)).WithMany()
                .HasForeignKey("UnitId")
                .IsRequired();
            modelBuilder.Entity<InventoryBalance>()
                .HasOne(typeof(Product)).WithMany()
                .HasForeignKey("ProductId")
                .IsRequired();
            modelBuilder.Entity<InventoryReceiptLine>()
                .HasOne(typeof(Product)).WithMany()
                .HasForeignKey("ProductId")
                .IsRequired();
            modelBuilder.Entity<InventoryReceiptLine>()
                .HasOne(typeof(Unit)).WithMany()
                .HasForeignKey("UnitId")
                .IsRequired();
            modelBuilder.Entity<InventoryIssueLine>()
                .HasOne(typeof(Product)).WithMany()
                .HasForeignKey("ProductId")
                .IsRequired();
            modelBuilder.Entity<InventoryIssueLine>()
                .HasOne(typeof(Unit)).WithMany()
                .HasForeignKey("UnitId")
                .IsRequired();
            modelBuilder.Entity<StockTransferLine>()
                .HasOne(typeof(Product)).WithMany()
                .HasForeignKey("ProductId")
                .IsRequired();
            modelBuilder.Entity<StockTransferLine>()
                .HasOne(typeof(Unit)).WithMany()
                .HasForeignKey("UnitId")
                .IsRequired();
            modelBuilder.Entity<StockAdjustmentLine>()
                .HasOne(typeof(Product)).WithMany()
                .HasForeignKey("ProductId")
                .IsRequired();
            modelBuilder.Entity<StockAdjustmentLine>()
                .HasOne(typeof(Unit)).WithMany()
                .HasForeignKey("UnitId")
                .IsRequired();
            modelBuilder.Entity<StockReservation>()
                .HasOne(typeof(Product)).WithMany()
                .HasForeignKey("ProductId")
                .IsRequired();
            modelBuilder.Entity<InventoryBatch>()
                .HasOne(typeof(Product)).WithMany()
                .HasForeignKey("ProductId")
                .IsRequired();
            modelBuilder.Entity<InventorySerial>()
                .HasOne(typeof(Product)).WithMany()
                .HasForeignKey("ProductId")
                .IsRequired();
            modelBuilder.Entity<InventoryCostLayer>()
                .HasOne(typeof(Product)).WithMany()
                .HasForeignKey("ProductId")
                .IsRequired();
            modelBuilder.Entity<global::Inventory.Domain.Inventory>()
                .HasOne(typeof(User)).WithMany()
                .HasForeignKey("UserId");

            // Cross-module Dealer/Branch/Invoice navigations dropped (Domain must not reference
            // another module's Domain). Fluent "no navigation" FKs keep the same columns/constraints.
            modelBuilder.Entity<User>()
                .HasOne(typeof(Branch)).WithMany()
                .HasForeignKey("BranchId");
            modelBuilder.Entity<Product>()
                .HasOne(typeof(Dealer)).WithMany()
                .HasForeignKey("DealerId");
            modelBuilder.Entity<Transaction>()
                .HasOne(typeof(Dealer)).WithMany()
                .HasForeignKey("DealerId");
            modelBuilder.Entity<InventoryReceipt>()
                .HasOne(typeof(Dealer)).WithMany()
                .HasForeignKey("DealerId");
            modelBuilder.Entity<InventoryIssue>()
                .HasOne(typeof(Dealer)).WithMany()
                .HasForeignKey("DealerId");
            modelBuilder.Entity<Invoice>()
                .HasOne(typeof(Dealer)).WithMany()
                .HasForeignKey("DealerId")
                .IsRequired();
            modelBuilder.Entity<Financial>()
                .HasOne(typeof(Dealer)).WithMany()
                .HasForeignKey("DealerId");
            modelBuilder.Entity<FinancialInvoice>()
                .HasOne(typeof(Invoice)).WithMany()
                .HasForeignKey("InvoiceId");
            modelBuilder.Entity<Purchasing.Domain.PurchaseOrder>()
                .HasOne(typeof(Dealer)).WithMany()
                .HasForeignKey("DealerId")
                .IsRequired();
            modelBuilder.Entity<Stock>()
                .HasOne(typeof(Branch)).WithMany()
                .HasForeignKey("BranchId")
                .IsRequired();

            ConfigureInventoryHardening(modelBuilder);
            ConfigureCatalog(modelBuilder);
            ConfigureOrganization(modelBuilder);
            ConfigureParties(modelBuilder);
            ConfigureSaaS(modelBuilder);
        }

        /// <summary>
        /// EF configuration for the new SaaS module (docs/architecture/adr/tenant-vs-company.md).
        /// Tenant/Plan/Feature/PlanFeature/Subscription all live in SaaS.Domain; Plan/Feature/
        /// Subscription use real intra-module navigations (same module), matching this codebase's
        /// "no navigation only across module boundaries" convention. Company.TenantId (added below,
        /// in ConfigureOrganization) is the retrofit's stage-1 nullable scalar-only FK — no
        /// Organization.Domain -> SaaS.Domain navigation, same pattern as Company.CountryId into
        /// MasterData.
        /// </summary>
        private static void ConfigureSaaS(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SaaS.Domain.Tenant>()
                .HasIndex(e => e.Name)
                .IsUnique();

            modelBuilder.Entity<SaaS.Domain.PlanFeature>()
                .HasOne(e => e.Plan).WithMany(e => e.PlanFeatures)
                .HasForeignKey(e => e.PlanId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
            modelBuilder.Entity<SaaS.Domain.PlanFeature>()
                .HasOne(e => e.Feature).WithMany()
                .HasForeignKey(e => e.FeatureId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
            modelBuilder.Entity<SaaS.Domain.PlanFeature>()
                .HasIndex(e => new { e.PlanId, e.FeatureId })
                .IsUnique();

            modelBuilder.Entity<SaaS.Domain.Feature>()
                .HasIndex(e => e.Key)
                .IsUnique();

            // A Subscription must never survive its Tenant/Plan being deleted out from under it —
            // Restrict, not Cascade: Tenant/Plan deletion must be blocked while billing history
            // exists, same "don't cascade-delete master data with history" rule Branch->Company uses.
            modelBuilder.Entity<SaaS.Domain.Subscription>()
                .HasOne(e => e.Tenant).WithMany()
                .HasForeignKey(e => e.TenantId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
            modelBuilder.Entity<SaaS.Domain.Subscription>()
                .HasOne(e => e.Plan).WithMany()
                .HasForeignKey(e => e.PlanId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            // Tenant retrofit stage 1 (ADR): nullable scalar-only FK into SaaS.Domain.Tenant, no
            // Organization.Domain -> SaaS.Domain navigation/reference at all — wired centrally here,
            // same "no navigation" convention as Company.CountryId/DefaultCurrencyId into MasterData.
            modelBuilder.Entity<Company>()
                .HasOne(typeof(SaaS.Domain.Tenant)).WithMany()
                .HasForeignKey("TenantId");
        }

        /// <summary>
        /// EF configuration for the Parties module's new CustomerProfile/SupplierProfile/
        /// PartyContact/PartyAddress additions (docs/parties/party-target-architecture.md). Dealer
        /// itself keeps its existing shape/table — these are additive child tables, not a
        /// relocation. AccountId on both profiles mirrors Dealer.AccountId's own scalar-only "no
        /// navigation" FK into Accounting.Domain.Account (GeneralLedger bounded-context isolation).
        /// </summary>
        private static void ConfigureParties(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Dealer>()
                .HasMany(e => e.Contacts).WithOne(e => e.Dealer)
                .HasForeignKey(e => e.DealerId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Dealer>()
                .HasMany(e => e.Addresses).WithOne(e => e.Dealer)
                .HasForeignKey(e => e.DealerId).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Parties.Domain.CustomerProfile>()
                .HasOne(e => e.Dealer).WithOne(e => e.CustomerProfile)
                .HasForeignKey<Parties.Domain.CustomerProfile>(e => e.DealerId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Parties.Domain.CustomerProfile>()
                .HasIndex(e => e.DealerId).IsUnique();
            modelBuilder.Entity<Parties.Domain.CustomerProfile>()
                .HasOne(typeof(Account)).WithMany()
                .HasForeignKey("AccountId");

            modelBuilder.Entity<Parties.Domain.SupplierProfile>()
                .HasOne(e => e.Dealer).WithOne(e => e.SupplierProfile)
                .HasForeignKey<Parties.Domain.SupplierProfile>(e => e.DealerId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Parties.Domain.SupplierProfile>()
                .HasIndex(e => e.DealerId).IsUnique();
            modelBuilder.Entity<Parties.Domain.SupplierProfile>()
                .HasOne(typeof(Account)).WithMany()
                .HasForeignKey("AccountId");
        }

        /// <summary>
        /// EF configuration for the Organization module's new Company/OrganizationSettings aggregates
        /// and the new Branch.CompanyId FK (docs/organization/organization-target-architecture.md).
        /// Company/Branch stay in Organization.Domain; Currency/Country/City/District ownership was
        /// deliberately left in MasterData this pass, so Company/OrganizationSettings reference them
        /// as scalar-only "no navigation" FKs, same pattern as Journal.CurrencyId above.
        /// </summary>
        private static void ConfigureOrganization(ModelBuilder modelBuilder)
        {
            // A Branch must belong to a Company (brief §1.4) — Restrict, not Cascade: deleting a
            // Company must never silently delete every Branch (and everything a Branch is
            // transitively referenced by) underneath it.
            modelBuilder.Entity<Branch>()
                .HasOne(e => e.Company).WithMany(e => e.Branches)
                .HasForeignKey(e => e.CompanyId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            modelBuilder.Entity<Department>()
                .HasOne(e => e.Company).WithMany()
                .HasForeignKey(e => e.CompanyId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            modelBuilder.Entity<Company>()
                .HasOne(typeof(Currency)).WithMany()
                .HasForeignKey("DefaultCurrencyId");
            modelBuilder.Entity<Company>()
                .HasOne(typeof(Country)).WithMany()
                .HasForeignKey("CountryId");

            // One settings row per Company.
            modelBuilder.Entity<Organization.Domain.OrganizationSettings>()
                .HasOne(e => e.Company).WithOne()
                .HasForeignKey<Organization.Domain.OrganizationSettings>(e => e.CompanyId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
            modelBuilder.Entity<Organization.Domain.OrganizationSettings>()
                .HasIndex(e => e.CompanyId)
                .IsUnique();
            modelBuilder.Entity<Organization.Domain.OrganizationSettings>()
                .HasOne(typeof(Currency)).WithMany()
                .HasForeignKey("DefaultCurrencyId");
            modelBuilder.Entity<Organization.Domain.OrganizationSettings>()
                .HasOne(typeof(Country)).WithMany()
                .HasForeignKey("DefaultCountryId");
        }

        /// <summary>
        /// EF configuration for the Catalog module (docs/catalog/catalog-target-architecture.md).
        /// Product/ProductUnit/Classification/Unit/Property/PropertyElement/ProductPropertyElement
        /// were relocated here unchanged from Inventory.Domain/MasterData.Domain (same table names,
        /// same columns, same [Table]/[ForeignKey] attributes on the entities themselves) — only
        /// Brand/PriceList/PriceListEntry and Product.BrandId are genuinely new configuration.
        /// </summary>
        private static void ConfigureCatalog(ModelBuilder modelBuilder)
        {
            // Brand deletion must never cascade-delete every Product that references it (brief's
            // own "avoid cascade deletes on master data" guidance) — deactivate the Brand instead.
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Brand).WithMany()
                .HasForeignKey(p => p.BrandId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Brand>().HasIndex(e => e.Code).IsUnique();

            modelBuilder.Entity<PriceList>()
                .HasMany(pl => pl.Entries).WithOne(e => e.PriceList)
                .HasForeignKey(e => e.PriceListId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<PriceListEntry>()
                .HasOne(e => e.Product).WithMany()
                .HasForeignKey(e => e.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<PriceListEntry>()
                .HasOne(e => e.Unit).WithMany()
                .HasForeignKey(e => e.UnitId)
                .OnDelete(DeleteBehavior.Restrict);

            // ProductRecipe.ProductId lost its implicit relationship-by-convention once
            // Product.ProductRecipes was dropped during the Catalog relocation (ProductRecipe
            // itself stays Inventory-owned — docs/catalog/catalog-ownership.md). Restored as an
            // explicit Fluent "no navigation" FK, same shape as InvoiceProduct->Product below, so
            // the DB-level constraint is preserved unchanged rather than silently dropped.
            modelBuilder.Entity<Inventory.Domain.ProductRecipe>()
                .HasOne(typeof(Product)).WithMany()
                .HasForeignKey("ProductId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        }

        /// <summary>
        /// EF configuration for the new Inventory aggregates (docs/ddd/inventory-target-architecture.md
        /// §6) — mirrors the Payables/Receivables "child collection via private backing field" pattern
        /// used a few lines above for SupplierPaymentApplication.Lines, plus the invariants the brief
        /// calls out explicitly (§58 unique constraints, §61 concurrency tokens).
        /// </summary>
        private static void ConfigureInventoryHardening(ModelBuilder modelBuilder)
        {
            // One warehouse code per company (brief §4.1). Nullable Code (inherited from BaseModel)
            // is fine — SQL Server unique indexes allow multiple NULLs.
            modelBuilder.Entity<Stock>().HasIndex(e => e.Code).IsUnique();
            // Property initializers (`= true`) are a C#-only default — EF does NOT infer a SQL
            // DEFAULT constraint from them, so without this explicit HasDefaultValue the AddColumn
            // migration would default every EXISTING Stock row's new IsActive column to false
            // (CLR default(bool)), deactivating every warehouse that already exists. Confirmed by
            // generating the migration once and inspecting it before wiring this in.
            modelBuilder.Entity<Stock>().Property(e => e.IsActive).HasDefaultValue(true);
            modelBuilder.Entity<Product>().Property(e => e.IsActive).HasDefaultValue(true);

            modelBuilder.Entity<WarehouseLocation>()
                .HasOne(l => l.ParentLocation).WithMany()
                .HasForeignKey(l => l.ParentLocationId)
                // Self-referencing FK: cascade delete would create a cycle SQL Server rejects.
                .OnDelete(DeleteBehavior.Restrict);

            // The optimized current-state projection (brief §8) — one row per Item+Warehouse
            // (+Location)(+Batch); RowVersion (see InventoryBalance.cs's [Timestamp]) is EF's
            // concurrency token, picked up automatically from the Data Annotation.
            //
            // SQL Server treats every NULL in a unique index as distinct from every other NULL, so a
            // single filtered `.IsUnique()` on all four columns (EF's own default filter is
            // "LocationId IS NOT NULL AND BatchId IS NOT NULL") only guards the one case where both
            // are set — the common Location=NULL/Batch=NULL case would silently allow duplicate
            // balance rows for the same Item+Warehouse, which is exactly the "competing stock truth"
            // brief §70 forbids. Four filtered indexes, one per null-pattern, close all of them.
            modelBuilder.Entity<InventoryBalance>()
                .HasIndex(e => new { e.ProductId, e.StockId, e.LocationId, e.BatchId })
                .IsUnique()
                .HasFilter("[LocationId] IS NOT NULL AND [BatchId] IS NOT NULL")
                .HasDatabaseName("IX_InventoryBalance_Product_Stock_Location_Batch");
            modelBuilder.Entity<InventoryBalance>()
                .HasIndex(e => new { e.ProductId, e.StockId, e.LocationId })
                .IsUnique()
                .HasFilter("[LocationId] IS NOT NULL AND [BatchId] IS NULL")
                .HasDatabaseName("IX_InventoryBalance_Product_Stock_Location_NoBatch");
            modelBuilder.Entity<InventoryBalance>()
                .HasIndex(e => new { e.ProductId, e.StockId, e.BatchId })
                .IsUnique()
                .HasFilter("[LocationId] IS NULL AND [BatchId] IS NOT NULL")
                .HasDatabaseName("IX_InventoryBalance_Product_Stock_Batch_NoLocation");
            modelBuilder.Entity<InventoryBalance>()
                .HasIndex(e => new { e.ProductId, e.StockId })
                .IsUnique()
                .HasFilter("[LocationId] IS NULL AND [BatchId] IS NULL")
                .HasDatabaseName("IX_InventoryBalance_Product_Stock_NoLocation_NoBatch");

            modelBuilder.Entity<InventoryReceipt>()
                .HasMany(r => r.Lines).WithOne(l => l.InventoryReceipt)
                .HasForeignKey(l => l.InventoryReceiptId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<InventoryReceipt>()
                .HasOne(typeof(PurchaseOrder)).WithMany()
                .HasForeignKey("PurchaseOrderId")
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

            modelBuilder.Entity<InventoryIssue>()
                .HasMany(i => i.Lines).WithOne(l => l.InventoryIssue)
                .HasForeignKey(l => l.InventoryIssueId).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<StockTransfer>()
                .HasMany(t => t.Lines).WithOne(l => l.StockTransfer)
                .HasForeignKey(l => l.StockTransferId).OnDelete(DeleteBehavior.Cascade);
            // A transfer references two warehouses (From/To) — EF can't infer which FK belongs to
            // which navigation without both being told explicitly; Restrict avoids SQL Server's
            // "multiple cascade paths" error from two FKs into the same Stock table.
            modelBuilder.Entity<StockTransfer>()
                .HasOne(t => t.FromStock).WithMany()
                .HasForeignKey(t => t.FromStockId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<StockTransfer>()
                .HasOne(t => t.ToStock).WithMany()
                .HasForeignKey(t => t.ToStockId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<StockAdjustment>()
                .HasMany(a => a.Lines).WithOne(l => l.StockAdjustment)
                .HasForeignKey(l => l.StockAdjustmentId).OnDelete(DeleteBehavior.Cascade);

            // Idempotency guard (brief §31) — the same SourceDocumentType+SourceDocumentId+
            // SourceDocumentLineId (or an explicit IdempotencyKey) must never produce two movements.
            // Nullable, so historical/unrelated Transaction rows are unaffected.
            modelBuilder.Entity<Transaction>().HasIndex(e => e.IdempotencyKey).IsUnique();

            // Batch/serial identity (brief §16/§17).
            modelBuilder.Entity<InventoryBatch>().HasIndex(e => new { e.ProductId, e.BatchNumber }).IsUnique();
            modelBuilder.Entity<InventorySerial>().HasIndex(e => new { e.ProductId, e.SerialNumber }).IsUnique();
        }

        public Task BeginTransactionAsync()
        {
           return this.Database.BeginTransactionAsync();
        }

        public Task CommitAsync()
        {
           return this.Database.CommitTransactionAsync();
        }

        public Task RollbackAsync()
        {
            return this.Database.RollbackTransactionAsync();
        }

        public virtual DbSet<Unit> Units { get; set; }
        public virtual DbSet<Property> Properties { get; set; }
        public virtual DbSet<DealerGroup> DealerGroups { get; set; }
        public virtual DbSet<Dealer> Dealers { get; set; }
        public virtual DbSet<Parties.Domain.CustomerProfile> CustomerProfiles { get; set; }
        public virtual DbSet<Parties.Domain.SupplierProfile> SupplierProfiles { get; set; }
        public virtual DbSet<Parties.Domain.PartyContact> PartyContacts { get; set; }
        public virtual DbSet<Parties.Domain.PartyAddress> PartyAddresses { get; set; }
        public virtual DbSet<Classification> Classifications { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductUnit> ProductUnits { get; set; }
        public virtual DbSet<Brand> Brands { get; set; }
        public virtual DbSet<PriceList> PriceLists { get; set; }
        public virtual DbSet<PriceListEntry> PriceListEntries { get; set; }
        public virtual DbSet<Branch> Branches { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Budget> Budgets { get; set; }
        public virtual DbSet<BudgetLine> BudgetLines { get; set; }
        public virtual DbSet<InvoiceTaxSnapshot> InvoiceTaxSnapshots { get; set; }
        public virtual DbSet<InvoiceTaxSnapshotLine> InvoiceTaxSnapshotLines { get; set; }
        public virtual DbSet<FixedAssetCategory> FixedAssetCategories { get; set; }
        public virtual DbSet<FixedAsset> FixedAssets { get; set; }
        public virtual DbSet<DepreciationEntry> DepreciationEntries { get; set; }
        public virtual DbSet<OutboxMessage> OutboxMessages { get; set; }
        public virtual DbSet<InboxMessage> InboxMessages { get; set; }
        public virtual DbSet<Reporting.Infrastructure.Projections.CustomerAgingReadModel> CustomerAgingReadModels { get; set; }
        public virtual DbSet<Reporting.Infrastructure.Projections.SalesSummaryReadModel> SalesSummaryReadModels { get; set; }
        public virtual DbSet<Organization.Domain.Company> Companies { get; set; }
        public virtual DbSet<Organization.Domain.OrganizationSettings> OrganizationSettings { get; set; }
        public virtual DbSet<SaaS.Domain.Tenant> Tenants { get; set; }
        public virtual DbSet<SaaS.Domain.Plan> Plans { get; set; }
        public virtual DbSet<SaaS.Domain.Feature> Features { get; set; }
        public virtual DbSet<SaaS.Domain.PlanFeature> PlanFeatures { get; set; }
        public virtual DbSet<SaaS.Domain.Subscription> Subscriptions { get; set; }
        public virtual DbSet<Stock> Stocks { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Shift> Shifts { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Permission> Permissions { get; set; }
        public virtual DbSet<Invoice> Invoices { get; set; }
        public virtual DbSet<InvoiceProduct> InvoiceProducts { get; set; }
        public virtual DbSet<InvoiceType> InvoiceTypes { get; set; }
        public virtual DbSet<Purchasing.Domain.PurchaseRequisition> PurchaseRequisitions { get; set; }
        public virtual DbSet<Purchasing.Domain.PurchaseRequisitionProduct> PurchaseRequisitionProducts { get; set; }
        public virtual DbSet<Purchasing.Domain.PurchaseOrder> PurchaseOrders { get; set; }
        public virtual DbSet<Purchasing.Domain.PurchaseOrderProduct> PurchaseOrderProducts { get; set; }
        public virtual DbSet<PaymentType> PaymentTypes { get; set; }

        public virtual DbSet<ReferenceType> ReferenceTypes { get; set; }
        //public virtual DbSet<LogSys> LogSys { get; set; }
        // Explicit DbSet needed now that Product moved to Catalog.Domain and dropped its
        // ProductRecipes navigation (ProductRecipe stays Inventory-owned — see
        // docs/catalog/catalog-ownership.md); without this, EF no longer discovers the type via
        // any navigation and would drop the table. ProductId is a scalar-only reference into
        // Catalog, same convention as every other Inventory entity.
        public virtual DbSet<Inventory.Domain.ProductRecipe> ProductRecipes { get; set; }
        //public virtual DbSet<PropertyElement> PropertyElements { get; set; }
        //public virtual DbSet<ProductPropertyElement> ProductPropertyElements { get; set; }
        public virtual DbSet<Preference> Preferences { get; set; }
        public virtual DbSet<TransactionType> TransactionTypes { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }
        public virtual DbSet<TransactionProduct> TransactionProducts { get; set; }
        public virtual DbSet<global::Inventory.Domain.Inventory> Inventories { get; set; }
        public virtual DbSet<InventoryProduct> InventoryProducts { get; set; }
        public virtual DbSet<Table> Tables { get; set; }
        public virtual DbSet<CashBox> CashBoxes { get; set; }
        public virtual DbSet<Financial> Financials { get; set; }
        public virtual DbSet<FinancialAccount> FinancialAccounts { get; set; }
        public virtual DbSet<FinancialTransfer> FinancialTransfers { get; set; }
        public virtual DbSet<FinancialInvoice> FinancialInvoices { get; set; }
        public virtual DbSet<FinancialType> FinancialTypes { get; set; }
        public virtual DbSet<Outlay> Outlays { get; set; }
        public virtual DbSet<Currency> Currencys { get; set; }
        public virtual DbSet<RolePermission> RolePermissions { get; set; }
        //public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<BankAccount> BankAccounts { get; set; }
        public virtual DbSet<AccountType> AccountTypes { get; set; }
        public virtual DbSet<Bank> Banks { get; set; }
        public virtual DbSet<BankBranch> BankBranchs { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<District> Districts { get; set; }
        public virtual DbSet<JournalType> JournalTypes { get; set; }
        public virtual DbSet<Journal> Journals { get; set; }
        public virtual DbSet<JournalItem> JournalItem { get; set; }
        public virtual DbSet<FiscalYear> FiscalYears { get; set; }
        public virtual DbSet<FiscalPeriod> FiscalPeriods { get; set; }
        public virtual DbSet<Receivable> Receivables { get; set; }
        public virtual DbSet<PaymentApplication> PaymentApplications { get; set; }
        public virtual DbSet<PaymentApplicationLine> PaymentApplicationLines { get; set; }
        public virtual DbSet<Payable> Payables { get; set; }
        public virtual DbSet<SupplierPaymentApplication> SupplierPaymentApplications { get; set; }
        public virtual DbSet<SupplierPaymentApplicationLine> SupplierPaymentApplicationLines { get; set; }
        public virtual DbSet<Custody> Custodies { get; set; }
        public virtual DbSet<CustodyHandover> CustodyHandovers { get; set; }
        public virtual DbSet<ApprovalRequest> ApprovalRequests { get; set; }
        public virtual DbSet<ApprovalDecision> ApprovalDecisions { get; set; }
        public virtual DbSet<Quotation> SalesQuotations { get; set; }
        public virtual DbSet<QuotationLine> SalesQuotationLines { get; set; }
        public virtual DbSet<SalesOrder> SalesOrders { get; set; }
        public virtual DbSet<SalesOrderLine> SalesOrderLines { get; set; }

        // Inventory bounded-context hardening (docs/ddd/inventory-target-architecture.md) — additive
        // tables alongside the existing Product/Stock/Transaction/Inventory ones, never replacing them.
        public virtual DbSet<WarehouseLocation> WarehouseLocations { get; set; }
        public virtual DbSet<InventoryBalance> InventoryBalances { get; set; }
        public virtual DbSet<InventoryReceipt> InventoryReceipts { get; set; }
        public virtual DbSet<InventoryReceiptLine> InventoryReceiptLines { get; set; }
        public virtual DbSet<InventoryIssue> InventoryIssues { get; set; }
        public virtual DbSet<InventoryIssueLine> InventoryIssueLines { get; set; }
        public virtual DbSet<StockTransfer> StockTransfers { get; set; }
        public virtual DbSet<StockTransferLine> StockTransferLines { get; set; }
        public virtual DbSet<StockAdjustment> StockAdjustments { get; set; }
        public virtual DbSet<StockAdjustmentLine> StockAdjustmentLines { get; set; }
        public virtual DbSet<StockAdjustmentReason> StockAdjustmentReasons { get; set; }
        public virtual DbSet<StockReservation> StockReservations { get; set; }
        public virtual DbSet<InventoryBatch> InventoryBatches { get; set; }
        public virtual DbSet<InventorySerial> InventorySerials { get; set; }
        public virtual DbSet<InventoryCostLayer> InventoryCostLayers { get; set; }


        public void ResetDbContextState()
        {
            foreach (var entry in this.ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;

                    case EntityState.Modified:
                        entry.CurrentValues.SetValues(entry.OriginalValues);
                        entry.State = EntityState.Unchanged;
                        break;

                    case EntityState.Deleted:
                        entry.State = EntityState.Unchanged;
                        break;
                }
            }
        }
        //public virtual DbSet<Country> Countries { get; set; }
        //public virtual DbSet<Journal> Journals { get; set; }
        //public virtual DbSet<JournalItem> JournalItems { get; set; }
        //public virtual DbSet<CompanyProfile> CompanyProfiles { get; set; }

        //[NotMapped]
        //public virtual DbSet<DealerList> DealerListReport { get; set; }
        //[NotMapped]
        //public virtual DbSet<DealerStatment> DealerStatmentReport { get; set; }

        //[NotMapped]
        //public virtual DbSet<DealerBalance> DealerBalanceReport { get; set; }
        //[NotMapped]
        //public virtual DbSet<SalesBalance> SalesBalanceReport { get; set; }

        //[NotMapped]
        //public virtual DbSet<SalesClient> SalesClientReport { get; set; }

        //[NotMapped]
        //public virtual DbSet<StockStatment> StockStatmentReport { get; set; }

        //[NotMapped]
        //public virtual DbSet<ProductStatment> ProductStatmentReport { get; set; }

        //[NotMapped]
        //public virtual DbSet<SafeStatment> SafeStatmentReport { get; set; }

        //[NotMapped]
        //public virtual DbSet<SafeBalance> SafeBalanceReport { get; set; }

        //[NotMapped]
        //public virtual DbSet<StockBalance> StockBalanceReport { get; set; }

        //[NotMapped]
        //public virtual DbSet<ProductBalance> ProductBalanceReport { get; set; }

        //[NotMapped]
        //public virtual DbSet<ProductList> ProductListReport { get; set; }

        //[NotMapped]
        //public virtual DbSet<StockList> StockListReport { get; set; }

        //[NotMapped]
        //public virtual DbSet<SafeList> SafeListReport { get; set; }
    }    
}
