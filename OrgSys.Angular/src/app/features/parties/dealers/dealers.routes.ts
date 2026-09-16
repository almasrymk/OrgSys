import { Routes } from '@angular/router';
import { DealerType } from './models/dealer.model';

const listComponent = () => import('./pages/dealer-list/dealer-list.component').then((m) => m.DealerListComponent);
const formComponent = () => import('./pages/dealer-form/dealer-form.component').then((m) => m.DealerFormComponent);

export const DEALERS_ROUTES: Routes = [
  {
    path: 'customers',
    loadComponent: listComponent,
    data: { dealerType: DealerType.Client, title: 'Customers', permissionPrefix: 'Clients', basePath: '/parties/dealers/customers' },
  },
  {
    path: 'customers/new',
    loadComponent: formComponent,
    data: { dealerType: DealerType.Client, title: 'Customers', basePath: '/parties/dealers/customers' },
  },
  {
    path: 'customers/:id/edit',
    loadComponent: formComponent,
    data: { dealerType: DealerType.Client, title: 'Customers', basePath: '/parties/dealers/customers' },
  },
  {
    path: 'suppliers',
    loadComponent: listComponent,
    data: { dealerType: DealerType.Supplier, title: 'Suppliers', permissionPrefix: 'Suppliers', basePath: '/parties/dealers/suppliers' },
  },
  {
    path: 'suppliers/new',
    loadComponent: formComponent,
    data: { dealerType: DealerType.Supplier, title: 'Suppliers', basePath: '/parties/dealers/suppliers' },
  },
  {
    path: 'suppliers/:id/edit',
    loadComponent: formComponent,
    data: { dealerType: DealerType.Supplier, title: 'Suppliers', basePath: '/parties/dealers/suppliers' },
  },
];
