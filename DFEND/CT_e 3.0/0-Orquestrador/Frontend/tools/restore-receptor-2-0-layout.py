#!/usr/bin/env python3
"""Restore Receptor dashboard UI closer to CT_e 2.0 (no duplicate banners / always anatomy)."""
from __future__ import annotations

import re
from pathlib import Path

path = Path(__file__).resolve().parents[1] / "libs/service-monitors/src/lib/receptor/dashboard-page.component.ts"
text = path.read_text(encoding="utf-8")

old_imp = """import { DatePipe } from '@angular/common';
import { RouterLink } from '@angular/router';
import { ServiceMonitorStore } from '../service-monitor.store';
import { TableHealthCardsComponent } from '../table-health-cards.component';
import {
  connectionHealthLabel,
  formatHeartbeatAge,
  friendlyActionMessage,
  monitorConnectionLabel,
} from '@orquestrador/shared-utils';"""

new_imp = """import { DatePipe } from '@angular/common';
import { ServiceMonitorStore } from '../service-monitor.store';
import { TableHealthCardsComponent } from '../table-health-cards.component';
import {
  connectionHealthLabel,
  formatHeartbeatAge,
  friendlyActionMessage,
  monitorConnectionLabel,
  receptorStatusLabel,
} from '@orquestrador/shared-utils';"""

if old_imp not in text:
    # already patched or different — try without RouterLink
    if "receptorStatusLabel" not in text.split("template:")[0]:
        raise SystemExit("imports block not found / unexpected")
else:
    text = text.replace(old_imp, new_imp)

text = text.replace(
    "imports: [DatePipe, RouterLink, ReceptorAnatomyFlowComponent, TableHealthCardsComponent],",
    "imports: [DatePipe, ReceptorAnatomyFlowComponent, TableHealthCardsComponent],",
)

