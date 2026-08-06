import {
  ChangeDetectionStrategy,
  Component,
  computed,
  inject,
  input,
  OnDestroy,
  signal,
} from '@angular/core';
import { DatePipe } from '@angular/common';
import { RouterLink } from '@angular/router';
import { ServiceMonitorStore } from '../service-monitor.store';
import { RecentDocument } from '@orquestrador/shared-data';

/** Estágios do ciclo de análise (diagrama fluxo — 5 plataformas). */
export type AnatomyStage =
  | 'fila'
  | 'temp'
  | 'classificar'
  | 'detalhar'
  | 'limpar';

export interface FlyingPacket {
  id: string;
  kind: 'cte' | 'envelope';
  label: string;
  /** 0=fila … 4=limpar */
  lane: number;
}

@Component({
  selector: 'lib-analisador-anatomy-flow',
  standalone: true,
  imports: [DatePipe, RouterLink],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <div
      class="pipeline-anatomy anatomy-poster anatomy-poster-fill flex h-full min-h-0 flex-col overflow-hidden rounded-xl border border-zinc-600/60 shadow-md"
      [class.anatomy-poster-live]="running()"
      [class.anatomy-poster-busy]="!!activeStage()"
    >
      <div class="anatomy-poster-head flex shrink-0 flex-wrap items-center justify-between gap-2 px-4 pt-3">
        <div class="min-w-0">
          <p class="text-[10px] font-semibold uppercase tracking-[0.16em] text-violet-300/90">
            Pipeline interno do Analisador
          </p>
          <h2
            class="text-base font-semibold leading-tight text-slate-50"
            title="DFEND_CTe_Analisador — ciclo fila → temp → classificar → detalhar → limpar"
          >
            Fluxo do Analisador CT-e
          </h2>
          <p class="text-[11px] text-zinc-400">
            {{ caption() }}
          </p>
          <ul class="anatomy-legend" aria-label="Legenda de status">
            <li class="anatomy-legend-item">
              <span class="anatomy-legend-swatch anatomy-legend-agora"></span>
              Agora
            </li>
            <li class="anatomy-legend-item">
              <span class="anatomy-legend-swatch anatomy-legend-running"></span>
              Feito
            </li>
            <li class="anatomy-legend-item">
              <span class="anatomy-legend-swatch anatomy-legend-stopped"></span>
              Parado
            </li>
          </ul>
        </div>
        <div class="flex flex-wrap items-center gap-2">
          @if (latest(); as lote) {
            <div class="rounded-lg border border-zinc-600/70 bg-zinc-900/70 px-3 py-1.5 shadow-sm">
              <p class="text-[9px] font-semibold uppercase tracking-wide text-violet-400">Último lote</p>
              <p class="font-mono text-xs text-zinc-200">
                NSU {{ lote.nsu }} → {{ lote.nsuFinal ?? lote.nsu }}
                <span class="text-zinc-500"> · {{ lote.qtdDocumento }} CT-e</span>
                @if (lote.dtcAtualizacao) {
                  <span class="text-zinc-500"> · {{ lote.dtcAtualizacao | date: 'HH:mm:ss' }}</span>
                }
              </p>
            </div>
          }
          <a
            routerLink="/monitores/analisador/mais-informacoes"
            class="rounded border border-zinc-600 bg-zinc-900/60 px-2.5 py-1 text-[11px] font-medium text-violet-300 hover:bg-zinc-800"
          >
            Mais informações →
          </a>
        </div>
      </div>

      @if (beltMoving()) {
        <div
          class="anatomy-belt mx-4 mt-2 shrink-0 overflow-hidden rounded-md border border-zinc-600/50 bg-zinc-900/40"
          title="NSU em trânsito na esteira"
        >
          <div class="anatomy-belt-track anatomy-belt-move">
            @for (n of beltItems(); track n) {
              <span class="anatomy-cte-doc">CT-e</span>
            }
            @for (n of beltItems(); track 'b' + n) {
              <span class="anatomy-cte-doc">CT-e</span>
            }
          </div>
        </div>
      } @else {
        <p
          class="anatomy-belt-idle-hint mx-4 mt-2 shrink-0 text-center text-[11px] text-slate-500"
          title="Aguardando próximo ciclo de análise"
        >
          Sem CT-e em trânsito
        </p>
      }

      <!-- Diagrama principal — uma composição centrada -->
      <div class="relative mx-3 mt-2 min-h-0 min-w-0 flex-1 overflow-x-auto overflow-y-hidden">
        <div class="anatomy-fill-board relative flex h-full min-w-[880px] flex-col items-stretch px-2 py-2">
          <div class="anatomy-cycle-block relative z-[1] mx-auto w-full">
            <div class="anatomy-path-line anatomy-path-line-5" aria-hidden="true"></div>

            @for (pkt of packets(); track pkt.id; let pi = $index) {
              <div
                class="anatomy-flyer"
                [class.anatomy-flyer-envelope]="pkt.kind === 'envelope'"
                [style.left.%]="lanePercent(pkt.lane) + (pi % 3) * 1.4 - 1.4"
                [style.top.rem]="0.2 + (pi % 3) * 0.9"
              >
                <span class="anatomy-flyer-label">{{ pkt.kind === 'envelope' ? '✉' : 'CT-e' }}</span>
                <span class="anatomy-flyer-nsu">{{ pkt.label }}</span>
              </div>
            }

            <div class="anatomy-stages relative z-[1] shrink-0 py-1">
              @for (step of steps; track step.id; let i = $index) {
                <div
                  class="anatomy-stage"
                  [class.anatomy-stage-active]="activeStage() === step.id"
                  [class.anatomy-stage-done]="isDone(step.id)"
                >
                  @if (activeStage() === step.id) {
                    <span class="anatomy-now">AGORA</span>
                  }
                  <div
                    class="anatomy-platform"
                    [class.anatomy-platform-active]="activeStage() === step.id"
                    [class.anatomy-platform-done]="isDone(step.id)"
                    [attr.title]="step.blurb + ' · ' + step.techHint"
                  >
                    <div class="anatomy-iso" [attr.data-icon]="step.id"></div>
                  </div>
                  <p class="anatomy-stage-title">{{ step.title }}</p>
                  <p class="anatomy-stage-tag">{{ step.tag }}</p>
                  <p class="anatomy-stage-count">{{ count(step.id) }}</p>
                </div>
                @if (i < steps.length - 1) {
                  <div
                    class="anatomy-step-arrow"
                    [class.anatomy-step-arrow-hot]="arrowHot(i)"
                    aria-hidden="true"
                  >
                    →
                  </div>
                }
              }
            </div>

            <div class="anatomy-summary-bar mt-3 shrink-0">
              <div>
                <span class="anatomy-summary-label">NSU</span>
                <span class="anatomy-summary-value">{{ store.global()?.mainNsu || '—' }}</span>
              </div>
              <div>
                <span class="anatomy-summary-label">Na temporária</span>
                <span class="anatomy-summary-value">{{ store.queues()?.tempBacklog ?? 0 }}</span>
              </div>
              <div>
                <span class="anatomy-summary-label">Na fila</span>
                <span class="anatomy-summary-value">{{ store.queues()?.serviceBrokerDepth ?? 0 }}</span>
              </div>
              <div title="Threads">
                <a
                  routerLink="/monitores/analisador/threads"
                  class="anatomy-summary-label hover:text-violet-300"
                  >Linhas de trabalho</a
                >
                <span class="anatomy-summary-value">{{ store.global()?.configuredThreads ?? '—' }}</span>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  `,
})
export class AnalisadorAnatomyFlowComponent implements OnDestroy {
  readonly store = inject(ServiceMonitorStore);

  readonly running = input(false);
  readonly activeStage = input<AnatomyStage | null>(null);
  readonly caption = input('Ligue o Analisador para ver o fluxo.');
  readonly latest = input<RecentDocument | null>(null);
  readonly packets = input<FlyingPacket[]>([]);
  /** true enquanto fila/temp estão diminuindo (processamento real). */
  readonly consuming = input(false);

  /**
   * Esteira anda com NSU em trânsito OU enquanto as filas estão caindo.
   */
  readonly beltMoving = computed(() => {
    if (!this.running()) return false;
    if (this.consuming()) return true;
    if (this.packets().length > 0) return true;
    const stage = this.activeStage();
    return (
      stage === 'fila' ||
      stage === 'temp' ||
      stage === 'classificar' ||
      stage === 'detalhar' ||
      stage === 'limpar'
    );
  });

  /** Poucos chips — quantidade do último lote (máx. 8). */
  readonly beltItems = computed(() => {
    const q = this.latest()?.qtdDocumento ?? 4;
    const n = Math.min(8, Math.max(3, Math.min(q, 8)));
    return Array.from({ length: n }, (_, i) => i + 1);
  });

  readonly steps: {
    id: AnatomyStage;
    title: string;
    tag: string;
    blurb: string;
    techHint: string;
  }[] = [
    {
      id: 'fila',
      title: 'Fila',
      tag: 'Entrada',
      blurb: 'RECEIVE na fila do analisador.',
      techHint: 'fila_alvo_cte_analisador · RECEIVE / chave retirada',
    },
    {
      id: 'temp',
      title: 'Temporária',
      tag: 'Leitura',
      blurb: 'Obtém o lote em tmp_analise.',
      techHint: 'tmp_analise_* · lote obtido no banco',
    },
    {
      id: 'classificar',
      title: 'Classificar',
      tag: 'Schema',
      blurb: 'Roteia por schema (aut / evento / inut).',
      techHint: 'NegCTeSintetico.SintetizarLote · schema routing',
    },
    {
      id: 'detalhar',
      title: 'Detalhar',
      tag: 'INSERT',
      blurb: 'Grava documento_* sintético.',
      techHint: 'INSERT documento_* (+ NSU faltante)',
    },
    {
      id: 'limpar',
      title: 'Limpar',
      tag: 'Saída',
      blurb: 'DELETE temp ou AtualizarErro.',
      techHint: 'ExcluirLote / AtualizarErro · documento excluído',
    },
  ];

  private readonly order: AnatomyStage[] = [
    'fila',
    'temp',
    'classificar',
    'detalhar',
    'limpar',
  ];

  private clock?: ReturnType<typeof setInterval>;
  private readonly nowMs = signal(Date.now());

  constructor() {
    // Mantém OnPush fresco para counts de fila/tmp via store pushes; tick leve.
    this.clock = setInterval(() => this.nowMs.set(Date.now()), 1000);
  }

  ngOnDestroy(): void {
    if (this.clock) clearInterval(this.clock);
  }

  lanePercent(lane: number): number {
    return 10 + lane * 20;
  }

  count(id: AnatomyStage): string {
    this.nowMs();
    switch (id) {
      case 'fila': {
        const n = this.store.queues()?.serviceBrokerDepth ?? 0;
        return n > 0 ? `${n} na fila` : '0 na fila';
      }
      case 'temp': {
        const n = this.store.queues()?.tempBacklog ?? 0;
        return n > 0 ? `${n} aguardando` : '0 aguardando';
      }
      case 'classificar':
        return 'schema';
      case 'detalhar': {
        const q = this.latest()?.qtdDocumento;
        return q != null && q > 0 ? `${q} doc(s)` : 'documento_*';
      }
      case 'limpar':
        return 'DELETE / erro';
    }
  }

  isDone(id: AnatomyStage): boolean {
    const active = this.activeStage();
    if (!active) return false;
    return this.order.indexOf(id) < this.order.indexOf(active);
  }

  arrowHot(afterIndex: number): boolean {
    const active = this.activeStage();
    if (!active) return false;
    return this.order.indexOf(active) === afterIndex + 1;
  }
}
