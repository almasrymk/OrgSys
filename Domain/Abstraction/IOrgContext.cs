namespace Domain.Abstraction
{
    using Domain.Entities;
    using Microsoft.EntityFrameworkCore;

    public interface IOrgContext : IDisposable
    {
        DbSet<Unit> Units { get; set; }
        DbSet<Property> Properties { get; set; }
        DbSet<DealerGroup> DealerGroups { get; set; }
        DbSet<Dealer> Dealers { get; set; }
        DbSet<Classification> Classifications { get; set; }
        DbSet<Product> Products { get; set; }
        DbSet<ProductUnit> ProductUnits { get; set; }
        DbSet<Branch> Branches { get; set; }
        DbSet<Stock> Stocks { get; set; }
        DbSet<Role> Roles { get; set; }
        DbSet<Shift> Shifts { get; set; }
        DbSet<User> Users { get; set; }
        DbSet<Permission> Permissions { get; set; }
        DbSet<Invoice> Invoices { get; set; }
        DbSet<InvoiceProduct> InvoiceProducts { get; set; }
        DbSet<InvoiceType> InvoiceTypes { get; set; }
        //DbSet<Order> Orders { get; set; }
        //DbSet<OrderProduct> OrderProducts { get; set; }
        DbSet<PaymentType> PaymentTypes { get; set; }
        //DbSet<LogSys> LogSys { get; set; }
        //DbSet<ProductRecipe> ProductRecipes { get; set; }
        //DbSet<PropertyElement> PropertyElements { get; set; }
        //DbSet<ProductPropertyElement> ProductPropertyElements { get; set; }
        DbSet<Preference> Preferences { get; set; }
        //DbSet<TransactionType> TransactionTypes { get; set; }
        //DbSet<Transaction> Transactions { get; set; }
        //DbSet<TransactionProduct> TransactionProducts { get; set; }
        //DbSet<Inventory> Inventories { get; set; }
        //DbSet<InventoryProduct> InventoryProducts { get; set; }
        //DbSet<OrderType> OrderTypes { get; set; }
        DbSet<Table> Tables { get; set; }
        DbSet<CashBox> CashBoxes { get; set; }
        DbSet<Financial> Financials { get; set; }
        DbSet<FinancialAccount> FinancialAccounts { get; set; }
        DbSet<FinancialType> FinancialTypes { get; set; }
        DbSet<FinancialTransfer> FinancialTransfers { get; set; }
        DbSet<FinancialInvoice> FinancialInvoices { get; set; }
        DbSet<Outlay> Outlays { get; set; }
        DbSet<Currency> Currencys { get; set; }
        DbSet<RolePermission> RolePermissions { get; set; }
        //DbSet<Notification> Notifications { get; set; }
        DbSet<Account> Accounts { get; set; }
        DbSet<BankAccount> BankAccounts { get; set; }
        DbSet<AccountType> AccountTypes { get; set; }
        DbSet<Bank> Banks { get; set; }
        DbSet<BankBranch> BankBranchs { get; set; }
        DbSet<City> Cities { get; set; }
        DbSet<Country> Countries { get; set; }
        DbSet<District> Districts { get; set; }
        DbSet<JournalType> JournalTypes { get; set; }
        DbSet<Journal> Journals { get; set; }
        DbSet<FiscalYear> FiscalYears { get; set; }
        DbSet<FiscalPeriod> FiscalPeriods { get; set; }
        //DbSet<JournalItem> JournalItems { get; set; }
        //DbSet<CompanyProfile> CompanyProfiles { get; set; }

        void ResetDbContextState();

        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        int SaveChanges();

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        Task BeginTransactionAsync();

        Task CommitAsync();

        Task RollbackAsync();
    }
}
