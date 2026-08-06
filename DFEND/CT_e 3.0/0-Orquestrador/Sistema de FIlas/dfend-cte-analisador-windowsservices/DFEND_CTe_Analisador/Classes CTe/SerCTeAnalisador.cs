using System;
using System.Data;
using System.Diagnostics;
using System.Reflection;
using System.Xml;



using System.Text;


using System.IO;
using System.Xml.Serialization;


namespace DFe
{
    class SerCTeAnalisador
    {
        #region " Variaveis "

        // Classes utilizadas
        private Log clsLog;
        private BdCTeSintetico clsBDSin;
        private BdCTeAnalitico clsBDAna;       
        private NegCTeAnalitico clsNeg;

        // Variaveis utilizadas
        private string strClasse;        
        private string strNomeServico;
        private readonly short intCodServico;
        private readonly short intThread;
        private short intLogEvento;
        private short intLogBanco;
        private short intLogCompleto;
        private short intExecutar;
        private short intQtdeMaxFila;
        private string strReEnviarFila;
        
        #endregion

        #region " Construtores "

        public SerCTeAnalisador(string strBDCTeSinteticoPar, string strBDCTeAnaliticoPar, short intCodServicoPar, short intThreadPar)
        {
            // Inicializando variaveis
            strClasse = this.GetType().Name;
            intCodServico = intCodServicoPar;
            intThread = intThreadPar;            

            // Inicializando classes de banco
            clsBDSin = new BdCTeSintetico(strBDCTeSinteticoPar);
            clsBDAna = new BdCTeAnalitico(strBDCTeAnaliticoPar);
            
            // Obtendo configuracao do banco
            this.ObterConfigBanco();

            // Inicializando classes de negocio
            clsLog = new Log(strBDCTeAnaliticoPar, strClasse, strNomeServico, intCodServico, intThread, intLogEvento, intLogBanco, intLogCompleto);
            clsNeg = new NegCTeAnalitico(clsLog, clsBDAna);
        }

        #endregion

        #region " ObterConfigBanco "

