import { Routes } from '@angular/router';

const outstanding = (title: string, entityRoute: string) => ({
  path: '',
  loadComponent: () =>
    import('../../shared/components/crud-search-list/crud-search-list.component').then((m) => m.CrudSearchListComponent),
  data: { title, entityRoute, listMode: 'action', action: 'Outstanding' },
});

export const RECEIVABLES_ROUTES: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'outstanding' },
  { path: 'outstanding', children: [outstanding('Outstanding Receivables', 'Receivable')] },
];

export const PAYABLES_ROUTES: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'outstanding' },
  { path: 'outstanding', children: [outstanding('Outstanding Payables', 'Payable')] },
];
