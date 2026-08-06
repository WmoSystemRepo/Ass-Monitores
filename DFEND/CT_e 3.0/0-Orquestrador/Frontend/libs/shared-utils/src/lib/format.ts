import { describeLogActivity } from './pipeline-activity';

export function severityLabel(severity: string | number): string {
  if (typeof severity === 'number') {
    return ['Info', 'Atenção', 'Alerta', 'Crítico'][severity] ?? String(severity);
  }
  return severity === 'Critico' ? 'Crítico' : severity;
}

/** Conexão do Orquestrador (poll snapshot). */
export function monitorConnectionLabel(live: boolean): string {
  return live ? 'Orquestrador online' : 'Orquestrador offline';
}

export function connectionHealthLabel(health: string | number): string {
  const raw =
    typeof health === 'number'
      ? (['Healthy', 'Degraded', 'Down'][health] ?? String(health))
      : health;
  switch (raw) {
    case 'Healthy':
      return 'Conectado';
    case 'Degraded':
      return 'Sem conexão';
    case 'Down':
      return 'Sem conexão';
    case 'Unknown':
    case 'Unchecked':
    case 'SemDados':
    case 'SemTelemetria':
      return 'Sem dados do banco';
    default:
      return String(raw);
  }
}

/** Processo (SCM/DevHost) — separado do trabalho (Executar). */
export function processStatusLabel(scmStatus?: string | null): string {
  const status = (scmStatus ?? '').trim().toLowerCase();
  if (status === 'running') return 'No ar';
  if (status === 'stopped' || !status) return 'Parado';
  if (status === 'notfound') return 'Não disponível';
  if (status.includes('start')) return 'Ligando…';
  if (status.includes('stop')) return 'Desligando…';
  return scmStatus?.trim() || '—';
}

/** Trabalho (flag Executar) — ativo / pausado / sem telemetria. */
export function workStatusLabel(
  scmStatus?: string | null,
  executar?: number | null,
  opts?: { workNoun?: string; executarKnown?: boolean | null }
): string {
  const noun = opts?.workNoun ?? 'Trabalho';
  const processUp = (scmStatus ?? '').trim().toLowerCase() === 'running';
  const known = opts?.executarKnown !== false && executar != null;
  if (!processUp) return `${noun} parado`;
  if (!known) return `${noun}: sem telemetria`;
  if (executar === 1) return `${noun} ativo`;
  return `${noun} pausado`;
}

/** Status do processo para leigo (SCM/Running/Stopped/NotFound). */
export function receptorStatusLabel(scmStatus?: string | null, executar?: number | null): string {
  const status = (scmStatus ?? '').trim();
  const running = status.toLowerCase() === 'running';
  if (running && executar === 1) return 'Trabalho ativo';
  if (running && executar == null) return 'Processo no ar · sem telemetria';
  if (running && executar !== 1) return 'Processo no ar · recepção pausada';
  if (status.toLowerCase() === 'stopped' || !status) {
    return 'Processo parado';
  }
  if (status.toLowerCase() === 'notfound') {
    return 'Não disponível';
  }
  return status || '—';
}

function serviceStatusLabel(
  scmStatus: string | null | undefined,
  executar: number | null | undefined,
  pausedLabel: string
): string {
  const status = (scmStatus ?? '').trim();
  const running = status.toLowerCase() === 'running';
  if (running && executar === 1) return 'Trabalho ativo';
  if (running && executar == null) return 'Processo no ar · sem telemetria';
  if (running && executar !== 1) return pausedLabel;
  if (status.toLowerCase() === 'stopped' || !status) return 'Processo parado';
  if (status.toLowerCase() === 'notfound') return 'Não disponível';
  return status || '—';
}

export const arquivadorStatusLabel = (status?: string | null, executar?: number | null) =>
  serviceStatusLabel(status, executar, 'Processo no ar · arquivamento pausado');
export const sintetizadorStatusLabel = (status?: string | null, executar?: number | null) =>
  serviceStatusLabel(status, executar, 'Processo no ar · síntese pausada');
export const analisadorStatusLabel = (status?: string | null, executar?: number | null) =>
  serviceStatusLabel(status, executar, 'Processo no ar · análise pausada');
export const integradorStatusLabel = (status?: string | null, executar?: number | null) =>
  serviceStatusLabel(status, executar, 'Processo no ar · integração pausada');
export const CargaStatusLabel = (status?: string | null, executar?: number | null) =>
  serviceStatusLabel(status, executar, 'Processo no ar · carga pausada');

