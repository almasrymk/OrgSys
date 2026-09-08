import { Routes } from '@angular/router';

/** Root delegates here; this delegates to each customers-suppliers sub-feature's own routes. */
export const CUSTOMERS_SUPPLIERS_ROUTES: Routes = [
  {
    path: 'dealers',
    loadChildren: () => import('./dealers/dealers.routes').then((m) => m.DEALERS_ROUTES),
  },
  {
    path: 'dealer-groups',
    loadChildren: () => import('./dealer-groups/dealer-groups.routes').then((m) => m.DEALER_GROUPS_ROUTES),
  },
];
