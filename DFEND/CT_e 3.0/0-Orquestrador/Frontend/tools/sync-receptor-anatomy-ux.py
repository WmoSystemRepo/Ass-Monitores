#!/usr/bin/env python3
"""Replica animações/copy do Receptor nos anatomy-flow dos outros monitores."""
from __future__ import annotations

import re
from pathlib import Path

ROOT = Path(__file__).resolve().parents[1] / "libs" / "service-monitors" / "src" / "lib"

# service_id -> (eyebrow, h2, caption_default, idle_hint, soft_blurbs by stage id)
SOFT = {
    "arquivador": {
        "eyebrow": "Como o documento anda",
        "h2": "Caminho do CT-e no Arquivador",
        "caption": "Use Ligar o fluxo no topo para o Arquivador começar a processar a fila.",
        "idle": "Nenhum documento passando agora — o Arquivador processa a fila de tempos em tempos",
        "blurbs": {
            "fila": ("Entrada", "Pega o CT-e que chegou na fila."),
            "temp": ("Guarda rápido", "Lê o lote na temporária."),
            "sintetizador": ("Encaminha", "Avisa o Sintetizador."),
            "analisador": ("Análise", "Segue para análise do lote."),
            "integrador": ("Próximo", "Integra e limpa o temporário."),
        },
        "wait_id": "fila",
    },
    "sintetizador": {
        "eyebrow": "Como o documento anda",
        "h2": "Caminho do CT-e no Sintetizador",
        "caption": "Use Ligar o fluxo no topo para o Sintetizador começar a processar a fila.",
        "idle": "Nenhum documento passando agora — o Sintetizador processa a fila de tempos em tempos",
        "blurbs": {
            "fila": ("Entrada", "Retira o lote da fila."),
            "temp": ("Guarda rápido", "Lê o lote na temporária."),
            "classificar": ("Organiza", "Separa por tipo de documento."),
            "persistir": ("Grava", "Salva o resultado sintético."),
            "limpar": ("Limpa", "Remove o temporário ou registra erro."),
        },
        "wait_id": "fila",
    },
    "analisador": {
        "eyebrow": "Como o documento anda",
        "h2": "Caminho do CT-e no Analisador",
        "caption": "Use Ligar o fluxo no topo para o Analisador começar a processar a fila.",
        "idle": "Nenhum documento passando agora — o Analisador processa a fila de tempos em tempos",
        "blurbs": {
            "fila": ("Entrada", "Retira o lote da fila."),
            "temp": ("Guarda rápido", "Lê o lote na temporária."),
            "classificar": ("Organiza", "Classifica o documento."),
            "detalhar": ("Detalha", "Aprofunda a análise do lote."),
            "limpar": ("Limpa", "Remove o temporário ou registra erro."),
        },
        "wait_id": "fila",
    },
    "integrador": {
        "eyebrow": "Como o documento anda",
        "h2": "Caminho do CT-e no Integrador",
        "caption": "Use Ligar o fluxo no topo para o Integrador começar a processar a fila.",
        "idle": "Nenhum documento passando agora — o Integrador processa a fila de tempos em tempos",
        "blurbs": {
            "fila": ("Entrada", "Retira o lote da fila."),
            "temp": ("Guarda rápido", "Lê o lote na temporária."),
            "classificar": ("Organiza", "Prepara a integração."),
            "persistir": ("Grava", "Persiste o resultado integrado."),
            "limpar": ("Limpa", "Remove o temporário ou registra erro."),
        },
        "wait_id": "fila",
    },
    "carga": {
        "eyebrow": "Como o documento anda",
        "h2": "Caminho do CT-e na Carga",
        "caption": "Use Ligar o fluxo no topo para a Carga começar o download/processamento.",
        "idle": "Nenhum documento passando agora — a Carga processa sob demanda",
        "blurbs": {
            "fila": ("Entrada", "Retira o pedido da fila."),
            "temp": ("Guarda rápido", "Lê o lote na temporária."),
            "classificar": ("Organiza", "Prepara o download/carga."),
            "persistir": ("Grava", "Persiste o resultado da carga."),
            "limpar": ("Limpa", "Remove o temporário ou registra erro."),
        },
        "wait_id": "fila",
    },
}


