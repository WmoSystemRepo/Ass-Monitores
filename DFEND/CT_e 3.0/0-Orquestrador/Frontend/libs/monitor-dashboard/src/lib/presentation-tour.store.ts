import { Injectable, computed, inject, signal } from '@angular/core';
import { Router } from '@angular/router';
import type { CascadePhase, ChainSystemView } from '@orquestrador/shared-data';
import {
  PRESENTATION_STEPS,
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

export interface PresentationSimulation {
  mode: PresentationSimulateMode;
  systems: ChainSystemView[];
  cascadePhase: CascadePhase;
  beltMoving: boolean;
  lastLoteQtd: number;
}

@Injectable({ providedIn: 'root' })
export class PresentationTourStore {
  private readonly router = inject(Router);
  private flowTimer?: ReturnType<typeof setInterval>;
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

  readonly isSimulating = computed(() => {
    const s = this.step()?.simulate ?? 'none';
    return s === 'flow' || s === 'stoppedBacklog';
  });

  readonly panelPlacement = computed((): 'top' | 'bottom' => {
    return this.step()?.panelPlacement === 'top' ? 'top' : 'bottom';
  });

  start(): void {
    this.stopFlowTimer();
    this.active.set(true);
    this.stepIndex.set(0);
    void this.applyStep(0);
  }

  /** Reinicia a apresentação do primeiro passo (disponível a qualquer momento). */
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
    }

    queueMicrotask(() => {
      if (gen !== this.applyGeneration) return;
      this.scrollTarget(step.target);
    });
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
  }

  private scrollTarget(selector?: string): void {
    if (!selector || typeof document === 'undefined') return;
    const el = document.querySelector(selector);
    if (!el) return;
    el.scrollIntoView({ behavior: 'smooth', block: 'nearest', inline: 'nearest' });
  }
}
