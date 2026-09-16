import { Routes } from '@angular/router';

export const MASTER_DATA_ROUTES: Routes = [
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
];
