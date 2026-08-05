import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { ArquivadorMonitorStore } from '@arquivador/monitor-core';

@Component({
  selector: 'lib-config-page',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <section class="space-y-4">
      <h1 class="text-2xl font-semibold text-zinc-50">Configurações</h1>
      <p class="text-sm text-zinc-400">
        Somente leitura das configurações de origem do Arquivador (SQL DEV · sts_ativo=1).
      </p>

      <div class="overflow-auto rounded border border-zinc-700">
        <table class="min-w-full text-left font-mono text-xs">
          <thead class="bg-zinc-900 text-zinc-400">
            <tr>
              <th class="px-3 py-2">Chave</th>
              <th class="px-3 py-2">Valor</th>
            </tr>
          </thead>
          <tbody>
            @for (c of store.config(); track c.key) {
              <tr class="border-t border-zinc-800">
                <td class="px-3 py-2 text-amber-300">{{ c.key }}</td>
                <td class="px-3 py-2 text-zinc-200">{{ c.value }}</td>
              </tr>
            } @empty {
              <tr>
                <td colspan="2" class="px-3 py-6 text-zinc-500">Sem configuração no snapshot.</td>
              </tr>
            }
          </tbody>
        </table>
      </div>
    </section>
  `,
})
export class ConfigPageComponent {
  readonly store = inject(ArquivadorMonitorStore);
}
