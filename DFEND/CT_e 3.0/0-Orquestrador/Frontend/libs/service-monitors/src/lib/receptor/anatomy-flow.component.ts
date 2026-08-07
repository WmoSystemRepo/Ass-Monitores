import {
  ChangeDetectionStrategy,
  Component,
  computed,
  effect,
  inject,
  input,
  OnDestroy,
  signal,
} from '@angular/core';
import { DatePipe } from '@angular/common';
import { RouterLink } from '@angular/router';
import { ServiceMonitorStore } from '../service-monitor.store';
import { RecentDocument } from '@orquestrador/shared-data';

/** Estágios do ciclo (diagrama fluxo — 5 plataformas). */
export type AnatomyStage = 'sefaz' | 'consulta' | 'temp' | 'broker' | 'arquivador';

export interface FlyingPacket {
  id: string;
  kind: 'cte' | 'envelope';
  label: string;
  /** 0=sefaz … 4=arquivador */
  lane: number;
}

@Component({
  selector: 'lib-receptor-anatomy-flow',
  standalone: true,
  imports: [DatePipe, RouterLink],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <div
      class="pipeline-anatomy anatomy-poster anatomy-poster-fill flex h-full min-h-0 flex-col overflow-hidden rounded-xl border border-slate-600/60 shadow-md"
      [class.anatomy-poster-live]="running()"
      [class.anatomy-poster-busy]="!!activeStage()"
      [class.anatomy-poster-starting]="isBooting()"
      data-tour="anatomy"
    >
      <div class="anatomy-poster-head flex shrink-0 flex-wrap items-center justify-between gap-2 px-4 pt-3">
        <div class="min-w-0" data-tour="anatomy-legend">
          <p class="text-[10px] font-semibold uppercase tracking-[0.16em] text-sky-300/90">
            Como o documento anda
          </p>
          <h2
            class="text-base font-semibold leading-tight text-slate-50"
            title="Ciclo SEFAZ → consulta → temporária → fila → Arquivador"
          >
            Caminho do CT-e no Receptor
          </h2>
          <p class="text-[11px] text-slate-400">
            {{ caption() }}
          </p>
          <ul class="anatomy-legend" aria-label="Legenda de status">
            <li class="anatomy-legend-item">
              <span class="anatomy-legend-swatch anatomy-legend-agora"></span>
              Agora
            </li>
            <li class="anatomy-legend-item">
              <span class="anatomy-legend-swatch anatomy-legend-running"></span>
              Feito
            </li>
            <li class="anatomy-legend-item">
              <span class="anatomy-legend-swatch anatomy-legend-stopped"></span>
              Parado
            </li>
          </ul>
        </div>
        <div class="flex flex-wrap items-center gap-2">
          @if (latest(); as lote) {
            <div class="rounded-lg border border-slate-600/70 bg-slate-900/70 px-3 py-1.5 shadow-sm">
              <p class="text-[9px] font-semibold uppercase tracking-wide text-sky-400">Último lote</p>
              <p class="font-mono text-xs text-slate-200">
                NSU {{ lote.nsu }} → {{ lote.nsuFinal ?? lote.nsu }}
                <span class="text-slate-500"> · {{ lote.qtdDocumento }} CT-e</span>
                @if (lote.dtcAtualizacao) {
                  <span class="text-slate-500"> · {{ lote.dtcAtualizacao | date: 'HH:mm:ss' }}</span>
                }
              </p>
            </div>
          }
          <a
            routerLink="/monitores/receptor/mais-informacoes"
            class="rounded border border-slate-600 bg-slate-900/60 px-2.5 py-1 text-[11px] font-medium text-sky-300 hover:bg-slate-800"
            data-tour="nav-mais-informacoes"
          >
            Mais informações →
          </a>
        </div>
      </div>

      @if (beltMoving()) {
        <div
          class="anatomy-belt mx-4 mt-2 shrink-0 overflow-hidden rounded-md border border-slate-600/50 bg-slate-900/40"
          title="CT-e em trânsito na esteira"
        >
          <div class="anatomy-belt-track anatomy-belt-move">
            @for (n of beltItems(); track n) {
              <span class="anatomy-cte-doc">CT-e</span>
            }
            @for (n of beltItems(); track 'b' + n) {
              <span class="anatomy-cte-doc">CT-e</span>
            }
          </div>
        </div>
      } @else {
        <p
          class="anatomy-belt-idle-hint mx-4 mt-2 shrink-0 rounded-md border border-slate-600/40 bg-slate-900/50 px-3 py-1.5 text-center text-[11px] font-medium text-slate-300"
          title="Aguardando CT-e novo — consulta sem documento não move a esteira"
        >
          Nenhum documento passando agora — o Receptor consulta a SEFAZ de tempos em tempos
        </p>
      }

      <!-- Diagrama principal — uma composição centrada -->
      <div class="relative mx-3 mt-2 min-h-0 min-w-0 flex-1 overflow-x-auto overflow-y-hidden">
        <div class="anatomy-fill-board relative flex h-full min-w-[880px] flex-col items-stretch px-2 py-2">
          <div class="anatomy-cycle-block relative z-[1] mx-auto w-full">
            <div
              class="anatomy-path-line anatomy-path-line-5"
              [class.anatomy-path-line-active]="running() || isBooting()"
              aria-hidden="true"
            ></div>

            @for (pkt of packets(); track pkt.id; let pi = $index) {
              <div
                class="anatomy-flyer"
                [class.anatomy-flyer-envelope]="pkt.kind === 'envelope'"
                [style.left.%]="lanePercent(pkt.lane) + (pi % 3) * 1.4 - 1.4"
                [style.top.rem]="0.2 + (pi % 3) * 0.9"
              >
                <span class="anatomy-flyer-label">{{ pkt.kind === 'envelope' ? '✉' : 'CT-e' }}</span>
                <span class="anatomy-flyer-nsu">{{ pkt.label }}</span>
              </div>
            }

            <div
              class="anatomy-stages relative z-[1] shrink-0 py-1"
              data-tour="anatomy-stages"
            >
              @for (step of steps; track step.id; let i = $index) {
                <div
                  class="anatomy-stage"
                  [class.anatomy-stage-active]="activeStage() === step.id"
                  [class.anatomy-stage-done]="isDone(step.id)"
                  [class.anatomy-stage-booting]="isBooting()"
                  [style.--boot-delay]="i * 0.12 + 's'"
                >
                  @if (activeStage() === step.id) {
                    <span class="anatomy-now">AGORA</span>
                  }
                  <div
                    class="anatomy-platform"
                    [class.anatomy-platform-active]="activeStage() === step.id"
                    [class.anatomy-platform-done]="isDone(step.id)"
                    [class.anatomy-platform-waiting]="
                      running() && !activeStage() && step.id === 'consulta'
                    "
                    [class.anatomy-platform-rising]="queueMotion(step.id) === 'rising'"
                    [class.anatomy-platform-draining]="queueMotion(step.id) === 'draining'"
                    [class.anatomy-platform-booting]="isBooting()"
                    [attr.title]="step.techHint"
                  >
                    <div class="anatomy-iso" [attr.data-icon]="step.id"></div>
                    @if (queueChips(step.id); as chips) {
                      @if (chips.length > 0) {
                        <div class="anatomy-queue-stack" aria-hidden="true">
                          @for (c of chips; track c) {
                            <span
                              class="anatomy-queue-block"
                              [style.--chip-i]="c"
                            ></span>
                          }
                        </div>
                      }
                    }
                  </div>
                  <p class="anatomy-stage-title">{{ step.title }}</p>
                  <p class="anatomy-stage-tag">{{ step.tag }}</p>
                  <p
                    class="anatomy-stage-count"
                    [class.anatomy-stage-count-hot]="depthOf(step.id) > 0"
                  >
                    {{ count(step.id) }}
                  </p>
                  <p class="anatomy-stage-blurb">{{ step.blurb }}</p>
                </div>
                @if (i < steps.length - 1) {
                  <div
                    class="anatomy-step-arrow"
                    [class.anatomy-step-arrow-hot]="arrowHot(i)"
                    aria-hidden="true"
                  >
                    →
                  </div>
                }
              }
            </div>

            <div class="anatomy-summary-bar mt-3 shrink-0" data-tour="anatomy-summary">
              <div>
                <span class="anatomy-summary-label">NSU</span>
                <span class="anatomy-summary-value">{{ store.global()?.mainNsu || '—' }}</span>
              </div>
              <div>
                <span class="anatomy-summary-label">Na temporária</span>
                <span class="anatomy-summary-value">{{ store.queues()?.tempBacklog ?? 0 }}</span>
              </div>
              <div>
                <span class="anatomy-summary-label">Na fila</span>
                <span class="anatomy-summary-value">{{ store.queues()?.serviceBrokerDepth ?? 0 }}</span>
              </div>
              <div title="Threads">
                <a
                  routerLink="/monitores/receptor/threads"
                  class="anatomy-summary-label hover:text-cyan-300"
                  >Linhas de trabalho</a
                >
                <span class="anatomy-summary-value">{{ store.global()?.configuredThreads ?? '—' }}</span>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  `,
})
export class ReceptorAnatomyFlowComponent implements OnDestroy {
  readonly store = inject(ServiceMonitorStore);

  readonly running = input(false);
  readonly activeStage = input<AnatomyStage | null>(null);
  readonly caption = input(
    'Use Ligar o fluxo no topo para o Receptor começar a buscar CT-e.'
  );
  readonly latest = input<RecentDocument | null>(null);
  readonly packets = input<FlyingPacket[]>([]);

  /** Ligar: anima as plataformas em cascata enquanto o start está em andamento. */
  readonly isBooting = computed(
    () => this.store.actionBusy() && !this.running()
  );

  private readonly prevTemp = signal(0);
  private readonly prevBroker = signal(0);
  readonly tempMotion = signal<'idle' | 'rising' | 'draining'>('idle');
  readonly brokerMotion = signal<'idle' | 'rising' | 'draining'>('idle');
  private tempMotionTimer?: ReturnType<typeof setTimeout>;
  private brokerMotionTimer?: ReturnType<typeof setTimeout>;

  /**
   * Esteira só anda com CT-e em trânsito (pacotes / tmp→broker→arquivador).
   * Consulta SEFAZ sem documento novo → parada (não “falso movimento”).
   */
  readonly beltMoving = computed(() => {
    if (!this.running()) return false;
    if (this.packets().length > 0) return true;
    const stage = this.activeStage();
    return stage === 'temp' || stage === 'broker' || stage === 'arquivador';
  });

  /** Poucos chips — quantidade do último lote (máx. 8). */
  readonly beltItems = computed(() => {
    const q = this.latest()?.qtdDocumento ?? 4;
    const n = Math.min(8, Math.max(3, Math.min(q, 8)));
    return Array.from({ length: n }, (_, i) => i + 1);
  });

  readonly steps: {
    id: AnatomyStage;
    title: string;
    tag: string;
    blurb: string;
    techHint: string;
  }[] = [
    {
      id: 'sefaz',
      title: 'SEFAZ',
      tag: 'De onde vem',
      blurb: 'Governo envia os CT-e novos.',
      techHint: 'Autoridade fiscal — origem da distribuição',
    },
    {
      id: 'consulta',
      title: 'Consulta',
      tag: 'Busca novos',
      blurb: 'Pede só o que ainda não veio.',
      techHint: 'Consulta à SEFAZ · cteDistDFe / distCTeSVD',
    },
    {
      id: 'temp',
      title: 'Temporária',
      tag: 'Guarda rápido',
      blurb: 'Lote fica guardado um instante.',
      techHint: 'Persistência · tmp_documento_conhecimento_transporte_eletronico',
    },
    {
      id: 'broker',
      title: 'Fila',
      tag: 'Avisa o próximo',
      blurb: 'Chama o Arquivador para pegar.',
      techHint: 'Notificação · SQL Server Service Broker',
    },
    {
      id: 'arquivador',
      title: 'Arquivador',
      tag: 'Próximo serviço',
      blurb: 'Recebe o aviso e arquiva.',
      techHint: 'DFEND_CTe_Arquivador',
    },
  ];

  private readonly order: AnatomyStage[] = [
    'sefaz',
    'consulta',
    'temp',
    'broker',
    'arquivador',
  ];

  private clock?: ReturnType<typeof setInterval>;
  private readonly nowMs = signal(Date.now());

  constructor() {
    // Mantém OnPush fresco para counts de fila/tmp via store pushes; tick leve.
    this.clock = setInterval(() => this.nowMs.set(Date.now()), 1000);

    effect(() => {
      const temp = Math.max(0, Math.floor(this.store.queues()?.tempBacklog ?? 0));
      const prev = this.prevTemp();
      if (temp !== prev) {
        if (this.tempMotionTimer) clearTimeout(this.tempMotionTimer);
        this.tempMotion.set(temp > prev ? 'rising' : 'draining');
        this.prevTemp.set(temp);
        this.tempMotionTimer = setTimeout(() => this.tempMotion.set('idle'), 700);
      }
    });

    effect(() => {
      const broker = Math.max(
        0,
        Math.floor(this.store.queues()?.serviceBrokerDepth ?? 0)
      );
      const prev = this.prevBroker();
      if (broker !== prev) {
        if (this.brokerMotionTimer) clearTimeout(this.brokerMotionTimer);
        this.brokerMotion.set(broker > prev ? 'rising' : 'draining');
        this.prevBroker.set(broker);
        this.brokerMotionTimer = setTimeout(
          () => this.brokerMotion.set('idle'),
          700
        );
      }
    });
  }

  ngOnDestroy(): void {
    if (this.clock) clearInterval(this.clock);
    if (this.tempMotionTimer) clearTimeout(this.tempMotionTimer);
    if (this.brokerMotionTimer) clearTimeout(this.brokerMotionTimer);
  }

  lanePercent(lane: number): number {
    return 10 + lane * 20;
  }

  depthOf(id: AnatomyStage): number {
    this.nowMs();
    if (id === 'temp') return this.store.queues()?.tempBacklog ?? 0;
    if (id === 'broker') return this.store.queues()?.serviceBrokerDepth ?? 0;
    return 0;
  }

  queueChips(id: AnatomyStage): number[] {
    const d = Math.max(0, Math.floor(this.depthOf(id)));
    if (d <= 0) return [];
    const n = Math.min(8, Math.max(1, d));
    return Array.from({ length: n }, (_, i) => i);
  }

  queueMotion(id: AnatomyStage): 'idle' | 'rising' | 'draining' {
    if (id === 'temp') return this.tempMotion();
    if (id === 'broker') return this.brokerMotion();
    return 'idle';
  }

  count(id: AnatomyStage): string {
    this.nowMs();
    const active = this.activeStage() === id;
    switch (id) {
      case 'sefaz':
        return active ? 'consultando…' : 'origem';
      case 'consulta':
        return active
          ? this.store.global()?.mainNsu
            ? `NSU ${this.store.global()?.mainNsu}`
            : 'buscando…'
          : 'por NSU';
      case 'temp': {
        const n = this.store.queues()?.tempBacklog ?? 0;
        return n > 0 ? `${n} aguardando` : 'vazia';
      }
      case 'broker': {
        const n = this.store.queues()?.serviceBrokerDepth ?? 0;
        return n > 0 ? `${n} na fila` : 'vazia';
      }
      case 'arquivador':
        return active ? 'recebendo…' : 'próximo passo';
    }
  }

  isDone(id: AnatomyStage): boolean {
    const active = this.activeStage();
    if (!active) return false;
    return this.order.indexOf(id) < this.order.indexOf(active);
  }

  arrowHot(afterIndex: number): boolean {
    const active = this.activeStage();
    if (!active) return false;
    return this.order.indexOf(active) === afterIndex + 1;
  }
}
