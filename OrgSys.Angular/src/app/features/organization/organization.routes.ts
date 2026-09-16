import { Routes } from '@angular/router';

export const ORGANIZATION_ROUTES: Routes = [
  {
    path: 'branches',
    loadChildren: () => import('./branches/branches.routes').then((m) => m.BRANCHES_ROUTES),
  },
  {
    path: 'companies',
    loadChildren: () => import('./companies/companies.routes').then((m) => m.COMPANIES_ROUTES),
  },
  {
    path: 'settings',
    loadComponent: () =>
      import('./settings/pages/organization-settings-form/organization-settings-form.component').then(
        (m) => m.OrganizationSettingsFormComponent,
      ),
  },
  {
    path: 'departments',
    loadComponent: () =>
      import('../../shared/components/crud-search-list/crud-search-list.component').then((m) => m.CrudSearchListComponent),
    data: { title: 'Departments', entityRoute: 'Department', listMode: 'search' },
  },
];
