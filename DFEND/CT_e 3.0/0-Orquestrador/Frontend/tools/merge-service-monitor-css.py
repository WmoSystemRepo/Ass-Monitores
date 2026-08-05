from pathlib import Path
import re

base = Path(r"C:\Users\Mendes\Desktop\Clones\Assefaz\DFEND\CT_e 2.0")
out = Path(
    r"C:\Users\Mendes\Desktop\Clones\Assefaz\DFEND\CT_e\0-Orquestrador\Frontend\apps\cte-orquestrador\src\service-monitor-extras.css"
)

styles = {
    "receptor": base / "1-Receptor/Frontend/apps/cte-receptor/src/styles.css",
    "arquivador": base / "2-Arquivador/Frontend/apps/cte-arquivador/src/styles.css",
    "sintetizador": base / "3-Sintetizador/Frontend/apps/cte-sintetizador/src/styles.css",
    "analisador": base / "4-Analisador/Frontend/apps/cte-analisador/src/styles.css",
    "integrador": base / "5-Integrador/Frontend/apps/cte-integrador/src/styles.css",
    "carga": base / "6-Carga/Frontend/apps/cte-carga/src/styles.css",
}

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

out.write_text("\n".join(parts), encoding="utf-8")
icons = sorted(
    {
        re.search(r"data-icon='([^']+)'", k).group(1)
        for k in icon_rules
        if re.search(r"data-icon='([^']+)'", k)
    }
)
print(f"Wrote {out} bytes={out.stat().st_size} icons={len(icon_rules)}")
print("Icons:", ", ".join(icons))
