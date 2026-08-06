export type ConfirmDialogTone = 'primary' | 'danger' | 'neutral';

export type ConfirmDialogMode = 'confirm' | 'info';

export interface ConfirmDialogOptions {
  /** Título curto do aviso. */
  title: string;
  /** Corpo explicativo — pode mudar por uso. */
  message: string;
  /** Bloco monoespaçado (ex.: erro original) abaixo da mensagem. Quando presente, o modal oferece botão Copiar texto. */
  detail?: string;
  /** Rótulo acima do bloco detail (ex.: "Texto original"). */
  detailLabel?: string;
  /** Texto completo para a área de transferência (senão: message + detail). */
  copyText?: string;
  /** confirm = OK/Cancelar; info = só Fechar. */
  mode?: ConfirmDialogMode;
  /** Rótulo do botão de confirmação. */
  confirmLabel?: string;
  /** Rótulo do botão de cancelar (ignorado em mode=info). */
  cancelLabel?: string;
  /** Cor do botão principal. */
  tone?: ConfirmDialogTone;
}

export interface ConfirmDialogState extends ConfirmDialogOptions {
  resolve: (value: boolean) => void;
}
