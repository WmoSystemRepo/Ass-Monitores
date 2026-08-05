import { LogEntry, ThreadView } from '@orquestrador/shared-data';
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
  /** Executar do serviço (1 = recepção ativa). */
  executar: number;
  /** Processo Receptor em execução (SCM/DevHost). */
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

/** Missão de cada linha — o que ela faz (1º nível, sem jargão). */
export function threadMission(threadId: number): string {
  switch (threadId) {
    case 1:
      return 'Busca os CT-e novos na SEFAZ — é a linha principal da recepção.';
    case 2:
      return 'Busca complementar (auxiliar). Só entra em ação quando o contador desta linha for ligado.';
    case 3:
      return 'Busca documentos de autorização. Fica parada enquanto o contador estiver em zero.';
    case 4:
      return 'Busca documentos pelo destinatário. Fica parada enquanto o contador estiver em zero.';
    case 5:
      return 'Usa um arquivo local no servidor em vez do banco — o monitor não vê o número daqui.';
    default:
      return 'Linha de busca de documentos na SEFAZ.';
  }
}

/** Nome amigável da linha (título do card). */
export function threadCardTitle(threadId: number, role: string): string {
  return `${role} · linha ${threadId}`;
}

/** Onde a posição da busca é guardada — leigo + técnico no title. */
export function nsuSourceFriendly(nsuSource: string): { label: string; technical: string } {
  switch (nsuSource) {
    case 'num_sequencial_unico':
      return { label: 'No banco (serviço principal)', technical: nsuSource };
    case 'NSUAux':
      return { label: 'No banco (contador auxiliar)', technical: nsuSource };
    case 'NSUAuxAut':
      return { label: 'No banco (contador de autorização)', technical: nsuSource };
    case 'NSUAuxDest':
      return { label: 'No banco (contador de destinatário)', technical: nsuSource };
    case 'NSU.txt':
      return { label: 'Em arquivo no servidor', technical: nsuSource };
    default:
      return { label: nsuSource, technical: nsuSource };
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

/** Explica o Δ da posição da busca (só com recepção ligada). */
export function nsuDeltaMeaning(delta: number | null): string {
  if (delta == null) return 'Ainda medindo se a posição avançou…';
  if (delta === 0) return 'A posição não mudou desde a última atualização da tela.';
  if (delta > 0) return `Avançou ${delta} posição(ões) — a busca está progredindo.`;
  return `Recuou ${Math.abs(delta)} — confira no Histórico se houve ajuste.`;
}

/** Mensagem quando não há eventos recentes (só modo ligado). */
export function emptyEventsMeaning(status: ThreadRunStatus): string {
  switch (status) {
    case 'idle':
      return 'Nada a mostrar: esta linha não está buscando (contador em zero).';
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
 * Chip neutro quando a recepção está parada.
 * O banner da página explica o global — cards não repetem o alerta.
 */
export function awaitingReceptionStatus(): ThreadStatusResult {
  return {
    status: 'paused',
    label: 'Aguardando recepção',
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
 * Status específico da linha — usar só com recepção ligada.
 * Se a recepção estiver parada, a página deve usar `awaitingReceptionStatus()`.
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
        'Esta linha guarda a posição da busca em arquivo no servidor, não no banco — o número não aparece nesta tela.',
    };
  }

  if (thread.isIdle) {
    return {
      status: 'idle',
      label: 'Não busca agora',
      meaning:
        'Contador em zero: a linha existe, mas está desligada de propósito até alguém ativar essa busca auxiliar.',
    };
  }

  const windowMs = threadEvidenceWindowMs(input.intervaloSeconds);
  const at = thread.lastActivityAt ? new Date(thread.lastActivityAt).getTime() : NaN;
  const recentLog = Number.isFinite(at) && now - at <= windowMs;
  const moved = nsuDelta != null && nsuDelta !== 0;

  if (recentLog || moved) {
    return {
      status: 'in_cycle',
      label: 'Buscando',
      meaning: 'Há sinal recente de atividade — a linha consultou ou avançou a posição da busca.',
    };
  }

  return {
    status: 'no_evidence',
    label: 'Sem atividade recente',
    meaning:
      'A linha está apta a buscar, mas não há registro recente no banco. Pode estar entre um ciclo e outro.',
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
