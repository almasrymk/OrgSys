import { Routes } from '@angular/router';

export const REPORTS_ROUTES: Routes = [
  {
    path: 'warehouse/movement',
    loadComponent: () => import('./pages/stock-movement/stock-movement.component').then((m) => m.StockMovementComponent),
  },
  {
    path: 'warehouse/balance',
    loadComponent: () => import('./pages/stock-balance/stock-balance.component').then((m) => m.StockBalanceComponent),
  },
  {
    path: 'dealers/:dealerTypeId/balance',
    loadComponent: () => import('./pages/dealer-balance/dealer-balance.component').then((m) => m.DealerBalanceComponent),
  },
  {
    path: 'dealers/:dealerTypeId/statement',
    loadComponent: () => import('./pages/dealer-statement/dealer-statement.component').then((m) => m.DealerStatementComponent),
  },
  {
    path: 'finance/safe-movement',
    loadComponent: () => import('./pages/safe-movement/safe-movement.component').then((m) => m.SafeMovementComponent),
  },
  {
    path: 'finance/safe-balance',
    loadComponent: () => import('./pages/safe-balance/safe-balance.component').then((m) => m.SafeBalanceComponent),
  },
  {
    path: 'sales/balance',
    loadComponent: () => import('./pages/sales-balance/sales-balance.component').then((m) => m.SalesBalanceComponent),
  },
];
