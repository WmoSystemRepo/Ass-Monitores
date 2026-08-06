using System;
using System.Data;
using System.Diagnostics;
using System.Reflection;
using System.Xml;
using System.Net;

namespace DFe
{
    class SerCTeCarga
    {
        #region " Variaveis "

        // Classes utilizadas
        private Facilitador clsFacil;
        private Log clsLog;
        private BdCTeRecepcao clsBDRec;
        private BdCTeSintetico clsBDSin;
        private BdCTeAnalitico clsBDAna;
        private BdCTeHistorico clsBDHis;
        private NegCTe clsNeg;
        private NegCTeSintetico clsNegSin;
        private NegCTeAnalitico clsNegAna;

        // Variaveis utilizadas
        private string strClasse;
        private string strNomeServico;
        private readonly short intCodServico;
        private readonly short intThread;
        private short intLogEvento;
        private short intLogBanco;
        private short intLogCompleto;
        private short intExecutar;
        private short intExecutarAuto;
        private short intExecutarEven;
        private short intExecutarInut;
        private int intAno;
        private readonly string strCertificadoDigital;
        private readonly string strWSCTeConsulta;
        private readonly short intTipoAmbiente;

        #endregion

        #region " Construtores "

        public SerCTeCarga(Facilitador clsFacilPar, string strBDCTeRecepcaoPar, string strBDCTeSinteticoPar, string strBDCTeAnaliticoPar, string strBDNFeHistoricoPar, string strCertificadoDigitalPar, string strWSCTeConsultaPar, short intTipoAmbientePar, short intCodServicoPar, short intThreadPar)
        {
            // Inicializando classes
            clsFacil = clsFacilPar;

            // Inicializando variaveis
            strClasse = this.GetType().Name;
            intCodServico = intCodServicoPar;
            intThread = intThreadPar;
            strCertificadoDigital = strCertificadoDigitalPar;
            strWSCTeConsulta = strWSCTeConsultaPar;
            intTipoAmbiente = intTipoAmbientePar;

            // Inicializando classes de banco
            clsBDRec = new BdCTeRecepcao(clsFacil, strBDCTeRecepcaoPar);
            clsBDSin = new BdCTeSintetico(clsFacil, strBDCTeSinteticoPar);
            clsBDAna = new BdCTeAnalitico(clsFacil, strBDCTeAnaliticoPar);
            clsBDHis = new BdCTeHistorico(clsFacil, strBDNFeHistoricoPar);

            //Criptografia clsCript = new Criptografia();
            //clsBDSin = new BdCTeSintetico(clsFacil, clsCript.Decriptar("Ivkn5oBiAYyCw4lP1BDuoJKMewG949yLlEcyxniegyb5xYnxaNNxk7mRdnuZeZ+7SBiqD49RrPYmIw2JgibqcdDJzVrJhBrJLHUyKY1dUO9jI2fAXoik4o+Qgi1jsRJYWCJSW02iti8/mdNrt4d9qHisny5yADtjOGwhylzx82E=goub5BobIBSSkMsng6kVyEA8l0GDLb2kMHifF+O2C5iRe0P9czmlotL0LhVJpHc7cgIwfhAuMrqwtRZ7Y0e1hgSgcYZq8Ij4cbgmt9dg1xTY+9hOq4bU1issuWus/RnaL8ymbqUVWw2foq27u32IQX1sxL4wifLs6LayWEzY4nM=egr9aRRCQlERrfrqAMZB03iwv2mu0is8PdI/BaXdQNMUD4m5Ou4vHsBGyckq1fN2xlPmyGKWgHlnZOSjJcrJSvNqf1bEHL3VrJyPK7TA5d02MlzOwlTkPu6oUkkNNcvOCSn3sJ0DzcHCJ/igA7EO4+BbwiNxLPXuZuSHirVG/ZQ=sSlVjwaulyKVVnHxCKgGnIqH3DzsjpPRuW10uI0EfwdGfqP7rqyX9aoiPpblKBahA6WIs635LYwhiCl+aGebsJW/GNDA+QdqqnsIMSGCW0EPG7kjfIsXu2c7LhsEtRbic55yA2htceIOS3NzElWUMDBI53zBPVBizmXPtGCQu1o=O6TZWtxqu8VYE1FiRu461XJkJkLesVrGfV4n43y/ejfm4n/eUM1pHoyryAYGIC4cvN4fvuzhy35VLiBANkuQ5g7igI6EB6REPYRkBpx6qUKd9rnVcCR2ZiON6EBEke4QC0ZIQDdp4GeVY8ycuTLQaldOHyHRQxHVWZfsUNdGnh0=kaJVxSihDpHT0Q1aSp5ynNNAhEqPk+01cZyYUuOxpWiQbJTzF2TlNZJF91mRrgXEiuYPmASVHfLL2TrZws8BHFETWyF5afZDFpMxTMOHkMFC1pSvCgfXE/BaDseScPqV35s9btioHKUpl2/cgVRBZlGHNy+cYdsceNDRMFX+EA8=/1JkIosYDWbvnCkYGtCkbWnaQXT3cW90sW5i0t5GceWJXophvEfXF0LlUzTlGz/OVfQ+yOlYESFAseMOXjC6iAKpMMoao76mYXcmrgUWm8iR0P+AfD6QQya4KVCJZEPhcGUYtO93EN374mfsiZf52vzFCUPaitiJVacw8XwWaD4="));
            //clsBDSin = new BdCTeSintetico(clsFacil, clsCript.Decriptar("9hCM7OA6I6tdS2A8fd//gJrXkJwT84n0gWFNLtUDaRtrAXjYf2ZNMHA9xyKrYz54g+HB9kzmaC5NBrWmm6l5HY96yYnQg8F1NskO8dAW1uWyvuLvdb2BcElp8mLugHQqj30BRjqvctCXQGrRa+GBLG8laq/0TeEGyXgGZoB34yM=A9CLSECGuQ5NgZ7HlEJzhGjhkdUD+5MJzQjS7z/IV25dDqGUIeP1x6M1HKPlU+p6fw1n91l077+SREVy/eQgc0tYgN7uEw8lalwxlk50RH53qgaeTM1e5uxu5HSbdtsr9e9LMKz17SZwXSW1cDDncV8U7z0c7sBaHK9783nbGUA=/QUUSl9jpDrOn0v8PZplO8pBfcHfudvolBQ9CRll55O1nQkrmc9bZYWRdz6Fz+UUaX4Vtiu83w7Gd3sRaP5i6z1Il5x/Lmi7xuBBuNXgzvVfe6JPM9BeWDaG/x0CkVZwD6eDIWDkB3JmFi/gRU5cWqoBhNM11z9ulHDDXEmQFiw=zg4CGC941AEioocTfajKlYxmvzM2YoD5sdoTr95nk8Lwg+aaIIg5m/ywjoy1ZVWNcV7dqX8yuj1AoBslxettoIbUZDtliaPT6OoEUGGxVmW5cVnytwBVxfQciLEF6qm8PD78jzLXrLaqIIAFf/sgVOv4kgBPKbQ3nJet2M2nxws=quWt5f041ovhTD0SBxSgbNRF3oPc+YeCwIB1c9OkLrw3vVxwxuXeZKT+4PoInPRTXG0bePMwFrS5SZBAo8bP2cgDnvdKBZRItu0t1p8Y9iqS1kWF+V3h9T+rJ9zw1CGwyCqgQYKZ72//5K8L0BhwF5ksyC5kh/CyH2ZXaaGQrHw=zKdnkK/3Tr8Be5sIk+GaHHO36j7417ZIbMaACjIHX21KNhAgRghtSxsl+fegbr0FXmwG2R7M1MRI1aDCls9GK9ngjjsMiJJyDzJalYYN8TerXZOMIGGkX/Ps2vQGcCSzXX4ei+E9ZcQkn8N5lYNIsi5bJc2ngoYy9gKyO1nX7Z8=nP58wBJ7/Uy7pRC02idCRqLd2zHGc6SsoSCXS50FjerU+823/FXuIrt/ivAverl+jqB9UI9NsWvqn6vkYZj1JmowDUZzCoWwI6PJZsSMkhKjj/2xTKzm7WNkisWdKwRdiU71B65HDprjedhaa6zvl2/qIlZPoGIfCIrQwrT3eFU="));

            // Obtendo configuracao do banco
            this.ObterConfigBanco();

            // Inicializando classes de negocio
            clsLog = new Log(clsFacil, strBDCTeRecepcaoPar, strClasse, strNomeServico, intCodServico, intThread, intLogEvento, intLogBanco, intLogCompleto);
            clsNeg = new NegCTe(clsFacil);
            clsNegSin = new NegCTeSintetico(clsFacil, clsLog, clsBDSin);
            clsNegAna = new NegCTeAnalitico(clsFacil, clsLog, clsBDAna);
        }