        protected void ObterConfigBanco()
        {
            // Classes e variaveis utilizadas
            Facilitador clsFacil = new Facilitador();

            try
            {
                // Obtendo configuracao do banco
                strNomeServico = Convert.ToString(clsFacil.ValidarItemConfigBanco(clsBDAna.ObterServico(intCodServico.ToString()), "NomeServico"));
                intLogEvento = Convert.ToInt16(clsFacil.ValidarItemConfigBanco(clsBDAna.ObterConfiguracao(intCodServico.ToString(), "LogEvento"), "LogEvento"));
                intLogBanco = Convert.ToInt16(clsFacil.ValidarItemConfigBanco(clsBDAna.ObterConfiguracao(intCodServico.ToString(), "LogBanco"), "LogBanco"));
                intLogCompleto = Convert.ToInt16(clsFacil.ValidarItemConfigBanco(clsBDAna.ObterConfiguracao(intCodServico.ToString(), "LogCompleto"), "LogCompleto"));
                intExecutar = Convert.ToInt16(clsFacil.ValidarItemConfigBanco(clsBDAna.ObterConfiguracao(intCodServico.ToString(), "Executar"), "Executar"));
                intQtdeMaxFila = Convert.ToInt16(clsFacil.ValidarItemConfigBanco(clsBDAna.ObterConfiguracao(intCodServico.ToString(), "QtdeMaxFila"), "QtdeMaxFila"));
                strReEnviarFila = Convert.ToString(clsFacil.ValidarItemConfigBanco(clsBDAna.ObterConfiguracao(intCodServico.ToString(), "ReEnviarFila"), "ReEnviarFila"));
                //strEnviarBDAnalitico = Convert.ToString(clsFacil.ValidarItemConfigBanco(clsBDSin.ObterConfiguracao(intCodServico.ToString(), "EnviarBDAnalitico"), "EnviarBDAnalitico"));
                //strEnviarBDHistorico = Convert.ToString(clsFacil.ValidarItemConfigBanco(clsBDSin.ObterConfiguracao(intCodServico.ToString(), "EnviarBDDefinitivo"), "EnviarBDDefinitivo"));
                //strEnviarIntegracaoNetezza = Convert.ToString(clsFacil.ValidarItemConfigBanco(clsBDSin.ObterConfiguracao(intCodServico.ToString(), "EnviarIntegracaoNetezza"), "EnviarIntegracaoNetezza"));
                //strEnviarIntegracaoDocVinculado = Convert.ToString(clsFacil.ValidarItemConfigBanco(clsBDSin.ObterConfiguracao(intCodServico.ToString(), "EnviarIntegracaoDocVinculado"), "EnviarIntegracaoDocVinculado"));
                //strEnviarIntegracaoFICS = Convert.ToString(clsFacil.ValidarItemConfigBanco(clsBDSin.ObterConfiguracao(intCodServico.ToString(), "EnviarIntegracaoFICS"), "EnviarIntegracaoFICS"));
            }
            catch
            {
                throw;
            }
            finally
            {
                clsFacil = null;
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

                    // Verificando se deve reenviar para fila
                    if (strReEnviarFila == "S")
                    {
                        if ((datUltimaExecucao.Hour != DateTime.Now.Hour) && (intThread == 1))
                        {
                            this.ReEnviarFila();
                            datUltimaExecucao = DateTime.Now;
                        }
                    }

                    // Processando enquanto houver retorno
                    bool bolExecutar = true;
                    while (bolExecutar)
                    {
                        bolExecutar = this.Processar();
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
                clsLog.RegistrarLog(ex);
            }
        }

        #endregion

        #region " Processar "

        public bool Processar()
        {
            // Classes e variaveis utilizadas
            string strChave = string.Empty;
            bool bolRetorno = false;

            // Obtendo nome do metodo
            string strMetodo = MethodBase.GetCurrentMethod().Name;

            try
            {
                // Retirando chave da fila
                strChave = clsBDSin.RetirarFilaAnalisador();

                //Chaves de Testes
                //strChave = "35150100003942000123570010000085851000558507"; //2.0
                //strChave = "31130100091731000190570010000087721000184403"; //1.0
                //strChave = "2249827";

                // Verificando se existe chave
                if (strChave != string.Empty)
                {
                    // Registrando log de informacao
                    clsLog.RegistrarLog(strMetodo, Constante.MsgChaveRetiradaFila + strChave, EventLogEntryType.Information);

                    // Informando que encontrou item
                    bolRetorno = true;

                    // Verificando se a chave e um NSU, Chave de acesso ou evento
                    if (strChave.Length < 44)
                    {
                        // Analisando documento apartir da chave retirada da fila
                        this.Analisar(strChave);
                    }
                    else if (strChave.Length == 44)
                    {
                        // Obtendo Autorizacao apartir da chave retirada da fila
                        this.ObterAutorizacao(strChave);
                    }
                    else if (strChave.Length > 44)
                    {
                        // Obtendo Evento apartir da chave retirada da fila
                        this.ObterEvento(strChave);
                    }
                }
            }
            catch (Exception ex)
            {
                // Informando erro e retornando chave para a fila
                this.AtualizarErro(strChave, ex.Message);
                throw;
            }

            return bolRetorno;
        }

        #endregion

        #region " Analisar "

        public void Analisar(string strNSU)
        {
            // Obtendo nome do metodo
            string strMetodo = MethodBase.GetCurrentMethod().Name;

            try
            {
                // Obtendo dados do banco
                DataSet dstDados = clsBDSin.ObterTempAnalisador(strNSU);

                // Verificando se houve retorno
                if ((dstDados.Tables.Count > 0) && (dstDados.Tables[0].Rows.Count > 0))
                {
                    // Registrando log de informacao
                    clsLog.RegistrarLog(strMetodo, Constante.MsgLoteObtidoBanco + strNSU, EventLogEntryType.Information);

                    // Obtendo dados do retorno
                    string strEsquema = dstDados.Tables[0].Rows[0]["des_esquema"].ToString();
                    string strXML = dstDados.Tables[0].Rows[0]["xml_documento"].ToString();
                    string strProtocolo = dstDados.Tables[0].Rows[0]["num_protocolo"].ToString();
                    string strQtde = dstDados.Tables[0].Rows[0]["num_protocolo"].ToString();
                    string strData = dstDados.Tables[0].Rows[0]["dtc_documento"].ToString();

                    // Obtendo XML
                    XmlDocument xmlDocumento = new XmlDocument();
                    xmlDocumento.LoadXml(strXML);

                    // Analisando documento de lote
                    this.AnalisarLote(xmlDocumento, strNSU, strEsquema);

                    // Excluindo documento analisado
                    this.ExcluirLote(strNSU);
                }
                else
                {
                    // Registrando log de informacao
                    clsLog.RegistrarLog(strMetodo, Constante.MsgDadoNaoObtidoBanco + strNSU, EventLogEntryType.Information);
                }
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region " AnalisarLote "

        private void AnalisarLote(XmlDocument xmlDocumento, string strNSU, string strEsquemaLote)
        {
            // Classes e variaveis utilizadas
            Facilitador clsFacil = new Facilitador();

            // Obtendo nome do metodo
            string strMetodo = MethodBase.GetCurrentMethod().Name;

            try
            {
                // Obtendo lista de documentos
                XmlNodeList xmlLote;
                xmlLote = xmlDocumento[Constante.EsqCTeRetSVD][Constante.EsqLote].ChildNodes;

                // Percorrendo a lista de itens retornados
                foreach (XmlNode xmlItem in xmlLote)
                {
                    // Obtendo elementos do XML
                    string strXML = xmlItem.OuterXml;
                    string strNSUDFe = xmlItem.Attributes["NSUSVD"].InnerText;
                    string strEsquema = xmlItem.Attributes["schema"].InnerText;

                    // Montando XML
                    XmlDocument xmlDescompactado = new XmlDocument();
                    xmlDescompactado.LoadXml(xmlItem.OuterXml);
                    XmlElement xmlComp = (XmlElement)xmlDescompactado.GetElementsByTagName("procComp")[0];

                    // Verificando se o retorno esta compactado
                    if (xmlComp != null)
                    {
                        // Substituindo resposta compactada
                        string strDescompactado = clsFacil.DescompactarTexto(xmlItem["procComp"].InnerText);
                        string strInicial = strXML.Substring(0, strXML.IndexOf("<procComp>"));
                        string strFinal = strXML.Substring(strXML.IndexOf("</procComp>") + 11);
                        strDescompactado = strDescompactado.Replace(Constante.CabecalhoXML, string.Empty);
                        strDescompactado = strDescompactado.Replace(Constante.CabecalhoXML2, string.Empty);
                        strDescompactado = strDescompactado.Replace(Constante.CabecalhoXML3, string.Empty);
                        strDescompactado = strDescompactado.Replace(Constante.CabecalhoXML4, string.Empty);
                        strDescompactado = strDescompactado.Replace(Constante.CabecalhoXML5, string.Empty);
                        strDescompactado = strDescompactado.Replace(Constante.CabecalhoXML6, string.Empty);
                        strDescompactado = strDescompactado.Replace(Constante.CabecalhoXML7, string.Empty);
                        strDescompactado = strDescompactado.Replace(Constante.CabecalhoXML8, string.Empty);
                        strDescompactado = strDescompactado.Replace(Constante.CabecalhoXML9, string.Empty);
                        strDescompactado = strDescompactado.Replace(Constante.CabecalhoXML10, string.Empty);
                        xmlDescompactado.LoadXml(strDescompactado);
                        xmlDescompactado.LoadXml(strInicial + xmlDescompactado.LastChild.OuterXml + strFinal);
                    }
                    else
                    {
                        // Levantando excecao
                        throw new Exception("Lote não compactado");
                    }

                    // Verificando o tipo do schema retornado
                    //if (strEsquema.StartsWith(Constante.EsqCTeAutorizacaoSchema))
                    if (strEsquema.Substring(0, 7) == Constante.EsqCTeAutorizacaoSchema && strEsquema.Substring(0, 9) != Constante.EsqCTeOSAutorizacaoSchema && strEsquema.Substring(0, 11) != Constante.EsqCTeSimpAutorizacaoSchema)
                    {
                        // Analisando documento de Autorizacao
                        this.AnalisarAutorizacaoCTe(xmlDescompactado.FirstChild);
                    }
                    else if (strEsquema.StartsWith(Constante.EsqCTeOSAutorizacaoSchema))
                    {
                        // Analisando o CTeOS
                        this.AnalisarAutorizacaoCTeOS(xmlDescompactado.FirstChild);

                    }
                    else if (strEsquema.StartsWith(Constante.EsqCTeEventoSchema))
                    {
                        // Analisando documento de Evento
                        this.AnalisarEvento(xmlDescompactado.FirstChild);
                    }                    
                    else if (strEsquema.StartsWith(Constante.EsqGTVeAutorizacaoSchema))
                    {
                        // Analisando documento de Autorizacao GTV                        
                        this.AnalisarAutorizacaoGTVe(xmlDescompactado.FirstChild);
                    }
                    else if (strEsquema.StartsWith(Constante.EsqGTVeEventoSchema))
                    {
                        // Analisando documento de Evento GTV
                        //this.AnalisarEventoGTVe(strNSUDFe, xmlDescompactado.FirstChild, strEsquemaLote);
                    }
                    else if (strEsquema.StartsWith(Constante.EsqCTeSimpAutorizacaoSchema))
                    {
                        // Analisando o CTeSimp
                        this.AnalisarAutorizacaoCTeSimp(xmlDescompactado.FirstChild);

                    }
                    else if (strEsquema.StartsWith(Constante.EsqCTeInutilizacaoSchema))
                    {
                        // Analisando documento de Inutilizacao
                        //this.AnalisarInutilizacao(xmlDescompactado.FirstChild, strEsquemaLote);
                    }
                    else if (strEsquema.StartsWith(Constante.EsqGTVeInutilizacaoSchema))
                    {
                        //Analisando documento de Inutilizacao GTV
                        //this.AnalisarInutilizacaoGTV(strNSUDFe, xmlDescompactado.FirstChild, strEsquemaLote);
                    }
                    else
                    {
                        // Levantando excecao
                        throw new Exception(Constante.MsgLoteElementoNaoEsperado + strEsquema);
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region " AnalisarAutorizacaoCTe "

        private void AnalisarAutorizacaoCTe(XmlNode xmlDocumento)
        {

            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                MemoryStream readerCTE = new MemoryStream(Encoding.UTF8.GetBytes(xmlDocumento.OuterXml));
                XmlSerializer peCTe = new XmlSerializer(typeof(Common.SerializableClasses.CTe.xmlCTe_v400_NT202402.proc));
                Common.SerializableClasses.CTe.xmlCTe_v400_NT202402.proc CTe = (Common.SerializableClasses.CTe.xmlCTe_v400_NT202402.proc)peCTe.Deserialize(readerCTE);

                clsNeg.AnalisarAutorizacaoCTe(CTe);

            }
            catch
            {
                throw;
            }

        }


        private void AnalisarAutorizacaoCTeV2(XmlNode xmlDocumento)
        {

            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                MemoryStream readerCTE = new MemoryStream(Encoding.UTF8.GetBytes(xmlDocumento.OuterXml));
                XmlSerializer peCTe = new XmlSerializer(typeof(Common.SerializableClasses.CTe.ClsCTe_v2.proc));
                Common.SerializableClasses.CTe.ClsCTe_v2.proc CTe = (Common.SerializableClasses.CTe.ClsCTe_v2.proc)peCTe.Deserialize(readerCTE);

                clsNeg.AnalisarAutorizacaoCTeV2(CTe);

            }
            catch
            {
                throw;
            }

        }

        private void AnalisarAutorizacaoCTeV1(XmlNode xmlDocumento)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                MemoryStream readerCTE = new MemoryStream(Encoding.UTF8.GetBytes(xmlDocumento.OuterXml));
                XmlSerializer peCTe = new XmlSerializer(typeof(Common.SerializableClasses.CTe.ClsCTe_v1.proc));
                Common.SerializableClasses.CTe.ClsCTe_v1.proc CTe = (Common.SerializableClasses.CTe.ClsCTe_v1.proc)peCTe.Deserialize(readerCTE);

                clsNeg.AnalisarAutorizacaoCTeV1(CTe);

            }
            catch
            {
                throw;
            }

        }

        private void AnalisarAutorizacaoCTeSimp(XmlNode xmlDocumento)
        {

            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                MemoryStream readerCTE = new MemoryStream(Encoding.UTF8.GetBytes(xmlDocumento.OuterXml));
                XmlSerializer peCTe = new XmlSerializer(typeof(Common.SerializableClasses.CTe.xmlCTeSimp_v400.proc));
                Common.SerializableClasses.CTe.xmlCTeSimp_v400.proc CTe = (Common.SerializableClasses.CTe.xmlCTeSimp_v400.proc)peCTe.Deserialize(readerCTE);

                clsNeg.AnalisarAutorizacaoCTeSimp(CTe);

            }
            catch
            {
                throw;
            }

        }

        #endregion

        #region " AnalisarEvento "

        private void AnalisarEvento(XmlNode xmlDocumento)
        {

            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                MemoryStream readerCTE = new MemoryStream(Encoding.UTF8.GetBytes(xmlDocumento.OuterXml));
                XmlSerializer peventoCTe = new XmlSerializer(typeof(Common.SerializableClasses.CTe.xmlEventoCTe_v400.proc));
                Common.SerializableClasses.CTe.xmlEventoCTe_v400.proc eventoCTe = (Common.SerializableClasses.CTe.xmlEventoCTe_v400.proc)peventoCTe.Deserialize(readerCTE);

                clsNeg.AnalisarEvento(eventoCTe);
            }
            catch
            {
                throw;
            }

        }

        #endregion        

        #region " AnalisarAutorizacaoGTV "

        private void AnalisarAutorizacaoGTVe(XmlNode xmlDocumento)
        {
            XmlDocument xmlDoc = new XmlDocument();
            MemoryStream readerCTE = new MemoryStream(Encoding.UTF8.GetBytes(xmlDocumento.OuterXml));
            XmlSerializer peGTVe = new XmlSerializer(typeof(Common.SerializableClasses.CTe.ClsXmlGTVeNT202402.proc));
            Common.SerializableClasses.CTe.ClsXmlGTVeNT202402.proc GTVe = (Common.SerializableClasses.CTe.ClsXmlGTVeNT202402.proc)peGTVe.Deserialize(readerCTE);

            try
            {

                clsNeg.AnalisarAutorizacaoGTVe(GTVe);
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region " AnalisarAutorizacaoCTeOS "

        private void AnalisarAutorizacaoCTeOS(XmlNode xmlDocumento)
        {

            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                MemoryStream readerCTE = new MemoryStream(Encoding.UTF8.GetBytes(xmlDocumento.OuterXml));
                XmlSerializer peCTe = new XmlSerializer(typeof(Common.SerializableClasses.CTe.ClsCTeOS_v4.proc));
                Common.SerializableClasses.CTe.ClsCTeOS_v4.proc CTeOS = (Common.SerializableClasses.CTe.ClsCTeOS_v4.proc)peCTe.Deserialize(readerCTE);

                clsNeg.AnalisarAutorizacaoCTeOS(CTeOS);

            }
            catch
            {
                throw;
            }

        }
        #endregion
        
        #region " ObterAutorizacao "

        private void ObterAutorizacao(string strChave)
        {
            // Classes e variaveis utilizadas
            Facilitador clsFacil = new Facilitador();

            // Obtendo nome do metodo
            string strMetodo = MethodBase.GetCurrentMethod().Name;

            try
            {
                // Obtendo dados do banco
                DataSet dstDados = clsBDSin.ObterAutorizacao(clsFacil.ObterDataReferencia(strChave), strChave);
                //DataSet dstDados = clsBDSin.ObterAutorizacaoHistoricoTeste(strChave);

                // Verificando se houve retorno
                if ((dstDados.Tables.Count > 0) && (dstDados.Tables[0].Rows.Count > 0))
                {
                    // Registrando log de informacao
                    clsLog.RegistrarLog(strMetodo, Constante.MsgDadoObtidoBanco + strChave, EventLogEntryType.Information);

                    // Obtendo dados do retorno
                    string strXML = dstDados.Tables[0].Rows[0]["xml_documento"].ToString();

                    // Obtendo XML
                    XmlDocument xmlDocumento = new XmlDocument();
                    xmlDocumento.LoadXml(strXML);

                    //testes para o xml 1.0 e 2.0
                    //this.AnalisarAutorizacaoCTeV2(xmlDocumento);
                    //this.AnalisarAutorizacaoCTeV1(xmlDocumento);

                    //CTe
                    if (strChave.Substring(20, 2) == "57")
                    {
                        if (strXML.Substring(0,1000).Contains("CTeSimp"))
                        {
                            this.AnalisarAutorizacaoCTeSimp(xmlDocumento);
                        }
                        else
                        {
                            this.AnalisarAutorizacaoCTe(xmlDocumento);
                        }
                    }
                    //GTVe
                    if (strChave.Substring(20, 2) == "64")
                    {
                        this.AnalisarAutorizacaoGTVe(xmlDocumento);
                    }
                    //CTeOS
                    if (strChave.Substring(20, 2) == "67")
                    {
                        this.AnalisarAutorizacaoCTeOS(xmlDocumento);
                    }

                }
                else
                {
                    // Registrando log de informacao
                    clsLog.RegistrarLog(strMetodo, Constante.MsgDadoNaoObtidoBanco + strChave, EventLogEntryType.Information);
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                clsFacil = null;
            }
        }

        #endregion

        #region " ObterEvento "

        private void ObterEvento(string strChave)
        {
            // Classes e variaveis utilizadas
            Facilitador clsFacil = new Facilitador();

            // Obtendo nome do metodo
            string strMetodo = MethodBase.GetCurrentMethod().Name;

            try
            {
                // Obtendo as partes da chave
                string strChaveAcesso = clsFacil.ObterParteChave(strChave, 0);
                string strTipoEvento = clsFacil.ObterParteChave(strChave, 1);
                string strSeqEvento = clsFacil.ObterParteChave(strChave, 2);

                // Obtendo dados do banco
                DataSet dstDados = clsBDSin.ObterEvento(clsFacil.ObterDataReferencia(strChaveAcesso), strChaveAcesso, strTipoEvento, strSeqEvento);

                // Verificando se houve retorno
                if ((dstDados.Tables.Count > 0) && (dstDados.Tables[0].Rows.Count > 0))
                {
                    // Registrando log de informacao
                    clsLog.RegistrarLog(strMetodo, Constante.MsgDadoObtidoBanco + strChave, EventLogEntryType.Information);

                    // Obtendo dados do retorno
                    string strXML = dstDados.Tables[0].Rows[0]["xml_documento"].ToString();

                    // Obtendo XML
                    XmlDocument xmlDocumento = new XmlDocument();
                    xmlDocumento.LoadXml(strXML);

                    // Analisando documento de Evento
                    this.AnalisarEvento(xmlDocumento);
                }
                else
                {
                    // Registrando log de informacao
                    clsLog.RegistrarLog(strMetodo, Constante.MsgDadoNaoObtidoBanco + strChave, EventLogEntryType.Information);
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                clsFacil = null;
            }
        }

        #endregion

        #region " AtualizarErro "

        private void AtualizarErro(string strChave, string strMensagem)
        {
            // Obtendo nome do metodo
            string strMetodo = MethodBase.GetCurrentMethod().Name;

            try
            {
                // Verificando se existe chave
                if (strChave != string.Empty)
                {
                    // Atualizando documento com erro
                    if (strChave.Length < 44)
                    {
                        clsBDSin.AtualizarTempAnalisadorErro(strChave, strMensagem);
                    }

                    // Registrando log de informacao
                    clsLog.RegistrarLog(strMetodo, Constante.MsgDocAtualizado + strChave, EventLogEntryType.Information);

                    //// Enviando chave para fila
                    //clsBDSin.EnviarFilaAnalisador(strChave);

                    //// Registrando log de informacao
                    //clsLog.RegistrarLog(strMetodo, Constante.MsgDocInseridoFila + strChave, EventLogEntryType.Information);
                }
            }
            catch
            {
                throw;
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
                clsBDSin.ExcluirTempAnalisador(strNSU);

                // Registrando log de informacao
                clsLog.RegistrarLog(strMetodo, Constante.MsgDocExcluido + strNSU, EventLogEntryType.Information);
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region " ReEnviarFila "

        public void ReEnviarFila()
        {
            // Obtendo nome do metodo
            string strMetodo = MethodBase.GetCurrentMethod().Name;

            try
            {
                // Verificando a quantidade de itens na fila
                if (clsBDSin.ObterQtdeFilaAnalisador() < intQtdeMaxFila)
                {
                    // Registrando log de informacao
                    clsLog.RegistrarLog(strMetodo, Constante.MsgFilaObterRegistros, EventLogEntryType.Information);

                    // Obtendo dados de registros nao arquivados
                    DataSet dstDados = clsBDSin.ObterTempAnalisadorTop(intQtdeMaxFila);

                    // Registrando log de informacao
                    clsLog.RegistrarLog(strMetodo, Constante.MsgFilaRegistrosObtidos, EventLogEntryType.Information);

                    // Verificando se retornou dados
                    if (dstDados.Tables.Count > 0 && dstDados.Tables[0].Rows.Count > 0)
                    {
                        // Percorrendo a lista de itens retornados
                        foreach (DataRow drwDados in dstDados.Tables[0].Rows)
                        {
                            // Obtendo dados do retorno
                            string strNSU = drwDados["num_sequencial_unico"].ToString();

                            // Enviando chave para fila
                            clsBDSin.EnviarFilaAnalisador(strNSU);

                            // Registrando log de informacao
                            clsLog.RegistrarLog(strMetodo, Constante.MsgChaveInseridaFila + strNSU, EventLogEntryType.Information);
                        }
                    }
                }
                else
                {
                    // Registrando log de informacao
                    clsLog.RegistrarLog(strMetodo, Constante.MsgFilaQtdeAcima, EventLogEntryType.Information);
                }
            }
            catch
            {
                throw;
            }
        }

        #endregion
           
    }
}
