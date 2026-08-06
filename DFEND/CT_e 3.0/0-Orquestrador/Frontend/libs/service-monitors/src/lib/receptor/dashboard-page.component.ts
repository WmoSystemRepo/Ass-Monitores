import {
  ChangeDetectionStrategy,
  Component,
  computed,
  DestroyRef,
  effect,
  inject,
  signal,
} from '@angular/core';
import { DatePipe } from '@angular/common';
import { RouterLink } from '@angular/router';
import { ServiceMonitorStore } from '../service-monitor.store';
import { TableHealthCardsComponent } from '../table-health-cards.component';
import {
  connectionHealthLabel,
  formatHeartbeatAge,
  friendlyActionMessage,
  monitorConnectionLabel,
} from '@orquestrador/shared-utils';
import { resolvePipelineActivity, type PipelineStage } from './pipeline-activity';
import {
  ReceptorAnatomyFlowComponent,
  type AnatomyStage,
  type FlyingPacket,
} from './anatomy-flow.component';

@Component({
  selector: 'lib-receptor-dashboard-page',
  standalone: true,
  imports: [DatePipe, RouterLink, ReceptorAnatomyFlowComponent, TableHealthCardsComponent],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <section class="dashboard-fit flex h-[calc(100vh-3rem)] max-h-[calc(100vh-3rem)] flex-col gap-2 overflow-hidden">
      <header class="flex shrink-0 flex-wrap items-center justify-between gap-2">
        <div class="min-w-0 flex flex-wrap items-center gap-2.5">
          <div class="min-w-0">
            <h1 class="text-base font-semibold leading-tight text-slate-50">
              Receptor CT-e
            </h1>
            <p class="text-[11px] text-slate-400">
              Está recebendo documentos agora?
            </p>
          </div>
          @if (heroStatus(); as hero) {
            <div
              class="status-hero"
              [class.status-hero-ok]="hero.tone === 'ok'"
              [class.status-hero-wait]="hero.tone === 'wait'"
              [class.status-hero-warn]="hero.tone === 'warn'"
              [class.status-hero-off]="hero.tone === 'off'"
              [attr.title]="hero.hint"
              role="status"
            >
              @if (hero.tone === 'ok') {
                <span class="live-dot"></span>
              }
              <span class="status-hero-label">{{ hero.label }}</span>
              @if (hero.detail) {
                <span class="status-hero-detail">{{ hero.detail }}</span>
              }
            </div>
          }
        </div>
        <div class="flex flex-wrap items-center gap-1.5">
          <span
            class="inline-flex items-center gap-1.5 rounded border px-2 py-1 text-[11px]"
            [class.border-emerald-500]="store.live()"
            [class.text-emerald-400]="store.live()"
            [class.border-amber-500]="!store.live()"
            [class.text-amber-400]="!store.live()"
            [attr.title]="
              store.live()
                ? 'Monitor recebendo atualizações (SignalR)'
                : 'Monitor sem push recente'
            "
          >
            @if (store.live()) {
              <span class="live-dot"></span>
            }
            {{ connectionLabel() }}
            @if (store.lastPushAt(); as t) {
              · {{ t | date: 'HH:mm:ss' }}
            }
          </span>
          <button
            type="button"
            class="rounded bg-emerald-600 px-2.5 py-1.5 text-xs font-medium text-white transition hover:bg-emerald-500 disabled:opacity-40"
            [disabled]="store.actionBusy() || canStart() === false"
            (click)="store.startService()"
          >
            {{ primaryActionLabel() }}
          </button>
          <button
            type="button"
            class="rounded border border-rose-500/60 px-2.5 py-1.5 text-xs text-rose-300 transition hover:bg-rose-950/40 disabled:opacity-40"
            [disabled]="store.actionBusy() || !processUp()"
            (click)="confirmStop()"
          >
            Desligar
          </button>
        </div>
      </header>

      @if (store.bootError(); as err) {
        <div class="shrink-0 rounded border border-rose-500/40 bg-rose-950/40 px-3 py-1 text-xs text-rose-200">
          Não foi possível falar com o monitor. {{ err }}
        </div>
      }
      @if (actionBanner(); as banner) {
        <div class="shrink-0 rounded border border-sky-500/30 bg-sky-950/30 px-3 py-1 text-xs text-sky-100">
          <span class="font-medium">{{ banner.title }}</span>
          @if (banner.detail) {
            <span class="ml-1 font-mono text-[10px] text-sky-300/80">{{ banner.detail }}</span>
          }
        </div>
      }

      @if (isLimitedTelemetry()) {
        <div
          class="shrink-0 rounded border border-amber-500/40 bg-amber-950/35 px-3 py-1.5 text-[11px] text-amber-100"
          role="status"
        >
          Snapshot limitado — o Orquestrador vê o processo (DevHost), mas ainda não lê filas/Executar do banco.
          @if (processUp()) {
            <span class="font-medium"> Processo no ar; trabalho sem telemetria completa.</span>
          }
          <button
            type="button"
            class="ml-2 rounded border border-amber-400/50 px-2 py-0.5 text-[10px] font-medium text-amber-50 transition hover:bg-amber-900/50 disabled:opacity-40"
            [disabled]="store.actionBusy()"
            (click)="store.startService()"
          >
            Reiniciar Receptor
          </button>
        </div>
      }

      @if (isRunning()) {
        <div
          class="cycle-bar pulse-banner flex shrink-0 flex-wrap items-center justify-between gap-x-3 gap-y-1.5 rounded-md border border-emerald-500/30 bg-emerald-950/30 px-3 py-1.5"
        >
          <div class="flex min-w-0 flex-wrap items-center gap-1.5">
            <span class="text-[10px] font-semibold uppercase tracking-wider text-emerald-400/80"
              >Ciclo SEFAZ</span
            >
            @if (cycleCountdown(); as clock) {
              <div
                class="cycle-chrono shrink-0"
                [class.cycle-chrono-busy]="clock.mode === 'busy'"
                [class.cycle-chrono-zero]="clock.mode === 'zero'"
                [attr.title]="clock.hint"
              >
                <span class="cycle-chrono-label">{{ clock.caption }}</span>
                <span class="cycle-chrono-digits">{{ clock.display }}</span>
              </div>
            }
            @if (fileWaitChrono(); as wait) {
              <div
                class="cycle-chrono cycle-chrono-wait shrink-0"
                [class.cycle-chrono-found]="wait.mode === 'found'"
                [attr.title]="wait.hint"
              >
                <span class="cycle-chrono-label">{{ wait.caption }}</span>
                <span class="cycle-chrono-digits">{{ wait.display }}</span>
              </div>
            }
          </div>
          <div
            class="flex min-w-0 flex-wrap items-center gap-x-3 gap-y-0.5 text-[11px] text-slate-400"
            [attr.title]="
              heartbeat().stale
                ? 'Última batida no banco (dtc_execucao) antiga — SVC_STALE conhecido na POC'
                : 'Saúde auxiliar'
            "
          >
            <span>
              Banco
              <span class="font-medium text-slate-200">{{ healthLabel() }}</span>
            </span>
            <span class="truncate">
              {{ service()?.nomServidor || '—' }}
              <span [class.text-amber-200]="heartbeat().stale">· {{ heartbeat().text }}</span>
            </span>
          </div>
        </div>
      } @else if (processUp()) {
        <div
          class="flex shrink-0 flex-wrap items-center justify-between gap-2 rounded-md border border-amber-500/35 bg-amber-950/25 px-3 py-1.5 text-[11px] text-amber-100"
        >
          <span>
            @if (isLimitedTelemetry()) {
              Processo no ar — telemetria incompleta. Reinicie para tentar ler filas/Executar.
            } @else {
              Recepção pausada (Executar≠1). Ative para voltar a consultar a SEFAZ.
            }
          </span>
          <button
            type="button"
            class="rounded bg-emerald-600 px-2.5 py-1 text-xs font-medium text-white transition hover:bg-emerald-500 disabled:opacity-40"
            [disabled]="store.actionBusy()"
            (click)="store.startService()"
          >
            {{ primaryActionLabel() }}
          </button>
        </div>
      }

      @if (store.tableHealth().length && isRunning()) {
        <lib-table-health-cards class="block shrink-0" [items]="store.tableHealth()" />
      }

      <div class="min-h-0 flex-1 overflow-hidden">
        @if (!processUp()) {
          <div class="receptor-empty flex h-full flex-col items-center justify-center gap-4 rounded-xl border border-slate-600/60 bg-slate-950/40 px-6 text-center">
            <div class="max-w-md space-y-2">
              <p class="text-[10px] font-semibold uppercase tracking-[0.16em] text-sky-300/90">
                Pipeline do Receptor
              </p>
              <h2 class="text-lg font-semibold text-slate-50">Receptor parado</h2>
              <p class="text-sm text-slate-400">
                Ligue para buscar CT-e na SEFAZ e acompanhar o fluxo:
                consulta → temporária → fila → Arquivador.
              </p>
              <p class="font-mono text-[11px] text-slate-500">
                SEFAZ → Consulta → Temporária → Fila → Arquivador
              </p>
            </div>
            <div class="flex flex-wrap items-center justify-center gap-2">
              <button
                type="button"
                class="rounded bg-emerald-600 px-4 py-2 text-sm font-medium text-white transition hover:bg-emerald-500 disabled:opacity-40"
                [disabled]="store.actionBusy()"
                (click)="store.startService()"
              >
                {{ primaryActionLabel() }}
              </button>
              <a
                routerLink="/monitores/receptor/mais-informacoes"
                class="rounded border border-slate-600 px-3 py-2 text-xs font-medium text-sky-300 hover:bg-slate-800"
              >
                Mais informações →
              </a>
            </div>
          </div>
        } @else {
          <lib-receptor-anatomy-flow
            class="block h-full"
            [running]="isRunning()"
            [activeStage]="visualStage()"
            [caption]="flowCaption()"
            [latest]="latestLote()"
            [packets]="flyingPackets()"
          />
        }
      </div>
    </section>
  `,
})
export class ReceptorDashboardPageComponent {
  readonly store = inject(ServiceMonitorStore);
  private readonly destroyRef = inject(DestroyRef);
  readonly service = this.store.service;
  readonly queues = this.store.queues;

  readonly journeyStage = signal<AnatomyStage | null>(null);
  readonly flyingPackets = signal<FlyingPacket[]>([]);
  /** Tick a cada 1s para o cronômetro do banner. */
  readonly nowMs = signal(Date.now());
  /**
   * Início estável da janela de espera até o próximo ciclo.
   * Não usa liveActivity.at — logs/traces recentes “grudavam” o display em 01:00.
   */
  private readonly cycleEpochMs = signal<number | null>(null);

  private lastSeenDocKey = '';
  private lastTemp = -1;
  private lastBroker = -1;
  private journeyTimers: ReturnType<typeof setTimeout>[] = [];
  private cycleEpochLoteKey = '';
  private cycleWasJourneyBusy = false;

  readonly healthLabel = computed(() =>
    connectionHealthLabel(this.store.connectionHealth() ?? 'Down')
  );

  readonly connectionLabel = computed(() => monitorConnectionLabel(this.store.live()));

  readonly processUp = computed(() => !!this.service()?.isRunning);

  readonly isLimitedTelemetry = computed(
    () => this.store.snapshot()?.mode === 'in-process-limited'
  );

  readonly isRunning = computed(() => {
    const s = this.service();
    return !!s?.isRunning && s.executar === 1;
  });

  /** Status único legível em 2s — substitui Processo/Recepção fragmentados. */
  readonly heroStatus = computed(() => {
    if (this.processUp() && this.isLimitedTelemetry() && !this.isRunning()) {
      return {
        tone: 'warn' as const,
        label: 'Telemetria limitada',
        detail: 'processo no ar',
        hint: 'DevHost sobe o processo, mas filas/Executar ainda não vêm do banco',
      };
    }
    if (this.isRunning()) {
      if (this.visualStage()) {
        return {
          tone: 'ok' as const,
          label: 'Recebendo',
          detail: this.stageShortLabel(this.visualStage()),
          hint: this.flowCaption(),
        };
      }
      return {
        tone: 'wait' as const,
        label: 'Aguardando ciclo',
        detail: null as string | null,
        hint: 'Receptor ligado — aguardando próxima consulta à SEFAZ',
      };
    }
    if (this.processUp()) {
      return {
        tone: 'warn' as const,
        label: 'Pausado',
        detail: 'recepção off',
        hint: 'Processo no ar, mas Executar≠1 — ative a recepção',
      };
    }
    return {
      tone: 'off' as const,
      label: 'Parado',
      detail: null as string | null,
      hint: 'Receptor desligado — não busca novos CT-e',
    };
  });

  readonly primaryActionLabel = computed(() => {
    if (this.processUp() && !this.isRunning()) {
      return this.isLimitedTelemetry()
        ? 'Reiniciar Receptor'
        : 'Ativar recepção';
    }
    return 'Ligar Receptor CT-e';
  });

  readonly heartbeat = computed(() => {
    this.nowMs();
    return formatHeartbeatAge(this.service()?.dtcExecucao, {
      intervaloSec: this.resolveIntervaloSec(),
      processRunning: this.processUp(),
    });
  });

  readonly latestLote = computed(() => this.store.documents()[0] ?? null);

  readonly liveActivity = computed(() => {
    if (!this.isRunning()) return null;
    const intervaloSec = this.store.global()?.intervaloSeconds ?? 60;
    const sec = intervaloSec >= 1000 ? Math.round(intervaloSec / 1000) : intervaloSec;
    const maxAgeMs = Math.max(180_000, sec * 2500);

    const fromSql = this.store.logs().map((l, i) => ({
      mensagem: l.mensagem,
      threadId: l.threadId,
      cStat: l.cStat,
      dtcLog: l.dtcLog,
      seqLog: l.seqLog ?? i,
      source: 'sql' as const,
    }));

    const fromDebug = this.store.liveTrace().map((t, i) => ({
      mensagem: t.message,
      dtcLog: t.at,
      seqLog: 1_000_000_000 + i,
      source: 'debug' as const,
    }));

    const doc = this.latestLote();
    if (doc?.dtcAtualizacao) {
      const age = Date.now() - new Date(doc.dtcAtualizacao).getTime();
      if (age >= 0 && age < maxAgeMs) {
        fromSql.push({
          mensagem: `Lote inserido no banco. NSU: ${doc.nsu}`,
          threadId: null,
          cStat: null,
          dtcLog: doc.dtcAtualizacao,
          seqLog: 2_000_000_000,
          source: 'sql' as const,
        });
      }
    }

    return resolvePipelineActivity([...fromSql, ...fromDebug], { maxAgeMs });
  });

  readonly visualStage = computed((): AnatomyStage | null => {
    const journey = this.journeyStage();
    if (journey) return journey;
    return this.mapPipelineToAnatomy(this.liveActivity()?.stage ?? null);
  });

  /** Narrativa única — só no pôster (banner ficou só com cronômetros). */
  readonly flowCaption = computed(() => {
    const stage = this.visualStage();
    const lote = this.latestLote();
    const act = this.liveActivity();
    if (!this.isRunning()) {
      if (this.processUp() && this.isLimitedTelemetry()) {
        return 'Processo no ar — telemetria de filas/Executar indisponível neste modo (snapshot limitado).';
      }
      if (this.processUp()) {
        return 'Processo no ar, mas a recepção está pausada (Executar≠1). Ative a recepção para ver o pipeline.';
      }
      return 'Ligue o Receptor para ver o pipeline interno (SEFAZ → consulta → temporária → fila → Arquivador).';
    }
    if (!stage) {
      return 'Receptor ligado — aguardando próxima consulta à SEFAZ.';
    }

    const nsuBit =
      lote != null
        ? `NSU ${lote.nsu}${lote.nsuFinal != null ? ` → ${lote.nsuFinal}` : ''} · ${lote.qtdDocumento} CT-e`
        : null;

    switch (stage) {
      case 'sefaz':
        return act?.detail ?? 'Origem SEFAZ — preparando / respondendo consulta…';
      case 'consulta':
        return nsuBit
          ? `Consulta · ${nsuBit}`
          : act?.detail ?? 'Lendo NSU e consultando a SEFAZ…';
      case 'temp':
        return nsuBit
          ? `Temporária · ${nsuBit}`
          : 'Gravando lote na temporária…';
      case 'broker':
        return nsuBit
          ? `Fila · ${nsuBit}`
          : 'Enviando aviso ao Arquivador…';
      case 'arquivador':
        return 'Arquivador — lote encaminhado.';
    }
  });

  /** Contagem regressiva até a próxima consulta à SEFAZ (intervalo configurado). */
  readonly cycleCountdown = computed(() => {
    if (!this.isRunning()) return null;
    const now = this.nowMs();
    const intervalo = this.resolveIntervaloSec();

    if (this.visualStage()) {
      return {
        mode: 'busy' as const,
        caption: 'próxima consulta',
        display: 'em andamento',
        secondsLeft: 0,
        hint: `Consulta em andamento agora · intervalo configurado ${intervalo}s`,
      };
    }

    const epoch = this.cycleEpochMs();
    if (epoch == null) {
      return {
        mode: 'countdown' as const,
        caption: 'próxima consulta',
        display: this.formatMmSs(intervalo),
        secondsLeft: intervalo,
        hint: `Próxima consulta à SEFAZ · a cada ${intervalo}s`,
      };
    }

    const elapsedSec = Math.max(0, Math.floor((now - epoch) / 1000));
    const rem = elapsedSec % intervalo;
    const left = rem === 0 ? (elapsedSec === 0 ? intervalo : 0) : intervalo - rem;

    if (left <= 0) {
      return {
        mode: 'zero' as const,
        caption: 'próxima consulta',
        display: '00:00',
        secondsLeft: 0,
        hint: `Hora de consultar a SEFAZ · intervalo ${intervalo}s`,
      };
    }

    return {
      mode: 'countdown' as const,
      caption: 'próxima consulta',
      display: this.formatMmSs(left),
      secondsLeft: left,
      hint: `Próxima consulta à SEFAZ em ${left}s (a cada ${intervalo}s)`,
    };
  });

  /**
   * Cronômetro de movimentação: sobe desde o último lote até achar CT-e novo.
   * Zera quando o “Último lote” muda.
   */
  readonly fileWaitChrono = computed(() => {
    if (!this.isRunning()) return null;
    const now = this.nowMs();
    const lote = this.latestLote();
    const loteAt = lote?.dtcAtualizacao ? new Date(lote.dtcAtualizacao).getTime() : NaN;
    const hasLote = Number.isFinite(loteAt) && loteAt > 0;

    if (this.visualStage() && hasLote && now - loteAt < 20_000) {
      return {
        mode: 'found' as const,
        caption: 'CT-e novo',
        display: 'agora',
        hint: hasLote
          ? `Lote novo (NSU ${lote?.nsu}) — espera por arquivo reinicia`
          : 'CT-e em movimentação no fluxo',
      };
    }

    const start = hasLote ? loteAt : (this.cycleEpochMs() ?? now);
    const elapsed = Math.max(0, Math.floor((now - start) / 1000));

    return {
      mode: (elapsed === 0 ? 'fresh' : 'waiting') as 'fresh' | 'waiting',
      caption: 'sem documento',
      display: this.formatElapsedClock(elapsed),
      hint: hasLote
        ? `Há ${elapsed}s sem CT-e novo · último lote às ${new Date(loteAt).toLocaleTimeString('pt-BR')}`
        : `Buscando o primeiro CT-e · ${elapsed}s`,
    };
  });

  readonly actionBanner = computed(() => {
    const msg = this.store.actionMessage();
    if (!msg) return null;
    return friendlyActionMessage(msg);
  });

  canStart = computed(() => {
    const s = this.service();
    if (!s) return true;
    return !(s.isRunning && s.executar === 1);
  });

  constructor() {
    const clock = setInterval(() => {
      const now = Date.now();
      this.nowMs.set(now);
      this.rollCycleEpoch(now);
    }, 1000);
    this.destroyRef.onDestroy(() => clearInterval(clock));

    // Âncora estável do countdown: lote novo, fim da animação, ou seed único ao ligar.
    effect(() => {
      if (!this.isRunning()) {
        this.cycleEpochMs.set(null);
        this.cycleEpochLoteKey = '';
        this.cycleWasJourneyBusy = false;
        return;
      }

      const lote = this.latestLote();
      const loteKey = lote?.dtcAtualizacao
        ? `${lote.nsu}-${lote.nsuFinal ?? ''}-${lote.dtcAtualizacao}`
        : '';

      if (loteKey && loteKey !== this.cycleEpochLoteKey && lote?.dtcAtualizacao) {
        this.cycleEpochLoteKey = loteKey;
        const t = new Date(lote.dtcAtualizacao).getTime();
        if (Number.isFinite(t) && t > 0) {
          this.cycleEpochMs.set(t);
        }
      }

      const journeyBusy = this.journeyStage() != null;
      if (journeyBusy) {
        this.cycleWasJourneyBusy = true;
        return;
      }

      if (this.cycleWasJourneyBusy) {
        this.cycleWasJourneyBusy = false;
        this.cycleEpochMs.set(Date.now());
        return;
      }

      if (this.cycleEpochMs() == null) {
        const dtc = lote?.dtcAtualizacao ?? this.service()?.dtcExecucao;
        const t = dtc ? new Date(dtc).getTime() : NaN;
        this.cycleEpochMs.set(Number.isFinite(t) && t > 0 ? t : Date.now());
      }
    });

    effect(() => {
      if (!this.isRunning()) return;

      const docs = this.store.documents();
      const top = docs[0];
      const temp = this.queues()?.tempBacklog ?? 0;
      const broker = this.queues()?.serviceBrokerDepth ?? 0;

      const docKey = top
        ? `${top.nsu}-${top.nsuFinal ?? ''}-${top.dtcAtualizacao ?? ''}-${top.qtdDocumento}`
        : '';

      if (this.lastSeenDocKey === '' && this.lastTemp < 0) {
        this.lastSeenDocKey = docKey;
        this.lastTemp = temp;
        this.lastBroker = broker;
        return;
      }

      const docChanged = !!docKey && docKey !== this.lastSeenDocKey;
      const tempUp = this.lastTemp >= 0 && temp > this.lastTemp;
      const brokerUp = this.lastBroker >= 0 && broker > this.lastBroker;

      if (docChanged || tempUp) {
        this.lastSeenDocKey = docKey || this.lastSeenDocKey;
        this.playLoteJourney(top?.nsu ?? null, top?.qtdDocumento ?? 1);
      } else if (brokerUp) {
        this.pulseStage('broker', 2500);
        this.spawnPackets(String(top?.nsu ?? 'fila'), 2, 3, 'envelope');
      }

      this.lastTemp = temp;
      this.lastBroker = broker;
    });
  }

  private stageShortLabel(stage: AnatomyStage | null): string | null {
    switch (stage) {
      case 'sefaz':
        return 'SEFAZ';
      case 'consulta':
        return 'consulta';
      case 'temp':
        return 'temporária';
      case 'broker':
        return 'fila';
      case 'arquivador':
        return 'Arquivador';
      default:
        return null;
    }
  }

  private formatMmSs(totalSec: number): string {
    const m = Math.floor(Math.max(0, totalSec) / 60);
    const s = Math.max(0, totalSec) % 60;
    return `${m.toString().padStart(2, '0')}:${s.toString().padStart(2, '0')}`;
  }

  /** Relógio crescente HH:MM:SS (ou MM:SS se < 1h). */
  private formatElapsedClock(totalSec: number): string {
    const sec = Math.max(0, totalSec);
    const h = Math.floor(sec / 3600);
    const m = Math.floor((sec % 3600) / 60);
    const s = sec % 60;
    if (h > 0) {
      return `${h}:${m.toString().padStart(2, '0')}:${s.toString().padStart(2, '0')}`;
    }
    return `${m.toString().padStart(2, '0')}:${s.toString().padStart(2, '0')}`;
  }

  private resolveIntervaloSec(): number {
    let intervalo = this.store.global()?.intervaloSeconds ?? 60;
    if (intervalo >= 1000) intervalo = Math.round(intervalo / 1000);
    if (intervalo <= 0) intervalo = 60;
    return intervalo;
  }

  /** Avança a âncora em intervalos completos para o próximo ciclo recomeçar em MM:SS limpo. */
  private rollCycleEpoch(now: number): void {
    if (!this.isRunning() || this.visualStage()) return;
    const epoch = this.cycleEpochMs();
    if (epoch == null) return;
    const step = this.resolveIntervaloSec() * 1000;
    const elapsed = now - epoch;
    if (elapsed < step) return;
    const steps = Math.floor(elapsed / step);
    this.cycleEpochMs.set(epoch + steps * step);
  }

  private mapPipelineToAnatomy(stage: PipelineStage | null): AnatomyStage | null {
    if (!stage) return null;
    switch (stage) {
      case 'bootstrap':
        // Bootstrap fica só no feed — não rouba AGORA no poster
        return null;
      case 'nsu':
        return 'consulta';
      case 'sefaz':
        return 'sefaz';
      case 'temp':
        return 'temp';
      case 'broker':
        return 'broker';
      default:
        return null;
    }
  }

  private clearJourneyTimers(): void {
    for (const t of this.journeyTimers) clearTimeout(t);
    this.journeyTimers = [];
  }

  private playLoteJourney(nsu: number | null, qtd: number): void {
    this.clearJourneyTimers();
    const label = nsu != null ? String(nsu) : 'lote';
    const chips = Math.min(6, Math.max(2, Math.min(qtd, 6)));

    const sequence: { stage: AnatomyStage; lane: number; kind: 'cte' | 'envelope'; ms: number }[] =
      [
        { stage: 'sefaz', lane: 0, kind: 'cte', ms: 0 },
        { stage: 'consulta', lane: 1, kind: 'cte', ms: 1400 },
        { stage: 'temp', lane: 2, kind: 'cte', ms: 2800 },
        { stage: 'broker', lane: 3, kind: 'envelope', ms: 4200 },
        { stage: 'arquivador', lane: 4, kind: 'envelope', ms: 5600 },
      ];

    for (const step of sequence) {
      const timer = setTimeout(() => {
        this.journeyStage.set(step.stage);
        this.spawnPackets(label, chips, step.lane, step.kind);
      }, step.ms);
      this.journeyTimers.push(timer);
    }

    const end = setTimeout(() => {
      this.journeyStage.set(null);
      this.flyingPackets.set([]);
    }, 7200);
    this.journeyTimers.push(end);
  }

  private pulseStage(stage: AnatomyStage, ms: number): void {
    this.journeyStage.set(stage);
    const t = setTimeout(() => {
      if (this.journeyStage() === stage) this.journeyStage.set(null);
    }, ms);
    this.journeyTimers.push(t);
  }

  private spawnPackets(
    label: string,
    count: number,
    lane: number,
    kind: 'cte' | 'envelope'
  ): void {
    const now = Date.now();
    const items: FlyingPacket[] = Array.from({ length: count }, (_, i) => ({
      id: `${now}-${lane}-${i}`,
      kind,
      label: count > 1 ? `${label}` : label,
      lane,
    }));
    this.flyingPackets.set(items);
  }

  confirmStop(): void {
    if (confirm('Desligar o Receptor CT-e? Ele para de buscar novos documentos.')) {
      void this.store.stopService();
    }
  }
}
