#!/usr/bin/env python3
"""Injeta métodos de animação de fila/boot nos anatomy-flow (template já referencia)."""
from __future__ import annotations

from pathlib import Path

ROOT = Path(__file__).resolve().parents[1] / "libs" / "service-monitors" / "src" / "lib"
SERVICES = ["arquivador", "sintetizador", "analisador", "integrador", "carga"]

MOTION_FIELDS = """
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

CTOR_OLD = """  constructor() {
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

CTOR_NEW = """  constructor() {
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


def ensure_effect_import(text: str) -> str:
    if "\n  effect,\n" in text or "\n  effect," in text:
        return text
    return text.replace(
        "  computed,\n  inject,\n  input,",
        "  computed,\n  effect,\n  inject,\n  input,",
        1,
    )


def main() -> None:
    for svc in SERVICES:
        path = ROOT / svc / "anatomy-flow.component.ts"
        text = path.read_text(encoding="utf-8")
        if "readonly isBooting = computed" in text:
            print(f"skip {svc}")
            continue
        text = ensure_effect_import(text)

        # Insert fields after packets (or after consuming if present)
        if "readonly consuming = input(false);" in text:
            anchor = "  readonly consuming = input(false);\n"
        else:
            anchor = "  readonly packets = input<FlyingPacket[]>([]);\n"
        if anchor not in text:
            raise SystemExit(f"{svc}: anchor missing")
        text = text.replace(anchor, anchor + MOTION_FIELDS, 1)

        if CTOR_OLD not in text:
            raise SystemExit(f"{svc}: ctor block missing")
        text = text.replace(CTOR_OLD, CTOR_NEW, 1)

        text = text.replace("'0 na fila'", "'vazia'")
        text = text.replace("'0 aguardando'", "'vazia'")

        path.write_text(text, encoding="utf-8")
        print(f"ok {svc}")


if __name__ == "__main__":
    main()
