import { ChangeDetectionStrategy, Component, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import {
  clearToken,
  getToken,
  resgateApi,
  ResgateUnauthorizedError,
  type EnfileirarResult,
  type FilaDownloadStatus,
  type StatusChavesResult,
} from './resgate-api';

@Component({
  standalone: true,
  imports: [FormsModule, RouterLink],
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: 'orq-resgate-novo-page',
  template: `
    <div class="space-y-6 text-indigo-50">
      <header>
        <h1 class="text-2xl font-semibold">Resgate CT-e AN</h1>
        <p class="mt-1 text-sm text-indigo-300">
          Informe chaves de acesso (44 dígitos). O Resgate só informa as chaves — o Download (Carga) executa a baixa.
          NSU não é usado nesta demanda.
        </p>
        <ul class="mt-2 list-inside list-disc text-xs text-indigo-400">
          <li>Requer Carga ligada: Executar=1, ExecutarAuto=1, CodServico 99</li>
          <li><strong class="text-indigo-200">Enfileirado ≠ resgatado</strong> — acompanhe a fila / status abaixo</li>
          <li>Monitor Carga: <a class="underline text-violet-300" routerLink="/monitores/carga">/monitores/carga</a></li>
        </ul>
      </header>

      @if (!autenticado()) {
        <section class="max-w-md space-y-3 rounded border border-indigo-800 bg-indigo-950/60 p-4">
          <h2 class="text-sm font-medium text-violet-300">Login DEV</h2>
          <p class="text-xs text-indigo-400">
            Padrão <strong class="text-indigo-100">dev / dev</strong>
          </p>
          <label class="block text-xs text-indigo-400">Usuário
            <input class="mt-1 w-full rounded border border-indigo-700 bg-indigo-900 px-3 py-2" [(ngModel)]="usuario" name="usuario" autocomplete="username" />
          </label>
          <label class="block text-xs text-indigo-400">Senha
            <input type="password" class="mt-1 w-full rounded border border-indigo-700 bg-indigo-900 px-3 py-2" [(ngModel)]="senha" name="senha" autocomplete="current-password" />
          </label>
          @if (erro()) {
            <p class="text-sm text-rose-300">{{ erro() }}</p>
          }
          <button type="button" class="rounded bg-violet-700 px-4 py-2 text-sm hover:bg-violet-600" (click)="login()">Entrar</button>
        </section>
      } @else {
        <section class="space-y-4 rounded border border-indigo-800 bg-indigo-950/60 p-4">
          <div class="flex items-center justify-between">
            <h2 class="text-sm font-medium text-violet-300">Informar chaves ao Download</h2>
            <button type="button" class="text-xs text-indigo-400 underline" (click)="logout()">Sair</button>
          </div>
          <label class="block text-xs text-indigo-400">Arquivo CSV / TXT / XLSX
            <input type="file" accept=".csv,.txt,.xlsx" class="mt-1 block w-full text-sm" (change)="onFile($event)" />
          </label>
          @if (arquivoSelecionado()) {
            <p class="text-xs text-indigo-300">Arquivo: {{ arquivoSelecionado() }}</p>
          }
          <label class="block text-xs text-indigo-400">Ou cole/digite (uma chave por linha)
            <textarea class="mt-1 h-40 w-full rounded border border-indigo-700 bg-indigo-900 px-3 py-2 font-mono text-xs" [(ngModel)]="texto" name="texto" (ngModelChange)="recalcular()"></textarea>
          </label>
          <p class="text-lg">Quantidade: <strong>{{ quantidade() }}</strong> <span class="text-xs text-indigo-400">(1–1000)</span></p>
          @if (erro()) {
            <div class="rounded border border-rose-700/80 bg-rose-950/50 px-3 py-2 text-sm text-rose-200">
              <p class="font-semibold text-rose-100">Falha</p>
              <p class="mt-1 break-words whitespace-pre-wrap">{{ erro() }}</p>
            </div>
          }
          @if (sucesso(); as ok) {
            <div class="rounded border border-amber-700/80 bg-amber-950/40 px-3 py-2 text-sm text-amber-100">
              <p class="font-semibold">Chaves aceitas e enfileiradas</p>
              <p class="mt-1 text-xs text-amber-200">{{ ok.aviso || 'Enfileirado não significa resgatado.' }}</p>
              <p class="mt-1">{{ ok.mensagem }}</p>
              <p class="mt-1 text-xs text-amber-300">
                {{ ok.enfileirados }} chave(s) · temp: {{ ok.pendentesTemp }} · fila broker: {{ ok.profundidadeFilaBroker ?? '—' }}
                @if (ok.idadeMaxTempMinutos != null) {
                  · idade máx. temp: {{ ok.idadeMaxTempMinutos }} min
                }
              </p>
              @if (ok.riscoFila) {
                <p class="mt-1 text-xs text-amber-400">{{ ok.riscoFila }}</p>
              }
            </div>
          }
          <div class="flex flex-wrap gap-2">
            <button type="button" class="rounded bg-violet-700 px-4 py-2 text-sm hover:bg-violet-600 disabled:opacity-40" [disabled]="!podeProcessar() || processando()" (click)="processar()">
              {{ processando() ? 'Enviando…' : 'Processar' }}
            </button>
            <button type="button" class="rounded border border-indigo-600 px-4 py-2 text-sm hover:bg-indigo-900 disabled:opacity-40" [disabled]="consultando()" (click)="verFila()">
              Ver fila Download
            </button>
            <button type="button" class="rounded border border-indigo-600 px-4 py-2 text-sm hover:bg-indigo-900 disabled:opacity-40" [disabled]="!ultimasChaves().length || consultando()" (click)="atualizarStatus()">
              Atualizar status das chaves
            </button>
          </div>
        </section>

        @if (fila(); as f) {
          <section class="space-y-2 rounded border border-indigo-800 bg-indigo-950/40 p-4 text-sm">
            <h2 class="text-sm font-medium text-violet-300">Fila Download (temp)</h2>
            <p class="text-xs text-indigo-400">{{ f.aviso || f.mensagem }}</p>
            <p class="text-xs text-indigo-300">
              Pendentes: {{ f.pendentesTemp }} · Broker: {{ f.profundidadeFilaBroker }}
              @if (f.idadeMaxTempMinutos != null) {
                · idade máx.: {{ f.idadeMaxTempMinutos }} min
              }
            </p>
            @if (f.riscoConcorrencia) {
              <p class="text-xs text-amber-300">{{ f.riscoConcorrencia }}</p>
            }
            <ul class="max-h-48 space-y-1 overflow-auto font-mono text-xs">
              @for (i of f.itens; track i.id) {
                <li>
                  <span class="text-violet-300">{{ i.status }}</span>
                  — {{ i.chaveMascarada }}
                  @if (i.erro) {
                    <span class="text-rose-300"> ({{ i.erro }})</span>
                  }
                </li>
              }
            </ul>
          </section>
        }

        @if (statusChaves(); as st) {
          <section class="space-y-2 rounded border border-indigo-800 bg-indigo-950/40 p-4 text-sm">
            <h2 class="text-sm font-medium text-violet-300">Status das chaves enviadas</h2>
            <p class="text-xs text-indigo-400">{{ st.aviso }}</p>
            <ul class="max-h-48 space-y-1 overflow-auto font-mono text-xs">
              @for (i of st.itens; track i.chaveMascarada) {
                <li>
                  <span
                    [class]="
                      i.status === 'Baixado'
                        ? 'text-emerald-300'
                        : i.status === 'Erro'
                          ? 'text-rose-300'
                          : i.status === 'Pendente'
                            ? 'text-amber-300'
                            : 'text-indigo-300'
                    "
                    >{{ i.status }}</span
                  >
                  — {{ i.chaveMascarada }}
                  @if (i.detalhe) {
                    <span class="text-indigo-500"> · {{ i.detalhe }}</span>
                  }
                </li>
              }
            </ul>
          </section>
        }
      }
    </div>
  `,
})
export class ResgateNovoPageComponent {
  usuario = 'dev';
  senha = 'dev';
  texto = '';
  readonly autenticado = signal(!!getToken());
  readonly quantidade = signal(0);
  readonly erro = signal<string | null>(null);
  readonly sucesso = signal<EnfileirarResult | null>(null);
  readonly fila = signal<FilaDownloadStatus | null>(null);
  readonly statusChaves = signal<StatusChavesResult | null>(null);
  readonly ultimasChaves = signal<string[]>([]);
  readonly arquivoSelecionado = signal<string | null>(null);
  readonly processando = signal(false);
  readonly consultando = signal(false);
  private chaves: string[] = [];
  private arquivoUpload: File | null = null;

  async login(): Promise<void> {
    this.erro.set(null);
    this.sucesso.set(null);
    try {
      await resgateApi.login(this.usuario, this.senha);
      this.autenticado.set(true);
    } catch (e) {
      this.erro.set(e instanceof Error ? e.message : 'Login inválido');
    }
  }

  logout(): void {
    clearToken();
    this.autenticado.set(false);
    this.sucesso.set(null);
    this.fila.set(null);
    this.statusChaves.set(null);
  }

  private reportarErro(e: unknown, fallback: string): void {
    if (e instanceof ResgateUnauthorizedError) {
      this.logout();
      this.erro.set(e.message);
      return;
    }
    this.erro.set(e instanceof Error ? e.message : fallback);
  }

  onFile(ev: Event): void {
    const input = ev.target as HTMLInputElement;
    const file = input.files?.[0];
    if (!file) return;
    if (file.size > 5 * 1024 * 1024) {
      this.erro.set('Arquivo maior que 5 MB');
      return;
    }
    const ext = file.name.split('.').pop()?.toLowerCase();
    if (ext === 'xlsx') {
      this.arquivoUpload = file;
      this.arquivoSelecionado.set(file.name);
      this.texto = '';
      this.chaves = [];
      this.quantidade.set(0);
      this.erro.set(null);
      this.sucesso.set(null);
      return;
    }
    this.arquivoUpload = null;
    this.arquivoSelecionado.set(null);
    const reader = new FileReader();
    reader.onload = () => {
      this.texto = String(reader.result ?? '');
      this.recalcular();
    };
    reader.readAsText(file);
  }

  recalcular(): void {
    this.arquivoUpload = null;
    this.arquivoSelecionado.set(null);
    this.sucesso.set(null);
    const lines = this.texto
      .split(/\r?\n/)
      .map((l) => l.split(/[;,]/)[0].trim())
      .filter((l) => l && !/^chave(_acesso)?$/i.test(l));
    const seen = new Set<string>();
    const valid: string[] = [];
    const invalid: string[] = [];
    for (const l of lines) {
      if (!/^\d{44}$/.test(l)) {
        invalid.push(l);
        continue;
      }
      if (!seen.has(l)) {
        seen.add(l);
        valid.push(l);
      }
    }
    this.chaves = valid;
    this.quantidade.set(valid.length);
    if (invalid.length) this.erro.set(`${invalid.length} chave(s) inválida(s) — envio será rejeitado`);
    else if (valid.length < 1 || valid.length > 1000) this.erro.set('Quantidade fora de 1–1000');
    else this.erro.set(null);
  }

  podeProcessar(): boolean {
    if (this.arquivoUpload) return true;
    return this.chaves.length >= 1 && this.chaves.length <= 1000 && !this.erro();
  }

  async processar(): Promise<void> {
    this.erro.set(null);
    this.sucesso.set(null);
    this.processando.set(true);
    try {
      const enviadas = [...this.chaves];
      const result = this.arquivoUpload
        ? await resgateApi.uploadEnfileirar(this.arquivoUpload)
        : await resgateApi.enfileirarDownload(this.chaves);
      this.sucesso.set(result);
      if (enviadas.length) this.ultimasChaves.set(enviadas);
      await this.verFila();
      if (enviadas.length) await this.atualizarStatus();
    } catch (e) {
      this.reportarErro(e, 'Erro');
    } finally {
      this.processando.set(false);
    }
  }

  async verFila(): Promise<void> {
    this.consultando.set(true);
    this.erro.set(null);
    try {
      this.fila.set(await resgateApi.filaDownload());
    } catch (e) {
      this.reportarErro(e, 'Erro ao consultar fila');
    } finally {
      this.consultando.set(false);
    }
  }

  async atualizarStatus(): Promise<void> {
    const keys = this.ultimasChaves();
    if (!keys.length) return;
    this.consultando.set(true);
    this.erro.set(null);
    try {
      this.statusChaves.set(await resgateApi.statusChaves(keys));
    } catch (e) {
      this.reportarErro(e, 'Erro ao consultar status');
    } finally {
      this.consultando.set(false);
    }
  }
}
