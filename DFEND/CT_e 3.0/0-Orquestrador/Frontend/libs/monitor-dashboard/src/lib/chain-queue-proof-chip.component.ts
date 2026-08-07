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
  template: `
    <lib-queue-proof-chip
      idleLabel="Validar cadeia"
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
    if (this.failedMessage()) return 'Falha na validação';
    const p = this.proof();
    if (!p) return null;
    if (!p.ok) return 'Falha na validação';
    if (p.isClear) {
      const t = formatLocalTime(p.verifiedAtUtc);
      return t ? `Validada vazia · ${t}` : 'Validada vazia';
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
      return 'Valida temp + filas Service Broker dos 6 serviços (contagem estrita, sem READPAST).';
    }
    const perService = (p.services ?? [])
      .map(
        (s) =>
          `${s.serviceId}: temp=${s.tempCount} fila=${s.brokerCount} erros=${s.tempErrorCount}`
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
  return 'Não foi possível validar a cadeia.';
}
