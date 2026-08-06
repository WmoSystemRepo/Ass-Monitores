import {
  ChangeDetectionStrategy,
  Component,
  computed,
  inject,
} from '@angular/core';
import { DatePipe, NgClass } from '@angular/common';
import { RouterLink } from '@angular/router';
import { ConfirmDialogService } from '@orquestrador/shared-ui';
import { LogEntry } from '@orquestrador/shared-data';
import { ServiceMonitorStore } from '../service-monitor.store';
import {
  classifyLogKind,
  describeLogActivity,
  logKindLabel,
  severityLabel,
  summarizeLogMessage,
} from '@orquestrador/shared-utils';

const FEED_CAP = 12;
const EVENT_CAP = 12;
const ALERT_CAP = 8;
const LOTE_CAP = 6;

@Component({
  selector: 'lib-receptor-details-page',
  standalone: true,
  imports: [RouterLink, DatePipe, NgClass],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <section
      class="details-fit flex h-[calc(100vh-3.25rem)] max-h-[calc(100vh-3.25rem)] flex-col gap-2 overflow-hidden"
    >
      <header class="flex shrink-0 flex-wrap items-center justify-between gap-2">
        <div>
          <p class="text-[10px] uppercase tracking-[0.18em] text-slate-500">
            Entenda o que o Receptor está fazendo
          </p>
          <h1 class="text-lg font-semibold text-slate-50">Mais informações</h1>
        </div>
        <div class="flex flex-wrap items-center gap-3">
          <a
            routerLink="/monitores/receptor/logs"
            class="text-xs text-cyan-400 hover:underline"
            >Histórico →</a
          >
          <a
            routerLink="/monitores/receptor"
            class="text-xs text-cyan-400 hover:underline"
            >← Voltar ao painel</a
          >
        </div>
      </header>

      <div
        class="grid min-h-0 flex-1 grid-cols-1 grid-rows-2 gap-2 overflow-hidden lg:grid-cols-2"
      >
        <!-- Passos -->
        <section class="details-card flex min-h-0 flex-col">
          <div class="mb-1.5 shrink-0">
            <h2 class="text-sm font-semibold text-slate-100">O que aconteceu agora</h2>
            <p class="text-[11px] text-slate-500">
              Passos recentes (técnicos e do banco) — do mais novo para o mais antigo
            </p>
          </div>
          <div class="min-h-0 flex-1 space-y-1 overflow-hidden">
            @for (row of flowFeed(); track row.key) {
              <div class="flex gap-2 text-[11px] leading-snug text-slate-400">
                @if (row.at) {
                  <span class="shrink-0 font-mono text-slate-600">{{
                    row.at | date: 'HH:mm:ss'
                  }}</span>
                }
                <span
                  class="shrink-0 rounded px-1 py-0.5 text-[9px] uppercase"
                  [class.bg-violet-950]="row.kind === 'debug'"
                  [class.text-violet-300]="row.kind === 'debug'"
                  [class.bg-slate-800]="row.kind === 'sql'"
                  [class.text-slate-300]="row.kind === 'sql'"
                  >{{ row.kind === 'debug' ? 'passo' : 'banco' }}</span
                >
                <span class="min-w-0 truncate text-slate-200">{{ row.text }}</span>
              </div>
            } @empty {
              <p class="text-xs leading-relaxed text-slate-500">
                Ainda sem atividade. Ligue o fluxo no painel do Receptor para ver os
                passos aparecerem aqui.
              </p>
            }
          </div>
        </section>

        <!-- Eventos SQL -->
        <section class="details-card flex min-h-0 flex-col">
          <div class="mb-1.5 shrink-0">
            <h2 class="text-sm font-semibold text-slate-100">Últimos eventos do banco</h2>
            <p class="text-[11px] text-slate-500">
              Sucesso, aviso ou erro. Em erro, abra o texto original para estudar.
            </p>
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
                <span class="min-w-0 flex-1 truncate text-slate-200">{{
                  summarize(l.mensagem, l.cStat)
                }}</span>
                @if (isErrorEvent(l)) {
                  <button
                    type="button"
                    class="shrink-0 rounded border border-rose-500/50 bg-rose-950/40 px-1.5 py-0.5 text-[10px] font-semibold text-rose-200 hover:bg-rose-900/50"
                    title="Ver erro original"
                    (click)="openErrorDetail(l)"
                  >
                    Ver erro
                  </button>
                }
              </div>
            } @empty {
              <p class="text-xs leading-relaxed text-slate-500">
                Sem eventos no banco ainda. Eles aparecem quando o Receptor consulta a
                SEFAZ ou grava lotes.
              </p>
            }
          </div>
        </section>

        <!-- Lotes -->
        <section class="details-card flex min-h-0 flex-col">
          <div class="mb-1.5 flex shrink-0 flex-wrap items-center justify-between gap-2">
            <div>
              <h2 class="text-sm font-semibold text-slate-100">Configuração e lotes</h2>
              <p class="text-[11px] text-slate-500">
                Como o Receptor está configurado e CT-e recentes na temporária
              </p>
            </div>
            <a
              routerLink="/monitores/receptor/config"
              class="text-[10px] text-cyan-400 hover:underline"
              >Config →</a
            >
          </div>
          <div class="flex shrink-0 flex-wrap gap-x-3 gap-y-1 text-[11px]">
            <span class="rounded bg-slate-900/80 px-1.5 py-0.5 text-slate-500"
              >NSU
              <span class="font-mono text-slate-100">{{
                store.global()?.mainNsu || '—'
              }}</span></span
            >
            <span class="rounded bg-slate-900/80 px-1.5 py-0.5 text-slate-500"
              >A cada <span class="text-slate-100">{{ intervaloLabel() }}</span></span
            >
            <span class="rounded bg-slate-900/80 px-1.5 py-0.5 text-slate-500"
              >Na temporária
              <span class="font-mono text-sky-300">{{
                store.queues()?.tempBacklog ?? 0
              }}</span></span
            >
            <span class="rounded bg-slate-900/80 px-1.5 py-0.5 text-slate-500"
              >Na fila
              <span class="font-mono text-amber-300">{{
                store.queues()?.serviceBrokerDepth ?? 0
              }}</span></span
            >
          </div>
          <div
            class="mt-2 min-h-0 flex-1 space-y-1 overflow-hidden border-t border-slate-800/80 pt-2"
          >
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
              <p class="text-[11px] leading-relaxed text-slate-500">
                @if ((store.queues()?.tempBacklog ?? 0) > 0) {
                  Há {{ store.queues()?.tempBacklog }} na temporária, mas a lista
                  detalhada de lotes ainda não veio. Tente Ligar o fluxo ou aguarde o
                  próximo ciclo.
                } @else {
                  Nenhum lote recente na temporária — normal quando o Receptor está
                  parado ou sem CT-e novos.
                }
              </p>
            }
          </div>
        </section>

        <!-- Avisos -->
        <section class="details-card flex min-h-0 flex-col">
          <div class="mb-1.5 shrink-0">
            <h2 class="text-sm font-semibold text-slate-100">Avisos e saúde</h2>
            <p class="text-[11px] text-slate-500">
              Situações que pedem atenção (processo, fila, batida no banco)
            </p>
          </div>
          <div class="min-h-0 flex-1 space-y-1.5 overflow-hidden">
            @for (a of topAlerts(); track a.code + a.message) {
              <div
                class="rounded-md border px-2.5 py-1.5 text-[11px]"
                [ngClass]="alertBorder(a.severity)"
              >
                <span class="text-slate-500"
                  >{{ severityLabel(a.severity) }}
                  @if (a.code) {
                    · {{ a.code }}
                  }
                </span>
                <p class="mt-0.5 text-slate-100">{{ a.message }}</p>
              </div>
            } @empty {
              <p class="text-xs leading-relaxed text-slate-500">
                Sem avisos no momento. Quando o serviço estiver parado ou a batida
                atrasar, os alertas aparecem aqui.
              </p>
            }
          </div>
        </section>
      </div>
    </section>
  `,
  styles: [
    `
      .details-card {
        border-radius: 0.75rem;
        border: 1px solid rgba(51, 65, 85, 0.85);
        background: linear-gradient(
          180deg,
          rgba(15, 23, 42, 0.72),
          rgba(2, 6, 23, 0.55)
        );
        padding: 0.75rem 0.85rem;
      }
    `,
  ],
})
export class ReceptorDetailsPageComponent {
  readonly store = inject(ServiceMonitorStore);
  private readonly confirmDialog = inject(ConfirmDialogService);
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
          describeLogActivity({ mensagem: t.message, dtcLog: t.at, source: 'debug' })
            ?.detail ?? t.message,
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

  isErrorEvent(l: LogEntry): boolean {
    return classifyLogKind(l.severityHint, l.cStat) === 'error';
  }

  openErrorDetail(l: LogEntry): void {
    const original = (l.mensagem || '').trim() || '(sem mensagem no registro)';
    void this.confirmDialog.ask({
      mode: 'info',
      title: 'Erro original (banco)',
      message: `Evento #${l.seqLog}${l.cStat ? ` · cStat ${l.cStat}` : ''} — texto completo para estudo:`,
      detail: original,
      confirmLabel: 'Fechar',
      tone: 'danger',
    });
  }

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
        return 'bg-slate-500';
    }
  }

  activityChip(hint?: string | null, cStat?: string | null): string {
    switch (classifyLogKind(hint, cStat)) {
      case 'success':
        return 'rounded px-1 text-[9px] font-semibold uppercase text-emerald-300';
      case 'error':
        return 'rounded px-1 text-[9px] font-semibold uppercase text-rose-300';
      case 'warning':
        return 'rounded px-1 text-[9px] font-semibold uppercase text-amber-300';
      default:
        return 'rounded px-1 text-[9px] font-semibold uppercase text-slate-400';
    }
  }

  alertBorder(severity: string | number): string {
    const s = severityLabel(severity);
    if (s === 'Crítico' || s === 'Critico') {
      return 'border-rose-500/50 bg-rose-950/30';
    }
    if (s === 'Alerta') {
      return 'border-orange-500/45 bg-orange-950/25';
    }
    if (s === 'Atenção') {
      return 'border-amber-500/40 bg-amber-950/20';
    }
    return 'border-slate-600/70 bg-slate-900/40';
  }
}
