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
          <span class="presentation-tour-kicker">Apresentação</span>
          <span class="presentation-tour-count">{{ tour.stepLabel() }}</span>
        </div>
        @if (tour.step(); as step) {
          <h3 class="presentation-tour-title">{{ step.title }}</h3>
          <p class="presentation-tour-body">{{ step.body }}</p>
          @if (tour.isSimulating()) {
            <p class="presentation-tour-sim">Apresentação · dados simulados</p>
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
      bottom: 1rem;
      z-index: 80;
      width: min(36rem, calc(100vw - 1.5rem));
      transform: translateX(-50%);
      border-radius: 0.85rem;
      border: 1px solid rgba(99, 102, 241, 0.55);
      background: rgba(15, 23, 42, 0.96);
      box-shadow: 0 18px 40px rgba(2, 6, 23, 0.55);
      padding: 0.85rem 1rem 0.9rem;
      color: #e2e8f0;
    }
    .presentation-tour-meta {
      display: flex;
      justify-content: space-between;
      align-items: center;
      gap: 0.5rem;
      margin-bottom: 0.35rem;
    }
    .presentation-tour-kicker {
      font-size: 0.65rem;
      font-weight: 700;
      letter-spacing: 0.12em;
      text-transform: uppercase;
      color: #a5b4fc;
    }
    .presentation-tour-count {
      font-family: 'IBM Plex Mono', ui-monospace, monospace;
      font-size: 0.7rem;
      color: #94a3b8;
    }
    .presentation-tour-title {
      margin: 0;
      font-size: 0.95rem;
      font-weight: 650;
      color: #f8fafc;
    }
    .presentation-tour-body {
      margin: 0.35rem 0 0;
      font-size: 0.8rem;
      line-height: 1.4;
      color: #cbd5e1;
    }
    .presentation-tour-sim {
      margin: 0.45rem 0 0;
      font-size: 0.7rem;
      font-weight: 600;
      color: #fbbf24;
    }
    .presentation-tour-actions {
      display: flex;
      justify-content: flex-end;
      gap: 0.4rem;
      margin-top: 0.75rem;
    }
    .presentation-tour-btn {
      border-radius: 0.45rem;
      border: 1px solid rgba(100, 116, 139, 0.7);
      background: rgba(30, 41, 59, 0.9);
      color: #e2e8f0;
      font-size: 0.75rem;
      font-weight: 600;
      padding: 0.35rem 0.7rem;
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
      background: rgba(163, 230, 53, 0.92);
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
      document
        .querySelectorAll('.tour-target-active')
        .forEach((el) => el.classList.remove('tour-target-active'));

      if (!active || !step?.target) return;
      setTimeout(() => {
        const el = document.querySelector(step.target!);
        el?.classList.add('tour-target-active');
      }, 80);
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
