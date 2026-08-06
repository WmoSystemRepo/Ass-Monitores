import { Injectable, computed, inject, signal } from '@angular/core';
import { HubConnection, HubConnectionBuilder, HubConnectionState, LogLevel } from '@microsoft/signalr';
import { firstValueFrom } from 'rxjs';
import { LogEntry, MonitorSnapshot, ServiceControlResult } from '@orquestrador/shared-data';
import { getHubUrl } from '@orquestrador/monitor-core';
import { ServiceMonitorApiService } from './service-monitor-api.service';

export type MonitorTransport = 'signalr' | 'rest' | 'offline';

@Injectable({ providedIn: 'root' })
export class ServiceMonitorStore {
  private readonly api = inject(ServiceMonitorApiService);
  private hub?: HubConnection;
  private pollingTimer?: ReturnType<typeof setInterval>;
  private restPollingStarted = false;
  private boundServiceId = '';
  private logsInFlight = false;
  private hubConnectGeneration = 0;

  readonly serviceId = signal('');
  readonly snapshot = signal<MonitorSnapshot | null>(null);
  readonly logs = signal<LogEntry[]>([]);
  readonly live = signal(false);
  readonly lastPushAt = signal<Date | null>(null);
  readonly transport = signal<MonitorTransport>('offline');
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

  bind(serviceId: string): void {
    if (this.boundServiceId === serviceId) return;
    if (this.pollingTimer) {
      clearInterval(this.pollingTimer);
      this.pollingTimer = undefined;
    }
    this.restPollingStarted = false;
    this.boundServiceId = serviceId;
    this.serviceId.set(serviceId);
    this.snapshot.set(null);
    this.logs.set([]);
    this.live.set(false);
    this.lastPushAt.set(null);
    this.transport.set('offline');
    this.actionMessage.set(null);
    this.bootError.set(null);
    this.api.setServiceId(serviceId);
    void this.initialize();
  }

  async initialize(): Promise<void> {
    if (!this.boundServiceId) return;

    try {
      const snap = await firstValueFrom(this.api.snapshot());
      this.applySnapshot(snap);
      this.markOnline('rest');
    } catch (err) {
      this.bootError.set(
        err instanceof Error ? err.message : 'Falha ao conectar ao monitor.'
      );
      this.live.set(false);
      this.transport.set('offline');
      this.startRestPolling();
    }

    try {
      const initialLogs = await firstValueFrom(this.api.logs(0, 300));
      this.logs.set(Array.isArray(initialLogs) ? initialLogs : []);
    } catch {
      /* logs opcionais no boot */
    }

    // SignalR em background — se falhar, REST polling mantém a tela (paridade CT_e 2.0).
    void this.connectHub().catch(() => this.startRestPolling());
  }

  private markOnline(via: Exclude<MonitorTransport, 'offline'>): void {
    this.live.set(true);
    this.lastPushAt.set(new Date());
    this.bootError.set(null);
    this.transport.set(via);
  }

  private applySnapshot(snap: MonitorSnapshot): void {
    this.snapshot.set(snap);
  }

  private startRestPolling(): void {
    if (this.restPollingStarted || !this.boundServiceId) return;
    this.restPollingStarted = true;
    this.pollingTimer = setInterval(() => {
      void this.refreshRest();
      void this.refreshLogs(false);
    }, 2_000);
  }

  private async refreshRest(): Promise<void> {
    if (!this.boundServiceId) return;
    try {
      this.applySnapshot(await firstValueFrom(this.api.snapshot()));
      this.markOnline(
        this.hub?.state === HubConnectionState.Connected ? 'signalr' : 'rest'
      );
    } catch {
      this.live.set(false);
      this.transport.set('offline');
    }
  }

