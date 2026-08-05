export type PipelineStage =
  | 'bootstrap'
  | 'fila'
  | 'temp'
  | 'sintetizador'
  | 'analisador'
  | 'integrador';

export interface PipelineActivity {
  stage: PipelineStage;
  headline: string;
  detail: string;
  threadId?: number | null;
  nsu?: string | null;
  cStat?: string | null;
  at?: string | null;
  source?: 'sql' | 'debug' | string;
}

export interface LogLike {
  mensagem?: string | null;
  threadId?: number | null;
  cStat?: string | null;
  dtcLog?: string | null;
  seqLog?: number;
  source?: string;
}

/** Janela mínima; o painel usa max(isto, intervalo×1.5). */
export const PIPELINE_ACTIVITY_MAX_AGE_MS = 120_000;

export function threadFriendlyName(threadId?: number | null): string {
  if (threadId == null || threadId <= 0) return 'linha de trabalho';
  return `Linha ${threadId}`;
}

function extractNsu(msg: string): string | null {
  const m =
    msg.match(/nsu\s*:\s*(\d+)/i) ||
    msg.match(/n[ºo°]?\s*:\s*(\d+)/i) ||
    msg.match(/chave\s*:\s*(\d+)/i) ||
    msg.match(/nsu[:\s]+(\d+)/i) ||
    msg.match(/threads\s*=\s*(\d+)/i);
  return m?.[1] ?? null;
}

function extractMachine(msg: string): string | null {
  const m = msg.match(/;\s*([A-Z0-9_-]+)\s*$/i) || msg.match(/máquina:\s*([^\s;]+)/i);
  return m?.[1] ?? null;
}

function extractCert(msg: string): string | null {
  const m = msg.match(/certificado\s*=\s*(.+)$/i);
  return m?.[1]?.trim() ?? null;
}

function extractIntervalo(msg: string): string | null {
  const m = msg.match(/intervalo\s*=\s*(\d+)/i);
  return m?.[1] ?? null;
}

function extractThreadsCfg(msg: string): string | null {
  const m = msg.match(/threads\s*=\s*(\d+)/i);
  return m?.[1] ?? null;
}

/**
 * Interpreta log SQL ou linha Debug (Saída do VS / monitor-live.log).
 */
