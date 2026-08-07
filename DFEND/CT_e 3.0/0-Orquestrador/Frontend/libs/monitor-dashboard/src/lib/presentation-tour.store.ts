import { Injectable, computed, inject, signal } from '@angular/core';
import { Router } from '@angular/router';
import type {
  CascadePhase,
  ChainSystemView,
  LiveTraceLine,
  LogEntry,
  MonitorAlert,
  TableDetailDto,
  TableHealthView,
  ThreadView,
} from '@orquestrador/shared-data';
import {
  PRESENTATION_STEPS,
  type PresentationReceptorStage,
  type PresentationStep,
  type PresentationSimulateMode,
} from './presentation-steps';

const CHAIN_IDS = [
  'receptor',
  'arquivador',
  'sintetizador',
  'analisador',
  'integrador',
  'carga',
] as const;

const SYMBOLS: Record<string, string> = {
  receptor: 'R',
  arquivador: 'A',
  sintetizador: 'S',
  analisador: 'An',
  integrador: 'I',
  carga: 'C',
};

const LABELS: Record<string, string> = {
  receptor: 'Serviço Receptor',
  arquivador: 'Serviço Arquivador',
  sintetizador: 'Serviço Sintetizador',
  analisador: 'Serviço Analisador',
  integrador: 'Serviço Integrador',
  carga: 'Serviço Carga',
};

const RECEPTOR_STAGES: PresentationReceptorStage[] = [
  'sefaz',
  'consulta',
  'temp',
  'broker',
  'arquivador',
];

const RECEPTOR_CAPTIONS: Record<PresentationReceptorStage, string> = {
  sefaz: 'Simulação · origem SEFAZ — preparando a consulta…',
  consulta: 'Simulação · consultando a SEFAZ (NSU do lote)…',
  temp: 'Simulação · gravando lote na temporária…',
  broker: 'Simulação · avisando o Arquivador pela fila…',
  arquivador: 'Simulação · Arquivador recebe o aviso e segue…',
};

export interface PresentationSimPacket {
  id: string;
  kind: 'cte' | 'envelope';
  label: string;
  lane: number;
}

export interface PresentationDetailsSim {
  logs: LogEntry[];
  liveTrace: LiveTraceLine[];
  alerts: MonitorAlert[];
  tableHealth: TableHealthView[];
  /** Token que sobe a cada abertura/fechamento automático do modal. */
  errorModalToken: number;
  /** ímpar = abrir modal; par (>0) = fechar */
  criticalErrorSeqLog: number;
}

export interface PresentationTablesSim {
  detail: TableDetailDto;
  tableHealth: TableHealthView[];
}

export interface PresentationThreadsSim {
  threads: ThreadView[];
  logs: LogEntry[];
  /** Δ de NSU por threadId (para status “Buscando”). */
  nsuDeltas: Record<number, number | null>;
  intervaloSeconds: number;
}

export interface PresentationSimulation {
  mode: PresentationSimulateMode;
  systems: ChainSystemView[];
  cascadePhase: CascadePhase;
  beltMoving: boolean;
  lastLoteQtd: number;
  receptorStage?: PresentationReceptorStage | null;
  receptorCaption?: string;
  receptorPackets?: PresentationSimPacket[];
  details?: PresentationDetailsSim;
  tables?: PresentationTablesSim;
  threads?: PresentationThreadsSim;
}

@Injectable({ providedIn: 'root' })
export class PresentationTourStore {
  private readonly router = inject(Router);
  private flowTimer?: ReturnType<typeof setInterval>;
  private detailsTimers: ReturnType<typeof setTimeout>[] = [];
  private flowStage = 0;
  private applyGeneration = 0;

  readonly active = signal(false);
  readonly stepIndex = signal(0);
  readonly simulation = signal<PresentationSimulation | null>(null);

  readonly steps = PRESENTATION_STEPS;

