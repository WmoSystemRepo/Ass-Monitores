using System;
using System.Data;
using System.Text;

namespace DFe
{
    class BdCTeHistorico
    {
        #region " Variaveis "

        // Classes utilizadas
        private Facilitador clsFacil;

        #endregion

        #region " Propriedades "

        public string Conexao { get; set; }

        #endregion

        #region " Construtores "

        public BdCTeHistorico(Facilitador clsFacilPar, string strConexao)
        {
            // Inicializando classes
            clsFacil = clsFacilPar;

            // Inicializando propriedades
            this.Conexao = strConexao;
        }

        #endregion

        #region " ObterDadosCTeAutorizacaoPorChave "

        public DataSet ObterDadosCTeAutorizacaoPorChave(string strChaveAcesso)
        {
            // Classes e variaveis utilizadas
            AcessoDados clsDados = new AcessoDados();
            StringBuilder stbSQL = new StringBuilder();
            DataSet dstRetorno;

            try
            {
                // Montando query a ser executada
                stbSQL.Append(clsFacil.MontarQuery("exec up_obter_dados_conhecimento_transporte_eletronico_autorizacao_por_chave "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso "));
                string strSQL = stbSQL.ToString();

                // Informando parametros
                clsDados.LimparParametro();
                clsFacil.AdicionarParametro(ref strSQL, strChaveAcesso, "@cod_chave_acesso", SqlDbType.VarChar);

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
            }

            return dstRetorno;
        }

        #endregion

        #region " ObterDadosCTeEventoPorChave "

        public DataSet ObterDadosCTeEventoPorChave(string strChaveAcesso, string strCodigo, string strTipo)
        {
            // Classes e variaveis utilizadas
            AcessoDados clsDados = new AcessoDados();
            StringBuilder stbSQL = new StringBuilder();
            DataSet dstRetorno;

            try
            {
                // Montando query a ser executada
                stbSQL.Append(clsFacil.MontarQuery("exec up_obter_dados_conhecimento_transporte_eletronico_evento_por_chave "));
                stbSQL.Append(clsFacil.MontarQuery("  @seq_evento_nota_fiscal_eletronica, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_tipo_evento_nota_fiscal_eletronica, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso "));
                string strSQL = stbSQL.ToString();

                // Informando parametros
                clsDados.LimparParametro();
                clsFacil.AdicionarParametro(ref strSQL, strCodigo, "@seq_evento_nota_fiscal_eletronica", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, strTipo, "@cod_tipo_evento_nota_fiscal_eletronica", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, strChaveAcesso, "@cod_chave_acesso", SqlDbType.VarChar);

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
            }

            return dstRetorno;
        }

        #endregion

        #region " ObterDadosCTeInutilizacaoPorChave "

        public DataSet ObterDadosCTeInutilizacaoPorChave(string strNumProtocolo)
        {
            // Classes e variaveis utilizadas
            AcessoDados clsDados = new AcessoDados();
            StringBuilder stbSQL = new StringBuilder();
            DataSet dstRetorno;

            try
            {
                // Montando query a ser executada
                stbSQL.Append(clsFacil.MontarQuery("exec up_obter_dados_conhecimento_transporte_eletronico_inutilizacao_por_chave "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_protocolo "));
                string strSQL = stbSQL.ToString();

                // Informando parametros
                clsDados.LimparParametro();
                clsFacil.AdicionarParametro(ref strSQL, strNumProtocolo, "@num_protocolo", SqlDbType.VarChar);

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
            }

            return dstRetorno;
        }

        #endregion

        #region " InserirDadosCTeAutorizacao "

        public int InserirDadosCTeAutorizacao(DocCTe DocCTe, string strCodSituacao)
        {
            // Classes e variaveis utilizadas
            AcessoDados clsDados = new AcessoDados();
            StringBuilder stbSQL = new StringBuilder();
            int intRetorno = 0;

            try
            {
                // Montando query a ser executada
                stbSQL.Append(clsFacil.MontarQuery("exec up_inserir_dados_conhecimento_transporte_eletronico_autorizacao "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_protocolo, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_modelo, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_serie, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_documento, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_cfop, "));
                stbSQL.Append(clsFacil.MontarQuery("  @sig_unid_federacao_emi, "));
                stbSQL.Append(clsFacil.MontarQuery("  @des_municipio_emi, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_cnpj_base_emi, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_cnpj_filial_emi, "));
                stbSQL.Append(clsFacil.MontarQuery("  @dig_cnpj_emi, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_insc_estad_emi, "));
                stbSQL.Append(clsFacil.MontarQuery("  @nom_razao_social_emi, "));
                stbSQL.Append(clsFacil.MontarQuery("  @sig_unid_federacao_rem, "));
                stbSQL.Append(clsFacil.MontarQuery("  @des_municipio_rem, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_cnpj_base_rem, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_cnpj_filial_rem, "));
                stbSQL.Append(clsFacil.MontarQuery("  @dig_cnpj_rem, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_insc_estad_rem, "));
                stbSQL.Append(clsFacil.MontarQuery("  @nom_razao_social_rem, "));
                stbSQL.Append(clsFacil.MontarQuery("  @sig_unid_federacao_dest, "));
                stbSQL.Append(clsFacil.MontarQuery("  @des_municipio_dest, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_cnpj_base_dest, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_cnpj_filial_dest, "));
                stbSQL.Append(clsFacil.MontarQuery("  @dig_cnpj_dest, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_insc_estad_dest, "));
                stbSQL.Append(clsFacil.MontarQuery("  @nom_razao_social_dest, "));
                stbSQL.Append(clsFacil.MontarQuery("  @sig_unid_federacao_exped, "));
                stbSQL.Append(clsFacil.MontarQuery("  @des_municipio_exped, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_cnpj_base_exped, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_cnpj_filial_exped, "));
                stbSQL.Append(clsFacil.MontarQuery("  @dig_cnpj_exped, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_insc_estad_exped, "));
                stbSQL.Append(clsFacil.MontarQuery("  @nom_razao_social_exped, "));
                stbSQL.Append(clsFacil.MontarQuery("  @sig_unid_federacao_receb, "));
                stbSQL.Append(clsFacil.MontarQuery("  @des_municipio_receb, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_cnpj_base_receb, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_cnpj_filial_receb, "));
                stbSQL.Append(clsFacil.MontarQuery("  @dig_cnpj_receb, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_insc_estad_receb, "));
                stbSQL.Append(clsFacil.MontarQuery("  @nom_razao_social_receb, "));
                stbSQL.Append(clsFacil.MontarQuery("  @sig_unid_federacao_inicio_prestacao, "));
                stbSQL.Append(clsFacil.MontarQuery("  @des_municipio_inicio_prestacao, "));
                stbSQL.Append(clsFacil.MontarQuery("  @sig_unid_federacao_fim_prestacao, "));
                stbSQL.Append(clsFacil.MontarQuery("  @des_municipio_fim_prestacao, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_placa, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_rntrc, "));
                stbSQL.Append(clsFacil.MontarQuery("  @qtd_documento, "));
                stbSQL.Append(clsFacil.MontarQuery("  @val_total_prestacao, "));
                stbSQL.Append(clsFacil.MontarQuery("  @des_icms, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_classificacao_sistema_tributario, "));
                stbSQL.Append(clsFacil.MontarQuery("  @val_reducao_base_calculo, "));
                stbSQL.Append(clsFacil.MontarQuery("  @val_base_calculo, "));
                stbSQL.Append(clsFacil.MontarQuery("  @val_aliquota, "));
                stbSQL.Append(clsFacil.MontarQuery("  @val_icms, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_situacao_nota_fisc_eletr, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_tipo_forma_emissao_conhecimento_transporte_eletronico, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_tipo_forma_pagamento_conhecimento_transporte_eletronico, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_tipo_tomador_conhecimento_transporte_eletronico, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_tipo_documento_conhecimento_transporte_eletronico, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_tipo_servico_conhecimento_transporte_eletronico, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_tipo_modal_conhecimento_transporte_eletronico, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_versao_xml, "));
                stbSQL.Append(clsFacil.MontarQuery("  @xml_pedido, "));
                stbSQL.Append(clsFacil.MontarQuery("  @xml_resposta, "));
                stbSQL.Append(clsFacil.MontarQuery("  @seq_unico_ambiente_nacional, "));
                stbSQL.Append(clsFacil.MontarQuery("  @dtc_emissao, "));
                stbSQL.Append(clsFacil.MontarQuery("  @dtc_recepcao, "));
                stbSQL.Append(clsFacil.MontarQuery("  @dtc_autorizacao_uso, "));
                stbSQL.Append(clsFacil.MontarQuery("  @des_endereco_logico "));
                string strSQL = stbSQL.ToString();

                // Montando parametros
                clsDados.LimparParametro();
                clsFacil.AdicionarParametro(ref strSQL, DocCTe.ChaveAcesso, "@cod_chave_acesso", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, DocCTe.Protocolo, "@num_protocolo", SqlDbType.BigInt);
                clsFacil.AdicionarParametro(ref strSQL, DocCTe.Modelo, "@cod_modelo", SqlDbType.TinyInt);
                clsFacil.AdicionarParametro(ref strSQL, DocCTe.Serie, "@num_serie", SqlDbType.SmallInt);
                clsFacil.AdicionarParametro(ref strSQL, DocCTe.Numero, "@num_documento", SqlDbType.Int);
                clsFacil.AdicionarParametro(ref strSQL, DocCTe.CFOP, "@num_cfop", SqlDbType.Int);
                clsFacil.AdicionarParametro(ref strSQL, DocCTe.EmitEndUF, "@sig_unid_federacao_emi", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, DocCTe.EmitEndMunicipio, "@des_municipio_emi", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, DocCTe.EmitCNPJCPFBase, "@num_cnpj_base_emi", SqlDbType.Int);
                clsFacil.AdicionarParametro(ref strSQL, DocCTe.EmitCNPJCPFFilial, "@num_cnpj_filial_emi", SqlDbType.SmallInt);
                clsFacil.AdicionarParametro(ref strSQL, DocCTe.EmitCNPJCPFDigito, "@dig_cnpj_emi", SqlDbType.TinyInt);
                clsFacil.AdicionarParametro(ref strSQL, DocCTe.EmitIE, "@num_insc_estad_emi", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, DocCTe.EmitNome, "@nom_razao_social_emi", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, DocCTe.RemeEndUF, "@sig_unid_federacao_rem", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, DocCTe.RemeEndMunicipio, "@des_municipio_rem", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, DocCTe.RemeCNPJCPFBase, "@num_cnpj_base_rem", SqlDbType.Int);
                clsFacil.AdicionarParametro(ref strSQL, DocCTe.RemeCNPJCPFFilial, "@num_cnpj_filial_rem", SqlDbType.SmallInt);
                clsFacil.AdicionarParametro(ref strSQL, DocCTe.RemeCNPJCPFDigito, "@dig_cnpj_rem", SqlDbType.TinyInt);
                clsFacil.AdicionarParametro(ref strSQL, DocCTe.RemeIE, "@num_insc_estad_rem", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, DocCTe.RemeNome, "@nom_razao_social_rem", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, DocCTe.DestEndUF, "@sig_unid_federacao_dest", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, DocCTe.DestEndMunicipio, "@des_municipio_dest", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, DocCTe.DestCNPJCPFBase, "@num_cnpj_base_dest", SqlDbType.Int);
                clsFacil.AdicionarParametro(ref strSQL, DocCTe.DestCNPJCPFFilial, "@num_cnpj_filial_dest", SqlDbType.SmallInt);
                clsFacil.AdicionarParametro(ref strSQL, DocCTe.DestCNPJCPFDigito, "@dig_cnpj_dest", SqlDbType.TinyInt);
                clsFacil.AdicionarParametro(ref strSQL, DocCTe.DestIE, "@num_insc_estad_dest", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, DocCTe.DestNome, "@nom_razao_social_dest", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, DocCTe.ExpeEndUF, "@sig_unid_federacao_exped", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, DocCTe.ExpeEndMunicipio, "@des_municipio_exped", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, DocCTe.ExpeCNPJCPFBase, "@num_cnpj_base_exped", SqlDbType.Int);
                clsFacil.AdicionarParametro(ref strSQL, DocCTe.ExpeCNPJCPFFilial, "@num_cnpj_filial_exped", SqlDbType.SmallInt);
                clsFacil.AdicionarParametro(ref strSQL, DocCTe.ExpeCNPJCPFDigito, "@dig_cnpj_exped", SqlDbType.TinyInt);
                clsFacil.AdicionarParametro(ref strSQL, DocCTe.ExpeIE, "@num_insc_estad_exped", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, DocCTe.ExpeNome, "@nom_razao_social_exped", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, DocCTe.ReceEndUF, "@sig_unid_federacao_receb", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, DocCTe.ReceEndMunicipio, "@des_municipio_receb", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, DocCTe.ReceCNPJCPFBase, "@num_cnpj_base_receb", SqlDbType.Int);
                clsFacil.AdicionarParametro(ref strSQL, DocCTe.ReceCNPJCPFFilial, "@num_cnpj_filial_receb", SqlDbType.SmallInt);
                clsFacil.AdicionarParametro(ref strSQL, DocCTe.ReceCNPJCPFDigito, "@dig_cnpj_receb", SqlDbType.TinyInt);
                clsFacil.AdicionarParametro(ref strSQL, DocCTe.ReceIE, "@num_insc_estad_receb", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, DocCTe.ReceNome, "@nom_razao_social_receb", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, DocCTe.UFIni, "@sig_unid_federacao_inicio_prestacao", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, DocCTe.MunicipioIni, "@des_municipio_inicio_prestacao", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, DocCTe.UFFim, "@sig_unid_federacao_fim_prestacao", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, DocCTe.MunicipioFim, "@des_municipio_fim_prestacao", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, string.Empty, "@num_placa", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, string.Empty, "@num_rntrc", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, "0", "@qtd_documento", SqlDbType.Int);
                clsFacil.AdicionarParametro(ref strSQL, DocCTe.ValorTotal, "@val_total_prestacao", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, DocCTe.ImpostoICMS, "@des_icms", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, DocCTe.ImpostoCST, "@num_classificacao_sistema_tributario", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, DocCTe.ImpostoValorBCPerc, "@val_reducao_base_calculo", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, DocCTe.ImpostoValorBC, "@val_base_calculo", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, DocCTe.ImpostoValorICMSPerc, "@val_aliquota", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, DocCTe.ImpostoValorICMS, "@val_icms", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, strCodSituacao, "@cod_situacao_nota_fisc_eletr", SqlDbType.TinyInt);
                clsFacil.AdicionarParametro(ref strSQL, DocCTe.TipoEmissao, "@cod_tipo_forma_emissao_conhecimento_transporte_eletronico", SqlDbType.TinyInt);
                clsFacil.AdicionarParametro(ref strSQL, DocCTe.TipoFormaPagto, "@cod_tipo_forma_pagamento_conhecimento_transporte_eletronico", SqlDbType.TinyInt);
                clsFacil.AdicionarParametro(ref strSQL, DocCTe.TomaCod, "@cod_tipo_tomador_conhecimento_transporte_eletronico", SqlDbType.TinyInt);
                clsFacil.AdicionarParametro(ref strSQL, DocCTe.TipoCTe, "@cod_tipo_documento_conhecimento_transporte_eletronico", SqlDbType.TinyInt);
                clsFacil.AdicionarParametro(ref strSQL, DocCTe.TipoServico, "@cod_tipo_servico_conhecimento_transporte_eletronico", SqlDbType.TinyInt);
                clsFacil.AdicionarParametro(ref strSQL, DocCTe.Modal, "@cod_tipo_modal_conhecimento_transporte_eletronico", SqlDbType.TinyInt);
                clsFacil.AdicionarParametro(ref strSQL, DocCTe.Versao, "@num_versao_xml", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, DocCTe.XMLEnvio, "@xml_pedido", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, DocCTe.XMLRetorno, "@xml_resposta", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, DocCTe.NSU, "@seq_unico_ambiente_nacional", SqlDbType.BigInt);
                clsFacil.AdicionarParametro(ref strSQL, DocCTe.DataEmissao, "@dtc_emissao", SqlDbType.SmallDateTime);
                clsFacil.AdicionarParametro(ref strSQL, DocCTe.DataAutorizacao, "@dtc_recepcao", SqlDbType.DateTime);
                clsFacil.AdicionarParametro(ref strSQL, DocCTe.DataAutorizacao, "@dtc_autorizacao_uso", SqlDbType.DateTime);
                clsFacil.AdicionarParametro(ref strSQL, DocCTe.IP, "@des_endereco_logico", SqlDbType.VarChar);

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

        #region " InserirDadosCTeEvento "

        public int InserirDadosCTeEvento(string strChaveAcesso, string strCodigo, string strTipo, string strNumProtocolo, string strCNPJTrans_Base, string strCNPJTrans_Filial, string strCNPJTrans_Digito, string strVersao, string strXMLPedido, string strXMLResposta, string strNSU, string strDataPedido, string strDataRegistro, string strIP)
        {
            // Classes e variaveis utilizadas
            AcessoDados clsDados = new AcessoDados();
            StringBuilder stbSQL = new StringBuilder();
            int intRetorno = 0;

            try
            {
                // Montando query a ser executada
                stbSQL.Append(clsFacil.MontarQuery("exec up_inserir_dados_conhecimento_transporte_eletronico_evento "));
                stbSQL.Append(clsFacil.MontarQuery("  @seq_evento_nota_fiscal_eletronica, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_tipo_evento_nota_fiscal_eletronica, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_protocolo, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_cnpj_base_trans, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_cnpj_filial_trans, "));
                stbSQL.Append(clsFacil.MontarQuery("  @dig_cnpj_trans, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_versao_xml, "));
                stbSQL.Append(clsFacil.MontarQuery("  @dtc_pedido_evento, "));
                stbSQL.Append(clsFacil.MontarQuery("  @dtc_registro_evento, "));
                stbSQL.Append(clsFacil.MontarQuery("  @xml_pedido_evento, "));
                stbSQL.Append(clsFacil.MontarQuery("  @xml_resposta_evento, "));
                stbSQL.Append(clsFacil.MontarQuery("  @seq_unico_ambiente_nacional, "));
                stbSQL.Append(clsFacil.MontarQuery("  @des_endereco_logico "));
                string strSQL = stbSQL.ToString();

                // Montando parametros
                clsDados.LimparParametro();
                clsFacil.AdicionarParametro(ref strSQL, strCodigo, "@seq_evento_nota_fiscal_eletronica", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, strTipo, "@cod_tipo_evento_nota_fiscal_eletronica", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, strChaveAcesso, "@cod_chave_acesso", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, strNumProtocolo, "@num_protocolo", SqlDbType.BigInt);
                clsFacil.AdicionarParametro(ref strSQL, strCNPJTrans_Base, "@num_cnpj_base_trans", SqlDbType.Int);
                clsFacil.AdicionarParametro(ref strSQL, strCNPJTrans_Filial, "@num_cnpj_filial_trans", SqlDbType.SmallInt);
                clsFacil.AdicionarParametro(ref strSQL, strCNPJTrans_Digito, "@dig_cnpj_trans", SqlDbType.TinyInt);
                clsFacil.AdicionarParametro(ref strSQL, strVersao, "@num_versao_xml", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, strDataPedido, "@dtc_pedido_evento", SqlDbType.SmallDateTime);
                clsFacil.AdicionarParametro(ref strSQL, strDataRegistro, "@dtc_registro_evento", SqlDbType.DateTime);
                clsFacil.AdicionarParametro(ref strSQL, strXMLPedido, "@xml_pedido_evento", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, strXMLResposta, "@xml_resposta_evento", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, strNSU, "@seq_unico_ambiente_nacional", SqlDbType.BigInt);
                clsFacil.AdicionarParametro(ref strSQL, strIP, "@des_endereco_logico", SqlDbType.VarChar);

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

        #region " InserirDadosCTeInutilizacao "

        public int InserirDadosCTeInutilizacao(string strNumProtocolo, string strModelo, string strAno, string strSerie, string strNumInicialFaixa, string strNumFinalFaixa, string strCNPJEmi_Base, string strCNPJEmi_Filial, string strCNPJEmi_Digito, string strVersao, string strXMLPedido, string strXMLResposta, string strNSU, string strDataPedido, string strIP)
        {
            // Classes e variaveis utilizadas
            AcessoDados clsDados = new AcessoDados();
            StringBuilder stbSQL = new StringBuilder();
            int intRetorno = 0;

            try
            {
                // Montando query a ser executada
                stbSQL.Append(clsFacil.MontarQuery("exec up_inserir_dados_conhecimento_transporte_eletronico_inutilizacao "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_protocolo, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_modelo, "));
                stbSQL.Append(clsFacil.MontarQuery("  @ano_inutilizacao, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_serie, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_inicial_faixa, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_final_faixa, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_cnpj_base_emi, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_cnpj_filial_emi, "));
                stbSQL.Append(clsFacil.MontarQuery("  @dig_cnpj_emi, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_versao_xml, "));
                stbSQL.Append(clsFacil.MontarQuery("  @xml_pedido, "));
                stbSQL.Append(clsFacil.MontarQuery("  @xml_resposta, "));
                stbSQL.Append(clsFacil.MontarQuery("  @seq_unico_ambiente_nacional, "));
                stbSQL.Append(clsFacil.MontarQuery("  @dtc_pedido_inutilizacao, "));
                stbSQL.Append(clsFacil.MontarQuery("  @des_endereco_logico "));
                string strSQL = stbSQL.ToString();

                // Montando parametros
                clsDados.LimparParametro();
                clsFacil.AdicionarParametro(ref strSQL, strNumProtocolo, "@num_protocolo", SqlDbType.BigInt);
                clsFacil.AdicionarParametro(ref strSQL, strModelo, "@cod_modelo", SqlDbType.TinyInt);
                clsFacil.AdicionarParametro(ref strSQL, strAno, "@ano_inutilizacao", SqlDbType.SmallInt);
                clsFacil.AdicionarParametro(ref strSQL, strSerie, "@num_serie", SqlDbType.SmallInt);
                clsFacil.AdicionarParametro(ref strSQL, strNumInicialFaixa, "@num_inicial_faixa", SqlDbType.Int);
                clsFacil.AdicionarParametro(ref strSQL, strNumFinalFaixa, "@num_final_faixa", SqlDbType.Int);
                clsFacil.AdicionarParametro(ref strSQL, strCNPJEmi_Base, "@num_cnpj_base_emi", SqlDbType.Int);
                clsFacil.AdicionarParametro(ref strSQL, strCNPJEmi_Filial, "@num_cnpj_filial_emi", SqlDbType.SmallInt);
                clsFacil.AdicionarParametro(ref strSQL, strCNPJEmi_Digito, "@dig_cnpj_emi", SqlDbType.TinyInt);
                clsFacil.AdicionarParametro(ref strSQL, strVersao, "@num_versao_xml", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, strXMLPedido, "@xml_pedido", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, strXMLResposta, "@xml_resposta", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, strNSU, "@seq_unico_ambiente_nacional", SqlDbType.BigInt);
                clsFacil.AdicionarParametro(ref strSQL, strDataPedido, "@dtc_pedido_inutilizacao", SqlDbType.DateTime);
                clsFacil.AdicionarParametro(ref strSQL, strIP, "@des_endereco_logico", SqlDbType.VarChar);

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

        #region " ObterCTeAutorizacaoParaCarga "

        public DataSet ObterCTeAutorizacaoParaCarga(string strDataInicio, string strDataFim)
        {
            // Classes e variaveis utilizadas
            AcessoDados clsDados = new AcessoDados();
            StringBuilder stbSQL = new StringBuilder();
            DataSet dstRetorno;

            try
            {
                // Montando query a ser executada
                stbSQL.Append(clsFacil.MontarQuery("SELECT TOP(1) "));
                stbSQL.Append(clsFacil.MontarQuery("  seq_unico_ambiente_nacional as nsu, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso, "));
                stbSQL.Append(clsFacil.MontarQuery("  convert(varchar(max),xml_pedido) as xml_pedido, "));
                stbSQL.Append(clsFacil.MontarQuery("  convert(varchar(max),xml_resposta) as xml_resposta, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_versao_xml, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_protocolo, "));
                stbSQL.Append(clsFacil.MontarQuery("  dtc_autorizacao_uso as dtc_documento "));
                stbSQL.Append(clsFacil.MontarQuery("FROM "));
                stbSQL.Append(clsFacil.MontarQuery("  mdfe.manifesto_documento_fiscal_eletronico_autorizacao WITH (READPAST) "));
                stbSQL.Append(clsFacil.MontarQuery("WHERE "));
                stbSQL.Append(clsFacil.MontarQuery("  dtc_historico between @dtc_inicio AND @dtc_fim "));
                string strSQL = stbSQL.ToString();

                // Informando parametros
                clsDados.LimparParametro();
                clsFacil.AdicionarParametro(ref strSQL, strDataInicio, "@dtc_inicio", SqlDbType.DateTime);
                clsFacil.AdicionarParametro(ref strSQL, strDataFim, "@dtc_fim", SqlDbType.DateTime);

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
            }

            return dstRetorno;
        }

        #endregion

        #region " ObterCTeEventoParaCarga "

        public DataSet ObterCTeEventoParaCarga(string strDataInicio, string strDataFim)
        {
            // Classes e variaveis utilizadas
            AcessoDados clsDados = new AcessoDados();
            StringBuilder stbSQL = new StringBuilder();
            DataSet dstRetorno;

            try
            {
                // Montando query a ser executada
                stbSQL.Append(clsFacil.MontarQuery("SELECT TOP(1) "));
                stbSQL.Append(clsFacil.MontarQuery("  seq_unico_ambiente_nacional as nsu, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_tipo_evento_nota_fiscal_eletronica as cod_tipo_evento, "));
                stbSQL.Append(clsFacil.MontarQuery("  seq_evento_nota_fiscal_eletronica as seq_evento, "));
                stbSQL.Append(clsFacil.MontarQuery("  convert(varchar(max),xml_pedido_evento) as xml_pedido, "));
                stbSQL.Append(clsFacil.MontarQuery("  convert(varchar(max),xml_resposta_evento) as xml_resposta, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_versao_xml, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_protocolo, "));
                stbSQL.Append(clsFacil.MontarQuery("  dtc_pedido_evento as dtc_documento "));
                stbSQL.Append(clsFacil.MontarQuery("FROM "));
                stbSQL.Append(clsFacil.MontarQuery("  mdfe.manifesto_documento_fiscal_eletronico_evento WITH (READPAST) "));
                stbSQL.Append(clsFacil.MontarQuery("WHERE "));
                stbSQL.Append(clsFacil.MontarQuery("  dtc_historico between @dtc_inicio AND @dtc_fim "));
                string strSQL = stbSQL.ToString();

                // Informando parametros
                clsDados.LimparParametro();
                clsFacil.AdicionarParametro(ref strSQL, strDataInicio, "@dtc_inicio", SqlDbType.DateTime);
                clsFacil.AdicionarParametro(ref strSQL, strDataFim, "@dtc_fim", SqlDbType.DateTime);

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
            }

            return dstRetorno;
        }

        #endregion

        #region " ExcluirCTeAutorizacao "

        public int ExcluirCTeAutorizacao(string strChave)
        {
            // Classes e variaveis utilizadas
            AcessoDados clsDados = new AcessoDados();
            StringBuilder stbSQL = new StringBuilder();
            int intRetorno = 0;

            try
            {
                // Montando query a ser executada
                stbSQL.Append(clsFacil.MontarQuery("DELETE TOP(1) "));
                stbSQL.Append(clsFacil.MontarQuery("FROM "));
                stbSQL.Append(clsFacil.MontarQuery("  mdfe.manifesto_documento_fiscal_eletronico_autorizacao "));
                stbSQL.Append(clsFacil.MontarQuery("WHERE "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso = @cod_chave_acesso "));
                string strSQL = stbSQL.ToString();

                // Montando parametros
                clsDados.LimparParametro();
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

        #region " ExcluirCTeEvento "

        public int ExcluirCTeEvento(string strChave, string strTipo, string strSeq)
        {
            // Classes e variaveis utilizadas
            AcessoDados clsDados = new AcessoDados();
            StringBuilder stbSQL = new StringBuilder();
            int intRetorno = 0;

            try
            {
                // Montando query a ser executada
                stbSQL.Append(clsFacil.MontarQuery("DELETE TOP(1) "));
                stbSQL.Append(clsFacil.MontarQuery("FROM "));
                stbSQL.Append(clsFacil.MontarQuery("  mdfe.manifesto_documento_fiscal_eletronico_evento "));
                stbSQL.Append(clsFacil.MontarQuery("WHERE "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso = @cod_chave_acesso "));
                stbSQL.Append(clsFacil.MontarQuery("  AND cod_tipo_evento_nota_fiscal_eletronica = @cod_tipo_evento_nota_fiscal_eletronica "));
                stbSQL.Append(clsFacil.MontarQuery("  AND seq_evento_nota_fiscal_eletronica = @seq_evento_nota_fiscal_eletronica "));
                string strSQL = stbSQL.ToString();

                // Montando parametros
                clsDados.LimparParametro();
                clsFacil.AdicionarParametro(ref strSQL, strChave, "@cod_chave_acesso", SqlDbType.Char);
                clsFacil.AdicionarParametro(ref strSQL, strTipo, "@cod_tipo_evento_nota_fiscal_eletronica", SqlDbType.Int);
                clsFacil.AdicionarParametro(ref strSQL, strSeq, "@seq_evento_nota_fiscal_eletronica", SqlDbType.TinyInt);

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