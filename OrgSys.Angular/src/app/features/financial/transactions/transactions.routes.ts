import { Routes } from '@angular/router';

export const TRANSACTIONS_ROUTES: Routes = [
  {
    path: ':typeId',
    loadComponent: () =>
      import('./pages/financial-transaction-list/financial-transaction-list.component').then((m) => m.FinancialTransactionListComponent),
  },
  {
    path: ':typeId/new',
    loadComponent: () =>
      import('./pages/financial-transaction-form/financial-transaction-form.component').then((m) => m.FinancialTransactionFormComponent),
  },
];
