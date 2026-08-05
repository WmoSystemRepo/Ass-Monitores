import { ChangeDetectionStrategy, Component } from '@angular/core';

/** Stub — route not mounted in Orquestrador (Monitor only). */
@Component({
  selector: 'lib-tables-hub-page',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `<p class="text-sm text-indigo-300">Tabelas não fazem parte do Orquestrador.</p>`,
})
export class TablesHubPageComponent {}

@Component({
  selector: 'lib-table-detail-page',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `<p class="text-sm text-indigo-300">Tabelas não fazem parte do Orquestrador.</p>`,
})
export class TableDetailPageComponent {}
