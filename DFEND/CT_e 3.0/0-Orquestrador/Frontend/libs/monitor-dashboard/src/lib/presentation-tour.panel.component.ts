import {
  ChangeDetectionStrategy,
  Component,
  HostListener,
  OnDestroy,
  effect,
  inject,
  signal,
} from '@angular/core';
import { PresentationTourStore } from './presentation-tour.store';

interface SpotlightBox {
  top: number;
  left: number;
  width: number;
  height: number;
  label: string;
  /** Quando o alvo está perto do topo, o título vai abaixo do anel. */
  labelBelow: boolean;
}

const SPOTLIGHT_PAD = 6;
const FIND_RETRIES = 6;
const FIND_DELAY_MS = 120;

@Component({
  selector: 'lib-presentation-tour-panel',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    @if (tour.active()) {
      @if (spotlight(); as spot) {
        <div
          class="tour-spotlight-ring"
          [class.tour-spotlight-ring-label-below]="spot.labelBelow"
          [style.top.px]="spot.top"
          [style.left.px]="spot.left"
          [style.width.px]="spot.width"
          [style.height.px]="spot.height"
          aria-hidden="true"
        >
          <span class="tour-spotlight-label">{{ spot.label }}</span>
        </div>
      }

      <div
        class="presentation-tour-panel"
        [class.presentation-tour-panel-top]="tour.panelPlacement() === 'top'"
        [class.presentation-tour-panel-left]="tour.panelPlacement() === 'left'"
        [class.presentation-tour-panel-right]="tour.panelPlacement() === 'right'"
        role="dialog"
        aria-label="Apresentação"
      >
        <div class="presentation-tour-meta">
          <span class="presentation-tour-kicker">Apresentação guiada</span>
          <span class="presentation-tour-count">Passo {{ tour.stepLabel() }}</span>
        </div>

        @if (tour.step(); as step) {
          @if (step.spotlightLabel) {
            <p class="presentation-tour-looking">
              <span class="presentation-tour-looking-dot" aria-hidden="true"></span>
              Olhe para:
              <strong>{{ step.spotlightLabel }}</strong>
            </p>
          }

          <h3 class="presentation-tour-title">{{ step.title }}</h3>

          <ul class="presentation-tour-lines">
            @for (line of step.lines; track $index) {
              <li>{{ line }}</li>
            }
          </ul>

          @if (tour.isSimulating()) {
            <p class="presentation-tour-sim">
              Demonstração visual — não são documentos reais.
            </p>
          }
        }

        <div class="presentation-tour-actions">
          <button
            type="button"
            class="presentation-tour-btn presentation-tour-btn-ghost"
            (click)="tour.exit()"
          >
            Sair
          </button>
          <button
            type="button"
            class="presentation-tour-btn"
            (click)="tour.restart()"
          >
            Reiniciar
          </button>
          <button
            type="button"
            class="presentation-tour-btn"
            [disabled]="!tour.canBack()"
            (click)="tour.back()"
          >
            Voltar
          </button>
          @if (tour.isLastStep()) {
            <button
              type="button"
              class="presentation-tour-btn presentation-tour-btn-primary"
              (click)="tour.exit()"
            >
              Finalizar
            </button>
          } @else {
            <button
              type="button"
              class="presentation-tour-btn presentation-tour-btn-primary"
              [disabled]="!tour.canNext()"
              (click)="tour.next()"
            >
              Avançar
            </button>
          }
        </div>
      </div>
    }
  `,
  styles: `
    .tour-spotlight-ring {
      position: fixed;
      z-index: 55;
      pointer-events: none;
      border-radius: 0.65rem;
      outline: 3px solid #fbbf24;
      outline-offset: 0;
      box-shadow:
        0 0 0 8px rgba(251, 191, 36, 0.22),
        0 0 36px rgba(251, 191, 36, 0.45),
        0 12px 28px rgba(2, 6, 23, 0.45);
      animation: tour-target-pulse 1.8s ease-in-out infinite;
    }
    .tour-spotlight-label {
      position: absolute;
      left: 50%;
      top: -0.4rem;
      transform: translate(-50%, -100%);
      z-index: 56;
      max-width: min(18rem, 80vw);
      padding: 0.35rem 0.7rem;
      border-radius: 9999px;
      border: 1px solid rgba(251, 191, 36, 0.85);
      background: #78350f;
      color: #fffbeb;
      font-size: 0.72rem;
      font-weight: 750;
      letter-spacing: 0.02em;
      white-space: nowrap;
      overflow: hidden;
      text-overflow: ellipsis;
      box-shadow: 0 8px 18px rgba(2, 6, 23, 0.5);
      pointer-events: none;
    }
    .tour-spotlight-ring-label-below .tour-spotlight-label {
      top: auto;
      bottom: -0.4rem;
      transform: translate(-50%, 100%);
    }
    .presentation-tour-panel {
      position: fixed;
      left: 50%;
      bottom: 1.1rem;
      top: auto;
      right: auto;
      z-index: 90;
      width: min(28rem, calc(100vw - 1.5rem));
      max-height: calc(100vh - 2rem);
      overflow-y: auto;
      transform: translateX(-50%);
      border-radius: 1rem;
      border: 1px solid rgba(129, 140, 248, 0.65);
      background: rgba(8, 15, 30, 0.97);
      box-shadow:
        0 0 0 1px rgba(99, 102, 241, 0.2),
        0 22px 48px rgba(2, 6, 23, 0.65);
      padding: 1rem 1.15rem 1.05rem;
      color: #e2e8f0;
    }
    .presentation-tour-panel-top {
      top: 1.1rem;
      bottom: auto;
    }
    .presentation-tour-panel-left,
    .presentation-tour-panel-right {
      top: 50%;
      bottom: auto;
      left: 1rem;
      right: auto;
      width: min(22rem, calc(100vw - 2rem));
      transform: translateY(-50%);
    }
    .presentation-tour-panel-right {
      left: auto;
      right: 1rem;
    }
    .presentation-tour-meta {
      display: flex;
      justify-content: space-between;
      align-items: center;
      gap: 0.75rem;
      margin-bottom: 0.65rem;
    }
    .presentation-tour-kicker {
      font-size: 0.68rem;
      font-weight: 700;
      letter-spacing: 0.1em;
      text-transform: uppercase;
      color: #a5b4fc;
    }
    .presentation-tour-count {
      font-family: 'IBM Plex Mono', ui-monospace, monospace;
      font-size: 0.72rem;
      color: #94a3b8;
      white-space: nowrap;
    }
    .presentation-tour-looking {
      display: flex;
      align-items: center;
      flex-wrap: wrap;
      gap: 0.35rem 0.45rem;
      margin: 0 0 0.7rem;
      padding: 0.45rem 0.6rem;
      border-radius: 0.55rem;
      border: 1px solid rgba(251, 191, 36, 0.45);
      background: rgba(120, 53, 15, 0.35);
      font-size: 0.78rem;
      line-height: 1.35;
      color: #fde68a;
    }
    .presentation-tour-looking strong {
      color: #fffbeb;
      font-weight: 700;
    }
    .presentation-tour-looking-dot {
      width: 0.55rem;
      height: 0.55rem;
      border-radius: 9999px;
      background: #fbbf24;
      box-shadow: 0 0 0 0 rgba(251, 191, 36, 0.7);
      animation: tour-dot-pulse 1.4s ease-out infinite;
      flex-shrink: 0;
    }
    @keyframes tour-dot-pulse {
      0% {
        box-shadow: 0 0 0 0 rgba(251, 191, 36, 0.55);
      }
      70% {
        box-shadow: 0 0 0 8px rgba(251, 191, 36, 0);
      }
      100% {
        box-shadow: 0 0 0 0 rgba(251, 191, 36, 0);
      }
    }
    .presentation-tour-title {
      margin: 0 0 0.65rem;
      font-size: 1.05rem;
      font-weight: 700;
      line-height: 1.3;
      color: #f8fafc;
    }
    .presentation-tour-lines {
      margin: 0;
      padding: 0;
      list-style: none;
      display: flex;
      flex-direction: column;
      gap: 0.55rem;
    }
    .presentation-tour-lines li {
      position: relative;
      padding-left: 0.95rem;
      font-size: 0.86rem;
      line-height: 1.5;
      color: #dbe4f0;
    }
    .presentation-tour-lines li::before {
      content: '';
      position: absolute;
      left: 0;
      top: 0.55em;
      width: 0.35rem;
      height: 0.35rem;
      border-radius: 9999px;
      background: #818cf8;
    }
    .presentation-tour-sim {
      margin: 0.85rem 0 0;
      padding: 0.4rem 0.55rem;
      border-radius: 0.45rem;
      background: rgba(251, 191, 36, 0.12);
      border: 1px dashed rgba(251, 191, 36, 0.4);
      font-size: 0.75rem;
      font-weight: 600;
      line-height: 1.4;
      color: #fcd34d;
    }
    .presentation-tour-actions {
      display: flex;
      justify-content: flex-end;
      align-items: center;
      gap: 0.45rem;
      margin-top: 1rem;
      padding-top: 0.85rem;
      border-top: 1px solid rgba(51, 65, 85, 0.85);
    }
    .presentation-tour-btn {
      border-radius: 0.5rem;
      border: 1px solid rgba(100, 116, 139, 0.7);
      background: rgba(30, 41, 59, 0.9);
      color: #e2e8f0;
      font-size: 0.8rem;
      font-weight: 650;
      padding: 0.45rem 0.85rem;
      cursor: pointer;
    }
    .presentation-tour-btn:disabled {
      opacity: 0.4;
      cursor: not-allowed;
    }
    .presentation-tour-btn-ghost {
      margin-right: auto;
      border-color: transparent;
      background: transparent;
      color: #94a3b8;
    }
    .presentation-tour-btn-primary {
      border-color: rgba(163, 230, 53, 0.55);
      background: rgba(163, 230, 53, 0.95);
      color: #0f172a;
    }
  `,
})
export class PresentationTourPanelComponent implements OnDestroy {
  readonly tour = inject(PresentationTourStore);
  readonly spotlight = signal<SpotlightBox | null>(null);

  private generation = 0;
  private findTimer?: ReturnType<typeof setTimeout>;
  private activeEl: HTMLElement | null = null;
  private readonly onViewportChange = () => this.refreshSpotlightBox();

  constructor() {
    effect(() => {
      const active = this.tour.active();
      const step = this.tour.step();
      const placement = this.tour.panelPlacement();
      if (typeof document === 'undefined') return;

      document.body.classList.toggle('presentation-tour-active', active);
      this.clearTargetClass();
      this.clearFindTimer();
      this.spotlight.set(null);

      if (!active || !step?.target) {
        this.detachViewportListeners();
        return;
      }

      const gen = ++this.generation;
      this.scheduleFind(step.target, step.spotlightLabel || step.title, placement, gen, 0);
    });
  }

  ngOnDestroy(): void {
    this.clearFindTimer();
    this.clearTargetClass();
    this.detachViewportListeners();
    if (typeof document !== 'undefined') {
      document.body.classList.remove('presentation-tour-active');
    }
  }

  @HostListener('document:keydown', ['$event'])
  onKey(ev: KeyboardEvent): void {
    if (!this.tour.active()) return;
    if (ev.key === 'Escape') {
      ev.preventDefault();
      this.tour.exit();
      return;
    }
    if (ev.key === 'ArrowRight' || ev.key === 'Enter') {
      ev.preventDefault();
      if (this.tour.isLastStep()) {
        this.tour.exit();
      } else {
        this.tour.next();
      }
      return;
    }
    if (ev.key === 'ArrowLeft') {
      ev.preventDefault();
      this.tour.back();
    }
  }

  private scheduleFind(
    selector: string,
    label: string,
    placement: 'top' | 'bottom' | 'left' | 'right',
    gen: number,
    attempt: number
  ): void {
    this.findTimer = setTimeout(() => {
      if (gen !== this.generation) return;
      const el = document.querySelector(selector) as HTMLElement | null;
      if (!el) {
        if (attempt + 1 < FIND_RETRIES) {
          this.scheduleFind(selector, label, placement, gen, attempt + 1);
        }
        return;
      }
      this.clearTargetClass();
      this.activeEl = el;
      el.classList.add('tour-target-active');
      el.scrollIntoView({
        behavior: 'smooth',
        block: placement === 'top' ? 'end' : 'center',
        inline: placement === 'left' || placement === 'right' ? 'nearest' : 'nearest',
      });
      this.attachViewportListeners();
      // Recalcula após o scroll começar a acomodar o card.
      requestAnimationFrame(() => {
        if (gen !== this.generation) return;
        this.refreshSpotlightBox(label);
        setTimeout(() => {
          if (gen !== this.generation) return;
          this.refreshSpotlightBox(label);
        }, 280);
      });
    }, attempt === 0 ? 220 : FIND_DELAY_MS);
  }

  private refreshSpotlightBox(labelOverride?: string): void {
    const el = this.activeEl;
    if (!el || typeof document === 'undefined') {
      this.spotlight.set(null);
      return;
    }
    const rect = el.getBoundingClientRect();
    if (rect.width < 2 && rect.height < 2) {
      this.spotlight.set(null);
      return;
    }
    const prev = this.spotlight();
    this.spotlight.set({
      top: Math.max(0, rect.top - SPOTLIGHT_PAD),
      left: Math.max(0, rect.left - SPOTLIGHT_PAD),
      width: rect.width + SPOTLIGHT_PAD * 2,
      height: rect.height + SPOTLIGHT_PAD * 2,
      label:
        labelOverride ||
        prev?.label ||
        this.tour.step()?.spotlightLabel ||
        this.tour.step()?.title ||
        '',
      labelBelow: rect.top < 48,
    });
  }

  private clearTargetClass(): void {
    if (typeof document === 'undefined') return;
    document.querySelectorAll('.tour-target-active').forEach((node) => {
      node.classList.remove('tour-target-active');
      node.removeAttribute('data-tour-label');
    });
    this.activeEl = null;
  }

  private clearFindTimer(): void {
    if (this.findTimer) {
      clearTimeout(this.findTimer);
      this.findTimer = undefined;
    }
  }

  private attachViewportListeners(): void {
    if (typeof window === 'undefined') return;
    window.addEventListener('resize', this.onViewportChange);
    window.addEventListener('scroll', this.onViewportChange, true);
  }

  private detachViewportListeners(): void {
    if (typeof window === 'undefined') return;
    window.removeEventListener('resize', this.onViewportChange);
    window.removeEventListener('scroll', this.onViewportChange, true);
  }
}
