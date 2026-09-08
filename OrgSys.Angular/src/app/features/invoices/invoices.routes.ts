import { Routes } from '@angular/router';

export const INVOICES_ROUTES: Routes = [
  {
    path: ':typeId',
    loadComponent: () => import('./pages/invoice-list/invoice-list.component').then((m) => m.InvoiceListComponent),
  },
  {
    path: ':typeId/new',
    loadComponent: () => import('./pages/invoice-form/invoice-form.component').then((m) => m.InvoiceFormComponent),
  },
  {
    path: ':typeId/:id/edit',
    loadComponent: () => import('./pages/invoice-form/invoice-form.component').then((m) => m.InvoiceFormComponent),
  },
];
