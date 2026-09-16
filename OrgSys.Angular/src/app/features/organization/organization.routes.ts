import { Routes } from '@angular/router';

export const ORGANIZATION_ROUTES: Routes = [
  {
    path: 'branches',
    loadChildren: () => import('./branches/branches.routes').then((m) => m.BRANCHES_ROUTES),
  },
];
