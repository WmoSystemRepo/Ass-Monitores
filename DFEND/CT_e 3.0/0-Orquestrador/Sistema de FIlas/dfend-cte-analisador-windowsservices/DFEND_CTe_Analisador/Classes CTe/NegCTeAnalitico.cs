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
        private Log clsLog;
        private BdCTeAnalitico clsBDAna;

        #endregion

        #region " Construtores "

        public NegCTeAnalitico(Log clsLogPar, BdCTeAnalitico clsBDAnaPar)
        {
            // Inicializando classes
            clsLog = clsLogPar;
            clsBDAna = clsBDAnaPar;
        }

        #endregion

        #region " AnalisarAutorizacaoCTe "

        public void AnalisarAutorizacaoCTe(Common.SerializableClasses.CTe.xmlCTe_v400_NT202402.proc CTe)
        {
            try
            {
                // Inserindo nas tabelas de autorizacao                
                this.InserirAutorizacaoCTe(CTe);
            }
            catch
            {
                throw;
            }
        }

        public void AnalisarAutorizacaoCTeV2(Common.SerializableClasses.CTe.ClsCTe_v2.proc CTe)
        {
            try
            {
                // Inserindo nas tabelas de autorizacao                
                this.InserirAutorizacaoCTeV2(CTe);
            }
            catch
            {
                throw;
            }
        }

        public void AnalisarAutorizacaoCTeV1(Common.SerializableClasses.CTe.ClsCTe_v1.proc CTe)
        {
            try
            {
                // Inserindo nas tabelas de autorizacao                
                this.InserirAutorizacaoCTeV1(CTe);
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region " AnalisarAutorizacaoCTeSimp "

        public void AnalisarAutorizacaoCTeSimp(Common.SerializableClasses.CTe.xmlCTeSimp_v400.proc CTe)
        {
            try
            {
                // Inserindo nas tabelas de autorizacao                
                this.InserirAutorizacaoCTeSimp(CTe);
            }
            catch
            {
                throw;
            }
        }

        #endregion


        #region " AnalisarEvento "

        public void AnalisarEvento(Common.SerializableClasses.CTe.xmlEventoCTe_v400.proc eventoCTe)
        {
            try
            {
                if (eventoCTe.procEventoCTe.eventoCTe != null)
                {
                    // Inserindo na tabela de evento                
                    this.InserirEvento(eventoCTe);
                }
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region "AnalisarAutorizacaoGTVe"

        public void AnalisarAutorizacaoGTVe(Common.SerializableClasses.CTe.ClsXmlGTVeNT202402.proc GTVe)
        {
            try
            {
                // Inserindo nas tabelas de autorizacao                
                this.InserirAutorizacaoGTVe(GTVe);
            }
            catch
            {
                throw;
            }
        }

        #endregion


        public void AnalisarAutorizacaoCTeOS(Common.SerializableClasses.CTe.ClsCTeOS_v4.proc CTeOS)
        {
            // Obtendo nome do metodo
            string strMetodo = MethodBase.GetCurrentMethod().Name;

            try
            {

                clsBDAna.InserirXMLCTeOS(CTeOS);
                //Registrando log de informacao                
                clsLog.RegistrarLog(strMetodo, Constante.MsgDocAutorizacaoInseridoBanco + "NSU" + CTeOS.NSUSVD + ". Chave: " + CTeOS.cteOSProc.protCTe.InfProt.chCTe, EventLogEntryType.Information);

            }
            catch (Exception ex)
            {
                // Verificando o tipo da excecao
                if ((ex.ToString().ToUpper().Contains("PRIMARY KEY") || ex.ToString().ToUpper().Contains("DUPLICATE KEY")) == false)
                {
                    throw;
                }
            }
        }

        #region " InserirAutorizacaoCTe "

        private void InserirAutorizacaoCTe(Common.SerializableClasses.CTe.xmlCTe_v400_NT202402.proc CTe)
        {
            // Obtendo nome do metodo
            string strMetodo = MethodBase.GetCurrentMethod().Name;

            try
            {

                clsBDAna.InserirXMLCTe(CTe);
                //clsBDAna.InserirXMLCTe_porTabela(CTe);

                //Registrando log de informacao                
                clsLog.RegistrarLog(strMetodo, Constante.MsgDocAutorizacaoInseridoBanco + "NSU" + CTe.NSUSVD + ". Chave: " + CTe.CteProc.protCTe.InfProt.chCTe, EventLogEntryType.Information);

            }
            catch (Exception ex)
            {
                // Verificando o tipo da excecao
                if ((ex.ToString().ToUpper().Contains("PRIMARY KEY") || ex.ToString().ToUpper().Contains("DUPLICATE KEY")) == false)
                {
                    throw;
                }
            }
        }

        private void InserirAutorizacaoCTeV2(Common.SerializableClasses.CTe.ClsCTe_v2.proc CTe)
        {
            // Obtendo nome do metodo
            string strMetodo = MethodBase.GetCurrentMethod().Name;

            try
            {

                clsBDAna.InserirXMLCTeV2(CTe);
                //Registrando log de informacao                
                clsLog.RegistrarLog(strMetodo, Constante.MsgDocAutorizacaoInseridoBanco + "NSU" + CTe.NSUSVD + ". Chave: " + CTe.CteProc.protCTe.InfProt.chCTe, EventLogEntryType.Information);

            }
            catch (Exception ex)
            {
                // Verificando o tipo da excecao
                if ((ex.ToString().ToUpper().Contains("PRIMARY KEY") || ex.ToString().ToUpper().Contains("DUPLICATE KEY")) == false)
                {
                    throw;
                }
            }
        }

        private void InserirAutorizacaoCTeV1(Common.SerializableClasses.CTe.ClsCTe_v1.proc CTe)
        {
            // Obtendo nome do metodo
            string strMetodo = MethodBase.GetCurrentMethod().Name;

            try
            {

                clsBDAna.InserirXMLCTeV1(CTe);
                //Registrando log de informacao                
                clsLog.RegistrarLog(strMetodo, Constante.MsgDocAutorizacaoInseridoBanco + "NSU" + CTe.NSUSVD + ". Chave: " + CTe.CteProc.protCTe.InfProt.chCTe, EventLogEntryType.Information);

            }
            catch (Exception ex)
            {
                // Verificando o tipo da excecao
                if ((ex.ToString().ToUpper().Contains("PRIMARY KEY") || ex.ToString().ToUpper().Contains("DUPLICATE KEY")) == false)
                {
                    throw;
                }
            }
        }


        #endregion

        #region " InserirEvento "

        //private void InserirEvento(DocCTeEvent clsDocEvent)
        private void InserirEvento(Common.SerializableClasses.CTe.xmlEventoCTe_v400.proc eventoCTe)
        {
            // Obtendo nome do metodo
            string strMetodo = MethodBase.GetCurrentMethod().Name;

            try
            {
                // Inserindo dados no banco                
                clsBDAna.InserirXML_detalhe_xml_conhecimento_transporte_eletronico_evento(eventoCTe);

                // Registrando log de informacao                
                clsLog.RegistrarLog(strMetodo, Constante.MsgDocEventoInseridoBanco + eventoCTe.NSUSVD + ". Chave: " + eventoCTe.procEventoCTe.eventoCTe.infEvento.chCTe, EventLogEntryType.Information);
            }
            catch (Exception ex)
            {
                // Verificando o tipo da excecao
                if ((ex.ToString().ToUpper().Contains("PRIMARY KEY") || ex.ToString().ToUpper().Contains("DUPLICATE KEY")) == false)
                {
                    throw;
                }
            }
        }

        #endregion

        #region "InserirAutorizacaoGTVe"

        private void InserirAutorizacaoGTVe(Common.SerializableClasses.CTe.ClsXmlGTVeNT202402.proc GTVe)
        {
            // Obtendo nome do metodo
            string strMetodo = MethodBase.GetCurrentMethod().Name;

            try
            {

                clsBDAna.InserirXMLGTVe(GTVe);

                //Registrando log de informacao                
                clsLog.RegistrarLog(strMetodo, Constante.MsgDocAutorizacaoInseridoBanco + "NSU" + GTVe.NSUSVD + ". Chave: " + GTVe.GTVeProc.protCTe.InfProt.chCTe, EventLogEntryType.Information);

            }
            catch (Exception ex)
            {
                // Verificando o tipo da excecao
                if ((ex.ToString().ToUpper().Contains("PRIMARY KEY") || ex.ToString().ToUpper().Contains("DUPLICATE KEY")) == false)
                {
                    throw;
                }
            }
        }

        #endregion

        #region " InserirAutorizacaoSimp "

        private void InserirAutorizacaoCTeSimp(Common.SerializableClasses.CTe.xmlCTeSimp_v400.proc CTe)
        {
            // Obtendo nome do metodo
            string strMetodo = MethodBase.GetCurrentMethod().Name;

            try
            {

                clsBDAna.InserirXMLCTeSimp(CTe);

                //Registrando log de informacao                
                clsLog.RegistrarLog(strMetodo, Constante.MsgDocAutorizacaoInseridoBanco + "NSU" + CTe.NSUSVD + ". Chave: " + CTe.CteSimpProc.protCTe.InfProt.chCTe, EventLogEntryType.Information);

            }
            catch (Exception ex)
            {
                // Verificando o tipo da excecao
                if ((ex.ToString().ToUpper().Contains("PRIMARY KEY") || ex.ToString().ToUpper().Contains("DUPLICATE KEY")) == false)
                {
                    throw;
                }
            }
        }

        #endregion

    }
}