import { Routes } from '@angular/router';
import { INVOICE_TYPE_PATHS } from './models/invoice-lookups.model';

const listComponent = () => import('./pages/invoice-list/invoice-list.component').then((m) => m.InvoiceListComponent);
const formComponent = () => import('./pages/invoice-form/invoice-form.component').then((m) => m.InvoiceFormComponent);

function typedRoutes(path: string, typeId: number): Routes {
  return [
    { path, loadComponent: listComponent, data: { typeId } },
    { path: `${path}/new`, loadComponent: formComponent, data: { typeId } },
    { path: `${path}/:id/edit`, loadComponent: formComponent, data: { typeId } },
  ];
}

export const COMMERCIAL_DOCUMENTS_ROUTES: Routes = [
  ...typedRoutes(INVOICE_TYPE_PATHS[1], 1),
  ...typedRoutes(INVOICE_TYPE_PATHS[2], 2),
  ...typedRoutes(INVOICE_TYPE_PATHS[3], 3),
  ...typedRoutes(INVOICE_TYPE_PATHS[4], 4),
];
