import { Injectable, computed, inject, signal } from '@angular/core';
import {
  CascadePhase,
  ChainSnapshot,
  ChainSystemView,
  SystemRuntimeStatus,
} from '@orquestrador/shared-data';
import { MonitorApiService } from './monitor-api.service';
import { firstValueFrom } from 'rxjs';

const STATUS_BY_ORDINAL: Record<number, string> = {
  0: 'disabled',
  1: 'offline',
  2: 'starting',
  3: 'running',
  4: 'stopping',
  5: 'stopped',
  6: 'failed',
  7: 'unknown',
};

const PHASE_BY_ORDINAL: Record<number, string> = {
  0: 'idle',
  1: 'starting',
  2: 'running',
  3: 'stopping',
};

@Injectable({ providedIn: 'root' })
export class ChainOrchestratorStore {
  private readonly api = inject(MonitorApiService);
  private pollTimer?: ReturnType<typeof setTimeout>;
  private refreshInFlight = false;

  readonly snapshot = signal<ChainSnapshot | null>(null);
  readonly live = signal(false);
  readonly lastPushAt = signal<Date | null>(null);
  readonly starting = signal(false);
  readonly stopping = signal(false);
  readonly openingSystemId = signal<string | null>(null);
  readonly actionMessage = signal<string | null>(null);
  readonly bootError = signal<string | null>(null);

  readonly systems = computed(() => this.snapshot()?.systems ?? []);
  readonly cascadePhase = computed(() =>
    normalizePhase(this.snapshot()?.cascadePhase ?? 'idle')
  );
  readonly cascadeMessage = computed(() => this.snapshot()?.cascadeMessage ?? null);
  readonly lastLote = computed(() => this.snapshot()?.lastLote ?? null);
  readonly alerts = computed(() => this.snapshot()?.alerts ?? []);
  readonly beltMoving = computed(() => !!this.snapshot()?.beltMoving);
  readonly activeIds = computed(() => this.snapshot()?.activeIds ?? []);
  readonly actionBusy = computed(
    () => this.starting() || this.stopping() || !!this.openingSystemId()
  );

  readonly runningCount = computed(
    () =>
      this.systems().filter((s) => {
        if (normalizeStatus(s.status) !== 'running') return false;
        // Sem telemetria de Executar: processo no ar = filas ligadas.
        if (s.executar == null) return true;
        return Number(s.executar) === 1;
      }).length
  );

  /** Processo/DevHost no ar (filas ligadas na cascata). */
  readonly processUpCount = computed(
    () =>
      this.systems().filter((s) => normalizeStatus(s.status) === 'running').length
  );

  /** Mantido por compatibilidade; cascata não usa mais “pausado”. */
  readonly pausedCount = computed(() => 0);

  readonly anyRunning = computed(() => this.runningCount() > 0);

  async initialize(): Promise<void> {
    try {
      await this.refreshSnapshot();
      this.bootError.set(null);
    } catch (err) {
      this.bootError.set(
        err instanceof Error ? err.message : 'Falha ao conectar na API do Orquestrador'
      );
      this.live.set(false);
    }
    this.startPolling();
    // Não bloqueia a UI: sobe Angular (e API) dos monitores Enabled em background.
    void this.ensureChildFronts();
  }

  /** Ao abrir o Orquestrador, garante fronts dos monitores abaixo (R, A, futuros…). */
  async ensureChildFronts(): Promise<void> {
    this.actionMessage.set(
      'Subindo Angular dos monitores habilitados (Receptor, Arquivador, …)…'
    );
    try {
      const result = await firstValueFrom(this.api.ensureStacks());
      this.actionMessage.set(
        result.message ??
          `Fronts: ${result.readyCount}/${result.totalCount} online.`
      );
      await this.refreshSnapshot().catch(() => undefined);
    } catch (err) {
      this.actionMessage.set(
        extractCascadeMessage(err) ??
          'Não foi possível subir todos os Angular dos monitores (confira API/LocalDev).'
      );
    }
  }

