using System;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.Remoting.Messaging;

namespace DFe
{
    public class AcessoDados
    {
        #region " Variaveis "

        // Variaveis utilizadas
        protected SqlCommand sqlComando;
        private static string strConexaoStat = "StringConexao";
        public const string strConexaoConst = "StringConexao";

        #endregion

        #region " Construtores "

        public AcessoDados() 
        {
            sqlComando = new SqlCommand();
        }

        public AcessoDados(string strConexaoPar) 
        {
            sqlComando = new SqlCommand();
            strConexaoStat = strConexaoPar;
        }

        #endregion

        #region " CriarConexao "

        protected static SqlConnection CriarConexao(string strConexao)
        {
            try
            {
                // Criando conexao
                SqlConnection sqlConexao = new SqlConnection(strConexao);
                sqlConexao.Open();
                return sqlConexao;
            }
            catch (SqlException ex)
            {
                string strErro = "Falha SQL ao conectar. Number=" + ex.Number +
                    " Server=" + ex.Server +
                    " Message=" + ex.Message;
                System.Diagnostics.Debug.WriteLine(strErro);
                try
                {
                    System.Diagnostics.EventLog.WriteEntry("DFEND_CTe_Carga", strErro, System.Diagnostics.EventLogEntryType.Error);
                }
                catch
                {
                }
                throw new Exception(strErro, ex);
            }
        }

        #endregion

        #region " LimparParametro "

        public void LimparParametro() 
        {
            sqlComando.Parameters.Clear();
        }
    
        #endregion

        #region " AdicionarParametro "

        public void AdicionarParametro(object objValor, string strNome) 
        {
            SqlParameter sqlParametro = this.ObterParametro(objValor, this.ObterTiposParametrosSQL(objValor), strNome, ParameterDirection.Input, -1);
            sqlComando.Parameters.Add(sqlParametro);
        }
    
        #endregion

        #region " AdicionarParametro "

        public void AdicionarParametro(object objValor, SqlDbType sqlTipo, string strNome) 
        {
            SqlParameter sqlParametro = this.ObterParametro(objValor, sqlTipo, strNome, ParameterDirection.Input, -1);
            sqlComando.Parameters.Add(sqlParametro);
        }
    
        #endregion

        #region " ObterParametro "

        public SqlParameter ObterParametro(object ojValor, SqlDbType sqlTipo, string strNome, ParameterDirection sqlDirecao, Int32 intTamanho) 
        {
            SqlParameter sqlParametro = new SqlParameter(strNome, sqlTipo);
            sqlParametro.Value = ojValor;
            sqlParametro.Direction = sqlDirecao;

            if (intTamanho != -1)
            {
                sqlParametro.Size = intTamanho;
            }

            return sqlParametro;
        }
    
        #endregion

        #region " ObterTiposParametrosSQL "

        private SqlDbType ObterTiposParametrosSQL(object objValor)
        {
            SqlDbType sqlTipo = SqlDbType.NVarChar;

            switch (objValor.GetType().ToString())
            {
                case "System.String":
                    sqlTipo = SqlDbType.NVarChar;
                    break;
                case "System.Byte":
                    sqlTipo = SqlDbType.TinyInt;
                    break;
                case "System.Int16":
                    sqlTipo = SqlDbType.SmallInt;
                    break;
                case "System.Int32":
                    sqlTipo = SqlDbType.Int;
                    break;
                case "System.Int64":
                    sqlTipo = SqlDbType.BigInt;
                    break;
                case "System.Boolean":
                    sqlTipo = SqlDbType.Bit;
                    break;
                case "System.DateTime":
                    sqlTipo = SqlDbType.DateTime;
                    break;
                case "System.Decimal":
                    sqlTipo = SqlDbType.Decimal;
                    break;
                case "System.Double":
                    sqlTipo = SqlDbType.Float;
                    break;
                case "System.Single":
                    sqlTipo = SqlDbType.Real;
                    break;
            }

            return sqlTipo;
        }

        #endregion

        #region " ExecutarQuery "

        public int ExecutarQuery(string strClausula, string strConexao) 
        {
            // Classes e variaveis utilizadas
            SqlConnection sqlConexao = null;
            int intRetorno;

            try 
            {
                // Executando query
                sqlConexao = AcessoDados.CriarConexao(strConexao);
                this.PrepararComando(strClausula, CommandType.Text, sqlConexao, strConexao);
                intRetorno = sqlComando.ExecuteNonQuery();
            }
            catch
            {
                throw;
            }
            finally 
            {
                if (sqlConexao != null)
                {
                    sqlConexao.Close();
                }
                sqlComando.Dispose();
            }

            return intRetorno;
        }

