import { Routes } from '@angular/router';

export const PRICE_LISTS_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () => import('./pages/price-list-list/price-list-list.component').then((m) => m.PriceListListComponent),
  },
  {
    path: 'new',
    loadComponent: () => import('./pages/price-list-form/price-list-form.component').then((m) => m.PriceListFormComponent),
  },
  {
    path: ':id/edit',
    loadComponent: () => import('./pages/price-list-form/price-list-form.component').then((m) => m.PriceListFormComponent),
  },
];
