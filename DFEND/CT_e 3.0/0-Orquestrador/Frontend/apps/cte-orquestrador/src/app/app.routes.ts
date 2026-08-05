import { Route } from '@angular/router';

export const appRoutes: Route[] = [
  {
    path: '',
    loadComponent: () =>
      import('@orquestrador/monitor-dashboard').then((m) => m.DashboardPageComponent),
  },
  {
    path: 'resgate',
    loadComponent: () =>
      import('@orquestrador/resgate-cte').then((m) => m.ResgateNovoPageComponent),
  },
  // Legado: painel de lote próprio removido — Resgate só enfileira na Carga
  { path: 'resgate/lote/:id', redirectTo: 'resgate', pathMatch: 'full' },
  { path: 'resgate/lote/:id/relatorio', redirectTo: 'resgate', pathMatch: 'full' },
  {
    path: 'monitores/:servico',
    loadComponent: () =>
      import('@orquestrador/service-monitors').then((m) => m.ServiceMonitorShellComponent),
    children: [
      {
        path: '',
        loadComponent: () =>
          import('@orquestrador/service-monitors').then((m) => m.ServiceMonitorHomeComponent),
      },
      {
        path: 'threads',
        loadComponent: () =>
          import('@orquestrador/service-monitors').then((m) => m.ThreadsPageComponent),
      },
      {
        path: 'logs',
        loadComponent: () =>
          import('@orquestrador/service-monitors').then((m) => m.LogsPageComponent),
      },
      {
        path: 'tabelas/:key',
        loadComponent: () =>
          import('@orquestrador/service-monitors').then((m) => m.TableDetailPageComponent),
      },
      {
        path: 'tabelas',
        loadComponent: () =>
          import('@orquestrador/service-monitors').then((m) => m.TablesHubPageComponent),
      },
      {
        path: 'mais-informacoes',
        loadComponent: () =>
          import('@orquestrador/service-monitors').then((m) => m.ServiceMonitorDetailsHomeComponent),
      },
      {
        path: 'config',
        loadComponent: () =>
          import('@orquestrador/service-monitors').then((m) => m.ConfigPageComponent),
      },
    ],
  },
  { path: '**', redirectTo: '' },
];
