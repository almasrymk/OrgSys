import { PermissionKeys, permissionList } from '../permissions/permission-keys';

export interface MenuItem {
  labelKey: string;
  icon: string;
  route?: string;
  permissionKeys?: string;
  children?: MenuItem[];
}

/**
 * Structured replacement for OrgSys.App's hardcoded Views/Shared/_MainMenu.cshtml.
 * Icons use the same Dore icon fonts as the MVC menu. Grouping follows backend bounded contexts.
 */
export const MENU: MenuItem[] = [
  {
    labelKey: 'Dashboard',
    icon: 'iconsminds-home-3',
    route: '/dashboard',
  },
  {
    labelKey: 'Master Data',
    icon: 'iconsminds-big-data',
    children: [
      {
        labelKey: 'Countries',
        icon: 'iconsminds-globe-2',
        route: '/master-data/countries',
        permissionKeys: PermissionKeys.CountriesAll,
      },
      {
        labelKey: 'Cities',
        icon: 'iconsminds-map2',
        route: '/master-data/cities',
        permissionKeys: permissionList(PermissionKeys.CitiesAll, PermissionKeys.CitiesView),
      },
      {
        labelKey: 'Districts',
        icon: 'iconsminds-location-2',
        route: '/master-data/districts',
        permissionKeys: permissionList(PermissionKeys.DistrictsAll, PermissionKeys.DistrictsView),
      },
      {
        labelKey: 'Currencies',
        icon: 'iconsminds-coins',
        route: '/master-data/currencies',
        permissionKeys: permissionList(PermissionKeys.CurrenciesAll, PermissionKeys.CurrenciesView),
      },
    ],
  },
  {
    labelKey: 'Organization',
    icon: 'simple-icon-share',
    children: [
      {
        labelKey: 'Branches',
        icon: 'simple-icon-share',
        route: '/organization/branches',
        permissionKeys: permissionList(PermissionKeys.BranchesAll, PermissionKeys.BranchesView),
      },
    ],
  },
  {
    labelKey: 'Catalog',
    icon: 'iconsminds-shopping-basket',
    children: [
      {
        labelKey: 'Products',
        icon: 'iconsminds-shopping-basket',
        route: '/catalog/products',
        permissionKeys: permissionList(PermissionKeys.ProductsAll, PermissionKeys.ProductsView),
      },
    ],
  },
  {
    labelKey: 'Accounting',
    icon: 'iconsminds-file-clipboard',
    children: [
      {
        labelKey: 'Chart of Accounts',
        icon: 'iconsminds-wallet',
        route: '/accounting/accounts',
        permissionKeys: permissionList(PermissionKeys.AccountsAll, PermissionKeys.AccountsView),
      },
      {
        labelKey: 'Journal Entries',
        icon: 'iconsminds-address-book-2',
        route: '/accounting/journal-entries',
        permissionKeys: permissionList(PermissionKeys.JournalAll, PermissionKeys.JournalView),
      },
      {
        labelKey: 'Fiscal Years',
        icon: 'iconsminds-calendar-4',
        route: '/accounting/fiscal-years',
        permissionKeys: permissionList(PermissionKeys.FiscalYearsAll, PermissionKeys.FiscalYearsView),
      },
    ],
  },
  {
    labelKey: 'Treasury',
    icon: 'iconsminds-coins',
    children: [
      { labelKey: 'Cash Boxes', icon: 'iconsminds-coins', route: '/treasury/financial-accounts/cash-boxes', permissionKeys: permissionList(PermissionKeys.CashBoxesAll, PermissionKeys.CashBoxesView) },
      { labelKey: 'Bank Accounts', icon: 'iconsminds-safe-box', route: '/treasury/financial-accounts/bank-accounts', permissionKeys: permissionList(PermissionKeys.BankAccountsAll, PermissionKeys.BankAccountsView) },
      { labelKey: 'Banks', icon: 'iconsminds-bank', route: '/treasury/banks', permissionKeys: permissionList(PermissionKeys.BanksAll, PermissionKeys.BanksView) },
      { labelKey: 'Bank Branches', icon: 'iconsminds-hotel', route: '/treasury/bank-branches', permissionKeys: permissionList(PermissionKeys.BankBranchesAll, PermissionKeys.BankBranchesView) },
      { labelKey: 'Transfers', icon: 'simple-icon-shuffle', route: '/treasury/transfers', permissionKeys: permissionList(PermissionKeys.FinancialAll, PermissionKeys.FinancialView) },
      { labelKey: 'Opening Balances', icon: 'iconsminds-start-2', route: '/treasury/opening-balances', permissionKeys: permissionList(PermissionKeys.FinancialAll, PermissionKeys.FinancialView) },
      { labelKey: 'Receipt', icon: 'iconsminds-financial', route: '/treasury/receipts', permissionKeys: permissionList(PermissionKeys.FinancialAll, PermissionKeys.FinancialView) },
      { labelKey: 'Payment', icon: 'iconsminds-handshake', route: '/treasury/payments', permissionKeys: permissionList(PermissionKeys.FinancialAll, PermissionKeys.FinancialView) },
      { labelKey: 'Transfer In', icon: 'iconsminds-arrow-down-in-circle', route: '/treasury/transfer-in', permissionKeys: permissionList(PermissionKeys.FinancialAll, PermissionKeys.FinancialView) },
      { labelKey: 'Deposit', icon: 'iconsminds-down-1', route: '/treasury/deposits', permissionKeys: permissionList(PermissionKeys.FinancialAll, PermissionKeys.FinancialView) },
      { labelKey: 'Withdrawal', icon: 'iconsminds-up-1', route: '/treasury/withdrawals', permissionKeys: permissionList(PermissionKeys.FinancialAll, PermissionKeys.FinancialView) },
      { labelKey: 'Fee', icon: 'iconsminds-receipt-4', route: '/treasury/fees', permissionKeys: permissionList(PermissionKeys.FinancialAll, PermissionKeys.FinancialView) },
      { labelKey: 'Interest', icon: 'iconsminds-line-chart-1', route: '/treasury/interest', permissionKeys: permissionList(PermissionKeys.FinancialAll, PermissionKeys.FinancialView) },
      { labelKey: 'Cheque', icon: 'iconsminds-check', route: '/treasury/cheques', permissionKeys: permissionList(PermissionKeys.FinancialAll, PermissionKeys.FinancialView) },
      { labelKey: 'Adjustment', icon: 'iconsminds-gear', route: '/treasury/adjustments', permissionKeys: permissionList(PermissionKeys.FinancialAll, PermissionKeys.FinancialView) },
      { labelKey: 'Transfer Out', icon: 'iconsminds-arrow-up-in-circle', route: '/treasury/transfer-out', permissionKeys: permissionList(PermissionKeys.FinancialAll, PermissionKeys.FinancialView) },
    ],
  },
  {
    labelKey: 'Parties',
    icon: 'iconsminds-conference',
    children: [
      { labelKey: 'Customers', icon: 'iconsminds-business-man-woman', route: '/parties/dealers/customers', permissionKeys: permissionList(PermissionKeys.ClientsAll, PermissionKeys.ClientsView) },
      { labelKey: 'Suppliers', icon: 'iconsminds-business-mens', route: '/parties/dealers/suppliers', permissionKeys: permissionList(PermissionKeys.SuppliersAll, PermissionKeys.SuppliersView) },
      { labelKey: 'Client Groups', icon: 'iconsminds-conference', route: '/parties/dealer-groups/client-groups', permissionKeys: permissionList(PermissionKeys.ClientGroupsAll, PermissionKeys.ClientGroupsView) },
      { labelKey: 'Supplier Groups', icon: 'iconsminds-network', route: '/parties/dealer-groups/supplier-groups', permissionKeys: permissionList(PermissionKeys.SupplierGroupsAll, PermissionKeys.SupplierGroupsView) },
    ],
  },
  {
    labelKey: 'Commercial Documents',
    icon: 'simple-icon-basket-loaded',
    children: [
      { labelKey: 'Sales Invoice', icon: 'simple-icon-basket-loaded', route: '/commercial-documents/sales-invoices', permissionKeys: PermissionKeys.InvoicesAll },
      { labelKey: 'Purchase Invoice', icon: 'simple-icon-basket-loaded', route: '/commercial-documents/purchase-invoices', permissionKeys: PermissionKeys.InvoicesAll },
      { labelKey: 'Sales Return', icon: 'simple-icon-action-undo', route: '/commercial-documents/sales-returns', permissionKeys: PermissionKeys.InvoicesAll },
      { labelKey: 'Purchase Return', icon: 'simple-icon-action-undo', route: '/commercial-documents/purchase-returns', permissionKeys: PermissionKeys.InvoicesAll },
    ],
  },
  {
    labelKey: 'Inventory',
    icon: 'iconsminds-synchronize-2',
    children: [
      { labelKey: 'Addition', icon: 'iconsminds-down-1', route: '/inventory/movements/addition', permissionKeys: PermissionKeys.TransactionsAll },
      { labelKey: 'Issue', icon: 'iconsminds-up-1', route: '/inventory/movements/issue', permissionKeys: PermissionKeys.TransactionsAll },
      { labelKey: 'Transfer', icon: 'iconsminds-shuffle-1', route: '/inventory/movements/transfer', permissionKeys: PermissionKeys.TransactionsAll },
      { labelKey: 'Adjustment In', icon: 'iconsminds-arrow-down-in-circle', route: '/inventory/movements/adjustment-in', permissionKeys: PermissionKeys.TransactionsAll },
      { labelKey: 'Adjustment Out', icon: 'iconsminds-arrow-up-in-circle', route: '/inventory/movements/adjustment-out', permissionKeys: PermissionKeys.TransactionsAll },
      { labelKey: 'Opening Balance', icon: 'iconsminds-folder-open', route: '/inventory/movements/opening-balance', permissionKeys: PermissionKeys.TransactionsAll },
      { labelKey: 'Damaged', icon: 'iconsminds-bio-hazard', route: '/inventory/movements/damaged', permissionKeys: PermissionKeys.TransactionsAll },
      { labelKey: 'Inventory Count', icon: 'iconsminds-check', route: '/inventory/count', permissionKeys: permissionList(PermissionKeys.InventoryAll, PermissionKeys.InventoryView) },
      { labelKey: 'Locations', icon: 'iconsminds-map-marker-2', route: '/inventory/locations' },
      { labelKey: 'Receipts', icon: 'iconsminds-down-1', route: '/inventory/receipts' },
      { labelKey: 'Balance', icon: 'iconsminds-data-center', route: '/inventory/balances' },
      { labelKey: 'Reservations', icon: 'iconsminds-lock-2', route: '/inventory/reservations' },
    ],
  },
  {
    labelKey: 'Reporting',
    icon: 'simple-icon-list',
    children: [
      { labelKey: 'Stock & Product Movement', icon: 'iconsminds-orientation-1', route: '/reporting/warehouse/movement' },
      { labelKey: 'Stock & Product Balance', icon: 'iconsminds-data-center', route: '/reporting/warehouse/balance' },
      { labelKey: 'Clients Balance', icon: 'iconsminds-profile', route: '/reporting/dealers/1/balance' },
      { labelKey: 'Clients Statement', icon: 'glyph-icon iconsminds-letter-open', route: '/reporting/dealers/1/statement' },
      { labelKey: 'Suppliers Balance', icon: 'iconsminds-notepad', route: '/reporting/dealers/2/balance' },
      { labelKey: 'Suppliers Statement', icon: 'glyph-icon iconsminds-files', route: '/reporting/dealers/2/statement' },
      { labelKey: 'Safe Movement', icon: 'simple-icon-list', route: '/reporting/finance/safe-movement' },
      { labelKey: 'Safe Balance', icon: 'iconsminds-wallet', route: '/reporting/finance/safe-balance' },
      { labelKey: 'Sales Balance', icon: 'iconsminds-profile', route: '/reporting/sales/balance' },
    ],
  },
];
