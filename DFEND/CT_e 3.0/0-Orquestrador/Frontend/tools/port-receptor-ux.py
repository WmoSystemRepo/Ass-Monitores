#!/usr/bin/env python3
"""Port Receptor dashboard UX (hero status, cycle-bar, empty state) to other monitors."""
from __future__ import annotations

import re
from pathlib import Path

ROOT = Path(__file__).resolve().parents[1] / "libs" / "service-monitors" / "src" / "lib"

SERVICES = {
    "arquivador": {
        "title": "Arquivador CT-e",
        "question": "Está arquivando documentos agora?",
        "name": "Arquivador",
        "article": "o",
        "work_noun": "arquivamento",
        "work_gerund": "arquivar",
        "cycle_label": "Ciclo Arquivador",
        "wait_hint": "Arquivador ligado — aguardando próximo ciclo de arquivamento",
        "paused_msg": "Arquivamento pausado (Executar≠1). Ative para voltar a arquivar CT-e.",
        "empty_eyebrow": "Pipeline do Arquivador",
        "empty_stopped": "Arquivador parado",
        "empty_body": "Ligue para arquivar CT-e da fila e acompanhar o fluxo:\n                fila → temp → Sintetizador → Analisador → Integrador.",
        "empty_pipe": "Fila → Temp → Sintetizador → Analisador → Integrador",
        "empty_accent": "text-amber-300/90",
        "link_accent": "text-amber-300",
        "action_banner": (
            'border-amber-500/30 bg-amber-950/30 px-3 py-1 text-xs text-amber-100',
            'text-amber-300/80',
        ),
        "live_on": ("border-teal-500", "text-teal-400"),
        "live_off": ("border-amber-500", "text-amber-400"),
        "btn": "teal",
        "cycle_border": "border-teal-500/30 bg-teal-950/30",
        "cycle_label_color": "text-teal-400/80",
        "status_import": "arquivadorStatusLabel",
        "anatomy_tag": "lib-arquivador-anatomy-flow",
        "anatomy_class": "ArquivadorAnatomyFlowComponent",
        "stage_labels": {
            "fila": "fila",
            "temp": "temporária",
            "sintetizador": "Sintetizador",
            "analisador": "Analisador",
            "integrador": "Integrador",
        },
        "limited_work": "arquivamento sem telemetria completa",
        "found_caption_keep": True,
    },
    "sintetizador": {
        "title": "Sintetizador CT-e",
        "question": "Está sintetizando documentos agora?",
        "name": "Sintetizador",
        "article": "o",
        "work_noun": "síntese",
        "work_gerund": "sintetizar",
        "cycle_label": "Ciclo Sintetizador",
        "wait_hint": "Sintetizador ligado — aguardando próximo ciclo de síntese",
        "paused_msg": "Síntese pausada (Executar≠1). Ative para voltar a sintetizar CT-e.",
        "empty_eyebrow": "Pipeline do Sintetizador",
        "empty_stopped": "Sintetizador parado",
        "empty_body": "Ligue para sintetizar CT-e da fila e acompanhar o fluxo:\n                fila → temporária → classificar → persistir → limpar.",
        "empty_pipe": "Fila → Temporária → Classificar → Persistir → Limpar",
        "empty_accent": "text-violet-300/90",
        "link_accent": "text-violet-300",
        "action_banner": (
            "border-violet-500/30 bg-violet-950/30 px-3 py-1 text-xs text-violet-100",
            "text-violet-300/80",
        ),
        "live_on": ("border-teal-500", "text-teal-400"),
        "live_off": ("border-zinc-500", "text-zinc-400"),
        "btn": "teal",
        "cycle_border": "border-teal-500/30 bg-teal-950/30",
        "cycle_label_color": "text-teal-400/80",
        "status_import": "sintetizadorStatusLabel",
        "anatomy_tag": "lib-sintetizador-anatomy-flow",
        "anatomy_class": "SintetizadorAnatomyFlowComponent",
        "stage_labels": {
            "fila": "fila",
            "temp": "temporária",
            "classificar": "classificar",
            "persistir": "persistir",
            "limpar": "limpar",
        },
        "limited_work": "síntese sem telemetria completa",
        "found_caption_keep": True,
    },
    "analisador": {
        "title": "Analisador CT-e",
        "question": "Está analisando documentos agora?",
        "name": "Analisador",
        "article": "o",
        "work_noun": "análise",
        "work_gerund": "analisar",
        "cycle_label": "Ciclo Analisador",
        "wait_hint": "Analisador ligado — aguardando próximo ciclo de análise",
        "paused_msg": "Análise pausada (Executar≠1). Ative para voltar a analisar CT-e.",
        "empty_eyebrow": "Pipeline do Analisador",
        "empty_stopped": "Analisador parado",
        "empty_body": "Ligue para analisar CT-e da fila e acompanhar o fluxo:\n                fila → temporária → classificar → detalhar → limpar.",
        "empty_pipe": "Fila → Temporária → Classificar → Detalhar → Limpar",
        "empty_accent": "text-violet-300/90",
        "link_accent": "text-violet-300",
        "action_banner": (
            "border-violet-500/30 bg-violet-950/30 px-3 py-1 text-xs text-violet-100",
            "text-violet-300/80",
        ),
        "live_on": ("border-teal-500", "text-teal-400"),
        "live_off": ("border-zinc-500", "text-zinc-400"),
        "btn": "teal",
        "cycle_border": "border-teal-500/30 bg-teal-950/30",
        "cycle_label_color": "text-teal-400/80",
        "status_import": "analisadorStatusLabel",
        "anatomy_tag": "lib-analisador-anatomy-flow",
        "anatomy_class": "AnalisadorAnatomyFlowComponent",
        "stage_labels": {
            "fila": "fila",
            "temp": "temporária",
            "classificar": "classificar",
            "detalhar": "detalhar",
            "limpar": "limpar",
        },
        "limited_work": "análise sem telemetria completa",
        "found_caption_keep": True,
    },
    "integrador": {
        "title": "Integrador CT-e",
        "question": "Está integrando documentos agora?",
        "name": "Integrador",
        "article": "o",
        "work_noun": "integração",
        "work_gerund": "integrar",
        "cycle_label": "Ciclo Integrador",
        "wait_hint": "Integrador ligado — aguardando próximo ciclo de integração",
        "paused_msg": "Integração pausada (Executar≠1). Ative para voltar a integrar CT-e.",
        "empty_eyebrow": "Pipeline do Integrador",
        "empty_stopped": "Integrador parado",
        "empty_body": "Ligue para integrar CT-e da fila e acompanhar o fluxo:\n                fila → temporária → classificar → persistir → limpar.",
        "empty_pipe": "Fila → Temporária → Classificar → Persistir → Limpar",
        "empty_accent": "text-violet-300/90",
        "link_accent": "text-violet-300",
        "action_banner": (
            "border-violet-500/30 bg-violet-950/30 px-3 py-1 text-xs text-violet-100",
            "text-violet-300/80",
        ),
        "live_on": ("border-teal-500", "text-teal-400"),
        "live_off": ("border-zinc-500", "text-zinc-400"),
        "btn": "teal",
        "cycle_border": "border-teal-500/30 bg-teal-950/30",
        "cycle_label_color": "text-teal-400/80",
        "status_import": "integradorStatusLabel",
        "anatomy_tag": "lib-integrador-anatomy-flow",
        "anatomy_class": "IntegradorAnatomyFlowComponent",
        "stage_labels": {
            "fila": "fila",
            "temp": "temporária",
            "classificar": "classificar",
            "persistir": "persistir",
            "limpar": "limpar",
        },
        "limited_work": "integração sem telemetria completa",
        "found_caption_keep": True,
    },
    "carga": {
        "title": "Carga CT-e",
        "question": "Está baixando documentos agora?",
        "name": "Carga",
        "article": "a",
        "work_noun": "carga",
        "work_gerund": "consultar",
        "cycle_label": "Ciclo Carga",
        "wait_hint": "Carga ligada — aguardando próximo download pontual",
        "paused_msg": "Carga pausada (Executar≠1). Ative para voltar ao download pontual.",
        "empty_eyebrow": "Pipeline da Carga",
        "empty_stopped": "Carga parada",
        "empty_body": "Ligue para baixar CT-e pontuais e acompanhar o fluxo:\n                fila → chave → Consultar WS → persistir → limpar.",
        "empty_pipe": "Fila → Chave → Consultar WS → Persistir → Limpar",
        "empty_accent": "text-teal-300/90",
        "link_accent": "text-teal-300",
        "action_banner": (
            "border-teal-500/30 bg-teal-950/30 px-3 py-1 text-xs text-teal-100",
            "text-teal-300/80",
        ),
        "live_on": ("border-teal-500", "text-teal-400"),
        "live_off": ("border-zinc-500", "text-zinc-400"),
        "btn": "teal",
        "cycle_border": "border-teal-500/30 bg-teal-950/30",
        "cycle_label_color": "text-teal-400/80",
        "status_import": "CargaStatusLabel",
        "anatomy_tag": "lib-carga-anatomy-flow",
        "anatomy_class": "CargaAnatomyFlowComponent",
        "stage_labels": {
            "fila": "fila",
            "temp": "chave",
            "classificar": "Consultar WS",
            "persistir": "persistir",
            "limpar": "limpar",
        },
        "limited_work": "carga sem telemetria completa",
        "found_caption_keep": True,
    },
}