  private startPolling(): void {
    if (this.pollTimer) clearTimeout(this.pollTimer);
    const tick = () => {
      void this.refreshSnapshot()
        .catch(() => {
          this.live.set(false);
        })
        .finally(() => {
          // Online: 1s. Offline: 5s (evita pilha de requests com timeout).
          const delay = this.live() ? 1000 : 5000;
          this.pollTimer = setTimeout(tick, delay);
        });
    };
    this.pollTimer = setTimeout(tick, 0);
  }

  private async refreshSnapshot(): Promise<void> {
    if (this.refreshInFlight) {
      return;
    }
    this.refreshInFlight = true;
    try {
      const snap = await firstValueFrom(this.api.snapshot());
      this.applySnapshot(snap);
      this.live.set(true);
      this.lastPushAt.set(new Date());
      this.bootError.set(null);
      // Limpa banner de falha de "Ligar as filas" quando o snapshot já está saudável.
      const msg = this.actionMessage();
      if (
        msg &&
        /Host POC não encontrado/i.test(msg) &&
        (snap.systems?.every(
          (s) => normalizeStatus(s.status) === 'running' && !s.lastError
        ) ??
          false)
      ) {
        this.actionMessage.set(null);
      }
    } catch (err) {
      this.live.set(false);
      // Não mantém erros velhos de Host POC se a API sumiu — limpa lastError visual.
      const current = this.snapshot();
      if (current?.systems.some((s) => !!s.lastError)) {
        this.snapshot.set({
          ...current,
          systems: current.systems.map((s) => ({ ...s, lastError: null })),
          alerts: [],
        });
      }
      throw err;
    } finally {
      this.refreshInFlight = false;
    }
  }

  private applySnapshot(snap: ChainSnapshot): void {
    this.snapshot.set(normalizeSnapshot(snap));
  }

  async startChain(): Promise<void> {
    this.starting.set(true);
    this.actionMessage.set(null);
    try {
      const result = await firstValueFrom(this.api.start());
      this.actionMessage.set(result.cascadeMessage ?? 'Ligar as filas iniciado.');
      await this.refreshSnapshot();
    } catch (err) {
      this.actionMessage.set(
        extractCascadeMessage(err) ?? 'Falha ao ligar as filas'
      );
      await this.refreshSnapshot().catch(() => undefined);
    } finally {
      this.starting.set(false);
    }
  }

  async stopChain(): Promise<void> {
    this.stopping.set(true);
    this.actionMessage.set(null);
    try {
      const result = await firstValueFrom(this.api.stop());
      this.actionMessage.set(result.cascadeMessage ?? 'Desligar filas iniciado.');
      await this.refreshSnapshot();
    } catch (err) {
      this.actionMessage.set(
        extractCascadeMessage(err) ?? 'Falha ao desligar filas'
      );
      await this.refreshSnapshot().catch(() => undefined);
    } finally {
      this.stopping.set(false);
    }
  }

