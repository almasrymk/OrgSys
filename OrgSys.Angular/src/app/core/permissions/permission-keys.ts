/**
 * Mirrors seeded `Permission.Key` values from
 * `Modules/Administration/Administration.Infrastructure/Seeding/AdministrationDataSeeder.cs`.
 * Frontend gating is UX only — the API remains the security authority.
 * Spellings such as Countrys/Citys/Branchs/BankBranchs are intentional; they match the seed.
 */
export const PermissionKeys = {
  CountriesAll: 'Countrys.All',
  CitiesAll: 'Citys.All',
  CitiesView: 'Citys.View',
  CitiesAdd: 'Citys.Add',
  DistrictsAll: 'Districts.All',
  DistrictsView: 'Districts.View',
  CurrenciesAll: 'Currencies.All',
  CurrenciesView: 'Currencies.View',
  FiscalYearsAll: 'FiscalYears.All',
  FiscalYearsView: 'FiscalYears.View',
  BranchesAll: 'Branchs.All',
  BranchesView: 'Branchs.View',
  BanksAll: 'Banks.All',
  BanksView: 'Banks.View',
  BankBranchesAll: 'BankBranchs.All',
  BankBranchesView: 'BankBranchs.View',
  ProductsAll: 'Products.All',
  ProductsView: 'Products.View',
  AccountsAll: 'Accounts.All',
  AccountsView: 'Accounts.View',
  JournalAll: 'Journal.All',
  JournalView: 'Journal.View',
  CashBoxesAll: 'CashBoxes.All',
  CashBoxesView: 'CashBoxes.View',
  BankAccountsAll: 'BankAccounts.All',
  BankAccountsView: 'BankAccounts.View',
  FinancialAll: 'Financial.All',
  FinancialView: 'Financial.View',
  FinancialAdd: 'Financial.Add',
  ClientsAll: 'Clients.All',
  ClientsView: 'Clients.View',
  SuppliersAll: 'Suppliers.All',
  SuppliersView: 'Suppliers.View',
  ClientGroupsAll: 'ClientGroups.All',
  ClientGroupsView: 'ClientGroups.View',
  SupplierGroupsAll: 'SupplierGroups.All',
  SupplierGroupsView: 'SupplierGroups.View',
  InvoicesAll: 'Invoices.All',
  TransactionsAll: 'Transactions.All',
  InventoryAll: 'Inventory.All',
  InventoryView: 'Inventory.View',
} as const;

export function permissionList(...keys: string[]): string {
  return keys.join(',');
}
