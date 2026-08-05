import { getApiBaseUrl } from '@orquestrador/monitor-core';

/**
 * Wave 4 (SDD Monitor Unificado): Resgate absorvido no Orquestrador.Api — não roda mais em
 * processo/porta próprios (antes :5070). Usa a mesma base do Orquestrador (padrão :5000).
 */
export const RESGATE_API_BASE =
  (typeof window !== 'undefined' &&
    (window as unknown as { __CTE_RESGATE_API_BASE__?: string })
      .__CTE_RESGATE_API_BASE__) ||
  getApiBaseUrl();

const TOKEN_KEY = 'resgate.jwt';
const MODE_KEY = 'resgate.panelMode';

export type PanelMode = 'online' | 'offline';

export type EnfileirarResult = {
  modo: string;
  enfileirados: number;
  pendentesTemp: number;
  profundidadeFilaBroker?: number;
  idadeMaxTempMinutos?: number | null;
  ids: number[];
  aviso?: string;
  mensagem: string;
  riscoFila?: string;
  checklistCarga?: {
    executar: string;
    executarAuto: string;
    codServico: number;
    monitor: string;
  };
};

export type FilaDownloadStatus = {
  modo: string;
  aviso?: string;
  mensagem?: string;
  pendentesTemp: number;
  profundidadeFilaBroker: number;
  idadeMaxTempMinutos?: number | null;
  riscoConcorrencia?: string | null;
  consumidoresFila?: string[];
  itens: Array<{
    id: number;
    chaveMascarada: string;
    status: string;
    erro?: string | null;
    atualizadoEm?: string | null;
  }>;
};

export type StatusChavesResult = {
  modo: string;
  aviso?: string;
  total: number;
  itens: Array<{ chaveMascarada: string; status: string; detalhe?: string | null }>;
};

/** Sessão inválida/expirada: a tela deve voltar ao login, não exibir como falha de envio. */
export class ResgateUnauthorizedError extends Error {
  constructor(message = 'Sessão expirada — entre novamente (dev / dev).') {
    super(message);
    this.name = 'ResgateUnauthorizedError';
  }
}

function readExpiryMs(token: string): number | null {
  const payload = token.split('.')[1];
  if (!payload) return null;
  try {
    const claims = JSON.parse(
      atob(payload.replace(/-/g, '+').replace(/_/g, '/'))
    ) as { exp?: number };
    return typeof claims.exp === 'number' ? claims.exp * 1000 : null;
  } catch {
    return null;
  }
}

export function getToken(): string | null {
  const token = localStorage.getItem(TOKEN_KEY);
  if (!token) return null;

  const expiresAt = readExpiryMs(token);
  if (expiresAt !== null && expiresAt <= Date.now()) {
    clearToken();
    return null;
  }
  return token;
}

export function setToken(token: string): void {
  localStorage.setItem(TOKEN_KEY, token);
}

export function clearToken(): void {
  localStorage.removeItem(TOKEN_KEY);
}

export function getPanelMode(): PanelMode {
  return localStorage.getItem(MODE_KEY) === 'offline' ? 'offline' : 'online';
}

export function setPanelMode(mode: PanelMode): void {
  localStorage.setItem(MODE_KEY, mode);
}

async function readApiError(res: Response, fallback: string): Promise<string> {
  const text = await res.text().catch(() => '');
  let body: Record<string, unknown> = {};
  if (text) {
    try {
      body = JSON.parse(text) as Record<string, unknown>;
    } catch {
      const clipped = text.replace(/\s+/g, ' ').trim().slice(0, 180);
      return `${fallback} (HTTP ${res.status})${clipped ? `: ${clipped}` : ''}`;
    }
  }

  const errors = body['errors'];
  if (Array.isArray(errors) && errors.length) {
    const joined = errors.map(String).filter(Boolean).join('; ');
    const hint = typeof body['hint'] === 'string' ? ` — ${body['hint']}` : '';
    const detail = typeof body['detail'] === 'string' ? ` [${body['detail']}]` : '';
    return `${joined}${detail}${hint}`;
  }

  const error = typeof body['error'] === 'string' ? body['error'] : null;
  const title = typeof body['title'] === 'string' ? body['title'] : null;
  const detail = typeof body['detail'] === 'string' ? body['detail'] : null;
  const hint = typeof body['hint'] === 'string' ? body['hint'] : null;
  const parts = [error || title || fallback, detail, hint].filter(Boolean);
  return `${parts.join(' — ')} (HTTP ${res.status})`;
}

