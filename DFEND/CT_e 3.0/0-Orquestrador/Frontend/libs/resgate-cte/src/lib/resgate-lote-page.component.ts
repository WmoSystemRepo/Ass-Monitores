import {
  ChangeDetectionStrategy,
  Component,
  OnDestroy,
  OnInit,
  inject,
  signal,
} from '@angular/core';
import { DatePipe, NgClass } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import {
  getPanelMode,
  resgateApi,
  setPanelMode,
  type PanelMode,
} from './resgate-api';

type PassoTrilha = { id: string; label: string; destaque?: boolean };

const PASSOS_TRILHA: PassoTrilha[] = [
  { id: 'P0', label: 'Fila' },
  { id: 'P1', label: 'Pegou' },
  { id: 'P2', label: 'Consulta AN' },
  { id: 'P3', label: 'Resposta' },
  { id: 'P4', label: 'Banco' },
  { id: 'P5a', label: 'Ignorar' },
  { id: 'P5b', label: 'Entrega XML', destaque: true },
  { id: 'P5c', label: 'Não loc.' },
  { id: 'P5d', label: 'Erro' },
  { id: 'P6', label: 'Log' },
  { id: 'P7', label: 'Fim' },
];

@Component({
  standalone: true,
  imports: [RouterLink, DatePipe, NgClass],
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: 'orq-resgate-lote-page',
  template: `
    <div class="space-y-4 text-indigo-50">
      <div class="flex flex-wrap items-center justify-between gap-3">
        <div>
          <h1 class="text-xl font-semibold">Lote {{ loteId }}</h1>
          <p class="text-xs text-indigo-400">Última atualização: {{ lastFetchAt() || '—' }}</p>
        </div>
        <div class="flex items-center gap-2 text-sm">
          <span
            class="rounded px-2 py-1 text-xs font-semibold"
            [ngClass]="mode() === 'online' ? 'bg-emerald-800' : 'bg-slate-700'"
          >
            {{ mode() === 'online' ? 'Online' : 'Offline' }}
          </span>
          <button type="button" class="rounded border border-indigo-600 px-2 py-1 text-xs" (click)="toggleMode()">
            Alternar Online/Offline
          </button>
          <button type="button" class="rounded border border-indigo-600 px-2 py-1 text-xs" (click)="refresh()">
            Atualizar agora
          </button>
          <a class="rounded bg-violet-800 px-2 py-1 text-xs" [routerLink]="['/resgate/lote', loteId, 'relatorio']">Relatório</a>
        </div>
      </div>

      @if (erro()) {
        <p class="text-sm text-rose-300">{{ erro() }}</p>
      }

      @if (painel(); as p) {
        <div class="rounded border border-indigo-800 bg-indigo-950/50 p-3 text-sm">
          <p class="mb-2 text-xs font-medium text-violet-300">Trilha de passos (P5b = entrega do documento)</p>
          <div class="flex flex-wrap gap-1">
            @for (s of passos; track s.id) {
              <span
                class="rounded px-1.5 py-0.5 text-[10px] font-mono border"
                [ngClass]="passoChipClass(p, s)"
                [attr.title]="s.destaque ? 'Entrega: gravação do XML recuperado' : s.label"
              >
                {{ s.id }}
              </span>
            }
          </div>
          <p class="mt-2">Passo lote: <strong>{{ p.passoAtual || '—' }}</strong> · Chave: <strong>{{ p.chaveAtual || '—' }}</strong></p>
          @if (p.passoAtual === 'P5b' || itemPassoP5b(p)) {
            <p class="mt-1 rounded border border-amber-500 bg-amber-950 px-2 py-1 text-xs text-amber-200">
              P5b — Entrega: gravando XML recuperado no banco sintético
            </p>
          }
          <p class="mt-2 text-indigo-200">
            Total {{ p.lote?.total }} | Recuperados {{ p.lote?.recuperados }} | Existentes {{ p.lote?.existentes }}
            | Não loc. {{ p.lote?.naoLocalizados }} | Erros {{ p.lote?.erros }} · Status {{ p.lote?.status }}
          </p>
        </div>

        <div class="grid gap-4 lg:grid-cols-2">
          <div class="max-h-80 overflow-auto rounded border border-indigo-800">
            <table class="w-full text-left text-xs">
              <thead class="bg-indigo-900 text-indigo-300">
                <tr><th class="p-2">Chave</th><th>Passo</th><th>Status</th></tr>
              </thead>
              <tbody>
                @for (i of p.itens || []; track i.id) {
                  <tr
                    class="border-t border-indigo-900"
                    [ngClass]="i.passoAtual === 'P5b' ? 'bg-amber-950' : ''"
                  >
                    <td class="p-2 font-mono">{{ i.chave }}</td>
                    <td>
                      <span [ngClass]="i.passoAtual === 'P5b' ? 'text-amber-300 font-semibold' : ''">
                        {{ i.passoAtual }}
                      </span>
                    </td>
                    <td>{{ i.status }}</td>
                  </tr>
                }
              </tbody>
            </table>
          </div>
          <div class="max-h-80 overflow-auto rounded border border-indigo-800 p-2 text-xs">
            <p class="mb-2 font-medium text-violet-300">Linha do tempo</p>
            @for (e of p.eventos || []; track e.id) {
              <p
                class="border-b border-indigo-900 py-1"
                [ngClass]="e.passo === 'P5b' ? 'text-amber-200' : 'text-indigo-200'"
              >
                {{ e.horario | date: 'HH:mm:ss' }} — {{ e.mensagem }}
                @if (e.passo === 'P5b') {
                  <span class="text-amber-400">[P5b]</span>
                }
              </p>
            }
          </div>
        </div>
      }
    </div>
  `,
})
export class ResgateLotePageComponent implements OnInit, OnDestroy {
  private readonly route = inject(ActivatedRoute);
  readonly passos = PASSOS_TRILHA;
  loteId = 0;
  readonly mode = signal<PanelMode>(getPanelMode());
  readonly painel = signal<any>(null);
  readonly lastFetchAt = signal<string>('');
  readonly erro = signal<string | null>(null);
  private timer: ReturnType<typeof setInterval> | null = null;

