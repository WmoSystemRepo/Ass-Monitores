import { ChangeDetectionStrategy, Component, Type, computed, inject } from '@angular/core';
import { NgComponentOutlet } from '@angular/common';
import { ServiceMonitorStore } from './service-monitor.store';
import {
  AnalisadorDashboardPageComponent,
  ArquivadorDashboardPageComponent,
  CargaDashboardPageComponent,
  IntegradorDashboardPageComponent,
  ReceptorDashboardPageComponent,
  SintetizadorDashboardPageComponent,
} from './dashboards';

@Component({
  selector: 'lib-service-monitor-home',
  standalone: true,
  imports: [NgComponentOutlet],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `<ng-container *ngComponentOutlet="dashboard()" />`,
})
export class ServiceMonitorHomeComponent {
  readonly store = inject(ServiceMonitorStore);
  readonly dashboard = computed<Type<unknown>>(() => ({
    receptor: ReceptorDashboardPageComponent, arquivador: ArquivadorDashboardPageComponent,
    sintetizador: SintetizadorDashboardPageComponent, analisador: AnalisadorDashboardPageComponent,
    integrador: IntegradorDashboardPageComponent, carga: CargaDashboardPageComponent,
  }[this.store.serviceId()] ?? ReceptorDashboardPageComponent));
}

