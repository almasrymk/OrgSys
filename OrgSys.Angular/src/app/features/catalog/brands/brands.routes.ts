import { Routes } from '@angular/router';

export const BRANDS_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () => import('./pages/brand-list/brand-list.component').then((m) => m.BrandListComponent),
  },
  {
    path: 'new',
    loadComponent: () => import('./pages/brand-form/brand-form.component').then((m) => m.BrandFormComponent),
  },
  {
    path: ':id/edit',
    loadComponent: () => import('./pages/brand-form/brand-form.component').then((m) => m.BrandFormComponent),
  },
];
