import { describeLogActivity } from './pipeline-activity';

export function severityLabel(severity: string | number): string {
  if (typeof severity === 'number') {
    return ['Info', 'Atenção', 'Alerta', 'Crítico'][severity] ?? String(severity);
  }
  return severity === 'Critico' ? 'Crítico' : severity;
}

/** Conexão SignalR do monitor (não confundir com Carga ligado). */
export function monitorConnectionLabel(live: boolean): string {
  return live ? 'Monitor online' : 'Monitor offline';
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
      // Degraded existe no modelo; aggregator ainda não atribui nesta POC.
      return 'Sem conexão';
    case 'Down':
      return 'Sem conexão';
    default:
      return String(raw);
  }
}

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

/** Status do processo para leigo (SCM/Running/Stopped/NotFound). */
export function CargaStatusLabel(scmStatus?: string | null, executar?: number | null): string {
  const status = (scmStatus ?? '').trim();
  const running = status.toLowerCase() === 'running';
  if (running && executar === 1) return 'Ligado';
  if (running && executar !== 1) return 'Carga pausada';
  if (status.toLowerCase() === 'stopped' || !status) {
    return 'Desligado';
  }
  if (status.toLowerCase() === 'notfound') {
    return 'Não disponível';
  }
  return status || '—';
}

/** Mensagens da API start/stop → texto simples + detalhe técnico. */
export function friendlyActionMessage(raw?: string | null): { title: string; detail?: string } {
  if (!raw) return { title: '' };
  const t = raw.toLowerCase();
  if (t.includes('encerrado') || t.includes('stopped') || t.includes('executar=0') || t.includes('desligado')) {
    return {
      title: 'Carga desligado',
      detail: raw,
    };
  }
  if (t.includes('iniciado') || t.includes('running') || t.includes('executar=1') || t.includes('ligado')) {
    return {
      title: 'Carga ligado',
      detail: raw,
    };
  }
  if (t.includes('já em execução') || t.includes('já estava')) {
    return { title: 'Carga já estava ligado', detail: raw };
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
  if (cStat === '118') return 'Documentos nDownload pontual · Carga (cStat 118)';
  if (cStat === '117') return 'NSU sincronizado (cStat 117)';
  if (cStat === '146') return 'Ajuste de NSU (cStat 146)';
  if (cStat === '108') return 'Serviço em manutenção (cStat 108)';
  if (cStat === '285') return 'Problema de certificado (cStat 285)';
  return msg.length > 160 ? `${msg.slice(0, 160)}…` : msg;
}