export class ResgateApiClient {
  async login(usuario: string, senha: string): Promise<void> {
    let res: Response;
    try {
      res = await fetch(`${RESGATE_API_BASE}/api/auth/token`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ usuario, senha }),
      });
    } catch {
      throw new Error(
        `API Orquestrador offline (${RESGATE_API_BASE}). Suba: cd 0-Orquestrador/Orquestrador.Api/src/Orquestrador.Api && dotnet run`
      );
    }
    if (!res.ok) {
      throw new Error(await readApiError(res, 'Usuário ou senha inválidos'));
    }
    const json = (await res.json()) as { token: string };
    setToken(json.token);
  }

  async enfileirarDownload(chaves: string[]): Promise<EnfileirarResult> {
    let res: Response;
    try {
      res = await this.authFetch('/api/resgate/lotes', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ chaves }),
      });
    } catch (e) {
      if (e instanceof ResgateUnauthorizedError) throw e;
      throw new Error(
        `Sem conexão com a API Orquestrador (${RESGATE_API_BASE}). Confira se está no ar.`
      );
    }
    if (!res.ok) {
      throw new Error(await readApiError(res, 'Erro ao enfileirar no Download'));
    }
    return (await res.json()) as EnfileirarResult;
  }

  async uploadEnfileirar(file: File): Promise<EnfileirarResult> {
    const fd = new FormData();
    fd.append('file', file);
    let res: Response;
    try {
      res = await this.authFetch('/api/resgate/lotes/upload', {
        method: 'POST',
        body: fd,
      });
    } catch (e) {
      if (e instanceof ResgateUnauthorizedError) throw e;
      throw new Error(
        `Sem conexão com a API Orquestrador (${RESGATE_API_BASE}). Confira se está no ar.`
      );
    }
    if (!res.ok) {
      throw new Error(await readApiError(res, 'Erro no upload'));
    }
    return (await res.json()) as EnfileirarResult;
  }

  async filaDownload(): Promise<FilaDownloadStatus> {
    const res = await this.authFetch('/api/resgate/fila-download');
    if (!res.ok) throw new Error(await readApiError(res, 'Falha ao consultar fila'));
    return (await res.json()) as FilaDownloadStatus;
  }

  async statusChaves(chaves: string[]): Promise<StatusChavesResult> {
    const res = await this.authFetch('/api/resgate/status-chaves', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ chaves }),
    });
    if (!res.ok) throw new Error(await readApiError(res, 'Falha ao consultar status'));
    return (await res.json()) as StatusChavesResult;
  }

  /** @deprecated painel de lote próprio removido */
  async aoVivo(_id: number): Promise<FilaDownloadStatus> {
    return this.filaDownload();
  }

  /** @deprecated */
  async relatorio(_id: number): Promise<FilaDownloadStatus> {
    return this.filaDownload();
  }

  private async authFetch(path: string, init: RequestInit = {}): Promise<Response> {
    const token = getToken();
    if (!token) throw new ResgateUnauthorizedError();

    const headers = new Headers(init.headers);
    headers.set('Authorization', `Bearer ${token}`);
    const res = await fetch(`${RESGATE_API_BASE}${path}`, { ...init, headers });

    if (res.status === 401) {
      clearToken();
      throw new ResgateUnauthorizedError();
    }
    return res;
  }
}

export const resgateApi = new ResgateApiClient();
