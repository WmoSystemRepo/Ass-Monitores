/** Runtime status of a chain system (estados oficiais + aliases legados). */
export type SystemRuntimeStatus =
  | 'disabled'
  | 'offline'
  | 'starting'
  | 'running'
  | 'stopping'
  | 'stopped'
  | 'failed'
  | 'unknown'
  | 'off'
  | 'error'
  | 'sem_monitor'
  | number;

export type CascadePhase =
  | 'idle'
  | 'starting'
  | 'running'
  | 'stopping'
  | number;

export interface ChainSystemView {
  id: string;
  symbol: string;
  label: string;
  status: SystemRuntimeStatus;
  executar: number;
  scmStatus?: string | null;
  agora: boolean;
  metricPill: string;
  hint: string;
  lastError?: string | null;
  enabled: boolean;
  /** URL do frontend do sistema (ex.: Receptor :4200). */
  frontendUrl?: string | null;
  version?: string | null;
  uiIcon?: string | null;
  uiColor?: string | null;
  /** Há itens na fila/staging/temporária. */
  hasQueueWork?: boolean;
  /** Profundidade relevante da fila. */
  queueDepth?: number;
  /** Thread/processo ativo (role · hint) quando telemetria expõe. */
  processHint?: string | null;
}

export interface LastLoteView {
  nsu?: number | null;
  nsuFinal?: number | null;
  qtdDocumento?: number | null;
  at?: string | null;
}

export interface ChainSnapshot {
  systems: ChainSystemView[];
  activeIds: string[];
  cascadePhase: CascadePhase;
  lastLote: LastLoteView | null;
  alerts: string[];
  cascadeMessage: string | null;
  snapshotAtUtc: string;
  beltMoving: boolean;
}

export interface CascadeControlResult {
  success: boolean;
  cascadePhase: CascadePhase;
  cascadeMessage?: string | null;
}

/** Resposta de POST /api/orchestrator/systems/{id}/ensure-open */
export interface SystemEnsureOpenResult {
  success: boolean;
  systemId: string;
  frontendUrl?: string | null;
  apiReady: boolean;
  apiStarted: boolean;
  frontendReachable: boolean;
  frontendStarted: boolean;
  message?: string | null;
}

/** Resposta de POST /api/orchestrator/ensure-stacks (boot do front Orquestrador) */
export interface EnsureStacksResult {
  success: boolean;
  message?: string | null;
  readyCount: number;
  totalCount: number;
}

/* —— Legacy shapes kept so unused libs still typecheck —— */
export type AlertSeverity = 'Info' | 'Atenção' | 'Alerta' | 'Critico';
export type ConnectionHealth = 'Healthy' | 'Degraded' | 'Down';

export interface MonitorAlert {
  code: string;
  severity: AlertSeverity | number;
  message: string;
  detectedAtUtc: string;
}

export interface ServiceStatusView {
  windowsServiceName: string;
  scmStatus: string;
  isRunning: boolean;
  executar: number;
  nomServidor?: string | null;
  dtcExecucao?: string | null;
  desServico?: string | null;
}

export interface ThreadView {
  threadId: number;
  role: string;
  nsuSource: string;
  indDFe: number;
  nsuAtual?: string | null;
  isIdle: boolean;
  outsideDatabase: boolean;
  lastActivityHint?: string | null;
  lastActivityAt?: string | null;
  lastCStat?: string | null;
  lastSeverityHint?: string | null;
}

export interface QueueStats {
  tempBacklog: number;
  serviceBrokerDepth: number;
  sintetizadorDepth?: number;
  analisadorDepth?: number;
  integradorDepth?: number;
  oldestTempAt?: string | null;
  brokerDepthTrend: number[];
}

export interface RecentDocument {
  nsu: number;
  nsuFinal?: number | null;
  qtdDocumento: number;
  dtcDocumento?: string | null;
  dtcAtualizacao?: string | null;
  mensagemErro?: string | null;
  hasError: boolean;
}

export interface LogEntry {
  seqLog: number;
  dtcLog?: string | null;
  mensagem?: string | null;
  threadId?: number | null;
  cStat?: string | null;
  severityHint: string;
}

export interface ConfigItem {
  key: string;
  value: string;
}

export interface GlobalStatus {
  service: ServiceStatusView;
  intervaloSeconds: number;
  pacoteCompleto: number;
  reBuscar: number;
  configuredThreads: number;
  mainNsu?: string | null;
  snapshotAtUtc: string;
}

export interface MonitorSnapshot {
  global: GlobalStatus;
  threads: ThreadView[];
  queues: QueueStats;
  recentDocuments: RecentDocument[];
  alerts: MonitorAlert[];
  config: ConfigItem[];
  connectionHealth: ConnectionHealth | number;
  connectionError?: string | null;
  liveTrace?: LiveTraceLine[];
  sessionStartUtc?: string | null;
  tableHealth?: TableHealthView[];
}

export type TableHealthStatus = 'Ok' | 'Atencao' | 'Critico' | string;

export interface TableHealthView {
  key: string;
  label: string;
  status: TableHealthStatus;
  primaryValue: string;
  dataAgeSeconds?: number | null;
  queryMs: number;
  hint: string;
  route: string;
}

export interface ServiceDetailRow {
  desServico?: string | null;
  nomServidor?: string | null;
  nsu?: string | null;
  dtcExecucao?: string | null;
  dtcAtualizacao?: string | null;
}

export interface ConfigDetailRow {
  key: string;
  value: string;
  dtcAtualizacao?: string | null;
  isProcessKey: boolean;
}

export interface FilaDetailView {
  depth: number;
  depthTrend: number[];
  highThreshold: number;
  trendHint: string;
}

export interface TableDetailDto {
  key: string;
  label: string;
  sessionStartUtc?: string | null;
  receptionOn: boolean;
  bannerMessage?: string | null;
  health: TableHealthView;
  serviceRows?: ServiceDetailRow[] | null;
  configRows?: ConfigDetailRow[] | null;
  tempRows?: RecentDocument[] | null;
  logRows?: LogEntry[] | null;
  fila?: FilaDetailView | null;
  contextLogs?: LogEntry[] | null;
}

export interface LiveTraceLine {
  at: string;
  message: string;
  step?: string | null;
  source: string;
}

export interface ServiceControlResult {
  success: boolean;
  status: string;
  message?: string | null;
}
