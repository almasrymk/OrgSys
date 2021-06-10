using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using Utility;

namespace Repository
{
    public class OrgContext : DbContext
    {
        public OrgContext(DbContextOptions<OrgContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfigurationRoot config = builder.Build();
            optionsBuilder.UseSqlServer(config.GetConnectionString("OrgConnection"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Permission>().HasData(
                new Permission { Id = 1, Key = "AllPage", Value = "AllPage.All", ParentId = 0 },
                    new Permission { Id = 10, Key = "Setting", Value = "Setting.All", ParentId = 1 },

                        new Permission { Id = 101, Key = "Branchs", Value = "Branchs.All", ParentId = 10 },
                            new Permission { Id = 10101, Key = "View", Value = "Branchs.View", ParentId = 101 },
                            new Permission { Id = 10102, Key = "Add", Value = "Branchs.Add", ParentId = 101 },
                            new Permission { Id = 10103, Key = "Edit", Value = "Branchs.Edit", ParentId = 101 },
                            new Permission { Id = 10104, Key = "Delete", Value = "Branchs.Delete", ParentId = 101 },

                        new Permission { Id = 102, Key = "Roles", Value = "Roles.All", ParentId = 10 },
                            new Permission { Id = 10201, Key = "View", Value = "Roles.View", ParentId = 102 },
                            new Permission { Id = 10202, Key = "Add", Value = "Roles.Add", ParentId = 102 },
                            new Permission { Id = 10203, Key = "Edit", Value = "Roles.Edit", ParentId = 102 },
                            new Permission { Id = 10204, Key = "Delete", Value = "Roles.Delete", ParentId = 102 },

                        new Permission { Id = 103, Key = "Users", Value = "Users.All", ParentId = 10 },
                            new Permission { Id = 10301, Key = "View", Value = "Users.View", ParentId = 103 },
                            new Permission { Id = 10302, Key = "Add", Value = "Users.Add", ParentId = 103 },
                            new Permission { Id = 10303, Key = "Edit", Value = "Users.Edit", ParentId = 103 },
                            new Permission { Id = 10304, Key = "Delete", Value = "Users.Delete", ParentId = 103 },

                        new Permission { Id = 104, Key = "Shifts", Value = "Shifts.All", ParentId = 10 },
                            new Permission { Id = 10401, Key = "View", Value = "Shifts.View", ParentId = 104 },
                            new Permission { Id = 10402, Key = "Add", Value = "Shifts.Add", ParentId = 104 },
                            new Permission { Id = 10403, Key = "Edit", Value = "Shifts.Edit", ParentId = 104 },
                            new Permission { Id = 10404, Key = "Delete", Value = "Shifts.Delete", ParentId = 104 },

                     new Permission { Id = 20, Key = "Sales", Value = "Sales.All", ParentId = 1 },

                        new Permission { Id = 201, Key = "Products", Value = "Products.All", ParentId = 20 },
                            new Permission { Id = 20101, Key = "View", Value = "Products.View", ParentId = 201 },
                            new Permission { Id = 20102, Key = "Add", Value = "Products.Add", ParentId = 201 },
                            new Permission { Id = 20103, Key = "Edit", Value = "Products.Edit", ParentId = 201 },
                            new Permission { Id = 20104, Key = "Delete", Value = "Products.Delete", ParentId = 201 },

                        new Permission { Id = 202, Key = "Classifications", Value = "Classifications.All", ParentId = 20 },
                            new Permission { Id = 20201, Key = "View", Value = "Classifications.View", ParentId = 202 },
                            new Permission { Id = 20202, Key = "Add", Value = "Classifications.Add", ParentId = 202 },
                            new Permission { Id = 20203, Key = "Edit", Value = "Classifications.Edit", ParentId = 202 },
                            new Permission { Id = 20204, Key = "Delete", Value = "Classifications.Delete", ParentId = 202 },

                        new Permission { Id = 203, Key = "UnitsMeasure", Value = "UnitsMeasure.All", ParentId = 20 },
                            new Permission { Id = 20301, Key = "View", Value = "UnitsMeasure.View", ParentId = 203 },
                            new Permission { Id = 20302, Key = "Add", Value = "UnitsMeasure.Add", ParentId = 203 },
                            new Permission { Id = 20303, Key = "Edit", Value = "UnitsMeasure.Edit", ParentId = 203 },
                            new Permission { Id = 20304, Key = "Delete", Value = "UnitsMeasure.Delete", ParentId = 203 },

                        new Permission { Id = 204, Key = "Clients", Value = "Clients.All", ParentId = 20 },
                            new Permission { Id = 20401, Key = "View", Value = "Clients.View", ParentId = 204 },
                            new Permission { Id = 20402, Key = "Add", Value = "Clients.Add", ParentId = 204 },
                            new Permission { Id = 20403, Key = "Edit", Value = "Clients.Edit", ParentId = 204 },
                            new Permission { Id = 20404, Key = "Delete", Value = "Clients.Delete", ParentId = 204 }
                );

            modelBuilder.Entity<Role>().HasData(new Role { Id = 1, Name = "Owner", Hide = false });

            modelBuilder.Entity<User>().HasData(new User { Id = 1, Name = "Owner", UserName = "Owner", Password = Security.Encrypt("OwnerAbc@123"), RoleId = 1, Hide = false });

            modelBuilder.Entity<InvoiceType>().HasData(
                new InvoiceType { Id = 1 , Group = "Sales", Name = "Invoice", Hide = false, InOut = -1   , Icon = "simple-icon-basket-loaded" },
                new InvoiceType { Id = 2, Group = "Purchases", Name = "Invoice", Hide = false, InOut = 1, Icon = "simple-icon-basket-loaded" },
                new InvoiceType { Id = 3, Group = "Sales", Name = "Return", Hide = false, InOut = 1, Icon = "simple-icon-action-undo" },
                new InvoiceType { Id = 4, Group = "Purchases", Name = "Return", Hide = false, InOut = -1, Icon = "simple-icon-action-undo" }
                );

            modelBuilder.Entity<TransactionType>().HasData(
              new TransactionType { Id = 1, Name = "Addition", Hide = false, InOut = 1 , Icon = "iconsminds-down-1" },
              new TransactionType { Id = 2, Name = "Issue", Hide = false, InOut = -1, Icon = "iconsminds-up-1" },
              new TransactionType { Id = 3, Name = "Transafer", Hide = false, InOut = -1, Icon = "iconsminds-shuffle-1" },
              new TransactionType { Id = 4, Name = "Received", Hide = false, InOut = 1, Icon = "iconsminds-file-edit" }
              );

            modelBuilder.Entity<PaymentType>().HasData(
                new PaymentType { Id = 1, Name = "Cash", Hide = false },
                new PaymentType { Id = 2, Name = "Check", Hide = false }
                );

            modelBuilder.Entity<Branch>().HasData(new Branch { Id = 1, Name = "Main Branch", Hide = false });

            modelBuilder.Entity<Store>().HasData(new Store { Id = 1, Name = "Main Store", BranchId = 1, Hide = false });

            modelBuilder.Entity<Dealer>().HasData(new Dealer { Id = 1, Code = "1", CodeNumber = 1, Name = "...", TypeId = 0, Hide = false });

            modelBuilder.Entity<Preference>().HasData(
               new Preference { Id = 1, Key = "DefaultStore", Value = "1", Reference = "Invoice", TypeId = 1, Hide = false },
               new Preference { Id = 2, Key = "DefaultCustomer", Value = "1", Reference = "Invoice", TypeId = 1, Hide = false },
               new Preference { Id = 3, Key = "DefaultPaymentType", Value = "1", Reference = "Invoice", TypeId = 1, Hide = false },
               new Preference { Id = 4, Key = "DiscountValue", Value = "", Reference = "Invoice", TypeId = 1, Hide = false },
               new Preference { Id = 5, Key = "DefaultDiscountType", Value = "2", Reference = "Invoice", TypeId = 1, Hide = false },
               new Preference { Id = 6, Key = "ServiceValue", Value = "", Reference = "Invoice", TypeId = 1, Hide = false },
               new Preference { Id = 7, Key = "DefaultServiceType", Value = "2", Reference = "Invoice", TypeId = 1, Hide = false },
               new Preference { Id = 8, Key = "TaxValue", Value = "14", Reference = "Invoice", TypeId = 1, Hide = false },
               new Preference { Id = 9, Key = "DefaultTaxType", Value = "2", Reference = "Invoice", TypeId = 1, Hide = false },
               new Preference { Id = 10, Key = "NumberLine", Value = "6", Reference = "Invoice", TypeId = 1, Hide = false },
               new Preference { Id = 11, Key = "OrderTabe", Value = "1", Reference = "Invoice", TypeId = 1, Hide = false },
               new Preference { Id = 12, Key = "AutoSave", Value = "0", Reference = "Invoice", TypeId = 1, Hide = false },
               new Preference { Id = 13, Key = "TypeSerial", Value = "1", Reference = "Invoice", TypeId = 1, Hide = false },
               new Preference { Id = 14, Key = "AllowRepeated", Value = "1", Reference = "Invoice", TypeId = 1, Hide = false },
               new Preference { Id = 15, Key = "SaveLastStatusSetting", Value = "1", Reference = "Invoice", TypeId = 1, Hide = false },

               new Preference { Id = 16, Key = "DefaultStore", Value = "1", Reference = "Invoice", TypeId = 2, Hide = false },
               new Preference { Id = 17, Key = "DefaultSupplier", Value = "1", Reference = "Invoice", TypeId = 2, Hide = false },
               new Preference { Id = 18, Key = "DefaultPaymentType", Value = "1", Reference = "Invoice", TypeId = 2, Hide = false },
               new Preference { Id = 19, Key = "DiscountValue", Value = "", Reference = "Invoice", TypeId = 2, Hide = false },
               new Preference { Id = 20, Key = "DefaultDiscountType", Value = "2", Reference = "Invoice", TypeId = 2, Hide = false },
               new Preference { Id = 21, Key = "ServiceValue", Value = "", Reference = "Invoice", TypeId = 2, Hide = false },
               new Preference { Id = 22, Key = "DefaultServiceType", Value = "2", Reference = "Invoice", TypeId = 2, Hide = false },
               new Preference { Id = 23, Key = "TaxValue", Value = "14", Reference = "Invoice", TypeId = 2, Hide = false },
               new Preference { Id = 24, Key = "DefaultTaxType", Value = "2", Reference = "Invoice", TypeId = 2, Hide = false },
               new Preference { Id = 25, Key = "NumberLine", Value = "6", Reference = "Invoice", TypeId = 2, Hide = false },
               new Preference { Id = 26, Key = "OrderTabe", Value = "1", Reference = "Invoice", TypeId = 2, Hide = false },
               new Preference { Id = 27, Key = "AutoSave", Value = "0", Reference = "Invoice", TypeId = 2, Hide = false },
               new Preference { Id = 28, Key = "TypeSerial", Value = "1", Reference = "Invoice", TypeId = 2, Hide = false },
               new Preference { Id = 29, Key = "AllowRepeated", Value = "1", Reference = "Invoice", TypeId = 2, Hide = false },
               new Preference { Id = 30, Key = "SaveLastStatusSetting", Value = "1", Reference = "Invoice", TypeId = 2, Hide = false },

               new Preference { Id = 31, Key = "DefaultStore", Value = "1", Reference = "Invoice", TypeId = 3, Hide = false },
               new Preference { Id = 32, Key = "DefaultCustomer", Value = "1", Reference = "Invoice", TypeId = 3, Hide = false },
               new Preference { Id = 33, Key = "DefaultPaymentType", Value = "1", Reference = "Invoice", TypeId = 3, Hide = false },
               new Preference { Id = 34, Key = "DiscountValue", Value = "", Reference = "Invoice", TypeId = 3, Hide = false },
               new Preference { Id = 35, Key = "DefaultDiscountType", Value = "2", Reference = "Invoice", TypeId = 3, Hide = false },
               new Preference { Id = 36, Key = "ServiceValue", Value = "", Reference = "Invoice", TypeId = 3, Hide = false },
               new Preference { Id = 37, Key = "DefaultServiceType", Value = "2", Reference = "Invoice", TypeId = 3, Hide = false },
               new Preference { Id = 38, Key = "TaxValue", Value = "14", Reference = "Invoice", TypeId = 3, Hide = false },
               new Preference { Id = 39, Key = "DefaultTaxType", Value = "2", Reference = "Invoice", TypeId = 3, Hide = false },
               new Preference { Id = 40, Key = "NumberLine", Value = "6", Reference = "Invoice", TypeId = 3, Hide = false },
               new Preference { Id = 41, Key = "OrderTabe", Value = "1", Reference = "Invoice", TypeId = 3, Hide = false },
               new Preference { Id = 42, Key = "AutoSave", Value = "0", Reference = "Invoice", TypeId = 3, Hide = false },
               new Preference { Id = 43, Key = "TypeSerial", Value = "1", Reference = "Invoice", TypeId = 3, Hide = false },
               new Preference { Id = 44, Key = "AllowRepeated", Value = "1", Reference = "Invoice", TypeId = 3, Hide = false },
               new Preference { Id = 45, Key = "SaveLastStatusSetting", Value = "1", Reference = "Invoice", TypeId = 3, Hide = false },

               new Preference { Id = 46, Key = "DefaultStore", Value = "1", Reference = "Invoice", TypeId = 4, Hide = false },
               new Preference { Id = 47, Key = "DefaultSupplier", Value = "1", Reference = "Invoice", TypeId = 4, Hide = false },
               new Preference { Id = 48, Key = "DefaultPaymentType", Value = "1", Reference = "Invoice", TypeId = 4, Hide = false },
               new Preference { Id = 49, Key = "DiscountValue", Value = "", Reference = "Invoice", TypeId = 4, Hide = false },
               new Preference { Id = 50, Key = "DefaultDiscountType", Value = "2", Reference = "Invoice", TypeId = 4, Hide = false },
               new Preference { Id = 51, Key = "ServiceValue", Value = "", Reference = "Invoice", TypeId = 4, Hide = false },
               new Preference { Id = 52, Key = "DefaultServiceType", Value = "2", Reference = "Invoice", TypeId = 4, Hide = false },
               new Preference { Id = 53, Key = "TaxValue", Value = "14", Reference = "Invoice", TypeId = 4, Hide = false },
               new Preference { Id = 54, Key = "DefaultTaxType", Value = "2", Reference = "Invoice", TypeId = 4, Hide = false },
               new Preference { Id = 55, Key = "NumberLine", Value = "6", Reference = "Invoice", TypeId = 4, Hide = false },
               new Preference { Id = 56, Key = "OrderTabe", Value = "1", Reference = "Invoice", TypeId = 4, Hide = false },
               new Preference { Id = 57, Key = "AutoSave", Value = "0", Reference = "Invoice", TypeId = 4, Hide = false },
               new Preference { Id = 58, Key = "TypeSerial", Value = "1", Reference = "Invoice", TypeId = 4, Hide = false },
               new Preference { Id = 59, Key = "AllowRepeated", Value = "1", Reference = "Invoice", TypeId = 4, Hide = false },
               new Preference { Id = 60, Key = "SaveLastStatusSetting", Value = "1", Reference = "Invoice", TypeId = 4, Hide = false },

               new Preference { Id = 61, Key = "DefaultStore", Value = "1", Reference = "Transaction", TypeId = 1, Hide = false },
               new Preference { Id = 62, Key = "DefaultSupplier", Value = "1", Reference = "Transaction", TypeId = 1, Hide = false },
               new Preference { Id = 63, Key = "NumberLine", Value = "6", Reference = "Transaction", TypeId = 1, Hide = false },
               new Preference { Id = 64, Key = "OrderTabe", Value = "2", Reference = "Transaction", TypeId = 1, Hide = false },
               new Preference { Id = 65, Key = "AutoSave", Value = "0", Reference = "Transaction", TypeId = 1, Hide = false },
               new Preference { Id = 66, Key = "TypeSerial", Value = "1", Reference = "Transaction", TypeId = 1, Hide = false },
               new Preference { Id = 67, Key = "AllowRepeated", Value = "1", Reference = "Transaction", TypeId = 1, Hide = false },
               new Preference { Id = 68, Key = "SaveLastStatusSetting", Value = "1", Reference = "Transaction", TypeId = 1, Hide = false },

               new Preference { Id = 69, Key = "DefaultStore", Value = "1", Reference = "Transaction", TypeId = 2, Hide = false },
               new Preference { Id = 70, Key = "DefaultCustomer", Value = "1", Reference = "Transaction", TypeId = 2, Hide = false },
               new Preference { Id = 71, Key = "NumberLine", Value = "6", Reference = "Transaction", TypeId = 2, Hide = false },
               new Preference { Id = 72, Key = "OrderTabe", Value = "2", Reference = "Transaction", TypeId = 2, Hide = false },
               new Preference { Id = 73, Key = "AutoSave", Value = "0", Reference = "Transaction", TypeId = 2, Hide = false },
               new Preference { Id = 74, Key = "TypeSerial", Value = "1", Reference = "Transaction", TypeId = 2, Hide = false },
               new Preference { Id = 75, Key = "AllowRepeated", Value = "1", Reference = "Transaction", TypeId = 2, Hide = false },
               new Preference { Id = 76, Key = "SaveLastStatusSetting", Value = "1", Reference = "Transaction", TypeId = 2, Hide = false },

               new Preference { Id = 77, Key = "DefaultStore", Value = "1", Reference = "Transaction", TypeId = 3, Hide = false },
               new Preference { Id = 78, Key = "NumberLine", Value = "6", Reference = "Transaction", TypeId = 3, Hide = false },
               new Preference { Id = 79, Key = "OrderTabe", Value = "2", Reference = "Transaction", TypeId = 3, Hide = false },
               new Preference { Id = 80, Key = "AutoSave", Value = "0", Reference = "Transaction", TypeId = 3, Hide = false },
               new Preference { Id = 81, Key = "TypeSerial", Value = "1", Reference = "Transaction", TypeId = 3, Hide = false },
               new Preference { Id = 82, Key = "AllowRepeated", Value = "1", Reference = "Transaction", TypeId = 3, Hide = false },
               new Preference { Id = 83, Key = "SaveLastStatusSetting", Value = "1", Reference = "Transaction", TypeId = 3, Hide = false },
               new Preference { Id = 84, Key = "AutoReceived", Value = "0", Reference = "Transaction", TypeId = 3, Hide = false },

               new Preference { Id = 85, Key = "DefaultStore", Value = "1", Reference = "Transaction", TypeId = 4, Hide = false },
               new Preference { Id = 86, Key = "NumberLine", Value = "6", Reference = "Transaction", TypeId = 4, Hide = false },
               new Preference { Id = 87, Key = "OrderTabe", Value = "2", Reference = "Transaction", TypeId = 4, Hide = false },
               new Preference { Id = 88, Key = "AutoSave", Value = "0", Reference = "Transaction", TypeId = 4, Hide = false },
               new Preference { Id = 89, Key = "TypeSerial", Value = "1", Reference = "Transaction", TypeId = 4, Hide = false },
               new Preference { Id = 90, Key = "AllowRepeated", Value = "1", Reference = "Transaction", TypeId = 4, Hide = false }
               );
        }

        public virtual DbSet<Unit> Units { get; set; }
        public virtual DbSet<Property> Properties { get; set; }
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
    }
}