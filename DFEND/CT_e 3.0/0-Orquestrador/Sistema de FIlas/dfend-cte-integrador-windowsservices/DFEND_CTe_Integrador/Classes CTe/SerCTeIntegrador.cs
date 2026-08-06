using System;
using System.Data;
using System.Diagnostics;
using System.Reflection;
using System.Xml;

namespace DFe
{
    class SerCTeIntegrador
    {
        #region " Variaveis "

        // Classes utilizadas
        private Facilitador clsFacil;
        private Log clsLog;
        private BdCTeSintetico clsBDSin;
        private BdCTeAnalitico clsBDAna;
        private BdCTeHistorico clsBDHis;
        private BdCTeStaging clsBDStg;

        // Variaveis utilizadas
        private string strClasse;
        private string strNomeServico;
        private readonly short intCodServico;
        private readonly short intThread;
        private short intLogEvento;
        private short intLogBanco;
        private short intLogCompleto;
        private short intExecutar;
        private short intReEnviarFila;
        private short intQtdeMaxFila;
        private short intIntegrarNetezza;
        private short intIntegrarDocVinculado;
        private short intIntegrarFICS;

        #endregion

        #region " Construtores "

        public SerCTeIntegrador(Facilitador clsFacilPar, string strBDCTeSinteticoPar, string strBDCTeAnaliticoPar, string strBDNFeHistoricoPar, string strBDStagingPar, short intCodServicoPar, short intThreadPar)
        {
            // Inicializando classes
            clsFacil = clsFacilPar;

            // Inicializando variaveis
            strClasse = this.GetType().Name;
            intCodServico = intCodServicoPar;
            intThread = intThreadPar;

            // Inicializando classes de banco
            clsBDSin = new BdCTeSintetico(clsFacil, strBDCTeSinteticoPar);
            clsBDAna = new BdCTeAnalitico(clsFacil, strBDCTeAnaliticoPar);
            clsBDHis = new BdCTeHistorico(clsFacil, strBDNFeHistoricoPar);
            clsBDStg = new BdCTeStaging(clsFacil, strBDStagingPar);

            // Obtendo configuracao do banco
            this.ObterConfigBanco();

            // Inicializando classes de negocio
            clsLog = new Log(clsFacil, strBDCTeSinteticoPar, strClasse, strNomeServico, intCodServico, intThread, intLogEvento, intLogBanco, intLogCompleto);
        }

        #endregion

        #region " ObterConfigBanco "

