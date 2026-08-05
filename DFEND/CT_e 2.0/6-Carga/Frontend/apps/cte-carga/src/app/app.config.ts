import {
  APP_INITIALIZER,
  ApplicationConfig,
  provideZoneChangeDetection,
} from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { appRoutes } from './app.routes';
import { CargaMonitorStore } from '@Carga/monitor-core';
import { timeoutInterceptor } from './timeout.interceptor';

function initMonitor(store: CargaMonitorStore) {
  return () => store.initialize();
}

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(appRoutes),
    provideHttpClient(withInterceptors([timeoutInterceptor])),
    {
      provide: APP_INITIALIZER,
      useFactory: initMonitor,
      deps: [CargaMonitorStore],
      multi: true,
    },
  ],
};
