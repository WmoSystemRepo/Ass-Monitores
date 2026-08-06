using System;
using System.Diagnostics;
using System.Reflection;
using System.Xml;

namespace DFe
{
    class NegCTeAnalitico
    {
        #region " Variaveis "

        // Classes utilizadas
        private Facilitador clsFacil;
        private Log clsLog;
        private BdCTeAnalitico clsBDAna;

        #endregion

        #region " Construtores "

        public NegCTeAnalitico(Facilitador clsFacilPar, Log clsLogPar, BdCTeAnalitico clsBDAnaPar)
        {
            // Inicializando classes
            clsFacil = clsFacilPar;
            clsLog = clsLogPar;
            clsBDAna = clsBDAnaPar;
        }

        #endregion

        #region " AnalisarAutorizacao "

        public void AnalisarAutorizacao(DocCTe clsDoc)
        {
            try
            {
                // Inserindo nas tabelas de autorizacao
                this.InserirAutorizacao(clsDoc);
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region " AnalisarEvento "

        public void AnalisarEvento(DocCTeEvent clsDocEvent)
        {
            try
            {
                // Inserindo nas tabelas de evento
                this.InserirEvento(clsDocEvent);
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region " AnalisarInutilizacao "

        public void AnalisarInutilizacao(DocCTeInut clsDocInut)
        {
            try
            {
                // Nao destrincha inutilizacao
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region " InserirAutorizacao "

        private void InserirAutorizacao(DocCTe clsDoc)
        {
            // Obtendo nome do metodo
            string strMetodo = MethodBase.GetCurrentMethod().Name;

            try
            {
                // Inserindo dados no banco
                //clsBDAna.InserirXMLAutorizacao(clsDoc);

                // Registrando log de informacao
                clsLog.RegistrarLog(strMetodo, Constante.MsgDocAutorizacaoInseridoBanco + clsDoc.NSU + ". Chave: " + clsDoc.Chave, EventLogEntryType.Information);
            }
            catch (Exception ex)
            {
                // Verificando o tipo da excecao
                if (ex.ToString().ToUpper().Contains("PRIMARY KEY") || ex.ToString().ToUpper().Contains("DUPLICATE KEY"))
                {
                    // Atualizando dados no banco
                    this.AtualizarAutorizacao(clsDoc);
                }
                else
                {
                    throw;
                }
            }
        }

        #endregion

        #region " AtualizarAutorizacao "

        private void AtualizarAutorizacao(DocCTe clsDoc)
        {
            // Obtendo nome do metodo
            string strMetodo = MethodBase.GetCurrentMethod().Name;

            try
            {
                // Atualizando dados no banco
                //clsBDAna.AtualizarXMLAutorizacao(clsDoc);

                // Registrando log de informacao
                clsLog.RegistrarLog(strMetodo, Constante.MsgDocAutorizacaoAtualizadoBanco + clsDoc.NSU + ". Chave: " + clsDoc.Chave, EventLogEntryType.Information);
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region " InserirEvento "

        private void InserirEvento(DocCTeEvent clsDocEvent)
        {
            // Obtendo nome do metodo
            string strMetodo = MethodBase.GetCurrentMethod().Name;

            try
            {
                // Inserindo dados no banco
                //clsBDAna.InserirXMLEvento(clsDocEvent);

                // Registrando log de informacao
                clsLog.RegistrarLog(strMetodo, Constante.MsgDocEventoInseridoBanco + clsDocEvent.NSU + ". Chave: " + clsDocEvent.Chave, EventLogEntryType.Information);
            }
            catch (Exception ex)
            {
                // Verificando o tipo da excecao
                if (ex.ToString().ToUpper().Contains("PRIMARY KEY") || ex.ToString().ToUpper().Contains("DUPLICATE KEY"))
                {
                    // Atualizando dados no banco
                    this.AtualizarEvento(clsDocEvent);
                }
                else
                {
                    throw;
                }
            }
        }

        #endregion

        #region " AtualizarEvento "

        private void AtualizarEvento(DocCTeEvent clsDocEvent)
        {
            // Obtendo nome do metodo
            string strMetodo = MethodBase.GetCurrentMethod().Name;

            try
            {
                // Atualizando dados no banco
                //clsBDAna.AtualizarXMLEvento(clsDocEvent);

                // Registrando log de informacao
                clsLog.RegistrarLog(strMetodo, Constante.MsgDocEventoAtualizadoBanco + clsDocEvent.NSU + ". Chave: " + clsDocEvent.Chave, EventLogEntryType.Information);
            }
            catch
            {
                throw;
            }
        }

        #endregion
    }
}