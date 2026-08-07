import {
  ChangeDetectionStrategy,
  Component,
  HostListener,
  effect,
  inject,
} from '@angular/core';
import { PresentationTourStore } from './presentation-tour.store';

@Component({
  selector: 'lib-presentation-tour-panel',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    @if (tour.active()) {
      <div class="presentation-tour-panel" role="dialog" aria-label="Apresentação">
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
            [disabled]="!tour.canBack()"
            (click)="tour.back()"
          >
            Voltar
          </button>
          <button
            type="button"
            class="presentation-tour-btn presentation-tour-btn-primary"
            [disabled]="!tour.canNext()"
            (click)="tour.next()"
          >
            Avançar
          </button>
        </div>
      </div>
    }
  `,
  styles: `
    .presentation-tour-panel {
      position: fixed;
      left: 50%;
      bottom: 1.1rem;
      z-index: 90;
      width: min(28rem, calc(100vw - 1.5rem));
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
export class PresentationTourPanelComponent {
  readonly tour = inject(PresentationTourStore);

  constructor() {
    effect(() => {
      const active = this.tour.active();
      const step = this.tour.step();
      if (typeof document === 'undefined') return;

      document.body.classList.toggle('presentation-tour-active', active);
      document.querySelectorAll('.tour-target-active').forEach((el) => {
        el.classList.remove('tour-target-active');
        el.removeAttribute('data-tour-label');
      });

      if (!active || !step?.target) return;
      setTimeout(() => {
        const el = document.querySelector(step.target!) as HTMLElement | null;
        if (!el) return;
        el.classList.add('tour-target-active');
        el.setAttribute(
          'data-tour-label',
          step.spotlightLabel || step.title
        );
        el.scrollIntoView({
          behavior: 'smooth',
          block: 'nearest',
          inline: 'nearest',
        });
      }, 220);
    });
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
      this.tour.next();
      return;
    }
    if (ev.key === 'ArrowLeft') {
      ev.preventDefault();
      this.tour.back();
    }
  }
}
