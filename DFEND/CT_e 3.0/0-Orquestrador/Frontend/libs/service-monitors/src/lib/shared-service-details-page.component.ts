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
import { ServiceMonitorStore } from './service-monitor.store';
import {
  classifyLogKind,
  connectionHealthLabel,
  describeLogActivity,
  explainLogError,
  extractStatusCode,
  formatDataAgeSeconds,
  formatLogErrorCopyPayload,
  formatLogErrorPlainMessage,
  logKindLabel,
  severityLabel,
  summarizeLogMessage,
  tableHealthStatusLabel,
} from '@orquestrador/shared-utils';

const FEED_CAP = 12;
const EVENT_CAP = 12;
const ALERT_CAP = 8;
const TABLE_HEALTH_CAP = 8;

interface ServiceDetailsMeta {
  label: string;
  eyebrow: string;
  accentLink: string;
  emptyFeed: string;
  emptyEvents: string;
}

const META: Record<string, ServiceDetailsMeta> = {
  receptor: {
    label: 'Receptor',
    eyebrow: 'Entenda o que o Receptor está fazendo',
    accentLink: 'text-cyan-400',
    emptyFeed:
      'Ainda sem atividade. Ligue o fluxo no painel do Receptor para ver os passos aparecerem aqui.',
    emptyEvents:
      'Sem eventos no banco ainda. Eles aparecem quando o Receptor consulta a SEFAZ ou grava lotes.',
  },
  arquivador: {
    label: 'Arquivador',
    eyebrow: 'Entenda o que o Arquivador está fazendo',
    accentLink: 'text-amber-400',
    emptyFeed:
      'Ainda sem atividade. Ligue o fluxo no painel do Arquivador para ver os passos aparecerem aqui.',
    emptyEvents:
      'Sem eventos no banco ainda. Eles aparecem quando o Arquivador retira da fila ou grava lotes.',
  },
  sintetizador: {
    label: 'Sintetizador',
    eyebrow: 'Entenda o que o Sintetizador está fazendo',
    accentLink: 'text-violet-400',
    emptyFeed:
      'Ainda sem atividade. Ligue o fluxo no painel do Sintetizador para ver os passos aparecerem aqui.',
    emptyEvents:
      'Sem eventos no banco ainda. Eles aparecem quando o Sintetizador processa lotes da fila.',
  },
  analisador: {
    label: 'Analisador',
    eyebrow: 'Entenda o que o Analisador está fazendo',
    accentLink: 'text-violet-400',
    emptyFeed:
      'Ainda sem atividade. Ligue o fluxo no painel do Analisador para ver os passos aparecerem aqui.',
    emptyEvents:
      'Sem eventos no banco ainda. Eles aparecem quando o Analisador processa lotes da fila.',
  },
  integrador: {
    label: 'Integrador',
    eyebrow: 'Entenda o que o Integrador está fazendo',
    accentLink: 'text-violet-400',
    emptyFeed:
      'Ainda sem atividade. Ligue o fluxo no painel do Integrador para ver os passos aparecerem aqui.',
    emptyEvents:
      'Sem eventos no banco ainda. Eles aparecem quando o Integrador processa lotes da fila.',
  },
  carga: {
    label: 'Carga',
    eyebrow: 'Entenda o que a Carga está fazendo',
    accentLink: 'text-teal-400',
    emptyFeed:
      'Ainda sem atividade. Ligue o fluxo no painel da Carga para ver os passos aparecerem aqui.',
    emptyEvents:
      'Sem eventos no banco ainda. Eles aparecem quando a Carga processa downloads/lotes.',
  },
};

/**
 * Mais informações unificado (padrão Receptor) para todos os monitores CT-e.
 * Meta por `store.serviceId()` — uma fonte de verdade de layout/UX.
 */
