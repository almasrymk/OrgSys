import { Routes } from '@angular/router';

export const OPENING_BALANCES_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./pages/opening-balance-list/opening-balance-list.component').then((m) => m.OpeningBalanceListComponent),
  },
  {
    path: 'new',
    loadComponent: () =>
      import('./pages/opening-balance-form/opening-balance-form.component').then((m) => m.OpeningBalanceFormComponent),
  },
  {
    path: ':id/edit',
    loadComponent: () =>
      import('./pages/opening-balance-form/opening-balance-form.component').then((m) => m.OpeningBalanceFormComponent),
  },
];
