import {
  ChangeDetectionStrategy,
  Component,
  computed,
  effect,
  inject,
  signal,
} from '@angular/core';
import { FormsModule } from '@angular/forms';
import { DatePipe, NgClass } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { toSignal } from '@angular/core/rxjs-interop';
import { map } from 'rxjs/operators';
import { ArquivadorMonitorStore } from '@arquivador/monitor-core';
import { LogEntry } from '@arquivador/shared-data';
import {
  LogKind,
  classifyLogKind,
  logKindLabel,
  summarizeLogMessage,
} from '@arquivador/shared-utils';

@Component({
  selector: 'lib-logs-page',
  standalone: true,
  imports: [FormsModule, DatePipe, NgClass],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <section class="flex h-[calc(100vh-8rem)] flex-col gap-4">
      <header class="flex flex-wrap items-end justify-between gap-3">
        <div>
          <h1 class="text-2xl font-semibold text-zinc-50">Histórico</h1>
          <p class="text-sm text-zinc-400">
            Linha do tempo completa do que o Arquivador fez no banco.
            @if (store.live()) {
              <span class="ml-2 inline-flex items-center gap-1 text-teal-400">
                <span class="live-dot"></span> online
              </span>
            }
          </p>
        </div>
        <div class="flex flex-wrap items-center gap-2">
          <button
            type="button"
            class="rounded border border-zinc-600 px-3 py-1.5 text-sm hover:bg-zinc-900"
            (click)="togglePause()"
          >
            {{ paused() ? 'Retomar online' : 'Pausar' }}
          </button>
        </div>
      </header>

      <div class="flex flex-wrap gap-2">
        @for (f of filters; track f.id) {
          <button
            type="button"
            class="rounded-full border px-3 py-1.5 text-xs transition"
            [ngClass]="
              kindFilter() === f.id
                ? 'border-amber-400 bg-amber-950/50 text-amber-200'
                : 'border-zinc-700 text-zinc-400 hover:border-zinc-500'
            "
            (click)="kindFilter.set(f.id)"
          >
            {{ f.label }}
            <span class="ml-1 opacity-70">({{ countByKind(f.id) }})</span>
          </button>
        }
      </div>

      <div class="flex flex-wrap gap-3">
        <label class="text-xs text-zinc-400">
          Linha de trabalho
          <select
            class="ml-1 rounded border border-zinc-600 bg-zinc-900 px-2 py-1"
            [ngModel]="threadFilter()"
            (ngModelChange)="threadFilter.set($event)"
          >
            <option [ngValue]="null">Todas</option>
            @for (n of [1, 2, 3, 4, 5]; track n) {
              <option [ngValue]="n">Linha {{ n }}</option>
            }
          </select>
        </label>
        <label class="text-xs text-zinc-400">
          Buscar
          <input
            class="ml-1 min-w-[200px] rounded border border-zinc-600 bg-zinc-900 px-2 py-1"
            placeholder="texto do evento…"
            [ngModel]="textFilter()"
            (ngModelChange)="textFilter.set($event)"
          />
        </label>
        <p class="self-end text-xs text-zinc-500">
          Mostrando {{ visibleLogs().length }} de {{ sourceLogs().length }} eventos
        </p>
      </div>

      <div class="relative min-h-0 flex-1 overflow-auto rounded border border-zinc-700 bg-zinc-950/80 p-4">
        <div class="absolute bottom-0 left-[1.65rem] top-4 w-px bg-gradient-to-b from-amber-500/40 via-zinc-700 to-transparent"></div>
        <div class="space-y-0">
          @for (l of visibleNewestFirst(); track l.seqLog; let i = $index) {
            <article
              class="timeline-item relative flex gap-4 pb-5"
              [class.timeline-flash]="i === 0 && store.live() && !paused()"
            >
              <div class="relative z-10 mt-1 flex w-6 shrink-0 justify-center">
                <span
                  class="h-3 w-3 rounded-full ring-4 ring-zinc-950"
                  [ngClass]="dotClass(l)"
                ></span>
              </div>
              <div class="min-w-0 flex-1 rounded-lg border border-zinc-800 bg-zinc-900/50 px-3 py-2">
                <div class="flex flex-wrap items-center gap-2 text-[11px]">
                  <span
                    class="rounded px-1.5 py-0.5 font-medium"
                    [ngClass]="chipClass(l)"
                  >
                    {{ kindLabel(l) }}
                  </span>
                  @if (l.dtcLog) {
                    <time class="text-zinc-400">{{ l.dtcLog | date: 'dd/MM HH:mm:ss' }}</time>
                  }
                  <span class="text-zinc-600">#{{ l.seqLog }}</span>
                  @if (l.threadId) {
                    <span class="text-amber-400/90">Linha {{ l.threadId }}</span>
                  }
                  @if (l.cStat) {
                    <span class="text-violet-300/90">cStat {{ l.cStat }}</span>
                  }
                </div>
                <p class="mt-1 text-sm text-zinc-100">{{ summary(l) }}</p>
                @if ((l.mensagem ?? '') !== summary(l)) {
                  <details class="mt-1">
                    <summary class="cursor-pointer text-[11px] text-zinc-500 hover:text-zinc-300">
                      Ver texto completo
                    </summary>
                    <p class="mt-1 whitespace-pre-wrap font-mono text-[11px] text-zinc-400">
                      {{ l.mensagem }}
                    </p>
                  </details>
                }
              </div>
            </article>
          } @empty {
            <p class="p-6 text-center text-zinc-500">
              Nenhum evento neste filtro. Tente “Todos” ou ligue o Arquivador.
            </p>
          }
        </div>
      </div>
    </section>
  `,
})
export class LogsPageComponent {
  readonly store = inject(ArquivadorMonitorStore);
  private readonly route = inject(ActivatedRoute);

  readonly paused = signal(false);
  readonly frozen = signal<LogEntry[]>([]);
  readonly threadFilter = signal<number | null>(null);
  readonly textFilter = signal('');
  readonly kindFilter = signal<LogKind>('all');

  private readonly threadQuery = toSignal(
    this.route.queryParamMap.pipe(
      map((q) => {
        const raw = q.get('thread');
        if (raw == null || raw === '') return null;
        const n = Number(raw);
        return Number.isInteger(n) && n >= 1 && n <= 5 ? n : null;
      })
    ),
    { initialValue: null as number | null }
  );

  readonly filters: { id: LogKind; label: string }[] = [
    { id: 'all', label: 'Todos' },
    { id: 'success', label: 'Sucesso' },
    { id: 'error', label: 'Somente erros' },
    { id: 'warning', label: 'Avisos' },
    { id: 'info', label: 'Outros' },
  ];

  readonly sourceLogs = computed(() =>
    this.paused() ? this.frozen() : this.store.logs()
  );

  readonly visibleLogs = computed(() => {
    const kind = this.kindFilter();
    const thread = this.threadFilter();
    const text = this.textFilter().toLowerCase();
    return this.sourceLogs().filter((l) => {
      const k = classifyLogKind(l.severityHint, l.cStat);
      if (kind !== 'all' && k !== kind) return false;
      if (thread != null && l.threadId !== thread) return false;
      if (text && !(l.mensagem ?? '').toLowerCase().includes(text)) return false;
      return true;
    });
  });

  readonly visibleNewestFirst = computed(() =>
    [...this.visibleLogs()].reverse()
  );

  constructor() {
    effect(() => {
      if (!this.paused()) {
        this.frozen.set(this.store.logs());
      }
    });

    effect(() => {
      const fromQuery = this.threadQuery();
      if (fromQuery != null) {
        this.threadFilter.set(fromQuery);
      }
    });
  }

  countByKind(kind: LogKind): number {
    const all = this.sourceLogs();
    if (kind === 'all') return all.length;
    return all.filter((l) => classifyLogKind(l.severityHint, l.cStat) === kind).length;
  }

  kindLabel(l: LogEntry): string {
    return logKindLabel(classifyLogKind(l.severityHint, l.cStat));
  }

  summary(l: LogEntry): string {
    return summarizeLogMessage(l.mensagem, l.cStat);
  }

  dotClass(l: LogEntry): string {
    switch (classifyLogKind(l.severityHint, l.cStat)) {
      case 'success':
        return 'bg-teal-400';
      case 'error':
        return 'bg-rose-400';
      case 'warning':
        return 'bg-amber-400';
      default:
        return 'bg-zinc-400';
    }
  }

  chipClass(l: LogEntry): string {
    switch (classifyLogKind(l.severityHint, l.cStat)) {
      case 'success':
        return 'bg-teal-950 text-teal-300';
      case 'error':
        return 'bg-rose-950 text-rose-300';
      case 'warning':
        return 'bg-amber-950 text-amber-300';
      default:
        return 'bg-zinc-800 text-zinc-300';
    }
  }

  togglePause(): void {
    if (!this.paused()) {
      this.frozen.set(this.store.logs());
    }
    this.paused.update((v) => !v);
  }
}
