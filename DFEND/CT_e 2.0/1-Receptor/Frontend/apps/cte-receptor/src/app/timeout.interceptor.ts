import { HttpInterceptorFn } from '@angular/common/http';
import { timeout } from 'rxjs';

/** Timeout global de HTTP (POC DEV). */
export const timeoutInterceptor: HttpInterceptorFn = (req, next) =>
  next(req).pipe(timeout(15000));