def patch_header(text: str, soft: dict) -> str:
    text = re.sub(
        r"\[class\.anatomy-poster-busy\]=\"!!activeStage\(\)\"\s*\n\s*>",
        '[class.anatomy-poster-busy]="!!activeStage()"\n'
        '      [class.anatomy-poster-starting]="isBooting()"\n'
        "    >",
        text,
        count=1,
    )
    text = re.sub(
        r'<p class="text-\[10px\] font-semibold uppercase tracking-\[0\.16em\] [^"]+">\s*[^<]+\s*</p>',
        f'<p class="text-[10px] font-semibold uppercase tracking-[0.16em] text-sky-300/90">\n'
        f'            {soft["eyebrow"]}\n'
        f"          </p>",
        text,
        count=1,
    )
    text = re.sub(
        r"(<h2\b[^>]*>)\s*[^<]+\s*(</h2>)",
        rf'\1\n            {soft["h2"]}\n          \2',
        text,
        count=1,
    )
    text = re.sub(
        r'Sem CT-e em trânsito[^\n<]*',
        soft["idle"],
        text,
        count=1,
    )
    text = re.sub(
        r"readonly caption = input\('([^']*)'\);",
        f"readonly caption = input(\n    '{soft['caption']}'\n  );",
        text,
        count=1,
    )
    return text


def patch_stages_template(text: str, wait_id: str) -> str:
    old = """            <div class="anatomy-stages relative z-[1] shrink-0 py-1">
              @for (step of steps; track step.id; let i = $index) {
                <div
                  class="anatomy-stage"
                  [class.anatomy-stage-active]="activeStage() === step.id"
                  [class.anatomy-stage-done]="isDone(step.id)"
                >
                  @if (activeStage() === step.id) {
                    <span class="anatomy-now">AGORA</span>
                  }
                  <div
                    class="anatomy-platform"
                    [class.anatomy-platform-active]="activeStage() === step.id"
                    [class.anatomy-platform-done]="isDone(step.id)"
                    [class.anatomy-platform-waiting]="running() && !activeStage() && step.id === 'fila'"
                    [attr.title]="step.techHint"
                  >
                    <div class="anatomy-iso" [attr.data-icon]="step.id"></div>
                  </div>
                  <p class="anatomy-stage-title">{{ step.title }}</p>
                  <p class="anatomy-stage-tag">{{ step.tag }}</p>
                  <p class="anatomy-stage-count">{{ count(step.id) }}</p>
                  <p class="anatomy-stage-blurb">{{ step.blurb }}</p>
                </div>"""

    # carga may have slightly different waiting line
    if old not in text:
        old = old.replace(
            "[class.anatomy-platform-waiting]=\"running() && !activeStage() && step.id === 'fila'\"",
            "[class.anatomy-platform-waiting]=\"\n                      running() && !activeStage() && step.id === 'fila'\n                    \"",
        )

    new = f"""            <div class="anatomy-stages relative z-[1] shrink-0 py-1">
              @for (step of steps; track step.id; let i = $index) {{
                <div
                  class="anatomy-stage"
                  [class.anatomy-stage-active]="activeStage() === step.id"
                  [class.anatomy-stage-done]="isDone(step.id)"
                  [class.anatomy-stage-booting]="isBooting()"
                  [style.--boot-delay]="i * 0.12 + 's'"
                >
                  @if (activeStage() === step.id) {{
                    <span class="anatomy-now">AGORA</span>
                  }}
                  <div
                    class="anatomy-platform"
                    [class.anatomy-platform-active]="activeStage() === step.id"
                    [class.anatomy-platform-done]="isDone(step.id)"
                    [class.anatomy-platform-waiting]="
                      running() && !activeStage() && step.id === '{wait_id}'
                    "
                    [class.anatomy-platform-rising]="queueMotion(step.id) === 'rising'"
                    [class.anatomy-platform-draining]="queueMotion(step.id) === 'draining'"
                    [class.anatomy-platform-booting]="isBooting()"
                    [attr.title]="step.techHint"
                  >
                    <div class="anatomy-iso" [attr.data-icon]="step.id"></div>
                    @if (queueChips(step.id); as chips) {{
                      @if (chips.length > 0) {{
                        <div class="anatomy-queue-stack" aria-hidden="true">
                          @for (c of chips; track c) {{
                            <span
                              class="anatomy-queue-block"
                              [style.--chip-i]="c"
                            ></span>
                          }}
                        </div>
                      }}
                    }}
                  </div>
                  <p class="anatomy-stage-title">{{{{ step.title }}}}</p>
                  <p class="anatomy-stage-tag">{{{{ step.tag }}}}</p>
                  <p
                    class="anatomy-stage-count"
                    [class.anatomy-stage-count-hot]="depthOf(step.id) > 0"
                  >
                    {{{{ count(step.id) }}}}
                  </p>
                  <p class="anatomy-stage-blurb">{{{{ step.blurb }}}}</p>
                </div>"""

    if old not in text:
        raise SystemExit("stages block not found")
    return text.replace(old, new, 1)


