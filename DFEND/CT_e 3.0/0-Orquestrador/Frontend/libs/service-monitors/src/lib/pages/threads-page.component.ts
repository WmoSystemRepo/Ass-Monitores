import {
  ChangeDetectionStrategy,
  Component,
  computed,
  effect,
  inject,
  signal,
} from '@angular/core';
import { NgClass } from '@angular/common';
import { RouterLink } from '@angular/router';
import { ServiceMonitorStore } from '../service-monitor.store';
import { LogEntry, ThreadView } from '@orquestrador/shared-data';
import {
  awaitingReceptionStatus,
  computeNsuDelta,
  documentKindLabel,
  emptyEventsMeaning,
  formatAge,
  formatNsuDelta,
  nsuDeltaMeaning,
  nsuSourceFriendly,
  recentLogsForThread,
  resolveThreadRunStatus,
  summarizeLogMessage,
  summarizeThreadStatuses,
  threadCardTitle,
  threadMission,
  ThreadRunStatus,
  ThreadStatusResult,
} from '@orquestrador/shared-utils';

interface ThreadCardVm {
  thread: ThreadView;
  status: ThreadStatusResult;
  nsuDelta: number | null;
  nsuDeltaLabel: string;
  nsuDeltaMeaning: string;
  lastAge: string;
  recentLogs: LogEntry[];
  isHero: boolean;
  title: string;
  mission: string;
  sourceLabel: string;
  sourceTechnical: string;
  documentKind: string;
  emptyEvents: string;
  positionLabel: string;
}

