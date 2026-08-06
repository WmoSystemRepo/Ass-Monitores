using System;

namespace DFe
{
    public class Constante
    {
        #region " Constantes "

        public const string CodOrgaoAutorizador = "1";
        public const string CodUFBA = "29";
        public const string SigUFBA = "BA";
        public const string SiglaNFe = "NF-e";
        public const string SiglaNFCe = "NFC-e";
        public const string SiglaNFCom = "NFCom";
        public const string SiglaNF3e = "NF3-e";
        public const string SiglaMDFe = "MDF-e";
        public const string SiglaCTe = "CT-e";
        public const string SiglaCTe1 = "CT-e Rodo";
        public const string SiglaCTe2 = "CT-e Aéreo";
        public const string SiglaCTe3 = "CT-e Aqua";
        public const string SiglaCTe4 = "CT-e Ferro";
        public const string SiglaCTe5 = "CT-e Duto";
        public const string SiglaCTe6 = "CT-e Multi";
        public const string SiglaCTeOS = "CT-e OS";
        public const string SiglaBPe = "BP-e";
        public const string SiglaGTVe = "GTV-e";
        public const string ModeloNFe = "55";
        public const string ModeloCTe = "57";
        public const string ModeloCTe1 = "5701";
        public const string ModeloCTe2 = "5702";
        public const string ModeloCTe3 = "5703";
        public const string ModeloCTe4 = "5704";
        public const string ModeloCTe5 = "5705";
        public const string ModeloCTe6 = "5706";
        public const string ModeloMDFe = "58";
        public const string ModeloNFCom = "62";
        public const string ModeloBPe = "63";
        public const string ModeloNFCe = "65";
        public const string ModeloNF3e = "66";
        public const string ModeloGTVe = "64";
        public const string ModeloCTeOS = "67";
        public const string FusoHorario = "-03:00";
        public const string MascaraCNPJ = "00,000,000/0000-00";
        public const string MascaraCPF = "000,000,000-00";
        public const string VersaoDados100 = "1.00";
        public const string VersaoDados200 = "2.00";
        public const string VersaoDados300 = "3.00";
        public const string VersaoDados310 = "3.10";
        public const string VersaoDados400 = "4.00";
        public const string VersaoDFEND = "SFZBA_DFEND";
        public const string VersaoDFENW = "SFZBA_DFENW";
        public const string CabecalhoXML = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
        public const string CabecalhoXML2 = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";
        public const string CabecalhoXML3 = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
        public const string CabecalhoXML4 = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>";
        public const string CabecalhoXML5 = "<?xml version = \"1.0\" encoding=\"UTF-8\" ?>";
        public const string CabecalhoXML6 = "<?xml version = \"1.0\" encoding=\"utf-8\" ?>";
        public const string CabecalhoXML7 = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"no\"?>";
        public const string CabecalhoXML8 = "<?xml version=\"1.0\" encoding=\"utf-8\" standalone=\"no\"?>";
        public const string CabecalhoXML9 = "<?xml version = \"1.0\" encoding=\"UTF-8\" standalone=\"no\"?>";
        public const string CabecalhoXML10 = "<?xml version = \"1.0\" encoding=\"utf-8\" standalone=\"no\"?>";
        public const string NamespacePadraoNFe = "http://www.portalfiscal.inf.br/nfe";
        public const string NamespacePadraoNFCom = "http://www.portalfiscal.inf.br/nfcom";
        public const string NamespacePadraoNF3e = "http://www.portalfiscal.inf.br/nf3e";
        public const string NamespacePadraoMDFe = "http://www.portalfiscal.inf.br/mdfe";
        public const string NamespacePadraoCTe = "http://www.portalfiscal.inf.br/cte";
        public const string NamespacePadraoBPe = "http://www.portalfiscal.inf.br/bpe";
        public const string NamespaceXSD = "http://www.w3.org/2001/XMLSchema";
        public const string NamespaceXSI = "http://www.w3.org/2001/XMLSchema-instance";

        #endregion

        #region " Mensagens "