  private async refreshLogs(full: boolean): Promise<void> {
    if (this.logsInFlight || !this.boundServiceId) return;
    this.logsInFlight = true;
    try {
      const seqs = this.logs().map((l) => Number(l.seqLog) || 0);
      const after = full || seqs.length === 0 ? 0 : Math.max(...seqs);
      const take = full ? 300 : 100;
      const more = await firstValueFrom(this.api.logs(after, take));
      if (!Array.isArray(more) || more.length === 0) return;
      this.mergeLogs(more, full || after <= 0);
    } catch {
      /* logs opcionais */
    } finally {
      this.logsInFlight = false;
    }
  }

  private mergeLogs(entries: LogEntry[], replace: boolean): void {
    const normalized = entries.map((e) => this.normalizeLog(e));
    if (replace) {
      this.logs.set(normalized);
      return;
    }
    this.logs.update((prev) => {
      const map = new Map(prev.map((l) => [Number(l.seqLog) || 0, l]));
      for (const e of normalized) {
        map.set(Number(e.seqLog) || 0, e);
      }
      return [...map.values()]
        .sort((a, b) => (Number(a.seqLog) || 0) - (Number(b.seqLog) || 0))
        .slice(-800);
    });
  }

  private async connectHub(): Promise<void> {
    const generation = ++this.hubConnectGeneration;
    const serviceId = this.boundServiceId;
    if (!serviceId) return;

    if (!this.hub) {
      this.hub = new HubConnectionBuilder()
        .withUrl(getHubUrl())
        .withAutomaticReconnect()
        .configureLogging(LogLevel.Warning)
        .build();

      this.hub.on('snapshot', (snap: MonitorSnapshot) => {
        this.applySnapshot(snap);
        this.markOnline('signalr');
      });

      this.hub.on('logsAppend', (entries: LogEntry[]) => {
        if (!Array.isArray(entries) || entries.length === 0) return;
        this.mergeLogs(entries, false);
        this.lastPushAt.set(new Date());
        this.transport.set('signalr');
      });

      this.hub.onreconnected(() => {
        void this.joinCurrentService();
        this.markOnline('signalr');
      });
      // Não setar live=false no close — REST cobre o gap (paridade 2.0).
      this.hub.onclose(() => this.startRestPolling());
    }

    if (this.hub.state === HubConnectionState.Disconnected) {
      await Promise.race([
        this.hub.start(),
        new Promise<never>((_, reject) =>
          setTimeout(() => reject(new Error('SignalR timeout (15s)')), 15_000)
        ),
      ]);
    }

    if (generation !== this.hubConnectGeneration || this.boundServiceId !== serviceId) {
      return;
    }

    await this.joinCurrentService();
    this.markOnline('signalr');
  }

  private async joinCurrentService(): Promise<void> {
    if (!this.hub || this.hub.state !== HubConnectionState.Connected || !this.boundServiceId) {
      return;
    }
    await this.hub.invoke('JoinService', this.boundServiceId);
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
    await this.runControl(
      () => firstValueFrom(this.api.start()),
      'Falha ao iniciar serviço'
    );
  }

  async stopService(): Promise<void> {
    await this.runControl(
      () => firstValueFrom(this.api.stop()),
      'Falha ao parar serviço'
    );
  }

  private async runControl(
    action: () => Promise<ServiceControlResult>,
    fallback: string
  ): Promise<void> {
    this.actionBusy.set(true);
    this.actionMessage.set(null);
    try {
      const result = await action();
      this.actionMessage.set(result.message ?? result.status);
      await this.refreshRest();
      await this.refreshLogs(true);
    } catch (err) {
      this.actionMessage.set(err instanceof Error ? err.message : fallback);
    } finally {
      this.actionBusy.set(false);
    }
  }
}
export { ServiceMonitorStore as ReceptorMonitorStore };
export { ServiceMonitorStore as ArquivadorMonitorStore };
export { ServiceMonitorStore as SintetizadorMonitorStore };
export { ServiceMonitorStore as AnalisadorMonitorStore };
export { ServiceMonitorStore as IntegradorMonitorStore };
export { ServiceMonitorStore as CargaMonitorStore };
