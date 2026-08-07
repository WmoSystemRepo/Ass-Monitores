import {
  ChangeDetectionStrategy,
  Component,
  DestroyRef,
  computed,
  inject,
  signal,
} from '@angular/core';
import { DatePipe, NgClass } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { takeUntilDestroyed, toSignal } from '@angular/core/rxjs-interop';
import { map, switchMap, timer, catchError, of } from 'rxjs';
import {
  ConfigDetailRow,
  LogEntry,
  RecentDocument,
  TableDetailDto,
} from '@orquestrador/shared-data';
import {
  formatDataAgeSeconds,
  summarizeLogMessage,
  tableHealthStatusLabel,
} from '@orquestrador/shared-utils';
import { ServiceMonitorStore } from '../service-monitor.store';
import { ServiceMonitorApiService } from '../service-monitor-api.service';
import { journeyForService, situacaoLote } from '../cte-journey';
import {
  MonitorDataGridComponent,
  MonitorGridColumn,
} from '../monitor-data-grid.component';

const TABLE_KEYS = ['servico', 'configuracao', 'temporaria', 'log', 'fila'] as const;
const TABLE_DETAIL_TAKE = 1000;

interface TempRowView extends RecentDocument {
  origem: string;
  estagio: string;
  proximo: string;
  situacao: string;
}

function fmtDate(v?: string | null): string {
  if (!v) return '—';
  const d = new Date(v);
  if (Number.isNaN(d.getTime())) return '—';
  const dd = String(d.getDate()).padStart(2, '0');
  const mm = String(d.getMonth() + 1).padStart(2, '0');
  const hh = String(d.getHours()).padStart(2, '0');
  const mi = String(d.getMinutes()).padStart(2, '0');
  const ss = String(d.getSeconds()).padStart(2, '0');
  return `${dd}/${mm} ${hh}:${mi}:${ss}`;
}

