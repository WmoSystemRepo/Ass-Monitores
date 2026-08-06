"""
Mescla CSS de anatomia/cronômetros dos monitores CT_e 2.0 em
service-monitor-extras.css do Orquestrador 3.0 (escopo .pipeline-anatomy
é aplicado manualmente / já presente no arquivo gerado).

Paths padrão:
  - Fonte: Ass-Monitores/DFEND/CT_e 2.0
  - Destino: Ass-Monitores (workspace) DFEND/CT_e 3.0/.../service-monitor-extras.css
"""
from pathlib import Path
import re
import os

# Resolve a partir deste script: .../Frontend/tools → sobe até DFEND
_tools = Path(__file__).resolve().parent
_frontend = _tools.parent
_orquestrador = _frontend.parent
_cte30 = _orquestrador.parent  # CT_e 3.0
_dfend = _cte30.parent  # DFEND (workspace ou Ass-Monitores)

# Preferir CT_e 2.0 irmão no mesmo DFEND; senão clone Ass-Monitores clássico.
_candidates_20 = [
    Path(r"C:\Users\Mendes\Desktop\Clones\Assefaz\Ass-Monitores\DFEND\CT_e 2.0"),
    _dfend / "CT_e 2.0",
    Path(r"C:\Users\Mendes\Desktop\Clones\Assefaz\DFEND\Ass-Monitores\DFEND\CT_e 2.0"),
]
base = next((p for p in _candidates_20 if p.is_dir()), _candidates_20[0])

out = (
    _frontend
    / "apps"
    / "cte-orquestrador"
    / "src"
    / "service-monitor-extras.css"
)

styles = {
    "receptor": base / "1-Receptor/Frontend/apps/cte-receptor/src/styles.css",
    "arquivador": base / "2-Arquivador/Frontend/apps/cte-arquivador/src/styles.css",
    "sintetizador": base / "3-Sintetizador/Frontend/apps/cte-sintetizador/src/styles.css",
    "analisador": base / "4-Analisador/Frontend/apps/cte-analisador/src/styles.css",
    "integrador": base / "5-Integrador/Frontend/apps/cte-integrador/src/styles.css",
    "carga": base / "6-Carga/Frontend/apps/cte-carga/src/styles.css",
}

missing = [k for k, p in styles.items() if not p.is_file()]
if missing:
    raise SystemExit(
        f"CT_e 2.0 styles ausentes ({base}): {', '.join(missing)}\n"
        "Ajuste _candidates_20 em merge-service-monitor-css.py."
    )

rx = styles["receptor"].read_text(encoding="utf-8")
start = rx.find("/* —— Cronômetro")
if start < 0:
    start = rx.find(".cycle-chrono {")
chunk = rx[start:] if start >= 0 else rx

iso_base = """/* —— Service monitor anatomy extras (exact CT_e 2.0) —— */
.anatomy-iso {
  position: relative;
  width: 2.75rem;
  height: 2.75rem;
}
.anatomy-iso::before,
.anatomy-iso::after {
  content: '';
  position: absolute;
  inset: 0;
  margin: auto;
}
"""

icon_re = re.compile(
    r"(\.anatomy-iso\[data-icon='[^']+'\]::(?:before|after)\s*\{[^}]*\})",
    re.S,
)
icon_rules: dict[str, str] = {}
for path in styles.values():
    text = path.read_text(encoding="utf-8")
    for m in icon_re.finditer(text):
        rule = m.group(1).strip()
        sel_m = re.match(
            r"(\.anatomy-iso\[data-icon='[^']+'\]::(?:before|after))", rule
        )
        if not sel_m:
            continue
        sel = sel_m.group(1)
        if sel not in icon_rules:
            icon_rules[sel] = rule

chunk_wo_icons = icon_re.sub("", chunk)

parts = [
    iso_base,
    chunk_wo_icons,
    "\n/* —— data-icon set (all services) —— */\n",
]
for sel in sorted(icon_rules.keys()):
    parts.append(icon_rules[sel])
    parts.append("\n")

# Não sobrescrever o extras já escopado em .pipeline-anatomy —
# grava um artefato de referência ao lado para diff/merge manual.
ref_out = out.with_name("service-monitor-extras.from-2.0.css")
ref_out.write_text("\n".join(parts), encoding="utf-8")
icons = sorted(
    {
        re.search(r"data-icon='([^']+)'", k).group(1)
        for k in icon_rules
        if re.search(r"data-icon='([^']+)'", k)
    }
)
print(f"Fonte CT_e 2.0: {base}")
print(f"Wrote reference {ref_out} bytes={ref_out.stat().st_size} icons={len(icon_rules)}")
print(f"Live extras (não sobrescrito): {out}")
print("Icons:", ", ".join(icons))
