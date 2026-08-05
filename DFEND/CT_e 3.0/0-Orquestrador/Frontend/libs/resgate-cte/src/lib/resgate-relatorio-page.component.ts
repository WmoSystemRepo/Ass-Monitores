import { ChangeDetectionStrategy, Component, OnInit, inject, signal } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { resgateApi } from './resgate-api';

@Component({
  standalone: true,
  imports: [RouterLink],
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: 'orq-resgate-relatorio-page',
  template: `
    <div class="space-y-4 text-indigo-50">
      <div class="flex items-center justify-between">
        <h1 class="text-xl font-semibold">Relatório — lote {{ loteId }}</h1>
        <div class="flex gap-2 text-xs">
          <a class="rounded border border-indigo-600 px-2 py-1" [routerLink]="['/resgate/lote', loteId]">Voltar painel</a>
          <button type="button" class="rounded bg-violet-800 px-2 py-1" (click)="exportCsv()">Exportar CSV</button>
        </div>
      </div>
      @if (erro()) {
        <p class="text-rose-300 text-sm">{{ erro() }}</p>
      }
      @if (data(); as d) {
        <p class="text-sm text-indigo-200">
          Recuperados {{ d.lote?.recuperados }} · Existentes {{ d.lote?.existentes }} ·
          Não loc. {{ d.lote?.naoLocalizados }} · Erros {{ d.lote?.erros }}
        </p>
        <table class="w-full text-left text-xs border border-indigo-800">
          <thead class="bg-indigo-900 text-indigo-300">
            <tr><th class="p-2">Chave</th><th>Status</th><th>Motivo</th><th>Horário</th></tr>
          </thead>
          <tbody>
            @for (i of d.itens || []; track i.chave) {
              <tr class="border-t border-indigo-900">
                <td class="p-2 font-mono">{{ i.chave }}</td>
                <td>{{ i.status }}</td>
                <td>{{ i.motivo }}</td>
                <td>{{ i.atualizadoEm }}</td>
              </tr>
            }
          </tbody>
        </table>
      }
    </div>
  `,
})
export class ResgateRelatorioPageComponent implements OnInit {
  private readonly route = inject(ActivatedRoute);
  loteId = 0;
  readonly data = signal<any>(null);
  readonly erro = signal<string | null>(null);

  ngOnInit(): void {
    this.loteId = Number(this.route.snapshot.paramMap.get('id'));
    void this.load();
  }

  async load(): Promise<void> {
    try {
      this.data.set(await resgateApi.relatorio(this.loteId));
    } catch (e) {
      this.erro.set(e instanceof Error ? e.message : 'Erro');
    }
  }

  exportCsv(): void {
    const d = this.data();
    if (!d?.itens?.length) return;
    const rows = [['chave', 'status', 'motivo', 'atualizadoEm']];
    for (const i of d.itens) {
      rows.push([i.chave, i.status, i.motivo ?? '', i.atualizadoEm ?? '']);
    }
    const csv = rows.map((r) => r.map((c) => `"${String(c).replace(/"/g, '""')}"`).join(',')).join('\n');
    const blob = new Blob([csv], { type: 'text/csv;charset=utf-8' });
    const a = document.createElement('a');
    a.href = URL.createObjectURL(blob);
    a.download = `relatorio-lote-${this.loteId}.csv`;
    a.click();
  }
}
