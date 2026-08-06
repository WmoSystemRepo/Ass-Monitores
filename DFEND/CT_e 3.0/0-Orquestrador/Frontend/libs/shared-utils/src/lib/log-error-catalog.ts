/**
 * Catálogo histórico de erros/avisos técnicos → frase clara para leigo.
 * Cresce conforme novos casos aparecem na operação (SEFAZ, SQL, workers).
 *
 * Cada entrada: código/padrão visto + o que significa + o que podemos fazer.
 */

export interface LogErrorPlain {
  /** Identificador estável no histórico (ex.: sefaz-215). */
  id: string;
  /** Código SEFAZ / interno, se houver. */
  code?: string;
  /** Título curto para a lista de eventos. */
  title: string;
  /** Explicação em linguagem simples. */
  explanation: string;
  /** O que a equipe pode verificar / fazer do nosso lado. */
  whatWeCanDo: string;
}

export interface LogErrorMatchContext {
  mensagem?: string | null;
  cStat?: string | null;
  severityHint?: string | null;
}

interface CatalogEntry extends LogErrorPlain {
  match: (ctx: Required<Pick<LogErrorMatchContext, 'mensagem' | 'cStat'>> & {
    lower: string;
  }) => boolean;
}

/** Extrai cStat / Status numérico da mensagem quando o campo SQL vem vazio. */
export function extractStatusCode(
  mensagem?: string | null,
  cStat?: string | null
): string | null {
  const fromField = (cStat ?? '').trim();
  if (/^\d{3}$/.test(fromField)) return fromField;

  const raw = mensagem ?? '';
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

/**
 * Histórico mapeado — acrescentar novos casos aqui quando aparecerem na tela.
 * Ordem: mais específico primeiro.
 */
const CATALOG: CatalogEntry[] = [
  {
    id: 'sefaz-215',
    code: '215',
    title: 'SEFAZ rejeitou o XML (esquema inválido)',
    explanation:
      'A SEFAZ recusou a chamada porque o arquivo XML enviado não segue o formato oficial (schema). É a rejeição 215 — “Falha no esquema XML”. Não é falta de CT-e na fila: o pedido ou o documento estava mal montado (campo errado, caractere inválido, tag faltando, versão de layout desatualizada).',
    whatWeCanDo:
      'Do nosso lado: revisar o XML gerado pelo serviço (no log: Método/Classe/Aplicação — ex. Carga · ProcessarDownload · retDistCTeSVD), validar no validador da SEFAZ-RS, checar versão do schema CT-e, certificado/ambiente e caracteres especiais. Corrigir a montagem do XML e repetir a operação.',
    match: ({ cStat, lower }) =>
      cStat === '215' ||
      lower.includes('falha no esquema xml') ||
      lower.includes('falha no schema xml'),
  },
  {
    id: 'sefaz-108',
    code: '108',
    title: 'SEFAZ em manutenção / serviço parado',
    explanation:
      'A SEFAZ respondeu que o serviço de distribuição está temporariamente indisponível (cStat 108). Em geral é janela de manutenção do ambiente fiscal, não um bug da nossa fila.',
    whatWeCanDo:
      'Aguardar a SEFAZ voltar e religar/aguardar o próximo ciclo. Se persistir fora de horário de manutenção, conferir URL/ambiente (homolog vs produção) e certificado.',
    match: ({ cStat }) => cStat === '108',
  },
  {
    id: 'sefaz-117',
    code: '117',
    title: 'Nenhum documento novo (NSU em dia)',
    explanation:
      'A SEFAZ confirmou a consulta, mas não havia CT-e novo para o NSU pedido (cStat 117). É resposta normal quando a cadeia está sincronizada.',
    whatWeCanDo:
      'Nada obrigatório — acompanhar. Se esperava documentos, conferir NSU configurado e se o ambiente (CNPJ/certificado) está correto.',
    match: ({ cStat }) => cStat === '117',
  },
  {
    id: 'sefaz-118',
    code: '118',
    title: 'Documentos novos recebidos da SEFAZ',
    explanation:
      'A consulta trouxe CT-e novos (cStat 118). O Receptor deve gravar na temporária e seguir a cadeia.',
    whatWeCanDo:
      'Se a fila não avançar depois disso, olhar backlog da temporária e o Arquivador.',
    match: ({ cStat }) => cStat === '118',
  },
  {
    id: 'sefaz-146',
    code: '146',
    title: 'Ajuste de NSU pela SEFAZ',
    explanation:
      'A SEFAZ pediu realinhamento do NSU (cStat 146). O monitor costuma atualizar o ponteiro sozinho.',
    whatWeCanDo:
      'Confirmar se o NSU principal avançou após o evento. Se ficar em loop, revisar MainNsu no SQL.',
    match: ({ cStat }) => cStat === '146',
  },
  {
    id: 'sefaz-285',
    code: '285',
    title: 'Problema com o certificado digital',
    explanation:
      'A SEFAZ (ou o nosso cliente) sinalizou falha ligada ao certificado (cStat 285) — vencido, não instalado, senha, ou sem permissão no ambiente.',
    whatWeCanDo:
      'Verificar validade do certificado A1/A3, instalação na máquina do worker, senha e se o CNPJ do certificado bate com o configurado.',
    match: ({ cStat, lower }) =>
      cStat === '285' ||
      (lower.includes('certificado') &&
        (lower.includes('inválid') ||
          lower.includes('invalid') ||
          lower.includes('expir') ||
          lower.includes('não encontr') ||
          lower.includes('nao encontr'))),
  },
  {
    id: 'ws-retorno-inesperado',
    title: 'Retorno do WebService não esperado',
    explanation:
      'O serviço chamou a SEFAZ e o retorno não era o esperado para aquele passo (status/corpo fora do fluxo normal). Muitas vezes vem junto com um cStat de rejeição (ex.: 215).',
    whatWeCanDo:
      'Abrir o texto original, anotar Status/cStat e Método/Classe. Se houver código mapeado (215, 108…), seguir a orientação daquele código. Caso contrário, guardar o trecho e acrescentar no catálogo.',
    match: ({ lower }) =>
      lower.includes('retorno do webservice não esperado') ||
      lower.includes('retorno do webservice nao esperado') ||
      lower.includes('webservice não esperado') ||
      lower.includes('webservice nao esperado'),
  },
  {
    id: 'svc-stale',
    title: 'Batida do serviço desatualizada',
    explanation:
      'O monitor não vê “batida” recente do worker SQL (último heartbeat antigo). Pode estar parado, em outra máquina, ou com relógio/registro defasado.',
    whatWeCanDo:
      'Conferir se o processo/serviço Windows está no ar na máquina indicada, Ligar as filas se estiver Parada, e se o registro de execução no SQL está sendo atualizado.',
    match: ({ lower }) =>
      lower.includes('svc_stale') ||
      (lower.includes('batida') && lower.includes('desatualiz')),
  },
];

export function explainLogError(
  ctx: LogErrorMatchContext
): LogErrorPlain | null {
  const mensagem = (ctx.mensagem ?? '').trim();
  const cStat = extractStatusCode(mensagem, ctx.cStat) ?? '';
  const lower = mensagem.toLowerCase();

  for (const entry of CATALOG) {
    if (entry.match({ mensagem, cStat, lower })) {
      const { match: _m, ...plain } = entry;
      return {
        ...plain,
        code: plain.code ?? (cStat || undefined),
      };
    }
  }
  return null;
}

/** Texto para o modal: linguagem clara + o que fazer. */
export function formatLogErrorPlainMessage(
  plain: LogErrorPlain,
  meta?: { seqLog?: number | null; cStat?: string | null }
): string {
  const bits: string[] = [];
  if (meta?.seqLog != null) {
    bits.push(`Evento #${meta.seqLog}`);
  }
  const code = plain.code ?? meta?.cStat;
  if (code) {
    bits.push(`código ${code}`);
  }
  const head = bits.length ? `${bits.join(' · ')}` : plain.title;

  return [
    head,
    '',
    plain.title,
    '',
    plain.explanation,
    '',
    `O que podemos fazer: ${plain.whatWeCanDo}`,
  ].join('\n');
}

/** Payload único para área de transferência (traduzido + original). */
export function formatLogErrorCopyPayload(opts: {
  plain?: LogErrorPlain | null;
  original: string;
  seqLog?: number | null;
  cStat?: string | null;
}): string {
  const original = (opts.original || '').trim() || '(sem mensagem)';
  if (!opts.plain) return original;

  return [
    '=== Em linguagem clara ===',
    formatLogErrorPlainMessage(opts.plain, {
      seqLog: opts.seqLog,
      cStat: opts.cStat ?? opts.plain.code,
    }),
    '',
    '=== Texto original (banco) ===',
    original,
  ].join('\n');
}
