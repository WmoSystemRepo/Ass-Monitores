"""
Adapt CT_e 2.0 monitor dashboards into unified service-monitors lib.
Run from Frontend folder or any cwd.
"""
from __future__ import annotations

import re
from pathlib import Path

ROOT = Path(r"C:\Users\Mendes\Desktop\Clones\Assefaz\DFEND\CT_e\0-Orquestrador\Frontend")
SRC20 = Path(r"C:\Users\Mendes\Desktop\Clones\Assefaz\DFEND\CT_e 2.0")
DEST = ROOT / "libs/service-monitors/src/lib"

SERVICES = [
    ("receptor", "1-Receptor", "Receptor", "ReceptorMonitorStore", "@receptor"),
    ("arquivador", "2-Arquivador", "Arquivador", "ArquivadorMonitorStore", "@arquivador"),
    ("sintetizador", "3-Sintetizador", "Sintetizador", "SintetizadorMonitorStore", "@sintetizador"),
    ("analisador", "4-Analisador", "Analisador", "AnalisadorMonitorStore", "@analisador"),
    ("integrador", "5-Integrador", "Integrador", "IntegradorMonitorStore", "@integrador"),
    ("carga", "6-Carga", "Carga", "CargaMonitorStore", "@Carga"),
]


def adapt(text: str, service_id: str, prefix: str, old_store: str, alias: str, kind: str) -> str:
    aliases = {alias, alias.lower(), "@carga" if service_id == "carga" else alias}
    for a in aliases:
        text = text.replace(f"{a}/shared-data", "@orquestrador/shared-data")
        text = text.replace(f"{a}/monitor-tables", "../table-health-cards.component")
        text = text.replace(f"{a}/monitor-core", "../service-monitor.store")
        text = text.replace(f"from '{a}/shared-utils'", "from '@orquestrador/shared-utils'")

    if kind == "dashboard" and "resolvePipelineActivity" in text:
        if "from './pipeline-activity'" not in text:
            text = text.replace(
                "from '@orquestrador/shared-utils';",
                "from '@orquestrador/shared-utils';\n"
                "import {\n"
                "  resolvePipelineActivity,\n"
                "  type PipelineStage,\n"
                "} from './pipeline-activity';",
            )
        text = re.sub(r"(?m)^\s*resolvePipelineActivity,\s*\n", "", text)
        text = re.sub(r"(?m)^\s*type PipelineStage,\s*\n", "", text)

    text = text.replace(old_store, "ServiceMonitorStore")

    if kind == "dashboard":
        text = text.replace(
            "export class DashboardPageComponent",
            f"export class {prefix}DashboardPageComponent",
        )
        text = text.replace(
            "selector: 'lib-dashboard-page'",
            f"selector: 'lib-{service_id}-dashboard-page'",
        )
        text = text.replace("AnatomyFlowComponent", f"{prefix}AnatomyFlowComponent")
        text = text.replace("<lib-anatomy-flow", f"<lib-{service_id}-anatomy-flow")
        text = text.replace("</lib-anatomy-flow>", f"</lib-{service_id}-anatomy-flow>")
    elif kind == "anatomy":
        text = text.replace(
            "export class AnatomyFlowComponent",
            f"export class {prefix}AnatomyFlowComponent",
        )
        text = text.replace(
            "selector: 'lib-anatomy-flow'",
            f"selector: 'lib-{service_id}-anatomy-flow'",
        )
    elif kind == "details":
        text = text.replace(
            "export class DetailsPageComponent",
            f"export class {prefix}DetailsPageComponent",
        )
        text = text.replace(
            "selector: 'lib-details-page'",
            f"selector: 'lib-{service_id}-details-page'",
        )

    text = text.replace(
        'routerLink="/mais-informacoes"',
        f'routerLink="/monitores/{service_id}/mais-informacoes"',
    )
    text = text.replace(
        'routerLink="/threads"', f'routerLink="/monitores/{service_id}/threads"'
    )
    text = text.replace(
        'routerLink="/logs"', f'routerLink="/monitores/{service_id}/logs"'
    )
    text = text.replace(
        'routerLink="/config"', f'routerLink="/monitores/{service_id}/config"'
    )
    text = text.replace(
        'routerLink="/tabelas"', f'routerLink="/monitores/{service_id}/tabelas"'
    )
    text = re.sub(
        r'routerLink="/"(\s+class="text-xs)',
        f'routerLink="/monitores/{service_id}"\\1',
        text,
    )
    return text


def main() -> None:
    for service_id, folder, prefix, old_store, alias in SERVICES:
        src_dash = (
            SRC20 / folder / "Frontend/libs/monitor-dashboard/src/lib"
        )
        dest = DEST / service_id
        dest.mkdir(parents=True, exist_ok=True)

        # ensure pipeline exists
        pipe_src = (
            SRC20
            / folder
            / "Frontend/libs/shared-utils/src/lib/pipeline-activity.ts"
        )
        if pipe_src.exists():
            (dest / "pipeline-activity.ts").write_text(
                pipe_src.read_text(encoding="utf-8"), encoding="utf-8"
            )

        mapping = {
            "dashboard-page.component.ts": "dashboard",
            "anatomy-flow.component.ts": "anatomy",
            "details-page.component.ts": "details",
        }
        for fname, kind in mapping.items():
            src = src_dash / fname
            text = src.read_text(encoding="utf-8")
            adapted = adapt(text, service_id, prefix, old_store, alias, kind)
            (dest / fname).write_text(adapted, encoding="utf-8")
            print(f"OK {service_id}/{fname}")

    # table health cards
    th = (
        SRC20
        / "1-Receptor/Frontend/libs/monitor-tables/src/lib/table-health-cards.component.ts"
    )
    th_text = th.read_text(encoding="utf-8")
    th_text = th_text.replace("@receptor/shared-data", "@orquestrador/shared-data")
    th_text = th_text.replace("@receptor/shared-utils", "@orquestrador/shared-utils")
    # Make routerLink service-aware via relative path
    th_text = th_text.replace('routerLink="/tabelas/', 'routerLink="tabelas/')
    (DEST / "table-health-cards.component.ts").write_text(th_text, encoding="utf-8")
    print("OK table-health-cards")


if __name__ == "__main__":
    main()
