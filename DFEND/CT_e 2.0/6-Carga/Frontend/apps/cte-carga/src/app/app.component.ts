import { ChangeDetectionStrategy, Component, computed, inject } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { CargaMonitorStore } from '@Carga/monitor-core';
import { monitorConnectionLabel, CargaStatusLabel } from '@Carga/shared-utils';

@Component({
  standalone: true,
  imports: [RouterOutlet, RouterLink, RouterLinkActive],
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: 'app-root',
  template: `
    <div class="min-h-screen bg-zinc-950 text-zinc-100">
      <div class="flex min-h-screen">
        <aside class="w-60 shrink-0 border-r border-zinc-800 bg-zinc-900/80 p-4">
          <p class="text-sm font-semibold tracking-wide text-zinc-100">
            Monitor Carga CT-e
          </p>
          <p class="mt-1 text-[10px] uppercase tracking-widest text-zinc-500">Ambiente de testes</p>
          <nav class="mt-6 flex flex-col gap-1 text-sm">
            @for (l of links; track l.path) {
              <a
                [routerLink]="l.path"
                routerLinkActive="bg-zinc-800 text-teal-300"
                [routerLinkActiveOptions]="{ exact: l.path === '/' }"
                class="rounded px-3 py-2 text-zinc-300 hover:bg-zinc-900"
                [attr.title]="l.hint"
              >
                <span class="block">{{ l.label }}</span>
                <span class="block text-[10px] text-zinc-500">{{ l.hint }}</span>
              </a>
            }
          </nav>
          <div class="mt-8 text-xs text-zinc-500">
            <p
              class="inline-flex items-center gap-1.5"
              [attr.title]="
                store.live()
                  ? 'Monitor recebendo atualizações (SignalR)'
                  : 'Monitor sem push recente'
              "
            >
              @if (store.live()) {
                <span class="live-dot"></span>
              }
              {{ connectionLabel() }}
            </p>
            <p class="mt-2 text-sm text-zinc-300">Carga · {{ statusLabel() }}</p>
          </div>
        </aside>
        <main class="min-w-0 flex-1 overflow-y-auto p-6">
          <router-outlet />
        </main>
      </div>
    </div>
  `,
})
export class AppComponent {
  readonly store = inject(CargaMonitorStore);
  readonly statusLabel = computed(() =>
    CargaStatusLabel(this.store.service()?.scmStatus, this.store.service()?.executar)
  );
  readonly connectionLabel = computed(() => monitorConnectionLabel(this.store.live()));
  readonly links = [
    { path: '/', label: 'Monitor', hint: 'Visão operacional' },
    { path: '/threads', label: 'Threads', hint: 'Linhas de trabalho' },
    { path: '/logs', label: 'Histórico', hint: 'O que aconteceu' },
    { path: '/tabelas', label: 'Tabelas', hint: 'Banco em tempo real' },
    {
      path: '/config',
      label: 'Configurações',
      hint: 'Somente leitura · origem Carga',
    },
  ];
}
