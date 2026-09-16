import { Routes } from '@angular/router';
import { FinancialTransactionType } from './models/financial.model';

const listComponent = () =>
  import('./pages/financial-transaction-list/financial-transaction-list.component').then((m) => m.FinancialTransactionListComponent);
const formComponent = () =>
  import('./pages/financial-transaction-form/financial-transaction-form.component').then((m) => m.FinancialTransactionFormComponent);

function typedRoutes(path: string, typeId: number): Routes {
  return [
    { path, loadComponent: listComponent, data: { typeId } },
    { path: `${path}/new`, loadComponent: formComponent, data: { typeId } },
  ];
}

export const TRANSACTIONS_ROUTES: Routes = [
  ...typedRoutes('receipts', FinancialTransactionType.Receipt),
  ...typedRoutes('payments', FinancialTransactionType.Payment),
  ...typedRoutes('transfer-in', FinancialTransactionType.TransferIn),
  ...typedRoutes('deposits', FinancialTransactionType.Deposit),
  ...typedRoutes('withdrawals', FinancialTransactionType.Withdrawal),
  ...typedRoutes('fees', FinancialTransactionType.Fee),
  ...typedRoutes('interest', FinancialTransactionType.Interest),
  ...typedRoutes('cheques', FinancialTransactionType.Cheque),
  ...typedRoutes('adjustments', FinancialTransactionType.Adjustment),
  ...typedRoutes('transfer-out', FinancialTransactionType.TransferOut),
];