@Component({
  selector: 'lib-shared-service-details-page',
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
            {{ meta().eyebrow }}
          </p>
          <h1 class="text-lg font-semibold text-slate-50">Mais informações</h1>
        </div>
        <div class="flex flex-wrap items-center gap-3">
          <a
            [routerLink]="'/monitores/' + serviceId() + '/logs'"
            class="text-xs hover:underline"
            [ngClass]="meta().accentLink"
            >Histórico →</a
          >
          <a
            [routerLink]="'/monitores/' + serviceId()"
            class="text-xs hover:underline"
            [ngClass]="meta().accentLink"
            >← Voltar ao painel</a
          >
        </div>
      </header>

      <div
        class="grid min-h-0 flex-1 grid-cols-1 grid-rows-2 gap-2 overflow-hidden lg:grid-cols-2"
      >
        <section class="details-card flex min-h-0 flex-col">
          <div class="mb-1.5 shrink-0">
            <h2 class="text-sm font-semibold text-slate-100">O que aconteceu agora</h2>
            <p class="text-[11px] leading-snug text-slate-500">
              Veja o passo mais recente — use para saber se o serviço está andando
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
              <p class="text-[11px] leading-relaxed text-slate-500">{{ meta().emptyFeed }}</p>
            }
          </div>
        </section>

        <section class="details-card flex min-h-0 flex-col">
          <div class="mb-1.5 shrink-0">
            <h2 class="text-sm font-semibold text-slate-100">Últimos eventos do banco</h2>
            <p class="text-[11px] leading-snug text-slate-500">
              Em erro ou aviso, abra a explicação — não precisa caçar no log cru
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
                @if (canOpenErrorDetail(l)) {
                  <button
                    type="button"
                    class="shrink-0 rounded border border-rose-500/50 bg-rose-950/40 px-1.5 py-0.5 text-[10px] font-semibold text-rose-200 hover:bg-rose-900/50"
                    title="Ver explicação e texto original"
                    (click)="openErrorDetail(l)"
                  >
                    {{ isErrorEvent(l) ? 'Ver erro' : 'Ver detalhes' }}
                  </button>
                }
              </div>
            } @empty {
              <p class="text-[11px] leading-relaxed text-slate-500">{{ meta().emptyEvents }}</p>
            }
          </div>
        </section>

        <section class="details-card flex min-h-0 flex-col">
          <div class="mb-1.5 flex shrink-0 flex-wrap items-center justify-between gap-2">
            <div>
              <h2 class="text-sm font-semibold text-slate-100">Saúde dos bancos</h2>
              <p class="text-[11px] leading-snug text-slate-500">
                Confira conexão e tabelas — detalhe e jornada ficam em Tabelas
              </p>
            </div>
            <div class="flex flex-wrap items-center gap-2">
              <a
                [routerLink]="'/monitores/' + serviceId() + '/tabelas/temporaria'"
                class="text-[10px] font-medium hover:underline"
                [ngClass]="meta().accentLink"
                >Ver jornada dos lotes →</a
              >
              <a
                [routerLink]="'/monitores/' + serviceId() + '/tabelas'"
                class="text-[10px] hover:underline"
                [ngClass]="meta().accentLink"
                >Tabelas →</a
              >
              <a
                [routerLink]="'/monitores/' + serviceId() + '/config'"
                class="text-[10px] hover:underline"
                [ngClass]="meta().accentLink"
                >Config →</a
              >
            </div>
          </div>
          <div class="mb-1.5 flex shrink-0 flex-wrap items-center gap-2 text-[11px]">
            <span class="text-slate-500">Conexão</span>
            <span
              class="rounded px-1.5 py-0.5 text-[10px] font-semibold"
              [ngClass]="connectionBadgeClass()"
            >
              {{ connectionLabel() }}
            </span>
          </div>
          <div class="min-h-0 flex-1 space-y-1 overflow-hidden border-t border-slate-800/80 pt-2">
            @for (card of tableHealthRows(); track card.key) {
              <a
                [routerLink]="['/monitores', serviceId(), 'tabelas', card.key]"
                class="flex items-start gap-2 rounded-md border border-transparent px-1.5 py-1 text-[11px] leading-snug transition hover:border-slate-600/80 hover:bg-slate-900/50"
                [attr.title]="card.hint"
              >
                <span
                  class="mt-0.5 shrink-0 rounded px-1 py-0.5 text-[9px] font-semibold uppercase"
                  [ngClass]="tableStatusBadge(card.status)"
                >
                  {{ tableStatusLabel(card.status) }}
                </span>
                <span class="min-w-0 flex-1">
                  <span class="block truncate font-medium text-slate-100">{{
                    card.label
                  }}</span>
                  <span class="block truncate text-slate-400">{{
                    card.primaryValue
                  }}</span>
                  <span class="text-slate-600"
                    >Idade {{ formatAge(card.dataAgeSeconds) }} · Consulta
                    {{ card.queryMs }}ms</span
                  >
                </span>
              </a>
            } @empty {
              <div class="space-y-2 text-[11px] leading-relaxed text-slate-500">
                <p>
                  Sem telemetria de tabelas ainda. Confira a connection string do monitor
                  ou abra a Temporária para ver lotes da sessão.
                </p>
                <a
                  [routerLink]="'/monitores/' + serviceId() + '/tabelas/temporaria'"
                  class="inline-block font-medium hover:underline"
                  [ngClass]="meta().accentLink"
                  >Ver jornada dos lotes →</a
                >
              </div>
            }
          </div>
        </section>

        <section class="details-card flex min-h-0 flex-col">
          <div class="mb-1.5 shrink-0">
            <h2 class="text-sm font-semibold text-slate-100">Avisos e saúde</h2>
            <p class="text-[11px] leading-snug text-slate-500">
              Priorize o que pede ação (processo parado, fila alta, batida atrasada)
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
              <p class="text-[11px] leading-relaxed text-slate-500">
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
export class SharedServiceDetailsPageComponent {
  readonly store = inject(ServiceMonitorStore);
  private readonly confirmDialog = inject(ConfirmDialogService);
  readonly severityLabel = severityLabel;
  readonly summarize = summarizeLogMessage;
  readonly tableStatusLabel = tableHealthStatusLabel;
  readonly formatAge = formatDataAgeSeconds;

