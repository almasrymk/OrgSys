import { Routes } from '@angular/router';

export const INVENTORY_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () => import('./pages/inventory-list/inventory-list.component').then((m) => m.InventoryListComponent),
  },
  {
    path: 'new',
    loadComponent: () => import('./pages/inventory-form/inventory-form.component').then((m) => m.InventoryFormComponent),
  },
  {
    path: ':id/edit',
    loadComponent: () => import('./pages/inventory-form/inventory-form.component').then((m) => m.InventoryFormComponent),
  },
];
