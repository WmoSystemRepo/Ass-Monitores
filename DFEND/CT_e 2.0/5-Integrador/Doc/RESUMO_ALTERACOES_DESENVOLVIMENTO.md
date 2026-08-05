# Cadeia CT-e — Guia de apresentação (Orquestrador · Receptor · Arquivador)

> Documento para apresentação: o que cada sistema faz, menus, cards e significado dos dados.  
> **Escopo:** Orquestrador → Receptor → Arquivador.  
> **Data:** 27/07/2026.

---

## Sumário

1. [Visão geral da cadeia](#1-visão-geral-da-cadeia)
2. [Orquestrador](#2-orquestrador)
3. [Receptor](#3-receptor)
4. [Arquivador](#4-arquivador)
5. [Glossário comum](#5-glossário-comum)
6. [Roteiro sugerido](#6-roteiro-sugerido)
7. [Anexo — alterações DEV do Integrador](#7-anexo--alterações-dev-do-integrador)

---

## 1. Visão geral da cadeia

### Pipeline fiscal

```text
SEFAZ → Receptor → Arquivador → Sintetizador → Analisador → Integrador → Carga
```

| Sistema        | Papel em uma frase                                      | CodServico |
|----------------|---------------------------------------------------------|------------|
| **Orquestrador** | Painel de controle: liga/desliga e observa a cadeia   | —          |
| **Receptor**     | Porta de entrada: busca CT-e na SEFAZ                 | **2**      |
| **Arquivador**   | Distribuidor: recebe do Receptor e faz fan-out        | **3**      |

### Dois planos (importante na apresentação)

| Plano | O que é | Como viaja |
|-------|---------|------------|
| **Controle** | Ligar, desligar, ver saúde | HTTP Monitor + API Key |
| **Dados** | XML / NSU / lotes CT-e | SQL Server Service Broker + tabelas temp |

> Orquestrador **não processa CT-e**. Receptor e Arquivador **processam** via Windows Service.

---

## 2. Orquestrador

### 2.1 O que é / o que faz

- Pacote: `0-Orquestrador`
- UI: `http://localhost:4220` · API: `:5000`
- **Não** consulta SEFAZ e **não** escreve em filas fiscais
- Agrega telemetria dos Monitors e executa **cascata** Ligar / Desligar
- Clique em um estágio abre o Monitor daquele sistema (API + Angular)

### 2.2 Menu

| Menu | Rota | O que é |
|------|------|---------|
| **Monitor** | `/` | **Única tela real** — visão da cadeia inteira |

> Threads, Histórico, Config etc. existem só como stubs do template; **não estão no menu**.

### 2.3 Shell (barra lateral)

| Elemento | Significado |
|----------|-------------|
| **Orquestrador CT-e** | Nome do produto |
| **Ambiente de testes** | Badge de ambiente DEV |
| **Monitor · Visão da cadeia** | Único item de navegação |
| Ponto live + **Orquestrador online/offline** | Poll do snapshot OK ou falhou |
| **N sistema(s) ligado(s)** | Quantos estágios com status `running` |

### 2.4 Tela Monitor — cabeçalho

| Elemento | Significado / ação |
|----------|-------------------|
| Título **Orquestrador cadeia CT-e** | Identidade da tela |
| Subtítulo | “Ligue ou desligue a cadeia e acompanhe os 6 sistemas em tempo real.” |
| Badge **Orquestrador online/offline · HH:mm:ss** | Conexão com a API; horário do último poll |
| **Ligar cadeia CT-e** | Confirma → sobe stacks → inicia serviços na ordem Receptor → … → Carga |
| **Desligar cadeia** | Confirma → para na **ordem inversa** |

### 2.5 Faixas e KPIs

| Bloco | O que mostra | Significado |
|-------|--------------|-------------|
| Banner de cascata | Ligando / Em execução / Desligando / mensagem | Fase atual da cascata |
| Erro de boot (rosa) | Falha ao conectar na API | Orquestrador API inacessível |
| Mensagem de ação (violeta) | Resultado de Ligar/Desligar/abrir sistema | Feedback da última ação |
| **Orquestrador** online/offline | Saúde do poll | Controle vivo? |
| **Sistemas ligados** | Contagem | Quantos workers em `running` |
| **Cascata** | Texto da cascata | Mensagem operacional ao vivo |
| Alertas (até 3) | Monitor offline, auth, lastError… | Problemas agregados da cadeia |

### 2.6 Poster — Fluxo da cadeia CT-e

#### Card “Último lote” (vindo do Receptor)

| Campo | Significado |
|-------|-------------|
| **NSU** | Número sequencial inicial do último lote |
| **→ nsuFinal** | NSU final do lote (se houver) |
| **N CT-e** | Quantidade de documentos no lote |
| **HH:mm:ss** | Horário do lote |

#### Esteira CT-e

| Estado | Significado |
|--------|-------------|
| Chips “CT-e” animados | Há sistema ligado **e** tráfego (temp, fila ou documentos recentes) |
| “Sem CT-e em trânsito…” | Sem sinal de movimento |

#### Cards dos 6 estágios (clicáveis)

Ordem: **R → A → S → An → I → C**

| Símbolo | Sistema       | Métrica no card (`metricPill`) | O que a métrica significa |
|---------|---------------|--------------------------------|---------------------------|
| **R**   | Receptor      | `NSU {valor}`                  | Último NSU principal da distribuição |
| **A**   | Arquivador    | `fila {N}`                     | Profundidade da fila de entrada |
| **S**   | Sintetizador  | `fila {N}`                     | Profundidade da fila do Sintetizador |
| **An**  | Analisador    | `fila {N}`                     | Profundidade da fila do Analisador |
| **I**   | Integrador    | `staging {N}`                  | Profundidade do staging / Netezza |
| **C**   | Carga         | `fila {N}`                     | Profundidade da fila da Carga |

**Em cada card de estágio**

| Elemento | Significado |
|----------|-------------|
| Símbolo + nome | Qual sistema |
| Tag de status | Desabilitado / Offline / Ligando / Ligado / Desligando / Parado / Falha / Desconhecido |
| Linha de métrica | KPI específico (tabela acima) |
| Texto de dica (`hint`) | Explicação da métrica ou do motivo de offline |
| Badge **AGORA** | `Executar=1` **e** efetivamente rodando |
| Clique | `ensure-open` → abre UI do Monitor daquele sistema (não inicia o worker sozinho) |

#### Barra resumo do poster

| KPI | Significado |
|-----|-------------|
| **Sistemas ligados** | Contagem de `running` |
| **Fase** | Parada / Ligando / Em execução / Desligando |
| **Cascata** | Mensagem atual da cascata |

### 2.7 Ações — o que cada botão faz

| Ação | Efeito |
|------|--------|
| **Ligar cadeia** | Garante API+Angular dos habilitados → `service/start` na ordem de dependência |
| **Desligar cadeia** | `service/stop` na ordem inversa |
| **Clique no estágio** | Sobe stack daquele sistema (se preciso) e abre o frontend |
| Poll automático (1s) | Mantém snapshot ao vivo (não há botão Refresh) |

### 2.8 Status oficiais (Orquestrador / Monitor)

| Valor API | Label na UI | Significado |
|-----------|-------------|-------------|
| `disabled` | Desabilitado | Fora do registro / Enabled=false |
| `offline` | Offline | Monitor sem resposta |
| `starting` | Ligando… | Subindo |
| `running` | Ligado | Processo em execução |
| `stopping` | Desligando… | Parando |
| `stopped` | Parado | Parado de propósito |
| `failed` | Falha | Erro / API key inválida |
| `unknown` | Desconhecido | Estado não mapeado |

### 2.9 Mensagem-chave

> Orquestrador = **painel de controle**. Clique nos cards para abrir Receptor, Arquivador etc.

---

## 3. Receptor

### 3.1 O que é / o que faz

- Pacote: `1-Receptor`
- Windows Service: **DFEND_CTe_Receptor** · `CodServico = 2`
- Monitor UI: `:4200` · API: `:5010`
- Consulta distribuição SEFAZ (`cteDistSVD`), grava lote em **temporária**, avisa o **Arquivador** via Service Broker
- Só processa se **`Executar = 1`**

```text
SEFAZ → Consulta NSU → Temporária → Fila → Arquivador
```

### 3.2 Menu (sidebar)

| Menu | Hint | Rota | Para que serve |
|------|------|------|----------------|
| **Monitor** | Visão operacional | `/` | Fluxo, saúde, Ligar/Desligar, cards |
| **Threads** | Linhas de trabalho | `/threads` | As 5 linhas de busca (T1–T5) |
| **Histórico** | O que aconteceu | `/logs` | Timeline de eventos SQL |
| **Tabelas** | Banco em tempo real | `/tabelas` | Hub das 5 vigilancias |
| **Configurações** | Somente leitura | `/config` | Todas as chaves SQL ativas (leitura) |

**Fora do menu:** `Mais informações` (`/mais-informacoes`) — link no poster do fluxo.

**Rodapé da sidebar**

| Campo | Significado |
|-------|-------------|
| **Monitor online/offline** | SignalR do painel (não é o Windows Service) |
| **Receptor · {status}** | Ligado / Recepção pausada / Desligado / Não disponível |

---

### 3.3 Tela Monitor (`/`)

#### Cabeçalho

| Elemento | Significado / ação |
|----------|-------------------|
| **Monitor do Receptor CT-e** | Home operacional |
| Badge Monitor online + horário | Push SignalR vivo |
| **Ligar Receptor CT-e** | Sobe DevHost + `Executar=1` |
| **Desligar** | Confirma → mata host + `Executar=0` |

#### Cronômetros (só com Receptor ligado)

| Cronômetro | Significado |
|------------|-------------|
| **próxima consulta** ↓ MM:SS | Contagem até a próxima chamada SEFAZ (`Intervalo`) |
| **sem CT-e novo** / **achou CT-e** ↑ | Tempo desde o último lote novo na temp |

#### Faixa de saúde

| Campo | Valores | Significado |
|-------|---------|-------------|
| **Receptor** | Ligado / Recepção pausada / Desligado… | Processo + Executar |
| **Recepção** | Ativa / Ociosa | `Executar=1` / `0` |
| **Banco** | Conectado / Sem conexão | Ping SQL |
| **Servidor** | hostname · última batida | Máquina + idade do `dtc_execucao` |

#### Cards de vigilância (5 — clicáveis)

Cada card: **status** (OK / Atenção / Crítico) · valor principal · **Idade** · **Consulta ms** · **Ver dados →**

| Card | Valor principal | O que significa | Quando fica crítico/atenção |
|------|-----------------|-----------------|----------------------------|
| **Serviço (NSU)** | Posição da busca (NSU) | Onde a consulta SEFAZ está | Crítico: batida antiga com Executar=1 · Atenção: Executar=0 |
| **Configuração** | Recepção ligada/desligada | Espelho do Executar | Atenção se desligada |
| **Temporária** | N aguardando (± erros) | Lotes na temp antes do Arquivador | Crítico se backlog ≥ 100 · Atenção se há erros |
| **Log** | Horário do último evento | Silêncio ou atividade recente | Crítico: silêncio com recepção ligada |
| **Fila Arquivador** | Profundidade da fila | Mensagens no Service Broker para o Arquivador | Crítico ≥ 100 · Atenção se crescendo |

#### Poster — Fluxo do Receptor CT-e

| Elemento | Significado |
|----------|-------------|
| **Último lote** | NSU → NSU final · qtd CT-e · horário |
| **Mais informações →** | Abre tela de detalhes |
| Esteira CT-e | Move só com trânsito real de documento |
| Badge **AGORA** | Etapa atual do ciclo (não é “tem backlog”) |

**5 estágios do fluxo**

| Estágio | Tag | Contagem | Significado |
|---------|-----|----------|-------------|
| **SEFAZ** | Origem | Último NSU | Origem fiscal dos CT-e |
| **Consulta** | Por NSU | NSU atual | Chamada SOAP de distribuição |
| **Temporária** | Gravação | N aguardando | Lote gravado aguardando Arquivador |
| **Fila** | Aviso ao Arquivador | N na fila | Service Broker |
| **Arquivador** | Destino | — | Próximo serviço da cadeia |

**Barra resumo**

| Campo | Significado |
|-------|-------------|
| **NSU** | NSU principal oficial |
| **Na temporária** | Backlog da temp |
| **Na fila** | Depth do Broker |
| **Linhas de trabalho** | Qtd de threads configuradas → link `/threads` |

---

### 3.4 Tela Threads (`/threads`)

Cinco linhas de trabalho:

| Linha | Papel | O que busca | Onde guarda o NSU |
|-------|-------|-------------|-------------------|
| **T1** (Principal) | Busca principal SEFAZ | CT-e e DF-e | Banco (serviço principal) |
| **T2** (Auxiliar) | Complementar (só se contador ≠ 0) | CT-e e DF-e | `NSUAux` |
| **T3** (Autorização) | Docs de autorização | Autorizações | `NSUAuxAut` |
| **T4** (Destinatário) | Por destinatário | Documentos do destinatário | `NSUAuxDest` |
| **T5** (Arquivo) | NSU em arquivo | CT-e e DF-e | Arquivo `NSU.txt` |

**Campos de cada card**

| Campo | Significado |
|-------|-------------|
| Chip de status | Buscando / Não busca agora / Arquivo local / Sem atividade recente / Aguardando recepção |
| O que busca | Tipo de documento (`indDFe`) |
| Posição da busca | NSU atual + variação (↑/↓/0) |
| Onde guarda | Fonte do ponteiro NSU |
| Última atividade | Idade + cStat opcional |
| Histórico → | Filtra logs daquela linha |

---

### 3.5 Tela Histórico (`/logs`)

| Controle / dado | Significado |
|-----------------|-------------|
| **Pausar / Retomar online** | Congela ou libera a timeline |
| Filtros Todos / Sucesso / Erros / Avisos / Outros | Por tipo de evento |
| Linha 1–5 / Todas | Filtra por thread |
| Buscar | Texto na mensagem |
| Item da timeline | Tipo · data/hora · `#seqLog` · Linha · **cStat SEFAZ** · resumo |

**cStat mais citados**

| cStat | Significado |
|-------|-------------|
| **118** | Documentos recebidos (sucesso) |
| **117** | Sem DF-e novo — NSU sincronizado |
| **146** | Salto / gap de NSU |
| **730 / 992** | NSU antigo — avança +1 |
| **108** | SEFAZ em manutenção |
| **285** | Problema de certificado |

---

### 3.6 Tela Tabelas (`/tabelas` + detalhes)

Hub = mesmos 5 cards do Monitor. Detalhes (atualização ~2s):

| Rota | Colunas / painéis principais | Significado |
|------|------------------------------|-------------|
| `/tabelas/servico` | Serviço · Servidor · NSU · Última batida · Atualização | Heartbeat e posição oficial |
| `/tabelas/configuracao` | Chave · Valor · Atualizado | Params do processo (Executar em destaque) |
| `/tabelas/temporaria` | NSU · NSU final · Qtd · Datas · Erro | Lotes na temp (**sem XML** na tela) |
| `/tabelas/log` | Hora · Linha · cStat · resumo | Eventos da sessão |
| `/tabelas/fila` | Na fila agora · Limite (100) · Tendência | Depth + tendência do Broker |

---

### 3.7 Tela Mais informações (`/mais-informacoes`)

| Bloco | Conteúdo |
|-------|----------|
| Passos do Receptor · online | Debug live + SQL (alimenta AGORA) |
| Últimos eventos SQL | Eventos classificados |
| Parâmetros e lotes | NSU · Intervalo · Pacote completo · Rebusca · Linhas + lotes recentes |
| Avisos do Monitor | Códigos de alerta por severidade |

---

### 3.8 Tela Configurações (`/config`)

- Tabela **somente leitura** de todas as chaves SQL ativas (`CodServico = 2`)
- Monitor **só grava** `Executar` (via Ligar/Desligar)

**Parâmetros importantes**

| Chave | Significado |
|-------|-------------|
| **Executar** | 1 = recebe · 0 = ocioso |
| **Intervalo** | Espera entre ciclos SEFAZ |
| **Threads** | Tamanho do pool (papéis T1–T5 fixos) |
| **PacoteCompleto** | Pode recusar pacote incompleto |
| **ReBuscar** | Reset mensal do NSU auxiliar (T2) |
| **NSUAux / NSUAuxAut / NSUAuxDest** | Ponteiros auxiliares (`"0"` = idle) |
| **WSURL / WSTimeOut / WSVersao / WSTipoAmbiente** | Endpoint e ambiente SOAP |
| **LogBanco / LogEvento / LogCompleto** | Verbosity de log |

### 3.9 Mensagem-chave

> Receptor = **porta de entrada**. Busca na SEFAZ, guarda na temp e enfileira para o Arquivador.

---

## 4. Arquivador

### 4.1 O que é / o que faz

- Pacote: `2-Arquivador`
- Windows Service: **DFEND_CTe_Arquivador** · `CodServico = 3`
- Monitor UI: `:4210` · API: `:5020`
- Consome `fila_alvo_cte_arquivador`, lê temp, faz **fan-out** e exclui temp
- Só processa se **`Executar = 1`**

```text
Fila Arquivador → Temp Recepção → Sintetizador
                                 → Analisador
                                 → Integrador
                    → Exclui temp (após fan-out OK)
```

### 4.2 Menu (sidebar)

| Menu | Hint | Rota | Para que serve |
|------|------|------|----------------|
| **Monitor** | Visão operacional | `/` | Fluxo, saúde, Ligar/Desligar, 8 cards |
| **Threads** | Linhas de trabalho | `/threads` | Pool Principal + Workers |
| **Histórico** | O que aconteceu | `/logs` | Timeline SQL |
| **Tabelas** | Banco em tempo real | `/tabelas` | Hub das 8 vigilancias |
| **Configurações** | Somente leitura | `/config` | Chaves SQL ativas |

**Fora do menu:** `Mais informações` · detalhes `/tabelas/:key`

**Rodapé:** Monitor online/offline · **Arquivador · {status}**

---

### 4.3 Tela Monitor (`/`)

#### Cabeçalho

| Elemento | Significado / ação |
|----------|-------------------|
| **Monitor do Arquivador CT-e** | Home operacional |
| Badge Monitor online + horário | SignalR vivo |
| **Ligar Arquivador CT-e** | DevHost + `Executar=1` (cod 3) |
| **Desligar** | `Executar=0` + encerra host |

#### Cronômetros (só ligado)

| Cronômetro | Significado |
|------------|-------------|
| **próx. ciclo** | Contagem até o próximo ciclo (`Intervalo`) |
| **arquivando** / **sem arquivamento** | Há drenagem/atividade vs tempo sem lote |

#### Faixa de saúde

| Campo | Significado |
|-------|-------------|
| **Arquivador** | Ligado / pausado / desligado |
| **Ciclo** | **Ativo** (`Executar=1`) ou **Ocioso** |
| **Banco** | Conectado / Sem conexão |
| **Servidor** | hostname + idade da batida |

#### Cards de vigilância (8 — clicáveis)

| Card | Valor típico | Significado |
|------|--------------|-------------|
| **Serviço (NSU)** | Posição da busca: {NSU} | Heartbeat / NSU do serviço 3 |
| **Configuração** | Arquivamento ligado/desligado | Espelho do Executar |
| **Temporária** | Na temporária: N [· erros] | Lotes ainda na temp de recepção |
| **Log** | Último evento · HH:mm:ss | Atividade recente ou silêncio |
| **Fila entrada (Arquivador)** | Na fila: N | Fila que o Receptor alimenta |
| **Fila Sintetizador** | Na fila: N | Fan-out destino 1 |
| **Fila Analisador** | Na fila: N | Fan-out destino 2 |
| **Fila Integrador** | Na fila: N | Fan-out destino 3 (alimenta o Integrador) |

Status dos cards: **OK** · **Atenção** · **Crítico** (ex.: fila ≥ 100, batida antiga, erros na temp).

#### Poster — Fluxo do Arquivador CT-e

| Elemento | Significado |
|----------|-------------|
| **Último lote** | NSU → NSU final · qtd · horário |
| Esteira | NSU em trânsito vs “Sem NSU em trânsito…” |
| **AGORA** | Estágio atual do ciclo |

**5 estágios**

| Estágio | Tag | Contagem | Ação no Windows Service |
|---------|-----|----------|-------------------------|
| **Fila Arquivador** | Entrada | N na fila | `RECEIVE` da fila de entrada |
| **Temp Recepção** | Leitura | N aguardando | Lê `tmp_documento…` |
| **Sintetizador** | Encaminha | depth destino | Envia fila Sintetizador |
| **Analisador** | Análise | depth destino | Envia fila Analisador |
| **Integrador** | Destino | depth destino | Envia fila Integrador + exclui temp |

**Barra resumo:** NSU · Na temporária · Na fila · Linhas de trabalho → `/threads`

---

### 4.4 Tela Threads (`/threads`)

| Campo | Significado |
|-------|-------------|
| T1 **Principal (pool)** | Ciclo principal de arquivamento |
| Workers T2… | Workers paralelos do pool |
| Chip | Processando / Não processa agora / Arquivo local / Sem atividade / Aguardando Arquivador |
| Posição | NSU atual + Δ |
| Histórico → | Logs daquela linha |

---

### 4.5 Tela Histórico (`/logs`)

Igual ideia do Receptor: Pausar/Retomar · filtros Sucesso/Erros/Avisos · Linha · Buscar · timeline com cStat.

---

### 4.6 Tela Tabelas

Hub = 8 cards. Detalhes principais:

| Chave | O que detalha |
|-------|---------------|
| `servico` | Heartbeat, NSU, servidor |
| `configuracao` | Chaves do processo (Executar em destaque) |
| `temporaria` | Lotes NSU/qtd/erro (sem XML) |
| `log` | Eventos da sessão |
| Filas (`fila_entrada`, sintetizador, analisador, integrador) | Depth · limite 100 · tendência |

---

### 4.7 Tela Mais informações

| Bloco | Conteúdo |
|-------|----------|
| Passos do Arquivador · online | Debug + SQL (AGORA) |
| Últimos eventos SQL | Classificados |
| Parâmetros e lotes | NSU · Intervalo · Pacote · Rebusca · Linhas + lotes |
| Avisos do Monitor | Alertas (BD_DOWN, FILA_ALTA, NSU_SEM_TEMP, FANOUT_PARCIAL…) |

---

### 4.8 Tela Configurações (`/config`)

Somente leitura (`CodServico = 3`). Monitor só escreve **Executar**.

| Chave | Significado |
|-------|-------------|
| **Executar** | 1 = arquiva · 0 = ocioso |
| **Intervalo** | Sleep entre ciclos |
| **Threads** | Tamanho do pool |
| **ReEnviarFila** | Reenvio horário de pendências na temp (thread 1) |
| **QtdeMaxFila** | Teto da fila para permitir reenvio |
| **LogBanco / LogEvento / LogCompleto** | Logs |

### 4.9 Regras rápidas (falar na apresentação)

| Regra | Significado |
|-------|-------------|
| Um NSU por consumo | Não processa lote misturado |
| Temp obrigatória | Fila sem temp = erro (`NSU_SEM_TEMP`) |
| Fan-out completo | Só apaga temp depois dos 3 destinos |
| Fan-out parcial | Alerta `FANOUT_PARCIAL` |
| Erro | Marca temp; não reenfileira na hora |
| Reenvio | Thread 1, horário, se fila &lt; `QtdeMaxFila` |

### 4.10 Mensagem-chave

> Arquivador = **distribuidor**. Tira da fila do Receptor e espalha para Sintetizador, Analisador e Integrador.

---

## 5. Glossário comum

| Termo | Significado |
|-------|-------------|
| **NSU** | Número Sequencial Único da distribuição SEFAZ |
| **CT-e** | Conhecimento de Transporte Eletrônico |
| **DF-e** | Documento fiscal eletrônico relacionado |
| **Executar** | Interruptor soft no SQL (1=processa · 0=ocioso) |
| **CodServico** | ID do serviço nas tabelas compartilhadas |
| **Temporária / Temp** | Tabela staging do lote antes do próximo passo |
| **Fila / depth** | Qtd de mensagens no Service Broker |
| **Fan-out** | Enviar o mesmo lote a vários destinos |
| **Monitor online** | Painel recebendo SignalR (≠ serviço fiscal ligado) |
| **AGORA** | Etapa atual destacada no fluxo |
| **Idade** | Segundos desde a última mudança do indicador |
| **Consulta ms** | Latência do SELECT daquela vigilância |
| **cStat** | Código de retorno SEFAZ / evento |
| **Ligar / Desligar** | Sobe/desce processo + liga/desliga Executar |
| **Cascata** | Ligar/Desligar vários sistemas em ordem |
| **DevHost** | Host POC que roda o serviço sem InstallUtil |
| **FilaAlta** | Limite de alerta de fila (padrão **100**) |

---

## 6. Roteiro sugerido

1. **Cadeia em 10s** — Receptor entra, Arquivador distribui, Orquestrador controla  
2. **Orquestrador** — Ligar/Desligar · 6 cards · clique abre Monitor · AGORA  
3. **Receptor** — menu · Ligar · fluxo SEFAZ→Fila · cards NSU/Temp/Fila · Threads T1–T5 · cStat 118/117  
4. **Arquivador** — menu · 8 cards (entrada + 3 destinos) · fan-out · exclui temp  
5. **Fechamento** — controle (HTTP) ≠ dados (Service Broker)

---

## 7. Anexo — alterações DEV do Integrador

> Espelha o padrão histórico deste arquivo. **Estado em 25/07/2026:** POC DEV aplicada.

### Certificado digital — não se aplica

O **DFEND_CTe_Integrador** não consulta SEFAZ via SOAP e não tem `CertificadoDigital` no `App.config`.

### Login SQL — aplicado

`Threads.cs` resolve quatro conexões: `BDCTeSintetico`, `BDCTeAnalitico`, `BDNFeDefinitivo`, `BDStaging`.  
Em DEV: `Integrated Security=SSPI`. Homolog/Prod: strings criptografadas.

### Erro visível — aplicado

`GravarErro` → `DFEND_CTe_Integrador_erro.txt` (Desktop / Temp / BaseDirectory).

### Congelamento / Monitor

Monitor em `Frontend/` · `Integrador.Api/` · `tools/Integrador.DevHost/`.  
Demais do Windows Service permanece congelado além das alterações deste anexo.

| Item | Valor |
|------|--------|
| CodServico | **7** |
| Fila entrada | `fila_alvo_cte_integrador` |
| Destinos | Netezza · DocVinculado · FICS |
| Monitor API / UI | `:5050` / `:4250` |