@Component({
  selector: 'lib-threads-page',
  standalone: true,
  imports: [NgClass, RouterLink],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <section
      class="threads-fit flex h-[calc(100vh-3rem)] max-h-[calc(100vh-3rem)] flex-col gap-2 overflow-hidden"
    >
      <header
        class="flex shrink-0 flex-wrap items-end justify-between gap-2"
        data-tour="threads-header"
      >
        <div class="min-w-0">
          <p
            class="text-[10px] font-medium uppercase tracking-[0.14em]"
            [ngClass]="receptionRunning() ? 'text-cyan-400/90' : 'text-slate-400'"
          >
            {{ receptionRunning() ? 'Em tempo real' : 'Recepção parada' }}
          </p>
          <h1 class="text-xl font-semibold text-slate-50">Linhas de trabalho</h1>
          <p class="truncate text-xs text-slate-400">
            @if (receptionRunning()) {
              Até 5 linhas buscam na SEFAZ — cada card é uma linha (o que busca, se está ativa,
              posição).
            } @else {
              Números congelados — ligue no Monitor para ver as linhas buscando.
            }
          </p>
        </div>
        <div class="flex shrink-0 flex-wrap items-center gap-2">
          <a
            routerLink="../logs"
            class="rounded border border-slate-600 bg-slate-900/60 px-2.5 py-1.5 text-xs text-cyan-200 hover:border-cyan-500"
            data-tour="nav-historico"
          >
            Histórico →
          </a>
          @if (!receptionRunning()) {
            <a
              routerLink=".."
              class="rounded border border-cyan-700/60 bg-cyan-950/40 px-3 py-1.5 text-xs text-cyan-200 hover:border-cyan-500"
            >
              Abrir Monitor →
            </a>
          }
        </div>
      </header>

      @if (!receptionRunning()) {
        <div
          class="flex shrink-0 items-center justify-between gap-3 rounded border border-slate-600 bg-slate-900/70 px-3 py-2 text-xs text-slate-200"
          role="status"
          data-tour="threads-summary"
        >
          <p class="min-w-0 truncate">
            O Receptor não está recebendo. Cards mostram só configuração e última posição.
          </p>
          <span class="shrink-0 text-slate-400">Recepção: <span class="text-slate-100">parada</span></span>
        </div>
      } @else {
        <div
          class="flex shrink-0 flex-wrap items-center gap-x-4 gap-y-0.5 rounded border border-slate-700/80 bg-slate-950/60 px-3 py-1.5 text-xs text-slate-300"
          aria-label="Resumo das linhas"
          data-tour="threads-summary"
        >
          <span title="Há sinal recente de consulta ou avanço">
            <span class="font-semibold text-emerald-300">{{ summary().inCycle }}</span>
            buscando
          </span>
          <span title="Contador em zero — linha auxiliar desligada">
            <span class="font-semibold text-slate-200">{{ summary().idle }}</span>
            não buscam agora
          </span>
          <span title="Posição guardada em arquivo, não no banco">
            <span class="font-semibold text-sky-300">{{ summary().outsideDb }}</span>
            em arquivo local
          </span>
          <span title="Apta a buscar, sem registro recente no banco">
            <span class="font-semibold text-amber-200/90">{{ summary().noEvidence }}</span>
            sem atividade recente
          </span>
        </div>
      }

      <div
        class="flex min-h-0 flex-1 flex-col gap-2 overflow-hidden"
        data-tour="threads-cards"
      >
        <p class="shrink-0 text-[10px] font-semibold uppercase tracking-[0.14em] text-slate-500">
          Cartões das linhas
        </p>
        @for (card of cards(); track card.thread.threadId) {
          @if (card.isHero) {
            <article
              class="shrink-0 overflow-hidden rounded border px-3 py-2.5"
              [ngClass]="cardBorderClass(card.status.status, receptionRunning())"
            >
              <div class="flex items-start justify-between gap-2">
                <div class="min-w-0">
                  <div class="flex flex-wrap items-center gap-2">
                    <h2 class="text-base font-semibold text-slate-100">{{ card.title }}</h2>
                    <span
                      class="rounded px-1.5 py-0.5 text-[10px] font-medium"
                      [ngClass]="statusChipClass(card.status.status)"
                    >
                      {{ card.status.label }}
                    </span>
                  </div>
                  <p class="mt-0.5 line-clamp-1 text-xs text-slate-400">{{ card.mission }}</p>
                </div>
                @if (receptionRunning()) {
                  <a
                    class="shrink-0 text-[11px] text-cyan-400/90 hover:text-cyan-300"
                    [routerLink]="['../logs']"
                    [queryParams]="{ thread: card.thread.threadId }"
                    >Histórico →</a
                  >
                }
              </div>

              @if (receptionRunning() && card.status.meaning) {
                <p
                  class="mt-1.5 line-clamp-1 text-xs text-slate-300"
                  [title]="card.status.meaning"
                >
                  {{ card.status.meaning }}
                </p>
              }

              @if (receptionRunning()) {
                <div class="mt-2 grid grid-cols-2 gap-2 lg:grid-cols-4">
                  <div class="min-w-0 overflow-hidden">
                    <p class="text-[10px] uppercase tracking-wide text-slate-500">O que busca</p>
                    <p
                      class="truncate text-xs text-slate-100"
                      [title]="'indDFe=' + card.thread.indDFe"
                    >
                      {{ card.documentKind }}
                    </p>
                  </div>
                  <div class="min-w-0 overflow-hidden">
                    <p class="text-[10px] uppercase tracking-wide text-slate-500">
                      Posição da busca
                    </p>
                    <p class="truncate font-mono text-xs text-slate-100" [title]="card.nsuDeltaMeaning">
                      {{ card.positionLabel }}
                      <span [ngClass]="deltaClass(card.nsuDelta)">{{ card.nsuDeltaLabel }}</span>
                    </p>
                  </div>
                  <div class="min-w-0 overflow-hidden">
                    <p class="text-[10px] uppercase tracking-wide text-slate-500">Onde guarda</p>
                    <p class="truncate text-xs text-slate-100" [title]="card.sourceTechnical">
                      {{ card.sourceLabel }}
                    </p>
                  </div>
                  <div class="min-w-0 overflow-hidden">
                    <p class="text-[10px] uppercase tracking-wide text-slate-500">
                      Última atividade
                    </p>
                    <p class="truncate text-xs text-slate-100">
                      @if (card.thread.lastActivityAt) {
                        há {{ card.lastAge }}
                        @if (card.thread.lastCStat) {
                          <span class="text-violet-300/90">· {{ card.thread.lastCStat }}</span>
                        }
                      } @else {
                        Sem registro
                      }
                    </p>
                  </div>
                </div>
                <p class="mt-1.5 truncate text-[11px] text-slate-500">
                  @if (card.recentLogs.length) {
                    <span class="text-slate-400">{{ summarizeLog(card.recentLogs[0]) }}</span>
                  } @else {
                    {{ card.emptyEvents }}
                  }
                </p>
              } @else {
                <div class="mt-1.5 flex flex-wrap gap-x-6 gap-y-1 text-xs">
                  <p>
                    <span class="text-slate-500">Posição · </span>
                    <span class="font-mono text-slate-100" title="NSU">{{ card.positionLabel }}</span>
                  </p>
                  <p class="min-w-0 truncate" [title]="card.sourceTechnical">
                    <span class="text-slate-500">Guarda · </span>
                    <span class="text-slate-200">{{ card.sourceLabel }}</span>
                  </p>
                </div>
              }
            </article>
          }
        }

        <div
          class="grid min-h-0 flex-1 grid-cols-1 gap-2 overflow-hidden sm:grid-cols-2 sm:grid-rows-2"
        >
          @for (card of cards(); track card.thread.threadId) {
            @if (!card.isHero) {
              <article
                class="flex min-h-0 flex-col overflow-hidden rounded border px-2.5 py-2"
                [ngClass]="cardBorderClass(card.status.status, receptionRunning())"
              >
                <div class="flex shrink-0 items-start justify-between gap-1">
                  <h2 class="truncate text-sm font-semibold text-slate-100">{{ card.title }}</h2>
                  <span
                    class="shrink-0 rounded px-1.5 py-0.5 text-[10px] font-medium"
                    [ngClass]="statusChipClass(card.status.status)"
                  >
                    {{ card.status.label }}
                  </span>
                </div>
                <p class="mt-0.5 line-clamp-1 shrink-0 text-[11px] text-slate-400">
                  {{ card.mission }}
                </p>

                @if (receptionRunning()) {
                  @if (card.status.meaning) {
                    <p
                      class="mt-1 line-clamp-1 shrink-0 text-[11px] text-slate-300"
                      [title]="card.status.meaning"
                    >
                      {{ card.status.meaning }}
                    </p>
                  }
                  <dl class="mt-1.5 min-h-0 flex-1 space-y-0.5 overflow-hidden text-[11px]">
                    <div class="flex justify-between gap-2">
                      <dt class="shrink-0 text-slate-500">Busca</dt>
                      <dd class="truncate text-slate-200" [title]="card.documentKind">
                        {{ card.documentKind }}
                      </dd>
                    </div>
                    <div class="flex justify-between gap-2">
                      <dt class="shrink-0 text-slate-500">Posição</dt>
                      <dd
                        class="truncate font-mono text-slate-100"
                        [title]="card.nsuDeltaMeaning"
                      >
                        {{ card.positionLabel }}
                        <span [ngClass]="deltaClass(card.nsuDelta)">{{ card.nsuDeltaLabel }}</span>
                      </dd>
                    </div>
                    <div class="flex justify-between gap-2">
                      <dt class="shrink-0 text-slate-500">Guarda</dt>
                      <dd class="truncate text-slate-200" [title]="card.sourceTechnical">
                        {{ card.sourceLabel }}
                      </dd>
                    </div>
                    <div class="flex justify-between gap-2">
                      <dt class="shrink-0 text-slate-500">Atividade</dt>
                      <dd class="truncate text-slate-200">
                        @if (card.thread.lastActivityAt) {
                          há {{ card.lastAge }}
                        } @else {
                          —
                        }
                      </dd>
                    </div>
                  </dl>
                  <div
                    class="mt-1 flex shrink-0 items-center justify-between gap-1 border-t border-slate-800/80 pt-1"
                  >
                    <p class="min-w-0 truncate text-[10px] text-slate-500">
                      @if (card.recentLogs.length) {
                        {{ summarizeLog(card.recentLogs[0]) }}
                      } @else {
                        {{ card.emptyEvents }}
                      }
                    </p>
                    <a
                      class="shrink-0 text-[10px] text-cyan-400/90 hover:text-cyan-300"
                      [routerLink]="['../logs']"
                      [queryParams]="{ thread: card.thread.threadId }"
                      >Histórico →</a
                    >
                  </div>
                } @else {
                  <dl class="mt-1.5 space-y-0.5 text-[11px]">
                    <div class="flex justify-between gap-2">
                      <dt class="text-slate-500">Posição</dt>
                      <dd class="font-mono text-slate-100" title="NSU">{{ card.positionLabel }}</dd>
                    </div>
                    <div class="flex justify-between gap-2">
                      <dt class="shrink-0 text-slate-500">Guarda</dt>
                      <dd class="truncate text-slate-200" [title]="card.sourceTechnical">
                        {{ card.sourceLabel }}
                      </dd>
                    </div>
                  </dl>
                }
              </article>
            }
          }
        </div>

        @if (!cards().length) {
          <p class="rounded border border-slate-700 p-4 text-center text-sm text-slate-500">
            Sem dados das linhas — confira a conexão com o banco DEV.
          </p>
        }
      </div>
    </section>
  `,
})
export class ThreadsPageComponent {
  readonly store = inject(ServiceMonitorStore);

  private readonly nsuDeltaByThread = signal<Record<number, number | null>>({});
  private readonly prevNsu = signal<Record<number, string | null>>({});
  private readonly lastSnapAt = signal<string | null>(null);

  readonly receptionRunning = computed(() => {
    const service = this.store.service();
    return !!service?.isRunning && service.executar === 1;
  });

  readonly cards = computed((): ThreadCardVm[] => {
    const threads = this.store.threads();
    const logs = this.store.logs();
    const global = this.store.global();
    const service = this.store.service();
    const deltas = this.nsuDeltaByThread();
    const executar = service?.executar ?? 0;
    const processRunning = !!service?.isRunning;
    const running = processRunning && executar === 1;
    const intervalo = global?.intervaloSeconds ?? 60;

    return threads.map((thread) => {
      const nsuDelta = running ? (deltas[thread.threadId] ?? null) : null;
      const status = running
        ? resolveThreadRunStatus({
            thread,
            executar,
            processRunning,
            intervaloSeconds: intervalo,
            nsuDelta,
          })
        : awaitingReceptionStatus();
      const source = nsuSourceFriendly(thread.nsuSource);
      return {
        thread,
        status,
        nsuDelta,
        nsuDeltaLabel: running ? formatNsuDelta(nsuDelta) : '',
        nsuDeltaMeaning: running ? nsuDeltaMeaning(nsuDelta) : '',
        lastAge: formatAge(thread.lastActivityAt),
        recentLogs: running ? recentLogsForThread(logs, thread.threadId, 1) : [],
        isHero: thread.threadId === 1,
        title: threadCardTitle(thread.threadId, thread.role),
        mission: threadMission(thread.threadId),
        sourceLabel: source.label,
        sourceTechnical: source.technical,
        documentKind: documentKindLabel(thread.indDFe),
        emptyEvents: running ? emptyEventsMeaning(status.status) : '',
        positionLabel: thread.nsuAtual || '—',
      };
    });
  });

  readonly summary = computed(() =>
    summarizeThreadStatuses(this.cards().map((c) => c.status))
  );

  constructor() {
    effect(() => {
      const threads = this.store.threads();
      const snapAt = this.store.global()?.snapshotAtUtc ?? null;
      if (!snapAt || !threads.length) return;
      if (this.lastSnapAt() === snapAt) return;

      const prev = this.prevNsu();
      const first = this.lastSnapAt() == null;
      const deltas: Record<number, number | null> = {};
      const nextPrev: Record<number, string | null> = {};

      for (const t of threads) {
        const cur = t.nsuAtual ?? null;
        deltas[t.threadId] = first ? null : computeNsuDelta(cur, prev[t.threadId]);
        nextPrev[t.threadId] = cur;
      }

      this.nsuDeltaByThread.set(deltas);
      this.prevNsu.set(nextPrev);
      this.lastSnapAt.set(snapAt);
    });
  }

  summarizeLog(l: LogEntry): string {
    return summarizeLogMessage(l.mensagem, l.cStat);
  }

  cardBorderClass(status: ThreadRunStatus, running: boolean): string {
    if (!running) return 'border-slate-700 bg-slate-950/40';
    switch (status) {
      case 'in_cycle':
        return 'border-emerald-700/70 bg-slate-950/40';
      case 'idle':
        return 'border-slate-700 bg-slate-950/30';
      case 'outside_db':
        return 'border-sky-800/60 bg-slate-950/40';
      case 'no_evidence':
        return 'border-amber-900/50 bg-slate-950/40';
      case 'paused':
        return 'border-slate-600 bg-slate-950/50';
      default:
        return 'border-slate-700';
    }
  }

  statusChipClass(status: ThreadRunStatus): string {
    switch (status) {
      case 'in_cycle':
        return 'bg-emerald-950 text-emerald-300';
      case 'idle':
        return 'bg-slate-800 text-slate-300';
      case 'outside_db':
        return 'bg-sky-950 text-sky-300';
      case 'no_evidence':
        return 'bg-amber-950/80 text-amber-200/90';
      case 'paused':
        return 'bg-slate-800 text-slate-400';
      default:
        return 'bg-slate-800 text-slate-300';
    }
  }

  deltaClass(delta: number | null): string {
    if (delta == null || delta === 0) return 'text-slate-500';
    if (delta > 0) return 'text-emerald-400';
    return 'text-amber-300';
  }
}