  /**
   * Clique no estágio: abre aba no gesto do usuário (evita bloqueio de popup),
   * sobe API+Angular via ensure-open e navega para a URL do front.
   * Se o ensure falhar mas houver URL conhecida, ainda navega (refresh quando o Angular subir).
   */
  async openSystemUi(
    systemId: string,
    pendingTab: Window | null = null,
    knownFrontendUrl: string | null = null
  ): Promise<void> {
    if (!systemId.trim() || this.openingSystemId()) {
      writePendingStatus(
        pendingTab,
        systemId || 'serviço',
        'Já há uma abertura em andamento — aguarde ou feche a outra aba.',
        knownFrontendUrl
      );
      return;
    }

    this.openingSystemId.set(systemId);
    this.actionMessage.set(`Preparando ${systemId} (API + Angular)…`);
    writePendingStatus(
      pendingTab,
      systemId,
      'Preparando API + Angular… aguarde (pode levar 1–3 min na 1ª vez).',
      knownFrontendUrl
    );

    let navigated = false;
    const navigateTo = (url: string, note: string | null = null) => {
      if (navigated) {
        return;
      }
      navigated = true;
      this.actionMessage.set(note);
      if (pendingTab && !pendingTab.closed) {
        try {
          pendingTab.location.replace(url);
          return;
        } catch {
          /* fallback abaixo */
        }
      }
      const opened = window.open(url, '_blank');
      if (!opened) {
        this.actionMessage.set(
          `${note ?? 'Front.'} Popup bloqueado — permita popups ou abra: ${url}`
        );
        writePendingStatus(pendingTab, systemId, 'Popup bloqueado.', url);
      }
    };

    const pollUrl = knownFrontendUrl?.trim() || null;
    const pollReady =
      pollUrl != null
        ? pollFrontendReady(pollUrl, 300_000).then((ok) => {
            if (ok && pollUrl) {
              navigateTo(
                pollUrl,
                `${systemId}: front respondeu — abrindo.`
              );
            }
            return ok;
          })
        : Promise.resolve(false);

    try {
      const result = await firstValueFrom(this.api.ensureOpen(systemId));
      const url = (result.frontendUrl ?? knownFrontendUrl)?.trim() || null;

      if (navigated) {
        this.actionMessage.set(result.message ?? this.actionMessage());
        return;
      }

      if (url && result.success && result.frontendReachable) {
        navigateTo(url, result.message ?? `${systemId}: abrindo.`);
        return;
      }

      if (url) {
        navigateTo(
          url,
          result.message ??
            `Front de ${systemId} ainda subindo — abrindo ${url}. Atualize a aba se necessário.`
        );
        return;
      }

      writePendingStatus(
        pendingTab,
        systemId,
        result.message ??
          `Front de ${systemId} offline e sem URL configurada.`,
        null
      );
      this.actionMessage.set(
        result.message ?? `Front de ${systemId} ainda offline.`
      );
    } catch (err) {
      if (navigated) {
        return;
      }
      const bodyMsg = extractEnsureOpenMessage(err);
      const timeoutHint =
        err &&
        typeof err === 'object' &&
        'name' in err &&
        (err as { name?: string }).name === 'TimeoutError'
          ? `Timeout ao preparar ${systemId} (Angular pode estar compilando).`
          : null;
      const msg =
        bodyMsg ??
        timeoutHint ??
        (err instanceof Error ? err.message : `Falha ao abrir ${systemId}`);
      const url = knownFrontendUrl?.trim() || null;

      if (url) {
        navigateTo(url, `${msg} — abrindo ${url} mesmo assim.`);
      } else {
        writePendingStatus(pendingTab, systemId, msg, null);
        this.actionMessage.set(msg);
      }
    } finally {
      void pollReady.catch(() => undefined);
      this.openingSystemId.set(null);
    }
  }
}

async function pollFrontendReady(url: string, maxMs: number): Promise<boolean> {
  const deadline = Date.now() + maxMs;
  while (Date.now() < deadline) {
    try {
      // no-cors: qualquer resposta da porta (mesmo compile parcial) conta como no ar.
      await fetch(url, { method: 'GET', cache: 'no-store', mode: 'no-cors' });
      return true;
    } catch {
      /* ainda offline */
    }
    await new Promise((r) => setTimeout(r, 2000));
  }
  return false;
}

