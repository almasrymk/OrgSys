export interface MenuItem {
  labelKey: string;
  icon: string;
  route?: string;
  permissionKeys?: string;
  children?: MenuItem[];
}

/**
 * Structured replacement for OrgSys.App's hardcoded Views/Shared/_MainMenu.cshtml.
 * Icons use the same Dore icon fonts as the MVC menu (iconsminds- and simple-icon- prefixes, see
 * OrgSys/wwwroot/font/iconsmind-s and .../simple-line-icons) — reusing the exact class MVC
 * assigns to the same concept wherever one exists (e.g. Financial TypeId 2-10, CashBoxes,
 * Countries...), and a same-family icon elsewhere. Do not switch back to bootstrap-icons here.
 * Extend this as each feature is migrated (see docs/ANGULAR_MIGRATION_INVENTORY.md §7-8);
 * do not hardcode nav markup in layout components again.
 */
export const MENU: MenuItem[] = [
  {
    labelKey: 'Dashboard',
    icon: 'iconsminds-home-3',
    route: '/dashboard',
  },
  {
    labelKey: 'Settings',
    icon: 'iconsminds-big-data',
    children: [
      {
        labelKey: 'Countries',
        icon: 'iconsminds-globe-2',
        route: '/administration/countries',
        permissionKeys: 'Countrys.All',
      },
      {
        labelKey: 'Cities',
        icon: 'iconsminds-map2',
        route: '/administration/cities',
        permissionKeys: 'Citys.All,Citys.View',
      },
      {
        labelKey: 'Districts',
        icon: 'iconsminds-location-2',
        route: '/administration/districts',
        permissionKeys: 'Districts.All,Districts.View',
      },
      {
        labelKey: 'Currencies',
        icon: 'iconsminds-coins',
        route: '/administration/currencies',
        permissionKeys: 'Currencies.All,Currencies.View',
      },
      {
        labelKey: 'Fiscal Years',
        icon: 'iconsminds-calendar-4',
        route: '/administration/fiscal-years',
        permissionKeys: 'FiscalYears.All,FiscalYears.View',
      },
      {
        labelKey: 'Branches',
        icon: 'simple-icon-share',
        route: '/administration/branches',
        permissionKeys: 'Branchs.All,Branchs.View',
      },
      {
        labelKey: 'Banks',
        icon: 'iconsminds-bank',
        route: '/administration/banks',
        permissionKeys: 'Banks.All,Banks.View',
      },
      {
        labelKey: 'Bank Branches',
        icon: 'iconsminds-hotel',
        route: '/administration/bank-branches',
        permissionKeys: 'BankBranchs.All,BankBranchs.View',
      },
      {
        labelKey: 'Products',
        icon: 'iconsminds-shopping-basket',
        route: '/administration/products',
        permissionKeys: 'Products.All,Products.View',
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
        permissionKeys: 'Accounts.All,Accounts.View',
      },
      {
        labelKey: 'Journal Entries',
        icon: 'iconsminds-address-book-2',
        route: '/accounting/journal-entries',
        permissionKeys: 'Journal.All,Journal.View',
      },
    ],
  },
  {
    labelKey: 'Financial',
    icon: 'iconsminds-coins',
    children: [
      { labelKey: 'Cash Boxes', icon: 'iconsminds-coins', route: '/financial/financial-accounts/cash-boxes', permissionKeys: 'CashBoxes.All,CashBoxes.View' },
      { labelKey: 'Bank Accounts', icon: 'iconsminds-safe-box', route: '/financial/financial-accounts/bank-accounts', permissionKeys: 'BankAccounts.All,BankAccounts.View' },
      { labelKey: 'Transfers', icon: 'simple-icon-shuffle', route: '/financial/transfers', permissionKeys: 'Financial.All,Financial.View' },
      { labelKey: 'Opening Balances', icon: 'iconsminds-start-2', route: '/financial/opening-balances', permissionKeys: 'Financial.All,Financial.View' },
      // FinancialType Ids per Domain.Enums.FinancialTransactionType — OpeningBalance (1) has its
      // own dedicated route above (a different Draft-then-Post workflow, not the unified list/form
      // these 10 share), so it's deliberately not repeated here as a generic /transactions/1 entry.
      // Icons for TypeId 2-10 reuse _MainMenu.cshtml's exact per-type assignment.
      { labelKey: 'Receipt', icon: 'iconsminds-financial', route: '/financial/transactions/2', permissionKeys: 'Financial.All,Financial.View' },
      { labelKey: 'Payment', icon: 'iconsminds-handshake', route: '/financial/transactions/3', permissionKeys: 'Financial.All,Financial.View' },
      { labelKey: 'Transfer In', icon: 'iconsminds-arrow-down-in-circle', route: '/financial/transactions/4', permissionKeys: 'Financial.All,Financial.View' },
      { labelKey: 'Deposit', icon: 'iconsminds-down-1', route: '/financial/transactions/5', permissionKeys: 'Financial.All,Financial.View' },
      { labelKey: 'Withdrawal', icon: 'iconsminds-up-1', route: '/financial/transactions/6', permissionKeys: 'Financial.All,Financial.View' },
      { labelKey: 'Fee', icon: 'iconsminds-receipt-4', route: '/financial/transactions/7', permissionKeys: 'Financial.All,Financial.View' },
      { labelKey: 'Interest', icon: 'iconsminds-line-chart-1', route: '/financial/transactions/8', permissionKeys: 'Financial.All,Financial.View' },
      { labelKey: 'Cheque', icon: 'iconsminds-check', route: '/financial/transactions/9', permissionKeys: 'Financial.All,Financial.View' },
      { labelKey: 'Adjustment', icon: 'iconsminds-gear', route: '/financial/transactions/10', permissionKeys: 'Financial.All,Financial.View' },
      { labelKey: 'Transfer Out', icon: 'iconsminds-arrow-up-in-circle', route: '/financial/transactions/11', permissionKeys: 'Financial.All,Financial.View' },
    ],
  },
  {
    labelKey: 'Customers & Suppliers',
    icon: 'iconsminds-conference',
    children: [
      { labelKey: 'Customers', icon: 'iconsminds-business-man-woman', route: '/customers-suppliers/dealers/customers', permissionKeys: 'Clients.All,Clients.View' },
      { labelKey: 'Suppliers', icon: 'iconsminds-business-mens', route: '/customers-suppliers/dealers/suppliers', permissionKeys: 'Suppliers.All,Suppliers.View' },
      { labelKey: 'Client Groups', icon: 'iconsminds-conference', route: '/customers-suppliers/dealer-groups/client-groups', permissionKeys: 'ClientGroups.All,ClientGroups.View' },
      { labelKey: 'Supplier Groups', icon: 'iconsminds-network', route: '/customers-suppliers/dealer-groups/supplier-groups', permissionKeys: 'SupplierGroups.All,SupplierGroups.View' },
    ],
  },
  {
    labelKey: 'Invoices',
    icon: 'simple-icon-basket-loaded',
    children: [
      // InvoiceType Ids per Infrastructure/Seed/InitialData.cs InitialInvoiceType.
      { labelKey: 'Sales Invoice', icon: 'simple-icon-basket-loaded', route: '/invoices/1', permissionKeys: 'Invoices.All' },
      { labelKey: 'Purchase Invoice', icon: 'simple-icon-basket-loaded', route: '/invoices/2', permissionKeys: 'Invoices.All' },
      { labelKey: 'Sales Return', icon: 'simple-icon-action-undo', route: '/invoices/3', permissionKeys: 'Invoices.All' },
      { labelKey: 'Purchase Return', icon: 'simple-icon-action-undo', route: '/invoices/4', permissionKeys: 'Invoices.All' },
    ],
  },
  {
    labelKey: 'Inventory',
    icon: 'iconsminds-synchronize-2',
    children: [
      // TransactionType Ids per Infrastructure/Seed/InitialData.cs InitialTransactionType.
      // Received (4) is auto-generated by Transfer (3) and has no standalone menu entry.
      { labelKey: 'Addition', icon: 'iconsminds-down-1', route: '/transactions/1', permissionKeys: 'Transactions.All' },
      { labelKey: 'Issue', icon: 'iconsminds-up-1', route: '/transactions/2', permissionKeys: 'Transactions.All' },
      { labelKey: 'Transfer', icon: 'iconsminds-shuffle-1', route: '/transactions/3', permissionKeys: 'Transactions.All' },
      { labelKey: 'Adjustment In', icon: 'iconsminds-arrow-down-in-circle', route: '/transactions/5', permissionKeys: 'Transactions.All' },
      { labelKey: 'Adjustment Out', icon: 'iconsminds-arrow-up-in-circle', route: '/transactions/6', permissionKeys: 'Transactions.All' },
      { labelKey: 'Opening Balance', icon: 'iconsminds-folder-open', route: '/transactions/7', permissionKeys: 'Transactions.All' },
      { labelKey: 'Damaged', icon: 'iconsminds-bio-hazard', route: '/transactions/8', permissionKeys: 'Transactions.All' },
      // Inventory has no TypeId dimension in MVC — a single "/Transactions/Inventory" screen.
      { labelKey: 'Inventory Count', icon: 'iconsminds-check', route: '/inventory', permissionKeys: 'Inventory.All,Inventory.View' },
    ],
  },
  {
    // New screens for the hardened Inventory bounded context (docs/ddd/inventory-target-architecture.md) —
    // alongside, not replacing, the legacy "Inventory" group above.
    labelKey: 'Warehouse',
    icon: 'iconsminds-shop-4',
    children: [
      { labelKey: 'Locations', icon: 'iconsminds-map-marker-2', route: '/warehouse/locations' },
      { labelKey: 'Receipts', icon: 'iconsminds-down-1', route: '/warehouse/receipts' },
      { labelKey: 'Balance', icon: 'iconsminds-data-center', route: '/warehouse/balance' },
      { labelKey: 'Reservations', icon: 'iconsminds-lock-2', route: '/warehouse/reservations' },
    ],
  },
  {
    labelKey: 'Reports',
    icon: 'simple-icon-list',
    children: [
      { labelKey: 'Stock & Product Movement', icon: 'iconsminds-orientation-1', route: '/reports/warehouse/movement' },
      { labelKey: 'Stock & Product Balance', icon: 'iconsminds-data-center', route: '/reports/warehouse/balance' },
      { labelKey: 'Clients Balance', icon: 'iconsminds-profile', route: '/reports/dealers/1/balance' },
      { labelKey: 'Clients Statement', icon: 'glyph-icon iconsminds-letter-open', route: '/reports/dealers/1/statement' },
      { labelKey: 'Suppliers Balance', icon: 'iconsminds-notepad', route: '/reports/dealers/2/balance' },
      { labelKey: 'Suppliers Statement', icon: 'glyph-icon iconsminds-files', route: '/reports/dealers/2/statement' },
      { labelKey: 'Safe Movement', icon: 'simple-icon-list', route: '/reports/finance/safe-movement' },
      { labelKey: 'Safe Balance', icon: 'iconsminds-wallet', route: '/reports/finance/safe-balance' },
      { labelKey: 'Sales Balance', icon: 'iconsminds-profile', route: '/reports/sales/balance' },
    ],
  },
];
