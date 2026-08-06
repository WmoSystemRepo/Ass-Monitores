import { Injectable, signal } from '@angular/core';
import type {
  ConfirmDialogOptions,
  ConfirmDialogState,
} from './confirm-dialog.models';

/**
 * Abre o modal de confirmação padronizado.
 * Uso: `const ok = await confirmDialog.ask({ title, message });`
 */
@Injectable({ providedIn: 'root' })
export class ConfirmDialogService {
  private readonly state = signal<ConfirmDialogState | null>(null);
  readonly active = this.state.asReadonly();

  ask(options: ConfirmDialogOptions): Promise<boolean> {
    // Fecha pedido anterior como cancelado (evita Promise pendente).
    const prev = this.state();
    if (prev) {
      prev.resolve(false);
    }

    return new Promise<boolean>((resolve) => {
      this.state.set({
        title: options.title,
        message: options.message,
        detail: options.detail,
        mode: options.mode ?? 'confirm',
        confirmLabel:
          options.confirmLabel ??
          (options.mode === 'info' ? 'Fechar' : 'Confirmar'),
        cancelLabel: options.cancelLabel ?? 'Cancelar',
        tone: options.tone ?? (options.mode === 'info' ? 'neutral' : 'primary'),
        resolve,
      });
    });
  }

  close(confirmed: boolean): void {
    const current = this.state();
    if (!current) return;
    this.state.set(null);
    current.resolve(confirmed);
  }
}
