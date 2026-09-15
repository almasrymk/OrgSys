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

    public class OrgContext : DbContext , IOrgContext
    {
        //public string Schema { get; set; } = "org";

        public OrgContext(DbContextOptions<OrgContext> options) : base(options)
        {

        }
         
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: true, reloadOnChange: false);
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
                new TreasuryDataSeeder());

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
            modelBuilder.Entity<global::Inventory.Domain.Inventory>()
                .HasOne(typeof(User)).WithMany()
                .HasForeignKey("UserId");

            ConfigureInventoryHardening(modelBuilder);
            ConfigureCatalog(modelBuilder);
            ConfigureOrganization(modelBuilder);
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
        public virtual DbSet<Classification> Classifications { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductUnit> ProductUnits { get; set; }
        public virtual DbSet<Brand> Brands { get; set; }
        public virtual DbSet<PriceList> PriceLists { get; set; }
        public virtual DbSet<PriceListEntry> PriceListEntries { get; set; }
        public virtual DbSet<Branch> Branches { get; set; }
        public virtual DbSet<Organization.Domain.Company> Companies { get; set; }
        public virtual DbSet<Organization.Domain.OrganizationSettings> OrganizationSettings { get; set; }
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
