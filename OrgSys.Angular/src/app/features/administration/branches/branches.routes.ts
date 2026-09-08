import { Routes } from '@angular/router';

export const BRANCHES_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () => import('./pages/branch-list/branch-list.component').then((m) => m.BranchListComponent),
  },
  {
    path: 'new',
    loadComponent: () => import('./pages/branch-form/branch-form.component').then((m) => m.BranchFormComponent),
  },
  {
    path: ':id/edit',
    loadComponent: () => import('./pages/branch-form/branch-form.component').then((m) => m.BranchFormComponent),
  },
];
