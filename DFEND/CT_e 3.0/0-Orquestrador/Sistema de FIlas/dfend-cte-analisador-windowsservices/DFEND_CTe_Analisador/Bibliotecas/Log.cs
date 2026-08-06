using System;
using System.Diagnostics;
using System.Reflection;

namespace DFe
{
    public class Log
    {
        #region " Variaveis "

        // Variaveis utilizadas
        private readonly string strConexao;
        private readonly string strClasse;
        private readonly string strNomeServico;
        private readonly short intCodServico;
        private readonly short intThread;
        private readonly short intLogEvento;
        private readonly short intLogBanco;
        private readonly short intLogCompleto;

        #endregion

        #region " Construtores "

        public Log(string strConexaoPar, string strClassePar, string strNomeServicoPar, short intCodServicoPar, short intThreadPar, short intLogEventoPar, short intLogBancoPar, short intLogCompletoPar)
        {
            // Inicializando variaveis
            strConexao = strConexaoPar;
            strClasse = strClassePar;
            strNomeServico = strNomeServicoPar;
            intCodServico = intCodServicoPar;
            intThread = intThreadPar;
            intLogEvento = intLogEventoPar;
            intLogBanco = intLogBancoPar;
            intLogCompleto = intLogCompletoPar;
        }

        #endregion

        #region " MontarMensagemLog "

        private string MontarMensagemLog(string strMetodo, string strMensagem, string strErroCompleto, EventLogEntryType objTipo)
        {
            // Classes e variaveis utilizadas
            string strLog = string.Empty;
            string strTipoLog = ((short)Constante.TipoOcorrencia.Sucesso).ToString();

            // Verificando o tipo do log
            if (objTipo == EventLogEntryType.Error)
            {
                strTipoLog = ((short)Constante.TipoOcorrencia.Erro).ToString();
            }
            else if (objTipo == EventLogEntryType.Warning)
            {
                strTipoLog = ((short)Constante.TipoOcorrencia.Alerta).ToString();
            }
            else if (objTipo == EventLogEntryType.FailureAudit)
            {
                strTipoLog = ((short)Constante.TipoOcorrencia.Rejeicao).ToString();
            }

            // Adicionando o tipo do log
            strLog = (strLog + strTipoLog + ": " + objTipo.ToString() + ". ");

            // Adicionando a mensagem
            if (strMensagem.Length < 1800)
            {
                strLog = (strLog + Environment.NewLine + "Mensagem: " + strMensagem + ". ");
            }
            else
            {
                strLog = (strLog + Environment.NewLine + "Mensagem: " + strMensagem.Remove(1800) + ". ");
            }

            // Adicionando o nome do método
            strLog = (strLog + Environment.NewLine + "Método: " + strMetodo + ". ");

            // Adicionando o nome da classe
            strLog = (strLog + Environment.NewLine + "Classe: " + strClasse + ". ");

            // Adicionando o nome da classe
            strLog = (strLog + Environment.NewLine + "Aplicação: " + strNomeServico + ". ");

            // Adicionando o nome da máquina
            strLog = (strLog + Environment.NewLine + "Máquina: " + Environment.MachineName + ". ");

            // Adicionando o numero da thread caso exista
            if (intThread > 0)
            {
                strLog = (strLog + Environment.NewLine + "Thread: " + intThread.ToString() + ". ");
            }

            // Adicionando a mensagem de erro completo
            if ((intLogCompleto == 1) && (strErroCompleto != string.Empty))
            {
                strLog = (strLog + Environment.NewLine + "Erro Completo: " + strErroCompleto + ". ");
            }

            return strLog;
        }

        #endregion

        #region " MontarLog "

        public string MontarLog(string strMetodo, string strMensagem, EventLogEntryType objTipo)
        {
            // Montando mensagem do log
            return this.MontarMensagemLog(strMetodo, strMensagem, string.Empty, objTipo);
        }

        #endregion

        #region " MontarLog "

        public string MontarLog(Exception objExcecao, EventLogEntryType objTipo)
        {
            // Montando mensagem do log
            return this.MontarMensagemLog(objExcecao.TargetSite.Name, objExcecao.Message, objExcecao.ToString(), objTipo);
        }

        #endregion

        #region " RegistrarLog "

        public void RegistrarLog(string strMetodo, string strMensagem, EventLogEntryType objTipo)
        {
            // Registrando log no EventViewer
            this.RegistrarLogEventViewer(strMetodo, strMensagem, objTipo);

            // Registrando log no Banco
            this.RegistrarLogBanco(strMetodo, strMensagem, objTipo);
        }

