declare global {
  interface Window {
    /** Injetado no deploy (index.html) ou definido via /config.json */
    __CTE_ORQ_API_BASE__?: string;
  }
}

const DEV_FALLBACK = 'http://localhost:5000';

let resolvedBase = DEV_FALLBACK;

export function getApiBaseUrl(): string {
  if (typeof window !== 'undefined' && window.__CTE_ORQ_API_BASE__?.trim()) {
    return window.__CTE_ORQ_API_BASE__.trim().replace(/\/$/, '');
  }

  return resolvedBase.replace(/\/$/, '');
}

/** @deprecated Prefer getApiBaseUrl() — valor pode mudar após loadRuntimeApiConfig. */
export const API_BASE_URL = DEV_FALLBACK;

/**
 * Carrega /config.json (gerado no deploy) e define window.__CTE_ORQ_API_BASE__.
 * Deve rodar antes do bootstrap Angular.
 */
export async function loadRuntimeApiConfig(): Promise<string> {
  if (typeof window === 'undefined') {
    resolvedBase = DEV_FALLBACK;
    return resolvedBase;
  }

  if (window.__CTE_ORQ_API_BASE__?.trim()) {
    resolvedBase = window.__CTE_ORQ_API_BASE__.trim().replace(/\/$/, '');
    return resolvedBase;
  }

  try {
    const res = await fetch('/config.json', { cache: 'no-store' });
    if (res.ok) {
      const json = (await res.json()) as { apiBaseUrl?: string };
      if (json.apiBaseUrl?.trim()) {
        resolvedBase = json.apiBaseUrl.trim().replace(/\/$/, '');
        window.__CTE_ORQ_API_BASE__ = resolvedBase;
        return resolvedBase;
      }
    }
  } catch {
    // fallback DEV
  }

  resolvedBase = DEV_FALLBACK;
  window.__CTE_ORQ_API_BASE__ = resolvedBase;
  return resolvedBase;
}
