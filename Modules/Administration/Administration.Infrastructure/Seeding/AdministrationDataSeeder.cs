namespace Administration.Infrastructure.Seeding
{
    using Administration.Application.Security;
    using Microsoft.EntityFrameworkCore;
    using OrgSys.SharedKernel;

    public interface IAdministrationDataSeeder
    {
        void Seed(DbContext dbContext);
    }

    /// <summary>
    /// Administration's slice of the legacy InitialData seed (Permission tree, Preferences,
    /// Role/RolePermission/User) — relocated verbatim, split by module ownership per
    /// docs/modular-monolith-analysis.md. Runs against the shared OrgContext (still one physical
    /// database — see BuildingBlocks/OrgSys.DatabaseMigrator) via the generic DbContext API only,
    /// so this project needs no reference back to the migrator.
    /// </summary>
    public sealed class AdministrationDataSeeder : IAdministrationDataSeeder
    {
        public void Seed(DbContext dbContext)
        {
            InitialPermission(dbContext);
            InitialPreference(dbContext);
            InitialRole(dbContext);
            InitialRolePermission(dbContext);
            InitialUser(dbContext);
        }

        public void InitialPermission(Microsoft.EntityFrameworkCore.DbContext orgContext)
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
                                   new Permission { Id = 5011207, Name = "Post", Key = "Financial.Post", ParentId = 50112, TypeId = 1 },
                                   new Permission { Id = 5011208, Name = "Reverse", Key = "Financial.Reverse", ParentId = 50112, TypeId = 1 },

               };

            foreach (var ob in list)
            {
                if (!orgContext.Set<Permission>().Any(e => e.Id == ob.Id))
                    orgContext.Set<Permission>().Add(ob);
                else
                    orgContext.Entry<Permission>(orgContext.Set<Permission>().Find(ob.Id)).CurrentValues.SetValues(ob);
            }
            orgContext.SaveChanges();
        }

        public void InitialPreference(Microsoft.EntityFrameworkCore.DbContext orgContext)
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
               new Preference { Id = 2004, Key = "OpeningBalanceEquityAccountId", Value = "0", Reference = "Financial", TypeId = 1, Hide = false },
               new Preference { Id = 2005, Key = "CashBoxAccount", Value = "0", Reference = "Financial", TypeId = 1, Hide = false },
               new Preference { Id = 2006, Key = "BankAccount", Value = "0", Reference = "Financial", TypeId = 1, Hide = false },

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

            var openingBalancePurchaseAccount = orgContext.Set<Preference>().FirstOrDefault(e =>
                e.Reference == "Transaction" && e.TypeId == 7 && e.Key == "PurchaseAccount");
            if (openingBalancePurchaseAccount != null)
                openingBalancePurchaseAccount.Key = "OpeningBalanceAccount";

            var obsoleteOpeningBalancePreferences = orgContext.Set<Preference>().Where(e =>
                e.Reference == "Transaction" && e.TypeId == 7 && e.Key == "SalesReturnAccount");
            orgContext.Set<Preference>().RemoveRange(obsoleteOpeningBalancePreferences);

            var inventoryDamagePurchaseAccount = orgContext.Set<Preference>().FirstOrDefault(e =>
                e.Reference == "Transaction" && e.TypeId == 8 && e.Key == "PurchaseAccount");
            if (inventoryDamagePurchaseAccount != null)
                inventoryDamagePurchaseAccount.Key = "InventoryDamageExpenseAccount";

            var obsoleteInventoryDamagePreferences = orgContext.Set<Preference>().Where(e =>
                e.Reference == "Transaction" && e.TypeId == 8 && e.Key == "SalesReturnAccount");
            orgContext.Set<Preference>().RemoveRange(obsoleteInventoryDamagePreferences);

            foreach (var ob in list)
            {
                if (!orgContext.Set<Preference>().Any(e => e.Id == ob.Id))
                    orgContext.Set<Preference>().Add(ob);
                //else
                //    orgContext.Entry<Preference>(orgContext.Set<Preference>().Find(ob.Id)).CurrentValues.SetValues(ob);
            }
            orgContext.SaveChanges();
        }

        public void InitialRole(Microsoft.EntityFrameworkCore.DbContext orgContext)
        {
            List<Role> list = new List<Role> {
                new Role {Name = "Owner", Hide = true },
                new Role {Name = "Admin", Hide = false }
            };

            foreach (var ob in list)
            {
                ob.Id = orgContext.Set<Role>().FirstOrDefault(e => e.Name == ob.Name)?.Id ?? 0;
                if (ob.Id == 0)
                    orgContext.Set<Role>().Add(ob);
                else
                {
                    orgContext.Entry<Role>(orgContext.Set<Role>().Find(ob.Id)).CurrentValues.SetValues(ob);
                }
            }
            orgContext.SaveChanges();
        }

        public void InitialRolePermission(Microsoft.EntityFrameworkCore.DbContext orgContext)
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
                ob.Id = orgContext.Set<RolePermission>().FirstOrDefault(e => e.RoleId == ob.RoleId && e.PermissionId == ob.PermissionId)?.Id ?? 0;
                if (ob.Id == 0)
                    orgContext.Set<RolePermission>().AddRange(ob);
                else
                    orgContext.Entry<RolePermission>(orgContext.Set<RolePermission>().Find(ob.Id)).CurrentValues.SetValues(ob);
            }
            orgContext.SaveChanges();
        }

        public void InitialUser(Microsoft.EntityFrameworkCore.DbContext orgContext)
        {
            List<User> list = new List<User> {
                 new User {Name = "Owner", UserName = "Owner", Password = new PasswordHasher().HashPassword("P@ssw0rd"), MustResetPassword = false, RoleId = orgContext.Set<Role>().FirstOrDefault(e => e.Name == "Owner").Id, LoginUserId = 1, Hide = true },
                 new User {Name = "Admin", UserName = "Admin", Password = new PasswordHasher().HashPassword("P@ssw0rd"), MustResetPassword = false, RoleId = orgContext.Set<Role>().FirstOrDefault(e => e.Name == "Admin").Id, LoginUserId = 2, Hide = false },
                 new User {Name = "Emp", UserName = "Admin2", RoleId = 2, Hide = false, MustResetPassword = true }
            };

            foreach (var ob in list)
            {
                ob.Id = orgContext.Set<User>().FirstOrDefault(e => e.Name == ob.Name)?.Id ?? 0;
                if (ob.Id == 0)
                    orgContext.Set<User>().Add(ob);
                else
                    orgContext.Entry<User>(orgContext.Set<User>().Find(ob.Id)).CurrentValues.SetValues(ob);
            }
            orgContext.SaveChanges();
        }
    }
}