function writePendingStatus(
  tab: Window | null,
  systemId: string,
  message: string,
  url: string | null
): void {
  if (!tab || tab.closed) {
    return;
  }
  try {
    tab.document.title = `Abrindo ${systemId}…`;
    const safeMsg = escapeHtml(message);
    const safeUrl = url ? escapeHtml(url) : '';
    const link = url
      ? `<p><a id="go" href="${safeUrl}">${safeUrl}</a></p>
         <p><button type="button" onclick="location.href='${safeUrl}'">Abrir agora</button></p>
         <script>
         (function(){
           var url=${JSON.stringify(url)};
           var deadline=Date.now()+300000;
           function tick(){
             if(Date.now()>deadline) return;
             fetch(url,{cache:'no-store',mode:'no-cors'}).then(function(){
               location.replace(url);
             }).catch(function(){ setTimeout(tick,2000); });
           }
           setTimeout(tick,1500);
         })();
         </script>`
      : '';
    tab.document.open();
    tab.document.write(`<!doctype html><html><head><meta charset="utf-8"><title>Abrindo ${escapeHtml(systemId)}</title>
<style>body{font-family:system-ui,sans-serif;background:#0b1220;color:#e2e8f0;padding:2rem;line-height:1.5}
a{color:#7dd3fc}button{margin-top:.5rem;padding:.5rem 1rem;cursor:pointer}</style></head>
<body><h1>Abrindo ${escapeHtml(systemId)}</h1><p>${safeMsg}</p>${link}
<p style="opacity:.7;font-size:.9rem">Esta página redireciona sozinha quando o monitor responder.</p>
</body></html>`);
    tab.document.close();
  } catch {
    /* about:blank / cross-origin — ignore */
  }
}

