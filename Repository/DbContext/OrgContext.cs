using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Internal;
using Microsoft.EntityFrameworkCore.SqlServer.Migrations.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Reflection;
using System.Text;
using Utility;

namespace Repository
{
    public class OrgContext : DbContext
    {
        public string Schema { get; set; } = "org";

        public OrgContext(DbContextOptions<OrgContext> options) : base(options)
        {

        }

        public OrgContext(DbContextOptions<OrgContext> MyOptions, string schema) : base(MyOptions)
        {
            if ("" + schema != "")
                Schema = schema;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfigurationRoot config = builder.Build();
            string assemblyName = typeof(OrgContext).Namespace;
            optionsBuilder.UseSqlServer(config.GetConnectionString("OrgConnection"), e => e.MigrationsHistoryTable($"__MigrationsHistory", Schema)).ReplaceService<IModelCacheKeyFactory, DbSchemaAwareModelCacheKeyFactory>().ReplaceService<IMigrationsAssembly, DbSchemaAwareMigrationAssembly>();
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(Schema);
        }

        public virtual DbSet<Unit> Units { get; set; }
        public virtual DbSet<Property> Properties { get; set; }
        public virtual DbSet<DealerGroup> DealerGroups { get; set; }
        public virtual DbSet<Dealer> Dealers { get; set; }
        public virtual DbSet<Classification> Classifications { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductUnit> ProductUnits { get; set; }
        public virtual DbSet<Branch> Branches { get; set; }
        public virtual DbSet<Store> Stores { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Shift> Shifts { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Permission> Permissions { get; set; }
        public virtual DbSet<Invoice> Invoices { get; set; }
        public virtual DbSet<InvoiceProduct> InvoiceProducts { get; set; }
        public virtual DbSet<InvoiceType> InvoiceTypes { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderProduct> OrderProducts { get; set; }
        public virtual DbSet<PaymentType> PaymentTypes { get; set; }
        public virtual DbSet<LogSys> LogSys { get; set; }
        public virtual DbSet<ProductRecipe> ProductRecipes { get; set; }
        public virtual DbSet<PropertyElement> PropertyElements { get; set; }
        public virtual DbSet<ProductPropertyElement> ProductPropertyElements { get; set; }
        public virtual DbSet<Preference> Preferences { get; set; }
        public virtual DbSet<TransactionType> TransactionTypes { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }
        public virtual DbSet<TransactionProduct> TransactionProducts { get; set; }
        public virtual DbSet<Inventory> Inventories { get; set; }
        public virtual DbSet<InventoryProduct> InventoryProducts { get; set; }
        public virtual DbSet<OrderType> OrderTypes { get; set; }
        public virtual DbSet<Table> Tables { get; set; }
        public virtual DbSet<Safe> Safes { get; set; }
        public virtual DbSet<Financial> Financials { get; set; }
        public virtual DbSet<FinancialInvoice> FinancialInvoices { get; set; }
        public virtual DbSet<FinancialType> FinancialTypes { get; set; }
        public virtual DbSet<Outlay> Outlays { get; set; }
        public virtual DbSet<Currency> Currencys { get; set; }
        public virtual DbSet<RolePermission> RolePermissions { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<AccountBank> AccountBanks { get; set; }
        public virtual DbSet<AccountType> AccountTypes { get; set; }
        public virtual DbSet<Bank> Banks { get; set; }
        public virtual DbSet<BankBranch> BankBranchs { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<District> Districts { get; set; }
        public virtual DbSet<Journal> Journals { get; set; }
        public virtual DbSet<JournalItem> JournalItems { get; set; }
        public virtual DbSet<CompanyProfile> CompanyProfiles { get; set; }
    }

    public class DbSchemaAwareModelCacheKeyFactory : IModelCacheKeyFactory
    {
        private string _schemaName;
        public object Create(DbContext context)
        {
            var dataContext = context as OrgContext;
            if (dataContext != null)
            {
                _schemaName = dataContext.Schema;
            }
            return new MultiTenantModelCacheKey(_schemaName, context);
        }

    }

    public class MultiTenantModelCacheKey : ModelCacheKey
    {
        private readonly string _schemaName;
        public MultiTenantModelCacheKey(string schemaName, DbContext context) : base(context)
        {
            _schemaName = schemaName;
        }
        public override int GetHashCode()
        {
            return _schemaName.GetHashCode();
        }
    }

    public class DbSchemaAwareMigrationAssembly : MigrationsAssembly
    {
        private readonly DbContext _context;

        public DbSchemaAwareMigrationAssembly(ICurrentDbContext currentContext,
              IDbContextOptions options, IMigrationsIdGenerator idGenerator,
              IDiagnosticsLogger<DbLoggerCategory.Migrations> logger)
          : base(currentContext, options, idGenerator, logger)
        {
            _context = currentContext.Context;
        }
        public override string FindMigrationId(string nameOrId)
        {
            return base.FindMigrationId(nameOrId);
        }

        public override Migration CreateMigration(TypeInfo migrationClass,
              string activeProvider)
        {
            if (activeProvider == null)
                throw new ArgumentNullException(nameof(activeProvider));

            PropertyInfo pinfo = typeof(OrgContext).GetProperty("Schema");
            var Schema = "" + pinfo.GetValue(_context);

            var hasCtorWithSchema = migrationClass.GetConstructor(new[] { typeof(string) }) != null;
            if (hasCtorWithSchema)
            {
                var instance = (Migration)Activator.CreateInstance(migrationClass.AsType(), Schema);
                instance.ActiveProvider = activeProvider;
                return instance;
            }
            return base.CreateMigration(migrationClass, activeProvider);
        }
    }
}