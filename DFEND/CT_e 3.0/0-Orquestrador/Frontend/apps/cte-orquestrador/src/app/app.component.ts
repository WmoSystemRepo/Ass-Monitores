import { ChangeDetectionStrategy, Component, computed, inject } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { ChainOrchestratorStore } from '@orquestrador/monitor-core';

@Component({
  standalone: true,
  imports: [RouterOutlet, RouterLink, RouterLinkActive],
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: 'app-root',
  template: `
    <div class="min-h-screen bg-indigo-950 text-indigo-50">
      <div class="flex min-h-screen">
        <aside class="w-60 shrink-0 border-r border-indigo-900 bg-indigo-950/90 p-4">
          <p class="text-sm font-semibold tracking-wide text-indigo-50">
            Orquestrador CT-e
          </p>
          <p class="mt-1 text-[10px] uppercase tracking-widest text-indigo-400">
            Ambiente de testes
          </p>
          <nav class="mt-6 flex flex-col gap-1 text-sm">
            @for (l of links; track l.path) {
              <a
                [routerLink]="l.path"
                routerLinkActive="bg-indigo-900 text-violet-300"
                [routerLinkActiveOptions]="{ exact: l.path === '/' }"
                class="rounded px-3 py-2 text-indigo-200 hover:bg-indigo-900/70"
                [attr.title]="l.hint"
              >
                <span class="block">{{ l.label }}</span>
                <span class="block text-[10px] text-indigo-500">{{ l.hint }}</span>
              </a>
            }
          </nav>
          <div class="mt-8 text-xs text-indigo-400">
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
            <p class="mt-2 text-sm text-indigo-200">
              {{ store.runningCount() }} sistema(s) ligado(s)
            </p>
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
  readonly store = inject(ChainOrchestratorStore);
  readonly connectionLabel = computed(() =>
    this.store.live() ? 'Orquestrador online' : 'Orquestrador offline'
  );
  readonly links = [
    { path: '/', label: 'Monitor', hint: 'Visão da cadeia' },
    { path: '/resgate', label: 'Resgate CT-e', hint: 'Recuperar do AN' },
  ];
}
