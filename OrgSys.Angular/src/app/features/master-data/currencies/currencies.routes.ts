import { Routes } from '@angular/router';

export const CURRENCIES_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () => import('./pages/currency-list/currency-list.component').then((m) => m.CurrencyListComponent),
  },
  {
    path: 'new',
    loadComponent: () => import('./pages/currency-form/currency-form.component').then((m) => m.CurrencyFormComponent),
  },
  {
    path: ':id/edit',
    loadComponent: () => import('./pages/currency-form/currency-form.component').then((m) => m.CurrencyFormComponent),
  },
];
