import { Routes } from '@angular/router';

/** Root delegates here; this delegates to each accounting sub-feature's own routes. */
export const ACCOUNTING_ROUTES: Routes = [
  {
    path: 'accounts',
    loadChildren: () => import('./accounts/accounts.routes').then((m) => m.ACCOUNTS_ROUTES),
  },
  {
    path: 'journal-entries',
    loadChildren: () => import('./journal-entries/journal-entries.routes').then((m) => m.JOURNAL_ENTRIES_ROUTES),
  },
];
