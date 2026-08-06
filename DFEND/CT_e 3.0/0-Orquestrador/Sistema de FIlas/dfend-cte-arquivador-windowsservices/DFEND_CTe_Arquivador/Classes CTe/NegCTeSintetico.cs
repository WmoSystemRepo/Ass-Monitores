using System;
using System.Diagnostics;
using System.Reflection;
using System.Xml;

namespace DFe
{
    class NegCTeSintetico
    {
        #region " Variaveis "

        // Classes utilizadas
        private Facilitador clsFacil;
        private Log clsLog;
        private BdCTeSintetico clsBDSin;

        #endregion

        #region " Construtores "

        public NegCTeSintetico(Facilitador clsFacilPar, Log clsLogPar, BdCTeSintetico clsBDSinPar)
        {
            // Inicializando classes
            clsFacil = clsFacilPar;
            clsLog = clsLogPar;
            clsBDSin = clsBDSinPar;
        }

        #endregion

        #region " SintetizarLote "

        public void SintetizarLote(XmlDocument xmlDocumento, string strNSU)
        {
            // Obtendo nome do metodo
            string strMetodo = MethodBase.GetCurrentMethod().Name;

            try
            {
                // Obtendo lista de documentos
                XmlNodeList xmlLote;
                xmlLote = xmlDocumento[Constante.EsqCTeRetSVD][Constante.EsqLote].ChildNodes;

                // Iniciando o contador de NSU
                long intContNSU = Convert.ToInt64(strNSU);

                // Percorrendo a lista de itens retornados
                foreach (XmlNode xmlItem in xmlLote)
                {
                    // Obtendo elementos do XML
                    string strNSUDFe = xmlItem.Attributes["NSUSVD"].InnerText;
                    string strEsquema = xmlItem.Attributes["schema"].InnerText;
                    XmlDocument xmlDescompactado = clsFacil.DescompactarProc(xmlItem);

                    // Verificando se existe o schema
                    if (strEsquema == string.Empty)
                    {
                        strEsquema = clsFacil.ObterEsquemaCTe(xmlDescompactado.DocumentElement);
                    }

                    // Verificando o tipo do schema retornado
                    if (strEsquema.StartsWith(Constante.EsqCTeAutorizacaoSchema))
                    {
                        // Sintetizando documento de Autorizacao
                        this.SintetizarAutorizacao(xmlDescompactado.FirstChild, strNSUDFe);
                    }
                    else if (strEsquema.StartsWith(Constante.EsqCTeEventoSchema))
                    {
                        // Sintetizando documento de Evento
                        this.SintetizarEvento(xmlDescompactado.FirstChild, strNSUDFe);
                    }
                    else if (strEsquema.StartsWith(Constante.EsqCTeInutilizacaoSchema))
                    {
                        // Sintetizando documento de Inutilizacao
                        this.SintetizarInutilizacao(xmlDescompactado.FirstChild, strNSUDFe);
                    }
                    else if (strEsquema.StartsWith(Constante.EsqGTVeAutorizacaoSchema))
                    {
                        // Sintetizando documento de Autorizacao GTV
                        this.SintetizarAutorizacaoGTV(xmlDescompactado.FirstChild, strNSUDFe);
                    }
                    else if (strEsquema.StartsWith(Constante.EsqGTVeEventoSchema))
                    {
                        // Sintetizando documento de Evento GTV
                        this.SintetizarEventoGTV(xmlDescompactado.FirstChild, strNSUDFe);
                    }
                    else if (strEsquema.StartsWith(Constante.EsqGTVeInutilizacaoSchema))
                    {
                        // Sintetizando documento de Inutilizacao GTV
                        this.SintetizarInutilizacaoGTV(xmlDescompactado.FirstChild, strNSUDFe);
                    }
                    else
                    {
                        // Levantando excecao
                        throw new Exception(Constante.MsgLoteElementoNaoEsperado + strEsquema);
                    }

                    // Validando os NSUs do pacote
                    if (((intContNSU + 1) == Convert.ToInt64(strNSUDFe)) || ((intContNSU) == Convert.ToInt64(strNSUDFe)))
                    {
                        intContNSU = Convert.ToInt64(strNSUDFe);
                    }
                    else
                    {
                        // Registrando log de erro
                        this.InserirNSUFaltante((intContNSU + 1).ToString());
                        clsLog.RegistrarLog(strMetodo, Constante.MsgLoteFuroNSU + (intContNSU + 1).ToString(), EventLogEntryType.Error);
                        intContNSU = Convert.ToInt64(strNSUDFe);
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region " SintetizarAutorizacao "

        public void SintetizarAutorizacao(XmlNode xmlDocumento, string strNSU)
        {
            // Obtendo nome do metodo
            string strMetodo = MethodBase.GetCurrentMethod().Name;

            try
            {
                // Obtendo elementos do XML
                XmlElement xmlRet = (XmlElement)((XmlElement)xmlDocumento).GetElementsByTagName(Constante.EsqCTeAutorizacaoRet)[0];

                string strChaveAcesso = clsFacil.ObterItemXML(xmlRet["infProt"]["chCTe"]);
                string strProtocolo = clsFacil.ObterItemXML(xmlRet["infProt"]["nProt"]);
                string strData = clsFacil.ObterItemXML(xmlRet["infProt"]["dhRecbto"]);
                string strDataReferencia = "20" + strChaveAcesso.Substring(2, 2) + strChaveAcesso.Substring(4, 2);

                if (strData == string.Empty)
                {
                    strData = clsFacil.ObterItemXML(xmlDocumento["CTe"]["dhRecbto"]);
                }

                // Montando a chave
                string strChave = strDataReferencia + ";" + strChaveAcesso;

                // Inserindo dados no banco
                clsBDSin.InserirAutorizacao(strDataReferencia, strChaveAcesso, strProtocolo, strNSU, xmlDocumento.OuterXml, strData);

                // Registrando log de informacao
                clsLog.RegistrarLog(strMetodo, Constante.MsgDocAutorizacaoInseridoBanco + strNSU + ". Chave: " + strChave, EventLogEntryType.Information);
            }
            catch (Exception ex)
            {
                // Verificando o tipo da excecao
                if (ex.ToString().ToUpper().Contains("PRIMARY KEY") || ex.ToString().ToUpper().Contains("DUPLICATE KEY"))
                {
                    // Registrando log de informacao
                    clsLog.RegistrarLog(strMetodo, Constante.MsgDocJaExistente + strNSU, EventLogEntryType.Information);
                }
                else
                {
                    throw;
                }
            }
        }

        #endregion

        #region " SintetizarEvento "

        public void SintetizarEvento(XmlNode xmlDocumento, string strNSU)
        {
            // Obtendo nome do metodo
            string strMetodo = MethodBase.GetCurrentMethod().Name;

            try
            {
                // Obtendo elementos do XML
                XmlElement xmlEnv = (XmlElement)((XmlElement)xmlDocumento).GetElementsByTagName(Constante.EsqCTeEventoEnv)[0];
                XmlElement xmlRet = (XmlElement)((XmlElement)xmlDocumento).GetElementsByTagName(Constante.EsqCTeEventoRet)[0];

                string strSeqEvento = string.Empty;
                string strTipoEvento = string.Empty;
                string strChaveAcesso = string.Empty;
                string strProtocolo = string.Empty;
                string strData = string.Empty;

                if (xmlEnv != null)
                {
                    strChaveAcesso = clsFacil.ObterItemXML(xmlRet["infEvento"]["chCTe"]);
                    strTipoEvento = clsFacil.ObterItemXML(xmlEnv["infEvento"]["tpEvento"]);
                    strSeqEvento = clsFacil.ObterItemXML(xmlEnv["infEvento"]["nSeqEvento"]);
                    strProtocolo = clsFacil.ObterItemXML(xmlRet["infEvento"]["nProt"]);
                    strData = clsFacil.ObterItemXML(xmlRet["infEvento"]["dhRegEvento"]);
                }
                else
                {
                    strChaveAcesso = clsFacil.ObterItemXML(xmlRet["infEvento"]["chCTe"]);
                    strTipoEvento = clsFacil.ObterItemXML(xmlRet["infEvento"]["tpEvento"]);
                    strSeqEvento = clsFacil.ObterItemXML(xmlRet["infEvento"]["nSeqEvento"]);
                    strProtocolo = clsFacil.ObterItemXML(xmlRet["infEvento"]["nProt"]);
                    strData = clsFacil.ObterItemXML(xmlRet["infEvento"]["dhRegEvento"]);
                }

                // Montando a chave
                string strDataReferencia = "20" + strChaveAcesso.Substring(2, 2) + strChaveAcesso.Substring(4, 2);
                string strChave = strDataReferencia + ";" + strChaveAcesso + ";" + strSeqEvento + ";" + strTipoEvento;

                // Verificando o tipo de evento
                if (strTipoEvento != ((int)Constante.TipoEvento.Referenciada).ToString())
                {
                    // Inserindo dados no banco
                    clsBDSin.InserirEvento(strDataReferencia, strChaveAcesso, strTipoEvento, strSeqEvento, strProtocolo, strNSU, xmlDocumento.OuterXml, strData);
                }
                else
                {
                    // Obtendo a chave referenciada
                    string strChaveAcessoRef = clsFacil.ObterItemXML(xmlEnv["infEvento"]["detEvento"]["chNFeRefte"]);

                    // Inserindo dados no banco
                    clsBDSin.InserirEventoRef(strDataReferencia, strChaveAcesso, strChaveAcessoRef, strTipoEvento, strSeqEvento, strProtocolo, strNSU, xmlDocumento.OuterXml, strData);
                }

                // Registrando log de informacao
                clsLog.RegistrarLog(strMetodo, Constante.MsgDocEventoInseridoBanco + strNSU + ". Chave: " + strChave, EventLogEntryType.Information);
            }
            catch (Exception ex)
            {
                // Verificando o tipo da excecao
                if (ex.ToString().ToUpper().Contains("PRIMARY KEY") || ex.ToString().ToUpper().Contains("DUPLICATE KEY"))
                {
                    // Registrando log de informacao
                    clsLog.RegistrarLog(strMetodo, Constante.MsgDocJaExistente + strNSU, EventLogEntryType.Information);
                }
                else
                {
                    throw;
                }
            }
        }

        #endregion

        #region " SintetizarInutilizacao "

        public void SintetizarInutilizacao(XmlNode xmlDocumento, string strNSU)
        {
            // Obtendo nome do metodo
            string strMetodo = MethodBase.GetCurrentMethod().Name;

            try
            {
                // Obtendo elementos do XML
                XmlElement xmlEnv = (XmlElement)((XmlElement)xmlDocumento).GetElementsByTagName(Constante.EsqCTeInutilizacaoEnv)[0];
                XmlElement xmlRet = (XmlElement)((XmlElement)xmlDocumento).GetElementsByTagName(Constante.EsqCTeInutilizacaoRet)[0];

                string strAno = clsFacil.ObterItemXML(xmlEnv["infInut"]["ano"]);
                string strSerie = clsFacil.ObterItemXML(xmlEnv["infInut"]["serie"]);
                string strFaixaInicial = clsFacil.ObterItemXML(xmlEnv["infInut"]["nCTIni"]);
                string strFaixaFinal = clsFacil.ObterItemXML(xmlEnv["infInut"]["nCTFin"]);
                string strCNPJ = clsFacil.ObterItemXML(xmlEnv["infInut"]["CNPJ"]);
                string strCPF = clsFacil.ObterItemXML(xmlEnv["infInut"]["CPF"]);
                string strProtocolo = clsFacil.ObterItemXML(xmlRet["infInut"]["nProt"]);
                string strData = clsFacil.ObterItemXML(xmlRet["infInut"]["dhRecbto"]);
                string strCNPJCPFBase = string.Empty;
                string strCNPJCPFFilial = string.Empty;
                string strCNPJCPFDigito = string.Empty;

                // Verificando se o Ano tem 4 digitos
                if (strAno.Length != 4)
                {
                    strAno = (2000 + Convert.ToInt32(strAno)).ToString();
                }

                // Obtendo CNPJ/CPF do XML
                if (strCNPJ != string.Empty)
                {
                    strCNPJCPFBase = strCNPJ.Substring(0, 8);
                    strCNPJCPFFilial = strCNPJ.Substring(8, 4);
                    strCNPJCPFDigito = strCNPJ.Substring(12, 2);
                }
                else if (strCPF != string.Empty)
                {
                    strCNPJCPFBase = strCPF.Substring(0, 9);
                    strCNPJCPFFilial = "0";
                    strCNPJCPFDigito = strCPF.Substring(9, 2);
                }

                // Montando a chave
                string strChave = strAno + ";" + strSerie + ";" + strFaixaInicial + ";" + strFaixaFinal + ";" + strCNPJCPFBase + ";" + strCNPJCPFFilial + ";" + strCNPJCPFDigito;

                // Inserindo dados no banco
                clsBDSin.InserirInutilizacao(strAno, strSerie, strFaixaInicial, strFaixaFinal, strCNPJCPFBase, strCNPJCPFFilial, strCNPJCPFDigito, strProtocolo, strNSU, xmlDocumento.OuterXml, strData);

                // Registrando log de informacao
                clsLog.RegistrarLog(strMetodo, Constante.MsgDocInutilizacaoInseridoBanco + strNSU + ". Chave: " + strChave, EventLogEntryType.Information);
            }
            catch (Exception ex)
            {
                // Verificando o tipo da excecao
                if (ex.ToString().ToUpper().Contains("PRIMARY KEY") || ex.ToString().ToUpper().Contains("DUPLICATE KEY"))
                {
                    // Registrando log de informacao
                    clsLog.RegistrarLog(strMetodo, Constante.MsgDocJaExistente + strNSU, EventLogEntryType.Information);
                }
                else
                {
                    throw;
                }
            }
        }

        #endregion

        #region " SintetizarAutorizacaoGTV "

        public void SintetizarAutorizacaoGTV(XmlNode xmlDocumento, string strNSU)
        {
            // Obtendo nome do metodo
            string strMetodo = MethodBase.GetCurrentMethod().Name;

            try
            {
                // Obtendo elementos do XML
                XmlElement xmlRet = (XmlElement)((XmlElement)xmlDocumento).GetElementsByTagName(Constante.EsqGTVeAutorizacaoRet)[0];

                string strChaveAcesso = clsFacil.ObterItemXML(xmlRet["infProt"]["chCTe"]);
                string strProtocolo = clsFacil.ObterItemXML(xmlRet["infProt"]["nProt"]);
                string strData = clsFacil.ObterItemXML(xmlRet["infProt"]["dhRecbto"]);
                string strDataReferencia = "20" + strChaveAcesso.Substring(2, 2) + strChaveAcesso.Substring(4, 2);

                // Montando a chave
                string strChave = strDataReferencia + ";" + strChaveAcesso;

                // Inserindo dados no banco
                clsBDSin.InserirAutorizacao(strDataReferencia, strChaveAcesso, strProtocolo, strNSU, xmlDocumento.OuterXml, strData);

                // Registrando log de informacao
                clsLog.RegistrarLog(strMetodo, Constante.MsgDocAutorizacaoInseridoBanco + strNSU + ". Chave: " + strChave, EventLogEntryType.Information);
            }
            catch (Exception ex)
            {
                // Verificando o tipo da excecao
                if (ex.ToString().ToUpper().Contains("PRIMARY KEY") || ex.ToString().ToUpper().Contains("DUPLICATE KEY"))
                {
                    // Registrando log de informacao
                    clsLog.RegistrarLog(strMetodo, Constante.MsgDocJaExistente + strNSU, EventLogEntryType.Information);
                }
                else
                {
                    throw;
                }
            }
        }

        #endregion

        #region " SintetizarEventoGTV "

        public void SintetizarEventoGTV(XmlNode xmlDocumento, string strNSU)
        {
            // Obtendo nome do metodo
            string strMetodo = MethodBase.GetCurrentMethod().Name;

            try
            {
                // Obtendo elementos do XML
                XmlElement xmlEnv = (XmlElement)((XmlElement)xmlDocumento).GetElementsByTagName(Constante.EsqGTVeEventoEnv)[0];
                XmlElement xmlRet = (XmlElement)((XmlElement)xmlDocumento).GetElementsByTagName(Constante.EsqGTVeEventoRet)[0];

                string strSeqEvento = clsFacil.ObterItemXML(xmlEnv["infEvento"]["nSeqEvento"]);
                string strTipoEvento = clsFacil.ObterItemXML(xmlEnv["infEvento"]["tpEvento"]);
                string strChaveAcesso = clsFacil.ObterItemXML(xmlRet["infEvento"]["chCTe"]);
                string strProtocolo = clsFacil.ObterItemXML(xmlRet["infEvento"]["nProt"]);
                string strData = clsFacil.ObterItemXML(xmlRet["infEvento"]["dhRegEvento"]);
                string strDataReferencia = "20" + strChaveAcesso.Substring(2, 2) + strChaveAcesso.Substring(4, 2);

                // Montando a chave
                string strChave = strDataReferencia + ";" + strChaveAcesso + ";" + strSeqEvento + ";" + strTipoEvento;

                // Verificando o tipo de evento
                if (strTipoEvento != ((int)Constante.TipoEvento.Referenciada).ToString())
                {
                    // Inserindo dados no banco
                    clsBDSin.InserirEvento(strDataReferencia, strChaveAcesso, strTipoEvento, strSeqEvento, strProtocolo, strNSU, xmlDocumento.OuterXml, strData);
                }
                else
                {
                    // Obtendo a chave referenciada
                    string strChaveAcessoRef = clsFacil.ObterItemXML(xmlEnv["infEvento"]["detEvento"]["chNFeRefte"]);

                    // Inserindo dados no banco
                    clsBDSin.InserirEventoRef(strDataReferencia, strChaveAcesso, strChaveAcessoRef, strTipoEvento, strSeqEvento, strProtocolo, strNSU, xmlDocumento.OuterXml, strData);
                }

                // Registrando log de informacao
                clsLog.RegistrarLog(strMetodo, Constante.MsgDocEventoInseridoBanco + strNSU + ". Chave: " + strChave, EventLogEntryType.Information);
            }
            catch (Exception ex)
            {
                // Verificando o tipo da excecao
                if (ex.ToString().ToUpper().Contains("PRIMARY KEY") || ex.ToString().ToUpper().Contains("DUPLICATE KEY"))
                {
                    // Registrando log de informacao
                    clsLog.RegistrarLog(strMetodo, Constante.MsgDocJaExistente + strNSU, EventLogEntryType.Information);
                }
                else
                {
                    throw;
                }
            }
        }

        #endregion

        #region " SintetizarInutilizacaoGTV "

        public void SintetizarInutilizacaoGTV(XmlNode xmlDocumento, string strNSU)
        {
            // Obtendo nome do metodo
            string strMetodo = MethodBase.GetCurrentMethod().Name;

            try
            {
                // Obtendo elementos do XML
                XmlElement xmlEnv = (XmlElement)((XmlElement)xmlDocumento).GetElementsByTagName(Constante.EsqGTVeInutilizacaoEnv)[0];
                XmlElement xmlRet = (XmlElement)((XmlElement)xmlDocumento).GetElementsByTagName(Constante.EsqGTVeInutilizacaoRet)[0];

                string strAno = clsFacil.ObterItemXML(xmlEnv["infInut"]["ano"]);
                string strSerie = clsFacil.ObterItemXML(xmlEnv["infInut"]["serie"]);
                string strFaixaInicial = clsFacil.ObterItemXML(xmlEnv["infInut"]["nCTIni"]);
                string strFaixaFinal = clsFacil.ObterItemXML(xmlEnv["infInut"]["nCTFin"]);
                string strCNPJ = clsFacil.ObterItemXML(xmlEnv["infInut"]["CNPJ"]);
                string strCPF = clsFacil.ObterItemXML(xmlEnv["infInut"]["CPF"]);
                string strProtocolo = clsFacil.ObterItemXML(xmlRet["infInut"]["nProt"]);
                string strData = clsFacil.ObterItemXML(xmlRet["infInut"]["dhRecbto"]);
                string strCNPJCPFBase = string.Empty;
                string strCNPJCPFFilial = string.Empty;
                string strCNPJCPFDigito = string.Empty;

                // Verificando se o Ano tem 4 digitos
                if (strAno.Length != 4)
                {
                    strAno = (2000 + Convert.ToInt32(strAno)).ToString();
                }

                // Obtendo CNPJ/CPF do XML
                if (strCNPJ != string.Empty)
                {
                    strCNPJCPFBase = strCNPJ.Substring(0, 8);
                    strCNPJCPFFilial = strCNPJ.Substring(8, 4);
                    strCNPJCPFDigito = strCNPJ.Substring(12, 2);
                }
                else if (strCPF != string.Empty)
                {
                    strCNPJCPFBase = strCPF.Substring(0, 9);
                    strCNPJCPFFilial = "0";
                    strCNPJCPFDigito = strCPF.Substring(9, 2);
                }

                // Montando a chave
                string strChave = strAno + ";" + strSerie + ";" + strFaixaInicial + ";" + strFaixaFinal + ";" + strCNPJCPFBase + ";" + strCNPJCPFFilial + ";" + strCNPJCPFDigito;

                // Inserindo dados no banco
                clsBDSin.InserirInutilizacao(strAno, strSerie, strFaixaInicial, strFaixaFinal, strCNPJCPFBase, strCNPJCPFFilial, strCNPJCPFDigito, strProtocolo, strNSU, xmlDocumento.OuterXml, strData);

                // Registrando log de informacao
                clsLog.RegistrarLog(strMetodo, Constante.MsgDocInutilizacaoInseridoBanco + strNSU + ". Chave: " + strChave, EventLogEntryType.Information);
            }
            catch (Exception ex)
            {
                // Verificando o tipo da excecao
                if (ex.ToString().ToUpper().Contains("PRIMARY KEY") || ex.ToString().ToUpper().Contains("DUPLICATE KEY"))
                {
                    // Registrando log de informacao
                    clsLog.RegistrarLog(strMetodo, Constante.MsgDocJaExistente + strNSU, EventLogEntryType.Information);
                }
                else
                {
                    throw;
                }
            }
        }

        #endregion

        #region " EnviarFilaSintetizador "

        public void EnviarFilaSintetizador(XmlNode xmlDocumento, string strNSU, string strNSUFinal, string strProtocolo, string strQtde, string strEsquema, string strData)
        {
            // Obtendo nome do metodo
            string strMetodo = MethodBase.GetCurrentMethod().Name;

            try
            {
                // Inserindo dados no banco
                clsBDSin.InserirTempFilaSintetizador(strNSU, strNSUFinal, strProtocolo, strQtde, xmlDocumento.OuterXml, strEsquema, strData);

                // Registrando log de informacao
                clsLog.RegistrarLog(strMetodo, Constante.MsgLoteInseridoBanco + strNSU, EventLogEntryType.Information);

                // Enviando chave para fila
                clsBDSin.EnviarFilaSintetizador(strNSU);

                // Registrando log de informacao
                clsLog.RegistrarLog(strMetodo, Constante.MsgLoteInseridoFila + strNSU, EventLogEntryType.Information);
            }
            catch (Exception ex)
            {
                // Verificando o tipo da excecao
                if (ex.ToString().ToUpper().Contains("PRIMARY KEY") || ex.ToString().ToUpper().Contains("DUPLICATE KEY"))
                {
                    // Registrando log de alerta
                    clsLog.RegistrarLog(strMetodo, Constante.MsgLoteJaExistente + strNSU, EventLogEntryType.Information);
                }
                else
                {
                    throw;
                }
            }
        }

        #endregion

        #region " EnviarFilaAnalisador "

        public void EnviarFilaAnalisador(XmlNode xmlDocumento, string strNSU, string strProtocolo, string strEsquema, string strData)
        {
            // Obtendo nome do metodo
            string strMetodo = MethodBase.GetCurrentMethod().Name;

            try
            {
                // Inserindo dados no banco
                clsBDSin.InserirTempFilaAnalisador(strNSU, strProtocolo, xmlDocumento.OuterXml, strEsquema, strData);

                // Registrando log de informacao
                clsLog.RegistrarLog(strMetodo, Constante.MsgLoteInseridoBanco + strNSU, EventLogEntryType.Information);

                // Enviando chave para fila
                clsBDSin.EnviarFilaAnalisador(strNSU);

                // Registrando log de informacao
                clsLog.RegistrarLog(strMetodo, Constante.MsgLoteInseridoFila + strNSU, EventLogEntryType.Information);
            }
            catch (Exception ex)
            {
                // Verificando o tipo da excecao
                if (ex.ToString().ToUpper().Contains("PRIMARY KEY") || ex.ToString().ToUpper().Contains("DUPLICATE KEY"))
                {
                    // Registrando log de alerta
                    clsLog.RegistrarLog(strMetodo, Constante.MsgLoteJaExistente + strNSU, EventLogEntryType.Information);
                }
                else
                {
                    throw;
                }
            }
        }

        #endregion

        #region " EnviarFilaIntegrador "

        public void EnviarFilaIntegrador(XmlNode xmlDocumento, string strNSU, string strNSUFinal, string strQtde, string strEsquema, string strData)
        {
            // Obtendo nome do metodo
            string strMetodo = MethodBase.GetCurrentMethod().Name;

            try
            {
                // Inserindo dados no banco
                clsBDSin.InserirTempFilaIntegrador(strNSU, strNSUFinal, strQtde, xmlDocumento.OuterXml, strEsquema, strData);

                // Registrando log de informacao
                clsLog.RegistrarLog(strMetodo, Constante.MsgLoteInseridoBanco + strNSU, EventLogEntryType.Information);

                // Enviando chave para fila
                clsBDSin.EnviarFilaIntegrador(strNSU);

                // Registrando log de informacao
                clsLog.RegistrarLog(strMetodo, Constante.MsgLoteInseridoFila + strNSU, EventLogEntryType.Information);
            }
            catch (Exception ex)
            {
                // Verificando o tipo da excecao
                if (ex.ToString().ToUpper().Contains("PRIMARY KEY") || ex.ToString().ToUpper().Contains("DUPLICATE KEY"))
                {
                    // Registrando log de alerta
                    clsLog.RegistrarLog(strMetodo, Constante.MsgLoteJaExistente + strNSU, EventLogEntryType.Information);
                }
                else
                {
                    throw;
                }
            }
        }

        #endregion

        #region " InserirNSUFaltante "

        private void InserirNSUFaltante(string strNSU)
        {
            // Obtendo nome do metodo
            string strMetodo = MethodBase.GetCurrentMethod().Name;

            try
            {
                // Inserindo dados no banco
                clsBDSin.InserirNSUFaltante(strNSU);

                // Registrando log de informacao
                clsLog.RegistrarLog(strMetodo, Constante.MsgNSUFaltanteInserido + strNSU, EventLogEntryType.Information);
            }
            catch (Exception ex)
            {
                // Verificando o tipo da excecao
                if (ex.ToString().ToUpper().Contains("PRIMARY KEY") || ex.ToString().ToUpper().Contains("DUPLICATE KEY"))
                {
                    // Registrando log de informacao
                    clsLog.RegistrarLog(strMetodo, Constante.MsgNSUFaltanteJaExistente + strNSU, EventLogEntryType.Information);
                }
                else
                {
                    throw;
                }
            }
        }

        #endregion
    }
}