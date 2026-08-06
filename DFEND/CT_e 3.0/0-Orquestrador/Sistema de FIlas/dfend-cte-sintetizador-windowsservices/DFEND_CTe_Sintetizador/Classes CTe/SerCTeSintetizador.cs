using System;
using System.Data;
using System.Diagnostics;
using System.Reflection;
using System.Xml;

namespace DFe
{
    class SerCTeSintetizador
    {
        #region " Variaveis "

        // Classes utilizadas
        private Facilitador clsFacil;
        private Log clsLog;
        private BdCTeSintetico clsBDSin;
        private NegCTeSintetico clsNeg;

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

        #endregion

        #region " Construtores "

        public SerCTeSintetizador(Facilitador clsFacilPar, string strBDCTeSinteticoPar, short intCodServicoPar, short intThreadPar)
        {
            // Inicializando classes
            clsFacil = clsFacilPar;

            // Inicializando variaveis
            strClasse = this.GetType().Name;
            intCodServico = intCodServicoPar;
            intThread = intThreadPar;

            // Inicializando classes de banco
            clsBDSin = new BdCTeSintetico(clsFacil, strBDCTeSinteticoPar);

            // Obtendo configuracao do banco
            this.ObterConfigBanco();

            // Inicializando classes de negocio
            clsLog = new Log(clsFacil, strBDCTeSinteticoPar, strClasse, strNomeServico, intCodServico, intThread, intLogEvento, intLogBanco, intLogCompleto);
            clsNeg = new NegCTeSintetico(clsFacil, clsLog, clsBDSin);
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
                    string strErro = "SerCTeSintetizador.Iniciar ERRO: " + ex.ToString();
                    System.Diagnostics.Debug.WriteLine(strErro);
                    string strDesktop = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "DFEND_CTe_Sintetizador_erro.txt");
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
                strChave = clsBDSin.RetirarFilaSintetizador();

                // Verificando se existe chave
                if (strChave != string.Empty)
                {
                    // Registrando log de informacao
                    clsLog.RegistrarLog(strMetodo, Constante.MsgChaveRetiradaFila + strChave, EventLogEntryType.Information);

                    // Informando que encontrou item
                    bolRetorno = true;

                    // Sintetizando documento apartir da chave retirada da fila
                    this.Sintetizar(strChave);
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

        #region " Sintetizar "

        public void Sintetizar(string strNSU)
        {
            // Obtendo nome do metodo
            string strMetodo = MethodBase.GetCurrentMethod().Name;

            try
            {
                // Obtendo dados do banco
                DataSet dstDados = clsBDSin.ObterTempFilaSintetizador(strNSU);

                // Verificando se houve retorno
                if ((dstDados != null) && (dstDados.Tables.Count > 0) && (dstDados.Tables[0].Rows.Count > 0))
                {
                    // Registrando log de informacao
                    clsLog.RegistrarLog(strMetodo, Constante.MsgLoteObtidoBanco + strNSU, EventLogEntryType.Information);

                    // Obtendo dados do retorno
                    string strEsquema = dstDados.Tables[0].Rows[0]["des_esquema"].ToString();
                    string strXML = dstDados.Tables[0].Rows[0]["xml_documento"].ToString();
                    string strProtocolo = dstDados.Tables[0].Rows[0]["num_protocolo"].ToString();
                    string strQtde = dstDados.Tables[0].Rows[0]["qtd_documento"].ToString();
                    string strData = dstDados.Tables[0].Rows[0]["dtc_autorizacao"].ToString();

                    // Obtendo XML
                    XmlDocument xmlDocumento = new XmlDocument();
                    xmlDocumento.LoadXml(strXML);

                    // Sintetizando documento de lote
                    clsNeg.SintetizarLote(xmlDocumento, strNSU);

                    // Excluindo documento sintetizado
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
                    clsBDSin.AtualizarTempFilaSintetizadorErro(strChave, strMensagem);

                    // Registrando log de informacao
                    clsLog.RegistrarLog(strMetodo, Constante.MsgDocAtualizado + strChave, EventLogEntryType.Information);

                    //// Enviando chave para fila
                    //clsBDSin.EnviarFilaSintetizador(strChave);

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
                clsBDSin.ExcluirTempFilaSintetizador(strNSU);

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
                if (clsBDSin.ObterQtdeFilaSintetizador() < intQtdeMaxFila)
                {
                    // Registrando log de informacao
                    clsLog.RegistrarLog(strMetodo, Constante.MsgFilaObterRegistros, EventLogEntryType.Information);

                    // Obtendo dados de registros nao arquivados
                    DataSet dstDados = clsBDSin.ObterTempFilaSintetizadorTop(intQtdeMaxFila);

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
                            clsBDSin.EnviarFilaSintetizador(strNSU);

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
