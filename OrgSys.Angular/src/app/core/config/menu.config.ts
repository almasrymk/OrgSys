export interface MenuItem {
  labelKey: string;
  icon: string;
  route?: string;
  permissionKeys?: string;
  children?: MenuItem[];
}

/**
 * Structured replacement for OrgSys.App's hardcoded Views/Shared/_MainMenu.cshtml.
 * Extend this as each feature is migrated (see docs/ANGULAR_MIGRATION_INVENTORY.md §7-8);
 * do not hardcode nav markup in layout components again.
 */
export const MENU: MenuItem[] = [
  {
    labelKey: 'Dashboard',
    icon: 'bi-speedometer2',
    route: '/dashboard',
  },
  {
    labelKey: 'Settings',
    icon: 'bi-gear',
    children: [
      {
        labelKey: 'Countries',
        icon: 'bi-globe',
        route: '/administration/countries',
        permissionKeys: 'Countrys.All',
      },
      {
        labelKey: 'Cities',
        icon: 'bi-buildings',
        route: '/administration/cities',
        permissionKeys: 'Citys.All,Citys.View',
      },
      {
        labelKey: 'Districts',
        icon: 'bi-signpost-split',
        route: '/administration/districts',
        permissionKeys: 'Districts.All,Districts.View',
      },
      {
        labelKey: 'Currencies',
        icon: 'bi-currency-exchange',
        route: '/administration/currencies',
        permissionKeys: 'Currencies.All,Currencies.View',
      },
      {
        labelKey: 'Fiscal Years',
        icon: 'bi-calendar-range',
        route: '/administration/fiscal-years',
        permissionKeys: 'FiscalYears.All,FiscalYears.View',
      },
      {
        labelKey: 'Branches',
        icon: 'bi-building',
        route: '/administration/branches',
        permissionKeys: 'Branchs.All,Branchs.View',
      },
      {
        labelKey: 'Banks',
        icon: 'bi-bank2',
        route: '/administration/banks',
        permissionKeys: 'Banks.All,Banks.View',
      },
      {
        labelKey: 'Bank Branches',
        icon: 'bi-bank',
        route: '/administration/bank-branches',
        permissionKeys: 'BankBranchs.All,BankBranchs.View',
      },
      {
        labelKey: 'Products',
        icon: 'bi-box',
        route: '/administration/products',
        permissionKeys: 'Products.All,Products.View',
      },
    ],
  },
  {
    labelKey: 'Accounting',
    icon: 'bi-journal-text',
    children: [
      {
        labelKey: 'Chart of Accounts',
        icon: 'bi-diagram-3',
        route: '/accounting/accounts',
        permissionKeys: 'Accounts.All,Accounts.View',
      },
      {
        labelKey: 'Journal Entries',
        icon: 'bi-journal-check',
        route: '/accounting/journal-entries',
        permissionKeys: 'Journal.All,Journal.View',
      },
    ],
  },
  {
    labelKey: 'Financial',
    icon: 'bi-cash-coin',
    children: [
      {
        labelKey: 'Cash Boxes',
        icon: 'bi-box-seam',
        route: '/financial/financial-accounts/cash-boxes',
        permissionKeys: 'CashBoxes.All,CashBoxes.View',
      },
      {
        labelKey: 'Bank Accounts',
        icon: 'bi-bank',
        route: '/financial/financial-accounts/bank-accounts',
        permissionKeys: 'BankAccounts.All,BankAccounts.View',
      },
      { labelKey: 'Transfers', icon: 'bi-arrow-left-right', route: '/financial/transfers', permissionKeys: 'Financial.All,Financial.View' },
      { labelKey: 'Opening Balances', icon: 'bi-flag', route: '/financial/opening-balances', permissionKeys: 'Financial.All,Financial.View' },
      // FinancialType Ids per Domain.Enums.FinancialTransactionType — OpeningBalance (1) has its
      // own dedicated route above (a different Draft-then-Post workflow, not the unified list/form
      // these 10 share), so it's deliberately not repeated here as a generic /transactions/1 entry.
      { labelKey: 'Receipt', icon: 'bi-download', route: '/financial/transactions/2', permissionKeys: 'Financial.All,Financial.View' },
      { labelKey: 'Payment', icon: 'bi-upload', route: '/financial/transactions/3', permissionKeys: 'Financial.All,Financial.View' },
      { labelKey: 'Transfer In', icon: 'bi-arrow-down-left-circle', route: '/financial/transactions/4', permissionKeys: 'Financial.All,Financial.View' },
      { labelKey: 'Deposit', icon: 'bi-piggy-bank', route: '/financial/transactions/5', permissionKeys: 'Financial.All,Financial.View' },
      { labelKey: 'Withdrawal', icon: 'bi-cash', route: '/financial/transactions/6', permissionKeys: 'Financial.All,Financial.View' },
      { labelKey: 'Fee', icon: 'bi-receipt', route: '/financial/transactions/7', permissionKeys: 'Financial.All,Financial.View' },
      { labelKey: 'Interest', icon: 'bi-percent', route: '/financial/transactions/8', permissionKeys: 'Financial.All,Financial.View' },
      { labelKey: 'Cheque', icon: 'bi-file-earmark-ruled', route: '/financial/transactions/9', permissionKeys: 'Financial.All,Financial.View' },
      { labelKey: 'Adjustment', icon: 'bi-sliders', route: '/financial/transactions/10', permissionKeys: 'Financial.All,Financial.View' },
      { labelKey: 'Transfer Out', icon: 'bi-arrow-up-right-circle', route: '/financial/transactions/11', permissionKeys: 'Financial.All,Financial.View' },
    ],
  },
  {
    labelKey: 'Customers & Suppliers',
    icon: 'bi-people',
    children: [
      { labelKey: 'Customers', icon: 'bi-person-check', route: '/customers-suppliers/dealers/customers', permissionKeys: 'Clients.All,Clients.View' },
      { labelKey: 'Suppliers', icon: 'bi-truck', route: '/customers-suppliers/dealers/suppliers', permissionKeys: 'Suppliers.All,Suppliers.View' },
      { labelKey: 'Client Groups', icon: 'bi-diagram-2', route: '/customers-suppliers/dealer-groups/client-groups', permissionKeys: 'ClientGroups.All,ClientGroups.View' },
      { labelKey: 'Supplier Groups', icon: 'bi-diagram-2', route: '/customers-suppliers/dealer-groups/supplier-groups', permissionKeys: 'SupplierGroups.All,SupplierGroups.View' },
    ],
  },
  {
    labelKey: 'Invoices',
    icon: 'bi-receipt-cutoff',
    children: [
      // InvoiceType Ids per Infrastructure/Seed/InitialData.cs InitialInvoiceType.
      { labelKey: 'Sales Invoice', icon: 'bi-cart-check', route: '/invoices/1', permissionKeys: 'Invoices.All' },
      { labelKey: 'Purchase Invoice', icon: 'bi-cart-plus', route: '/invoices/2', permissionKeys: 'Invoices.All' },
      { labelKey: 'Sales Return', icon: 'bi-arrow-counterclockwise', route: '/invoices/3', permissionKeys: 'Invoices.All' },
      { labelKey: 'Purchase Return', icon: 'bi-arrow-counterclockwise', route: '/invoices/4', permissionKeys: 'Invoices.All' },
    ],
  },
  {
    labelKey: 'Inventory',
    icon: 'bi-box-seam-fill',
    children: [
      // TransactionType Ids per Infrastructure/Seed/InitialData.cs InitialTransactionType.
      // Received (4) is auto-generated by Transfer (3) and has no standalone menu entry.
      { labelKey: 'Addition', icon: 'bi-plus-circle', route: '/transactions/1', permissionKeys: 'Transactions.All' },
      { labelKey: 'Issue', icon: 'bi-dash-circle', route: '/transactions/2', permissionKeys: 'Transactions.All' },
      { labelKey: 'Transfer', icon: 'bi-arrow-left-right', route: '/transactions/3', permissionKeys: 'Transactions.All' },
      { labelKey: 'Adjustment In', icon: 'bi-arrow-down-square', route: '/transactions/5', permissionKeys: 'Transactions.All' },
      { labelKey: 'Adjustment Out', icon: 'bi-arrow-up-square', route: '/transactions/6', permissionKeys: 'Transactions.All' },
      { labelKey: 'Opening Balance', icon: 'bi-flag', route: '/transactions/7', permissionKeys: 'Transactions.All' },
      { labelKey: 'Damaged', icon: 'bi-exclamation-triangle', route: '/transactions/8', permissionKeys: 'Transactions.All' },
      // Inventory has no TypeId dimension in MVC — a single "/Transactions/Inventory" screen.
      { labelKey: 'Inventory Count', icon: 'bi-clipboard-check', route: '/inventory', permissionKeys: 'Inventory.All,Inventory.View' },
    ],
  },
  {
    labelKey: 'Reports',
    icon: 'bi-bar-chart-line',
    children: [
      { labelKey: 'Stock & Product Movement', icon: 'bi-arrow-left-right', route: '/reports/warehouse/movement' },
      { labelKey: 'Stock & Product Balance', icon: 'bi-boxes', route: '/reports/warehouse/balance' },
      { labelKey: 'Clients Balance', icon: 'bi-person-check', route: '/reports/dealers/1/balance' },
      { labelKey: 'Clients Statement', icon: 'bi-file-earmark-text', route: '/reports/dealers/1/statement' },
      { labelKey: 'Suppliers Balance', icon: 'bi-truck', route: '/reports/dealers/2/balance' },
      { labelKey: 'Suppliers Statement', icon: 'bi-file-earmark-text', route: '/reports/dealers/2/statement' },
      { labelKey: 'Safe Movement', icon: 'bi-safe', route: '/reports/finance/safe-movement' },
      { labelKey: 'Safe Balance', icon: 'bi-cash-stack', route: '/reports/finance/safe-balance' },
      { labelKey: 'Sales Balance', icon: 'bi-graph-up', route: '/reports/sales/balance' },
    ],
  },
];
