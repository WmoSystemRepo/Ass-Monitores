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
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "DFEND_CTe_Arquivador_erro.txt"),
                Path.Combine(Path.GetTempPath(), "DFEND_CTe_Arquivador_erro.txt"),
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DFEND_CTe_Arquivador_erro.txt")
            };
            foreach (string strCaminho in arrCaminhos)
            {
                try
                {
                    File.WriteAllText(strCaminho, strErro);
                    Debug.WriteLine("DFEND_CTe_Arquivador erro gravado em: " + strCaminho);
                    break;
                }
                catch
                {
                }
            }
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
                this.ObterConfigCTeArquivador();

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
                this.GravarErro("DFEND_CTe_Arquivador ERRO StartPooledThread: " + ex.ToString());
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
            SerCTeArquivador clsServ;
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
                        clsServ = new SerCTeArquivador(clsFacil, strBDCTeRecepcao, strBDCTeSintetico, intCodServico, intThread);
                        clsServ.Iniciar(ref datUltimaExecucao);

                        // Dormindo a thread pelo tempo configurado
                        Thread.Sleep((int)(dblIntervalo));
                    }
                    catch (Exception ex)
                    {
                        this.GravarErro("DFEND_CTe_Arquivador ERRO Thread " + intThread + ": " + ex.ToString());
                        try
                        {
                            EventLog.WriteEntry(strNomeServico, ex.Message, EventLogEntryType.Error);
                        }
                        catch
                        {
                        }
                        Thread.Sleep(5000);
                    }
                }
            }
            catch (Exception ex)
            {
                this.GravarErro("DFEND_CTe_Arquivador ERRO Run: " + ex.ToString());
            }
            finally
            {
                clsServ = null;
            }
        }

        #endregion        

        #region " ObterConfigCTeArquivador "

        protected void ObterConfigCTeArquivador()
        {
            // Classes e variaveis utilizadas
            Criptografia clsCript = new Criptografia();

            try
            {
                // Obtendo do config os itens Comuns
                AppSettingsReader appConfig = new AppSettingsReader();
                strNomeServico = Convert.ToString(appConfig.GetValue("NomeServico", typeof(string)));
                intCodServico = Convert.ToInt16(appConfig.GetValue("CodServicoArquivador", typeof(string)));

                // Desenvolvimento: aceita connection string em texto claro (Data Source=...)
                string strBDRecConfig = Convert.ToString(clsFacil.ValidarItemConfigArquivo(appConfig.GetValue("BDCTeRecepcao", typeof(string)), "BDCTeRecepcao"));
                if (strBDRecConfig.StartsWith("Data Source=", StringComparison.OrdinalIgnoreCase))
                {
                    strBDCTeRecepcao = strBDRecConfig;
                }
                else
                {
                    strBDCTeRecepcao = clsCript.Decriptar(strBDRecConfig);
                }

                string strBDSinConfig = Convert.ToString(clsFacil.ValidarItemConfigArquivo(appConfig.GetValue("BDCTeSintetico", typeof(string)), "BDCTeSintetico"));
                if (strBDSinConfig.StartsWith("Data Source=", StringComparison.OrdinalIgnoreCase))
                {
                    strBDCTeSintetico = strBDSinConfig;
                }
                else
                {
                    strBDCTeSintetico = clsCript.Decriptar(strBDSinConfig);
                }

                Debug.WriteLine("DFEND_CTe_Arquivador conexao iniciando. Recepcao=" + strBDCTeRecepcao + " Sintetico=" + strBDCTeSintetico);

                // Registrando log de informacao
                EventLog.WriteEntry(strNomeServico, Constante.MsgConfigObtidaArquivo, EventLogEntryType.Information);

                // Obtendo configuracao do banco
                this.ObterConfigBancoCTeArquivador(strBDCTeRecepcao);
                Debug.WriteLine("DFEND_CTe_Arquivador config banco OK. Threads=" + intThreads + " Intervalo=" + dblIntervalo);
            }
            catch (Exception ex)
            {
                this.GravarErro("DFEND_CTe_Arquivador ERRO config/banco: " + ex.ToString());
                throw;
            }
            finally
            {
                clsCript = null;
            }
        }

        #endregion

        #region " ObterConfigBancoCTeArquivador "

        protected void ObterConfigBancoCTeArquivador(string strBD)
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