@Component({
  selector: 'lib-tables-hub-page',
  standalone: true,
  imports: [RouterLink, NgClass],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <section class="flex flex-col gap-4" data-tour="tables-hub">
      <header>
        <h1 class="text-2xl font-semibold text-slate-50">Tabelas do banco</h1>
        <p class="text-sm text-slate-400">
          Vigilância das tabelas da sessão atual — clique para ver os dados.
        </p>
      </header>
      <div class="grid gap-3 sm:grid-cols-2 lg:grid-cols-3">
        @for (card of cards(); track card.key) {
          <a
            [routerLink]="card.route"
            class="rounded-lg border border-slate-700 bg-slate-900/50 p-4 transition hover:border-cyan-500/40"
            [attr.data-tour]="card.key === 'servico' ? 'nav-table-detail' : null"
          >
            <div class="flex items-center justify-between">
              <h2 class="font-medium text-slate-100">{{ card.label }}</h2>
              <span
                class="rounded px-2 py-0.5 text-[10px] font-semibold uppercase"
                [ngClass]="badgeClass(card.status)"
              >
                {{ statusLabel(card.status) }}
              </span>
            </div>
            <p class="mt-2 text-sm text-slate-300">{{ card.primaryValue }}</p>
            <p class="mt-2 text-xs text-slate-500">{{ card.hint }}</p>
            <p class="mt-3 text-xs text-cyan-400">Ver dados →</p>
          </a>
        } @empty {
          @for (sample of emptySamples; track sample.key) {
            <a
              [routerLink]="sample.key"
              class="rounded-lg border border-dashed border-slate-600 bg-slate-900/30 p-4 transition hover:border-cyan-500/40"
              [attr.data-tour]="sample.key === 'servico' ? 'nav-table-detail' : null"
            >
              <div class="flex items-center justify-between">
                <h2 class="font-medium text-slate-100">{{ sample.label }}</h2>
                <span
                  class="rounded px-2 py-0.5 text-[10px] font-semibold uppercase"
                  [ngClass]="badgeClass(sample.status)"
                >
                  {{ statusLabel(sample.status) }}
                </span>
              </div>
              <p class="mt-2 text-sm text-slate-400">{{ sample.hint }}</p>
              <p class="mt-3 text-xs text-cyan-400">Ver dados →</p>
            </a>
          }
        }
      </div>
    </section>
  `,
})
export class TablesHubPageComponent {
  private readonly store = inject(ServiceMonitorStore);
  readonly cards = computed(() => this.store.tableHealth());

  /** Exemplos visíveis quando ainda não há telemetria — status + Ver dados. */
  readonly emptySamples = [
    {
      key: 'servico',
      label: 'Serviço',
      status: 'ok',
      hint: 'Exemplo — status ok quando a tabela responde.',
    },
    {
      key: 'log',
      label: 'Log',
      status: 'atencao',
      hint: 'Exemplo — status atenção quando algo precisa de olho.',
    },
  ] as const;

  statusLabel(s: string): string {
    return tableHealthStatusLabel(s);
  }

  badgeClass(status: string): string {
    switch ((status ?? '').toLowerCase()) {
      case 'critico':
        return 'bg-rose-950 text-rose-300';
      case 'atencao':
        return 'bg-amber-950 text-amber-300';
      default:
        return 'bg-emerald-950 text-emerald-300';
    }
  }
}

@Component({
  selector: 'lib-table-detail-page',
  standalone: true,
  imports: [DatePipe, NgClass, RouterLink, MonitorDataGridComponent],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <section class="flex flex-col gap-4" data-tour="table-detail">
      <header class="flex flex-wrap items-end justify-between gap-3">
        <div>
          <a routerLink=".." class="text-xs text-cyan-400 hover:underline">← Tabelas</a>
          <h1 class="mt-1 text-2xl font-semibold text-slate-50">
            {{ detail()?.label || keyLabel() }}
          </h1>
          <p class="text-sm text-slate-400">
            Dados da última sessão de recepção
            @if (detail()?.sessionStartUtc; as start) {
              · desde {{ start | date: 'dd/MM HH:mm:ss' }}
            }
          </p>
        </div>
        <div class="flex flex-wrap items-center gap-2">
          <a
            routerLink="../../threads"
            class="rounded border border-slate-600 bg-slate-900/60 px-2.5 py-1 text-[11px] font-medium text-cyan-300 hover:bg-slate-800"
            data-tour="nav-threads"
          >
            Linhas de trabalho →
          </a>
          @if (health(); as h) {
            <div class="text-right text-xs text-slate-400">
              <span
                class="rounded px-2 py-1 text-[10px] font-semibold uppercase"
                [ngClass]="badgeClass(h.status)"
              >
                {{ statusLabel(h.status) }}
              </span>
              <p class="mt-1">Idade {{ formatAge(h.dataAgeSeconds) }} · Consulta {{ h.queryMs }}ms</p>
            </div>
          }
        </div>
      </header>

      @if (error(); as err) {
        <div class="rounded border border-rose-500/40 bg-rose-950/30 px-3 py-2 text-sm text-rose-200">
          {{ err }}
        </div>
      }

      @if (detail()?.bannerMessage; as banner) {
        <div class="rounded border border-amber-500/30 bg-amber-950/20 px-3 py-2 text-sm text-amber-100">
          {{ banner }}
        </div>
      }

      @if (health(); as h) {
        <div class="rounded border border-slate-700 bg-slate-900/40 px-3 py-2 text-sm text-slate-300">
          <span class="font-medium text-slate-100">{{ h.primaryValue }}</span>
          <span class="ml-2 text-slate-500">{{ h.hint }}</span>
        </div>
      }

      <!-- Serviço -->
      @if (key() === 'servico') {
        <div class="overflow-auto rounded border border-slate-700">
          <table class="min-w-full text-left text-sm">
            <thead class="sticky top-0 bg-slate-900 text-xs uppercase text-slate-500">
              <tr>
                <th class="px-3 py-2">Serviço</th>
                <th class="px-3 py-2">Servidor</th>
                <th class="px-3 py-2">Posição da busca (NSU)</th>
                <th class="px-3 py-2">Última batida</th>
                <th class="px-3 py-2">Atualização</th>
              </tr>
            </thead>
            <tbody>
              @for (r of detail()?.serviceRows ?? []; track r.nsu) {
                <tr class="border-t border-slate-800">
                  <td class="px-3 py-2">{{ r.desServico || '—' }}</td>
                  <td class="px-3 py-2">{{ r.nomServidor || '—' }}</td>
                  <td class="px-3 py-2 font-mono text-cyan-300">{{ r.nsu || '—' }}</td>
                  <td class="px-3 py-2">{{ r.dtcExecucao | date: 'dd/MM HH:mm:ss' }}</td>
                  <td class="px-3 py-2">{{ r.dtcAtualizacao | date: 'dd/MM HH:mm:ss' }}</td>
                </tr>
              } @empty {
                <tr>
                  <td colspan="5" class="px-3 py-4 text-slate-500">Sem linha de serviço.</td>
                </tr>
              }
            </tbody>
          </table>
        </div>
        <h2 class="text-sm font-medium text-slate-300">Eventos da sessão (NSU / cStat)</h2>
        <div class="max-h-[40vh] overflow-auto rounded border border-slate-700 bg-slate-950/60 p-3">
          @for (l of detail()?.contextLogs ?? []; track l.seqLog) {
            <div class="border-b border-slate-800/80 py-2 text-xs last:border-0">
              <span class="text-slate-500">{{ l.dtcLog | date: 'HH:mm:ss' }}</span>
              @if (l.cStat) {
                <span class="ml-2 rounded bg-slate-800 px-1 text-cyan-300">cStat {{ l.cStat }}</span>
              }
              <p class="mt-0.5 text-slate-300">{{ summarize(l.mensagem, l.cStat) }}</p>
            </div>
          } @empty {
            <p class="text-sm text-slate-500">Nenhum evento NSU/cStat nesta sessão.</p>
          }
        </div>
      }

      <!-- Configuração -->
      @if (key() === 'configuracao') {
        <lib-monitor-data-grid
          [rows]="configRows()"
          [columns]="configColumns"
          [takeApplied]="detail()?.takeApplied ?? 1000"
          [emptyMessage]="'Sem configurações do processo.'"
          [rowTrackId]="configTrackId"
        />
      }

      <!-- Temporária -->
      @if (key() === 'temporaria') {
        <lib-monitor-data-grid
          [rows]="tempRows()"
          [columns]="tempColumns"
          [takeApplied]="detail()?.takeApplied ?? 1000"
          [emptyMessage]="'Nenhum lote nesta sessão. Acompanhe a jornada em Mais informações.'"
          [rowTrackId]="tempTrackId"
        />
      }

      <!-- Log -->
      @if (key() === 'log') {
        <lib-monitor-data-grid
          [rows]="logRows()"
          [columns]="logColumns"
          [takeApplied]="detail()?.takeApplied ?? 1000"
          [emptyMessage]="'Nenhum log nesta sessão.'"
          [rowTrackId]="logTrackId"
        />
      }

      <!-- Fila -->
      @if (key() === 'fila') {
        @if (detail()?.fila; as fila) {
          <div class="grid gap-3 sm:grid-cols-3">
            <div class="rounded border border-slate-700 bg-slate-900/40 p-4">
              <p class="text-xs text-slate-500">Na fila agora</p>
              <p class="mt-1 text-2xl font-semibold text-slate-100">{{ fila.depth }}</p>
            </div>
            <div class="rounded border border-slate-700 bg-slate-900/40 p-4">
              <p class="text-xs text-slate-500">Limite de alerta</p>
              <p class="mt-1 text-2xl font-semibold text-slate-100">{{ fila.highThreshold }}</p>
            </div>
            <div class="rounded border border-slate-700 bg-slate-900/40 p-4">
              <p class="text-xs text-slate-500">Tendência</p>
              <p class="mt-1 text-sm text-slate-200">{{ fila.trendHint }}</p>
            </div>
          </div>
          <div class="rounded border border-slate-700 bg-slate-900/40 p-4">
            <p class="text-xs text-slate-500">Últimos ciclos (profundidade)</p>
            <p class="mt-2 font-mono text-sm text-cyan-300">
              {{ fila.depthTrend.join(' → ') || '—' }}
            </p>
            <p class="mt-3 text-xs text-slate-500">
              A fila não lista mensagens individuais — só a quantidade aguardando o próximo
              serviço.
            </p>
          </div>
        }
      }
    </section>
  `,
})
export class TableDetailPageComponent {
  private readonly route = inject(ActivatedRoute);
  private readonly api = inject(ServiceMonitorApiService);
  private readonly store = inject(ServiceMonitorStore);
  private readonly destroyRef = inject(DestroyRef);

