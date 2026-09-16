import { Routes } from '@angular/router';

export const ADVANCES_ROUTES: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'custodies' },
  {
    path: 'custodies',
    loadComponent: () => import('./pages/custody-list/custody-list.component').then((m) => m.CustodyListComponent),
  },
  {
    path: 'custodies/new',
    loadComponent: () => import('./pages/custody-form/custody-form.component').then((m) => m.CustodyFormComponent),
  },
  {
    path: 'custodies/:id',
    loadComponent: () => import('./pages/custody-form/custody-form.component').then((m) => m.CustodyFormComponent),
  },
];
