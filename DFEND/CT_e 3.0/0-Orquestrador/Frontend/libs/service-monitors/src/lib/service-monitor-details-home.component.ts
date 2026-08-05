import { NgComponentOutlet } from '@angular/common';
import { ChangeDetectionStrategy, Component, Type, computed, inject } from '@angular/core';
import { ServiceMonitorStore } from './service-monitor.store';
import { AnalisadorDetailsPageComponent } from './analisador/details-page.component';
import { ArquivadorDetailsPageComponent } from './arquivador/details-page.component';
import { CargaDetailsPageComponent } from './carga/details-page.component';
import { IntegradorDetailsPageComponent } from './integrador/details-page.component';
import { ReceptorDetailsPageComponent } from './receptor/details-page.component';
import { SintetizadorDetailsPageComponent } from './sintetizador/details-page.component';

@Component({
  selector: 'lib-service-monitor-details-home',
  standalone: true,
  imports: [NgComponentOutlet],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `<ng-container *ngComponentOutlet="details()" />`,
})
export class ServiceMonitorDetailsHomeComponent {
  readonly store = inject(ServiceMonitorStore);
  readonly details = computed<Type<unknown>>(() => ({
    receptor: ReceptorDetailsPageComponent, arquivador: ArquivadorDetailsPageComponent,
    sintetizador: SintetizadorDetailsPageComponent, analisador: AnalisadorDetailsPageComponent,
    integrador: IntegradorDetailsPageComponent, carga: CargaDetailsPageComponent,
  }[this.store.serviceId()] ?? ReceptorDetailsPageComponent));
}
