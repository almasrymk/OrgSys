import { Routes } from '@angular/router';

export const COMPANIES_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () => import('./pages/company-list/company-list.component').then((m) => m.CompanyListComponent),
  },
  {
    path: 'new',
    loadComponent: () => import('./pages/company-form/company-form.component').then((m) => m.CompanyFormComponent),
  },
  {
    path: ':id/edit',
    loadComponent: () => import('./pages/company-form/company-form.component').then((m) => m.CompanyFormComponent),
  },
];
