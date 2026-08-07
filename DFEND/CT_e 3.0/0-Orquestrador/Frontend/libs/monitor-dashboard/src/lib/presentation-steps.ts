/** Passos do modo Apresentação (tour guiado + simulação visual). */
export type PresentationSimulateMode = 'none' | 'flow' | 'stoppedBacklog';

export interface PresentationStep {
  id: string;
  title: string;
  /** Linhas curtas, linguagem simples (uma ideia por linha). */
  lines: string[];
  /** Nome amigável do pedaço da tela em destaque. */
  spotlightLabel?: string;
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
    title: 'O que é esta tela?',
    spotlightLabel: 'Painel principal',
    lines: [
      'Esta é a tela principal do Orquestrador.',
      'Aqui você acompanha o caminho do CT-e pelos 6 serviços.',
      'Pense nela como um painel de controle da fila.',
    ],
    target: '[data-tour="overview"]',
    route: '/',
  },
  {
    id: 'controls',
    title: 'Botões Ligar e Desligar',
    spotlightLabel: 'Ligar / Desligar',
    lines: [
      'Ligar as filas = os serviços começam a trabalhar.',
      'Desligar filas = eles param de processar.',
      'Importante: desligar não apaga o que já está na fila.',
      'Os documentos ficam guardados até você ligar de novo.',
    ],
    target: '[data-tour="controls"]',
    route: '/',
  },
  {
    id: 'health',
    title: 'Resumo de cima',
    spotlightLabel: 'Resumo rápido',
    lines: [
      'Esta faixa mostra o estado geral, em números.',
      'Fase diz se a cadeia está ligada ou parada.',
      'Com fila e Arquivos mostram se há CT-e esperando.',
      'Verde = ligado e saudável.',
      'Laranja = parado, mas ainda há documentos na fila.',
    ],
    target: '[data-tour="health"]',
    route: '/',
  },
  {
    id: 'legend',
    title: 'Cores e significados',
    spotlightLabel: 'Legenda',
    lines: [
      'Vermelho = erro, precisa de atenção.',
      'Azul = está processando agora.',
      'Laranja = há documentos na fila.',
      'Verde = ligado, mas sem movimento no momento.',
      'Cinza = parado.',
    ],
    target: '[data-tour="legend"]',
    route: '/',
  },
  {
    id: 'stations',
    title: 'Os 6 serviços',
    spotlightLabel: 'Cadeia de serviços',
    lines: [
      'Cada caixa é uma etapa do caminho do CT-e.',
      'A ordem é: Receptor → Arquivador → Sintetizador → Analisador → Integrador → Carga.',
      'O número embaixo mostra quantos documentos estão naquela etapa.',
      'Clique em uma caixa para abrir o detalhe daquele serviço.',
    ],
    target: '[data-tour="stations"]',
    route: '/',
  },
  {
    id: 'simulate-flow',
    title: 'Como o CT-e caminha',
    spotlightLabel: 'Simulação do fluxo',
    lines: [
      'Agora estamos só demonstrando — não é CT-e real.',
      'Veja o destaque azul “AGORA” passando de etapa em etapa.',
      'A esteira no meio mostra documentos em movimento.',
      'Assim fica fácil entender o fluxo completo.',
    ],
    target: '[data-tour="stations"]',
    route: '/',
    simulate: 'flow',
  },
  {
    id: 'stopped-backlog',
    title: 'Parou, mas a fila ficou',
    spotlightLabel: 'Foco · fila parada',
    lines: [
      'Se alguém desligar com documentos ainda na fila…',
      '…o serviço fica com o selo laranja “NA FILA”.',
      'Isso não é erro: os documentos estão esperando.',
      'Para continuar, use Ligar as filas.',
    ],
    target: '[data-tour="foco"]',
    route: '/',
    simulate: 'stoppedBacklog',
  },
  {
    id: 'validate',
    title: 'Botão Validar cadeia',
    spotlightLabel: 'Validar cadeia',
    lines: [
      'Use quando quiser ter certeza de que a fila está vazia.',
      'O sistema confere de forma mais rigorosa no banco.',
      'Se estiver tudo limpo, aparece “Validada vazia”.',
      'Se ainda houver documentos, ele mostra quanto sobrou.',
    ],
    target: '[data-tour="validate"]',
    route: '/',
  },
  {
    id: 'monitor',
    title: 'Tela de um serviço',
    spotlightLabel: 'Monitor do Receptor',
    lines: [
      'Abrimos o Receptor como exemplo.',
      'Cada serviço tem a própria tela de acompanhamento.',
      'Lá você vê o ciclo, a fila e se está ligado ou não.',
      'É o “zoom” de uma etapa da cadeia.',
    ],
    target: '[data-tour="stations"]',
    route: '/monitores/receptor',
  },
  {
    id: 'resgate',
    title: 'Menu Resgate',
    spotlightLabel: 'Resgate CT-e',
    lines: [
      'No menu à esquerda fica o Resgate.',
      'Serve para buscar de novo um CT-e que faltou.',
      'Depois ele entra na fila da Carga para ser processado.',
    ],
    target: '[data-tour="nav-resgate"]',
    route: '/resgate',
  },
  {
    id: 'end',
    title: 'Fim da apresentação',
    spotlightLabel: 'Painel principal',
    lines: [
      'Pronto — você já conhece as partes principais.',
      'Clique em Sair para voltar à tela ao vivo.',
      'Pode abrir a apresentação de novo pelo botão Apresentação.',
    ],
    target: '[data-tour="overview"]',
    route: '/',
  },
];