        public const string MsgServicoIniciado = "Serviço iniciado com sucesso: ";
        public const string MsgServicoDesativado = "Serviço desativado com sucesso: ";
        public const string MsgProcessoIniciado = "Processo iniciado com sucesso";
        public const string MsgProcessoNaoIniciado = "Processo não iniciado. Configuração desabilitada.";
        public const string MsgLogInserido = "Log de teste inserido com sucesso";
        public const string MsgChaveNaoInformada = "Deve ser informado alguma Chave";
        public const string MsgQueryNaoInformada = "Deve ser informado alguma Query";
        public const string MsgQueryExecutada = "Query executada com sucesso";
        public const string MsgConfigObtidaArquivo = "Configuração obtida no arquivo config com sucesso";
        public const string MsgConfigObtidaBanco = "Configuração obtida no banco de dados com sucesso";
        public const string MsgConfigAtualizada = "Configuração atualizada com sucesso. Descrição: ";
        public const string MsgConfigInserida = "Configuração inserida com sucesso. Descrição: ";
        public const string MsgConfigDesabilitadaIntegracao = "Configuração desabilitada para envio à Integração";
        public const string MsgConfigDesabilitadaBDSintetico = "Configuração desabilitada para envio ao BD Sintético";
        public const string MsgConfigDesabilitadaBDAnalitico = "Configuração desabilitada para envio ao BD Analítico";
        public const string MsgConfigDesabilitadaBDHistorico = "Configuração desabilitada para envio ao BD Historico";
        public const string MsgNSUObtidoBanco = "NSU obtido do banco. Nº: ";
        public const string MsgNSUMovObtidoBanco = "NSU do Movimento obtido do banco. Nº: ";
        public const string MsgNSUObtidoArquivo = "NSU obtido do arquivo. Nº: ";
        public const string MsgNSUFuturoCalculado = "NSU futuro calculado. Nº: ";
        public const string MsgNSUAtualizadoBanco = "NSU atualizado no banco. NSU: ";
        public const string MsgNSUMovAtualizadoBanco = "NSU do Movimento atualizado no banco. NSU: 1";
        public const string MsgNSUAtualizadoArquivo = "NSU atualizado no arquivo. NSU: ";
        public const string MsgNSUFuturoAtualizado = "NSU futuro atualizado. NSU: ";
        public const string MsgNSURetornadoInvalido = "NSU retornado pelo WS inválido. NSU: ";
        public const string MsgNSURetornadoOK = "NSU retornado pelo WS OK. NSU: ";
        public const string MsgWSConfigurado = "WebService configurado com sucesso";
        public const string MsgWSComunicado = "WebService comunicado com sucesso: ";
        public const string MsgWSFiltroConfigurado = "Filtro do WebService configurado com sucesso";
        public const string MsgWSDadoNaoObtido = "Nenhum resultado encontrado. Retorno: ";
        public const string MsgWSRetornoNaoEsperado = "Retorno do WebService não esperado: ";
        public const string MsgDadoObtidoBanco = "Dado obtido no banco. Chave: ";
        public const string MsgDadoNaoObtidoBanco = "Nenhum dado obtido no banco. Chave: ";
        public const string MsgChaveInseridaFila = "Chave inserida na fila. Chave: ";
        public const string MsgChaveRetiradaFila = "Chave retirada da fila. Chave: ";
        public const string MsgLoteInseridoFila = "Chave do Lote inserida na fila. NSU: ";
        public const string MsgLoteInseridoBanco = "Lote inserido no banco. NSU: ";
        public const string MsgLoteObtidoBanco = "Lote obtido no banco. NSU: ";
        public const string MsgLoteElementoNaoEsperado = "Elemento do lote não esperado. Esquema:  ";
        public const string MsgLoteStatusNaoEsperado = "Status do lote não esperado. cStat:  ";
        public const string MsgLoteJaExistente = "Inserção de Lote já existente. NSU: ";
        public const string MsgLoteFuroNSU = "Lote recepcionado com furos de NSU. NSU: ";
        public const string MsgLoteErroNSUMenor = "Lote recepcionado com valor de último NSU menor do que o NSU pesquisado. Tag ultNSURet / NSU: ";
        public const string MsgLoteErroUltimoNSU = "Lote recepcionado com valor de último NSU diferente do último NSU real. Tag ultNSURet / NSU: ";
        public const string MsgDocAutorizacaoInseridoFila = "Chave do Documento de Autorização inserida na fila. NSU: ";
        public const string MsgDocAutorizacaoInseridoBanco = "Documento de Autorização inserido no banco. NSU: ";
        public const string MsgDocAutorizacaoAtualizadoBanco = "Documento de Autorização atualizado no banco. NSU: ";
        public const string MsgDocAutorizacaoExcluidoBanco = "Documento de Autorização excluído do banco. NSU: ";
        public const string MsgDocAutorizacaoObtidoBanco = "Documento de Autorização obtido do banco. Protocolo: ";
        public const string MsgDocEventoInseridoFila = "Chave do Documento de Evento inserida na fila. NSU: ";
        public const string MsgDocEventoInseridoBanco = "Documento de Evento inserido no banco. NSU: ";
        public const string MsgDocEventoAtualizadoBanco = "Documento de Evento atualizado no banco. NSU: ";
        public const string MsgDocEventoExcluidoBanco = "Documento de Evento excluído do banco. NSU: ";
        public const string MsgDocEventoObtidoBanco = "Documento de Evento obtido do banco. Protocolo: ";
        public const string MsgDocInutilizacaoInseridoFila = "Chave do Documento de Inutilização inserida na fila. NSU: ";
        public const string MsgDocInutilizacaoInseridoBanco = "Documento de Inutilização inserido no banco. NSU: ";
        public const string MsgDocInutilizacaoAtualizadoBanco = "Documento de Inutilização atualizado no banco. NSU: ";
        public const string MsgDocInutilizacaoExcluidoBanco = "Documento de Inutilização excluído do banco. NSU: ";
        public const string MsgDocInutilizacaoObtidoBanco = "Documento de Inutilização obtido do banco. Protocolo: ";
        public const string MsgDocSubItemInseridoBanco = "Documento do Item inserido no banco. Nº do Item: ";
        public const string MsgDocSubItemAtualizadoBanco = "Documento do Item atualizado no banco. Nº do Item: ";
        public const string MsgDocSubItemJaExistente = "Inserção de Documento do Item já existente. Nº do Item: ";
        public const string MsgDocJaExistente = "Inserção de Documento já existente. NSU: ";
        public const string MsgDocAtualizado = "Documento atualizado no banco. NSU: ";
        public const string MsgDocExcluido = "Documento excluído do banco. NSU: ";
        public const string MsgDocInseridoFila = "Chave do Documento inserida na fila. Chave: ";
        public const string MsgContribuinteInseridoFila = "Chave do Contribuinte do CCC inserida na fila. NSU: ";
        public const string MsgContribuinteInseridoBanco = "Contribuinte inserido no banco. NSU: ";
        public const string MsgContribuinteAtualizadoBanco = "Contribuinte atualizado no banco. NSU: ";
        public const string MsgContribuinteAtualizadoCadastro = "Contribuinte do CCC atualizado no banco do Cadastro. IE: ";
        public const string MsgContribuinteJaExistente = "Inserção de Contribuinte já existente. NSU: ";
        public const string MsgContribuinteObtido = "Contribuinte obtido no banco";
        public const string MsgContribuinteNaoObtido = "Contribuinte não encontrado no banco";
        public const string MsgContribuintesObtidos = "Lista de Contribuintes obtidos no banco";
        public const string MsgCCCMontadoParaEnvio = "Informações de Contribuintes montadas para o CCC";
        public const string MsgCCCSalvoParaEnvio = "Informações de Contribuintes salvas para envio ao CCC";
        public const string MsgCCCInseridoFila = "Chave do Contribuinte do CCC inserida na fila. Chave: ";
        public const string MsgCCCInseridoBanco = "Contribuinte do CCC inserido no banco. Chave: ";
        public const string MsgCCCAtualizado = "Contribuinte atualizado com sucesso. ";
        public const string MsgCCCJaAtualizado = "Contribuinte já atualizado. ";
        public const string MsgCCCExcluido = "Documento excluído do banco";
        public const string MsgCCCJaExcluido = "Documento já excluído";
        public const string MsgCCCContribuinteNaoAtualizado = "Contribuinte não atualizado";
        public const string MsgCCCControleInserido = "Controle do Contribuinte do CCC inserido no banco. IE: ";
        public const string MsgCCCControleAtualizado = "Controle do Contribuinte do CCC atualizado no banco. IE: ";
        public const string MsgCCCControleJaExistente = "Controle do Contribuinte já existente. IE: ";
        public const string MsgCCGInseridoFila = "GTIN inserido na fila. Chave: ";
        public const string MsgCCGInseridoBanco = "GTIN inserido no banco. Chave: ";
        public const string MsgCCGAtualizadoBanco = "GTIN atualizado no banco. Chave: ";
        public const string MsgCCGJaExistente = "GTIN já existente. Chave: ";
        public const string MsgLCCInseridoFila = "Contribuinte inserido na fila. Chave: ";
        public const string MsgLCCInseridoBanco = "Contribuinte inserido no banco. Chave: ";
        public const string MsgLCCAtualizadoBanco = "Contribuinte atualizado no banco. Chave: ";
        public const string MsgLCCJaExistente = "Contribuinte já existente. Chave: ";
        public const string MsgNSUFaltanteInserido = "NSU Faltante inserido no banco. NSU: ";
        public const string MsgNSUFaltanteJaExistente = "Inserção de NSU Faltante já existente. NSU: ";
        public const string MsgFilaObterRegistros = "Obtendo registros para serem reinseridos na fila";
        public const string MsgFilaRegistrosObtidos = "Registros para serem reinseridos na fila obtidos com sucesso";
        public const string MsgFilaQtdeAcima = "Quantidade de itens na fila acima do esperado";
        public const string MsgServGerenciados = "Gerenciamento dos serviços realizado com sucesso. Banco: ";
        public const string MsgServListaReiniciar = "Lista de serviços para Reiniciar, obtida com sucesso. Banco: ";
        public const string MsgServListaParar = "Lista de serviços para Parar, obtida com sucesso. Banco: ";
        public const string MsgServParadoReiniciado = "Serviço estava Parado e foi Reiniciado com sucesso. Serviço: ";
        public const string MsgServRodandoReiniciado = "Serviço estava Rodando e foi Reiniciado com sucesso. Serviço: ";
        public const string MsgServDesabilitadoNaoReiniciado = "Serviço está Desabilitado, portanto não deve ser Reiniciado. Serviço: ";
        public const string MsgServDesabilitadoNaoParado = "Serviço está Desabilitado, portanto não precisa ser Parado. Serviço: ";
        public const string MsgServParado = "Serviço estava Rodando e foi Parado com sucesso. Serviço: ";
        public const string MsgServJaParado = "Serviço já estava Parado. Serviço: ";
        public const string MsgNPBRegrasNaoContempladas = "Não necessário o envio ao banco do NBP: Regras não contempladas.";
        public const string MsgNPBCFOPNaoParticipante = "Não necessário o envio ao banco do NBP: CFOP não participante.";
        public const string MsgNPBConsumidorNaoParticipante = "Não necessário o envio ao banco do NBP: Consumidor não participante.";
        public const string MsgCDFeCNPJNaoParticipante = "Não necessário o envio ao banco do CDFe: CNPJ não participante.";
        public const string MsgMEIContribuinteInvalido = "Contribuinte não é valido para as compras MEI. CNPJ: ";
        public const string MsgMEIContribuinteInserido = "Contribuinte inserido com sucesso nas Compras MEI. CNPJ: ";
        public const string MsgMEIContribuinteAtualizado = "Contribuinte atualizado com sucesso nas Compras MEI. CNPJ: ";
        public const string MsgMEIContribuinteInapto = "Contribuinte tornado inapto com sucesso, por ter atingido o limite das Compras MEI. CNPJ: ";
        public const string MsgMEIContribuinteNaoInapto = "Contribuinte não tornado inapto, mesmo tendo atingido o limite das Compras MEI. CNPJ: ";
        public const string MsgMEINotaInvalida = "Nota não é valida para as compras MEI. Chave: ";
        public const string MsgMEINotaInserida = "Nota inserida com sucesso nas Compras MEI. Chave: ";
        public const string MsgMEINotaCancelada = "Nota cancelada com sucesso nas Compras MEI. Chave: ";
        public const string MsgMEINotaExcluida = "Nota excluida com sucesso nas Compras MEI. Chave: ";
        public const string MsgMEIValoresZerados = "Valores zerados com sucesso nas compras MEI.";
        public const string MsgMEINotasIncluidasTruncadas = "Notas incluídas nas compras MEI, truncadas com sucesso.";
        public const string MsgMEINotasExcluidasTruncadas = "Notas excluídas nas compras MEI, truncadas com sucesso.";
        public const string MsgFATeNotaInserida = "FAT-e inserida com sucesso. Chave: ";
        public const string MsgFATeEventoInserido = "Evento de FAT-e inserido com sucesso. Chave: ";
        public const string MsgFATeNotaJaExistente = "Inserção de FAT-e já existente. Chave: ";
        public const string MsgFATeEventoJaExistente = "Inserção de Evento de FAT-e já existente. Chave: ";

