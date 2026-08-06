import { ChangeDetectionStrategy, Component } from '@angular/core';
import { SharedServiceDetailsPageComponent } from '../shared-service-details-page.component';

/** Alias — UX unificada (padrão Receptor) em SharedServiceDetailsPageComponent. */
@Component({
  selector: 'lib-arquivador-details-page',
  standalone: true,
  imports: [SharedServiceDetailsPageComponent],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `<lib-shared-service-details-page />`,
})
export class ArquivadorDetailsPageComponent {}
