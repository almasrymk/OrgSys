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
                new Permission { Id = 1, Name = "Organizer", Key = "Organizer", ParentId = 0 },
                    new Permission { Id = 10, Name = "Data", Key = "Data.All", ParentId = 1 },

                      new Permission { Id = 101, Name = "Organization", Key = "Organization", ParentId = 10 },

                        new Permission { Id = 10101, Name = "Branchs", Key = "Branchs.All", ParentId = 101 },
                            new Permission { Id = 1010101, Name = "View", Key = "Branchs.View", ParentId = 10101, TypeId = 1 },
                            new Permission { Id = 1010102, Name = "Add", Key = "Branchs.Add", ParentId = 10101, TypeId = 1 },
                            new Permission { Id = 1010103, Name = "Edit", Key = "Branchs.Edit", ParentId = 10101, TypeId = 1 },
                            new Permission { Id = 1010104, Name = "Delete", Key = "Branchs.Delete", ParentId = 10101, TypeId = 1 },

                        new Permission { Id = 10102, Name = "Stores", Key = "Stores.All", ParentId = 101 },
                            new Permission { Id = 1010201, Name = "View", Key = "Stores.View", ParentId = 10102, TypeId = 1 },
                            new Permission { Id = 1010202, Name = "Add", Key = "Stores.Add", ParentId = 10102, TypeId = 1 },
                            new Permission { Id = 1010203, Name = "Edit", Key = "Stores.Edit", ParentId = 10102, TypeId = 1 },
                            new Permission { Id = 1010204, Name = "Delete", Key = "Stores.Delete", ParentId = 10102, TypeId = 1 },

                        new Permission { Id = 10103, Name = "Tables", Key = "Tables.All", ParentId = 101 },
                            new Permission { Id = 1010301, Name = "View", Key = "Tables.View", ParentId = 10103, TypeId = 1 },
                            new Permission { Id = 1010302, Name = "Add", Key = "Tables.Add", ParentId = 10103, TypeId = 1 },
                            new Permission { Id = 1010303, Name = "Edit", Key = "Tables.Edit", ParentId = 10103, TypeId = 1 },
                            new Permission { Id = 1010304, Name = "Delete", Key = "Tables.Delete", ParentId = 10103, TypeId = 1 },
                        
                     new Permission { Id = 102, Name = "Security", Key = "Security", ParentId = 10 },

                        new Permission { Id = 10201, Name = "Roles", Key = "Roles.All", ParentId = 102 },
                            new Permission { Id = 1020101, Name = "View", Key = "Roles.View", ParentId = 10201, TypeId = 1 },
                            new Permission { Id = 1020102, Name = "Add", Key = "Roles.Add", ParentId = 10201, TypeId = 1 },
                            new Permission { Id = 1020103, Name = "Edit", Key = "Roles.Edit", ParentId = 10201, TypeId = 1 },
                            new Permission { Id = 1020104, Name = "Delete", Key = "Roles.Delete", ParentId = 10201, TypeId = 1 },

                        new Permission { Id = 10202, Name = "Users", Key = "Users.All", ParentId = 102 },
                            new Permission { Id = 1020201, Name = "View", Key = "Users.View", ParentId = 10202, TypeId = 1 },
                            new Permission { Id = 1020202, Name = "Add", Key = "Users.Add", ParentId = 10202, TypeId = 1 },
                            new Permission { Id = 1020203, Name = "Edit", Key = "Users.Edit", ParentId = 10202, TypeId = 1 },
                            new Permission { Id = 1020204, Name = "Delete", Key = "Users.Delete", ParentId = 10202, TypeId = 1 },

                        new Permission { Id = 10203, Name = "Shifts", Key = "Shifts.All", ParentId = 102 },
                            new Permission { Id = 1020301, Name = "View", Key = "Shifts.View", ParentId = 10203, TypeId = 1 },
                            new Permission { Id = 1020302, Name = "Add", Key = "Shifts.Add", ParentId = 10203, TypeId = 1 },
                            new Permission { Id = 1020303, Name = "Edit", Key = "Shifts.Edit", ParentId = 10203, TypeId = 1 },
                            new Permission { Id = 1020304, Name = "Delete", Key = "Shifts.Delete", ParentId = 10203, TypeId = 1 },

                     new Permission { Id = 103, Name = "Products", Key = "Products", ParentId = 10 },

                        new Permission { Id = 10301, Name = "Products", Key = "Products.All", ParentId = 103 },
                            new Permission { Id = 1030101, Name = "View", Key = "Products.View", ParentId = 10301, TypeId = 1 },
                            new Permission { Id = 1030102, Name = "Add", Key = "Products.Add", ParentId = 10301, TypeId = 1 },
                            new Permission { Id = 1030103, Name = "Edit", Key = "Products.Edit", ParentId = 10301, TypeId = 1 },
                            new Permission { Id = 1030104, Name = "Delete", Key = "Products.Delete", ParentId = 10301, TypeId = 1 },

                        new Permission { Id = 10302, Name = "Classifications", Key = "Classifications.All", ParentId = 103 },
                            new Permission { Id = 1030201, Name = "View", Key = "Classifications.View", ParentId = 10302, TypeId = 1 },
                            new Permission { Id = 1030202, Name = "Add", Key = "Classifications.Add", ParentId = 10302, TypeId = 1 },
                            new Permission { Id = 1030203, Name = "Edit", Key = "Classifications.Edit", ParentId = 10302, TypeId = 1 },
                            new Permission { Id = 1030204, Name = "Delete", Key = "Classifications.Delete", ParentId = 10302, TypeId = 1 },

                        new Permission { Id = 10303, Name = "Units Measure", Key = "UnitsMeasure.All", ParentId = 103 },
                            new Permission { Id = 1030301, Name = "View", Key = "UnitsMeasure.View", ParentId = 10303, TypeId = 1 },
                            new Permission { Id = 1030302, Name = "Add", Key = "UnitsMeasure.Add", ParentId = 10303, TypeId = 1 },
                            new Permission { Id = 1030303, Name = "Edit", Key = "UnitsMeasure.Edit", ParentId = 10303, TypeId = 1 },
                            new Permission { Id = 1030304, Name = "Delete", Key = "UnitsMeasure.Delete", ParentId = 10303, TypeId = 1 },

                      new Permission { Id = 104, Name = "Dealers", Key = "Dealers", ParentId = 10 },

                        new Permission { Id = 10401, Name = "Clients", Key = "Clients.All", ParentId = 104 },
                            new Permission { Id = 1040101, Name = "View", Key = "Clients.View", ParentId = 10401, TypeId = 1 },
                            new Permission { Id = 1040102, Name = "Add", Key = "Clients.Add", ParentId = 10401, TypeId = 1 },
                            new Permission { Id = 1040103, Name = "Edit", Key = "Clients.Edit", ParentId = 10401, TypeId = 1 },
                            new Permission { Id = 1040104, Name = "Delete", Key = "Clients.Delete", ParentId = 10401, TypeId = 1 },

                        new Permission { Id = 10402, Name = "Suppliers", Key = "Suppliers.All", ParentId = 104 },
                            new Permission { Id = 1040201, Name = "View", Key = "Suppliers.View", ParentId = 10402, TypeId = 1 },
                            new Permission { Id = 1040202, Name = "Add", Key = "Suppliers.Add", ParentId = 10402, TypeId = 1 },
                            new Permission { Id = 1040203, Name = "Edit", Key = "Suppliers.Edit", ParentId = 10402, TypeId = 1 },
                            new Permission { Id = 1040204, Name = "Delete", Key = "Suppliers.Delete", ParentId = 10402, TypeId = 1 },

                      new Permission { Id = 105, Name = "Financials", Key = "Financials", ParentId = 10 },

                       new Permission { Id = 10501, Name = "Safes", Key = "Safes.All", ParentId = 105 },
                            new Permission { Id = 1050101, Name = "View", Key = "Safes.View", ParentId = 10501, TypeId = 1 },
                            new Permission { Id = 1050102, Name = "Add", Key = "Safes.Add", ParentId = 10501, TypeId = 1 },
                            new Permission { Id = 1050103, Name = "Edit", Key = "Safes.Edit", ParentId = 10501, TypeId = 1 },
                            new Permission { Id = 1050104, Name = "Delete", Key = "Safes.Delete", ParentId = 10501, TypeId = 1 },

                        new Permission { Id = 10502, Name = "Outlay Terms", Key = "OutlayTerms.All", ParentId = 105 },
                            new Permission { Id = 1050201, Name = "View", Key = "OutlayTerms.View", ParentId = 10502, TypeId = 1 },
                            new Permission { Id = 1050202, Name = "Add", Key = "OutlayTerms.Add", ParentId = 10502, TypeId = 1 },
                            new Permission { Id = 1050203, Name = "Edit", Key = "OutlayTerms.Edit", ParentId = 10502, TypeId = 1 },
                            new Permission { Id = 1050204, Name = "Delete", Key = "OutlayTerms.Delete", ParentId = 10502, TypeId = 1 },

                        new Permission { Id = 10503, Name = "Currencies", Key = "Currencies.All", ParentId = 105 },
                            new Permission { Id = 1050301, Name = "View", Key = "Currencies.View", ParentId = 10503, TypeId = 1 },
                            new Permission { Id = 1050302, Name = "Add", Key = "Currencies.Add", ParentId = 10503, TypeId = 1 },
                            new Permission { Id = 1050303, Name = "Edit", Key = "Currencies.Edit", ParentId = 10503, TypeId = 1 },
                            new Permission { Id = 1050304, Name = "Delete", Key = "Currencies.Delete", ParentId = 10503, TypeId = 1 },

                new Permission { Id = 20, Name = "Orders", Key = "Orders.All", ParentId = 1 },

                   new Permission { Id = 201, Name = "Order Notices", Key = "Orders", ParentId = 20 },

                        new Permission { Id = 20101, Name = "Internal", Key = "Internal.All", ParentId = 201 },
                            new Permission { Id = 2010101, Name = "View", Key = "Internal.View", ParentId = 20101, TypeId = 1 },
                            new Permission { Id = 2010102, Name = "Add", Key = "Internal.Add", ParentId = 20101, TypeId = 1 },
                            new Permission { Id = 2010103, Name = "Edit", Key = "Internal.Edit", ParentId = 20101, TypeId = 1 },
                            new Permission { Id = 2010104, Name = "Delete", Key = "Internal.Delete", ParentId = 20101, TypeId = 1 },
                            new Permission { Id = 2010105, Name = "Preference", Key = "Internal.Preference", ParentId = 20101, TypeId = 1 },

                        new Permission { Id = 20102, Name = "External", Key = "External.All", ParentId = 201 },
                            new Permission { Id = 2010201, Name = "View", Key = "External.View", ParentId = 20102, TypeId = 1 },
                            new Permission { Id = 2010202, Name = "Add", Key = "External.Add", ParentId = 20102, TypeId = 1 },
                            new Permission { Id = 2010203, Name = "Edit", Key = "External.Edit", ParentId = 20102, TypeId = 1 },
                            new Permission { Id = 2010204, Name = "Delete", Key = "External.Delete", ParentId = 20102, TypeId = 1 },
                            new Permission { Id = 2010205, Name = "Preference", Key = "External.Preference", ParentId = 20102, TypeId = 1 },

                new Permission { Id = 30, Name = "Invoices", Key = "Invoices.All", ParentId = 1 },

                   new Permission { Id = 301, Name = "Sales", Key = "Sales", ParentId = 30 },

                        new Permission { Id = 30101, Name = "Invoices", Key = "SalesInvoices.All", ParentId = 301 },
                            new Permission { Id = 3010101, Name = "View", Key = "SalesInvoices.View", ParentId = 30101, TypeId = 1 },
                            new Permission { Id = 3010102, Name = "Add", Key = "SalesInvoices.Add", ParentId = 30101, TypeId = 1 },
                            new Permission { Id = 3010103, Name = "Edit", Key = "SalesInvoices.Edit", ParentId = 30101, TypeId = 1 },
                            new Permission { Id = 3010104, Name = "Delete", Key = "SalesInvoices.Delete", ParentId = 30101, TypeId = 1 },
                            new Permission { Id = 3010105, Name = "Preference", Key = "SalesInvoices.Preference", ParentId = 30101, TypeId = 1 },

                        new Permission { Id = 30102, Name = "Returns", Key = "SalesReturns.All", ParentId = 301 },
                            new Permission { Id = 3010201, Name = "View", Key = "SalesReturns.View", ParentId = 30102, TypeId = 1 },
                            new Permission { Id = 3010202, Name = "Add", Key = "SalesReturns.Add", ParentId = 30102, TypeId = 1 },
                            new Permission { Id = 3010203, Name = "Edit", Key = "SalesReturns.Edit", ParentId = 30102, TypeId = 1 },
                            new Permission { Id = 3010204, Name = "Delete", Key = "SalesReturns.Delete", ParentId = 30102, TypeId = 1 },
                            new Permission { Id = 3010205, Name = "Preference", Key = "SalesReturns.Preference", ParentId = 30102, TypeId = 1 },

                    new Permission { Id = 302, Name = "Purchases", Key = "Purchases", ParentId = 30 },

                        new Permission { Id = 30201, Name = "Invoices", Key = "PurchasesInvoices.All", ParentId = 302 },
                            new Permission { Id = 3020101, Name = "View", Key = "PurchasesInvoices.View", ParentId = 30201, TypeId = 1 },
                            new Permission { Id = 3020102, Name = "Add", Key = "PurchasesInvoices.Add", ParentId = 30201, TypeId = 1 },
                            new Permission { Id = 3020103, Name = "Edit", Key = "PurchasesInvoices.Edit", ParentId = 30201, TypeId = 1 },
                            new Permission { Id = 3020104, Name = "Delete", Key = "PurchasesInvoices.Delete", ParentId = 30201, TypeId = 1 },
                            new Permission { Id = 3020105, Name = "Preference", Key = "PurchasesInvoices.Preference", ParentId = 30201, TypeId = 1 },

                        new Permission { Id = 30202, Name = "Returns", Key = "PurchasesReturns.All", ParentId = 302 },
                            new Permission { Id = 3020201, Name = "View", Key = "PurchasesReturns.View", ParentId = 30202, TypeId = 1 },
                            new Permission { Id = 3020202, Name = "Add", Key = "PurchasesReturns.Add", ParentId = 30202, TypeId = 1 },
                            new Permission { Id = 3020203, Name = "Edit", Key = "PurchasesReturns.Edit", ParentId = 30202, TypeId = 1 },
                            new Permission { Id = 3020204, Name = "Delete", Key = "PurchasesReturns.Delete", ParentId = 30202, TypeId = 1 },
                            new Permission { Id = 3020205, Name = "Preference", Key = "PurchasesReturns.Preference", ParentId = 30202, TypeId = 1 },

                    new Permission { Id = 40, Name = "Transactions", Key = "Transactions.All", ParentId = 1 },

                           new Permission { Id = 401, Name = "Transaction Notices", Key = "TransactionNotices", ParentId = 40 },

                                new Permission { Id = 40101, Name = "Addition", Key = "Addition.All", ParentId = 401 },
                                    new Permission { Id = 4010101, Name = "View", Key = "Addition.View", ParentId = 40101, TypeId = 1 },
                                    new Permission { Id = 4010102, Name = "Add", Key = "Addition.Add", ParentId = 40101, TypeId = 1 },
                                    new Permission { Id = 4010103, Name = "Edit", Key = "Addition.Edit", ParentId = 40101, TypeId = 1 },
                                    new Permission { Id = 4010104, Name = "Delete", Key = "Addition.Delete", ParentId = 40101, TypeId = 1 },
                                    new Permission { Id = 4010105, Name = "Preference", Key = "Addition.Preference", ParentId = 40101, TypeId = 1 },

                                new Permission { Id = 40102, Name = "Issue", Key = "Issue.All", ParentId = 401 },
                                    new Permission { Id = 4010201, Name = "View", Key = "Issue.View", ParentId = 40102, TypeId = 1 },
                                    new Permission { Id = 4010202, Name = "Add", Key = "Issue.Add", ParentId = 40102, TypeId = 1 },
                                    new Permission { Id = 4010203, Name = "Edit", Key = "Issue.Edit", ParentId = 40102, TypeId = 1 },
                                    new Permission { Id = 4010204, Name = "Delete", Key = "Issue.Delete", ParentId = 40102, TypeId = 1 },
                                    new Permission { Id = 4010205, Name = "Preference", Key = "Issue.Preference", ParentId = 40102, TypeId = 1 },

                                new Permission { Id = 40103, Name = "Transafer", Key = "Transafer.All", ParentId = 401 },
                                    new Permission { Id = 4010301, Name = "View", Key = "Transafer.View", ParentId = 40103, TypeId = 1 },
                                    new Permission { Id = 4010302, Name = "Add", Key = "Transafer.Add", ParentId = 40103, TypeId = 1 },
                                    new Permission { Id = 4010303, Name = "Edit", Key = "Transafer.Edit", ParentId = 40103, TypeId = 1 },
                                    new Permission { Id = 4010304, Name = "Delete", Key = "Transafer.Delete", ParentId = 40103, TypeId = 1 },
                                    new Permission { Id = 4010305, Name = "Preference", Key = "Transafer.Preference", ParentId = 40103, TypeId = 1 },

                                new Permission { Id = 40104, Name = "Received", Key = "Received.All", ParentId = 401 },
                                    new Permission { Id = 4010401, Name = "View", Key = "Received.View", ParentId = 40104, TypeId = 1 },
                                    new Permission { Id = 4010402, Name = "Add", Key = "Received.Add", ParentId = 40104, TypeId = 1 },
                                    new Permission { Id = 4010403, Name = "Edit", Key = "Received.Edit", ParentId = 40104, TypeId = 1 },
                                    new Permission { Id = 4010404, Name = "Delete", Key = "Received.Delete", ParentId = 40104, TypeId = 1 },
                                    new Permission { Id = 4010405, Name = "Preference", Key = "Received.Preference", ParentId = 40104, TypeId = 1 },

                                new Permission { Id = 40105, Name = "Inventory", Key = "Inventory.All", ParentId = 401 },
                                    new Permission { Id = 4010501, Name = "View", Key = "Inventory.View", ParentId = 40105, TypeId = 1 },
                                    new Permission { Id = 4010502, Name = "Add", Key = "Inventory.Add", ParentId = 40105, TypeId = 1 },
                                    new Permission { Id = 4010503, Name = "Edit", Key = "Inventory.Edit", ParentId = 40105, TypeId = 1 },
                                    new Permission { Id = 4010504, Name = "Delete", Key = "Inventory.Delete", ParentId = 40105, TypeId = 1 },
                                    new Permission { Id = 4010505, Name = "Preference", Key = "Inventory.Preference", ParentId = 40105, TypeId = 1 },

                        new Permission { Id = 50, Name = "Financials", Key = "Financials.All", ParentId = 1 },

                           new Permission { Id = 501, Name = "Safe Notices", Key = "SafeNotices", ParentId = 50 },

                                new Permission { Id = 50101, Name = "Collection", Key = "Collection.All", ParentId = 501 },
                                    new Permission { Id = 5010101, Name = "View", Key = "Collection.View", ParentId = 50101, TypeId = 1 },
                                    new Permission { Id = 5010102, Name = "Add", Key = "Collection.Add", ParentId = 50101, TypeId = 1 },
                                    new Permission { Id = 5010103, Name = "Edit", Key = "Collection.Edit", ParentId = 50101, TypeId = 1 },
                                    new Permission { Id = 5010104, Name = "Delete", Key = "Collection.Delete", ParentId = 50101, TypeId = 1 },
                                    new Permission { Id = 5010105, Name = "Preference", Key = "Collection.Preference", ParentId = 50101, TypeId = 1 },

                                new Permission { Id = 50102, Name = "Payment", Key = "Payment.All", ParentId = 501 },
                                    new Permission { Id = 5010201, Name = "View", Key = "Payment.View", ParentId = 50102, TypeId = 1 },
                                    new Permission { Id = 5010202, Name = "Add", Key = "Payment.Add", ParentId = 50102, TypeId = 1 },
                                    new Permission { Id = 5010203, Name = "Edit", Key = "Payment.Edit", ParentId = 50102, TypeId = 1 },
                                    new Permission { Id = 5010204, Name = "Delete", Key = "Payment.Delete", ParentId = 50102, TypeId = 1 },
                                    new Permission { Id = 5010205, Name = "Preference", Key = "Payment.Preference", ParentId = 50102, TypeId = 1 },

                                new Permission { Id = 50103, Name = "Outlay", Key = "Outlay.All", ParentId = 501 },
                                    new Permission { Id = 5010301, Name = "View", Key = "Outlay.View", ParentId = 50103, TypeId = 1 },
                                    new Permission { Id = 5010302, Name = "Add", Key = "Outlay.Add", ParentId = 50103, TypeId = 1 },
                                    new Permission { Id = 5010303, Name = "Edit", Key = "Outlay.Edit", ParentId = 50103, TypeId = 1 },
                                    new Permission { Id = 5010304, Name = "Delete", Key = "Outlay.Delete", ParentId = 50103, TypeId = 1 },
                                    new Permission { Id = 5010305, Name = "Preference", Key = "Outlay.Preference", ParentId = 50103, TypeId = 1 }
                );

            modelBuilder.Entity<Role>().HasData(new Role { Id = 1, Name = "Owner", Hide = true });
            //modelBuilder.Entity<Role>().HasData(new Role { Id = 2, Name = "Admin", Hide = false });

            modelBuilder.Entity<User>().HasData(new User { Id = 1, Name = "Owner", UserName = "Owner", Password = Security.Encrypt("OwnerAbc@123"), RoleId = 1, Hide = true });

            //modelBuilder.Entity<RolePermission>().HasData(
            //   new RolePermission { Id = 1, RoleId = 2, PermissionId = 102 },
            //   new RolePermission { Id = 2, RoleId = 2, PermissionId = 10201 },
            //   new RolePermission { Id = 3, RoleId = 2, PermissionId = 10202 },
            //   new RolePermission { Id = 4, RoleId = 2, PermissionId = 10203 },
            //   new RolePermission { Id = 5, RoleId = 2, PermissionId = 10204 },
            //   new RolePermission { Id = 6, RoleId = 2, PermissionId = 103 },
            //   new RolePermission { Id = 7, RoleId = 2, PermissionId = 10301 },
            //   new RolePermission { Id = 8, RoleId = 2, PermissionId = 10302 },
            //   new RolePermission { Id = 9, RoleId = 2, PermissionId = 10303 },
            //   new RolePermission { Id = 10, RoleId = 2, PermissionId = 10304 }


            //   );

            modelBuilder.Entity<InvoiceType>().HasData(
                new InvoiceType { Id = 1, Group = "Sales", Name = "Invoice", Hide = false, InOut = -1, Icon = "simple-icon-basket-loaded" },
                new InvoiceType { Id = 2, Group = "Purchases", Name = "Invoice", Hide = false, InOut = 1, Icon = "simple-icon-basket-loaded" },
                new InvoiceType { Id = 3, Group = "Sales", Name = "Return", Hide = false, InOut = 1, Icon = "simple-icon-action-undo" },
                new InvoiceType { Id = 4, Group = "Purchases", Name = "Return", Hide = false, InOut = -1, Icon = "simple-icon-action-undo" }
                );

            modelBuilder.Entity<OrderType>().HasData(
               new OrderType { Id = 1, Name = "Internal", Hide = false, Icon = "iconsminds-right-1" },
               new OrderType { Id = 2, Name = "External", Hide = false, Icon = "iconsminds-left-1" }
               );

            modelBuilder.Entity<TransactionType>().HasData(
              new TransactionType { Id = 1, Name = "Addition", Hide = false, InOut = 1, Icon = "iconsminds-down-1" },
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
               new Preference { Id = 1205, Key = "TypeSerial", Value = "1", Reference = "Financial", TypeId = 3, Hide = false },

               new Preference { Id = 1300, Key = "DefaultStore", Value = "1", Reference = "Inventory", TypeId = 0, Hide = false },
               new Preference { Id = 1301, Key = "AutoSave", Value = "0", Reference = "Inventory", TypeId = 0, Hide = false },
               new Preference { Id = 1302, Key = "TypeSerial", Value = "1", Reference = "Inventory", TypeId = 0, Hide = false }

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
        public virtual DbSet<RolePermission> RolePermissions { get; set; }
    }
}