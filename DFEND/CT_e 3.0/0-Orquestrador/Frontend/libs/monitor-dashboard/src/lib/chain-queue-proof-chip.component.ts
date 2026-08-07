import {
  ChangeDetectionStrategy,
  Component,
  computed,
  inject,
  signal,
} from '@angular/core';
import { MonitorApiService } from '@orquestrador/monitor-core';
import {
  QueueProofChipComponent,
  type QueueProofChipState,
} from '@orquestrador/shared-ui';
import type { ChainQueueProof } from '@orquestrador/shared-data';

@Component({
  selector: 'lib-chain-queue-proof-chip',
  standalone: true,
  imports: [QueueProofChipComponent],
  changeDetection: ChangeDetectionStrategy.OnPush,
  host: {
    'data-tour': 'validate',
  },
  template: `
    <lib-queue-proof-chip
      idleLabel="Validar filas"
      [state]="state()"
      [resultLabel]="resultLabel()"
      [title]="tooltip()"
      (validate)="run()"
    />
  `,
})
export class ChainQueueProofChipComponent {
  private readonly api = inject(MonitorApiService);

  private readonly proof = signal<ChainQueueProof | null>(null);
  private readonly loading = signal(false);
  private readonly failedMessage = signal<string | null>(null);

  readonly state = computed<QueueProofChipState>(() => {
    if (this.loading()) return 'loading';
    if (this.failedMessage()) return 'failed';
    const p = this.proof();
    if (!p) return 'idle';
    if (!p.ok) return 'failed';
    if (p.isClear) return 'clear';
    if (p.isEmpty && p.tempErrorCount > 0) return 'empty_with_errors';
    return 'not_empty';
  });

  readonly resultLabel = computed(() => {
    if (this.loading()) return null;
    if (this.failedMessage()) {
      const msg = this.failedMessage()!;
      return msg.length > 48 ? 'Falha na validação — veja o detalhe' : msg;
    }
    const p = this.proof();
    if (!p) return null;
    if (!p.ok) {
      const first = p.errors?.[0];
      if (p.tempCount + p.brokerCount > 0) {
        return `Atenção · ${p.tempCount} temp / ${p.brokerCount} fila`;
      }
      return first && first.length <= 48 ? first : 'Falha na validação — veja o detalhe';
    }
    if (p.isClear) {
      const t = formatLocalTime(p.verifiedAtUtc);
      return t ? `Filas vazias · ${t}` : 'Filas vazias';
    }
    if (p.isEmpty && p.tempErrorCount > 0) {
      return `${p.tempErrorCount} com erro`;
    }
    return `Há ${p.tempCount} na temp / ${p.brokerCount} na fila`;
  });

  readonly tooltip = computed(() => {
    const err = this.failedMessage();
    if (err) return err;
    const p = this.proof();
    if (!p) {
      return 'Confere se as filas e temporárias dos 6 serviços estão vazias (contagem estrita no SQL).';
    }
    const perService = (p.services ?? [])
      .map(
        (s) =>
          `${s.serviceId}: temp=${s.tempCount} fila=${s.brokerCount} erros=${s.tempErrorCount}` +
          (s.ok ? '' : ` · falhou: ${(s.errors ?? []).join(', ') || 'erro'}`)
      )
      .join('\n');
    return [
      `Total temp=${p.tempCount} fila=${p.brokerCount} erros=${p.tempErrorCount}`,
      `Em: ${p.verifiedAtUtc}`,
      perService,
      ...(p.errors?.length ? p.errors : []),
    ]
      .filter(Boolean)
      .join('\n');
  });

  run(): void {
    if (this.loading()) return;
    this.loading.set(true);
    this.failedMessage.set(null);
    this.api.chainQueueProof().subscribe({
      next: (p) => {
        this.proof.set(p);
        this.loading.set(false);
        if (!p.ok) {
          this.failedMessage.set(p.errors?.join('; ') || 'Validação retornou ok=false.');
        }
      },
      error: (e: unknown) => {
        this.proof.set(null);
        this.loading.set(false);
        this.failedMessage.set(extractHttpError(e));
      },
    });
  }
}

function formatLocalTime(iso: string): string | null {
  const d = new Date(iso);
  if (Number.isNaN(d.getTime())) return null;
  return d.toLocaleTimeString('pt-BR', { hour: '2-digit', minute: '2-digit', second: '2-digit' });
}

function extractHttpError(e: unknown): string {
  if (e && typeof e === 'object' && 'error' in e) {
    const body = (e as { error?: { message?: string; detail?: string } }).error;
    if (body?.detail) return body.detail;
    if (body?.message) return body.message;
  }
  if (e instanceof Error) return e.message;
  return 'Não foi possível validar as filas.';
}