        #endregion

        #region " RegistrarLog "

        public void RegistrarLog(Exception objExcecao)
        {
            // Obtendo informacoes do log
            EventLogEntryType objTipo = EventLogEntryType.Error;
            string strMensagem = objExcecao.Message;
            string strMetodo = objExcecao.TargetSite.Name;

            // Verificando qual a excecao
            if ((strMensagem.ToUpper().Contains("TIMEOUT EXPIRED"))
                || (strMensagem.ToUpper().Contains("TIME OUT"))
                || (strMensagem.ToUpper().Contains("TIME-OUT"))
                || (strMensagem.ToUpper().Contains("TIMED OUT"))
                || (strMensagem.ToUpper().Contains("DEADLOCK"))
                || (strMensagem.ToUpper().Contains("UNABLE TO CONNECT"))
                || (strMensagem.ToUpper().Contains("UNABLE TO PROCESS"))
                || (strMensagem.ToUpper().Contains("NO LONGER USABLE"))
                || (strMensagem.ToUpper().Contains("NO LONGER AVAILABLE"))
                || (strMensagem.ToUpper().Contains("CONNECTION WAS CLOSED"))
                || (strMensagem.ToUpper().Contains("SERVER WAS NOT FOUND"))
                || (strMensagem.ToUpper().Contains("SERVER UNAVAILABLE"))
                || (strMensagem.ToUpper().Contains("SERVICE UNAVAILABLE"))
                || (strMensagem.ToUpper().Contains("FORBIDDEN"))
                || (strMensagem.ToUpper().Contains("BAD REQUEST"))
                || (strMensagem.ToUpper().Contains("BAD GATEWAY"))
                || (strMensagem.ToUpper().Contains("COULD NOT BE RESOLVED"))
                || (strMensagem.ToUpper().Contains("SYSTEM.NULLREFERENCEEXCEPTION"))
                || (strMensagem.ToUpper().Contains("SERPRO.SPED")))
            {
                objTipo = EventLogEntryType.Warning;
            }

            // Registrando log no EventViewer
            this.RegistrarLogEventViewer(strMetodo, strMensagem, objTipo);

            // Registrando log no Banco
            this.RegistrarLogBanco(strMetodo, strMensagem, objTipo);
        }

        #endregion

        #region " RegistrarLogEventViewer "

        private void RegistrarLogEventViewer(string strMetodo, string strMensagem, EventLogEntryType objTipo)
        {
            // Verificando o nivel no Config para poder gravar no EventViewer
            if ((intLogEvento == (short)Constante.TipoLog.ErroAlertaSucesso)
                || ((intLogEvento == (short)Constante.TipoLog.ErroAlerta) && (objTipo != EventLogEntryType.Information))
                || ((intLogEvento == (short)Constante.TipoLog.Erro) && (objTipo == EventLogEntryType.Error)))
            {
                // Montando log
                string strLog = this.MontarLog(strMetodo, strMensagem, objTipo);

                // Registrando log no EventViewer
                EventLog.WriteEntry(strNomeServico, strLog, objTipo);
            }
        }

        #endregion

        #region " RegistrarLogBanco "

        private void RegistrarLogBanco(string strMetodo, string strMensagem, EventLogEntryType objTipo)
        {
            // Classes e variaveis utilizadas
            //BdCTeSintetico clsBDExe = new BdCTeSintetico(strConexao);
            BdCTeAnalitico clsBDExe = new BdCTeAnalitico(strConexao);

            try
            {
                // Verificando o nivel no Config para poder gravar no Banco
                if ((intLogBanco == (short)Constante.TipoLog.ErroAlertaSucesso)
                    || ((intLogBanco == (short)Constante.TipoLog.ErroAlerta) && (objTipo != EventLogEntryType.Information))
                    || ((intLogBanco == (short)Constante.TipoLog.Erro) && (objTipo == EventLogEntryType.Error)))
                {
                    // Montando log
                    string strLog = this.MontarLog(strMetodo, strMensagem, objTipo);

                    // Inserindo o log no banco
                    clsBDExe.InserirLog(strLog, intCodServico.ToString());
                }
            }
            catch (Exception ex)
            {
                // Registrando log no EventViewer
                this.RegistrarLogEventViewer(MethodBase.GetCurrentMethod().Name, ex.Message, EventLogEntryType.Error);
                throw;
            }
            finally
            {
                clsBDExe = null;
            }
        }

        #endregion
    }
}

