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
  { path: '**', redirectTo: '' },
];
