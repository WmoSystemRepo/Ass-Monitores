/** Jornada CT-e inferida pelo serviço atual na cadeia (Fase 1 — sem rastreio por chave). */
export interface CteJourneyInfo {
  origem: string;
  estagio: string;
  proximo: string;
}

const JOURNEY_BY_SERVICE: Record<string, CteJourneyInfo> = {
  receptor: {
    origem: 'SEFAZ / distribuição',
    estagio: 'Receptor (temporária)',
    proximo: 'Arquivador',
  },
  arquivador: {
    origem: 'Receptor',
    estagio: 'Arquivador (temporária)',
    proximo: 'Sintetizador',
  },
  sintetizador: {
    origem: 'Arquivador',
    estagio: 'Sintetizador (temporária)',
    proximo: 'Analisador',
  },
  analisador: {
    origem: 'Sintetizador',
    estagio: 'Analisador (temporária)',
    proximo: 'Integrador',
  },
  integrador: {
    origem: 'Analisador',
    estagio: 'Integrador (temporária)',
    proximo: 'Carga',
  },
  carga: {
    origem: 'Integrador',
    estagio: 'Carga (temporária)',
    proximo: '— (fim)',
  },
};

export function journeyForService(serviceId: string): CteJourneyInfo {
  return (
    JOURNEY_BY_SERVICE[serviceId?.toLowerCase() ?? ''] ?? {
      origem: '—',
      estagio: serviceId || '—',
      proximo: '—',
    }
  );
}

/** Heurística local: erro → fila → na temp. */
export function situacaoLote(hasError: boolean, queueDepth: number): string {
  if (hasError) return 'Com erro';
  if (queueDepth > 0) return 'Enfileirado';
  return 'Na temp';
}