def build_template(svc: str, cfg: dict) -> str:
    btn = cfg["btn"]
    live_on_b, live_on_t = cfg["live_on"]
    live_off_b, live_off_t = cfg["live_off"]
    ab_cls, ab_detail = cfg["action_banner"]
    return f"""  template: `
    <section class="dashboard-fit flex h-[calc(100vh-3rem)] max-h-[calc(100vh-3rem)] flex-col gap-2 overflow-hidden">
      <header class="flex shrink-0 flex-wrap items-center justify-between gap-2">
        <div class="min-w-0 flex flex-wrap items-center gap-2.5">
          <div class="min-w-0">
            <h1 class="text-base font-semibold leading-tight text-zinc-50">
              {cfg["title"]}
            </h1>
            <p class="text-[11px] text-zinc-400">
              {cfg["question"]}
            </p>
          </div>
          @if (heroStatus(); as hero) {{
            <div
              class="status-hero"
              [class.status-hero-ok]="hero.tone === 'ok'"
              [class.status-hero-wait]="hero.tone === 'wait'"
              [class.status-hero-warn]="hero.tone === 'warn'"
              [class.status-hero-off]="hero.tone === 'off'"
              [attr.title]="hero.hint"
              role="status"
            >
              @if (hero.tone === 'ok') {{
                <span class="live-dot"></span>
              }}
              <span class="status-hero-label">{{{{ hero.label }}}}</span>
              @if (hero.detail) {{
                <span class="status-hero-detail">{{{{ hero.detail }}}}</span>
              }}
            </div>
          }}
        </div>
        <div class="flex flex-wrap items-center gap-1.5">
          <span
            class="inline-flex items-center gap-1.5 rounded border px-2 py-1 text-[11px]"
            [class.{live_on_b}]="store.live()"
            [class.{live_on_t}]="store.live()"
            [class.{live_off_b}]="!store.live()"
            [class.{live_off_t}]="!store.live()"
            [attr.title]="
              store.live()
                ? 'Monitor recebendo atualizações (SignalR)'
                : 'Monitor sem push recente'
            "
          >
            @if (store.live()) {{
              <span class="live-dot"></span>
            }}
            {{{{ connectionLabel() }}}}
            @if (store.lastPushAt(); as t) {{
              · {{{{ t | date: 'HH:mm:ss' }}}}
            }}
          </span>
          <button
            type="button"
            class="rounded bg-{btn}-600 px-2.5 py-1.5 text-xs font-medium text-white transition hover:bg-{btn}-500 disabled:opacity-40"
            [disabled]="store.actionBusy() || canStart() === false"
            (click)="store.startService()"
          >
            {{{{ primaryActionLabel() }}}}
          </button>
          <button
            type="button"
            class="rounded border border-rose-500/60 px-2.5 py-1.5 text-xs text-rose-300 transition hover:bg-rose-950/40 disabled:opacity-40"
            [disabled]="store.actionBusy() || !processUp()"
            (click)="confirmStop()"
          >
            Desligar
          </button>
        </div>
      </header>

      @if (store.bootError(); as err) {{
        <div class="shrink-0 rounded border border-rose-500/40 bg-rose-950/40 px-3 py-1 text-xs text-rose-200">
          Não foi possível falar com o monitor. {{{{ err }}}}
        </div>
      }}
      @if (actionBanner(); as banner) {{
        <div class="shrink-0 rounded border {ab_cls}">
          <span class="font-medium">{{{{ banner.title }}}}</span>
          @if (banner.detail) {{
            <span class="ml-1 font-mono text-[10px] {ab_detail}">{{{{ banner.detail }}}}</span>
          }}
        </div>
      }}

      @if (isLimitedTelemetry()) {{
        <div
          class="shrink-0 rounded border border-amber-500/40 bg-amber-950/35 px-3 py-1.5 text-[11px] text-amber-100"
          role="status"
        >
          Snapshot limitado — processo (DevHost) visível; filas/Executar ainda sem telemetria completa do banco.
          @if (processUp()) {{
            <span class="font-medium"> Processo no ar; {cfg["limited_work"]}.</span>
          }}
          <button
            type="button"
            class="ml-2 rounded border border-amber-400/50 px-2 py-0.5 text-[10px] font-medium text-amber-50 transition hover:bg-amber-900/50 disabled:opacity-40"
            [disabled]="store.actionBusy()"
            (click)="store.startService()"
          >
            Reiniciar {cfg["name"]}
          </button>
        </div>
      }}

      @if (isRunning()) {{
        <div
          class="cycle-bar pulse-banner flex shrink-0 flex-wrap items-center justify-between gap-x-3 gap-y-1.5 rounded-md border {cfg["cycle_border"]} px-3 py-1.5"
        >
          <div class="flex min-w-0 flex-wrap items-center gap-1.5">
            <span class="text-[10px] font-semibold uppercase tracking-wider {cfg["cycle_label_color"]}"
              >{cfg["cycle_label"]}</span
            >
            @if (cycleCountdown(); as clock) {{
              <div
                class="cycle-chrono shrink-0"
                [class.cycle-chrono-busy]="clock.mode === 'busy'"
                [class.cycle-chrono-zero]="clock.mode === 'zero'"
                [attr.title]="clock.hint"
              >
                <span class="cycle-chrono-label">{{{{ clock.caption }}}}</span>
                <span class="cycle-chrono-digits">{{{{ clock.display }}}}</span>
              </div>
            }}
            @if (fileWaitChrono(); as wait) {{
              <div
                class="cycle-chrono cycle-chrono-wait shrink-0"
                [class.cycle-chrono-found]="wait.mode === 'found'"
                [attr.title]="wait.hint"
              >
                <span class="cycle-chrono-label">{{{{ wait.caption }}}}</span>
                <span class="cycle-chrono-digits">{{{{ wait.display }}}}</span>
              </div>
            }}
          </div>
          <div
            class="flex min-w-0 flex-wrap items-center gap-x-3 gap-y-0.5 text-[11px] text-zinc-400"
            [attr.title]="
              heartbeat().stale
                ? 'Última batida no banco (dtc_execucao) antiga — SVC_STALE conhecido na POC'
                : 'Saúde auxiliar'
            "
          >
            <span>
              Banco
              <span class="font-medium text-zinc-200">{{{{ healthLabel() }}}}</span>
            </span>
            <span class="truncate">
              {{{{ service()?.nomServidor || '—' }}}}
              <span [class.text-amber-200]="heartbeat().stale">· {{{{ heartbeat().text }}}}</span>
            </span>
          </div>
        </div>
      }} @else if (processUp()) {{
        <div
          class="flex shrink-0 flex-wrap items-center justify-between gap-2 rounded-md border border-amber-500/35 bg-amber-950/25 px-3 py-1.5 text-[11px] text-amber-100"
        >
          <span>
            @if (isLimitedTelemetry()) {{
              Processo no ar — telemetria incompleta. Reinicie para tentar ler filas/Executar.
            }} @else {{
              {cfg["paused_msg"]}
            }}
          </span>
          <button
            type="button"
            class="rounded bg-{btn}-600 px-2.5 py-1 text-xs font-medium text-white transition hover:bg-{btn}-500 disabled:opacity-40"
            [disabled]="store.actionBusy()"
            (click)="store.startService()"
          >
            {{{{ primaryActionLabel() }}}}
          </button>
        </div>
      }}

      @if (store.tableHealth().length && isRunning()) {{
        <lib-table-health-cards class="block shrink-0" [items]="store.tableHealth()" />
      }}

      <div class="min-h-0 flex-1 overflow-hidden">
        @if (!processUp()) {{
          <div class="receptor-empty flex h-full flex-col items-center justify-center gap-4 rounded-xl border border-zinc-600/60 bg-zinc-950/40 px-6 text-center">
            <div class="max-w-md space-y-2">
              <p class="text-[10px] font-semibold uppercase tracking-[0.16em] {cfg["empty_accent"]}">
                {cfg["empty_eyebrow"]}
              </p>
              <h2 class="text-lg font-semibold text-zinc-50">{cfg["empty_stopped"]}</h2>
              <p class="text-sm text-zinc-400">
                {cfg["empty_body"]}
              </p>
              <p class="font-mono text-[11px] text-zinc-500">
                {cfg["empty_pipe"]}
              </p>
            </div>
            <div class="flex flex-wrap items-center justify-center gap-2">
              <button
                type="button"
                class="rounded bg-{btn}-600 px-4 py-2 text-sm font-medium text-white transition hover:bg-{btn}-500 disabled:opacity-40"
                [disabled]="store.actionBusy()"
                (click)="store.startService()"
              >
                {{{{ primaryActionLabel() }}}}
              </button>
              <a
                routerLink="/monitores/{svc}/mais-informacoes"
                class="rounded border border-zinc-600 px-3 py-2 text-xs font-medium {cfg["link_accent"]} hover:bg-zinc-800"
              >
                Mais informações →
              </a>
            </div>
          </div>
        }} @else {{
          <{cfg["anatomy_tag"]}
            class="block h-full"
            [running]="isRunning()"
            [activeStage]="visualStage()"
            [caption]="flowCaption()"
            [latest]="latestLote()"
            [packets]="flyingPackets()"
            [consuming]="queuesConsuming()"
          />
        }}
      </div>
    </section>
  `,
"""


