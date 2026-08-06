using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;

namespace DFe
{
    class Threads
    {
        #region " Variaveis "

        // Classes utilizadas
        private Facilitador clsFacil;

        // Variaveis utilizadas Gerais
        private string strClasse;
        private string strNomeServico;
        private short intCodServico;
        private short intThreads;
        private double dblIntervalo;

        // Variaveis utilizadas de Bancos
        private string strBDCTeRecepcao;
        private string strBDCTeSintetico;
        private string strBDCTeAnalitico;
        private string strBDNFeHistorico;

        // Certificado e WS
        private string strCertificadoDigital;
        private string strWSCTeConsulta;
        private short intTipoAmbiente;

        // Variaveis de controle
        private static int intContThread = 0;

        #endregion

        #region " Construtores "

        public Threads()
        {
            // Inicializando variaveis
            clsFacil = new Facilitador();
            strClasse = this.GetType().Name;
        }

        #endregion

        #region " GravarErro "

        private void GravarErro(string strErro)
        {
            Debug.WriteLine(strErro);
            string[] arrCaminhos = new string[]
            {
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "DFEND_CTe_Carga_erro.txt"),
                Path.Combine(Path.GetTempPath(), "DFEND_CTe_Carga_erro.txt"),
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DFEND_CTe_Carga_erro.txt")
            };
            foreach (string strCaminho in arrCaminhos)
            {
                try
                {
                    File.WriteAllText(strCaminho, strErro);
                    Debug.WriteLine("DFEND_CTe_Carga erro gravado em: " + strCaminho);
                    break;
                }
                catch
                {
                }
            }
        }

        #endregion

        #region " ResolverConnectionString "

        private string ResolverConnectionString(Criptografia clsCript, string strValorBruto, string strChave)
        {
            string strValor = Convert.ToString(clsFacil.ValidarItemConfigArquivo(strValorBruto, strChave));
            if (strValor.StartsWith("Data Source=", StringComparison.OrdinalIgnoreCase))
            {
                return strValor;
            }
            return clsCript.Decriptar(strValor);
        }

        #endregion

        #region " StartPooledThread "

        public void StartPooledThread()
        {
            try
            {
                // Inicializando variaveis
                int intThreadCriada = 0;

                // Obtendo dados do arquivo de configuracao
                this.ObterConfigCTeCarga();

                // Criando a quantidade de threads informadas
                while (intThreadCriada < intThreads)
                {
                    // Adicionando ao contador
                    intThreadCriada++;

                    // Criando callback para o Pool de Threads
                    WaitCallback objCallback = new WaitCallback(this.RunPooledThread);
                    ThreadPool.QueueUserWorkItem(objCallback, null);
                }
            }
            catch (Exception ex)
            {
                this.GravarErro("DFEND_CTe_Carga ERRO StartPooledThread: " + ex.ToString());
                EventLog.WriteEntry(strNomeServico ?? "DFEND_CTe_Carga", ex.Message, EventLogEntryType.Error);
            }
        }

        #endregion

        #region " RunPooledThread "

        private void RunPooledThread(object state)
        {
            intContThread = (intContThread + 1);
            this.Run((short)intContThread);
        }

        #endregion

        #region " Run "

