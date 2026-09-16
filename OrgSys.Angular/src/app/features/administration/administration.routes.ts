import { Routes } from '@angular/router';

export const ADMINISTRATION_ROUTES: Routes = [
  {
    path: 'users',
    loadChildren: () => import('./users/users.routes').then((m) => m.USERS_ROUTES),
  },
  {
    path: 'roles',
    loadChildren: () => import('./roles/roles.routes').then((m) => m.ROLES_ROUTES),
  },
];
