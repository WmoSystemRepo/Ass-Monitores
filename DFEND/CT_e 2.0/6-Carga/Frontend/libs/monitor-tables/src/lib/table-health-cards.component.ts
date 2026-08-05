import { ChangeDetectionStrategy, Component, input } from '@angular/core';
import { NgClass } from '@angular/common';
import { RouterLink } from '@angular/router';
import { TableHealthView } from '@Carga/shared-data';
import {
  formatDataAgeSeconds,
  tableHealthStatusLabel,
} from '@Carga/shared-utils';

@Component({
  selector: 'lib-table-health-cards',
  standalone: true,
  imports: [NgClass, RouterLink],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <div class="grid shrink-0 grid-cols-1 gap-2 sm:grid-cols-2 lg:grid-cols-5">
      @for (card of items(); track card.key) {
        <a
          [routerLink]="card.route"
          class="group rounded-md border px-2.5 py-2 transition hover:border-teal-500/50 hover:bg-zinc-900/80"
          [ngClass]="borderClass(card.status)"
          [attr.title]="card.hint"
        >
          <div class="flex items-start justify-between gap-1">
            <p class="text-[10px] font-medium uppercase tracking-wide text-zinc-500">
              {{ card.label }}
            </p>
            <span
              class="rounded px-1.5 py-0.5 text-[9px] font-semibold uppercase"
              [ngClass]="badgeClass(card.status)"
            >
              {{ statusLabel(card.status) }}
            </span>
          </div>
          <p class="mt-1 line-clamp-2 text-xs font-medium text-zinc-100">
            {{ card.primaryValue }}
          </p>
          <div class="mt-1.5 flex flex-wrap gap-x-2 gap-y-0.5 text-[10px] text-zinc-500">
            <span>Idade {{ formatAge(card.dataAgeSeconds) }}</span>
            <span>·</span>
            <span>Consulta {{ card.queryMs }}ms</span>
          </div>
          <p class="mt-1 text-[10px] text-teal-400/80 opacity-0 transition group-hover:opacity-100">
            Ver dados →
          </p>
        </a>
      }
    </div>
  `,
})
export class TableHealthCardsComponent {
  readonly items = input<TableHealthView[]>([]);

  statusLabel(status: string): string {
    return tableHealthStatusLabel(status);
  }

  formatAge(sec?: number | null): string {
    return formatDataAgeSeconds(sec);
  }

  borderClass(status: string): string {
    switch ((status ?? '').toLowerCase()) {
      case 'critico':
        return 'border-rose-500/50 bg-rose-950/20';
      case 'atencao':
        return 'border-amber-500/40 bg-amber-950/15';
      default:
        return 'border-zinc-700/80 bg-zinc-900/40';
    }
  }

  badgeClass(status: string): string {
    switch ((status ?? '').toLowerCase()) {
      case 'critico':
        return 'bg-rose-950 text-rose-300';
      case 'atencao':
        return 'bg-amber-950 text-amber-300';
      default:
        return 'bg-teal-950 text-teal-300';
    }
  }
}