        #endregion

        #region " Esquemas "

        public const string EsqLote = "loteDist";
        public const string EsqBPeLote = "loteDistBPe";
        public const string EsqBPeAutorizacaoSchema = "procBPe";
        public const string EsqBPeAutorizacaoProc = "bpeProc";
        public const string EsqBPeAutorizacaoEnv = "BPe";
        public const string EsqBPeAutorizacaoEnvTA = "BPeTA";
        public const string EsqBPeAutorizacaoRet = "protBPe";
        public const string EsqBPeEventoSchema = "procEventoBPe";
        public const string EsqBPeEventoProc = "procEventoBPe";
        public const string EsqBPeEventoEnv = "eventoBPe";
        public const string EsqBPeEventoRet = "retEventoBPe";
        public const string EsqBPeInutilizacaoSchema = "procInutBPe";
        public const string EsqBPeInutilizacaoProc = "procInutBPe";
        public const string EsqBPeInutilizacaoEnv = "inutBPe";
        public const string EsqBPeInutilizacaoRet = "retInutBPe";
        public const string EsqCTeRetSVD = "retDistCTeSVD";
        public const string EsqCTeAutorizacaoSchema = "procCTe";
        public const string EsqCTeAutorizacaoProc = "cteProc";
        public const string EsqCTeAutorizacaoEnv = "CTe";
        public const string EsqCTeAutorizacaoRet = "protCTe";
        public const string EsqCTeEventoSchema = "procEventoCTe";
        public const string EsqCTeEventoProc = "procEventoCTe";
        public const string EsqCTeEventoEnv = "eventoCTe";
        public const string EsqCTeEventoRet = "retEventoCTe";
        public const string EsqCTeInutilizacaoSchema = "procInutCTe";
        public const string EsqCTeInutilizacaoProc = "procInutCTe";
        public const string EsqCTeInutilizacaoEnv = "inutCTe";
        public const string EsqCTeInutilizacaoRet = "retInutCTe";
        public const string EsqCTeOSAutorizacaoSchema = "procCTeOS";
        public const string EsqCTeOSAutorizacaoProc = "cteOSProc";
        public const string EsqCTeOSAutorizacaoEnv = "CTeOS";
        public const string EsqCTeOSAutorizacaoRet = "protCTe";
        public const string EsqCTeSimpAutorizacaoSchema = "procCTeSimp";
        public const string EsqCTeSimpAutorizacaoProc = "cteProcSimp";
        public const string EsqCTeSimpAutorizacaoProc2 = "cteSimpProc";
        public const string EsqCTeSimpAutorizacaoEnv = "CTeSimp";
        public const string EsqCTeSimpAutorizacaoRet = "protCTe";
        public const string EsqGTVeAutorizacaoSchema = "procGTVe";
        public const string EsqGTVeAutorizacaoProc = "GTVeProc";
        public const string EsqGTVeAutorizacaoEnv = "GTVe";
        public const string EsqGTVeAutorizacaoRet = "protCTe";
        public const string EsqGTVeEventoSchema = "procEventoGTVe";
        public const string EsqGTVeEventoProc = "procEventoGTVe";
        public const string EsqGTVeEventoEnv = "eventoGTVe";
        public const string EsqGTVeEventoRet = "retEventoCTe";
        public const string EsqGTVeInutilizacaoSchema = "procInutGTVe";
        public const string EsqGTVeInutilizacaoProc = "procInutGTVe";
        public const string EsqGTVeInutilizacaoEnv = "inutGTVe";
        public const string EsqGTVeInutilizacaoRet = "retInutCTe";
        public const string EsqMDFeLote = "loteDistMDFe";
        public const string EsqMDFeAutorizacaoSchema = "procMDFe";
        public const string EsqMDFeAutorizacaoProc = "mdfeProc";
        public const string EsqMDFeAutorizacaoEnv = "MDFe";
        public const string EsqMDFeAutorizacaoRet = "protMDFe";
        public const string EsqMDFeEventoSchema = "procEventoMDFe";
        public const string EsqMDFeEventoProc = "procEventoMDFe";
        public const string EsqMDFeEventoEnv = "eventoMDFe";
        public const string EsqMDFeEventoRet = "retEventoMDFe";
        public const string EsqMDFeInutilizacaoSchema = "procInutMDFe";
        public const string EsqMDFeInutilizacaoProc = "procInutMDFe";
        public const string EsqMDFeInutilizacaoEnv = "inutMDFe";
        public const string EsqMDFeInutilizacaoRet = "retInutMDFe";
        public const string EsqNF3eLote = "loteDistNF3e";
        public const string EsqNF3eAutorizacaoSchema = "procNF3e";
        public const string EsqNF3eAutorizacaoProc = "NF3eProc";
        public const string EsqNF3eAutorizacaoProc2 = "nf3eProc";
        public const string EsqNF3eAutorizacaoEnv = "NF3e";
        public const string EsqNF3eAutorizacaoRet = "protNF3e";
        public const string EsqNF3eEventoSchema = "procEventoNF3e";
        public const string EsqNF3eEventoProc = "procEventoNF3e";
        public const string EsqNF3eEventoEnv = "eventoNF3e";
        public const string EsqNF3eEventoRet = "retEventoNF3e";
        public const string EsqNF3eInutilizacaoSchema = "procInutNF3e";
        public const string EsqNF3eInutilizacaoProc = "procInutNF3e";
        public const string EsqNF3eInutilizacaoEnv = "inutNF3e";
        public const string EsqNF3eInutilizacaoRet = "retInutNF3e";
        public const string EsqNFComRetSVD = "retDistNFComSVD";
        public const string EsqNFComAutorizacaoSchema = "procNFCom";
        public const string EsqNFComAutorizacaoProc = "NFComProc";
        public const string EsqNFComAutorizacaoProc2 = "nfcomProc";
        public const string EsqNFComAutorizacaoEnv = "NFCom";
        public const string EsqNFComAutorizacaoRet = "protNFCom";
        public const string EsqNFComEventoSchema = "procEventoNFCom";
        public const string EsqNFComEventoProc = "procEventoNFCom";
        public const string EsqNFComEventoEnv = "eventoNFCom";
        public const string EsqNFComEventoRet = "retEventoNFCom";
        public const string EsqNFComInutilizacaoSchema = "procInutNFCom";
        public const string EsqNFComInutilizacaoProc = "procInutNFCom";
        public const string EsqNFComInutilizacaoEnv = "inutNFCom";
        public const string EsqNFComInutilizacaoRet = "retInutNFCom";
        public const string EsqNFCeLote = "loteDist";
        public const string EsqNFCeAutorizacaoSchema = "procNFe";
        public const string EsqNFCeAutorizacaoProc = "proc";
        public const string EsqNFCeAutorizacaoInt = "nfeProc";
        public const string EsqNFCeAutorizacaoEnv = "NFe";
        public const string EsqNFCeAutorizacaoRet = "protNFe";
        public const string EsqNFCeEventoSchema = "procEventoNFe";
        public const string EsqNFCeEventoProc = "procEventoNFe";
        public const string EsqNFCeEventoEnv = "evento";
        public const string EsqNFCeEventoRet = "retEvento";
        public const string EsqNFCeInutilizacaoSchema = "procInutNfe";
        public const string EsqNFCeInutilizacaoProc = "procInutNFe";
        public const string EsqNFCeInutilizacaoEnv = "inutNFe";
        public const string EsqNFCeInutilizacaoRet = "retInutNFe";
        public const string EsqNFeLote = "loteDistNFe";
        public const string EsqNFeAutorizacaoSchema = "procNFe";
        public const string EsqNFeAutorizacaoProc = "proc";
        public const string EsqNFeAutorizacaoInt = "nfeProc";
        public const string EsqNFeAutorizacaoEnv = "NFe";
        public const string EsqNFeAutorizacaoRet = "protNFe";
        public const string EsqNFeCancSchema = "procCancNFe";
        public const string EsqNFeCancProc = "proc";
        public const string EsqNFeCancEnv = "cancNFe";
        public const string EsqNFeCancRet = "retCancNFe";
        public const string EsqNFeEventoSchema = "procEventoNFe";
        public const string EsqNFeEventoProc = "proc";
        public const string EsqNFeEventoInt = "procEventoNFe";
        public const string EsqNFeEventoEnv = "evento";
        public const string EsqNFeEventoRet = "retEvento";
        public const string EsqNFeInutilizacaoSchema = "procInutNFe";
        public const string EsqNFeInutilizacaoProc = "proc";
        public const string EsqNFeInutilizacaoInt = "ProcInutNFe";
        public const string EsqNFeInutilizacaoEnv = "inutNFe";
        public const string EsqNFeInutilizacaoRet = "retInutNFe";
        public const string EsqNFeAutorizadoBA = "procNFeBA";
        public const string EsqNFeEventoAutorizadoBA = "procEventoNFeBA";
        public const string EsqNFeInutilizacaoAutorizadoBA = "procInutNFeBA";

