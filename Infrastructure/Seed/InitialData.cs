using Domain.Entities;
using Domain.Shared;
using Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Seed
{
    public class InitialData
    {
        public async Task Run()
        {
            await using var orgContext = new OrgContext(new DbContextOptions<OrgContext>());
            await orgContext.Database.MigrateAsync().ConfigureAwait(false);
        }

        public void Seed(OrgContext orgContext)
        {
            InitialPermission(orgContext);
            InitialPreference(orgContext);
            InitialOrderType(orgContext);
            InitialJournalType(orgContext);
            InitialInvoiceType(orgContext);
            InitialTransactionType(orgContext);
            InitialFinancialType(orgContext);
            InitialPaymentType(orgContext);
            InitialAccountType(orgContext);
            InitialAccount(orgContext);
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
            InitialCountry(orgContext);
            InitialCity(orgContext);
            InitialDistrict(orgContext);
            InitialBank(orgContext);
            InitialBankBranch(orgContext);
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

                       new Permission { Id = 10401, Name = "ClientGroups", Key = "ClientGroups.All", ParentId = 104 },
                           new Permission { Id = 1040101, Name = "View", Key = "ClientGroups.View", ParentId = 10401, TypeId = 1 },
                           new Permission { Id = 1040102, Name = "Add", Key = "ClientGroups.Add", ParentId = 10401, TypeId = 1 },
                           new Permission { Id = 1040103, Name = "Edit", Key = "ClientGroups.Edit", ParentId = 10401, TypeId = 1 },
                           new Permission { Id = 1040104, Name = "Delete", Key = "ClientGroups.Delete", ParentId = 10401, TypeId = 1 },

                       new Permission { Id = 10402, Name = "Clients", Key = "Clients.All", ParentId = 104 },
                           new Permission { Id = 1040201, Name = "View", Key = "Clients.View", ParentId = 10402, TypeId = 1 },
                           new Permission { Id = 1040202, Name = "Add", Key = "Clients.Add", ParentId = 10402, TypeId = 1 },
                           new Permission { Id = 1040203, Name = "Edit", Key = "Clients.Edit", ParentId = 10402, TypeId = 1 },
                           new Permission { Id = 1040204, Name = "Delete", Key = "Clients.Delete", ParentId = 10402, TypeId = 1 },

                       new Permission { Id = 10403, Name = "SupplierGroups", Key = "SupplierGroups.All", ParentId = 104 },
                           new Permission { Id = 1040301, Name = "View", Key = "SupplierGroups.View", ParentId = 10403, TypeId = 2 },
                           new Permission { Id = 1040302, Name = "Add", Key = "SupplierGroups.Add", ParentId = 10403, TypeId = 2 },
                           new Permission { Id = 1040303, Name = "Edit", Key = "SupplierGroups.Edit", ParentId = 10403, TypeId = 2 },
                           new Permission { Id = 1040304, Name = "Delete", Key = "SupplierGroups.Delete", ParentId = 10403, TypeId = 2 },

                       new Permission { Id = 10404, Name = "Suppliers", Key = "Suppliers.All", ParentId = 104 },
                           new Permission { Id = 1040401, Name = "View", Key = "Suppliers.View", ParentId = 10404, TypeId = 2 },
                           new Permission { Id = 1040402, Name = "Add", Key = "Suppliers.Add", ParentId = 10404, TypeId = 2 },
                           new Permission { Id = 1040403, Name = "Edit", Key = "Suppliers.Edit", ParentId = 10404, TypeId = 2 },
                           new Permission { Id = 1040404, Name = "Delete", Key = "Suppliers.Delete", ParentId = 10404, TypeId = 2 },

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

                       new Permission { Id = 10506, Name = "Cash Boxes", Key = "CashBoxes.All", ParentId = 105 },
                           new Permission { Id = 1050601, Name = "View", Key = "CashBoxes.View", ParentId = 10506, TypeId = 1 },
                           new Permission { Id = 1050602, Name = "Add", Key = "CashBoxes.Add", ParentId = 10506, TypeId = 1 },
                           new Permission { Id = 1050603, Name = "Edit", Key = "CashBoxes.Edit", ParentId = 10506, TypeId = 1 },
                           new Permission { Id = 1050604, Name = "Delete", Key = "CashBoxes.Delete", ParentId = 10506, TypeId = 1 },

                       new Permission { Id = 10507, Name = "Safe Terms", Key = "SafeTerms.All", ParentId = 105 },
                           new Permission { Id = 1050701, Name = "View", Key = "SafeTerms.View", ParentId = 10507, TypeId = 1 },
                           new Permission { Id = 1050702, Name = "Add", Key = "SafeTerms.Add", ParentId = 10507, TypeId = 1 },
                           new Permission { Id = 1050703, Name = "Edit", Key = "SafeTerms.Edit", ParentId = 10507, TypeId = 1 },
                           new Permission { Id = 1050704, Name = "Delete", Key = "SafeTerms.Delete", ParentId = 10507, TypeId = 1 },

                       new Permission { Id = 10508, Name = "Currencies", Key = "Currencies.All", ParentId = 105 },
                           new Permission { Id = 1050801, Name = "View", Key = "Currencies.View", ParentId = 10508, TypeId = 1 },
                           new Permission { Id = 1050802, Name = "Add", Key = "Currencies.Add", ParentId = 10508, TypeId = 1 },
                           new Permission { Id = 1050803, Name = "Edit", Key = "Currencies.Edit", ParentId = 10508, TypeId = 1 },
                           new Permission { Id = 1050804, Name = "Delete", Key = "Currencies.Delete", ParentId = 10508, TypeId = 1 },

                       new Permission { Id = 10509, Name = "FiscalYears", Key = "FiscalYears.All", ParentId = 105 },
                           new Permission { Id = 1050901, Name = "View", Key = "FiscalYears.View", ParentId = 10509, TypeId = 1 },
                           new Permission { Id = 1050902, Name = "Add", Key = "FiscalYears.Add", ParentId = 10509, TypeId = 1 },
                           new Permission { Id = 1050903, Name = "Edit", Key = "FiscalYears.Edit", ParentId = 10509, TypeId = 1 },
                           new Permission { Id = 1050904, Name = "Delete", Key = "FiscalYears.Delete", ParentId = 10509, TypeId = 1 },

                       new Permission { Id = 10510, Name = "Bank Accounts", Key = "BankAccounts.All", ParentId = 105 },
                           new Permission { Id = 1051001, Name = "View", Key = "BankAccounts.View", ParentId = 10510, TypeId = 1 },
                           new Permission { Id = 1051002, Name = "Add", Key = "BankAccounts.Add", ParentId = 10510, TypeId = 1 },
                           new Permission { Id = 1051003, Name = "Edit", Key = "BankAccounts.Edit", ParentId = 10510, TypeId = 1 },
                           new Permission { Id = 1051004, Name = "Delete", Key = "BankAccounts.Delete", ParentId = 10510, TypeId = 1 },

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
                           new Permission { Id = 3010107, Name = "Redo", Key = "SalesInvoices.Redo", ParentId = 30101, TypeId = 1 },

                       new Permission { Id = 30102, Name = "Returns", Key = "SalesReturns.All", ParentId = 301 },
                           new Permission { Id = 3010201, Name = "View", Key = "SalesReturns.View", ParentId = 30102, TypeId = 1 },
                           new Permission { Id = 3010202, Name = "Add", Key = "SalesReturns.Add", ParentId = 30102, TypeId = 1 },
                           new Permission { Id = 3010203, Name = "Edit", Key = "SalesReturns.Edit", ParentId = 30102, TypeId = 1 },
                           new Permission { Id = 3010204, Name = "Delete", Key = "SalesReturns.Delete", ParentId = 30102, TypeId = 1 },
                           new Permission { Id = 3010205, Name = "Cancel", Key = "SalesReturns.Cancel", ParentId = 30102, TypeId = 1 },
                           new Permission { Id = 3010206, Name = "Preference", Key = "SalesReturns.Preference", ParentId = 30102, TypeId = 1 },
                           new Permission { Id = 3010207, Name = "Redo", Key = "SalesReturns.Redo", ParentId = 30102, TypeId = 1 },

                   new Permission { Id = 302, Name = "Purchases", Key = "Purchases", ParentId = 30 },

                       new Permission { Id = 30201, Name = "Invoices", Key = "PurchasesInvoices.All", ParentId = 302 },
                           new Permission { Id = 3020101, Name = "View", Key = "PurchasesInvoices.View", ParentId = 30201, TypeId = 1 },
                           new Permission { Id = 3020102, Name = "Add", Key = "PurchasesInvoices.Add", ParentId = 30201, TypeId = 1 },
                           new Permission { Id = 3020103, Name = "Edit", Key = "PurchasesInvoices.Edit", ParentId = 30201, TypeId = 1 },
                           new Permission { Id = 3020104, Name = "Delete", Key = "PurchasesInvoices.Delete", ParentId = 30201, TypeId = 1 },
                           new Permission { Id = 3020105, Name = "Cancel", Key = "PurchasesInvoices.Cancel", ParentId = 30201, TypeId = 1 },
                           new Permission { Id = 3020106, Name = "Preference", Key = "PurchasesInvoices.Preference", ParentId = 30201, TypeId = 1 },
                           new Permission { Id = 3020107, Name = "Redo", Key = "PurchasesInvoices.Redo", ParentId = 30201, TypeId = 1 },

                       new Permission { Id = 30202, Name = "Returns", Key = "PurchasesReturns.All", ParentId = 302 },
                           new Permission { Id = 3020201, Name = "View", Key = "PurchasesReturns.View", ParentId = 30202, TypeId = 1 },
                           new Permission { Id = 3020202, Name = "Add", Key = "PurchasesReturns.Add", ParentId = 30202, TypeId = 1 },
                           new Permission { Id = 3020203, Name = "Edit", Key = "PurchasesReturns.Edit", ParentId = 30202, TypeId = 1 },
                           new Permission { Id = 3020204, Name = "Delete", Key = "PurchasesReturns.Delete", ParentId = 30202, TypeId = 1 },
                           new Permission { Id = 3020205, Name = "Cancel", Key = "PurchasesReturns.Cancel", ParentId = 30202, TypeId = 1 },
                           new Permission { Id = 3020206, Name = "Preference", Key = "PurchasesReturns.Preference", ParentId = 30202, TypeId = 1 },
                           new Permission { Id = 3020207, Name = "Redo", Key = "PurchasesReturns.Redo", ParentId = 30202, TypeId = 1 },

                   new Permission { Id = 40, Name = "Transactions", Key = "Transactions.All", ParentId = 1 },

                          new Permission { Id = 401, Name = "Transaction Notices", Key = "TransactionNotices", ParentId = 40 },

                               new Permission { Id = 40101, Name = "Addition", Key = "Addition.All", ParentId = 401 },
                                   new Permission { Id = 4010101, Name = "View", Key = "Addition.View", ParentId = 40101, TypeId = 1 },
                                   new Permission { Id = 4010102, Name = "Add", Key = "Addition.Add", ParentId = 40101, TypeId = 1 },
                                   new Permission { Id = 4010103, Name = "Edit", Key = "Addition.Edit", ParentId = 40101, TypeId = 1 },
                                   new Permission { Id = 4010104, Name = "Delete", Key = "Addition.Delete", ParentId = 40101, TypeId = 1 },
                                   new Permission { Id = 4010105, Name = "Preference", Key = "Addition.Preference", ParentId = 40101, TypeId = 1 },
                                   new Permission { Id = 4010106, Name = "Cancel", Key = "Addition.Cancel", ParentId = 40101, TypeId = 1 },
                                   new Permission { Id = 4010107, Name = "Redo", Key = "Addition.Redo", ParentId = 40101, TypeId = 1 },

                               new Permission { Id = 40102, Name = "Issue", Key = "Issue.All", ParentId = 401 },
                                   new Permission { Id = 4010201, Name = "View", Key = "Issue.View", ParentId = 40102, TypeId = 1 },
                                   new Permission { Id = 4010202, Name = "Add", Key = "Issue.Add", ParentId = 40102, TypeId = 1 },
                                   new Permission { Id = 4010203, Name = "Edit", Key = "Issue.Edit", ParentId = 40102, TypeId = 1 },
                                   new Permission { Id = 4010204, Name = "Delete", Key = "Issue.Delete", ParentId = 40102, TypeId = 1 },
                                   new Permission { Id = 4010205, Name = "Preference", Key = "Issue.Preference", ParentId = 40102, TypeId = 1 },
                                   new Permission { Id = 4010206, Name = "Cancel", Key = "Issue.Cancel", ParentId = 40102, TypeId = 1 },
                                   new Permission { Id = 4010207, Name = "Redo", Key = "Issue.Redo", ParentId = 40102, TypeId = 1 },

                               new Permission { Id = 40103, Name = "Transafer", Key = "Transafer.All", ParentId = 401 },
                                   new Permission { Id = 4010301, Name = "View", Key = "Transafer.View", ParentId = 40103, TypeId = 1 },
                                   new Permission { Id = 4010302, Name = "Add", Key = "Transafer.Add", ParentId = 40103, TypeId = 1 },
                                   new Permission { Id = 4010303, Name = "Edit", Key = "Transafer.Edit", ParentId = 40103, TypeId = 1 },
                                   new Permission { Id = 4010304, Name = "Delete", Key = "Transafer.Delete", ParentId = 40103, TypeId = 1 },
                                   new Permission { Id = 4010305, Name = "Preference", Key = "Transafer.Preference", ParentId = 40103, TypeId = 1 },
                                   new Permission { Id = 4010306, Name = "Cancel", Key = "Transafer.Cancel", ParentId = 40103, TypeId = 1 },
                                   new Permission { Id = 4010307, Name = "Redo", Key = "Transafer.Redo", ParentId = 40103, TypeId = 1 },

                               new Permission { Id = 40104, Name = "Received", Key = "Received.All", ParentId = 401 },
                                   new Permission { Id = 4010401, Name = "View", Key = "Received.View", ParentId = 40104, TypeId = 1 },
                                   new Permission { Id = 4010402, Name = "Add", Key = "Received.Add", ParentId = 40104, TypeId = 1 },
                                   new Permission { Id = 4010403, Name = "Edit", Key = "Received.Edit", ParentId = 40104, TypeId = 1 },
                                   new Permission { Id = 4010404, Name = "Delete", Key = "Received.Delete", ParentId = 40104, TypeId = 1 },
                                   new Permission { Id = 4010405, Name = "Preference", Key = "Received.Preference", ParentId = 40104, TypeId = 1 },
                                   new Permission { Id = 4010406, Name = "Cancel", Key = "Received.Cancel", ParentId = 40104, TypeId = 1 },
                                   new Permission { Id = 4010407, Name = "Redo", Key = "Received.Redo", ParentId = 40104, TypeId = 1 },

                               new Permission { Id = 40105, Name = "Inventory", Key = "Inventory.All", ParentId = 401 },
                                   new Permission { Id = 4010501, Name = "View", Key = "Inventory.View", ParentId = 40105, TypeId = 1 },
                                   new Permission { Id = 4010502, Name = "Add", Key = "Inventory.Add", ParentId = 40105, TypeId = 1 },
                                   new Permission { Id = 4010503, Name = "Edit", Key = "Inventory.Edit", ParentId = 40105, TypeId = 1 },
                                   new Permission { Id = 4010504, Name = "Delete", Key = "Inventory.Delete", ParentId = 40105, TypeId = 1 },
                                   new Permission { Id = 4010505, Name = "Preference", Key = "Inventory.Preference", ParentId = 40105, TypeId = 1 },
                                   new Permission { Id = 4010506, Name = "Cancel", Key = "Inventory.Cancel", ParentId = 40105, TypeId = 1 },
                               new Permission { Id = 4010507, Name = "Redo", Key = "Inventory.Redo", ParentId = 40105, TypeId = 1 },

                               new Permission { Id = 40106, Name = "Stock Opening Balance", Key = "StockOpeningBalance.All", ParentId = 401 },
                                   new Permission { Id = 4010601, Name = "View", Key = "StockOpeningBalance.View", ParentId = 40106, TypeId = 1 },
                                   new Permission { Id = 4010602, Name = "Add", Key = "StockOpeningBalance.Add", ParentId = 40106, TypeId = 1 },
                                   new Permission { Id = 4010603, Name = "Edit", Key = "StockOpeningBalance.Edit", ParentId = 40106, TypeId = 1 },
                                   new Permission { Id = 4010604, Name = "Delete", Key = "StockOpeningBalance.Delete", ParentId = 40106, TypeId = 1 },
                                   new Permission { Id = 4010605, Name = "Preference", Key = "StockOpeningBalance.Preference", ParentId = 40106, TypeId = 1 },
                                   new Permission { Id = 4010606, Name = "Cancel", Key = "StockOpeningBalance.Cancel", ParentId = 40106, TypeId = 1 },
                               new Permission { Id = 4010607, Name = "Redo", Key = "StockOpeningBalance.Redo", ParentId = 40106, TypeId = 1 },

                               new Permission { Id = 40107, Name = "Damaged", Key = "Damaged.All", ParentId = 401 },
                                   new Permission { Id = 4010701, Name = "View", Key = "Damaged.View", ParentId = 40107, TypeId = 1 },
                                   new Permission { Id = 4010702, Name = "Add", Key = "Damaged.Add", ParentId = 40107, TypeId = 1 },
                                   new Permission { Id = 4010703, Name = "Edit", Key = "Damaged.Edit", ParentId = 40107, TypeId = 1 },
                                   new Permission { Id = 4010704, Name = "Delete", Key = "Damaged.Delete", ParentId = 40107, TypeId = 1 },
                                   new Permission { Id = 4010705, Name = "Preference", Key = "Damaged.Preference", ParentId = 40107, TypeId = 1 },
                                   new Permission { Id = 4010706, Name = "Cancel", Key = "Damaged.Cancel", ParentId = 40107, TypeId = 1 },
                                   new Permission { Id = 4010707, Name = "Redo", Key = "Damaged.Redo", ParentId = 40107, TypeId = 1 },

                       new Permission { Id = 50, Name = "Financials", Key = "Financials.All", ParentId = 1 },

                          new Permission { Id = 501, Name = "Safe Notices", Key = "SafeNotices", ParentId = 50 },

                              new Permission { Id = 50101, Name = "FinancialOpeningBalance", Key = "FinancialOpeningBalance.All", ParentId = 501 },
                                   new Permission { Id = 5010101, Name = "View", Key = "FinancialOpeningBalance.View", ParentId = 50101, TypeId = 1 },
                                   new Permission { Id = 5010102, Name = "Add", Key = "FinancialOpeningBalance.Add", ParentId = 50101, TypeId = 1 },
                                   new Permission { Id = 5010103, Name = "Edit", Key = "FinancialOpeningBalance.Edit", ParentId = 50101, TypeId = 1 },
                                   new Permission { Id = 5010104, Name = "Delete", Key = "FinancialOpeningBalance.Delete", ParentId = 50101, TypeId = 1 },
                                   new Permission { Id = 5010105, Name = "Preference", Key = "FinancialOpeningBalance.Preference", ParentId = 50101, TypeId = 1 },

                              new Permission { Id = 50102, Name = "FinancialReceipt", Key = "FinancialReceipt.All", ParentId = 501 },
                                   new Permission { Id = 5010201, Name = "View", Key = "FinancialReceipt.View", ParentId = 50102, TypeId = 1 },
                                   new Permission { Id = 5010202, Name = "Add", Key = "FinancialReceipt.Add", ParentId = 50102, TypeId = 1 },
                                   new Permission { Id = 5010203, Name = "Edit", Key = "FinancialReceipt.Edit", ParentId = 50102, TypeId = 1 },
                                   new Permission { Id = 5010204, Name = "Delete", Key = "FinancialReceipt.Delete", ParentId = 50102, TypeId = 1 },
                                   new Permission { Id = 5010205, Name = "Preference", Key = "FinancialReceipt.Preference", ParentId = 50102, TypeId = 1 },

                              new Permission { Id = 50103, Name = "FinancialPayment", Key = "FinancialPayment.All", ParentId = 501 },
                                   new Permission { Id = 5010301, Name = "View", Key = "FinancialPayment.View", ParentId = 50103, TypeId = 1 },
                                   new Permission { Id = 5010302, Name = "Add", Key = "FinancialPayment.Add", ParentId = 50103, TypeId = 1 },
                                   new Permission { Id = 5010303, Name = "Edit", Key = "FinancialPayment.Edit", ParentId = 50103, TypeId = 1 },
                                   new Permission { Id = 5010304, Name = "Delete", Key = "FinancialPayment.Delete", ParentId = 50103, TypeId = 1 },
                                   new Permission { Id = 5010305, Name = "Preference", Key = "FinancialPayment.Preference", ParentId = 50103, TypeId = 1 },

                              new Permission { Id = 50104, Name = "FinancialTransfer", Key = "FinancialTransfer.All", ParentId = 501 },
                                   new Permission { Id = 5010401, Name = "View", Key = "FinancialTransfer.View", ParentId = 50104, TypeId = 1 },
                                   new Permission { Id = 5010402, Name = "Add", Key = "FinancialTransfer.Add", ParentId = 50104, TypeId = 1 },
                                   new Permission { Id = 5010403, Name = "Edit", Key = "FinancialTransfer.Edit", ParentId = 50104, TypeId = 1 },
                                   new Permission { Id = 5010404, Name = "Delete", Key = "FinancialTransfer.Delete", ParentId = 50104, TypeId = 1 },
                                   new Permission { Id = 5010405, Name = "Preference", Key = "FinancialTransfer.Preference", ParentId = 50104, TypeId = 1 },
                                   new Permission { Id = 5010406, Name = "Post", Key = "FinancialTransfer.Post", ParentId = 50104, TypeId = 1 },
                                   new Permission { Id = 5010407, Name = "Reverse", Key = "FinancialTransfer.Reverse", ParentId = 50104, TypeId = 1 },

                              new Permission { Id = 50105, Name = "FinancialDeposit", Key = "FinancialDeposit.All", ParentId = 501 },
                                   new Permission { Id = 5010501, Name = "View", Key = "FinancialDeposit.View", ParentId = 50105, TypeId = 1 },
                                   new Permission { Id = 5010502, Name = "Add", Key = "FinancialDeposit.Add", ParentId = 50105, TypeId = 1 },
                                   new Permission { Id = 5010503, Name = "Edit", Key = "FinancialDeposit.Edit", ParentId = 50105, TypeId = 1 },
                                   new Permission { Id = 5010504, Name = "Delete", Key = "FinancialDeposit.Delete", ParentId = 50105, TypeId = 1 },
                                   new Permission { Id = 5010505, Name = "Preference", Key = "FinancialDeposit.Preference", ParentId = 50105, TypeId = 1 },

                              new Permission { Id = 50106, Name = "FinancialWithdrawal", Key = "FinancialWithdrawal.All", ParentId = 501 },
                                   new Permission { Id = 5010601, Name = "View", Key = "FinancialWithdrawal.View", ParentId = 50106, TypeId = 1 },
                                   new Permission { Id = 5010602, Name = "Add", Key = "FinancialWithdrawal.Add", ParentId = 50106, TypeId = 1 },
                                   new Permission { Id = 5010603, Name = "Edit", Key = "FinancialWithdrawal.Edit", ParentId = 50106, TypeId = 1 },
                                   new Permission { Id = 5010604, Name = "Delete", Key = "FinancialWithdrawal.Delete", ParentId = 50106, TypeId = 1 },
                                   new Permission { Id = 5010605, Name = "Preference", Key = "FinancialWithdrawal.Preference", ParentId = 50106, TypeId = 1 },

                              new Permission { Id = 50107, Name = "FinancialFee", Key = "FinancialFee.All", ParentId = 501 },
                                   new Permission { Id = 5010701, Name = "View", Key = "FinancialFee.View", ParentId = 50107, TypeId = 1 },
                                   new Permission { Id = 5010702, Name = "Add", Key = "FinancialFee.Add", ParentId = 50107, TypeId = 1 },
                                   new Permission { Id = 5010703, Name = "Edit", Key = "FinancialFee.Edit", ParentId = 50107, TypeId = 1 },
                                   new Permission { Id = 5010704, Name = "Delete", Key = "FinancialFee.Delete", ParentId = 50107, TypeId = 1 },
                                   new Permission { Id = 5010705, Name = "Preference", Key = "FinancialFee.Preference", ParentId = 50107, TypeId = 1 },

                              new Permission { Id = 50108, Name = "FinancialInterest", Key = "FinancialInterest.All", ParentId = 501 },
                                   new Permission { Id = 5010801, Name = "View", Key = "FinancialInterest.View", ParentId = 50108, TypeId = 1 },
                                   new Permission { Id = 5010802, Name = "Add", Key = "FinancialInterest.Add", ParentId = 50108, TypeId = 1 },
                                   new Permission { Id = 5010803, Name = "Edit", Key = "FinancialInterest.Edit", ParentId = 50108, TypeId = 1 },
                                   new Permission { Id = 5010804, Name = "Delete", Key = "FinancialInterest.Delete", ParentId = 50108, TypeId = 1 },
                                   new Permission { Id = 5010805, Name = "Preference", Key = "FinancialInterest.Preference", ParentId = 50108, TypeId = 1 },

                              new Permission { Id = 50109, Name = "FinancialCheque", Key = "FinancialCheque.All", ParentId = 501 },
                                   new Permission { Id = 5010901, Name = "View", Key = "FinancialCheque.View", ParentId = 50109, TypeId = 1 },
                                   new Permission { Id = 5010902, Name = "Add", Key = "FinancialCheque.Add", ParentId = 50109, TypeId = 1 },
                                   new Permission { Id = 5010903, Name = "Edit", Key = "FinancialCheque.Edit", ParentId = 50109, TypeId = 1 },
                                   new Permission { Id = 5010904, Name = "Delete", Key = "FinancialCheque.Delete", ParentId = 50109, TypeId = 1 },
                                   new Permission { Id = 5010905, Name = "Preference", Key = "FinancialCheque.Preference", ParentId = 50109, TypeId = 1 },

                              new Permission { Id = 50110, Name = "FinancialAdjustment", Key = "FinancialAdjustment.All", ParentId = 501 },
                                   new Permission { Id = 5011001, Name = "View", Key = "FinancialAdjustment.View", ParentId = 50110, TypeId = 1 },
                                   new Permission { Id = 5011002, Name = "Add", Key = "FinancialAdjustment.Add", ParentId = 50110, TypeId = 1 },
                                   new Permission { Id = 5011003, Name = "Edit", Key = "FinancialAdjustment.Edit", ParentId = 50110, TypeId = 1 },
                                   new Permission { Id = 5011004, Name = "Delete", Key = "FinancialAdjustment.Delete", ParentId = 50110, TypeId = 1 },
                                   new Permission { Id = 5011005, Name = "Preference", Key = "FinancialAdjustment.Preference", ParentId = 50110, TypeId = 1 },

                              new Permission { Id = 50111, Name = "Journal", Key = "Journal.All", ParentId = 501 },
                                   new Permission { Id = 5011101, Name = "View", Key = "Journal.View", ParentId = 50111, TypeId = 1 },
                                   new Permission { Id = 5011102, Name = "Add", Key = "Journal.Add", ParentId = 50111, TypeId = 1 },
                                   new Permission { Id = 5011103, Name = "Edit", Key = "Journal.Edit", ParentId = 50111, TypeId = 1 },
                                   new Permission { Id = 5011104, Name = "Delete", Key = "Journal.Delete", ParentId = 50111, TypeId = 1 },
                                   new Permission { Id = 5011105, Name = "Preference", Key = "Journal.Preference", ParentId = 50111, TypeId = 1 },
                                   new Permission { Id = 5011106, Name = "Post", Key = "Journal.Post", ParentId = 50111, TypeId = 1 },
                                   new Permission { Id = 5011107, Name = "Cancel", Key = "Journal.Cancel", ParentId = 50111, TypeId = 1 },
                                   new Permission { Id = 5011108, Name = "Redo", Key = "Journal.Redo", ParentId = 50111, TypeId = 1 },
                                   new Permission { Id = 5011109, Name = "Reverse", Key = "Journal.Reverse", ParentId = 50111, TypeId = 1 },

                              new Permission { Id = 50112, Name = "Financial", Key = "Financial.All", ParentId = 501 },
                                   new Permission { Id = 5011201, Name = "View", Key = "Financial.View", ParentId = 50112, TypeId = 1 },
                                   new Permission { Id = 5011202, Name = "Add", Key = "Financial.Add", ParentId = 50112, TypeId = 1 },
                                   new Permission { Id = 5011203, Name = "Edit", Key = "Financial.Edit", ParentId = 50112, TypeId = 1 },
                                   new Permission { Id = 5011204, Name = "Delete", Key = "Financial.Delete", ParentId = 50112, TypeId = 1 },
                                   new Permission { Id = 5011205, Name = "Cancel", Key = "Financial.Cancel", ParentId = 50112, TypeId = 1 },
                                   new Permission { Id = 5011206, Name = "Redo", Key = "Financial.Redo", ParentId = 50112, TypeId = 1 },
                               
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
               new Preference { Id = 21, Key = "AccountsIntegration", Value = "5", Reference = "Invoice", TypeId = 1, Hide = false },
               new Preference { Id = 22, Key = "SalesAccount", Value = "0", Reference = "Invoice", TypeId = 1, Hide = false },
               new Preference { Id = 23, Key = "DealerAccount", Value = "0", Reference = "Invoice", TypeId = 1, Hide = false },
               new Preference { Id = 24, Key = "TaxAccount", Value = "0", Reference = "Invoice", TypeId = 1, Hide = false },
               new Preference { Id = 25, Key = "AutoCreateJournalEntry", Value = "0", Reference = "Invoice", TypeId = 1, Hide = false },

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
               new Preference { Id = 121, Key = "AccountsIntegration", Value = "0", Reference = "Invoice", TypeId = 2, Hide = false },
               new Preference { Id = 122, Key = "PurchaseAccount", Value = "0", Reference = "Invoice", TypeId = 2, Hide = false },
               new Preference { Id = 123, Key = "DealerAccount", Value = "0", Reference = "Invoice", TypeId = 2, Hide = false },
               new Preference { Id = 124, Key = "TaxAccount", Value = "0", Reference = "Invoice", TypeId = 2, Hide = false },
               new Preference { Id = 125, Key = "AutoCreateJournalEntry", Value = "0", Reference = "Invoice", TypeId = 2, Hide = false },

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
               new Preference { Id = 218, Key = "AccountsIntegration", Value = "0", Reference = "Invoice", TypeId = 3, Hide = false },
               new Preference { Id = 219, Key = "SalesAccount", Value = "0", Reference = "Invoice", TypeId = 3, Hide = false },
               new Preference { Id = 220, Key = "DealerAccount", Value = "0", Reference = "Invoice", TypeId = 3, Hide = false },
               new Preference { Id = 221, Key = "TaxAccount", Value = "0", Reference = "Invoice", TypeId = 3, Hide = false },
               new Preference { Id = 222, Key = "AutoCreateJournalEntry", Value = "0", Reference = "Invoice", TypeId = 3, Hide = false },

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
               new Preference { Id = 318, Key = "AccountsIntegration", Value = "0", Reference = "Invoice", TypeId = 4, Hide = false },
               new Preference { Id = 319, Key = "PurchaseAccount", Value = "0", Reference = "Invoice", TypeId = 4, Hide = false },
               new Preference { Id = 320, Key = "DealerAccount", Value = "0", Reference = "Invoice", TypeId = 4, Hide = false },
               new Preference { Id = 321, Key = "TaxAccount", Value = "0", Reference = "Invoice", TypeId = 4, Hide = false },
               new Preference { Id = 322, Key = "AutoCreateJournalEntry", Value = "0", Reference = "Invoice", TypeId = 4, Hide = false },

               new Preference { Id = 401, Key = "DefaultStock", Value = "1", Reference = "Transaction", TypeId = 1, Hide = false },
               new Preference { Id = 402, Key = "DefaultSupplier", Value = "1", Reference = "Transaction", TypeId = 1, Hide = false },
               new Preference { Id = 403, Key = "NumberLine", Value = "6", Reference = "Transaction", TypeId = 1, Hide = false },
               new Preference { Id = 404, Key = "OrderTabe", Value = "2", Reference = "Transaction", TypeId = 1, Hide = false },
               new Preference { Id = 405, Key = "AutoSave", Value = "0", Reference = "Transaction", TypeId = 1, Hide = false },
               new Preference { Id = 406, Key = "TypeSerial", Value = "1", Reference = "Transaction", TypeId = 1, Hide = false },
               new Preference { Id = 407, Key = "AllowRepeated", Value = "1", Reference = "Transaction", TypeId = 1, Hide = false },
               new Preference { Id = 408, Key = "SaveLastStatusSetting", Value = "1", Reference = "Transaction", TypeId = 1, Hide = false },
               new Preference { Id = 409, Key = "AccountsIntegration", Value = "0", Reference = "Transaction", TypeId = 1, Hide = false },
               new Preference { Id = 410, Key = "AutoCreateJournalEntry", Value = "0", Reference = "Transaction", TypeId = 1, Hide = false },
               new Preference { Id = 411, Key = "StockAccount", Value = "0", Reference = "Transaction", TypeId = 1, Hide = false },
               new Preference { Id = 412, Key = "PurchaseAccount", Value = "0", Reference = "Transaction", TypeId = 1, Hide = false },
               new Preference { Id = 413, Key = "SalesReturnAccount", Value = "0", Reference = "Transaction", TypeId = 1, Hide = false },

               new Preference { Id = 501, Key = "DefaultStock", Value = "1", Reference = "Transaction", TypeId = 2, Hide = false },
               new Preference { Id = 502, Key = "DefaultCustomer", Value = "1", Reference = "Transaction", TypeId = 2, Hide = false },
               new Preference { Id = 503, Key = "NumberLine", Value = "6", Reference = "Transaction", TypeId = 2, Hide = false },
               new Preference { Id = 504, Key = "OrderTabe", Value = "2", Reference = "Transaction", TypeId = 2, Hide = false },
               new Preference { Id = 505, Key = "AutoSave", Value = "0", Reference = "Transaction", TypeId = 2, Hide = false },
               new Preference { Id = 506, Key = "TypeSerial", Value = "1", Reference = "Transaction", TypeId = 2, Hide = false },
               new Preference { Id = 507, Key = "AllowRepeated", Value = "1", Reference = "Transaction", TypeId = 2, Hide = false },
               new Preference { Id = 508, Key = "SaveLastStatusSetting", Value = "1", Reference = "Transaction", TypeId = 2, Hide = false },
               new Preference { Id = 509, Key = "AccountsIntegration", Value = "0", Reference = "Transaction", TypeId = 2, Hide = false },
               new Preference { Id = 510, Key = "AutoCreateJournalEntry", Value = "0", Reference = "Transaction", TypeId = 2, Hide = false },
               new Preference { Id = 511, Key = "StockAccount", Value = "0", Reference = "Transaction", TypeId = 2, Hide = false },
               new Preference { Id = 512, Key = "SalesAccount", Value = "0", Reference = "Transaction", TypeId = 2, Hide = false },
               new Preference { Id = 513, Key = "PurchaseReturnAccount", Value = "0", Reference = "Transaction", TypeId = 2, Hide = false },

               new Preference { Id = 601, Key = "DefaultStock", Value = "1", Reference = "Transaction", TypeId = 3, Hide = false },
               new Preference { Id = 602, Key = "NumberLine", Value = "6", Reference = "Transaction", TypeId = 3, Hide = false },
               new Preference { Id = 603, Key = "OrderTabe", Value = "2", Reference = "Transaction", TypeId = 3, Hide = false },
               new Preference { Id = 604, Key = "AutoSave", Value = "0", Reference = "Transaction", TypeId = 3, Hide = false },
               new Preference { Id = 605, Key = "TypeSerial", Value = "1", Reference = "Transaction", TypeId = 3, Hide = false },
               new Preference { Id = 606, Key = "AllowRepeated", Value = "1", Reference = "Transaction", TypeId = 3, Hide = false },
               new Preference { Id = 607, Key = "SaveLastStatusSetting", Value = "1", Reference = "Transaction", TypeId = 3, Hide = false },
               new Preference { Id = 608, Key = "AutoReceived", Value = "0", Reference = "Transaction", TypeId = 3, Hide = false },
               new Preference { Id = 609, Key = "AccountsIntegration", Value = "0", Reference = "Transaction", TypeId = 3, Hide = false },
               new Preference { Id = 610, Key = "AutoCreateJournalEntry", Value = "0", Reference = "Transaction", TypeId = 3, Hide = false },
               new Preference { Id = 611, Key = "SourceInventoryAccount", Value = "0", Reference = "Transaction", TypeId = 3, Hide = false },
               new Preference { Id = 612, Key = "TransitAccount", Value = "0", Reference = "Transaction", TypeId = 3, Hide = false },

               new Preference { Id = 701, Key = "DefaultStock", Value = "1", Reference = "Transaction", TypeId = 4, Hide = false },
               new Preference { Id = 702, Key = "NumberLine", Value = "6", Reference = "Transaction", TypeId = 4, Hide = false },
               new Preference { Id = 703, Key = "OrderTabe", Value = "2", Reference = "Transaction", TypeId = 4, Hide = false },
               new Preference { Id = 704, Key = "AutoSave", Value = "0", Reference = "Transaction", TypeId = 4, Hide = false },
               new Preference { Id = 705, Key = "TypeSerial", Value = "1", Reference = "Transaction", TypeId = 4, Hide = false },
               new Preference { Id = 706, Key = "AllowRepeated", Value = "1", Reference = "Transaction", TypeId = 4, Hide = false },
               new Preference { Id = 707, Key = "AccountsIntegration", Value = "0", Reference = "Transaction", TypeId = 4, Hide = false },
               new Preference { Id = 708, Key = "AutoCreateJournalEntry", Value = "0", Reference = "Transaction", TypeId = 4, Hide = false },
               new Preference { Id = 709, Key = "DestinationInventoryAccount", Value = "0", Reference = "Transaction", TypeId = 4, Hide = false },
               new Preference { Id = 710, Key = "TransitAccount", Value = "0", Reference = "Transaction", TypeId = 4, Hide = false },

               new Preference { Id = 1501, Key = "DefaultStock", Value = "1", Reference = "Transaction", TypeId = 7, Hide = false },
               new Preference { Id = 1502, Key = "NumberLine", Value = "10", Reference = "Transaction", TypeId = 7, Hide = false },
               new Preference { Id = 1503, Key = "OrderTabe", Value = "1", Reference = "Transaction", TypeId = 7, Hide = false },
               new Preference { Id = 1504, Key = "AutoSave", Value = "0", Reference = "Transaction", TypeId = 7, Hide = false },
               new Preference { Id = 1505, Key = "TypeSerial", Value = "1", Reference = "Transaction", TypeId = 7, Hide = false },
               new Preference { Id = 1506, Key = "AllowRepeated", Value = "0", Reference = "Transaction", TypeId = 7, Hide = false },
               new Preference { Id = 1507, Key = "SaveLastStatusSetting", Value = "1", Reference = "Transaction", TypeId = 7, Hide = false },
               new Preference { Id = 1509, Key = "AccountsIntegration", Value = "0", Reference = "Transaction", TypeId = 7, Hide = false },
               new Preference { Id = 1510, Key = "AutoCreateJournalEntry", Value = "0", Reference = "Transaction", TypeId = 7, Hide = false },
               new Preference { Id = 1511, Key = "StockAccount", Value = "0", Reference = "Transaction", TypeId = 7, Hide = false },
               new Preference { Id = 1512, Key = "OpeningBalanceAccount", Value = "0", Reference = "Transaction", TypeId = 7, Hide = false },

               new Preference { Id = 1601, Key = "DefaultStock", Value = "1", Reference = "Transaction", TypeId = 8, Hide = false },
               new Preference { Id = 1602, Key = "NumberLine", Value = "10", Reference = "Transaction", TypeId = 8, Hide = false },
               new Preference { Id = 1603, Key = "OrderTabe", Value = "1", Reference = "Transaction", TypeId = 8, Hide = false },
               new Preference { Id = 1604, Key = "AutoSave", Value = "0", Reference = "Transaction", TypeId = 8, Hide = false },
               new Preference { Id = 1605, Key = "TypeSerial", Value = "1", Reference = "Transaction", TypeId = 8, Hide = false },
               new Preference { Id = 1606, Key = "AllowRepeated", Value = "0", Reference = "Transaction", TypeId = 8, Hide = false },
               new Preference { Id = 1607, Key = "SaveLastStatusSetting", Value = "1", Reference = "Transaction", TypeId = 8, Hide = false },
               new Preference { Id = 1609, Key = "AccountsIntegration", Value = "0", Reference = "Transaction", TypeId = 8, Hide = false },
               new Preference { Id = 1610, Key = "AutoCreateJournalEntry", Value = "0", Reference = "Transaction", TypeId = 8, Hide = false },
               new Preference { Id = 1611, Key = "StockAccount", Value = "0", Reference = "Transaction", TypeId = 8, Hide = false },
               new Preference { Id = 1612, Key = "InventoryDamageExpenseAccount", Value = "0", Reference = "Transaction", TypeId = 8, Hide = false },

               new Preference { Id = 1700, Key = "DefaultStock", Value = "1", Reference = "Inventory", TypeId = 0, Hide = false },
               new Preference { Id = 1701, Key = "AutoSave", Value = "0", Reference = "Inventory", TypeId = 0, Hide = false },
               new Preference { Id = 1702, Key = "TypeSerial", Value = "1", Reference = "Inventory", TypeId = 0, Hide = false },
               new Preference { Id = 1703, Key = "AutoCreateAdjustment", Value = "0", Reference = "Inventory", TypeId = 0, Hide = false },

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

               new Preference { Id = 2000, Key = "SaveLastStatusSetting", Value = "1", Reference = "Financial", TypeId = 1, Hide = false },
               new Preference { Id = 2001, Key = "AccountsIntegration", Value = "0", Reference = "Financial", TypeId = 1, Hide = false },
               new Preference { Id = 2002, Key = "AutoCreateJournalEntry", Value = "0", Reference = "Financial", TypeId = 1, Hide = false },
               new Preference { Id = 2003, Key = "AutoSave", Value = "0", Reference = "Financial", TypeId = 1, Hide = false },
               new Preference { Id = 2004, Key = "TypeSerial", Value = "1", Reference = "Financial", TypeId = 1, Hide = false },

               new Preference { Id = 2100, Key = "SaveLastStatusSetting", Value = "1", Reference = "Financial", TypeId = 2, Hide = false },
               new Preference { Id = 2101, Key = "AccountsIntegration", Value = "0", Reference = "Financial", TypeId = 2, Hide = false },
               new Preference { Id = 2102, Key = "AutoCreateJournalEntry", Value = "0", Reference = "Financial", TypeId = 2, Hide = false },
               new Preference { Id = 2103, Key = "AutoSave", Value = "0", Reference = "Financial", TypeId = 2, Hide = false },
               new Preference { Id = 2104, Key = "TypeSerial", Value = "1", Reference = "Financial", TypeId = 2, Hide = false },

               new Preference { Id = 2200, Key = "SaveLastStatusSetting", Value = "1", Reference = "Financial", TypeId = 3, Hide = false },
               new Preference { Id = 2201, Key = "AccountsIntegration", Value = "0", Reference = "Financial", TypeId = 3, Hide = false },
               new Preference { Id = 2202, Key = "AutoCreateJournalEntry", Value = "0", Reference = "Financial", TypeId = 3, Hide = false },
               new Preference { Id = 2203, Key = "AutoSave", Value = "0", Reference = "Financial", TypeId = 3, Hide = false },
               new Preference { Id = 2204, Key = "TypeSerial", Value = "1", Reference = "Financial", TypeId = 3, Hide = false },

               new Preference { Id = 2300, Key = "SaveLastStatusSetting", Value = "1", Reference = "Financial", TypeId = 4, Hide = false },
               new Preference { Id = 2301, Key = "AccountsIntegration", Value = "0", Reference = "Financial", TypeId = 4, Hide = false },
               new Preference { Id = 2302, Key = "AutoCreateJournalEntry", Value = "0", Reference = "Financial", TypeId = 4, Hide = false },
               new Preference { Id = 2303, Key = "AutoSave", Value = "0", Reference = "Financial", TypeId = 4, Hide = false },
               new Preference { Id = 2304, Key = "TypeSerial", Value = "1", Reference = "Financial", TypeId = 4, Hide = false },

               new Preference { Id = 2400, Key = "SaveLastStatusSetting", Value = "1", Reference = "Financial", TypeId = 5, Hide = false },
               new Preference { Id = 2401, Key = "AccountsIntegration", Value = "0", Reference = "Financial", TypeId = 5, Hide = false },
               new Preference { Id = 2402, Key = "AutoCreateJournalEntry", Value = "0", Reference = "Financial", TypeId = 5, Hide = false },
               new Preference { Id = 2403, Key = "AutoSave", Value = "0", Reference = "Financial", TypeId = 5, Hide = false },
               new Preference { Id = 2404, Key = "TypeSerial", Value = "1", Reference = "Financial", TypeId = 5, Hide = false },

               new Preference { Id = 2500, Key = "SaveLastStatusSetting", Value = "1", Reference = "Financial", TypeId = 6, Hide = false },
               new Preference { Id = 2501, Key = "AccountsIntegration", Value = "0", Reference = "Financial", TypeId = 6, Hide = false },
               new Preference { Id = 2502, Key = "AutoCreateJournalEntry", Value = "0", Reference = "Financial", TypeId = 6, Hide = false },
               new Preference { Id = 2503, Key = "AutoSave", Value = "0", Reference = "Financial", TypeId = 6, Hide = false },
               new Preference { Id = 2504, Key = "TypeSerial", Value = "1", Reference = "Financial", TypeId = 6, Hide = false },

               new Preference { Id = 2600, Key = "SaveLastStatusSetting", Value = "1", Reference = "Financial", TypeId = 7, Hide = false },
               new Preference { Id = 2601, Key = "AccountsIntegration", Value = "0", Reference = "Financial", TypeId = 7, Hide = false },
               new Preference { Id = 2602, Key = "AutoCreateJournalEntry", Value = "0", Reference = "Financial", TypeId = 7, Hide = false },
               new Preference { Id = 2603, Key = "AutoSave", Value = "0", Reference = "Financial", TypeId = 7, Hide = false },
               new Preference { Id = 2604, Key = "TypeSerial", Value = "1", Reference = "Financial", TypeId = 7, Hide = false },

               new Preference { Id = 2700, Key = "SaveLastStatusSetting", Value = "1", Reference = "Financial", TypeId = 8, Hide = false },
               new Preference { Id = 2701, Key = "AccountsIntegration", Value = "0", Reference = "Financial", TypeId = 8, Hide = false },
               new Preference { Id = 2702, Key = "AutoCreateJournalEntry", Value = "0", Reference = "Financial", TypeId = 8, Hide = false },
               new Preference { Id = 2703, Key = "AutoSave", Value = "0", Reference = "Financial", TypeId = 8, Hide = false },
               new Preference { Id = 2704, Key = "TypeSerial", Value = "1", Reference = "Financial", TypeId = 8, Hide = false },

               new Preference { Id = 2800, Key = "SaveLastStatusSetting", Value = "1", Reference = "Financial", TypeId = 9, Hide = false },
               new Preference { Id = 2801, Key = "AccountsIntegration", Value = "0", Reference = "Financial", TypeId = 9, Hide = false },
               new Preference { Id = 2802, Key = "AutoCreateJournalEntry", Value = "0", Reference = "Financial", TypeId = 9, Hide = false },
               new Preference { Id = 2803, Key = "AutoSave", Value = "0", Reference = "Financial", TypeId = 9, Hide = false },
               new Preference { Id = 2804, Key = "TypeSerial", Value = "1", Reference = "Financial", TypeId = 9, Hide = false },

               new Preference { Id = 2900, Key = "SaveLastStatusSetting", Value = "1", Reference = "Financial", TypeId = 10, Hide = false },
               new Preference { Id = 2901, Key = "AccountsIntegration", Value = "0", Reference = "Financial", TypeId = 10, Hide = false },
               new Preference { Id = 2902, Key = "AutoCreateJournalEntry", Value = "0", Reference = "Financial", TypeId = 10, Hide = false },
               new Preference { Id = 2903, Key = "AutoSave", Value = "0", Reference = "Financial", TypeId = 10, Hide = false },
               new Preference { Id = 2904, Key = "TypeSerial", Value = "1", Reference = "Financial", TypeId = 10, Hide = false },

               new Preference { Id = 3000, Key = "NumberLine", Value = "2", Reference = "Journal", TypeId = 0, Hide = false },
               new Preference { Id = 3001, Key = "OrderTabe", Value = "1", Reference = "Journal", TypeId = 0, Hide = false },
               new Preference { Id = 3002, Key = "AutoSave", Value = "0", Reference = "Journal", TypeId = 0, Hide = false },
               new Preference { Id = 3003, Key = "TypeSerial", Value = "1", Reference = "Journal", TypeId = 0, Hide = false },
               new Preference { Id = 3004, Key = "SaveLastStatusSetting", Value = "1", Reference = "Journal", TypeId = 0, Hide = false },
               new Preference { Id = 3005, Key = "DefaultCurrency", Value = "1", Reference = "Journal", TypeId = 0, Hide = false },
               new Preference { Id = 3006, Key = "DefaultJournalType", Value = "2", Reference = "Journal", TypeId = 0, Hide = false },

               // AR / Customer accounting setup — TypeId 1 mirrors the Dealer.TypeId convention (Client).
               new Preference { Id = 3101, Key = "ReceivableParentAccountId", Value = "0", Reference = "Dealer", TypeId = 1, Hide = false },
               new Preference { Id = 3102, Key = "AutoCreateReceivableAccount", Value = "0", Reference = "Dealer", TypeId = 1, Hide = false },
               new Preference { Id = 3103, Key = "OpeningBalanceClearingAccountId", Value = "0", Reference = "Dealer", TypeId = 1, Hide = false },

               // AP / Supplier accounting setup — TypeId 2 mirrors the Dealer.TypeId convention (Supplier).
               new Preference { Id = 3104, Key = "PayableParentAccountId", Value = "0", Reference = "Dealer", TypeId = 2, Hide = false },
               new Preference { Id = 3105, Key = "AutoCreatePayableAccount", Value = "0", Reference = "Dealer", TypeId = 2, Hide = false },
               new Preference { Id = 3106, Key = "OpeningBalanceClearingAccountId", Value = "0", Reference = "Dealer", TypeId = 2, Hide = false }
            };

            var openingBalancePurchaseAccount = orgContext.Preferences.FirstOrDefault(e =>
                e.Reference == "Transaction" && e.TypeId == 7 && e.Key == "PurchaseAccount");
            if (openingBalancePurchaseAccount != null)
                openingBalancePurchaseAccount.Key = "OpeningBalanceAccount";

            var obsoleteOpeningBalancePreferences = orgContext.Preferences.Where(e =>
                e.Reference == "Transaction" && e.TypeId == 7 && e.Key == "SalesReturnAccount");
            orgContext.Preferences.RemoveRange(obsoleteOpeningBalancePreferences);

            var inventoryDamagePurchaseAccount = orgContext.Preferences.FirstOrDefault(e =>
                e.Reference == "Transaction" && e.TypeId == 8 && e.Key == "PurchaseAccount");
            if (inventoryDamagePurchaseAccount != null)
                inventoryDamagePurchaseAccount.Key = "InventoryDamageExpenseAccount";

            var obsoleteInventoryDamagePreferences = orgContext.Preferences.Where(e =>
                e.Reference == "Transaction" && e.TypeId == 8 && e.Key == "SalesReturnAccount");
            orgContext.Preferences.RemoveRange(obsoleteInventoryDamagePreferences);

            foreach (var ob in list)
            {
                if (!orgContext.Preferences.Any(e => e.Id == ob.Id))
                    orgContext.Set<Preference>().Add(ob);
                //else
                //    orgContext.Entry<Preference>(orgContext.Set<Preference>().Find(ob.Id)).CurrentValues.SetValues(ob);
            }
            orgContext.SaveChanges();
        }

        public void InitialOrderType(OrgContext orgContext)
        {
            List<OrderType> list = new List<OrderType> {
                   new OrderType { Id = 1, Name = "Internal", Hide = false, Icon = "iconsminds-right-1" },
                   new OrderType { Id = 2, Name = "External", Hide = false, Icon = "iconsminds-left-1" }
            };

            //foreach (var ob in list)
            //{
            //    if (!orgContext.OrderTypes.Any(e => e.Id == ob.Id))
            //        orgContext.Set<OrderType>().Add(ob);
            //    else
            //        orgContext.Entry<OrderType>(orgContext.Set<OrderType>().Find(ob.Id)).CurrentValues.SetValues(ob);
            //}
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

        public void InitialJournalType(OrgContext orgContext)
        {
            List<JournalType> list = new List<JournalType> {
                    new JournalType { Id = 1, Group = "Journal", Name = "Openning Balance", Hide = false, IsOpeningBlance = true, Icon = "simple-icon-basket-loaded" },
                    new JournalType { Id = 2, Group = "Journal", Name = "Journal", Hide = false, IsOpeningBlance = false, Icon = "simple-icon-basket-loaded" }
            };

            foreach (var ob in list)
            {
                if (!orgContext.JournalTypes.Any(e => e.Id == ob.Id))
                    orgContext.Set<JournalType>().Add(ob);
                else
                    orgContext.Entry<JournalType>(orgContext.Set<JournalType>().Find(ob.Id)).CurrentValues.SetValues(ob);
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
                  new TransactionType { Id = 6, Name = "Adjustment Out", Hide = false, InOut = -1, Icon = "" },
                  new TransactionType { Id = 7, Name = "Opening Balance", Hide = false, InOut = 1, Icon = "iconsminds-folder-open" },
                  new TransactionType { Id = 8, Name = "Damaged", Hide = false, InOut = -1, Icon = "iconsminds-bio-hazard" }
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
                 new FinancialType { Id = 1, Name = "OpeningBalance", Hide = false, InOut = 1, Icon = "iconsminds-start-2" },
                 new FinancialType { Id = 2, Name = "Receipt", Hide = false, InOut = 1, Icon = "iconsminds-financial" },
                 new FinancialType { Id = 3, Name = "Payment", Hide = false, InOut = -1, Icon = "iconsminds-handshake" },
                 new FinancialType { Id = 4, Name = "Transfer", Hide = false, InOut = 0, Icon = "simple-icon-shuffle" },
                 new FinancialType { Id = 5, Name = "Deposit", Hide = false, InOut = 1, Icon = "iconsminds-down-1" },
                 new FinancialType { Id = 6, Name = "Withdrawal", Hide = false, InOut = -1, Icon = "iconsminds-up-1" },
                 new FinancialType { Id = 7, Name = "Fee", Hide = false, InOut = -1, Icon = "iconsminds-receipt-4" },
                 new FinancialType { Id = 8, Name = "Interest", Hide = false, InOut = 1, Icon = "iconsminds-line-chart-1" },
                 new FinancialType { Id = 9, Name = "Cheque", Hide = false, InOut = 0, Icon = "iconsminds-check" },
                 new FinancialType { Id = 10, Name = "Adjustment", Hide = false, InOut = 0, Icon = "iconsminds-gear" }
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

        public void InitialAccountType(OrgContext orgContext)
        {
            // Id is left database-generated: AccountType.Id is a SQL Server identity column, and
            // an explicit-value insert (as every other Initial* seed above does with Id = 1, 2, ...)
            // requires SET IDENTITY_INSERT, which is not available from inside the EF Core
            // migration-seeding transaction here (confirmed by SqlException 544 when this was tried).
            // Idempotency and lookups are done by Name instead — the unique business key for this table.
            //
            // DebitOrCredit follows the same +1/-1/0 direction convention already used by
            // TransactionType.InOut / FinancialType.InOut in this file: 1 = Debit nature,
            // -1 = Credit nature, 0 = Variable (control/closing accounts).
            List<AccountType> list = new List<AccountType> {
                new AccountType { Name = "Asset", DebitOrCredit = 1, Hide = false },
                new AccountType { Name = "Contra Asset", DebitOrCredit = -1, Hide = false },
                new AccountType { Name = "Liability", DebitOrCredit = -1, Hide = false },
                new AccountType { Name = "Equity", DebitOrCredit = -1, Hide = false },
                new AccountType { Name = "Contra Equity", DebitOrCredit = 1, Hide = false },
                new AccountType { Name = "Revenue", DebitOrCredit = -1, Hide = false },
                new AccountType { Name = "Contra Revenue", DebitOrCredit = 1, Hide = false },
                new AccountType { Name = "Cost", DebitOrCredit = 1, Hide = false },
                new AccountType { Name = "Expense", DebitOrCredit = 1, Hide = false },
                new AccountType { Name = "Control", DebitOrCredit = 0, Hide = false },
                new AccountType { Name = "Closing", DebitOrCredit = 0, Hide = false }
            };

            foreach (var ob in list)
            {
                var existing = orgContext.AccountTypes.FirstOrDefault(e => e.Name == ob.Name);
                if (existing == null)
                    orgContext.Set<AccountType>().Add(ob);
                else
                    existing.DebitOrCredit = ob.DebitOrCredit;
            }
            orgContext.SaveChanges();
        }

        public void InitialAccount(OrgContext orgContext)
        {
            // Id is left database-generated, for the same reason as InitialAccountType above
            // (Account.Id is an identity column; explicit-value inserts fail with SqlException 544
            // inside this seeding transaction). Rows are listed parent-before-child (already the
            // natural order of a chart of accounts), and ParentId / AccountTypeId are resolved from
            // real generated Ids via lookup maps built as each row is processed — Code -> Account.Id
            // and AccountType.Name -> AccountType.Id — so re-seeding stays fully deterministic
            // regardless of the underlying identity values. Idempotency is by Code (the account's
            // business key), per row, immediately below.
            var accountTypeIdByName = orgContext.AccountTypes.ToDictionary(e => e.Name!, e => e.Id);

            // (Code, ParentCode, EnglishName, AccountTypeName) — ParentCode is null for the 7 root accounts.
            var rows = new (string Code, string? ParentCode, string Name, string AccountTypeName)[]
            {
                // Assets
                ("1", null, "Assets", "Asset"),
                ("11", "1", "Current Assets", "Asset"),
                ("1101", "11", "Cash and Cash Equivalents", "Asset"),
                ("110101", "1101", "Main Cash", "Asset"),
                ("110102", "1101", "Branch Cash", "Asset"),
                ("110103", "1101", "Cash Advances", "Asset"),
                ("1102", "11", "Banks", "Asset"),
                ("110201", "1102", "Bank - Current Account", "Asset"),
                ("110202", "1102", "Bank - Savings Account", "Asset"),
                ("110203", "1102", "Bank Transfers in Transit", "Asset"),
                ("1103", "11", "Receivables", "Asset"),
                ("110301", "1103", "Local Customers", "Asset"),
                ("110302", "1103", "Foreign Customers", "Asset"),
                ("110303", "1103", "Notes Receivable", "Asset"),
                ("110304", "1103", "Allowance for Doubtful Debts", "Contra Asset"),
                ("1104", "11", "Inventory", "Asset"),
                ("110401", "1104", "Merchandise Inventory", "Asset"),
                ("110402", "1104", "Raw Materials Inventory", "Asset"),
                ("110403", "1104", "Work in Process Inventory", "Asset"),
                ("110404", "1104", "Finished Goods Inventory", "Asset"),
                ("110405", "1104", "Inventory Obsolescence Allowance", "Contra Asset"),
                ("1105", "11", "Prepayments and Advances", "Asset"),
                ("110501", "1105", "Supplier Advances", "Asset"),
                ("110502", "1105", "Prepaid Expenses", "Asset"),
                ("110503", "1105", "Refundable Deposits", "Asset"),
                ("1106", "11", "Tax Receivables", "Asset"),
                ("110601", "1106", "Input VAT", "Asset"),
                ("110602", "1106", "Withholding Tax Receivable", "Asset"),
                ("110603", "1106", "Advance Income Tax", "Asset"),

                ("12", "1", "Non-current Assets", "Asset"),
                ("1201", "12", "Property, Plant and Equipment", "Asset"),
                ("120101", "1201", "Land", "Asset"),
                ("120102", "1201", "Buildings", "Asset"),
                ("120103", "1201", "Vehicles", "Asset"),
                ("120104", "1201", "Machinery and Equipment", "Asset"),
                ("120105", "1201", "Furniture and Fixtures", "Asset"),
                ("120106", "1201", "Computer Equipment", "Asset"),
                ("1202", "12", "Accumulated Depreciation", "Contra Asset"),
                ("120201", "1202", "Accumulated Depreciation - Buildings", "Contra Asset"),
                ("120202", "1202", "Accumulated Depreciation - Vehicles", "Contra Asset"),
                ("120203", "1202", "Accumulated Depreciation - Equipment", "Contra Asset"),
                ("120204", "1202", "Accumulated Depreciation - Furniture", "Contra Asset"),
                ("120205", "1202", "Accumulated Depreciation - Computers", "Contra Asset"),
                ("1203", "12", "Intangible Assets", "Asset"),
                ("120301", "1203", "Software and Licenses", "Asset"),
                ("120302", "1203", "Goodwill", "Asset"),
                ("120303", "1203", "Accumulated Amortization", "Contra Asset"),

                // Liabilities
                ("2", null, "Liabilities", "Liability"),
                ("21", "2", "Current Liabilities", "Liability"),
                ("2101", "21", "Payables", "Liability"),
                ("210101", "2101", "Local Suppliers", "Liability"),
                ("210102", "2101", "Foreign Suppliers", "Liability"),
                ("210103", "2101", "Notes Payable", "Liability"),
                ("2102", "21", "Accrued Liabilities", "Liability"),
                ("210201", "2102", "Accrued Expenses", "Liability"),
                ("210202", "2102", "Salaries Payable", "Liability"),
                ("210203", "2102", "Social Insurance Payable", "Liability"),
                ("210204", "2102", "Commissions Payable", "Liability"),
                ("2103", "21", "Taxes Payable", "Liability"),
                ("210301", "2103", "Output VAT", "Liability"),
                ("210302", "2103", "VAT Payable", "Liability"),
                ("210303", "2103", "Withholding Tax Payable", "Liability"),
                ("210304", "2103", "Income Tax Payable", "Liability"),
                ("210305", "2103", "Schedule / Excise Tax Payable", "Liability"),
                ("2104", "21", "Customer Advances", "Liability"),
                ("2105", "21", "Short-term Loans", "Liability"),
                ("2106", "21", "Other Payables", "Liability"),
                ("22", "2", "Non-current Liabilities", "Liability"),
                ("2201", "22", "Long-term Loans", "Liability"),
                ("2202", "22", "Lease Liabilities", "Liability"),
                ("2203", "22", "Long-term Provisions", "Liability"),

                // Equity
                ("3", null, "Equity", "Equity"),
                ("3101", "3", "Capital", "Equity"),
                ("3102", "3", "Legal Reserve", "Equity"),
                ("3103", "3", "Other Reserves", "Equity"),
                ("3104", "3", "Retained Earnings", "Equity"),
                ("3105", "3", "Current Year Profit or Loss", "Equity"),
                ("3106", "3", "Owner Drawings", "Contra Equity"),

                // Revenue
                ("4", null, "Revenue", "Revenue"),
                ("4101", "4", "Sales Revenue", "Revenue"),
                ("410101", "4101", "Merchandise Sales", "Revenue"),
                ("410102", "4101", "Product Sales", "Revenue"),
                ("410103", "4101", "Service Revenue", "Revenue"),
                ("410104", "4101", "Subscription Revenue", "Revenue"),
                ("4102", "4", "Sales Returns and Discounts", "Contra Revenue"),
                ("410201", "4102", "Sales Returns", "Contra Revenue"),
                ("410202", "4102", "Sales Discounts Allowed", "Contra Revenue"),
                ("410203", "4102", "Trade Discounts on Sales", "Contra Revenue"),
                ("4201", "4", "Other Income", "Revenue"),
                ("420101", "4201", "Interest Income", "Revenue"),
                ("420102", "4201", "Gain on Disposal of Assets", "Revenue"),
                ("420103", "4201", "Foreign Exchange Gain", "Revenue"),

                // Cost of Sales
                ("5", null, "Cost of Sales", "Cost"),
                ("5101", "5", "Cost of Goods Sold", "Cost"),
                ("5102", "5", "Cost of Services", "Cost"),
                ("5103", "5", "Manufacturing Cost", "Cost"),
                ("510301", "5103", "Direct Materials", "Cost"),
                ("510302", "5103", "Direct Labor", "Cost"),
                ("510303", "5103", "Manufacturing Overhead", "Cost"),

                // Expenses
                ("6", null, "Expenses", "Expense"),
                ("6101", "6", "Selling and Marketing Expenses", "Expense"),
                ("610101", "6101", "Advertising and Marketing", "Expense"),
                ("610102", "6101", "Sales Commissions", "Expense"),
                ("610103", "6101", "Delivery Expense", "Expense"),
                ("6201", "6", "General and Administrative Expenses", "Expense"),
                ("620101", "6201", "Salaries and Wages", "Expense"),
                ("620102", "6201", "Rent Expense", "Expense"),
                ("620103", "6201", "Utilities Expense", "Expense"),
                ("620104", "6201", "Telecommunications", "Expense"),
                ("620105", "6201", "Office Supplies", "Expense"),
                ("620106", "6201", "Repairs and Maintenance", "Expense"),
                ("620107", "6201", "Vehicle and Transportation", "Expense"),
                ("620108", "6201", "Insurance Expense", "Expense"),
                ("620109", "6201", "Professional Fees", "Expense"),
                ("620110", "6201", "Government Fees", "Expense"),
                ("620111", "6201", "Hospitality and Cleaning", "Expense"),
                ("620112", "6201", "Training and Development", "Expense"),
                ("620113", "6201", "Subscriptions and Cloud Software", "Expense"),
                ("6202", "6", "Depreciation and Amortization", "Expense"),
                ("620201", "6202", "Depreciation - Buildings", "Expense"),
                ("620202", "6202", "Depreciation - Vehicles", "Expense"),
                ("620203", "6202", "Depreciation - Equipment", "Expense"),
                ("620204", "6202", "Amortization - Software", "Expense"),
                ("6301", "6", "Finance Costs", "Expense"),
                ("630101", "6301", "Interest and Bank Charges", "Expense"),
                ("630102", "6301", "Foreign Exchange Loss", "Expense"),
                ("6401", "6", "Other Expenses", "Expense"),
                ("640101", "6401", "Loss on Disposal of Assets", "Expense"),
                ("640102", "6401", "Bad Debts Expense", "Expense"),
                ("640103", "6401", "Fines and Penalties", "Expense"),

                // Closing and Control Accounts
                ("7", null, "Closing and Control Accounts", "Control"),
                ("7101", "7", "Profit and Loss Summary", "Closing"),
                ("7102", "7", "Inventory Adjustment", "Control"),
                ("7103", "7", "Rounding Differences", "Control"),
                ("7104", "7", "Suspense Account", "Control")
            };

            // A code that appears as some other row's ParentCode is a group/parent account — postings
            // (including a Dealer's receivable account link) must target a leaf below it instead.
            var parentCodes = rows.Where(e => e.ParentCode != null).Select(e => e.ParentCode!).ToHashSet();

            var idByCode = new Dictionary<string, long>();

            foreach (var row in rows)
            {
                long parentId = row.ParentCode == null ? 0 : idByCode[row.ParentCode];
                long accountTypeId = accountTypeIdByName[row.AccountTypeName];
                long codeNumber = long.Parse(row.Code);
                bool isPostable = !parentCodes.Contains(row.Code);

                var existing = orgContext.Accounts.FirstOrDefault(e => e.Code == row.Code);
                if (existing == null)
                {
                    var account = new Account
                    {
                        Code = row.Code,
                        CodeNumber = codeNumber,
                        Name = row.Name,
                        ParentId = parentId,
                        AccountTypeId = accountTypeId,
                        Debit = 0,
                        Credit = 0,
                        Hide = false,
                        IsPostable = isPostable
                    };
                    orgContext.Set<Account>().Add(account);
                    orgContext.SaveChanges();
                    idByCode[row.Code] = account.Id;
                }
                else
                {
                    // Update seeded master data only — never touch transactional Debit/Credit balances.
                    existing.Name = row.Name;
                    existing.CodeNumber = codeNumber;
                    existing.ParentId = parentId;
                    existing.AccountTypeId = accountTypeId;
                    existing.Hide = false;
                    existing.IsPostable = isPostable;
                    orgContext.SaveChanges();
                    idByCode[row.Code] = existing.Id;
                }
            }
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

            //foreach (var ob in list)
            //{
            //    ob.Id = orgContext.CompanyProfiles.FirstOrDefault(e => e.Name == ob.Name)?.Id ?? 0;
            //    if (ob.Id == 0)
            //        orgContext.Set<CompanyProfile>().AddRange(ob);
            //    else
            //        orgContext.Entry<CompanyProfile>(orgContext.Set<CompanyProfile>().Find(ob.Id)).CurrentValues.SetValues(ob);
            //}
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

        // Country/City/District are plain BaseModel lockups (Name + Code only — no NameAr/ISO2/ISO3/
        // PhoneCode columns exist on these entities, so this seed doesn't add any), identity-Id like
        // everything else in this file. Idempotency is by business key, not Id: Country by Code (its
        // ISO2), City by (CountryId, Name), District by (CityId, Name) — matching InitialAccount's
        // Code-based approach, since two different countries/cities can legitimately share a Name.
        public void InitialCountry(OrgContext orgContext)
        {
            var rows = new (string Code, string Name)[]
            {
                ("EG", "Egypt"),
                ("SA", "Saudi Arabia"),
                ("AE", "United Arab Emirates"),
                ("KW", "Kuwait"),
                ("QA", "Qatar"),
                ("BH", "Bahrain"),
                ("OM", "Oman"),
                ("SD", "Sudan"),
                ("LY", "Libya"),
                ("TN", "Tunisia"),
                ("DZ", "Algeria"),
                ("MA", "Morocco"),
                ("MR", "Mauritania"),
            };

            foreach (var row in rows)
            {
                var existing = orgContext.Countries.FirstOrDefault(e => e.Code == row.Code);
                if (existing == null)
                    orgContext.Set<Country>().Add(new Country { Code = row.Code, Name = row.Name, Hide = false });
                else
                    existing.Name = row.Name;
            }
            orgContext.SaveChanges();
        }

        public void InitialCity(OrgContext orgContext)
        {
            var countryIdByCode = orgContext.Countries.ToDictionary(e => e.Code!, e => e.Id);

            AddCities(orgContext, countryIdByCode["EG"], EgyptCities());
            AddCities(orgContext, countryIdByCode["SA"], SaudiArabiaCities());
            AddCities(orgContext, countryIdByCode["AE"], UAECities());
            AddCities(orgContext, countryIdByCode["KW"], KuwaitCities());
            AddCities(orgContext, countryIdByCode["QA"], QatarCities());
            AddCities(orgContext, countryIdByCode["BH"], BahrainCities());
            AddCities(orgContext, countryIdByCode["OM"], OmanCities());
            AddCities(orgContext, countryIdByCode["SD"], SudanCities());
            AddCities(orgContext, countryIdByCode["LY"], LibyaCities());
            AddCities(orgContext, countryIdByCode["TN"], TunisiaCities());
            AddCities(orgContext, countryIdByCode["DZ"], AlgeriaCities());
            AddCities(orgContext, countryIdByCode["MA"], MoroccoCities());
            AddCities(orgContext, countryIdByCode["MR"], MauritaniaCities());

            orgContext.SaveChanges();
        }

        private static void AddCities(OrgContext orgContext, long countryId, string[] names)
        {
            foreach (var name in names)
            {
                var existing = orgContext.Cities.FirstOrDefault(e => e.CountryId == countryId && e.Name == name);
                if (existing == null)
                    orgContext.Set<City>().Add(new City { CountryId = countryId, Name = name, Hide = false });
            }
        }

        private static string[] EgyptCities() => new[]
        {
            "Cairo", "Giza", "Alexandria", "Dakahlia", "Red Sea", "Beheira", "Fayoum", "Gharbia",
            "Ismailia", "Menofia", "Minya", "Qalyubia", "New Valley", "Suez", "Aswan", "Assiut",
            "Beni Suef", "Port Said", "Damietta", "Sharqia", "South Sinai", "Kafr El Sheikh",
            "Matrouh", "Luxor", "Qena", "North Sinai", "Sohag",
        };

        private static string[] SaudiArabiaCities() => new[]
        {
            "Riyadh", "Jeddah", "Mecca", "Medina", "Dammam", "Khobar", "Dhahran", "Taif", "Tabuk",
            "Abha", "Khamis Mushait", "Buraidah", "Hail", "Jubail", "Yanbu", "Jazan", "Najran", "Al Ahsa",
        };

        private static string[] UAECities() => new[]
        {
            "Abu Dhabi", "Dubai", "Sharjah", "Ajman", "Umm Al Quwain", "Ras Al Khaimah", "Fujairah", "Al Ain",
        };

        private static string[] KuwaitCities() => new[]
        {
            "Kuwait City", "Hawalli", "Salmiya", "Farwaniya", "Jahra", "Ahmadi", "Mubarak Al-Kabeer",
        };

        private static string[] QatarCities() => new[]
        {
            "Doha", "Al Rayyan", "Al Wakrah", "Al Khor", "Umm Salal", "Lusail",
        };

        private static string[] BahrainCities() => new[]
        {
            "Manama", "Muharraq", "Riffa", "Hamad Town", "Isa Town",
        };

        private static string[] OmanCities() => new[]
        {
            "Muscat", "Salalah", "Sohar", "Nizwa", "Sur", "Barka", "Seeb",
        };

        private static string[] SudanCities() => new[]
        {
            "Khartoum", "Omdurman", "Khartoum North", "Port Sudan", "Kassala", "Gedaref",
            "Wad Madani", "El Obeid", "Nyala", "Atbara",
        };

        private static string[] LibyaCities() => new[]
        {
            "Tripoli", "Benghazi", "Misrata", "Sabha", "Sirte", "Zawiya", "Tobruk",
        };

        private static string[] TunisiaCities() => new[]
        {
            "Tunis", "Sfax", "Sousse", "Bizerte", "Kairouan", "Gabes", "Monastir",
        };

        private static string[] AlgeriaCities() => new[]
        {
            "Algiers", "Oran", "Constantine", "Annaba", "Blida", "Setif", "Batna", "Tlemcen",
        };

        private static string[] MoroccoCities() => new[]
        {
            "Casablanca", "Rabat", "Marrakech", "Fez", "Tangier", "Agadir", "Meknes", "Oujda", "Tetouan",
        };

        private static string[] MauritaniaCities() => new[]
        {
            "Nouakchott", "Nouadhibou", "Rosso", "Kaedi", "Atar",
        };

        public void InitialDistrict(OrgContext orgContext)
        {
            var countryIdByCode = orgContext.Countries.ToDictionary(e => e.Code!, e => e.Id);
            var cityIdByCountryAndName = orgContext.Cities
                .Where(e => e.CountryId != null)
                .ToDictionary(e => (e.CountryId!.Value, e.Name!), e => e.Id);

            AddDistricts(orgContext, countryIdByCode["EG"], cityIdByCountryAndName, EgyptDistricts());
            AddDistricts(orgContext, countryIdByCode["SA"], cityIdByCountryAndName, SaudiArabiaDistricts());
            AddDistricts(orgContext, countryIdByCode["AE"], cityIdByCountryAndName, UAEDistricts());

            orgContext.SaveChanges();
        }

        private static void AddDistricts(OrgContext orgContext, long countryId,
            Dictionary<(long CountryId, string CityName), long> cityIdByCountryAndName, (string City, string District)[] rows)
        {
            foreach (var row in rows)
            {
                // Skip silently if the city lookup ever falls out of sync with the district list above —
                // seeding the rest of the batch matters more than failing the whole run over one row.
                if (!cityIdByCountryAndName.TryGetValue((countryId, row.City), out var cityId))
                    continue;

                var existing = orgContext.Districts.FirstOrDefault(e => e.CityId == cityId && e.Name == row.District);
                if (existing == null)
                    orgContext.Set<District>().Add(new District { CountryId = countryId, CityId = cityId, Name = row.District, Hide = false });
            }
        }

        private static (string City, string District)[] EgyptDistricts()
        {
            var cairo = new[]
            {
                "Nasr City", "Heliopolis", "New Cairo", "Fifth Settlement", "First Settlement", "Third Settlement",
                "Maadi", "Zahraa El Maadi", "Mokattam", "Dar El Salam", "Basateen", "Sayeda Zeinab",
                "Downtown Cairo", "Abdeen", "Zamalek", "Shubra", "Ain Shams", "Matariya", "Zeitoun",
                "Hadayek El Kobba", "El Marg", "Salam City", "Nozha", "Badr City", "El Shorouk",
                "New Administrative Capital", "Helwan", "15 May City",
            };
            var giza = new[]
            {
                "Dokki", "Mohandessin", "Agouza", "Haram", "Faisal", "Omrania", "Boulaq El Dakrour",
                "Imbaba", "Warraq", "Sheikh Zayed", "6th of October", "Hadayek October", "New Giza",
                "Kerdasa", "Abu Rawash", "Hawamdia", "Badrasheen",
            };
            var alexandria = new[]
            {
                "Montaza", "Miami", "Sidi Bishr", "San Stefano", "Gleem", "Roushdy", "Smouha", "Sidi Gaber",
                "Sporting", "Stanley", "Louran", "Mandara", "Asafra", "Agami", "Borg El Arab",
                "New Borg El Arab", "Moharam Bek", "Mansheya",
            };
            // Remaining 24 governorates: main markaz/towns, not exhaustive — enough to make the District
            // dropdown usable everywhere without trying to model every markaz in the country.
            var rest = new (string City, string[] Districts)[]
            {
                ("Dakahlia", new[] { "Mansoura", "Talkha", "Mit Ghamr", "Aga" }),
                ("Red Sea", new[] { "Hurghada", "Safaga", "Marsa Alam", "Ras Gharib" }),
                ("Beheira", new[] { "Damanhur", "Kafr El Dawwar", "Rashid", "Edku" }),
                ("Fayoum", new[] { "Fayoum City", "Sinnuris", "Tamiya", "Ibsheway" }),
                ("Gharbia", new[] { "Tanta", "Mahalla El Kubra", "Kafr El Zayat", "Zefta" }),
                ("Ismailia", new[] { "Ismailia City", "Fayed", "Qantara", "Tel El Kebir" }),
                ("Menofia", new[] { "Shibin El Kom", "Sadat City", "Menouf", "Ashmoun" }),
                ("Minya", new[] { "Minya City", "Mallawi", "Beni Mazar", "Samalut" }),
                ("Qalyubia", new[] { "Banha", "Shubra El Kheima", "Qalyub", "Khanka", "Obour City" }),
                ("New Valley", new[] { "Kharga", "Dakhla", "Farafra" }),
                ("Suez", new[] { "Suez City", "Ain Sokhna" }),
                ("Aswan", new[] { "Aswan City", "Kom Ombo", "Edfu", "Daraw" }),
                ("Assiut", new[] { "Assiut City", "Dairut", "Manfalut", "Abnub" }),
                ("Beni Suef", new[] { "Beni Suef City", "El Wasta", "Nasser", "Biba" }),
                ("Port Said", new[] { "Port Fouad", "Al Manakh", "Al Zohour" }),
                ("Damietta", new[] { "Damietta City", "New Damietta", "Faraskur", "Ras El Bar" }),
                ("Sharqia", new[] { "Zagazig", "Belbeis", "Abu Kabir", "10th of Ramadan City" }),
                ("South Sinai", new[] { "Sharm El Sheikh", "Dahab", "Nuweiba", "Taba", "Saint Catherine" }),
                ("Kafr El Sheikh", new[] { "Kafr El Sheikh City", "Desouk", "Fuwwah", "Baltim" }),
                ("Matrouh", new[] { "Marsa Matrouh", "Siwa", "El Alamein", "El Dabaa" }),
                ("Luxor", new[] { "Luxor City", "Esna", "Armant" }),
                ("Qena", new[] { "Qena City", "Nag Hammadi", "Qus", "Deshna" }),
                ("North Sinai", new[] { "Arish", "Sheikh Zuweid", "Rafah", "Bir al-Abd" }),
                ("Sohag", new[] { "Sohag City", "Akhmim", "Girga", "Tahta" }),
            };

            return Combine("Cairo", cairo)
                .Concat(Combine("Giza", giza))
                .Concat(Combine("Alexandria", alexandria))
                .Concat(rest.SelectMany(g => Combine(g.City, g.Districts)))
                .ToArray();
        }

        private static (string City, string District)[] SaudiArabiaDistricts()
        {
            var groups = new (string City, string[] Districts)[]
            {
                ("Riyadh", new[] { "Olaya", "Malaz", "Al Naseem", "Al Malqa", "Al Sulimaniyah", "Al Murabba", "Diriyah" }),
                ("Jeddah", new[] { "Al Rawdah", "Al Salamah", "Al Hamra", "Al Zahra", "Al Naeem", "Al Balad", "Obhur" }),
                ("Mecca", new[] { "Al Aziziyah", "Al Shoqiah", "Al Nassim", "Ajyad", "Al Awali" }),
                ("Medina", new[] { "Al Aziziyah", "Quba", "Al Haram", "Al Ranuna", "Al Awali" }),
                ("Dammam", new[] { "Al Faisaliyah", "Al Shati", "Al Rakah", "Al Adamah", "Al Manar" }),
                ("Khobar", new[] { "Al Aqrabiyah", "Al Ulaya", "Al Thuqbah", "Al Yarmouk", "Corniche" }),
            };
            return groups.SelectMany(g => Combine(g.City, g.Districts)).ToArray();
        }

        private static (string City, string District)[] UAEDistricts()
        {
            var groups = new (string City, string[] Districts)[]
            {
                ("Dubai", new[] { "Deira", "Bur Dubai", "Jumeirah", "Downtown Dubai", "Dubai Marina", "Al Barsha", "Business Bay", "Al Qusais" }),
                ("Abu Dhabi", new[] { "Al Khalidiyah", "Al Bateen", "Al Reem Island", "Al Zahiyah", "Mussafah", "Khalifa City", "Al Muroor" }),
                ("Sharjah", new[] { "Al Majaz", "Al Qasimia", "Al Nahda", "Al Taawun", "Al Khan", "Muwaileh" }),
            };
            return groups.SelectMany(g => Combine(g.City, g.Districts)).ToArray();
        }

        private static IEnumerable<(string City, string District)> Combine(string city, string[] districts) =>
            districts.Select(d => (city, d));

        // Bank/BankBranch: same idempotency approach as Country/City/District (business key, not Id),
        // but batched rather than per-row queried — existing Banks/BankBranches are loaded once up front
        // into lookup structures and reconciled in memory, then written with a single AddRange +
        // SaveChanges per method, since this seed is an order of magnitude larger (~500 branch candidates)
        // than the per-row InitialAccount/InitialDistrict pattern comfortably handles as N+1 queries.
        // No SWIFT/branch codes are seeded — none were supplied with confirmed values, and Code sits
        // unused (null) rather than invented, so the fallback natural keys below are always what's used:
        // Bank by (CountryId, Name), BankBranch by (BankId, CityId, DistrictId, Name).
        public void InitialBank(OrgContext orgContext)
        {
            var countryIdByCode = orgContext.Countries.ToDictionary(e => e.Code!, e => e.Id);
            var existingBanks = orgContext.Banks
                .Where(e => e.CountryId != null)
                .Select(e => new { e.CountryId, e.Name })
                .AsEnumerable()
                .Select(e => (e.CountryId!.Value, e.Name!))
                .ToHashSet();

            var toAdd = new List<Bank>();
            void Collect(string countryCode, string[] names)
            {
                var countryId = countryIdByCode[countryCode];
                foreach (var name in names)
                    if (!existingBanks.Contains((countryId, name)))
                        toAdd.Add(new Bank { CountryId = countryId, Name = name, Hide = false });
            }

            Collect("EG", EgyptBanks());
            Collect("SA", SaudiArabiaBanks());
            Collect("AE", UAEBanks());
            Collect("KW", KuwaitBanks());
            Collect("QA", QatarBanks());
            Collect("BH", BahrainBanks());
            Collect("OM", OmanBanks());
            Collect("SD", SudanBanks());
            Collect("LY", LibyaBanks());
            Collect("TN", TunisiaBanks());
            Collect("DZ", AlgeriaBanks());
            Collect("MA", MoroccoBanks());
            Collect("MR", MauritaniaBanks());

            if (toAdd.Count > 0)
            {
                orgContext.Set<Bank>().AddRange(toAdd);
                orgContext.SaveChanges();
            }
        }

        private static string[] EgyptBanks() => new[]
        {
            // Government / major banks
            "National Bank of Egypt", "Banque Misr", "Banque du Caire", "Agricultural Bank of Egypt",
            // Private / commercial / foreign-subsidiary banks
            "Commercial International Bank (CIB)", "QNB Egypt", "Arab African International Bank", "AlexBank",
            "Credit Agricole Egypt", "HSBC Bank Egypt", "Emirates NBD Egypt", "Abu Dhabi Islamic Bank Egypt (ADIB)",
            "Abu Dhabi Commercial Bank Egypt (ADCB)", "Mashreq Bank Egypt", "First Abu Dhabi Bank Egypt (FABMISR)",
            "Arab International Bank", "Arab Bank Egypt", "Kuwait Finance House Egypt",
            "Housing and Development Bank", "Suez Canal Bank", "Export Development Bank of Egypt",
            "Egyptian Gulf Bank (EG Bank)", "Al Baraka Bank Egypt", "Faisal Islamic Bank of Egypt",
            "MIDBANK", "saib", "Industrial Development Bank", "The United Bank",
        };

        private static string[] SaudiArabiaBanks() => new[]
        {
            "Saudi National Bank (SNB)", "Al Rajhi Bank", "Riyad Bank", "Alinma Bank", "Bank Albilad",
            "Bank AlJazira", "Saudi Awwal Bank (SAB)", "Arab National Bank (ANB)", "Banque Saudi Fransi",
            "Gulf International Bank Saudi Arabia",
        };

        private static string[] UAEBanks() => new[]
        {
            "First Abu Dhabi Bank (FAB)", "Emirates NBD", "Abu Dhabi Commercial Bank (ADCB)",
            "Abu Dhabi Islamic Bank (ADIB)", "Mashreq", "Dubai Islamic Bank", "Emirates Islamic",
            "Commercial Bank of Dubai", "RAKBANK", "Sharjah Islamic Bank", "National Bank of Fujairah",
        };

        private static string[] KuwaitBanks() => new[]
        {
            "National Bank of Kuwait", "Kuwait Finance House", "Gulf Bank", "Commercial Bank of Kuwait",
            "Burgan Bank", "Boubyan Bank", "Warba Bank", "Kuwait International Bank", "Al Ahli Bank of Kuwait",
        };

        private static string[] QatarBanks() => new[]
        {
            "Qatar National Bank (QNB)", "Qatar Islamic Bank (QIB)", "Commercial Bank Qatar", "Doha Bank",
            "Dukhan Bank", "Qatar International Islamic Bank", "Ahlibank Qatar", "Masraf Al Rayan",
        };

        private static string[] BahrainBanks() => new[]
        {
            "National Bank of Bahrain", "Bank of Bahrain and Kuwait", "Al Salam Bank", "Bahrain Islamic Bank",
            "Ahli United Bank", "Gulf International Bank", "ila Bank",
        };

        private static string[] OmanBanks() => new[]
        {
            "Bank Muscat", "Bank Dhofar", "National Bank of Oman", "Sohar International", "Oman Arab Bank",
            "Bank Nizwa", "Ahli Bank Oman",
        };

        private static string[] SudanBanks() => new[]
        {
            "Bank of Khartoum", "Omdurman National Bank", "Faisal Islamic Bank Sudan",
            "Sudanese French Bank", "Blue Nile Mashreg Bank",
        };

        private static string[] LibyaBanks() => new[]
        {
            "Jumhouria Bank", "National Commercial Bank (Libya)", "Sahara Bank", "Wahda Bank",
            "Bank of Commerce and Development",
        };

        private static string[] TunisiaBanks() => new[]
        {
            "Banque Nationale Agricole (BNA)", "Société Tunisienne de Banque (STB)", "Banque de l'Habitat (BH Bank)",
            "Attijari Bank Tunisie", "Banque Internationale Arabe de Tunisie (BIAT)", "Amen Bank",
            "Al Baraka Bank Tunisia",
        };

        private static string[] AlgeriaBanks() => new[]
        {
            "Banque Extérieure d'Algérie (BEA)", "Banque Nationale d'Algérie (BNA)",
            "Crédit Populaire d'Algérie (CPA)", "Banque de Développement Local (BDL)",
            "Société Générale Algérie", "BNP Paribas El Djazair",
        };

        private static string[] MoroccoBanks() => new[]
        {
            "Attijariwafa Bank", "Banque Populaire", "Bank of Africa", "CIH Bank", "Crédit du Maroc",
            "Société Générale Maroc",
        };

        private static string[] MauritaniaBanks() => new[]
        {
            "Banque Nationale de Mauritanie (BNM)", "Générale de Banque de Mauritanie (GBM)", "Chinguitty Bank",
        };

        // Only Egypt/Saudi Arabia/UAE get branches — the only countries with District-level geography
        // seeded (BankBranch.CityId/DistrictId are non-nullable FKs, so a branch literally cannot be
        // created without a real district). Egypt's branch list mirrors the district-level detail asked
        // for; Saudi/UAE get one representative "main branch" location per city instead, matching the
        // shallower request for those countries. Every (bank, city, district) triple below resolves
        // against rows InitialCity/InitialDistrict already created — anything that doesn't resolve is
        // skipped rather than guessed at.
        public void InitialBankBranch(OrgContext orgContext)
        {
            var countryIdByCode = orgContext.Countries.ToDictionary(e => e.Code!, e => e.Id);
            var cityIdByCountryAndName = orgContext.Cities
                .Where(e => e.CountryId != null)
                .ToDictionary(e => (e.CountryId!.Value, e.Name!), e => e.Id);
            var districtIdByCityAndName = orgContext.Districts
                .Where(e => e.CityId != null)
                .ToDictionary(e => (e.CityId!.Value, e.Name!), e => e.Id);
            var bankIdByCountryAndName = orgContext.Banks
                .Where(e => e.CountryId != null)
                .ToDictionary(e => (e.CountryId!.Value, e.Name!), e => e.Id);
            var existingBranches = orgContext.BankBranchs
                .Select(e => new { e.BankId, e.CityId, e.DistrictId, e.Name })
                .AsEnumerable()
                .Select(e => (e.BankId, e.CityId, e.DistrictId, e.Name!))
                .ToHashSet();

            var toAdd = new List<BankBranch>();
            void Collect(string countryCode, string[] bankNames, (string City, string District)[] locations)
            {
                if (!countryIdByCode.TryGetValue(countryCode, out var countryId)) return;
                foreach (var bankName in bankNames)
                {
                    if (!bankIdByCountryAndName.TryGetValue((countryId, bankName), out var bankId)) continue;
                    foreach (var loc in locations)
                    {
                        if (!cityIdByCountryAndName.TryGetValue((countryId, loc.City), out var cityId)) continue;
                        if (!districtIdByCityAndName.TryGetValue((cityId, loc.District), out var districtId)) continue;

                        var key = (bankId, cityId, districtId, loc.District);
                        if (!existingBranches.Add(key)) continue; // already in DB, or a duplicate location in this run

                        toAdd.Add(new BankBranch
                        {
                            BankId = bankId,
                            CountryId = countryId,
                            CityId = cityId,
                            DistrictId = districtId,
                            Name = loc.District,
                            Hide = false,
                        });
                    }
                }
            }

            Collect("EG", EgyptBranchBanks(), EgyptBranchLocations());
            Collect("SA", SaudiArabiaBranchBanks(), SaudiArabiaBranchLocations());
            Collect("AE", UAEBranchBanks(), UAEBranchLocations());

            if (toAdd.Count > 0)
            {
                orgContext.Set<BankBranch>().AddRange(toAdd);
                orgContext.SaveChanges();
            }
        }

        // The 5 Egyptian banks with a genuinely nationwide branch network — confident enough to claim
        // presence in every district below. The other 23 Egypt banks (foreign-subsidiary, specialised or
        // smaller commercial banks) are seeded with no branches at all rather than guessing their footprint.
        private static string[] EgyptBranchBanks() => new[]
        {
            "National Bank of Egypt", "Banque Misr", "Banque du Caire",
            "Commercial International Bank (CIB)", "QNB Egypt",
        };

        private static (string City, string District)[] EgyptBranchLocations()
        {
            var cairo = new[]
            {
                "Downtown Cairo", "Nasr City", "Heliopolis", "New Cairo", "Fifth Settlement", "Maadi",
                "Zamalek", "Shubra", "Mokattam", "Helwan", "Badr City", "El Shorouk",
            };
            var giza = new[] { "Dokki", "Mohandessin", "Faisal", "Haram", "Sheikh Zayed", "6th of October" };
            // "Downtown Alexandria" isn't a separate seeded district — Mansheya is Alexandria's actual
            // historic downtown/city-centre district, so it stands in for it rather than inventing a row.
            var alexandria = new[] { "Smouha", "Sidi Gaber", "Roushdy", "Miami", "Montaza", "Mansheya", "Borg El Arab" };
            var others = new (string City, string District)[]
            {
                ("Dakahlia", "Mansoura"), ("Gharbia", "Tanta"), ("Sharqia", "Zagazig"),
                ("Ismailia", "Ismailia City"), ("Suez", "Suez City"), ("Port Said", "Al Manakh"),
                ("Damietta", "Damietta City"), ("Assiut", "Assiut City"), ("Sohag", "Sohag City"),
                ("Minya", "Minya City"), ("Beni Suef", "Beni Suef City"), ("Fayoum", "Fayoum City"),
                ("Luxor", "Luxor City"), ("Aswan", "Aswan City"), ("Red Sea", "Hurghada"),
                ("South Sinai", "Sharm El Sheikh"),
            };

            return Combine("Cairo", cairo)
                .Concat(Combine("Giza", giza))
                .Concat(Combine("Alexandria", alexandria))
                .Concat(others)
                .ToArray();
        }

        // Saudi/UAE: one confirmed, well-known central district per city as the "main branch" location —
        // matching the shallower "أهم الفروع" ask for these countries rather than Egypt's district-by-district
        // detail. Gulf International Bank Saudi Arabia is excluded: it's a wholesale/corporate bank without
        // a retail branch network to place with any confidence.
        private static string[] SaudiArabiaBranchBanks() => new[]
        {
            "Saudi National Bank (SNB)", "Al Rajhi Bank", "Riyad Bank", "Alinma Bank", "Bank Albilad",
            "Bank AlJazira", "Saudi Awwal Bank (SAB)", "Arab National Bank (ANB)", "Banque Saudi Fransi",
        };

        // Jubail, Taif, Abha and Tabuk (also asked for) have no District-level data seeded, so no branch
        // rows can be created there — see the Summary for this gap.
        private static (string City, string District)[] SaudiArabiaBranchLocations() => new[]
        {
            ("Riyadh", "Olaya"), ("Jeddah", "Al Rawdah"), ("Mecca", "Ajyad"),
            ("Medina", "Al Haram"), ("Dammam", "Al Faisaliyah"), ("Khobar", "Corniche"),
        };

        private static string[] UAEBranchBanks() => new[]
        {
            "First Abu Dhabi Bank (FAB)", "Emirates NBD", "Abu Dhabi Commercial Bank (ADCB)",
            "Abu Dhabi Islamic Bank (ADIB)", "Mashreq", "Dubai Islamic Bank", "Emirates Islamic",
            "Commercial Bank of Dubai", "RAKBANK", "Sharjah Islamic Bank", "National Bank of Fujairah",
        };

        // Ajman, Al Ain and Ras Al Khaimah (also asked for) have no District-level data seeded either.
        private static (string City, string District)[] UAEBranchLocations() => new[]
        {
            ("Dubai", "Deira"), ("Abu Dhabi", "Al Khalidiyah"), ("Sharjah", "Al Majaz"),
        };
    }
}
