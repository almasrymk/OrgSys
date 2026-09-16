import { Routes } from '@angular/router';

const search = (title: string, entityRoute: string, extraParams: Record<string, string | number> = {}) => ({
  path: '',
  loadComponent: () =>
    import('../../shared/components/crud-search-list/crud-search-list.component').then((m) => m.CrudSearchListComponent),
  data: { title, entityRoute, listMode: 'search', extraParams },
});

export const SAAS_ROUTES: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'tenants' },
  { path: 'tenants', children: [search('Tenants', 'Tenant')] },
  { path: 'plans', children: [search('Plans', 'Plan')] },
  { path: 'features', children: [search('Features', 'Feature')] },
  { path: 'subscriptions', children: [search('Subscriptions', 'Subscription', { tenantId: 0, typeId: 0 })] },
];