        #endregion

        #region " Enumeracoes "

        public enum TipoFormatData
        {
            Nenhuma,
            Data,
            DataHora,
            DataHoraFuso,
            DataHoraT,
            DataHoraFusoT
        };

        public enum TipoLog
        {
            Nada = 0,
            Erro = 1,
            ErroAlerta = 2,
            ErroAlertaSucesso = 3,
        };

        public enum TipoOcorrencia
        {
            Nada = 0,
            Erro = 1,
            Alerta = 2,
            Sucesso = 3,
            Rejeicao = 4,
        };

        public enum TipoMensagem
        {
            Msg_033_NaoPermitidoTornarContribuinteInapto = 33,
            Msg_100_AutorizadoUsodaNFe = 100,
            Msg_101_CancelamentoHomologado = 101,
            Msg_102_InutilizacaoNumeroHomologado = 102,
            Msg_103_LoteRecebidocomSucesso = 103,
            Msg_104_LoteProcessado = 104,
            Msg_105_LoteEmProcessamento = 105,
            Msg_106_LoteNaoLocalizado = 106,
            Msg_107_ServicoEmOperacao = 107,
            Msg_108_ServicoEmManutencao = 108,
            Msg_109_ServicoParalisado = 109,
            Msg_110_UsoDenegado = 110,
            Msg_111_ConsultaCadastroUmaOcorrencia = 111,
            Msg_112_ConsultaCadastroMaisUmaOcorrencia = 112,
            Msg_113_DFeRecebido = 113,
            Msg_114_NenhumProtocoloFaltanteLocalizado = 114,
            Msg_115_ProtocolosFaltantesLocalizados = 115,
            Msg_116_ProtocolosFaltantesLocalizadosExcedeuLimite = 116,
            Msg_117_NenhumDFeLocalizado = 117,
            Msg_118_DFeLocalizado = 118,
            Msg_119_AtualizacaoEfetuadaAmbienteNacional = 119,
            Msg_120_NenhumCadastroLocalizado = 120,
            Msg_121_ConsultaComResultado = 121,
            Msg_122_ConsultaComResultadoContinuacao = 122,
            Msg_128_LoteEventoProcessado = 128,
            Msg_129_NFeAutorizada = 129,
            Msg_130_NFeDenegada = 130,
            Msg_131_NFeCancelada = 131,
            Msg_135_EventoProcessado = 135,
            Msg_137_NenhumRegistroLocalizado = 137,
            Msg_138_RegistroLocalizado = 138,
            Msg_141_LoteDFeProcessado = 141,
            Msg_143_NSUFinalDeveSerSuperiorNSUInicial = 143,
            Msg_146_NSUSolicitadoMenorDisponivel = 146,
            Msg_150_AutorizadoUsodaNFeForadePrazo = 150,
            Msg_151_CancelamentoNFeHomologadoForaPrazo = 151,
            Msg_201_LimiteNumeracaoInutilizarUltrapassado = 201,
            Msg_202_FalhaIntegridade = 202,
            Msg_203_EmissorNaoHabilitado = 203,
            Msg_204_DFeJaAutorizado = 204,
            Msg_205_DFeDenegado = 205,
            Msg_206_DFeJaInutilizado = 206,
            Msg_207_CNPJEmitenteInvalido = 207,
            Msg_208_CNPJDestinatarioInvalido = 208,
            Msg_209_IEEmitenteInvalida = 209,
            Msg_210_IEDestinatarioInvalida = 210,
            Msg_211_IESubstitutoInvalida = 211,
            Msg_212_DataEmissaoPosteriorDataRecebimento = 212,
            Msg_213_CNPJEmitenteDifereCNPJCertificado = 213,
            Msg_214_TamanhoArquivoExcedido = 214,
            Msg_215_FalhaEsquemaXML = 215,
            Msg_216_ChaveAcessoInvalida = 216,
            Msg_217_DFeNaoConstaSefaz = 217,
            Msg_218_DFeJaCancelada = 218,
            Msg_219_CirculacaoNotaVerificada = 219,
            Msg_220_NotaEmitidaMaisde24Horas = 220,
            Msg_221_RecebimentoConfirmadoDestinatario = 221,
            Msg_222_ProtocoloAutorizacaoInvalido = 222,
            Msg_223_CNPJCPFTransmissorDifereConsulta = 223,
            Msg_224_FaixaInicialMaiorFinal = 224,
            Msg_225_FalhaEsquemaXMLNota = 225,
            Msg_226_UFEmitenteDiferenteUFAutorizadora = 226,
            Msg_227_CPFEmitenteDifereCPFCertificado = 227,
            Msg_228_DataEmissaoAtrasada = 228,
            Msg_229_IEEmitenteNaoInformada = 229,
            Msg_230_IEEmitenteNaoCadastrada = 230,
            Msg_231_IEEmitenteNaoVinculadaCNPJ = 231,
            Msg_232_IEDestinatarioNaoInformada = 232,
            Msg_233_IEDestinatarioNaoCadastrada = 233,
            Msg_234_IEDestinatarioNaoVinculadaCNPJ = 234,
            Msg_235_InscricaoSuframaInvalida = 235,
            Msg_236_ChaveAcessoNFeInvalida = 236,
            Msg_237_CPFDestinatarioInvalido = 237,
            Msg_238_VersaoArquivoXMLSuperiorVigente = 238,
            Msg_239_VersaoArquivoXMLNaoSuportada = 239,
            Msg_240_IrregularidadeEmitente = 240,
            Msg_241_NumeroFaixaJaUtilizado = 241,
            Msg_242_FalhaEsquemaXMLCabecalho = 242,
            Msg_248_UFNumeroReciboDifereUFWebService = 248,
            Msg_249_UFChaveAcessoDifereUFWebService = 249,
            Msg_250_UFInutilizacaoDifereUFWebService = 250,
            Msg_252_AmbienteInformadoDivergeRecebimento = 252,
            Msg_256_NumeroFaixaJaInutilizado = 256,
            Msg_257_CNPJSolicitanteNaoEmissorNFe = 257,
            Msg_258_CNPJConsultaCadastroInvalido = 258,
            Msg_259_ContribuinteInexistenteCNPJ = 259,
            Msg_260_IEConsultaCadastroInvalida = 260,
            Msg_261_ContribuinteInexistenteIE = 261,
            Msg_262_UFNaoForneceConsultaCPF = 262,
            Msg_263_CPFConsultaCadastroInvalida = 263,
            Msg_264_ContribuinteInexistenteCPF = 264,
            Msg_265_UFConsultaDifereUFWebService = 265,
            Msg_266_NumeroSerieInvalido = 266,
            Msg_280_CertificadoTransmissorInvalido = 280,
            Msg_281_CertificadoTransmissorDataValidade = 281,
            Msg_282_CertificadoTransmissorsemCNPJCPF = 282,
            Msg_283_ErroCadeiaCertificacaoTransmissor = 283,
            Msg_284_CertificadoTransmissorRevogado = 284,
            Msg_285_CertificadoTransmissorDifereICP = 285,
            Msg_289_CodigoUFConsultaDifereUFWebService = 289,
            Msg_290_CertificadoAssinaturaInvalido = 290,
            Msg_291_CertificadoAssinaturaDataValidade = 291,
            Msg_292_CertificadoAssinaturaSemCNPJCPF = 292,
            Msg_293_ErroCadeiaCertificacaoAssinatura = 293,
            Msg_294_CertificadoAssinaturaRevogado = 294,
            Msg_295_CertificadoAssinaturaDifereICP = 295,
            Msg_296_DigestDifereCalculadoAssinatura = 296,
            Msg_297_AssinaturaDifereCalculado = 297,
            Msg_299_FalhaCabecalhoCodificacaoUTF8 = 299,
            Msg_301_DenegacaoIrregularidadeEmitente = 301,
            Msg_302_DenegacaoIrregularidadeDestinatario = 302,
            Msg_402_FalhaDadosCodificacaoUTF8 = 402,
            Msg_404_FalhaPrefixoNamespace = 404,
            Msg_409_ElementoUFInexistenteSoapHeader = 409,
            Msg_410_ElementoUFNaoAtendidaWebService = 410,
            Msg_411_ElementoUFNaoAtendidaWebService = 411,
            Msg_412_ElementoVersaoDadosInexistenteSoapHeaderConsultaNFe = 412,
            Msg_415_CNPJTransmissorNaoAutorizadoParaUF = 415,
            Msg_416_FalhaDescompactacaoAreaDados = 416,
            Msg_420_CancelamentoNFeJaCancelada = 420,
            Msg_426_FalhaXMLAreaDadosDescompactada = 426,
            Msg_441_CredenciamentoNaoPermitido = 441,
            Msg_450_ModeloDiferente55 = 450,
            Msg_453_AnoInutilizacaoSuperior = 453,
            Msg_454_AnoInutilizacaoInferior = 454,
            Msg_493_EventoNaoAtendeSchema = 493,
            Msg_494_ChaveAcessoInexistente = 494,
            Msg_495_InscricaoNaoAutorizadaConsulta = 495,
            Msg_502_ChaveDifereConcatenacaoCampos = 502,
            Msg_526_ChaveAcessoSuperior6Meses = 526,
            Msg_546_CampoIdInvalido = 546,
            Msg_553_TipoAutorizadorReciboDifereOrgaoAutorizador = 553,
            Msg_561_CampoMMDiferenteExistenteBD = 561,
            Msg_562_CodigoNumericoInexistenteBD = 562,
            Msg_563_FaixaNumeracaoJaInutilizada = 563,
            Msg_570_FalhaValidarDFeComSchemaInformado = 570,
            Msg_573_DuplicidadeEvento = 573,
            Msg_580_EventoExigeNFeAutorizada = 580,
            Msg_587_PadraoNamespace = 587,
            Msg_600_ProtocoloNaoVinculadoDFe = 600,
            Msg_613_ChaveAcessoDifereBD = 613,
            Msg_614_ChaveAcessoInvalidaUF = 614,
            Msg_615_ChaveAcessoInvalidaAno = 615,
            Msg_616_ChaveAcessoInvalidaMes = 616,
            Msg_617_ChaveAcessoInvalidaCNPJCPF = 617,
            Msg_618_ChaveAcessoInvalidaModelo = 618,
            Msg_619_ChaveAcessoInvalidaNF = 619,
            Msg_642_FalhaConsultaRegistroPassagem = 642,
            Msg_645_DataEmissaoSuperiorDataAutorizacao = 645,
            Msg_646_DataAutorizacaoSuperiorDataRecebimento = 646,
            Msg_702_NFCeNaoAceitaPelaUFEmitente = 702,
            Msg_730_NSUSolicitadoMuitoAntigo = 730,
            Msg_776_UFNaoDisponibilizaEsteAtendimento = 776,
            Msg_901_CancelamentoNaoRealizadoImpossibilidaAcessoAN = 901,
            Msg_992_NSUSolicitadoMuitoAntigo = 992,
            Msg_999_FalhaNaoTratada = 999,
            Msg_2202_NotaEmitidaMaisde168Horas = 2202,
            Msg_9120_NenhumCadastroLocalizado = 9120,
            Msg_9121_ConsultaComResultado = 9121,
            Msg_9122_ConsultaComResultadoContinuacao = 9122,
            Msg_9210_AtualizacaoEfetuadaCadastro = 9210,
            Msg_9302_CNPJInvalido = 9302,
            Msg_9303_CPFInvalido = 9303,
            Msg_9304_NaoAceitoCadastramentoCPF = 9304,
            Msg_9305_IEInvalida = 9305,
            Msg_9308_CNAEInexistente = 9308,
            Msg_9320_ContribuinteJaCadastrado = 9320,
            Msg_9322_ContribuinteJaExcluido = 9322,
            Msg_9326_ExclusaoEfetuadaCadastro = 9326,
            Msg_9343_CPFUnicamenteParaProdutorRural = 9343,
            Msg_9353_UFEnderecoMesmaUFCadastramento = 9353
        };