template = r"""  template: `
    <section class="dashboard-fit flex h-full max-h-full flex-col gap-1.5 overflow-hidden">
      <header class="flex shrink-0 flex-wrap items-center justify-between gap-2">
        <div class="min-w-0">
          <h1 class="text-base font-semibold leading-tight text-slate-50">
            Monitor do Receptor CT-e
          </h1>
          <p class="text-[11px] text-slate-400">
            Acompanhe se o sistema está recebendo documentos agora.
          </p>
        </div>
        <div class="flex flex-wrap items-center gap-1.5">
          <span
            class="inline-flex items-center gap-1.5 rounded border px-2 py-1 text-[11px]"
            [class.border-emerald-500]="store.live()"
            [class.text-emerald-400]="store.live()"
            [class.border-amber-500]="!store.live()"
            [class.text-amber-400]="!store.live()"
            [attr.title]="
              store.live()
                ? 'Monitor recebendo atualizações (SignalR)'
                : 'Monitor sem push recente'
            "
          >
            @if (store.live()) {
              <span class="live-dot"></span>
            }
            {{ connectionLabel() }}
            @if (store.lastPushAt(); as t) {
              · {{ t | date: 'HH:mm:ss' }}
            }
          </span>
          <button
            type="button"
            class="rounded bg-emerald-600 px-2.5 py-1.5 text-xs font-medium text-white transition hover:bg-emerald-500 disabled:opacity-40"
            [disabled]="store.actionBusy() || canStart() === false"
            (click)="store.startService()"
          >
            {{ primaryActionLabel() }}
          </button>
          <button
            type="button"
            class="rounded border border-rose-500/60 px-2.5 py-1.5 text-xs text-rose-300 transition hover:bg-rose-950/40 disabled:opacity-40"
            [disabled]="store.actionBusy()"
            (click)="confirmStop()"
          >
            Desligar
          </button>
        </div>
      </header>

      @if (isRunning()) {
        <div
          class="pulse-banner flex shrink-0 items-center justify-end gap-2 rounded-md border border-emerald-500/30 bg-emerald-950/30 px-3 py-1"
        >
          <div class="flex shrink-0 flex-wrap items-center justify-end gap-1.5">
            @if (cycleCountdown(); as clock) {
              <div
                class="cycle-chrono shrink-0"
                [class.cycle-chrono-busy]="clock.mode === 'busy'"
                [class.cycle-chrono-zero]="clock.mode === 'zero'"
                [attr.title]="clock.hint"
              >
                <span class="cycle-chrono-label">{{ clock.caption }}</span>
                <span class="cycle-chrono-digits">{{ clock.display }}</span>
              </div>
            }
            @if (fileWaitChrono(); as wait) {
              <div
                class="cycle-chrono cycle-chrono-wait shrink-0"
                [class.cycle-chrono-found]="wait.mode === 'found'"
                [attr.title]="wait.hint"
              >
                <span class="cycle-chrono-label">{{ wait.caption }}</span>
                <span class="cycle-chrono-digits">{{ wait.display }}</span>
              </div>
            }
          </div>
        </div>
      }

      @if (store.bootError(); as err) {
        <div class="shrink-0 rounded border border-rose-500/40 bg-rose-950/40 px-3 py-1 text-xs text-rose-200">
          Não foi possível falar com o monitor. {{ err }}
        </div>
      }
      @if (actionBanner(); as banner) {
        <div class="shrink-0 rounded border border-sky-500/30 bg-sky-950/30 px-3 py-1 text-xs text-sky-100">
          <span class="font-medium">{{ banner.title }}</span>
          @if (banner.detail) {
            <span class="ml-1 font-mono text-[10px] text-sky-300/80">{{ banner.detail }}</span>
          }
        </div>
      }

      <div
        class="health-strip flex shrink-0 flex-wrap items-center gap-x-4 gap-y-1 rounded-md border border-slate-700/80 bg-slate-900/50 px-3 py-1.5 text-[11px]"
        [class.health-strip-live]="isRunning()"
      >
        <span class="inline-flex items-baseline gap-1.5">
          <span class="text-slate-500">Receptor</span>
          <span class="font-medium text-slate-100">{{ statusLabel() }}</span>
        </span>
        <span class="hidden text-slate-700 sm:inline" aria-hidden="true">·</span>
        <span class="inline-flex items-baseline gap-1.5">
          <span class="text-slate-500">Recepção</span>
          <span
            class="font-medium"
            [class.text-emerald-300]="service()?.executar === 1"
            [class.text-amber-300]="isLimitedTelemetry()"
            [class.text-slate-300]="service()?.executar !== 1 && !isLimitedTelemetry()"
          >
            {{ receptionLabel() }}
          </span>
        </span>
        <span class="hidden text-slate-700 sm:inline" aria-hidden="true">·</span>
        <span class="inline-flex items-baseline gap-1.5">
          <span class="text-slate-500">Banco</span>
          <span class="font-medium text-slate-100">{{ healthLabel() }}</span>
        </span>
        <span class="hidden text-slate-700 sm:inline" aria-hidden="true">·</span>
        <span
          class="inline-flex min-w-0 items-baseline gap-1.5"
          [attr.title]="
            heartbeat().stale
              ? 'Última batida no banco (dtc_execucao) antiga — SVC_STALE conhecido na POC'
              : 'Última batida no banco'
          "
        >
          <span class="text-slate-500">Servidor</span>
          <span
            class="truncate font-medium"
            [class.text-amber-300]="heartbeat().stale"
            [class.text-slate-100]="!heartbeat().stale"
          >
            {{ service()?.nomServidor || '—' }}
            <span
              class="font-normal"
              [class.text-amber-200]="heartbeat().stale"
              [class.text-slate-400]="!heartbeat().stale"
            >
              · {{ heartbeat().text }}
            </span>
          </span>
        </span>
      </div>

      @if (store.tableHealth().length) {
        <lib-table-health-cards class="block shrink-0" [items]="store.tableHealth()" />
      }

      <div class="min-h-0 flex-1 overflow-hidden">
        <lib-receptor-anatomy-flow
          class="block h-full"
          [running]="isRunning()"
          [activeStage]="visualStage()"
          [caption]="flowCaption()"
          [latest]="latestLote()"
          [packets]="flyingPackets()"
        />
      </div>
    </section>
  `,
"""

text2, n = re.subn(r"  template: `.*?  `,\n\}\)", template + "})", text, count=1, flags=re.S)
if n != 1:
    raise SystemExit(f"template replace failed: {n}")
text = text2

if "readonly heroStatus" in text:
    text = re.sub(
        r"\n  /\*\* Status único legível em 2s.*?\n  \}\);\n\n  readonly primaryActionLabel",
        "\n  readonly statusLabel = computed(() =>\n"
        "    receptorStatusLabel(this.service()?.scmStatus, this.service()?.executar)\n"
        "  );\n\n"
        "  readonly receptionLabel = computed(() => {\n"
        "    if (this.service()?.executar === 1) return 'Ativa';\n"
        "    if (this.isLimitedTelemetry()) return 'Sem telemetria';\n"
        "    return 'Ociosa';\n"
        "  });\n\n"
        "  readonly primaryActionLabel",
        text,
        count=1,
        flags=re.S,
    )

if "stageShortLabel" in text:
    text = re.sub(
        r"\n  private stageShortLabel\(stage: AnatomyStage \| null\): string \| null \{\n.*?  \}\n\n  private formatMmSs",
        "\n  private formatMmSs",
        text,
        count=1,
        flags=re.S,
    )

text = text.replace(
    "return 'Ligue o Receptor para ver o pipeline interno (SEFAZ → consulta → temporária → fila → Arquivador).';",
    "return 'Ligue o Receptor para ver o fluxo SEFAZ → consulta → temporária → fila → Arquivador.';",
)

path.write_text(text, encoding="utf-8")
print("OK", path)
print("heroStatus=", "readonly heroStatus" in text)
print("receptionLabel=", "receptionLabel" in text)
print("health-strip=", "health-strip" in text)
print("receptor-empty=", "receptor-empty" in text)
