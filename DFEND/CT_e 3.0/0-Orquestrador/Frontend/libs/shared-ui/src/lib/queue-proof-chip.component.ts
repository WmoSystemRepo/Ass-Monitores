import {
  ChangeDetectionStrategy,
  Component,
  computed,
  input,
  output,
} from '@angular/core';

export type QueueProofChipState =
  | 'idle'
  | 'loading'
  | 'clear'
  | 'empty_with_errors'
  | 'not_empty'
  | 'failed';

/**
 * Chip de validação estrita de fila — só UI; o pai dispara a consulta sob demanda.
 */
@Component({
  selector: 'lib-queue-proof-chip',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <div class="queue-proof-chip" [attr.data-state]="state()">
      <button
        type="button"
        class="queue-proof-btn"
        [disabled]="state() === 'loading'"
        [attr.title]="title() || null"
        [attr.aria-busy]="state() === 'loading'"
        (click)="validate.emit()"
      >
        {{ buttonLabel() }}
      </button>
      @if (resultLabel(); as result) {
        <span class="queue-proof-result" [attr.title]="title() || null">{{ result }}</span>
      }
    </div>
  `,
  styles: `
    .queue-proof-chip {
      display: flex;
      flex-direction: column;
      gap: 0.15rem;
      min-width: 0;
    }
    .queue-proof-btn {
      display: inline-flex;
      align-items: center;
      justify-content: center;
      border-radius: 0.375rem;
      border: 1px solid rgb(51 65 85 / 0.9);
      background: rgb(15 23 42 / 0.75);
      padding: 0.2rem 0.55rem;
      font-size: 0.65rem;
      font-weight: 600;
      letter-spacing: 0.04em;
      text-transform: uppercase;
      color: rgb(186 230 253);
      cursor: pointer;
      transition: border-color 0.15s ease, color 0.15s ease, background 0.15s ease;
    }
    .queue-proof-btn:hover:not(:disabled) {
      border-color: rgb(56 189 248 / 0.7);
      color: rgb(224 242 254);
    }
    .queue-proof-btn:disabled {
      cursor: wait;
      opacity: 0.7;
    }
    .queue-proof-result {
      font-size: 0.7rem;
      font-weight: 600;
      line-height: 1.2;
      white-space: nowrap;
    }
    .queue-proof-chip[data-state='clear'] .queue-proof-result {
      color: var(--monitor-success, #a3e635);
    }
    .queue-proof-chip[data-state='empty_with_errors'] .queue-proof-result,
    .queue-proof-chip[data-state='not_empty'] .queue-proof-result,
    .queue-proof-chip[data-state='failed'] .queue-proof-result {
      color: var(--monitor-error, #f87171);
    }
    .queue-proof-chip[data-state='loading'] .queue-proof-result {
      color: var(--monitor-queue, #fbbf24);
    }
  `,
})
export class QueueProofChipComponent {
  readonly state = input<QueueProofChipState>('idle');
  readonly idleLabel = input('Validar');
  readonly resultLabel = input<string | null>(null);
  readonly title = input<string | null>(null);
  readonly validate = output<void>();

  readonly buttonLabel = computed(() =>
    this.state() === 'loading' ? 'Validando…' : this.idleLabel()
  );
}
