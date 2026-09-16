import { Routes } from '@angular/router';

const lookup = (title: string, entityRoute: string, action: string, fields: { name: string; label: string; type?: string; value?: string | number }[]) => ({
  path: '',
  loadComponent: () =>
    import('../../shared/components/query-workspace/query-workspace.component').then((m) => m.QueryWorkspaceComponent),
  data: { title, entityRoute, action, fields },
});

export const BUDGETING_ROUTES: Routes = [
  { path: '', children: [lookup('Budget vs Actual', 'Budget', 'VsActual', [{ name: 'budgetId', label: 'Budget Id', type: 'number' }])] },
];

export const WORKFLOW_ROUTES: Routes = [
  {
    path: '',
    children: [
      lookup('Approval by Document', 'Approval', 'GetByDocument', [
        { name: 'documentType', label: 'Document Type', value: 'PurchaseRequisition' },
        { name: 'documentId', label: 'Document Id', type: 'number' },
      ]),
    ],
  },
];

export const TAX_ROUTES: Routes = [
  { path: '', children: [lookup('Invoice Tax Snapshot', 'Tax', 'Snapshot', [{ name: 'invoiceId', label: 'Invoice Id', type: 'number' }])] },
];

export const FIXED_ASSETS_ROUTES: Routes = [
  { path: '', children: [lookup('Fixed Asset', 'FixedAsset', 'Get', [{ name: 'assetId', label: 'Asset Id', type: 'number' }])] },
];
