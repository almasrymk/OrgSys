import { Routes } from '@angular/router';

export const TRANSACTIONS_ROUTES: Routes = [
  {
    path: ':typeId',
    loadComponent: () => import('./pages/transaction-list/transaction-list.component').then((m) => m.TransactionListComponent),
  },
  {
    path: ':typeId/new',
    loadComponent: () => import('./pages/transaction-form/transaction-form.component').then((m) => m.TransactionFormComponent),
  },
  {
    path: ':typeId/:id/edit',
    loadComponent: () => import('./pages/transaction-form/transaction-form.component').then((m) => m.TransactionFormComponent),
  },
];
