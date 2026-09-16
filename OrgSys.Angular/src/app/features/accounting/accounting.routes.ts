import { Routes } from '@angular/router';

export const ACCOUNTING_ROUTES: Routes = [
  {
    path: 'accounts',
    loadChildren: () => import('./accounts/accounts.routes').then((m) => m.ACCOUNTS_ROUTES),
  },
  {
    path: 'journal-entries',
    loadChildren: () => import('./journal-entries/journal-entries.routes').then((m) => m.JOURNAL_ENTRIES_ROUTES),
  },
  {
    path: 'fiscal-years',
    loadChildren: () => import('./fiscal-years/fiscal-years.routes').then((m) => m.FISCAL_YEARS_ROUTES),
  },
];
