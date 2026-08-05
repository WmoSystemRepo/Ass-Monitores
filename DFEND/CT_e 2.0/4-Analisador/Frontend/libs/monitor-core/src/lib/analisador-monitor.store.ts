import { Injectable, computed, inject, signal } from '@angular/core';
import { HubConnection, HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
import {
  LogEntry,
  MonitorSnapshot,
  ServiceControlResult,
} from '@analisador/shared-data';
import { HUB_URL } from './api-config';
import { MonitorApiService } from './monitor-api.service';
import { firstValueFrom } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class AnalisadorMonitorStore {
  private readonly api = inject(MonitorApiService);
  private hub?: HubConnection;
  private restPollingStarted = false;

  readonly snapshot = signal<MonitorSnapshot | null>(null);
  readonly logs = signal<LogEntry[]>([]);
  readonly live = signal(false);
  readonly lastPushAt = signal<Date | null>(null);
  readonly actionBusy = signal(false);
  readonly actionMessage = signal<string | null>(null);
  readonly bootError = signal<string | null>(null);

  readonly connectionHealth = computed(() => this.snapshot()?.connectionHealth);
  readonly alerts = computed(() => this.snapshot()?.alerts ?? []);
  readonly threads = computed(() => this.snapshot()?.threads ?? []);
  readonly queues = computed(() => this.snapshot()?.queues);
  readonly documents = computed(() => this.snapshot()?.recentDocuments ?? []);
  readonly config = computed(() => this.snapshot()?.config ?? []);
  readonly service = computed(() => this.snapshot()?.global.service);
  readonly global = computed(() => this.snapshot()?.global);
  readonly liveTrace = computed(() => this.snapshot()?.liveTrace ?? []);
  readonly tableHealth = computed(() => this.snapshot()?.tableHealth ?? []);
  readonly sessionStartUtc = computed(() => this.snapshot()?.sessionStartUtc ?? null);

  async initialize(): Promise<void> {
    // Snapshot basta para UI; logs são best-effort (não devem deixar banner vermelho).
    try {
      const snap = await firstValueFrom(this.api.snapshot());
      this.applySnapshot(snap);
      this.markOnline();
    } catch (err) {
      this.bootError.set(
        err instanceof Error
          ? `${err.message} (API esperada em http://localhost:5040)`
          : 'Falha ao conectar na API DEV (http://localhost:5040)'
      );
      this.live.set(false);
      this.startRestPolling();
    }

    try {
      const initialLogs = await firstValueFrom(this.api.logs(0, 300));
      this.logs.set(initialLogs);
    } catch {
      /* logs opcionais no boot */
    }

    // SignalR em background — nunca derruba o "online" no close/reconnect.
    void this.connectHub().catch(() => this.startRestPolling());
  }

  private markOnline(): void {
    this.live.set(true);
    this.lastPushAt.set(new Date());
    this.bootError.set(null);
  }

  private startRestPolling(): void {
    if (this.restPollingStarted) {
      return;
    }
    this.restPollingStarted = true;
    setInterval(async () => {
      try {
        const snap = await firstValueFrom(this.api.snapshot());
        this.applySnapshot(snap);
        this.markOnline();
      } catch {
        this.live.set(false);
      }
    }, 2000);
  }

  private async connectHub(): Promise<void> {
    this.hub = new HubConnectionBuilder()
      .withUrl(HUB_URL)
      .withAutomaticReconnect()
      .configureLogging(LogLevel.Warning)
      .build();

    this.hub.on('snapshot', (snap: MonitorSnapshot) => {
      this.applySnapshot(this.normalizeSnapshot(snap));
      this.markOnline();
    });

    this.hub.on('logsAppend', (entries: LogEntry[]) => {
      const normalized = entries.map((e) => this.normalizeLog(e));
      this.logs.update((prev) => {
        const map = new Map(prev.map((l) => [l.seqLog, l]));
        for (const e of normalized) {
          map.set(e.seqLog, e);
        }
        return [...map.values()]
          .sort((a, b) => a.seqLog - b.seqLog)
          .slice(-800);
      });
      this.lastPushAt.set(new Date());
    });

    this.hub.onreconnected(() => this.markOnline());
    this.hub.onclose(() => this.startRestPolling());

    await Promise.race([
      this.hub.start(),
      new Promise<never>((_, reject) =>
        setTimeout(() => reject(new Error('SignalR timeout (15s)')), 15_000)
      ),
    ]);
    this.markOnline();
  }

  private applySnapshot(snap: MonitorSnapshot): void {
    this.snapshot.set(this.normalizeSnapshot(snap));
  }

  /** ASP.NET camelCase + enum numbers */
  private normalizeSnapshot(snap: MonitorSnapshot): MonitorSnapshot {
    const anySnap = snap as unknown as Record<string, unknown>;
    // Already typed from JSON camelCase
    return snap;
  }

  private normalizeLog(e: LogEntry): LogEntry {
    const raw = e as unknown as Record<string, unknown>;
    return {
      seqLog: (raw['seqLog'] as number) ?? (raw['SeqLog'] as number),
      dtcLog: (raw['dtcLog'] as string) ?? (raw['DtcLog'] as string),
      mensagem: (raw['mensagem'] as string) ?? (raw['Mensagem'] as string),
      threadId: (raw['threadId'] as number) ?? (raw['ThreadId'] as number),
      cStat: (raw['cStat'] as string) ?? (raw['CStat'] as string),
      severityHint:
        (raw['severityHint'] as string) ??
        (raw['SeverityHint'] as string) ??
        'info',
    };
  }

  async startService(): Promise<void> {
    this.actionBusy.set(true);
    this.actionMessage.set(null);
    try {
      const result = await firstValueFrom(this.api.start());
      this.actionMessage.set(result.message ?? result.status);
      const snap = await firstValueFrom(this.api.snapshot());
      this.applySnapshot(snap);
    } catch (err) {
      this.actionMessage.set(
        err instanceof Error ? err.message : 'Falha ao iniciar serviço'
      );
    } finally {
      this.actionBusy.set(false);
    }
  }

  async stopService(): Promise<void> {
    this.actionBusy.set(true);
    this.actionMessage.set(null);
    try {
      const result: ServiceControlResult = await firstValueFrom(this.api.stop());
      this.actionMessage.set(result.message ?? result.status);
      const snap = await firstValueFrom(this.api.snapshot());
      this.applySnapshot(snap);
    } catch (err) {
      this.actionMessage.set(
        err instanceof Error ? err.message : 'Falha ao parar serviço'
      );
    } finally {
      this.actionBusy.set(false);
    }
  }
}
