import { ChangeDetectionStrategy, Component } from '@angular/core';
import { SharedServiceDetailsPageComponent } from './shared-service-details-page.component';

/**
 * Rota Mais informações — um único layout (padrão Receptor) para todos os serviços.
 * Meta/copy/accent vêm de store.serviceId() dentro do shared.
 */
@Component({
  selector: 'lib-service-monitor-details-home',
  standalone: true,
  imports: [SharedServiceDetailsPageComponent],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `<lib-shared-service-details-page />`,
})
export class ServiceMonitorDetailsHomeComponent {}