def patch_path_line(text: str) -> str:
    return text.replace(
        '[class.anatomy-path-line-active]="running()"',
        '[class.anatomy-path-line-active]="running() || isBooting()"',
        1,
    )


def patch_imports(text: str) -> str:
    if "effect," in text:
        return text
    return text.replace(
        "  computed,\n  inject,\n  input,",
        "  computed,\n  effect,\n  inject,\n  input,",
        1,
    )


def patch_class_body(text: str) -> str:
    if "readonly isBooting = computed" in text:
        return text

    insert_after = "  readonly packets = input<FlyingPacket[]>([]);\n"
    if "readonly consuming" in text:
        insert_after = "  readonly consuming = input(false);\n"

    motion_block = """
  /** Ligar: anima as plataformas em cascata enquanto o start está em andamento. */
  readonly isBooting = computed(
    () => this.store.actionBusy() && !this.running()
  );

  private readonly prevTemp = signal(0);
  private readonly prevBroker = signal(0);
  readonly tempMotion = signal<'idle' | 'rising' | 'draining'>('idle');
  readonly brokerMotion = signal<'idle' | 'rising' | 'draining'>('idle');
  private tempMotionTimer?: ReturnType<typeof setTimeout>;
  private brokerMotionTimer?: ReturnType<typeof setTimeout>;

"""
    if insert_after not in text:
        raise SystemExit("packets/consuming anchor not found")
    text = text.replace(insert_after, insert_after + motion_block, 1)

    # blurbs soft replace per step — done separately

    ctor_old = """  constructor() {
    // Mantém OnPush fresco para counts de fila/tmp via store pushes; tick leve.
    this.clock = setInterval(() => this.nowMs.set(Date.now()), 1000);
  }

  ngOnDestroy(): void {
    if (this.clock) clearInterval(this.clock);
  }

  lanePercent(lane: number): number {
    return 10 + lane * 20;
  }

  count(id: AnatomyStage): string {"""

    ctor_new = """  constructor() {
    // Mantém OnPush fresco para counts de fila/tmp via store pushes; tick leve.
    this.clock = setInterval(() => this.nowMs.set(Date.now()), 1000);

    effect(() => {
      const temp = Math.max(0, Math.floor(this.store.queues()?.tempBacklog ?? 0));
      const prev = this.prevTemp();
      if (temp !== prev) {
        if (this.tempMotionTimer) clearTimeout(this.tempMotionTimer);
        this.tempMotion.set(temp > prev ? 'rising' : 'draining');
        this.prevTemp.set(temp);
        this.tempMotionTimer = setTimeout(() => this.tempMotion.set('idle'), 700);
      }
    });

    effect(() => {
      const broker = Math.max(
        0,
        Math.floor(this.store.queues()?.serviceBrokerDepth ?? 0)
      );
      const prev = this.prevBroker();
      if (broker !== prev) {
        if (this.brokerMotionTimer) clearTimeout(this.brokerMotionTimer);
        this.brokerMotion.set(broker > prev ? 'rising' : 'draining');
        this.prevBroker.set(broker);
        this.brokerMotionTimer = setTimeout(
          () => this.brokerMotion.set('idle'),
          700
        );
      }
    });
  }

  ngOnDestroy(): void {
    if (this.clock) clearInterval(this.clock);
    if (this.tempMotionTimer) clearTimeout(this.tempMotionTimer);
    if (this.brokerMotionTimer) clearTimeout(this.brokerMotionTimer);
  }

  lanePercent(lane: number): number {
    return 10 + lane * 20;
  }

  depthOf(id: AnatomyStage): number {
    this.nowMs();
    if (id === 'temp') return this.store.queues()?.tempBacklog ?? 0;
    if (id === 'fila') return this.store.queues()?.serviceBrokerDepth ?? 0;
    return 0;
  }

  queueChips(id: AnatomyStage): number[] {
    const d = Math.max(0, Math.floor(this.depthOf(id)));
    if (d <= 0) return [];
    const n = Math.min(8, Math.max(1, d));
    return Array.from({ length: n }, (_, i) => i);
  }

  queueMotion(id: AnatomyStage): 'idle' | 'rising' | 'draining' {
    if (id === 'temp') return this.tempMotion();
    if (id === 'fila') return this.brokerMotion();
    return 'idle';
  }

  count(id: AnatomyStage): string {"""

    if ctor_old not in text:
        raise SystemExit("constructor block not found")
    text = text.replace(ctor_old, ctor_new, 1)

    # Soften zero counts
    text = text.replace("'0 na fila'", "'vazia'")
    text = text.replace("'0 aguardando'", "'vazia'")
    return text