  readonly serviceId = computed(() => this.store.serviceId());

  readonly meta = computed(
    () => META[this.serviceId()] ?? META['receptor']
  );

  readonly connectionLabel = computed(() =>
    connectionHealthLabel(this.store.connectionHealth() ?? 'Down')
  );

  readonly tableHealthRows = computed(() =>
    this.store.tableHealth().slice(0, TABLE_HEALTH_CAP)
  );

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

  canOpenErrorDetail(l: LogEntry): boolean {
    if (this.isErrorEvent(l)) return true;
    return !!explainLogError({
      mensagem: l.mensagem,
      cStat: l.cStat,
      severityHint: l.severityHint,
    });
  }

  openErrorDetail(l: LogEntry): void {
    const original = (l.mensagem || '').trim() || '(sem mensagem no registro)';
    const plain = explainLogError({
      mensagem: original,
      cStat: l.cStat,
      severityHint: l.severityHint,
    });
    const code = extractStatusCode(original, l.cStat);

    if (plain) {
      void this.confirmDialog.ask({
        mode: 'info',
        title: plain.title,
        message: formatLogErrorPlainMessage(plain, {
          seqLog: l.seqLog,
          cStat: code,
        }),
        detailLabel: 'Texto original (banco)',
        detail: original,
        copyText: formatLogErrorCopyPayload({
          plain,
          original,
          seqLog: l.seqLog,
          cStat: code,
        }),
        confirmLabel: 'Fechar',
        tone: 'danger',
      });
      return;
    }

    void this.confirmDialog.ask({
      mode: 'info',
      title: 'Erro original (banco)',
      message: `Evento #${l.seqLog}${code ? ` · código ${code}` : ''} — ainda sem tradução no histórico. Texto completo:`,
      detailLabel: 'Texto original (banco)',
      detail: original,
      copyText: original,
      confirmLabel: 'Fechar',
      tone: 'danger',
    });
  }

  connectionBadgeClass(): string {
    const raw = String(this.store.connectionHealth() ?? 'Down');
    const n = Number(raw);
    const key = Number.isFinite(n)
      ? (['Healthy', 'Degraded', 'Down'][n] ?? raw)
      : raw;
    switch (key) {
      case 'Healthy':
        return 'bg-emerald-950 text-emerald-300';
      case 'Degraded':
        return 'bg-amber-950 text-amber-300';
      case 'Down':
        return 'bg-rose-950 text-rose-300';
      default:
        return 'bg-slate-800 text-slate-300';
    }
  }

  tableStatusBadge(status: string): string {
    switch ((status ?? '').toLowerCase()) {
      case 'critico':
        return 'bg-rose-950 text-rose-300';
      case 'atencao':
        return 'bg-amber-950 text-amber-300';
      case 'ok':
        return 'bg-emerald-950 text-emerald-300';
      default:
        return 'bg-slate-800 text-slate-300';
    }
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
