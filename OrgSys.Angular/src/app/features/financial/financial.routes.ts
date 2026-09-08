import { Routes } from '@angular/router';

/** Root delegates here; this delegates to each financial sub-feature's own routes. */
export const FINANCIAL_ROUTES: Routes = [
  {
    path: 'financial-accounts',
    loadChildren: () => import('./financial-accounts/financial-accounts.routes').then((m) => m.FINANCIAL_ACCOUNTS_ROUTES),
  },
  {
    path: 'transactions',
    loadChildren: () => import('./transactions/transactions.routes').then((m) => m.TRANSACTIONS_ROUTES),
  },
  {
    path: 'transfers',
    loadChildren: () => import('./transfers/transfers.routes').then((m) => m.TRANSFERS_ROUTES),
  },
  {
    path: 'opening-balances',
    loadChildren: () => import('./opening-balances/opening-balances.routes').then((m) => m.OPENING_BALANCES_ROUTES),
  },
];
