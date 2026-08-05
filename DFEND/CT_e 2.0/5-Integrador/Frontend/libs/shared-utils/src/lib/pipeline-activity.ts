export type PipelineStage =
  | 'bootstrap'
  | 'fila'
  | 'temp'
  | 'classificar'
  | 'persistir'
  | 'limpar';

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
 * Interpreta log SQL ou linha Debug (Saída do VS / monitor-live.log)
 * para o ciclo do Integrador: fila → temp → classificar → persistir → limpar.
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
          ? `ciclo de síntese a cada ${ivSec}s`
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
      headline: 'Integrador ligado',
      detail: machine
        ? `Serviço iniciou na máquina ${machine}`
        : 'Serviço do Integrador acabou de iniciar',
      threadId,
      at: log.dtcLog,
      source,
    };
  }

  if (msg.includes('processo não iniciado')) {
    return {
      stage: 'bootstrap',
      headline: 'Ciclo bloqueado',
      detail: 'Executar=0 — não sintetiza até ligar o Integrador',
      threadId,
      at: log.dtcLog,
      source,
    };
  }

  if (msg.includes('processo iniciado com sucesso')) {
    return {
      stage: 'fila',
      headline: 'Ciclo começou',
      detail: `${threadName} iniciou o loop de síntese`,
      threadId,
      at: log.dtcLog,
      source,
    };
  }

  // —— Fila — RECEIVE / Constante.MsgChaveRetiradaFila / MsgChaveInseridaFila ——
  if (
    msg.includes('chave retirada da fila') ||
    msg.includes('retirada da fila') ||
    msg.includes('retirar fila') ||
    msg.includes('receive')
  ) {
    return {
      stage: 'fila',
      headline: 'Retirou da fila',
      detail: nsu
        ? `${threadName} retirou NSU ${nsu} da fila_alvo_cte_integrador`
        : `${threadName} retirou mensagem da fila Integrador`,
      threadId,
      nsu,
      at: log.dtcLog,
      source,
    };
  }

  if (
    msg.includes('chave inserida na fila') ||
    msg.includes('chave do documento inserida na fila') ||
    msg.includes('chave do lote inserida na fila') ||
    (msg.includes('fila') && msg.includes('vazia'))
  ) {
    return {
      stage: 'fila',
      headline: 'Fila de entrada',
      detail: nsu
        ? `${threadName} · NSU ${nsu}`
        : `${threadName} na fila Integrador`,
      threadId,
      nsu,
      at: log.dtcLog,
      source,
    };
  }

  // —— Temp — Constante.MsgLoteObtidoBanco / MsgDadoNaoObtidoBanco ——
  if (msg.includes('lote obtido no banco') || msg.includes('dado obtido no banco')) {
    return {
      stage: 'temp',
      headline: 'Leu temporária',
      detail: nsu
        ? `${threadName} obteve lote NSU ${nsu} em tmp_integrador`
        : `${threadName} leu a temporária do integrador`,
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

  // —— Limpar — DELETE temp / AtualizarErro (antes de persistir genérico) ——
  if (
    msg.includes('documento excluído do banco') ||
    msg.includes('documento excluido do banco') ||
    msg.includes('excluir lote') ||
    msg.includes('excluir temp') ||
    msg.includes('atualizarerro') ||
    msg.includes('atualizar erro') ||
    msg.includes('mensagem erro') ||
    msg.includes('des_mensagem_erro')
  ) {
    const isError =
      msg.includes('atualizarerro') ||
      msg.includes('atualizar erro') ||
      msg.includes('mensagem erro') ||
      msg.includes('des_mensagem_erro');
    return {
      stage: 'limpar',
      headline: isError ? 'AtualizarErro' : 'Limpou temp',
      detail: nsu
        ? isError
          ? `${threadName} gravou erro no temp · NSU ${nsu}`
          : `${threadName} excluiu temp · NSU ${nsu}`
        : isError
          ? `${threadName} AtualizarErro na temporária`
          : `${threadName} concluiu síntese (excluiu temp)`,
      threadId,
      nsu,
      at: log.dtcLog,
      source,
    };
  }

  // Documento atualizado = AtualizarErro grava des_mensagem_erro
  if (msg.includes('documento atualizado no banco')) {
    return {
      stage: 'limpar',
      headline: 'AtualizarErro',
      detail: nsu
        ? `${threadName} atualizou temp com erro · NSU ${nsu}`
        : `${threadName} atualizou temp (erro)`,
      threadId,
      nsu,
      at: log.dtcLog,
      source,
    };
  }

  // —— Persistir — INSERT documento_* ——
  if (
    msg.includes('documento de autorização inserido') ||
    msg.includes('documento de autorizacao inserido') ||
    msg.includes('documento de evento inserido') ||
    msg.includes('documento de inutilização inserido') ||
    msg.includes('documento de inutilizacao inserido') ||
    msg.includes('documento do item inserido') ||
    msg.includes('inserção de documento já existente') ||
    msg.includes('insercao de documento ja existente') ||
    msg.includes('nsu faltante') ||
    msg.includes('furos de nsu') ||
    msg.includes('inserido no banco')
  ) {
    return {
      stage: 'persistir',
      headline: 'Persistiu',
      detail: nsu
        ? `${threadName} gravou documento_* · NSU ${nsu}`
        : `${threadName} INSERT documento_*`,
      threadId,
      nsu,
      at: log.dtcLog,
      source,
    };
  }

  // —— Classificar — SintetizarLote / schema routing ——
  if (
    msg.includes('sintetizar') ||
    msg.includes('elemento do lote não esperado') ||
    msg.includes('elemento do lote nao esperado') ||
    msg.includes('esquema') ||
    msg.includes('schema') ||
    msg.includes('proccte') ||
    msg.includes('procevento') ||
    msg.includes('procinut') ||
    msg.includes('procgtve')
  ) {
    return {
      stage: 'classificar',
      headline: 'Classificar',
      detail: nsu
        ? `${threadName} roteou schema · NSU ${nsu}`
        : `${threadName} classificando lote por schema`,
      threadId,
      nsu,
      at: log.dtcLog,
      source,
    };
  }

  // Fallback cStat genérico
  if (cStat) {
    return {
      stage: 'fila',
      headline: 'Ciclo de síntese',
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
      return 'Fila';
    case 'temp':
      return 'Temporária';
    case 'classificar':
      return 'Classificar';
    case 'persistir':
      return 'Persistir';
    case 'limpar':
      return 'Limpar';
  }
}
