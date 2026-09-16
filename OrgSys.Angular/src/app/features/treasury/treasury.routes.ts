import { Routes } from '@angular/router';
import { TREASURY_TRANSACTION_PATHS } from './transactions/models/financial.model';

export const TREASURY_ROUTES: Routes = [
  {
    path: 'financial-accounts',
    loadChildren: () => import('./financial-accounts/financial-accounts.routes').then((m) => m.FINANCIAL_ACCOUNTS_ROUTES),
  },
  {
    path: 'transfers',
    loadChildren: () => import('./transfers/transfers.routes').then((m) => m.TRANSFERS_ROUTES),
  },
  {
    path: 'opening-balances',
    loadChildren: () => import('./opening-balances/opening-balances.routes').then((m) => m.OPENING_BALANCES_ROUTES),
  },
  {
    path: 'banks',
    loadChildren: () => import('./banks/banks.routes').then((m) => m.BANKS_ROUTES),
  },
  {
    path: 'bank-branches',
    loadChildren: () => import('./bank-branches/bank-branches.routes').then((m) => m.BANK_BRANCHES_ROUTES),
  },
  {
    path: 'transactions/:typeId/new',
    redirectTo: ({ params }) => {
      const segment = TREASURY_TRANSACTION_PATHS[Number(params['typeId'])] ?? 'receipts';
      return `/treasury/${segment}/new`;
    },
  },
  {
    path: 'transactions/:typeId',
    redirectTo: ({ params }) => {
      const segment = TREASURY_TRANSACTION_PATHS[Number(params['typeId'])] ?? 'receipts';
      return `/treasury/${segment}`;
    },
  },
  {
    path: '',
    loadChildren: () => import('./transactions/transactions.routes').then((m) => m.TRANSACTIONS_ROUTES),
  },
];
