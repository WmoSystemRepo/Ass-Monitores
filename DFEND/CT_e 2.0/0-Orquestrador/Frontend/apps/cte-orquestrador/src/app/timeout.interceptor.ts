import { HttpInterceptorFn } from '@angular/common/http';
import { timeout } from 'rxjs';

/** Timeout padrão (poll/snapshot). Cascata Ligar/Desligar / ensure-open precisa esperar API+Angular. */
const DEFAULT_MS = 15_000;
const CASCADE_MS = 300_000; // 5 min — npm start + ensure monitores

/** Timeout global de HTTP (POC DEV). */
export const timeoutInterceptor: HttpInterceptorFn = (req, next) => {
  const url = req.url.toLowerCase();
  const isCascade =
    url.includes('/api/orchestrator/start') ||
    url.includes('/api/orchestrator/stop') ||
    url.includes('/api/orchestrator/ensure-stacks') ||
    (url.includes('/api/orchestrator/systems/') && url.includes('/ensure-open'));
  return next(req).pipe(timeout(isCascade ? CASCADE_MS : DEFAULT_MS));
};
