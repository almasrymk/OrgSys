import { Routes, UrlSegment } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';
import { INVOICE_TYPE_PATHS } from './features/commercial-documents/models/invoice-lookups.model';
import { INVENTORY_MOVEMENT_PATHS } from './features/inventory/models/transaction.model';

function remainder(url: UrlSegment[]): string {
  return url.map((s) => s.path).join('/');
}

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
        path: 'accounting',
        loadChildren: () => import('./features/accounting/accounting.routes').then((m) => m.ACCOUNTING_ROUTES),
      },
      {
        path: 'treasury',
        loadChildren: () => import('./features/treasury/treasury.routes').then((m) => m.TREASURY_ROUTES),
      },
      {
        path: 'parties',
        loadChildren: () => import('./features/parties/parties.routes').then((m) => m.PARTIES_ROUTES),
      },
      {
        path: 'commercial-documents',
        loadChildren: () =>
          import('./features/commercial-documents/commercial-documents.routes').then((m) => m.COMMERCIAL_DOCUMENTS_ROUTES),
      },
      {
        path: 'inventory',
        loadChildren: () => import('./features/inventory/inventory.routes').then((m) => m.INVENTORY_ROUTES),
      },
      {
        path: 'catalog',
        loadChildren: () => import('./features/catalog/catalog.routes').then((m) => m.CATALOG_ROUTES),
      },
      {
        path: 'master-data',
        loadChildren: () => import('./features/master-data/master-data.routes').then((m) => m.MASTER_DATA_ROUTES),
      },
      {
        path: 'organization',
        loadChildren: () => import('./features/organization/organization.routes').then((m) => m.ORGANIZATION_ROUTES),
      },
      {
        path: 'reporting',
        loadChildren: () => import('./features/reporting/reporting.routes').then((m) => m.REPORTING_ROUTES),
      },

      // --- Legacy compatibility redirects (bookmarked / menu URLs) ---
      {
        path: 'administration',
        children: [
          {
            path: '**',
            redirectTo: ({ url }) => {
              const parts = url.map((s) => s.path);
              const head = parts[0] ?? '';
              const rest = parts.slice(1).join('/');
              const map: Record<string, string> = {
                countries: '/master-data/countries',
                cities: '/master-data/cities',
                districts: '/master-data/districts',
                currencies: '/master-data/currencies',
                'fiscal-years': '/accounting/fiscal-years',
                branches: '/organization/branches',
                banks: '/treasury/banks',
                'bank-branches': '/treasury/bank-branches',
                products: '/catalog/products',
              };
              const base = map[head] ?? '/dashboard';
              return rest ? `${base}/${rest}` : base;
            },
          },
        ],
      },
      {
        path: 'financial',
        children: [
          { path: '**', redirectTo: ({ url }) => `/treasury/${remainder(url)}` },
        ],
      },
      {
        path: 'customers-suppliers',
        children: [
          { path: '**', redirectTo: ({ url }) => `/parties/${remainder(url)}` },
        ],
      },
      {
        path: 'invoices',
        children: [
          {
            path: ':typeId/:id/edit',
            redirectTo: ({ params }) =>
              `/commercial-documents/${INVOICE_TYPE_PATHS[Number(params['typeId'])] ?? params['typeId']}/${params['id']}/edit`,
          },
          {
            path: ':typeId/new',
            redirectTo: ({ params }) =>
              `/commercial-documents/${INVOICE_TYPE_PATHS[Number(params['typeId'])] ?? params['typeId']}/new`,
          },
          {
            path: ':typeId',
            redirectTo: ({ params }) =>
              `/commercial-documents/${INVOICE_TYPE_PATHS[Number(params['typeId'])] ?? params['typeId']}`,
          },
        ],
      },
      {
        path: 'transactions',
        children: [
          {
            path: ':typeId/:id/edit',
            redirectTo: ({ params }) =>
              `/inventory/movements/${INVENTORY_MOVEMENT_PATHS[Number(params['typeId'])] ?? params['typeId']}/${params['id']}/edit`,
          },
          {
            path: ':typeId/new',
            redirectTo: ({ params }) =>
              `/inventory/movements/${INVENTORY_MOVEMENT_PATHS[Number(params['typeId'])] ?? params['typeId']}/new`,
          },
          {
            path: ':typeId',
            redirectTo: ({ params }) =>
              `/inventory/movements/${INVENTORY_MOVEMENT_PATHS[Number(params['typeId'])] ?? params['typeId']}`,
          },
        ],
      },
      {
        path: 'warehouse',
        children: [
          { path: 'balance', redirectTo: '/inventory/balances' },
          { path: '**', redirectTo: ({ url }) => `/inventory/${remainder(url)}` },
        ],
      },
      {
        path: 'reports',
        children: [
          { path: '**', redirectTo: ({ url }) => `/reporting/${remainder(url)}` },
        ],
      },
    ],
  },
  { path: '**', redirectTo: '' },
];
