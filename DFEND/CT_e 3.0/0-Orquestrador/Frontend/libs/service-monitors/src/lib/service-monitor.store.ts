import { Injectable, computed, inject, signal } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { LogEntry, MonitorSnapshot, ServiceControlResult } from '@orquestrador/shared-data';
import { ServiceMonitorApiService } from './service-monitor-api.service';

@Injectable({ providedIn: 'root' })
export class ServiceMonitorStore {
  private readonly api = inject(ServiceMonitorApiService);
  private pollingTimer?: ReturnType<typeof setInterval>;
  private boundServiceId = '';
  private logsInFlight = false;
  readonly serviceId = signal('');
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

  bind(serviceId: string): void {
    if (this.boundServiceId === serviceId) return;
    if (this.pollingTimer) clearInterval(this.pollingTimer);
    this.boundServiceId = serviceId;
    this.serviceId.set(serviceId);
    this.snapshot.set(null);
    this.logs.set([]);
    this.live.set(false);
    this.lastPushAt.set(null);
    this.actionMessage.set(null);
    this.bootError.set(null);
    this.api.setServiceId(serviceId);
    void this.initialize();
  }

  async initialize(): Promise<void> {
    if (!this.boundServiceId) return;
    await this.refresh();
    await this.refreshLogs(true);
    this.pollingTimer = setInterval(() => {
      void this.refresh();
      void this.refreshLogs(false);
    }, 2_000);
  }

  private async refresh(): Promise<void> {
    try {
      this.snapshot.set(await firstValueFrom(this.api.snapshot()));
      this.live.set(true);
      this.lastPushAt.set(new Date());
      this.bootError.set(null);
    } catch (err) {
      this.live.set(false);
      this.bootError.set(
        err instanceof Error ? err.message : 'Falha ao conectar ao monitor.'
      );
    }
  }

  /** Poll contínuo de logs (paridade CT_e 2.0 SignalR) — alimenta visualStage/AGORA. */
  private async refreshLogs(full: boolean): Promise<void> {
    if (this.logsInFlight || !this.boundServiceId) return;
    this.logsInFlight = true;
    try {
      const seqs = this.logs().map((l) => Number(l.seqLog) || 0);
      const after = full || seqs.length === 0 ? 0 : Math.max(...seqs);
      const take = full ? 300 : 100;
      const more = await firstValueFrom(this.api.logs(after, take));
      if (!Array.isArray(more) || more.length === 0) return;

      if (full || after <= 0) {
        this.logs.set(more);
        return;
      }

      const bySeq = new Map<number, LogEntry>();
      for (const row of this.logs()) {
        bySeq.set(Number(row.seqLog) || 0, row);
      }
      for (const row of more) {
        bySeq.set(Number(row.seqLog) || 0, row);
      }
      this.logs.set(
        [...bySeq.values()].sort(
          (a, b) => (Number(a.seqLog) || 0) - (Number(b.seqLog) || 0)
        )
      );
    } catch {
      /* logs opcionais — snapshot já carrega o essencial */
    } finally {
      this.logsInFlight = false;
    }
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
      await this.refresh();
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