def build_stage_short_label(cfg: dict) -> str:
    cases = "\n".join(
        f"      case '{k}':\n        return '{v}';" for k, v in cfg["stage_labels"].items()
    )
    return f"""
  private stageShortLabel(stage: AnatomyStage | null): string | null {{
    switch (stage) {{
{cases}
      default:
        return null;
    }}
  }}
"""


def build_hero_status(cfg: dict) -> str:
    return f"""
  /** Status único legível em 2s — substitui Processo/{cfg["work_noun"].capitalize()} fragmentados. */
  readonly heroStatus = computed(() => {{
    if (this.processUp() && this.isLimitedTelemetry() && !this.isRunning()) {{
      return {{
        tone: 'warn' as const,
        label: 'Telemetria limitada',
        detail: 'processo no ar',
        hint: 'DevHost sobe o processo, mas filas/Executar ainda não vêm do banco',
      }};
    }}
    if (this.isRunning()) {{
      if (this.visualStage()) {{
        return {{
          tone: 'ok' as const,
          label: 'Processando',
          detail: this.stageShortLabel(this.visualStage()),
          hint: this.flowCaption(),
        }};
      }}
      return {{
        tone: 'wait' as const,
        label: 'Aguardando ciclo',
        detail: null as string | null,
        hint: '{cfg["wait_hint"]}',
      }};
    }}
    if (this.processUp()) {{
      return {{
        tone: 'warn' as const,
        label: 'Pausado',
        detail: '{cfg["work_noun"]} off',
        hint: 'Processo no ar, mas Executar≠1 — ative o trabalho',
      }};
    }}
    return {{
      tone: 'off' as const,
      label: 'Parado',
      detail: null as string | null,
      hint: '{cfg["name"]} desligado — não processa novos CT-e',
    }};
  }});
"""


