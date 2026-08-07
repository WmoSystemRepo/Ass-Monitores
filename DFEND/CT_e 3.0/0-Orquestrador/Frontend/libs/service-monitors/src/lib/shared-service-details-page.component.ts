import {
  ChangeDetectionStrategy,
  Component,
  OnDestroy,
  computed,
  effect,
  inject,
} from '@angular/core';
import { DatePipe, NgClass } from '@angular/common';
import { RouterLink } from '@angular/router';
import { ConfirmDialogService } from '@orquestrador/shared-ui';
import { LogEntry } from '@orquestrador/shared-data';
import { PresentationTourStore } from '@orquestrador/monitor-dashboard';
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
const TABLE_HEALTH_CAP = 8;

/** Catálogo fixo das tabelas consultáveis (GET …/tables/{key}?take=1000). */
const BROWSEABLE_TABLES: ReadonlyArray<{
  key: string;
  label: string;
  sqlName: string;
  hint: string;
  primaryValue: string;
}> = [
  {
    key: 'servico',
    label: 'Serviço (NSU)',
    sqlName: 'servico_*_conhecimento_transporte_eletronico',
    hint: 'Tabela de serviço — NSU e batida do processo',
    primaryValue: 'NSU, servidor e última execução',
  },
  {
    key: 'configuracao',
    label: 'Configuração',
    sqlName: 'configuracao_*_conhecimento_transporte_eletronico',
    hint: 'Parâmetros ativos (sts_ativo=1)',
    primaryValue: 'Chaves e valores de configuração',
  },
  {
    key: 'temporaria',
    label: 'Temporária',
    sqlName: 'tmp_documento_conhecimento_transporte_eletronico',
    hint: 'Lotes em trânsito neste serviço',
    primaryValue: 'Documentos recentes na temporária',
  },
  {
    key: 'log',
    label: 'Log',
    sqlName: 'log_*_conhecimento_transporte_eletronico',
    hint: 'Eventos gravados no banco',
    primaryValue: 'Eventos e mensagens recentes',
  },
  {
    key: 'fila',
    label: 'Fila',
    sqlName: 'Service Broker / fila do próximo serviço',
    hint: 'Profundidade da fila Service Broker',
    primaryValue: 'Estado da fila e tendência',
  },
];


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
      <header
        class="flex shrink-0 flex-wrap items-center justify-between gap-2"
        data-tour="details-header"
      >
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
        <section class="details-card flex min-h-0 flex-col" data-tour="details-feed">
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
              <p class="text-xs leading-relaxed text-slate-500">{{ meta().emptyFeed }}</p>
            }
          </div>
        </section>

        <section class="details-card flex min-h-0 flex-col" data-tour="details-events">
          <div class="mb-1.5 shrink-0">
            <h2 class="text-sm font-semibold text-slate-100">Últimos eventos do banco</h2>
            <p class="text-[11px] text-slate-500">
              Sucesso, aviso ou erro. Em erro/aviso mapeado, abra a explicação e o texto
              original.
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
              <p class="text-xs leading-relaxed text-slate-500">{{ meta().emptyEvents }}</p>
            }
          </div>
        </section>

        <section class="details-card flex min-h-0 flex-col" data-tour="details-db-health">
          <div class="mb-1.5 flex shrink-0 flex-wrap items-center justify-between gap-2">
            <div>
              <h2 class="text-sm font-semibold text-slate-100">Saúde dos bancos</h2>
              <p class="text-[11px] text-slate-500">
                Conexão SQL · abra uma tabela para ver até os últimos 1000 registros
              </p>
            </div>
            <div class="flex flex-wrap items-center gap-2">
              <a
                [routerLink]="'/monitores/' + serviceId() + '/tabelas'"
                class="text-[10px] hover:underline"
                [ngClass]="meta().accentLink"
                data-tour="nav-tabelas"
                >Tabelas →</a
              >
              <a
                [routerLink]="'/monitores/' + serviceId() + '/config'"
                class="text-[10px] hover:underline"
                [ngClass]="meta().accentLink"
                >Configuração →</a
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
            @if (!hasLiveTableHealth()) {
              <span class="text-[10px] text-slate-500"
                >· catálogo padrão (status ao vivo ainda não chegou)</span
              >
            }
          </div>
          <div
            class="min-h-0 flex-1 overflow-y-auto border-t border-slate-800/80 pt-2"
          >
            <div class="grid grid-cols-1 gap-1.5 sm:grid-cols-2">
              @for (card of tableBrowseCards(); track card.key) {
                <a
                  [routerLink]="['/monitores', serviceId(), 'tabelas', card.key]"
                  class="group flex min-h-0 flex-col rounded-md border px-2.5 py-2 transition hover:border-cyan-500/45 hover:bg-slate-900/70"
                  [ngClass]="tableMiniBorder(card.status)"
                  [attr.title]="card.hint"
                >
                  <div class="flex items-start justify-between gap-1">
                    <span class="truncate text-[11px] font-semibold text-slate-100">{{
                      card.label
                    }}</span>
                    <span
                      class="shrink-0 rounded px-1 py-0.5 text-[9px] font-semibold uppercase"
                      [ngClass]="tableStatusBadge(card.status)"
                    >
                      {{ tableStatusLabel(card.status) }}
                    </span>
                  </div>
                  <p
                    class="mt-0.5 truncate font-mono text-[9px] text-slate-500"
                    [attr.title]="card.sqlName"
                  >
                    {{ card.sqlName }}
                  </p>
                  <p class="mt-1 line-clamp-2 text-[10px] leading-snug text-slate-400">
                    {{ card.primaryValue }}
                  </p>
                  @if (card.hasTelemetry) {
                    <p class="mt-1 text-[9px] text-slate-600">
                      Idade {{ formatAge(card.dataAgeSeconds) }} · Consulta
                      {{ card.queryMs }}ms
                    </p>
                  }
                  <p
                    class="mt-auto pt-1.5 text-[10px] font-medium"
                    [ngClass]="meta().accentLink"
                  >
                    Ver últimos 1000 →
                  </p>
                </a>
              }
            </div>
          </div>
        </section>

        <section class="details-card flex min-h-0 flex-col" data-tour="details-alerts">
          <div class="mb-1.5 shrink-0">
            <h2 class="text-sm font-semibold text-slate-100">Avisos e saúde</h2>
            <p class="text-[11px] text-slate-500">
              Todos os status: ok/info, atenção e alerta (processo, fila, batida, SQL)
            </p>
          </div>
          <div class="min-h-0 flex-1 space-y-1.5 overflow-y-auto">
            @for (a of allHealthNotices(); track a.code + a.message) {
              <div
                class="rounded-md border px-2.5 py-1.5 text-[11px]"
                [ngClass]="alertBorder(a.severity, a.code)"
              >
                <span [ngClass]="alertSeverityTone(a.severity, a.code)"
                  >{{ severityLabel(a.severity) }}
                  @if (a.code) {
                    · {{ a.code }}
                  }
                </span>
                <p class="mt-0.5 text-slate-100">{{ a.message }}</p>
              </div>
            } @empty {
              <p class="text-xs leading-relaxed text-slate-500">
                Sem telemetria de avisos ainda — aguarde o snapshot do monitor.
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
export class SharedServiceDetailsPageComponent implements OnDestroy {
  readonly store = inject(ServiceMonitorStore);
  readonly tour = inject(PresentationTourStore);
  private readonly confirmDialog = inject(ConfirmDialogService);
  readonly severityLabel = severityLabel;
  readonly summarize = summarizeLogMessage;
  readonly tableStatusLabel = tableHealthStatusLabel;
  readonly formatAge = formatDataAgeSeconds;

  private lastErrorModalToken = 0;

  constructor() {
    effect(() => {
      const sim = this.tour.simulation();
      if (sim?.mode !== 'detailsFlow' || !sim.details) {
        if (this.lastErrorModalToken > 0) {
          this.confirmDialog.close(false);
        }
        this.lastErrorModalToken = 0;
        return;
      }
      const token = sim.details.errorModalToken;
      if (token === this.lastErrorModalToken) return;
      this.lastErrorModalToken = token;

      if (token <= 0) return;

      // Ímpar = abrir modal do erro crítico; par = fechar.
      if (token % 2 === 1) {
        const critical = sim.details.logs.find(
          (l) => l.seqLog === sim.details!.criticalErrorSeqLog
        );
        if (critical) this.openErrorDetail(critical);
      } else {
        this.confirmDialog.close(false);
      }
    });
  }

  ngOnDestroy(): void {
    if (this.tour.isDetailsSimulating()) {
      this.confirmDialog.close(false);
    }
  }

  readonly serviceId = computed(() => this.store.serviceId());

  readonly meta = computed(
    () => META[this.serviceId()] ?? META['receptor']
  );

  readonly connectionLabel = computed(() =>
    connectionHealthLabel(this.store.connectionHealth() ?? 'Down')
  );

  readonly hasLiveTableHealth = computed(() => {
    if (this.tour.isDetailsSimulating()) return true;
    return (this.store.tableHealth()?.length ?? 0) > 0;
  });

  /**
   * Mini cards sempre disponíveis: com telemetria usa status real;
   * sem telemetria ainda permite abrir os últimos 1000 registros.
   */
  readonly tableBrowseCards = computed(() => {
    const live =
      this.tour.simulation()?.mode === 'detailsFlow'
        ? this.tour.simulation()?.details?.tableHealth ?? []
        : this.store.tableHealth() ?? [];
    const byKey = new Map(live.map((t) => [t.key.toLowerCase(), t]));

    const fromCatalog = BROWSEABLE_TABLES.map((t) => {
      const hit = byKey.get(t.key);
      if (hit) {
        return {
          key: hit.key,
          label: hit.label || t.label,
          sqlName: t.sqlName,
          status: hit.status || 'ok',
          primaryValue: hit.primaryValue || t.primaryValue,
          hint: hit.hint || t.hint,
          dataAgeSeconds: hit.dataAgeSeconds,
          queryMs: hit.queryMs ?? 0,
          hasTelemetry: true,
        };
      }
      return {
        key: t.key,
        label: t.label,
        sqlName: t.sqlName,
        status: 'ok',
        primaryValue: t.primaryValue,
        hint: t.hint,
        dataAgeSeconds: null as number | null,
        queryMs: 0,
        hasTelemetry: false,
      };
    });

    const known = new Set(BROWSEABLE_TABLES.map((t) => t.key));
    const extras = live
      .filter((t) => !known.has(t.key.toLowerCase()) && !known.has(t.key))
      .slice(0, Math.max(0, TABLE_HEALTH_CAP - fromCatalog.length))
      .map((hit) => ({
        key: hit.key,
        label: hit.label,
        sqlName: hit.key,
        status: hit.status || 'ok',
        primaryValue: hit.primaryValue,
        hint: hit.hint,
        dataAgeSeconds: hit.dataAgeSeconds,
        queryMs: hit.queryMs ?? 0,
        hasTelemetry: true,
      }));

    return [...fromCatalog, ...extras].slice(0, TABLE_HEALTH_CAP);
  });

  tableMiniBorder(status: string): string {
    switch ((status ?? '').toLowerCase()) {
      case 'critico':
        return 'border-rose-500/45 bg-rose-950/20';
      case 'atencao':
        return 'border-amber-500/40 bg-amber-950/15';
      default:
        return 'border-slate-700/80 bg-slate-900/45';
    }
  }

  readonly recentActivity = computed(() => {
    const logs =
      this.tour.simulation()?.mode === 'detailsFlow'
        ? this.tour.simulation()?.details?.logs ?? []
        : this.store.logs();
    return [...logs].slice(-EVENT_CAP).reverse();
  });

  readonly allHealthNotices = computed(() => {
    const rank = (severity: string | number): number => {
      const s = severityLabel(severity);
      if (s === 'Crítico' || s === 'Critico') return 0;
      if (s === 'Alerta') return 1;
      if (s === 'Atenção') return 2;
      return 3;
    };
    const alerts =
      this.tour.simulation()?.mode === 'detailsFlow'
        ? this.tour.simulation()?.details?.alerts ?? []
        : this.store.alerts();
    return [...alerts].sort((a, b) => rank(a.severity) - rank(b.severity));
  });

  readonly topAlerts = this.allHealthNotices;

  readonly flowFeed = computed(() => {
    const simDetails =
      this.tour.simulation()?.mode === 'detailsFlow'
        ? this.tour.simulation()?.details
        : null;
    const liveTrace = simDetails?.liveTrace ?? this.store.liveTrace();
    const logs = simDetails?.logs ?? this.store.logs();

    const debug = [...liveTrace]
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

    const sql = [...logs]
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

  alertBorder(severity: string | number, code?: string | null): string {
    const s = severityLabel(severity);
    const c = (code ?? '').toUpperCase();
    if (s === 'Crítico' || s === 'Critico') {
      return 'border-rose-500/50 bg-rose-950/30';
    }
    if (s === 'Alerta') {
      return 'border-orange-500/45 bg-orange-950/25';
    }
    if (s === 'Atenção') {
      return 'border-amber-500/40 bg-amber-950/20';
    }
    if (c.endsWith('_OK') || c.endsWith('_EMPTY') || c === 'OK' || c === 'PROC_ON') {
      return 'border-emerald-500/35 bg-emerald-950/20';
    }
    return 'border-sky-600/40 bg-sky-950/20';
  }

  alertSeverityTone(severity: string | number, code?: string | null): string {
    const s = severityLabel(severity);
    const c = (code ?? '').toUpperCase();
    if (s === 'Crítico' || s === 'Critico') return 'text-rose-300';
    if (s === 'Alerta') return 'text-orange-300';
    if (s === 'Atenção') return 'text-amber-300';
    if (c.endsWith('_OK') || c.endsWith('_EMPTY') || c === 'OK' || c === 'PROC_ON') {
      return 'text-emerald-300';
    }
    return 'text-sky-300';
  }
}
