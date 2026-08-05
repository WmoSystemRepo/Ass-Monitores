import { ChangeDetectionStrategy, Component, computed, effect, inject } from '@angular/core';
import { ActivatedRoute, Router, RouterLink, RouterOutlet } from '@angular/router';
import { toSignal } from '@angular/core/rxjs-interop';
import { map } from 'rxjs';
import { ServiceMonitorStore } from './service-monitor.store';

export const SERVICE_MONITORS = {
  receptor: 'Receptor CT-e',
  arquivador: 'Arquivador CT-e',
  sintetizador: 'Sintetizador CT-e',
  analisador: 'Analisador CT-e',
  integrador: 'Integrador CT-e',
  carga: 'Carga CT-e',
} as const;
export type ServiceMonitorId = keyof typeof SERVICE_MONITORS;

@Component({
  selector: 'lib-service-monitor-shell',
  standalone: true,
  imports: [RouterLink, RouterOutlet],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <main class="mx-auto max-w-[1440px] px-4 py-3">
      <header class="mb-3 flex items-center gap-3">
        <a routerLink="/" class="text-sm text-sky-300 hover:text-sky-100">← Voltar ao painel</a>
        <span class="text-slate-700">|</span>
        <h1 class="text-sm font-semibold text-slate-100">Monitor do {{ label() }}</h1>
      </header>
      <router-outlet />
    </main>
  `,
})
export class ServiceMonitorShellComponent {
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly store = inject(ServiceMonitorStore);
  readonly serviceId = toSignal(this.route.paramMap.pipe(map((p) => p.get('servico') ?? '')), {
    initialValue: '',
  });
  readonly label = computed(() => SERVICE_MONITORS[this.serviceId() as ServiceMonitorId] ?? '');

  constructor() {
    effect(() => {
      const serviceId = this.serviceId();
      if (!(serviceId in SERVICE_MONITORS)) {
        if (serviceId) void this.router.navigateByUrl('/');
        return;
      }
      this.store.bind(serviceId);
    });
  }
}