export function formatAge(iso?: string | null): string {
  if (!iso) return '—';
  const d = new Date(iso);
  const sec = Math.max(0, Math.floor((Date.now() - d.getTime()) / 1000));
  return formatDataAgeSeconds(sec);
}

/** Idade em segundos (vindo do snapshot tableHealth.dataAgeSeconds). */
export function formatDataAgeSeconds(sec?: number | null): string {
  if (sec == null || Number.isNaN(sec)) return '—';
  const s = Math.max(0, Math.floor(sec));
  if (s < 60) return `${s}s`;
  if (s < 3600) return `${Math.floor(s / 60)}m`;
  return `${Math.floor(s / 3600)}h`;
}

export function tableHealthStatusLabel(status?: string | null): string {
  switch ((status ?? '').toLowerCase()) {
    case 'ok':
      return 'OK';
    case 'atencao':
      return 'Atenção';
    case 'critico':
      return 'Crítico';
    default:
      return status || '—';
  }
}

/**
 * Idade da última batida no banco (`dtc_execucao`). Valores absurdos (POC / SVC_STALE)
 * não fingem que o serviço “parou há milhares de horas”.
 * @param floorMinutes piso de stale (default 5)
 * @param intervaloSec Intervalo do ciclo; stale ≥ max(floor, 3×intervalo)
 */
export function formatHeartbeatAge(
  iso?: string | null,
  opts?: { floorMinutes?: number; intervaloSec?: number; processRunning?: boolean }
): { text: string; stale: boolean } {
  if (!iso) return { text: '—', stale: false };
  const d = new Date(iso);
  const sec = Math.max(0, Math.floor((Date.now() - d.getTime()) / 1000));
  const floorMin = opts?.floorMinutes ?? 5;
  let intervalo = opts?.intervaloSec ?? 60;
  if (intervalo >= 1000) intervalo = Math.round(intervalo / 1000);
  if (intervalo <= 0) intervalo = 60;
  const thresholdSec = Math.max(floorMin * 60, intervalo * 3);
  const stale = sec >= thresholdSec;

  if (stale && opts?.processRunning) {
    return { text: 'Última batida no banco desatualizada (POC)', stale: true };
  }
  if (stale) {
    return { text: `última batida desatualizada · há ${formatAge(iso)}`, stale: true };
  }
  return { text: `última batida há ${formatAge(iso)}`, stale: false };
}

/** Mensagens da API start/stop → texto simples + detalhe técnico. */
export function friendlyActionMessage(raw?: string | null): { title: string; detail?: string } {
  if (!raw) return { title: '' };
  const t = raw.toLowerCase();
  if (t.includes('encerrado') || t.includes('stopped') || t.includes('executar=0') || t.includes('desligado')) {
    return {
      title: 'Serviço desligado',
      detail: raw,
    };
  }
  if (t.includes('iniciado') || t.includes('running') || t.includes('executar=1') || t.includes('ligado')) {
    return {
      title: 'Serviço ligado',
      detail: raw,
    };
  }
  if (t.includes('já em execução') || t.includes('já estava')) {
    return { title: 'Serviço já estava ligado', detail: raw };
  }
  return { title: raw };
}

export type LogKind = 'all' | 'success' | 'error' | 'warning' | 'info';

export function classifyLogKind(severityHint?: string | null, cStat?: string | null): Exclude<LogKind, 'all'> {
  const s = (severityHint ?? 'info').toLowerCase();
  if (s === 'error') return 'error';
  if (s === 'warning' || s === 'warn') return 'warning';
  if (s === 'success' || cStat === '118' || cStat === '117' || cStat === '146') return 'success';
  return 'info';
}

export function logKindLabel(kind: Exclude<LogKind, 'all'>): string {
  switch (kind) {
    case 'success':
      return 'Sucesso';
    case 'error':
      return 'Erro';
    case 'warning':
      return 'Aviso';
    default:
      return 'Info';
  }
}

export function summarizeLogMessage(mensagem?: string | null, cStat?: string | null): string {
  const activity = describeLogActivity({ mensagem, cStat });
  if (activity) return activity.detail;

  const msg = (mensagem ?? '').trim();
  if (!msg) return 'Evento sem descrição';
  if (cStat === '118') return 'Documentos recebidos da SEFAZ (cStat 118)';
  if (cStat === '117') return 'NSU sincronizado (cStat 117)';
  if (cStat === '146') return 'Ajuste de NSU (cStat 146)';
  if (cStat === '108') return 'SEFAZ em manutenção (cStat 108)';
  if (cStat === '285') return 'Problema de certificado (cStat 285)';
  return msg.length > 160 ? `${msg.slice(0, 160)}…` : msg;
}

