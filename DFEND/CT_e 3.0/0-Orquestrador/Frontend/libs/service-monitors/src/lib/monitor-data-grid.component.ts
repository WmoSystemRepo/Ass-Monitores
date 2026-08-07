import {
  ChangeDetectionStrategy,
  Component,
  DestroyRef,
  computed,
  effect,
  inject,
  input,
  signal,
} from '@angular/core';
import { NgClass } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ScrollingModule } from '@angular/cdk/scrolling';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { Subject, debounceTime } from 'rxjs';

export interface MonitorGridColumn<T = Record<string, unknown>> {
  id: string;
  header: string;
  /** Extrai texto para célula e filtro. */
  value: (row: T) => string;
  filterable?: boolean;
  filterPlaceholder?: string;
  mono?: boolean;
  /** Classe extra na célula (ex.: text-rose-300). */
  cellClass?: (row: T) => string;
  /** Destaca a linha inteira (ex.: erro). */
  rowHighlight?: (row: T) => boolean;
}

const VIRTUAL_SCROLL_THRESHOLD = 300;

@Component({
  selector: 'lib-monitor-data-grid',
  standalone: true,
  imports: [NgClass, FormsModule, ScrollingModule],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <div class="flex flex-col gap-2">
      <p class="text-xs text-slate-500">{{ footerLabel() }}</p>
      <div class="overflow-auto rounded border border-slate-700" style="max-height: 70vh">
        <table class="min-w-full text-left text-sm">
          <thead class="sticky top-0 z-10 bg-slate-900 text-xs uppercase text-slate-500 shadow-sm">
            <tr>
              @for (col of columns(); track col.id) {
                <th class="px-3 py-2 font-semibold">{{ col.header }}</th>
              }
            </tr>
            @if (hasFilters()) {
              <tr class="border-t border-slate-800 normal-case">
                @for (col of columns(); track col.id) {
                  <th class="px-2 py-1.5">
                    @if (col.filterable) {
                      <input
                        type="search"
                        class="w-full min-w-[4.5rem] rounded border border-slate-700 bg-slate-950 px-1.5 py-1 text-[11px] font-normal text-slate-200 placeholder:text-slate-600 focus:border-cyan-600 focus:outline-none"
                        [placeholder]="col.filterPlaceholder || 'Filtrar…'"
                        [ngModel]="draftFilters()[col.id] ?? ''"
                        (ngModelChange)="onFilterInput(col.id, $event)"
                      />
                    }
                  </th>
                }
              </tr>
            }
          </thead>
        </table>
        @if (useVirtual()) {
          <cdk-virtual-scroll-viewport [itemSize]="36" class="block h-[60vh] w-full">
            <div
              *cdkVirtualFor="let row of filteredRows(); trackBy: trackByFn"
              class="grid border-t border-slate-800 text-sm"
              [style.grid-template-columns]="gridTemplate()"
              [ngClass]="rowClass(row)"
              style="height: 36px; min-width: 100%"
            >
              @for (col of columns(); track col.id) {
                <div
                  class="truncate px-3 py-2"
                  [ngClass]="cellNgClass(col, row)"
                  [title]="col.value(row)"
                >
                  {{ col.value(row) }}
                </div>
              }
            </div>
          </cdk-virtual-scroll-viewport>
        } @else {
          <table class="min-w-full text-left text-sm">
            <tbody>
              @for (row of filteredRows(); track trackByFn($index, row)) {
                <tr class="border-t border-slate-800" [ngClass]="rowClass(row)">
                  @for (col of columns(); track col.id) {
                    <td
                      class="max-w-[18rem] truncate px-3 py-2"
                      [ngClass]="cellNgClass(col, row)"
                      [title]="col.value(row)"
                    >
                      {{ col.value(row) }}
                    </td>
                  }
                </tr>
              } @empty {
                <tr>
                  <td
                    [attr.colspan]="columns().length"
                    class="px-3 py-4 text-slate-500"
                  >
                    {{ emptyMessage() }}
                  </td>
                </tr>
              }
            </tbody>
          </table>
        }
      </div>
    </div>
  `,
})
export class MonitorDataGridComponent<T extends object = Record<string, unknown>> {
  private readonly destroyRef = inject(DestroyRef);

  readonly rows = input<T[]>([]);
  readonly columns = input.required<MonitorGridColumn<T>[]>();
  readonly emptyMessage = input('Nenhuma linha.');
  readonly takeApplied = input<number | null>(1000);
  readonly rowTrackId = input<(row: T) => string | number>((row) =>
    String((row as { nsu?: number; seqLog?: number; key?: string }).nsu
      ?? (row as { seqLog?: number }).seqLog
      ?? (row as { key?: string }).key
      ?? '')
  );

  /** Partial: chave ausente é undefined em runtime (Record tipava string e gerava NG8102 no ??). */
  readonly draftFilters = signal<Partial<Record<string, string>>>({});
  readonly appliedFilters = signal<Partial<Record<string, string>>>({});

  private readonly filter$ = new Subject<{ id: string; value: string }>();
  private lastColumnKey = '';

  readonly hasFilters = computed(() => this.columns().some((c) => c.filterable));

  readonly filteredRows = computed(() => {
    const rows = this.rows();
    const filters = this.appliedFilters();
    const cols = this.columns().filter(
      (c) => c.filterable && (filters[c.id] ?? '').trim().length > 0
    );
    if (cols.length === 0) return rows;
    return rows.filter((row) =>
      cols.every((col) => {
        const q = (filters[col.id] ?? '').trim().toLowerCase();
        return col.value(row).toLowerCase().includes(q);
      })
    );
  });

  readonly activeFilterCount = computed(
    () =>
      Object.values(this.appliedFilters()).filter((v) => (v ?? '').trim().length > 0)
        .length
  );

  readonly useVirtual = computed(
    () => this.filteredRows().length > VIRTUAL_SCROLL_THRESHOLD
  );

  readonly gridTemplate = computed(() =>
    this.columns()
      .map(() => 'minmax(6rem, 1fr)')
      .join(' ')
  );

  readonly footerLabel = computed(() => {
    const shown = this.filteredRows().length;
    const total = this.rows().length;
    const max = this.takeApplied() ?? 1000;
    const filters = this.activeFilterCount();
    const lineLabel =
      filters > 0 && shown !== total
        ? `${shown} de ${total} linhas`
        : `${shown} linha${shown === 1 ? '' : 's'}`;
    const filterLabel =
      filters > 0
        ? `${filters} filtro${filters === 1 ? '' : 's'} ativo${filters === 1 ? '' : 's'}`
        : 'sem filtros';
    return `${lineLabel} · ${filterLabel} · máx. ${max} · sem XML`;
  });

  constructor() {
    this.filter$
      .pipe(debounceTime(250), takeUntilDestroyed(this.destroyRef))
      .subscribe(({ id, value }) => {
        this.appliedFilters.update((prev) => ({ ...prev, [id]: value }));
      });

    effect(() => {
      const key = this.columns()
        .map((c) => c.id)
        .join('|');
      if (key !== this.lastColumnKey) {
        this.lastColumnKey = key;
        this.draftFilters.set({});
        this.appliedFilters.set({});
      }
    });
  }

  onFilterInput(id: string, value: string): void {
    this.draftFilters.update((prev) => ({ ...prev, [id]: value }));
    this.filter$.next({ id, value });
  }

  trackByFn = (index: number, row: T): string | number => {
    const id = this.rowTrackId()(row);
    return id === '' ? index : id;
  };

  rowClass(row: T): string {
    const highlight = this.columns().some((c) => c.rowHighlight?.(row));
    return highlight ? 'bg-rose-950/20' : '';
  }

  cellNgClass(col: MonitorGridColumn<T>, row: T): string {
    const parts: string[] = [];
    if (col.mono) parts.push('font-mono');
    const extra = col.cellClass?.(row) ?? '';
    if (col.mono && !extra.includes('rose')) parts.push('text-cyan-300');
    if (extra) parts.push(extra);
    return parts.join(' ');
  }
}
