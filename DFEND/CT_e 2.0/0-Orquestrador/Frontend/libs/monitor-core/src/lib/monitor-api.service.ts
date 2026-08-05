import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CascadeControlResult, ChainSnapshot, EnsureStacksResult, SystemEnsureOpenResult } from '@orquestrador/shared-data';
import { getApiBaseUrl } from './api-config';

@Injectable({ providedIn: 'root' })
export class MonitorApiService {
  private readonly http = inject(HttpClient);

  private get base(): string {
    return getApiBaseUrl();
  }

  snapshot(): Observable<ChainSnapshot> {
    return this.http.get<ChainSnapshot>(`${this.base}/api/orchestrator/snapshot`);
  }

  status(): Observable<CascadeControlResult> {
    return this.http.get<CascadeControlResult>(`${this.base}/api/orchestrator/status`);
  }

  start(): Observable<CascadeControlResult> {
    return this.http.post<CascadeControlResult>(
      `${this.base}/api/orchestrator/start`,
      {}
    );
  }

  stop(): Observable<CascadeControlResult> {
    return this.http.post<CascadeControlResult>(
      `${this.base}/api/orchestrator/stop`,
      {}
    );
  }

  /** Sobe API + Angular de todos os Enabled (sem ligar workers). */
  ensureStacks(): Observable<EnsureStacksResult> {
    return this.http.post<EnsureStacksResult>(
      `${this.base}/api/orchestrator/ensure-stacks`,
      {}
    );
  }

  ensureOpen(systemId: string): Observable<SystemEnsureOpenResult> {
    return this.http.post<SystemEnsureOpenResult>(
      `${this.base}/api/orchestrator/systems/${encodeURIComponent(systemId)}/ensure-open`,
      {}
    );
  }
}
