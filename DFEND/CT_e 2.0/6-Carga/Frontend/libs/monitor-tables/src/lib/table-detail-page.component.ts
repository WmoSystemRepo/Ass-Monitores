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
import { MonitorApiService, CargaMonitorStore } from '@Carga/monitor-core';
import { TableDetailDto } from '@Carga/shared-data';
import {
  formatDataAgeSeconds,
  summarizeLogMessage,
  tableHealthStatusLabel,
} from '@Carga/shared-utils';

/** Keys alinhadas ao BFF (TableHealthBuilder): servico, config, tmp, log, fila.
 * Aliases legado (configuracao/temporaria) aceitos para rotas antigas. */
const TABLE_KEYS = [
  'servico',
  'config',
  'tmp',
  'log',
  'fila',
  'configuracao',
  'temporaria',
] as const;

@Component({
  selector: 'lib-tables-hub-page',
  standalone: true,
  imports: [RouterLink, NgClass],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <section class="flex flex-col gap-4">
      <header>
        <h1 class="text-2xl font-semibold text-zinc-50">Tabelas do banco</h1>
        <p class="text-sm text-zinc-400">
          Vigilância das tabelas da sessão atual — clique para ver os dados.
        </p>
      </header>
      <div class="grid gap-3 sm:grid-cols-2 lg:grid-cols-3">
        @for (card of cards(); track card.key) {
          <a
            [routerLink]="card.route"
            class="rounded-lg border border-zinc-700 bg-zinc-900/50 p-4 transition hover:border-teal-500/40"
          >
            <div class="flex items-center justify-between">
              <h2 class="font-medium text-zinc-100">{{ card.label }}</h2>
              <span
                class="rounded px-2 py-0.5 text-[10px] font-semibold uppercase"
                [ngClass]="badgeClass(card.status)"
              >
                {{ statusLabel(card.status) }}
              </span>
            </div>
            <p class="mt-2 text-sm text-zinc-300">{{ card.primaryValue }}</p>
            <p class="mt-2 text-xs text-zinc-500">{{ card.hint }}</p>
            <p class="mt-3 text-xs text-teal-400">Ver dados →</p>
          </a>
        } @empty {
          <p class="text-sm text-zinc-500">Aguardando snapshot do monitor…</p>
        }
      </div>
    </section>
  `,
})
export class TablesHubPageComponent {
  private readonly store = inject(CargaMonitorStore);
  readonly cards = computed(() => this.store.tableHealth());

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
        return 'bg-teal-950 text-teal-300';
    }
  }
}

@Component({
  selector: 'lib-table-detail-page',
  standalone: true,
  imports: [DatePipe, NgClass, RouterLink],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <section class="flex flex-col gap-4">
      <header class="flex flex-wrap items-end justify-between gap-3">
        <div>
          <a routerLink="/tabelas" class="text-xs text-teal-400 hover:underline">← Tabelas</a>
          <h1 class="mt-1 text-2xl font-semibold text-zinc-50">
            {{ detail()?.label || keyLabel() }}
          </h1>
          <p class="text-sm text-zinc-400">
            Dados da última sessão de carga
            @if (detail()?.sessionStartUtc; as start) {
              · desde {{ start | date: 'dd/MM HH:mm:ss' }}
            }
          </p>
        </div>
        @if (health(); as h) {
          <div class="text-right text-xs text-zinc-400">
            <span
              class="rounded px-2 py-1 text-[10px] font-semibold uppercase"
              [ngClass]="badgeClass(h.status)"
            >
              {{ statusLabel(h.status) }}
            </span>
            <p class="mt-1">Idade {{ formatAge(h.dataAgeSeconds) }} · Consulta {{ h.queryMs }}ms</p>
          </div>
        }
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
        <div class="rounded border border-zinc-700 bg-zinc-900/40 px-3 py-2 text-sm text-zinc-300">
          <span class="font-medium text-zinc-100">{{ h.primaryValue }}</span>
          <span class="ml-2 text-zinc-500">{{ h.hint }}</span>
        </div>
      }

      <!-- Serviço -->
      @if (key() === 'servico') {
        <div class="overflow-auto rounded border border-zinc-700">
          <table class="min-w-full text-left text-sm">
            <thead class="bg-zinc-900 text-xs uppercase text-zinc-500">
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
                <tr class="border-t border-zinc-800">
                  <td class="px-3 py-2">{{ r.desServico || '—' }}</td>
                  <td class="px-3 py-2">{{ r.nomServidor || '—' }}</td>
                  <td class="px-3 py-2 font-mono text-teal-300">{{ r.nsu || '—' }}</td>
                  <td class="px-3 py-2">{{ r.dtcExecucao | date: 'dd/MM HH:mm:ss' }}</td>
                  <td class="px-3 py-2">{{ r.dtcAtualizacao | date: 'dd/MM HH:mm:ss' }}</td>
                </tr>
              } @empty {
                <tr>
                  <td colspan="5" class="px-3 py-4 text-zinc-500">Sem linha de serviço.</td>
                </tr>
              }
            </tbody>
          </table>
        </div>
        <h2 class="text-sm font-medium text-zinc-300">Eventos da sessão (NSU / cStat)</h2>
        <div class="max-h-[40vh] overflow-auto rounded border border-zinc-700 bg-zinc-950/60 p-3">
          @for (l of detail()?.contextLogs ?? []; track l.seqLog) {
            <div class="border-b border-zinc-800/80 py-2 text-xs last:border-0">
              <span class="text-zinc-500">{{ l.dtcLog | date: 'HH:mm:ss' }}</span>
              @if (l.cStat) {
                <span class="ml-2 rounded bg-zinc-800 px-1 text-teal-300">cStat {{ l.cStat }}</span>
              }
              <p class="mt-0.5 text-zinc-300">{{ summarize(l.mensagem, l.cStat) }}</p>
            </div>
          } @empty {
            <p class="text-sm text-zinc-500">Nenhum evento NSU/cStat nesta sessão.</p>
          }
        </div>
      }

      <!-- Configuração -->
      @if (key() === 'config' || key() === 'configuracao') {
        <div class="overflow-auto rounded border border-zinc-700">
          <table class="min-w-full text-left text-sm">
            <thead class="bg-zinc-900 text-xs uppercase text-zinc-500">
              <tr>
                <th class="px-3 py-2">Chave</th>
                <th class="px-3 py-2">Valor</th>
                <th class="px-3 py-2">Atualizado</th>
              </tr>
            </thead>
            <tbody>
              @for (r of detail()?.configRows ?? []; track r.key) {
                <tr
                  class="border-t border-zinc-800"
                  [ngClass]="r.key === 'Executar' ? 'bg-teal-950/20' : ''"
                >
                  <td class="px-3 py-2 font-medium text-zinc-200">{{ r.key }}</td>
                  <td class="px-3 py-2 font-mono text-teal-300">{{ r.value }}</td>
                  <td class="px-3 py-2 text-zinc-400">
                    {{ r.dtcAtualizacao | date: 'dd/MM HH:mm:ss' }}
                  </td>
                </tr>
              } @empty {
                <tr>
                  <td colspan="3" class="px-3 py-4 text-zinc-500">Sem configurações do processo.</td>
                </tr>
              }
            </tbody>
          </table>
        </div>
      }

      <!-- Temporária -->
      @if (key() === 'tmp' || key() === 'temporaria') {
        <p class="text-xs text-zinc-500">
          {{ (detail()?.tempRows ?? []).length }} lote(s) desta sessão (máx. 100) · sem XML
        </p>
        <div class="overflow-auto rounded border border-zinc-700">
          <table class="min-w-full text-left text-sm">
            <thead class="bg-zinc-900 text-xs uppercase text-zinc-500">
              <tr>
                <th class="px-3 py-2">NSU</th>
                <th class="px-3 py-2">NSU final</th>
                <th class="px-3 py-2">Qtd</th>
                <th class="px-3 py-2">Documento</th>
                <th class="px-3 py-2">Atualização</th>
                <th class="px-3 py-2">Erro</th>
              </tr>
            </thead>
            <tbody>
              @for (r of detail()?.tempRows ?? []; track r.nsu) {
                <tr
                  class="border-t border-zinc-800"
                  [ngClass]="r.hasError ? 'bg-rose-950/20' : ''"
                >
                  <td class="px-3 py-2 font-mono text-teal-300">{{ r.nsu }}</td>
                  <td class="px-3 py-2 font-mono">{{ r.nsuFinal ?? '—' }}</td>
                  <td class="px-3 py-2">{{ r.qtdDocumento }}</td>
                  <td class="px-3 py-2">{{ r.dtcDocumento | date: 'dd/MM HH:mm:ss' }}</td>
                  <td class="px-3 py-2">{{ r.dtcAtualizacao | date: 'dd/MM HH:mm:ss' }}</td>
                  <td class="px-3 py-2 text-rose-300">{{ r.mensagemErro || '—' }}</td>
                </tr>
              } @empty {
                <tr>
                  <td colspan="6" class="px-3 py-4 text-zinc-500">
                    Nenhum lote nesta sessão.
                  </td>
                </tr>
              }
            </tbody>
          </table>
        </div>
      }

      <!-- Log -->
      @if (key() === 'log') {
        <div class="max-h-[70vh] overflow-auto rounded border border-zinc-700 bg-zinc-950/60 p-3">
          @for (l of detail()?.logRows ?? []; track l.seqLog) {
            <div class="border-b border-zinc-800/80 py-2 text-xs last:border-0">
              <span class="text-zinc-500">{{ l.dtcLog | date: 'dd/MM HH:mm:ss' }}</span>
              @if (l.threadId) {
                <span class="ml-2 text-zinc-400">Linha {{ l.threadId }}</span>
              }
              @if (l.cStat) {
                <span class="ml-2 rounded bg-zinc-800 px-1 text-teal-300">cStat {{ l.cStat }}</span>
              }
              <p class="mt-0.5 text-zinc-300">{{ summarize(l.mensagem, l.cStat) }}</p>
            </div>
          } @empty {
            <p class="text-sm text-zinc-500">Nenhum log nesta sessão.</p>
          }
        </div>
      }

      <!-- Fila -->
      @if (key() === 'fila') {
        @if (detail()?.fila; as fila) {
          <div class="grid gap-3 sm:grid-cols-3">
            <div class="rounded border border-zinc-700 bg-zinc-900/40 p-4">
              <p class="text-xs text-zinc-500">Na fila agora</p>
              <p class="mt-1 text-2xl font-semibold text-zinc-100">{{ fila.depth }}</p>
            </div>
            <div class="rounded border border-zinc-700 bg-zinc-900/40 p-4">
              <p class="text-xs text-zinc-500">Limite de alerta</p>
              <p class="mt-1 text-2xl font-semibold text-zinc-100">{{ fila.highThreshold }}</p>
            </div>
            <div class="rounded border border-zinc-700 bg-zinc-900/40 p-4">
              <p class="text-xs text-zinc-500">Tendência</p>
              <p class="mt-1 text-sm text-zinc-200">{{ fila.trendHint }}</p>
            </div>
          </div>
          <div class="rounded border border-zinc-700 bg-zinc-900/40 p-4">
            <p class="text-xs text-zinc-500">Últimos ciclos (profundidade)</p>
            <p class="mt-2 font-mono text-sm text-teal-300">
              {{ fila.depthTrend.join(' → ') || '—' }}
            </p>
            <p class="mt-3 text-xs text-zinc-500">
              A fila não lista mensagens individuais — só a quantidade aguardando o Carga.
            </p>
          </div>
        }
      }
    </section>
  `,
})
export class TableDetailPageComponent {
  private readonly route = inject(ActivatedRoute);
  private readonly api = inject(MonitorApiService);
  private readonly destroyRef = inject(DestroyRef);

  readonly key = toSignal(
    this.route.paramMap.pipe(map((p) => (p.get('key') ?? '').toLowerCase())),
    { initialValue: '' }
  );

  readonly detail = signal<TableDetailDto | null>(null);
  readonly error = signal<string | null>(null);

  readonly health = computed(() => this.detail()?.health ?? null);

  constructor() {
    this.route.paramMap
      .pipe(
        map((p) => (p.get('key') ?? '').toLowerCase()),
        switchMap((key) =>
          timer(0, 2000).pipe(
            switchMap(() =>
              this.api.tableDetail(key, 100).pipe(
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
      config: 'Configuração',
      configuracao: 'Configuração',
      tmp: 'Temporária',
      temporaria: 'Temporária',
      log: 'Log',
      fila: 'Fila Carga',
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
        return 'bg-teal-950 text-teal-300';
    }
  }
}
