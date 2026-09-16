import { Routes } from '@angular/router';

export const DISTRICTS_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () => import('./pages/district-list/district-list.component').then((m) => m.DistrictListComponent),
  },
  {
    path: 'new',
    loadComponent: () => import('./pages/district-form/district-form.component').then((m) => m.DistrictFormComponent),
  },
  {
    path: ':id/edit',
    loadComponent: () => import('./pages/district-form/district-form.component').then((m) => m.DistrictFormComponent),
  },
];