  ngOnInit(): void {
    this.loteId = Number(this.route.snapshot.paramMap.get('id'));
    void this.refresh();
    this.syncPolling();
  }

  ngOnDestroy(): void {
    this.stopPolling();
  }

  passoChipClass(
    p: { passoAtual?: string; itens?: { passoAtual?: string }[] },
    s: PassoTrilha
  ): string {
    const ativo = this.isPassoAtivo(p, s.id);
    if (s.destaque) {
      return ativo
        ? 'border-amber-400 bg-amber-900 text-amber-200'
        : 'border-amber-400 text-amber-200';
    }
    if (ativo) return 'border-indigo-700 bg-violet-800';
    return 'border-indigo-700 opacity-40';
  }

  isPassoAtivo(p: { passoAtual?: string; itens?: { passoAtual?: string }[] }, passoId: string): boolean {
    if (p.passoAtual === passoId) return true;
    return (p.itens ?? []).some((i) => i.passoAtual === passoId);
  }

  itemPassoP5b(p: { itens?: { passoAtual?: string }[] }): boolean {
    return (p.itens ?? []).some((i) => i.passoAtual === 'P5b');
  }

  toggleMode(): void {
    const next = this.mode() === 'online' ? 'offline' : 'online';
    setPanelMode(next);
    this.mode.set(next);
    this.syncPolling();
  }

  async refresh(): Promise<void> {
    try {
      const data = await resgateApi.aoVivo(this.loteId);
      this.painel.set(data);
      this.lastFetchAt.set(new Date().toLocaleTimeString());
      this.erro.set(null);
      const status = (data as any)?.lote?.status;
      if (status === 'Concluido') this.stopPolling();
    } catch (e) {
      this.erro.set(e instanceof Error ? e.message : 'Falha de rede — UI Offline até recuperar');
    }
  }

  private syncPolling(): void {
    this.stopPolling();
    if (this.mode() === 'online') {
      this.timer = setInterval(() => void this.refresh(), 1500);
    }
  }

  private stopPolling(): void {
    if (this.timer) {
      clearInterval(this.timer);
      this.timer = null;
    }
  }
}
