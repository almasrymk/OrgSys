import { Routes } from '@angular/router';

export const BANK_BRANCHES_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () => import('./pages/bank-branch-list/bank-branch-list.component').then((m) => m.BankBranchListComponent),
  },
  {
    path: 'new',
    loadComponent: () => import('./pages/bank-branch-form/bank-branch-form.component').then((m) => m.BankBranchFormComponent),
  },
  {
    path: ':id/edit',
    loadComponent: () => import('./pages/bank-branch-form/bank-branch-form.component').then((m) => m.BankBranchFormComponent),
  },
];