def patch_blurbs(text: str, blurbs: dict[str, tuple[str, str]]) -> str:
    for stage_id, (tag, blurb) in blurbs.items():
        pattern = re.compile(
            rf"(id: '{stage_id}',\s*title: '[^']*',\s*tag: ')[^']*(',\s*blurb: ')[^']*(',)",
            re.M,
        )
        text2, n = pattern.subn(rf"\g<1>{tag}\g<2>{blurb}\g<3>", text, count=1)
        if n == 0:
            # try multiline with newlines
            pattern = re.compile(
                rf"(id: '{stage_id}',\n\s*title: '[^']*',\n\s*tag: ')[^']*(',\n\s*blurb: ')[^']*(',)",
                re.M,
            )
            text2, n = pattern.subn(rf"\g<1>{tag}\g<2>{blurb}\g<3>", text, count=1)
        if n == 0:
            print(f"  warn: blurb not patched for {stage_id}")
        else:
            text = text2
    return text


def main() -> None:
    for svc, soft in SOFT.items():
        path = ROOT / svc / "anatomy-flow.component.ts"
        text = path.read_text(encoding="utf-8")
        if "isBooting" in text:
            print(f"skip {svc} (already patched)")
            continue
        text = patch_imports(text)
        text = patch_header(text, soft)
        text = patch_path_line(text)
        text = patch_stages_template(text, soft["wait_id"])
        text = patch_class_body(text)
        text = patch_blurbs(text, soft["blurbs"])
        path.write_text(text, encoding="utf-8")
        print(f"ok {svc}")


if __name__ == "__main__":
    main()
