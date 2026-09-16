import { Routes } from '@angular/router';
import { FinancialAccountType } from './models/financial-account.model';

const listComponent = () =>
  import('./pages/financial-account-list/financial-account-list.component').then((m) => m.FinancialAccountListComponent);
const formComponent = () =>
  import('./pages/financial-account-form/financial-account-form.component').then((m) => m.FinancialAccountFormComponent);

/** One component pair drives both routes, parameterized by route `data` — see the list component's docstring. */
export const FINANCIAL_ACCOUNTS_ROUTES: Routes = [
  {
    path: 'cash-boxes',
    loadComponent: listComponent,
    data: { financialAccountType: FinancialAccountType.CashBox, title: 'Cash Boxes', permissionPrefix: 'CashBoxes', basePath: '/financial/financial-accounts/cash-boxes' },
  },
  {
    path: 'cash-boxes/new',
    loadComponent: formComponent,
    data: { financialAccountType: FinancialAccountType.CashBox, title: 'Cash Boxes', basePath: '/financial/financial-accounts/cash-boxes' },
  },
  {
    path: 'cash-boxes/:id/edit',
    loadComponent: formComponent,
    data: { financialAccountType: FinancialAccountType.CashBox, title: 'Cash Boxes', basePath: '/financial/financial-accounts/cash-boxes' },
  },
  {
    path: 'bank-accounts',
    loadComponent: listComponent,
    data: { financialAccountType: FinancialAccountType.Bank, title: 'Bank Accounts', permissionPrefix: 'BankAccounts', basePath: '/financial/financial-accounts/bank-accounts' },
  },
  {
    path: 'bank-accounts/new',
    loadComponent: formComponent,
    data: { financialAccountType: FinancialAccountType.Bank, title: 'Bank Accounts', basePath: '/financial/financial-accounts/bank-accounts' },
  },
  {
    path: 'bank-accounts/:id/edit',
    loadComponent: formComponent,
    data: { financialAccountType: FinancialAccountType.Bank, title: 'Bank Accounts', basePath: '/financial/financial-accounts/bank-accounts' },
  },
];
