import { Routes } from '@angular/router';

export const BANKS_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () => import('./pages/bank-list/bank-list.component').then((m) => m.BankListComponent),
  },
  {
    path: 'new',
    loadComponent: () => import('./pages/bank-form/bank-form.component').then((m) => m.BankFormComponent),
  },
  {
    path: ':id/edit',
    loadComponent: () => import('./pages/bank-form/bank-form.component').then((m) => m.BankFormComponent),
  },
];
