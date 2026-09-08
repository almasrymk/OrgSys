import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';

/** Root routes delegate to feature routes only — no individual screens listed here. */
export const routes: Routes = [
  {
    path: 'login',
    loadComponent: () => import('./features/auth/pages/login/login.component').then((m) => m.LoginComponent),
  },
  {
    path: '',
    loadComponent: () => import('./layout/main-layout/main-layout.component').then((m) => m.MainLayoutComponent),
    canActivate: [authGuard],
    children: [
      { path: '', pathMatch: 'full', redirectTo: 'dashboard' },
      {
        path: 'dashboard',
        loadComponent: () => import('./features/dashboard/dashboard.component').then((m) => m.DashboardComponent),
      },
      {
        path: 'administration',
        loadChildren: () =>
          import('./features/administration/administration.routes').then((m) => m.ADMINISTRATION_ROUTES),
      },
      {
        path: 'accounting',
        loadChildren: () => import('./features/accounting/accounting.routes').then((m) => m.ACCOUNTING_ROUTES),
      },
      {
        path: 'financial',
        loadChildren: () => import('./features/financial/financial.routes').then((m) => m.FINANCIAL_ROUTES),
      },
      {
        path: 'customers-suppliers',
        loadChildren: () =>
          import('./features/customers-suppliers/customers-suppliers.routes').then((m) => m.CUSTOMERS_SUPPLIERS_ROUTES),
      },
      {
        path: 'invoices',
        loadChildren: () => import('./features/invoices/invoices.routes').then((m) => m.INVOICES_ROUTES),
      },
      {
        path: 'transactions',
        loadChildren: () => import('./features/transactions/transactions.routes').then((m) => m.TRANSACTIONS_ROUTES),
      },
      {
        path: 'inventory',
        loadChildren: () => import('./features/inventory/inventory.routes').then((m) => m.INVENTORY_ROUTES),
      },
      {
        path: 'reports',
        loadChildren: () => import('./features/reports/reports.routes').then((m) => m.REPORTS_ROUTES),
      },
    ],
  },
  { path: '**', redirectTo: '' },
];
