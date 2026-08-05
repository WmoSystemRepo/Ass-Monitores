import { Route } from '@angular/router';

export const appRoutes: Route[] = [
  {
    path: '',
    loadComponent: () =>
      import('@arquivador/monitor-dashboard').then((m) => m.DashboardPageComponent),
  },
  {
    path: 'threads',
    loadComponent: () =>
      import('@arquivador/monitor-threads').then((m) => m.ThreadsPageComponent),
  },
  {
    path: 'logs',
    loadComponent: () =>
      import('@arquivador/monitor-logs').then((m) => m.LogsPageComponent),
  },
  {
    path: 'tabelas',
    loadComponent: () =>
      import('@arquivador/monitor-tables').then((m) => m.TablesHubPageComponent),
  },
  {
    path: 'tabelas/:key',
    loadComponent: () =>
      import('@arquivador/monitor-tables').then((m) => m.TableDetailPageComponent),
  },
  {
    path: 'mais-informacoes',
    loadComponent: () =>
      import('@arquivador/monitor-dashboard').then((m) => m.DetailsPageComponent),
  },
  {
    path: 'config',
    loadComponent: () =>
      import('@arquivador/monitor-config').then((m) => m.ConfigPageComponent),
  },
  { path: '**', redirectTo: '' },
];