  readonly step = computed((): PresentationStep | null => {
    if (!this.active()) return null;
    return this.steps[this.stepIndex()] ?? null;
  });

  readonly stepLabel = computed(() => {
    if (!this.active()) return '';
    return `${this.stepIndex() + 1} / ${this.steps.length}`;
  });

  readonly canBack = computed(() => this.active() && this.stepIndex() > 0);
  readonly canNext = computed(
    () => this.active() && this.stepIndex() < this.steps.length - 1
  );
  readonly isLastStep = computed(
    () => this.active() && this.stepIndex() === this.steps.length - 1
  );

  readonly isSimulating = computed(() => {
    const s = this.step()?.simulate ?? 'none';
    return (
      s === 'flow' ||
      s === 'stoppedBacklog' ||
      s === 'receptorFlow' ||
      s === 'detailsFlow' ||
      s === 'tablesFlow' ||
      s === 'threadsFlow'
    );
  });

  readonly panelPlacement = computed((): 'top' | 'bottom' | 'left' | 'right' => {
    const p = this.step()?.panelPlacement;
    if (p === 'top' || p === 'left' || p === 'right') return p;
    return 'bottom';
  });

  readonly isChainSimulating = computed(() => {
    const mode = this.simulation()?.mode;
    return mode === 'flow' || mode === 'stoppedBacklog';
  });

  readonly isReceptorSimulating = computed(
    () => this.simulation()?.mode === 'receptorFlow'
  );

  readonly isDetailsSimulating = computed(
    () => this.simulation()?.mode === 'detailsFlow'
  );

  readonly isTablesSimulating = computed(
    () => this.simulation()?.mode === 'tablesFlow'
  );

  readonly isThreadsSimulating = computed(
    () => this.simulation()?.mode === 'threadsFlow'
  );

  start(): void {
    this.stopFlowTimer();
    this.active.set(true);
    this.stepIndex.set(0);
    void this.applyStep(0);
  }

  restart(): void {
    this.start();
  }

  exit(): void {
    this.stopFlowTimer();
    this.simulation.set(null);
    this.active.set(false);
    this.stepIndex.set(0);
    void this.router.navigateByUrl('/');
  }

  next(): void {
    if (!this.canNext()) return;
    const next = this.stepIndex() + 1;
    this.stepIndex.set(next);
    void this.applyStep(next);
  }

  back(): void {
    if (!this.canBack()) return;
    const prev = this.stepIndex() - 1;
    this.stepIndex.set(prev);
    void this.applyStep(prev);
  }

  private async applyStep(index: number): Promise<void> {
    const step = this.steps[index];
    if (!step) return;

    const gen = ++this.applyGeneration;
    this.stopFlowTimer();
    this.simulation.set(null);

    if (step.route) {
      await this.router.navigateByUrl(step.route);
    }
    if (gen !== this.applyGeneration) return;

    const mode = step.simulate ?? 'none';
    if (mode === 'flow') {
      this.flowStage = 0;
      this.pushFlowFrame(0);
      this.flowTimer = setInterval(() => {
        this.flowStage = (this.flowStage + 1) % CHAIN_IDS.length;
        this.pushFlowFrame(this.flowStage);
      }, 1600);
    } else if (mode === 'stoppedBacklog') {
      this.simulation.set(this.buildStoppedBacklog());
    } else if (mode === 'receptorFlow') {
      this.flowStage = 0;
      this.pushReceptorFrame(0);
      this.flowTimer = setInterval(() => {
        this.flowStage = (this.flowStage + 1) % RECEPTOR_STAGES.length;
        this.pushReceptorFrame(this.flowStage);
      }, 1600);
    } else if (mode === 'detailsFlow') {
      this.startDetailsSimulation(gen);
    } else if (mode === 'tablesFlow') {
      this.startTablesSimulation();
    } else if (mode === 'threadsFlow') {
      this.startThreadsSimulation();
    }

    queueMicrotask(() => {
      if (gen !== this.applyGeneration) return;
      this.scrollTarget(step.target);
    });
  }