        public enum TipoEvento
        {
            Autorizacao = 110100,
            Denegacao = 110101,
            CartaCorrecao = 110110,
            Cancelamento = 110111,
            Encerramento = 110112,
            EPEC = 110113,
            Confirmacao = 210200,
            Ciencia = 210210,
            Desconhecimento = 210220,
            OperacaoNaoRealizada = 210240,
            Referenciada = 410300,
            RegistroPassagem = 610500,
            RegistroPassagemBRId = 610550,
            CTeAutorizado = 610600,
            CTeCancelado = 610601,
            MDFeAutorizado = 610610,
            MDFeCancelado = 610611,
            Averbacao = 790700,
            VistoriaSefaz = 630690,
            VistoriaSuframa = 990900,
            InternalizacaoSuframa = 990910
        };

        public enum SubTipoDFe
        {
            Autorizacao = 1,
            Evento = 2,
            Inutilizacao = 3,
        };

        #endregion

        #region " Metodos "

        public string ObterEstado(int intCodigo)
        {
            string strRetorno = string.Empty;
            switch (intCodigo)
            {
                case 11:
                    strRetorno = "RO";
                    break;
                case 12:
                    strRetorno = "AC";
                    break;
                case 13:
                    strRetorno = "AM";
                    break;
                case 14:
                    strRetorno = "RR";
                    break;
                case 15:
                    strRetorno = "PA";
                    break;
                case 16:
                    strRetorno = "AP";
                    break;
                case 17:
                    strRetorno = "TO";
                    break;
                case 21:
                    strRetorno = "MA";
                    break;
                case 22:
                    strRetorno = "PI";
                    break;
                case 23:
                    strRetorno = "CE";
                    break;
                case 24:
                    strRetorno = "RN";
                    break;
                case 25:
                    strRetorno = "PB";
                    break;
                case 26:
                    strRetorno = "PE";
                    break;
                case 27:
                    strRetorno = "AL";
                    break;
                case 28:
                    strRetorno = "SE";
                    break;
                case 29:
                    strRetorno = "BA";
                    break;
                case 31:
                    strRetorno = "MG";
                    break;
                case 32:
                    strRetorno = "ES";
                    break;
                case 33:
                    strRetorno = "RJ";
                    break;
                case 35:
                    strRetorno = "SP";
                    break;
                case 41:
                    strRetorno = "PR";
                    break;
                case 42:
                    strRetorno = "SC";
                    break;
                case 43:
                    strRetorno = "RS";
                    break;
                case 50:
                    strRetorno = "MS";
                    break;
                case 51:
                    strRetorno = "MT";
                    break;
                case 52:
                    strRetorno = "GO";
                    break;
                case 53:
                    strRetorno = "DF";
                    break;
                case 91:
                    strRetorno = "AN";
                    break;
            }
            return strRetorno;
        }

