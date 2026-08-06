import {
  ChangeDetectionStrategy,
  Component,
  HostListener,
  effect,
  inject,
  signal,
} from '@angular/core';
import { ConfirmDialogService } from './confirm-dialog.service';

/**
 * Modal de confirmação reutilizável — só troque título/mensagem via ConfirmDialogService.ask().
 * Montar uma vez no shell da app (ex.: app.component).
 */
@Component({
  selector: 'lib-confirm-dialog',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    @if (dialog.active(); as d) {
      <div
        class="confirm-dialog-backdrop"
        role="presentation"
        (click)="onBackdrop()"
      >
        <div
          class="confirm-dialog-panel"
          role="alertdialog"
          aria-modal="true"
          [attr.aria-labelledby]="titleId"
          [attr.aria-describedby]="bodyId"
          (click)="$event.stopPropagation()"
        >
          <header class="confirm-dialog-head">
            <h2 [id]="titleId" class="confirm-dialog-title">{{ d.title }}</h2>
          </header>
          <p [id]="bodyId" class="confirm-dialog-message confirm-dialog-message-plain">
            {{ d.message }}
          </p>
          @if (d.detail) {
            @if (d.detailLabel) {
              <p class="confirm-dialog-detail-label">{{ d.detailLabel }}</p>
            }
            <pre class="confirm-dialog-detail">{{ d.detail }}</pre>
          }
          <footer class="confirm-dialog-actions">
            @if (d.detail || d.copyText) {
              <button
                type="button"
                class="confirm-dialog-btn confirm-dialog-btn-copy"
                [attr.aria-live]="'polite'"
                (click)="copyDetail(d.copyText || buildCopyFallback(d.message, d.detail))"
              >
                {{ copied() ? 'Copiado!' : 'Copiar texto' }}
              </button>
            }
            @if (d.mode !== 'info') {
              <button
                type="button"
                class="confirm-dialog-btn confirm-dialog-btn-cancel"
                (click)="cancel()"
              >
                {{ d.cancelLabel }}
              </button>
            }
            <button
              type="button"
              class="confirm-dialog-btn"
              [class.confirm-dialog-btn-primary]="d.tone === 'primary'"
              [class.confirm-dialog-btn-danger]="d.tone === 'danger'"
              [class.confirm-dialog-btn-neutral]="d.tone === 'neutral'"
              (click)="confirm()"
            >
              {{ d.confirmLabel }}
            </button>
          </footer>
        </div>
      </div>
    }
  `,
  styles: [
    `
      .confirm-dialog-backdrop {
        position: fixed;
        inset: 0;
        z-index: 80;
        display: flex;
        align-items: center;
        justify-content: center;
        padding: 1rem;
        background: rgba(2, 6, 23, 0.72);
        backdrop-filter: blur(4px);
        animation: confirm-fade-in 0.18s ease-out;
      }

      .confirm-dialog-panel {
        width: min(26rem, 100%);
        border-radius: 0.85rem;
        border: 1px solid rgba(99, 102, 241, 0.35);
        background: linear-gradient(180deg, #0f1c33 0%, #0a1222 100%);
        box-shadow:
          0 24px 48px rgba(0, 0, 0, 0.45),
          0 0 0 1px rgba(15, 23, 42, 0.8);
        padding: 1.15rem 1.2rem 1rem;
        animation: confirm-pop-in 0.2s ease-out;
      }

      .confirm-dialog-head {
        margin-bottom: 0.55rem;
      }

      .confirm-dialog-title {
        margin: 0;
        font-size: 1rem;
        font-weight: 700;
        letter-spacing: 0.01em;
        color: #f8fafc;
      }

      .confirm-dialog-message {
        margin: 0;
        font-size: 0.875rem;
        line-height: 1.45;
        color: #94a3b8;
        white-space: pre-wrap;
      }

      .confirm-dialog-message-plain {
        color: #cbd5e1;
      }

      .confirm-dialog-detail-label {
        margin: 0.85rem 0 0.35rem;
        font-size: 0.68rem;
        font-weight: 700;
        letter-spacing: 0.08em;
        text-transform: uppercase;
        color: #64748b;
      }

      .confirm-dialog-detail {
        margin: 0.75rem 0 0;
        max-height: 14rem;
        overflow: auto;
        border-radius: 0.5rem;
        border: 1px solid rgba(71, 85, 105, 0.7);
        background: rgba(2, 6, 23, 0.75);
        padding: 0.65rem 0.75rem;
        font-family: 'IBM Plex Mono', ui-monospace, monospace;
        font-size: 0.72rem;
        line-height: 1.4;
        color: #fecdd3;
        white-space: pre-wrap;
        word-break: break-word;
        user-select: text;
      }

      .confirm-dialog-panel:has(.confirm-dialog-detail) {
        width: min(34rem, 100%);
      }

      .confirm-dialog-actions {
        display: flex;
        justify-content: flex-end;
        flex-wrap: wrap;
        gap: 0.5rem;
        margin-top: 1.15rem;
      }

      .confirm-dialog-btn {
        border-radius: 0.5rem;
        border: 1px solid transparent;
        font-size: 0.8125rem;
        font-weight: 600;
        padding: 0.5rem 0.95rem;
        cursor: pointer;
        transition:
          background 0.15s ease,
          border-color 0.15s ease,
          transform 0.15s ease;
      }

      .confirm-dialog-btn:hover {
        transform: translateY(-1px);
      }

      .confirm-dialog-btn:focus-visible {
        outline: 2px solid rgba(165, 180, 252, 0.9);
        outline-offset: 2px;
      }

      .confirm-dialog-btn-copy {
        margin-right: auto;
        background: rgba(30, 41, 59, 0.9);
        border-color: rgba(148, 163, 184, 0.55);
        color: #e2e8f0;
      }

      .confirm-dialog-btn-copy:hover {
        background: rgba(51, 65, 85, 0.95);
        border-color: rgba(165, 180, 252, 0.7);
      }

      .confirm-dialog-btn-cancel {
        background: rgba(30, 41, 59, 0.9);
        border-color: rgba(71, 85, 105, 0.8);
        color: #cbd5e1;
      }

      .confirm-dialog-btn-cancel:hover {
        background: rgba(51, 65, 85, 0.95);
      }

      .confirm-dialog-btn-primary {
        background: #84cc16;
        color: #0f172a;
        box-shadow: 0 6px 16px rgba(132, 204, 22, 0.25);
      }

      .confirm-dialog-btn-primary:hover {
        background: #a3e635;
      }

      .confirm-dialog-btn-danger {
        background: #e11d48;
        color: #fff1f2;
        box-shadow: 0 6px 16px rgba(225, 29, 72, 0.28);
      }

      .confirm-dialog-btn-danger:hover {
        background: #f43f5e;
      }

      .confirm-dialog-btn-neutral {
        background: #3b82f6;
        color: #eff6ff;
      }

      .confirm-dialog-btn-neutral:hover {
        background: #60a5fa;
      }

      @keyframes confirm-fade-in {
        from {
          opacity: 0;
        }
        to {
          opacity: 1;
        }
      }

      @keyframes confirm-pop-in {
        from {
          opacity: 0;
          transform: translateY(8px) scale(0.98);
        }
        to {
          opacity: 1;
          transform: translateY(0) scale(1);
        }
      }

      @media (prefers-reduced-motion: reduce) {
        .confirm-dialog-backdrop,
        .confirm-dialog-panel {
          animation: none;
        }
      }
    `,
  ],
})
export class ConfirmDialogComponent {
  readonly dialog = inject(ConfirmDialogService);
  readonly titleId = 'confirm-dialog-title';
  readonly bodyId = 'confirm-dialog-body';
  readonly copied = signal(false);
  private copyResetTimer: ReturnType<typeof setTimeout> | null = null;

  constructor() {
    effect(() => {
      const open = !!this.dialog.active();
      document.body.style.overflow = open ? 'hidden' : '';
      if (!open) {
        this.resetCopied();
      }
    });
  }

  @HostListener('document:keydown.escape')
  onEscape(): void {
    if (this.dialog.active()) {
      this.cancel();
    }
  }

  onBackdrop(): void {
    this.cancel();
  }

  cancel(): void {
    this.dialog.close(false);
  }

  confirm(): void {
    this.dialog.close(true);
  }

  async copyDetail(text: string): Promise<void> {
    const value = (text || '').trim();
    if (!value) return;
    try {
      await navigator.clipboard.writeText(value);
    } catch {
      // Fallback para ambientes sem Clipboard API / permissão.
      const ta = document.createElement('textarea');
      ta.value = value;
      ta.setAttribute('readonly', '');
      ta.style.position = 'fixed';
      ta.style.left = '-9999px';
      document.body.appendChild(ta);
      ta.select();
      document.execCommand('copy');
      document.body.removeChild(ta);
    }
    this.copied.set(true);
    if (this.copyResetTimer) clearTimeout(this.copyResetTimer);
    this.copyResetTimer = setTimeout(() => this.resetCopied(), 2000);
  }

  buildCopyFallback(message?: string, detail?: string): string {
    const parts = [(message || '').trim(), (detail || '').trim()].filter(Boolean);
    return parts.join('\n\n');
  }

  private resetCopied(): void {
    this.copied.set(false);
    if (this.copyResetTimer) {
      clearTimeout(this.copyResetTimer);
      this.copyResetTimer = null;
    }
  }
}