        protected void ObterConfigBanco()
        {
            try
            {
                // Obtendo configuracao do banco
                strNomeServico = Convert.ToString(clsFacil.ValidarItemConfigBanco(clsBDSin.ObterServico(intCodServico.ToString()), "NomeServico"));
                intLogEvento = Convert.ToInt16(clsFacil.ValidarItemConfigBanco(clsBDSin.ObterConfiguracao(intCodServico.ToString(), "LogEvento"), "LogEvento"));
                intLogBanco = Convert.ToInt16(clsFacil.ValidarItemConfigBanco(clsBDSin.ObterConfiguracao(intCodServico.ToString(), "LogBanco"), "LogBanco"));
                intLogCompleto = Convert.ToInt16(clsFacil.ValidarItemConfigBanco(clsBDSin.ObterConfiguracao(intCodServico.ToString(), "LogCompleto"), "LogCompleto"));
                intExecutar = Convert.ToInt16(clsFacil.ValidarItemConfigBanco(clsBDSin.ObterConfiguracao(intCodServico.ToString(), "Executar"), "Executar"));
                intReEnviarFila = Convert.ToInt16(clsFacil.ValidarItemConfigBanco(clsBDSin.ObterConfiguracao(intCodServico.ToString(), "ReEnviarFila"), "ReEnviarFila"));
                intQtdeMaxFila = Convert.ToInt16(clsFacil.ValidarItemConfigBanco(clsBDSin.ObterConfiguracao(intCodServico.ToString(), "QtdeMaxFila"), "QtdeMaxFila"));
                intIntegrarNetezza = Convert.ToInt16(clsFacil.ValidarItemConfigBanco(clsBDSin.ObterConfiguracao(intCodServico.ToString(), "IntegrarNetezza"), "IntegrarNetezza"));
                intIntegrarDocVinculado = Convert.ToInt16(clsFacil.ValidarItemConfigBanco(clsBDSin.ObterConfiguracao(intCodServico.ToString(), "IntegrarDocVinculado"), "IntegrarDocVinculado"));
                intIntegrarFICS = Convert.ToInt16(clsFacil.ValidarItemConfigBanco(clsBDSin.ObterConfiguracao(intCodServico.ToString(), "IntegrarFICS"), "IntegrarFICS"));
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

                    // Verificando se deve reenviar para fila
                    if (intReEnviarFila == 1)
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
                try
                {
                    string strErro = "SerCTeIntegrador.Iniciar ERRO: " + ex.ToString();
                    System.Diagnostics.Debug.WriteLine(strErro);
                    string strDesktop = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "DFEND_CTe_Integrador_erro.txt");
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
                strChave = clsBDSin.RetirarFilaIntegrador();

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
                        // Integrando documento apartir da chave retirada da fila
                        this.Integrar(strChave);
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

        #region " Integrar "

        public void Integrar(string strNSU)
        {
            // Obtendo nome do metodo
            string strMetodo = MethodBase.GetCurrentMethod().Name;

            try
            {
                // Obtendo dados do banco
                DataSet dstDados = clsBDSin.ObterTempFilaIntegrador(strNSU);

                // Verificando se houve retorno
                if ((dstDados != null) && (dstDados.Tables.Count > 0) && (dstDados.Tables[0].Rows.Count > 0))
                {
                    // Registrando log de informacao
                    clsLog.RegistrarLog(strMetodo, Constante.MsgLoteObtidoBanco + strNSU, EventLogEntryType.Information);

                    // Obtendo dados do retorno
                    string strEsquema = dstDados.Tables[0].Rows[0]["des_esquema"].ToString();
                    string strXML = dstDados.Tables[0].Rows[0]["xml_documento"].ToString();
                    string strNSUFinal = dstDados.Tables[0].Rows[0]["num_sequencial_unico_final"].ToString();
                    string strQtde = dstDados.Tables[0].Rows[0]["qtd_documento"].ToString();
                    string strData = dstDados.Tables[0].Rows[0]["dtc_autorizacao"].ToString();

                    // Obtendo XML
                    XmlDocument xmlDocumento = new XmlDocument();
                    xmlDocumento.LoadXml(strXML);

                    // Verificando se deve fazer integracao com o FICS
                    if ((intIntegrarFICS == 1) && (strEsquema == Constante.EsqCTeRetSVD))
                    {
                        this.EnviarIntegracaoFICS(strNSU, xmlDocumento, strQtde, strEsquema, strData);
                    }

                    // Integrando documento de lote
                    this.IntegrarLote(strNSU, xmlDocumento, strEsquema);

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

        #region " IntegrarLote "

        private void IntegrarLote(string strNSU, XmlDocument xmlDocumento, string strEsquemaLote)
        {
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
                        // Integrando documento de Autorizacao
                        this.IntegrarAutorizacao(xmlDescompactado.FirstChild, strEsquemaLote);
                    }
                    else if (strEsquema.StartsWith(Constante.EsqCTeEventoSchema))
                    {
                        // Integrando documento de Evento
                        this.IntegrarEvento(xmlDescompactado.FirstChild, strEsquemaLote);
                    }
                    else if (strEsquema.StartsWith(Constante.EsqCTeInutilizacaoSchema))
                    {
                        // Integrando documento de Inutilizacao
                        this.IntegrarInutilizacao(xmlDescompactado.FirstChild, strEsquemaLote);
                    }
                    else if (strEsquema.StartsWith(Constante.EsqGTVeAutorizacaoSchema))
                    {
                        // Integrando documento de Autorizacao GTV
                        this.IntegrarAutorizacaoGTV(strNSUDFe, xmlDescompactado.FirstChild, strEsquemaLote, strEsquema);
                    }
                    else if (strEsquema.StartsWith(Constante.EsqGTVeEventoSchema))
                    {
                        // Integrando documento de Evento GTV
                        this.IntegrarEventoGTV(strNSUDFe, xmlDescompactado.FirstChild, strEsquemaLote);
                    }
                    else if (strEsquema.StartsWith(Constante.EsqGTVeInutilizacaoSchema))
                    {
                        // Integrando documento de Inutilizacao GTV
                        this.IntegrarInutilizacaoGTV(strNSUDFe, xmlDescompactado.FirstChild, strEsquemaLote);
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

        #region " IntegrarAutorizacao "

        private void IntegrarAutorizacao(XmlNode xmlDocumento, string strEsquemaLote)
        {
            // Classes e variaveis utilizadas
            DocCTe clsDoc = new DocCTe(clsFacil, xmlDocumento);

            try
            {
                // Verificando se deve fazer integracao com o Netezza
                if (intIntegrarNetezza == 1)
                {
                    this.InserirAutorizacaoIntegracaoNetezza(clsDoc);
                }

                // Verificando se deve fazer integracao com o DocVinculado
                if (intIntegrarDocVinculado == 1)
                {
                    this.InserirAutorizacaoIntegracaoDocVinculado(clsDoc);
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                clsDoc = null;
            }
        }

        #endregion

        #region " IntegrarEvento "

        private void IntegrarEvento(XmlNode xmlDocumento, string strEsquemaLote)
        {
            // Classes e variaveis utilizadas
            DocCTeEvent clsDocEvent = new DocCTeEvent(clsFacil, xmlDocumento);

            try
            {
                // Nao existe integracao de evento para este tipo de documento
            }
            catch
            {
                throw;
            }
            finally
            {
                clsDocEvent = null;
            }
        }

        #endregion

        #region " IntegrarInutilizacao "

        private void IntegrarInutilizacao(XmlNode xmlDocumento, string strEsquemaLote)
        {
            // Classes e variaveis utilizadas
            DocCTeInut clsDocInut = new DocCTeInut(clsFacil, xmlDocumento);

            try
            {
                // Nao existe integracao de inutilizacao para este tipo de documento
            }
            catch
            {
                throw;
            }
            finally
            {
                clsDocInut = null;
            }
        }

        #endregion

        #region " IntegrarAutorizacaoGTV "

        private void IntegrarAutorizacaoGTV(string strNSU, XmlNode xmlDocumento, string strEsquemaLote, string strEsquema)
        {
            // Nao existe integracao para este tipo de documento
        }

        #endregion

        #region " IntegrarEventoGTV "

        private void IntegrarEventoGTV(string strNSU, XmlNode xmlDocumento, string strEsquemaLote)
        {
            // Nao existe integracao para este tipo de documento
        }

        #endregion

        #region " IntegrarInutilizacaoGTV "

        private void IntegrarInutilizacaoGTV(string strNSU, XmlNode xmlDocumento, string strEsquemaLote)
        {
            // Nao existe integracao para este tipo de documento
        }

        #endregion

        #region " ObterAutorizacao "

        private void ObterAutorizacao(string strChave)
        {
            // Obtendo nome do metodo
            string strMetodo = MethodBase.GetCurrentMethod().Name;

            try
            {
                // Obtendo dados do banco
                DataSet dstDados = clsBDSin.ObterAutorizacao(clsFacil.ObterDataReferencia(strChave), strChave);

                // Verificando se houve retorno
                if ((dstDados != null) && (dstDados.Tables.Count > 0) && (dstDados.Tables[0].Rows.Count > 0))
                {
                    // Registrando log de informacao
                    clsLog.RegistrarLog(strMetodo, Constante.MsgDadoObtidoBanco + strChave, EventLogEntryType.Information);

                    // Obtendo dados do retorno
                    string strXML = dstDados.Tables[0].Rows[0]["xml_documento"].ToString();

                    // Obtendo XML
                    XmlDocument xmlDocumento = new XmlDocument();
                    xmlDocumento.LoadXml(strXML);

                    // Integrando documento de Autorizacao
                    this.IntegrarAutorizacao(xmlDocumento, string.Empty);
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
        }

        #endregion

        #region " ObterEvento "

        private void ObterEvento(string strChave)
        {
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
                if ((dstDados != null) && (dstDados.Tables.Count > 0) && (dstDados.Tables[0].Rows.Count > 0))
                {
                    // Registrando log de informacao
                    clsLog.RegistrarLog(strMetodo, Constante.MsgDadoObtidoBanco + strChave, EventLogEntryType.Information);

                    // Obtendo dados do retorno
                    string strXML = dstDados.Tables[0].Rows[0]["xml_documento"].ToString();

                    // Obtendo XML
                    XmlDocument xmlDocumento = new XmlDocument();
                    xmlDocumento.LoadXml(strXML);

                    // Integrando documento de Evento
                    this.IntegrarEvento(xmlDocumento, string.Empty);
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
                    clsBDSin.AtualizarTempFilaIntegradorErro(strChave, strMensagem);

                    // Registrando log de informacao
                    clsLog.RegistrarLog(strMetodo, Constante.MsgDocAtualizado + strChave, EventLogEntryType.Information);

                    //// Enviando chave para fila
                    //clsBDSin.EnviarFilaIntegrador(strChave);

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

        #region " ReEnviarFila "

        public void ReEnviarFila()
        {
            // Obtendo nome do metodo
            string strMetodo = MethodBase.GetCurrentMethod().Name;

            try
            {
                // Verificando a quantidade de itens na fila
                if (clsBDSin.ObterQtdeFilaIntegrador() < intQtdeMaxFila)
                {
                    // Registrando log de informacao
                    clsLog.RegistrarLog(strMetodo, Constante.MsgFilaObterRegistros, EventLogEntryType.Information);

                    // Obtendo dados de registros nao arquivados
                    DataSet dstDados = clsBDSin.ObterTempFilaIntegradorTop(intQtdeMaxFila);

                    // Registrando log de informacao
                    clsLog.RegistrarLog(strMetodo, Constante.MsgFilaRegistrosObtidos, EventLogEntryType.Information);

                    // Verificando se houve retorno
                    if ((dstDados != null) && (dstDados.Tables.Count > 0) && (dstDados.Tables[0].Rows.Count > 0))
                    {
                        // Percorrendo a lista de itens retornados
                        foreach (DataRow drwDados in dstDados.Tables[0].Rows)
                        {
                            // Obtendo dados do retorno
                            string strNSU = drwDados["num_sequencial_unico"].ToString();

                            // Enviando chave para fila
                            clsBDSin.EnviarFilaIntegrador(strNSU);

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

        #region " Integracao Netezza "

        #region " InserirAutorizacaoIntegracaoNetezza "

        private void InserirAutorizacaoIntegracaoNetezza(DocCTe clsDoc)
        {
            // Obtendo nome do metodo
            string strMetodo = MethodBase.GetCurrentMethod().Name;

            try
            {
                // Obtendo cancelamento do documento
                DataSet dtsDadosSin = clsBDSin.ObterEvento(clsDoc.DataReferencia, clsDoc.ChaveAcesso, ((int)Constante.TipoEvento.Cancelamento).ToString(), string.Empty);
                DataSet dtsDadosDef = clsBDHis.ObterDadosCTeEventoPorChave(clsDoc.ChaveAcesso, "1", ((int)Constante.TipoEvento.Cancelamento).ToString());

                // Verificando se houve retorno
                if (((dtsDadosSin.Tables.Count > 0) && (dtsDadosSin.Tables[0].Rows.Count > 0)) || ((dtsDadosDef.Tables.Count > 0) && (dtsDadosDef.Tables[0].Rows.Count > 0)))
                {
                    clsDoc.Status = "101";
                    clsDoc.Motivo = "Cancelamento de NF-e homologado";
                }

                // Obtendo o semaforo utilizado
                short intSemaforo = clsBDStg.ObterSemaforoCTe();

                // Excluindo dados no staging do Netezza
                clsBDStg.ExcluirDFe(clsDoc.DataReferencia, clsDoc.ChaveAcesso, intSemaforo);

                // Inserindo dados no Netezza
                clsBDStg.InserirDFe(clsDoc, intSemaforo);

                // Registrando log de informacao
                clsLog.RegistrarLog(strMetodo, Constante.MsgDocAutorizacaoInseridoBanco + clsDoc.NSU + ". Chave: " + clsDoc.Chave, EventLogEntryType.Information);
            }
            catch (Exception ex)
            {
                // Verificando o tipo da excecao
                if (ex.ToString().ToUpper().Contains("PRIMARY KEY") || ex.ToString().ToUpper().Contains("DUPLICATE KEY"))
                {
                    // Registrando log de alerta
                    clsLog.RegistrarLog(strMetodo, Constante.MsgDocJaExistente + clsDoc.NSU, EventLogEntryType.Information);
                }
                else
                {
                    throw;
                }
            }
        }

        #endregion

        #region " InserirEventoIntegracaoNetezza "

        private void InserirEventoIntegracaoNetezza(DocCTeEvent clsDocEvent)
        {
            // Obtendo nome do metodo
            string strMetodo = MethodBase.GetCurrentMethod().Name;

            try
            {
                // Verificando se o evento e um Cancelamento
                if (clsDocEvent.TipoEvento == ((int)Constante.TipoEvento.Cancelamento).ToString())
                {
                    // Variaveis utilizadas
                    string strXML = string.Empty;

                    // Obtendo cancelamento do documento
                    DataSet dtsDadosSin = clsBDSin.ObterAutorizacao(clsDocEvent.DataReferencia, clsDocEvent.ChaveAcesso);
                    if ((dtsDadosSin.Tables.Count > 0) && (dtsDadosSin.Tables[0].Rows.Count > 0))
                    {
                        strXML = dtsDadosSin.Tables[0].Rows[0]["xml_documento"].ToString();
                    }
                    else
                    {
                        DataSet dtsDadosDef = clsBDHis.ObterDadosCTeAutorizacaoPorChave(clsDocEvent.ChaveAcesso);
                        if ((dtsDadosDef.Tables.Count > 0) && (dtsDadosDef.Tables[0].Rows.Count > 0))
                        {
                            strXML = dtsDadosDef.Tables[0].Rows[0]["xml_documento"].ToString();
                        }
                    }

                    // Verificando se achou o documento
                    if (strXML != string.Empty)
                    {
                        DocCTe clsDoc = new DocCTe(clsFacil, strXML);
                        this.InserirAutorizacaoIntegracaoNetezza(clsDoc);
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #endregion

        #region " Integracao DocVinculado "

        #region " InserirAutorizacaoIntegracaoDocVinculado "

        private void InserirAutorizacaoIntegracaoDocVinculado(DocCTe clsDoc)
        {
            // Obtendo nome do metodo
            string strMetodo = MethodBase.GetCurrentMethod().Name;

            try
            {
                // Verificando o modelo do documento
                if (clsDoc.Modelo == Constante.ModeloCTe)
                {
                    // Inserindo dados no banco
                    clsBDAna.InserirTempFilaDocVinculado(clsDoc.NSU, clsDoc.Protocolo, clsDoc.XMLEnvio, clsDoc.Schema, clsDoc.DataEmissao);

                    // Registrando log de informacao
                    clsLog.RegistrarLog(strMetodo, Constante.MsgDocAutorizacaoInseridoBanco + clsDoc.NSU, EventLogEntryType.Information);

                    // Enviando chave para fila
                    clsBDAna.EnviarFilaDocVinculado(clsDoc.NSU);

                    // Registrando log de informacao
                    clsLog.RegistrarLog(strMetodo, Constante.MsgDocInseridoFila + clsDoc.NSU, EventLogEntryType.Information);
                }
            }
            catch (Exception ex)
            {
                // Verificando o tipo da excecao
                if (ex.ToString().ToUpper().Contains("PRIMARY KEY") || ex.ToString().ToUpper().Contains("DUPLICATE KEY"))
                {
                    // Registrando log de alerta
                    clsLog.RegistrarLog(strMetodo, Constante.MsgDocJaExistente + clsDoc.NSU, EventLogEntryType.Information);
                }
                else
                {
                    throw;
                }
            }
        }

        #endregion

        #endregion

        #region " Integracao FICS "

        #region " EnviarIntegracaoFICS "

        private void EnviarIntegracaoFICS(string strNSU, XmlNode xmlDocumento, string strProtocolo, string strEsquema, string strData)
        {
            // Obtendo nome do metodo
            string strMetodo = MethodBase.GetCurrentMethod().Name;

            try
            {
                // Inserindo dados no banco
                clsBDAna.InserirTempFilaFICS(strNSU, strProtocolo, xmlDocumento.OuterXml, strEsquema, strData);

                // Registrando log de informacao
                clsLog.RegistrarLog(strMetodo, Constante.MsgLoteInseridoBanco + strNSU, EventLogEntryType.Information);

                // Enviando chave para fila
                clsBDAna.EnviarFilaFICS(strNSU);

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

        #endregion
    }
}
