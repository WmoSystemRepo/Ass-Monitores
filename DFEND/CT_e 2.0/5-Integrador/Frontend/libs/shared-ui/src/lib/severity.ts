export function severityBadgeClass(severity: string | number): string {
  const label =
    typeof severity === 'number'
      ? ['Info', 'Atenção', 'Alerta', 'Crítico'][severity]
      : severity;
  switch (label) {
    case 'Critico':
    case 'Crítico':
      return 'border-rose-500 text-rose-300';
    case 'Alerta':
      return 'border-orange-500 text-orange-300';
    case 'Atenção':
      return 'border-amber-500 text-amber-300';
    default:
      return 'border-zinc-600 text-zinc-300';
  }
}
