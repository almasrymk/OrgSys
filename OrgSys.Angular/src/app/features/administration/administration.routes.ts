import { Routes } from '@angular/router';

/** Root delegates here; this delegates to each master-data feature's own routes. */
export const ADMINISTRATION_ROUTES: Routes = [
  {
    path: 'countries',
    loadChildren: () => import('./countries/countries.routes').then((m) => m.COUNTRIES_ROUTES),
  },
  {
    path: 'cities',
    loadChildren: () => import('./cities/cities.routes').then((m) => m.CITIES_ROUTES),
  },
  {
    path: 'districts',
    loadChildren: () => import('./districts/districts.routes').then((m) => m.DISTRICTS_ROUTES),
  },
  {
    path: 'currencies',
    loadChildren: () => import('./currencies/currencies.routes').then((m) => m.CURRENCIES_ROUTES),
  },
  {
    path: 'fiscal-years',
    loadChildren: () => import('./fiscal-years/fiscal-years.routes').then((m) => m.FISCAL_YEARS_ROUTES),
  },
  {
    path: 'branches',
    loadChildren: () => import('./branches/branches.routes').then((m) => m.BRANCHES_ROUTES),
  },
  {
    path: 'banks',
    loadChildren: () => import('./banks/banks.routes').then((m) => m.BANKS_ROUTES),
  },
  {
    path: 'bank-branches',
    loadChildren: () => import('./bank-branches/bank-branches.routes').then((m) => m.BANK_BRANCHES_ROUTES),
  },
  {
    path: 'products',
    loadChildren: () => import('./products/products.routes').then((m) => m.PRODUCTS_ROUTES),
  },
];
