import { Routes } from '@angular/router';

const list = (title: string, entityRoute: string) => ({
  path: '',
  loadComponent: () =>
    import('../../shared/components/crud-search-list/crud-search-list.component').then((m) => m.CrudSearchListComponent),
  data: { title, entityRoute, listMode: 'search' },
});

export const PURCHASING_ROUTES: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'requisitions' },
  { path: 'requisitions', children: [list('Purchase Requisitions', 'PurchaseRequisition')] },
  { path: 'orders', children: [list('Purchase Orders', 'PurchaseOrder')] },
  {
    path: 'match',
    loadComponent: () =>
      import('../../shared/components/query-workspace/query-workspace.component').then((m) => m.QueryWorkspaceComponent),
    data: {
      title: 'Three-way match',
      entityRoute: 'PurchaseOrder',
      action: 'ThreeWayMatch',
      fields: [{ name: 'purchaseOrderId', label: 'Purchase Order Id', type: 'number' }],
    },
  },
];