  private startDetailsSimulation(gen: number): void {
    this.simulation.set({
      mode: 'detailsFlow',
      systems: [],
      cascadePhase: 'running',
      beltMoving: false,
      lastLoteQtd: 8,
      details: this.buildDetailsSim(0),
    });

    // Abre o modal do erro crítico sozinho.
    this.detailsTimers.push(
      setTimeout(() => {
        if (gen !== this.applyGeneration) return;
        const cur = this.simulation();
        if (cur?.mode !== 'detailsFlow' || !cur.details) return;
        this.simulation.set({
          ...cur,
          details: { ...cur.details, errorModalToken: 1 },
        });
      }, 1800)
    );

    // Fecha o modal automaticamente após o usuário “ver” o detalhe.
    this.detailsTimers.push(
      setTimeout(() => {
        if (gen !== this.applyGeneration) return;
        const cur = this.simulation();
        if (cur?.mode !== 'detailsFlow' || !cur.details) return;
        this.simulation.set({
          ...cur,
          details: { ...cur.details, errorModalToken: 2 },
        });
      }, 6500)
    );
  }

  private buildDetailsSim(token: number): PresentationDetailsSim {
    const now = Date.now();
    const iso = (secAgo: number) => new Date(now - secAgo * 1000).toISOString();
    const criticalSeq = 9_000_215;

    const logs: LogEntry[] = [
      {
        seqLog: criticalSeq,
        dtcLog: iso(8),
        mensagem:
          'Retorno SEFAZ Status=215 Rejeicao: Falha no esquema XML do pedido distCTeSVD.',
        cStat: '215',
        severityHint: 'error',
        threadId: 1,
      },
      {
        seqLog: 9_000_118,
        dtcLog: iso(25),
        mensagem:
          'Consulta SEFAZ cStat=118 — documentos localizados e gravados na temporária.',
        cStat: '118',
        severityHint: 'success',
        threadId: 1,
      },
      {
        seqLog: 9_000_108,
        dtcLog: iso(40),
        mensagem: 'Serviço de distribuição temporariamente indisponível (cStat 108).',
        cStat: '108',
        severityHint: 'warning',
        threadId: 2,
      },
      {
        seqLog: 9_000_001,
        dtcLog: iso(55),
        mensagem: 'Lote inserido no banco. NSU: 900001 → 900008.',
        cStat: null,
        severityHint: 'info',
        threadId: 1,
      },
    ];

    const liveTrace: LiveTraceLine[] = [
      {
        at: iso(6),
        message: 'Simulação · abrindo detalhe do erro crítico (cStat 215)…',
        step: 'erro',
        source: 'debug',
      },
      {
        at: iso(20),
        message: 'Simulação · gravou lote na temporária (8 CT-e).',
        step: 'temp',
        source: 'debug',
      },
      {
        at: iso(35),
        message: 'Simulação · consulta SEFAZ concluída.',
        step: 'consulta',
        source: 'debug',
      },
    ];

    const alerts: MonitorAlert[] = [
      {
        code: 'SQL_OK',
        severity: 'Info',
        message: 'Conexão SQL disponível para ler tabelas e filas.',
        detectedAtUtc: iso(1),
      },
      {
        code: 'PROC_ON',
        severity: 'Info',
        message: 'Processo ligado e consumindo (Executar=1).',
        detectedAtUtc: iso(1),
      },
      {
        code: 'TEMP_BACKLOG',
        severity: 'Info',
        message: '3 documento(s) na temporária aguardando.',
        detectedAtUtc: iso(1),
      },
      {
        code: 'FILA_EMPTY',
        severity: 'Info',
        message: 'Fila vazia — nenhum item aguardando o próximo serviço.',
        detectedAtUtc: iso(1),
      },
      {
        code: 'SVC_STALE',
        severity: 'Alerta',
        message:
          'Última batida em CAPANEMA desatualizada (há 3h). Verifique se o serviço está vivo.',
        detectedAtUtc: iso(1),
      },
    ];

    const tableHealth: TableHealthView[] = [
      {
        key: 'servico',
        label: 'Serviço (NSU)',
        status: 'Ok',
        primaryValue: 'NSU 900008 · online',
        dataAgeSeconds: 12,
        queryMs: 18,
        hint: 'Simulação · tabela de serviço',
        route: 'servico',
      },
      {
        key: 'log',
        label: 'Log',
        status: 'Atencao',
        primaryValue: 'Último evento: rejeição 215',
        dataAgeSeconds: 8,
        queryMs: 22,
        hint: 'Simulação · há erro mapeado para Ver detalhes',
        route: 'log',
      },
      {
        key: 'temporaria',
        label: 'Temporária',
        status: 'Ok',
        primaryValue: '3 documentos',
        dataAgeSeconds: 20,
        queryMs: 15,
        hint: 'Simulação · backlog leve',
        route: 'temporaria',
      },
      {
        key: 'fila',
        label: 'Fila',
        status: 'Ok',
        primaryValue: '0 na fila',
        dataAgeSeconds: 5,
        queryMs: 11,
        hint: 'Simulação · fila vazia',
        route: 'fila',
      },
      {
        key: 'configuracao',
        label: 'Configuração',
        status: 'Ok',
        primaryValue: '12 chaves ativas',
        dataAgeSeconds: 40,
        queryMs: 9,
        hint: 'Simulação · config',
        route: 'configuracao',
      },
    ];

    return {
      logs,
      liveTrace,
      alerts,
      tableHealth,
      errorModalToken: token,
      criticalErrorSeqLog: criticalSeq,
    };
  }

