import { HttpInterceptorFn } from '@angular/common/http';
import { timeout } from 'rxjs';

/** Timeout global de HTTP (POC DEV). */
export const timeoutInterceptor: HttpInterceptorFn = (req, next) => {
  const request = req.url.startsWith('http://localhost:5040/api/monitor/')
    ? req.clone({
        setHeaders: {
          'X-Cte-Internal-Api-Key': 'dev-cte-chain-key',
        },
      })
    : req;

  return next(request).pipe(timeout(15000));
};
