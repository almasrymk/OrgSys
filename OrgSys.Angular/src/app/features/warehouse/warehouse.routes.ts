import { Routes } from '@angular/router';

export const WAREHOUSE_ROUTES: Routes = [
  {
    path: 'locations',
    loadComponent: () => import('./pages/locations-list/locations-list.component').then((m) => m.LocationsListComponent),
  },
  {
    path: 'locations/new',
    loadComponent: () => import('./pages/locations-form/locations-form.component').then((m) => m.LocationsFormComponent),
  },
  {
    path: 'balance',
    loadComponent: () => import('./pages/balance/balance.component').then((m) => m.BalanceComponent),
  },
  {
    path: 'reservations',
    loadComponent: () => import('./pages/reservations/reservations.component').then((m) => m.ReservationsComponent),
  },
  {
    path: 'receipts',
    loadComponent: () => import('./pages/receipts-list/receipts-list.component').then((m) => m.ReceiptsListComponent),
  },
  {
    path: 'receipts/new',
    loadComponent: () => import('./pages/receipts-form/receipts-form.component').then((m) => m.ReceiptsFormComponent),
  },
];
