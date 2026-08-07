import {
  ChangeDetectionStrategy,
  Component,
  OnInit,
  computed,
  inject,
} from '@angular/core';
import { DatePipe, NgClass } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import {
  ChainOrchestratorStore,
  normalizePhase,
} from '@orquestrador/monitor-core';
import { ConfirmDialogService } from '@orquestrador/shared-ui';
import { ChainAnatomyComponent } from './chain-anatomy.component';
import { PresentationTourStore } from './presentation-tour.store';

@Component({
  selector: 'lib-dashboard-page',
  standalone: true,
  imports: [
    DatePipe,
    NgClass,
    ChainAnatomyComponent,
  ],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <section
      class="dashboard-fit flex h-full max-h-full flex-col gap-1.5 overflow-hidden"
      data-tour="overview"
    >
      <header class="flex shrink-0 flex-wrap items-center justify-between gap-2">
        <div class="min-w-0">
          <h1 class="text-base font-semibold leading-tight text-slate-50">
            Orquestrador cadeia CT-e
          </h1>
          <p class="text-[11px] text-slate-400">
            Ligue ou desligue as filas e acompanhe os 6 sistemas em tempo real.
          </p>
        </div>
        <div class="flex flex-wrap items-center gap-1.5" data-tour="controls">
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
            class="rounded-md border border-indigo-500/50 px-2.5 py-2 text-xs font-medium text-indigo-200 transition hover:bg-indigo-900/50"
            [attr.title]="
              tour.active()
                ? 'Apresentação em andamento'
                : 'Tour guiado + simulação visual (sem CT-e real)'
            "
            (click)="tour.active() ? tour.exit() : tour.start()"
          >
            {{ tour.active() ? 'Sair da apresentação' : 'Apresentação' }}
          </button>
          <button
            type="button"
            class="cta-start rounded-md bg-lime-500 px-3.5 py-2 text-sm font-semibold text-indigo-950 shadow-md shadow-lime-900/30 transition hover:bg-lime-400 disabled:opacity-40"
            [disabled]="store.actionBusy() || !canStart()"
            (click)="confirmStart()"
          >
            Ligar as filas
          </button>
          <button
            type="button"
            class="rounded-md border border-rose-500/60 px-2.5 py-2 text-xs font-medium text-rose-300 transition hover:bg-rose-950/40 disabled:opacity-40"
            [disabled]="store.actionBusy() || !canStop()"
            (click)="confirmStop()"
          >
            Desligar filas
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
        data-tour="health"
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
        @if (store.anyRunning() || queueBusyCount() > 0) {
          <span class="hidden text-indigo-700 sm:inline" aria-hidden="true">·</span>
          <span class="inline-flex items-baseline gap-1.5">
            <span class="text-slate-400">Com fila</span>
            <span
              class="font-medium"
              [class.text-lime-300]="store.anyRunning()"
              [class.text-amber-300]="!store.anyRunning()"
              [attr.title]="
                store.anyRunning()
                  ? 'Serviços com backlog enquanto a cadeia está ligada (ativo)'
                  : 'Backlog com cadeia parada — use Ligar as filas'
              "
            >
              {{ queueBusyCount() }}
            </span>
          </span>
          <span class="hidden text-indigo-700 sm:inline" aria-hidden="true">·</span>
          <span class="inline-flex items-baseline gap-1.5">
            <span class="text-slate-400">Arquivos</span>
            <span
              class="font-medium"
              [class.text-lime-300]="store.anyRunning()"
              [class.text-amber-300]="!store.anyRunning()"
            >
              {{ totalQueueFiles() }}
            </span>
          </span>
          @if (store.anyRunning() && totalQueueFiles() === 0) {
            <span class="hidden text-indigo-700 sm:inline" aria-hidden="true">·</span>
            <span class="inline-flex items-baseline gap-1.5" title="Cadeia ligada e consumindo; nenhum CT-e na fila no momento">
              <span class="text-slate-400">Fluxo</span>
              <span class="font-medium text-lime-300">ativo · sem fila</span>
            </span>
          }
        }
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
export class DashboardPageComponent implements OnInit {
  readonly store = inject(ChainOrchestratorStore);
  readonly tour = inject(PresentationTourStore);
  private readonly confirmDialog = inject(ConfirmDialogService);
  private readonly route = inject(ActivatedRoute);

  ngOnInit(): void {
    const q = this.route.snapshot.queryParamMap;
    if (q.get('apresentacao') === '1' || q.get('apresentacao') === 'true') {
      this.tour.start();
    }
  }

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

  readonly queueBusyCount = computed(
    () =>
      this.store.systems().filter((s) => s.agora || !!s.hasQueueWork).length
  );

  readonly totalQueueFiles = computed(() =>
    this.store.systems().reduce((sum, s) => sum + (Number(s.queueDepth) || 0), 0)
  );

  readonly canStart = computed(() => {
    const phase = normalizePhase(this.store.cascadePhase());
    return phase !== 'starting' && phase !== 'stopping';
  });

  readonly canStop = computed(() => {
    const phase = normalizePhase(this.store.cascadePhase());
    if (phase === 'starting' || phase === 'stopping') return false;
    // Desligar filas fica habilitado com processos no ar (não só com Executar=1).
    return (
      this.store.processUpCount() > 0 ||
      this.store.anyRunning() ||
      phase === 'running'
    );
  });

  /** Um banner só — erro em rose; não repete a mesma frase no health-strip. */
  readonly cascadeBanner = computed(() => {
    const phase = normalizePhase(this.store.cascadePhase());
    const msg = (this.store.cascadeMessage() || '').trim();
    if (phase === 'starting' || phase === 'stopping') {
      return {
        tone: 'wait' as const,
        text: msg || (phase === 'starting' ? 'Ligando as filas…' : 'Desligando filas…'),
      };
    }
    if (this.isFailureMessage(msg)) {
      return { tone: 'error' as const, text: msg };
    }
    if (phase === 'running' || this.store.anyRunning() || this.store.processUpCount() > 0) {
      return {
        tone: 'ok' as const,
        text: msg || 'Filas em execução.',
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

  async confirmStart(): Promise<void> {
    const ok = await this.confirmDialog.ask({
      title: 'Ligar as filas CT-e?',
      message:
        'O Orquestrador sobe os 6 serviços na ordem Receptor → Arquivador → Sintetizador → Analisador → Integrador → Carga e começa a processar documentos.',
      confirmLabel: 'Ligar filas',
      cancelLabel: 'Cancelar',
      tone: 'primary',
    });
    if (ok) {
      void this.store.startChain();
    }
  }

  async confirmStop(): Promise<void> {
    const ok = await this.confirmDialog.ask({
      title: 'Desligar as filas CT-e?',
      message:
        'Os serviços param na ordem inversa e deixam de processar até você ligar novamente. Documentos já na fila permanecem (a tela pode mostrar Fase Parada com NA FILA) — isso é normal; não apaga o backlog.',
      confirmLabel: 'Desligar filas',
      cancelLabel: 'Cancelar',
      tone: 'danger',
    });
    if (ok) {
      void this.store.stopChain();
    }
  }
}
