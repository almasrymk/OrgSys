import { Routes } from '@angular/router';

export const CITIES_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () => import('./pages/city-list/city-list.component').then((m) => m.CityListComponent),
  },
  {
    path: 'new',
    loadComponent: () => import('./pages/city-form/city-form.component').then((m) => m.CityFormComponent),
  },
  {
    path: ':id/edit',
    loadComponent: () => import('./pages/city-form/city-form.component').then((m) => m.CityFormComponent),
  },
];