        #endregion

        #region " ObterConfigBanco "

        protected void ObterConfigBanco()
        {
            try
            {
                // Obtendo configuracao do banco
                strNomeServico = Convert.ToString(clsFacil.ValidarItemConfigBanco(clsBDRec.ObterServico(intCodServico.ToString()), "NomeServico"));
                intLogEvento = Convert.ToInt16(clsFacil.ValidarItemConfigBanco(clsBDRec.ObterConfiguracao(intCodServico.ToString(), "LogEvento"), "LogEvento"));
                intLogBanco = Convert.ToInt16(clsFacil.ValidarItemConfigBanco(clsBDRec.ObterConfiguracao(intCodServico.ToString(), "LogBanco"), "LogBanco"));
                intLogCompleto = Convert.ToInt16(clsFacil.ValidarItemConfigBanco(clsBDRec.ObterConfiguracao(intCodServico.ToString(), "LogCompleto"), "LogCompleto"));
                intExecutar = Convert.ToInt16(clsFacil.ValidarItemConfigBanco(clsBDRec.ObterConfiguracao(intCodServico.ToString(), "Executar"), "Executar"));
                intExecutarAuto = Convert.ToInt16(clsFacil.ValidarItemConfigBanco(clsBDRec.ObterConfiguracao(intCodServico.ToString(), "ExecutarAuto"), "ExecutarAuto"));
                intExecutarEven = Convert.ToInt16(clsFacil.ValidarItemConfigBanco(clsBDRec.ObterConfiguracao(intCodServico.ToString(), "ExecutarEven"), "ExecutarEven"));
                intExecutarInut = Convert.ToInt16(clsFacil.ValidarItemConfigBanco(clsBDRec.ObterConfiguracao(intCodServico.ToString(), "ExecutarInut"), "ExecutarInut"));
                intAno = Convert.ToInt32(clsFacil.ValidarItemConfigBanco(clsBDRec.ObterConfiguracao(intCodServico.ToString(), "Ano"), "Ano"));
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region " Iniciar "

        public void Iniciar(ref DateTime datUltimaExecucao)
        {
            // Obtendo nome do metodo
            string strMetodo = MethodBase.GetCurrentMethod().Name;

            try
            {
                // Verificando se deve executar
                if (intExecutar == 1)
                {
                    // Registrando log de informacao
                    clsLog.RegistrarLog(strMetodo, Constante.MsgProcessoIniciado, EventLogEntryType.Information);

                    // Processando
                    //this.Processar();

                    // Processando só download
                    if (intExecutarAuto == 1)
                    {
                        // Processando enquanto houver retorno
                        bool bolExecutar = true;
                        while (bolExecutar)
                        {
                            bolExecutar = this.ProcessarDownload();
                        }
                    }
                }
                else
                {
                    // Registrando log de informacao
                    clsLog.RegistrarLog(strMetodo, Constante.MsgProcessoNaoIniciado, EventLogEntryType.Information);
                }
            }
            catch (Exception ex)
            {
                try
                {
                    string strErro = "SerCTeCarga.Iniciar ERRO: " + ex.ToString();
                    System.Diagnostics.Debug.WriteLine(strErro);
                    string strDesktop = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "DFEND_CTe_Carga_erro.txt");
                    System.IO.File.WriteAllText(strDesktop, strErro);
                    System.Diagnostics.Debug.WriteLine("Erro gravado em: " + strDesktop);
                }
                catch
                {
                }
                try
                {
                    clsLog.RegistrarLog(ex);
                }
                catch (Exception exLog)
                {
                    System.Diagnostics.Debug.WriteLine("Falha ao registrar log: " + exLog.Message);
                }
                throw;
            }
        }

        #endregion

        #region " Processar "

        public void Processar()
        {
            // Obtendo nome do metodo
            string strMetodo = MethodBase.GetCurrentMethod().Name;

            try
            {
                // Montando variaveis utilizadas
                bool bolRetornoAuto = false;
                bool bolRetornoEven = false;
                bool bolRetornoInut = false;
                string strAnoInicial = intAno.ToString();
                string strAnoFinal = intAno.ToString();
                string strMesInicial = intThread.ToString().PadLeft(2, '0');
                string strMesFinal = (intThread + 1).ToString().PadLeft(2, '0');

                if (strMesInicial == "12")
                {
                    strMesFinal = "01";
                    strAnoFinal = (intAno + 1).ToString();
                }

                // Montando periodos de consulta
                string strDataReferencia = strAnoInicial + strMesInicial;
                string strDataInicial = strAnoInicial + "-" + strMesInicial + "-" + "01";
                string strDataFinal = strAnoFinal + "-" + strMesFinal + "-" + "01";

                // Transferindo Evento
                if (intExecutarEven == 1)
                {
                    bolRetornoEven = this.MigrarEvento(strDataInicial, strDataFinal);
                }

                // Transferindo Autorizacao
                if (intExecutarAuto == 1)
                {
                    bolRetornoAuto = this.MigrarAutorizacao(strDataInicial, strDataFinal);
                }

                // Transferindo Inutilizacao
                if (intExecutarInut == 1)
                {
                    bolRetornoInut = false;
                }

                // Verificando se houve alguma transferencia para a thread
                if ((bolRetornoAuto == false) && (bolRetornoEven == false) && (bolRetornoInut == false))
                {
                    while (!bolRetornoAuto)
                    {
                        bolRetornoAuto = false;
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region " MigrarAutorizacao "

        protected bool MigrarAutorizacao(string strDataInicial, string strDataFinal)
        {
            // Classes e variaveis utilizadas
            bool bolRetorno = false;

            // Obtendo nome do metodo
            string strMetodo = MethodBase.GetCurrentMethod().Name;

            try
            {
                // Obtendo Autorizacao no banco Historico
                DataSet dstDados = clsBDHis.ObterCTeAutorizacaoParaCarga(strDataInicial, strDataFinal);

                // Verificando se houve retorno
                if ((dstDados != null) && (dstDados.Tables.Count > 0) && (dstDados.Tables[0].Rows.Count > 0))
                {
                    // Informando que encontrou registro
                    bolRetorno = true;

                    // Obtendo dados do retorno
                    string strChaveAcesso = dstDados.Tables[0].Rows[0]["cod_chave_acesso"].ToString();
                    string strXMLPedido = dstDados.Tables[0].Rows[0]["xml_pedido"].ToString();
                    string strXMLResposta = dstDados.Tables[0].Rows[0]["xml_resposta"].ToString();
                    string strNSU = (dstDados.Tables[0].Rows[0]["nsu"].ToString().Trim()).PadLeft(15, '0');
                    string strProtocolo = dstDados.Tables[0].Rows[0]["num_protocolo"].ToString().Trim();
                    string strVersao = dstDados.Tables[0].Rows[0]["num_versao_xml"].ToString().Trim();
                    string strIP = string.Empty; //dstDados.Tables[0].Rows[0]["des_endereco_logico"].ToString().Trim();
                    string strDataDocumento = dstDados.Tables[0].Rows[0]["dtc_documento"].ToString();
                    string strQtde = dstDados.Tables[0].Rows.Count.ToString();

                    // Registrando log de informacao
                    clsLog.RegistrarLog(strMetodo, Constante.MsgLoteObtidoBanco + strNSU, EventLogEntryType.Information);

                    // Montando xml do lote
                    XmlDocument xmlLote = clsNeg.MontarXMLLote(strXMLPedido, strXMLResposta, strVersao, clsFacil.MontarEsquema(Constante.EsqCTeAutorizacaoSchema, strVersao), strNSU, strIP);

                    // Inserindo dados no banco
                    clsBDRec.InserirTempArquivador(strNSU, strNSU, strProtocolo, strQtde, xmlLote.OuterXml, Constante.EsqCTeAutorizacaoSchema, strDataDocumento);

                    // Registrando log de informacao
                    clsLog.RegistrarLog(strMetodo, Constante.MsgLoteInseridoBanco + strNSU, EventLogEntryType.Information);

                    // Enviando chave para fila
                    clsBDRec.EnviarFilaArquivador(strNSU);

                    // Registrando log de informacao
                    clsLog.RegistrarLog(strMetodo, Constante.MsgLoteInseridoFila + strNSU, EventLogEntryType.Information);

                    // Excluindo documento do banco Historico
                    clsBDHis.ExcluirCTeAutorizacao(strChaveAcesso);

                    // Registrando log de informacao
                    clsLog.RegistrarLog(strMetodo, Constante.MsgDocExcluido + strNSU, EventLogEntryType.Information);
                }
            }
            catch
            {
                throw;
            }

            return bolRetorno;
        }

        #endregion

        #region " MigrarEvento "

        protected bool MigrarEvento(string strDataInicial, string strDataFinal)
        {
            // Classes e variaveis utilizadas
            bool bolRetorno = false;

            // Obtendo nome do metodo
            string strMetodo = MethodBase.GetCurrentMethod().Name;

            try
            {
                // Obtendo Evento no banco Historico
                DataSet dstDados = clsBDHis.ObterCTeEventoParaCarga(strDataInicial, strDataFinal);

                // Verificando se houve retorno
                if ((dstDados != null) && (dstDados.Tables.Count > 0) && (dstDados.Tables[0].Rows.Count > 0))
                {
                    // Informando que encontrou registro
                    bolRetorno = true;

                    // Obtendo dados do retorno
                    string strChaveAcesso = dstDados.Tables[0].Rows[0]["cod_chave_acesso"].ToString();
                    string strTipo = dstDados.Tables[0].Rows[0]["cod_tipo_evento"].ToString();
                    string strSeq = dstDados.Tables[0].Rows[0]["seq_evento"].ToString();
                    string strXMLPedido = dstDados.Tables[0].Rows[0]["xml_pedido"].ToString();
                    string strXMLResposta = dstDados.Tables[0].Rows[0]["xml_resposta"].ToString();
                    string strNSU = (dstDados.Tables[0].Rows[0]["nsu"].ToString().Trim()).PadLeft(15, '0');
                    string strProtocolo = dstDados.Tables[0].Rows[0]["num_protocolo"].ToString().Trim();
                    string strVersao = dstDados.Tables[0].Rows[0]["num_versao_xml"].ToString().Trim();
                    string strIP = string.Empty; //dstDados.Tables[0].Rows[0]["des_endereco_logico"].ToString().Trim();
                    string strDataDocumento = dstDados.Tables[0].Rows[0]["dtc_documento"].ToString();
                    string strQtde = dstDados.Tables[0].Rows.Count.ToString();

                    // Registrando log de informacao
                    clsLog.RegistrarLog(strMetodo, Constante.MsgLoteObtidoBanco + strNSU, EventLogEntryType.Information);

                    // Montando xml do lote
                    XmlDocument xmlLote = clsNeg.MontarXMLLote(strXMLPedido, strXMLResposta, strVersao, clsFacil.MontarEsquema(Constante.EsqCTeEventoSchema, strVersao), strNSU, strIP);

                    // Inserindo dados no banco
                    clsBDRec.InserirTempArquivador(strNSU, strNSU, strProtocolo, strQtde, xmlLote.OuterXml, Constante.EsqCTeEventoSchema, strDataDocumento);

                    // Registrando log de informacao
                    clsLog.RegistrarLog(strMetodo, Constante.MsgLoteInseridoBanco + strNSU, EventLogEntryType.Information);

                    // Enviando chave para fila
                    clsBDRec.EnviarFilaArquivador(strNSU);

                    // Registrando log de informacao
                    clsLog.RegistrarLog(strMetodo, Constante.MsgLoteInseridoFila + strNSU, EventLogEntryType.Information);

                    // Excluindo documento do banco Historico
                    clsBDHis.ExcluirCTeEvento(strChaveAcesso, strTipo, strSeq);

                    // Registrando log de informacao
                    clsLog.RegistrarLog(strMetodo, Constante.MsgDocExcluido + strNSU, EventLogEntryType.Information);
                }
            }
            catch
            {
                throw;
            }

            return bolRetorno;
        }

        #endregion


        #region " ProcessarDownload "

        public bool ProcessarDownload()
        {
            // Classes e variaveis utilizadas
            string strChave = string.Empty;
            bool bolRetorno = false;

            // Obtendo nome do metodo
            string strMetodo = MethodBase.GetCurrentMethod().Name;

            try
            {
                // Retirando chave da fila
                strChave = clsBDSin.RetirarFilaIntegrador();

                // Verificando se existe chave
                if (strChave != string.Empty)
                {
                    // Registrando log de informacao
                    clsLog.RegistrarLog(strMetodo, Constante.MsgChaveRetiradaFila + strChave, EventLogEntryType.Information);

                    // Informando que encontrou item
                    bolRetorno = true;

                    // Obtendo dados do banco
                    DataSet dstDados = clsBDSin.ObterTempFilaIntegrador(strChave);

                    // Verificando se houve retorno
                    if ((dstDados != null) && (dstDados.Tables.Count > 0) && (dstDados.Tables[0].Rows.Count > 0))
                    {
                        // Obtendo dados do retorno
                        string strChaveAcesso = dstDados.Tables[0].Rows[0]["des_esquema"].ToString();

                        // Configurando o WebService (certificado / URL / tpAmb via App.config)
                        wsvCTeConsultaDFe.cteConsultaDFe wsvWebService = this.ConfigurarWSConsulta(strCertificadoDigital, strWSCTeConsulta, 60000, "1.00");

                        // Registrando log de informacao
                        clsLog.RegistrarLog(strMetodo, Constante.MsgWSConfigurado, EventLogEntryType.Information);

                        // Montando xml de envio
                        XmlDocument xmlEnvio = clsNeg.MontarXMLConsulta(strChaveAcesso, intTipoAmbiente, "CONSULTAR", "1.00");

                        // Obtendo retorno do WebService
                        XmlElement xmlRetorno = (XmlElement)wsvWebService.cteConsDFe(xmlEnvio.LastChild);

                        // Obtendo elementos do XML
                        string strStatus = xmlRetorno["cStat"].InnerText;
                        string strMotivo = xmlRetorno["xMotivo"].InnerText;
                        string strMensagemRetorno = "Status: " + strStatus + ". Retorno: " + strMotivo;

                        // Registrando log de informacao
                        //clsLog.RegistrarLog(strMetodo, strChave + " - " + strMensagemRetorno, EventLogEntryType.Warning);

                        // Verificando o status do retorno
                        if ((strStatus == ((short)Constante.TipoMensagem.Msg_129_NFeAutorizada).ToString()) || (strStatus == ((short)Constante.TipoMensagem.Msg_130_NFeDenegada).ToString()) || (strStatus == ((short)Constante.TipoMensagem.Msg_131_NFeCancelada).ToString()))
                        {
                            // Obtendo lista de documentos
                            XmlNodeList xmlLote;
                            xmlLote = xmlRetorno["CTeDFe"].ChildNodes;

                            // Percorrendo a lista de itens retornados
                            foreach (XmlNode xmlItem in xmlLote)
                            {
                                // Obtendo elementos do XML
                                string strXML = xmlItem.OuterXml;
                                string strVersao = xmlItem.Attributes["versao"].InnerText;
                                string strEsquema = clsFacil.MontarEsquema(xmlItem.Name, strVersao);
                                string strNSU = "0";

                                // Montando xml de envio
                                XmlDocument xmlProc = clsNeg.MontarXMLProc(strXML, strEsquema, strNSU);

                                // Verificando o tipo do schema retornado
                                if (strEsquema.StartsWith(Constante.EsqCTeAutorizacaoSchema))
                                {
                                    // Sintetizando documento de Autorizacao
                                    this.SintetizarAutorizacaoCTe(strNSU, xmlProc.FirstChild);
                                }
                                else if (strEsquema.StartsWith(Constante.EsqCTeEventoSchema))
                                {
                                    // Sintetizando documento de Evento
                                    this.SintetizarEventoCTe(strNSU, xmlProc.FirstChild);
                                }
                                else if (strEsquema.StartsWith(Constante.EsqCTeInutilizacaoSchema))
                                {
                                    // Sintetizando documento de Inutilizacao
                                    this.SintetizarInutilizacaoCTe(strNSU, xmlProc.FirstChild);
                                }
                                else if (strEsquema.StartsWith(Constante.EsqCTeOSAutorizacaoProc))
                                {
                                    // Sintetizando documento de Autorizacao
                                    this.SintetizarAutorizacaoCTeOS(strNSU, xmlProc.FirstChild);
                                }
                                else if (strEsquema.StartsWith(Constante.EsqGTVeAutorizacaoProc))
                                {
                                    // Sintetizando documento de Autorizacao GTV
                                    this.SintetizarAutorizacaoGTV(strNSU, xmlProc.FirstChild);
                                }
                                else if (strEsquema.StartsWith(Constante.EsqGTVeEventoProc))
                                {
                                    // Sintetizando documento de Evento GTV
                                    this.SintetizarEventoGTV(strNSU, xmlProc.FirstChild);
                                }
                                else if (strEsquema.StartsWith(Constante.EsqGTVeInutilizacaoProc))
                                {
                                    // Sintetizando documento de Inutilizacao GTV
                                    this.SintetizarInutilizacaoGTV(strNSU, xmlProc.FirstChild);
                                }
                                else
                                {
                                    // Levantando excecao
                                    throw new Exception(strChaveAcesso + " - " + strMensagemRetorno + " - " + Constante.MsgLoteElementoNaoEsperado + strEsquema);
                                }
                            }

                            // Registrando log de informacao
                            clsLog.RegistrarLog(strMetodo, strChaveAcesso + " - " + strMensagemRetorno + " - " + "Todos os DFe dessa chave inseridos com sucesso", EventLogEntryType.Information);

                            // Excluindo documento analisado
                            this.ExcluirLote(strChave);
                        }
                        else
                        {
                            // Registrando log de alerta
                            clsLog.RegistrarLog(strMetodo, strChaveAcesso + " - " + strMensagemRetorno + " - " + Constante.MsgWSRetornoNaoEsperado + strMensagemRetorno, EventLogEntryType.Warning);
                        }
                    }
                }
            }
            catch
            {
                // Enviando chave para fila
                clsBDSin.EnviarFilaIntegrador(strChave);
                throw;
            }

            return bolRetorno;
        }

        #endregion

        #region " SintetizarAutorizacaoCTe "

        private void SintetizarAutorizacaoCTe(string strNSU, XmlNode xmlDocumento)
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

        #region " SintetizarEventoCTe "

        private void SintetizarEventoCTe(string strNSU, XmlNode xmlDocumento)
        {
            // Obtendo nome do metodo
            string strMetodo = MethodBase.GetCurrentMethod().Name;

            try
            {
                // Obtendo elementos do XML
                XmlElement xmlEnv = (XmlElement)((XmlElement)xmlDocumento).GetElementsByTagName(Constante.EsqCTeEventoEnv)[0];
                XmlElement xmlRet = (XmlElement)((XmlElement)xmlDocumento).GetElementsByTagName(Constante.EsqCTeEventoRet)[0];

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

        #region " SintetizarInutilizacaoCTe "

        private void SintetizarInutilizacaoCTe(string strNSU, XmlNode xmlDocumento)
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

        #region " SintetizarAutorizacaoCTeOS "

        private void SintetizarAutorizacaoCTeOS(string strNSU, XmlNode xmlDocumento)
        {
            // Obtendo nome do metodo
            string strMetodo = MethodBase.GetCurrentMethod().Name;

            try
            {
                // Obtendo elementos do XML
                XmlElement xmlRet = (XmlElement)((XmlElement)xmlDocumento).GetElementsByTagName(Constante.EsqCTeOSAutorizacaoRet)[0];

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

        #region " SintetizarAutorizacaoGTV "

        private void SintetizarAutorizacaoGTV(string strNSU, XmlNode xmlDocumento)
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

        private void SintetizarEventoGTV(string strNSU, XmlNode xmlDocumento)
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

        private void SintetizarInutilizacaoGTV(string strNSU, XmlNode xmlDocumento)
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

        #region " ExcluirLote "

        private void ExcluirLote(string strNSU)
        {
            // Obtendo nome do metodo
            string strMetodo = MethodBase.GetCurrentMethod().Name;

            try
            {
                // Excluindo item arquivado
                clsBDSin.ExcluirTempFilaIntegrador(strNSU);

                // Registrando log de informacao
                clsLog.RegistrarLog(strMetodo, Constante.MsgDocExcluido + strNSU, EventLogEntryType.Information);
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region " ConfigurarWSConsulta "

        public wsvCTeConsultaDFe.cteConsultaDFe ConfigurarWSConsulta(string strCertificado, string strURL, int intTimeOut, string strVersao)
        {
            // Classes e variaveis utilizadas
            wsvCTeConsultaDFe.cteConsultaDFe wsvRetorno = new wsvCTeConsultaDFe.cteConsultaDFe();
            wsvCTeConsultaDFe.cteCabecMsg wsvCabecalho = new wsvCTeConsultaDFe.cteCabecMsg();

            // Configurando WebService
            wsvCabecalho.versaoDados = strVersao;
            wsvRetorno.cteCabecMsgValue = wsvCabecalho;
            wsvRetorno.Url = strURL;
            wsvRetorno.Timeout = intTimeOut;
            wsvRetorno.ClientCertificates.Add(clsFacil.ObterCertificado(strCertificado));
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            // Retornando WebService
            return wsvRetorno;
        }

        #endregion
    }
}
