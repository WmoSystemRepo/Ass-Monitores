import {
  ChangeDetectionStrategy,
  Component,
  computed,
  inject,
} from '@angular/core';
import { DatePipe, NgClass } from '@angular/common';
import { RouterLink } from '@angular/router';
import { ReceptorMonitorStore } from '@receptor/monitor-core';
import {
  classifyLogKind,
  describeLogActivity,
  logKindLabel,
  severityLabel,
  summarizeLogMessage,
} from '@receptor/shared-utils';

/** Quantidade alta para preencher o quadrante; overflow-hidden corta o excedente sem scroll. */
const FEED_CAP = 18;
const EVENT_CAP = 18;
const ALERT_CAP = 12;
const LOTE_CAP = 8;

@Component({
  selector: 'lib-details-page',
  standalone: true,
  imports: [RouterLink, DatePipe, NgClass],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <section
      class="details-fit flex h-[calc(100vh-3rem)] max-h-[calc(100vh-3rem)] flex-col gap-2 overflow-hidden"
    >
      <header class="flex shrink-0 flex-wrap items-center justify-between gap-2">
        <div>
          <p class="text-[10px] uppercase tracking-[0.2em] text-slate-500">Diagnóstico</p>
          <h1 class="text-xl font-semibold text-slate-50">Mais informações</h1>
        </div>
        <div class="flex flex-wrap items-center gap-3">
          <a routerLink="/logs" class="text-xs text-cyan-400 hover:underline">Histórico →</a>
          <a routerLink="/" class="text-xs text-cyan-400 hover:underline">← Voltar ao Monitor</a>
        </div>
      </header>

      <div class="grid min-h-0 flex-1 grid-cols-1 gap-2 lg:grid-cols-2">
        <section
          class="flex min-h-0 flex-col rounded border border-slate-700 bg-slate-950/40 p-3"
        >
          <div class="mb-1.5 shrink-0">
            <h2 class="text-sm font-medium text-slate-200">Passos do Receptor · online</h2>
            <p class="text-[10px] text-slate-500">Debug do DevHost + SQL — alimentam o AGORA do Monitor</p>
          </div>
          <div class="min-h-0 flex-1 space-y-1 overflow-hidden">
            @for (row of flowFeed(); track row.key) {
              <div class="flex gap-2 text-[11px] leading-snug text-slate-400">
                @if (row.at) {
                  <span class="shrink-0 font-mono text-slate-600">{{ row.at | date: 'HH:mm:ss' }}</span>
                }
                <span
                  class="shrink-0 rounded px-1 py-0.5 text-[9px] uppercase"
                  [class.bg-violet-950]="row.kind === 'debug'"
                  [class.text-violet-300]="row.kind === 'debug'"
                  [class.bg-slate-800]="row.kind === 'sql'"
                  [class.text-slate-300]="row.kind === 'sql'"
                  >{{ row.kind }}</span
                >
                <span class="min-w-0 truncate text-slate-200">{{ row.text }}</span>
              </div>
            } @empty {
              <p class="text-xs text-slate-500">
                Sem passos Debug/SQL. Ligue o Receptor pelo Monitor.
              </p>
            }
          </div>
        </section>

        <section
          class="flex min-h-0 flex-col rounded border border-slate-700 bg-slate-950/40 p-3"
        >
          <div class="mb-1.5 shrink-0">
            <h2 class="text-sm font-medium text-slate-200">Últimos eventos SQL do Receptor</h2>
            <p class="text-[10px] text-slate-500">Classificados no banco (sucesso, erro, aviso)</p>
          </div>
          <div class="min-h-0 flex-1 space-y-1 overflow-hidden">
            @for (l of recentActivity(); track l.seqLog; let i = $index) {
              <div
                class="flex items-center gap-2 text-[11px] leading-snug"
                [class.activity-new]="i === 0 && store.live()"
              >
                <span
                  class="h-2 w-2 shrink-0 rounded-full"
                  [ngClass]="activityDot(l.severityHint, l.cStat)"
                ></span>
                <span
                  class="shrink-0"
                  [ngClass]="activityChip(l.severityHint, l.cStat)"
                  >{{ activityLabel(l.severityHint, l.cStat) }}</span
                >
                @if (l.dtcLog) {
                  <span class="shrink-0 font-mono text-slate-600">{{
                    l.dtcLog | date: 'HH:mm:ss'
                  }}</span>
                }
                <span class="min-w-0 truncate text-slate-200">{{
                  summarize(l.mensagem, l.cStat)
                }}</span>
              </div>
            } @empty {
              <p class="text-xs text-slate-500">Ainda sem eventos SQL.</p>
            }
          </div>
        </section>

        <section
          class="flex min-h-0 flex-col rounded border border-slate-700 bg-slate-950/40 p-3"
        >
          <div class="mb-1.5 flex shrink-0 flex-wrap items-center justify-between gap-2">
            <div>
              <h2 class="text-sm font-medium text-slate-200">Receptor · parâmetros e lotes</h2>
              <p class="text-[10px] text-slate-500">Configuração ativa e CT-e recentes na temporária</p>
            </div>
            <div class="flex flex-wrap gap-2">
              <a routerLink="/config" class="text-[10px] text-cyan-400 hover:underline"
                >Config →</a
              >
            </div>
          </div>
          <div class="flex shrink-0 flex-wrap gap-x-3 gap-y-1 text-[11px]">
            <span class="text-slate-500"
              >NSU
              <span class="font-mono text-slate-200">{{ store.global()?.mainNsu || '—' }}</span></span
            >
            <span class="text-slate-500"
              >Intervalo <span class="text-slate-200">{{ intervaloLabel() }}</span></span
            >
            <span class="text-slate-500"
              >Pacote completo
              <span class="text-slate-200">{{
                store.global()?.pacoteCompleto === 1 ? 'Sim' : 'Não'
              }}</span></span
            >
            <span class="text-slate-500"
              >Rebusca
              <span class="text-slate-200">{{
                store.global()?.reBuscar === 1 ? 'Ligada' : 'Desligada'
              }}</span></span
            >
            <span class="text-slate-500" title="Threads"
              >Linhas
              <span class="text-slate-200">{{ store.global()?.configuredThreads }}</span></span
            >
          </div>
          <div class="mt-2 min-h-0 flex-1 space-y-1 overflow-hidden border-t border-slate-800 pt-2">
            @for (lote of recentLotes(); track lote.nsu + (lote.dtcAtualizacao ?? '')) {
              <p class="truncate text-[11px] text-slate-400">
                <span class="font-mono text-slate-200"
                  >{{ lote.nsu }} → {{ lote.nsuFinal ?? lote.nsu }}</span
                >
                · {{ lote.qtdDocumento }} CT-e ·
                @if (lote.dtcAtualizacao) {
                  {{ lote.dtcAtualizacao | date: 'dd/MM HH:mm:ss' }}
                } @else {
                  —
                }
              </p>
            } @empty {
              <p class="text-[11px] text-slate-500">Nenhum lote recente na temporária.</p>
            }
          </div>
        </section>

        <section
          class="flex min-h-0 flex-col rounded border border-slate-700 bg-slate-950/40 p-3"
        >
          <div class="mb-1.5 shrink-0">
            <h2 class="text-sm font-medium text-slate-200">Avisos do Monitor</h2>
            <p class="text-[10px] text-slate-500">Alertas de saúde, fila, NSU e regras RN</p>
          </div>
          <div class="min-h-0 flex-1 space-y-1 overflow-hidden">
            @for (a of topAlerts(); track a.code + a.message) {
              <div class="rounded border px-2 py-1 text-[11px]" [ngClass]="alertBorder(a.severity)">
                <span class="text-slate-500"
                  >{{ severityLabel(a.severity) }} · {{ a.code }}</span
                >
                <p class="truncate text-slate-200">{{ a.message }}</p>
              </div>
            } @empty {
              <p class="text-xs text-slate-500">Nenhum aviso do Monitor no momento.</p>
            }
          </div>
        </section>
      </div>
    </section>
  `,
})
export class DetailsPageComponent {
  readonly store = inject(ReceptorMonitorStore);
  readonly severityLabel = severityLabel;
  readonly summarize = summarizeLogMessage;

  readonly recentLotes = computed(() => this.store.documents().slice(0, LOTE_CAP));

  readonly intervaloLabel = computed(() => {
    const raw = this.store.global()?.intervaloSeconds ?? 0;
    const sec = raw >= 1000 ? Math.round(raw / 1000) : raw;
    return `${sec}s`;
  });

  readonly recentActivity = computed(() =>
    [...this.store.logs()].slice(-EVENT_CAP).reverse()
  );

  readonly topAlerts = computed(() => {
    const rank = (severity: string | number): number => {
      const s = severityLabel(severity);
      if (s === 'Crítico' || s === 'Critico') return 0;
      if (s === 'Alerta') return 1;
      if (s === 'Atenção') return 2;
      return 3;
    };
    return [...this.store.alerts()]
      .sort((a, b) => rank(a.severity) - rank(b.severity))
      .slice(0, ALERT_CAP);
  });

  readonly flowFeed = computed(() => {
    const debug = [...this.store.liveTrace()]
      .reverse()
      .slice(0, FEED_CAP)
      .map((t) => ({
        key: `d-${t.at}-${t.message}`,
        at: t.at,
        text:
          describeLogActivity({ mensagem: t.message, dtcLog: t.at, source: 'debug' })?.detail ??
          t.message,
        kind: 'debug' as const,
      }));

    const sql = [...this.store.logs()]
      .reverse()
      .slice(0, FEED_CAP)
      .map((l) => ({
        key: `s-${l.seqLog}`,
        at: l.dtcLog,
        text: summarizeLogMessage(l.mensagem, l.cStat),
        kind: 'sql' as const,
      }));

    return [...debug, ...sql]
      .sort((a, b) => {
        const ta = a.at ? new Date(a.at).getTime() : 0;
        const tb = b.at ? new Date(b.at).getTime() : 0;
        return tb - ta;
      })
      .slice(0, FEED_CAP);
  });

  activityLabel(hint?: string | null, cStat?: string | null): string {
    return logKindLabel(classifyLogKind(hint, cStat));
  }

  activityDot(hint?: string | null, cStat?: string | null): string {
    switch (classifyLogKind(hint, cStat)) {
      case 'success':
        return 'bg-emerald-400';
      case 'error':
        return 'bg-rose-400';
      case 'warning':
        return 'bg-amber-400';
      default:
        return 'bg-slate-400';
    }
  }

  activityChip(hint?: string | null, cStat?: string | null): string {
    switch (classifyLogKind(hint, cStat)) {
      case 'success':
        return 'text-emerald-300';
      case 'error':
        return 'text-rose-300';
      case 'warning':
        return 'text-amber-300';
      default:
        return 'text-slate-400';
    }
  }

  alertBorder(severity: string | number): string {
    const s = severityLabel(severity);
    if (s === 'Crítico' || s === 'Critico') return 'border-rose-500';
    if (s === 'Alerta') return 'border-orange-500';
    if (s === 'Atenção') return 'border-amber-500';
    return 'border-slate-600';
  }
}
