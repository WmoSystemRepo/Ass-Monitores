import { Route } from '@angular/router';

export const appRoutes: Route[] = [
  {
    path: '',
    loadComponent: () =>
      import('@receptor/monitor-dashboard').then((m) => m.DashboardPageComponent),
  },
  {
    path: 'threads',
    loadComponent: () =>
      import('@receptor/monitor-threads').then((m) => m.ThreadsPageComponent),
  },
  {
    path: 'logs',
    loadComponent: () =>
      import('@receptor/monitor-logs').then((m) => m.LogsPageComponent),
  },
  {
    path: 'tabelas',
    loadComponent: () =>
      import('@receptor/monitor-tables').then((m) => m.TablesHubPageComponent),
  },
  {
    path: 'tabelas/:key',
    loadComponent: () =>
      import('@receptor/monitor-tables').then((m) => m.TableDetailPageComponent),
  },
  {
    path: 'mais-informacoes',
    loadComponent: () =>
      import('@receptor/monitor-dashboard').then((m) => m.DetailsPageComponent),
  },
  {
    path: 'config',
    loadComponent: () =>
      import('@receptor/monitor-config').then((m) => m.ConfigPageComponent),
  },
  { path: '**', redirectTo: '' },
];
