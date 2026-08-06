import {
  ChangeDetectionStrategy,
  Component,
  computed,
  input,
  output,
} from '@angular/core';
import { QueueMeterComponent, type QueueMeterTone } from './queue-meter.component';

export type StationBadge = 'agora' | 'fila' | 'erro' | 'ativo' | 'parado' | 'ligando' | 'desligando';

@Component({
  selector: 'lib-station-card',
  standalone: true,
  imports: [QueueMeterComponent],
  changeDetection: ChangeDetectionStrategy.OnPush,
  styles: [
    `
      :host {
        display: contents;
      }
    `,
  ],
  template: `
    <button
      type="button"
      class="station-card anatomy-stage anatomy-stage-link"
      [class.station-card-agora]="badge() === 'agora'"
      [class.station-card-fila]="badge() === 'fila'"
      [class.station-card-erro]="badge() === 'erro'"
      [class.station-card-ativo]="badge() === 'ativo'"
      [class.station-card-parado]="badge() === 'parado'"
      [class.station-card-muted]="muted()"
      [class.station-card-booting]="booting()"
      [class.anatomy-stage-active]="badge() === 'agora'"
      [class.anatomy-stage-queued]="badge() === 'fila'"
      [class.anatomy-stage-error]="badge() === 'erro'"
      [class.anatomy-stage-running]="badge() === 'ativo'"
      [class.anatomy-stage-muted]="muted()"
      [class.anatomy-stage-booting]="booting()"
      [style.--boot-delay]="bootDelay()"
      [attr.title]="titleAttr()"
      (click)="opened.emit()"
    >
      <span
        class="station-badge"
        [class.station-badge-agora]="badge() === 'agora'"
        [class.station-badge-fila]="badge() === 'fila'"
        [class.station-badge-erro]="badge() === 'erro'"
        [class.station-badge-ativo]="badge() === 'ativo'"
        [class.station-badge-parado]="badge() === 'parado' || badge() === 'ligando' || badge() === 'desligando'"
      >
        {{ badgeLabel() }}
      </span>

      <div class="station-body">
        <span class="station-symbol" aria-hidden="true">{{ symbol() }}</span>
        <lib-queue-meter [depth]="depth()" [tone]="meterTone()" />
      </div>

      <p class="station-title anatomy-stage-title">{{ label() }}</p>
      <p class="station-metric font-mono" [attr.title]="metric()">{{ metric() }}</p>
      @if (processHint()) {
        <p class="station-process" [title]="processHint()!">{{ processHint() }}</p>
      }
    </button>
  `,
})
export class StationCardComponent {
  readonly symbol = input.required<string>();
  readonly label = input.required<string>();
  readonly metric = input('—');
  readonly depth = input(0);
  readonly badge = input<StationBadge>('parado');
  readonly processHint = input<string | null>(null);
  readonly muted = input(false);
  readonly booting = input(false);
  readonly bootDelay = input('0s');
  readonly titleAttr = input('');

  readonly opened = output<void>();

  readonly badgeLabel = computed(() => {
    switch (this.badge()) {
      case 'agora':
        return 'AGORA';
      case 'fila':
        return 'NA FILA';
      case 'erro':
        return 'ERRO';
      case 'ativo':
        return 'ATIVO';
      case 'ligando':
        return 'LIGANDO';
      case 'desligando':
        return 'DESLIGANDO';
      default:
        return 'PARADO';
    }
  });

  readonly meterTone = computed((): QueueMeterTone => {
    switch (this.badge()) {
      case 'agora':
        return 'agora';
      case 'fila':
        return 'fila';
      case 'erro':
        return 'error';
      case 'ativo':
        return 'running';
      default:
        return 'idle';
    }
  });
}
