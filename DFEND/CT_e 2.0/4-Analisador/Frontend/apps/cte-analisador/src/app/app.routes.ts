import { Route } from '@angular/router';

export const appRoutes: Route[] = [
  {
    path: '',
    loadComponent: () =>
      import('@analisador/monitor-dashboard').then((m) => m.DashboardPageComponent),
  },
  {
    path: 'threads',
    loadComponent: () =>
      import('@analisador/monitor-threads').then((m) => m.ThreadsPageComponent),
  },
  {
    path: 'logs',
    loadComponent: () =>
      import('@analisador/monitor-logs').then((m) => m.LogsPageComponent),
  },
  {
    path: 'tabelas',
    loadComponent: () =>
      import('@analisador/monitor-tables').then((m) => m.TablesHubPageComponent),
  },
  {
    path: 'tabelas/:key',
    loadComponent: () =>
      import('@analisador/monitor-tables').then((m) => m.TableDetailPageComponent),
  },
  {
    path: 'mais-informacoes',
    loadComponent: () =>
      import('@analisador/monitor-dashboard').then((m) => m.DetailsPageComponent),
  },
  {
    path: 'config',
    loadComponent: () =>
      import('@analisador/monitor-config').then((m) => m.ConfigPageComponent),
  },
  { path: '**', redirectTo: '' },
];