function escapeHtml(value: string): string {
  return value
    .replace(/&/g, '&amp;')
    .replace(/</g, '&lt;')
    .replace(/>/g, '&gt;')
    .replace(/"/g, '&quot;')
    .replace(/'/g, '&#39;');
}

/** Extrai cascadeMessage do corpo 400 do Orquestrador (HttpErrorResponse). */
function extractCascadeMessage(err: unknown): string | null {
  if (!err || typeof err !== 'object') {
    return null;
  }

  const httpErr = err as { error?: unknown; message?: string };
  const body = httpErr.error;
  if (body && typeof body === 'object') {
    const msg = (body as { cascadeMessage?: unknown }).cascadeMessage;
    if (typeof msg === 'string' && msg.trim()) {
      return msg.trim();
    }
  }

  if (typeof httpErr.message === 'string' && httpErr.message.trim()) {
    return httpErr.message.trim();
  }

  return null;
}

function extractEnsureOpenMessage(err: unknown): string | null {
  if (!err || typeof err !== 'object') {
    return null;
  }

  const body = (err as { error?: unknown }).error;
  if (body && typeof body === 'object') {
    const msg = (body as { message?: unknown }).message;
    if (typeof msg === 'string' && msg.trim()) {
      return msg.trim();
    }
  }

  return null;
}

/** Alias usado pelo shell / APP_INITIALIZER. */
export { ChainOrchestratorStore as OrquestradorMonitorStore };

export function normalizeStatus(status: SystemRuntimeStatus | string | undefined): string {
  if (typeof status === 'number') {
    return STATUS_BY_ORDINAL[status] ?? String(status);
  }
  const raw = String(status ?? 'unknown').toLowerCase();
  switch (raw) {
    case 'off':
    case 'stopped':
      return 'stopped';
    case 'error':
    case 'failed':
      return 'failed';
    case 'sem_monitor':
    case 'disabled':
      return 'disabled';
    case 'offline':
      return 'offline';
    case 'starting':
    case 'running':
    case 'stopping':
    case 'unknown':
      return raw;
    default:
      return raw;
  }
}

export function normalizePhase(phase: CascadePhase | string | undefined): string {
  if (typeof phase === 'number') {
    return PHASE_BY_ORDINAL[phase] ?? String(phase);
  }
  return String(phase ?? 'idle').toLowerCase();
}

function normalizeSnapshot(snap: ChainSnapshot): ChainSnapshot {
  const raw = snap as unknown as Record<string, unknown>;
  const systemsRaw = (raw['systems'] ?? raw['Systems'] ?? []) as ChainSystemView[];
  const systems = (Array.isArray(systemsRaw) ? systemsRaw : []).map(normalizeSystem);
  const lastLoteRaw = (raw['lastLote'] ?? raw['LastLote'] ?? null) as
    | Record<string, unknown>
    | null;

  return {
    systems,
    activeIds: (raw['activeIds'] ?? raw['ActiveIds'] ?? []) as string[],
    cascadePhase: normalizePhase(
      (raw['cascadePhase'] ?? raw['CascadePhase']) as CascadePhase
    ) as CascadePhase,
    lastLote: lastLoteRaw
      ? {
          nsu: (lastLoteRaw['nsu'] ?? lastLoteRaw['Nsu']) as number | null,
          nsuFinal: (lastLoteRaw['nsuFinal'] ?? lastLoteRaw['NsuFinal']) as
            | number
            | null,
          qtdDocumento: (lastLoteRaw['qtdDocumento'] ??
            lastLoteRaw['QtdDocumento']) as number | null,
          at: (lastLoteRaw['at'] ?? lastLoteRaw['At']) as string | null,
        }
      : null,
    alerts: (raw['alerts'] ?? raw['Alerts'] ?? []) as string[],
    cascadeMessage: (raw['cascadeMessage'] ??
      raw['CascadeMessage'] ??
      null) as string | null,
    snapshotAtUtc: String(
      raw['snapshotAtUtc'] ?? raw['SnapshotAtUtc'] ?? new Date().toISOString()
    ),
    beltMoving: !!(raw['beltMoving'] ?? raw['BeltMoving']),
  };
}

function normalizeSystem(s: ChainSystemView): ChainSystemView {
  const raw = s as unknown as Record<string, unknown>;
  const metricPill = String(raw['metricPill'] ?? raw['MetricPill'] ?? '—');
  const queueFromApi = Number(raw['queueDepth'] ?? raw['QueueDepth'] ?? 0);
  const queueFromPill = parseQueueDepthFromPill(metricPill);
  const queueDepth = queueFromApi > 0 ? queueFromApi : queueFromPill;
  const hasQueueWork =
    !!(raw['hasQueueWork'] ?? raw['HasQueueWork']) || queueDepth > 0;
  const status = normalizeStatus(
    (raw['status'] ?? raw['Status']) as SystemRuntimeStatus
  ) as SystemRuntimeStatus;
  const lastError = (raw['lastError'] ?? raw['LastError'] ?? null) as
    | string
    | null;

  return {
    id: String(raw['id'] ?? raw['Id'] ?? ''),
    symbol: String(raw['symbol'] ?? raw['Symbol'] ?? ''),
    label: String(raw['label'] ?? raw['Label'] ?? ''),
    status,
    executar: Number(raw['executar'] ?? raw['Executar'] ?? 0),
    scmStatus: (raw['scmStatus'] ?? raw['ScmStatus'] ?? null) as string | null,
    agora: !!(raw['agora'] ?? raw['Agora']),
    metricPill,
    hint: String(raw['hint'] ?? raw['Hint'] ?? ''),
    lastError,
    enabled: !!(raw['enabled'] ?? raw['Enabled'] ?? true),
    frontendUrl: (raw['frontendUrl'] ?? raw['FrontendUrl'] ?? null) as
      | string
      | null,
    version: (raw['version'] ?? raw['Version'] ?? null) as string | null,
    uiIcon: (raw['uiIcon'] ?? raw['UiIcon'] ?? null) as string | null,
    uiColor: (raw['uiColor'] ?? raw['UiColor'] ?? null) as string | null,
    hasQueueWork,
    queueDepth,
    processHint: (raw['processHint'] ?? raw['ProcessHint'] ?? null) as
      | string
      | null,
  };
}

/** Fallback se a API antiga não manda hasQueueWork — lê "fila 1239" / "staging 10". */
function parseQueueDepthFromPill(pill: string): number {
  const m = pill.match(/(?:fila|staging|temp(?:orária)?)\s*[:\s]*(\d+)/i);
  if (!m) {
    return 0;
  }
  const n = Number(m[1]);
  return Number.isFinite(n) ? n : 0;
}