  readonly key = toSignal(
    this.route.paramMap.pipe(map((p) => (p.get('key') ?? '').toLowerCase())),
    { initialValue: '' }
  );

  readonly detail = signal<TableDetailDto | null>(null);
  readonly error = signal<string | null>(null);

  readonly health = computed(() => this.detail()?.health ?? null);

  readonly queueDepth = computed(
    () => this.store.queues()?.serviceBrokerDepth ?? this.detail()?.fila?.depth ?? 0
  );

  readonly tempRows = computed((): TempRowView[] => {
    const rows = this.detail()?.tempRows ?? [];
    const j = journeyForService(this.store.serviceId());
    const depth = this.queueDepth();
    return rows.map((r) => ({
      ...r,
      origem: j.origem,
      estagio: j.estagio,
      proximo: j.proximo,
      situacao: situacaoLote(r.hasError, depth),
    }));
  });

  readonly configRows = computed(() => this.detail()?.configRows ?? []);
  readonly logRows = computed(() => this.detail()?.logRows ?? []);

  readonly tempColumns: MonitorGridColumn<TempRowView>[] = [
    {
      id: 'nsu',
      header: 'NSU',
      filterable: true,
      filterPlaceholder: 'NSU',
      mono: true,
      value: (r) => String(r.nsu),
      rowHighlight: (r) => r.hasError,
    },
    {
      id: 'nsuFinal',
      header: 'NSU final',
      filterable: true,
      filterPlaceholder: 'Final',
      mono: true,
      value: (r) => (r.nsuFinal != null ? String(r.nsuFinal) : '—'),
      rowHighlight: (r) => r.hasError,
    },
    {
      id: 'qtd',
      header: 'Qtd',
      filterable: true,
      filterPlaceholder: 'Qtd',
      value: (r) => String(r.qtdDocumento),
      rowHighlight: (r) => r.hasError,
    },
    {
      id: 'doc',
      header: 'Documento',
      filterable: true,
      filterPlaceholder: 'Data',
      value: (r) => fmtDate(r.dtcDocumento),
      rowHighlight: (r) => r.hasError,
    },
    {
      id: 'atualizacao',
      header: 'Atualização',
      filterable: true,
      filterPlaceholder: 'Data',
      value: (r) => fmtDate(r.dtcAtualizacao),
      rowHighlight: (r) => r.hasError,
    },
    {
      id: 'origem',
      header: 'Origem',
      filterable: true,
      value: (r) => r.origem,
      rowHighlight: (r) => r.hasError,
    },
    {
      id: 'estagio',
      header: 'Estágio atual',
      filterable: true,
      value: (r) => r.estagio,
      rowHighlight: (r) => r.hasError,
    },
    {
      id: 'proximo',
      header: 'Próximo',
      filterable: true,
      value: (r) => r.proximo,
      rowHighlight: (r) => r.hasError,
    },
    {
      id: 'situacao',
      header: 'Situação',
      filterable: true,
      value: (r) => r.situacao,
      rowHighlight: (r) => r.hasError,
    },
    {
      id: 'erro',
      header: 'Erro',
      filterable: true,
      filterPlaceholder: 'Erro',
      value: (r) => r.mensagemErro || '—',
      cellClass: (r) => (r.hasError ? 'text-rose-300' : ''),
      rowHighlight: (r) => r.hasError,
    },
  ];