        #endregion

        #region " ExecutarProcedure "

        public int ExecutarProcedure(string strClausula, string strConexao)
        {
            // Classes e variaveis utilizadas
            SqlConnection sqlConexao = null;
            int intRetorno;

            try
            {
                // Executando procedure
                sqlConexao = AcessoDados.CriarConexao(strConexao);
                this.PrepararComando(strClausula, CommandType.StoredProcedure, sqlConexao, strConexao);
                intRetorno = sqlComando.ExecuteNonQuery();
            }
            catch
            {
                throw;
            }
            finally
            {
                if (sqlConexao != null)
                {
                    sqlConexao.Close();
                }
                sqlComando.Dispose();
            }

            return intRetorno;
        }

        #endregion

        #region " ExecutarDataset "

        public DataSet ExecutarDataset(string strClausula, string strConexao)
        {
            // Classes e variaveis utilizadas
            SqlConnection sqlConexao = null;
            SqlDataAdapter sqlAdapter;
            DataSet dstRetorno = new DataSet();

            try
            {
                // Executando query com retorno em dataset
                sqlConexao = AcessoDados.CriarConexao(strConexao);
                this.PrepararComando(strClausula, CommandType.Text, sqlConexao, strConexao);
                sqlAdapter = new SqlDataAdapter(sqlComando);
                sqlAdapter.Fill(dstRetorno);
            }
            catch
            {
                throw;
            }
            finally
            {
                if (sqlConexao != null)
                {
                    sqlConexao.Close();
                }
                sqlComando.Dispose();
            }

            return dstRetorno;
        }

        #endregion

        #region " ExecutarScalar "

        public object ExecutarScalar(string strClausula, string strConexao) 
        {
            // Classes e variaveis utilizadas
            SqlConnection sqlConexao = null;
            object objRetorno;

            try 
            {
                // Executando query
                sqlConexao = AcessoDados.CriarConexao(strConexao);
                this.PrepararComando(strClausula, CommandType.Text, sqlConexao, strConexao);
                objRetorno = sqlComando.ExecuteScalar();
            }
            catch
            {
                throw;
            }
            finally 
            {
                if (sqlConexao != null)
                {
                    sqlConexao.Close();
                }
                sqlComando.Dispose();
            }

            return objRetorno;
        }
        
        #endregion

        #region " PrepararComando "

        protected void PrepararComando(string strClausula, CommandType tipComando, SqlConnection sqlConexao, string strConexao) 
        {
            SqlTransaction sqlTransacao = null;
            sqlTransacao = this.ObterTransacao(strConexaoStat);

            if (sqlTransacao != null)
            {
                sqlConexao = sqlTransacao.Connection;
                sqlComando.Transaction = sqlTransacao;
            }

            if (sqlConexao == null) 
            {
                sqlConexao = AcessoDados.CriarConexao(strConexao);
            }

            sqlComando.Connection = sqlConexao;
            sqlComando.CommandText = strClausula;
            sqlComando.CommandType = tipComando;
            sqlComando.CommandTimeout = sqlConexao.ConnectionTimeout;
        }
    
        #endregion

        #region " ObterTransacao "

        protected SqlTransaction ObterTransacao(string strConexao) 
        {
            SqlTransaction sqlTransacao = ((SqlTransaction)(CallContext.GetData(("transacao" + strConexao))));
            return sqlTransacao;
        }

        #endregion

        #region " BeginTransaction "

        public void BeginTransaction(string strConexao) 
        {
            IDbTransaction objTransacao = ((IDbTransaction)(CallContext.GetData(("transacao" + strConexaoConst))));
            Int32 intCont;

            if (objTransacao == null)
            {
                SqlConnection sqlConexao = AcessoDados.CriarConexao(strConexao);
                objTransacao = sqlConexao.BeginTransaction(IsolationLevel.ReadCommitted);
                CallContext.SetData(("transacao" + strConexaoConst), objTransacao);
                CallContext.SetData(("contadorTransacao" + strConexaoConst), 1);
                CallContext.SetData(("commitTransacao" + strConexaoConst), true);
            }
            else if (CallContext.GetData(("contadorTransacao" + strConexaoConst)) != null) 
            {
                intCont = (int)(CallContext.GetData(("contadorTransacao" + strConexaoConst)));
                intCont = (intCont + 1);
                CallContext.SetData(("contadorTransacao" + strConexaoConst), intCont);
            }        
        }
    
