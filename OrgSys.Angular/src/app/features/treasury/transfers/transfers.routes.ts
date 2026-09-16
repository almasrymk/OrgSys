import { Routes } from '@angular/router';

export const TRANSFERS_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./pages/financial-transfer-list/financial-transfer-list.component').then((m) => m.FinancialTransferListComponent),
  },
  {
    path: 'new',
    loadComponent: () =>
      import('./pages/financial-transfer-form/financial-transfer-form.component').then((m) => m.FinancialTransferFormComponent),
  },
];
