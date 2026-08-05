import { ChangeDetectionStrategy, Component } from '@angular/core';

/** Legacy stub — route removed; kept so old imports do not break. */
@Component({
  selector: 'lib-anatomy-flow',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `<p class="text-sm text-indigo-300">Use lib-chain-anatomy.</p>`,
})
export class AnatomyFlowComponent {}

export type AnatomyStage = string;
export interface FlyingPacket {
  id: string;
  kind: string;
  label: string;
  lane: number;
}