        #endregion

        #region " CommitTransaction "

        public void CommitTransaction() 
        {
            IDbConnection sqlConexaoCorrente = null;
            IDbTransaction sqlTransacaoCorrente = ((IDbTransaction)(CallContext.GetData(("transacao" + strConexaoConst))));
            int intCont;
            bool bolFecharConexao = false;
            bool bolCommit;

            if (sqlTransacaoCorrente != null)
            {
                try 
                {
                    if ((CallContext.GetData(("contadorTransacao" + strConexaoConst)) != null) && (CallContext.GetData(("commitTransacao" + strConexaoConst)) != null))
                    {
                        intCont = (int)(CallContext.GetData(("contadorTransacao" + strConexaoConst)));
                        bolCommit = (bool)(CallContext.GetData(("commitTransacao" + strConexaoConst)));

                        if (intCont == 1)
                        {
                            bolFecharConexao = true;
                            sqlConexaoCorrente = sqlTransacaoCorrente.Connection;

                            try
                            {
                                if (bolCommit)
                                {
                                    sqlTransacaoCorrente.Commit();
                                }
                                else
                                {
                                    sqlTransacaoCorrente.Rollback();
                                }
                            }
                            finally
                            {
                                CallContext.FreeNamedDataSlot(("transacao" + strConexaoConst));
                                CallContext.FreeNamedDataSlot(("contadorTransacao" + strConexaoConst));
                                CallContext.FreeNamedDataSlot(("commitTransacao" + strConexaoConst));
                            }
                        }
                        else
                        {
                            intCont = (intCont - 1);
                            CallContext.SetData(("contadorTransacao" + strConexaoConst), intCont);
                        }
                    }
                    else
                    {
                        throw new FormatException("Contexto de transação contém informações incompletas");
                    }
                }
                finally
                {
                    if ((bolFecharConexao) && (sqlConexaoCorrente != null))
                    {
                        if (sqlConexaoCorrente.State != ConnectionState.Closed)
                        {
                            sqlConexaoCorrente.Close();
                        }
                    }
                }
            }
        }
    
        #endregion

        #region " RollbackTransaction "

        public void RollbackTransaction() 
        {
            IDbConnection sqlConexaoCorrente = null;
            IDbTransaction sqlTransacaoCorrente = ((IDbTransaction)(CallContext.GetData(("transacao" + strConexaoConst))));
            int intCont;
            bool bolFecharConexao = false;

            if (sqlTransacaoCorrente != null)
            {
                try
                {
                    if ((CallContext.GetData(("contadorTransacao" + strConexaoConst)) != null) && (CallContext.GetData(("commitTransacao" + strConexaoConst)) != null))
                    {
                        intCont = (int)(CallContext.GetData(("contadorTransacao" + strConexaoConst)));
                        CallContext.SetData(("commitTransacao" + strConexaoConst), false);

                        if (intCont == 1)
                        {
                            bolFecharConexao = true;
                            sqlConexaoCorrente = sqlTransacaoCorrente.Connection;

                            try
                            {
                                sqlTransacaoCorrente.Rollback();
                            }
                            finally
                            {
                                CallContext.FreeNamedDataSlot(("transacao" + strConexaoConst));
                                CallContext.FreeNamedDataSlot(("contadorTransacao" + strConexaoConst));
                                CallContext.FreeNamedDataSlot(("commitTransacao" + strConexaoConst));
                            }
                        }
                        else
                        {
                            intCont = (intCont - 1);
                            CallContext.SetData(("contadorTransacao" + strConexaoConst), intCont);
                        }
                    }
                    else
                    {
                        throw new FormatException("Contexto de transação contém informações incompletas");
                    }
                }
                finally
                {
                    if ((bolFecharConexao) && (sqlConexaoCorrente != null))
                    {
                        if (sqlConexaoCorrente.State != ConnectionState.Closed)
                        {
                            sqlConexaoCorrente.Close();
                        }
                    }
                }
            }
        }
    
        #endregion
    }
}
