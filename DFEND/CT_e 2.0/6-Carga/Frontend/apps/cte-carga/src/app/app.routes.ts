import { Route } from '@angular/router';

export const appRoutes: Route[] = [
  {
    path: '',
    loadComponent: () =>
      import('@Carga/monitor-dashboard').then((m) => m.DashboardPageComponent),
  },
  {
    path: 'threads',
    loadComponent: () =>
      import('@Carga/monitor-threads').then((m) => m.ThreadsPageComponent),
  },
  {
    path: 'logs',
    loadComponent: () =>
      import('@Carga/monitor-logs').then((m) => m.LogsPageComponent),
  },
  {
    path: 'tabelas',
    loadComponent: () =>
      import('@Carga/monitor-tables').then((m) => m.TablesHubPageComponent),
  },
  {
    path: 'tabelas/:key',
    loadComponent: () =>
      import('@Carga/monitor-tables').then((m) => m.TableDetailPageComponent),
  },
  {
    path: 'mais-informacoes',
    loadComponent: () =>
      import('@Carga/monitor-dashboard').then((m) => m.DetailsPageComponent),
  },
  {
    path: 'config',
    loadComponent: () =>
      import('@Carga/monitor-config').then((m) => m.ConfigPageComponent),
  },
  { path: '**', redirectTo: '' },
];
