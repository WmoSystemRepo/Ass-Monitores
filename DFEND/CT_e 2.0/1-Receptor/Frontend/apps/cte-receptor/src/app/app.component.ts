import { ChangeDetectionStrategy, Component, computed, inject } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { ReceptorMonitorStore } from '@receptor/monitor-core';
import { monitorConnectionLabel, receptorStatusLabel } from '@receptor/shared-utils';

@Component({
  standalone: true,
  imports: [RouterOutlet, RouterLink, RouterLinkActive],
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: 'app-root',
  template: `
    <div class="min-h-screen bg-slate-950 text-slate-100">
      <div class="flex min-h-screen">
        <aside class="w-60 shrink-0 border-r border-slate-800 bg-slate-925/80 p-4">
          <p class="text-sm font-semibold tracking-wide text-slate-100">
            Monitor Receptor CT-e
          </p>
          <p class="mt-1 text-[10px] uppercase tracking-widest text-slate-500">Ambiente de testes</p>
          <nav class="mt-6 flex flex-col gap-1 text-sm">
            @for (l of links; track l.path) {
              <a
                [routerLink]="l.path"
                routerLinkActive="bg-slate-800 text-cyan-300"
                [routerLinkActiveOptions]="{ exact: l.path === '/' }"
                class="rounded px-3 py-2 text-slate-300 hover:bg-slate-900"
                [attr.title]="l.hint"
              >
                <span class="block">{{ l.label }}</span>
                <span class="block text-[10px] text-slate-500">{{ l.hint }}</span>
              </a>
            }
          </nav>
          <div class="mt-8 text-xs text-slate-500">
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
            <p class="mt-2 text-sm text-slate-300">Receptor · {{ statusLabel() }}</p>
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
  readonly store = inject(ReceptorMonitorStore);
  readonly statusLabel = computed(() =>
    receptorStatusLabel(this.store.service()?.scmStatus, this.store.service()?.executar)
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
      hint: 'Somente leitura · origem Receptor',
    },
  ];
}