  private startTablesSimulation(): void {
    this.simulation.set({
      mode: 'tablesFlow',
      systems: [],
      cascadePhase: 'running',
      beltMoving: false,
      lastLoteQtd: 8,
      tables: this.buildTablesSim(),
    });
  }

  private buildTablesSim(): PresentationTablesSim {
    const now = Date.now();
    const iso = (secAgo: number) => new Date(now - secAgo * 1000).toISOString();

    const health: TableHealthView = {
      key: 'servico',
      label: 'Serviço (NSU)',
      status: 'Ok',
      primaryValue: 'NSU 900008 · online',
      dataAgeSeconds: 18,
      queryMs: 14,
      hint: 'Simulação · posição da busca na SEFAZ',
      route: '/tabelas/servico',
    };

    const detail: TableDetailDto = {
      key: 'servico',
      label: 'Serviço (NSU)',
      sessionStartUtc: iso(3600),
      receptionOn: true,
      bannerMessage: null,
      health,
      serviceRows: [
        {
          desServico: 'Receptor CT-e (simulação)',
          nomServidor: 'CAPANEMA',
          nsu: '900008',
          dtcExecucao: iso(18),
          dtcAtualizacao: iso(18),
        },
      ],
      configRows: null,
      tempRows: null,
      logRows: null,
      fila: null,
      contextLogs: [
        {
          seqLog: 9_000_118,
          dtcLog: iso(25),
          mensagem:
            'Consulta SEFAZ cStat=118 — documentos localizados. NSU 900001 → 900008.',
          cStat: '118',
          severityHint: 'success',
          threadId: 1,
        },
        {
          seqLog: 9_000_108,
          dtcLog: iso(90),
          mensagem: 'Serviço de distribuição temporariamente indisponível (cStat 108).',
          cStat: '108',
          severityHint: 'warning',
          threadId: 2,
        },
        {
          seqLog: 9_000_001,
          dtcLog: iso(120),
          mensagem: 'Avanço de NSU na sessão: posição atual 900008.',
          cStat: null,
          severityHint: 'info',
          threadId: 1,
        },
      ],
      takeApplied: 100,
      rowCount: 1,
    };

    const tableHealth: TableHealthView[] = [
      health,
      {
        key: 'configuracao',
        label: 'Configuração',
        status: 'Ok',
        primaryValue: '12 chave(s)',
        dataAgeSeconds: 40,
        queryMs: 9,
        hint: 'Simulação · config',
        route: '/tabelas/configuracao',
      },
      {
        key: 'temporaria',
        label: 'Temporária',
        status: 'Ok',
        primaryValue: '3 documentos',
        dataAgeSeconds: 20,
        queryMs: 15,
        hint: 'Simulação · backlog leve',
        route: '/tabelas/temporaria',
      },
      {
        key: 'log',
        label: 'Log',
        status: 'Ok',
        primaryValue: '4 evento(s)',
        dataAgeSeconds: 8,
        queryMs: 22,
        hint: 'Simulação · eventos recentes',
        route: '/tabelas/log',
      },
      {
        key: 'fila',
        label: 'Fila Arquivador',
        status: 'Ok',
        primaryValue: 'Fila vazia',
        dataAgeSeconds: 5,
        queryMs: 11,
        hint: 'Simulação · fila vazia',
        route: '/tabelas/fila',
      },
    ];

    return { detail, tableHealth };
  }

