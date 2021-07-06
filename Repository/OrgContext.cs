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

            modelBuilder.Entity<OrderType>().HasData(
               new OrderType { Id = 1,  Name = "Internal", Hide = false, Icon = "iconsminds-right-1" },
               new OrderType { Id = 2,  Name = "External", Hide = false,Icon = "iconsminds-left-1" }
               );

            modelBuilder.Entity<TransactionType>().HasData(
              new TransactionType { Id = 1, Name = "Addition", Hide = false, InOut = 1 , Icon = "iconsminds-down-1" },
              new TransactionType { Id = 2, Name = "Issue", Hide = false, InOut = -1, Icon = "iconsminds-up-1" },
              new TransactionType { Id = 3, Name = "Transafer", Hide = false, InOut = -1, Icon = "iconsminds-shuffle-1" },
              new TransactionType { Id = 4, Name = "Received", Hide = false, InOut = 1, Icon = "iconsminds-file-edit" },
              new TransactionType { Id = 5, Name = "Adjustment In", Hide = false, InOut = 1, Icon = "" },
              new TransactionType { Id = 6, Name = "Adjustment Out", Hide = false, InOut = -1, Icon = "" }
              );

            modelBuilder.Entity<FinancialType>().HasData(
             new FinancialType { Id = 1, Name = "Collection", Hide = false, InOut = 1, Icon = "iconsminds-financial" },
             new FinancialType { Id = 2, Name = "Payment", Hide = false, InOut = -1, Icon = "iconsminds-handshake" },
             new FinancialType { Id = 3, Name = "Outlay", Hide = false, InOut = -1, Icon = "iconsminds-wallet" }
             );

            modelBuilder.Entity<PaymentType>().HasData(
                new PaymentType { Id = 1, Name = "Cash", Hide = false },
                new PaymentType { Id = 2, Name = "Check", Hide = false }
                );

            modelBuilder.Entity<Branch>().HasData(new Branch { Id = 1, Name = "Main Branch", Hide = false });

            modelBuilder.Entity<Store>().HasData(new Store { Id = 1, Name = "Main Store", BranchId = 1, Hide = false });

            modelBuilder.Entity<Dealer>().HasData(new Dealer { Id = 1, Code = "1", CodeNumber = 1, Name = "...", TypeId = 0, Hide = false });

            modelBuilder.Entity<Safe>().HasData(new Safe { Id = 1, Name = "Main Safe", Hide = false });

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
               new Preference { Id = 16, Key = "AutoCreateTransaction", Value = "0", Reference = "Invoice", TypeId = 1, Hide = false },

               new Preference { Id = 101, Key = "DefaultStore", Value = "1", Reference = "Invoice", TypeId = 2, Hide = false },
               new Preference { Id = 102, Key = "DefaultSupplier", Value = "1", Reference = "Invoice", TypeId = 2, Hide = false },
               new Preference { Id = 103, Key = "DefaultPaymentType", Value = "1", Reference = "Invoice", TypeId = 2, Hide = false },
               new Preference { Id = 104, Key = "DiscountValue", Value = "", Reference = "Invoice", TypeId = 2, Hide = false },
               new Preference { Id = 105, Key = "DefaultDiscountType", Value = "2", Reference = "Invoice", TypeId = 2, Hide = false },
               new Preference { Id = 106, Key = "ServiceValue", Value = "", Reference = "Invoice", TypeId = 2, Hide = false },
               new Preference { Id = 107, Key = "DefaultServiceType", Value = "2", Reference = "Invoice", TypeId = 2, Hide = false },
               new Preference { Id = 108, Key = "TaxValue", Value = "14", Reference = "Invoice", TypeId = 2, Hide = false },
               new Preference { Id = 109, Key = "DefaultTaxType", Value = "2", Reference = "Invoice", TypeId = 2, Hide = false },
               new Preference { Id = 110, Key = "NumberLine", Value = "6", Reference = "Invoice", TypeId = 2, Hide = false },
               new Preference { Id = 111, Key = "OrderTabe", Value = "1", Reference = "Invoice", TypeId = 2, Hide = false },
               new Preference { Id = 112, Key = "AutoSave", Value = "0", Reference = "Invoice", TypeId = 2, Hide = false },
               new Preference { Id = 113, Key = "TypeSerial", Value = "1", Reference = "Invoice", TypeId = 2, Hide = false },
               new Preference { Id = 114, Key = "AllowRepeated", Value = "1", Reference = "Invoice", TypeId = 2, Hide = false },
               new Preference { Id = 115, Key = "SaveLastStatusSetting", Value = "1", Reference = "Invoice", TypeId = 2, Hide = false },
               new Preference { Id = 116, Key = "AutoCreateTransaction", Value = "0", Reference = "Invoice", TypeId = 2, Hide = false },

               new Preference { Id = 201, Key = "DefaultStore", Value = "1", Reference = "Invoice", TypeId = 3, Hide = false },
               new Preference { Id = 202, Key = "DefaultCustomer", Value = "1", Reference = "Invoice", TypeId = 3, Hide = false },
               new Preference { Id = 203, Key = "DefaultPaymentType", Value = "1", Reference = "Invoice", TypeId = 3, Hide = false },
               new Preference { Id = 204, Key = "DiscountValue", Value = "", Reference = "Invoice", TypeId = 3, Hide = false },
               new Preference { Id = 205, Key = "DefaultDiscountType", Value = "2", Reference = "Invoice", TypeId = 3, Hide = false },
               new Preference { Id = 206, Key = "ServiceValue", Value = "", Reference = "Invoice", TypeId = 3, Hide = false },
               new Preference { Id = 207, Key = "DefaultServiceType", Value = "2", Reference = "Invoice", TypeId = 3, Hide = false },
               new Preference { Id = 208, Key = "TaxValue", Value = "14", Reference = "Invoice", TypeId = 3, Hide = false },
               new Preference { Id = 209, Key = "DefaultTaxType", Value = "2", Reference = "Invoice", TypeId = 3, Hide = false },
               new Preference { Id = 210, Key = "NumberLine", Value = "6", Reference = "Invoice", TypeId = 3, Hide = false },
               new Preference { Id = 211, Key = "OrderTabe", Value = "1", Reference = "Invoice", TypeId = 3, Hide = false },
               new Preference { Id = 212, Key = "AutoSave", Value = "0", Reference = "Invoice", TypeId = 3, Hide = false },
               new Preference { Id = 213, Key = "TypeSerial", Value = "1", Reference = "Invoice", TypeId = 3, Hide = false },
               new Preference { Id = 214, Key = "AllowRepeated", Value = "1", Reference = "Invoice", TypeId = 3, Hide = false },
               new Preference { Id = 215, Key = "SaveLastStatusSetting", Value = "1", Reference = "Invoice", TypeId = 3, Hide = false },
               new Preference { Id = 216, Key = "AutoCreateTransaction", Value = "0", Reference = "Invoice", TypeId = 4, Hide = false },

               new Preference { Id = 301, Key = "DefaultStore", Value = "1", Reference = "Invoice", TypeId = 4, Hide = false },
               new Preference { Id = 302, Key = "DefaultSupplier", Value = "1", Reference = "Invoice", TypeId = 4, Hide = false },
               new Preference { Id = 303, Key = "DefaultPaymentType", Value = "1", Reference = "Invoice", TypeId = 4, Hide = false },
               new Preference { Id = 304, Key = "DiscountValue", Value = "", Reference = "Invoice", TypeId = 4, Hide = false },
               new Preference { Id = 305, Key = "DefaultDiscountType", Value = "2", Reference = "Invoice", TypeId = 4, Hide = false },
               new Preference { Id = 306, Key = "ServiceValue", Value = "", Reference = "Invoice", TypeId = 4, Hide = false },
               new Preference { Id = 307, Key = "DefaultServiceType", Value = "2", Reference = "Invoice", TypeId = 4, Hide = false },
               new Preference { Id = 308, Key = "TaxValue", Value = "14", Reference = "Invoice", TypeId = 4, Hide = false },
               new Preference { Id = 309, Key = "DefaultTaxType", Value = "2", Reference = "Invoice", TypeId = 4, Hide = false },
               new Preference { Id = 310, Key = "NumberLine", Value = "6", Reference = "Invoice", TypeId = 4, Hide = false },
               new Preference { Id = 311, Key = "OrderTabe", Value = "1", Reference = "Invoice", TypeId = 4, Hide = false },
               new Preference { Id = 312, Key = "AutoSave", Value = "0", Reference = "Invoice", TypeId = 4, Hide = false },
               new Preference { Id = 313, Key = "TypeSerial", Value = "1", Reference = "Invoice", TypeId = 4, Hide = false },
               new Preference { Id = 314, Key = "AllowRepeated", Value = "1", Reference = "Invoice", TypeId = 4, Hide = false },
               new Preference { Id = 315, Key = "SaveLastStatusSetting", Value = "1", Reference = "Invoice", TypeId = 4, Hide = false },
               new Preference { Id = 316, Key = "AutoCreateTransaction", Value = "0", Reference = "Invoice", TypeId = 4, Hide = false },

               new Preference { Id = 401, Key = "DefaultStore", Value = "1", Reference = "Transaction", TypeId = 1, Hide = false },
               new Preference { Id = 402, Key = "DefaultSupplier", Value = "1", Reference = "Transaction", TypeId = 1, Hide = false },
               new Preference { Id = 403, Key = "NumberLine", Value = "6", Reference = "Transaction", TypeId = 1, Hide = false },
               new Preference { Id = 404, Key = "OrderTabe", Value = "2", Reference = "Transaction", TypeId = 1, Hide = false },
               new Preference { Id = 405, Key = "AutoSave", Value = "0", Reference = "Transaction", TypeId = 1, Hide = false },
               new Preference { Id = 406, Key = "TypeSerial", Value = "1", Reference = "Transaction", TypeId = 1, Hide = false },
               new Preference { Id = 407, Key = "AllowRepeated", Value = "1", Reference = "Transaction", TypeId = 1, Hide = false },
               new Preference { Id = 408, Key = "SaveLastStatusSetting", Value = "1", Reference = "Transaction", TypeId = 1, Hide = false },

               new Preference { Id = 501, Key = "DefaultStore", Value = "1", Reference = "Transaction", TypeId = 2, Hide = false },
               new Preference { Id = 502, Key = "DefaultCustomer", Value = "1", Reference = "Transaction", TypeId = 2, Hide = false },
               new Preference { Id = 503, Key = "NumberLine", Value = "6", Reference = "Transaction", TypeId = 2, Hide = false },
               new Preference { Id = 504, Key = "OrderTabe", Value = "2", Reference = "Transaction", TypeId = 2, Hide = false },
               new Preference { Id = 505, Key = "AutoSave", Value = "0", Reference = "Transaction", TypeId = 2, Hide = false },
               new Preference { Id = 506, Key = "TypeSerial", Value = "1", Reference = "Transaction", TypeId = 2, Hide = false },
               new Preference { Id = 507, Key = "AllowRepeated", Value = "1", Reference = "Transaction", TypeId = 2, Hide = false },
               new Preference { Id = 508, Key = "SaveLastStatusSetting", Value = "1", Reference = "Transaction", TypeId = 2, Hide = false },

               new Preference { Id = 601, Key = "DefaultStore", Value = "1", Reference = "Transaction", TypeId = 3, Hide = false },
               new Preference { Id = 602, Key = "NumberLine", Value = "6", Reference = "Transaction", TypeId = 3, Hide = false },
               new Preference { Id = 603, Key = "OrderTabe", Value = "2", Reference = "Transaction", TypeId = 3, Hide = false },
               new Preference { Id = 604, Key = "AutoSave", Value = "0", Reference = "Transaction", TypeId = 3, Hide = false },
               new Preference { Id = 605, Key = "TypeSerial", Value = "1", Reference = "Transaction", TypeId = 3, Hide = false },
               new Preference { Id = 606, Key = "AllowRepeated", Value = "1", Reference = "Transaction", TypeId = 3, Hide = false },
               new Preference { Id = 607, Key = "SaveLastStatusSetting", Value = "1", Reference = "Transaction", TypeId = 3, Hide = false },
               new Preference { Id = 608, Key = "AutoReceived", Value = "0", Reference = "Transaction", TypeId = 3, Hide = false },

               new Preference { Id = 701, Key = "DefaultStore", Value = "1", Reference = "Transaction", TypeId = 4, Hide = false },
               new Preference { Id = 702, Key = "NumberLine", Value = "6", Reference = "Transaction", TypeId = 4, Hide = false },
               new Preference { Id = 703, Key = "OrderTabe", Value = "2", Reference = "Transaction", TypeId = 4, Hide = false },
               new Preference { Id = 704, Key = "AutoSave", Value = "0", Reference = "Transaction", TypeId = 4, Hide = false },
               new Preference { Id = 705, Key = "TypeSerial", Value = "1", Reference = "Transaction", TypeId = 4, Hide = false },
               new Preference { Id = 706, Key = "AllowRepeated", Value = "1", Reference = "Transaction", TypeId = 4, Hide = false },

               new Preference { Id = 801, Key = "NumberLine", Value = "6", Reference = "Order", TypeId = 1, Hide = false },
               new Preference { Id = 802, Key = "OrderTabe", Value = "2", Reference = "Order", TypeId = 1, Hide = false },
               new Preference { Id = 803, Key = "AutoSave", Value = "0", Reference = "Order", TypeId = 1, Hide = false },
               new Preference { Id = 804, Key = "TypeSerial", Value = "1", Reference = "Order", TypeId = 1, Hide = false },
               new Preference { Id = 805, Key = "AllowRepeated", Value = "1", Reference = "Order", TypeId = 1, Hide = false },
               new Preference { Id = 806, Key = "DiscountValue", Value = "", Reference = "Order", TypeId = 1, Hide = false },
               new Preference { Id = 807, Key = "DefaultDiscountType", Value = "2", Reference = "Order", TypeId = 1, Hide = false },
               new Preference { Id = 808, Key = "ServiceValue", Value = "", Reference = "Order", TypeId = 1, Hide = false },
               new Preference { Id = 809, Key = "DefaultServiceType", Value = "2", Reference = "Order", TypeId = 1, Hide = false },
               new Preference { Id = 810, Key = "TaxValue", Value = "14", Reference = "Order", TypeId = 1, Hide = false },
               new Preference { Id = 811, Key = "DefaultTaxType", Value = "2", Reference = "Order", TypeId = 1, Hide = false },
               new Preference { Id = 812, Key = "AutoCreateInvoice", Value = "0", Reference = "Order", TypeId = 1, Hide = false },
               new Preference { Id = 813, Key = "DefaultCustomer", Value = "1", Reference = "Order", TypeId = 1, Hide = false },

               new Preference { Id = 901, Key = "NumberLine", Value = "6", Reference = "Order", TypeId = 2, Hide = false },
               new Preference { Id = 902, Key = "OrderTabe", Value = "2", Reference = "Order", TypeId = 2, Hide = false },
               new Preference { Id = 903, Key = "AutoSave", Value = "0", Reference = "Order", TypeId = 2, Hide = false },
               new Preference { Id = 904, Key = "TypeSerial", Value = "1", Reference = "Order", TypeId = 2, Hide = false },
               new Preference { Id = 905, Key = "AllowRepeated", Value = "1", Reference = "Order", TypeId = 2, Hide = false },
               new Preference { Id = 906, Key = "DiscountValue", Value = "", Reference = "Order", TypeId = 2, Hide = false },
               new Preference { Id = 907, Key = "DefaultDiscountType", Value = "2", Reference = "Order", TypeId = 2, Hide = false },
               new Preference { Id = 908, Key = "ServiceValue", Value = "", Reference = "Order", TypeId = 2, Hide = false },
               new Preference { Id = 909, Key = "DefaultServiceType", Value = "2", Reference = "Order", TypeId = 2, Hide = false },
               new Preference { Id = 910, Key = "TaxValue", Value = "14", Reference = "Order", TypeId = 2, Hide = false },
               new Preference { Id = 911, Key = "DefaultTaxType", Value = "2", Reference = "Order", TypeId = 2, Hide = false },
               new Preference { Id = 912, Key = "AutoCreateInvoice", Value = "0", Reference = "Order", TypeId = 2, Hide = false },
               new Preference { Id = 913, Key = "DefaultCustomer", Value = "1", Reference = "Order", TypeId = 2, Hide = false },

               new Preference { Id = 1000, Key = "DefaultClient", Value = "1", Reference = "Financial", TypeId = 1, Hide = false },
               new Preference { Id = 1001, Key = "DefaultSafe", Value = "1", Reference = "Financial", TypeId = 1, Hide = false },
               new Preference { Id = 1002, Key = "DefaultPaymentType", Value = "1", Reference = "Financial", TypeId = 1, Hide = false },
               new Preference { Id = 1003, Key = "DefaultCurrency", Value = "1", Reference = "Financial", TypeId = 1, Hide = false },
               new Preference { Id = 1004, Key = "AutoSave", Value = "0", Reference = "Financial", TypeId = 1, Hide = false },
               new Preference { Id = 1005, Key = "TypeSerial", Value = "1", Reference = "Financial", TypeId = 1, Hide = false },

               new Preference { Id = 1100, Key = "DefaultSupplier", Value = "1", Reference = "Financial", TypeId = 2, Hide = false },
               new Preference { Id = 1101, Key = "DefaultSafe", Value = "1", Reference = "Financial", TypeId = 2, Hide = false },
               new Preference { Id = 1102, Key = "DefaultPaymentType", Value = "1", Reference = "Financial", TypeId = 2, Hide = false },
               new Preference { Id = 1103, Key = "DefaultCurrency", Value = "1", Reference = "Financial", TypeId = 2, Hide = false },
               new Preference { Id = 1104, Key = "AutoSave", Value = "0", Reference = "Financial", TypeId = 2, Hide = false },
               new Preference { Id = 1105, Key = "TypeSerial", Value = "1", Reference = "Financial", TypeId = 2, Hide = false },

               new Preference { Id = 1200, Key = "DefaultOutlay", Value = "1", Reference = "Financial", TypeId = 3, Hide = false },
               new Preference { Id = 1201, Key = "DefaultSafe", Value = "1", Reference = "Financial", TypeId = 3, Hide = false },
               new Preference { Id = 1202, Key = "DefaultPaymentType", Value = "1", Reference = "Financial", TypeId = 3, Hide = false },
               new Preference { Id = 1203, Key = "DefaultCurrency", Value = "1", Reference = "Financial", TypeId = 3, Hide = false },
               new Preference { Id = 1204, Key = "AutoSave", Value = "0", Reference = "Financial", TypeId = 3, Hide = false },
               new Preference { Id = 1205, Key = "TypeSerial", Value = "1", Reference = "Financial", TypeId = 3, Hide = false }

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
    }
}