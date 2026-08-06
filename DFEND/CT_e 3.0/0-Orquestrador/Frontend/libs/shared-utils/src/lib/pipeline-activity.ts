export type PipelineStage =
  | 'bootstrap'
  | 'nsu'
  | 'sefaz'
  | 'temp'
  | 'broker';

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

const THREAD_LABEL: Record<number, string> = {
  1: 'Principal',
  2: 'Auxiliar',
  3: 'Autorização',
  4: 'Destinatário',
  5: 'Arquivo',
};

/** Janela mínima; o painel usa max(isto, intervalo×1.5). */
export const PIPELINE_ACTIVITY_MAX_AGE_MS = 120_000;

export function threadFriendlyName(threadId?: number | null): string {
  if (threadId == null || threadId <= 0) return 'linha de trabalho';
  return THREAD_LABEL[threadId] ?? `Linha ${threadId}`;
}

function extractNsu(msg: string): string | null {
  const m =
    msg.match(/n[ºo°]?\s*:\s*(\d+)/i) ||
    msg.match(/nsu\s*:\s*(\d+)/i) ||
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

  // —— Bootstrap (igual Saída do VS) ——
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
        : 'Abrindo conexão com o banco de recepção',
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
          ? `consulta à SEFAZ a cada ${ivSec}s`
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
      headline: 'Receptor ligado',
      detail: machine
        ? `Serviço iniciou na máquina ${machine}`
        : 'Serviço do Receptor acabou de iniciar',
      threadId,
      at: log.dtcLog,
      source,
    };
  }

  if (msg.includes('processo não iniciado')) {
    return {
      stage: 'bootstrap',
      headline: 'Ciclo bloqueado',
      detail: 'Executar=0 — não busca documentos até ligar a recepção',
      threadId,
      at: log.dtcLog,
      source,
    };
  }

  if (msg.includes('processo iniciado com sucesso')) {
    return {
      stage: 'nsu',
      headline: 'Ciclo começou',
      detail: `${threadName} iniciou o loop de busca`,
      threadId,
      at: log.dtcLog,
      source,
    };
  }

  // —— NSU ——
  if (msg.includes('nsu obtido do banco')) {
    return {
      stage: 'nsu',
      headline: 'Pegou NSU no banco',
      detail: nsu
        ? `${threadName} leu NSU ${nsu} no banco (ponteiro da consulta)`
        : `${threadName} leu o NSU no banco`,
      threadId,
      nsu,
      at: log.dtcLog,
      source,
    };
  }

  if (msg.includes('nsu obtido do arquivo')) {
    return {
      stage: 'nsu',
      headline: 'Pegou NSU no arquivo',
      detail: nsu
        ? `${threadName} leu NSU ${nsu} em NSU.txt`
        : `${threadName} leu o NSU no arquivo local`,
      threadId,
      nsu,
      at: log.dtcLog,
      source,
    };
  }

  if (msg.includes('nsu atualizado no banco') || msg.includes('nsu atualizado no arquivo')) {
    return {
      stage: 'nsu',
      headline: 'Atualizou ponteiro NSU',
      detail: nsu
        ? `${threadName} avançou NSU para ${nsu}`
        : `${threadName} atualizou o NSU`,
      threadId,
      nsu,
      at: log.dtcLog,
      source,
    };
  }

  // —— SEFAZ ——
  if (msg.includes('webservice configurado')) {
    return {
      stage: 'sefaz',
      headline: 'Preparou consulta SEFAZ',
      detail: `${threadName} configurou WebService (TLS + certificado) para distCTeSVD`,
      threadId,
      at: log.dtcLog,
      source,
    };
  }

  if (msg.includes('webservice comunicado')) {
    return {
      stage: 'sefaz',
      headline: 'Consultou a SEFAZ',
      detail: `${threadName} chamou cteDistSVD${cStat ? ` · cStat ${cStat}` : ''}${
        nsu ? ` · NSU ${nsu}` : ''
      }`,
      threadId,
      nsu,
      cStat,
      at: log.dtcLog,
      source,
    };
  }

  if (msg.includes('retorno do webservice') || msg.includes('nenhum resultado encontrado')) {
    return {
      stage: 'sefaz',
      headline: 'Resposta da SEFAZ',
      detail: cStat
        ? `Retorno oficial cStat ${cStat}`
        : 'Retorno do WebService recebido',
      threadId,
      cStat,
      at: log.dtcLog,
      source,
    };
  }

  if (cStat === '118') {
    return {
      stage: 'sefaz',
      headline: 'Documentos encontrados',
      detail: `cStat 118 — lote com CT-e novo${nsu ? ` · NSU ${nsu}` : ''}`,
      threadId,
      nsu,
      cStat,
      at: log.dtcLog,
      source,
    };
  }

  if (cStat === '117') {
    return {
      stage: 'sefaz',
      headline: 'Nada novo na SEFAZ',
      detail: 'cStat 117 — sincroniza NSU, sem documentos novos',
      threadId,
      nsu,
      cStat,
      at: log.dtcLog,
      source,
    };
  }

  if (cStat === '146' || cStat === '730' || cStat === '992') {
    return {
      stage: 'sefaz',
      headline: 'Ajuste de NSU',
      detail: `cStat ${cStat} — pulo/correção na sequência de NSU`,
      threadId,
      nsu,
      cStat,
      at: log.dtcLog,
      source,
    };
  }

  if (cStat) {
    return {
      stage: 'sefaz',
      headline: 'Consulta SEFAZ',
      detail: `Resposta cStat ${cStat}${nsu ? ` · NSU ${nsu}` : ''}`,
      threadId,
      nsu,
      cStat,
      at: log.dtcLog,
      source,
    };
  }

  // —— temp_documento ——
  if (msg.includes('lote inserido no banco')) {
    return {
      stage: 'temp',
      headline: 'Gravou em tmp_documento',
      detail: nsu
        ? `Temporária · lote NSU ${nsu}`
        : 'Temporária · gravando lote',
      threadId,
      nsu,
      at: log.dtcLog,
      source,
    };
  }

  if (msg.includes('inserção de lote já existente') || msg.includes('lote já existente')) {
    return {
      stage: 'temp',
      headline: 'Lote já existia',
      detail: nsu
        ? `NSU ${nsu} já estava em tmp — ignorou duplicata`
        : 'Lote já existente (PK) — não regravou',
      threadId,
      nsu,
      at: log.dtcLog,
      source,
    };
  }

  // —— Service Broker / Arquivador ——
  if (
    msg.includes('chave do lote inserida na fila') ||
    msg.includes('inserida na fila') ||
    msg.includes('service broker')
  ) {
    return {
      stage: 'broker',
      headline: 'Notificou o Arquivador',
      detail: nsu
        ? `Mensagem na fila Service Broker · NSU ${nsu}`
        : 'Sinal na fila para o DFEND_CTe_Arquivador',
      threadId,
      nsu,
      at: log.dtcLog,
      source,
    };
  }

  if (msg.includes('lote recepcionado com valor de último nsu')) {
    return {
      stage: 'sefaz',
      headline: 'NSU inconsistente',
      detail: 'Último NSU da SEFAZ menor que o pesquisado',
      threadId,
      nsu,
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
  const patterns = [
    /cstat[^0-9]*(\d{3})/i,
    /status\s*:\s*(\d{3})/i,
    /status[=:\s]+(\d{3})/i,
    /rejei[cç][aã]o[^0-9]*(\d{3})/i,
  ];
  for (const re of patterns) {
    const m = raw.match(re);
    if (m?.[1]) return m[1];
  }
  return null;
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
    case 'nsu':
      return 'NSU / linhas';
    case 'sefaz':
      return 'Consulta SEFAZ';
    case 'temp':
      return 'Temporária';
    case 'broker':
      return 'Fila → Arquivador';
  }
}