        public void Run(short intThread)
        {
            // Classes e variaveis utilizadas
            SerCTeCarga clsServ;
            DateTime datUltimaExecucao = DateTime.Now;

            try
            {
                // Registrando mensagem
                EventLog.WriteEntry(strNomeServico, "Thread " + intThread.ToString() + " criada com sucesso", EventLogEntryType.Information);

                while (true)
                {
                    try
                    {
                        // Iniciando processamento
                        clsServ = new SerCTeCarga(clsFacil, strBDCTeRecepcao, strBDCTeSintetico, strBDCTeAnalitico, strBDNFeHistorico, strCertificadoDigital, strWSCTeConsulta, intTipoAmbiente, intCodServico, intThread);
                        clsServ.Iniciar(ref datUltimaExecucao);

                        // Dormindo a thread pelo tempo configurado
                        Thread.Sleep((int)(dblIntervalo));
                    }
                    catch (Exception ex)
                    {
                        this.GravarErro("DFEND_CTe_Carga ERRO Thread " + intThread + ": " + ex.ToString());
                        EventLog.WriteEntry(strNomeServico, ex.Message, EventLogEntryType.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                this.GravarErro("DFEND_CTe_Carga ERRO Run: " + ex.ToString());
                EventLog.WriteEntry(strNomeServico, ex.Message, EventLogEntryType.Error);
            }
            finally
            {
                clsServ = null;
            }
        }

        #endregion        

        #region " ObterConfigCTeCarga "

        protected void ObterConfigCTeCarga()
        {
            // Classes e variaveis utilizadas
            Criptografia clsCript = new Criptografia();

            try
            {
                // Obtendo do config os itens Comuns
                AppSettingsReader appConfig = new AppSettingsReader();
                strNomeServico = Convert.ToString(appConfig.GetValue("NomeServico", typeof(string)));
                intCodServico = Convert.ToInt16(appConfig.GetValue("CodServicoCarga", typeof(string)));

                // Desenvolvimento: aceita connection string em texto claro (Data Source=...); Homolog/Prod decripta
                strBDCTeRecepcao = this.ResolverConnectionString(clsCript, Convert.ToString(appConfig.GetValue("BDCTeRecepcao", typeof(string))), "BDCTeRecepcao");
                strBDCTeSintetico = this.ResolverConnectionString(clsCript, Convert.ToString(appConfig.GetValue("BDCTeSintetico", typeof(string))), "BDCTeSintetico");
                strBDCTeAnalitico = this.ResolverConnectionString(clsCript, Convert.ToString(appConfig.GetValue("BDCTeAnalitico", typeof(string))), "BDCTeAnalitico");
                strBDNFeHistorico = this.ResolverConnectionString(clsCript, Convert.ToString(appConfig.GetValue("BDNFeDefinitivo", typeof(string))), "BDNFeDefinitivo");

                strCertificadoDigital = Convert.ToString(clsFacil.ValidarItemConfigArquivo(appConfig.GetValue("CertificadoDigital", typeof(string)), "CertificadoDigital"));
                strWSCTeConsulta = Convert.ToString(clsFacil.ValidarItemConfigArquivo(appConfig.GetValue("WSCTeConsulta", typeof(string)), "WSCTeConsulta"));
                intTipoAmbiente = Convert.ToInt16(clsFacil.ValidarItemConfigArquivo(appConfig.GetValue("TipoAmbiente", typeof(string)), "TipoAmbiente"));

                // Registrando log de informacao
                EventLog.WriteEntry(strNomeServico, Constante.MsgConfigObtidaArquivo, EventLogEntryType.Information);

                // Obtendo configuracao do banco
                this.ObterConfigBancoCTeCarga(strBDCTeRecepcao);
            }
            catch (Exception ex)
            {
                this.GravarErro("DFEND_CTe_Carga ERRO config/banco: " + ex.ToString());
                throw;
            }
            finally
            {
                clsCript = null;
            }
        }

        #endregion

        #region " ObterConfigBancoCTeCarga "

        protected void ObterConfigBancoCTeCarga(string strBD)
        {
            // Classes e variaveis utilizadas
            BdCTeRecepcao clsBDRec = new BdCTeRecepcao(clsFacil, strBD);

            try
            {
                // Obtendo configuracao do banco
                strNomeServico = Convert.ToString(clsFacil.ValidarItemConfigBanco(clsBDRec.ObterServico(intCodServico.ToString()), "NomeServico"));
                dblIntervalo = Convert.ToDouble(clsFacil.ValidarItemConfigBanco(clsBDRec.ObterConfiguracao(intCodServico.ToString(), "Intervalo"), "Intervalo"));
                intThreads = Convert.ToInt16(clsFacil.ValidarItemConfigBanco(clsBDRec.ObterConfiguracao(intCodServico.ToString(), "Threads"), "Threads"));

                // Atualizando o nome do servidor no banco
                if (!Environment.MachineName.ToUpper().StartsWith("SF"))
                {
                    clsBDRec.AtualizarServico(intCodServico.ToString(), Environment.MachineName);
                }

                // Registrando log de informacao
                EventLog.WriteEntry(strNomeServico, Constante.MsgConfigObtidaBanco, EventLogEntryType.Information);

                // Registrando log de inicializacao
                clsBDRec.InserirLog(clsFacil.MontarLogInicializacao(), intCodServico.ToString());
            }
            catch
            {
                throw;
            }
            finally
            {
                clsBDRec = null;
            }
        }

        #endregion
    }
}