  readonly configColumns: MonitorGridColumn<ConfigDetailRow>[] = [
    {
      id: 'key',
      header: 'Chave',
      filterable: true,
      filterPlaceholder: 'Chave',
      value: (r) => r.key,
    },
    {
      id: 'value',
      header: 'Valor',
      filterable: true,
      filterPlaceholder: 'Valor',
      mono: true,
      value: (r) => r.value,
    },
    {
      id: 'atualizado',
      header: 'Atualizado',
      filterable: true,
      value: (r) => fmtDate(r.dtcAtualizacao),
    },
  ];

  readonly logColumns: MonitorGridColumn<LogEntry>[] = [
    {
      id: 'data',
      header: 'Data',
      filterable: true,
      filterPlaceholder: 'Data',
      value: (r) => fmtDate(r.dtcLog),
    },
    {
      id: 'thread',
      header: 'Linha',
      filterable: true,
      filterPlaceholder: 'Thread',
      value: (r) => (r.threadId != null ? String(r.threadId) : '—'),
    },
    {
      id: 'cStat',
      header: 'cStat',
      filterable: true,
      filterPlaceholder: 'cStat',
      mono: true,
      value: (r) => r.cStat || '—',
    },
    {
      id: 'mensagem',
      header: 'Mensagem',
      filterable: true,
      filterPlaceholder: 'Texto',
      value: (r) => summarizeLogMessage(r.mensagem, r.cStat),
    },
  ];

