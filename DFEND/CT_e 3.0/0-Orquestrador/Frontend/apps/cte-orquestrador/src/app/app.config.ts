import {
  APP_INITIALIZER,
  ApplicationConfig,
  provideZoneChangeDetection,
} from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { appRoutes } from './app.routes';
import { ChainOrchestratorStore } from '@orquestrador/monitor-core';
import { timeoutInterceptor } from './timeout.interceptor';

function initMonitor(store: ChainOrchestratorStore) {
  // Não bloqueia o bootstrap Angular aguardando a API (antes: tela branca até snapshot/timeout).
  return () => {
    void store.initialize();
  };
}

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(appRoutes),
    provideHttpClient(withInterceptors([timeoutInterceptor])),
    {
      provide: APP_INITIALIZER,
      useFactory: initMonitor,
      deps: [ChainOrchestratorStore],
      multi: true,
    },
  ],
};
