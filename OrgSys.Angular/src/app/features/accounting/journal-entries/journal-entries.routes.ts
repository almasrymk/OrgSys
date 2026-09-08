import { Routes } from '@angular/router';

export const JOURNAL_ENTRIES_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () => import('./pages/journal-list/journal-list.component').then((m) => m.JournalListComponent),
  },
  {
    path: 'new',
    loadComponent: () => import('./pages/journal-form/journal-form.component').then((m) => m.JournalFormComponent),
  },
  {
    path: ':id/edit',
    loadComponent: () => import('./pages/journal-form/journal-form.component').then((m) => m.JournalFormComponent),
  },
];
