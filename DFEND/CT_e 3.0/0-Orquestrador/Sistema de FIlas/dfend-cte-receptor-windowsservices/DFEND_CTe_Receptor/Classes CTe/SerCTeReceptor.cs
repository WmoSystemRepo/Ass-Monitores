using System;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Xml;

namespace DFe
{
    class SerCTeReceptor
    {
        #region " Variaveis "

        // Classes utilizadas
        private Facilitador clsFacil;
        private Log clsLog;
        private BdCTeRecepcao clsBDRec;
        private NegCTe clsNeg;

        // Variaveis utilizadas
        private string strClasse;
        private string strNomeServico;
        private readonly string strCertificadoDigital;
        private readonly short intCodServico;
        private readonly short intThread;
        private short intLogEvento;
        private short intLogBanco;
        private short intLogCompleto;
        private short intExecutar;
        private short intReBuscar;
        private short intPacoteCompleto;
        private short intWSTipoAmbiente;
        private short intWSCompactacao;
        private int intWSTimeOut;
        private string strWSVersao;
        private string strWSURL;
        private string strNSUAux = "NSUAux";
        private string strNSUAuxAut = "NSUAuxAut";
        private string strNSUAuxDest = "NSUAuxDest";

        #endregion

        #region " Construtores "

        public SerCTeReceptor(Facilitador clsFacilPar, string strBDCTeRecepcaoPar, string strCertificadoDigitalPar, short intCodServicoPar, short intThreadPar)
        {
            // Inicializando classes
            clsFacil = clsFacilPar;

            // Inicializando variaveis
            strClasse = this.GetType().Name;
            strCertificadoDigital = strCertificadoDigitalPar;
            intCodServico = intCodServicoPar;
            intThread = intThreadPar;

            // Inicializando classes de banco
            clsBDRec = new BdCTeRecepcao(clsFacil, strBDCTeRecepcaoPar);

            // Obtendo configuracao do banco
            this.ObterConfigBanco();

            // Inicializando classes de negocio
            clsLog = new Log(clsFacil, strBDCTeRecepcaoPar, strClasse, strNomeServico, intCodServico, intThread, intLogEvento, intLogBanco, intLogCompleto);
            clsNeg = new NegCTe(clsFacil);
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
                intReBuscar = Convert.ToInt16(clsFacil.ValidarItemConfigBanco(clsBDRec.ObterConfiguracao(intCodServico.ToString(), "ReBuscar"), "ReBuscar"));
                intPacoteCompleto = Convert.ToInt16(clsFacil.ValidarItemConfigBanco(clsBDRec.ObterConfiguracao(intCodServico.ToString(), "PacoteCompleto"), "PacoteCompleto"));
                intWSTipoAmbiente = Convert.ToInt16(clsFacil.ValidarItemConfigBanco(clsBDRec.ObterConfiguracao(intCodServico.ToString(), "WSTipoAmbiente"), "WSTipoAmbiente"));
                intWSCompactacao = Convert.ToInt16(clsFacil.ValidarItemConfigBanco(clsBDRec.ObterConfiguracao(intCodServico.ToString(), "WSCompactacao"), "WSCompactacao"));
                intWSTimeOut = Convert.ToInt32(clsFacil.ValidarItemConfigBanco(clsBDRec.ObterConfiguracao(intCodServico.ToString(), "WSTimeOut"), "WSTimeOut"));
                strWSVersao = Convert.ToString(clsFacil.ValidarItemConfigBanco(clsBDRec.ObterConfiguracao(intCodServico.ToString(), "WSVersao"), "WSVersao"));
                strWSURL = Convert.ToString(clsFacil.ValidarItemConfigBanco(clsBDRec.ObterConfiguracao(intCodServico.ToString(), "WSURL"), "WSURL"));
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

                    // Verificando se deve rebuscar NSUs antigos
                    if (intReBuscar == 1)
                    {
                        if ((datUltimaExecucao.Month != DateTime.Now.Month) && (intThread == 2))
                        {
                            this.AtualizarNSUAux("1");
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
                    string strErro = "SerCTeReceptor.Iniciar ERRO: " + ex.ToString();
                    System.Diagnostics.Debug.WriteLine(strErro);
                    string strDesktop = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "DFEND_CTe_Receptor_erro.txt");
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
            string strNSU = "0";
            bool bolRetorno = false;

            // Obtendo nome do metodo
            string strMetodo = MethodBase.GetCurrentMethod().Name;

            try
            {
                // Verificando qual a thread rodando
                if (intThread == 1)
                {
                    // Obtendo o NSU no banco
                    strNSU = clsBDRec.ObterNSU(intCodServico.ToString());

                    // Registrando log de informacao
                    clsLog.RegistrarLog(strMetodo, Constante.MsgNSUObtidoBanco + strNSU, EventLogEntryType.Information);

                    // Recepcionando documento apartir do NSU obtido
                    bolRetorno = this.Recepcionar(strNSU, 2, 1);
                }
                else if (intThread == 2)
                {
                    // Obtendo a configuracao no banco
                    strNSU = clsBDRec.ObterConfiguracao(intCodServico.ToString(), strNSUAux);

                    // Registrando log de informacao
                    clsLog.RegistrarLog(strMetodo, Constante.MsgNSUObtidoBanco + strNSU, EventLogEntryType.Information);

                    // Verificando o NSU obtido
                    if (strNSU != "0")
                    {
                        // Recepcionando documento apartir do NSU obtido
                        bolRetorno = this.Recepcionar(strNSU, 2, 1);
                    }
                }
                else if (intThread == 3)
                {
                    // Obtendo a configuracao no banco
                    strNSU = clsBDRec.ObterConfiguracao(intCodServico.ToString(), strNSUAuxAut);

                    // Registrando log de informacao
                    clsLog.RegistrarLog(strMetodo, Constante.MsgNSUObtidoBanco + strNSU, EventLogEntryType.Information);

                    // Verificando o NSU obtido
                    if (strNSU != "0")
                    {
                        // Recepcionando documento apartir do NSU obtido
                        bolRetorno = this.Recepcionar(strNSU, 0, 1);
                    }
                }
                else if (intThread == 4)
                {
                    // Obtendo a configuracao no banco
                    strNSU = clsBDRec.ObterConfiguracao(intCodServico.ToString(), strNSUAuxDest);

                    // Registrando log de informacao
                    clsLog.RegistrarLog(strMetodo, Constante.MsgNSUObtidoBanco + strNSU, EventLogEntryType.Information);

                    // Verificando o NSU obtido
                    if (strNSU != "0")
                    {
                        // Recepcionando documento apartir do NSU obtido
                        bolRetorno = this.Recepcionar(strNSU, 1, 1);
                    }
                }
                else if (intThread == 5)
                {
                    // Obtendo o NSU no arquivo
                    strNSU = clsFacil.ObterNSUArquivo();

                    // Registrando log de informacao
                    clsLog.RegistrarLog(strMetodo, Constante.MsgNSUObtidoArquivo + strNSU, EventLogEntryType.Information);

                    // Recepcionando documento apartir do NSU obtido
                    bolRetorno = this.Recepcionar(strNSU, 2, 1);
                }
            }
            catch
            {
                throw;
            }

            return bolRetorno;
        }

        #endregion

        #region " Recepcionar "

        protected bool Recepcionar(string strNSU, short intTipoConsulta, short intTipoRetorno)
        {
            // Classes e variaveis utilizadas
            bool bolRetorno = false;

            // Obtendo nome do metodo
            string strMetodo = MethodBase.GetCurrentMethod().Name;

            try
            {
                // Verificando se existe NSU
                if (strNSU != string.Empty)
                {
                    // Configurando o WebService
                    wsvCTeDistribuicaoSVD.CTeDistSVD wsvWebService = this.ConfigurarWSDistribuicao();

                    // Registrando log de informacao
                    clsLog.RegistrarLog(strMetodo, Constante.MsgWSConfigurado, EventLogEntryType.Information);

                    // Montando xml de envio
                    XmlDocument xmlEnvio = clsNeg.MontarXMLDistribuicao(strNSU, intTipoConsulta, intTipoRetorno, intWSTipoAmbiente, strWSVersao, Constante.CodUFBA);

                    // Obtendo retorno do WebService
                    XmlElement xmlRetorno = (XmlElement)wsvWebService.cteDistSVD(xmlEnvio.LastChild);

                    // Verificando se houve retorno
                    if (xmlRetorno != null)
                    {
                        // Obtendo elementos do XML
                        string strStatus = xmlRetorno["cStat"].InnerText;
                        string strMotivo = xmlRetorno["xMotivo"].InnerText;
                        string strMensagemRetorno = "Status: " + strStatus + ". Retorno: " + strMotivo;

                        // Registrando log de informacao
                        clsLog.RegistrarLog(strMetodo, Constante.MsgWSComunicado + strMensagemRetorno, EventLogEntryType.Information);

                        // Verificando o status do retorno
                        if (strStatus == ((short)Constante.TipoMensagem.Msg_118_DFeLocalizado).ToString())
                        {
                            // Obtendo quantidade de itens
                            int intQtdeItens = xmlRetorno.GetElementsByTagName("procComp").Count;

                            // Verificando tamanho do pacote
                            if (intQtdeItens >= 50)
                            {
                                // Informando que deve continuar buscando
                                bolRetorno = true;
                            }

                            // Verificando se e para baixar o pacote completo apenas
                            if ((intPacoteCompleto == 0) || ((intPacoteCompleto == 1) && (bolRetorno == true)))
                            {
                                // Obtendo elementos do XML
                                string strUltNSU = xmlRetorno["ultNSU"].InnerText;
                                string strData = DateTime.Now.ToString();

                                // Recepcionando o lote
                                this.RecepcionarLote(strNSU, strUltNSU, intQtdeItens, xmlRetorno, strData);

                                // Verificando os NSUs
                                if (Convert.ToInt64(strNSU) > Convert.ToInt64(strUltNSU))
                                {
                                    // Registrando log de erro
                                    clsLog.RegistrarLog(strMetodo, Constante.MsgLoteErroNSUMenor + Convert.ToInt64(strUltNSU).ToString() + " / " + Convert.ToInt64(strNSU).ToString() + Environment.NewLine + " XML Envio: " + xmlEnvio.OuterXml + Environment.NewLine + " XML Retorno: " + xmlRetorno.OuterXml, EventLogEntryType.Error);
                                }
                                else
                                {
                                    // Atualizando o NSU
                                    this.AtualizarNSU(strUltNSU, bolRetorno);
                                }
                            }
                        }
                        else if (strStatus == ((short)Constante.TipoMensagem.Msg_117_NenhumDFeLocalizado).ToString())
                        {
                            // Obtendo elementos do XML
                            string strUltNSU = clsFacil.ObterItemXML(xmlRetorno["ultNSU"]);
                            string strUltNSUSVRS = clsFacil.ObterItemXML(xmlRetorno["ultNSUSVRS"]);

                            // Verificando os NSUs
                            if (strUltNSU != string.Empty)
                            {
                                if (Convert.ToInt64(strNSU) > Convert.ToInt64(strUltNSU))
                                {
                                    // Registrando log de erro
                                    clsLog.RegistrarLog(strMetodo, Constante.MsgLoteErroNSUMenor + Convert.ToInt64(strUltNSU).ToString() + " / " + Convert.ToInt64(strNSU).ToString() + Environment.NewLine + " XML Envio: " + xmlEnvio.OuterXml + Environment.NewLine + " XML Retorno: " + xmlRetorno.OuterXml, EventLogEntryType.Error);
                                }
                                else if ((strUltNSUSVRS != string.Empty) && (Convert.ToInt64(strNSU) < Convert.ToInt64(strUltNSUSVRS)))
                                {
                                    // Informando que deve continuar buscando
                                    bolRetorno = true;

                                    // Calculando um novo NSU
                                    strNSU = (Convert.ToInt64(strNSU) + 1).ToString();

                                    // Atualizando o NSU
                                    this.AtualizarNSU(strNSU, bolRetorno);
                                }
                                else
                                {
                                    // Atualizando o NSU
                                    this.AtualizarNSU(strUltNSU, bolRetorno);
                                }
                            }
                        }
                        else if (strStatus == ((short)Constante.TipoMensagem.Msg_146_NSUSolicitadoMenorDisponivel).ToString())
                        {
                            // Obtendo elementos do XML
                            string strUltNSU = clsFacil.ObterItemXML(xmlRetorno["ultNSURet"]);

                            // Verificando os NSUs
                            if (strUltNSU != string.Empty)
                            {
                                if (Convert.ToInt64(strNSU) > Convert.ToInt64(strUltNSU))
                                {
                                    // Registrando log de erro
                                    clsLog.RegistrarLog(strMetodo, Constante.MsgLoteErroNSUMenor + Convert.ToInt64(strUltNSU).ToString() + " / " + Convert.ToInt64(strNSU).ToString() + Environment.NewLine + " XML Envio: " + xmlEnvio.OuterXml + Environment.NewLine + " XML Retorno: " + xmlRetorno.OuterXml, EventLogEntryType.Error);
                                }
                                else
                                {
                                    // Atualizando o NSU
                                    this.AtualizarNSU(strUltNSU, bolRetorno);
                                }
                            }
                        }
                        else if ((strStatus == ((short)Constante.TipoMensagem.Msg_730_NSUSolicitadoMuitoAntigo).ToString()) || (strStatus == ((short)Constante.TipoMensagem.Msg_992_NSUSolicitadoMuitoAntigo).ToString()))
                        {
                            // Informando que deve continuar buscando
                            bolRetorno = true;

                            // Calculando um novo NSU
                            strNSU = (Convert.ToInt64(strNSU) + 1).ToString();

                            // Atualizando o NSU
                            this.AtualizarNSU(strNSU, bolRetorno);
                        }
                        else if (strStatus == ((short)Constante.TipoMensagem.Msg_108_ServicoEmManutencao).ToString())
                        {
                            // Registrando log de informacao
                            clsLog.RegistrarLog(strMetodo, strMensagemRetorno, EventLogEntryType.Information);
                        }
                        else if (strStatus == ((short)Constante.TipoMensagem.Msg_285_CertificadoTransmissorDifereICP).ToString())
                        {
                            // Registrando log de alerta
                            clsLog.RegistrarLog(strMetodo, strMensagemRetorno, EventLogEntryType.Warning);
                        }
                        else
                        {
                            // Registrando log de alerta
                            clsLog.RegistrarLog(strMetodo, Constante.MsgWSRetornoNaoEsperado + strMensagemRetorno, EventLogEntryType.Warning);
                        }
                    }
                    else
                    {
                        // Registrando log de alerta
                        clsLog.RegistrarLog(strMetodo, Constante.MsgWSRetornoNaoEsperado, EventLogEntryType.Warning);
                    }
                }
            }
            catch
            {
                throw;
            }

            return bolRetorno;
        }

        #endregion

        #region " RecepcionarLote "

        private void RecepcionarLote(string strNSU, string strUltNSU, int intQteDocumentos, XmlElement xmlDocumento, string strData)
        {
            // Obtendo nome do metodo
            string strMetodo = MethodBase.GetCurrentMethod().Name;

            try
            {
                // Obtendo elementos do XML
                try
                {
                    strData = xmlDocumento.GetElementsByTagName("dhRecbto")[0].InnerText;
                }
                catch
                {
                    try
                    {
                        strData = xmlDocumento.GetElementsByTagName("dhRegEvento")[0].InnerText;
                    }
                    catch
                    {
                        strData = DateTime.Now.ToString();
                    }
                }

                // Inserindo dados no banco
                clsBDRec.InserirTempArquivador(strNSU, strUltNSU, string.Empty, intQteDocumentos.ToString(), xmlDocumento.OuterXml, xmlDocumento.Name, strData);

                // Registrando log de informacao
                clsLog.RegistrarLog(strMetodo, Constante.MsgLoteInseridoBanco + strNSU, EventLogEntryType.Information);

                // Enviando chave para fila
                clsBDRec.EnviarFilaArquivador(strNSU);

                // Registrando log de informacao
                clsLog.RegistrarLog(strMetodo, Constante.MsgLoteInseridoFila + strNSU, EventLogEntryType.Information);
            }
            catch (Exception ex)
            {
                // Verificando o tipo da excecao
                if (ex.ToString().ToUpper().Contains("PRIMARY KEY") || ex.ToString().ToUpper().Contains("DUPLICATE KEY"))
                {
                    // Registrando log de informacao
                    clsLog.RegistrarLog(strMetodo, Constante.MsgLoteJaExistente + strNSU, EventLogEntryType.Information);
                }
                else
                {
                    throw;
                }
            }
        }

        #endregion

        #region " AtualizarNSU "

        public void AtualizarNSU(string strNSU, bool bolRetorno)
        {
            // Obtendo nome do metodo
            string strMetodo = MethodBase.GetCurrentMethod().Name;

            try
            {
                // Verificando qual a thread rodando
                if (intThread == 1)
                {
                    // Atualizando o NSU no banco
                    clsBDRec.AtualizarNSU(intCodServico.ToString(), strNSU);

                    // Registrando log de informacao
                    clsLog.RegistrarLog(strMetodo, Constante.MsgNSUAtualizadoBanco + strNSU, EventLogEntryType.Information);
                }
                else if (intThread == 2)
                {
                    // Verificando se ficou online para zerar o NSU
                    if (!bolRetorno)
                    {
                        strNSU = "0";
                    }

                    // Atualizando a configuracao no banco
                    clsBDRec.AtualizarConfiguracao(intCodServico.ToString(), strNSUAux, strNSU, "1");

                    // Registrando log de informacao
                    clsLog.RegistrarLog(strMetodo, Constante.MsgNSUAtualizadoBanco + strNSU, EventLogEntryType.Information);
                }
                else if (intThread == 3)
                {
                    // Atualizando a configuracao no banco
                    clsBDRec.AtualizarConfiguracao(intCodServico.ToString(), strNSUAuxAut, strNSU, "1");

                    // Registrando log de informacao
                    clsLog.RegistrarLog(strMetodo, Constante.MsgNSUAtualizadoBanco + strNSU, EventLogEntryType.Information);
                }
                else if (intThread == 4)
                {
                    // Atualizando a configuracao no banco
                    clsBDRec.AtualizarConfiguracao(intCodServico.ToString(), strNSUAuxDest, strNSU, "1");

                    // Registrando log de informacao
                    clsLog.RegistrarLog(strMetodo, Constante.MsgNSUAtualizadoBanco + strNSU, EventLogEntryType.Information);
                }
                else if (intThread == 5)
                {
                    // Atualizando o NSU no arquivo
                    clsFacil.AtualizarNSUArquivo(strNSU);

                    // Registrando log de informacao
                    clsLog.RegistrarLog(strMetodo, Constante.MsgNSUAtualizadoArquivo + strNSU, EventLogEntryType.Information);
                }
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region " AtualizarNSUAux "

        public void AtualizarNSUAux(string strNSU)
        {
            // Obtendo nome do metodo
            string strMetodo = MethodBase.GetCurrentMethod().Name;

            try
            {
                // Atualizando a configuracao no banco
                clsBDRec.AtualizarConfiguracao(intCodServico.ToString(), strNSUAux, strNSU, "1");

                // Registrando log de informacao
                clsLog.RegistrarLog(strMetodo, Constante.MsgNSUAtualizadoBanco + strNSU, EventLogEntryType.Information);
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region " ConfigurarWSDistribuicao "

        public wsvCTeDistribuicaoSVD.CTeDistSVD ConfigurarWSDistribuicao()
        {
            // Classes e variaveis utilizadas
            wsvCTeDistribuicaoSVD.CTeDistSVD wsvRetorno = new wsvCTeDistribuicaoSVD.CTeDistSVD();

            // Configurando WebService
            wsvRetorno.Url = strWSURL;
            wsvRetorno.Timeout = intWSTimeOut;
            wsvRetorno.ClientCertificates.Add(clsFacil.ObterCertificado(strCertificadoDigital));
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            // Retornando WebService
            return wsvRetorno;
        }

        #endregion
    }
}
