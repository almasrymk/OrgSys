namespace Accounting.Infrastructure.Seeding
{
    using Microsoft.EntityFrameworkCore;

    public interface IAccountingDataSeeder
    {
        void Seed(DbContext dbContext);
    }

    /// <summary>
    /// Accounting's slice of the legacy InitialData seed (JournalType, AccountType, Chart of
    /// Accounts) — relocated verbatim, split by module ownership. See AdministrationDataSeeder
    /// for the shared rationale.
    /// </summary>
    public sealed class AccountingDataSeeder : IAccountingDataSeeder
    {
        public void Seed(DbContext dbContext)
        {
            InitialJournalType(dbContext);
            InitialAccountType(dbContext);
            InitialAccount(dbContext);
        }

        public void InitialJournalType(Microsoft.EntityFrameworkCore.DbContext orgContext)
        {
            List<JournalType> list = new List<JournalType> {
                    new JournalType { Id = 1, Group = "Journal", Name = "Openning Balance", Hide = false, IsOpeningBlance = true, Icon = "simple-icon-basket-loaded" },
                    new JournalType { Id = 2, Group = "Journal", Name = "Journal", Hide = false, IsOpeningBlance = false, Icon = "simple-icon-basket-loaded" }
            };

            foreach (var ob in list)
            {
                if (!orgContext.Set<JournalType>().Any(e => e.Id == ob.Id))
                    orgContext.Set<JournalType>().Add(ob);
                else
                    orgContext.Entry<JournalType>(orgContext.Set<JournalType>().Find(ob.Id)).CurrentValues.SetValues(ob);
            }
            orgContext.SaveChanges();
        }

        public void InitialAccountType(Microsoft.EntityFrameworkCore.DbContext orgContext)
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
                var existing = orgContext.Set<AccountType>().FirstOrDefault(e => e.Name == ob.Name);
                if (existing == null)
                    orgContext.Set<AccountType>().Add(ob);
                else
                    existing.DebitOrCredit = ob.DebitOrCredit;
            }
            orgContext.SaveChanges();
        }

        public void InitialAccount(Microsoft.EntityFrameworkCore.DbContext orgContext)
        {
            // Id is left database-generated, for the same reason as InitialAccountType above
            // (Account.Id is an identity column; explicit-value inserts fail with SqlException 544
            // inside this seeding transaction). Rows are listed parent-before-child (already the
            // natural order of a chart of accounts), and ParentId / AccountTypeId are resolved from
            // real generated Ids via lookup maps built as each row is processed — Code -> Account.Id
            // and AccountType.Name -> AccountType.Id — so re-seeding stays fully deterministic
            // regardless of the underlying identity values. Idempotency is by Code (the account's
            // business key), per row, immediately below.
            var accountTypeIdByName = orgContext.Set<AccountType>().ToDictionary(e => e.Name!, e => e.Id);

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

                var existing = orgContext.Set<Account>().FirstOrDefault(e => e.Code == row.Code);
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
    }
}
