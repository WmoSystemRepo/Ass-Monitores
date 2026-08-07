import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { LogEntry, MonitorSnapshot, ServiceControlResult, TableDetailDto } from '@orquestrador/shared-data';
import { getApiBaseUrl } from '@orquestrador/monitor-core';

@Injectable({ providedIn: 'root' })
export class ServiceMonitorApiService {
  private readonly http = inject(HttpClient);
  private serviceId = '';
  setServiceId(serviceId: string): void { this.serviceId = serviceId; }
  private get base(): string {
    if (!this.serviceId) throw new Error('Monitor service is not bound.');
    return `${getApiBaseUrl()}/api/monitores/${encodeURIComponent(this.serviceId)}`;
  }
  snapshot(): Observable<MonitorSnapshot> { return this.http.get<MonitorSnapshot>(`${this.base}/snapshot`); }
  logs(afterSeq = 0, take = 300): Observable<LogEntry[]> {
    return this.http.get<LogEntry[]>(`${this.base}/logs`, { params: { afterSeq, take } });
  }
  tableDetail(key: string, take = 1000): Observable<TableDetailDto> {
    return this.http.get<TableDetailDto>(`${this.base}/tables/${encodeURIComponent(key)}`, { params: { take } });
  }
  serviceStatus(): Observable<ServiceControlResult> { return this.http.get<ServiceControlResult>(`${this.base}/service/status`); }
  start(): Observable<ServiceControlResult> { return this.http.post<ServiceControlResult>(`${this.base}/service/start`, {}); }
  stop(): Observable<ServiceControlResult> { return this.http.post<ServiceControlResult>(`${this.base}/service/stop`, {}); }
}
