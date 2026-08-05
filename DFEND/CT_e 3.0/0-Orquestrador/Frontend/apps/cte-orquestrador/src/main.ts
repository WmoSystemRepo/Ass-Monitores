import { bootstrapApplication } from '@angular/platform-browser';
import { loadRuntimeApiConfig } from '@orquestrador/monitor-core';
import { appConfig } from './app/app.config';
import { AppComponent } from './app/app.component';

loadRuntimeApiConfig()
  .catch(() => undefined)
  .then(() => bootstrapApplication(AppComponent, appConfig))
  .catch((err) => console.error(err));
