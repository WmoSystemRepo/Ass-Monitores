import { Route } from '@angular/router';

export const appRoutes: Route[] = [
  {
    path: '',
    loadComponent: () =>
      import('@integrador/monitor-dashboard').then((m) => m.DashboardPageComponent),
  },
  {
    path: 'threads',
    loadComponent: () =>
      import('@integrador/monitor-threads').then((m) => m.ThreadsPageComponent),
  },
  {
    path: 'logs',
    loadComponent: () =>
      import('@integrador/monitor-logs').then((m) => m.LogsPageComponent),
  },
  {
    path: 'tabelas',
    loadComponent: () =>
      import('@integrador/monitor-tables').then((m) => m.TablesHubPageComponent),
  },
  {
    path: 'tabelas/:key',
    loadComponent: () =>
      import('@integrador/monitor-tables').then((m) => m.TableDetailPageComponent),
  },
  {
    path: 'mais-informacoes',
    loadComponent: () =>
      import('@integrador/monitor-dashboard').then((m) => m.DetailsPageComponent),
  },
  {
    path: 'config',
    loadComponent: () =>
      import('@integrador/monitor-config').then((m) => m.ConfigPageComponent),
  },
  { path: '**', redirectTo: '' },
];
