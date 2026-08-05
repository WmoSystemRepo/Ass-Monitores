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
import { CargaMonitorStore } from '@Carga/monitor-core';
import { LogEntry, ThreadView } from '@Carga/shared-data';
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
} from '@Carga/shared-utils';

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
      <header class="flex shrink-0 flex-wrap items-end justify-between gap-2">
        <div class="min-w-0">
          <p
            class="text-[10px] font-medium uppercase tracking-[0.14em]"
            [ngClass]="receptionRunning() ? 'text-amber-400/90' : 'text-zinc-400'"
          >
            {{ receptionRunning() ? 'Em tempo real' : 'Carga parado' }}
          </p>
          <h1 class="text-xl font-semibold text-zinc-50">Linhas de trabalho</h1>
          <p class="truncate text-xs text-zinc-400">
            @if (receptionRunning()) {
              Pool de workers do Carga — cada card é uma linha (status, atividade,
              posição).
            } @else {
              Números congelados — ligue no Monitor para ver os workers processando.
            }
          </p>
        </div>
        @if (!receptionRunning()) {
          <a
            routerLink="/"
            class="shrink-0 rounded border border-amber-700/60 bg-amber-950/40 px-3 py-1.5 text-xs text-amber-200 hover:border-amber-500"
          >
            Abrir Monitor →
          </a>
        }
      </header>

      @if (!receptionRunning()) {
        <div
          class="flex shrink-0 items-center justify-between gap-3 rounded border border-zinc-600 bg-zinc-900/70 px-3 py-2 text-xs text-zinc-200"
          role="status"
        >
          <p class="min-w-0 truncate">
            O Carga não está sintetizando. Cards mostram só configuração e última posição.
          </p>
          <span class="shrink-0 text-zinc-400">Ciclo: <span class="text-zinc-100">parado</span></span>
        </div>
      } @else {
        <div
          class="flex shrink-0 flex-wrap items-center gap-x-4 gap-y-0.5 rounded border border-zinc-700/80 bg-zinc-950/60 px-3 py-1.5 text-xs text-zinc-300"
          aria-label="Resumo das linhas"
        >
          <span title="Há sinal recente de processamento no ciclo">
            <span class="font-semibold text-teal-300">{{ summary().inCycle }}</span>
            processando
          </span>
          <span title="Contador em zero — worker desligado">
            <span class="font-semibold text-zinc-200">{{ summary().idle }}</span>
            ociosos
          </span>
          <span title="Estado guardado em arquivo, não no banco">
            <span class="font-semibold text-amber-300">{{ summary().outsideDb }}</span>
            em arquivo local
          </span>
          <span title="Apta a processar, sem registro recente no banco">
            <span class="font-semibold text-amber-200/90">{{ summary().noEvidence }}</span>
            sem atividade recente
          </span>
        </div>
      }

      <div class="flex min-h-0 flex-1 flex-col gap-2 overflow-hidden">
        @for (card of cards(); track card.thread.threadId) {
          @if (card.isHero) {
            <article
              class="shrink-0 overflow-hidden rounded border px-3 py-2.5"
              [ngClass]="cardBorderClass(card.status.status, receptionRunning())"
            >
              <div class="flex items-start justify-between gap-2">
                <div class="min-w-0">
                  <div class="flex flex-wrap items-center gap-2">
                    <h2 class="text-base font-semibold text-zinc-100">{{ card.title }}</h2>
                    <span
                      class="rounded px-1.5 py-0.5 text-[10px] font-medium"
                      [ngClass]="statusChipClass(card.status.status)"
                    >
                      {{ card.status.label }}
                    </span>
                  </div>
                  <p class="mt-0.5 line-clamp-1 text-xs text-zinc-400">{{ card.mission }}</p>
                </div>
                @if (receptionRunning()) {
                  <a
                    class="shrink-0 text-[11px] text-teal-400/90 hover:text-teal-300"
                    [routerLink]="['/logs']"
                    [queryParams]="{ thread: card.thread.threadId }"
                    >Histórico →</a
                  >
                }
              </div>

              @if (receptionRunning() && card.status.meaning) {
                <p
                  class="mt-1.5 line-clamp-1 text-xs text-zinc-300"
                  [title]="card.status.meaning"
                >
                  {{ card.status.meaning }}
                </p>
              }

              @if (receptionRunning()) {
                <div class="mt-2 grid grid-cols-2 gap-2 lg:grid-cols-4">
                  <div class="min-w-0 overflow-hidden">
                    <p class="text-[10px] uppercase tracking-wide text-zinc-500">Papel</p>
                    <p
                      class="truncate text-xs text-zinc-100"
                      [title]="'indDFe=' + card.thread.indDFe"
                    >
                      {{ card.documentKind }}
                    </p>
                  </div>
                  <div class="min-w-0 overflow-hidden">
                    <p class="text-[10px] uppercase tracking-wide text-zinc-500">
                      Posição
                    </p>
                    <p class="truncate font-mono text-xs text-zinc-100" [title]="card.nsuDeltaMeaning">
                      {{ card.positionLabel }}
                      <span [ngClass]="deltaClass(card.nsuDelta)">{{ card.nsuDeltaLabel }}</span>
                    </p>
                  </div>
                  <div class="min-w-0 overflow-hidden">
                    <p class="text-[10px] uppercase tracking-wide text-zinc-500">Onde guarda</p>
                    <p class="truncate text-xs text-zinc-100" [title]="card.sourceTechnical">
                      {{ card.sourceLabel }}
                    </p>
                  </div>
                  <div class="min-w-0 overflow-hidden">
                    <p class="text-[10px] uppercase tracking-wide text-zinc-500">
                      Última atividade
                    </p>
                    <p class="truncate text-xs text-zinc-100">
                      @if (card.thread.lastActivityAt) {
                        há {{ card.lastAge }}
                        @if (card.thread.lastCStat) {
                          <span class="text-teal-300/90">· {{ card.thread.lastCStat }}</span>
                        }
                      } @else {
                        Sem registro
                      }
                    </p>
                  </div>
                </div>
                <p class="mt-1.5 truncate text-[11px] text-zinc-500">
                  @if (card.recentLogs.length) {
                    <span class="text-zinc-400">{{ summarizeLog(card.recentLogs[0]) }}</span>
                  } @else {
                    {{ card.emptyEvents }}
                  }
                </p>
              } @else {
                <div class="mt-1.5 flex flex-wrap gap-x-6 gap-y-1 text-xs">
                  <p>
                    <span class="text-zinc-500">Posição · </span>
                    <span class="font-mono text-zinc-100" title="NSU">{{ card.positionLabel }}</span>
                  </p>
                  <p class="min-w-0 truncate" [title]="card.sourceTechnical">
                    <span class="text-zinc-500">Guarda · </span>
                    <span class="text-zinc-200">{{ card.sourceLabel }}</span>
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
                  <h2 class="truncate text-sm font-semibold text-zinc-100">{{ card.title }}</h2>
                  <span
                    class="shrink-0 rounded px-1.5 py-0.5 text-[10px] font-medium"
                    [ngClass]="statusChipClass(card.status.status)"
                  >
                    {{ card.status.label }}
                  </span>
                </div>
                <p class="mt-0.5 line-clamp-1 shrink-0 text-[11px] text-zinc-400">
                  {{ card.mission }}
                </p>

                @if (receptionRunning()) {
                  @if (card.status.meaning) {
                    <p
                      class="mt-1 line-clamp-1 shrink-0 text-[11px] text-zinc-300"
                      [title]="card.status.meaning"
                    >
                      {{ card.status.meaning }}
                    </p>
                  }
                  <dl class="mt-1.5 min-h-0 flex-1 space-y-0.5 overflow-hidden text-[11px]">
                    <div class="flex justify-between gap-2">
                      <dt class="shrink-0 text-zinc-500">Papel</dt>
                      <dd class="truncate text-zinc-200" [title]="card.documentKind">
                        {{ card.documentKind }}
                      </dd>
                    </div>
                    <div class="flex justify-between gap-2">
                      <dt class="shrink-0 text-zinc-500">Posição</dt>
                      <dd
                        class="truncate font-mono text-zinc-100"
                        [title]="card.nsuDeltaMeaning"
                      >
                        {{ card.positionLabel }}
                        <span [ngClass]="deltaClass(card.nsuDelta)">{{ card.nsuDeltaLabel }}</span>
                      </dd>
                    </div>
                    <div class="flex justify-between gap-2">
                      <dt class="shrink-0 text-zinc-500">Guarda</dt>
                      <dd class="truncate text-zinc-200" [title]="card.sourceTechnical">
                        {{ card.sourceLabel }}
                      </dd>
                    </div>
                    <div class="flex justify-between gap-2">
                      <dt class="shrink-0 text-zinc-500">Atividade</dt>
                      <dd class="truncate text-zinc-200">
                        @if (card.thread.lastActivityAt) {
                          há {{ card.lastAge }}
                        } @else {
                          —
                        }
                      </dd>
                    </div>
                  </dl>
                  <div
                    class="mt-1 flex shrink-0 items-center justify-between gap-1 border-t border-zinc-800/80 pt-1"
                  >
                    <p class="min-w-0 truncate text-[10px] text-zinc-500">
                      @if (card.recentLogs.length) {
                        {{ summarizeLog(card.recentLogs[0]) }}
                      } @else {
                        {{ card.emptyEvents }}
                      }
                    </p>
                    <a
                      class="shrink-0 text-[10px] text-teal-400/90 hover:text-teal-300"
                      [routerLink]="['/logs']"
                      [queryParams]="{ thread: card.thread.threadId }"
                      >Histórico →</a
                    >
                  </div>
                } @else {
                  <dl class="mt-1.5 space-y-0.5 text-[11px]">
                    <div class="flex justify-between gap-2">
                      <dt class="text-zinc-500">Posição</dt>
                      <dd class="font-mono text-zinc-100" title="NSU">{{ card.positionLabel }}</dd>
                    </div>
                    <div class="flex justify-between gap-2">
                      <dt class="shrink-0 text-zinc-500">Guarda</dt>
                      <dd class="truncate text-zinc-200" [title]="card.sourceTechnical">
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
          <p class="rounded border border-zinc-700 p-4 text-center text-sm text-zinc-500">
            Sem dados das linhas — confira a conexão com o banco DEV.
          </p>
        }
      </div>
    </section>
  `,
})
export class ThreadsPageComponent {
  readonly store = inject(CargaMonitorStore);

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
    if (!running) return 'border-zinc-700 bg-zinc-950/40';
    switch (status) {
      case 'in_cycle':
        return 'border-teal-700/70 bg-zinc-950/40';
      case 'idle':
        return 'border-zinc-700 bg-zinc-950/30';
      case 'outside_db':
        return 'border-amber-800/60 bg-zinc-950/40';
      case 'no_evidence':
        return 'border-amber-900/50 bg-zinc-950/40';
      case 'paused':
        return 'border-zinc-600 bg-zinc-950/50';
      default:
        return 'border-zinc-700';
    }
  }

  statusChipClass(status: ThreadRunStatus): string {
    switch (status) {
      case 'in_cycle':
        return 'bg-teal-950 text-teal-300';
      case 'idle':
        return 'bg-zinc-800 text-zinc-300';
      case 'outside_db':
        return 'bg-amber-950 text-amber-300';
      case 'no_evidence':
        return 'bg-amber-950/80 text-amber-200/90';
      case 'paused':
        return 'bg-zinc-800 text-zinc-400';
      default:
        return 'bg-zinc-800 text-zinc-300';
    }
  }

  deltaClass(delta: number | null): string {
    if (delta == null || delta === 0) return 'text-zinc-500';
    if (delta > 0) return 'text-teal-400';
    return 'text-amber-300';
  }
}
