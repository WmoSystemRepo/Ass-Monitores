import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { ServiceMonitorStore } from '../service-monitor.store';

@Component({
  selector: 'lib-config-page',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <section class="space-y-4">
      <h1 class="text-2xl font-semibold text-slate-50">Configurações</h1>
      <p class="text-sm text-slate-400">
        Somente leitura das configurações de origem do Receptor (SQL DEV · sts_ativo=1).
      </p>

      <div class="overflow-auto rounded border border-slate-700">
        <table class="min-w-full text-left font-mono text-xs">
          <thead class="bg-slate-900 text-slate-400">
            <tr>
              <th class="px-3 py-2">Chave</th>
              <th class="px-3 py-2">Valor</th>
            </tr>
          </thead>
          <tbody>
            @for (c of store.config(); track c.key) {
              <tr class="border-t border-slate-800">
                <td class="px-3 py-2 text-cyan-300">{{ c.key }}</td>
                <td class="px-3 py-2 text-slate-200">{{ c.value }}</td>
              </tr>
            } @empty {
              <tr>
                <td colspan="2" class="px-3 py-6 text-slate-500">Sem configuração no snapshot.</td>
              </tr>
            }
          </tbody>
        </table>
      </div>
    </section>
  `,
})
export class ConfigPageComponent {
  readonly store = inject(ServiceMonitorStore);
}
