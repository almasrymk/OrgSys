import { Routes } from '@angular/router';

export const CATALOG_ROUTES: Routes = [
  {
    path: 'products',
    loadChildren: () => import('./products/products.routes').then((m) => m.PRODUCTS_ROUTES),
  },
];