        public string ObterMensagem(int intCodigo)
        {
            string strRetorno = string.Empty;
            switch (intCodigo)
            {
                case 2:
                    strRetorno = "Certificado Assinatura invalido";
                    break;
                case 3:
                    strRetorno = "Certificado Assinatura invalido";
                    break;
                case 4:
                    strRetorno = "Certificado Assinatura invalido";
                    break;
                case 5:
                    strRetorno = "Certificado Assinatura invalido";
                    break;
                case 6:
                    strRetorno = "Certificado Assinatura Data Validade";
                    break;
                case 7:
                    strRetorno = "Certificado Assinatura sem CNPJ";
                    break;
                case 8:
                    strRetorno = "Certificado Assinatura - erro Cadeia de Certificacao";
                    break;
                case 9:
                    strRetorno = "Certificado Assinatura - erro Cadeia de Certificacao";
                    break;
                case 11:
                    strRetorno = "Certificado Assinatura revogado";
                    break;
                case 12:
                    strRetorno = "Certificado Assinatura difere ICP-Brasil";
                    break;
                case 14:
                    strRetorno = "Certificado Assinatura difere do padrao do projeto";
                    break;
                case 16:
                    strRetorno = "Assinatura difere do calculado";
                    break;
                case 100:
                    strRetorno = "Autorizado o uso da NF-e";
                    break;
                case 101:
                    strRetorno = "Cancelamento de NF-e homologado";
                    break;
                case 102:
                    strRetorno = "Inutilizacao de numero homologado";
                    break;
                case 103:
                    strRetorno = "Lote recebido com sucesso";
                    break;
                case 104:
                    strRetorno = "Lote processado";
                    break;
                case 105:
                    strRetorno = "Lote em processamento";
                    break;
                case 106:
                    strRetorno = "Lote nao localizado";
                    break;
                case 107:
                    strRetorno = "Servico em Operacao";
                    break;
                case 108:
                    strRetorno = "Servico Paralisado Momentaneamente (curto prazo)";
                    break;
                case 109:
                    strRetorno = "Servico Paralisado sem Previsao";
                    break;
                case 110:
                    strRetorno = "Uso Denegado";
                    break;
                case 111:
                    strRetorno = "Consulta cadastro com uma ocorrencia";
                    break;
                case 112:
                    strRetorno = "Consulta cadastro com mais de uma ocorrencia";
                    break;
                case 117:
                    strRetorno = "Nenhum DF-e localizado para distribuicao";
                    break;
                case 118:
                    strRetorno = "DF-e localizados";
                    break;
                case 119:
                    strRetorno = "Rejeicao: NSU solicitado menor que o ultimo NSU disponivel";
                    break;
                case 128:
                    strRetorno = "Lote de Evento Processado";
                    break;
                case 129:
                    strRetorno = "NF-e autorizada";
                    break;
                case 130:
                    strRetorno = "NF-e denegada";
                    break;
                case 131:
                    strRetorno = "NF-e cancelada";
                    break;
                case 135:
                    strRetorno = "Evento Processado";
                    break;
                case 141:
                    strRetorno = "Lote de DF-e processado com sucesso";
                    break;
                case 143:
                    strRetorno = "Rejeicao: NSU Final deve ser superior ao ultNSU";
                    break;
                case 150:
                    strRetorno = "Autorizado o uso da NF-e, autorização fora de prazo";
                    break;
                case 151:
                    strRetorno = "Cancelamento de NF-e homologado fora de prazo";
                    break;
                case 201:
                    strRetorno = "Rejeicao: O numero maximo de numeracao de NF-e a inutilizar ultrapassou o limite";
                    break;
                case 202:
                    strRetorno = "Rejeicao: Falha no reconhecimento da autoria ou integridade do arquivo digital";
                    break;
                case 203:
                    strRetorno = "Rejeicao: Emissor nao habilitado para emissao da NF-e";
                    break;
                case 204:
                    strRetorno = "Rejeicao: Duplicidade de NF-e";
                    break;
                case 205:
                    strRetorno = "Rejeicao: NF-e esta denegada na base de dados da SEFAZ";
                    break;
                case 206:
                    strRetorno = "Rejeicao: NF-e ja esta inutilizada na Base de dados da SEFAZ";
                    break;
                case 207:
                    strRetorno = "Rejeicao: CNPJ do emitente invalido";
                    break;
                case 208:
                    strRetorno = "Rejeicao: CNPJ do destinatario invalido";
                    break;
                case 209:
                    strRetorno = "Rejeicao: IE do emitente invalida";
                    break;
                case 210:
                    strRetorno = "Rejeicao: IE do destinatario invalida";
                    break;
                case 211:
                    strRetorno = "Rejeicao: IE do substituto invalida";
                    break;
                case 212:
                    strRetorno = "Rejeicao: Data de emissao NF-e posterior a data de recebimento";
                    break;
                case 213:
                    strRetorno = "Rejeicao: CNPJ-Base do Emitente difere do CNPJ-Base do Certificado Digital";
                    break;
                case 214:
                    strRetorno = "Rejeicao: Tamanho da mensagem excedeu o limite estabelecido";
                    break;
                case 215:
                    strRetorno = "Rejeicao: Falha no schema XML";
                    break;
                case 216:
                    strRetorno = "Rejeicao: Chave de Acesso difere da cadastrada";
                    break;
                case 217:
                    strRetorno = "Rejeicao: NF-e nao consta na base de dados da SEFAZ";
                    break;
                case 218:
                    strRetorno = "Rejeicao: NF-e ja esta cancelada na base de dados da SEFAZ";
                    break;
                case 219:
                    strRetorno = "Rejeicao: Circulacao da NF-e verificada";
                    break;
                case 220:
                    strRetorno = "Rejeicao: Prazo de Cancelamento Superior ao Previsto na Legislacao";
                    break;
                case 2202:
                    strRetorno = "Rejeicao: Prazo de Cancelamento Superior ao Previsto na Legislacao";
                    break;
                case 221:
                    strRetorno = "Rejeicao: Confirmado o recebimento da NF-e pelo destinatario";
                    break;
                case 222:
                    strRetorno = "Rejeicao: Protocolo de Autorizacao de Uso difere do cadastrado";
                    break;
                case 223:
                    strRetorno = "Rejeicao: CNPJ/CPF do transmissor do lote difere do CNPJ/CPF do transmissor da consulta";
                    break;
                case 224:
                    strRetorno = "Rejeicao: A faixa inicial e maior que a faixa final";
                    break;
                case 225:
                    strRetorno = "Rejeicao: Falha no Schema XML da NFe";
                    break;
                case 226:
                    strRetorno = "Rejeicao: Codigo da UF do Emitente diverge da UF autorizadora";
                    break;
                case 227:
                    strRetorno = "Rejeicao: CPF do Emitente difere do CPF do Certificado Digital";
                    break;
                case 228:
                    strRetorno = "Rejeicao: Data de Emissao muito atrasada";
                    break;
                case 229:
                    strRetorno = "Rejeicao: IE do emitente nao informada";
                    break;
                case 230:
                    strRetorno = "Rejeicao: IE do emitente nao cadastrada";
                    break;
                case 231:
                    strRetorno = "Rejeicao: IE do emitente nao vinculada ao CNPJ";
                    break;
                case 232:
                    strRetorno = "Rejeicao: IE do destinatario nao informada";
                    break;
                case 233:
                    strRetorno = "Rejeicao: IE do destinatario nao cadastrada";
                    break;
                case 234:
                    strRetorno = "Rejeicao: IE do destinatario nao vinculada ao CNPJ";
                    break;
                case 235:
                    strRetorno = "Rejeicao: Inscricao SUFRAMA invalida";
                    break;
                case 236:
                    strRetorno = "Rejeicao: Chave de Acesso com digito verificador invalido";
                    break;
                case 237:
                    strRetorno = "Rejeicao: CPF do destinatario invalido";
                    break;
                case 238:
                    strRetorno = "Rejeicao: Cabecalho - Versao do arquivo XML superior a Versao vigente";
                    break;
                case 239:
                    strRetorno = "Rejeicao: Cabecalho - Versao do arquivo XML nao suportada";
                    break;
                case 240:
                    strRetorno = "Rejeicao: Cancelamento/Inutilizacao - Irregularidade Fiscal do Emitente";
                    break;
                case 241:
                    strRetorno = "Rejeicao: Um numero da faixa ja foi utilizado";
                    break;
                case 242:
                    strRetorno = "Rejeicao: Cabecalho - Falha no Schema XML";
                    break;
                case 243:
                    strRetorno = "Rejeicao: XML Mal Formado";
                    break;
                case 244:
                    strRetorno = "Rejeicao: CNPJ do Certificado Digital difere do CNPJ da Matriz e do CNPJ do Emitente";
                    break;
                case 245:
                    strRetorno = "Rejeicao: CNPJ Emitente nao cadastrado";
                    break;
                case 246:
                    strRetorno = "Rejeicao: CNPJ Destinatario nao cadastrado";
                    break;
                case 247:
                    strRetorno = "Rejeicao: Sigla da UF do Emitente diverge da UF autorizadora";
                    break;
                case 248:
                    strRetorno = "Rejeicao: UF do Recibo diverge da UF autorizadora";
                    break;
                case 249:
                    strRetorno = "Rejeicao: UF da Chave de Acesso diverge da UF autorizadora";
                    break;
                case 250:
                    strRetorno = "Rejeicao: UF diverge da UF autorizadora";
                    break;
                case 251:
                    strRetorno = "Rejeicao: UF/Municipio destinatario nao pertence a SUFRAMA";
                    break;
                case 252:
                    strRetorno = "Rejeicao: Ambiente informado diverge do Ambiente de recebimento";
                    break;
                case 253:
                    strRetorno = "Rejeicao: Digito Verificador da chave de acesso composta invalida";
                    break;
                case 254:
                    strRetorno = "Rejeicao: NF-e referenciada nao informada para NF-e complementar";
                    break;
                case 255:
                    strRetorno = "Rejeicao: Informada mais de uma NF-e referenciada para NF-e complementar";
                    break;
                case 256:
                    strRetorno = "Rejeicao: Uma NF-e da faixa ja esta inutilizada na Base de dados da SEFAZ";
                    break;
                case 257:
                    strRetorno = "Rejeicao: Solicitante nao habilitado para emissao da NF-e";
                    break;
                case 258:
                    strRetorno = "Rejeicao: CNPJ da consulta invalido";
                    break;
                case 259:
                    strRetorno = "Rejeicao: CNPJ da consulta nao cadastrado como contribuinte na UF";
                    break;
                case 260:
                    strRetorno = "Rejeicao: IE da consulta invalida";
                    break;
                case 261:
                    strRetorno = "Rejeicao: IE da consulta nao cadastrada como contribuinte na UF";
                    break;
                case 262:
                    strRetorno = "Rejeicao: UF nao fornece consulta por CPF";
                    break;
                case 263:
                    strRetorno = "Rejeicao: CPF da consulta invalido";
                    break;
                case 264:
                    strRetorno = "Rejeicao: CPF da consulta nao cadastrado como contribuinte na UF";
                    break;
                case 265:
                    strRetorno = "Rejeicao: Sigla da UF da consulta difere da UF do WebService";
                    break;
                case 266:
                    strRetorno = "Rejeicao: Serie utilizada nao permitida no Web Service";
                    break;
                case 267:
                    strRetorno = "Rejeicao: NF Complementar referencia uma NF-e inexistente";
                    break;
                case 268:
                    strRetorno = "Rejeicao: NF Complementar referencia uma outra NF-e Complementar";
                    break;
                case 269:
                    strRetorno = "Rejeicao: CNPJ Emitente da NF Complementar difere do CNPJ da NF Referenciada";
                    break;
                case 270:
                    strRetorno = "Rejeicao: Codigo Municipio do Fato Gerador: digito invalido";
                    break;
                case 271:
                    strRetorno = "Rejeicao: Codigo Municipio do Fato Gerador: difere da UF do emitente";
                    break;
                case 272:
                    strRetorno = "Rejeicao: Codigo Municipio do Emitente: digito invalido";
                    break;
                case 273:
                    strRetorno = "Rejeicao: Codigo Municipio do Emitente: difere da UF do emitente";
                    break;
                case 274:
                    strRetorno = "Rejeicao: Codigo Municipio do Destinatario: digito invalido";
                    break;
                case 275:
                    strRetorno = "Rejeicao: Codigo Municipio do Destinatario: difere da UF do Destinatario";
                    break;
                case 276:
                    strRetorno = "Rejeicao: Codigo Municipio do Local de Retirada: digito invalido";
                    break;
                case 277:
                    strRetorno = "Rejeicao: Codigo Municipio do Local de Retirada: difere da UF do Local de Retirada";
                    break;
                case 278:
                    strRetorno = "Rejeicao: Codigo Municipio do Local de Entrega: digito invalido";
                    break;
                case 279:
                    strRetorno = "Rejeicao: Codigo Municipio do Local de Entrega: difere da UF do Local de Entrega";
                    break;
                case 280:
                    strRetorno = "Rejeicao: Certificado Transmissor invalido";
                    break;
                case 281:
                    strRetorno = "Rejeicao: Certificado Transmissor Data Validade";
                    break;
                case 282:
                    strRetorno = "Rejeicao: Certificado Transmissor sem CNPJ/CPF";
                    break;
                case 283:
                    strRetorno = "Rejeicao: Certificado Transmissor - erro Cadeia de Certificacao";
                    break;
                case 284:
                    strRetorno = "Rejeicao: Certificado Transmissor revogado";
                    break;
                case 285:
                    strRetorno = "Rejeicao: Certificado Transmissor difere ICP-Brasil";
                    break;
                case 286:
                    strRetorno = "Rejeicao: Certificado Transmissor erro no acesso a LCR";
                    break;
                case 287:
                    strRetorno = "Rejeicao: Codigo Municipio do FG - ISSQN: digito invalido";
                    break;
                case 288:
                    strRetorno = "Rejeicao: Codigo Municipio do FG - Transporte: digito invalido";
                    break;
                case 289:
                    strRetorno = "Rejeicao: Codigo da UF informada diverge da UF solicitada";
                    break;
                case 290:
                    strRetorno = "Rejeicao: Certificado Assinatura invalido";
                    break;
                case 291:
                    strRetorno = "Rejeicao: Certificado Assinatura Data Validade";
                    break;
                case 292:
                    strRetorno = "Rejeicao: Certificado Assinatura sem CNPJ/CPF";
                    break;
                case 293:
                    strRetorno = "Rejeicao: Certificado Assinatura - erro Cadeia de Certificacao";
                    break;
                case 294:
                    strRetorno = "Rejeicao: Certificado Assinatura revogado";
                    break;
                case 295:
                    strRetorno = "Rejeicao: Certificado Assinatura difere ICP-Brasil";
                    break;
                case 296:
                    strRetorno = "Rejeicao: Certificado Assinatura erro no acesso a LCR";
                    break;
                case 297:
                    strRetorno = "Rejeicao: Assinatura difere do calculado";
                    break;
                case 298:
                    strRetorno = "Rejeicao: Assinatura difere do padrao do Projeto";
                    break;
                case 299:
                    strRetorno = "Rejeicao: XML da area de cabecalho com codificacao diferente de UTF-8";
                    break;
                case 401:
                    strRetorno = "Rejeicao: CPF do remetente invalido";
                    break;
                case 402:
                    strRetorno = "Rejeicao: XML da area de dados com codificacao diferente de UTF-8";
                    break;
                case 403:
                    strRetorno = "Rejeicao: O grupo de informacoes da NF-e avulsa e de uso exclusivo do Fisco";
                    break;
                case 404:
                    strRetorno = "Rejeicao: Uso de prefixo de namespace nao permitido";
                    break;
                case 405:
                    strRetorno = "Rejeicao: Codigo do pais do emitente: digito invalido";
                    break;
                case 406:
                    strRetorno = "Rejeicao: Codigo do pais do destinatario: digito invalido";
                    break;
                case 407:
                    strRetorno = "Rejeicao: O CPF so pode ser informado no campo emitente para a NF-e avulsa";
                    break;
                case 301:
                    strRetorno = "Uso Denegado: Irregularidade fiscal do emitente";
                    break;
                case 302:
                    strRetorno = "Uso Denegado: Irregularidade fiscal do destinatario";
                    break;
                case 409:
                    strRetorno = "Rejeicao: Campo cUF inexistente no elemento nfeCabecMsg do SOAP Header";
                    break;
                case 410:
                    strRetorno = "Rejeicao: UF informada no campo cUF nao e atendida pelo WebService";
                    break;
                case 411:
                    strRetorno = "Rejeicao: UF informada no campo cUF nao e atendida pelo WebService";
                    break;
                case 420:
                    strRetorno = "Rejeicao: Cancelamento para NF-e ja cancelada";
                    break;
                case 412:
                    strRetorno = "Rejeicao: Campo versaoDados inexistente no elemento nfeCabecMsg do SOAP Header";
                    break;
                case 415:
                    strRetorno = "Rejeicao: CNPJ do transmissor nao esta autorizado para esta UF";
                    break;
                case 416:
                    strRetorno = "Rejeicao: Falha na descompactacao da area de dados";
                    break;
                case 450:
                    strRetorno = "Rejeicao: Modelo da NF-e diferente de 55";
                    break;
                case 453:
                    strRetorno = "Rejeicao: Ano de inutilizacao nao pode ser superior ao Ano atual";
                    break;
                case 454:
                    strRetorno = "Rejeicao: Ano de inutilizacao nao pode ser inferior a 2006";
                    break;
                case 587:
                    strRetorno = "Rejeicao: Usar somente o namespace padrao da NF-e";
                    break;
                case 495:
                    strRetorno = "Rejeicao: Solicitante nao autorizado para consulta";
                    break;
                case 502:
                    strRetorno = "Rejeicao: Erro na Chave de Acesso  Campo Id nao corresponde a concatenacao dos campos correspondentes";
                    break;
                case 526:
                    strRetorno = "Rejeicao: Ano-Mes da Chave de Acesso com atraso superior a 6 meses em relacao ao Ano-Mes atual";
                    break;
                case 553:
                    strRetorno = "Rejeicao: Tipo autorizador do recibo diverge do Orgao Autorizador";
                    break;
                case 561:
                    strRetorno = "Rejeicao: Mes de Emissao informado na Chave de Acesso difere do Mes de Emissao da NFe";
                    break;
                case 562:
                    strRetorno = "Rejeicao: Codigo numerico informado na Chave de Acesso difere do Codigo Numerico da NF-e";
                    break;
                case 563:
                    strRetorno = "Rejeicao: Ja existe um pedido de inutilizacao com a mesma faixa de inutilizacao";
                    break;
                case 613:
                    strRetorno = "Rejeicao: Chave de Acesso diferente da existente em BD";
                    break;
                case 614:
                    strRetorno = "Rejeicao: Chave de Acesso invalida (Codigo UF invalido)";
                    break;
                case 615:
                    strRetorno = "Rejeicao: Chave de Acesso invalida (Ano menor que 05 ou Ano maior que Ano corrente)";
                    break;
                case 616:
                    strRetorno = "Rejeicao: Chave de Acesso invalida (Mes menor que 1 ou Mes maior que 12)";
                    break;
                case 617:
                    strRetorno = "Rejeicao: Chave de Acesso invalida (CNPJ/CPF zerado ou digito invalido)";
                    break;
                case 618:
                    strRetorno = "Rejeicao: Chave de Acesso invalida (modelo diferente de 55)";
                    break;
                case 619:
                    strRetorno = "Rejeicao: Chave de Acesso invalida (numero NF = 0)";
                    break;
                case 642:
                    strRetorno = "Rejeicao: Falha na consulta do Registro de Passagem";
                    break;
                case 702:
                    strRetorno = "Rejeicao: NFC-e nao e aceita pela UF do Emitente";
                    break;
                case 730:
                    strRetorno = "Rejeicao: NSU solicitado muito antigo";
                    break;
                case 776:
                    strRetorno = "Rejeicao: UF nao disponibiliza este atendimento";
                    break;
                case 901:
                    strRetorno = "Rejeicao: Erro nao catalogado Impossibilidade de verificar transito da mercadoria";
                    break;
                case 999:
                    strRetorno = "Rejeicao: Erro nao catalogado";
                    break;
                default:
                    strRetorno = "Falha no reconhecimento da autoria ou integridade do arquivo digital.";
                    break;
            }
            return strRetorno;
        }

        #endregion
    }
}
