import { ChangeDetectionStrategy, Component } from '@angular/core';

@Component({
  selector: 'lib-status-legend',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <ul class="anatomy-legend status-legend" aria-label="Legenda de status">
      <li class="anatomy-legend-item">
        <span class="anatomy-legend-swatch anatomy-legend-error"></span>
        Erro
      </li>
      <li class="anatomy-legend-item">
        <span class="anatomy-legend-swatch anatomy-legend-agora"></span>
        Agora
      </li>
      <li class="anatomy-legend-item">
        <span class="anatomy-legend-swatch anatomy-legend-queue"></span>
        Na fila
      </li>
      <li class="anatomy-legend-item">
        <span class="anatomy-legend-swatch anatomy-legend-running"></span>
        Ativo
      </li>
      <li class="anatomy-legend-item">
        <span class="anatomy-legend-swatch anatomy-legend-stopped"></span>
        Parado
      </li>
    </ul>
  `,
})
export class StatusLegendComponent {}
