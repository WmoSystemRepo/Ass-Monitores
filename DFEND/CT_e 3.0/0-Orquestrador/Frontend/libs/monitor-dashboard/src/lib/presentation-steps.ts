/** Passos do modo Apresentação (tour guiado + simulação visual). */
export type PresentationSimulateMode =
  | 'none'
  | 'flow'
  | 'stoppedBacklog'
  | 'receptorFlow'
  | 'detailsFlow'
  | 'tablesFlow'
  | 'threadsFlow';

export type PresentationReceptorStage =
  | 'sefaz'
  | 'consulta'
  | 'temp'
  | 'broker'
  | 'arquivador';

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
  /** Overlay visual na cadeia ou no monitor. */
  simulate?: PresentationSimulateMode;
  /** Onde fixar o flash card para não cobrir o alvo. */
  panelPlacement?: 'top' | 'bottom' | 'left' | 'right';
}

/**
 * Ordem: em cada tela, todos os componentes primeiro;
 * o último passo da tela destaca o clique que leva à próxima.
 * O Receptor é o exemplo — os outros monitores são iguais.
 */
export const PRESENTATION_STEPS: PresentationStep[] = [
  // —— Painel da cadeia ——
  {
    id: 'overview',
    title: 'O que é esta tela?',
    spotlightLabel: 'Painel principal',
    lines: [
      'Esta é a tela principal do Orquestrador.',
      'Aqui você acompanha o caminho do CT-e pelos 6 serviços.',
      'Pense nela como um painel de controle do processo.',
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
      'Orquestrador = se o painel está online e recebendo dados.',
      'Serviços ativos = quantos dos 6 serviços estão ligados.',
      'Processos no ar = quantos processos Windows estão rodando.',
      'Fase diz se a cadeia está ligada ou parada.',
      'Com fila e Arquivos mostram se há CT-e esperando.',
      'Verde = ligado e saudável.',
      'Amarelo = parado, mas ainda há documentos na fila.',
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
      'Amarelo = há documentos na fila.',
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
    panelPlacement: 'top',
    lines: [
      'Cada caixa é uma fila de processamento do CT-e.',
      'A ordem é: Receptor → Arquivador → Sintetizador → Analisador → Integrador → Carga.',
      'No Receptor, o número embaixo é o último NSU (lote) gerado.',
      'No Integrador, staging 0 = nenhum CT-e pendente nas tabelas de carga do Netezza.',
      'Os outros monitores são iguais ao Receptor — vamos abrir o Receptor como exemplo.',
    ],
    target: '[data-tour="stations"]',
    route: '/',
  },
  {
    id: 'stopped-backlog',
    title: 'Parou, mas a fila ficou',
    spotlightLabel: 'Foco · fila parada',
    lines: [
      'Se alguém desligar com documentos ainda na fila, o serviço fica com o selo amarelo “NA FILA”.',
      'Isso não é erro: os documentos estão esperando.',
      'Para continuar, use Ligar as filas.',
    ],
    target: '[data-tour="foco"]',
    route: '/',
    simulate: 'stoppedBacklog',
  },
  {
    id: 'validate',
    title: 'Botão Validar filas',
    spotlightLabel: 'Validar filas',
    lines: [
      'Fica no topo, junto de Ligar / Desligar filas.',
      'Confere se as filas e temporárias dos 6 serviços estão vazias.',
      'Se estiver limpo, aparece “Filas vazias”. Se houver backlog, mostra quanto sobrou.',
    ],
    target: '[data-tour="validate"]',
    route: '/',
  },
  {
    id: 'simulate-flow',
    title: 'Como o CT-e caminha',
    spotlightLabel: 'Simulação do fluxo',
    panelPlacement: 'right',
    lines: [
      'Agora estamos só demonstrando — não é CT-e real.',
      'Veja o destaque azul “AGORA” passando de etapa em etapa.',
      'A esteira no meio mostra documentos em movimento.',
      'Avançar abre o monitor do Receptor.',
    ],
    target: '[data-tour="stations"]',
    route: '/',
    simulate: 'flow',
  },
  {
    id: 'nav-monitor',
    title: 'Abrir o monitor do Receptor',
    spotlightLabel: 'Cadeia de serviços',
    panelPlacement: 'top',
    lines: [
      'Vamos entrar no Receptor — é o “zoom” de uma etapa.',
      'Dentro dele você vê como funciona o fluxo dentro de cada fila.',
      'Arquivador, Sintetizador e os demais têm a mesma organização de telas.',
      'Avançar abre o monitor do Receptor.',
    ],
    target: '[data-tour="stations"]',
    route: '/',
  },

  // —— Monitor Receptor (painel) ——
  {
    id: 'shell-back',
    title: 'Voltar ao painel geral',
    spotlightLabel: 'Voltar ao painel',
    lines: [
      'Este link volta para a cadeia com os 6 serviços.',
      'Fica sempre no topo do monitor.',
    ],
    target: '[data-tour="shell-back"]',
    route: '/monitores/receptor',
  },
  {
    id: 'dash-header',
    title: 'Título do serviço',
    spotlightLabel: 'Título do Receptor',
    lines: [
      'Aqui fica o nome do serviço e o que ele faz, em uma frase.',
      'Busca documentos novos na SEFAZ e envia para o próximo serviço da fila.',
    ],
    target: '[data-tour="dash-header"]',
    route: '/monitores/receptor',
  },
  {
    id: 'dash-live',
    title: 'Conexão ao vivo',
    spotlightLabel: 'Online / offline',
    lines: [
      'Mostra se a tela está recebendo atualizações.',
      'Verde = Orquestrador online. Amarelo = Orquestrador offline.',
    ],
    target: '[data-tour="dash-live"]',
    route: '/monitores/receptor',
  },
  {
    id: 'dash-controls',
    title: 'Ligar e Desligar neste serviço',
    spotlightLabel: 'Ligar / Desligar do monitor',
    lines: [
      'São os botões deste serviço específico.',
      'Ligar = ele começa a trabalhar.',
      'Desligar = ele para (a fila não é apagada).',
    ],
    target: '[data-tour="dash-controls"]',
    route: '/monitores/receptor',
  },
  {
    id: 'dash-health',
    title: 'Resumo do serviço',
    spotlightLabel: 'Faixa de status',
    lines: [
      'Receptor = se este serviço está ligado ou parado.',
      'Recepção = se a busca de CT-e está ativa.',
      'Banco = se a conexão SQL está respondendo.',
      'Servidor = qual máquina e se a batida (heartbeat) está em dia.',
    ],
    target: '[data-tour="dash-health"]',
    route: '/monitores/receptor',
  },
  {
    id: 'dash-tables',
    title: 'Cartões das tabelas',
    spotlightLabel: 'Saúde das tabelas',
    lines: [
      'Cada cartão é uma tabela importante do banco.',
      'O status diz se está ok ou precisa de atenção.',
      'Clicar abre os dados daquela tabela.',
    ],
    target: '[data-tour="dash-tables"]',
    route: '/monitores/receptor',
  },
  {
    id: 'anatomy',
    title: 'Caminho do CT-e',
    spotlightLabel: 'Desenho do fluxo',
    panelPlacement: 'top',
    lines: [
      'Este quadro mostra o caminho do documento neste serviço.',
      'É a “história visual” do que o Receptor faz.',
    ],
    target: '[data-tour="anatomy"]',
    route: '/monitores/receptor',
  },
  {
    id: 'anatomy-legend',
    title: 'Legenda do desenho',
    spotlightLabel: 'Legenda Agora / Feito / Parado',
    lines: [
      'Azul = etapa ativa agora.',
      'Verde = já passou.',
      'Cinza claro = parado ou aguardando.',
    ],
    target: '[data-tour="anatomy-legend"]',
    route: '/monitores/receptor',
  },
  {
    id: 'anatomy-stages',
    title: 'Etapas do Receptor',
    spotlightLabel: 'Etapas do fluxo',
    panelPlacement: 'top',
    lines: [
      'Cada plataforma é um passo: SEFAZ, consulta, temporária, fila e Arquivador.',
      'O documento “anda” da esquerda para a direita.',
    ],
    target: '[data-tour="anatomy-stages"]',
    route: '/monitores/receptor',
  },
  {
    id: 'anatomy-summary',
    title: 'Números do ciclo',
    spotlightLabel: 'NSU, temporária e fila',
    panelPlacement: 'top',
    lines: [
      'NSU = posição da busca na SEFAZ.',
      'Na temporária = lotes ainda neste serviço.',
      'Na fila = esperando o próximo serviço.',
      'Linhas de trabalho = quantas threads buscam na SEFAZ ao mesmo tempo (até 5).',
    ],
    target: '[data-tour="anatomy-summary"]',
    route: '/monitores/receptor',
  },
  {
    id: 'anatomy-validate',
    title: 'Validar a fila deste serviço',
    spotlightLabel: 'Validar',
    lines: [
      'Confere se a fila deste monitor está realmente vazia.',
      'Útil quando a tela mostra zero, mas você quer ter certeza.',
    ],
    target: '[data-tour="anatomy-validate"]',
    route: '/monitores/receptor',
  },
  {
    id: 'simulate-receptor',
    title: 'Como o Receptor trabalha',
    spotlightLabel: 'Simulação do monitor',
    panelPlacement: 'right',
    lines: [
      'Demonstração visual — não é CT-e real.',
      'Veja o “AGORA” andando: SEFAZ → consulta → temporária → fila → Arquivador.',
      'A esteira e os chips mostram o documento em movimento neste monitor.',
      'É o mesmo tipo de simulação da cadeia, agora por dentro do Receptor.',
    ],
    target: '[data-tour="anatomy"]',
    route: '/monitores/receptor',
    simulate: 'receptorFlow',
  },
  {
    id: 'nav-mais-informacoes',
    title: 'Próxima tela: Mais informações',
    spotlightLabel: 'Mais informações →',
    lines: [
      'Este botão abre a tela de detalhes do serviço.',
      'Lá ficam eventos, saúde do banco e avisos.',
      'Avançar leva para Mais informações.',
    ],
    target: '[data-tour="nav-mais-informacoes"]',
    route: '/monitores/receptor',
  },

  // —— Mais informações ——
  {
    id: 'details-header',
    title: 'Tela Mais informações',
    spotlightLabel: 'Cabeçalho',
    lines: [
      'Esta tela reúne o que aconteceu e a saúde do banco.',
      'É a mesma ideia em todos os monitores.',
    ],
    target: '[data-tour="details-header"]',
    route: '/monitores/receptor/mais-informacoes',
  },
  {
    id: 'details-feed',
    title: 'O que aconteceu agora',
    spotlightLabel: 'Passos recentes',
    lines: [
      'Lista os últimos passos do serviço.',
      'Do mais novo para o mais antigo.',
      'Ajuda a ver o que ele fez há pouco.',
    ],
    target: '[data-tour="details-feed"]',
    route: '/monitores/receptor/mais-informacoes',
  },
  {
    id: 'details-events',
    title: 'Últimos eventos do banco',
    spotlightLabel: 'Eventos do banco',
    lines: [
      'Cada evento tem um tipo: sucesso (verde), aviso (amarelo) ou erro (vermelho).',
      'Ex.: sucesso = lote gravado; aviso = fila acumulando; erro = falha na consulta.',
      'Em erro ou aviso mapeado, dá para abrir uma explicação mais clara.',
    ],
    target: '[data-tour="details-events"]',
    route: '/monitores/receptor/mais-informacoes',
  },
  {
    id: 'details-db-health',
    title: 'Saúde dos bancos',
    spotlightLabel: 'Saúde dos bancos',
    panelPlacement: 'top',
    lines: [
      'Diz se a conexão com o banco está ok.',
      'Os mini cards são as tabelas consultáveis — clique para ver até os últimos 1000 registros.',
      'Daqui também dá para ir a Tabelas e Configuração.',
    ],
    target: '[data-tour="details-db-health"]',
    route: '/monitores/receptor/mais-informacoes',
  },
  {
    id: 'details-alerts',
    title: 'Avisos e saúde',
    spotlightLabel: 'Avisos',
    panelPlacement: 'top',
    lines: [
      'Aqui entram todos os status: ok/info, atenção e alerta.',
      'Exemplos: SQL ok, processo ligado, fila/temporária vazia ou com backlog, batida atrasada.',
      'Verde/azul = informativo. Amarelo/laranja = precisa de olho.',
    ],
    target: '[data-tour="details-alerts"]',
    route: '/monitores/receptor/mais-informacoes',
  },
  {
    id: 'simulate-details',
    title: 'Como a tela Mais informações funciona',
    spotlightLabel: 'Simulação da tela',
    panelPlacement: 'right',
    lines: [
      'Demonstração automática — não é telemetria real.',
      'Os 4 painéis recebem exemplos: passos, eventos (sucesso/aviso/erro), tabelas e avisos.',
      'Em seguida o sistema abre sozinho o modal de um erro crítico (Ver detalhes).',
      'É o mesmo tipo de simulação da cadeia e do Receptor.',
    ],
    target: '[data-tour="details-header"]',
    route: '/monitores/receptor/mais-informacoes',
    simulate: 'detailsFlow',
  },
  {
    id: 'nav-tabelas',
    title: 'Próxima tela: Tabelas',
    spotlightLabel: 'Tabelas →',
    lines: [
      'Este link abre a lista de tabelas do banco.',
      'Avançar leva para Tabelas do banco.',
    ],
    target: '[data-tour="nav-tabelas"]',
    route: '/monitores/receptor/mais-informacoes',
  },

  // —— Tabelas ——
  {
    id: 'tables-hub',
    title: 'Tabelas do banco',
    spotlightLabel: 'Lista de tabelas',
    lines: [
      'Cada cartão é uma tabela vigiada.',
      'O status mostra se está ok ou com alerta.',
      'Clique em Ver dados para abrir o conteúdo.',
    ],
    target: '[data-tour="tables-hub"]',
    route: '/monitores/receptor/tabelas',
  },
  {
    id: 'nav-table-detail',
    title: 'Próxima tela: dados da tabela',
    spotlightLabel: 'Ver dados →',
    lines: [
      'Vamos abrir um exemplo de tabela.',
      'Avançar mostra os dados daquela tabela.',
    ],
    target: '[data-tour="nav-table-detail"]',
    route: '/monitores/receptor/tabelas',
  },
  {
    id: 'table-detail',
    title: 'Dados da tabela',
    spotlightLabel: 'Detalhe da tabela',
    lines: [
      'Aqui aparecem as linhas da tabela escolhida.',
      'É a visão “por dentro” do que os cartões resumem.',
      '← Tabelas volta para a lista.',
    ],
    target: '[data-tour="table-detail"]',
    route: '/monitores/receptor/tabelas/servico',
  },
  {
    id: 'simulate-tables',
    title: 'Como a tela Tabelas funciona',
    spotlightLabel: 'Simulação da tabela',
    panelPlacement: 'right',
    lines: [
      'Demonstração automática — não é telemetria real.',
      'A tabela Serviço (NSU) recebe uma linha de exemplo e eventos NSU/cStat.',
      'É o mesmo tipo de simulação da cadeia, do Receptor e de Mais informações.',
    ],
    target: '[data-tour="table-detail"]',
    route: '/monitores/receptor/tabelas/servico',
    simulate: 'tablesFlow',
  },
  {
    id: 'nav-threads',
    title: 'Próxima tela: Linhas de trabalho',
    spotlightLabel: 'Linhas de trabalho →',
    lines: [
      'Agora vamos ver as linhas que buscam na SEFAZ.',
      'Avançar abre Linhas de trabalho.',
    ],
    target: '[data-tour="nav-threads"]',
    route: '/monitores/receptor/tabelas/servico',
  },

  // —— Threads ——
  {
    id: 'threads-header',
    title: 'Linhas de trabalho',
    spotlightLabel: 'Título da tela',
    lines: [
      'Cada “linha” é um trabalhador buscando CT-e.',
      'O Receptor pode ter até 5 linhas ao mesmo tempo.',
    ],
    target: '[data-tour="threads-header"]',
    route: '/monitores/receptor/threads',
  },
  {
    id: 'threads-summary',
    title: 'Resumo das linhas',
    spotlightLabel: 'Resumo',
    lines: [
      'Mostra quantas estão buscando, paradas ou sem atividade recente.',
      'É um olhar rápido do time de linhas.',
    ],
    target: '[data-tour="threads-summary"]',
    route: '/monitores/receptor/threads',
  },
  {
    id: 'threads-cards',
    title: 'Cartões das linhas',
    spotlightLabel: 'Cartões das linhas',
    panelPlacement: 'top',
    lines: [
      'Cada cartão é uma linha: o que ela busca e se está ativa.',
      'Histórico → abre os eventos daquela linha.',
    ],
    target: '[data-tour="threads-cards"]',
    route: '/monitores/receptor/threads',
  },
  {
    id: 'simulate-threads',
    title: 'Como as Linhas de trabalho funcionam',
    spotlightLabel: 'Simulação das linhas',
    panelPlacement: 'right',
    lines: [
      'Demonstração automática — não é telemetria real.',
      'Os cartões recebem exemplos: buscando, parada, arquivo local e sem atividade.',
      'O resumo no topo conta quantas estão em cada situação.',
      'É o mesmo tipo de simulação das telas anteriores.',
    ],
    target: '[data-tour="threads-cards"]',
    route: '/monitores/receptor/threads',
    simulate: 'threadsFlow',
  },
  {
    id: 'nav-historico',
    title: 'Próxima tela: Histórico',
    spotlightLabel: 'Histórico →',
    lines: [
      'O Histórico mostra a linha do tempo completa.',
      'Avançar abre a tela de Histórico.',
    ],
    target: '[data-tour="nav-historico"]',
    route: '/monitores/receptor/threads',
  },

  // —— Logs ——
  {
    id: 'logs-header',
    title: 'Histórico',
    spotlightLabel: 'Título do Histórico',
    lines: [
      'Aqui fica a linha do tempo do que o serviço fez.',
      'Útil para entender o passado recente.',
    ],
    target: '[data-tour="logs-header"]',
    route: '/monitores/receptor/logs',
  },
  {
    id: 'logs-pause',
    title: 'Pausar ou retomar',
    spotlightLabel: 'Pausar / Retomar',
    lines: [
      'Pausar congela a lista para você ler com calma.',
      'Retomar volta a receber eventos novos.',
    ],
    target: '[data-tour="logs-pause"]',
    route: '/monitores/receptor/logs',
  },
  {
    id: 'logs-filters',
    title: 'Filtros',
    spotlightLabel: 'Filtros do histórico',
    lines: [
      'Dá para filtrar por tipo de evento e por linha de trabalho.',
      'Também dá para buscar por texto.',
    ],
    target: '[data-tour="logs-filters"]',
    route: '/monitores/receptor/logs',
  },
  {
    id: 'logs-timeline',
    title: 'Linha do tempo',
    spotlightLabel: 'Eventos',
    panelPlacement: 'top',
    lines: [
      'Cada bolinha é um evento.',
      'Leia de cima para baixo o que aconteceu.',
    ],
    target: '[data-tour="logs-timeline"]',
    route: '/monitores/receptor/logs',
  },
  {
    id: 'nav-config',
    title: 'Próxima tela: Configurações',
    spotlightLabel: 'Configurações →',
    lines: [
      'Por último no monitor: a tela de configurações (só leitura).',
      'Avançar abre Configurações.',
    ],
    target: '[data-tour="nav-config"]',
    route: '/monitores/receptor/logs',
  },

  // —— Config ——
  {
    id: 'config-table',
    title: 'Configurações',
    spotlightLabel: 'Tabela de configuração',
    lines: [
      'Lista as configurações ativas do serviço.',
      'O texto abaixo do título indica a origem (ex.: SQL DEV · sts_ativo=1).',
      'É só para consulta — não se altera por aqui.',
    ],
    target: '[data-tour="config-table"]',
    route: '/monitores/receptor/config',
  },
  {
    id: 'nav-back-chain',
    title: 'Voltar ao painel da cadeia',
    spotlightLabel: 'Voltar ao painel',
    lines: [
      'Com isso você já viu as telas do monitor.',
      'Os outros 5 serviços seguem o mesmo mapa de telas.',
      'Este link (Voltar ao painel) retorna à cadeia com os 6 serviços.',
      'Avançar segue para o menu Resgate CT-e.',
    ],
    target: '[data-tour="shell-back"]',
    route: '/monitores/receptor/config',
  },

  // —— Resgate + fim ——
  {
    id: 'resgate',
    title: 'Menu Resgate',
    spotlightLabel: 'Resgate CT-e',
    lines: [
      'No menu à esquerda fica o Resgate.',
      'Serve para buscar de novo um CT-e que faltou.',
      'Depois da busca, ele entra na fila da Carga.',
    ],
    target: '[data-tour="nav-resgate"]',
    route: '/resgate',
  },
  {
    id: 'end',
    title: 'Obrigado — dúvida e opinião',
    spotlightLabel: 'Agradecimento e suporte',
    lines: [
      'Obrigado por acompanhar esta apresentação do Orquestrador CT-e.',
      'Se algo ficou dúbio, ou se quiser enviar opinião/sugestão, fale com o suporte.',
      'Exemplo de contato (e-mail fictício): suporte.orquestrador.cte@assefaz.exemplo',
      'Clique em Finalizar para desligar a apresentação e voltar à tela ao vivo.',
    ],
    target: '[data-tour="overview"]',
    route: '/',
  },
];
