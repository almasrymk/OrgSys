import { Routes } from '@angular/router';

export const COUNTRIES_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./pages/country-list/country-list.component').then((m) => m.CountryListComponent),
  },
  {
    path: 'new',
    loadComponent: () =>
      import('./pages/country-form/country-form.component').then((m) => m.CountryFormComponent),
  },
  {
    path: ':id/edit',
    loadComponent: () =>
      import('./pages/country-form/country-form.component').then((m) => m.CountryFormComponent),
  },
];