  private startThreadsSimulation(): void {
    this.simulation.set({
      mode: 'threadsFlow',
      systems: [],
      cascadePhase: 'running',
      beltMoving: false,
      lastLoteQtd: 8,
      threads: this.buildThreadsSim(),
    });
  }

  private buildThreadsSim(): PresentationThreadsSim {
    const now = Date.now();
    const iso = (secAgo: number) => new Date(now - secAgo * 1000).toISOString();

    const threads: ThreadView[] = [
      {
        threadId: 1,
        role: 'principal',
        nsuSource: 'num_sequencial_unico',
        indDFe: 2,
        nsuAtual: '900008',
        isIdle: false,
        outsideDatabase: false,
        lastActivityAt: iso(12),
        lastCStat: '118',
        lastSeverityHint: 'success',
        lastActivityHint: 'Consulta SEFAZ ok',
      },
      {
        threadId: 2,
        role: 'auxiliar',
        nsuSource: 'NSUAux',
        indDFe: 0,
        nsuAtual: '0',
        isIdle: true,
        outsideDatabase: false,
        lastActivityAt: null,
        lastCStat: null,
        lastSeverityHint: null,
        lastActivityHint: null,
      },
      {
        threadId: 3,
        role: 'arquivo',
        nsuSource: 'NSU.txt',
        indDFe: 2,
        nsuAtual: null,
        isIdle: false,
        outsideDatabase: true,
        lastActivityAt: iso(45),
        lastCStat: null,
        lastSeverityHint: 'info',
        lastActivityHint: 'Posição em arquivo',
      },
      {
        threadId: 4,
        role: 'autorizacao',
        nsuSource: 'NSUAuxAut',
        indDFe: 0,
        nsuAtual: '450120',
        isIdle: false,
        outsideDatabase: false,
        lastActivityAt: iso(20),
        lastCStat: '137',
        lastSeverityHint: 'info',
        lastActivityHint: 'Sem documentos novos',
      },
      {
        threadId: 5,
        role: 'destinatario',
        nsuSource: 'NSUAuxDest',
        indDFe: 1,
        nsuAtual: '880001',
        isIdle: false,
        outsideDatabase: false,
        lastActivityAt: iso(900),
        lastCStat: null,
        lastSeverityHint: null,
        lastActivityHint: null,
      },
    ];

    const logs: LogEntry[] = [
      {
        seqLog: 9_100_118,
        dtcLog: iso(12),
        mensagem: 'Thread 1 · Consulta SEFAZ cStat=118 — lote gravado. NSU 900001 → 900008.',
        cStat: '118',
        severityHint: 'success',
        threadId: 1,
      },
      {
        seqLog: 9_100_137,
        dtcLog: iso(20),
        mensagem: 'Thread 4 · cStat=137 — nenhum documento na faixa solicitada.',
        cStat: '137',
        severityHint: 'info',
        threadId: 4,
      },
      {
        seqLog: 9_100_001,
        dtcLog: iso(45),
        mensagem: 'Thread 3 · posição lida de NSU.txt no servidor.',
        cStat: null,
        severityHint: 'info',
        threadId: 3,
      },
    ];

    return {
      threads,
      logs,
      nsuDeltas: {
        1: 8,
        2: null,
        3: null,
        4: 0,
        5: null,
      },
      intervaloSeconds: 60,
    };
  }

