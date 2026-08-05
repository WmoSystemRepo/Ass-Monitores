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
import { ServiceMonitorStore } from '../service-monitor.store';
import { TableHealthCardsComponent } from '../table-health-cards.component';
import {
  connectionHealthLabel,
  formatHeartbeatAge,
  friendlyActionMessage,
  monitorConnectionLabel,
  integradorStatusLabel,
} from '@orquestrador/shared-utils';
import { resolvePipelineActivity, type PipelineStage } from './pipeline-activity';
import {
  IntegradorAnatomyFlowComponent,
  type AnatomyStage,
  type FlyingPacket,
} from './anatomy-flow.component';

@Component({
  selector: 'lib-integrador-dashboard-page',
  standalone: true,
  imports: [DatePipe, IntegradorAnatomyFlowComponent, TableHealthCardsComponent],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <section class="dashboard-fit flex h-[calc(100vh-3rem)] max-h-[calc(100vh-3rem)] flex-col gap-2 overflow-hidden">
      <header class="flex shrink-0 flex-wrap items-center justify-between gap-2">
        <div class="min-w-0">
          <h1 class="text-base font-semibold leading-tight text-zinc-50">
            Monitor do Integrador CT-e
          </h1>
          <p class="text-[11px] text-zinc-400">
            Acompanhe o ciclo de integração em tempo real.
          </p>
        </div>
        <div class="flex flex-wrap items-center gap-1.5">
          <span
            class="inline-flex items-center gap-1.5 rounded border px-2 py-1 text-[11px]"
            [class.border-teal-500]="store.live()"
            [class.text-teal-400]="store.live()"
            [class.border-zinc-500]="!store.live()"
            [class.text-zinc-400]="!store.live()"
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
            class="rounded bg-teal-600 px-2.5 py-1.5 text-xs font-medium text-white transition hover:bg-teal-500 disabled:opacity-40"
            [disabled]="store.actionBusy() || canStart() === false"
            (click)="store.startService()"
          >
            Ligar Integrador CT-e
          </button>
          <button
            type="button"
            class="rounded border border-rose-500/60 px-2.5 py-1.5 text-xs text-rose-300 transition hover:bg-rose-950/40 disabled:opacity-40"
            [disabled]="store.actionBusy()"
            (click)="confirmStop()"
          >
            Desligar
          </button>
        </div>
      </header>

      @if (isRunning()) {
        <div
          class="pulse-banner flex shrink-0 items-center justify-end gap-2 rounded-md border border-teal-500/30 bg-teal-950/30 px-3 py-1"
        >
          <div class="flex shrink-0 flex-wrap items-center justify-end gap-1.5">
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
        </div>
      }

      @if (store.bootError(); as err) {
        <div class="shrink-0 rounded border border-rose-500/40 bg-rose-950/40 px-3 py-1 text-xs text-rose-200">
          Não foi possível falar com o monitor. {{ err }}
        </div>
      }
      @if (actionBanner(); as banner) {
        <div class="shrink-0 rounded border border-violet-500/30 bg-violet-950/30 px-3 py-1 text-xs text-violet-100">
          <span class="font-medium">{{ banner.title }}</span>
          @if (banner.detail) {
            <span class="ml-1 font-mono text-[10px] text-violet-300/80">{{ banner.detail }}</span>
          }
        </div>
      }

      <div
        class="health-strip flex shrink-0 flex-wrap items-center gap-x-4 gap-y-1 rounded-md border border-zinc-700/80 bg-zinc-900/50 px-3 py-1.5 text-[11px]"
        [class.health-strip-live]="isRunning()"
      >
        <span class="inline-flex items-baseline gap-1.5">
          <span class="text-zinc-500">Integrador</span>
          <span class="font-medium text-zinc-100">{{ statusLabel() }}</span>
        </span>
        <span class="hidden text-zinc-700 sm:inline" aria-hidden="true">·</span>
        <span class="inline-flex items-baseline gap-1.5">
          <span class="text-zinc-500">Ciclo</span>
          <span
            class="font-medium"
            [class.text-teal-300]="service()?.executar === 1"
            [class.text-zinc-300]="service()?.executar !== 1"
          >
            {{ service()?.executar === 1 ? 'Ativo' : 'Ocioso' }}
          </span>
        </span>
        <span class="hidden text-zinc-700 sm:inline" aria-hidden="true">·</span>
        <span class="inline-flex items-baseline gap-1.5">
          <span class="text-zinc-500">Banco</span>
          <span class="font-medium text-zinc-100">{{ healthLabel() }}</span>
        </span>
        <span class="hidden text-zinc-700 sm:inline" aria-hidden="true">·</span>
        <span
          class="inline-flex min-w-0 items-baseline gap-1.5"
          [attr.title]="
            heartbeat().stale
              ? 'Última batida no banco (dtc_execucao) antiga — SVC_STALE conhecido na POC'
              : 'Última batida no banco'
          "
        >
          <span class="text-zinc-500">Servidor</span>
          <span
            class="truncate font-medium"
            [class.text-amber-300]="heartbeat().stale"
            [class.text-zinc-100]="!heartbeat().stale"
          >
            {{ service()?.nomServidor || '—' }}
            <span
              class="font-normal"
              [class.text-amber-200]="heartbeat().stale"
              [class.text-zinc-400]="!heartbeat().stale"
            >
              · {{ heartbeat().text }}
            </span>
          </span>
        </span>
      </div>

      @if (store.tableHealth().length) {
        <lib-table-health-cards class="block shrink-0" [items]="store.tableHealth()" />
      }

      <div class="min-h-0 flex-1 overflow-hidden">
        <lib-integrador-anatomy-flow
          class="block h-full"
          [running]="isRunning()"
          [activeStage]="visualStage()"
          [caption]="flowCaption()"
          [latest]="latestLote()"
          [packets]="flyingPackets()"
          [consuming]="queuesConsuming()"
        />
      </div>
    </section>
  `,
})
export class IntegradorDashboardPageComponent {
  readonly store = inject(ServiceMonitorStore);
  private readonly destroyRef = inject(DestroyRef);
  readonly service = this.store.service;
  readonly queues = this.store.queues;

  readonly journeyStage = signal<AnatomyStage | null>(null);
  readonly flyingPackets = signal<FlyingPacket[]>([]);
  /**
   * true enquanto fila/temp estão caindo — o foco do monitor é mostrar CT-e em movimento.
   */
  readonly queuesConsuming = signal(false);
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
  private lastSeenLogSeq = -1;
  private journeyPacketBaseId = '';
  private journeyTimers: ReturnType<typeof setTimeout>[] = [];
  private cycleEpochLoteKey = '';
  private cycleWasJourneyBusy = false;
  /** Evita disparar a journey mais de uma vez por âncora de ciclo. */
  private cycleAnimEpochKey: number | null = null;
  private startupJourneyDone = false;
  /** Até quando considerar “filas em movimento” após a última queda. */
  private consumingUntilMs = 0;

  readonly healthLabel = computed(() =>
    connectionHealthLabel(this.store.connectionHealth() ?? 'Down')
  );

  readonly connectionLabel = computed(() => monitorConnectionLabel(this.store.live()));

  readonly statusLabel = computed(() =>
    integradorStatusLabel(this.service()?.scmStatus, this.service()?.executar)
  );

  readonly isRunning = computed(() => {
    const s = this.service();
    return !!s?.isRunning && s.executar === 1;
  });

  readonly heartbeat = computed(() => {
    this.nowMs();
    return formatHeartbeatAge(this.service()?.dtcExecucao, {
      intervaloSec: this.resolveIntervaloSec(),
      processRunning: this.isRunning(),
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

    return resolvePipelineActivity([...fromSql, ...fromDebug], { maxAgeMs });
  });

  readonly visualStage = computed((): AnatomyStage | null => {
    const journey = this.journeyStage();
    if (journey) return journey;
    const fromLog = this.mapPipelineToAnatomy(this.liveActivity()?.stage ?? null);
    if (fromLog) return fromLog;
    // Filas caindo = processando, mesmo sem log recente / journey no ar.
    if (this.queuesConsuming()) return 'fila';
    return null;
  });

  /** Narrativa única — só no pôster (banner ficou só com cronômetros). */
  readonly flowCaption = computed(() => {
    const stage = this.visualStage();
    const lote = this.latestLote();
    const act = this.liveActivity();
    const broker = this.queues()?.serviceBrokerDepth ?? 0;
    const temp = this.queues()?.tempBacklog ?? 0;
    if (!this.isRunning()) {
      return 'Ligue o Integrador para ver O Ciclo de Integração.';
    }
    if (this.queuesConsuming()) {
      return `Sintetizando CT-e em tempo real — fila ${broker} · temp ${temp}`;
    }
    if (!stage) {
      return 'Integrador ligado — aguardando próximo ciclo de integração.';
    }

    const nsuBit =
      lote != null
        ? `NSU ${lote.nsu}${lote.nsuFinal != null ? ` → ${lote.nsuFinal}` : ''} · ${lote.qtdDocumento} CT-e`
        : null;

    switch (stage) {
      case 'fila':
        return nsuBit
          ? `Fila · ${nsuBit}`
          : act?.detail ?? 'Retirando NSU da fila do integrador…';
      case 'temp':
        return nsuBit
          ? `Temporária · ${nsuBit}`
          : act?.detail ?? 'Obtendo lote na temporária…';
      case 'classificar':
        return nsuBit
          ? `Classificar · ${nsuBit}`
          : act?.detail ?? 'Classificando por schema…';
      case 'persistir':
        return nsuBit
          ? `Persistir · ${nsuBit}`
          : act?.detail ?? 'Gravando nas tabelas sintéticas…';
      case 'limpar':
        return nsuBit
          ? `Limpar · ${nsuBit}`
          : act?.detail ?? 'Limpando temp / registrando erro…';
    }
  });

  /** Contagem regressiva até o próximo ciclo de integração. */
  readonly cycleCountdown = computed(() => {
    if (!this.isRunning()) return null;
    const now = this.nowMs();
    const intervalo = this.resolveIntervaloSec();

    if (this.visualStage()) {
      return {
        mode: 'busy' as const,
        caption: 'próx. ciclo',
        display: '--:--',
        secondsLeft: 0,
        hint: `Ciclo em andamento agora · intervalo configurado ${intervalo}s`,
      };
    }

    const epoch = this.cycleEpochMs();
    if (epoch == null) {
      return {
        mode: 'countdown' as const,
        caption: 'próx. ciclo',
        display: this.formatMmSs(intervalo),
        secondsLeft: intervalo,
        hint: `Próximo ciclo de integração · a cada ${intervalo}s`,
      };
    }

    const elapsedSec = Math.max(0, Math.floor((now - epoch) / 1000));
    const rem = elapsedSec % intervalo;
    const left = rem === 0 ? (elapsedSec === 0 ? intervalo : 0) : intervalo - rem;

    if (left <= 0) {
      return {
        mode: 'zero' as const,
        caption: 'próx. ciclo',
        display: '00:00',
        secondsLeft: 0,
        hint: `Hora do próximo ciclo · intervalo ${intervalo}s`,
      };
    }

    return {
      mode: 'countdown' as const,
      caption: 'próx. ciclo',
      display: this.formatMmSs(left),
      secondsLeft: left,
      hint: `Próximo ciclo de integração em ${left}s (a cada ${intervalo}s)`,
    };
  });

  /**
   * Cronômetro de movimentação: sobe desde o último lote até nova integração.
   * Zera quando o “Último lote” muda.
   */
  readonly fileWaitChrono = computed(() => {
    if (!this.isRunning()) return null;
    const now = this.nowMs();
    const lote = this.latestLote();
    const loteAt = lote?.dtcAtualizacao ? new Date(lote.dtcAtualizacao).getTime() : NaN;
    const hasLote = Number.isFinite(loteAt) && loteAt > 0;

    // Filas caindo = integração em andamento (não “sem integração”).
    if (this.queuesConsuming() || (this.visualStage() && hasLote && now - loteAt < 20_000)) {
      const broker = this.queues()?.serviceBrokerDepth ?? 0;
      const temp = this.queues()?.tempBacklog ?? 0;
      return {
        mode: 'found' as const,
        caption: 'integrando',
        display: '00:00',
        hint: `CT-e em movimento · fila ${broker} · temp ${temp}`,
      };
    }

    const start = hasLote ? loteAt : (this.cycleEpochMs() ?? now);
    const elapsed = Math.max(0, Math.floor((now - start) / 1000));

    return {
      mode: (elapsed === 0 ? 'fresh' : 'waiting') as 'fresh' | 'waiting',
      caption: 'sem integração',
      display: this.formatElapsedClock(elapsed),
      hint: hasLote
        ? `Há ${elapsed}s sem integração · último lote às ${new Date(loteAt).toLocaleTimeString('pt-BR')}`
        : `Aguardando o primeiro ciclo · ${elapsed}s`,
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
      if (this.consumingUntilMs > 0 && now >= this.consumingUntilMs) {
        this.consumingUntilMs = 0;
        this.queuesConsuming.set(false);
      }
    }, 1000);
    this.destroyRef.onDestroy(() => clearInterval(clock));

    // Âncora estável do countdown: lote novo, fim da animação, ou seed único ao ligar.
    effect(() => {
      if (!this.isRunning()) {
        this.cycleEpochMs.set(null);
        this.cycleEpochLoteKey = '';
        this.cycleWasJourneyBusy = false;
        this.cycleAnimEpochKey = null;
        this.startupJourneyDone = false;
        this.consumingUntilMs = 0;
        this.queuesConsuming.set(false);
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
      const queues = this.queues();
      const temp = queues?.tempBacklog ?? 0;
      const broker = queues?.serviceBrokerDepth ?? 0;

      const docKey = top
        ? `${top.nsu}-${top.nsuFinal ?? ''}-${top.dtcAtualizacao ?? ''}-${top.qtdDocumento}`
        : '';

      if (this.lastSeenDocKey === '' && this.lastTemp < 0) {
        this.lastSeenDocKey = docKey;
        this.lastTemp = temp;
        this.lastBroker = broker;
        return;
      }

      const journeyBusy = this.journeyStage() != null;
      // Aceita Healthy/Degraded: o sinal de processamento é a fila caindo, não só o ping.
      const queuesOk = (this.store.connectionHealth() ?? 'Down') !== 'Down';
      // Queda real (ignora wipe suspeito: zera tudo de uma vez com banco Down já filtrado).
      const wipeGlitch = (prev: number, next: number) => next === 0 && prev > 200;
      const brokerDown =
        queuesOk &&
        this.lastBroker >= 0 &&
        broker < this.lastBroker &&
        !wipeGlitch(this.lastBroker, broker);
      const tempDown =
        queuesOk &&
        this.lastTemp >= 0 &&
        temp < this.lastTemp &&
        !wipeGlitch(this.lastTemp, temp);
      const brokerUp = queuesOk && this.lastBroker >= 0 && broker > this.lastBroker;
      const newDoc =
        queuesOk &&
        !!docKey &&
        docKey !== this.lastSeenDocKey &&
        this.lastSeenDocKey !== '';

      if (brokerDown || tempDown) {
        this.markQueuesConsuming();
        if (!journeyBusy) {
          this.playLoteJourney(this.resolveJourneyNsu(top), top?.qtdDocumento ?? 2);
        }
      } else if (!journeyBusy && brokerUp) {
        this.pulseStage('fila', 2500);
        this.spawnPackets(String(top?.nsu ?? 'fila'), 2, 0, 'envelope');
      } else if (!journeyBusy && newDoc) {
        this.pulseStage('persistir', 2200);
        this.spawnPackets(String(top?.nsu ?? 'doc'), 2, 3, 'cte');
      }

      this.lastSeenDocKey = docKey || this.lastSeenDocKey;
      if (queuesOk) {
        this.lastTemp = temp;
        this.lastBroker = broker;
      }
    });

    // Logs reais do Integrador disparam a journey (Constante.Msg*).
    effect(() => {
      if (!this.isRunning()) return;
      if (this.journeyStage() != null) return;

      const logs = this.store.logs();
      if (!logs.length) return;

      const newest = [...logs].sort((a, b) => (b.seqLog ?? 0) - (a.seqLog ?? 0))[0];
      const seq = newest?.seqLog ?? -1;
      if (seq < 0 || seq === this.lastSeenLogSeq) return;

      if (this.lastSeenLogSeq < 0) {
        this.lastSeenLogSeq = seq;
        return;
      }

      this.lastSeenLogSeq = seq;
      const msg = (newest.mensagem ?? '').toLowerCase();
      const nsuMatch = (newest.mensagem ?? '').match(/(?:nsu|chave)\s*:\s*(\d+)/i);
      const nsu = nsuMatch ? Number(nsuMatch[1]) : null;

      if (msg.includes('chave retirada da fila') || msg.includes('lote obtido no banco')) {
        this.playLoteJourney(Number.isFinite(nsu as number) ? nsu : null, 2);
      } else if (
        msg.includes('documento excluído do banco') ||
        msg.includes('documento excluido do banco') ||
        msg.includes('documento atualizado no banco')
      ) {
        this.pulseStage('limpar', 2500);
        this.spawnPackets(String(nsu ?? 'ok'), 2, 4, 'envelope');
      } else if (
        msg.includes('inserido no banco') ||
        msg.includes('furos de nsu') ||
        msg.includes('nsu faltante')
      ) {
        this.pulseStage('persistir', 2200);
        this.spawnPackets(String(nsu ?? 'doc'), 2, 3, 'cte');
      } else if (
        msg.includes('sintetizar') ||
        msg.includes('elemento do lote') ||
        msg.includes('esquema')
      ) {
        this.pulseStage('classificar', 2200);
        this.spawnPackets(String(nsu ?? 'schema'), 2, 2, 'cte');
      }
    });

    // Ao ligar com backlog: mesma sequência visual do Receptor (AGORA + esteira + flyers).
    effect(() => {
      if (!this.isRunning()) return;
      if (this.startupJourneyDone || this.journeyStage() != null) return;

      const broker = this.queues()?.serviceBrokerDepth ?? 0;
      const temp = this.queues()?.tempBacklog ?? 0;
      if (broker <= 0 && temp <= 0) return;

      this.startupJourneyDone = true;
      this.markQueuesConsuming();
      const top = this.store.documents()[0];
      const t = setTimeout(() => {
        if (!this.isRunning() || this.journeyStage() != null) return;
        this.playLoteJourney(this.resolveJourneyNsu(top), top?.qtdDocumento ?? 2);
      }, 600);
      this.journeyTimers.push(t);
    });

    // A cada tick do ciclo (00:00) com trabalho na fila → replay da journey (paridade visual).
    effect(() => {
      if (!this.isRunning()) return;
      this.nowMs();
      const clock = this.cycleCountdown();
      const epoch = this.cycleEpochMs();
      if (!clock || clock.mode !== 'zero' || epoch == null) return;
      if (this.cycleAnimEpochKey === epoch) return;
      if (this.journeyStage() != null) return;

      const broker = this.queues()?.serviceBrokerDepth ?? 0;
      const temp = this.queues()?.tempBacklog ?? 0;
      if (broker <= 0 && temp <= 0) return;

      this.cycleAnimEpochKey = epoch;
      this.markQueuesConsuming();
      const top = this.store.documents()[0];
      this.playLoteJourney(this.resolveJourneyNsu(top), top?.qtdDocumento ?? 2);
    });
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
      case 'fila':
        return 'fila';
      case 'temp':
        return 'temp';
      case 'classificar':
        return 'classificar';
      case 'persistir':
        return 'persistir';
      case 'limpar':
        return 'limpar';
      default:
        return null;
    }
  }

  private clearJourneyTimers(): void {
    for (const t of this.journeyTimers) clearTimeout(t);
    this.journeyTimers = [];
  }

  private markQueuesConsuming(): void {
    // Janela larga: enquanto a fila continua caindo a cada snapshot, a animação encadeia.
    this.consumingUntilMs = Date.now() + 12_000;
    this.queuesConsuming.set(true);
  }

  private resolveJourneyNsu(top: { nsu?: number } | null | undefined): number | null {
    if (top?.nsu != null && Number.isFinite(top.nsu)) return top.nsu;
    const main = this.store.global()?.mainNsu;
    if (main && /^\d+$/.test(main)) return Number(main);
    return null;
  }

  private playLoteJourney(nsu: number | null, qtd: number): void {
    this.clearJourneyTimers();
    const label = nsu != null ? String(nsu) : 'lote';
    const chips = Math.min(6, Math.max(2, Math.min(qtd, 6)));
    this.journeyPacketBaseId = `j-${Date.now()}`;

    const sequence: { stage: AnatomyStage; lane: number; kind: 'cte' | 'envelope'; ms: number }[] =
      [
        { stage: 'fila', lane: 0, kind: 'envelope', ms: 0 },
        { stage: 'temp', lane: 1, kind: 'cte', ms: 1100 },
        { stage: 'classificar', lane: 2, kind: 'cte', ms: 2200 },
        { stage: 'persistir', lane: 3, kind: 'cte', ms: 3300 },
        { stage: 'limpar', lane: 4, kind: 'envelope', ms: 4400 },
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
      this.journeyPacketBaseId = '';
      // Continua animando enquanto as filas ainda estão caindo.
      if (this.isRunning() && Date.now() < this.consumingUntilMs) {
        const top = this.store.documents()[0];
        this.playLoteJourney(this.resolveJourneyNsu(top), top?.qtdDocumento ?? 2);
      }
    }, 5600);
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
    const base = this.journeyPacketBaseId || `p-${Date.now()}`;
    const items: FlyingPacket[] = Array.from({ length: count }, (_, i) => ({
      id: `${base}-${i}`,
      kind,
      label: count > 1 ? `${label}` : label,
      lane,
    }));
    this.flyingPackets.set(items);
  }

  confirmStop(): void {
    if (confirm('Desligar o Integrador CT-e? Ele para o ciclo de integração.')) {
      void this.store.stopService();
    }
  }
}
