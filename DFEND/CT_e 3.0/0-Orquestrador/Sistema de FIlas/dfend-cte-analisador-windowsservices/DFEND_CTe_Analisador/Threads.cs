using System;
using System.Configuration;
using System.Diagnostics;
using System.Reflection;
using System.Threading;

namespace DFe
{
    class Threads
    {
        #region " Variaveis "

        // Variaveis utilizadas Gerais
        private string strClasse;
        private string strNomeServico;
        private short intCodServico;
        private short intThreads;
        private double dblIntervalo;        

        // Variaveis utilizadas de Bancos
        private string strBDCTeSintetico;
        private string strBDCTeAnalitico;
        
        // Variaveis de controle
        private static int intContThread = 0;

        #endregion

        #region " Construtores "

        public Threads()
        {
            // Inicializando variaveis
            strClasse = this.GetType().Name;
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
                this.ObterConfigCTeAnalisador();

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
                EventLog.WriteEntry(strNomeServico, ex.Message, EventLogEntryType.Error);
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
            SerCTeAnalisador clsServ;
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
                        clsServ = new SerCTeAnalisador(strBDCTeSintetico, strBDCTeAnalitico, intCodServico, intThread);
                        clsServ.Iniciar(ref datUltimaExecucao);

                        // Dormindo a thread pelo tempo configurado
                        Thread.Sleep((int)(dblIntervalo));
                    }
                    catch (Exception ex)
                    {
                        EventLog.WriteEntry(strNomeServico, ex.Message, EventLogEntryType.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry(strNomeServico, ex.Message, EventLogEntryType.Error);
            }
            finally
            {
                clsServ = null;
            }
        }

        #endregion        

        #region " ObterConfigCTeAnalisador "

        protected void ObterConfigCTeAnalisador()
        {
            // Classes e variaveis utilizadas
            Facilitador clsFacil = new Facilitador();
            Criptografia clsCript = new Criptografia();

            try
            {
                // Obtendo do config os itens Comuns
                AppSettingsReader appConfig = new AppSettingsReader();
                strNomeServico = Convert.ToString(appConfig.GetValue("NomeServico", typeof(string)));
                intCodServico = Convert.ToInt16(appConfig.GetValue("CodServicoAnalisador", typeof(string)));

                // Obtendo configuracao do arquivo
                strBDCTeSintetico = clsCript.Decriptar(Convert.ToString(clsFacil.ValidarItemConfigArquivo(appConfig.GetValue("BDCTeSintetico", typeof(string)), "BDCTeSintetico")));
                strBDCTeAnalitico = clsCript.Decriptar(Convert.ToString(clsFacil.ValidarItemConfigArquivo(appConfig.GetValue("BDCTeAnalitico", typeof(string)), "BDCTeAnalitico")));
                
                // Registrando log de informacao
                EventLog.WriteEntry(strNomeServico, Constante.MsgConfigObtidaArquivo, EventLogEntryType.Information);

                // Obtendo configuracao do banco
                this.ObterConfigBancoCTeAnalisador(strBDCTeAnalitico);
            }
            catch
            {
                throw;
            }
            finally
            {
                clsFacil = null;
                clsCript = null;
            }
        }

        #endregion

        #region " ObterConfigBancoCTeAnalisador "

        protected void ObterConfigBancoCTeAnalisador(string strBD)
        {
            // Classes e variaveis utilizadas
            //BdCTeSintetico clsBDSin = new BdCTeSintetico(strBD);
            BdCTeAnalitico  clsBDAna = new BdCTeAnalitico(strBD);
            Facilitador clsFacil = new Facilitador();

            try
            {
                // Obtendo configuracao do banco
                strNomeServico = Convert.ToString(clsFacil.ValidarItemConfigBanco(clsBDAna.ObterServico(intCodServico.ToString()), "NomeServico"));
                dblIntervalo =  Convert.ToDouble(clsFacil.ValidarItemConfigBanco(clsBDAna.ObterConfiguracao(intCodServico.ToString(), "Intervalo"), "Intervalo"));
                intThreads = Convert.ToInt16(clsFacil.ValidarItemConfigBanco(clsBDAna.ObterConfiguracao(intCodServico.ToString(), "Threads"), "Threads"));
                //intThreads = 1;

                // Atualizando o nome do servidor no banco
                if (!Environment.MachineName.ToUpper().StartsWith("SF"))
                {
                    clsBDAna.AtualizarServico(intCodServico.ToString(), Environment.MachineName);
                }

                clsBDAna.AtualizarConfiguracao(Convert.ToString(intCodServico), "Versão", Assembly.GetExecutingAssembly().GetName().Version.ToString(), "1");
                // Registrando log de informacao
                EventLog.WriteEntry(strNomeServico, Constante.MsgConfigObtidaBanco, EventLogEntryType.Information);

                // Registrando log de inicializacao
                clsBDAna.InserirLog(Constante.MsgServicoIniciado + clsFacil.FormatarVersao(Assembly.GetEntryAssembly().GetName()), intCodServico.ToString());
            }
            catch
            {
                throw;
            }
            finally
            {
                clsBDAna = null;
                clsFacil = null;
            }
        }

        #endregion
    }
}
