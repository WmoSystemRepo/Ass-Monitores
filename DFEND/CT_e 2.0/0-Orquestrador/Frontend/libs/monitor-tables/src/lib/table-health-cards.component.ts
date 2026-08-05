import { ChangeDetectionStrategy, Component, input } from '@angular/core';
import { TableHealthView } from '@orquestrador/shared-data';

/** Stub — not used on Orquestrador dashboard. */
@Component({
  selector: 'lib-table-health-cards',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: ``,
})
export class TableHealthCardsComponent {
  readonly items = input<TableHealthView[]>([]);
}
