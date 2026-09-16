import { Routes } from '@angular/router';

export const CATALOG_ROUTES: Routes = [
  {
    path: 'products',
    loadChildren: () => import('./products/products.routes').then((m) => m.PRODUCTS_ROUTES),
  },
  {
    path: 'brands',
    loadChildren: () => import('./brands/brands.routes').then((m) => m.BRANDS_ROUTES),
  },
  {
    path: 'price-lists',
    loadChildren: () => import('./price-lists/price-lists.routes').then((m) => m.PRICE_LISTS_ROUTES),
  },
  {
    path: 'properties',
    loadChildren: () => import('./properties/properties.routes').then((m) => m.PROPERTIES_ROUTES),
  },
];
