namespace Infrastructure.Persistence.Data
{
    using Domain.Entities;
    using System.Reflection;
    using Domain.Abstraction;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.EntityFrameworkCore.Migrations;
    using Microsoft.EntityFrameworkCore.Diagnostics;
    using Microsoft.EntityFrameworkCore.Infrastructure;
    using Microsoft.EntityFrameworkCore.Migrations.Internal;
    using Infrastructure.Seed;

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
                    var orgContext = (OrgContext)context;
                    new InitialData().Seed(orgContext);
                })
                .UseAsyncSeeding((context, _, _) =>
                {
                    var orgContext = (OrgContext)context;
                    new InitialData().Seed(orgContext);
                    return Task.CompletedTask;
                });
        }
         
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Journal>()
                .Property(journal => journal.Rate)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Safe>()
                .HasOne(e => e.FinancialAccount).WithOne(e => e.CashBox)
                .HasForeignKey<Safe>(e => e.FinancialAccountId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<AccountBank>()
                .HasOne(e => e.FinancialAccount).WithOne(e => e.BankAccount)
                .HasForeignKey<AccountBank>(e => e.FinancialAccountId).OnDelete(DeleteBehavior.Restrict);
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
        public virtual DbSet<Branch> Branches { get; set; }
        public virtual DbSet<Stock> Stocks { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Shift> Shifts { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Permission> Permissions { get; set; }
        public virtual DbSet<Invoice> Invoices { get; set; }
        public virtual DbSet<InvoiceProduct> InvoiceProducts { get; set; }
        public virtual DbSet<InvoiceType> InvoiceTypes { get; set; }
        //public virtual DbSet<Order> Orders { get; set; }
        //public virtual DbSet<OrderProduct> OrderProducts { get; set; }
        public virtual DbSet<PaymentType> PaymentTypes { get; set; }
        //public virtual DbSet<LogSys> LogSys { get; set; }
        //public virtual DbSet<ProductRecipe> ProductRecipes { get; set; }
        //public virtual DbSet<PropertyElement> PropertyElements { get; set; }
        //public virtual DbSet<ProductPropertyElement> ProductPropertyElements { get; set; }
        public virtual DbSet<Preference> Preferences { get; set; }
        public virtual DbSet<TransactionType> TransactionTypes { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }
        public virtual DbSet<TransactionProduct> TransactionProducts { get; set; }
        public virtual DbSet<Inventory> Inventories { get; set; }
        public virtual DbSet<InventoryProduct> InventoryProducts { get; set; }
        //public virtual DbSet<OrderType> OrderTypes { get; set; }
        public virtual DbSet<Table> Tables { get; set; }
        public virtual DbSet<Safe> Safes { get; set; }
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
        public virtual DbSet<AccountBank> AccountBanks { get; set; }
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