def patch_dashboard(svc: str, cfg: dict) -> None:
    path = ROOT / svc / "dashboard-page.component.ts"
    text = path.read_text(encoding="utf-8")

    # Imports
    if "RouterLink" not in text:
        text = text.replace(
            "import { DatePipe } from '@angular/common';\n",
            "import { DatePipe } from '@angular/common';\nimport { RouterLink } from '@angular/router';\n",
        )

    # Remove status label import
    si = cfg["status_import"]
    text = re.sub(rf",\s*{si}\s*", "", text)
    text = re.sub(rf"{si}\s*,\s*", "", text)

    # Fix imports array
    text = text.replace(
        f"imports: [DatePipe, {cfg['anatomy_class']}, TableHealthCardsComponent],",
        f"imports: [DatePipe, RouterLink, {cfg['anatomy_class']}, TableHealthCardsComponent],",
    )

    # Replace entire template
    text = re.sub(
        r"  template: `.*?  `,\n\}\)",
        build_template(svc, cfg) + "})",
        text,
        count=1,
        flags=re.S,
    )

    # Remove statusLabel + workLabel blocks
    text = re.sub(
        r"\n  readonly statusLabel = computed\(\(\) =>\n    \w+\(this\.service\(\)\?\.scmStatus, this\.service\(\)\?\.executar\)\n  \);\n",
        "\n",
        text,
    )
    text = re.sub(
        r"\n  readonly workLabel = computed\(\(\) => \{\n    if \(this\.isRunning\(\)\) return '[^']*';\n    if \(this\.processUp\(\) && this\.isLimitedTelemetry\(\)\) return 'Sem telemetria';\n    if \(this\.processUp\(\)\) return '[^']*';\n    return 'Parado';\n  \}\);\n",
        "\n",
        text,
    )

    # Insert heroStatus before primaryActionLabel (class body — not template)
    if "readonly heroStatus" not in text:
        text = text.replace(
            "  readonly primaryActionLabel = computed(() => {",
            build_hero_status(cfg) + "\n  readonly primaryActionLabel = computed(() => {",
        )

    # --:-- -> em andamento
    text = text.replace("display: '--:--',", "display: 'em andamento',")

    # found display 00:00 -> agora (when mode found)
    text = re.sub(
        r"(mode: 'found' as const,\n\s*caption: '[^']+',\n\s*)display: '00:00',",
        r"\1display: 'agora',",
        text,
    )

    # stageShortLabel before formatMmSs
    if "stageShortLabel" not in text:
        text = text.replace(
            "  private formatMmSs(totalSec: number): string {",
            build_stage_short_label(cfg) + "\n  private formatMmSs(totalSec: number): string {",
        )

    path.write_text(text, encoding="utf-8")
    print(f"patched dashboard: {svc}")


def patch_anatomy(svc: str) -> None:
    path = ROOT / svc / "anatomy-flow.component.ts"
    text = path.read_text(encoding="utf-8")
    text = text.replace(
        '[attr.title]="step.techHint"',
        '[attr.title]="step.blurb + \' · \' + step.techHint"',
    )
    text = re.sub(
        r"\n\s*<p class=\"anatomy-stage-blurb\">\{\{ step\.blurb \}\}</p>\n",
        "\n",
        text,
    )
    path.write_text(text, encoding="utf-8")
    print(f"patched anatomy: {svc}")


def main() -> None:
    for svc, cfg in SERVICES.items():
        patch_dashboard(svc, cfg)
        patch_anatomy(svc)


if __name__ == "__main__":
    main()
