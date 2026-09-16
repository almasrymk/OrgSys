import { Routes } from '@angular/router';

const list = (title: string, entityRoute: string) => ({
  path: '',
  loadComponent: () =>
    import('../../shared/components/crud-search-list/crud-search-list.component').then((m) => m.CrudSearchListComponent),
  data: { title, entityRoute, listMode: 'root' },
});

export const SALES_ROUTES: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'orders' },
  { path: 'orders', children: [list('Sales Orders', 'SalesOrder')] },
  { path: 'quotations', children: [list('Quotations', 'Quotation')] },
];
