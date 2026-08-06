import { ChangeDetectionStrategy, Component, computed, input } from '@angular/core';

export type QueueMeterTone = 'agora' | 'fila' | 'idle' | 'error' | 'running';

@Component({
  selector: 'lib-queue-meter',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <div
      class="queue-meter"
      [class.queue-meter-agora]="tone() === 'agora'"
      [class.queue-meter-fila]="tone() === 'fila'"
      [class.queue-meter-idle]="tone() === 'idle'"
      [class.queue-meter-error]="tone() === 'error'"
      [class.queue-meter-running]="tone() === 'running'"
      [attr.aria-label]="ariaLabel()"
      role="img"
    >
      <div class="queue-meter-track" aria-hidden="true">
        @for (chip of chips(); track chip) {
          <span class="queue-meter-chip" [style.--chip-i]="chip"></span>
        }
      </div>
      <span class="queue-meter-depth font-mono">{{ depthLabel() }}</span>
    </div>
  `,
})
export class QueueMeterComponent {
  /** Profundidade real da fila (arquivos). */
  readonly depth = input(0);
  readonly tone = input<QueueMeterTone>('idle');
  /** Cap visual de chips (não altera o número exibido). */
  readonly maxChips = input(10);

  readonly chips = computed(() => {
    const d = Math.max(0, Math.floor(this.depth()));
    if (d <= 0) return [] as number[];
    const n = Math.min(this.maxChips(), Math.max(1, d));
    return Array.from({ length: n }, (_, i) => i);
  });

  readonly depthLabel = computed(() => {
    const d = Math.max(0, Math.floor(this.depth()));
    return d.toLocaleString('pt-BR');
  });

  readonly ariaLabel = computed(() => {
    const d = Math.max(0, Math.floor(this.depth()));
    return d === 0 ? 'Fila vazia' : `${d} arquivo${d === 1 ? '' : 's'} na fila`;
  });
}