export function describeLogActivity(log: LogLike): PipelineActivity | null {
  const raw = log.mensagem ?? '';
  const msg = raw.toLowerCase();
  if (!msg.trim()) return null;

  const threadId = log.threadId ?? parseThreadFromMessage(raw);
  const threadName = threadFriendlyName(threadId);
  const cStat = log.cStat ?? extractCStat(raw);
  const nsu = extractNsu(raw);
  const source = log.source;

  // —— Bootstrap ——
  if (
    msg.includes('conexao iniciando') ||
    msg.includes('conexão iniciando') ||
    (msg.includes('devhost') && msg.includes('iniciando'))
  ) {
    const cert = extractCert(raw);
    return {
      stage: 'bootstrap',
      headline: 'Conectando no banco',
      detail: cert
        ? `Abrindo conexão SQL DEV · certificado ${cert}`
        : 'Abrindo conexão com o banco',
      at: log.dtcLog,
      source,
    };
  }

  if (msg.includes('config banco ok') || (msg.includes('bootstrap') && msg.includes('config'))) {
    const th = extractThreadsCfg(raw);
    const iv = extractIntervalo(raw);
    const ivSec = iv ? Math.round(Number(iv) / (Number(iv) >= 1000 ? 1000 : 1)) : null;
    return {
      stage: 'bootstrap',
      headline: 'Configuração OK',
      detail: [
        th ? `${th} linha(s) de trabalho` : null,
        ivSec != null && !Number.isNaN(ivSec)
          ? `ciclo de arquivamento a cada ${ivSec}s`
          : null,
      ]
        .filter(Boolean)
        .join(' · ') || 'Configuração lida do banco com sucesso',
      at: log.dtcLog,
      source,
    };
  }

  if (
    msg.includes('startdebug concluído') ||
    msg.includes('workers em loop') ||
    msg.includes('carregando serviço')
  ) {
    return {
      stage: 'bootstrap',
      headline: 'Inicialização',
      detail: raw.replace(/^\[.*?\]\s*/g, '').slice(0, 160),
      at: log.dtcLog,
      source,
    };
  }

  if (msg.includes('serviço iniciado com sucesso')) {
    const machine = extractMachine(raw);
    return {
      stage: 'bootstrap',
      headline: 'Arquivador ligado',
      detail: machine
        ? `Serviço iniciou na máquina ${machine}`
        : 'Serviço do Arquivador acabou de iniciar',
      threadId,
      at: log.dtcLog,
      source,
    };
  }

  if (msg.includes('processo não iniciado')) {
    return {
      stage: 'bootstrap',
      headline: 'Ciclo bloqueado',
      detail: 'Executar=0 — não arquiva até ligar o Arquivador',
      threadId,
      at: log.dtcLog,
      source,
    };
  }

  if (msg.includes('processo iniciado com sucesso')) {
    return {
      stage: 'fila',
      headline: 'Ciclo começou',
      detail: `${threadName} iniciou o loop de arquivamento`,
      threadId,
      at: log.dtcLog,
      source,
    };
  }

  // —— Fila (entrada) — Constante.MsgChaveRetiradaFila / MsgChaveInseridaFila ——
  if (
    msg.includes('chave retirada da fila') ||
    msg.includes('retirada da fila') ||
    msg.includes('retirar fila')
  ) {
    return {
      stage: 'fila',
      headline: 'Retirou da fila',
      detail: nsu
        ? `${threadName} retirou NSU ${nsu} da fila de entrada`
        : `${threadName} retirou mensagem da fila Arquivador`,
      threadId,
      nsu,
      at: log.dtcLog,
      source,
    };
  }

  if (
    msg.includes('chave inserida na fila') ||
    msg.includes('chave do documento inserida na fila') ||
    (msg.includes('fila') && msg.includes('vazia'))
  ) {
    return {
      stage: 'fila',
      headline: 'Fila de entrada',
      detail: nsu
        ? `${threadName} · NSU ${nsu}`
        : `${threadName} na fila Arquivador`,
      threadId,
      nsu,
      at: log.dtcLog,
      source,
    };
  }

  // —— Temp Recepção — Constante.MsgLoteObtidoBanco / MsgDadoNaoObtidoBanco ——
  if (msg.includes('lote obtido no banco') || msg.includes('dado obtido no banco')) {
    return {
      stage: 'temp',
      headline: 'Leu temp recepção',
      detail: nsu
        ? `${threadName} obteve lote NSU ${nsu} na temporária`
        : `${threadName} leu a temporária de recepção`,
      threadId,
      nsu,
      at: log.dtcLog,
      source,
    };
  }

  if (
    msg.includes('nenhum dado obtido no banco') ||
    msg.includes('dado não obtido') ||
    msg.includes('dado nao obtido')
  ) {
    return {
      stage: 'temp',
      headline: 'Dado não obtido no temp',
      detail: nsu
        ? `${threadName} · temp sem dado · NSU ${nsu}`
        : `${threadName} não encontrou dado na temporária`,
      threadId,
      nsu,
      at: log.dtcLog,
      source,
    };
  }

  // —— Fan-out (filas destino; logs explícitos ou inferidos) ——
  if (
    msg.includes('enviar fila sintetizador') ||
    msg.includes('fila sintetizador') ||
    msg.includes('sintetizador')
  ) {
    return {
      stage: 'sintetizador',
      headline: 'Sintetizador',
      detail: nsu
        ? `${threadName} enviou NSU ${nsu} à fila do sintetizador`
        : `${threadName} encaminhou para o sintetizador`,
      threadId,
      nsu,
      at: log.dtcLog,
      source,
    };
  }

  if (
    msg.includes('enviar fila analisador') ||
    msg.includes('fila analisador') ||
    msg.includes('analisador') ||
    msg.includes('analizador')
  ) {
    return {
      stage: 'analisador',
      headline: 'Analisador',
      detail: nsu
        ? `${threadName} enviou NSU ${nsu} ao analisador`
        : `${threadName} no analisador`,
      threadId,
      nsu,
      at: log.dtcLog,
      source,
    };
  }

  // —— Integrador / fim do ciclo — Constante.MsgDocExcluido ——
  if (
    msg.includes('documento excluído do banco') ||
    msg.includes('documento excluido do banco') ||
    msg.includes('excluir temp') ||
    msg.includes('integrador')
  ) {
    return {
      stage: 'integrador',
      headline: 'Integrador',
      detail: nsu
        ? `${threadName} arquivou e excluiu temp · NSU ${nsu}`
        : `${threadName} concluiu arquivamento (excluiu temp)`,
      threadId,
      nsu,
      at: log.dtcLog,
      source,
    };
  }

  // Fallback cStat genérico (sem narrativa de consulta externa)
  if (cStat) {
    return {
      stage: 'fila',
      headline: 'Ciclo de arquivamento',
      detail: `Resposta cStat ${cStat}${nsu ? ` · NSU ${nsu}` : ''}`,
      threadId,
      nsu,
      cStat,
      at: log.dtcLog,
      source,
    };
  }

  return null;
}

function parseThreadFromMessage(raw: string): number | null {
  const m = raw.match(/thread:\s*(\d+)/i);
  if (!m) return null;
  const n = Number(m[1]);
  return Number.isFinite(n) ? n : null;
}

function extractCStat(raw: string): string | null {
  const explicit = raw.match(/cstat[^0-9]*(\d{3})/i);
  return explicit?.[1] ?? null;
}

export function resolvePipelineActivity(
  logs: LogLike[],
  options?: { maxAgeMs?: number; nowMs?: number }
): PipelineActivity | null {
  const maxAge = options?.maxAgeMs ?? PIPELINE_ACTIVITY_MAX_AGE_MS;
  const now = options?.nowMs ?? Date.now();

  const newestFirst = [...logs].sort((a, b) => {
    const sa = a.seqLog ?? 0;
    const sb = b.seqLog ?? 0;
    if (sb !== sa) return sb - sa;
    const ta = a.dtcLog ? new Date(a.dtcLog).getTime() : 0;
    const tb = b.dtcLog ? new Date(b.dtcLog).getTime() : 0;
    return tb - ta;
  });

  for (const log of newestFirst.slice(0, 60)) {
    const activity = describeLogActivity(log);
    if (!activity) continue;

    if (log.dtcLog) {
      const age = now - new Date(log.dtcLog).getTime();
      if (Number.isFinite(age) && age > maxAge) {
        continue;
      }
    }

    return activity;
  }

  return null;
}

export function pipelineStageLabel(stage: PipelineStage): string {
  switch (stage) {
    case 'bootstrap':
      return 'Inicialização';
    case 'fila':
      return 'Fila Arquivador';
    case 'temp':
      return 'Temp Recepção';
    case 'sintetizador':
      return 'Sintetizador';
    case 'analisador':
      return 'Analisador';
    case 'integrador':
      return 'Integrador';
  }
}