  readonly tempTrackId = (r: TempRowView) => `${r.nsu}-${r.nsuFinal ?? ''}-${r.dtcAtualizacao ?? ''}`;
  readonly configTrackId = (r: ConfigDetailRow) => r.key;
  readonly logTrackId = (r: LogEntry) => r.seqLog;

  constructor() {
    this.route.paramMap
      .pipe(
        map((p) => (p.get('key') ?? '').toLowerCase()),
        switchMap((key) =>
          timer(0, 2000).pipe(
            switchMap(() =>
              this.api.tableDetail(key, TABLE_DETAIL_TAKE).pipe(
                map((d) => ({ key, d, err: null as string | null })),
                catchError((err) =>
                  of({
                    key,
                    d: null as TableDetailDto | null,
                    err:
                      err instanceof Error
                        ? err.message
                        : 'Falha ao carregar dados da tabela',
                  })
                )
              )
            )
          )
        ),
        takeUntilDestroyed(this.destroyRef)
      )
      .subscribe(({ key, d, err }) => {
        if (!TABLE_KEYS.includes(key as (typeof TABLE_KEYS)[number])) {
          this.error.set(`Tabela "${key}" não existe.`);
          this.detail.set(null);
          return;
        }
        if (err) {
          this.error.set(err);
          return;
        }
        this.error.set(null);
        this.detail.set(d);
      });
  }

  keyLabel(): string {
    const k = this.key();
    const labels: Record<string, string> = {
      servico: 'Serviço (NSU)',
      configuracao: 'Configuração',
      temporaria: 'Temporária',
      log: 'Log',
      fila: 'Fila',
    };
    return labels[k] ?? k;
  }

  statusLabel(s: string): string {
    return tableHealthStatusLabel(s);
  }

  formatAge(sec?: number | null): string {
    return formatDataAgeSeconds(sec);
  }

  summarize(msg?: string | null, cStat?: string | null): string {
    return summarizeLogMessage(msg, cStat);
  }

  badgeClass(status: string): string {
    switch ((status ?? '').toLowerCase()) {
      case 'critico':
        return 'bg-rose-950 text-rose-300';
      case 'atencao':
        return 'bg-amber-950 text-amber-300';
      default:
        return 'bg-emerald-950 text-emerald-300';
    }
  }
}
