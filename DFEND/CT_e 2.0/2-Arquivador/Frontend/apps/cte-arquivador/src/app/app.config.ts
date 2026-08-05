import {
  APP_INITIALIZER,
  ApplicationConfig,
  provideZoneChangeDetection,
} from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { appRoutes } from './app.routes';
import { ArquivadorMonitorStore } from '@arquivador/monitor-core';
import { timeoutInterceptor } from './timeout.interceptor';

function initMonitor(store: ArquivadorMonitorStore) {
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
      deps: [ArquivadorMonitorStore],
      multi: true,
    },
  ],
};
