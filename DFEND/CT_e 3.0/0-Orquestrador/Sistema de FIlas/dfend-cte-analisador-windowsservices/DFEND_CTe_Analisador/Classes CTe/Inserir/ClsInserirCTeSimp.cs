using System;
using System.Data;
using System.Text;

namespace DFe
{
    class ClsInserirCTeSimp
    {

        #region " Inserir CTeSimp "

        public string InserirXML_detalhe_xml_conhecimento_transporte_eletronico_simp_autorizado(Common.SerializableClasses.CTe.xmlCTeSimp_v400.proc CTe)
        {
            // Classes e variaveis utilizadas
            AcessoDados clsDados = new AcessoDados();
            Facilitador clsFacil = new Facilitador();
            StringBuilder stbSQL = new StringBuilder();
            string strRetorno = string.Empty;
            string strMetodo = System.Reflection.MethodBase.GetCurrentMethod().Name;

            try
            {

                // Montando query a ser executada
                stbSQL.Append(clsFacil.MontarQuery("INSERT INTO cte.detalhe_xml_conhecimento_transporte_eletronico_simp_autorizado "));
                stbSQL.Append(clsFacil.MontarQuery("( "));
                stbSQL.Append(clsFacil.MontarQuery("  dtr_referencia, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_chave_acesso, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_unid_federacao_ibge, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_cfop, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_natureza_operacao, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_modelo_documento_fiscal, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_serie_conhecimento_transporte_eletronico, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_conhecimento_transporte_eletronico, "));
                stbSQL.Append(clsFacil.MontarQuery("  dtc_emissao, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_tipo_emissao, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_digito_verificador_chave_acesso, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_tipo_cte_simplificada, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_tipo_processo_emissao_cte, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_versao_processo_emissao, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_municipio_ibge_transmitido, "));
                stbSQL.Append(clsFacil.MontarQuery("  des_municipio_ibge_transmitido, "));
                stbSQL.Append(clsFacil.MontarQuery("  sig_unid_federacao_transmitido, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_tipo_modal, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_tipo_servico, "));
                stbSQL.Append(clsFacil.MontarQuery("  sig_unid_federacao_inicio_prestacao, "));
                stbSQL.Append(clsFacil.MontarQuery("  sig_unid_federacao_termino_prestacao, "));
                stbSQL.Append(clsFacil.MontarQuery("  ind_recebedor_retira, "));
                stbSQL.Append(clsFacil.MontarQuery("  des_recebedor_retira, "));
                stbSQL.Append(clsFacil.MontarQuery("  dtc_entrega_contingencia, "));
                stbSQL.Append(clsFacil.MontarQuery("  des_entrega_contingencia, "));
                stbSQL.Append(clsFacil.MontarQuery("  des_caracteristica_adicional_transporte, "));
                stbSQL.Append(clsFacil.MontarQuery("  des_caracteristica_adicional_servico, "));
                stbSQL.Append(clsFacil.MontarQuery("  sig_interno_origem, "));
                stbSQL.Append(clsFacil.MontarQuery("  sig_interno_destino, "));
                stbSQL.Append(clsFacil.MontarQuery("  sig_interno_rota, "));
                stbSQL.Append(clsFacil.MontarQuery("  des_observacao, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_cnpj_cpf_base_emitente, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_cnpj_cpf_filial_emitente, "));
                stbSQL.Append(clsFacil.MontarQuery("  dig_cnpj_cpf_emitente, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_insc_estad_emitente, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_insc_estad_substituto_tributario, "));
                stbSQL.Append(clsFacil.MontarQuery("  nom_razao_social_emitente, "));
                stbSQL.Append(clsFacil.MontarQuery("  nom_fantasia_emitente, "));
                stbSQL.Append(clsFacil.MontarQuery("  des_logradouro_emitente, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_endereco_emitente, "));
                stbSQL.Append(clsFacil.MontarQuery("  des_compl_endereco_emitente, "));
                stbSQL.Append(clsFacil.MontarQuery("  des_bairro_emitente, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_municipio_ibge_emitente, "));
                stbSQL.Append(clsFacil.MontarQuery("  nom_municipio_ibge_emitente, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_cep_emitente, "));
                stbSQL.Append(clsFacil.MontarQuery("  sig_unid_federacao_emitente, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_telefone_emitente, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_tipo_regime_tributario_emitente, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_tomador_servico, "));
                stbSQL.Append(clsFacil.MontarQuery("  sig_tomador_servico, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_cnpj_cpf_base_tomador, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_cnpj_cpf_filial_tomador, "));
                stbSQL.Append(clsFacil.MontarQuery("  dig_cnpj_cpf_tomador, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_insc_estad_tomador, "));
                stbSQL.Append(clsFacil.MontarQuery("  nom_razao_social_tomador, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_insc_suframa_tomador, "));
                stbSQL.Append(clsFacil.MontarQuery("  des_logradouro_tomador, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_endereco_tomador, "));
                stbSQL.Append(clsFacil.MontarQuery("  des_compl_endereco_tomador, "));
                stbSQL.Append(clsFacil.MontarQuery("  des_bairro_tomador, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_municipio_ibge_tomador, "));
                stbSQL.Append(clsFacil.MontarQuery("  nom_municipio_ibge_tomador, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_cep_tomador, "));
                stbSQL.Append(clsFacil.MontarQuery("  sig_unid_federacao_tomador, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_telefone_tomador, "));
                stbSQL.Append(clsFacil.MontarQuery("  nom_email_tomador, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_tipo_classificacao_tributaria, "));
                stbSQL.Append(clsFacil.MontarQuery("  val_base_calculo, "));
                stbSQL.Append(clsFacil.MontarQuery("  val_aliquota, "));
                stbSQL.Append(clsFacil.MontarQuery("  val_icms, "));
                stbSQL.Append(clsFacil.MontarQuery("  prc_base_calculo_reducao, "));
                stbSQL.Append(clsFacil.MontarQuery("  val_base_calculo_reducao, "));
                stbSQL.Append(clsFacil.MontarQuery("  prc_aliquota_icms_reducao, "));
                stbSQL.Append(clsFacil.MontarQuery("  val_aliquota_icms, "));
                stbSQL.Append(clsFacil.MontarQuery("  val_base_calculo_st_retido, "));
                stbSQL.Append(clsFacil.MontarQuery("  val_st_retido, "));
                stbSQL.Append(clsFacil.MontarQuery("  prc_aliquota_icms_st_retido, "));
                stbSQL.Append(clsFacil.MontarQuery("  val_st_credito_outorgado, "));
                stbSQL.Append(clsFacil.MontarQuery("  per_reducao_base_calculo_outros, "));
                stbSQL.Append(clsFacil.MontarQuery("  val_base_calculo_outros, "));
                stbSQL.Append(clsFacil.MontarQuery("  prc_aliquota_icms_outros, "));
                stbSQL.Append(clsFacil.MontarQuery("  val_aliquota_icms_outros, "));
                stbSQL.Append(clsFacil.MontarQuery("  val_credito_outorgado_outros, "));
                stbSQL.Append(clsFacil.MontarQuery("  per_reducao_base_calculo_outra_uf, "));
                stbSQL.Append(clsFacil.MontarQuery("  val_base_calculo_outra_uf, "));
                stbSQL.Append(clsFacil.MontarQuery("  prc_aliquota_icms_outra_uf, "));
                stbSQL.Append(clsFacil.MontarQuery("  val_icms_outra_uf, "));
                stbSQL.Append(clsFacil.MontarQuery("  ind_simples_nacional, "));
                stbSQL.Append(clsFacil.MontarQuery("  val_total_tributos, "));
                stbSQL.Append(clsFacil.MontarQuery("  des_informacao_adicional_fisco, "));
                stbSQL.Append(clsFacil.MontarQuery("  val_base_calculo_uf_termino, "));
                stbSQL.Append(clsFacil.MontarQuery("  prc_base_calculo_icms_fundo_combate_pobreza_uf_termino, "));
                stbSQL.Append(clsFacil.MontarQuery("  prc_aliquota_interna_icms_uf_termino, "));
                stbSQL.Append(clsFacil.MontarQuery("  prc_aliquota_interestadual_uf_envolvida, "));
                stbSQL.Append(clsFacil.MontarQuery("  val_icms_fundo_combate_pobreza_uf_termino, "));
                stbSQL.Append(clsFacil.MontarQuery("  val_icms_partilha_uf_termino, "));
                stbSQL.Append(clsFacil.MontarQuery("  val_icms_partilha_uf_inicio, "));
                stbSQL.Append(clsFacil.MontarQuery("  val_total_carga, "));
                stbSQL.Append(clsFacil.MontarQuery("  des_produto_predominante_carga, "));
                stbSQL.Append(clsFacil.MontarQuery("  des_outras_caracteristicas_carga, "));
                stbSQL.Append(clsFacil.MontarQuery("  val_carga_averbacao, "));
                stbSQL.Append(clsFacil.MontarQuery("  val_total_prestacao_servico, "));
                stbSQL.Append(clsFacil.MontarQuery("  val_total_receber, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_cnpj_cpf_base_responsavel, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_cnpj_cpf_filial_responsavel, "));
                stbSQL.Append(clsFacil.MontarQuery("  dig_cnpj_cpf_responsavel, "));
                stbSQL.Append(clsFacil.MontarQuery("  nom_pessoa_responsavel, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_telefone_responsavel, "));
                stbSQL.Append(clsFacil.MontarQuery("  nom_email_responsavel, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_codigo_seguranca_responsavel, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_hash_token_responsavel, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso_cte, "));
                stbSQL.Append(clsFacil.MontarQuery("  sts_alteracao_tomador, "));
                stbSQL.Append(clsFacil.MontarQuery("  des_qrcode_impresso_dacte, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_fatura, "));
                stbSQL.Append(clsFacil.MontarQuery("  val_original, "));
                stbSQL.Append(clsFacil.MontarQuery("  val_desconto, "));
                stbSQL.Append(clsFacil.MontarQuery("  val_liquido, "));
                stbSQL.Append(clsFacil.MontarQuery("  des_campo_emissao_nff, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_cnpj_cpf_base_provedor_assinatura_autorizacao, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_cnpj_cpf_filial_provedor_assinatura_autorizacao, "));
                stbSQL.Append(clsFacil.MontarQuery("  dig_cnpj_cpf_provedor_assinatura_autorizacao, "));
                stbSQL.Append(clsFacil.MontarQuery("  des_assinatura_rsa, "));
                stbSQL.Append(clsFacil.MontarQuery("  des_chave_publica_xml_rsa_key, "));
                stbSQL.Append(clsFacil.MontarQuery("  val_icms_desoneracao, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_beneficio_fiscal_unid_federacao, "));
                stbSQL.Append(clsFacil.MontarQuery("  dtc_atualizacao "));
                stbSQL.Append(clsFacil.MontarQuery(") "));
                stbSQL.Append(clsFacil.MontarQuery("VALUES "));
                stbSQL.Append(clsFacil.MontarQuery("( "));
                stbSQL.Append(clsFacil.MontarQuery("  @dtr_referencia, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_chave_acesso, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_unid_federacao_ibge, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_cfop, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_natureza_operacao, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_modelo_documento_fiscal, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_serie_conhecimento_transporte_eletronico, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_conhecimento_transporte_eletronico, "));
                stbSQL.Append(clsFacil.MontarQuery("  @dtc_emissao, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_tipo_emissao, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_digito_verificador_chave_acesso, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_tipo_cte_simplificada, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_tipo_processo_emissao_cte, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_versao_processo_emissao, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_municipio_ibge_transmitido, "));
                stbSQL.Append(clsFacil.MontarQuery("  @des_municipio_ibge_transmitido, "));
                stbSQL.Append(clsFacil.MontarQuery("  @sig_unid_federacao_transmitido, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_tipo_modal, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_tipo_servico, "));
                stbSQL.Append(clsFacil.MontarQuery("  @sig_unid_federacao_inicio_prestacao, "));
                stbSQL.Append(clsFacil.MontarQuery("  @sig_unid_federacao_termino_prestacao, "));
                stbSQL.Append(clsFacil.MontarQuery("  @ind_recebedor_retira, "));
                stbSQL.Append(clsFacil.MontarQuery("  @des_recebedor_retira, "));
                stbSQL.Append(clsFacil.MontarQuery("  @dtc_entrega_contingencia, "));
                stbSQL.Append(clsFacil.MontarQuery("  @des_entrega_contingencia, "));
                stbSQL.Append(clsFacil.MontarQuery("  @des_caracteristica_adicional_transporte, "));
                stbSQL.Append(clsFacil.MontarQuery("  @des_caracteristica_adicional_servico, "));
                stbSQL.Append(clsFacil.MontarQuery("  @sig_interno_origem, "));
                stbSQL.Append(clsFacil.MontarQuery("  @sig_interno_destino, "));
                stbSQL.Append(clsFacil.MontarQuery("  @sig_interno_rota, "));
                stbSQL.Append(clsFacil.MontarQuery("  @des_observacao, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_cnpj_cpf_base_emitente, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_cnpj_cpf_filial_emitente, "));
                stbSQL.Append(clsFacil.MontarQuery("  @dig_cnpj_cpf_emitente, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_insc_estad_emitente, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_insc_estad_substituto_tributario, "));
                stbSQL.Append(clsFacil.MontarQuery("  @nom_razao_social_emitente, "));
                stbSQL.Append(clsFacil.MontarQuery("  @nom_fantasia_emitente, "));
                stbSQL.Append(clsFacil.MontarQuery("  @des_logradouro_emitente, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_endereco_emitente, "));
                stbSQL.Append(clsFacil.MontarQuery("  @des_compl_endereco_emitente, "));
                stbSQL.Append(clsFacil.MontarQuery("  @des_bairro_emitente, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_municipio_ibge_emitente, "));
                stbSQL.Append(clsFacil.MontarQuery("  @nom_municipio_ibge_emitente, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_cep_emitente, "));
                stbSQL.Append(clsFacil.MontarQuery("  @sig_unid_federacao_emitente, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_telefone_emitente, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_tipo_regime_tributario_emitente, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_tomador_servico, "));
                stbSQL.Append(clsFacil.MontarQuery("  @sig_tomador_servico, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_cnpj_cpf_base_tomador, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_cnpj_cpf_filial_tomador, "));
                stbSQL.Append(clsFacil.MontarQuery("  @dig_cnpj_cpf_tomador, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_insc_estad_tomador, "));
                stbSQL.Append(clsFacil.MontarQuery("  @nom_razao_social_tomador, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_insc_suframa_tomador, "));
                stbSQL.Append(clsFacil.MontarQuery("  @des_logradouro_tomador, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_endereco_tomador, "));
                stbSQL.Append(clsFacil.MontarQuery("  @des_compl_endereco_tomador, "));
                stbSQL.Append(clsFacil.MontarQuery("  @des_bairro_tomador, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_municipio_ibge_tomador, "));
                stbSQL.Append(clsFacil.MontarQuery("  @nom_municipio_ibge_tomador, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_cep_tomador, "));
                stbSQL.Append(clsFacil.MontarQuery("  @sig_unid_federacao_tomador, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_telefone_tomador, "));
                stbSQL.Append(clsFacil.MontarQuery("  @nom_email_tomador, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_tipo_classificacao_tributaria, "));
                stbSQL.Append(clsFacil.MontarQuery("  @val_base_calculo, "));
                stbSQL.Append(clsFacil.MontarQuery("  @val_aliquota, "));
                stbSQL.Append(clsFacil.MontarQuery("  @val_icms, "));
                stbSQL.Append(clsFacil.MontarQuery("  @prc_base_calculo_reducao, "));
                stbSQL.Append(clsFacil.MontarQuery("  @val_base_calculo_reducao, "));
                stbSQL.Append(clsFacil.MontarQuery("  @prc_aliquota_icms_reducao, "));
                stbSQL.Append(clsFacil.MontarQuery("  @val_aliquota_icms, "));
                stbSQL.Append(clsFacil.MontarQuery("  @val_base_calculo_st_retido, "));
                stbSQL.Append(clsFacil.MontarQuery("  @val_st_retido, "));
                stbSQL.Append(clsFacil.MontarQuery("  @prc_aliquota_icms_st_retido, "));
                stbSQL.Append(clsFacil.MontarQuery("  @val_st_credito_outorgado, "));
                stbSQL.Append(clsFacil.MontarQuery("  @per_reducao_base_calculo_outros, "));
                stbSQL.Append(clsFacil.MontarQuery("  @val_base_calculo_outros, "));
                stbSQL.Append(clsFacil.MontarQuery("  @prc_aliquota_icms_outros, "));
                stbSQL.Append(clsFacil.MontarQuery("  @val_aliquota_icms_outros, "));
                stbSQL.Append(clsFacil.MontarQuery("  @val_credito_outorgado_outros, "));
                stbSQL.Append(clsFacil.MontarQuery("  @per_reducao_base_calculo_outra_uf, "));
                stbSQL.Append(clsFacil.MontarQuery("  @val_base_calculo_outra_uf, "));
                stbSQL.Append(clsFacil.MontarQuery("  @prc_aliquota_icms_outra_uf, "));
                stbSQL.Append(clsFacil.MontarQuery("  @val_icms_outra_uf, "));
                stbSQL.Append(clsFacil.MontarQuery("  @ind_simples_nacional, "));
                stbSQL.Append(clsFacil.MontarQuery("  @val_total_tributos, "));
                stbSQL.Append(clsFacil.MontarQuery("  @des_informacao_adicional_fisco, "));
                stbSQL.Append(clsFacil.MontarQuery("  @val_base_calculo_uf_termino, "));
                stbSQL.Append(clsFacil.MontarQuery("  @prc_base_calculo_icms_fundo_combate_pobreza_uf_termino, "));
                stbSQL.Append(clsFacil.MontarQuery("  @prc_aliquota_interna_icms_uf_termino, "));
                stbSQL.Append(clsFacil.MontarQuery("  @prc_aliquota_interestadual_uf_envolvida, "));
                stbSQL.Append(clsFacil.MontarQuery("  @val_icms_fundo_combate_pobreza_uf_termino, "));
                stbSQL.Append(clsFacil.MontarQuery("  @val_icms_partilha_uf_termino, "));
                stbSQL.Append(clsFacil.MontarQuery("  @val_icms_partilha_uf_inicio, "));
                stbSQL.Append(clsFacil.MontarQuery("  @val_total_carga, "));
                stbSQL.Append(clsFacil.MontarQuery("  @des_produto_predominante_carga, "));
                stbSQL.Append(clsFacil.MontarQuery("  @des_outras_caracteristicas_carga, "));
                stbSQL.Append(clsFacil.MontarQuery("  @val_carga_averbacao, "));
                stbSQL.Append(clsFacil.MontarQuery("  @val_total_prestacao_servico, "));
                stbSQL.Append(clsFacil.MontarQuery("  @val_total_receber, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_cnpj_cpf_base_responsavel, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_cnpj_cpf_filial_responsavel, "));
                stbSQL.Append(clsFacil.MontarQuery("  @dig_cnpj_cpf_responsavel, "));
                stbSQL.Append(clsFacil.MontarQuery("  @nom_pessoa_responsavel, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_telefone_responsavel, "));
                stbSQL.Append(clsFacil.MontarQuery("  @nom_email_responsavel, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_codigo_seguranca_responsavel, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_hash_token_responsavel, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso_cte, "));
                stbSQL.Append(clsFacil.MontarQuery("  @sts_alteracao_tomador, "));
                stbSQL.Append(clsFacil.MontarQuery("  @des_qrcode_impresso_dacte, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_fatura, "));
                stbSQL.Append(clsFacil.MontarQuery("  @val_original, "));
                stbSQL.Append(clsFacil.MontarQuery("  @val_desconto, "));
                stbSQL.Append(clsFacil.MontarQuery("  @val_liquido, "));
                stbSQL.Append(clsFacil.MontarQuery("  @des_campo_emissao_nff, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_cnpj_cpf_base_provedor_assinatura_autorizacao, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_cnpj_cpf_filial_provedor_assinatura_autorizacao, "));
                stbSQL.Append(clsFacil.MontarQuery("  @dig_cnpj_cpf_provedor_assinatura_autorizacao, "));
                stbSQL.Append(clsFacil.MontarQuery("  @des_assinatura_rsa, "));
                stbSQL.Append(clsFacil.MontarQuery("  @des_chave_publica_xml_rsa_key, "));
                stbSQL.Append(clsFacil.MontarQuery("  @val_icms_desoneracao, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_beneficio_fiscal_unid_federacao, "));
                stbSQL.Append(clsFacil.MontarQuery("  getdate() "));
                stbSQL.Append(clsFacil.MontarQuery(") "));
                string strSQL = stbSQL.ToString();

                // Montando parametros
                clsDados.LimparParametro();
                // Montando parametros
                clsDados.LimparParametro();
                clsFacil.AdicionarParametro(ref strSQL, "20" + CTe.CteSimpProc.protCTe.InfProt.chCTe.Substring(2, 4), "@dtr_referencia", SqlDbType.Int);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.protCTe.InfProt.chCTe, "@cod_chave_acesso", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.ide.cCT, "@num_chave_acesso", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.ide.cUF, "@cod_unid_federacao_ibge", SqlDbType.SmallInt);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.ide.CFOP, "@num_cfop", SqlDbType.Int);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.ide.natOp, "@cod_natureza_operacao", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.ide.mod, "@num_modelo_documento_fiscal", SqlDbType.SmallInt);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.ide.serie, "@num_serie_conhecimento_transporte_eletronico", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.ide.nCT, "@num_conhecimento_transporte_eletronico", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.ide.dhEmi, "@dtc_emissao", SqlDbType.DateTime);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.ide.tpEmis, "@cod_tipo_emissao", SqlDbType.SmallInt);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.ide.cDV, "@num_digito_verificador_chave_acesso", SqlDbType.SmallInt);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.ide.tpCTe, "@cod_tipo_cte_simplificada", SqlDbType.SmallInt);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.ide.procEmi, "@cod_tipo_processo_emissao_cte", SqlDbType.SmallInt);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.ide.verProc, "@num_versao_processo_emissao", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.ide.cMunEnv, "@cod_municipio_ibge_transmitido", SqlDbType.Int);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.ide.xMunEnv, "@des_municipio_ibge_transmitido", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.ide.UFEnv, "@sig_unid_federacao_transmitido", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.ide.modal, "@cod_tipo_modal", SqlDbType.SmallInt);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.ide.tpServ, "@cod_tipo_servico", SqlDbType.SmallInt);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.ide.UFIni, "@sig_unid_federacao_inicio_prestacao", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.ide.UFFim, "@sig_unid_federacao_termino_prestacao", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.ide.retira, "@ind_recebedor_retira", SqlDbType.SmallInt);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.ide.xDetRetira, "@des_recebedor_retira", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.ide.dhCont, "@dtc_entrega_contingencia", SqlDbType.DateTime);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.ide.xJust, "@des_entrega_contingencia", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.compl.xCaracAd, "@des_caracteristica_adicional_transporte", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.compl.xCaracSer, "@des_caracteristica_adicional_servico", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.compl.fluxo.xOrig, "@sig_interno_origem", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.compl.fluxo.xDest, "@sig_interno_destino", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.compl.fluxo.xRota, "@sig_interno_rota", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.compl.xObs, "@des_observacao", SqlDbType.VarChar);
                if (CTe.CteSimpProc.CTeSimp.infCte.emit.CPF != null && CTe.CteSimpProc.CTeSimp.infCte.emit.CPF != string.Empty)
                {
                    clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.emit.CPF.Substring(0, 9), "@num_cnpj_cpf_base_emitente", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, "0", "@num_cnpj_cpf_filial_emitente", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.emit.CPF.Substring(9, 2), "@dig_cnpj_cpf_emitente", SqlDbType.SmallInt);
                }
                if (CTe.CteSimpProc.CTeSimp.infCte.emit.CNPJ != null && CTe.CteSimpProc.CTeSimp.infCte.emit.CNPJ != string.Empty)
                {
                    clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.emit.CNPJ.Substring(0, 8), "@num_cnpj_cpf_base_emitente", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.emit.CNPJ.Substring(8, 4), "@num_cnpj_cpf_filial_emitente", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.emit.CNPJ.Substring(12, 2), "@dig_cnpj_cpf_emitente", SqlDbType.SmallInt);
                }
                if ((CTe.CteSimpProc.CTeSimp.infCte.emit.CPF == null && CTe.CteSimpProc.CTeSimp.infCte.emit.CNPJ == null) || (CTe.CteSimpProc.CTeSimp.infCte.emit.CPF == null && CTe.CteSimpProc.CTeSimp.infCte.emit.CNPJ == string.Empty) || (CTe.CteSimpProc.CTeSimp.infCte.emit.CPF == string.Empty && CTe.CteSimpProc.CTeSimp.infCte.emit.CNPJ == null))
                {
                    clsFacil.AdicionarParametro(ref strSQL, string.Empty, "@num_cnpj_cpf_base_emitente", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, string.Empty, "@num_cnpj_cpf_filial_emitente", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, string.Empty, "@dig_cnpj_cpf_emitente", SqlDbType.SmallInt);
                }
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.emit.IE, "@num_insc_estad_emitente", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.emit.IEST, "@num_insc_estad_substituto_tributario", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.emit.xNome, "@nom_razao_social_emitente", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.emit.xFant, "@nom_fantasia_emitente", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.emit.enderEmit.xLgr, "@des_logradouro_emitente", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.emit.enderEmit.nro, "@num_endereco_emitente", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.emit.enderEmit.xCpl, "@des_compl_endereco_emitente", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.emit.enderEmit.xBairro, "@des_bairro_emitente", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.emit.enderEmit.cMun, "@cod_municipio_ibge_emitente", SqlDbType.Int);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.emit.enderEmit.xMun, "@nom_municipio_ibge_emitente", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.emit.enderEmit.CEP, "@num_cep_emitente", SqlDbType.Int);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.emit.enderEmit.UF, "@sig_unid_federacao_emitente", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.emit.enderEmit.fone, "@num_telefone_emitente", SqlDbType.BigInt);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.emit.CRT, "@cod_tipo_regime_tributario_emitente", SqlDbType.SmallInt);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.toma.tomaStr, "@cod_tomador_servico", SqlDbType.SmallInt);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.toma.indIEToma, "@sig_tomador_servico", SqlDbType.SmallInt);

                if (CTe.CteSimpProc.CTeSimp.infCte.toma.CPF != null && CTe.CteSimpProc.CTeSimp.infCte.toma.CPF != string.Empty)
                {
                    clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.toma.CPF.Substring(0, 9), "@num_cnpj_cpf_base_tomador", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, "0", "@num_cnpj_cpf_filial_tomador", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.toma.CPF.Substring(9, 2), "@dig_cnpj_cpf_tomador", SqlDbType.SmallInt);
                }
                if (CTe.CteSimpProc.CTeSimp.infCte.toma.CNPJ != null && CTe.CteSimpProc.CTeSimp.infCte.toma.CNPJ != string.Empty)
                {
                    clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.toma.CNPJ.Substring(0, 8), "@num_cnpj_cpf_base_tomador", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.toma.CNPJ.Substring(8, 4), "@num_cnpj_cpf_filial_tomador", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.toma.CNPJ.Substring(12, 2), "@dig_cnpj_cpf_tomador", SqlDbType.SmallInt);
                }
                if ((CTe.CteSimpProc.CTeSimp.infCte.toma.CPF == null && CTe.CteSimpProc.CTeSimp.infCte.toma.CNPJ == null) || (CTe.CteSimpProc.CTeSimp.infCte.toma.CPF == null && CTe.CteSimpProc.CTeSimp.infCte.toma.CNPJ == string.Empty) || (CTe.CteSimpProc.CTeSimp.infCte.toma.CPF == string.Empty && CTe.CteSimpProc.CTeSimp.infCte.toma.CNPJ == null))
                {
                    clsFacil.AdicionarParametro(ref strSQL, string.Empty, "@num_cnpj_cpf_base_tomador", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, string.Empty, "@num_cnpj_cpf_filial_tomador", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, string.Empty, "@dig_cnpj_cpf_tomador", SqlDbType.SmallInt);
                }

                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.toma.IE, "@num_insc_estad_tomador", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.toma.xNome, "@nom_razao_social_tomador", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.toma.ISUF, "@num_insc_suframa_tomador", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.toma.enderToma.xLgr, "@des_logradouro_tomador", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.toma.enderToma.nro, "@num_endereco_tomador", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.toma.enderToma.xCpl, "@des_compl_endereco_tomador", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.toma.enderToma.xBairro, "@des_bairro_tomador", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.toma.enderToma.cMun, "@cod_municipio_ibge_tomador", SqlDbType.Int);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.toma.enderToma.xMun, "@nom_municipio_ibge_tomador", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.toma.enderToma.CEP, "@num_cep_tomador", SqlDbType.Int);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.toma.enderToma.UF, "@sig_unid_federacao_tomador", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.toma.fone, "@num_telefone_tomador", SqlDbType.BigInt);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.toma.email, "@nom_email_tomador", SqlDbType.VarChar);

                //ICMS
                string CST = string.Empty, vBC = string.Empty, pICMS = string.Empty, vICMS = string.Empty, pRedBC = string.Empty, vICMSDeson = string.Empty;
                string cBenef = string.Empty;
                string vBCSTRet = string.Empty, vICMSSTRet = string.Empty, pICMSSTRet = string.Empty, vCred = string.Empty, pRedBCOutraUF = string.Empty;
                string vBCOutraUF = string.Empty, pICMSOutraUF = string.Empty, vICMSOutraUF = string.Empty; ;
                if (CTe.CteSimpProc.CTeSimp.infCte.imp.ICMS.ICMS00.CST != null)
                {
                    CST = CTe.CteSimpProc.CTeSimp.infCte.imp.ICMS.ICMS00.CST;
                }
                if (CTe.CteSimpProc.CTeSimp.infCte.imp.ICMS.ICMS20.CST != null)
                {
                    CST = CTe.CteSimpProc.CTeSimp.infCte.imp.ICMS.ICMS20.CST;
                    vICMSDeson = Convert.ToString(CTe.CteSimpProc.CTeSimp.infCte.imp.ICMS.ICMS20.vICMSDeson);
                    cBenef = Convert.ToString(CTe.CteSimpProc.CTeSimp.infCte.imp.ICMS.ICMS20.cBenef);
                }
                if (CTe.CteSimpProc.CTeSimp.infCte.imp.ICMS.ICMS45.CST != null)
                {
                    CST = CTe.CteSimpProc.CTeSimp.infCte.imp.ICMS.ICMS45.CST;
                    vICMSDeson = Convert.ToString(CTe.CteSimpProc.CTeSimp.infCte.imp.ICMS.ICMS45.vICMSDeson);
                    cBenef = CTe.CteSimpProc.CTeSimp.infCte.imp.ICMS.ICMS45.cBenef;
                }
                if (CTe.CteSimpProc.CTeSimp.infCte.imp.ICMS.ICMS60.CST != null)
                {
                    CST = CTe.CteSimpProc.CTeSimp.infCte.imp.ICMS.ICMS60.CST;
                    vICMSDeson = Convert.ToString(CTe.CteSimpProc.CTeSimp.infCte.imp.ICMS.ICMS60.vICMSDeson);
                    cBenef = Convert.ToString(CTe.CteSimpProc.CTeSimp.infCte.imp.ICMS.ICMS60.cBenef);
                }
                if (CTe.CteSimpProc.CTeSimp.infCte.imp.ICMS.ICMS90.CST != null)
                {
                    CST = CTe.CteSimpProc.CTeSimp.infCte.imp.ICMS.ICMS90.CST;
                    vICMSDeson = Convert.ToString(CTe.CteSimpProc.CTeSimp.infCte.imp.ICMS.ICMS90.vICMSDeson);
                    cBenef = Convert.ToString(CTe.CteSimpProc.CTeSimp.infCte.imp.ICMS.ICMS90.cBenef);
                }
                if (CTe.CteSimpProc.CTeSimp.infCte.imp.ICMS.ICMSOutraUF.CST != null)
                {
                    CST = CTe.CteSimpProc.CTeSimp.infCte.imp.ICMS.ICMSOutraUF.CST;
                    vICMSDeson = Convert.ToString(CTe.CteSimpProc.CTeSimp.infCte.imp.ICMS.ICMSOutraUF.vICMSDeson);
                    cBenef = CTe.CteSimpProc.CTeSimp.infCte.imp.ICMS.ICMSOutraUF.cBenef;
                }
                if (CTe.CteSimpProc.CTeSimp.infCte.imp.ICMS.ICMSSN.CST != null)
                {
                    CST = CTe.CteSimpProc.CTeSimp.infCte.imp.ICMS.ICMSSN.CST;
                }
                clsFacil.AdicionarParametro(ref strSQL, CST, "@cod_tipo_classificacao_tributaria", SqlDbType.SmallInt);
                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(vICMSDeson), "@val_icms_desoneracao", SqlDbType.Decimal);
                clsFacil.AdicionarParametro(ref strSQL, cBenef, "@cod_beneficio_fiscal_unid_federacao", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTe.CteSimpProc.CTeSimp.infCte.imp.ICMS.ICMS00.vBC), "@val_base_calculo", SqlDbType.Decimal);
                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTe.CteSimpProc.CTeSimp.infCte.imp.ICMS.ICMS00.pICMS), "@val_aliquota", SqlDbType.Decimal);
                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTe.CteSimpProc.CTeSimp.infCte.imp.ICMS.ICMS00.vICMS), "@val_icms", SqlDbType.Decimal);
                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTe.CteSimpProc.CTeSimp.infCte.imp.ICMS.ICMS20.pRedBC), "@prc_base_calculo_reducao", SqlDbType.Decimal);
                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTe.CteSimpProc.CTeSimp.infCte.imp.ICMS.ICMS20.vBC), "@val_base_calculo_reducao", SqlDbType.Decimal);
                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTe.CteSimpProc.CTeSimp.infCte.imp.ICMS.ICMS20.pICMS), "@prc_aliquota_icms_reducao", SqlDbType.Decimal);
                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTe.CteSimpProc.CTeSimp.infCte.imp.ICMS.ICMS20.vICMS), "@val_aliquota_icms", SqlDbType.Decimal);
                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTe.CteSimpProc.CTeSimp.infCte.imp.ICMS.ICMS60.vBCSTRet), "@val_base_calculo_st_retido", SqlDbType.Decimal);
                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTe.CteSimpProc.CTeSimp.infCte.imp.ICMS.ICMS60.vICMSSTRet), "@val_st_retido", SqlDbType.Decimal);
                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTe.CteSimpProc.CTeSimp.infCte.imp.ICMS.ICMS60.pICMSSTRet), "@prc_aliquota_icms_st_retido", SqlDbType.Decimal);
                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTe.CteSimpProc.CTeSimp.infCte.imp.ICMS.ICMS60.vCred), "@val_st_credito_outorgado", SqlDbType.Decimal);

                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTe.CteSimpProc.CTeSimp.infCte.imp.ICMS.ICMS90.pRedBC), "@per_reducao_base_calculo_outros", SqlDbType.Decimal);
                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTe.CteSimpProc.CTeSimp.infCte.imp.ICMS.ICMS90.vBC), "@val_base_calculo_outros", SqlDbType.Decimal);
                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTe.CteSimpProc.CTeSimp.infCte.imp.ICMS.ICMS90.pICMS), "@prc_aliquota_icms_outros", SqlDbType.Decimal);
                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTe.CteSimpProc.CTeSimp.infCte.imp.ICMS.ICMS90.vICMS), "@val_aliquota_icms_outros", SqlDbType.Decimal);
                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTe.CteSimpProc.CTeSimp.infCte.imp.ICMS.ICMS90.vCred), "@val_credito_outorgado_outros", SqlDbType.Decimal);

                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTe.CteSimpProc.CTeSimp.infCte.imp.ICMS.ICMSOutraUF.pRedBCOutraUF), "@per_reducao_base_calculo_outra_uf", SqlDbType.Decimal);
                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTe.CteSimpProc.CTeSimp.infCte.imp.ICMS.ICMSOutraUF.vBCOutraUF), "@val_base_calculo_outra_uf", SqlDbType.Decimal);
                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTe.CteSimpProc.CTeSimp.infCte.imp.ICMS.ICMSOutraUF.pICMSOutraUF), "@prc_aliquota_icms_outra_uf", SqlDbType.Decimal);
                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTe.CteSimpProc.CTeSimp.infCte.imp.ICMS.ICMSOutraUF.vICMSOutraUF), "@val_icms_outra_uf", SqlDbType.Decimal);

                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.imp.ICMS.ICMSSN.indSN, "@ind_simples_nacional", SqlDbType.SmallInt);
                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTe.CteSimpProc.CTeSimp.infCte.imp.vTotTrib), "@val_total_tributos", SqlDbType.Decimal);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.imp.InfAdFisco, "@des_informacao_adicional_fisco", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTe.CteSimpProc.CTeSimp.infCte.imp.ICMSUFFim.vBCUFFim), "@val_base_calculo_uf_termino", SqlDbType.Decimal);
                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTe.CteSimpProc.CTeSimp.infCte.imp.ICMSUFFim.pFCPUFFim), "@prc_base_calculo_icms_fundo_combate_pobreza_uf_termino", SqlDbType.Decimal);
                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTe.CteSimpProc.CTeSimp.infCte.imp.ICMSUFFim.pICMSUFFim), "@prc_aliquota_interna_icms_uf_termino", SqlDbType.Decimal);
                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTe.CteSimpProc.CTeSimp.infCte.imp.ICMSUFFim.pICMSInter), "@prc_aliquota_interestadual_uf_envolvida", SqlDbType.Decimal);
                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTe.CteSimpProc.CTeSimp.infCte.imp.ICMSUFFim.vFCPUFFim), "@val_icms_fundo_combate_pobreza_uf_termino", SqlDbType.Decimal);
                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTe.CteSimpProc.CTeSimp.infCte.imp.ICMSUFFim.vICMSUFFim), "@val_icms_partilha_uf_termino", SqlDbType.Decimal);
                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTe.CteSimpProc.CTeSimp.infCte.imp.ICMSUFFim.vICMSUFIni), "@val_icms_partilha_uf_inicio", SqlDbType.Decimal);

                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTe.CteSimpProc.CTeSimp.infCte.infCarga.vCarga), "@val_total_carga", SqlDbType.Decimal);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.infCarga.proPred, "@des_produto_predominante_carga", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.infCarga.xOutCat, "@des_outras_caracteristicas_carga", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTe.CteSimpProc.CTeSimp.infCte.infCarga.vCargaAverb), "@val_carga_averbacao", SqlDbType.Decimal);
                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTe.CteSimpProc.CTeSimp.infCte.total.vTPrest), "@val_total_prestacao_servico", SqlDbType.Decimal);
                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTe.CteSimpProc.CTeSimp.infCte.total.vTRec), "@val_total_receber", SqlDbType.Decimal);

                if (CTe.CteSimpProc.CTeSimp.infCte.infRespTec.CNPJ != null && CTe.CteSimpProc.CTeSimp.infCte.infRespTec.CNPJ != string.Empty)
                {
                    clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.infRespTec.CNPJ.Substring(0, 8), "@num_cnpj_cpf_base_responsavel", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.infRespTec.CNPJ.Substring(8, 4), "@num_cnpj_cpf_filial_responsavel", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.infRespTec.CNPJ.Substring(12, 2), "@dig_cnpj_cpf_responsavel", SqlDbType.SmallInt);
                }
                else
                {
                    clsFacil.AdicionarParametro(ref strSQL, string.Empty, "@num_cnpj_cpf_base_responsavel", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, string.Empty, "@num_cnpj_cpf_filial_responsavel", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, string.Empty, "@dig_cnpj_cpf_responsavel", SqlDbType.SmallInt);
                }
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.infRespTec.xContato, "@nom_pessoa_responsavel", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.infRespTec.email, "@nom_email_responsavel", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.infRespTec.fone, "@num_telefone_responsavel", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.infRespTec.idCSRT, "@num_codigo_seguranca_responsavel", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.infRespTec.hashCSRT, "@num_hash_token_responsavel", SqlDbType.VarChar);

                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.infCteSub.chCte, "@cod_chave_acesso_cte", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.infCteSub.indAlteraToma, "@sts_alteracao_tomador", SqlDbType.SmallInt);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCTeSupl.qrCodCTe, "@des_qrcode_impresso_dacte", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.cobr.fat.nFat, "@num_fatura", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTe.CteSimpProc.CTeSimp.infCte.cobr.fat.vOrig), "@val_original", SqlDbType.Decimal);
                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTe.CteSimpProc.CTeSimp.infCte.cobr.fat.vDesc), "@val_desconto", SqlDbType.Decimal);
                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTe.CteSimpProc.CTeSimp.infCte.cobr.fat.vLiq), "@val_liquido", SqlDbType.Decimal);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.infSolicNFF.xSolic, "@des_campo_emissao_nff", SqlDbType.VarChar);

                if (CTe.CteSimpProc.CTeSimp.infCte.infRespTec.CNPJ != null && CTe.CteSimpProc.CTeSimp.infCte.infRespTec.CNPJ != string.Empty)
                {
                    clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.infRespTec.CNPJ.Substring(0, 8), "@num_cnpj_cpf_base_provedor_assinatura_autorizacao", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.infRespTec.CNPJ.Substring(8, 4), "@num_cnpj_cpf_filial_provedor_assinatura_autorizacao", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.infRespTec.CNPJ.Substring(12, 2), "@dig_cnpj_cpf_provedor_assinatura_autorizacao", SqlDbType.SmallInt);
                }
                else
                {
                    clsFacil.AdicionarParametro(ref strSQL, string.Empty, "@num_cnpj_cpf_base_provedor_assinatura_autorizacao", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, string.Empty, "@num_cnpj_cpf_filial_provedor_assinatura_autorizacao", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, string.Empty, "@dig_cnpj_cpf_provedor_assinatura_autorizacao", SqlDbType.SmallInt);
                }

                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.infPAA.PAASignature.SignatureValue, "@des_assinatura_rsa", SqlDbType.VarBinary);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.infPAA.PAASignature.RSAKeyValue, "@des_chave_publica_xml_rsa_key", SqlDbType.VarBinary);




                strRetorno = strSQL;
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"[{strMetodo}]: " + ex.Message);
            }
            finally
            {
                stbSQL = null;
                clsDados = null;
                clsFacil = null;
            }

            return strRetorno;
        }

        public string InserirXML_detalhe_xml_conhecimento_transporte_eletronico_simp_autorizado_download(Common.SerializableClasses.CTe.xmlCTeSimp_v400.proc CTe)
        {
            // Classes e variaveis utilizadas
            AcessoDados clsDados = new AcessoDados();
            Facilitador clsFacil = new Facilitador();
            StringBuilder stbSQL = new StringBuilder();
            string strRetorno = string.Empty;
            string strMetodo = System.Reflection.MethodBase.GetCurrentMethod().Name;
            string strSQL = string.Empty;
            string strSQL_aux = string.Empty;

            try
            {
                int Lcont = 0;
                for (Lcont = 0; Lcont <= CTe.CteSimpProc.CTeSimp.infCte.autXML.Count - 1; Lcont++)
                {
                    strSQL_aux = string.Empty;
                    stbSQL.Clear();
                    // Montando query a ser executada                    
                    stbSQL.Append(clsFacil.MontarQuery("INSERT INTO cte.detalhe_xml_conhecimento_transporte_eletronico_simp_autorizado_download "));
                    stbSQL.Append(clsFacil.MontarQuery("( "));
                    stbSQL.Append(clsFacil.MontarQuery("  dtr_referencia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso, "));
                    stbSQL.Append(clsFacil.MontarQuery("  num_cnpj_cpf_base_autorizado, "));
                    stbSQL.Append(clsFacil.MontarQuery("  num_cnpj_cpf_filial_autorizado, "));
                    stbSQL.Append(clsFacil.MontarQuery("  dig_cnpj_cpf_autorizado, "));
                    stbSQL.Append(clsFacil.MontarQuery("  dtc_atualizacao "));
                    stbSQL.Append(clsFacil.MontarQuery(") "));
                    stbSQL.Append(clsFacil.MontarQuery("VALUES "));
                    stbSQL.Append(clsFacil.MontarQuery("( "));
                    stbSQL.Append(clsFacil.MontarQuery("  @dtr_referencia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @num_cnpj_cpf_base_autorizado, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @num_cnpj_cpf_filial_autorizado, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @dig_cnpj_cpf_autorizado, "));
                    stbSQL.Append(clsFacil.MontarQuery("  getdate() "));
                    stbSQL.Append(clsFacil.MontarQuery(") "));
                    strSQL_aux = stbSQL.ToString();

                    // Montando parametros
                    clsDados.LimparParametro();
                    clsFacil.AdicionarParametro(ref strSQL_aux, "20" + CTe.CteSimpProc.protCTe.InfProt.chCTe.Substring(2, 4), "@dtr_referencia", SqlDbType.Int);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.protCTe.InfProt.chCTe, "@cod_chave_acesso", SqlDbType.VarChar);

                    if (CTe.CteSimpProc.CTeSimp.infCte.autXML[Lcont].CPF != null && CTe.CteSimpProc.CTeSimp.infCte.autXML[Lcont].CPF != string.Empty)
                    {
                        clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.CTeSimp.infCte.autXML[Lcont].CPF.Substring(0, 9), "@num_cnpj_cpf_base_autorizado", SqlDbType.VarChar);
                        clsFacil.AdicionarParametro(ref strSQL_aux, "0", "@num_cnpj_cpf_filial_autorizado", SqlDbType.VarChar);
                        clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.CTeSimp.infCte.autXML[Lcont].CPF.Substring(9, 2), "@dig_cnpj_cpf_autorizado", SqlDbType.SmallInt);
                    }
                    if (CTe.CteSimpProc.CTeSimp.infCte.autXML[Lcont].CNPJ != null && CTe.CteSimpProc.CTeSimp.infCte.autXML[Lcont].CNPJ != string.Empty)
                    {
                        clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.CTeSimp.infCte.autXML[Lcont].CNPJ.Substring(0, 8), "@num_cnpj_cpf_base_autorizado", SqlDbType.VarChar);
                        clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.CTeSimp.infCte.autXML[Lcont].CNPJ.Substring(8, 4), "@num_cnpj_cpf_filial_autorizado", SqlDbType.VarChar);
                        clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.CTeSimp.infCte.autXML[Lcont].CNPJ.Substring(12, 2), "@dig_cnpj_cpf_autorizado", SqlDbType.SmallInt);
                    }

                    strSQL += strSQL_aux;

                }

                strRetorno = strSQL;

            }
            catch (Exception ex)
            {
                throw new ArgumentException($"[{strMetodo}]: " + ex.Message);
            }
            finally
            {
                stbSQL = null;
                clsDados = null;
                clsFacil = null;
            }

            return strRetorno;
        }

        public string InserirXML_detalhe_xml_conhecimento_transporte_eletronico_simp_informacao_contribuinte(Common.SerializableClasses.CTe.xmlCTeSimp_v400.proc CTe)
        {
            // Classes e variaveis utilizadas
            AcessoDados clsDados = new AcessoDados();
            Facilitador clsFacil = new Facilitador();
            StringBuilder stbSQL = new StringBuilder();
            string strRetorno = string.Empty;
            string strMetodo = System.Reflection.MethodBase.GetCurrentMethod().Name;
            string strSQL = string.Empty;
            string strSQL_aux = string.Empty;

            try
            {
                int Lcont = 0;
                for (Lcont = 0; Lcont <= CTe.CteSimpProc.CTeSimp.infCte.compl.ObsCont.Count  - 1; Lcont++)
                {
                    strSQL_aux = string.Empty;
                    stbSQL.Clear();
                    // Montando query a ser executada
                    stbSQL.Append(clsFacil.MontarQuery("INSERT INTO cte.detalhe_xml_conhecimento_transporte_eletronico_simp_informacao_contribuinte "));
                    stbSQL.Append(clsFacil.MontarQuery("( "));
                    stbSQL.Append(clsFacil.MontarQuery("  dtr_referencia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso, "));
                    stbSQL.Append(clsFacil.MontarQuery("  seq_detalhe_xml_conhecimento_transporte_eletronico_simp_informacao_contribuinte, "));
                    stbSQL.Append(clsFacil.MontarQuery("  nom_campo_livre_contribuinte, "));
                    stbSQL.Append(clsFacil.MontarQuery("  des_campo_livre_contribuinte, "));
                    stbSQL.Append(clsFacil.MontarQuery("  dtc_atualizacao "));
                    stbSQL.Append(clsFacil.MontarQuery(") "));
                    stbSQL.Append(clsFacil.MontarQuery("VALUES "));
                    stbSQL.Append(clsFacil.MontarQuery("( "));
                    stbSQL.Append(clsFacil.MontarQuery("  @dtr_referencia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @seq_detalhe_xml_conhecimento_transporte_eletronico_simp_informacao_contribuinte, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @nom_campo_livre_contribuinte, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @des_campo_livre_contribuinte, "));
                    stbSQL.Append(clsFacil.MontarQuery("  getdate() "));
                    stbSQL.Append(clsFacil.MontarQuery(") "));
                    strSQL_aux = stbSQL.ToString();

                    // Montando parametros
                    clsDados.LimparParametro();
                    clsFacil.AdicionarParametro(ref strSQL_aux, "20" + CTe.CteSimpProc.protCTe.InfProt.chCTe.Substring(2, 4), "@dtr_referencia", SqlDbType.Int);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.protCTe.InfProt.chCTe, "@cod_chave_acesso", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, Convert.ToString(Lcont + 1), "@seq_detalhe_xml_conhecimento_transporte_eletronico_simp_informacao_contribuinte", SqlDbType.Int);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.CTeSimp.infCte.compl.ObsCont[Lcont].xCampo, "@nom_campo_livre_contribuinte", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.CTeSimp.infCte.compl.ObsCont[Lcont].xTexto, "@des_campo_livre_contribuinte", SqlDbType.VarChar);

                    strSQL += strSQL_aux;

                }

                strRetorno = strSQL;


            }
            catch (Exception ex)
            {
                throw new ArgumentException($"[{strMetodo}]: " + ex.Message);
            }
            finally
            {
                stbSQL = null;
                clsDados = null;
                clsFacil = null;
            }

            return strRetorno;
        }

        public string InserirXML_detalhe_xml_conhecimento_transporte_eletronico_simp_informacao_fisco(Common.SerializableClasses.CTe.xmlCTeSimp_v400.proc CTe)
        {
            // Classes e variaveis utilizadas
            AcessoDados clsDados = new AcessoDados();
            Facilitador clsFacil = new Facilitador();
            StringBuilder stbSQL = new StringBuilder();
            string strRetorno = string.Empty;
            string strMetodo = System.Reflection.MethodBase.GetCurrentMethod().Name;
            string strSQL = string.Empty;
            string strSQL_aux = string.Empty;

            try
            {

                int Lcont = 0;
                for (Lcont = 0; Lcont <= CTe.CteSimpProc.CTeSimp.infCte.compl.ObsFisco.Count - 1; Lcont++)
                {
                    strSQL_aux = string.Empty;
                    stbSQL.Clear();
                    // Montando query a ser executada
                    stbSQL.Append(clsFacil.MontarQuery("INSERT INTO cte.detalhe_xml_conhecimento_transporte_eletronico_simp_informacao_fisco "));
                    stbSQL.Append(clsFacil.MontarQuery("( "));
                    stbSQL.Append(clsFacil.MontarQuery("  dtr_referencia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso, "));
                    stbSQL.Append(clsFacil.MontarQuery("  seq_detalhe_xml_conhecimento_transporte_eletronico_informacao_fisco, "));
                    stbSQL.Append(clsFacil.MontarQuery("  nom_campo_livre_fisco, "));
                    stbSQL.Append(clsFacil.MontarQuery("  des_campo_livre_fisco, "));
                    stbSQL.Append(clsFacil.MontarQuery("  dtc_atualizacao "));
                    stbSQL.Append(clsFacil.MontarQuery(") "));
                    stbSQL.Append(clsFacil.MontarQuery("VALUES "));
                    stbSQL.Append(clsFacil.MontarQuery("( "));
                    stbSQL.Append(clsFacil.MontarQuery("  @dtr_referencia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @seq_detalhe_xml_conhecimento_transporte_eletronico_informacao_fisco, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @nom_campo_livre_fisco, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @des_campo_livre_fisco, "));
                    stbSQL.Append(clsFacil.MontarQuery("  getdate() "));
                    stbSQL.Append(clsFacil.MontarQuery(") "));
                    strSQL_aux = stbSQL.ToString();

                    // Montando parametros
                    clsDados.LimparParametro();
                    clsFacil.AdicionarParametro(ref strSQL_aux, "20" + CTe.CteSimpProc.protCTe.InfProt.chCTe.Substring(2, 4), "@dtr_referencia", SqlDbType.Int);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.protCTe.InfProt.chCTe, "@cod_chave_acesso", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, Convert.ToString(Lcont + 1), "@seq_detalhe_xml_conhecimento_transporte_eletronico_informacao_fisco", SqlDbType.Int);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.CTeSimp.infCte.compl.ObsFisco[Lcont].xCampo, "@nom_campo_livre_fisco", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.CTeSimp.infCte.compl.ObsFisco[Lcont].xTexto, "@des_campo_livre_fisco", SqlDbType.VarChar);
                    strSQL = strSQL + strSQL_aux;

                }

                strRetorno = strSQL;

            }
            catch (Exception ex)
            {
                throw new ArgumentException($"[{strMetodo}]: " + ex.Message);
            }
            finally
            {
                stbSQL = null;
                clsDados = null;
                clsFacil = null;
            }

            return strRetorno;
        }

        public string InserirXML_detalhe_xml_conhecimento_transporte_eletronico_simp_registro_passagem(Common.SerializableClasses.CTe.xmlCTeSimp_v400.proc CTe)
        {
            // Classes e variaveis utilizadas
            AcessoDados clsDados = new AcessoDados();
            Facilitador clsFacil = new Facilitador();
            StringBuilder stbSQL = new StringBuilder();
            string strRetorno = string.Empty;
            string strMetodo = System.Reflection.MethodBase.GetCurrentMethod().Name;
            string strSQL = string.Empty;
            string strSQL_aux = string.Empty;

            try
            {

                int Lcont = 0;
                for (Lcont = 0; Lcont <= CTe.CteSimpProc.CTeSimp.infCte.compl.fluxo.pass.Count - 1; Lcont++)
                {
                    strSQL_aux = string.Empty;
                    stbSQL.Clear();
                    // Montando query a ser executada
                    stbSQL.Append(clsFacil.MontarQuery("INSERT INTO cte.detalhe_xml_conhecimento_transporte_eletronico_simp_registro_passagem "));
                    stbSQL.Append(clsFacil.MontarQuery("( "));
                    stbSQL.Append(clsFacil.MontarQuery("  dtr_referencia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso, "));
                    stbSQL.Append(clsFacil.MontarQuery("  seq_detalhe_xml_conhecimento_transporte_eletronico_simp_registro_passagem, "));
                    stbSQL.Append(clsFacil.MontarQuery("  sig_interno_passagem, "));
                    stbSQL.Append(clsFacil.MontarQuery("  dtc_atualizacao "));
                    stbSQL.Append(clsFacil.MontarQuery(") "));
                    stbSQL.Append(clsFacil.MontarQuery("VALUES "));
                    stbSQL.Append(clsFacil.MontarQuery("( "));
                    stbSQL.Append(clsFacil.MontarQuery("  @dtr_referencia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @seq_detalhe_xml_conhecimento_transporte_eletronico_simp_registro_passagem, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @sig_interno_passagem, "));
                    stbSQL.Append(clsFacil.MontarQuery("  getdate() "));
                    stbSQL.Append(clsFacil.MontarQuery(") "));
                    strSQL_aux = stbSQL.ToString();

                    // Montando parametros
                    clsDados.LimparParametro();
                    clsFacil.AdicionarParametro(ref strSQL_aux, "20" + CTe.CteSimpProc.protCTe.InfProt.chCTe.Substring(2, 4), "@dtr_referencia", SqlDbType.Int);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.protCTe.InfProt.chCTe, "@cod_chave_acesso", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, Convert.ToString(Lcont + 1), "@seq_detalhe_xml_conhecimento_transporte_eletronico_simp_registro_passagem", SqlDbType.SmallInt);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.CTeSimp.infCte.compl.fluxo.pass[Lcont].xPass, "@sig_interno_passagem", SqlDbType.VarChar);
                    strSQL = strSQL + strSQL_aux;

                }

                strRetorno = strSQL;

            }
            catch (Exception ex)
            {
                throw new ArgumentException($"[{strMetodo}]: " + ex.Message);
            }
            finally
            {
                stbSQL = null;
                clsDados = null;
                clsFacil = null;
            }

            return strRetorno;
        }

        public string InserirXML_detalhe_xml_conhecimento_transporte_eletronico_simp_duplicata(Common.SerializableClasses.CTe.xmlCTeSimp_v400.proc CTe)
        {
            // Classes e variaveis utilizadas
            AcessoDados clsDados = new AcessoDados();
            Facilitador clsFacil = new Facilitador();
            StringBuilder stbSQL = new StringBuilder();
            string strRetorno = string.Empty;
            string strMetodo = System.Reflection.MethodBase.GetCurrentMethod().Name;
            string strSQL = string.Empty;
            string strSQL_aux = string.Empty;

            try
            {

                int Lcont = 0;
                for (Lcont = 0; Lcont <= CTe.CteSimpProc.CTeSimp.infCte.cobr.dup.Count - 1; Lcont++)
                {
                    strSQL_aux = string.Empty;
                    stbSQL.Clear();
                    // Montando query a ser executada
                    stbSQL.Append(clsFacil.MontarQuery("INSERT INTO cte.detalhe_xml_conhecimento_transporte_eletronico_simp_duplicata "));
                    stbSQL.Append(clsFacil.MontarQuery("( "));
                    stbSQL.Append(clsFacil.MontarQuery("  dtr_referencia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso, "));
                    stbSQL.Append(clsFacil.MontarQuery("  num_duplicata, "));
                    stbSQL.Append(clsFacil.MontarQuery("  dtc_vencimento, "));
                    stbSQL.Append(clsFacil.MontarQuery("  val_duplicata, "));
                    stbSQL.Append(clsFacil.MontarQuery("  dtc_atualizacao "));
                    stbSQL.Append(clsFacil.MontarQuery(") "));
                    stbSQL.Append(clsFacil.MontarQuery("VALUES "));
                    stbSQL.Append(clsFacil.MontarQuery("( "));
                    stbSQL.Append(clsFacil.MontarQuery("  @dtr_referencia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @num_duplicata, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @dtc_vencimento, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @val_duplicata, "));
                    stbSQL.Append(clsFacil.MontarQuery("  getdate() "));
                    stbSQL.Append(clsFacil.MontarQuery(") "));
                    strSQL_aux = stbSQL.ToString();

                    // Montando parametros
                    clsDados.LimparParametro();
                    clsFacil.AdicionarParametro(ref strSQL_aux, "20" + CTe.CteSimpProc.protCTe.InfProt.chCTe.Substring(2, 4), "@dtr_referencia", SqlDbType.Int);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.protCTe.InfProt.chCTe, "@cod_chave_acesso", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.CTeSimp.infCte.cobr.dup[Lcont].nDup, "@num_duplicata", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.CTeSimp.infCte.cobr.dup[Lcont].dVenc, "@dtc_vencimento", SqlDbType.DateTime);
                    clsFacil.AdicionarParametro(ref strSQL_aux, Convert.ToString(CTe.CteSimpProc.CTeSimp.infCte.cobr.dup[Lcont].vDup), "@val_duplicata", SqlDbType.Decimal);
                    strSQL = strSQL + strSQL_aux;

                }

                strRetorno = strSQL;

            }
            catch (Exception ex)
            {
                throw new ArgumentException($"[{strMetodo}]: " + ex.Message);
            }
            finally
            {
                stbSQL = null;
                clsDados = null;
                clsFacil = null;
            }

            return strRetorno;
        }

        #region " detalhamento "

        public string InserirXML_detalhe_xml_conhecimento_transporte_eletronico_simp_det_entrega(Common.SerializableClasses.CTe.xmlCTeSimp_v400.proc CTe)
        {
            // Classes e variaveis utilizadas
            AcessoDados clsDados = new AcessoDados();
            Facilitador clsFacil = new Facilitador();
            StringBuilder stbSQL = new StringBuilder();
            string strRetorno = string.Empty;
            string strMetodo = System.Reflection.MethodBase.GetCurrentMethod().Name;
            string strSQL = string.Empty;
            string strSQL_aux = string.Empty;

            try
            {

                int Lcont = 0;
                for (Lcont = 0; Lcont <= CTe.CteSimpProc.CTeSimp.infCte.det.Count - 1; Lcont++)
                {
                    strSQL_aux = string.Empty;
                    stbSQL.Clear();
                    // Montando query a ser executada
                    stbSQL.Append(clsFacil.MontarQuery("INSERT INTO cte.detalhe_xml_conhecimento_transporte_eletronico_simp_det_entrega "));
                    stbSQL.Append(clsFacil.MontarQuery("( "));
                    stbSQL.Append(clsFacil.MontarQuery("  dtr_referencia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso, "));
                    stbSQL.Append(clsFacil.MontarQuery("  num_item, "));
                    stbSQL.Append(clsFacil.MontarQuery("  cod_municipio_inicio_prestacao, "));
                    stbSQL.Append(clsFacil.MontarQuery("  nom_municipio_inicio_prestacao, "));
                    stbSQL.Append(clsFacil.MontarQuery("  cod_municipio_fim_prestacao, "));
                    stbSQL.Append(clsFacil.MontarQuery("  nom_municipio_fim_prestacao, "));
                    stbSQL.Append(clsFacil.MontarQuery("  val_prestacao_servico, "));
                    stbSQL.Append(clsFacil.MontarQuery("  val_receber, "));
                    stbSQL.Append(clsFacil.MontarQuery("  dtc_atualizacao "));
                    stbSQL.Append(clsFacil.MontarQuery(") "));
                    stbSQL.Append(clsFacil.MontarQuery("VALUES "));
                    stbSQL.Append(clsFacil.MontarQuery("( "));
                    stbSQL.Append(clsFacil.MontarQuery("  @dtr_referencia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @num_item, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @cod_municipio_inicio_prestacao, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @nom_municipio_inicio_prestacao, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @cod_municipio_fim_prestacao, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @nom_municipio_fim_prestacao, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @val_prestacao_servico, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @val_receber, "));
                    stbSQL.Append(clsFacil.MontarQuery("  getdate() "));
                    stbSQL.Append(clsFacil.MontarQuery(") "));
                    strSQL_aux = stbSQL.ToString();

                    // Montando parametros
                    clsDados.LimparParametro();
                    clsFacil.AdicionarParametro(ref strSQL_aux, "20" + CTe.CteSimpProc.protCTe.InfProt.chCTe.Substring(2, 4), "@dtr_referencia", SqlDbType.Int);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.protCTe.InfProt.chCTe, "@cod_chave_acesso", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.CTeSimp.infCte.det[Lcont].nItem, "@num_item", SqlDbType.SmallInt);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.CTeSimp.infCte.det[Lcont].cMunIni, "@cod_municipio_inicio_prestacao", SqlDbType.Int);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.CTeSimp.infCte.det[Lcont].xMunIni, "@nom_municipio_inicio_prestacao", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.CTeSimp.infCte.det[Lcont].cMunFim, "@cod_municipio_fim_prestacao", SqlDbType.Int);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.CTeSimp.infCte.det[Lcont].xMunFim, "@nom_municipio_fim_prestacao", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, Convert.ToString(CTe.CteSimpProc.CTeSimp.infCte.det[Lcont].vPrest), "@val_prestacao_servico", SqlDbType.Decimal);
                    clsFacil.AdicionarParametro(ref strSQL_aux, Convert.ToString(CTe.CteSimpProc.CTeSimp.infCte.det[Lcont].vRec), "@val_receber", SqlDbType.Decimal);
                    strSQL = strSQL + strSQL_aux;
                }

                strRetorno = strSQL;
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"[{strMetodo}]: " + ex.Message);
            }
            finally
            {
                stbSQL = null;
                clsDados = null;
                clsFacil = null;
            }

            return strRetorno;
        }

        public string InserirXML_detalhe_xml_conhecimento_transporte_eletronico_simp_det_entrega_componente(Common.SerializableClasses.CTe.xmlCTeSimp_v400.proc CTe)
        {
            // Classes e variaveis utilizadas
            AcessoDados clsDados = new AcessoDados();
            Facilitador clsFacil = new Facilitador();
            StringBuilder stbSQL = new StringBuilder();
            string strRetorno = string.Empty;
            string strMetodo = System.Reflection.MethodBase.GetCurrentMethod().Name;
            string strSQL = string.Empty;
            string strSQL_aux = string.Empty;

            try
            {

                int i = 0;
                for (i = 0; i <= CTe.CteSimpProc.CTeSimp.infCte.det.Count - 1; i++)
                {
                    int j = 0;
                    for (j = 0; j <= CTe.CteSimpProc.CTeSimp.infCte.det[i].Comp.Count - 1; j++)
                    {
                        strSQL_aux = string.Empty;
                        stbSQL.Clear();
                        // Montando query a ser executada
                        stbSQL.Append(clsFacil.MontarQuery("INSERT INTO cte.detalhe_xml_conhecimento_transporte_eletronico_simp_det_entrega_componente "));
                        stbSQL.Append(clsFacil.MontarQuery("( "));
                        stbSQL.Append(clsFacil.MontarQuery("  dtr_referencia, "));
                        stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso, "));
                        stbSQL.Append(clsFacil.MontarQuery("  num_item, "));
                        stbSQL.Append(clsFacil.MontarQuery("  seq_detalhe_xml_conhecimento_transporte_eletronico_simp_det_entrega_componente, "));
                        stbSQL.Append(clsFacil.MontarQuery("  nom_componente, "));
                        stbSQL.Append(clsFacil.MontarQuery("  val_componente, "));
                        stbSQL.Append(clsFacil.MontarQuery("  dtc_atualizacao "));
                        stbSQL.Append(clsFacil.MontarQuery(") "));
                        stbSQL.Append(clsFacil.MontarQuery("VALUES "));
                        stbSQL.Append(clsFacil.MontarQuery("( "));
                        stbSQL.Append(clsFacil.MontarQuery("  @dtr_referencia, "));
                        stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso, "));
                        stbSQL.Append(clsFacil.MontarQuery("  @num_item, "));
                        stbSQL.Append(clsFacil.MontarQuery("  @seq_detalhe_xml_conhecimento_transporte_eletronico_simp_det_entrega_componente, "));
                        stbSQL.Append(clsFacil.MontarQuery("  @nom_componente, "));
                        stbSQL.Append(clsFacil.MontarQuery("  @val_componente, "));
                        stbSQL.Append(clsFacil.MontarQuery("  getdate() "));
                        stbSQL.Append(clsFacil.MontarQuery(") "));
                        strSQL_aux = stbSQL.ToString();

                        // Montando parametros
                        clsDados.LimparParametro();
                        clsFacil.AdicionarParametro(ref strSQL_aux, "20" + CTe.CteSimpProc.protCTe.InfProt.chCTe.Substring(2, 4), "@dtr_referencia", SqlDbType.Int);
                        clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.protCTe.InfProt.chCTe, "@cod_chave_acesso", SqlDbType.VarChar);
                        clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.CTeSimp.infCte.det[i].nItem, "@num_item", SqlDbType.SmallInt);
                        clsFacil.AdicionarParametro(ref strSQL_aux, Convert.ToString(j + 1), "@seq_detalhe_xml_conhecimento_transporte_eletronico_simp_det_entrega_componente", SqlDbType.SmallInt);
                        clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.CTeSimp.infCte.det[i].Comp[j].xNome, "@nom_componente", SqlDbType.VarChar);
                        clsFacil.AdicionarParametro(ref strSQL_aux, Convert.ToString(CTe.CteSimpProc.CTeSimp.infCte.det[i].Comp[j].vComp), "@val_componente", SqlDbType.Decimal);
                        strSQL = strSQL + strSQL_aux;

                    }
                }
                strRetorno = strSQL;


            }
            catch (Exception ex)
            {
                throw new ArgumentException($"[{strMetodo}]: " + ex.Message);
            }
            finally
            {
                stbSQL = null;
                clsDados = null;
                clsFacil = null;
            }

            return strRetorno;
        }

        public string InserirXML_detalhe_xml_conhecimento_transporte_eletronico_simp_det_entrega_info_doc_ant(Common.SerializableClasses.CTe.xmlCTeSimp_v400.proc CTe)
        {
            // Classes e variaveis utilizadas
            AcessoDados clsDados = new AcessoDados();
            Facilitador clsFacil = new Facilitador();
            StringBuilder stbSQL = new StringBuilder();
            string strRetorno = string.Empty;
            string strMetodo = System.Reflection.MethodBase.GetCurrentMethod().Name;
            string strSQL = string.Empty;
            string strSQL_aux = string.Empty;

            try
            {

                int i = 0;
                for (i = 0; i <= CTe.CteSimpProc.CTeSimp.infCte.det.Count - 1; i++)
                {
                    int j = 0;
                    for (j = 0; j <= CTe.CteSimpProc.CTeSimp.infCte.det[i].infDocAnt.Count - 1; j++)
                    {
                        strSQL_aux = string.Empty;
                        stbSQL.Clear();
                        // Montando query a ser executada
                        stbSQL.Append(clsFacil.MontarQuery("INSERT INTO cte.detalhe_xml_conhecimento_transporte_eletronico_simp_det_entrega_info_doc_ant "));
                        stbSQL.Append(clsFacil.MontarQuery("( "));
                        stbSQL.Append(clsFacil.MontarQuery("  dtr_referencia, "));
                        stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso, "));
                        stbSQL.Append(clsFacil.MontarQuery("  num_item, "));
                        stbSQL.Append(clsFacil.MontarQuery("  seq_detalhe_xml_conhecimento_transporte_eletronico_simp_det_entrega_info_doc_ant, "));
                        stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso_cte_ant, "));
                        stbSQL.Append(clsFacil.MontarQuery("  cod_tipo_prestacao, "));
                        stbSQL.Append(clsFacil.MontarQuery("  dtc_atualizacao "));
                        stbSQL.Append(clsFacil.MontarQuery(") "));
                        stbSQL.Append(clsFacil.MontarQuery("VALUES "));
                        stbSQL.Append(clsFacil.MontarQuery("( "));
                        stbSQL.Append(clsFacil.MontarQuery("  @dtr_referencia, "));
                        stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso, "));
                        stbSQL.Append(clsFacil.MontarQuery("  @num_item, "));
                        stbSQL.Append(clsFacil.MontarQuery("  @seq_detalhe_xml_conhecimento_transporte_eletronico_simp_det_entrega_info_doc_ant, "));
                        stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso_cte_ant, "));
                        stbSQL.Append(clsFacil.MontarQuery("  @cod_tipo_prestacao, "));
                        stbSQL.Append(clsFacil.MontarQuery("  getdate() "));
                        stbSQL.Append(clsFacil.MontarQuery(") "));
                        strSQL_aux = stbSQL.ToString();

                        // Montando parametros
                        clsDados.LimparParametro();
                        clsFacil.AdicionarParametro(ref strSQL_aux, "20" + CTe.CteSimpProc.protCTe.InfProt.chCTe.Substring(2, 4), "@dtr_referencia", SqlDbType.Int);
                        clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.protCTe.InfProt.chCTe, "@cod_chave_acesso", SqlDbType.VarChar);
                        clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.CTeSimp.infCte.det[i].nItem, "@num_item", SqlDbType.SmallInt);
                        clsFacil.AdicionarParametro(ref strSQL_aux, Convert.ToString(j + 1), "@seq_detalhe_xml_conhecimento_transporte_eletronico_simp_det_entrega_info_doc_ant", SqlDbType.SmallInt);
                        clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.CTeSimp.infCte.det[i].infDocAnt[j].chCTe, "@cod_chave_acesso_cte_ant", SqlDbType.VarChar);
                        clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.CTeSimp.infCte.det[i].infDocAnt[j].tpPrest, "@cod_tipo_prestacao", SqlDbType.SmallInt);
                        strSQL = strSQL + strSQL_aux;
                    }
                }
                strRetorno = strSQL;

            }
            catch (Exception ex)
            {
                throw new ArgumentException($"[{strMetodo}]: " + ex.Message);
            }
            finally
            {
                stbSQL = null;
                clsDados = null;
                clsFacil = null;
            }

            return strRetorno;
        }

        public string InserirXML_detalhe_xml_conhecimento_transporte_eletronico_simp_det_entrega_info_doc_ant_transp_parcial(Common.SerializableClasses.CTe.xmlCTeSimp_v400.proc CTe)
        {
            // Classes e variaveis utilizadas
            AcessoDados clsDados = new AcessoDados();
            Facilitador clsFacil = new Facilitador();
            StringBuilder stbSQL = new StringBuilder();
            string strRetorno = string.Empty;
            string strMetodo = System.Reflection.MethodBase.GetCurrentMethod().Name;
            string strSQL = string.Empty;
            string strSQL_aux = string.Empty;

            try
            {

                int i = 0;
                for (i = 0; i <= CTe.CteSimpProc.CTeSimp.infCte.det.Count - 1; i++)
                {
                    int j = 0;
                    for (j = 0; j <= CTe.CteSimpProc.CTeSimp.infCte.det[i].infDocAnt.Count - 1; j++)
                    {
                        int k = 0;
                        for (k = 0; k <= CTe.CteSimpProc.CTeSimp.infCte.det[i].infDocAnt[j].infNFeTranspParcial.Count - 1; k++)
                        {
                            strSQL_aux = string.Empty;
                            stbSQL.Clear();
                            // Montando query a ser executada
                            stbSQL.Append(clsFacil.MontarQuery("INSERT INTO cte.detalhe_xml_conhecimento_transporte_eletronico_simp_det_entrega_info_doc_ant_transp_parcial "));
                            stbSQL.Append(clsFacil.MontarQuery("( "));
                            stbSQL.Append(clsFacil.MontarQuery("  dtr_referencia, "));
                            stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso, "));
                            stbSQL.Append(clsFacil.MontarQuery("  num_item, "));
                            stbSQL.Append(clsFacil.MontarQuery("  seq_detalhe_xml_conhecimento_transporte_eletronico_simp_det_entrega_info_doc_ant, "));
                            stbSQL.Append(clsFacil.MontarQuery("  seq_detalhe_xml_conhecimento_transporte_eletronico_simp_det_entrega_info_doc_ant_transp_parcial, "));
                            stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso_nfe, "));
                            stbSQL.Append(clsFacil.MontarQuery("  dtc_atualizacao "));
                            stbSQL.Append(clsFacil.MontarQuery(") "));
                            stbSQL.Append(clsFacil.MontarQuery("VALUES "));
                            stbSQL.Append(clsFacil.MontarQuery("( "));
                            stbSQL.Append(clsFacil.MontarQuery("  @dtr_referencia, "));
                            stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso, "));
                            stbSQL.Append(clsFacil.MontarQuery("  @num_item, "));
                            stbSQL.Append(clsFacil.MontarQuery("  @seq_detalhe_xml_conhecimento_transporte_eletronico_simp_det_entrega_info_doc_ant, "));
                            stbSQL.Append(clsFacil.MontarQuery("  @seq_detalhe_xml_conhecimento_transporte_eletronico_simp_det_entrega_info_doc_ant_transp_parcial, "));
                            stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso_nfe, "));
                            stbSQL.Append(clsFacil.MontarQuery("  getdate() "));
                            stbSQL.Append(clsFacil.MontarQuery(") "));
                            strSQL_aux = stbSQL.ToString();

                            // Montando parametros
                            clsDados.LimparParametro();
                            clsFacil.AdicionarParametro(ref strSQL_aux, "20" + CTe.CteSimpProc.protCTe.InfProt.chCTe.Substring(2, 4), "@dtr_referencia", SqlDbType.Int);
                            clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.protCTe.InfProt.chCTe, "@cod_chave_acesso", SqlDbType.VarChar);
                            clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.CTeSimp.infCte.det[i].nItem, "@num_item", SqlDbType.SmallInt);
                            clsFacil.AdicionarParametro(ref strSQL_aux, Convert.ToString(j + 1), "@seq_detalhe_xml_conhecimento_transporte_eletronico_simp_det_entrega_info_doc_ant", SqlDbType.SmallInt);
                            clsFacil.AdicionarParametro(ref strSQL_aux, Convert.ToString(k + 1), "@seq_detalhe_xml_conhecimento_transporte_eletronico_simp_det_entrega_info_doc_ant_transp_parcial", SqlDbType.SmallInt);
                            clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.CTeSimp.infCte.det[i].infDocAnt[j].infNFeTranspParcial[k].chNFe, "@cod_chave_acesso_nfe", SqlDbType.VarChar);
                            strSQL = strSQL + strSQL_aux;
                        }
                    }
                }
                strRetorno = strSQL;


            }
            catch (Exception ex)
            {
                throw new ArgumentException($"[{strMetodo}]: " + ex.Message);
            }
            finally
            {
                stbSQL = null;
                clsDados = null;
                clsFacil = null;
            }

            return strRetorno;
        }

        public string InserirXML_detalhe_xml_conhecimento_transporte_eletronico_simp_det_entrega_info_nfe(Common.SerializableClasses.CTe.xmlCTeSimp_v400.proc CTe)
        {
            // Classes e variaveis utilizadas
            AcessoDados clsDados = new AcessoDados();
            Facilitador clsFacil = new Facilitador();
            StringBuilder stbSQL = new StringBuilder();
            string strRetorno = string.Empty;
            string strMetodo = System.Reflection.MethodBase.GetCurrentMethod().Name;
            string strSQL = string.Empty;
            string strSQL_aux = string.Empty;

            try
            {

                int i = 0;
                for (i = 0; i <= CTe.CteSimpProc.CTeSimp.infCte.det.Count - 1; i++)
                {
                    int j = 0;
                    for (j = 0; j <= CTe.CteSimpProc.CTeSimp.infCte.det[i].infNFe.Count - 1; j++)
                    {
                        strSQL_aux = string.Empty;
                        stbSQL.Clear();
                        // Montando query a ser executada
                        stbSQL.Append(clsFacil.MontarQuery("INSERT INTO cte.detalhe_xml_conhecimento_transporte_eletronico_simp_det_entrega_info_nfe "));
                        stbSQL.Append(clsFacil.MontarQuery("( "));
                        stbSQL.Append(clsFacil.MontarQuery("  dtr_referencia, "));
                        stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso, "));
                        stbSQL.Append(clsFacil.MontarQuery("  num_item, "));
                        stbSQL.Append(clsFacil.MontarQuery("  seq_detalhe_xml_conhecimento_transporte_eletronico_simp_det_entrega_info_nfe, "));
                        stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso_nfe, "));
                        stbSQL.Append(clsFacil.MontarQuery("  num_pin_suframa, "));
                        stbSQL.Append(clsFacil.MontarQuery("  dtc_entrega, "));
                        stbSQL.Append(clsFacil.MontarQuery("  dtc_atualizacao "));
                        stbSQL.Append(clsFacil.MontarQuery(") "));
                        stbSQL.Append(clsFacil.MontarQuery("VALUES "));
                        stbSQL.Append(clsFacil.MontarQuery("( "));
                        stbSQL.Append(clsFacil.MontarQuery("  @dtr_referencia, "));
                        stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso, "));
                        stbSQL.Append(clsFacil.MontarQuery("  @num_item, "));
                        stbSQL.Append(clsFacil.MontarQuery("  @seq_detalhe_xml_conhecimento_transporte_eletronico_simp_det_entrega_info_nfe, "));
                        stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso_nfe, "));
                        stbSQL.Append(clsFacil.MontarQuery("  @num_pin_suframa, "));
                        stbSQL.Append(clsFacil.MontarQuery("  @dtc_entrega, "));
                        stbSQL.Append(clsFacil.MontarQuery("  getdate() "));
                        stbSQL.Append(clsFacil.MontarQuery(") "));
                        strSQL_aux = stbSQL.ToString();

                        // Montando parametros
                        clsDados.LimparParametro();
                        clsFacil.AdicionarParametro(ref strSQL_aux, "20" + CTe.CteSimpProc.protCTe.InfProt.chCTe.Substring(2, 4), "@dtr_referencia", SqlDbType.Int);
                        clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.protCTe.InfProt.chCTe, "@cod_chave_acesso", SqlDbType.VarChar);
                        clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.CTeSimp.infCte.det[i].nItem, "@num_item", SqlDbType.SmallInt);
                        clsFacil.AdicionarParametro(ref strSQL_aux, Convert.ToString(j + 1), "@seq_detalhe_xml_conhecimento_transporte_eletronico_simp_det_entrega_info_nfe", SqlDbType.Int);
                        clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.CTeSimp.infCte.det[i].infNFe[j].chNFe, "@cod_chave_acesso_nfe", SqlDbType.VarChar);
                        clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.CTeSimp.infCte.det[i].infNFe[j].PIN, "@num_pin_suframa", SqlDbType.VarChar);
                        clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.CTeSimp.infCte.det[i].infNFe[j].dPrev, "@dtc_entrega", SqlDbType.DateTime);
                        strSQL = strSQL + strSQL_aux;
                    }
                }

                strRetorno = strSQL;


            }
            catch (Exception ex)
            {
                throw new ArgumentException($"[{strMetodo}]: " + ex.Message);
            }
            finally
            {
                stbSQL = null;
                clsDados = null;
                clsFacil = null;
            }

            return strRetorno;
        }

        public string InserirXML_detalhe_xml_conhecimento_transporte_eletronico_simp_det_entrega_info_nfe_unidade_carga(Common.SerializableClasses.CTe.xmlCTeSimp_v400.proc CTe)
        {
            // Classes e variaveis utilizadas
            AcessoDados clsDados = new AcessoDados();
            Facilitador clsFacil = new Facilitador();
            StringBuilder stbSQL = new StringBuilder();
            string strRetorno = string.Empty;
            string strMetodo = System.Reflection.MethodBase.GetCurrentMethod().Name;
            string strSQL = string.Empty;
            string strSQL_aux = string.Empty;

            try
            {

                int i = 0;
                for (i = 0; i <= CTe.CteSimpProc.CTeSimp.infCte.det.Count - 1; i++)
                {
                    int j = 0;
                    for (j = 0; j <= CTe.CteSimpProc.CTeSimp.infCte.det[i].infNFe.Count - 1; j++)
                    {
                        int k = 0;
                        for (k = 0; k <= CTe.CteSimpProc.CTeSimp.infCte.det[i].infNFe[j].infUnidCarga.Count - 1; k++)
                        {
                            strSQL_aux = string.Empty;
                            stbSQL.Clear();
                            // Montando query a ser executada
                            stbSQL.Append(clsFacil.MontarQuery("INSERT INTO cte.detalhe_xml_conhecimento_transporte_eletronico_simp_det_entrega_info_nfe_unidade_carga "));
                            stbSQL.Append(clsFacil.MontarQuery("( "));
                            stbSQL.Append(clsFacil.MontarQuery("  dtr_referencia, "));
                            stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso, "));
                            stbSQL.Append(clsFacil.MontarQuery("  num_item, "));
                            stbSQL.Append(clsFacil.MontarQuery("  seq_detalhe_xml_conhecimento_transporte_eletronico_simp_det_entrega_info_nfe, "));
                            stbSQL.Append(clsFacil.MontarQuery("  num_identificacao_unidade_carga, "));
                            stbSQL.Append(clsFacil.MontarQuery("  cod_tipo_unidade_carga_documento_fiscal_eletronico, "));
                            stbSQL.Append(clsFacil.MontarQuery("  qtd_rateada_unidade_carga, "));
                            stbSQL.Append(clsFacil.MontarQuery("  dtc_atualizacao "));
                            stbSQL.Append(clsFacil.MontarQuery(") "));
                            stbSQL.Append(clsFacil.MontarQuery("VALUES "));
                            stbSQL.Append(clsFacil.MontarQuery("( "));
                            stbSQL.Append(clsFacil.MontarQuery("  @dtr_referencia, "));
                            stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso, "));
                            stbSQL.Append(clsFacil.MontarQuery("  @num_item, "));
                            stbSQL.Append(clsFacil.MontarQuery("  @seq_detalhe_xml_conhecimento_transporte_eletronico_simp_det_entrega_info_nfe, "));
                            stbSQL.Append(clsFacil.MontarQuery("  @num_identificacao_unidade_carga, "));
                            stbSQL.Append(clsFacil.MontarQuery("  @cod_tipo_unidade_carga_documento_fiscal_eletronico, "));
                            stbSQL.Append(clsFacil.MontarQuery("  @qtd_rateada_unidade_carga, "));
                            stbSQL.Append(clsFacil.MontarQuery("  getdate() "));
                            stbSQL.Append(clsFacil.MontarQuery(") "));
                            strSQL_aux = stbSQL.ToString();

                            // Montando parametros
                            clsDados.LimparParametro();
                            clsFacil.AdicionarParametro(ref strSQL_aux, "20" + CTe.CteSimpProc.protCTe.InfProt.chCTe.Substring(2, 4), "@dtr_referencia", SqlDbType.Int);
                            clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.protCTe.InfProt.chCTe, "@cod_chave_acesso", SqlDbType.VarChar);
                            clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.CTeSimp.infCte.det[i].nItem, "@num_item", SqlDbType.SmallInt);
                            clsFacil.AdicionarParametro(ref strSQL_aux, Convert.ToString(j + 1), "@seq_detalhe_xml_conhecimento_transporte_eletronico_simp_det_entrega_info_nfe", SqlDbType.Int);
                            clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.CTeSimp.infCte.det[i].infNFe[j].infUnidCarga[k].idUnidCarga, "@num_identificacao_unidade_carga", SqlDbType.VarChar);
                            clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.CTeSimp.infCte.det[i].infNFe[j].infUnidCarga[k].tpUnidCarga, "@cod_tipo_unidade_carga_documento_fiscal_eletronico", SqlDbType.SmallInt);
                            clsFacil.AdicionarParametro(ref strSQL_aux, Convert.ToString(CTe.CteSimpProc.CTeSimp.infCte.det[i].infNFe[j].infUnidCarga[k].qtdRat), "@qtd_rateada_unidade_carga", SqlDbType.Decimal);
                            strSQL = strSQL + strSQL_aux;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw new ArgumentException($"[{strMetodo}]: " + ex.Message);
            }
            finally
            {
                stbSQL = null;
                clsDados = null;
                clsFacil = null;
            }

            return strRetorno;
        }

        public string InserirXML_detalhe_xml_conhecimento_transporte_eletronico_simp_det_entrega_info_nfe_unidade_carga_lacre(Common.SerializableClasses.CTe.xmlCTeSimp_v400.proc CTe)
        {
            // Classes e variaveis utilizadas
            AcessoDados clsDados = new AcessoDados();
            Facilitador clsFacil = new Facilitador();
            StringBuilder stbSQL = new StringBuilder();
            string strRetorno = string.Empty;
            string strMetodo = System.Reflection.MethodBase.GetCurrentMethod().Name;
            string strSQL = string.Empty;
            string strSQL_aux = string.Empty;

            try
            {

                int i = 0;
                for (i = 0; i <= CTe.CteSimpProc.CTeSimp.infCte.det.Count - 1; i++)
                {
                    int j = 0;
                    for (j = 0; j <= CTe.CteSimpProc.CTeSimp.infCte.det[i].infNFe.Count - 1; j++)
                    {
                        int k = 0;
                        for (k = 0; k <= CTe.CteSimpProc.CTeSimp.infCte.det[i].infNFe[j].infUnidCarga.Count - 1; k++)
                        {
                            int l = 0;
                            for (l = 0; l <= CTe.CteSimpProc.CTeSimp.infCte.det[i].infNFe[j].infUnidCarga[k].lacUnidCarga.Count - 1; l++)
                            {
                                strSQL_aux = string.Empty;
                                stbSQL.Clear();
                                // Montando query a ser executada
                                stbSQL.Append(clsFacil.MontarQuery("INSERT INTO cte.detalhe_xml_conhecimento_transporte_eletronico_simp_det_entrega_info_nfe_unidade_carga_lacre "));
                                stbSQL.Append(clsFacil.MontarQuery("( "));
                                stbSQL.Append(clsFacil.MontarQuery("  dtr_referencia, "));
                                stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso, "));
                                stbSQL.Append(clsFacil.MontarQuery("  num_item, "));
                                stbSQL.Append(clsFacil.MontarQuery("  seq_detalhe_xml_conhecimento_transporte_eletronico_simp_det_entrega_info_nfe, "));
                                stbSQL.Append(clsFacil.MontarQuery("  num_identificacao_unidade_carga, "));
                                stbSQL.Append(clsFacil.MontarQuery("  num_lacre_unidade_carga, "));
                                stbSQL.Append(clsFacil.MontarQuery("  dtc_atualizacao "));
                                stbSQL.Append(clsFacil.MontarQuery(") "));
                                stbSQL.Append(clsFacil.MontarQuery("VALUES "));
                                stbSQL.Append(clsFacil.MontarQuery("( "));
                                stbSQL.Append(clsFacil.MontarQuery("  @dtr_referencia, "));
                                stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso, "));
                                stbSQL.Append(clsFacil.MontarQuery("  @num_item, "));
                                stbSQL.Append(clsFacil.MontarQuery("  @seq_detalhe_xml_conhecimento_transporte_eletronico_simp_det_entrega_info_nfe, "));
                                stbSQL.Append(clsFacil.MontarQuery("  @num_identificacao_unidade_carga, "));
                                stbSQL.Append(clsFacil.MontarQuery("  @num_lacre_unidade_carga, "));
                                stbSQL.Append(clsFacil.MontarQuery("  getdate() "));
                                stbSQL.Append(clsFacil.MontarQuery(") "));
                                strSQL_aux = stbSQL.ToString();

                                // Montando parametros
                                clsDados.LimparParametro();
                                clsFacil.AdicionarParametro(ref strSQL_aux, "20" + CTe.CteSimpProc.protCTe.InfProt.chCTe.Substring(2, 4), "@dtr_referencia", SqlDbType.Int);
                                clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.protCTe.InfProt.chCTe, "@cod_chave_acesso", SqlDbType.VarChar);
                                clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.CTeSimp.infCte.det[i].nItem, "@num_item", SqlDbType.SmallInt);
                                clsFacil.AdicionarParametro(ref strSQL_aux, Convert.ToString(j + 1), "@seq_detalhe_xml_conhecimento_transporte_eletronico_simp_det_entrega_info_nfe", SqlDbType.Int);
                                clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.CTeSimp.infCte.det[i].infNFe[j].infUnidCarga[k].idUnidCarga, "@num_identificacao_unidade_carga", SqlDbType.VarChar);
                                clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.CTeSimp.infCte.det[i].infNFe[j].infUnidCarga[k].lacUnidCarga[l].nLacre, "@num_lacre_unidade_carga", SqlDbType.VarChar);
                                strSQL = strSQL + strSQL_aux;
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw new ArgumentException($"[{strMetodo}]: " + ex.Message);
            }
            finally
            {
                stbSQL = null;
                clsDados = null;
                clsFacil = null;
            }

            return strRetorno;
        }

        public string InserirXML_detalhe_xml_conhecimento_transporte_eletronico_simp_det_entrega_info_nfe_unidade_transporte(Common.SerializableClasses.CTe.xmlCTeSimp_v400.proc CTe)
        {
            // Classes e variaveis utilizadas
            AcessoDados clsDados = new AcessoDados();
            Facilitador clsFacil = new Facilitador();
            StringBuilder stbSQL = new StringBuilder();
            string strRetorno = string.Empty;
            string strMetodo = System.Reflection.MethodBase.GetCurrentMethod().Name;
            string strSQL = string.Empty;
            string strSQL_aux = string.Empty;

            try
            {

                int i = 0;
                for (i = 0; i <= CTe.CteSimpProc.CTeSimp.infCte.det.Count - 1; i++)
                {
                    int j = 0;
                    for (j = 0; j <= CTe.CteSimpProc.CTeSimp.infCte.det[i].infNFe.Count - 1; j++)
                    {
                        int k = 0;
                        for (k = 0; k <= CTe.CteSimpProc.CTeSimp.infCte.det[i].infNFe[j].infUnidTransp.Count - 1; k++)
                        {
                            strSQL_aux = string.Empty;
                            stbSQL.Clear();
                            // Montando query a ser executada
                            stbSQL.Append(clsFacil.MontarQuery("INSERT INTO cte.detalhe_xml_conhecimento_transporte_eletronico_simp_det_entrega_info_nfe_unidade_transporte "));
                            stbSQL.Append(clsFacil.MontarQuery("( "));
                            stbSQL.Append(clsFacil.MontarQuery("  dtr_referencia, "));
                            stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso, "));
                            stbSQL.Append(clsFacil.MontarQuery("  num_item, "));
                            stbSQL.Append(clsFacil.MontarQuery("  seq_detalhe_xml_conhecimento_transporte_eletronico_simp_det_entrega_info_nfe, "));
                            stbSQL.Append(clsFacil.MontarQuery("  num_identificacao_unidade_transporte, "));
                            stbSQL.Append(clsFacil.MontarQuery("  cod_tipo_unidade_transporte_documento_fiscal_eletronico, "));
                            stbSQL.Append(clsFacil.MontarQuery("  qtd_rateada_unidade_carga, "));
                            stbSQL.Append(clsFacil.MontarQuery("  dtc_atualizacao "));
                            stbSQL.Append(clsFacil.MontarQuery(") "));
                            stbSQL.Append(clsFacil.MontarQuery("VALUES "));
                            stbSQL.Append(clsFacil.MontarQuery("( "));
                            stbSQL.Append(clsFacil.MontarQuery("  @dtr_referencia, "));
                            stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso, "));
                            stbSQL.Append(clsFacil.MontarQuery("  @num_item, "));
                            stbSQL.Append(clsFacil.MontarQuery("  @seq_detalhe_xml_conhecimento_transporte_eletronico_simp_det_entrega_info_nfe, "));
                            stbSQL.Append(clsFacil.MontarQuery("  @num_identificacao_unidade_transporte, "));
                            stbSQL.Append(clsFacil.MontarQuery("  @cod_tipo_unidade_transporte_documento_fiscal_eletronico, "));
                            stbSQL.Append(clsFacil.MontarQuery("  @qtd_rateada_unidade_carga, "));
                            stbSQL.Append(clsFacil.MontarQuery("  getdate() "));
                            stbSQL.Append(clsFacil.MontarQuery(") "));
                            strSQL_aux = stbSQL.ToString();

                            // Montando parametros
                            clsFacil.AdicionarParametro(ref strSQL_aux, "20" + CTe.CteSimpProc.protCTe.InfProt.chCTe.Substring(2, 4), "@dtr_referencia", SqlDbType.Int);
                            clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.protCTe.InfProt.chCTe, "@cod_chave_acesso", SqlDbType.VarChar);
                            clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.CTeSimp.infCte.det[i].nItem, "@num_item", SqlDbType.SmallInt);
                            clsFacil.AdicionarParametro(ref strSQL_aux, Convert.ToString(j + 1), "@seq_detalhe_xml_conhecimento_transporte_eletronico_simp_det_entrega_info_nfe", SqlDbType.Int);
                            clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.CTeSimp.infCte.det[i].infNFe[j].infUnidTransp[k].idUnidTransp, "@num_identificacao_unidade_transporte", SqlDbType.VarChar);
                            clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.CTeSimp.infCte.det[i].infNFe[j].infUnidTransp[k].tpUnidTransp, "@cod_tipo_unidade_transporte_documento_fiscal_eletronico", SqlDbType.SmallInt);
                            clsFacil.AdicionarParametro(ref strSQL_aux, Convert.ToString(CTe.CteSimpProc.CTeSimp.infCte.det[i].infNFe[j].infUnidTransp[k].qtdRat), "@qtd_rateada_unidade_carga", SqlDbType.Decimal);
                            strSQL = strSQL + strSQL_aux;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw new ArgumentException($"[{strMetodo}]: " + ex.Message);
            }
            finally
            {
                stbSQL = null;
                clsDados = null;
                clsFacil = null;
            }

            return strRetorno;
        }

        public string InserirXML_detalhe_xml_conhecimento_transporte_eletronico_simp_det_entrega_info_nfe_unidade_transporte_lacre(Common.SerializableClasses.CTe.xmlCTeSimp_v400.proc CTe)
        {
            // Classes e variaveis utilizadas
            AcessoDados clsDados = new AcessoDados();
            Facilitador clsFacil = new Facilitador();
            StringBuilder stbSQL = new StringBuilder();
            string strRetorno = string.Empty;
            string strMetodo = System.Reflection.MethodBase.GetCurrentMethod().Name;
            string strSQL = string.Empty;
            string strSQL_aux = string.Empty;

            try
            {

                int i = 0;
                for (i = 0; i <= CTe.CteSimpProc.CTeSimp.infCte.det.Count - 1; i++)
                {
                    int j = 0;
                    for (j = 0; j <= CTe.CteSimpProc.CTeSimp.infCte.det[i].infNFe.Count - 1; j++)
                    {
                        int k = 0;
                        for (k = 0; k <= CTe.CteSimpProc.CTeSimp.infCte.det[i].infNFe[j].infUnidTransp.Count - 1; k++)
                        {
                            int l = 0;
                            for (l = 0; l <= CTe.CteSimpProc.CTeSimp.infCte.det[i].infNFe[j].infUnidTransp[k].lacUnidTransp.Count - 1; l++)
                            {
                                strSQL_aux = string.Empty;
                                stbSQL.Clear();
                                // Montando query a ser executada
                                stbSQL.Append(clsFacil.MontarQuery("INSERT INTO cte.detalhe_xml_conhecimento_transporte_eletronico_simp_det_entrega_info_nfe_unidade_transporte_lacre "));
                                stbSQL.Append(clsFacil.MontarQuery("( "));
                                stbSQL.Append(clsFacil.MontarQuery("  dtr_referencia, "));
                                stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso, "));
                                stbSQL.Append(clsFacil.MontarQuery("  num_item, "));
                                stbSQL.Append(clsFacil.MontarQuery("  seq_detalhe_xml_conhecimento_transporte_eletronico_simp_det_entrega_info_nfe, "));
                                stbSQL.Append(clsFacil.MontarQuery("  num_identificacao_unidade_transporte, "));
                                stbSQL.Append(clsFacil.MontarQuery("  num_lacre_unidade_transporte, "));
                                stbSQL.Append(clsFacil.MontarQuery("  dtc_atualizacao "));
                                stbSQL.Append(clsFacil.MontarQuery(") "));
                                stbSQL.Append(clsFacil.MontarQuery("VALUES "));
                                stbSQL.Append(clsFacil.MontarQuery("( "));
                                stbSQL.Append(clsFacil.MontarQuery("  @dtr_referencia, "));
                                stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso, "));
                                stbSQL.Append(clsFacil.MontarQuery("  @num_item, "));
                                stbSQL.Append(clsFacil.MontarQuery("  @seq_detalhe_xml_conhecimento_transporte_eletronico_simp_det_entrega_info_nfe, "));
                                stbSQL.Append(clsFacil.MontarQuery("  @num_identificacao_unidade_transporte, "));
                                stbSQL.Append(clsFacil.MontarQuery("  @num_lacre_unidade_transporte, "));
                                stbSQL.Append(clsFacil.MontarQuery("  getdate() "));
                                stbSQL.Append(clsFacil.MontarQuery(") "));
                                strSQL_aux = stbSQL.ToString();

                                // Montando parametros
                                clsDados.LimparParametro();
                                clsFacil.AdicionarParametro(ref strSQL_aux, "20" + CTe.CteSimpProc.protCTe.InfProt.chCTe.Substring(2, 4), "@dtr_referencia", SqlDbType.Int);
                                clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.protCTe.InfProt.chCTe, "@cod_chave_acesso", SqlDbType.VarChar);
                                clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.CTeSimp.infCte.det[i].nItem, "@num_item", SqlDbType.SmallInt);
                                clsFacil.AdicionarParametro(ref strSQL_aux, Convert.ToString(j + 1), "@seq_detalhe_xml_conhecimento_transporte_eletronico_simp_det_entrega_info_nfe", SqlDbType.Int);
                                clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.CTeSimp.infCte.det[i].infNFe[j].infUnidTransp[k].idUnidTransp, "@num_identificacao_unidade_transporte", SqlDbType.VarChar);
                                clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.CTeSimp.infCte.det[i].infNFe[j].infUnidTransp[k].lacUnidTransp[l].nLacre, "@num_lacre_unidade_transporte", SqlDbType.VarChar);
                                strSQL = strSQL + strSQL_aux;
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw new ArgumentException($"[{strMetodo}]: " + ex.Message);
            }
            finally
            {
                stbSQL = null;
                clsDados = null;
                clsFacil = null;
            }

            return strRetorno;
        }

        public string InserirXML_detalhe_xml_conhecimento_transporte_eletronico_simp_det_entrega_info_nfe_unidade_transporte_unidade_carga(Common.SerializableClasses.CTe.xmlCTeSimp_v400.proc CTe)
        {
            // Classes e variaveis utilizadas
            AcessoDados clsDados = new AcessoDados();
            Facilitador clsFacil = new Facilitador();
            StringBuilder stbSQL = new StringBuilder();
            string strRetorno = string.Empty;
            string strMetodo = System.Reflection.MethodBase.GetCurrentMethod().Name;
            string strSQL = string.Empty;
            string strSQL_aux = string.Empty;

            try
            {

                int i = 0;
                for (i = 0; i <= CTe.CteSimpProc.CTeSimp.infCte.det.Count - 1; i++)
                {
                    int j = 0;
                    for (j = 0; j <= CTe.CteSimpProc.CTeSimp.infCte.det[i].infNFe.Count - 1; j++)
                    {
                        int k = 0;
                        for (k = 0; k <= CTe.CteSimpProc.CTeSimp.infCte.det[i].infNFe[j].infUnidTransp.Count - 1; k++)
                        {
                            int l = 0;
                            for (l = 0; l <= CTe.CteSimpProc.CTeSimp.infCte.det[i].infNFe[j].infUnidTransp[k].infUnidCarga.Count - 1; l++)
                            {
                                strSQL_aux = string.Empty;
                                stbSQL.Clear();
                                // Montando query a ser executada
                                stbSQL.Append(clsFacil.MontarQuery("INSERT INTO cte.detalhe_xml_conhecimento_transporte_eletronico_simp_det_entrega_info_nfe_unidade_transporte_unidade_carga "));
                                stbSQL.Append(clsFacil.MontarQuery("( "));
                                stbSQL.Append(clsFacil.MontarQuery("  dtr_referencia, "));
                                stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso, "));
                                stbSQL.Append(clsFacil.MontarQuery("  num_item, "));
                                stbSQL.Append(clsFacil.MontarQuery("  seq_detalhe_xml_conhecimento_transporte_eletronico_simp_det_entrega_info_nfe, "));
                                stbSQL.Append(clsFacil.MontarQuery("  num_identificacao_unidade_transporte, "));
                                stbSQL.Append(clsFacil.MontarQuery("  num_identificacao_unidade_carga, "));
                                stbSQL.Append(clsFacil.MontarQuery("  cod_tipo_unidade_carga_documento_fiscal_eletronico, "));
                                stbSQL.Append(clsFacil.MontarQuery("  qtd_rateada_unidade_carga, "));
                                stbSQL.Append(clsFacil.MontarQuery("  dtc_atualizacao "));
                                stbSQL.Append(clsFacil.MontarQuery(") "));
                                stbSQL.Append(clsFacil.MontarQuery("VALUES "));
                                stbSQL.Append(clsFacil.MontarQuery("( "));
                                stbSQL.Append(clsFacil.MontarQuery("  @dtr_referencia, "));
                                stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso, "));
                                stbSQL.Append(clsFacil.MontarQuery("  @num_item, "));
                                stbSQL.Append(clsFacil.MontarQuery("  @seq_detalhe_xml_conhecimento_transporte_eletronico_simp_det_entrega_info_nfe, "));
                                stbSQL.Append(clsFacil.MontarQuery("  @num_identificacao_unidade_transporte, "));
                                stbSQL.Append(clsFacil.MontarQuery("  @num_identificacao_unidade_carga, "));
                                stbSQL.Append(clsFacil.MontarQuery("  @cod_tipo_unidade_carga_documento_fiscal_eletronico, "));
                                stbSQL.Append(clsFacil.MontarQuery("  @qtd_rateada_unidade_carga, "));
                                stbSQL.Append(clsFacil.MontarQuery("  getdate() "));
                                stbSQL.Append(clsFacil.MontarQuery(") "));
                                strSQL_aux = stbSQL.ToString();

                                // Montando parametros
                                clsDados.LimparParametro();
                                clsFacil.AdicionarParametro(ref strSQL_aux, "20" + CTe.CteSimpProc.protCTe.InfProt.chCTe.Substring(2, 4), "@dtr_referencia", SqlDbType.Int);
                                clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.protCTe.InfProt.chCTe, "@cod_chave_acesso", SqlDbType.VarChar);
                                clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.CTeSimp.infCte.det[i].nItem, "@num_item", SqlDbType.SmallInt);
                                clsFacil.AdicionarParametro(ref strSQL_aux, Convert.ToString(j + 1), "@seq_detalhe_xml_conhecimento_transporte_eletronico_simp_det_entrega_info_nfe", SqlDbType.Int);
                                clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.CTeSimp.infCte.det[i].infNFe[j].infUnidTransp[k].idUnidTransp, "@num_identificacao_unidade_transporte", SqlDbType.VarChar);
                                clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.CTeSimp.infCte.det[i].infNFe[j].infUnidTransp[k].infUnidCarga[l].idUnidCarga, "@num_identificacao_unidade_carga", SqlDbType.VarChar);
                                clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.CTeSimp.infCte.det[i].infNFe[j].infUnidTransp[k].infUnidCarga[l].tpUnidCarga, "@cod_tipo_unidade_carga_documento_fiscal_eletronico", SqlDbType.SmallInt);
                                clsFacil.AdicionarParametro(ref strSQL_aux, Convert.ToString(CTe.CteSimpProc.CTeSimp.infCte.det[i].infNFe[j].infUnidTransp[k].infUnidCarga[l].qtdRat), "@qtd_rateada_unidade_carga", SqlDbType.Decimal);
                                strSQL = strSQL + strSQL_aux;
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw new ArgumentException($"[{strMetodo}]: " + ex.Message);
            }
            finally
            {
                stbSQL = null;
                clsDados = null;
                clsFacil = null;
            }

            return strRetorno;
        }


        public string InserirXML_detalhe_xml_conhecimento_transporte_eletronico_simp_det_entrega_info_nfe_unidade_transporte_unidade_carga_lacre(Common.SerializableClasses.CTe.xmlCTeSimp_v400.proc CTe)
        {
            // Classes e variaveis utilizadas
            AcessoDados clsDados = new AcessoDados();
            Facilitador clsFacil = new Facilitador();
            StringBuilder stbSQL = new StringBuilder();
            string strRetorno = string.Empty;
            string strMetodo = System.Reflection.MethodBase.GetCurrentMethod().Name;

            string strSQL = string.Empty;
            string strSQL_aux = string.Empty;

            try
            {

                int i = 0;
                for (i = 0; i <= CTe.CteSimpProc.CTeSimp.infCte.det.Count - 1; i++)
                {
                    int j = 0;
                    for (j = 0; j <= CTe.CteSimpProc.CTeSimp.infCte.det[i].infNFe.Count - 1; j++)
                    {
                        int k = 0;
                        for (k = 0; k <= CTe.CteSimpProc.CTeSimp.infCte.det[i].infNFe[j].infUnidTransp.Count - 1; k++)
                        {
                            int l = 0;
                            for (l = 0; l <= CTe.CteSimpProc.CTeSimp.infCte.det[i].infNFe[j].infUnidTransp[k].infUnidCarga.Count - 1; l++)
                            {
                                int m = 0;
                                for (m = 0; m <= CTe.CteSimpProc.CTeSimp.infCte.det[i].infNFe[j].infUnidTransp[k].infUnidCarga[l].lacUnidCarga.Count - 1; m++)
                                {
                                    strSQL_aux = string.Empty;
                                    stbSQL.Clear();
                                    // Montando query a ser executada
                                    stbSQL.Append(clsFacil.MontarQuery("INSERT INTO cte.detalhe_xml_conhecimento_transporte_eletronico_simp_det_entrega_info_nfe_unidade_transporte_unidade_carga_lacre "));
                                    stbSQL.Append(clsFacil.MontarQuery("( "));
                                    stbSQL.Append(clsFacil.MontarQuery("  dtr_referencia, "));
                                    stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso, "));
                                    stbSQL.Append(clsFacil.MontarQuery("  num_item, "));
                                    stbSQL.Append(clsFacil.MontarQuery("  seq_detalhe_xml_conhecimento_transporte_eletronico_simp_det_entrega_info_nfe, "));
                                    stbSQL.Append(clsFacil.MontarQuery("  num_identificacao_unidade_transporte, "));
                                    stbSQL.Append(clsFacil.MontarQuery("  num_identificacao_unidade_carga, "));
                                    stbSQL.Append(clsFacil.MontarQuery("  num_lacre_unidade_carga, "));
                                    stbSQL.Append(clsFacil.MontarQuery("  dtc_atualizacao "));
                                    stbSQL.Append(clsFacil.MontarQuery(") "));
                                    stbSQL.Append(clsFacil.MontarQuery("VALUES "));
                                    stbSQL.Append(clsFacil.MontarQuery("( "));
                                    stbSQL.Append(clsFacil.MontarQuery("  @dtr_referencia, "));
                                    stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso, "));
                                    stbSQL.Append(clsFacil.MontarQuery("  @num_item, "));
                                    stbSQL.Append(clsFacil.MontarQuery("  @seq_detalhe_xml_conhecimento_transporte_eletronico_simp_det_entrega_info_nfe, "));
                                    stbSQL.Append(clsFacil.MontarQuery("  @num_identificacao_unidade_transporte, "));
                                    stbSQL.Append(clsFacil.MontarQuery("  @num_identificacao_unidade_carga, "));
                                    stbSQL.Append(clsFacil.MontarQuery("  @num_lacre_unidade_carga, "));
                                    stbSQL.Append(clsFacil.MontarQuery("  getdate() "));
                                    stbSQL.Append(clsFacil.MontarQuery(") "));
                                    strSQL_aux = stbSQL.ToString();

                                    // Montando parametros
                                    clsDados.LimparParametro();
                                    clsFacil.AdicionarParametro(ref strSQL_aux, "20" + CTe.CteSimpProc.protCTe.InfProt.chCTe.Substring(2, 4), "@dtr_referencia", SqlDbType.Int);
                                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.protCTe.InfProt.chCTe, "@cod_chave_acesso", SqlDbType.VarChar);
                                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.CTeSimp.infCte.det[i].nItem, "@num_item", SqlDbType.SmallInt);
                                    clsFacil.AdicionarParametro(ref strSQL_aux, Convert.ToString(j + 1), "@seq_detalhe_xml_conhecimento_transporte_eletronico_simp_det_entrega_info_nfe", SqlDbType.Int);
                                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.CTeSimp.infCte.det[i].infNFe[j].infUnidTransp[k].idUnidTransp, "@num_identificacao_unidade_transporte", SqlDbType.VarChar);
                                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.CTeSimp.infCte.det[i].infNFe[j].infUnidTransp[k].infUnidCarga[l].idUnidCarga, "@num_identificacao_unidade_carga", SqlDbType.VarChar);
                                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.CTeSimp.infCte.det[i].infNFe[j].infUnidTransp[k].infUnidCarga[l].lacUnidCarga[m].nLacre, "@num_lacre_unidade_carga", SqlDbType.VarChar);
                                    strSQL = strSQL + strSQL_aux;
                                }
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw new ArgumentException($"[{strMetodo}]: " + ex.Message);
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

        #region " modal "                

        public string InserirXML_detalhe_xml_conhecimento_transporte_eletronico_simp_modal_aereo(Common.SerializableClasses.CTe.xmlCTeSimp_v400.proc CTe)
        {
            // Classes e variaveis utilizadas
            AcessoDados clsDados = new AcessoDados();
            Facilitador clsFacil = new Facilitador();
            StringBuilder stbSQL = new StringBuilder();
            string strRetorno = string.Empty;
            string strMetodo = System.Reflection.MethodBase.GetCurrentMethod().Name;

            try
            {

                // Montando query a ser executada
                stbSQL.Append(clsFacil.MontarQuery("INSERT INTO cte.detalhe_xml_conhecimento_transporte_eletronico_simp_modal_aereo "));
                stbSQL.Append(clsFacil.MontarQuery("( "));
                stbSQL.Append(clsFacil.MontarQuery("  dtr_referencia, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso, "));
                stbSQL.Append(clsFacil.MontarQuery("  des_versao_modal, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_minuta, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_operacional_conhecimento_aereo, "));
                stbSQL.Append(clsFacil.MontarQuery("  dtc_previsao_entrega, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_tipo_classe_tarifaria, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_tarifa, "));
                stbSQL.Append(clsFacil.MontarQuery("  val_total_tarifa, "));
                stbSQL.Append(clsFacil.MontarQuery("  dtc_atualizacao "));
                stbSQL.Append(clsFacil.MontarQuery(") "));
                stbSQL.Append(clsFacil.MontarQuery("VALUES "));
                stbSQL.Append(clsFacil.MontarQuery("( "));
                stbSQL.Append(clsFacil.MontarQuery("  @dtr_referencia, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso, "));
                stbSQL.Append(clsFacil.MontarQuery("  @des_versao_modal, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_minuta, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_operacional_conhecimento_aereo, "));
                stbSQL.Append(clsFacil.MontarQuery("  @dtc_previsao_entrega, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_tipo_classe_tarifaria, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_tarifa, "));
                stbSQL.Append(clsFacil.MontarQuery("  @val_total_tarifa, "));
                stbSQL.Append(clsFacil.MontarQuery("  getdate() "));
                stbSQL.Append(clsFacil.MontarQuery(") "));
                string strSQL = stbSQL.ToString();

                // Montando parametros
                clsDados.LimparParametro();
                clsFacil.AdicionarParametro(ref strSQL, "20" + CTe.CteSimpProc.protCTe.InfProt.chCTe.Substring(2, 4), "@dtr_referencia", SqlDbType.Int);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.protCTe.InfProt.chCTe, "@cod_chave_acesso", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.infModal.versaoModal, "@des_versao_modal", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.infModal.aereo.nMinu, "@num_minuta", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.infModal.aereo.nOCA, "@num_operacional_conhecimento_aereo", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.infModal.aereo.dPrevAereo, "@dtc_previsao_entrega", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.infModal.aereo.tarifa.CL, "@cod_tipo_classe_tarifaria", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.infModal.aereo.tarifa.cTar, "@cod_tarifa", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTe.CteSimpProc.CTeSimp.infCte.infModal.aereo.tarifa.vTar), "@val_total_tarifa", SqlDbType.Decimal);

                strRetorno = strSQL;

            }
            catch (Exception ex)
            {
                throw new ArgumentException($"[{strMetodo}]: " + ex.Message);
            }
            finally
            {
                stbSQL = null;
                clsDados = null;
                clsFacil = null;
            }

            return strRetorno;
        }

        public string InserirXML_detalhe_xml_conhecimento_transporte_eletronico_simp_modal_aereo_natureza_carga(Common.SerializableClasses.CTe.xmlCTeSimp_v400.proc CTe)
        {
            // Classes e variaveis utilizadas
            AcessoDados clsDados = new AcessoDados();
            Facilitador clsFacil = new Facilitador();
            StringBuilder stbSQL = new StringBuilder();
            string strRetorno = string.Empty;
            string strMetodo = System.Reflection.MethodBase.GetCurrentMethod().Name;
            string strSQL = string.Empty;
            string strSQL_aux = string.Empty;

            try
            {

                int Lcont = 0;
                for (Lcont = 0; Lcont <= CTe.CteSimpProc.CTeSimp.infCte.infModal.aereo.natCarga.cInfManu.Count - 1; Lcont++)
                {
                    strSQL_aux = string.Empty;
                    stbSQL.Clear();
                    // Montando query a ser executada
                    stbSQL.Append(clsFacil.MontarQuery("INSERT INTO cte.detalhe_xml_conhecimento_transporte_eletronico_simp_modal_aereo_natureza_carga "));
                    stbSQL.Append(clsFacil.MontarQuery("( "));
                    stbSQL.Append(clsFacil.MontarQuery("  dtr_referencia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso, "));
                    stbSQL.Append(clsFacil.MontarQuery("  seq_detalhe_xml_conhecimento_transporte_eletronico_simp_modal_natureza_carga, "));
                    stbSQL.Append(clsFacil.MontarQuery("  des_dimensao, "));
                    stbSQL.Append(clsFacil.MontarQuery("  cod_tipo_manuseio, "));
                    stbSQL.Append(clsFacil.MontarQuery("  dtc_atualizacao "));
                    stbSQL.Append(clsFacil.MontarQuery(") "));
                    stbSQL.Append(clsFacil.MontarQuery("VALUES "));
                    stbSQL.Append(clsFacil.MontarQuery("( "));
                    stbSQL.Append(clsFacil.MontarQuery("  @dtr_referencia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @seq_detalhe_xml_conhecimento_transporte_eletronico_simp_modal_natureza_carga, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @des_dimensao, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @cod_tipo_manuseio, "));
                    stbSQL.Append(clsFacil.MontarQuery("  getdate() "));
                    stbSQL.Append(clsFacil.MontarQuery(") "));
                    strSQL_aux = stbSQL.ToString();

                    // Montando parametros
                    clsDados.LimparParametro();
                    clsFacil.AdicionarParametro(ref strSQL, "20" + CTe.CteSimpProc.protCTe.InfProt.chCTe.Substring(2, 4), "@dtr_referencia", SqlDbType.Int);
                    clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.protCTe.InfProt.chCTe, "@cod_chave_acesso", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, Convert.ToString(Lcont + 1), "@seq_detalhe_xml_conhecimento_transporte_eletronico_simp_modal_natureza_carga", SqlDbType.SmallInt);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.CTeSimp.infCte.infModal.aereo.natCarga.xDime, "@des_dimensao", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.CTeSimp.infCte.infModal.aereo.natCarga.cInfManu[Lcont], "@cod_tipo_manuseio", SqlDbType.SmallInt);
                    strSQL = strSQL + strSQL_aux;
                }

                strRetorno = strSQL;

            }
            catch (Exception ex)
            {
                throw new ArgumentException($"[{strMetodo}]: " + ex.Message);
            }
            finally
            {
                stbSQL = null;
                clsDados = null;
                clsFacil = null;
            }

            return strRetorno;
        }

        public string InserirXML_detalhe_xml_conhecimento_transporte_eletronico_simp_modal_aquaviario(Common.SerializableClasses.CTe.xmlCTeSimp_v400.proc CTe)
        {
            // Classes e variaveis utilizadas
            AcessoDados clsDados = new AcessoDados();
            Facilitador clsFacil = new Facilitador();
            StringBuilder stbSQL = new StringBuilder();
            string strRetorno = string.Empty;
            string strMetodo = System.Reflection.MethodBase.GetCurrentMethod().Name;

            try
            {

                // Montando query a ser executada
                stbSQL.Append(clsFacil.MontarQuery("INSERT INTO cte.detalhe_xml_conhecimento_transporte_eletronico_simp_modal_aquaviario "));
                stbSQL.Append(clsFacil.MontarQuery("( "));
                stbSQL.Append(clsFacil.MontarQuery("  dtr_referencia, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso, "));
                stbSQL.Append(clsFacil.MontarQuery("  des_versao_modal, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_identificacao_navio, "));
                stbSQL.Append(clsFacil.MontarQuery("  val_total_base_calculo_prestacao_afrmn, "));
                stbSQL.Append(clsFacil.MontarQuery("  val_total_adicional_frete_afrmn, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_viagem, "));
                stbSQL.Append(clsFacil.MontarQuery("  sig_direcao, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_irim, "));
                stbSQL.Append(clsFacil.MontarQuery("  ind_navegacao, "));
                stbSQL.Append(clsFacil.MontarQuery("  dtc_atualizacao "));
                stbSQL.Append(clsFacil.MontarQuery(") "));
                stbSQL.Append(clsFacil.MontarQuery("VALUES "));
                stbSQL.Append(clsFacil.MontarQuery("( "));
                stbSQL.Append(clsFacil.MontarQuery("  @dtr_referencia, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso, "));
                stbSQL.Append(clsFacil.MontarQuery("  @des_versao_modal, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_identificacao_navio, "));
                stbSQL.Append(clsFacil.MontarQuery("  @val_total_base_calculo_prestacao_afrmn, "));
                stbSQL.Append(clsFacil.MontarQuery("  @val_total_adicional_frete_afrmn, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_viagem, "));
                stbSQL.Append(clsFacil.MontarQuery("  @sig_direcao, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_irim, "));
                stbSQL.Append(clsFacil.MontarQuery("  @ind_navegacao, "));
                stbSQL.Append(clsFacil.MontarQuery("  getdate() "));
                stbSQL.Append(clsFacil.MontarQuery(") "));
                string strSQL = stbSQL.ToString();

                // Montando parametros
                clsDados.LimparParametro();
                clsFacil.AdicionarParametro(ref strSQL, "20" + CTe.CteSimpProc.protCTe.InfProt.chCTe.Substring(2, 4), "@dtr_referencia", SqlDbType.Int);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.protCTe.InfProt.chCTe, "@cod_chave_acesso", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.infModal.versaoModal, "@des_versao_modal", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.infModal.aquav.xNavio, "@num_identificacao_navio", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTe.CteSimpProc.CTeSimp.infCte.infModal.aquav.vPrest), "@val_total_base_calculo_prestacao_afrmn", SqlDbType.Decimal);
                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTe.CteSimpProc.CTeSimp.infCte.infModal.aquav.vAFRMM), "@val_total_adicional_frete_afrmn", SqlDbType.Decimal);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.infModal.aquav.nViag, "@num_viagem", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.infModal.aquav.direc, "@sig_direcao", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.infModal.aquav.irin, "@num_irim", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.infModal.aquav.tpNav, "@ind_navegacao", SqlDbType.SmallInt);

                strRetorno = strSQL;

            }
            catch (Exception ex)
            {
                throw new ArgumentException($"[{strMetodo}]: " + ex.Message);
            }
            finally
            {
                stbSQL = null;
                clsDados = null;
                clsFacil = null;
            }

            return strRetorno;
        }

        public string InserirXML_detalhe_xml_conhecimento_transporte_eletronico_simp_modal_aquaviario_balsa(Common.SerializableClasses.CTe.xmlCTeSimp_v400.proc CTe)
        {
            // Classes e variaveis utilizadas
            AcessoDados clsDados = new AcessoDados();
            Facilitador clsFacil = new Facilitador();
            StringBuilder stbSQL = new StringBuilder();
            string strRetorno = string.Empty;
            string strMetodo = System.Reflection.MethodBase.GetCurrentMethod().Name;
            string strSQL = string.Empty;
            string strSQL_aux = string.Empty;

            try
            {

                int Lcont = 0;
                for (Lcont = 0; Lcont <= CTe.CteSimpProc.CTeSimp.infCte.infModal.aquav.balsa.Count - 1; Lcont++)
                {
                    strSQL_aux = string.Empty;
                    stbSQL.Clear();
                    // Montando query a ser executada
                    stbSQL.Append(clsFacil.MontarQuery("INSERT INTO cte.detalhe_xml_conhecimento_transporte_eletronico_simp_modal_aquaviario_balsa "));
                    stbSQL.Append(clsFacil.MontarQuery("( "));
                    stbSQL.Append(clsFacil.MontarQuery("  dtr_referencia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso, "));
                    stbSQL.Append(clsFacil.MontarQuery("  seq_detalhe_xml_conhecimento_transporte_eletronico_simp_modal_aquaviario_balsa, "));
                    stbSQL.Append(clsFacil.MontarQuery("  num_identificacao_navio, "));
                    stbSQL.Append(clsFacil.MontarQuery("  num_balsa, "));
                    stbSQL.Append(clsFacil.MontarQuery("  dtc_atualizacao "));
                    stbSQL.Append(clsFacil.MontarQuery(") "));
                    stbSQL.Append(clsFacil.MontarQuery("VALUES "));
                    stbSQL.Append(clsFacil.MontarQuery("( "));
                    stbSQL.Append(clsFacil.MontarQuery("  @dtr_referencia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @seq_detalhe_xml_conhecimento_transporte_eletronico_simp_modal_aquaviario_balsa, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @num_identificacao_navio, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @num_balsa, "));
                    stbSQL.Append(clsFacil.MontarQuery("  getdate() "));
                    stbSQL.Append(clsFacil.MontarQuery(") "));
                    strSQL_aux = stbSQL.ToString();

                    // Montando parametros
                    clsDados.LimparParametro();
                    clsFacil.AdicionarParametro(ref strSQL, "20" + CTe.CteSimpProc.protCTe.InfProt.chCTe.Substring(2, 4), "@dtr_referencia", SqlDbType.Int);
                    clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.protCTe.InfProt.chCTe, "@cod_chave_acesso", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, Convert.ToString(Lcont + 1), "@seq_detalhe_xml_conhecimento_transporte_eletronico_simp_modal_aquaviario_balsa", SqlDbType.SmallInt);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.CTeSimp.infCte.infModal.aquav.xNavio, "@num_identificacao_navio", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.CTeSimp.infCte.infModal.aquav.balsa[Lcont].xBalsa, "@num_balsa", SqlDbType.VarChar);

                    strSQL = strSQL + strSQL_aux;
                }

                strRetorno = strSQL;

            }
            catch (Exception ex)
            {
                throw new ArgumentException($"[{strMetodo}]: " + ex.Message);
            }
            finally
            {
                stbSQL = null;
                clsDados = null;
                clsFacil = null;
            }

            return strRetorno;
        }

        public string InserirXML_detalhe_xml_conhecimento_transporte_eletronico_simp_modal_aquaviario_conteiner(Common.SerializableClasses.CTe.xmlCTeSimp_v400.proc CTe)
        {
            // Classes e variaveis utilizadas
            AcessoDados clsDados = new AcessoDados();
            Facilitador clsFacil = new Facilitador();
            StringBuilder stbSQL = new StringBuilder();
            string strRetorno = string.Empty;
            string strMetodo = System.Reflection.MethodBase.GetCurrentMethod().Name;
            string strSQL = string.Empty;
            string strSQL_aux = string.Empty;

            try
            {

                int Lcont = 0;
                for (Lcont = 0; Lcont <= CTe.CteSimpProc.CTeSimp.infCte.infModal.aquav.detCont.Count - 1; Lcont++)
                {
                    strSQL_aux = string.Empty;
                    stbSQL.Clear();
                    // Montando query a ser executada
                    stbSQL.Append(clsFacil.MontarQuery("INSERT INTO cte.detalhe_xml_conhecimento_transporte_eletronico_simp_modal_aquaviario_conteiner "));
                    stbSQL.Append(clsFacil.MontarQuery("( "));
                    stbSQL.Append(clsFacil.MontarQuery("  dtr_referencia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso, "));
                    stbSQL.Append(clsFacil.MontarQuery("  seq_detalhe_xml_conhecimento_transporte_eletronico_simp_modal_aquaviario_container, "));
                    stbSQL.Append(clsFacil.MontarQuery("  num_identificacao_navio, "));
                    stbSQL.Append(clsFacil.MontarQuery("  num_container, "));
                    stbSQL.Append(clsFacil.MontarQuery("  dtc_atualizacao "));
                    stbSQL.Append(clsFacil.MontarQuery(") "));
                    stbSQL.Append(clsFacil.MontarQuery("VALUES "));
                    stbSQL.Append(clsFacil.MontarQuery("( "));
                    stbSQL.Append(clsFacil.MontarQuery("  @dtr_referencia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @seq_detalhe_xml_conhecimento_transporte_eletronico_simp_modal_aquaviario_container, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @num_identificacao_navio, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @num_container, "));
                    stbSQL.Append(clsFacil.MontarQuery("  getdate() "));
                    stbSQL.Append(clsFacil.MontarQuery(") "));
                    strSQL_aux = stbSQL.ToString();

                    // Montando parametros
                    clsDados.LimparParametro();
                    clsFacil.AdicionarParametro(ref strSQL, "20" + CTe.CteSimpProc.protCTe.InfProt.chCTe.Substring(2, 4), "@dtr_referencia", SqlDbType.Int);
                    clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.protCTe.InfProt.chCTe, "@cod_chave_acesso", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, Convert.ToString(Lcont + 1), "@seq_detalhe_xml_conhecimento_transporte_eletronico_simp_modal_aquaviario_container", SqlDbType.SmallInt);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.CTeSimp.infCte.infModal.aquav.xNavio, "@num_identificacao_navio", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.CTeSimp.infCte.infModal.aquav.detCont[Lcont].nCont, "@num_container", SqlDbType.VarChar);

                    strSQL = strSQL + strSQL_aux;
                }

                strRetorno = strSQL;

            }
            catch (Exception ex)
            {
                throw new ArgumentException($"[{strMetodo}]: " + ex.Message);
            }
            finally
            {
                stbSQL = null;
                clsDados = null;
                clsFacil = null;
            }

            return strRetorno;
        }

        public string InserirXML_detalhe_xml_conhecimento_transporte_eletronico_simp_modal_aquaviario_conteiner_lacre(Common.SerializableClasses.CTe.xmlCTeSimp_v400.proc CTe)
        {
            // Classes e variaveis utilizadas
            AcessoDados clsDados = new AcessoDados();
            Facilitador clsFacil = new Facilitador();
            StringBuilder stbSQL = new StringBuilder();
            string strRetorno = string.Empty;
            string strMetodo = System.Reflection.MethodBase.GetCurrentMethod().Name;
            string strSQL = string.Empty;
            string strSQL_aux = string.Empty;

            try
            {

                int i = 0;
                for (i = 0; i <= CTe.CteSimpProc.CTeSimp.infCte.infModal.aquav.detCont.Count - 1; i++)
                {
                    int j = 0;
                    for (j = 0; j <= CTe.CteSimpProc.CTeSimp.infCte.infModal.aquav.detCont[i].lacre.Count - 1; j++)
                    {
                        strSQL_aux = string.Empty;
                        stbSQL.Clear();
                        // Montando query a ser executada
                        stbSQL.Append(clsFacil.MontarQuery("INSERT INTO cte.detalhe_xml_conhecimento_transporte_eletronico_simp_modal_aquaviario_conteiner_lacre "));
                        stbSQL.Append(clsFacil.MontarQuery("( "));
                        stbSQL.Append(clsFacil.MontarQuery("  dtr_referencia, "));
                        stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso, "));
                        stbSQL.Append(clsFacil.MontarQuery("  seq_detalhe_xml_conhecimento_transporte_eletronico_simp_modal_aquaviario_container, "));
                        stbSQL.Append(clsFacil.MontarQuery("  seq_detalhe_xml_conhecimento_transporte_eletronico_simp_modal_aquaviario_container_lacre, "));
                        stbSQL.Append(clsFacil.MontarQuery("  num_identificacao_navio, "));
                        stbSQL.Append(clsFacil.MontarQuery("  num_container, "));
                        stbSQL.Append(clsFacil.MontarQuery("  num_lacre, "));
                        stbSQL.Append(clsFacil.MontarQuery("  dtc_atualizacao "));
                        stbSQL.Append(clsFacil.MontarQuery(") "));
                        stbSQL.Append(clsFacil.MontarQuery("VALUES "));
                        stbSQL.Append(clsFacil.MontarQuery("( "));
                        stbSQL.Append(clsFacil.MontarQuery("  @dtr_referencia, "));
                        stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso, "));
                        stbSQL.Append(clsFacil.MontarQuery("  @seq_detalhe_xml_conhecimento_transporte_eletronico_simp_modal_aquaviario_container, "));
                        stbSQL.Append(clsFacil.MontarQuery("  @seq_detalhe_xml_conhecimento_transporte_eletronico_simp_modal_aquaviario_container_lacre, "));
                        stbSQL.Append(clsFacil.MontarQuery("  @num_identificacao_navio, "));
                        stbSQL.Append(clsFacil.MontarQuery("  @num_container, "));
                        stbSQL.Append(clsFacil.MontarQuery("  @num_lacre, "));
                        stbSQL.Append(clsFacil.MontarQuery("  getdate() "));
                        stbSQL.Append(clsFacil.MontarQuery(") "));
                        strSQL_aux = stbSQL.ToString();

                        // Montando parametros
                        clsDados.LimparParametro();
                        clsFacil.AdicionarParametro(ref strSQL, "20" + CTe.CteSimpProc.protCTe.InfProt.chCTe.Substring(2, 4), "@dtr_referencia", SqlDbType.Int);
                        clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.protCTe.InfProt.chCTe, "@cod_chave_acesso", SqlDbType.VarChar);
                        clsFacil.AdicionarParametro(ref strSQL_aux, Convert.ToString(i + 1), "@seq_detalhe_xml_conhecimento_transporte_eletronico_simp_modal_aquaviario_container", SqlDbType.SmallInt);
                        clsFacil.AdicionarParametro(ref strSQL_aux, Convert.ToString(j + 1), "@seq_detalhe_xml_conhecimento_transporte_eletronico_simp_modal_aquaviario_container_lacre", SqlDbType.SmallInt);
                        clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.CTeSimp.infCte.infModal.aquav.xNavio, "@num_identificacao_navio", SqlDbType.VarChar);
                        clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.CTeSimp.infCte.infModal.aquav.detCont[i].nCont, "@num_container", SqlDbType.VarChar);
                        clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.CTeSimp.infCte.infModal.aquav.detCont[i].lacre[j].nLacre, "@num_lacre", SqlDbType.VarChar);
                        strSQL = strSQL + strSQL_aux;

                    }
                }
                strRetorno = strSQL;

            }
            catch (Exception ex)
            {
                throw new ArgumentException($"[{strMetodo}]: " + ex.Message);
            }
            finally
            {
                stbSQL = null;
                clsDados = null;
                clsFacil = null;
            }

            return strRetorno;
        }

        public string InserirXML_detalhe_xml_conhecimento_transporte_eletronico_simp_modal_aquaviario_nota_fiscal(Common.SerializableClasses.CTe.xmlCTeSimp_v400.proc CTe)
        {
            // Classes e variaveis utilizadas
            AcessoDados clsDados = new AcessoDados();
            Facilitador clsFacil = new Facilitador();
            StringBuilder stbSQL = new StringBuilder();
            string strRetorno = string.Empty;
            string strMetodo = System.Reflection.MethodBase.GetCurrentMethod().Name;
            string strSQL = string.Empty;
            string strSQL_aux = string.Empty;

            try
            {

                int i = 0;
                for (i = 0; i <= CTe.CteSimpProc.CTeSimp.infCte.infModal.aquav.detCont.Count - 1; i++)
                {
                    int j = 0;
                    for (j = 0; j <= CTe.CteSimpProc.CTeSimp.infCte.infModal.aquav.detCont[i].infDoc.infNF.Count - 1; j++)
                    {
                        strSQL_aux = string.Empty;
                        stbSQL.Clear();
                        // Montando query a ser executada
                        stbSQL.Append(clsFacil.MontarQuery("INSERT INTO cte.detalhe_xml_conhecimento_transporte_eletronico_simp_modal_aquaviario_nota_fiscal "));
                        stbSQL.Append(clsFacil.MontarQuery("( "));
                        stbSQL.Append(clsFacil.MontarQuery("  dtr_referencia, "));
                        stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso, "));
                        stbSQL.Append(clsFacil.MontarQuery("  seq_detalhe_xml_conhecimento_transporte_eletronico_simp_modal_aquaviario_nota_fiscal, "));
                        stbSQL.Append(clsFacil.MontarQuery("  num_identificacao_navio, "));
                        stbSQL.Append(clsFacil.MontarQuery("  num_serie_nota_fiscal, "));
                        stbSQL.Append(clsFacil.MontarQuery("  num_documento_fiscal, "));
                        stbSQL.Append(clsFacil.MontarQuery("  num_unidade_medida_rateada, "));
                        stbSQL.Append(clsFacil.MontarQuery("  dtc_atualizacao "));
                        stbSQL.Append(clsFacil.MontarQuery(") "));
                        stbSQL.Append(clsFacil.MontarQuery("VALUES "));
                        stbSQL.Append(clsFacil.MontarQuery("( "));
                        stbSQL.Append(clsFacil.MontarQuery("  @dtr_referencia, "));
                        stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso, "));
                        stbSQL.Append(clsFacil.MontarQuery("  @seq_detalhe_xml_conhecimento_transporte_eletronico_simp_modal_aquaviario_nota_fiscal, "));
                        stbSQL.Append(clsFacil.MontarQuery("  @num_identificacao_navio, "));
                        stbSQL.Append(clsFacil.MontarQuery("  @num_serie_nota_fiscal, "));
                        stbSQL.Append(clsFacil.MontarQuery("  @num_documento_fiscal, "));
                        stbSQL.Append(clsFacil.MontarQuery("  @num_unidade_medida_rateada, "));
                        stbSQL.Append(clsFacil.MontarQuery("  getdate() "));
                        stbSQL.Append(clsFacil.MontarQuery(") "));
                        strSQL_aux = stbSQL.ToString();

                        // Montando parametros
                        clsDados.LimparParametro();
                        clsFacil.AdicionarParametro(ref strSQL, "20" + CTe.CteSimpProc.protCTe.InfProt.chCTe.Substring(2, 4), "@dtr_referencia", SqlDbType.Int);
                        clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.protCTe.InfProt.chCTe, "@cod_chave_acesso", SqlDbType.VarChar);
                        clsFacil.AdicionarParametro(ref strSQL_aux, Convert.ToString(j + 1), "@seq_detalhe_xml_conhecimento_transporte_eletronico_simp_modal_aquaviario_nota_fiscal", SqlDbType.SmallInt);
                        clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.CTeSimp.infCte.infModal.aquav.xNavio, "@num_identificacao_navio", SqlDbType.VarChar);
                        clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.CTeSimp.infCte.infModal.aquav.detCont[i].infDoc.infNF[j].serie, "@num_serie_nota_fiscal", SqlDbType.VarChar);
                        clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.CTeSimp.infCte.infModal.aquav.detCont[i].infDoc.infNF[j].nDoc, "@num_documento_fiscal", SqlDbType.VarChar);
                        clsFacil.AdicionarParametro(ref strSQL_aux, Convert.ToString(CTe.CteSimpProc.CTeSimp.infCte.infModal.aquav.detCont[i].infDoc.infNF[j].unidRat), "@num_unidade_medida_rateada", SqlDbType.Decimal);
                        strSQL = strSQL + strSQL_aux;

                    }
                }
                strRetorno = strSQL;

            }
            catch (Exception ex)
            {
                throw new ArgumentException($"[{strMetodo}]: " + ex.Message);
            }
            finally
            {
                stbSQL = null;
                clsDados = null;
                clsFacil = null;
            }

            return strRetorno;
        }

        public string InserirXML_detalhe_xml_conhecimento_transporte_eletronico_simp_modal_aquaviario_nota_fiscal_eletronica(Common.SerializableClasses.CTe.xmlCTeSimp_v400.proc CTe)
        {
            // Classes e variaveis utilizadas
            AcessoDados clsDados = new AcessoDados();
            Facilitador clsFacil = new Facilitador();
            StringBuilder stbSQL = new StringBuilder();
            string strRetorno = string.Empty;
            string strMetodo = System.Reflection.MethodBase.GetCurrentMethod().Name;
            string strSQL = string.Empty;
            string strSQL_aux = string.Empty;

            try
            {

                int i = 0;
                for (i = 0; i <= CTe.CteSimpProc.CTeSimp.infCte.infModal.aquav.detCont.Count - 1; i++)
                {
                    int j = 0;
                    for (j = 0; j <= CTe.CteSimpProc.CTeSimp.infCte.infModal.aquav.detCont[i].infDoc.infNFe.Count - 1; j++)
                    {
                        strSQL_aux = string.Empty;
                        stbSQL.Clear();
                        // Montando query a ser executada
                        stbSQL.Append(clsFacil.MontarQuery("INSERT INTO cte.detalhe_xml_conhecimento_transporte_eletronico_simp_modal_aquaviario_nota_fiscal_eletronica "));
                        stbSQL.Append(clsFacil.MontarQuery("( "));
                        stbSQL.Append(clsFacil.MontarQuery("  dtr_referencia, "));
                        stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso, "));
                        stbSQL.Append(clsFacil.MontarQuery("  seq_detalhe_xml_conhecimento_transporte_eletronico_simp_modal_aquaviario_nota_fiscal_eletronica, "));
                        stbSQL.Append(clsFacil.MontarQuery("  num_identificacao_navio, "));
                        stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso_nfe, "));
                        stbSQL.Append(clsFacil.MontarQuery("  val_unidade_medida, "));
                        stbSQL.Append(clsFacil.MontarQuery("  dtc_atualizacao "));
                        stbSQL.Append(clsFacil.MontarQuery(") "));
                        stbSQL.Append(clsFacil.MontarQuery("VALUES "));
                        stbSQL.Append(clsFacil.MontarQuery("( "));
                        stbSQL.Append(clsFacil.MontarQuery("  @dtr_referencia, "));
                        stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso, "));
                        stbSQL.Append(clsFacil.MontarQuery("  @seq_detalhe_xml_conhecimento_transporte_eletronico_simp_modal_aquaviario_nota_fiscal_eletronica, "));
                        stbSQL.Append(clsFacil.MontarQuery("  @num_identificacao_navio, "));
                        stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso_nfe, "));
                        stbSQL.Append(clsFacil.MontarQuery("  @val_unidade_medida, "));
                        stbSQL.Append(clsFacil.MontarQuery("  getdate() "));
                        stbSQL.Append(clsFacil.MontarQuery(") "));
                        strSQL_aux = stbSQL.ToString();

                        // Montando parametros
                        clsDados.LimparParametro();
                        clsFacil.AdicionarParametro(ref strSQL, "20" + CTe.CteSimpProc.protCTe.InfProt.chCTe.Substring(2, 4), "@dtr_referencia", SqlDbType.Int);
                        clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.protCTe.InfProt.chCTe, "@cod_chave_acesso", SqlDbType.VarChar);
                        clsFacil.AdicionarParametro(ref strSQL_aux, Convert.ToString(j + 1), "@seq_detalhe_xml_conhecimento_transporte_eletronico_modal_aquaviario_nota_fiscal_eletronica", SqlDbType.SmallInt);
                        clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.CTeSimp.infCte.infModal.aquav.xNavio, "@num_identificacao_navio", SqlDbType.VarChar);
                        clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.CTeSimp.infCte.infModal.aquav.detCont[i].infDoc.infNFe[j].chave, "@cod_chave_acesso_nfe", SqlDbType.VarChar);
                        clsFacil.AdicionarParametro(ref strSQL_aux, Convert.ToString(CTe.CteSimpProc.CTeSimp.infCte.infModal.aquav.detCont[i].infDoc.infNFe[j].unidRat), "@val_unidade_medida", SqlDbType.Decimal);
                        strSQL = strSQL + strSQL_aux;

                    }
                }
                // Executando query
                strRetorno = strSQL;

            }
            catch (Exception ex)
            {
                throw new ArgumentException($"[{strMetodo}]: " + ex.Message);
            }
            finally
            {
                stbSQL = null;
                clsDados = null;
                clsFacil = null;
            }

            return strRetorno;
        }

        public string InserirXML_detalhe_xml_conhecimento_transporte_eletronico_simp_modal_aereo_perigo_onu(Common.SerializableClasses.CTe.xmlCTeSimp_v400.proc CTe)
        {
            // Classes e variaveis utilizadas
            AcessoDados clsDados = new AcessoDados();
            Facilitador clsFacil = new Facilitador();
            StringBuilder stbSQL = new StringBuilder();
            string strRetorno = string.Empty;
            string strMetodo = System.Reflection.MethodBase.GetCurrentMethod().Name;
            string strSQL = string.Empty;
            string strSQL_aux = string.Empty;

            try
            {

                int Lcont = 0;
                for (Lcont = 0; Lcont <= CTe.CteSimpProc.CTeSimp.infCte.infModal.aereo.peri.Count - 1; Lcont++)
                {
                    strSQL_aux = string.Empty;
                    stbSQL.Clear();
                    // Montando query a ser executada
                    stbSQL.Append(clsFacil.MontarQuery("INSERT INTO cte.detalhe_xml_conhecimento_transporte_eletronico_simp_modal_aereo_perigo_onu "));
                    stbSQL.Append(clsFacil.MontarQuery("( "));
                    stbSQL.Append(clsFacil.MontarQuery("  dtr_referencia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso, "));
                    stbSQL.Append(clsFacil.MontarQuery("  seq_detalhe_xml_conhecimento_transporte_eletronico_simp_modal_aereo_perigo_onu, "));
                    stbSQL.Append(clsFacil.MontarQuery("  num_onu, "));
                    stbSQL.Append(clsFacil.MontarQuery("  qtd_total_volume_perigoso, "));
                    stbSQL.Append(clsFacil.MontarQuery("  qtd_total_artigo_perigoso, "));
                    stbSQL.Append(clsFacil.MontarQuery("  cod_tipo_unidade_medida, "));
                    stbSQL.Append(clsFacil.MontarQuery("  dtc_atualizacao "));
                    stbSQL.Append(clsFacil.MontarQuery(") "));
                    stbSQL.Append(clsFacil.MontarQuery("VALUES "));
                    stbSQL.Append(clsFacil.MontarQuery("( "));
                    stbSQL.Append(clsFacil.MontarQuery("  @dtr_referencia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @seq_detalhe_xml_conhecimento_transporte_eletronico_simp_modal_aereo_perigo_onu, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @num_onu, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @qtd_total_volume_perigoso, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @qtd_total_artigo_perigoso, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @cod_tipo_unidade_medida, "));
                    stbSQL.Append(clsFacil.MontarQuery("  getdate() "));
                    stbSQL.Append(clsFacil.MontarQuery(") "));
                    strSQL_aux = stbSQL.ToString();

                    // Montando parametros
                    clsDados.LimparParametro();
                    clsFacil.AdicionarParametro(ref strSQL_aux, "20" + CTe.CteSimpProc.protCTe.InfProt.chCTe.Substring(2, 4), "@dtr_referencia", SqlDbType.Int);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.protCTe.InfProt.chCTe, "@cod_chave_acesso", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, Convert.ToString(Lcont + 1), "@seq_detalhe_xml_conhecimento_transporte_eletronico_simp_modal_aereo_perigo_onu", SqlDbType.SmallInt);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.CTeSimp.infCte.infModal.aereo.peri[Lcont].nONU, "@num_onu", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.CTeSimp.infCte.infModal.aereo.peri[Lcont].qTotEmb, "@qtd_total_volume_perigoso", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, Convert.ToString(CTe.CteSimpProc.CTeSimp.infCte.infModal.aereo.peri[Lcont].infTotAP.qTotProd), "@qtd_total_artigo_perigoso", SqlDbType.Decimal);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.CTeSimp.infCte.infModal.aereo.peri[Lcont].infTotAP.uniAP, "@cod_tipo_unidade_medida", SqlDbType.SmallInt);
                    strSQL = strSQL + strSQL_aux;
                }

                strRetorno = strSQL;

            }
            catch (Exception ex)
            {
                throw new ArgumentException($"[{strMetodo}]: " + ex.Message);
            }
            finally
            {
                stbSQL = null;
                clsDados = null;
                clsFacil = null;
            }

            return strRetorno;
        }

        public string InserirXML_detalhe_xml_conhecimento_transporte_eletronico_simp_modal_dutoviario(Common.SerializableClasses.CTe.xmlCTeSimp_v400.proc CTe)
        {
            // Classes e variaveis utilizadas
            AcessoDados clsDados = new AcessoDados();
            Facilitador clsFacil = new Facilitador();
            StringBuilder stbSQL = new StringBuilder();
            string strRetorno = string.Empty;
            string strMetodo = System.Reflection.MethodBase.GetCurrentMethod().Name;

            try
            {

                // Montando query a ser executada
                stbSQL.Append(clsFacil.MontarQuery("INSERT INTO cte.detalhe_xml_conhecimento_transporte_eletronico_simp_modal_dutoviario "));
                stbSQL.Append(clsFacil.MontarQuery("( "));
                stbSQL.Append(clsFacil.MontarQuery("  dtr_referencia, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso, "));
                stbSQL.Append(clsFacil.MontarQuery("  des_versao_modal, "));
                stbSQL.Append(clsFacil.MontarQuery("  val_total_tarifa, "));
                stbSQL.Append(clsFacil.MontarQuery("  dtc_inicio_prestracao_servico, "));
                stbSQL.Append(clsFacil.MontarQuery("  dtc_fim_prestracao_servico, "));
                stbSQL.Append(clsFacil.MontarQuery("  dtc_atualizacao "));
                stbSQL.Append(clsFacil.MontarQuery(") "));
                stbSQL.Append(clsFacil.MontarQuery("VALUES "));
                stbSQL.Append(clsFacil.MontarQuery("( "));
                stbSQL.Append(clsFacil.MontarQuery("  @dtr_referencia, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso, "));
                stbSQL.Append(clsFacil.MontarQuery("  @des_versao_modal, "));
                stbSQL.Append(clsFacil.MontarQuery("  @val_total_tarifa, "));
                stbSQL.Append(clsFacil.MontarQuery("  @dtc_inicio_prestracao_servico, "));
                stbSQL.Append(clsFacil.MontarQuery("  @dtc_fim_prestracao_servico, "));
                stbSQL.Append(clsFacil.MontarQuery("  getdate() "));
                stbSQL.Append(clsFacil.MontarQuery(") "));
                string strSQL = stbSQL.ToString();

                // Montando parametros
                clsDados.LimparParametro();
                clsFacil.AdicionarParametro(ref strSQL, "20" + CTe.CteSimpProc.protCTe.InfProt.chCTe.Substring(2, 4), "@dtr_referencia", SqlDbType.Int);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.protCTe.InfProt.chCTe, "@cod_chave_acesso", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.infModal.versaoModal, "@des_versao_modal", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTe.CteSimpProc.CTeSimp.infCte.infModal.duto.vTar), "@val_total_tarifa", SqlDbType.Decimal);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.infModal.duto.dIni, "@dtc_inicio_prestracao_servico", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.infModal.duto.dFim, "@dtc_fim_prestracao_servico", SqlDbType.VarChar);

                strRetorno = strSQL;

            }
            catch (Exception ex)
            {
                throw new ArgumentException($"[{strMetodo}]: " + ex.Message);
            }
            finally
            {
                stbSQL = null;
                clsDados = null;
                clsFacil = null;
            }

            return strRetorno;
        }

        public string InserirXML_detalhe_xml_conhecimento_transporte_eletronico_simp_modal_ferroviario(Common.SerializableClasses.CTe.xmlCTeSimp_v400.proc CTe)
        {
            // Classes e variaveis utilizadas
            AcessoDados clsDados = new AcessoDados();
            Facilitador clsFacil = new Facilitador();
            StringBuilder stbSQL = new StringBuilder();
            string strRetorno = string.Empty;
            string strMetodo = System.Reflection.MethodBase.GetCurrentMethod().Name;

            try
            {

                // Montando query a ser executada
                stbSQL.Append(clsFacil.MontarQuery("INSERT INTO cte.detalhe_xml_conhecimento_transporte_eletronico_simp_modal_ferroviario "));
                stbSQL.Append(clsFacil.MontarQuery("( "));
                stbSQL.Append(clsFacil.MontarQuery("  dtr_referencia, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso, "));
                stbSQL.Append(clsFacil.MontarQuery("  des_versao_modal, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_tipo_trafego, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_responsavel_faturamento, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_ferrovia_emitente, "));
                stbSQL.Append(clsFacil.MontarQuery("  val_total_frete_trafego_mutuo, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso_cte_ferrovia, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_fluxo_ferroviario, "));
                stbSQL.Append(clsFacil.MontarQuery("  dtc_atualizacao "));
                stbSQL.Append(clsFacil.MontarQuery(") "));
                stbSQL.Append(clsFacil.MontarQuery("VALUES "));
                stbSQL.Append(clsFacil.MontarQuery("( "));
                stbSQL.Append(clsFacil.MontarQuery("  @dtr_referencia, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso, "));
                stbSQL.Append(clsFacil.MontarQuery("  @des_versao_modal, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_tipo_trafego, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_responsavel_faturamento, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_ferrovia_emitente, "));
                stbSQL.Append(clsFacil.MontarQuery("  @val_total_frete_trafego_mutuo, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso_cte_ferrovia, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_fluxo_ferroviario, "));
                stbSQL.Append(clsFacil.MontarQuery("  getdate() "));
                stbSQL.Append(clsFacil.MontarQuery(") "));
                string strSQL = stbSQL.ToString();

                // Montando parametros
                clsDados.LimparParametro();
                clsFacil.AdicionarParametro(ref strSQL, "20" + CTe.CteSimpProc.protCTe.InfProt.chCTe.Substring(2, 4), "@dtr_referencia", SqlDbType.Int);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.protCTe.InfProt.chCTe, "@cod_chave_acesso", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.infModal.versaoModal, "@des_versao_modal", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.infModal.ferrov.tpTraf, "@cod_tipo_trafego", SqlDbType.SmallInt);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.infModal.ferrov.trafMut.respFat, "@cod_responsavel_faturamento", SqlDbType.SmallInt);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.infModal.ferrov.trafMut.ferrEmi, "@cod_ferrovia_emitente", SqlDbType.SmallInt);
                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTe.CteSimpProc.CTeSimp.infCte.infModal.ferrov.trafMut.vFrete), "@val_total_frete_trafego_mutuo", SqlDbType.Decimal);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.infModal.ferrov.trafMut.chCTeFerroOrigem, "@cod_chave_acesso_cte_ferrovia", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.infModal.ferrov.trafMut.fluxo, "@num_fluxo_ferroviario", SqlDbType.VarChar);

                strRetorno = strSQL;
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"[{strMetodo}]: " + ex.Message);
            }
            finally
            {
                stbSQL = null;
                clsDados = null;
                clsFacil = null;
            }

            return strRetorno;
        }

        public string InserirXML_detalhe_xml_conhecimento_transporte_eletronico_simp_modal_ferroviario_ferrovia(Common.SerializableClasses.CTe.xmlCTeSimp_v400.proc CTe)
        {
            // Classes e variaveis utilizadas
            AcessoDados clsDados = new AcessoDados();
            Facilitador clsFacil = new Facilitador();
            StringBuilder stbSQL = new StringBuilder();
            string strRetorno = string.Empty;
            string strMetodo = System.Reflection.MethodBase.GetCurrentMethod().Name;
            string strSQL = string.Empty;
            string strSQL_aux = string.Empty;

            try
            {

                int Lcont = 0;
                for (Lcont = 0; Lcont <= CTe.CteSimpProc.CTeSimp.infCte.infModal.ferrov.trafMut.ferroEnv.Count - 1; Lcont++)
                {
                    strSQL_aux = string.Empty;
                    stbSQL.Clear();
                    // Montando query a ser executada
                    stbSQL.Append(clsFacil.MontarQuery("INSERT INTO cte.detalhe_xml_conhecimento_transporte_eletronico_simp_modal_ferroviario_ferrovia "));
                    stbSQL.Append(clsFacil.MontarQuery("( "));
                    stbSQL.Append(clsFacil.MontarQuery("  dtr_referencia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso, "));
                    stbSQL.Append(clsFacil.MontarQuery("  seq_detalhe_xml_conhecimento_transporte_eletronico_simp_modal_ferroviario_ferrovia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  num_cnpj_cpf_base_ferrovia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  num_cnpj_cpf_filial_ferrovia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  dig_cnpj_cpf_ferrovia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  cod_interno_ferrovia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  num_insc_estad_ferrovia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  nom_razao_social_ferrovia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  des_logradouro_ferrovia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  num_endereco_ferrovia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  des_compl_endereco_ferrovia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  des_bairro_ferrovia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  cod_municipio_ibge_ferrovia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  nom_municipio_ibge_ferrovia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  num_cep_ferrovia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  sig_unid_federacao_ferrovia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  dtc_atualizacao "));
                    stbSQL.Append(clsFacil.MontarQuery(") "));
                    stbSQL.Append(clsFacil.MontarQuery("VALUES "));
                    stbSQL.Append(clsFacil.MontarQuery("( "));
                    stbSQL.Append(clsFacil.MontarQuery("  @dtr_referencia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @seq_detalhe_xml_conhecimento_transporte_eletronico_simp_modal_ferroviario_ferrovia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @num_cnpj_cpf_base_ferrovia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @num_cnpj_cpf_filial_ferrovia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @dig_cnpj_cpf_ferrovia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @cod_interno_ferrovia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @num_insc_estad_ferrovia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @nom_razao_social_ferrovia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @des_logradouro_ferrovia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @num_endereco_ferrovia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @des_compl_endereco_ferrovia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @des_bairro_ferrovia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @cod_municipio_ibge_ferrovia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @nom_municipio_ibge_ferrovia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @num_cep_ferrovia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @sig_unid_federacao_ferrovia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  getdate() "));
                    stbSQL.Append(clsFacil.MontarQuery(") "));
                    strSQL_aux = stbSQL.ToString();

                    // Montando parametros
                    clsDados.LimparParametro();
                    clsFacil.AdicionarParametro(ref strSQL, "20" + CTe.CteSimpProc.protCTe.InfProt.chCTe.Substring(2, 4), "@dtr_referencia", SqlDbType.Int);
                    clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.protCTe.InfProt.chCTe, "@cod_chave_acesso", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, Convert.ToString(Lcont + 1), "@seq_detalhe_xml_conhecimento_transporte_eletronico_simp_modal_ferroviario_ferrovia", SqlDbType.SmallInt);

                    if (CTe.CteSimpProc.CTeSimp.infCte.infModal.ferrov.trafMut.ferroEnv[Lcont].CNPJ != null && CTe.CteSimpProc.CTeSimp.infCte.infModal.ferrov.trafMut.ferroEnv[Lcont].CNPJ != string.Empty)
                    {
                        clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.CTeSimp.infCte.infModal.ferrov.trafMut.ferroEnv[Lcont].CNPJ.Substring(0, 8), "@num_cnpj_cpf_base_ferrovia", SqlDbType.VarChar);
                        clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.CTeSimp.infCte.infModal.ferrov.trafMut.ferroEnv[Lcont].CNPJ.Substring(8, 4), "@num_cnpj_cpf_filial_ferrovia", SqlDbType.VarChar);
                        clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.CTeSimp.infCte.infModal.ferrov.trafMut.ferroEnv[Lcont].CNPJ.Substring(12, 2), "@dig_cnpj_cpf_ferrovia", SqlDbType.SmallInt);
                    }
                    else
                    {
                        clsFacil.AdicionarParametro(ref strSQL_aux, string.Empty, "@num_cnpj_cpf_base_ferrovia", SqlDbType.VarChar);
                        clsFacil.AdicionarParametro(ref strSQL_aux, string.Empty, "@num_cnpj_cpf_filial_ferrovia", SqlDbType.VarChar);
                        clsFacil.AdicionarParametro(ref strSQL_aux, string.Empty, "@dig_cnpj_cpf_ferrovia", SqlDbType.SmallInt);
                    }

                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.CTeSimp.infCte.infModal.ferrov.trafMut.ferroEnv[Lcont].cInt, "@cod_interno_ferrovia", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.CTeSimp.infCte.infModal.ferrov.trafMut.ferroEnv[Lcont].IE, "@num_insc_estad_ferrovia", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.CTeSimp.infCte.infModal.ferrov.trafMut.ferroEnv[Lcont].xNome, "@nom_razao_social_ferrovia", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.CTeSimp.infCte.infModal.ferrov.trafMut.ferroEnv[Lcont].enderFerro.xLgr, "@des_logradouro_ferrovia", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.CTeSimp.infCte.infModal.ferrov.trafMut.ferroEnv[Lcont].enderFerro.nro, "@num_endereco_ferrovia", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.CTeSimp.infCte.infModal.ferrov.trafMut.ferroEnv[Lcont].enderFerro.xCpl, "@des_compl_endereco_ferrovia", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.CTeSimp.infCte.infModal.ferrov.trafMut.ferroEnv[Lcont].enderFerro.xBairro, "@des_bairro_ferrovia", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.CTeSimp.infCte.infModal.ferrov.trafMut.ferroEnv[Lcont].enderFerro.cMun, "@cod_municipio_ibge_ferrovia", SqlDbType.Int);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.CTeSimp.infCte.infModal.ferrov.trafMut.ferroEnv[Lcont].enderFerro.xMun, "@nom_municipio_ibge_ferrovia", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.CTeSimp.infCte.infModal.ferrov.trafMut.ferroEnv[Lcont].enderFerro.CEP, "@num_cep_ferrovia", SqlDbType.Int);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.CTeSimp.infCte.infModal.ferrov.trafMut.ferroEnv[Lcont].enderFerro.UF, "@sig_unid_federacao_ferrovia", SqlDbType.VarChar);
                    strSQL = strSQL + strSQL_aux;
                }

                strRetorno = strSQL;

            }
            catch (Exception ex)
            {
                throw new ArgumentException($"[{strMetodo}]: " + ex.Message);
            }
            finally
            {
                stbSQL = null;
                clsDados = null;
                clsFacil = null;
            }

            return strRetorno;
        }

        public string InserirXML_detalhe_xml_conhecimento_transporte_eletronico_simp_modal_multimodal(Common.SerializableClasses.CTe.xmlCTeSimp_v400.proc CTe)
        {
            // Classes e variaveis utilizadas
            AcessoDados clsDados = new AcessoDados();
            Facilitador clsFacil = new Facilitador();
            StringBuilder stbSQL = new StringBuilder();
            string strRetorno = string.Empty;
            string strMetodo = System.Reflection.MethodBase.GetCurrentMethod().Name;

            try
            {

                // Montando query a ser executada
                stbSQL.Append(clsFacil.MontarQuery("INSERT INTO cte.detalhe_xml_conhecimento_transporte_eletronico_simp_modal_multimodal "));
                stbSQL.Append(clsFacil.MontarQuery("( "));
                stbSQL.Append(clsFacil.MontarQuery("  dtr_referencia, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso, "));
                stbSQL.Append(clsFacil.MontarQuery("  des_versao_modal, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_certificado_operador, "));
                stbSQL.Append(clsFacil.MontarQuery("  ind_negociavel, "));
                stbSQL.Append(clsFacil.MontarQuery("  nom_seguradora, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_cnpj_cpf_base_seguradora, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_cnpj_cpf_filial_seguradora, "));
                stbSQL.Append(clsFacil.MontarQuery("  dig_cnpj_cpf_seguradora, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_apolice_seguro, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_averbacao, "));
                stbSQL.Append(clsFacil.MontarQuery("  dtc_atualizacao "));
                stbSQL.Append(clsFacil.MontarQuery(") "));
                stbSQL.Append(clsFacil.MontarQuery("VALUES "));
                stbSQL.Append(clsFacil.MontarQuery("( "));
                stbSQL.Append(clsFacil.MontarQuery("  @dtr_referencia, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso, "));
                stbSQL.Append(clsFacil.MontarQuery("  @des_versao_modal, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_certificado_operador, "));
                stbSQL.Append(clsFacil.MontarQuery("  @ind_negociavel, "));
                stbSQL.Append(clsFacil.MontarQuery("  @nom_seguradora, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_cnpj_cpf_base_seguradora, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_cnpj_cpf_filial_seguradora, "));
                stbSQL.Append(clsFacil.MontarQuery("  @dig_cnpj_cpf_seguradora, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_apolice_seguro, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_averbacao, "));
                stbSQL.Append(clsFacil.MontarQuery("  getdate() "));
                stbSQL.Append(clsFacil.MontarQuery(") "));
                string strSQL = stbSQL.ToString();

                // Montando parametros
                clsDados.LimparParametro();
                clsFacil.AdicionarParametro(ref strSQL, "20" + CTe.CteSimpProc.protCTe.InfProt.chCTe.Substring(2, 4), "@dtr_referencia", SqlDbType.Int);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.protCTe.InfProt.chCTe, "@cod_chave_acesso", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.infModal.versaoModal, "@des_versao_modal", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.infModal.multimodal.COTM, "@num_certificado_operador", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.infModal.multimodal.indNegociavel, "@ind_negociavel", SqlDbType.SmallInt);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.infModal.multimodal.seg.infSeg.xSeg, "@nom_seguradora", SqlDbType.VarChar);

                if (CTe.CteSimpProc.CTeSimp.infCte.infModal.multimodal.seg.infSeg.CNPJ != null && CTe.CteSimpProc.CTeSimp.infCte.infModal.multimodal.seg.infSeg.CNPJ != string.Empty)
                {
                    clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.infModal.multimodal.seg.infSeg.CNPJ.Substring(0, 8), "@num_cnpj_cpf_base_seguradora", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.infModal.multimodal.seg.infSeg.CNPJ.Substring(8, 4), "@num_cnpj_cpf_filial_seguradora", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.infModal.multimodal.seg.infSeg.CNPJ.Substring(12, 2), "@dig_cnpj_cpf_seguradora", SqlDbType.SmallInt);
                }
                else
                {
                    clsFacil.AdicionarParametro(ref strSQL, string.Empty, "@num_cnpj_cpf_base_seguradora", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, string.Empty, "@num_cnpj_cpf_filial_seguradora", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, string.Empty, "@dig_cnpj_cpf_seguradora", SqlDbType.SmallInt);
                }
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.infModal.multimodal.seg.nApol, "@num_apolice_seguro", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteSimpProc.CTeSimp.infCte.infModal.multimodal.seg.nAver, "@num_averbacao", SqlDbType.VarChar);

                strRetorno = strSQL;

            }
            catch (Exception ex)
            {
                throw new ArgumentException($"[{strMetodo}]: " + ex.Message);
            }
            finally
            {
                stbSQL = null;
                clsDados = null;
                clsFacil = null;
            }

            return strRetorno;
        }

        public string InserirXML_detalhe_xml_conhecimento_transporte_eletronico_simp_modal_rodoviario(Common.SerializableClasses.CTe.xmlCTeSimp_v400.proc CTe)
        {
            // Classes e variaveis utilizadas
            AcessoDados clsDados = new AcessoDados();
            Facilitador clsFacil = new Facilitador();
            StringBuilder stbSQL = new StringBuilder();
            string strRetorno = string.Empty;
            string strMetodo = System.Reflection.MethodBase.GetCurrentMethod().Name;
            string strSQL = string.Empty;
            string strSQL_aux = string.Empty;

            try
            {

                int Lcont = 0;
                if (CTe.CteSimpProc.CTeSimp.infCte.infModal.rodo.occ.Count != 0)
                {
                    for (Lcont = 0; Lcont <= CTe.CteSimpProc.CTeSimp.infCte.infModal.rodo.occ.Count - 1; Lcont++)
                    {
                        strSQL_aux = string.Empty;
                        stbSQL.Clear();
                        // Montando query a ser executada
                        stbSQL.Append(clsFacil.MontarQuery("INSERT INTO cte.detalhe_xml_conhecimento_transporte_eletronico_simp_modal_rodoviario "));
                        stbSQL.Append(clsFacil.MontarQuery("( "));
                        stbSQL.Append(clsFacil.MontarQuery("  dtr_referencia, "));
                        stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso, "));
                        stbSQL.Append(clsFacil.MontarQuery("  seq_detalhe_xml_conhecimento_transporte_eletronico_simp_modal_rodoviario, "));
                        stbSQL.Append(clsFacil.MontarQuery("  des_versao_modal, "));
                        stbSQL.Append(clsFacil.MontarQuery("  cod_registro_nacional_transporte_carga, "));
                        stbSQL.Append(clsFacil.MontarQuery("  num_serie_ordem_coleta, "));
                        stbSQL.Append(clsFacil.MontarQuery("  num_ordem_coleta, "));
                        stbSQL.Append(clsFacil.MontarQuery("  dtc_emissao_ordem_coleta, "));
                        stbSQL.Append(clsFacil.MontarQuery("  num_cnpj_cpf_base_emissor_ordem_coleta, "));
                        stbSQL.Append(clsFacil.MontarQuery("  num_cnpj_cpf_filial_emissor_ordem_coleta, "));
                        stbSQL.Append(clsFacil.MontarQuery("  dig_cnpj_cpf_emissor_ordem_coleta, "));
                        stbSQL.Append(clsFacil.MontarQuery("  cod_interno_transportadora_emissor_ordem_coleta, "));
                        stbSQL.Append(clsFacil.MontarQuery("  num_insc_estad_emissor_ordem_coleta, "));
                        stbSQL.Append(clsFacil.MontarQuery("  sig_unid_federacao_emissor_ordem_coleta, "));
                        stbSQL.Append(clsFacil.MontarQuery("  num_telefone_emissor_ordem_coleta, "));
                        stbSQL.Append(clsFacil.MontarQuery("  dtc_atualizacao "));
                        stbSQL.Append(clsFacil.MontarQuery(") "));
                        stbSQL.Append(clsFacil.MontarQuery("VALUES "));
                        stbSQL.Append(clsFacil.MontarQuery("( "));
                        stbSQL.Append(clsFacil.MontarQuery("  @dtr_referencia, "));
                        stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso, "));
                        stbSQL.Append(clsFacil.MontarQuery("  @seq_detalhe_xml_conhecimento_transporte_eletronico_simp_modal_rodoviario, "));
                        stbSQL.Append(clsFacil.MontarQuery("  @des_versao_modal, "));
                        stbSQL.Append(clsFacil.MontarQuery("  @cod_registro_nacional_transporte_carga, "));
                        stbSQL.Append(clsFacil.MontarQuery("  @num_serie_ordem_coleta, "));
                        stbSQL.Append(clsFacil.MontarQuery("  @num_ordem_coleta, "));
                        stbSQL.Append(clsFacil.MontarQuery("  @dtc_emissao_ordem_coleta, "));
                        stbSQL.Append(clsFacil.MontarQuery("  @num_cnpj_cpf_base_emissor_ordem_coleta, "));
                        stbSQL.Append(clsFacil.MontarQuery("  @num_cnpj_cpf_filial_emissor_ordem_coleta, "));
                        stbSQL.Append(clsFacil.MontarQuery("  @dig_cnpj_cpf_emissor_ordem_coleta, "));
                        stbSQL.Append(clsFacil.MontarQuery("  @cod_interno_transportadora_emissor_ordem_coleta, "));
                        stbSQL.Append(clsFacil.MontarQuery("  @num_insc_estad_emissor_ordem_coleta, "));
                        stbSQL.Append(clsFacil.MontarQuery("  @sig_unid_federacao_emissor_ordem_coleta, "));
                        stbSQL.Append(clsFacil.MontarQuery("  @num_telefone_emissor_ordem_coleta, "));
                        stbSQL.Append(clsFacil.MontarQuery("  getdate() "));
                        stbSQL.Append(clsFacil.MontarQuery(") "));
                        strSQL_aux = stbSQL.ToString();

                        // Montando parametros
                        clsDados.LimparParametro();
                        clsFacil.AdicionarParametro(ref strSQL_aux, "20" + CTe.CteSimpProc.protCTe.InfProt.chCTe.Substring(2, 4), "@dtr_referencia", SqlDbType.Int);
                        clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.protCTe.InfProt.chCTe, "@cod_chave_acesso", SqlDbType.VarChar);
                        clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.CTeSimp.infCte.infModal.versaoModal, "@des_versao_modal", SqlDbType.VarChar);
                        clsFacil.AdicionarParametro(ref strSQL_aux, Convert.ToString(Lcont + 1), "@seq_detalhe_xml_conhecimento_transporte_eletronico_simp_modal_rodoviario", SqlDbType.SmallInt);
                        clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.CTeSimp.infCte.infModal.rodo.RNTRC, "@cod_registro_nacional_transporte_carga", SqlDbType.VarChar);
                        clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.CTeSimp.infCte.infModal.rodo.occ[Lcont].serie, "@num_serie_ordem_coleta", SqlDbType.VarChar);
                        clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.CTeSimp.infCte.infModal.rodo.occ[Lcont].nOcc, "@num_ordem_coleta", SqlDbType.VarChar);
                        clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.CTeSimp.infCte.infModal.rodo.occ[Lcont].dEmi, "@dtc_emissao_ordem_coleta", SqlDbType.VarChar);

                        if (CTe.CteSimpProc.CTeSimp.infCte.infModal.rodo.occ[Lcont].emiOcc.CNPJ != null && CTe.CteSimpProc.CTeSimp.infCte.infModal.rodo.occ[Lcont].emiOcc.CNPJ != string.Empty)
                        {
                            clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.CTeSimp.infCte.infModal.rodo.occ[Lcont].emiOcc.CNPJ.Substring(0, 8), "@num_cnpj_cpf_base_emissor_ordem_coleta", SqlDbType.VarChar);
                            clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.CTeSimp.infCte.infModal.rodo.occ[Lcont].emiOcc.CNPJ.Substring(8, 4), "@num_cnpj_cpf_filial_emissor_ordem_coleta", SqlDbType.VarChar);
                            clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.CTeSimp.infCte.infModal.rodo.occ[Lcont].emiOcc.CNPJ.Substring(12, 2), "@dig_cnpj_cpf_emissor_ordem_coleta", SqlDbType.SmallInt);
                        }
                        else
                        {
                            clsFacil.AdicionarParametro(ref strSQL_aux, string.Empty, "@num_cnpj_cpf_base_emissor_ordem_coleta", SqlDbType.VarChar);
                            clsFacil.AdicionarParametro(ref strSQL_aux, string.Empty, "@num_cnpj_cpf_filial_emissor_ordem_coleta", SqlDbType.VarChar);
                            clsFacil.AdicionarParametro(ref strSQL_aux, string.Empty, "@dig_cnpj_cpf_emissor_ordem_coleta", SqlDbType.SmallInt);
                        }

                        clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.CTeSimp.infCte.infModal.rodo.occ[Lcont].emiOcc.cInt, "@cod_interno_transportadora_emissor_ordem_coleta", SqlDbType.VarChar);
                        clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.CTeSimp.infCte.infModal.rodo.occ[Lcont].emiOcc.IE, "@num_insc_estad_emissor_ordem_coleta", SqlDbType.VarChar);
                        clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.CTeSimp.infCte.infModal.rodo.occ[Lcont].emiOcc.UF, "@sig_unid_federacao_emissor_ordem_coleta", SqlDbType.VarChar);
                        clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.CTeSimp.infCte.infModal.rodo.occ[Lcont].emiOcc.fone, "@num_telefone_emissor_ordem_coleta", SqlDbType.BigInt);
                        strSQL = strSQL + strSQL_aux;
                    }
                }
                else
                {
                    strSQL_aux = string.Empty;
                    stbSQL.Clear();
                    // Montando query a ser executada
                    stbSQL.Append(clsFacil.MontarQuery("INSERT INTO cte.detalhe_xml_conhecimento_transporte_eletronico_simp_modal_rodoviario "));
                    stbSQL.Append(clsFacil.MontarQuery("( "));
                    stbSQL.Append(clsFacil.MontarQuery("  dtr_referencia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso, "));
                    stbSQL.Append(clsFacil.MontarQuery("  seq_detalhe_xml_conhecimento_transporte_eletronico_simp_modal_rodoviario, "));
                    stbSQL.Append(clsFacil.MontarQuery("  des_versao_modal, "));
                    stbSQL.Append(clsFacil.MontarQuery("  cod_registro_nacional_transporte_carga, "));
                    stbSQL.Append(clsFacil.MontarQuery("  num_serie_ordem_coleta, "));
                    stbSQL.Append(clsFacil.MontarQuery("  num_ordem_coleta, "));
                    stbSQL.Append(clsFacil.MontarQuery("  dtc_emissao_ordem_coleta, "));
                    stbSQL.Append(clsFacil.MontarQuery("  num_cnpj_cpf_base_emissor_ordem_coleta, "));
                    stbSQL.Append(clsFacil.MontarQuery("  num_cnpj_cpf_filial_emissor_ordem_coleta, "));
                    stbSQL.Append(clsFacil.MontarQuery("  dig_cnpj_cpf_emissor_ordem_coleta, "));
                    stbSQL.Append(clsFacil.MontarQuery("  cod_interno_transportadora_emissor_ordem_coleta, "));
                    stbSQL.Append(clsFacil.MontarQuery("  num_insc_estad_emissor_ordem_coleta, "));
                    stbSQL.Append(clsFacil.MontarQuery("  sig_unid_federacao_emissor_ordem_coleta, "));
                    stbSQL.Append(clsFacil.MontarQuery("  num_telefone_emissor_ordem_coleta, "));
                    stbSQL.Append(clsFacil.MontarQuery("  dtc_atualizacao "));
                    stbSQL.Append(clsFacil.MontarQuery(") "));
                    stbSQL.Append(clsFacil.MontarQuery("VALUES "));
                    stbSQL.Append(clsFacil.MontarQuery("( "));
                    stbSQL.Append(clsFacil.MontarQuery("  @dtr_referencia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @seq_detalhe_xml_conhecimento_transporte_eletronico_simp_modal_rodoviario, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @des_versao_modal, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @cod_registro_nacional_transporte_carga, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @num_serie_ordem_coleta, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @num_ordem_coleta, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @dtc_emissao_ordem_coleta, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @num_cnpj_cpf_base_emissor_ordem_coleta, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @num_cnpj_cpf_filial_emissor_ordem_coleta, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @dig_cnpj_cpf_emissor_ordem_coleta, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @cod_interno_transportadora_emissor_ordem_coleta, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @num_insc_estad_emissor_ordem_coleta, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @sig_unid_federacao_emissor_ordem_coleta, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @num_telefone_emissor_ordem_coleta, "));
                    stbSQL.Append(clsFacil.MontarQuery("  getdate() "));
                    stbSQL.Append(clsFacil.MontarQuery(") "));
                    strSQL_aux = stbSQL.ToString();

                    // Montando parametros
                    clsDados.LimparParametro();
                    clsFacil.AdicionarParametro(ref strSQL_aux, "20" + CTe.CteSimpProc.protCTe.InfProt.chCTe.Substring(2, 4), "@dtr_referencia", SqlDbType.Int);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.protCTe.InfProt.chCTe, "@cod_chave_acesso", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.CTeSimp.infCte.infModal.versaoModal, "@des_versao_modal", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, "1", "@seq_detalhe_xml_conhecimento_transporte_eletronico_simp_modal_rodoviario", SqlDbType.SmallInt);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteSimpProc.CTeSimp.infCte.infModal.rodo.RNTRC, "@cod_registro_nacional_transporte_carga", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, string.Empty, "@num_serie_ordem_coleta", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, string.Empty, "@num_ordem_coleta", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, string.Empty, "@dtc_emissao_ordem_coleta", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, string.Empty, "@num_cnpj_cpf_base_emissor_ordem_coleta", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, string.Empty, "@num_cnpj_cpf_filial_emissor_ordem_coleta", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, string.Empty, "@dig_cnpj_cpf_emissor_ordem_coleta", SqlDbType.SmallInt);
                    clsFacil.AdicionarParametro(ref strSQL_aux, string.Empty, "@cod_interno_transportadora_emissor_ordem_coleta", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, string.Empty, "@num_insc_estad_emissor_ordem_coleta", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, string.Empty, "@sig_unid_federacao_emissor_ordem_coleta", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, string.Empty, "@num_telefone_emissor_ordem_coleta", SqlDbType.BigInt);
                    strSQL = strSQL + strSQL_aux;
                }

                strRetorno = strSQL;

            }
            catch (Exception ex)
            {
                throw new ArgumentException($"[{strMetodo}]: " + ex.Message);
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

        #endregion

        public string InserirXML(Common.SerializableClasses.CTe.xmlCTeSimp_v400.proc CTe)
        {
            // Classes e variaveis utilizadas
            AcessoDados clsDados = new AcessoDados();
            Facilitador clsFacil = new Facilitador();
            StringBuilder stbSQL = new StringBuilder();
            string strRetorno = string.Empty;
            string strMetodo = System.Reflection.MethodBase.GetCurrentMethod().Name;

            try
            {




            }
            catch (Exception ex)
            {
                throw new ArgumentException($"[{strMetodo}]: " + ex.Message);
            }
            finally
            {
                stbSQL = null;
                clsDados = null;
                clsFacil = null;
            }

            return strRetorno;
        }

    }
}
