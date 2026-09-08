import { Routes } from '@angular/router';

export const ACCOUNTS_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () => import('./pages/account-list/account-list.component').then((m) => m.AccountListComponent),
  },
  {
    path: 'new',
    loadComponent: () => import('./pages/account-form/account-form.component').then((m) => m.AccountFormComponent),
  },
  {
    path: ':id/edit',
    loadComponent: () => import('./pages/account-form/account-form.component').then((m) => m.AccountFormComponent),
  },
];
