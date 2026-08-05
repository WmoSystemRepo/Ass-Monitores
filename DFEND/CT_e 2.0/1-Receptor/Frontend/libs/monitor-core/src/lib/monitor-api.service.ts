import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import {
  LogEntry,
  MonitorSnapshot,
  ServiceControlResult,
  TableDetailDto,
} from '@receptor/shared-data';
import { API_BASE_URL } from './api-config';

@Injectable({ providedIn: 'root' })
export class MonitorApiService {
  private readonly http = inject(HttpClient);
  private readonly base = API_BASE_URL;

  snapshot(): Observable<MonitorSnapshot> {
    return this.http.get<MonitorSnapshot>(`${this.base}/api/monitor/snapshot`);
  }

  logs(afterSeq = 0, take = 300): Observable<LogEntry[]> {
    return this.http.get<LogEntry[]>(`${this.base}/api/monitor/logs`, {
      params: { afterSeq, take },
    });
  }

  tableDetail(key: string, take = 100): Observable<TableDetailDto> {
    return this.http.get<TableDetailDto>(
      `${this.base}/api/monitor/tables/${encodeURIComponent(key)}`,
      { params: { take } }
    );
  }

  serviceStatus(): Observable<ServiceControlResult> {
    return this.http.get<ServiceControlResult>(
      `${this.base}/api/monitor/service/status`
    );
  }

  start(): Observable<ServiceControlResult> {
    return this.http.post<ServiceControlResult>(
      `${this.base}/api/monitor/service/start`,
      {}
    );
  }

  stop(): Observable<ServiceControlResult> {
    return this.http.post<ServiceControlResult>(
      `${this.base}/api/monitor/service/stop`,
      {}
    );
  }
}
