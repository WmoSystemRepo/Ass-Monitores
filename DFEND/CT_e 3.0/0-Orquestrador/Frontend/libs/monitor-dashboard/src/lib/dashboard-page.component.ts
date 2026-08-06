import {
  ChangeDetectionStrategy,
  Component,
  computed,
  inject,
} from '@angular/core';
import { DatePipe, NgClass } from '@angular/common';
import {
  ChainOrchestratorStore,
  normalizePhase,
} from '@orquestrador/monitor-core';
import { ChainAnatomyComponent } from './chain-anatomy.component';

@Component({
  selector: 'lib-dashboard-page',
  standalone: true,
  imports: [DatePipe, NgClass, ChainAnatomyComponent],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <section
      class="dashboard-fit flex h-[calc(100vh-3rem)] max-h-[calc(100vh-3rem)] flex-col gap-2 overflow-hidden"
    >
      <header class="flex shrink-0 flex-wrap items-center justify-between gap-2">
        <div class="min-w-0">
          <h1 class="text-base font-semibold leading-tight text-slate-50">
            Orquestrador cadeia CT-e
          </h1>
          <p class="text-[11px] text-slate-400">
            Ligue ou desligue a cadeia e acompanhe os 6 sistemas em tempo real.
          </p>
        </div>
        <div class="flex flex-wrap items-center gap-1.5">
          <span
            class="inline-flex items-center gap-1.5 rounded border px-2 py-1 text-[11px]"
            [class.border-lime-500]="store.live()"
            [class.text-lime-400]="store.live()"
            [class.border-rose-500]="!store.live()"
            [class.text-rose-300]="!store.live()"
            [attr.title]="
              store.live()
                ? 'Orquestrador recebendo snapshot (poll 1s)'
                : 'Orquestrador sem resposta recente'
            "
          >
            @if (store.live()) {
              <span class="live-dot"></span>
            }
            {{ connectionLabel() }}
            @if (store.lastPushAt(); as t) {
              · {{ t | date: 'HH:mm:ss' }}
            }
          </span>
          <button
            type="button"
            class="rounded bg-lime-600 px-2.5 py-1.5 text-xs font-medium text-indigo-950 transition hover:bg-lime-500 disabled:opacity-40"
            [disabled]="store.actionBusy() || !canStart()"
            (click)="confirmStart()"
          >
            Ligar cadeia CT-e
          </button>
          <button
            type="button"
            class="rounded border border-rose-500/60 px-2.5 py-1.5 text-xs text-rose-300 transition hover:bg-rose-950/40 disabled:opacity-40"
            [disabled]="store.actionBusy() || !canStop()"
            (click)="confirmStop()"
          >
            Desligar cadeia
          </button>
        </div>
      </header>

      @if (cascadeBanner(); as banner) {
        <div
          class="pulse-banner flex shrink-0 items-center gap-2 rounded-md border px-3 py-1.5 text-xs"
          [ngClass]="bannerClasses(banner.tone)"
          [attr.role]="banner.tone === 'error' ? 'alert' : null"
        >
          <span class="font-medium">{{ banner.text }}</span>
        </div>
      }

      @if (store.bootError(); as err) {
        <div
          class="shrink-0 rounded border border-rose-500/40 bg-rose-950/40 px-3 py-1 text-xs text-rose-200"
          role="alert"
        >
          Não foi possível falar com o Orquestrador. {{ err }}
        </div>
      }

      @if (distinctActionMessage(); as msg) {
        <div
          class="shrink-0 rounded border border-sky-500/30 bg-sky-950/30 px-3 py-1 text-xs text-sky-100"
        >
          {{ msg }}
        </div>
      }

      <div
        class="health-strip flex shrink-0 flex-wrap items-center gap-x-4 gap-y-1 rounded-md border border-indigo-800/80 bg-indigo-950/50 px-3 py-1.5 text-[11px]"
        [class.health-strip-live]="store.anyRunning()"
      >
        <span class="inline-flex items-baseline gap-1.5">
          <span class="text-slate-400">Orquestrador</span>
          <span
            class="font-medium"
            [class.text-lime-300]="store.live()"
            [class.text-slate-200]="!store.live()"
          >
            {{ store.live() ? 'online' : 'offline' }}
          </span>
        </span>
        <span class="hidden text-indigo-700 sm:inline" aria-hidden="true">·</span>
        <span class="inline-flex items-baseline gap-1.5">
          <span class="text-slate-400">Serviços ativos</span>
          <span class="font-medium text-slate-50">{{ store.runningCount() }}</span>
        </span>
        <span class="hidden text-indigo-700 sm:inline" aria-hidden="true">·</span>
        <span class="inline-flex items-baseline gap-1.5">
          <span class="text-slate-400">Processos no ar</span>
          <span class="font-medium text-slate-50">{{ store.processUpCount() }}</span>
        </span>
        <span class="hidden text-indigo-700 sm:inline" aria-hidden="true">·</span>
        <span class="inline-flex items-baseline gap-1.5">
          <span class="text-slate-400">Fase</span>
          <span
            class="font-medium"
            [class.text-sky-300]="phaseLabel() === 'Ligando' || phaseLabel() === 'Desligando'"
            [class.text-lime-300]="phaseLabel() === 'Em execução'"
            [class.text-slate-200]="phaseLabel() === 'Parada'"
          >
            {{ phaseLabel() }}
          </span>
        </span>
      </div>

      @if (store.alerts().length) {
        <div
          class="shrink-0 rounded border border-rose-500/25 bg-rose-950/20 px-3 py-1 text-[11px] text-rose-200/90"
          role="alert"
        >
          @for (a of store.alerts().slice(0, 3); track a) {
            <span class="mr-3 inline-block">{{ a }}</span>
          }
        </div>
      }

      <div class="min-h-0 flex-1 overflow-hidden">
        <lib-chain-anatomy class="block h-full" />
      </div>
    </section>
  `,
})
export class DashboardPageComponent {
  readonly store = inject(ChainOrchestratorStore);

  readonly connectionLabel = computed(() =>
    this.store.live() ? 'Orquestrador online' : 'Orquestrador offline'
  );

  readonly phaseLabel = computed(() => {
    switch (normalizePhase(this.store.cascadePhase())) {
      case 'starting':
        return 'Ligando';
      case 'stopping':
        return 'Desligando';
      case 'running':
        return 'Em execução';
      default:
        return 'Parada';
    }
  });

  readonly canStart = computed(() => {
    const phase = normalizePhase(this.store.cascadePhase());
    return phase !== 'starting' && phase !== 'stopping';
  });

  readonly canStop = computed(() => {
    const phase = normalizePhase(this.store.cascadePhase());
    if (phase === 'starting' || phase === 'stopping') return false;
    return this.store.anyRunning() || phase === 'running';
  });

  /** Um banner só — erro em rose; não repete a mesma frase no health-strip. */
  readonly cascadeBanner = computed(() => {
    const phase = normalizePhase(this.store.cascadePhase());
    const msg = (this.store.cascadeMessage() || '').trim();
    if (phase === 'starting' || phase === 'stopping') {
      return {
        tone: 'wait' as const,
        text: msg || (phase === 'starting' ? 'Ligando a cadeia…' : 'Desligando a cadeia…'),
      };
    }
    if (this.isFailureMessage(msg)) {
      return { tone: 'error' as const, text: msg };
    }
    if (phase === 'running' || this.store.anyRunning()) {
      return {
        tone: 'ok' as const,
        text: msg || 'Cadeia em execução.',
      };
    }
    return null;
  });

  /** Evita duplicar a mesma frase do banner de cascata. */
  readonly distinctActionMessage = computed(() => {
    const msg = (this.store.actionMessage() || '').trim();
    if (!msg) return null;
    const banner = this.cascadeBanner()?.text?.trim();
    if (banner && msg === banner) return null;
    const cascade = (this.store.cascadeMessage() || '').trim();
    if (cascade && msg === cascade) return null;
    return msg;
  });

  bannerClasses(tone: 'wait' | 'ok' | 'idle' | 'error'): Record<string, boolean> {
    return {
      'border-sky-500/40 bg-sky-950/40 text-sky-100': tone === 'wait',
      'border-lime-500/40 bg-lime-950/30 text-lime-100': tone === 'ok',
      'border-indigo-700/50 bg-indigo-950/40 text-indigo-200': tone === 'idle',
      'border-rose-500/55 bg-rose-950/50 text-rose-100': tone === 'error',
    };
  }

  private isFailureMessage(msg: string): boolean {
    if (!msg) return false;
    const lower = msg.toLowerCase();
    return (
      lower.includes('falha') ||
      lower.includes('erro') ||
      lower.includes('failed') ||
      lower.includes('não foi possível') ||
      lower.includes('nao foi possivel')
    );
  }

  confirmStart(): void {
    if (
      confirm(
        'Ligar a cadeia CT-e? O Orquestrador iniciará os sistemas habilitados em ordem (Receptor → Arquivador → …).'
      )
    ) {
      void this.store.startChain();
    }
  }

  confirmStop(): void {
    if (
      confirm(
        'Desligar a cadeia CT-e? Os sistemas serão parados na ordem inversa.'
      )
    ) {
      void this.store.stopChain();
    }
  }
}
