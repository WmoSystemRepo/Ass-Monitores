import { ChangeDetectionStrategy, Component, computed, inject } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { ChainOrchestratorStore } from '@orquestrador/monitor-core';
import { ConfirmDialogComponent } from '@orquestrador/shared-ui';
import { PresentationTourPanelComponent } from '@orquestrador/monitor-dashboard';

@Component({
  standalone: true,
  imports: [
    RouterOutlet,
    RouterLink,
    RouterLinkActive,
    ConfirmDialogComponent,
    PresentationTourPanelComponent,
  ],
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: 'app-root',
  template: `
    <div class="flex h-screen overflow-hidden bg-indigo-950 text-indigo-50">
      <aside class="flex w-56 shrink-0 flex-col border-r border-indigo-900 bg-indigo-950/90 p-3">
        <p class="text-sm font-semibold tracking-wide text-indigo-50">
          Orquestrador CT-e
        </p>
        <p class="mt-0.5 text-[10px] uppercase tracking-widest text-indigo-400">
          Ambiente de testes
        </p>
        <nav class="mt-4 flex flex-col gap-0.5 text-sm">
          @for (l of links; track l.path) {
            <a
              [routerLink]="l.path"
              routerLinkActive="bg-indigo-900 text-violet-300"
              [routerLinkActiveOptions]="{ exact: l.path === '/' }"
              class="rounded px-2.5 py-1.5 text-indigo-200 hover:bg-indigo-900/70"
              [attr.title]="l.hint"
              [attr.data-tour]="l.tour"
            >
              <span class="block">{{ l.label }}</span>
              <span class="block text-[10px] text-indigo-500">{{ l.hint }}</span>
            </a>
          }
        </nav>
        <div class="mt-auto pt-4 text-xs text-indigo-400">
          <p
            class="inline-flex items-center gap-1.5"
            [attr.title]="
              store.live()
                ? 'Orquestrador recebendo snapshot'
                : 'Orquestrador sem push recente'
            "
          >
            @if (store.live()) {
              <span class="live-dot"></span>
            }
            {{ connectionLabel() }}
          </p>
          <p
            class="mt-1.5 text-sm text-indigo-200"
            [attr.title]="systemsSummaryTitle()"
          >
            {{ systemsSummary() }}
          </p>
        </div>
      </aside>
      <main class="min-h-0 min-w-0 flex-1 overflow-hidden p-3">
        <router-outlet />
      </main>
    </div>
    <lib-confirm-dialog />
    <lib-presentation-tour-panel />
  `,
})
export class AppComponent {
  readonly store = inject(ChainOrchestratorStore);
  readonly connectionLabel = computed(() =>
    this.store.live() ? 'Orquestrador online' : 'Orquestrador offline'
  );

  /** Ativos = filas ligadas (processo no ar + Executar=1). Sem estado “pausado” na cascata. */
  readonly systemsSummary = computed(() => {
    const ativos = this.store.runningCount();
    const noAr = this.store.processUpCount();
    if (ativos === 0 && noAr === 0) {
      return 'Nenhuma fila ativa';
    }
    return `${ativos} fila(s) ativa(s)`;
  });

  readonly systemsSummaryTitle = computed(() => {
    const ativos = this.store.runningCount();
    const noAr = this.store.processUpCount();
    return `Filas ativas: ${ativos} · Processos no ar: ${noAr}. Ligar as filas = processo + Executar=1.`;
  });

  readonly links = [
    {
      path: '/',
      label: 'Dashboard',
      hint: 'Ligar e acompanhar as filas CT-e',
      tour: 'nav-dashboard',
    },
    {
      path: '/resgate',
      label: 'Resgate CT-e',
      hint: 'Recuperar do AN',
      tour: 'nav-resgate',
    },
  ];
}
