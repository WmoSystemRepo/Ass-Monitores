import { LogEntry, ThreadView } from '@analisador/shared-data';
import { PIPELINE_ACTIVITY_MAX_AGE_MS } from './pipeline-activity';

/** Estado operacional da linha de trabalho (UI 1º nível). */
export type ThreadRunStatus =
  | 'paused'
  | 'idle'
  | 'outside_db'
  | 'in_cycle'
  | 'no_evidence';

export interface ThreadStatusInput {
  thread: ThreadView;
  /** Executar do serviço (1 = síntese ativa). */
  executar: number;
  /** Processo Analisador em execução (SCM/DevHost). */
  processRunning: boolean;
  intervaloSeconds: number;
  /** Δ NSU desde o snapshot anterior (null = ainda sem baseline). */
  nsuDelta: number | null;
  /** Agora (ms) — injetável para testes. */
  nowMs?: number;
}

export interface ThreadStatusResult {
  status: ThreadRunStatus;
  /** Chip curto (1–3 palavras). */
  label: string;
  /** Frase leiga: o que o status significa agora (vazio se o banner global cobre). */
  meaning: string;
}

/** Missão de cada linha — workers genéricos do pool. */
export function threadMission(threadId: number): string {
  if (threadId === 1) {
    return 'Worker principal do pool — processa o ciclo de síntese.';
  }
  return `Worker do pool (linha ${threadId}) — processa itens do ciclo em paralelo.`;
}

/** Nome amigável da linha (título do card). */
export function threadCardTitle(threadId: number, role: string): string {
  const name = role?.trim() ? role : `Linha ${threadId}`;
  return `${name} · linha ${threadId}`;
}

/** Onde o estado da linha é guardado — leigo + técnico no title. */
export function nsuSourceFriendly(nsuSource: string): { label: string; technical: string } {
  switch (nsuSource) {
    case 'num_sequencial_unico':
      return { label: 'No banco (serviço principal)', technical: nsuSource };
    case 'NSUAux':
      return { label: 'No banco (contador auxiliar)', technical: nsuSource };
    case 'NSUAuxAut':
      return { label: 'No banco (contador auxiliar)', technical: nsuSource };
    case 'NSUAuxDest':
      return { label: 'No banco (contador auxiliar)', technical: nsuSource };
    case 'NSU.txt':
      return { label: 'Em arquivo no servidor', technical: nsuSource };
    default:
      return { label: nsuSource || 'Não informado', technical: nsuSource };
  }
}

/** Tipo de documento sem código no 1º nível. */
export function documentKindLabel(indDFe: number): string {
  switch (indDFe) {
    case 0:
      return 'Autorizações';
    case 1:
      return 'Documentos do destinatário';
    case 2:
      return 'CT-e e DF-e';
    default:
      return `Tipo ${indDFe}`;
  }
}

/** Explica o Δ da posição (só com síntese ligada). */
export function nsuDeltaMeaning(delta: number | null): string {
  if (delta == null) return 'Ainda medindo se a posição avançou…';
  if (delta === 0) return 'A posição não mudou desde a última atualização da tela.';
  if (delta > 0) return `Avançou ${delta} posição(ões) — o worker está progredindo.`;
  return `Recuou ${Math.abs(delta)} — confira no Histórico se houve ajuste.`;
}

/** Mensagem quando não há eventos recentes (só modo ligado). */
export function emptyEventsMeaning(status: ThreadRunStatus): string {
  switch (status) {
    case 'idle':
      return 'Nada a mostrar: esta linha não está processando (contador em zero).';
    case 'outside_db':
      return 'Eventos desta linha podem não aparecer aqui (controle fora do banco).';
    case 'paused':
      return '';
    case 'no_evidence':
      return 'Ainda não chegou registro recente desta linha no histórico do banco.';
    case 'in_cycle':
      return 'A linha está ativa, mas o último registro ainda não apareceu na lista.';
    default:
      return 'Nenhum registro recente desta linha.';
  }
}

/**
 * Chip neutro quando o Analisador está parado.
 * O banner da página explica o global — cards não repetem o alerta.
 */
export function awaitingReceptionStatus(): ThreadStatusResult {
  return {
    status: 'paused',
    label: 'Aguardando Analisador',
    meaning: '',
  };
}

export interface ThreadStatusSummary {
  inCycle: number;
  idle: number;
  outsideDb: number;
  noEvidence: number;
  paused: number;
}

