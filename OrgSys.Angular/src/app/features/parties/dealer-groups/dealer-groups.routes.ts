import { Routes } from '@angular/router';
import { DealerType } from './models/dealer-group.model';

const listComponent = () =>
  import('./pages/dealer-group-list/dealer-group-list.component').then((m) => m.DealerGroupListComponent);
const formComponent = () =>
  import('./pages/dealer-group-form/dealer-group-form.component').then((m) => m.DealerGroupFormComponent);

export const DEALER_GROUPS_ROUTES: Routes = [
  {
    path: 'client-groups',
    loadComponent: listComponent,
    data: { dealerType: DealerType.Client, title: 'Client Groups', permissionPrefix: 'ClientGroups', basePath: '/customers-suppliers/dealer-groups/client-groups' },
  },
  {
    path: 'client-groups/new',
    loadComponent: formComponent,
    data: { dealerType: DealerType.Client, title: 'Client Groups', basePath: '/customers-suppliers/dealer-groups/client-groups' },
  },
  {
    path: 'client-groups/:id/edit',
    loadComponent: formComponent,
    data: { dealerType: DealerType.Client, title: 'Client Groups', basePath: '/customers-suppliers/dealer-groups/client-groups' },
  },
  {
    path: 'supplier-groups',
    loadComponent: listComponent,
    data: { dealerType: DealerType.Supplier, title: 'Supplier Groups', permissionPrefix: 'SupplierGroups', basePath: '/customers-suppliers/dealer-groups/supplier-groups' },
  },
  {
    path: 'supplier-groups/new',
    loadComponent: formComponent,
    data: { dealerType: DealerType.Supplier, title: 'Supplier Groups', basePath: '/customers-suppliers/dealer-groups/supplier-groups' },
  },
  {
    path: 'supplier-groups/:id/edit',
    loadComponent: formComponent,
    data: { dealerType: DealerType.Supplier, title: 'Supplier Groups', basePath: '/customers-suppliers/dealer-groups/supplier-groups' },
  },
];
