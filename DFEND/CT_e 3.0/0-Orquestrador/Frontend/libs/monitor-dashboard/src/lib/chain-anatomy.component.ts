import {
  ChangeDetectionStrategy,
  Component,
  computed,
  inject,
} from '@angular/core';
import { DatePipe } from '@angular/common';
import { Router } from '@angular/router';
import {
  ChainOrchestratorStore,
  normalizePhase,
  normalizeStatus,
} from '@orquestrador/monitor-core';
import { StatusLegendComponent } from './status-legend.component';
import {
  StationCardComponent,
  type StationBadge,
} from './station-card.component';
import { ChainQueueProofChipComponent } from './chain-queue-proof-chip.component';
import { PresentationTourStore } from './presentation-tour.store';

@Component({
  selector: 'lib-chain-anatomy',
  standalone: true,
  imports: [
    DatePipe,
    StatusLegendComponent,
    StationCardComponent,
    ChainQueueProofChipComponent,
  ],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <div
      class="chain-anatomy anatomy-poster anatomy-poster-fill flex h-full min-h-0 flex-col overflow-hidden rounded-xl border border-indigo-900/80 shadow-md"
      [class.anatomy-poster-live]="anyRunning()"
      [class.anatomy-poster-busy]="hasAgora() || hasQueueBusy()"
      [class.anatomy-poster-starting]="isStarting()"
      [class.anatomy-poster-idle]="isIdlePoster()"
      data-tour="stations"
    >
      <div class="anatomy-poster-head flex shrink-0 flex-wrap items-center justify-between gap-2 px-4 pt-3">
        <div class="min-w-0" data-tour="legend">
          <p class="text-[10px] font-semibold uppercase tracking-[0.16em] text-sky-300/90">
            Cadeia de serviços CT-e
          </p>
          <h2
            class="text-base font-semibold leading-tight text-slate-50"
            title="Cadeia CT-e — Receptor → Arquivador → Sintetizador → Analisador → Integrador → Carga"
          >
            Fluxo da cadeia CT-e
          </h2>
          <p class="text-[11px] text-slate-400">
            Clique no serviço para abrir o monitor
          </p>
          @if (tour.isSimulating()) {
            <p class="mt-1 text-[11px] font-semibold text-amber-300">
              Apresentação · dados simulados
            </p>
          }
          <lib-status-legend />
        </div>
        <div class="flex flex-wrap items-center gap-2">
          @if (lastLote(); as lote) {
            <div class="rounded-lg border border-indigo-800 bg-indigo-950/70 px-3 py-1.5 shadow-sm">
              <p class="text-[9px] font-semibold uppercase tracking-wide text-sky-300">
                Último lote
              </p>
              <p class="font-mono text-xs text-slate-100">
                NSU {{ lote.nsu ?? '—' }}
                @if (lote.nsuFinal != null) {
                  → {{ lote.nsuFinal }}
                }
                <span class="text-slate-400">
                  · {{ lote.qtdDocumento ?? 0 }} CT-e
                </span>
                @if (lote.at) {
                  <span class="text-slate-400"> · {{ lote.at | date: 'HH:mm:ss' }}</span>
                }
              </p>
            </div>
          }
        </div>
      </div>

      @if (busySystems().length > 0) {
        <div
          class="anatomy-live-rail anatomy-live-rail-sticky mx-4 mt-2 shrink-0"
          data-tour="foco"
        >
          <p class="anatomy-live-rail-title">Foco agora</p>
          @if (isStoppedWithBacklog()) {
            <p class="mt-0.5 text-[11px] leading-snug text-amber-200/90">
              Cadeia parada — a fila não foi apagada. Use
              <span class="font-medium text-lime-300">Ligar as filas</span>
              para retomar o consumo.
            </p>
          }
          <div class="anatomy-live-rail-cards">
            @for (sys of busySystems(); track sys.id) {
              <button
                type="button"
                class="anatomy-live-card"
                [class.anatomy-live-card-agora]="sys.agora"
                [class.anatomy-live-card-queue]="!!sys.hasQueueWork && !sys.agora"
                [attr.title]="'Abrir monitor ' + shortLabel(sys.label)"
                (click)="openUnified(sys)"
              >
                <span class="anatomy-live-symbol">{{ sys.symbol }}</span>
                <div class="min-w-0 flex-1">
                  <p class="anatomy-live-name">{{ shortLabel(sys.label) }}</p>
                  <p class="anatomy-live-metric">{{ sys.metricPill }}</p>
                  <p class="anatomy-live-process">
                    {{
                      sys.processHint ||
                        (sys.agora
                          ? 'Executando · drenando fila'
                          : isStoppedWithBacklog()
                            ? 'Na fila · aguardando Ligar as filas'
                            : anyRunning()
                              ? 'Backlog com cadeia ativa · aguardando consumo'
                              : 'Arquivos na fila aguardando consumo')
                    }}
                  </p>
                </div>
                <span class="anatomy-live-badge">
                  {{ sys.agora ? 'AGORA' : 'FILA' }}
                </span>
              </button>
            }
          </div>
        </div>
      }

      @if (errorSystems().length > 0) {
        <div class="anatomy-error-rail mx-4 mt-2 shrink-0" role="alert">
          <p class="anatomy-error-rail-title">Serviços com problema</p>
          @for (sys of errorSystems(); track sys.id) {
            <button
              type="button"
              class="anatomy-error-row"
              (click)="openUnified(sys)"
            >
              <span class="anatomy-error-symbol">{{ sys.symbol }}</span>
              <div class="min-w-0 flex-1 text-left">
                <p class="anatomy-error-name">{{ shortLabel(sys.label) }}</p>
                <p class="anatomy-error-detail">
                  {{ sys.lastError || sys.hint || 'Falha reportada pelo monitor.' }}
                </p>
              </div>
            </button>
          }
        </div>
      }

      @if (beltMoving()) {
        <div
          class="anatomy-belt mx-4 mt-2 shrink-0 overflow-hidden rounded-md border border-indigo-800/60 bg-indigo-950/50"
          title="CT-e em trânsito na cadeia"
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
      }

      <div
        class="relative mx-3 mt-2 min-h-0 min-w-0 flex-1 overflow-x-auto overflow-y-hidden"
        [class.chain-board-idle]="isIdlePoster()"
      >
        <div
          class="anatomy-fill-board relative flex h-full min-w-[980px] flex-col items-stretch px-2 py-2"
          [class.justify-center]="isIdlePoster()"
        >
          @if (isIdlePoster()) {
            <div class="chain-idle-hero mx-auto mb-4 max-w-md shrink-0 text-center">
              <div class="chain-idle-hero-icon" aria-hidden="true">
                <span class="anatomy-cte-doc">CT-e</span>
                <span class="anatomy-cte-doc">CT-e</span>
                <span class="anatomy-cte-doc">CT-e</span>
              </div>
              <p class="mt-3 text-sm font-semibold text-slate-100">
                Nenhuma fila ligada
              </p>
              <p class="mt-1 text-[12px] leading-relaxed text-slate-400">
                Use <span class="font-medium text-lime-300">Ligar as filas</span> no
                topo para iniciar a cadeia e ver AGORA, profundidade e documentos em
                trânsito.
              </p>
            </div>
          }

          <div class="anatomy-cycle-block relative z-[1] mx-auto w-full">
            <div
              class="anatomy-path-line anatomy-path-line-6"
              [class.anatomy-path-line-active]="anyRunning() || hasQueueBusy() || isStarting()"
              aria-hidden="true"
            ></div>

            <div class="anatomy-stages anatomy-stages-6 relative z-[1] shrink-0 py-1">
              @for (sys of systems(); track sys.id; let i = $index) {
                <lib-station-card
                  [symbol]="sys.symbol"
                  [label]="shortLabel(sys.label)"
                  [metric]="sys.metricPill"
                  [depth]="queueDepthOf(sys)"
                  [badge]="stationBadge(sys)"
                  [processHint]="
                    sys.agora || sys.hasQueueWork ? sys.processHint ?? null : null
                  "
                  [muted]="isVisuallyMuted(sys)"
                  [booting]="isStarting()"
                  [bootDelay]="i * 0.12 + 's'"
                  [titleAttr]="stageTitle(sys)"
                  (opened)="openUnified(sys)"
                />
                @if (i < systems().length - 1) {
                  <div
                    class="anatomy-step-arrow"
                    [class.anatomy-step-arrow-hot]="arrowHot(sys, i)"
                    aria-hidden="true"
                  >
                    →
                  </div>
                }
              }
            </div>

            <div class="anatomy-summary-bar anatomy-summary-bar-queue mt-3 shrink-0">
              @if (anyRunning() || hasQueueBusy()) {
                <div>
                  <span class="anatomy-summary-label">Com fila</span>
                  <span
                    class="anatomy-summary-value"
                    [class.anatomy-summary-value-live]="anyRunning()"
                    [class.anatomy-summary-value-warn]="!anyRunning()"
                  >{{ queueBusyCount() }}</span>
                </div>
                <div>
                  <span class="anatomy-summary-label">Arquivos na cadeia</span>
                  <span
                    class="anatomy-summary-value"
                    [class.anatomy-summary-value-live]="anyRunning()"
                    [class.anatomy-summary-value-warn]="!anyRunning()"
                  >{{ totalQueueFiles() }}</span>
                </div>
                <div>
                  <span class="anatomy-summary-label">Fase</span>
                  <span
                    class="anatomy-summary-value"
                    [class.anatomy-summary-value-live]="phaseLabel() === 'Em execução'"
                    [class.anatomy-summary-value-wait]="
                      phaseLabel() === 'Ligando' || phaseLabel() === 'Desligando'
                    "
                  >{{ phaseLabel() }}</span>
                </div>
                @if (anyRunning() && totalQueueFiles() === 0) {
                  <div title="Cadeia ligada; nenhum CT-e aguardando no momento">
                    <span class="anatomy-summary-label">Fluxo</span>
                    <span class="anatomy-summary-value anatomy-summary-value-live"
                      >ativo · sem fila</span
                    >
                  </div>
                }
              }
              <lib-chain-queue-proof-chip data-tour="validate" />
            </div>
          </div>
        </div>
      </div>
    </div>
  `,
})
export class ChainAnatomyComponent {
  readonly store = inject(ChainOrchestratorStore);
  readonly tour = inject(PresentationTourStore);
  private readonly router = inject(Router);

  readonly systems = computed(
    () => this.tour.simulation()?.systems ?? this.store.systems()
  );
  readonly lastLote = computed(() => {
    const sim = this.tour.simulation();
    if (sim) {
      return {
        nsu: 900001,
        nsuFinal: 900008,
        qtdDocumento: sim.lastLoteQtd,
        at: new Date().toISOString(),
      };
    }
    return this.store.lastLote();
  });
  readonly beltMoving = computed(
    () => this.tour.simulation()?.beltMoving ?? this.store.beltMoving()
  );
  readonly anyRunning = computed(() => {
    const mode = this.tour.simulation()?.mode;
    if (mode === 'flow') return true;
    if (mode === 'stoppedBacklog') return false;
    return this.store.anyRunning();
  });

  readonly hasAgora = computed(() => this.systems().some((s) => s.agora));
  readonly hasQueueBusy = computed(() =>
    this.systems().some((s) => s.agora || !!s.hasQueueWork)
  );

  readonly isStarting = computed(() => {
    if (this.tour.simulation()) return false;
    return normalizePhase(this.store.cascadePhase()) === 'starting';
  });

  readonly isIdlePoster = computed(() => {
    if (this.tour.simulation()) return false;
    const phase = normalizePhase(this.store.cascadePhase());
    return (
      phase === 'idle' &&
      !this.anyRunning() &&
      !this.hasQueueBusy() &&
      this.errorSystems().length === 0
    );
  });

  /** Parada com backlog: Desligar não limpa fila — UI explica o estado. */
  readonly isStoppedWithBacklog = computed(() => {
    if (this.tour.simulation()?.mode === 'stoppedBacklog') return true;
    const phase = normalizePhase(this.store.cascadePhase());
    return (
      phase === 'idle' &&
      !this.anyRunning() &&
      this.store.processUpCount() === 0 &&
      this.hasQueueBusy()
    );
  });

  readonly busySystems = computed(() =>
    this.systems().filter(
      (s) => (s.agora || !!s.hasQueueWork) && !this.isError(s)
    )
  );

  readonly errorSystems = computed(() => this.systems().filter((s) => this.isError(s)));

  readonly queueBusyCount = computed(
    () => this.systems().filter((s) => s.agora || !!s.hasQueueWork).length
  );

  readonly totalQueueFiles = computed(() =>
    this.systems().reduce((sum, s) => sum + (Number(s.queueDepth) || 0), 0)
  );

  readonly phaseLabel = computed(() => {
    const simPhase = this.tour.simulation()?.cascadePhase;
    const phase = normalizePhase(simPhase ?? this.store.cascadePhase());
    switch (phase) {
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

  readonly beltItems = computed(() => {
    const q = this.lastLote()?.qtdDocumento ?? 4;
    const n = Math.min(8, Math.max(3, Math.min(q, 8)));
    return Array.from({ length: n }, (_, i) => i + 1);
  });

  shortLabel(label: string): string {
    return label.replace(/^Serviço\s+/i, '').trim() || label;
  }

  queueDepthOf(sys: { queueDepth?: number }): number {
    return Number(sys.queueDepth) || 0;
  }

  stationBadge(sys: {
    status: string | number;
    agora: boolean;
    hasQueueWork?: boolean;
    lastError?: string | null;
    executar?: number | null;
  }): StationBadge {
    if (this.isError(sys)) return 'erro';
    if (sys.agora) return 'agora';
    if (sys.hasQueueWork) return 'fila';
    if (this.isStartingStatus(sys.status)) return 'ligando';
    if (this.isStoppingStatus(sys.status)) return 'desligando';
    if (this.isWorkActive(sys)) return 'ativo';
    return 'parado';
  }

  isRunning(status: string | number): boolean {
    return normalizeStatus(status) === 'running';
  }

  isProcessUp(sys: { status: string | number }): boolean {
    return this.isRunning(sys.status);
  }

  isWorkActive(sys: { status: string | number; executar?: number | null }): boolean {
    if (!this.isProcessUp(sys)) return false;
    if (sys.executar == null) return true;
    return Number(sys.executar) === 1;
  }

  isStartingStatus(status: string | number): boolean {
    return normalizeStatus(status) === 'starting';
  }

  isStoppingStatus(status: string | number): boolean {
    return normalizeStatus(status) === 'stopping';
  }

  isMuted(status: string | number): boolean {
    const s = normalizeStatus(status);
    return s === 'disabled' || s === 'stopped' || s === 'offline' || s === 'unknown';
  }

  isError(sys: {
    status: string | number;
    lastError?: string | null;
  }): boolean {
    const s = normalizeStatus(sys.status);
    if (s === 'running') {
      return false;
    }
    return s === 'failed' || s === 'offline' || !!sys.lastError?.trim();
  }

  isVisuallyMuted(sys: {
    status: string | number;
    agora: boolean;
    hasQueueWork?: boolean;
    lastError?: string | null;
    executar?: number | null;
  }): boolean {
    if (sys.agora || sys.hasQueueWork || this.isError(sys)) {
      return false;
    }
    if (this.isStarting()) {
      return false;
    }
    // Ativo sem fluxo na fila deve continuar verde/legível — não esmaecer
    // só porque outro serviço está drenando backlog.
    if (this.isWorkActive(sys)) {
      return false;
    }
    if (this.hasQueueBusy() || this.errorSystems().length > 0) {
      return true;
    }
    return this.isMuted(sys.status);
  }

  arrowHot(
    sys: {
      agora: boolean;
      hasQueueWork?: boolean;
      lastError?: string | null;
      status: string | number;
    },
    index: number
  ): boolean {
    if (this.isStarting()) return true;
    const next = this.systems()[index + 1];
    const left = sys.agora || !!sys.hasQueueWork || this.isError(sys);
    const right =
      !!next && (next.agora || !!next.hasQueueWork || this.isError(next));
    return left || right;
  }

  stageTitle(sys: {
    id: string;
    label: string;
    lastError?: string | null;
    hint: string;
    processHint?: string | null;
    hasQueueWork?: boolean;
    agora: boolean;
    status: string | number;
    executar?: number | null;
  }): string {
    if (this.store.openingSystemId() === sys.id) {
      return 'Preparando API + Angular…';
    }
    const parts = [this.shortLabel(sys.label)];
    if (this.isError(sys)) {
      parts.push(sys.lastError || 'erro');
    } else if (sys.processHint) {
      parts.push(sys.processHint);
    } else if (sys.agora) {
      parts.push('processando agora');
    } else if (sys.hasQueueWork) {
      parts.push(
        this.anyRunning()
          ? 'há itens na fila (cadeia ativa)'
          : 'há itens na fila (cadeia parada)'
      );
    } else if (this.isWorkActive(sys)) {
      parts.push('ativo · sem fluxo na fila');
    } else if (sys.hint) {
      parts.push(sys.hint);
    }
    parts.push('Clique para abrir o monitor');
    return parts.join(' — ');
  }

  openUnified(sys: { id: string }): void {
    void this.router.navigate(['/monitores', sys.id]);
  }
}
