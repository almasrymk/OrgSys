import { Routes } from '@angular/router';
import { INVENTORY_MOVEMENT_PATHS } from './models/transaction.model';

const movementList = () =>
  import('./pages/transaction-list/transaction-list.component').then((m) => m.TransactionListComponent);
const movementForm = () =>
  import('./pages/transaction-form/transaction-form.component').then((m) => m.TransactionFormComponent);

function movementRoutes(path: string, typeId: number): Routes {
  return [
    { path, loadComponent: movementList, data: { typeId } },
    { path: `${path}/new`, loadComponent: movementForm, data: { typeId } },
    { path: `${path}/:id/edit`, loadComponent: movementForm, data: { typeId } },
  ];
}

export const INVENTORY_ROUTES: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'count' },
  { path: 'new', redirectTo: 'count/new' },
  {
    path: 'count',
    loadComponent: () => import('./pages/inventory-list/inventory-list.component').then((m) => m.InventoryListComponent),
  },
  {
    path: 'count/new',
    loadComponent: () => import('./pages/inventory-form/inventory-form.component').then((m) => m.InventoryFormComponent),
  },
  {
    path: 'count/:id/edit',
    loadComponent: () => import('./pages/inventory-form/inventory-form.component').then((m) => m.InventoryFormComponent),
  },
  {
    path: 'locations',
    loadComponent: () => import('./pages/locations-list/locations-list.component').then((m) => m.LocationsListComponent),
  },
  {
    path: 'locations/new',
    loadComponent: () => import('./pages/locations-form/locations-form.component').then((m) => m.LocationsFormComponent),
  },
  {
    path: 'balances',
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
  ...movementRoutes(`movements/${INVENTORY_MOVEMENT_PATHS[1]}`, 1),
  ...movementRoutes(`movements/${INVENTORY_MOVEMENT_PATHS[2]}`, 2),
  ...movementRoutes(`movements/${INVENTORY_MOVEMENT_PATHS[3]}`, 3),
  ...movementRoutes(`movements/${INVENTORY_MOVEMENT_PATHS[5]}`, 5),
  ...movementRoutes(`movements/${INVENTORY_MOVEMENT_PATHS[6]}`, 6),
  ...movementRoutes(`movements/${INVENTORY_MOVEMENT_PATHS[7]}`, 7),
  ...movementRoutes(`movements/${INVENTORY_MOVEMENT_PATHS[8]}`, 8),
  {
    path: 'movements/:typeId/new',
    redirectTo: ({ params }) => {
      const segment = INVENTORY_MOVEMENT_PATHS[Number(params['typeId'])] ?? 'addition';
      return `/inventory/movements/${segment}/new`;
    },
  },
  {
    path: 'movements/:typeId/:id/edit',
    redirectTo: ({ params }) => {
      const segment = INVENTORY_MOVEMENT_PATHS[Number(params['typeId'])] ?? 'addition';
      return `/inventory/movements/${segment}/${params['id']}/edit`;
    },
  },
  {
    path: 'movements/:typeId',
    redirectTo: ({ params }) => {
      const segment = INVENTORY_MOVEMENT_PATHS[Number(params['typeId'])] ?? 'addition';
      return `/inventory/movements/${segment}`;
    },
  },
  {
    path: ':id/edit',
    redirectTo: ({ params }) => `/inventory/count/${params['id']}/edit`,
  },
];
