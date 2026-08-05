import { ChangeDetectionStrategy, Component } from '@angular/core';

/** Stub — route not mounted in Orquestrador (Monitor only). */
@Component({
  selector: 'lib-logs-page',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `<p class="text-sm text-indigo-300">Histórico não faz parte do Orquestrador.</p>`,
})
export class LogsPageComponent {}
