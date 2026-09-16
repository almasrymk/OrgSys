import { Routes } from '@angular/router';

export const FISCAL_YEARS_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./pages/fiscal-year-list/fiscal-year-list.component').then((m) => m.FiscalYearListComponent),
  },
  {
    path: 'new',
    loadComponent: () =>
      import('./pages/fiscal-year-form/fiscal-year-form.component').then((m) => m.FiscalYearFormComponent),
  },
  {
    path: ':id/edit',
    loadComponent: () =>
      import('./pages/fiscal-year-form/fiscal-year-form.component').then((m) => m.FiscalYearFormComponent),
  },
];
