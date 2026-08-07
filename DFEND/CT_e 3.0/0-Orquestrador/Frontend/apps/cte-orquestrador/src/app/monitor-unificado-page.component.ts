import { DatePipe } from '@angular/common';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import {
  ChangeDetectionStrategy,
  Component,
  DestroyRef,
  OnInit,
  computed,
  inject,
  signal,
} from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { ChainOrchestratorStore, getApiBaseUrl } from '@orquestrador/monitor-core';
import { Observable, firstValueFrom } from 'rxjs';

/** Ids oficiais dos monitores unificados (espelha DependencyInjection.KnownMonitorServiceIds). */
const SERVICOS_VALIDOS = [
  'receptor',
  'arquivador',
  'sintetizador',
  'analisador',
  'integrador',
  'carga',
] as const;

interface ErroChamada {
  acao: string;
  timestamp: Date;
  status: number | null;
  statusText: string | null;
  url: string | null;
  mensagem: string | null;
  corpo: unknown;
}

/**
 * W2: página do monitor unificado — chama /api/monitores/{servico}/* para todos os monitores
 * (receptor, arquivador, sintetizador, analisador, integrador, carga) já registrados via bridge
 * HTTP no Orquestrador. Consome as rotas mapeadas em UnifiedMonitorEndpoints (MapUnifiedMonitorEndpoints).
 */
