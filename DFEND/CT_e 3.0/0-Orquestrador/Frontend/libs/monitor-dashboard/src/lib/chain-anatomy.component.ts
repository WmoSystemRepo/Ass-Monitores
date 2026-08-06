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

@Component({
  selector: 'lib-chain-anatomy',
  standalone: true,
  imports: [DatePipe],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <div
      class="chain-anatomy anatomy-poster anatomy-poster-fill flex h-full min-h-0 flex-col overflow-hidden rounded-xl border border-indigo-900/80 shadow-md"
      [class.anatomy-poster-live]="anyRunning() || hasQueueBusy()"
      [class.anatomy-poster-busy]="hasAgora() || hasQueueBusy()"
      [class.anatomy-poster-starting]="isStarting()"
      [class.anatomy-poster-idle]="isIdlePoster()"
    >
      <div class="anatomy-poster-head flex shrink-0 flex-wrap items-center justify-between gap-2 px-4 pt-3">
        <div class="min-w-0">
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
          <ul class="anatomy-legend" aria-label="Legenda de status">
            <li class="anatomy-legend-item">
              <span class="anatomy-legend-swatch anatomy-legend-error"></span>
              Erro
            </li>
            <li class="anatomy-legend-item">
              <span class="anatomy-legend-swatch anatomy-legend-agora"></span>
              Agora
            </li>
            <li class="anatomy-legend-item">
              <span class="anatomy-legend-swatch anatomy-legend-queue"></span>
              Na fila
            </li>
            <li class="anatomy-legend-item">
              <span class="anatomy-legend-swatch anatomy-legend-running"></span>
              Ligado
            </li>
            <li class="anatomy-legend-item">
              <span class="anatomy-legend-swatch anatomy-legend-stopped"></span>
              Parado
            </li>
          </ul>
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
      } @else {
        <p class="anatomy-belt-idle-hint mx-4 mt-2 shrink-0 text-center text-[11px] text-slate-500">
          Sem CT-e em trânsito
        </p>
      }

      @if (busySystems().length > 0) {
        <div class="anatomy-live-rail mx-4 mt-2 shrink-0">
          <p class="anatomy-live-rail-title">Em processamento agora</p>
          <div class="anatomy-live-rail-cards">
            @for (sys of busySystems(); track sys.id) {
              <div
                class="anatomy-live-card"
                [class.anatomy-live-card-agora]="sys.agora"
                [class.anatomy-live-card-queue]="!!sys.hasQueueWork && !sys.agora"
                [class.anatomy-live-card-error]="isError(sys)"
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
                          : 'Arquivos na fila aguardando consumo')
                    }}
                  </p>
                </div>
                <span class="anatomy-live-badge">
                  {{ sys.agora ? 'AGORA' : isError(sys) ? 'ERRO' : 'FILA' }}
                </span>
              </div>
            }
          </div>
        </div>
      }

      @if (errorSystems().length > 0) {
        <div class="anatomy-error-rail mx-4 mt-2 shrink-0" role="alert">
          <p class="anatomy-error-rail-title">Serviços com problema</p>
          @for (sys of errorSystems(); track sys.id) {
            <div class="anatomy-error-row">
              <span class="anatomy-error-symbol">{{ sys.symbol }}</span>
              <div class="min-w-0 flex-1">
                <p class="anatomy-error-name">{{ shortLabel(sys.label) }}</p>
                <p class="anatomy-error-detail">
                  {{ sys.lastError || sys.hint || 'Falha reportada pelo monitor.' }}
                </p>
              </div>
            </div>
          }
        </div>
      }

      <div class="relative mx-3 mt-2 min-h-0 min-w-0 flex-1 overflow-x-auto overflow-y-hidden">
        <div class="anatomy-fill-board relative flex h-full min-w-[980px] flex-col items-stretch px-2 py-2">
          <div class="anatomy-cycle-block relative z-[1] mx-auto w-full">
            <div
              class="anatomy-path-line anatomy-path-line-6"
              [class.anatomy-path-line-active]="anyRunning() || hasQueueBusy() || isStarting()"
              aria-hidden="true"
            ></div>

            <div class="anatomy-stages anatomy-stages-6 relative z-[1] shrink-0 py-1">
              @for (sys of systems(); track sys.id; let i = $index) {
                <button
                  type="button"
                  class="anatomy-stage anatomy-stage-link"
                  [class.anatomy-stage-active]="sys.agora"
                  [class.anatomy-stage-queued]="!!sys.hasQueueWork && !sys.agora && !isError(sys)"
                  [class.anatomy-stage-error]="isError(sys)"
                  [class.anatomy-stage-running]="
                    isRunning(sys.status) && !sys.agora && !sys.hasQueueWork && !isError(sys)
                  "
                  [class.anatomy-stage-muted]="isVisuallyMuted(sys)"
                  [class.anatomy-stage-booting]="isStarting()"
                  [style.--boot-delay]="i * 0.12 + 's'"
                  [attr.title]="stageTitle(sys)"
                  (click)="openUnified(sys)"
                >
                  @if (isError(sys)) {
                    <span class="anatomy-now anatomy-now-error">ERRO</span>
                  } @else if (sys.agora) {
                    <span class="anatomy-now">AGORA</span>
                  } @else if (sys.hasQueueWork) {
                    <span class="anatomy-now anatomy-now-queue">NA FILA</span>
                  }
                  <div
                    class="anatomy-platform"
                    [class.anatomy-platform-active]="sys.agora && !isError(sys)"
                    [class.anatomy-platform-queued]="
                      !!sys.hasQueueWork && !sys.agora && !isError(sys)
                    "
                    [class.anatomy-platform-error]="isError(sys)"
                    [class.anatomy-platform-running]="
                      isRunning(sys.status) &&
                      !sys.agora &&
                      !sys.hasQueueWork &&
                      !isError(sys)
                    "
                    [class.anatomy-platform-muted]="isVisuallyMuted(sys)"
                  >
                    <div
                      class="anatomy-iso anatomy-iso-symbol"
                      [attr.data-symbol]="sys.symbol"
                    ></div>
                  </div>
                  <p class="anatomy-stage-title">{{ shortLabel(sys.label) }}</p>
                  <span
                    class="anatomy-status-chip"
                    [class.anatomy-status-chip-running]="
                      isRunning(sys.status) && !isError(sys)
                    "
                    [class.anatomy-status-chip-error]="isError(sys)"
                    [class.anatomy-status-chip-wait]="
                      isStartingStatus(sys.status) || isStoppingStatus(sys.status)
                    "
                    [class.anatomy-status-chip-stopped]="
                      !isRunning(sys.status) &&
                      !isError(sys) &&
                      !isStartingStatus(sys.status) &&
                      !isStoppingStatus(sys.status)
                    "
                  >
                    {{ statusLabel(sys.status) }}
                  </span>
                  <p
                    class="anatomy-stage-count"
                    [class.anatomy-stage-count-hot]="!!sys.hasQueueWork || sys.agora"
                    [class.anatomy-stage-count-error]="isError(sys)"
                    [attr.title]="sys.hint || sys.processHint || ''"
                  >
                    {{ queueLabel(sys) }}
                  </p>
                  @if (sys.processHint && (sys.agora || sys.hasQueueWork)) {
                    <p class="anatomy-stage-process" [title]="sys.processHint">
                      {{ sys.processHint }}
                    </p>
                  }
                </button>
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

            <div class="anatomy-summary-bar mt-3 shrink-0">
              <div>
                <span class="anatomy-summary-label">Sistemas ligados</span>
                <span class="anatomy-summary-value">{{ runningCount() }}</span>
              </div>
              <div>
                <span class="anatomy-summary-label">Com fila</span>
                <span class="anatomy-summary-value">{{ queueBusyCount() }}</span>
              </div>
              <div>
                <span class="anatomy-summary-label">Arquivos na cadeia</span>
                <span class="anatomy-summary-value">{{ totalQueueFiles() }}</span>
              </div>
              <div>
                <span class="anatomy-summary-label">Fase</span>
                <span class="anatomy-summary-value">{{ phaseLabel() }}</span>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  `,
})
export class ChainAnatomyComponent {
  readonly store = inject(ChainOrchestratorStore);
  private readonly router = inject(Router);

  readonly systems = this.store.systems;
  readonly lastLote = this.store.lastLote;
  readonly beltMoving = this.store.beltMoving;
  readonly runningCount = this.store.runningCount;
  readonly anyRunning = this.store.anyRunning;

  readonly hasAgora = computed(() => this.systems().some((s) => s.agora));
  readonly hasQueueBusy = computed(() =>
    this.systems().some((s) => s.agora || !!s.hasQueueWork)
  );

  readonly isStarting = computed(
    () => normalizePhase(this.store.cascadePhase()) === 'starting'
  );

  readonly isIdlePoster = computed(() => {
    const phase = normalizePhase(this.store.cascadePhase());
    return (
      phase === 'idle' &&
      !this.anyRunning() &&
      !this.hasQueueBusy() &&
      this.errorSystems().length === 0
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

  readonly beltItems = computed(() => {
    const q = this.lastLote()?.qtdDocumento ?? 4;
    const n = Math.min(8, Math.max(3, Math.min(q, 8)));
    return Array.from({ length: n }, (_, i) => i + 1);
  });

  shortLabel(label: string): string {
    return label.replace(/^Serviço\s+/i, '').trim() || label;
  }

  isRunning(status: string | number): boolean {
    return normalizeStatus(status) === 'running';
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
  }): boolean {
    if (sys.agora || sys.hasQueueWork || this.isError(sys)) {
      return false;
    }
    if (this.isStarting()) {
      return false;
    }
    if (this.hasQueueBusy() || this.errorSystems().length > 0) {
      return true;
    }
    return this.isMuted(sys.status);
  }

  queueLabel(sys: {
    metricPill: string;
    queueDepth?: number;
    hasQueueWork?: boolean;
  }): string {
    const depth = Number(sys.queueDepth) || 0;
    if (depth > 0) {
      return `${depth.toLocaleString('pt-BR')} arq. · ${sys.metricPill}`;
    }
    return sys.metricPill;
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
    frontendUrl?: string | null;
    lastError?: string | null;
    hint: string;
    processHint?: string | null;
    hasQueueWork?: boolean;
    agora: boolean;
    status: string | number;
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
      parts.push('há itens na fila');
    } else if (sys.hint) {
      parts.push(sys.hint);
    }
    parts.push('Clique para abrir o monitor');
    return parts.join(' — ');
  }

  statusLabel(status: string | number): string {
    switch (normalizeStatus(status)) {
      case 'running':
        return 'Ligado';
      case 'starting':
        return 'Ligando…';
      case 'stopping':
        return 'Desligando…';
      case 'failed':
        return 'Falha';
      case 'disabled':
        return 'Desabilitado';
      case 'offline':
        return 'Offline';
      case 'stopped':
        return 'Parado';
      case 'unknown':
        return 'Desconhecido';
      default:
        return 'Parado';
    }
  }

  openUnified(sys: { id: string }): void {
    void this.router.navigate(['/monitores', sys.id]);
  }
}
