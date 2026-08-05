import {
  ChangeDetectionStrategy,
  Component,
  computed,
  inject,
} from '@angular/core';
import { DatePipe } from '@angular/common';
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
      class="anatomy-poster anatomy-poster-fill flex h-full min-h-0 flex-col overflow-hidden rounded-xl border border-indigo-900/80 shadow-md"
      [class.anatomy-poster-live]="anyRunning() || hasQueueBusy()"
      [class.anatomy-poster-busy]="hasAgora() || hasQueueBusy()"
    >
      <div class="anatomy-poster-head flex shrink-0 flex-wrap items-center justify-between gap-2 px-4 pt-3">
        <div class="min-w-0">
          <p class="text-[10px] font-semibold uppercase tracking-[0.16em] text-violet-300/90">
            Em tempo real
          </p>
          <h2
            class="text-base font-semibold leading-tight text-indigo-50"
            title="Cadeia CT-e — Serviço Receptor → Arquivador → Sintetizador → Analisador → Integrador → Carga"
          >
            Fluxo da cadeia CT-e
          </h2>
          <p class="text-[11px] text-indigo-300/80">
            Clique no serviço para abrir o monitor (API + Angular prontos)
          </p>
        </div>
        <div class="flex flex-wrap items-center gap-2">
          @if (lastLote(); as lote) {
            <div class="rounded-lg border border-indigo-800 bg-indigo-950/70 px-3 py-1.5 shadow-sm">
              <p class="text-[9px] font-semibold uppercase tracking-wide text-violet-300">
                Último lote
              </p>
              <p class="font-mono text-xs text-indigo-100">
                NSU {{ lote.nsu ?? '—' }}
                @if (lote.nsuFinal != null) {
                  → {{ lote.nsuFinal }}
                }
                <span class="text-indigo-400">
                  · {{ lote.qtdDocumento ?? 0 }} CT-e
                </span>
                @if (lote.at) {
                  <span class="text-indigo-400"> · {{ lote.at | date: 'HH:mm:ss' }}</span>
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
        <p
          class="anatomy-belt-idle-hint mx-4 mt-2 shrink-0 rounded-md border border-indigo-800/50 bg-indigo-950/60 px-3 py-1.5 text-center text-[11px] font-medium text-indigo-300"
          title="Aguardando atividade na cadeia"
        >
          Sem CT-e em trânsito — aguardando atividade na cadeia
        </p>
      }

      <div class="relative mx-3 mt-2 min-h-0 min-w-0 flex-1 overflow-x-auto overflow-y-hidden">
        <div class="anatomy-fill-board relative flex h-full min-w-[980px] flex-col items-stretch px-2 py-2">
          <div class="anatomy-cycle-block relative z-[1] mx-auto w-full">
            <div class="anatomy-path-line anatomy-path-line-6" aria-hidden="true"></div>

            <div class="anatomy-stages anatomy-stages-6 relative z-[1] shrink-0 py-1">
              @for (sys of systems(); track sys.id; let i = $index) {
                <button
                  type="button"
                  class="anatomy-stage"
                  [class.anatomy-stage-active]="sys.agora"
                  [class.anatomy-stage-queued]="!!sys.hasQueueWork && !sys.agora"
                  [class.anatomy-stage-running]="isRunning(sys.status) && !sys.agora && !sys.hasQueueWork"
                  [class.anatomy-stage-muted]="isVisuallyMuted(sys)"
                  [class.anatomy-stage-link]="!!sys.frontendUrl"
                  [disabled]="!sys.frontendUrl || !!store.openingSystemId()"
                  [attr.title]="stageTitle(sys)"
                  (click)="openSystem(sys)"
                >
                  @if (sys.agora) {
                    <span class="anatomy-now">AGORA</span>
                  } @else if (sys.hasQueueWork) {
                    <span class="anatomy-now anatomy-now-queue">NA FILA</span>
                  }
                  <div
                    class="anatomy-platform"
                    [class.anatomy-platform-active]="sys.agora"
                    [class.anatomy-platform-queued]="!!sys.hasQueueWork && !sys.agora"
                    [class.anatomy-platform-running]="
                      isRunning(sys.status) && !sys.agora && !sys.hasQueueWork
                    "
                    [class.anatomy-platform-muted]="isVisuallyMuted(sys)"
                  >
                    <div
                      class="anatomy-iso anatomy-iso-symbol"
                      [attr.data-symbol]="sys.symbol"
                    ></div>
                  </div>
                  <p class="anatomy-stage-title">{{ sys.label }}</p>
                  <p class="anatomy-stage-tag">{{ statusLabel(sys.status) }}</p>
                  <p
                    class="anatomy-stage-count"
                    [class.anatomy-stage-count-hot]="!!sys.hasQueueWork || sys.agora"
                  >
                    {{ sys.metricPill }}
                  </p>
                  @if (sys.processHint) {
                    <p class="anatomy-stage-process" [title]="sys.processHint">
                      {{ sys.processHint }}
                    </p>
                  }
                  <p class="anatomy-stage-blurb">{{ sys.hint }}</p>
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
                <span class="anatomy-summary-label">Fase</span>
                <span class="anatomy-summary-value">{{ phaseLabel() }}</span>
              </div>
              <div class="min-w-0 flex-1">
                <span class="anatomy-summary-label">Cascata</span>
                <span class="anatomy-summary-value truncate">
                  {{ cascadeMessage() || '—' }}
                </span>
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

  readonly systems = this.store.systems;
  readonly lastLote = this.store.lastLote;
  readonly beltMoving = this.store.beltMoving;
  readonly runningCount = this.store.runningCount;
  readonly anyRunning = this.store.anyRunning;
  readonly cascadeMessage = this.store.cascadeMessage;

  readonly hasAgora = computed(() => this.systems().some((s) => s.agora));
  readonly hasQueueBusy = computed(() =>
    this.systems().some((s) => s.agora || !!s.hasQueueWork)
  );

  readonly phaseLabel = computed(() => {
    const p = normalizePhase(this.store.cascadePhase());
    switch (p) {
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

  isRunning(status: string | number): boolean {
    return normalizeStatus(status) === 'running';
  }

  isMuted(status: string | number): boolean {
    const s = normalizeStatus(status);
    return s === 'disabled' || s === 'stopped' || s === 'offline' || s === 'unknown';
  }

  /** Com fila ativa na cadeia, esmaece quem não tem trabalho. */
  isVisuallyMuted(sys: {
    status: string | number;
    agora: boolean;
    hasQueueWork?: boolean;
  }): boolean {
    if (sys.agora || sys.hasQueueWork) {
      return false;
    }
    if (this.hasQueueBusy()) {
      return true;
    }
    return this.isMuted(sys.status);
  }

  arrowHot(
    sys: { agora: boolean; hasQueueWork?: boolean },
    index: number
  ): boolean {
    const next = this.systems()[index + 1];
    const left = sys.agora || !!sys.hasQueueWork;
    const right = !!next && (next.agora || !!next.hasQueueWork);
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
  }): string {
    if (this.store.openingSystemId() === sys.id) {
      return 'Preparando API + Angular…';
    }
    if (sys.processHint) {
      return sys.processHint;
    }
    if (sys.agora) {
      return `${sys.label}: processando agora`;
    }
    if (sys.hasQueueWork) {
      return `${sys.label}: há itens na fila`;
    }
    if (sys.frontendUrl) {
      return `Abrir ${sys.label}`;
    }
    return sys.lastError || sys.hint;
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

  openSystem(sys: {
    id: string;
    frontendUrl?: string | null;
    label: string;
  }): void {
    if (!sys.frontendUrl?.trim()) {
      return;
    }
    // Abrir a aba no gesto do clique — depois do await o browser bloqueia window.open.
    const pending = window.open('about:blank', '_blank');
    if (!pending) {
      this.store.actionMessage.set(
        `Popup bloqueado — permita popups para este site ou abra: ${sys.frontendUrl}`
      );
      return;
    }
    pending.opener = null;
    void this.store.openSystemUi(sys.id, pending, sys.frontendUrl.trim());
  }
}
