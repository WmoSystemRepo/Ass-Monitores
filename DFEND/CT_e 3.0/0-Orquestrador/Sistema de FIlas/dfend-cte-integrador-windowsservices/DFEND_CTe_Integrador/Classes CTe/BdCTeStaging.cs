using System;
using System.Data;
using System.Text;

namespace DFe
{
    class BdCTeStaging
    {
        #region " Variaveis "

        // Classes utilizadas
        private Facilitador clsFacil;

        #endregion

        #region " Propriedades "

        public string Conexao { get; set; }

        #endregion

        #region " Construtores "

        public BdCTeStaging(Facilitador clsFacilPar, string strConexao)
        {
            // Inicializando classes
            clsFacil = clsFacilPar;

            // Inicializando propriedades
            this.Conexao = strConexao;
        }

        #endregion

        #region " ObterSemaforoCTe "

        public short ObterSemaforoCTe()
        {
            // Classes e variaveis utilizadas
            AcessoDados clsDados = new AcessoDados();
            StringBuilder stbSQL = new StringBuilder();
            short intRetorno = 0;

            try
            {
                // Montando query a ser executada
                stbSQL.Append(clsFacil.MontarQuery("SELECT TOP(1) "));
                stbSQL.Append(clsFacil.MontarQuery("  num_controle_execucao_carga as semaforo "));
                stbSQL.Append(clsFacil.MontarQuery("FROM "));
                stbSQL.Append(clsFacil.MontarQuery("  cte.controle_execucao_carga_conhecimento_transporte_eletronico WITH (READPAST) "));
                string strSQL = stbSQL.ToString();

                // Informando parametros
                clsDados.LimparParametro();

                // Executando query
                DataSet dstRetorno = clsDados.ExecutarDataset(strSQL, this.Conexao);
                if ((dstRetorno != null) && (dstRetorno.Tables.Count > 0) && (dstRetorno.Tables[0].Rows.Count > 0))
                {
                    intRetorno = Convert.ToInt16(dstRetorno.Tables[0].Rows[0]["semaforo"].ToString());
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
            }

            return intRetorno;
        }

        #endregion

        #region " TrocarSemaforoCTe "

        public int TrocarSemaforoCTe()
        {
            // Classes e variaveis utilizadas
            AcessoDados clsDados = new AcessoDados();
            StringBuilder stbSQL = new StringBuilder();
            int intRetorno = 0;

            try
            {
                // Montando query a ser executada
                stbSQL.Append(clsFacil.MontarQuery("DECLARE @semaforo AS tinyint "));
                stbSQL.Append(clsFacil.MontarQuery("SELECT @semaforo = num_controle_execucao_carga FROM cte.controle_execucao_carga_conhecimento_transporte_eletronico "));
                stbSQL.Append(clsFacil.MontarQuery("IF @semaforo = 1 "));
                stbSQL.Append(clsFacil.MontarQuery("BEGIN "));
                stbSQL.Append(clsFacil.MontarQuery("  EXEC up_truncar_tab_temporaria_bd 'cte.tmp_conhecimento_transporte_eletronico_segunda' "));
                stbSQL.Append(clsFacil.MontarQuery("  UPDATE cte.controle_execucao_carga_conhecimento_transporte_eletronico "));
                stbSQL.Append(clsFacil.MontarQuery("  SET num_controle_execucao_carga = 2, dtc_atualizacao = GETDATE() "));
                stbSQL.Append(clsFacil.MontarQuery("END "));
                stbSQL.Append(clsFacil.MontarQuery("ELSE IF @semaforo = 2 "));
                stbSQL.Append(clsFacil.MontarQuery("BEGIN "));
                stbSQL.Append(clsFacil.MontarQuery("  EXEC up_truncar_tab_temporaria_bd 'cte.tmp_conhecimento_transporte_eletronico_primeira' "));
                stbSQL.Append(clsFacil.MontarQuery("  UPDATE cte.controle_execucao_carga_conhecimento_transporte_eletronico "));
                stbSQL.Append(clsFacil.MontarQuery("  SET num_controle_execucao_carga = 1, dtc_atualizacao = GETDATE() "));
                stbSQL.Append(clsFacil.MontarQuery("END "));
                string strSQL = stbSQL.ToString();

                // Montando parametros
                clsDados.LimparParametro();

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
            }

            return intRetorno;
        }

        #endregion

        #region " InserirDFe "

        public int InserirDFe(DocCTe clsDocCTe, short intSemaforo)
        {
            // Classes e variaveis utilizadas
            AcessoDados clsDados = new AcessoDados();
            StringBuilder stbSQL = new StringBuilder();
            int intRetorno = 0;

            try
            {
                // Definindo a tabela de acordo com o semaforo
                string strTabela = "cte.tmp_conhecimento_transporte_eletronico_primeira";
                if (intSemaforo == 2)
                {
                    strTabela = "cte.tmp_conhecimento_transporte_eletronico_segunda";
                }

                // Montando query a ser executada
                stbSQL.Append(clsFacil.MontarQuery("INSERT INTO " + strTabela + " "));
                stbSQL.Append(clsFacil.MontarQuery("( "));
                stbSQL.Append(clsFacil.MontarQuery("  dtr_referencia, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_protocolo, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_modelo, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_serie, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_documento_fiscal, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_fiscal_operacao_prestacao, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_tipo_forma_emissao, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_tipo_forma_pagamento, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_tipo_tomador_conhecimento_transporte_eletronico, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_tipo_documento_conhecimento_transporte_eletronico, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_tipo_servico_conhecimento_transporte_eletronico, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_tipo_modal_conhecimento_transporte_eletronico, "));
                stbSQL.Append(clsFacil.MontarQuery("  sig_unid_federacao_emitente, "));
                stbSQL.Append(clsFacil.MontarQuery("  des_municipio_emitente, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_cnpj_base_emitente_dfena, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_cnpj_filial_emitente_dfena, "));
                stbSQL.Append(clsFacil.MontarQuery("  dig_cnpj_emitente, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_insc_estad_emitente, "));
                stbSQL.Append(clsFacil.MontarQuery("  nom_razao_social_emitente, "));
                stbSQL.Append(clsFacil.MontarQuery("  sig_unid_federacao_remetente, "));
                stbSQL.Append(clsFacil.MontarQuery("  des_municipio_remetente, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_cnpj_base_remetente_dfena, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_cnpj_filial_remetente_dfena, "));
                stbSQL.Append(clsFacil.MontarQuery("  dig_cnpj_remetente, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_insc_estad_remetente, "));
                stbSQL.Append(clsFacil.MontarQuery("  nom_razao_social_remetente, "));
                stbSQL.Append(clsFacil.MontarQuery("  sig_unid_federacao_destinatario, "));
                stbSQL.Append(clsFacil.MontarQuery("  des_municipio_destinatario, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_cnpj_base_destinatario_dfena, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_cnpj_filial_destinatario_dfena, "));
                stbSQL.Append(clsFacil.MontarQuery("  dig_cnpj_destinatario, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_insc_estad_destinatario, "));
                stbSQL.Append(clsFacil.MontarQuery("  nom_razao_social_destinatario, "));
                stbSQL.Append(clsFacil.MontarQuery("  sig_unid_federacao_expedidor, "));
                stbSQL.Append(clsFacil.MontarQuery("  des_municipio_expedidor, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_cnpj_base_expedidor_dfena, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_cnpj_filial_expedidor_dfena, "));
                stbSQL.Append(clsFacil.MontarQuery("  dig_cnpj_expedidor, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_insc_estad_expedidor, "));
                stbSQL.Append(clsFacil.MontarQuery("  nom_razao_social_expedidor, "));
                stbSQL.Append(clsFacil.MontarQuery("  sig_unid_federacao_recebedor, "));
                stbSQL.Append(clsFacil.MontarQuery("  des_municipio_recebedor, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_cnpj_base_recebedor_dfena, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_cnpj_filial_recebedor_dfena, "));
                stbSQL.Append(clsFacil.MontarQuery("  dig_cnpj_recebedor, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_insc_estad_recebedor, "));
                stbSQL.Append(clsFacil.MontarQuery("  nom_razao_social_recebedor, "));
                stbSQL.Append(clsFacil.MontarQuery("  sig_unid_federacao_inicio_prestacao, "));
                stbSQL.Append(clsFacil.MontarQuery("  des_municipio_inicio_prestacao, "));
                stbSQL.Append(clsFacil.MontarQuery("  sig_unid_federacao_fim_prestacao, "));
                stbSQL.Append(clsFacil.MontarQuery("  des_municipio_fim_prestacao, "));
                stbSQL.Append(clsFacil.MontarQuery("  qtd_documento_nota_fiscal_eletronica_vinculada, "));
                stbSQL.Append(clsFacil.MontarQuery("  val_total_prestacao_servico, "));
                stbSQL.Append(clsFacil.MontarQuery("  des_icms, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_classificacao_sistema_tributario, "));
                stbSQL.Append(clsFacil.MontarQuery("  val_reducao_base_calculo, "));
                stbSQL.Append(clsFacil.MontarQuery("  val_base_calculo, "));
                stbSQL.Append(clsFacil.MontarQuery("  val_aliquota, "));
                stbSQL.Append(clsFacil.MontarQuery("  val_icms, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_situacao_documento_fiscal_eletronico, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_versao_xml, "));
                stbSQL.Append(clsFacil.MontarQuery("  xml_pedido, "));
                stbSQL.Append(clsFacil.MontarQuery("  xml_resposta, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_sequencial_unico, "));
                stbSQL.Append(clsFacil.MontarQuery("  dtc_emissao, "));
                stbSQL.Append(clsFacil.MontarQuery("  dtc_autorizacao_uso, "));
                stbSQL.Append(clsFacil.MontarQuery("  dtc_historico, "));
                stbSQL.Append(clsFacil.MontarQuery("  des_endereco_logico, "));
                stbSQL.Append(clsFacil.MontarQuery("  dtc_atualizacao "));
                stbSQL.Append(clsFacil.MontarQuery(") "));
                stbSQL.Append(clsFacil.MontarQuery("VALUES "));
                stbSQL.Append(clsFacil.MontarQuery("( "));
                stbSQL.Append(clsFacil.MontarQuery("  @dtr_referencia, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_protocolo, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_modelo, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_serie, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_documento_fiscal, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_fiscal_operacao_prestacao, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_tipo_forma_emissao, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_tipo_forma_pagamento, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_tipo_tomador_conhecimento_transporte_eletronico, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_tipo_documento_conhecimento_transporte_eletronico, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_tipo_servico_conhecimento_transporte_eletronico, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_tipo_modal_conhecimento_transporte_eletronico, "));
                stbSQL.Append(clsFacil.MontarQuery("  @sig_unid_federacao_emitente, "));
                stbSQL.Append(clsFacil.MontarQuery("  @des_municipio_emitente, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_cnpj_base_emitente, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_cnpj_filial_emitente, "));
                stbSQL.Append(clsFacil.MontarQuery("  @dig_cnpj_emitente, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_insc_estad_emitente, "));
                stbSQL.Append(clsFacil.MontarQuery("  @nom_razao_social_emitente, "));
                stbSQL.Append(clsFacil.MontarQuery("  @sig_unid_federacao_remetente, "));
                stbSQL.Append(clsFacil.MontarQuery("  @des_municipio_remetente, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_cnpj_base_remetente, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_cnpj_filial_remetente, "));
                stbSQL.Append(clsFacil.MontarQuery("  @dig_cnpj_remetente, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_insc_estad_remetente, "));
                stbSQL.Append(clsFacil.MontarQuery("  @nom_razao_social_remetente, "));
                stbSQL.Append(clsFacil.MontarQuery("  @sig_unid_federacao_destinatario, "));
                stbSQL.Append(clsFacil.MontarQuery("  @des_municipio_destinatario, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_cnpj_base_destinatario, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_cnpj_filial_destinatario, "));
                stbSQL.Append(clsFacil.MontarQuery("  @dig_cnpj_destinatario, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_insc_estad_destinatario, "));
                stbSQL.Append(clsFacil.MontarQuery("  @nom_razao_social_destinatario, "));
                stbSQL.Append(clsFacil.MontarQuery("  @sig_unid_federacao_expedidor, "));
                stbSQL.Append(clsFacil.MontarQuery("  @des_municipio_expedidor, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_cnpj_base_expedidor, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_cnpj_filial_expedidor, "));
                stbSQL.Append(clsFacil.MontarQuery("  @dig_cnpj_expedidor, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_insc_estad_expedidor, "));
                stbSQL.Append(clsFacil.MontarQuery("  @nom_razao_social_expedidor, "));
                stbSQL.Append(clsFacil.MontarQuery("  @sig_unid_federacao_recebedor, "));
                stbSQL.Append(clsFacil.MontarQuery("  @des_municipio_recebedor, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_cnpj_base_recebedor, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_cnpj_filial_recebedor, "));
                stbSQL.Append(clsFacil.MontarQuery("  @dig_cnpj_recebedor, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_insc_estad_recebedor, "));
                stbSQL.Append(clsFacil.MontarQuery("  @nom_razao_social_recebedor, "));
                stbSQL.Append(clsFacil.MontarQuery("  @sig_unid_federacao_inicio_prestacao, "));
                stbSQL.Append(clsFacil.MontarQuery("  @des_municipio_inicio_prestacao, "));
                stbSQL.Append(clsFacil.MontarQuery("  @sig_unid_federacao_fim_prestacao, "));
                stbSQL.Append(clsFacil.MontarQuery("  @des_municipio_fim_prestacao, "));
                stbSQL.Append(clsFacil.MontarQuery("  @qtd_documento_nota_fiscal_eletronica_vinculada, "));
                stbSQL.Append(clsFacil.MontarQuery("  @val_total_prestacao_servico, "));
                stbSQL.Append(clsFacil.MontarQuery("  @des_icms, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_classificacao_sistema_tributario, "));
                stbSQL.Append(clsFacil.MontarQuery("  @val_reducao_base_calculo, "));
                stbSQL.Append(clsFacil.MontarQuery("  @val_base_calculo, "));
                stbSQL.Append(clsFacil.MontarQuery("  @val_aliquota, "));
                stbSQL.Append(clsFacil.MontarQuery("  @val_icms, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_situacao_documento_fiscal_eletronico, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_versao_xml, "));
                stbSQL.Append(clsFacil.MontarQuery("  @xml_pedido, "));
                stbSQL.Append(clsFacil.MontarQuery("  @xml_resposta, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_sequencial_unico, "));
                stbSQL.Append(clsFacil.MontarQuery("  @dtc_emissao, "));
                stbSQL.Append(clsFacil.MontarQuery("  @dtc_autorizacao_uso, "));
                stbSQL.Append(clsFacil.MontarQuery("  @dtc_historico, "));
                stbSQL.Append(clsFacil.MontarQuery("  @des_endereco_logico, "));
                stbSQL.Append(clsFacil.MontarQuery("  getdate() "));
                stbSQL.Append(clsFacil.MontarQuery(") "));
                string strSQL = stbSQL.ToString();

                // Montando parametros
                clsDados.LimparParametro();
                clsFacil.AdicionarParametro(ref strSQL, clsDocCTe.DataReferencia, "@dtr_referencia", SqlDbType.Int);
                clsFacil.AdicionarParametro(ref strSQL, clsDocCTe.ChaveAcesso, "@cod_chave_acesso", SqlDbType.Char);
                clsFacil.AdicionarParametro(ref strSQL, clsDocCTe.Protocolo, "@num_protocolo", SqlDbType.BigInt);
                clsFacil.AdicionarParametro(ref strSQL, clsDocCTe.Modelo, "@cod_modelo", SqlDbType.TinyInt);
                clsFacil.AdicionarParametro(ref strSQL, clsDocCTe.Serie, "@num_serie", SqlDbType.SmallInt);
                clsFacil.AdicionarParametro(ref strSQL, clsDocCTe.Numero, "@num_documento_fiscal", SqlDbType.Int);
                clsFacil.AdicionarParametro(ref strSQL, clsDocCTe.CFOP, "@cod_fiscal_operacao_prestacao", SqlDbType.Int);
                clsFacil.AdicionarParametro(ref strSQL, clsDocCTe.TipoEmissao, "@cod_tipo_forma_emissao", SqlDbType.TinyInt);
                clsFacil.AdicionarParametro(ref strSQL, clsDocCTe.TipoFormaPagto, "@cod_tipo_forma_pagamento", SqlDbType.TinyInt);
                clsFacil.AdicionarParametro(ref strSQL, clsDocCTe.TomaCod, "@cod_tipo_tomador_conhecimento_transporte_eletronico", SqlDbType.TinyInt);
                clsFacil.AdicionarParametro(ref strSQL, clsDocCTe.TipoCTe, "@cod_tipo_documento_conhecimento_transporte_eletronico", SqlDbType.TinyInt);
                clsFacil.AdicionarParametro(ref strSQL, clsDocCTe.TipoServico, "@cod_tipo_servico_conhecimento_transporte_eletronico", SqlDbType.TinyInt);
                clsFacil.AdicionarParametro(ref strSQL, clsDocCTe.Modal, "@cod_tipo_modal_conhecimento_transporte_eletronico", SqlDbType.TinyInt);
                clsFacil.AdicionarParametro(ref strSQL, clsDocCTe.EmitEndUF, "@sig_unid_federacao_emitente", SqlDbType.Char);
                clsFacil.AdicionarParametro(ref strSQL, clsDocCTe.EmitEndMunicipio, "@des_municipio_emitente", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, clsDocCTe.EmitCNPJCPFBase, "@num_cnpj_base_emitente", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, clsDocCTe.EmitCNPJCPFFilial, "@num_cnpj_filial_emitente", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, clsDocCTe.EmitCNPJCPFDigito, "@dig_cnpj_emitente", SqlDbType.TinyInt);
                clsFacil.AdicionarParametro(ref strSQL, clsDocCTe.EmitIE, "@num_insc_estad_emitente", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, clsDocCTe.EmitNome, "@nom_razao_social_emitente", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, clsDocCTe.RemeEndUF, "@sig_unid_federacao_remetente", SqlDbType.Char);
                clsFacil.AdicionarParametro(ref strSQL, clsDocCTe.RemeEndMunicipio, "@des_municipio_remetente", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, clsDocCTe.RemeCNPJCPFBase, "@num_cnpj_base_remetente", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, clsDocCTe.RemeCNPJCPFFilial, "@num_cnpj_filial_remetente", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, clsDocCTe.RemeCNPJCPFDigito, "@dig_cnpj_remetente", SqlDbType.TinyInt);
                clsFacil.AdicionarParametro(ref strSQL, clsDocCTe.RemeIE, "@num_insc_estad_remetente", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, clsDocCTe.RemeNome, "@nom_razao_social_remetente", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, clsDocCTe.DestEndUF, "@sig_unid_federacao_destinatario", SqlDbType.Char);
                clsFacil.AdicionarParametro(ref strSQL, clsDocCTe.DestEndMunicipio, "@des_municipio_destinatario", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, clsDocCTe.DestCNPJCPFBase, "@num_cnpj_base_destinatario", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, clsDocCTe.DestCNPJCPFFilial, "@num_cnpj_filial_destinatario", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, clsDocCTe.DestCNPJCPFDigito, "@dig_cnpj_destinatario", SqlDbType.TinyInt);
                clsFacil.AdicionarParametro(ref strSQL, clsDocCTe.DestIE, "@num_insc_estad_destinatario", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, clsDocCTe.DestNome, "@nom_razao_social_destinatario", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, clsDocCTe.ExpeEndUF, "@sig_unid_federacao_expedidor", SqlDbType.Char);
                clsFacil.AdicionarParametro(ref strSQL, clsDocCTe.ExpeEndMunicipio, "@des_municipio_expedidor", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, clsDocCTe.ExpeCNPJCPFBase, "@num_cnpj_base_expedidor", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, clsDocCTe.ExpeCNPJCPFFilial, "@num_cnpj_filial_expedidor", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, clsDocCTe.ExpeCNPJCPFDigito, "@dig_cnpj_expedidor", SqlDbType.TinyInt);
                clsFacil.AdicionarParametro(ref strSQL, clsDocCTe.ExpeIE, "@num_insc_estad_expedidor", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, clsDocCTe.ExpeNome, "@nom_razao_social_expedidor", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, clsDocCTe.ReceEndUF, "@sig_unid_federacao_recebedor", SqlDbType.Char);
                clsFacil.AdicionarParametro(ref strSQL, clsDocCTe.ReceEndMunicipio, "@des_municipio_recebedor", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, clsDocCTe.ReceCNPJCPFBase, "@num_cnpj_base_recebedor", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, clsDocCTe.ReceCNPJCPFFilial, "@num_cnpj_filial_recebedor", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, clsDocCTe.ReceCNPJCPFDigito, "@dig_cnpj_recebedor", SqlDbType.TinyInt);
                clsFacil.AdicionarParametro(ref strSQL, clsDocCTe.ReceIE, "@num_insc_estad_recebedor", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, clsDocCTe.ReceNome, "@nom_razao_social_recebedor", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, clsDocCTe.UFIni, "@sig_unid_federacao_inicio_prestacao", SqlDbType.Char);
                clsFacil.AdicionarParametro(ref strSQL, clsDocCTe.MunicipioIni, "@des_municipio_inicio_prestacao", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, clsDocCTe.UFFim, "@sig_unid_federacao_fim_prestacao", SqlDbType.Char);
                clsFacil.AdicionarParametro(ref strSQL, clsDocCTe.MunicipioFim, "@des_municipio_fim_prestacao", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, clsDocCTe.QtdeNFes, "@qtd_documento_nota_fiscal_eletronica_vinculada", SqlDbType.Int);
                clsFacil.AdicionarParametro(ref strSQL, clsDocCTe.ValorTotal, "@val_total_prestacao_servico", SqlDbType.Decimal);
                clsFacil.AdicionarParametro(ref strSQL, clsDocCTe.ImpostoICMS, "@des_icms", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, clsDocCTe.ImpostoCST, "@num_classificacao_sistema_tributario", SqlDbType.Char);
                clsFacil.AdicionarParametro(ref strSQL, clsDocCTe.ImpostoValorBCPerc, "@val_reducao_base_calculo", SqlDbType.Decimal);
                clsFacil.AdicionarParametro(ref strSQL, clsDocCTe.ImpostoValorBC, "@val_base_calculo", SqlDbType.Decimal);
                clsFacil.AdicionarParametro(ref strSQL, clsDocCTe.ImpostoValorICMSPerc, "@val_aliquota", SqlDbType.Decimal);
                clsFacil.AdicionarParametro(ref strSQL, clsDocCTe.ImpostoValorICMS, "@val_icms", SqlDbType.Decimal);
                clsFacil.AdicionarParametro(ref strSQL, clsDocCTe.Status, "@cod_situacao_documento_fiscal_eletronico", SqlDbType.TinyInt);
                clsFacil.AdicionarParametro(ref strSQL, clsDocCTe.Versao, "@num_versao_xml", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, clsDocCTe.XMLEnvio, "@xml_pedido", SqlDbType.VarBinary);
                clsFacil.AdicionarParametro(ref strSQL, clsDocCTe.XMLRetorno, "@xml_resposta", SqlDbType.VarBinary);
                clsFacil.AdicionarParametro(ref strSQL, clsDocCTe.NSU, "@num_sequencial_unico", SqlDbType.BigInt);
                clsFacil.AdicionarParametro(ref strSQL, clsDocCTe.DataEmissao, "@dtc_emissao", SqlDbType.DateTime);
                clsFacil.AdicionarParametro(ref strSQL, clsDocCTe.DataAutorizacao, "@dtc_autorizacao_uso", SqlDbType.DateTime);
                clsFacil.AdicionarParametro(ref strSQL, clsDocCTe.DataAutorizacao, "@dtc_historico", SqlDbType.DateTime);
                clsFacil.AdicionarParametro(ref strSQL, clsDocCTe.IP, "@des_endereco_logico", SqlDbType.VarChar);

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
            }

            return intRetorno;
        }

        #endregion

        #region " ExcluirDFe "

        public int ExcluirDFe(string strDataReferencia, string strChave, short intSemaforo)
        {
            // Classes e variaveis utilizadas
            AcessoDados clsDados = new AcessoDados();
            StringBuilder stbSQL = new StringBuilder();
            int intRetorno = 0;

            try
            {
                // Definindo a tabela de acordo com o semaforo
                string strTabela = "cte.tmp_conhecimento_transporte_eletronico_primeira";
                if (intSemaforo == 2)
                {
                    strTabela = "cte.tmp_conhecimento_transporte_eletronico_segunda";
                }

                // Montando query a ser executada
                stbSQL.Append(clsFacil.MontarQuery("DELETE TOP(1) "));
                stbSQL.Append(clsFacil.MontarQuery("FROM "));
                stbSQL.Append(clsFacil.MontarQuery("  " + strTabela + " "));
                stbSQL.Append(clsFacil.MontarQuery("WHERE "));
                stbSQL.Append(clsFacil.MontarQuery("  dtr_referencia = @dtr_referencia "));
                stbSQL.Append(clsFacil.MontarQuery("  AND cod_chave_acesso = @cod_chave_acesso "));
                string strSQL = stbSQL.ToString();

                // Montando parametros
                clsDados.LimparParametro();
                clsFacil.AdicionarParametro(ref strSQL, strDataReferencia, "@dtr_referencia", SqlDbType.Int);
                clsFacil.AdicionarParametro(ref strSQL, strChave, "@cod_chave_acesso", SqlDbType.Char);

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
            }

            return intRetorno;
        }

        #endregion
    }
}