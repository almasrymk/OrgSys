import { Routes } from '@angular/router';

export const PARTIES_ROUTES: Routes = [
  {
    path: 'dealers',
    loadChildren: () => import('./dealers/dealers.routes').then((m) => m.DEALERS_ROUTES),
  },
  {
    path: 'dealer-groups',
    loadChildren: () => import('./dealer-groups/dealer-groups.routes').then((m) => m.DEALER_GROUPS_ROUTES),
  },
];