@Component({
  selector: 'app-monitor-unificado-page',
  standalone: true,
  imports: [RouterLink, FormsModule, DatePipe],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <div class="monitor-unificado-page">
      <a routerLink="/" class="monitor-unificado-back">← Voltar ao painel</a>

      @if (!servicoValido()) {
        <div class="monitor-unificado-invalid">
          <h1 class="monitor-unificado-title">Serviço inválido</h1>
          <p class="monitor-unificado-hint">
            O serviço "{{ servico() }}" não é reconhecido pelo Monitor unificado.
          </p>
          <p class="monitor-unificado-hint">
            Serviços válidos: <code>{{ servicosValidos.join(', ') }}</code>.
          </p>
        </div>
      } @else {
        <header class="monitor-unificado-header">
          <div class="min-w-0">
            <p class="monitor-unificado-eyebrow">Monitor unificado</p>
            <h1 class="monitor-unificado-title">{{ displayName() }}</h1>
            <p class="monitor-unificado-subtitle">id: <code>{{ servico() }}</code></p>
          </div>
          @if (frontendUrlLegado()) {
            <button
              type="button"
              class="monitor-unificado-btn-secondary"
              (click)="abrirFrontLegado()"
              [disabled]="loading()"
              title="Abre o front Angular legado deste monitor (ensure-open)"
            >
              Abrir front legado ↗
            </button>
          }
        </header>

        <div class="monitor-unificado-actions">
          <button type="button" (click)="carregarSnapshot()" [disabled]="loading()">
            Atualizar snapshot
          </button>
          <button type="button" (click)="iniciar()" [disabled]="loading()">Iniciar</button>
          <button type="button" (click)="parar()" [disabled]="loading()">Parar</button>
          <button type="button" (click)="carregarStatus()" [disabled]="loading()">
            Atualizar status
          </button>
          <button type="button" (click)="carregarHealth()" [disabled]="loading()">
            Atualizar saúde
          </button>
        </div>

        @if (busyAction(); as acao) {
          <p class="monitor-unificado-hint">Executando "{{ acao }}"…</p>
        }
        @if (mensagem()) {
          <p class="monitor-unificado-hint">{{ mensagem() }}</p>
        }

        @if (erroAtual(); as erro) {
          <section class="monitor-unificado-error-panel" role="alert">
            <h2 class="monitor-unificado-panel-title">Erro na última chamada</h2>
            <dl class="monitor-unificado-error-grid">
              <dt>Ação</dt>
              <dd>{{ erro.acao }}</dd>
              <dt>HTTP</dt>
              <dd>{{ erro.status ?? '—' }} {{ erro.statusText ?? '' }}</dd>
              <dt>URL</dt>
              <dd class="font-mono">{{ erro.url ?? '—' }}</dd>
              <dt>Mensagem</dt>
              <dd>{{ erro.mensagem ?? '—' }}</dd>
            </dl>
            @if (erro.corpo) {
              <pre class="monitor-unificado-json">{{ pretty(erro.corpo) }}</pre>
            }
          </section>
        }

        @if (historicoErros().length > 0) {
          <details class="monitor-unificado-error-history">
            <summary>Histórico de erros ({{ historicoErros().length }})</summary>
            <ul>
              @for (item of historicoErros(); track item.timestamp) {
                <li>
                  <span class="font-mono">{{ item.timestamp | date: 'HH:mm:ss' }}</span>
                  — {{ item.acao }} — HTTP {{ item.status ?? '—' }} — {{ item.mensagem ?? '—' }}
                </li>
              }
            </ul>
          </details>
        }

        <div class="monitor-unificado-grid">
          <section class="monitor-unificado-panel">
            <div class="monitor-unificado-panel-head">
              <h2 class="monitor-unificado-panel-title">Info</h2>
              <button type="button" (click)="carregarInfo()" [disabled]="loading()">
                Atualizar
              </button>
            </div>
            <pre class="monitor-unificado-json">{{ pretty(info()) }}</pre>
          </section>

          <section class="monitor-unificado-panel">
            <div class="monitor-unificado-panel-head">
              <h2 class="monitor-unificado-panel-title">Snapshot</h2>
            </div>
            <pre class="monitor-unificado-json">{{ pretty(snapshot()) }}</pre>
          </section>

          <section class="monitor-unificado-panel">
            <div class="monitor-unificado-panel-head">
              <h2 class="monitor-unificado-panel-title">Status</h2>
            </div>
            <pre class="monitor-unificado-json">{{ pretty(status()) }}</pre>
          </section>

          <section class="monitor-unificado-panel">
            <div class="monitor-unificado-panel-head">
              <h2 class="monitor-unificado-panel-title">Saúde</h2>
            </div>
            <pre class="monitor-unificado-json">{{ pretty(health()) }}</pre>
          </section>

          <section class="monitor-unificado-panel monitor-unificado-panel-wide">
            <div class="monitor-unificado-panel-head">
              <h2 class="monitor-unificado-panel-title">Logs</h2>
              <div class="monitor-unificado-panel-controls">
                <label>
                  afterSeq
                  <input
                    type="number"
                    [ngModel]="logsAfterSeq()"
                    (ngModelChange)="logsAfterSeq.set($event)"
                  />
                </label>
                <label>
                  take
                  <input
                    type="number"
                    [ngModel]="logsTake()"
                    (ngModelChange)="logsTake.set($event)"
                  />
                </label>
                <button type="button" (click)="carregarLogs()" [disabled]="loading()">
                  Buscar
                </button>
              </div>
            </div>
            <pre class="monitor-unificado-json">{{ pretty(logs()) }}</pre>
          </section>

          <section class="monitor-unificado-panel monitor-unificado-panel-wide">
            <div class="monitor-unificado-panel-head">
              <h2 class="monitor-unificado-panel-title">Tabelas</h2>
              <div class="monitor-unificado-panel-controls">
                <label>
                  key
                  <input
                    type="text"
                    [ngModel]="tableKey()"
                    (ngModelChange)="tableKey.set($event)"
                    placeholder="ex.: fila"
                  />
                </label>
                <label>
                  take
                  <input
                    type="number"
                    [ngModel]="tableTake()"
                    (ngModelChange)="tableTake.set($event)"
                  />
                </label>
                <button
                  type="button"
                  (click)="carregarTabela()"
                  [disabled]="loading() || !tableKey().trim()"
                >
                  Buscar
                </button>
              </div>
            </div>
            <pre class="monitor-unificado-json">{{ pretty(tabela()) }}</pre>
          </section>
        </div>
      }
    </div>
  `,
  styles: [
    `
      .monitor-unificado-page {
        max-width: 1180px;
        margin: 0 auto;
        padding: 1.5rem;
        color: #e0e7ff;
        font-family: inherit;
      }
      .monitor-unificado-back {
        display: inline-block;
        margin-bottom: 1rem;
        color: #a5b4fc;
        text-decoration: none;
        font-size: 0.85rem;
      }
      .monitor-unificado-back:hover {
        text-decoration: underline;
      }
      .monitor-unificado-invalid {
        border: 1px solid rgba(248, 113, 113, 0.55);
        background: rgba(127, 29, 29, 0.35);
        border-radius: 0.65rem;
        padding: 1rem 1.25rem;
      }
      .monitor-unificado-header {
        display: flex;
        flex-wrap: wrap;
        align-items: flex-start;
        justify-content: space-between;
        gap: 0.75rem;
        margin-bottom: 1rem;
      }
      .monitor-unificado-eyebrow {
        font-size: 10px;
        font-weight: 700;
        letter-spacing: 0.14em;
        text-transform: uppercase;
        color: #a78bfa;
        margin: 0 0 0.2rem;
      }
      .monitor-unificado-title {
        font-size: 1.25rem;
        font-weight: 600;
        margin: 0;
      }
      .monitor-unificado-subtitle {
        font-size: 0.8rem;
        color: #94a3b8;
        margin: 0.15rem 0 0;
      }
      .monitor-unificado-subtitle code,
      .monitor-unificado-invalid code {
        font-family: 'IBM Plex Mono', ui-monospace, monospace;
        color: #c4b5fd;
      }
      .monitor-unificado-actions {
        display: flex;
        flex-wrap: wrap;
        gap: 0.5rem;
        margin-bottom: 1rem;
      }
      .monitor-unificado-actions button,
      .monitor-unificado-panel-head button,
      .monitor-unificado-panel-controls button,
      .monitor-unificado-btn-secondary {
        padding: 0.4rem 0.9rem;
        border-radius: 0.4rem;
        border: 1px solid #3730a3;
        background: rgba(30, 27, 75, 0.75);
        color: #e0e7ff;
        cursor: pointer;
        font-size: 0.85rem;
        white-space: nowrap;
      }
      .monitor-unificado-btn-secondary {
        border-color: #475569;
        background: rgba(30, 41, 59, 0.75);
        color: #cbd5e1;
        align-self: flex-start;
      }
      .monitor-unificado-actions button:hover:not(:disabled),
      .monitor-unificado-panel-head button:hover:not(:disabled),
      .monitor-unificado-panel-controls button:hover:not(:disabled),
      .monitor-unificado-btn-secondary:hover:not(:disabled) {
        background: rgba(55, 48, 163, 0.75);
      }
      .monitor-unificado-actions button:disabled,
      .monitor-unificado-panel-head button:disabled,
      .monitor-unificado-panel-controls button:disabled,
      .monitor-unificado-btn-secondary:disabled {
        opacity: 0.5;
        cursor: not-allowed;
      }
      .monitor-unificado-hint {
        font-size: 0.85rem;
        color: #a5b4fc;
        margin-bottom: 0.75rem;
      }
      .monitor-unificado-error-panel {
        border: 1px solid rgba(248, 113, 113, 0.55);
        background: linear-gradient(180deg, rgba(127, 29, 29, 0.4), rgba(69, 10, 10, 0.65));
        border-radius: 0.65rem;
        padding: 0.85rem 1rem;
        margin-bottom: 1rem;
      }
      .monitor-unificado-error-grid {
        display: grid;
        grid-template-columns: max-content 1fr;
        gap: 0.15rem 0.75rem;
        margin: 0 0 0.5rem;
        font-size: 0.82rem;
      }
      .monitor-unificado-error-grid dt {
        color: #fca5a5;
        font-weight: 700;
      }
      .monitor-unificado-error-grid dd {
        margin: 0;
        color: #fee2e2;
        word-break: break-word;
      }
      .monitor-unificado-error-history {
        margin-bottom: 1rem;
        font-size: 0.8rem;
        color: #cbd5e1;
      }
      .monitor-unificado-error-history summary {
        cursor: pointer;
        color: #a5b4fc;
      }
      .monitor-unificado-error-history ul {
        margin: 0.4rem 0 0;
        padding-left: 1.1rem;
      }
      .monitor-unificado-error-history li {
        margin-bottom: 0.2rem;
      }
      .monitor-unificado-grid {
        display: grid;
        grid-template-columns: repeat(2, minmax(0, 1fr));
        gap: 1rem;
      }
      .monitor-unificado-panel-wide {
        grid-column: 1 / -1;
      }
      .monitor-unificado-panel {
        border: 1px solid #3730a3;
        background: rgba(15, 13, 45, 0.5);
        border-radius: 0.65rem;
        padding: 0.85rem;
        min-width: 0;
      }
      .monitor-unificado-panel-head {
        display: flex;
        flex-wrap: wrap;
        align-items: center;
        justify-content: space-between;
        gap: 0.5rem;
        margin-bottom: 0.5rem;
      }
      .monitor-unificado-panel-controls {
        display: flex;
        flex-wrap: wrap;
        align-items: end;
        gap: 0.6rem;
      }
      .monitor-unificado-panel-controls label {
        display: flex;
        flex-direction: column;
        gap: 0.15rem;
        font-size: 0.7rem;
        color: #94a3b8;
      }
      .monitor-unificado-panel-controls input {
        padding: 0.3rem 0.5rem;
        border-radius: 0.35rem;
        border: 1px solid #3730a3;
        background: rgba(15, 13, 45, 0.85);
        color: #e0e7ff;
        font-size: 0.8rem;
        width: 8rem;
      }
      .monitor-unificado-panel-title {
        font-size: 0.85rem;
        font-weight: 700;
        text-transform: uppercase;
        letter-spacing: 0.04em;
        color: #93c5fd;
        margin: 0;
      }
      .monitor-unificado-json {
        background: rgba(15, 13, 45, 0.75);
        border: 1px solid #3730a3;
        border-radius: 0.5rem;
        padding: 0.85rem;
        font-size: 0.78rem;
        overflow: auto;
        max-height: 40vh;
        white-space: pre-wrap;
        word-break: break-word;
        margin: 0;
      }
    `,
  ],
})
export class MonitorUnificadoPageComponent implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly http = inject(HttpClient);
  private readonly store = inject(ChainOrchestratorStore);
  private readonly destroyRef = inject(DestroyRef);

  readonly servicosValidos = SERVICOS_VALIDOS;

  readonly servico = signal(this.route.snapshot.paramMap.get('servico') ?? '');
  readonly servicoValido = computed(() =>
    (SERVICOS_VALIDOS as readonly string[]).includes(this.servico())
  );

  readonly displayName = computed(() => {
    const sys = this.store.systems().find((s) => s.id === this.servico());
    if (sys?.label) {
      return sys.label;
    }
    const id = this.servico();
    return id.length ? id.charAt(0).toUpperCase() + id.slice(1) : id;
  });

  readonly frontendUrlLegado = computed(
    () => this.store.systems().find((s) => s.id === this.servico())?.frontendUrl ?? null
  );

  readonly busyAction = signal<string | null>(null);
  readonly loading = computed(() => this.busyAction() !== null);
  readonly mensagem = signal<string | null>(null);
  readonly erroAtual = signal<ErroChamada | null>(null);
  readonly historicoErros = signal<ErroChamada[]>([]);

  readonly info = signal<unknown>(null);
  readonly snapshot = signal<unknown>(null);
  readonly status = signal<unknown>(null);
  readonly health = signal<unknown>(null);
  readonly logs = signal<unknown>(null);
  readonly tabela = signal<unknown>(null);

  readonly logsAfterSeq = signal(0);
  readonly logsTake = signal(300);
  readonly tableKey = signal('');
  readonly tableTake = signal(1000);

  ngOnInit(): void {
    if (this.servicoValido()) {
      void this.carregarTudo();
    }

    this.route.paramMap.pipe(takeUntilDestroyed(this.destroyRef)).subscribe((params) => {
      const next = params.get('servico') ?? '';
      if (next === this.servico()) {
        return;
      }
      this.servico.set(next);
      this.resetState();
      if (this.servicoValido()) {
        void this.carregarTudo();
      }
    });
  }

  pretty(value: unknown): string {
    if (value === null || value === undefined) {
      return '—';
    }
    try {
      return JSON.stringify(value, null, 2);
    } catch {
      return String(value);
    }
  }

  private url(path: string): string {
    return `${getApiBaseUrl()}/api/monitores/${this.servico()}/${path}`;
  }

  async carregarTudo(): Promise<void> {
    await this.carregarInfo();
    await this.carregarSnapshot();
    await this.carregarStatus();
    await this.carregarHealth();
  }

  async carregarInfo(): Promise<void> {
    await this.run('info', this.http.get(this.url('info')), (d) => this.info.set(d), 'Info atualizada.');
  }

  async carregarSnapshot(): Promise<void> {
    await this.run(
      'snapshot',
      this.http.get(this.url('snapshot')),
      (d) => this.snapshot.set(d),
      'Snapshot atualizado.'
    );
  }

  async carregarStatus(): Promise<void> {
    await this.run(
      'status',
      this.http.get(this.url('service/status')),
      (d) => this.status.set(d),
      'Status atualizado.'
    );
  }

  async carregarHealth(): Promise<void> {
    await this.run('health', this.http.get(this.url('health')), (d) => this.health.set(d), 'Saúde atualizada.');
  }

  async carregarLogs(): Promise<void> {
    const params = { afterSeq: this.logsAfterSeq() ?? 0, take: this.logsTake() ?? 300 };
    await this.run(
      'logs',
      this.http.get(this.url('logs'), { params }),
      (d) => this.logs.set(d),
      'Logs atualizados.'
    );
  }

  async carregarTabela(): Promise<void> {
    const key = this.tableKey().trim();
    if (!key) {
      this.mensagem.set('Informe a key da tabela.');
      return;
    }
    const params = { take: this.tableTake() ?? 1000 };
    await this.run(
      'tables',
      this.http.get(this.url(`tables/${encodeURIComponent(key)}`), { params }),
      (d) => this.tabela.set(d),
      'Tabela atualizada.'
    );
  }

  async iniciar(): Promise<void> {
    await this.run(
      'start',
      this.http.post(this.url('service/start'), {}),
      (d) => this.status.set(d),
      'Início solicitado.'
    );
    await this.carregarSnapshot();
  }

  async parar(): Promise<void> {
    await this.run(
      'stop',
      this.http.post(this.url('service/stop'), {}),
      (d) => this.status.set(d),
      'Parada solicitada.'
    );
    await this.carregarSnapshot();
  }

  abrirFrontLegado(): void {
    const url = this.frontendUrlLegado();
    if (!url) {
      return;
    }
    const pending = window.open('about:blank', '_blank');
    if (pending) {
      pending.opener = null;
    }
    void this.store.openSystemUi(this.servico(), pending, url);
  }

  private resetState(): void {
    this.mensagem.set(null);
    this.erroAtual.set(null);
    this.historicoErros.set([]);
    this.info.set(null);
    this.snapshot.set(null);
    this.status.set(null);
    this.health.set(null);
    this.logs.set(null);
    this.tabela.set(null);
  }

  private async run<T>(
    acao: string,
    obs: Observable<T>,
    onSuccess: (data: T) => void,
    mensagemOk?: string
  ): Promise<void> {
    this.busyAction.set(acao);
    this.mensagem.set(null);
    try {
      const data = await firstValueFrom(obs);
      onSuccess(data);
      this.erroAtual.set(null);
      if (mensagemOk) {
        this.mensagem.set(mensagemOk);
      }
    } catch (err) {
      const info = this.toErroChamada(acao, err);
      this.erroAtual.set(info);
      this.historicoErros.update((list) => [info, ...list].slice(0, 10));
    } finally {
      this.busyAction.set(null);
    }
  }

  private toErroChamada(acao: string, err: unknown): ErroChamada {
    const timestamp = new Date();

    if (err instanceof HttpErrorResponse) {
      return {
        acao,
        timestamp,
        status: err.status || null,
        statusText: err.statusText || null,
        url: err.url,
        corpo: err.error ?? null,
        mensagem: this.extractMensagem(err.error) ?? err.message ?? null,
      };
    }

    if (err && typeof err === 'object' && (err as { name?: string }).name === 'TimeoutError') {
      return {
        acao,
        timestamp,
        status: null,
        statusText: 'Timeout',
        url: null,
        corpo: null,
        mensagem: `Timeout ao executar "${acao}" (sem resposta do serviço).`,
      };
    }

    return {
      acao,
      timestamp,
      status: null,
      statusText: null,
      url: null,
      corpo: null,
      mensagem: err instanceof Error ? err.message : 'Falha ao comunicar com o serviço.',
    };
  }

  private extractMensagem(body: unknown): string | null {
    if (body && typeof body === 'object') {
      const obj = body as Record<string, unknown>;
      const msg = obj['message'] ?? obj['detail'] ?? obj['mensagem'];
      if (typeof msg === 'string' && msg.trim()) {
        return msg.trim();
      }
    }
    return null;
  }
}
