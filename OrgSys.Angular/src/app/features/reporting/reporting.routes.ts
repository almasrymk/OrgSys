import { Routes } from '@angular/router';

export const REPORTING_ROUTES: Routes = [
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
  {
    path: 'projections/aging',
    loadComponent: () =>
      import('../../shared/components/query-workspace/query-workspace.component').then((m) => m.QueryWorkspaceComponent),
    data: {
      title: 'Customer Aging Projection',
      entityRoute: 'ProjectionReport',
      action: 'Aging',
      fields: [
        { name: 'customerId', label: 'Customer Id', type: 'number' },
        { name: 'asOfDate', label: 'As of', type: 'date' },
      ],
    },
  },
  {
    path: 'projections/sales-summary',
    loadComponent: () =>
      import('../../shared/components/query-workspace/query-workspace.component').then((m) => m.QueryWorkspaceComponent),
    data: {
      title: 'Sales Summary Projection',
      entityRoute: 'ProjectionReport',
      action: 'SalesSummary',
      fields: [
        { name: 'fromDate', label: 'From', type: 'date' },
        { name: 'toDate', label: 'To', type: 'date' },
      ],
    },
  },
];
