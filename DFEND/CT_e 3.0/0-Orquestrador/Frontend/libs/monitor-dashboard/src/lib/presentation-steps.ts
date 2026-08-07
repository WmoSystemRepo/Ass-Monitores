/** Passos do modo Apresentação (tour guiado + simulação visual). */
export type PresentationSimulateMode = 'none' | 'flow' | 'stoppedBacklog';

export interface PresentationStep {
  id: string;
  title: string;
  body: string;
  /** Seletor CSS do alvo (data-tour="…"). */
  target?: string;
  /** Rota opcional ao entrar no passo. */
  route?: string;
  /** Overlay visual na cadeia. */
  simulate?: PresentationSimulateMode;
}

export const PRESENTATION_STEPS: PresentationStep[] = [
  {
    id: 'overview',
    title: 'Visão geral',
    body: 'Este é o Orquestrador da cadeia CT-e. Daqui você liga ou desliga as filas e acompanha os 6 serviços em tempo real.',
    target: '[data-tour="overview"]',
    route: '/',
  },
  {
    id: 'controls',
    title: 'Ligar e Desligar',
    body: 'Ligar as filas sobe o processo e coloca Executar=1. Desligar faz o inverso e deixa de consumir — a fila no banco não é apagada.',
    target: '[data-tour="controls"]',
    route: '/',
  },
  {
    id: 'health',
    title: 'Faixa de saúde',
    body: 'Aqui estão Fase, serviços ativos, Com fila e Arquivos. Verde = cadeia ligada; âmbar = parada com backlog; cinza = parado sem fila.',
    target: '[data-tour="health"]',
    route: '/',
  },
  {
    id: 'legend',
    title: 'Legenda de status',
    body: 'Erro (vermelho), Agora (azul), Na fila (laranja), Ativo sem fluxo (verde) e Parado (cinza). A cor do badge segue esta legenda.',
    target: '[data-tour="legend"]',
    route: '/',
  },
  {
    id: 'stations',
    title: 'Estações da cadeia',
    body: 'Cada caixa é um serviço. Clique para abrir o monitor. O número é a profundidade da fila naquele ponto.',
    target: '[data-tour="stations"]',
    route: '/',
  },
  {
    id: 'simulate-flow',
    title: 'Simulação do fluxo',
    body: 'Sem CT-e real: animamos o lote passando Receptor → Arquivador → Sintetizador → Analisador → Integrador → Carga. Banner “dados simulados”.',
    target: '[data-tour="stations"]',
    route: '/',
    simulate: 'flow',
  },
  {
    id: 'stopped-backlog',
    title: 'Parada com fila',
    body: 'Se desligar com documentos na fila, o badge fica NA FILA em laranja — a cadeia está parada, mas o backlog permanece.',
    target: '[data-tour="foco"]',
    route: '/',
    simulate: 'stoppedBacklog',
  },
  {
    id: 'validate',
    title: 'Validar cadeia',
    body: 'Validar consulta o banco sem READPAST e confirma se temp e filas Service Broker estão realmente vazias.',
    target: '[data-tour="validate"]',
    route: '/',
  },
  {
    id: 'monitor',
    title: 'Monitor de serviço',
    body: 'Abrimos o Receptor como exemplo: anatomia do ciclo, Ligar o fluxo, temporária e fila daquele serviço.',
    target: '[data-tour="stations"]',
    route: '/monitores/receptor',
  },
  {
    id: 'resgate',
    title: 'Resgate CT-e',
    body: 'No menu lateral, Resgate recupera chaves no Ambiente Nacional e enfileira na Carga.',
    target: '[data-tour="nav-resgate"]',
    route: '/resgate',
  },
  {
    id: 'end',
    title: 'Fim da apresentação',
    body: 'Use Sair para voltar à telemetria ao vivo. Pode reiniciar a apresentação a qualquer momento pelo botão no dashboard.',
    target: '[data-tour="overview"]',
    route: '/',
  },
];