/** Janela de evidência recente: max(2 min, 1.5 × Intervalo). */
export function threadEvidenceWindowMs(intervaloSeconds: number): number {
  let intervalo = intervaloSeconds > 0 ? intervaloSeconds : 60;
  if (intervalo >= 1000) intervalo = Math.round(intervalo / 1000);
  return Math.max(PIPELINE_ACTIVITY_MAX_AGE_MS, Math.round(intervalo * 1.5 * 1000));
}

/**
 * Status específico da linha — usar só com Analisador ligado.
 * Se estiver parado, a página deve usar `awaitingReceptionStatus()`.
 */
export function resolveThreadRunStatus(input: ThreadStatusInput): ThreadStatusResult {
  const { thread, executar, processRunning, nsuDelta } = input;
  const now = input.nowMs ?? Date.now();

  if (!processRunning || executar !== 1) {
    return awaitingReceptionStatus();
  }

  if (thread.outsideDatabase) {
    return {
      status: 'outside_db',
      label: 'Arquivo local',
      meaning:
        'Esta linha guarda estado em arquivo no servidor, não no banco — o número não aparece nesta tela.',
    };
  }

  if (thread.isIdle) {
    return {
      status: 'idle',
      label: 'Não processa agora',
      meaning:
        'Contador em zero: a linha existe, mas está desligada de propósito até alguém ativar esse worker.',
    };
  }

  const windowMs = threadEvidenceWindowMs(input.intervaloSeconds);
  const at = thread.lastActivityAt ? new Date(thread.lastActivityAt).getTime() : NaN;
  const recentLog = Number.isFinite(at) && now - at <= windowMs;
  const moved = nsuDelta != null && nsuDelta !== 0;

  if (recentLog || moved) {
    return {
      status: 'in_cycle',
      label: 'Processando',
      meaning: 'Há sinal recente de atividade — a linha processou ou avançou no ciclo.',
    };
  }

  return {
    status: 'no_evidence',
    label: 'Sem atividade recente',
    meaning:
      'A linha está apta a processar, mas não há registro recente no banco. Pode estar entre um ciclo e outro.',
  };
}

export function summarizeThreadStatuses(
  results: readonly ThreadStatusResult[]
): ThreadStatusSummary {
  const summary: ThreadStatusSummary = {
    inCycle: 0,
    idle: 0,
    outsideDb: 0,
    noEvidence: 0,
    paused: 0,
  };
  for (const r of results) {
    switch (r.status) {
      case 'in_cycle':
        summary.inCycle++;
        break;
      case 'idle':
        summary.idle++;
        break;
      case 'outside_db':
        summary.outsideDb++;
        break;
      case 'no_evidence':
        summary.noEvidence++;
        break;
      case 'paused':
        summary.paused++;
        break;
    }
  }
  return summary;
}

/** Parse NSU string (pode ter zeros à esquerda) → número ou null. */
export function parseNsuValue(nsu?: string | null): number | null {
  if (nsu == null || nsu === '' || nsu === '—') return null;
  const n = Number(nsu);
  return Number.isFinite(n) ? n : null;
}

/**
 * Δ entre NSU atual e o valor anterior conhecido.
 * Retorna null se não houver baseline ou se algum lado for inválido.
 */
export function computeNsuDelta(
  current?: string | null,
  previous?: string | null
): number | null {
  const cur = parseNsuValue(current);
  const prev = parseNsuValue(previous);
  if (cur == null || prev == null) return null;
  return cur - prev;
}

/** Texto curto do Δ para a UI (↑12 · ↓3 · —). */
export function formatNsuDelta(delta: number | null): string {
  if (delta == null) return '—';
  if (delta === 0) return '0';
  if (delta > 0) return `↑${delta}`;
  return `↓${Math.abs(delta)}`;
}

/** indDFe → label leigo (tooltip guarda o código técnico). */
export function indDfeLabel(indDFe: number): string {
  switch (indDFe) {
    case 0:
      return 'Autorização (0)';
    case 1:
      return 'Destinatário (1)';
    case 2:
      return 'CT-e / DF-e (2)';
    default:
      return `Tipo ${indDFe}`;
  }
}

/** Últimos N eventos do buffer para uma linha (mais recentes primeiro). */
export function recentLogsForThread(
  logs: readonly LogEntry[],
  threadId: number,
  take = 3
): LogEntry[] {
  return [...logs]
    .filter((l) => l.threadId === threadId)
    .sort((a, b) => b.seqLog - a.seqLog)
    .slice(0, take);
}