  private pushFlowFrame(stage: number): void {
    const systems: ChainSystemView[] = CHAIN_IDS.map((id, i) => {
      const isAgora = i === stage;
      const depth = isAgora ? 42 + stage * 3 : i < stage ? 0 : i === stage + 1 ? 12 : 0;
      return {
        id,
        symbol: SYMBOLS[id],
        label: LABELS[id],
        status: 'running',
        executar: 1,
        agora: isAgora,
        metricPill: depth > 0 ? `fila ${depth}` : '0',
        hint: isAgora ? 'Simulação · processando' : 'Simulação',
        enabled: true,
        hasQueueWork: depth > 0,
        queueDepth: depth,
        processHint: isAgora ? 'Simulação · drenando fila' : null,
      };
    });

    this.simulation.set({
      mode: 'flow',
      systems,
      cascadePhase: 'running',
      beltMoving: true,
      lastLoteQtd: 8,
    });
  }

  private pushReceptorFrame(stageIdx: number): void {
    const stage = RECEPTOR_STAGES[stageIdx] ?? 'sefaz';
    const lane = stageIdx;
    const packets: PresentationSimPacket[] =
      stage === 'sefaz' || stage === 'consulta'
        ? []
        : [
            {
              id: `sim-cte-${stageIdx}`,
              kind: stage === 'broker' || stage === 'arquivador' ? 'envelope' : 'cte',
              label: '90000' + String(stageIdx + 1),
              lane,
            },
          ];

    this.simulation.set({
      mode: 'receptorFlow',
      systems: [],
      cascadePhase: 'running',
      beltMoving: stage === 'temp' || stage === 'broker' || stage === 'arquivador',
      lastLoteQtd: 8,
      receptorStage: stage,
      receptorCaption: RECEPTOR_CAPTIONS[stage],
      receptorPackets: packets,
    });
  }

  private buildStoppedBacklog(): PresentationSimulation {
    const systems: ChainSystemView[] = CHAIN_IDS.map((id) => {
      const isAnalisador = id === 'analisador';
      const depth = isAnalisador ? 677 : 0;
      return {
        id,
        symbol: SYMBOLS[id],
        label: LABELS[id],
        status: 'stopped',
        executar: 0,
        agora: false,
        metricPill: depth > 0 ? `fila ${depth}` : '0',
        hint: isAnalisador
          ? 'Simulação · backlog parado'
          : 'Simulação · parado',
        enabled: true,
        hasQueueWork: depth > 0,
        queueDepth: depth,
        processHint: isAnalisador
          ? 'Na fila · aguardando Ligar as filas'
          : null,
      };
    });

    return {
      mode: 'stoppedBacklog',
      systems,
      cascadePhase: 'idle',
      beltMoving: false,
      lastLoteQtd: 4,
    };
  }

  private stopFlowTimer(): void {
    if (this.flowTimer) {
      clearInterval(this.flowTimer);
      this.flowTimer = undefined;
    }
    for (const t of this.detailsTimers) clearTimeout(t);
    this.detailsTimers = [];
  }

  private scrollTarget(selector?: string): void {
    if (!selector || typeof document === 'undefined') return;
    const el = document.querySelector(selector);
    if (!el) return;
    el.scrollIntoView({ behavior: 'smooth', block: 'nearest', inline: 'nearest' });
  }
}
