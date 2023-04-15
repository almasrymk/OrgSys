using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace Repository.Seed
{
    public class InitialData
    {
        string _Schema = "org";

        public InitialData(string Schema)
        {
            this._Schema = Schema;
        }

        public async Task Run()
        {
            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfigurationRoot config = builder.Build();
            var DatabaseVersion = config.GetSection("DatabaseVersion")?.Value ?? "0";
            AdminContext adminContext = new AdminContext(new DbContextOptions<AdminContext>());
            var admin = adminContext.Clients.FirstOrDefault(e => e.DbSchema == _Schema);
            if (admin != null && admin.Id > 0 && "" + (admin?.VersionDb ?? 0) != DatabaseVersion)
            {
                admin.VersionDb = long.Parse("0" + DatabaseVersion);
                OrgContext orgContext = new OrgContext(new DbContextOptions<OrgContext>(), _Schema);
                await orgContext.Database.EnsureCreatedAsync().ConfigureAwait(false);
                await orgContext.Database.MigrateAsync().ConfigureAwait(false);

                InitialPermission(orgContext);
                InitialPreference(orgContext);
                InitialOrderType(orgContext);
                InitialInvoiceType(orgContext);
                InitialTransactionType(orgContext);
                InitialFinancialType(orgContext);
                InitialPaymentType(orgContext);
                InitialRole(orgContext);
                InitialRolePermission(orgContext);
                InitialUser(orgContext);
                InitialCompanyProfile(orgContext);
                InitialBranch(orgContext);
                InitialStock(orgContext);
                InitialDealerGroup(orgContext);
                InitialDealer(orgContext);
                InitialSafe(orgContext);
                InitialCurrency(orgContext);
                adminContext.SaveChanges();
            }
        }

        public void InitialPermission(OrgContext orgContext)
        {
            List<Permission> list = new List<Permission> {
               new Permission { Id = 1, Name = "Organizer", Key = "Organizer", ParentId = 0 },
                   new Permission { Id = 10, Name = "Data", Key = "Data.All", ParentId = 1 },

                     new Permission { Id = 101, Name = "Organization", Key = "Organization", ParentId = 10 },

                       new Permission { Id = 10101, Name = "Branchs", Key = "Branchs.All", ParentId = 101 },
                           new Permission { Id = 1010101, Name = "View", Key = "Branchs.View", ParentId = 10101, TypeId = 1 },
                           new Permission { Id = 1010102, Name = "Add", Key = "Branchs.Add", ParentId = 10101, TypeId = 1 },
                           new Permission { Id = 1010103, Name = "Edit", Key = "Branchs.Edit", ParentId = 10101, TypeId = 1 },
                           new Permission { Id = 1010104, Name = "Delete", Key = "Branchs.Delete", ParentId = 10101, TypeId = 1 },

                       new Permission { Id = 10102, Name = "Stocks", Key = "Stocks.All", ParentId = 101 },
                           new Permission { Id = 1010201, Name = "View", Key = "Stocks.View", ParentId = 10102, TypeId = 1 },
                           new Permission { Id = 1010202, Name = "Add", Key = "Stocks.Add", ParentId = 10102, TypeId = 1 },
                           new Permission { Id = 1010203, Name = "Edit", Key = "Stocks.Edit", ParentId = 10102, TypeId = 1 },
                           new Permission { Id = 1010204, Name = "Delete", Key = "Stocks.Delete", ParentId = 10102, TypeId = 1 },

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

                       new Permission { Id = 10501, Name = "Accounts", Key = "Accounts.All", ParentId = 105 },
                           new Permission { Id = 1050101, Name = "View", Key = "Accounts.View", ParentId = 10501, TypeId = 1 },
                           new Permission { Id = 1050102, Name = "Add", Key = "Accounts.Add", ParentId = 10501, TypeId = 1 },
                           new Permission { Id = 1050103, Name = "Edit", Key = "Accounts.Edit", ParentId = 10501, TypeId = 1 },
                           new Permission { Id = 1050104, Name = "Delete", Key = "Accounts.Delete", ParentId = 10501, TypeId = 1 },

                       new Permission { Id = 10502, Name = "Safes", Key = "Safes.All", ParentId = 105 },
                           new Permission { Id = 1050201, Name = "View", Key = "Safes.View", ParentId = 10502, TypeId = 1 },
                           new Permission { Id = 1050202, Name = "Add", Key = "Safes.Add", ParentId = 10502, TypeId = 1 },
                           new Permission { Id = 1050203, Name = "Edit", Key = "Safes.Edit", ParentId = 10502, TypeId = 1 },
                           new Permission { Id = 1050204, Name = "Delete", Key = "Safes.Delete", ParentId = 10502, TypeId = 1 },

                       new Permission { Id = 10503, Name = "Banks", Key = "Banks.All", ParentId = 105 },
                           new Permission { Id = 1050301, Name = "View", Key = "Banks.View", ParentId = 10503, TypeId = 1 },
                           new Permission { Id = 1050302, Name = "Add", Key = "Banks.Add", ParentId = 10503, TypeId = 1 },
                           new Permission { Id = 1050303, Name = "Edit", Key = "Banks.Edit", ParentId = 10503, TypeId = 1 },
                           new Permission { Id = 1050304, Name = "Delete", Key = "Banks.Delete", ParentId = 10503, TypeId = 1 },

                       new Permission { Id = 10504, Name = "Bank branchs", Key = "BankBranchs.All", ParentId = 105 },
                           new Permission { Id = 1050401, Name = "View", Key = "BankBranchs.View", ParentId = 10504, TypeId = 1 },
                           new Permission { Id = 1050402, Name = "Add", Key = "BankBranchs.Add", ParentId = 10504, TypeId = 1 },
                           new Permission { Id = 1050403, Name = "Edit", Key = "BankBranchs.Edit", ParentId = 10504, TypeId = 1 },
                           new Permission { Id = 1050404, Name = "Delete", Key = "BankBranchs.Delete", ParentId = 10504, TypeId = 1 },

                       new Permission { Id = 10505, Name = "Accounts of bank", Key = "Accountbanks.All", ParentId = 105 },
                           new Permission { Id = 1050501, Name = "View", Key = "Accountbanks.View", ParentId = 10505, TypeId = 1 },
                           new Permission { Id = 1050502, Name = "Add", Key = "Accountbanks.Add", ParentId = 10505, TypeId = 1 },
                           new Permission { Id = 1050503, Name = "Edit", Key = "Accountbanks.Edit", ParentId = 10505, TypeId = 1 },
                           new Permission { Id = 1050504, Name = "Delete", Key = "Accountbanks.Delete", ParentId = 10505, TypeId = 1 },

                       new Permission { Id = 10506, Name = "Safes", Key = "Safes.All", ParentId = 105 },
                           new Permission { Id = 1050601, Name = "View", Key = "Safes.View", ParentId = 10506, TypeId = 1 },
                           new Permission { Id = 1050602, Name = "Add", Key = "Safes.Add", ParentId = 10506, TypeId = 1 },
                           new Permission { Id = 1050603, Name = "Edit", Key = "Safes.Edit", ParentId = 10506, TypeId = 1 },
                           new Permission { Id = 1050604, Name = "Delete", Key = "Safes.Delete", ParentId = 10506, TypeId = 1 },

                       new Permission { Id = 10507, Name = "Outlay Terms", Key = "OutlayTerms.All", ParentId = 105 },
                           new Permission { Id = 1050701, Name = "View", Key = "OutlayTerms.View", ParentId = 10507, TypeId = 1 },
                           new Permission { Id = 1050702, Name = "Add", Key = "OutlayTerms.Add", ParentId = 10507, TypeId = 1 },
                           new Permission { Id = 1050703, Name = "Edit", Key = "OutlayTerms.Edit", ParentId = 10507, TypeId = 1 },
                           new Permission { Id = 1050704, Name = "Delete", Key = "OutlayTerms.Delete", ParentId = 10507, TypeId = 1 },

                       new Permission { Id = 10508, Name = "Currencies", Key = "Currencies.All", ParentId = 105 },
                           new Permission { Id = 1050801, Name = "View", Key = "Currencies.View", ParentId = 10508, TypeId = 1 },
                           new Permission { Id = 1050802, Name = "Add", Key = "Currencies.Add", ParentId = 10508, TypeId = 1 },
                           new Permission { Id = 1050803, Name = "Edit", Key = "Currencies.Edit", ParentId = 10508, TypeId = 1 },
                           new Permission { Id = 1050804, Name = "Delete", Key = "Currencies.Delete", ParentId = 10508, TypeId = 1 },

                   new Permission { Id = 106, Name = "Areas", Key = "Areas", ParentId = 10 },

                       new Permission { Id = 10601, Name = "Countries", Key = "Countrys.All", ParentId = 106 },
                           new Permission { Id = 1060101, Name = "View", Key = "Countrys.View", ParentId = 10601, TypeId = 1 },
                           new Permission { Id = 1060102, Name = "Add", Key = "Countrys.Add", ParentId = 10601, TypeId = 1 },
                           new Permission { Id = 1060103, Name = "Edit", Key = "Countrys.Edit", ParentId = 10601, TypeId = 1 },
                           new Permission { Id = 1060104, Name = "Delete", Key = "Countrys.Delete", ParentId = 10601, TypeId = 1 },

                       new Permission { Id = 10602, Name = "Cities", Key = "Citys.All", ParentId = 106 },
                           new Permission { Id = 1060201, Name = "View", Key = "Citys.View", ParentId = 10602, TypeId = 1 },
                           new Permission { Id = 1060202, Name = "Add", Key = "Citys.Add", ParentId = 10602, TypeId = 1 },
                           new Permission { Id = 1060203, Name = "Edit", Key = "Citys.Edit", ParentId = 10602, TypeId = 1 },
                           new Permission { Id = 1060204, Name = "Delete", Key = "Citys.Delete", ParentId = 10602, TypeId = 1 },

                       new Permission { Id = 10603, Name = "Districts", Key = "Districts.All", ParentId = 106 },
                           new Permission { Id = 1060301, Name = "View", Key = "Districts.View", ParentId = 10603, TypeId = 1 },
                           new Permission { Id = 1060302, Name = "Add", Key = "Districts.Add", ParentId = 10603, TypeId = 1 },
                           new Permission { Id = 1060303, Name = "Edit", Key = "Districts.Edit", ParentId = 10603, TypeId = 1 },
                           new Permission { Id = 1060304, Name = "Delete", Key = "Districts.Delete", ParentId = 10603, TypeId = 1 },

               new Permission { Id = 20, Name = "Orders", Key = "Orders.All", ParentId = 1 },

                  new Permission { Id = 201, Name = "Order Notices", Key = "Orders", ParentId = 20 },

                       new Permission { Id = 20101, Name = "Internal", Key = "Internal.All", ParentId = 201 },
                           new Permission { Id = 2010101, Name = "View", Key = "Internal.View", ParentId = 20101, TypeId = 1 },
                           new Permission { Id = 2010102, Name = "Add", Key = "Internal.Add", ParentId = 20101, TypeId = 1 },
                           new Permission { Id = 2010103, Name = "Edit", Key = "Internal.Edit", ParentId = 20101, TypeId = 1 },
                           new Permission { Id = 2010104, Name = "Delete", Key = "Internal.Delete", ParentId = 20101, TypeId = 1 },
                           new Permission { Id = 2010105, Name = "Cancel", Key = "Internal.Cancel", ParentId = 20101, TypeId = 1 },
                           new Permission { Id = 2010106, Name = "Preference", Key = "Internal.Preference", ParentId = 20101, TypeId = 1 },

                       new Permission { Id = 20102, Name = "External", Key = "External.All", ParentId = 201 },
                           new Permission { Id = 2010201, Name = "View", Key = "External.View", ParentId = 20102, TypeId = 1 },
                           new Permission { Id = 2010202, Name = "Add", Key = "External.Add", ParentId = 20102, TypeId = 1 },
                           new Permission { Id = 2010203, Name = "Edit", Key = "External.Edit", ParentId = 20102, TypeId = 1 },
                           new Permission { Id = 2010204, Name = "Delete", Key = "External.Delete", ParentId = 20102, TypeId = 1 },
                           new Permission { Id = 2010205, Name = "Cancel", Key = "External.Cancel", ParentId = 20102, TypeId = 1 },
                           new Permission { Id = 2010206, Name = "Preference", Key = "External.Preference", ParentId = 20102, TypeId = 1 },

               new Permission { Id = 30, Name = "Invoices", Key = "Invoices.All", ParentId = 1 },

                  new Permission { Id = 301, Name = "Sales", Key = "Sales", ParentId = 30 },

                       new Permission { Id = 30101, Name = "Invoices", Key = "SalesInvoices.All", ParentId = 301 },
                           new Permission { Id = 3010101, Name = "View", Key = "SalesInvoices.View", ParentId = 30101, TypeId = 1 },
                           new Permission { Id = 3010102, Name = "Add", Key = "SalesInvoices.Add", ParentId = 30101, TypeId = 1 },
                           new Permission { Id = 3010103, Name = "Edit", Key = "SalesInvoices.Edit", ParentId = 30101, TypeId = 1 },
                           new Permission { Id = 3010104, Name = "Delete", Key = "SalesInvoices.Delete", ParentId = 30101, TypeId = 1 },
                           new Permission { Id = 3010105, Name = "Cancel", Key = "SalesInvoices.Cancel", ParentId = 30101, TypeId = 1 },
                           new Permission { Id = 3010106, Name = "Preference", Key = "SalesInvoices.Preference", ParentId = 30101, TypeId = 1 },

                       new Permission { Id = 30102, Name = "Returns", Key = "SalesReturns.All", ParentId = 301 },
                           new Permission { Id = 3010201, Name = "View", Key = "SalesReturns.View", ParentId = 30102, TypeId = 1 },
                           new Permission { Id = 3010202, Name = "Add", Key = "SalesReturns.Add", ParentId = 30102, TypeId = 1 },
                           new Permission { Id = 3010203, Name = "Edit", Key = "SalesReturns.Edit", ParentId = 30102, TypeId = 1 },
                           new Permission { Id = 3010204, Name = "Delete", Key = "SalesReturns.Delete", ParentId = 30102, TypeId = 1 },
                           new Permission { Id = 3010205, Name = "Cancel", Key = "SalesReturns.Cancel", ParentId = 30102, TypeId = 1 },
                           new Permission { Id = 3010206, Name = "Preference", Key = "SalesReturns.Preference", ParentId = 30102, TypeId = 1 },

                   new Permission { Id = 302, Name = "Purchases", Key = "Purchases", ParentId = 30 },

                       new Permission { Id = 30201, Name = "Invoices", Key = "PurchasesInvoices.All", ParentId = 302 },
                           new Permission { Id = 3020101, Name = "View", Key = "PurchasesInvoices.View", ParentId = 30201, TypeId = 1 },
                           new Permission { Id = 3020102, Name = "Add", Key = "PurchasesInvoices.Add", ParentId = 30201, TypeId = 1 },
                           new Permission { Id = 3020103, Name = "Edit", Key = "PurchasesInvoices.Edit", ParentId = 30201, TypeId = 1 },
                           new Permission { Id = 3020104, Name = "Delete", Key = "PurchasesInvoices.Delete", ParentId = 30201, TypeId = 1 },
                           new Permission { Id = 3020105, Name = "Cancel", Key = "PurchasesInvoices.Cancel", ParentId = 30201, TypeId = 1 },
                           new Permission { Id = 3020106, Name = "Preference", Key = "PurchasesInvoices.Preference", ParentId = 30201, TypeId = 1 },

                       new Permission { Id = 30202, Name = "Returns", Key = "PurchasesReturns.All", ParentId = 302 },
                           new Permission { Id = 3020201, Name = "View", Key = "PurchasesReturns.View", ParentId = 30202, TypeId = 1 },
                           new Permission { Id = 3020202, Name = "Add", Key = "PurchasesReturns.Add", ParentId = 30202, TypeId = 1 },
                           new Permission { Id = 3020203, Name = "Edit", Key = "PurchasesReturns.Edit", ParentId = 30202, TypeId = 1 },
                           new Permission { Id = 3020204, Name = "Delete", Key = "PurchasesReturns.Delete", ParentId = 30202, TypeId = 1 },
                           new Permission { Id = 3020205, Name = "Cancel", Key = "PurchasesReturns.Cancel", ParentId = 30202, TypeId = 1 },
                           new Permission { Id = 3020206, Name = "Preference", Key = "PurchasesReturns.Preference", ParentId = 30202, TypeId = 1 },

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

                               new Permission { Id = 50103, Name = "Outlays", Key = "Outlay.All", ParentId = 501 },
                                   new Permission { Id = 5010301, Name = "View", Key = "Outlay.View", ParentId = 50103, TypeId = 1 },
                                   new Permission { Id = 5010302, Name = "Add", Key = "Outlay.Add", ParentId = 50103, TypeId = 1 },
                                   new Permission { Id = 5010303, Name = "Edit", Key = "Outlay.Edit", ParentId = 50103, TypeId = 1 },
                                   new Permission { Id = 5010304, Name = "Delete", Key = "Outlay.Delete", ParentId = 50103, TypeId = 1 },
                                   new Permission { Id = 5010305, Name = "Preference", Key = "Outlay.Preference", ParentId = 50103, TypeId = 1 },

                              new Permission { Id = 50105, Name = "Create journal accounts", Key = "CreateJournalAccounts.All", ParentId = 501 },
                                   new Permission { Id = 5010501, Name = "View", Key = "CreateJournalAccounts.View", ParentId = 50105, TypeId = 1 },
                                   new Permission { Id = 5010502, Name = "Add", Key = "CreateJournalAccounts.Add", ParentId = 50105, TypeId = 1 },
                                   new Permission { Id = 5010503, Name = "Edit", Key = "CreateJournalAccounts.Edit", ParentId = 50105, TypeId = 1 },
                                   new Permission { Id = 5010504, Name = "Delete", Key = "CreateJournalAccounts.Delete", ParentId = 50105, TypeId = 1 },
                                   new Permission { Id = 5010505, Name = "Preference", Key = "CreateJournalAccounts.Preference", ParentId = 50105, TypeId = 1 },

                               new Permission { Id = 50106, Name = "Journals", Key = "Journals.All", ParentId = 501 },
                                   new Permission { Id = 5010601, Name = "View", Key = "Journals.View", ParentId = 50106, TypeId = 1 },
                                   new Permission { Id = 5010602, Name = "Add", Key = "Journals.Add", ParentId = 50106, TypeId = 1 },
                                   new Permission { Id = 5010603, Name = "Edit", Key = "Journals.Edit", ParentId = 50106, TypeId = 1 },
                                   new Permission { Id = 5010604, Name = "Delete", Key = "Journals.Delete", ParentId = 50106, TypeId = 1 },
                                   new Permission { Id = 5010605, Name = "Preference", Key = "Journals.Preference", ParentId = 50106, TypeId = 1 }
               };
            foreach (var ob in list)
            {
                if (!orgContext.Permissions.Any(e => e.Id == ob.Id))
                    orgContext.Set<Permission>().Add(ob);
                else
                    orgContext.Entry<Permission>(orgContext.Set<Permission>().Find(ob.Id)).CurrentValues.SetValues(ob);
            }
            orgContext.SaveChanges();
        }

        public void InitialPreference(OrgContext orgContext)
        {
            List<Preference> list = new List<Preference> {
               new Preference { Id = 1, Key = "DefaultStock", Value = "1", Reference = "Invoice", TypeId = 1, Hide = false },
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
               new Preference { Id = 17, Key = "DefaultCurrency", Value = "1", Reference = "Invoice", TypeId = 1, Hide = false },
               new Preference { Id = 18, Key = "CodeElectronicScale", Value = "009", Reference = "Invoice", TypeId = 1, Hide = false },
               new Preference { Id = 19, Key = "LengthElectronicScale", Value = "7", Reference = "Invoice", TypeId = 1, Hide = false },
               new Preference { Id = 20, Key = "LengthQtyElectronicScale", Value = "5", Reference = "Invoice", TypeId = 1, Hide = false },

               new Preference { Id = 101, Key = "DefaultStock", Value = "1", Reference = "Invoice", TypeId = 2, Hide = false },
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
               new Preference { Id = 117, Key = "DefaultCurrency", Value = "1", Reference = "Invoice", TypeId = 2, Hide = false },
               new Preference { Id = 118, Key = "CodeElectronicScale", Value = "009", Reference = "Invoice", TypeId = 2, Hide = false },
               new Preference { Id = 119, Key = "LengthElectronicScale", Value = "7", Reference = "Invoice", TypeId = 2, Hide = false },
               new Preference { Id = 120, Key = "LengthQtyElectronicScale", Value = "5", Reference = "Invoice", TypeId = 2, Hide = false },

               new Preference { Id = 201, Key = "DefaultStock", Value = "1", Reference = "Invoice", TypeId = 3, Hide = false },
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
               new Preference { Id = 216, Key = "AutoCreateTransaction", Value = "0", Reference = "Invoice", TypeId = 3, Hide = false },
               new Preference { Id = 217, Key = "DefaultCurrency", Value = "1", Reference = "Invoice", TypeId = 3, Hide = false },

               new Preference { Id = 301, Key = "DefaultStock", Value = "1", Reference = "Invoice", TypeId = 4, Hide = false },
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
               new Preference { Id = 317, Key = "DefaultCurrency", Value = "1", Reference = "Invoice", TypeId = 4, Hide = false },

               new Preference { Id = 401, Key = "DefaultStock", Value = "1", Reference = "Transaction", TypeId = 1, Hide = false },
               new Preference { Id = 402, Key = "DefaultSupplier", Value = "1", Reference = "Transaction", TypeId = 1, Hide = false },
               new Preference { Id = 403, Key = "NumberLine", Value = "6", Reference = "Transaction", TypeId = 1, Hide = false },
               new Preference { Id = 404, Key = "OrderTabe", Value = "2", Reference = "Transaction", TypeId = 1, Hide = false },
               new Preference { Id = 405, Key = "AutoSave", Value = "0", Reference = "Transaction", TypeId = 1, Hide = false },
               new Preference { Id = 406, Key = "TypeSerial", Value = "1", Reference = "Transaction", TypeId = 1, Hide = false },
               new Preference { Id = 407, Key = "AllowRepeated", Value = "1", Reference = "Transaction", TypeId = 1, Hide = false },
               new Preference { Id = 408, Key = "SaveLastStatusSetting", Value = "1", Reference = "Transaction", TypeId = 1, Hide = false },

               new Preference { Id = 501, Key = "DefaultStock", Value = "1", Reference = "Transaction", TypeId = 2, Hide = false },
               new Preference { Id = 502, Key = "DefaultCustomer", Value = "1", Reference = "Transaction", TypeId = 2, Hide = false },
               new Preference { Id = 503, Key = "NumberLine", Value = "6", Reference = "Transaction", TypeId = 2, Hide = false },
               new Preference { Id = 504, Key = "OrderTabe", Value = "2", Reference = "Transaction", TypeId = 2, Hide = false },
               new Preference { Id = 505, Key = "AutoSave", Value = "0", Reference = "Transaction", TypeId = 2, Hide = false },
               new Preference { Id = 506, Key = "TypeSerial", Value = "1", Reference = "Transaction", TypeId = 2, Hide = false },
               new Preference { Id = 507, Key = "AllowRepeated", Value = "1", Reference = "Transaction", TypeId = 2, Hide = false },
               new Preference { Id = 508, Key = "SaveLastStatusSetting", Value = "1", Reference = "Transaction", TypeId = 2, Hide = false },

               new Preference { Id = 601, Key = "DefaultStock", Value = "1", Reference = "Transaction", TypeId = 3, Hide = false },
               new Preference { Id = 602, Key = "NumberLine", Value = "6", Reference = "Transaction", TypeId = 3, Hide = false },
               new Preference { Id = 603, Key = "OrderTabe", Value = "2", Reference = "Transaction", TypeId = 3, Hide = false },
               new Preference { Id = 604, Key = "AutoSave", Value = "0", Reference = "Transaction", TypeId = 3, Hide = false },
               new Preference { Id = 605, Key = "TypeSerial", Value = "1", Reference = "Transaction", TypeId = 3, Hide = false },
               new Preference { Id = 606, Key = "AllowRepeated", Value = "1", Reference = "Transaction", TypeId = 3, Hide = false },
               new Preference { Id = 607, Key = "SaveLastStatusSetting", Value = "1", Reference = "Transaction", TypeId = 3, Hide = false },
               new Preference { Id = 608, Key = "AutoReceived", Value = "0", Reference = "Transaction", TypeId = 3, Hide = false },

               new Preference { Id = 701, Key = "DefaultStock", Value = "1", Reference = "Transaction", TypeId = 4, Hide = false },
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

               new Preference { Id = 1300, Key = "DefaultStock", Value = "1", Reference = "Inventory", TypeId = 0, Hide = false },
               new Preference { Id = 1301, Key = "AutoSave", Value = "0", Reference = "Inventory", TypeId = 0, Hide = false },
               new Preference { Id = 1302, Key = "TypeSerial", Value = "1", Reference = "Inventory", TypeId = 0, Hide = false }
            };

            foreach (var ob in list)
            {
                if (!orgContext.Preferences.Any(e => e.Id == ob.Id))
                    orgContext.Set<Preference>().Add(ob);
                else
                    orgContext.Entry<Preference>(orgContext.Set<Preference>().Find(ob.Id)).CurrentValues.SetValues(ob);
            }
            orgContext.SaveChanges();
        }

        public void InitialOrderType(OrgContext orgContext)
        {
            List<OrderType> list = new List<OrderType> {
                   new OrderType { Id = 1, Name = "Internal", Hide = false, Icon = "iconsminds-right-1" },
                   new OrderType { Id = 2, Name = "External", Hide = false, Icon = "iconsminds-left-1" }
            };

            foreach (var ob in list)
            {
                if (!orgContext.OrderTypes.Any(e => e.Id == ob.Id))
                    orgContext.Set<OrderType>().Add(ob);
                else
                    orgContext.Entry<OrderType>(orgContext.Set<OrderType>().Find(ob.Id)).CurrentValues.SetValues(ob);
            }
            orgContext.SaveChanges();
        }

        public void InitialInvoiceType(OrgContext orgContext)
        {
            List<InvoiceType> list = new List<InvoiceType> {
                    new InvoiceType { Id = 1, Group = "Sales", Name = "Invoice", Hide = false, InOut = 1, Icon = "simple-icon-basket-loaded" },
                    new InvoiceType { Id = 2, Group = "Purchases", Name = "Invoice", Hide = false, InOut = 1, Icon = "simple-icon-basket-loaded" },
                    new InvoiceType { Id = 3, Group = "Sales", Name = "Return", Hide = false, InOut = -1, Icon = "simple-icon-action-undo" },
                    new InvoiceType { Id = 4, Group = "Purchases", Name = "Return", Hide = false, InOut = -1, Icon = "simple-icon-action-undo" }
            };

            foreach (var ob in list)
            {
                if (!orgContext.InvoiceTypes.Any(e => e.Id == ob.Id))
                    orgContext.Set<InvoiceType>().Add(ob);
                else
                    orgContext.Entry<InvoiceType>(orgContext.Set<InvoiceType>().Find(ob.Id)).CurrentValues.SetValues(ob);
            }
            orgContext.SaveChanges();
        }

        public void InitialTransactionType(OrgContext orgContext)
        {
            List<TransactionType> list = new List<TransactionType> {
                  new TransactionType { Id = 1, Name = "Addition", Hide = false, InOut = 1, Icon = "iconsminds-down-1" },
                  new TransactionType { Id = 2, Name = "Issue", Hide = false, InOut = -1, Icon = "iconsminds-up-1" },
                  new TransactionType { Id = 3, Name = "Transafer", Hide = false, InOut = -1, Icon = "iconsminds-shuffle-1" },
                  new TransactionType { Id = 4, Name = "Received", Hide = false, InOut = 1, Icon = "iconsminds-file-edit" },
                  new TransactionType { Id = 5, Name = "Adjustment In", Hide = false, InOut = 1, Icon = "" },
                  new TransactionType { Id = 6, Name = "Adjustment Out", Hide = false, InOut = -1, Icon = "" }
            };

            foreach (var ob in list)
            {
                if (!orgContext.TransactionTypes.Any(e => e.Id == ob.Id))
                    orgContext.Set<TransactionType>().Add(ob);
                else
                    orgContext.Entry<TransactionType>(orgContext.Set<TransactionType>().Find(ob.Id)).CurrentValues.SetValues(ob);
            }
            orgContext.SaveChanges();
        }

        public void InitialFinancialType(OrgContext orgContext)
        {
            List<FinancialType> list = new List<FinancialType> {
                 new FinancialType { Id = 1, Name = "Collection", Hide = false, InOut = 1, Icon = "iconsminds-financial" },
                 new FinancialType { Id = 2, Name = "Payment", Hide = false, InOut = -1, Icon = "iconsminds-handshake" },
                 new FinancialType { Id = 3, Name = "Outlay", Hide = false, InOut = -1, Icon = "iconsminds-wallet" }
            };

            foreach (var ob in list)
            {
                if (!orgContext.FinancialTypes.Any(e => e.Id == ob.Id))
                    orgContext.Set<FinancialType>().Add(ob);
                else
                    orgContext.Entry<FinancialType>(orgContext.Set<FinancialType>().Find(ob.Id)).CurrentValues.SetValues(ob);
            }
            orgContext.SaveChanges();
        }

        public void InitialPaymentType(OrgContext orgContext)
        {
            List<PaymentType> list = new List<PaymentType> {
                  new PaymentType { Id = 1, Name = "Cash", Hide = false },
                  new PaymentType { Id = 2, Name = "Check", Hide = false }
            };

            foreach (var ob in list)
            {
                if (!orgContext.PaymentTypes.Any(e => e.Id == ob.Id))
                    orgContext.Set<PaymentType>().Add(ob);
                else
                    orgContext.Entry<PaymentType>(orgContext.Set<PaymentType>().Find(ob.Id)).CurrentValues.SetValues(ob);
            }
            orgContext.SaveChanges();
        }

        public void InitialRole(OrgContext orgContext)
        {
            List<Role> list = new List<Role> {
                new Role {Name = "Owner", Hide = true },
                new Role {Name = "Admin", Hide = false }
            };

            foreach (var ob in list)
            {
                ob.Id = orgContext.Roles.FirstOrDefault(e => e.Name == ob.Name)?.Id ?? 0;
                if (ob.Id == 0)
                    orgContext.Set<Role>().Add(ob);
                else
                {
                    orgContext.Entry<Role>(orgContext.Set<Role>().Find(ob.Id)).CurrentValues.SetValues(ob);
                }
            }
            orgContext.SaveChanges();
        }

        public void InitialRolePermission(OrgContext orgContext)
        {
            List<RolePermission> list = new List<RolePermission> {
                   new RolePermission {RoleId = 2, PermissionId = 102 },
                   new RolePermission {RoleId = 2, PermissionId = 10201 },
                   new RolePermission {RoleId = 2, PermissionId = 1020101 },
                   new RolePermission {RoleId = 2, PermissionId = 1020102 },
                   new RolePermission {RoleId = 2, PermissionId = 1020103 },
                   new RolePermission {RoleId = 2, PermissionId = 1020104 },
                   new RolePermission {RoleId = 2, PermissionId = 10202 },
                   new RolePermission {RoleId = 2, PermissionId = 1020201 },
                   new RolePermission {RoleId = 2, PermissionId = 1020202 },
                   new RolePermission {RoleId = 2, PermissionId = 1020203 },
                   new RolePermission {RoleId = 2, PermissionId = 1020204 }
            };

            foreach (var ob in list)
            {
                ob.Id = orgContext.RolePermissions.FirstOrDefault(e => e.RoleId == ob.RoleId && e.PermissionId == ob.PermissionId)?.Id ?? 0;
                if (ob.Id == 0)
                    orgContext.Set<RolePermission>().AddRange(ob);
                else
                    orgContext.Entry<RolePermission>(orgContext.Set<RolePermission>().Find(ob.Id)).CurrentValues.SetValues(ob);
            }
            orgContext.SaveChanges();
        }

        public void InitialUser(OrgContext orgContext)
        {
            List<User> list = new List<User> {
                 new User {Name = "Owner", UserName = "Owner", Password = Security.Encrypt("P@ssw0rd"), RoleId = orgContext.Roles.FirstOrDefault(e => e.Name == "Owner").Id, LoginUserId = 1, Hide = true },
                 new User {Name = "Admin", UserName = "Admin", Password = Security.Encrypt("P@ssw0rd"), RoleId = orgContext.Roles.FirstOrDefault(e => e.Name == "Admin").Id, LoginUserId = 2, Hide = false },
                 new User {Name = "Emp", UserName = "Admin2", RoleId = 2, Hide = false }
            };

            foreach (var ob in list)
            {
                ob.Id = orgContext.Users.FirstOrDefault(e => e.Name == ob.Name)?.Id ?? 0;
                if (ob.Id == 0)
                    orgContext.Set<User>().Add(ob);
                else
                    orgContext.Entry<User>(orgContext.Set<User>().Find(ob.Id)).CurrentValues.SetValues(ob);
            }
            orgContext.SaveChanges();
        }

        public void InitialCompanyProfile(OrgContext orgContext)
        {
            List<CompanyProfile> list = new List<CompanyProfile> {
                 new CompanyProfile {ClientId = 1, Name = "Owner", Code = "1", CodeNumber = 1, Email1 = "info@org.com", Mobile1 = "0201111105784", Phone1 = "0201111105784", NationalityId = 68, SizeOfCompany = 1, Hide = true }
            };

            foreach (var ob in list)
            {
                ob.Id = orgContext.CompanyProfiles.FirstOrDefault(e => e.Name == ob.Name)?.Id ?? 0;
                if (ob.Id == 0)
                    orgContext.Set<CompanyProfile>().AddRange(ob);
                else
                    orgContext.Entry<CompanyProfile>(orgContext.Set<CompanyProfile>().Find(ob.Id)).CurrentValues.SetValues(ob);
            }
            orgContext.SaveChanges();
        }

        public void InitialBranch(OrgContext orgContext)
        {
            List<Branch> list = new List<Branch> {
                 new Branch {Name = "Main Branch", Hide = false }
            };

            if (!orgContext.Branches.Any())
                orgContext.Set<Branch>().AddRange(list);
            orgContext.SaveChanges();
        }

        public void InitialStock(OrgContext orgContext)
        {
            List<Stock> list = new List<Stock> {
                 new Stock {Name = "Main Stock", BranchId = 1, Hide = false }
            };

            if (!orgContext.Stocks.Any())
                orgContext.Set<Stock>().AddRange(list);
            orgContext.SaveChanges();
        }

        public void InitialDealerGroup(OrgContext orgContext)
        {
            List<DealerGroup> list = new List<DealerGroup> {
                 new DealerGroup {CodeNumber = 1, Name = "Group 1", TypeId = 1, Hide = false },
                 new DealerGroup {CodeNumber = 1, Name = "Group 1", TypeId = 1, Hide = false }
            };

            if (!orgContext.DealerGroups.Any())
                orgContext.Set<DealerGroup>().AddRange(list);
            orgContext.SaveChanges();
        }

        public void InitialDealer(OrgContext orgContext)
        {
            List<Dealer> list = new List<Dealer> {
                 new Dealer { Code = "1", CodeNumber = 1, Name = "...", TypeId = 0 , Hide = false }
            };

            if (!orgContext.Dealers.Any())
                orgContext.Set<Dealer>().AddRange(list);
            orgContext.SaveChanges();
        }

        public void InitialSafe(OrgContext orgContext)
        {
            List<Safe> list = new List<Safe> {
                 new Safe { Name = "Main Safe", Hide = false }
            };

            if (!orgContext.Safes.Any())
                orgContext.Set<Safe>().AddRange(list);
            orgContext.SaveChanges();
        }

        public void InitialCurrency(OrgContext orgContext)
        {
            List<Currency> list = new List<Currency> {
                 new Currency { Name = "Epg", Hide = false }
            };

            if (!orgContext.Currencys.Any())
                orgContext.Set<Currency>().AddRange(list);
            orgContext.SaveChanges();
        }

        public void InitialShift(OrgContext orgContext)
        {
            List<Shift> list = new List<Shift> {
                 new Shift { Code = "1" , CodeNumber = 1 , Name = "Full Time" , Start = new TimeSpan(9,0,0) , End = new TimeSpan(17,0,0), Hide = false },
                 new Shift { Code = "2" , CodeNumber = 2 , Name = "Over Time" , Start = new TimeSpan(18,0,0) , End = new TimeSpan(8,0,0), Hide = false }
            };

            if (!orgContext.Shifts.Any())
                orgContext.Set<Shift>().AddRange(list);
            orgContext.SaveChanges();
        }
    }
}