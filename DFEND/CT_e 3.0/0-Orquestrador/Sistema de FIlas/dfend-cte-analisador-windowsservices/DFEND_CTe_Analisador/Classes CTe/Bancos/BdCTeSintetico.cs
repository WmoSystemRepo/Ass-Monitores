using System;
using System.Data;
using System.Text;

namespace DFe
{
    class BdCTeSintetico
    {
        #region " Propriedades "

        public string Conexao { get; set; }

        #endregion

        #region " Construtores "

        public BdCTeSintetico(string strConexao)
        {
            // Inicializando propriedades
            this.Conexao = strConexao;
        }

        #endregion

        #region " InserirLog "

        public int InserirLog(string strDescLog, string strCodServico)
        {
            // Classes e variaveis utilizadas
            AcessoDados clsDados = new AcessoDados();
            Facilitador clsFacil = new Facilitador();
            StringBuilder stbSQL = new StringBuilder();
            int intRetorno = 0;

            // Inserindo Log
            try
            {
                // Montando query a ser executada
                stbSQL.Append(clsFacil.MontarQuery("INSERT INTO cte.log_sintetico_conhecimento_transporte_eletronico "));
                stbSQL.Append(clsFacil.MontarQuery("( "));
                stbSQL.Append(clsFacil.MontarQuery("  des_log, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_servico_sintetico_conhecimento_transporte_eletronico, "));
                stbSQL.Append(clsFacil.MontarQuery("  dtc_insercao, "));
                stbSQL.Append(clsFacil.MontarQuery("  dtc_atualizacao "));
                stbSQL.Append(clsFacil.MontarQuery(") "));
                stbSQL.Append(clsFacil.MontarQuery("VALUES "));
                stbSQL.Append(clsFacil.MontarQuery("( "));
                stbSQL.Append(clsFacil.MontarQuery("  substring(@des_log, 0, 2000), "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_servico, "));
                stbSQL.Append(clsFacil.MontarQuery("  getdate(), "));
                stbSQL.Append(clsFacil.MontarQuery("  getdate() "));
                stbSQL.Append(clsFacil.MontarQuery(") "));
                string strSQL = stbSQL.ToString();

                // Montando parametros
                clsDados.LimparParametro();
                clsFacil.AdicionarParametro(ref strSQL, strDescLog, "@des_log", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, strCodServico, "@cod_servico", SqlDbType.VarChar);

                // Executando query
                clsDados.BeginTransaction(this.Conexao);
                intRetorno = clsDados.ExecutarQuery(strSQL, this.Conexao);
                clsDados.CommitTransaction();
            }
            catch
            {
                clsDados.RollbackTransaction();
                throw;
            }
            finally
            {
                stbSQL = null;
                clsDados = null;
                clsFacil = null;
            }

            return intRetorno;
        }

        #endregion
        
        #region " ObterServicos "

        public DataSet ObterServicos()
        {
            // Classes e variaveis utilizadas
            AcessoDados clsDados = new AcessoDados();
            Facilitador clsFacil = new Facilitador();
            StringBuilder stbSQL = new StringBuilder();
            DataSet dstRetorno;

            try
            {
                // Montando query a ser executada
                stbSQL.Append(clsFacil.MontarQuery("SELECT "));
                stbSQL.Append(clsFacil.MontarQuery("  c.seq_configuracao_sintetico_conhecimento_transporte_eletronico as seq_configuracao, "));
                stbSQL.Append(clsFacil.MontarQuery("  c.cod_servico_sintetico_conhecimento_transporte_eletronico as cod_servico, "));
                stbSQL.Append(clsFacil.MontarQuery("  s.des_servico, "));
                stbSQL.Append(clsFacil.MontarQuery("  c.des_configuracao, "));
                stbSQL.Append(clsFacil.MontarQuery("  c.nom_configuracao, "));
                stbSQL.Append(clsFacil.MontarQuery("  c.sts_ativo, "));
                stbSQL.Append(clsFacil.MontarQuery("  c.dtc_atualizacao "));
                stbSQL.Append(clsFacil.MontarQuery("FROM "));
                stbSQL.Append(clsFacil.MontarQuery("  cte.configuracao_sintetico_conhecimento_transporte_eletronico c WITH (READPAST) "));
                stbSQL.Append(clsFacil.MontarQuery("  INNER JOIN cte.servico_sintetico_conhecimento_transporte_eletronico s WITH (READPAST) "));
                stbSQL.Append(clsFacil.MontarQuery("  ON (c.cod_servico_sintetico_conhecimento_transporte_eletronico = s.cod_servico_sintetico_conhecimento_transporte_eletronico) "));
                string strSQL = stbSQL.ToString();

                // Informando parametros
                clsDados.LimparParametro();

                // Executando query
                dstRetorno = clsDados.ExecutarDataset(strSQL, this.Conexao);
            }
            catch
            {
                throw;
            }
            finally
            {
                stbSQL = null;
                clsDados = null;
                clsFacil = null;
            }

            return dstRetorno;
        }

        #endregion

        #region " ObterServico "

        public string ObterServico(string strCodServico)
        {
            // Classes e variaveis utilizadas
            AcessoDados clsDados = new AcessoDados();
            Facilitador clsFacil = new Facilitador();
            StringBuilder stbSQL = new StringBuilder();
            string strRetorno = string.Empty;

            try
            {
                // Montando query a ser executada
                stbSQL.Append(clsFacil.MontarQuery("SELECT TOP(1) "));
                stbSQL.Append(clsFacil.MontarQuery("  des_servico, "));
                stbSQL.Append(clsFacil.MontarQuery("  dtc_atualizacao "));
                stbSQL.Append(clsFacil.MontarQuery("FROM "));
                stbSQL.Append(clsFacil.MontarQuery("  cte.servico_sintetico_conhecimento_transporte_eletronico WITH (READPAST) "));
                stbSQL.Append(clsFacil.MontarQuery("WHERE "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_servico_sintetico_conhecimento_transporte_eletronico = @cod_servico "));
                string strSQL = stbSQL.ToString();

                // Informando parametros
                clsDados.LimparParametro();
                clsFacil.AdicionarParametro(ref strSQL, strCodServico, "@cod_servico", SqlDbType.SmallInt);

                // Executando query
                DataSet dstRetorno = clsDados.ExecutarDataset(strSQL, this.Conexao);
                if ((dstRetorno != null) && (dstRetorno.Tables.Count > 0) && (dstRetorno.Tables[0].Rows.Count > 0))
                {
                    strRetorno = dstRetorno.Tables[0].Rows[0]["des_servico"].ToString();
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                stbSQL = null;
                clsDados = null;
                clsFacil = null;
            }

            return strRetorno;
        }

        #endregion

        #region " AtualizarServico "

        public int AtualizarServico(string strCodServico, string strServidor)
        {
            // Classes e variaveis utilizadas
            AcessoDados clsDados = new AcessoDados();
            Facilitador clsFacil = new Facilitador();
            StringBuilder stbSQL = new StringBuilder();
            int intRetorno = 0;

            try
            {
                // Montando query a ser executada
                stbSQL.Append(clsFacil.MontarQuery("UPDATE "));
                stbSQL.Append(clsFacil.MontarQuery("  cte.servico_sintetico_conhecimento_transporte_eletronico "));
                stbSQL.Append(clsFacil.MontarQuery("SET "));
                stbSQL.Append(clsFacil.MontarQuery("  nom_servidor = UPPER(@nom_servidor), "));
                stbSQL.Append(clsFacil.MontarQuery("  dtc_execucao = getdate(), "));
                stbSQL.Append(clsFacil.MontarQuery("  dtc_atualizacao = getdate() "));
                stbSQL.Append(clsFacil.MontarQuery("WHERE "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_servico_sintetico_conhecimento_transporte_eletronico = @cod_servico "));
                string strSQL = stbSQL.ToString();

                // Montando parametros
                clsDados.LimparParametro();
                clsFacil.AdicionarParametro(ref strSQL, strCodServico, "@cod_servico", SqlDbType.SmallInt);
                clsFacil.AdicionarParametro(ref strSQL, strServidor, "@nom_servidor", SqlDbType.VarChar);

                // Executando query
                clsDados.BeginTransaction(this.Conexao);
                intRetorno = clsDados.ExecutarQuery(strSQL, this.Conexao);
                clsDados.CommitTransaction();
            }
            catch
            {
                clsDados.RollbackTransaction();
                throw;
            }
            finally
            {
                stbSQL = null;
                clsDados = null;
                clsFacil = null;
            }

            return intRetorno;
        }

        #endregion

        #region " ObterConfiguracoes "

        public DataSet ObterConfiguracoes(string strCodServico, string strDescConfig, string strValorConfig, string strServidor)
        {
            // Classes e variaveis utilizadas
            AcessoDados clsDados = new AcessoDados();
            Facilitador clsFacil = new Facilitador();
            StringBuilder stbSQL = new StringBuilder();
            DataSet dstRetorno;

            try
            {
                // Montando query a ser executada
                stbSQL.Append(clsFacil.MontarQuery("SELECT "));
                stbSQL.Append(clsFacil.MontarQuery("  c.seq_configuracao_sintetico_conhecimento_transporte_eletronico as seq_configuracao, "));
                stbSQL.Append(clsFacil.MontarQuery("  c.cod_servico_sintetico_conhecimento_transporte_eletronico as cod_servico, "));
                stbSQL.Append(clsFacil.MontarQuery("  s.des_servico, "));
                stbSQL.Append(clsFacil.MontarQuery("  s.nom_servidor, "));
                stbSQL.Append(clsFacil.MontarQuery("  c.des_configuracao, "));
                stbSQL.Append(clsFacil.MontarQuery("  c.nom_configuracao, "));
                stbSQL.Append(clsFacil.MontarQuery("  c.sts_ativo, "));
                stbSQL.Append(clsFacil.MontarQuery("  c.dtc_atualizacao "));
                stbSQL.Append(clsFacil.MontarQuery("FROM "));
                stbSQL.Append(clsFacil.MontarQuery("  cte.configuracao_sintetico_conhecimento_transporte_eletronico c WITH (READPAST) "));
                stbSQL.Append(clsFacil.MontarQuery("  INNER JOIN cte.servico_sintetico_conhecimento_transporte_eletronico s WITH (READPAST) "));
                stbSQL.Append(clsFacil.MontarQuery("  ON (c.cod_servico_sintetico_conhecimento_transporte_eletronico = s.cod_servico_sintetico_conhecimento_transporte_eletronico) "));
                stbSQL.Append(clsFacil.MontarQuery("WHERE "));
                stbSQL.Append(clsFacil.MontarQuery("  sts_ativo = 1 "));
                if (strCodServico != string.Empty)
                {
                    stbSQL.Append(clsFacil.MontarQuery("  AND c.cod_servico_sintetico_conhecimento_transporte_eletronico = @cod_servico "));
                }
                if (strDescConfig != string.Empty)
                {
                    stbSQL.Append(clsFacil.MontarQuery("  AND c.des_configuracao = @des_configuracao "));
                }
                if (strValorConfig != string.Empty)
                {
                    stbSQL.Append(clsFacil.MontarQuery("  AND c.nom_configuracao = @nom_configuracao "));
                }
                if (strServidor != string.Empty)
                {
                    stbSQL.Append(clsFacil.MontarQuery("  AND UPPER(s.nom_servidor) = UPPER(@nom_servidor) "));
                }
                string strSQL = stbSQL.ToString();

                // Informando parametros
                clsDados.LimparParametro();
                clsFacil.AdicionarParametro(ref strSQL, strCodServico, "@cod_servico", SqlDbType.SmallInt);
                clsFacil.AdicionarParametro(ref strSQL, strDescConfig, "@des_configuracao", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, strValorConfig, "@nom_configuracao", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, strServidor, "@nom_servidor", SqlDbType.VarChar);

                // Executando query
                dstRetorno = clsDados.ExecutarDataset(strSQL, this.Conexao);
            }
            catch
            {
                throw;
            }
            finally
            {
                stbSQL = null;
                clsDados = null;
                clsFacil = null;
            }

            return dstRetorno;
        }

        #endregion

        #region " ObterConfiguracao "

        public string ObterConfiguracao(string strCodServico, string strDescConfig)
        {
            // Classes e variaveis utilizadas
            AcessoDados clsDados = new AcessoDados();
            Facilitador clsFacil = new Facilitador();
            StringBuilder stbSQL = new StringBuilder();
            string strRetorno = string.Empty;

            try
            {
                // Montando query a ser executada
                stbSQL.Append(clsFacil.MontarQuery("SELECT TOP(1) "));
                stbSQL.Append(clsFacil.MontarQuery("  nom_configuracao, "));
                stbSQL.Append(clsFacil.MontarQuery("  dtc_atualizacao "));
                stbSQL.Append(clsFacil.MontarQuery("FROM "));
                stbSQL.Append(clsFacil.MontarQuery("  cte.configuracao_sintetico_conhecimento_transporte_eletronico WITH (READPAST) "));
                stbSQL.Append(clsFacil.MontarQuery("WHERE "));
                stbSQL.Append(clsFacil.MontarQuery("  sts_ativo = 1 "));
                stbSQL.Append(clsFacil.MontarQuery("  AND cod_servico_sintetico_conhecimento_transporte_eletronico = @cod_servico "));
                stbSQL.Append(clsFacil.MontarQuery("  AND des_configuracao = @des_configuracao "));
                string strSQL = stbSQL.ToString();

                // Informando parametros
                clsDados.LimparParametro();
                clsFacil.AdicionarParametro(ref strSQL, strCodServico, "@cod_servico", SqlDbType.SmallInt);
                clsFacil.AdicionarParametro(ref strSQL, strDescConfig, "@des_configuracao", SqlDbType.VarChar);

                // Executando query
                DataSet dstRetorno = clsDados.ExecutarDataset(strSQL, this.Conexao);
                if ((dstRetorno != null) && (dstRetorno.Tables.Count > 0) && (dstRetorno.Tables[0].Rows.Count > 0))
                {
                    strRetorno = dstRetorno.Tables[0].Rows[0]["nom_configuracao"].ToString();
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                stbSQL = null;
                clsDados = null;
                clsFacil = null;
            }

            return strRetorno;
        }

        #endregion

        #region " AtualizarConfiguracao "

        public int AtualizarConfiguracao(string strCodServico, string strDescConfig, string strValorConfig, string strAtivo)
        {
            // Classes e variaveis utilizadas
            AcessoDados clsDados = new AcessoDados();
            Facilitador clsFacil = new Facilitador();
            StringBuilder stbSQL = new StringBuilder();
            int intRetorno = 0;

            try
            {
                // Montando query a ser executada
                stbSQL.Append(clsFacil.MontarQuery("UPDATE "));
                stbSQL.Append(clsFacil.MontarQuery("  cte.configuracao_sintetico_conhecimento_transporte_eletronico "));
                stbSQL.Append(clsFacil.MontarQuery("SET "));
                stbSQL.Append(clsFacil.MontarQuery("  nom_configuracao = @nom_configuracao, "));
                stbSQL.Append(clsFacil.MontarQuery("  sts_ativo = @sts_ativo, "));
                stbSQL.Append(clsFacil.MontarQuery("  dtc_atualizacao = getdate() "));
                stbSQL.Append(clsFacil.MontarQuery("WHERE "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_servico_sintetico_conhecimento_transporte_eletronico = @cod_servico "));
                stbSQL.Append(clsFacil.MontarQuery("  AND des_configuracao = @des_configuracao "));
                string strSQL = stbSQL.ToString();

                // Montando parametros
                clsDados.LimparParametro();
                clsFacil.AdicionarParametro(ref strSQL, strCodServico, "@cod_servico", SqlDbType.SmallInt);
                clsFacil.AdicionarParametro(ref strSQL, strDescConfig, "@des_configuracao", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, strValorConfig, "@nom_configuracao", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, strAtivo, "@sts_ativo", SqlDbType.TinyInt);

                // Executando query
                clsDados.BeginTransaction(this.Conexao);
                intRetorno = clsDados.ExecutarQuery(strSQL, this.Conexao);
                clsDados.CommitTransaction();
            }
            catch
            {
                clsDados.RollbackTransaction();
                throw;
            }
            finally
            {
                stbSQL = null;
                clsDados = null;
                clsFacil = null;
            }

            return intRetorno;
        }

        #endregion
      
        #region " ObterAutorizacao "

        public DataSet ObterAutorizacao(string strDataReferencia, string strChaveAcesso)
        {
            // Classes e variaveis utilizadas
            AcessoDados clsDados = new AcessoDados();
            Facilitador clsFacil = new Facilitador();
            StringBuilder stbSQL = new StringBuilder();
            DataSet dstRetorno;

            try
            {
                // Montando query a ser executada
                stbSQL.Append(clsFacil.MontarQuery("SELECT "));
                stbSQL.Append(clsFacil.MontarQuery("  dtr_referencia, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_protocolo, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_sequencial_unico, "));
                stbSQL.Append(clsFacil.MontarQuery("  convert(varchar(max),xml_documento) as xml_documento, "));
                stbSQL.Append(clsFacil.MontarQuery("  dtc_documento, "));
                stbSQL.Append(clsFacil.MontarQuery("  dtc_insercao, "));
                stbSQL.Append(clsFacil.MontarQuery("  dtc_atualizacao "));
                stbSQL.Append(clsFacil.MontarQuery("FROM "));
                stbSQL.Append(clsFacil.MontarQuery("  cte.documento_conhecimento_transporte_eletronico_autorizacao WITH (READPAST) "));
                stbSQL.Append(clsFacil.MontarQuery("WHERE "));
                stbSQL.Append(clsFacil.MontarQuery("  dtr_referencia = @dtr_referencia "));
                stbSQL.Append(clsFacil.MontarQuery("  AND cod_chave_acesso = @cod_chave_acesso "));
                string strSQL = stbSQL.ToString();

                // Informando parametros
                clsDados.LimparParametro();
                clsFacil.AdicionarParametro(ref strSQL, strDataReferencia, "@dtr_referencia", SqlDbType.Int);
                clsFacil.AdicionarParametro(ref strSQL, strChaveAcesso, "@cod_chave_acesso", SqlDbType.Char);

                // Executando query
                dstRetorno = clsDados.ExecutarDataset(strSQL, this.Conexao);                               
            }
            catch
            {
                throw;
            }
            finally
            {
                stbSQL = null;
                clsDados = null;
                clsFacil = null;
            }

            return dstRetorno;
        }

        #endregion

        #region " ObterEvento "

        public DataSet ObterEvento(string strDataReferencia, string strChaveAcesso, string strTipoEvento, string strSeqEvento)
        {
            // Classes e variaveis utilizadas
            AcessoDados clsDados = new AcessoDados();
            Facilitador clsFacil = new Facilitador();
            StringBuilder stbSQL = new StringBuilder();
            DataSet dstRetorno;

            try
            {
                // Montando query a ser executada
                stbSQL.Append(clsFacil.MontarQuery("SELECT "));
                stbSQL.Append(clsFacil.MontarQuery("  dtr_referencia, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_tipo_evento_documento_fiscal_eletronico, "));
                stbSQL.Append(clsFacil.MontarQuery("  seq_evento_documento_fiscal_eletronico, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_protocolo, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_sequencial_unico, "));
                stbSQL.Append(clsFacil.MontarQuery("  convert(varchar(max),xml_documento) as xml_documento, "));
                stbSQL.Append(clsFacil.MontarQuery("  dtc_documento, "));
                stbSQL.Append(clsFacil.MontarQuery("  dtc_insercao, "));
                stbSQL.Append(clsFacil.MontarQuery("  dtc_atualizacao "));
                stbSQL.Append(clsFacil.MontarQuery("FROM "));
                stbSQL.Append(clsFacil.MontarQuery("  cte.documento_conhecimento_transporte_eletronico_evento WITH (READPAST) "));
                stbSQL.Append(clsFacil.MontarQuery("WHERE "));
                stbSQL.Append(clsFacil.MontarQuery("  dtr_referencia = @dtr_referencia "));
                stbSQL.Append(clsFacil.MontarQuery("  AND cod_chave_acesso = @cod_chave_acesso "));
                if (strTipoEvento != string.Empty)
                {
                    stbSQL.Append(clsFacil.MontarQuery("  AND cod_tipo_evento_documento_fiscal_eletronico = @cod_tipo_evento_documento_fiscal_eletronico "));
                }
                if (strSeqEvento != string.Empty)
                {
                    stbSQL.Append(clsFacil.MontarQuery("  AND seq_evento_documento_fiscal_eletronico = @seq_evento_documento_fiscal_eletronico "));
                }
                string strSQL = stbSQL.ToString();

                // Informando parametros
                clsDados.LimparParametro();
                clsFacil.AdicionarParametro(ref strSQL, strDataReferencia, "@dtr_referencia", SqlDbType.Int);
                clsFacil.AdicionarParametro(ref strSQL, strChaveAcesso, "@cod_chave_acesso", SqlDbType.Char);
                clsFacil.AdicionarParametro(ref strSQL, strTipoEvento, "@cod_tipo_evento_documento_fiscal_eletronico", SqlDbType.Int);
                clsFacil.AdicionarParametro(ref strSQL, strSeqEvento, "@seq_evento_documento_fiscal_eletronico", SqlDbType.TinyInt);

                // Executando query
                dstRetorno = clsDados.ExecutarDataset(strSQL, this.Conexao);               
               
            }
            catch
            {
                throw;
            }
            finally
            {
                stbSQL = null;
                clsDados = null;
                clsFacil = null;
            }

            return dstRetorno;
        }

        #endregion

        #region " ObterInutilizacao "

        public DataSet ObterInutilizacao(string strAno, string strSerie, string strFaixaInicial, string strFaixaFinal, string strCNPJCPFBase, string strCNPJCPFFilial, string strCNPJCPFDigito)
        {
            // Classes e variaveis utilizadas
            AcessoDados clsDados = new AcessoDados();
            Facilitador clsFacil = new Facilitador();
            StringBuilder stbSQL = new StringBuilder();
            DataSet dstRetorno;

            try
            {
                // Montando query a ser executada
                stbSQL.Append(clsFacil.MontarQuery("SELECT "));
                stbSQL.Append(clsFacil.MontarQuery("  ano_referencia, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_serie, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_inicial_faixa, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_final_faixa, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_cnpj_base_emi, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_cnpj_filial_emi, "));
                stbSQL.Append(clsFacil.MontarQuery("  dig_cnpj_emi, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_protocolo, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_sequencial_unico, "));
                stbSQL.Append(clsFacil.MontarQuery("  convert(varchar(max),xml_documento) as xml_documento, "));
                stbSQL.Append(clsFacil.MontarQuery("  dtc_documento, "));
                stbSQL.Append(clsFacil.MontarQuery("  dtc_insercao, "));
                stbSQL.Append(clsFacil.MontarQuery("  dtc_atualizacao "));
                stbSQL.Append(clsFacil.MontarQuery("FROM "));
                stbSQL.Append(clsFacil.MontarQuery("  cte.documento_conhecimento_transporte_eletronico_inutilizacao WITH (READPAST) "));
                stbSQL.Append(clsFacil.MontarQuery("WHERE "));
                stbSQL.Append(clsFacil.MontarQuery("  ano_referencia = @ano_referencia "));
                if (strSerie != string.Empty)
                {
                    stbSQL.Append(clsFacil.MontarQuery("  AND num_serie = @num_serie "));
                }
                if (strFaixaInicial != string.Empty)
                {
                    stbSQL.Append(clsFacil.MontarQuery("  AND num_inicial_faixa = @num_inicial_faixa "));
                }
                if (strFaixaFinal != string.Empty)
                {
                    stbSQL.Append(clsFacil.MontarQuery("  AND num_final_faixa = @num_final_faixa "));
                }
                if (strCNPJCPFBase != string.Empty)
                {
                    stbSQL.Append(clsFacil.MontarQuery("  AND num_cnpj_base_emi = @num_cnpj_base_emi "));
                }
                if (strCNPJCPFFilial != string.Empty)
                {
                    stbSQL.Append(clsFacil.MontarQuery("  AND num_cnpj_filial_emi = @num_cnpj_filial_emi "));
                }
                if (strCNPJCPFDigito != string.Empty)
                {
                    stbSQL.Append(clsFacil.MontarQuery("  AND dig_cnpj_emi = @dig_cnpj_emi "));
                }
                string strSQL = stbSQL.ToString();

                // Informando parametros
                clsDados.LimparParametro();
                clsFacil.AdicionarParametro(ref strSQL, strAno, "@ano_referencia", SqlDbType.SmallInt);
                clsFacil.AdicionarParametro(ref strSQL, strSerie, "@num_serie", SqlDbType.SmallInt);
                clsFacil.AdicionarParametro(ref strSQL, strFaixaInicial, "@num_inicial_faixa", SqlDbType.Int);
                clsFacil.AdicionarParametro(ref strSQL, strFaixaFinal, "@num_final_faixa", SqlDbType.Int);
                clsFacil.AdicionarParametro(ref strSQL, strCNPJCPFBase, "@num_cnpj_base_emi", SqlDbType.Int);
                clsFacil.AdicionarParametro(ref strSQL, strCNPJCPFFilial, "@num_cnpj_filial_emi", SqlDbType.SmallInt);
                clsFacil.AdicionarParametro(ref strSQL, strCNPJCPFDigito, "@dig_cnpj_emi", SqlDbType.TinyInt);

                // Executando query
                dstRetorno = clsDados.ExecutarDataset(strSQL, this.Conexao);
            }
            catch
            {
                throw;
            }
            finally
            {
                stbSQL = null;
                clsDados = null;
                clsFacil = null;
            }

            return dstRetorno;
        }

        #endregion
              
        #region " AtualizarTempAnalisadorErro "

        public int AtualizarTempAnalisadorErro(string strNSU, string strMsgErro)
        {
            // Classes e variaveis utilizadas
            AcessoDados clsDados = new AcessoDados();
            Facilitador clsFacil = new Facilitador();
            StringBuilder stbSQL = new StringBuilder();
            int intRetorno = 0;

            try
            {
                // Montando query a ser executada
                stbSQL.Append(clsFacil.MontarQuery("UPDATE "));
                stbSQL.Append(clsFacil.MontarQuery("  cte.tmp_analise_conhecimento_transporte_eletronico "));
                stbSQL.Append(clsFacil.MontarQuery("SET "));
                stbSQL.Append(clsFacil.MontarQuery("  des_mensagem_erro = @des_mensagem_erro, "));
                stbSQL.Append(clsFacil.MontarQuery("  dtc_atualizacao = getdate() "));
                stbSQL.Append(clsFacil.MontarQuery("WHERE "));
                stbSQL.Append(clsFacil.MontarQuery("  num_sequencial_unico = @num_sequencial_unico "));
                string strSQL = stbSQL.ToString();

                // Montando parametros
                clsDados.LimparParametro();
                clsFacil.AdicionarParametro(ref strSQL, strNSU, "@num_sequencial_unico", SqlDbType.BigInt);
                clsFacil.AdicionarParametro(ref strSQL, strMsgErro, "@des_mensagem_erro", SqlDbType.VarChar);

                // Executando query
                clsDados.BeginTransaction(this.Conexao);
                intRetorno = clsDados.ExecutarQuery(strSQL, this.Conexao);
                clsDados.CommitTransaction();
            }
            catch
            {
                clsDados.RollbackTransaction();
                throw;
            }
            finally
            {
                stbSQL = null;
                clsDados = null;
                clsFacil = null;
            }

            return intRetorno;
        }

        #endregion

        #region " ExcluirTempAnalisador "

        public int ExcluirTempAnalisador(string strNSU)
        {
            // Classes e variaveis utilizadas
            AcessoDados clsDados = new AcessoDados();
            Facilitador clsFacil = new Facilitador();
            StringBuilder stbSQL = new StringBuilder();
            int intRetorno = 0;

            try
            {
                // Montando query a ser executada
                //stbSQL.Append(clsFacil.MontarQuery("DELETE TOP(1) "));
                stbSQL.Append(clsFacil.MontarQuery("DELETE  "));
                stbSQL.Append(clsFacil.MontarQuery("FROM "));
                stbSQL.Append(clsFacil.MontarQuery("  cte.tmp_analise_conhecimento_transporte_eletronico "));
                stbSQL.Append(clsFacil.MontarQuery("WHERE "));
                stbSQL.Append(clsFacil.MontarQuery("  num_sequencial_unico = @num_sequencial_unico "));
                string strSQL = stbSQL.ToString();

                // Informando parametros
                clsDados.LimparParametro();
                clsFacil.AdicionarParametro(ref strSQL, strNSU, "@num_sequencial_unico", SqlDbType.BigInt);

                // Executando query
                clsDados.BeginTransaction(this.Conexao);
                intRetorno = clsDados.ExecutarQuery(strSQL, this.Conexao);
                clsDados.CommitTransaction();
            }
            catch
            {
                clsDados.RollbackTransaction();
                throw;
            }
            finally
            {
                stbSQL = null;
                clsDados = null;
                clsFacil = null;
            }

            return intRetorno;
        }

        #endregion

        #region " ObterTempAnalisador "

        public DataSet ObterTempAnalisador(string strNSU)
        {
            // Classes e variaveis utilizadas
            AcessoDados clsDados = new AcessoDados();
            Facilitador clsFacil = new Facilitador();
            StringBuilder stbSQL = new StringBuilder();
            DataSet dstRetorno;

            try
            {
                // Montando query a ser executada
                stbSQL.Append(clsFacil.MontarQuery("SELECT TOP(1) "));
                stbSQL.Append(clsFacil.MontarQuery("  num_sequencial_unico, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_protocolo, "));
                stbSQL.Append(clsFacil.MontarQuery("  convert(varchar(max), xml_documento) as xml_documento, "));
                stbSQL.Append(clsFacil.MontarQuery("  des_esquema, "));
                stbSQL.Append(clsFacil.MontarQuery("  dtc_documento, "));
                stbSQL.Append(clsFacil.MontarQuery("  dtc_atualizacao "));
                stbSQL.Append(clsFacil.MontarQuery("FROM "));
                stbSQL.Append(clsFacil.MontarQuery("  cte.tmp_analise_conhecimento_transporte_eletronico WITH (READPAST) "));
                stbSQL.Append(clsFacil.MontarQuery("WHERE "));
                stbSQL.Append(clsFacil.MontarQuery("  num_sequencial_unico = @num_sequencial_unico "));
                string strSQL = stbSQL.ToString();

                // Informando parametros
                clsDados.LimparParametro();
                clsFacil.AdicionarParametro(ref strSQL, strNSU, "@num_sequencial_unico", SqlDbType.BigInt);

                // Executando query
                dstRetorno = clsDados.ExecutarDataset(strSQL, this.Conexao);
            }
            catch
            {
                throw;
            }
            finally
            {
                stbSQL = null;
                clsDados = null;
                clsFacil = null;
            }

            return dstRetorno;
        }

        #endregion

        #region " ObterTempAnalisadorTop "

        public DataSet ObterTempAnalisadorTop(int intTop)
        {
            // Classes e variaveis utilizadas
            AcessoDados clsDados = new AcessoDados();
            Facilitador clsFacil = new Facilitador();
            StringBuilder stbSQL = new StringBuilder();
            DataSet dstRetorno;

            try
            {
                // Montando query a ser executada
                stbSQL.Append(clsFacil.MontarQuery("SELECT TOP(" + intTop.ToString() + ") "));
                stbSQL.Append(clsFacil.MontarQuery("  num_sequencial_unico "));
                stbSQL.Append(clsFacil.MontarQuery("FROM "));
                stbSQL.Append(clsFacil.MontarQuery("  cte.tmp_analise_conhecimento_transporte_eletronico WITH (READPAST) "));
                string strSQL = stbSQL.ToString();

                // Informando parametros
                clsDados.LimparParametro();

                // Executando query
                dstRetorno = clsDados.ExecutarDataset(strSQL, this.Conexao);
            }
            catch
            {
                throw;
            }
            finally
            {
                stbSQL = null;
                clsDados = null;
                clsFacil = null;
            }

            return dstRetorno;
        }

        #endregion

        #region " EnviarFilaAnalisador "

        public int EnviarFilaAnalisador(string strChave)
        {
            // Classes e variaveis utilizadas
            AcessoDados clsDados = new AcessoDados();
            Facilitador clsFacil = new Facilitador();
            StringBuilder stbSQL = new StringBuilder();
            int intRetorno = 0;

            try
            {
                // Montando query a ser executada
                stbSQL.Append(clsFacil.MontarQuery("DECLARE @IdDialogo UNIQUEIDENTIFIER; "));
                stbSQL.Append(clsFacil.MontarQuery("BEGIN DIALOG @IdDialogo "));
                stbSQL.Append(clsFacil.MontarQuery("FROM SERVICE servico_iniciador_cte_analisador "));
                stbSQL.Append(clsFacil.MontarQuery("TO SERVICE N'servico_alvo_cte_analisador' "));
                stbSQL.Append(clsFacil.MontarQuery("ON CONTRACT  contrato_cte_analisador "));
                stbSQL.Append(clsFacil.MontarQuery("WITH ENCRYPTION = OFF; "));
                stbSQL.Append(clsFacil.MontarQuery("SEND ON CONVERSATION @IdDialogo "));
                stbSQL.Append(clsFacil.MontarQuery("MESSAGE TYPE tipo_mensagem_cte_analisador (@chave); "));
                string strSQL = stbSQL.ToString();

                // Montando parametros
                clsDados.LimparParametro();
                clsFacil.AdicionarParametro(ref strSQL, strChave, "@chave", SqlDbType.VarChar);

                // Executando query
                clsDados.BeginTransaction(this.Conexao);
                intRetorno = clsDados.ExecutarQuery(strSQL, this.Conexao);
                clsDados.CommitTransaction();
            }
            catch
            {
                clsDados.RollbackTransaction();
                throw;
            }
            finally
            {
                stbSQL = null;
                clsDados = null;
                clsFacil = null;
            }

            return intRetorno;
        }

        #endregion

        #region " RetirarFilaAnalisador "

        public string RetirarFilaAnalisador()
        {
            // Classes e variaveis utilizadas
            AcessoDados clsDados = new AcessoDados();
            Facilitador clsFacil = new Facilitador();
            StringBuilder stbSQL = new StringBuilder();
            string strRetorno = string.Empty;

            try
            {
                // Informando query a ser executada
                stbSQL.Append(clsFacil.MontarQuery("DECLARE @IdDialogo UNIQUEIDENTIFIER; "));
                stbSQL.Append(clsFacil.MontarQuery("DECLARE @chave varchar(max); "));
                stbSQL.Append(clsFacil.MontarQuery("DECLARE @data datetime; "));
                stbSQL.Append(clsFacil.MontarQuery("RECEIVE TOP(1) "));
                stbSQL.Append(clsFacil.MontarQuery("  @chave = CONVERT(VARCHAR(MAX),message_body), "));
                stbSQL.Append(clsFacil.MontarQuery("  @IdDialogo = conversation_handle, "));
                stbSQL.Append(clsFacil.MontarQuery("  @data = getdate() "));
                stbSQL.Append(clsFacil.MontarQuery("FROM "));
                stbSQL.Append(clsFacil.MontarQuery("  fila_alvo_cte_analisador; "));
                stbSQL.Append(clsFacil.MontarQuery("IF @IdDialogo IS NOT NULL "));
                stbSQL.Append(clsFacil.MontarQuery("BEGIN "));
                stbSQL.Append(clsFacil.MontarQuery("  END CONVERSATION @IdDialogo; "));
                stbSQL.Append(clsFacil.MontarQuery("END "));
                stbSQL.Append(clsFacil.MontarQuery("SELECT "));
                stbSQL.Append(clsFacil.MontarQuery("  @chave AS chave, "));
                stbSQL.Append(clsFacil.MontarQuery("  @IdDialogo as IdDialogo, "));
                stbSQL.Append(clsFacil.MontarQuery("  @data as data "));
                string strSQL = stbSQL.ToString();

                // Informando parametros
                clsDados.LimparParametro();

                // Executando query
                DataSet dstRetorno = clsDados.ExecutarDataset(strSQL, this.Conexao);
                if ((dstRetorno != null) && (dstRetorno.Tables.Count > 0) && (dstRetorno.Tables[0].Rows.Count > 0))
                {
                    strRetorno = dstRetorno.Tables[0].Rows[0]["chave"].ToString();
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                stbSQL = null;
                clsDados = null;
                clsFacil = null;
            }

            return strRetorno;
        }

        #endregion

        #region " ObterQtdeFilaAnalisador "

        public long ObterQtdeFilaAnalisador()
        {
            // Classes e variaveis utilizadas
            AcessoDados clsDados = new AcessoDados();
            Facilitador clsFacil = new Facilitador();
            StringBuilder stbSQL = new StringBuilder();
            long intRetorno = 0;

            try
            {
                // Montando query a ser executada
                stbSQL.Append(clsFacil.MontarQuery("SELECT "));
                stbSQL.Append(clsFacil.MontarQuery("  COUNT(1) as total "));
                stbSQL.Append(clsFacil.MontarQuery("FROM "));
                stbSQL.Append(clsFacil.MontarQuery("  fila_alvo_cte_analisador WITH (READPAST) "));
                string strSQL = stbSQL.ToString();

                // Informando parametros
                clsDados.LimparParametro();

                // Executando query
                DataSet dstRetorno = clsDados.ExecutarDataset(strSQL, this.Conexao);
                intRetorno = Convert.ToInt64(dstRetorno.Tables[0].Rows[0]["total"]);
            }
            catch
            {
                throw;
            }
            finally
            {
                stbSQL = null;
                clsDados = null;
                clsFacil = null;
            }

            return intRetorno;
        }

        #endregion



       


    }
}