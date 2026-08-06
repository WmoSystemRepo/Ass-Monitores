using System;
using System.Data;
using System.Text;

namespace DFe
{
    class ClsInserirCTeOS
    {

        #region "Inserir CTeOS"

        #endregion

        public string InserirXML_detalhe_xml_conhecimento_transporte_eletronico_outro_servico_autorizado(Common.SerializableClasses.CTe.ClsCTeOS_v4.proc CTeOS)
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
                stbSQL.Append(clsFacil.MontarQuery("INSERT INTO cte.detalhe_xml_conhecimento_transporte_eletronico_outro_servico_autorizado "));
                stbSQL.Append(clsFacil.MontarQuery("( "));
                stbSQL.Append(clsFacil.MontarQuery("  dtr_referencia, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_chave_acesso, "));
                stbSQL.Append(clsFacil.MontarQuery("  sig_unid_federacao, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_cfop, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_natureza_operacao, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_tipo_modelo_documento_fiscal, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_serie_conhecimento_transporte_eletronico_outro_servico, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_conhecimento_transporte_eletronico, "));
                stbSQL.Append(clsFacil.MontarQuery("  dtc_emissao_conhecimento_transporte_eletronico_outro_servico, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_forma_emissao_conhecimento_transporte_eletronico_outro_servico, "));
                stbSQL.Append(clsFacil.MontarQuery("  dig_conhecimento_transporte_eletronico_outro_servico, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_tipo_conhecimento_transporte_eletronico, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_tipo_emissao_conhecimento_transporte_eletronico, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_versao_processo_emissao, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_municipio_envio, "));
                stbSQL.Append(clsFacil.MontarQuery("  nom_municipio_envio, "));
                stbSQL.Append(clsFacil.MontarQuery("  sig_unid_federacao_envio, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_tipo_servico_conhecimento_transporte_eletronico_outro_servico, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_insc_estad_tomador, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_municipio_inicio_prestacao, "));
                stbSQL.Append(clsFacil.MontarQuery("  nom_municipio_inicio_prestacao, "));
                stbSQL.Append(clsFacil.MontarQuery("  sig_unid_federacao_inicio_prestacao, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_municipio_fim_prestacao, "));
                stbSQL.Append(clsFacil.MontarQuery("  nom_municipio_fim_prestacao, "));
                stbSQL.Append(clsFacil.MontarQuery("  sig_unid_federacao_fim_prestacao, "));
                stbSQL.Append(clsFacil.MontarQuery("  des_caracteristica_adicional_transporte, "));
                stbSQL.Append(clsFacil.MontarQuery("  des_caracteristica_adicional_servico, "));
                stbSQL.Append(clsFacil.MontarQuery("  nom_funcionario_emissor_cteos, "));
                stbSQL.Append(clsFacil.MontarQuery("  des_observacao_geral, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_cnpj_cpf_base_emitente, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_cnpj_cpf_filial_emitente, "));
                stbSQL.Append(clsFacil.MontarQuery("  dig_cnpj_cpf_emitente, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_insc_estad_emitente, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_insc_estad_substituto_tributario_emitente, "));
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
                stbSQL.Append(clsFacil.MontarQuery("  cod_tipo_regime_tributario, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_cnpj_cpf_base_tomador, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_cnpj_cpf_filial_tomador, "));
                stbSQL.Append(clsFacil.MontarQuery("  dig_cnpj_cpf_tomador, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_insc_estad_tomador, "));
                stbSQL.Append(clsFacil.MontarQuery("  nom_razao_social_tomador, "));
                stbSQL.Append(clsFacil.MontarQuery("  nom_fantasia_tomador, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_telefone_tomador, "));
                stbSQL.Append(clsFacil.MontarQuery("  des_logradouro_tomador, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_endereco_tomador, "));
                stbSQL.Append(clsFacil.MontarQuery("  des_compl_endereco_tomador, "));
                stbSQL.Append(clsFacil.MontarQuery("  des_bairro_tomador, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_municipio_ibge_tomador, "));
                stbSQL.Append(clsFacil.MontarQuery("  nom_municipio_ibge_tomador, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_cep_tomador, "));
                stbSQL.Append(clsFacil.MontarQuery("  sig_unid_federacao_tomador, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_pais_bacen_tomador, "));
                stbSQL.Append(clsFacil.MontarQuery("  nom_pais_bacen_tomador, "));
                stbSQL.Append(clsFacil.MontarQuery("  nom_email_tomador, "));
                stbSQL.Append(clsFacil.MontarQuery("  val_total_prestacao, "));
                stbSQL.Append(clsFacil.MontarQuery("  val_receber, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_cnpj_cpf_base_responsavel_tecnico, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_cnpj_cpf_filial_responsavel_tecnico, "));
                stbSQL.Append(clsFacil.MontarQuery("  dig_cnpj_cpf_responsavel_tecnico, "));
                stbSQL.Append(clsFacil.MontarQuery("  nom_contato_responsavel_tecnico, "));
                stbSQL.Append(clsFacil.MontarQuery("  nom_email__responsavel_tecnico, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_relefone_responsavel_tecnico, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_codigo_seguranca_responsavel_tecnico, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_hash_token_codigo_seguranca_responsavel_tecnico, "));
                stbSQL.Append(clsFacil.MontarQuery("  des_qrcod_dacte, "));
                stbSQL.Append(clsFacil.MontarQuery("  val_base_calculo_icms, "));
                stbSQL.Append(clsFacil.MontarQuery("  val_aliquota_icms, "));
                stbSQL.Append(clsFacil.MontarQuery("  val_icms, "));
                stbSQL.Append(clsFacil.MontarQuery("  prc_reducao_base_calculo, "));
                stbSQL.Append(clsFacil.MontarQuery("  val_credito_outorgado_presumido, "));
                stbSQL.Append(clsFacil.MontarQuery("  prc_reducao_base_calculo_outra_uf, "));
                stbSQL.Append(clsFacil.MontarQuery("  val_base_calculo_icms_outra_uf, "));
                stbSQL.Append(clsFacil.MontarQuery("  prc_aliquota_icms_outra_uf, "));
                stbSQL.Append(clsFacil.MontarQuery("  val_icms_devido_outra_uf, "));
                stbSQL.Append(clsFacil.MontarQuery("  ind_simples_nacional, "));
                stbSQL.Append(clsFacil.MontarQuery("  val_total_tributos_simples_nacional, "));
                stbSQL.Append(clsFacil.MontarQuery("  des_info_adicional_fiscao, "));
                stbSQL.Append(clsFacil.MontarQuery("  val_base_calculo_prestacao_servico_uf_fim, "));
                stbSQL.Append(clsFacil.MontarQuery("  prc_icms_fundo_combate_pobreza_uf_fim, "));
                stbSQL.Append(clsFacil.MontarQuery("  val_aliquota_interna_uf_fim, "));
                stbSQL.Append(clsFacil.MontarQuery("  val_aliquota_interestadual_uf_envolvida, "));
                stbSQL.Append(clsFacil.MontarQuery("  val_icms_fundo_combate_pobreza_uf_fim, "));
                stbSQL.Append(clsFacil.MontarQuery("  val_icms_partilha_uf_fim, "));
                stbSQL.Append(clsFacil.MontarQuery("  val_icms_partilha_uf_inicio, "));
                stbSQL.Append(clsFacil.MontarQuery("  val_pis, "));
                stbSQL.Append(clsFacil.MontarQuery("  val_confins, "));
                stbSQL.Append(clsFacil.MontarQuery("  val_ir, "));
                stbSQL.Append(clsFacil.MontarQuery("  val_inss, "));
                stbSQL.Append(clsFacil.MontarQuery("  val_csll, "));
                stbSQL.Append(clsFacil.MontarQuery("  des_servico_prestado, "));
                stbSQL.Append(clsFacil.MontarQuery("  qtd_carga, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso_cte_original, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso_cte_cancelado, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_fatura, "));
                stbSQL.Append(clsFacil.MontarQuery("  val_original_fatura, "));
                stbSQL.Append(clsFacil.MontarQuery("  val_desconto_fatura, "));
                stbSQL.Append(clsFacil.MontarQuery("  val_liquido_fatura, "));
                stbSQL.Append(clsFacil.MontarQuery("  dtc_atualizacao, "));
                stbSQL.Append(clsFacil.MontarQuery("  val_icms_desoneracao, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_beneficio_fiscal_unid_federacao, "));
                stbSQL.Append(clsFacil.MontarQuery("  dtc_entrada_contingencia, "));
                stbSQL.Append(clsFacil.MontarQuery("  des_justificativa_entrada_contingencia, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_tipo_modal_transporte_documento_fiscal_eletronico, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_tipo_classificacao_tributaria "));
                stbSQL.Append(clsFacil.MontarQuery(") "));
                stbSQL.Append(clsFacil.MontarQuery("VALUES "));
                stbSQL.Append(clsFacil.MontarQuery("( "));
                stbSQL.Append(clsFacil.MontarQuery("  @dtr_referencia, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_chave_acesso, "));
                stbSQL.Append(clsFacil.MontarQuery("  @sig_unid_federacao, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_cfop, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_natureza_operacao, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_tipo_modelo_documento_fiscal, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_serie_conhecimento_transporte_eletronico_outro_servico, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_conhecimento_transporte_eletronico, "));
                stbSQL.Append(clsFacil.MontarQuery("  @dtc_emissao_conhecimento_transporte_eletronico_outro_servico, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_forma_emissao_conhecimento_transporte_eletronico_outro_servico, "));
                stbSQL.Append(clsFacil.MontarQuery("  @dig_conhecimento_transporte_eletronico_outro_servico, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_tipo_conhecimento_transporte_eletronico, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_tipo_emissao_conhecimento_transporte_eletronico, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_versao_processo_emissao, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_municipio_envio, "));
                stbSQL.Append(clsFacil.MontarQuery("  @nom_municipio_envio, "));
                stbSQL.Append(clsFacil.MontarQuery("  @sig_unid_federacao_envio, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_tipo_servico_conhecimento_transporte_eletronico_outro_servico, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_insc_estad_tomador, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_municipio_inicio_prestacao, "));
                stbSQL.Append(clsFacil.MontarQuery("  @nom_municipio_inicio_prestacao, "));
                stbSQL.Append(clsFacil.MontarQuery("  @sig_unid_federacao_inicio_prestacao, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_municipio_fim_prestacao, "));
                stbSQL.Append(clsFacil.MontarQuery("  @nom_municipio_fim_prestacao, "));
                stbSQL.Append(clsFacil.MontarQuery("  @sig_unid_federacao_fim_prestacao, "));
                stbSQL.Append(clsFacil.MontarQuery("  @des_caracteristica_adicional_transporte, "));
                stbSQL.Append(clsFacil.MontarQuery("  @des_caracteristica_adicional_servico, "));
                stbSQL.Append(clsFacil.MontarQuery("  @nom_funcionario_emissor_cteos, "));
                stbSQL.Append(clsFacil.MontarQuery("  @des_observacao_geral, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_cnpj_cpf_base_emitente, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_cnpj_cpf_filial_emitente, "));
                stbSQL.Append(clsFacil.MontarQuery("  @dig_cnpj_cpf_emitente, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_insc_estad_emitente, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_insc_estad_substituto_tributario_emitente, "));
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
                stbSQL.Append(clsFacil.MontarQuery("  @cod_tipo_regime_tributario, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_cnpj_cpf_base_tomador, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_cnpj_cpf_filial_tomador, "));
                stbSQL.Append(clsFacil.MontarQuery("  @dig_cnpj_cpf_tomador, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_insc_estad_tomador, "));
                stbSQL.Append(clsFacil.MontarQuery("  @nom_razao_social_tomador, "));
                stbSQL.Append(clsFacil.MontarQuery("  @nom_fantasia_tomador, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_telefone_tomador, "));
                stbSQL.Append(clsFacil.MontarQuery("  @des_logradouro_tomador, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_endereco_tomador, "));
                stbSQL.Append(clsFacil.MontarQuery("  @des_compl_endereco_tomador, "));
                stbSQL.Append(clsFacil.MontarQuery("  @des_bairro_tomador, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_municipio_ibge_tomador, "));
                stbSQL.Append(clsFacil.MontarQuery("  @nom_municipio_ibge_tomador, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_cep_tomador, "));
                stbSQL.Append(clsFacil.MontarQuery("  @sig_unid_federacao_tomador, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_pais_bacen_tomador, "));
                stbSQL.Append(clsFacil.MontarQuery("  @nom_pais_bacen_tomador, "));
                stbSQL.Append(clsFacil.MontarQuery("  @nom_email_tomador, "));
                stbSQL.Append(clsFacil.MontarQuery("  @val_total_prestacao, "));
                stbSQL.Append(clsFacil.MontarQuery("  @val_receber, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_cnpj_cpf_base_responsavel_tecnico, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_cnpj_cpf_filial_responsavel_tecnico, "));
                stbSQL.Append(clsFacil.MontarQuery("  @dig_cnpj_cpf_responsavel_tecnico, "));
                stbSQL.Append(clsFacil.MontarQuery("  @nom_contato_responsavel_tecnico, "));
                stbSQL.Append(clsFacil.MontarQuery("  @nom_email__responsavel_tecnico, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_relefone_responsavel_tecnico, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_codigo_seguranca_responsavel_tecnico, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_hash_token_codigo_seguranca_responsavel_tecnico, "));
                stbSQL.Append(clsFacil.MontarQuery("  @des_qrcod_dacte, "));
                stbSQL.Append(clsFacil.MontarQuery("  @val_base_calculo_icms, "));
                stbSQL.Append(clsFacil.MontarQuery("  @val_aliquota_icms, "));
                stbSQL.Append(clsFacil.MontarQuery("  @val_icms, "));
                stbSQL.Append(clsFacil.MontarQuery("  @prc_reducao_base_calculo, "));
                stbSQL.Append(clsFacil.MontarQuery("  @val_credito_outorgado_presumido, "));
                stbSQL.Append(clsFacil.MontarQuery("  @prc_reducao_base_calculo_outra_uf, "));
                stbSQL.Append(clsFacil.MontarQuery("  @val_base_calculo_icms_outra_uf, "));
                stbSQL.Append(clsFacil.MontarQuery("  @prc_aliquota_icms_outra_uf, "));
                stbSQL.Append(clsFacil.MontarQuery("  @val_icms_devido_outra_uf, "));
                stbSQL.Append(clsFacil.MontarQuery("  @ind_simples_nacional, "));
                stbSQL.Append(clsFacil.MontarQuery("  @val_total_tributos_simples_nacional, "));
                stbSQL.Append(clsFacil.MontarQuery("  @des_info_adicional_fiscao, "));
                stbSQL.Append(clsFacil.MontarQuery("  @val_base_calculo_prestacao_servico_uf_fim, "));
                stbSQL.Append(clsFacil.MontarQuery("  @prc_icms_fundo_combate_pobreza_uf_fim, "));
                stbSQL.Append(clsFacil.MontarQuery("  @val_aliquota_interna_uf_fim, "));
                stbSQL.Append(clsFacil.MontarQuery("  @val_aliquota_interestadual_uf_envolvida, "));
                stbSQL.Append(clsFacil.MontarQuery("  @val_icms_fundo_combate_pobreza_uf_fim, "));
                stbSQL.Append(clsFacil.MontarQuery("  @val_icms_partilha_uf_fim, "));
                stbSQL.Append(clsFacil.MontarQuery("  @val_icms_partilha_uf_inicio, "));
                stbSQL.Append(clsFacil.MontarQuery("  @val_pis, "));
                stbSQL.Append(clsFacil.MontarQuery("  @val_confins, "));
                stbSQL.Append(clsFacil.MontarQuery("  @val_ir, "));
                stbSQL.Append(clsFacil.MontarQuery("  @val_inss, "));
                stbSQL.Append(clsFacil.MontarQuery("  @val_csll, "));
                stbSQL.Append(clsFacil.MontarQuery("  @des_servico_prestado, "));
                stbSQL.Append(clsFacil.MontarQuery("  @qtd_carga, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso_cte_original, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso_cte_cancelado, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_fatura, "));
                stbSQL.Append(clsFacil.MontarQuery("  @val_original_fatura, "));
                stbSQL.Append(clsFacil.MontarQuery("  @val_desconto_fatura, "));
                stbSQL.Append(clsFacil.MontarQuery("  @val_liquido_fatura, "));
                stbSQL.Append(clsFacil.MontarQuery("  getdate(), "));
                stbSQL.Append(clsFacil.MontarQuery("  @val_icms_desoneracao, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_beneficio_fiscal_unid_federacao, "));
                stbSQL.Append(clsFacil.MontarQuery("  @dtc_entrada_contingencia, "));
                stbSQL.Append(clsFacil.MontarQuery("  @des_justificativa_entrada_contingencia, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_tipo_modal_transporte_documento_fiscal_eletronico, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_tipo_classificacao_tributaria "));
                stbSQL.Append(clsFacil.MontarQuery(") "));
                string strSQL = stbSQL.ToString();

                // Montando parametros
                clsDados.LimparParametro();
                clsFacil.AdicionarParametro(ref strSQL, "20" + CTeOS.cteOSProc.protCTe.InfProt.chCTe.Substring(2, 4), "@dtr_referencia", SqlDbType.Int);
                clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.protCTe.InfProt.chCTe, "@cod_chave_acesso", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCte.ide.cCT, "@num_chave_acesso", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCte.ide.cUF, "@sig_unid_federacao", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCte.ide.CFOP, "@num_cfop", SqlDbType.Int);
                clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCte.ide.natOp, "@cod_natureza_operacao", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCte.ide.mod, "@cod_tipo_modelo_documento_fiscal", SqlDbType.SmallInt);
                clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCte.ide.serie, "@num_serie_conhecimento_transporte_eletronico_outro_servico", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCte.ide.nCT, "@num_conhecimento_transporte_eletronico", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCte.ide.dhEmi, "@dtc_emissao_conhecimento_transporte_eletronico_outro_servico", SqlDbType.DateTime);
                clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCte.ide.tpEmis, "@cod_forma_emissao_conhecimento_transporte_eletronico_outro_servico", SqlDbType.SmallInt);
                clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCte.ide.cDV, "@dig_conhecimento_transporte_eletronico_outro_servico", SqlDbType.SmallInt);
                clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCte.ide.tpCTe, "@cod_tipo_conhecimento_transporte_eletronico", SqlDbType.SmallInt);
                clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCte.ide.procEmi, "@cod_tipo_emissao_conhecimento_transporte_eletronico", SqlDbType.SmallInt);
                clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCte.ide.verProc, "@num_versao_processo_emissao", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCte.ide.cMunEnv, "@cod_municipio_envio", SqlDbType.Int);
                clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCte.ide.xMunEnv, "@nom_municipio_envio", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCte.ide.UFEnv, "@sig_unid_federacao_envio", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCte.ide.tpServ, "@cod_tipo_servico_conhecimento_transporte_eletronico_outro_servico", SqlDbType.SmallInt);
                clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCte.ide.indIEToma, "@cod_insc_estad_tomador", SqlDbType.SmallInt);
                clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCte.ide.cMunIni, "@cod_municipio_inicio_prestacao", SqlDbType.Int);
                clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCte.ide.xMunIni, "@nom_municipio_inicio_prestacao", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCte.ide.UFIni, "@sig_unid_federacao_inicio_prestacao", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCte.ide.cMunFim, "@cod_municipio_fim_prestacao", SqlDbType.Int);
                clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCte.ide.xMunFim, "@nom_municipio_fim_prestacao", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCte.ide.UFFim, "@sig_unid_federacao_fim_prestacao", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCte.compl.xCaracAd, "@des_caracteristica_adicional_transporte", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCte.compl.xCaracSer, "@des_caracteristica_adicional_servico", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCte.compl.xEmi, "@nom_funcionario_emissor_cteos", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCte.compl.xObs, "@des_observacao_geral", SqlDbType.VarChar);

                if (CTeOS.cteOSProc.CTeOS.infCte.emit.CNPJ != null && CTeOS.cteOSProc.CTeOS.infCte.emit.CNPJ != string.Empty)
                {
                    clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCte.emit.CNPJ.Substring(0, 8), "@num_cnpj_cpf_base_emitente", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCte.emit.CNPJ.Substring(8, 4), "@num_cnpj_cpf_filial_emitente", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCte.emit.CNPJ.Substring(12, 2), "@dig_cnpj_cpf_emitente", SqlDbType.SmallInt);
                }
                else
                {
                    clsFacil.AdicionarParametro(ref strSQL, string.Empty, "@num_cnpj_cpf_base_emitente", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, string.Empty, "@num_cnpj_cpf_filial_emitente", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, string.Empty, "@dig_cnpj_cpf_emitente", SqlDbType.SmallInt);
                }

                clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCte.emit.IE, "@num_insc_estad_emitente", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCte.emit.IEST, "@num_insc_estad_substituto_tributario_emitente", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCte.emit.xNome, "@nom_razao_social_emitente", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCte.emit.xFant, "@nom_fantasia_emitente", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCte.emit.enderEmit.xLgr, "@des_logradouro_emitente", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCte.emit.enderEmit.nro, "@num_endereco_emitente", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCte.emit.enderEmit.xCpl, "@des_compl_endereco_emitente", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCte.emit.enderEmit.xBairro, "@des_bairro_emitente", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCte.emit.enderEmit.cMun, "@cod_municipio_ibge_emitente", SqlDbType.Int);
                clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCte.emit.enderEmit.xMun, "@nom_municipio_ibge_emitente", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCte.emit.enderEmit.CEP, "@num_cep_emitente", SqlDbType.Int);
                clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCte.emit.enderEmit.UF, "@sig_unid_federacao_emitente", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCte.emit.enderEmit.fone, "@num_telefone_emitente", SqlDbType.BigInt);
                clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCte.emit.CRT, "@cod_tipo_regime_tributario", SqlDbType.SmallInt);

                if (CTeOS.cteOSProc.CTeOS.infCte.toma.CPF != null && CTeOS.cteOSProc.CTeOS.infCte.toma.CPF != string.Empty)
                {
                    clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCte.toma.CPF.Substring(0, 9), "@num_cnpj_cpf_base_tomador", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, "0", "@num_cnpj_cpf_filial_tomador", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCte.toma.CPF.Substring(9, 2), "@dig_cnpj_cpf_tomador", SqlDbType.SmallInt);
                }
                if (CTeOS.cteOSProc.CTeOS.infCte.toma.CNPJ != null && CTeOS.cteOSProc.CTeOS.infCte.toma.CNPJ != string.Empty)
                {
                    clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCte.toma.CNPJ.Substring(0, 8), "@num_cnpj_cpf_base_tomador", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCte.toma.CNPJ.Substring(8, 4), "@num_cnpj_cpf_filial_tomador", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCte.toma.CNPJ.Substring(12, 2), "@dig_cnpj_cpf_tomador", SqlDbType.SmallInt);
                }
                if ((CTeOS.cteOSProc.CTeOS.infCte.toma.CPF == null && CTeOS.cteOSProc.CTeOS.infCte.toma.CNPJ == null) || (CTeOS.cteOSProc.CTeOS.infCte.toma.CPF == null && CTeOS.cteOSProc.CTeOS.infCte.toma.CNPJ == string.Empty) || (CTeOS.cteOSProc.CTeOS.infCte.toma.CPF == string.Empty  && CTeOS.cteOSProc.CTeOS.infCte.toma.CNPJ == null))
                {
                    clsFacil.AdicionarParametro(ref strSQL, string.Empty, "@num_cnpj_cpf_base_tomador", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, string.Empty, "@num_cnpj_cpf_filial_tomador", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, string.Empty, "@dig_cnpj_cpf_tomador", SqlDbType.SmallInt);
                }

                clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCte.toma.IE, "@num_insc_estad_tomador", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCte.toma.xNome, "@nom_razao_social_tomador", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCte.toma.xFant, "@nom_fantasia_tomador", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCte.toma.fone, "@num_telefone_tomador", SqlDbType.BigInt);
                clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCte.toma.enderToma.xCpl, "@des_logradouro_tomador", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCte.toma.enderToma.nro, "@num_endereco_tomador", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCte.toma.enderToma.xCpl, "@des_compl_endereco_tomador", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCte.toma.enderToma.xBairro, "@des_bairro_tomador", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCte.toma.enderToma.cMun, "@cod_municipio_ibge_tomador", SqlDbType.Int);
                clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCte.toma.enderToma.xMun, "@nom_municipio_ibge_tomador", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCte.toma.enderToma.CEP, "@num_cep_tomador", SqlDbType.Int);
                clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCte.toma.enderToma.UF, "@sig_unid_federacao_tomador", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCte.toma.enderToma.cPais, "@cod_pais_bacen_tomador", SqlDbType.SmallInt);
                clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCte.toma.enderToma.xPais, "@nom_pais_bacen_tomador", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCte.toma.email, "@nom_email_tomador", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTeOS.cteOSProc.CTeOS.infCte.vPrest.vTPrest), "@val_total_prestacao", SqlDbType.Decimal);
                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTeOS.cteOSProc.CTeOS.infCte.vPrest.vRec), "@val_receber", SqlDbType.Decimal);

                if (CTeOS.cteOSProc.CTeOS.infCte.infRespTec.CNPJ != null && CTeOS.cteOSProc.CTeOS.infCte.infRespTec.CNPJ != string.Empty)
                {
                    clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCte.infRespTec.CNPJ.Substring(0, 8), "@num_cnpj_cpf_base_responsavel_tecnico", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCte.infRespTec.CNPJ.Substring(8, 4), "@num_cnpj_cpf_filial_responsavel_tecnico", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCte.infRespTec.CNPJ.Substring(12, 2), "@dig_cnpj_cpf_responsavel_tecnico", SqlDbType.SmallInt);
                }
                else
                {
                    clsFacil.AdicionarParametro(ref strSQL, string.Empty, "@num_cnpj_cpf_base_responsavel_tecnico", SqlDbType.Int);
                    clsFacil.AdicionarParametro(ref strSQL, string.Empty, "@num_cnpj_cpf_filial_responsavel_tecnico", SqlDbType.SmallInt);
                    clsFacil.AdicionarParametro(ref strSQL, string.Empty, "@dig_cnpj_cpf_responsavel_tecnico", SqlDbType.SmallInt);
                }
                clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCte.infRespTec.xContato, "@nom_contato_responsavel_tecnico", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCte.infRespTec.email, "@nom_email__responsavel_tecnico", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCte.infRespTec.fone, "@num_relefone_responsavel_tecnico", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCte.infRespTec.idCSRT, "@num_codigo_seguranca_responsavel_tecnico", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCte.infRespTec.hashCSRT, "@num_hash_token_codigo_seguranca_responsavel_tecnico", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCTeSupl.qrCodCTe, "@des_qrcod_dacte", SqlDbType.VarChar);
                //ICMS
                string CST = string.Empty, vBC = string.Empty, pICMS = string.Empty, vICMS = string.Empty, pRedBC = string.Empty, vCred = string.Empty, vICMSDeson = string.Empty, cBenef = string.Empty;
                if (CTeOS.cteOSProc.CTeOS.infCte.imp.ICMS.ICMS00.CST != null)
                {
                    CST = CTeOS.cteOSProc.CTeOS.infCte.imp.ICMS.ICMS00.CST;
                    vBC = Convert.ToString(CTeOS.cteOSProc.CTeOS.infCte.imp.ICMS.ICMS00.vBC);
                    pICMS = Convert.ToString(CTeOS.cteOSProc.CTeOS.infCte.imp.ICMS.ICMS00.pICMS);
                    vICMS = Convert.ToString(CTeOS.cteOSProc.CTeOS.infCte.imp.ICMS.ICMS00.vICMS);
                }
                if (CTeOS.cteOSProc.CTeOS.infCte.imp.ICMS.ICMS20.CST != null)
                {
                    CST = CTeOS.cteOSProc.CTeOS.infCte.imp.ICMS.ICMS20.CST;
                    pRedBC = Convert.ToString(CTeOS.cteOSProc.CTeOS.infCte.imp.ICMS.ICMS20.pRedBC);
                    vBC = Convert.ToString(CTeOS.cteOSProc.CTeOS.infCte.imp.ICMS.ICMS20.vBC);
                    pICMS = Convert.ToString(CTeOS.cteOSProc.CTeOS.infCte.imp.ICMS.ICMS20.pICMS);
                    vICMS = Convert.ToString(CTeOS.cteOSProc.CTeOS.infCte.imp.ICMS.ICMS20.vICMS);
                    vICMSDeson = Convert.ToString(CTeOS.cteOSProc.CTeOS.infCte.imp.ICMS.ICMS20.vICMSDeson);
                    cBenef = CTeOS.cteOSProc.CTeOS.infCte.imp.ICMS.ICMS20.cBenef;
                }
                if (CTeOS.cteOSProc.CTeOS.infCte.imp.ICMS.ICMS45.CST != null)
                {
                    CST = CTeOS.cteOSProc.CTeOS.infCte.imp.ICMS.ICMS45.CST;
                    vICMSDeson = Convert.ToString(CTeOS.cteOSProc.CTeOS.infCte.imp.ICMS.ICMS45.vICMSDeson);
                    cBenef = CTeOS.cteOSProc.CTeOS.infCte.imp.ICMS.ICMS45.cBenef;
                }
                if (CTeOS.cteOSProc.CTeOS.infCte.imp.ICMS.ICMS90.CST != null)
                {
                    CST = CTeOS.cteOSProc.CTeOS.infCte.imp.ICMS.ICMS90.CST;
                    pRedBC = Convert.ToString(CTeOS.cteOSProc.CTeOS.infCte.imp.ICMS.ICMS90.pRedBC);
                    vBC = Convert.ToString(CTeOS.cteOSProc.CTeOS.infCte.imp.ICMS.ICMS90.vBC);
                    pICMS = Convert.ToString(CTeOS.cteOSProc.CTeOS.infCte.imp.ICMS.ICMS90.pICMS);
                    vICMS = Convert.ToString(CTeOS.cteOSProc.CTeOS.infCte.imp.ICMS.ICMS90.vICMS);
                    vCred = Convert.ToString(CTeOS.cteOSProc.CTeOS.infCte.imp.ICMS.ICMS90.vCred);
                    vICMSDeson = Convert.ToString(CTeOS.cteOSProc.CTeOS.infCte.imp.ICMS.ICMS90.vICMSDeson);
                    cBenef = CTeOS.cteOSProc.CTeOS.infCte.imp.ICMS.ICMS90.cBenef;
                }
                if (CTeOS.cteOSProc.CTeOS.infCte.imp.ICMS.ICMSOutraUF.CST != null)
                {
                    CST = CTeOS.cteOSProc.CTeOS.infCte.imp.ICMS.ICMSOutraUF.CST;
                    vICMSDeson = Convert.ToString(CTeOS.cteOSProc.CTeOS.infCte.imp.ICMS.ICMSOutraUF.vICMSDeson);
                    cBenef = CTeOS.cteOSProc.CTeOS.infCte.imp.ICMS.ICMSOutraUF.cBenef;
                }
                if (CTeOS.cteOSProc.CTeOS.infCte.imp.ICMS.ICMSSN.CST != null)
                {
                    CST = CTeOS.cteOSProc.CTeOS.infCte.imp.ICMS.ICMSSN.CST;
                }

                clsFacil.AdicionarParametro(ref strSQL, CST, "@cod_tipo_classificacao_tributaria", SqlDbType.VarChar); //CST
                clsFacil.AdicionarParametro(ref strSQL, vBC, "@val_base_calculo_icms", SqlDbType.Decimal); //vBC
                clsFacil.AdicionarParametro(ref strSQL, pICMS, "@val_aliquota_icms", SqlDbType.Decimal); //pICMS
                clsFacil.AdicionarParametro(ref strSQL, vICMS, "@val_icms", SqlDbType.Decimal); //vICMS
                clsFacil.AdicionarParametro(ref strSQL, pRedBC, "@prc_reducao_base_calculo", SqlDbType.Decimal); //pRedBC
                clsFacil.AdicionarParametro(ref strSQL, vCred, "@val_credito_outorgado_presumido", SqlDbType.Decimal); //vCred
                clsFacil.AdicionarParametro(ref strSQL, vICMSDeson, "@val_icms_desoneracao", SqlDbType.Decimal); //vICMSDeson
                clsFacil.AdicionarParametro(ref strSQL, cBenef, "@cod_beneficio_fiscal_unid_federacao", SqlDbType.VarChar); //cBenef

                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTeOS.cteOSProc.CTeOS.infCte.imp.ICMS.ICMSOutraUF.pRedBCOutraUF), "@prc_reducao_base_calculo_outra_uf", SqlDbType.Decimal);
                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTeOS.cteOSProc.CTeOS.infCte.imp.ICMS.ICMSOutraUF.vBCOutraUF), "@val_base_calculo_icms_outra_uf", SqlDbType.Decimal);
                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTeOS.cteOSProc.CTeOS.infCte.imp.ICMS.ICMSOutraUF.pICMSOutraUF), "@prc_aliquota_icms_outra_uf", SqlDbType.Decimal);
                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTeOS.cteOSProc.CTeOS.infCte.imp.ICMS.ICMSOutraUF.vICMSOutraUF), "@val_icms_devido_outra_uf", SqlDbType.Decimal);

                clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCte.imp.ICMS.ICMSSN.indSN, "@ind_simples_nacional", SqlDbType.SmallInt);
                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTeOS.cteOSProc.CTeOS.infCte.imp.vTotTrib), "@val_total_tributos_simples_nacional", SqlDbType.Decimal);
                clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCte.imp.infAdFisco, "@des_info_adicional_fiscao", SqlDbType.VarChar);

                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTeOS.cteOSProc.CTeOS.infCte.imp.ICMSUFFim.vBCUFFim), "@val_base_calculo_prestacao_servico_uf_fim", SqlDbType.Decimal);
                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTeOS.cteOSProc.CTeOS.infCte.imp.ICMSUFFim.pFCPUFFim), "@prc_icms_fundo_combate_pobreza_uf_fim", SqlDbType.Decimal);
                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTeOS.cteOSProc.CTeOS.infCte.imp.ICMSUFFim.pICMSUFFim), "@val_aliquota_interna_uf_fim", SqlDbType.Decimal);
                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTeOS.cteOSProc.CTeOS.infCte.imp.ICMSUFFim.pICMSInter), "@val_aliquota_interestadual_uf_envolvida", SqlDbType.Decimal);
                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTeOS.cteOSProc.CTeOS.infCte.imp.ICMSUFFim.vFCPUFFim), "@val_icms_fundo_combate_pobreza_uf_fim", SqlDbType.Decimal);
                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTeOS.cteOSProc.CTeOS.infCte.imp.ICMSUFFim.vICMSUFFim), "@val_icms_partilha_uf_fim", SqlDbType.Decimal);
                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTeOS.cteOSProc.CTeOS.infCte.imp.ICMSUFFim.vICMSUFIni), "@val_icms_partilha_uf_inicio", SqlDbType.Decimal);

                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTeOS.cteOSProc.CTeOS.infCte.imp.infTribFed.vPIS), "@val_pis", SqlDbType.Decimal);
                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTeOS.cteOSProc.CTeOS.infCte.imp.infTribFed.vCOFINS), "@val_confins", SqlDbType.Decimal);
                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTeOS.cteOSProc.CTeOS.infCte.imp.infTribFed.vIR), "@val_ir", SqlDbType.Decimal);
                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTeOS.cteOSProc.CTeOS.infCte.imp.infTribFed.vINSS), "@val_inss", SqlDbType.Decimal);
                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTeOS.cteOSProc.CTeOS.infCte.imp.infTribFed.vCSLL), "@val_csll", SqlDbType.Decimal);

                clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCte.infCTeNorm.infServico.xDescServ, "@des_servico_prestado", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTeOS.cteOSProc.CTeOS.infCte.infCTeNorm.infServico.infQ.qCarga), "@qtd_carga", SqlDbType.Decimal);
                clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCte.infCTeNorm.infCteSub.chCte, "@cod_chave_acesso_cte_original", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCte.infCTeNorm.refCTeCanc, "@cod_chave_acesso_cte_cancelado", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCte.infCTeNorm.cobr.fat.nFat, "@num_fatura", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTeOS.cteOSProc.CTeOS.infCte.infCTeNorm.cobr.fat.vOrig), "@val_original_fatura", SqlDbType.Decimal);
                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTeOS.cteOSProc.CTeOS.infCte.infCTeNorm.cobr.fat.vDesc), "@val_desconto_fatura", SqlDbType.Decimal);
                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTeOS.cteOSProc.CTeOS.infCte.infCTeNorm.cobr.fat.vLiq), "@val_liquido_fatura", SqlDbType.Decimal);
                clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCte.ide.dhCont, "@dtc_entrada_contingencia", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCte.ide.xJust, "@des_justificativa_entrada_contingencia", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCte.ide.modal, "@cod_tipo_modal_transporte_documento_fiscal_eletronico", SqlDbType.SmallInt);
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

        public string InserirXML_detalhe_xml_conhecimento_transporte_eletronico_outro_servico_autorizado_download(Common.SerializableClasses.CTe.ClsCTeOS_v4.proc CTeOS)
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
                for (Lcont = 0; Lcont <= CTeOS.cteOSProc.CTeOS.infCte.autXML.Count - 1; Lcont++)
                {
                    strSQL_aux = string.Empty;
                    stbSQL.Clear();
                    // Montando query a ser executada
                    stbSQL.Append(clsFacil.MontarQuery("INSERT INTO cte.detalhe_xml_conhecimento_transporte_eletronico_outro_servico_autorizado_download "));
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
                    clsFacil.AdicionarParametro(ref strSQL_aux, "20" + CTeOS.cteOSProc.protCTe.InfProt.chCTe.Substring(2, 4), "@dtr_referencia", SqlDbType.Int);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTeOS.cteOSProc.protCTe.InfProt.chCTe, "@cod_chave_acesso", SqlDbType.VarChar);

                    if (CTeOS.cteOSProc.CTeOS.infCte.autXML[Lcont].CPF != null && CTeOS.cteOSProc.CTeOS.infCte.autXML[Lcont].CPF != string.Empty)
                    {
                        clsFacil.AdicionarParametro(ref strSQL_aux, CTeOS.cteOSProc.CTeOS.infCte.autXML[Lcont].CPF.Substring(0, 9), "@num_cnpj_cpf_base_autorizado", SqlDbType.VarChar);
                        clsFacil.AdicionarParametro(ref strSQL_aux, "0", "@num_cnpj_cpf_filial_autorizado", SqlDbType.VarChar);
                        clsFacil.AdicionarParametro(ref strSQL_aux, CTeOS.cteOSProc.CTeOS.infCte.autXML[Lcont].CPF.Substring(9, 2), "@dig_cnpj_cpf_autorizado", SqlDbType.SmallInt);
                    }
                    if (CTeOS.cteOSProc.CTeOS.infCte.autXML[Lcont].CNPJ != null && CTeOS.cteOSProc.CTeOS.infCte.autXML[Lcont].CNPJ != string.Empty)
                    {
                        clsFacil.AdicionarParametro(ref strSQL_aux, CTeOS.cteOSProc.CTeOS.infCte.autXML[Lcont].CNPJ.Substring(0, 8), "@num_cnpj_cpf_base_autorizado", SqlDbType.VarChar);
                        clsFacil.AdicionarParametro(ref strSQL_aux, CTeOS.cteOSProc.CTeOS.infCte.autXML[Lcont].CNPJ.Substring(8, 4), "@num_cnpj_cpf_filial_autorizado", SqlDbType.VarChar);
                        clsFacil.AdicionarParametro(ref strSQL_aux, CTeOS.cteOSProc.CTeOS.infCte.autXML[Lcont].CNPJ.Substring(12, 2), "@dig_cnpj_cpf_autorizado", SqlDbType.SmallInt);
                    }
                    strSQL += strSQL_aux;
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

        public string InserirXML_detalhe_xml_conhecimento_transporte_eletronico_outro_servico_complementado(Common.SerializableClasses.CTe.ClsCTeOS_v4.proc CTeOS)
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
                for (Lcont = 0; Lcont <= CTeOS.cteOSProc.CTeOS.infCte.infCteComp.Count - 1; Lcont++)
                {
                    strSQL_aux = string.Empty;
                    stbSQL.Clear();
                    stbSQL.Append(clsFacil.MontarQuery("INSERT INTO cte.detalhe_xml_conhecimento_transporte_eletronico_outro_servico_complementado "));
                    stbSQL.Append(clsFacil.MontarQuery("( "));
                    stbSQL.Append(clsFacil.MontarQuery("  dtr_referencia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso, "));
                    stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso_cte_complementado, "));
                    stbSQL.Append(clsFacil.MontarQuery("  dtc_atualizacao "));
                    stbSQL.Append(clsFacil.MontarQuery(") "));
                    stbSQL.Append(clsFacil.MontarQuery("VALUES "));
                    stbSQL.Append(clsFacil.MontarQuery("( "));
                    stbSQL.Append(clsFacil.MontarQuery("  @dtr_referencia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso_cte_complementado, "));
                    stbSQL.Append(clsFacil.MontarQuery("  getdate() "));
                    stbSQL.Append(clsFacil.MontarQuery(") "));
                    strSQL_aux = stbSQL.ToString();

                    // Montando parametros
                    clsDados.LimparParametro();
                    clsFacil.AdicionarParametro(ref strSQL_aux, "20" + CTeOS.cteOSProc.protCTe.InfProt.chCTe.Substring(2, 4), "@dtr_referencia", SqlDbType.Int);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTeOS.cteOSProc.protCTe.InfProt.chCTe, "@cod_chave_acesso", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTeOS.cteOSProc.CTeOS.infCte.infCteComp[Lcont].chCTe, "@cod_chave_acesso_cte_complementado", SqlDbType.VarChar);
                    strSQL += strSQL_aux;
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

        public string InserirXML_detalhe_xml_conhecimento_transporte_eletronico_outro_servico_info_contrib(Common.SerializableClasses.CTe.ClsCTeOS_v4.proc CTeOS)
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
                for (Lcont = 0; Lcont <= CTeOS.cteOSProc.CTeOS.infCte.compl.ObsCont.Count - 1; Lcont++)
                {
                    strSQL_aux = string.Empty;
                    stbSQL.Clear();
                    // Montando query a ser executada
                    stbSQL.Append(clsFacil.MontarQuery("INSERT INTO cte.detalhe_xml_conhecimento_transporte_eletronico_outro_servico_info_contrib "));
                    stbSQL.Append(clsFacil.MontarQuery("( "));
                    stbSQL.Append(clsFacil.MontarQuery("  dtr_referencia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso, "));
                    stbSQL.Append(clsFacil.MontarQuery("  nom_campo_livre_contribuinte, "));
                    stbSQL.Append(clsFacil.MontarQuery("  des_campo_livre_contribuinte, "));
                    stbSQL.Append(clsFacil.MontarQuery("  dtc_atualizacao "));
                    stbSQL.Append(clsFacil.MontarQuery(") "));
                    stbSQL.Append(clsFacil.MontarQuery("VALUES "));
                    stbSQL.Append(clsFacil.MontarQuery("( "));
                    stbSQL.Append(clsFacil.MontarQuery("  @dtr_referencia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @nom_campo_livre_contribuinte, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @des_campo_livre_contribuinte, "));
                    stbSQL.Append(clsFacil.MontarQuery("  getdate() "));
                    stbSQL.Append(clsFacil.MontarQuery(") "));
                    strSQL_aux = stbSQL.ToString();

                    // Montando parametros
                    clsDados.LimparParametro();
                    clsFacil.AdicionarParametro(ref strSQL_aux, "20" + CTeOS.cteOSProc.protCTe.InfProt.chCTe.Substring(2, 4), "@dtr_referencia", SqlDbType.Int);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTeOS.cteOSProc.protCTe.InfProt.chCTe, "@cod_chave_acesso", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTeOS.cteOSProc.CTeOS.infCte.compl.ObsCont[Lcont].xCampo, "@nom_campo_livre_contribuinte", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTeOS.cteOSProc.CTeOS.infCte.compl.ObsCont[Lcont].xTexto, "@des_campo_livre_contribuinte", SqlDbType.VarChar);
                    strSQL += strSQL_aux;
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

        public string InserirXML_detalhe_xml_conhecimento_transporte_eletronico_outro_servico_info_fisco(Common.SerializableClasses.CTe.ClsCTeOS_v4.proc CTeOS)
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
                for (Lcont = 0; Lcont <= CTeOS.cteOSProc.CTeOS.infCte.compl.ObsFisco.Count - 1; Lcont++)
                {
                    strSQL_aux = string.Empty;
                    stbSQL.Clear();
                    // Montando query a ser executada
                    stbSQL.Append(clsFacil.MontarQuery("INSERT INTO cte.detalhe_xml_conhecimento_transporte_eletronico_outro_servico_info_fisco "));
                    stbSQL.Append(clsFacil.MontarQuery("( "));
                    stbSQL.Append(clsFacil.MontarQuery("  dtr_referencia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso, "));
                    stbSQL.Append(clsFacil.MontarQuery("  nom_campo_livre_fisco, "));
                    stbSQL.Append(clsFacil.MontarQuery("  des_campo_livre_fisco, "));
                    stbSQL.Append(clsFacil.MontarQuery("  dtc_atualizacao "));
                    stbSQL.Append(clsFacil.MontarQuery(") "));
                    stbSQL.Append(clsFacil.MontarQuery("VALUES "));
                    stbSQL.Append(clsFacil.MontarQuery("( "));
                    stbSQL.Append(clsFacil.MontarQuery("  @dtr_referencia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @nom_campo_livre_fisco, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @des_campo_livre_fisco, "));
                    stbSQL.Append(clsFacil.MontarQuery("  getdate() "));
                    stbSQL.Append(clsFacil.MontarQuery(") "));
                    strSQL_aux = stbSQL.ToString();

                    // Montando parametros
                    clsDados.LimparParametro();
                    clsFacil.AdicionarParametro(ref strSQL_aux, "20" + CTeOS.cteOSProc.protCTe.InfProt.chCTe.Substring(2, 4), "@dtr_referencia", SqlDbType.Int);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTeOS.cteOSProc.protCTe.InfProt.chCTe, "@cod_chave_acesso", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTeOS.cteOSProc.CTeOS.infCte.compl.ObsFisco[Lcont].xCampo, "@nom_campo_livre_fisco", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTeOS.cteOSProc.CTeOS.infCte.compl.ObsFisco[Lcont].xTexto, "@des_campo_livre_fisco", SqlDbType.VarChar);
                    strSQL += strSQL_aux;
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

        public string InserirXML_detalhe_xml_conhecimento_transporte_eletronico_outro_servico_componente(Common.SerializableClasses.CTe.ClsCTeOS_v4.proc CTeOS)
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
                for (Lcont = 0; Lcont <= CTeOS.cteOSProc.CTeOS.infCte.vPrest.Comp.Count - 1; Lcont++)
                {
                    strSQL_aux = string.Empty;
                    stbSQL.Clear();
                    // Montando query a ser executada
                    stbSQL.Append(clsFacil.MontarQuery("INSERT INTO cte.detalhe_xml_conhecimento_transporte_eletronico_outro_servico_componente "));
                    stbSQL.Append(clsFacil.MontarQuery("( "));
                    stbSQL.Append(clsFacil.MontarQuery("  dtr_referencia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso, "));
                    stbSQL.Append(clsFacil.MontarQuery("  nom_componente, "));
                    stbSQL.Append(clsFacil.MontarQuery("  val_componente, "));
                    stbSQL.Append(clsFacil.MontarQuery("  dtc_atualizacao "));
                    stbSQL.Append(clsFacil.MontarQuery(") "));
                    stbSQL.Append(clsFacil.MontarQuery("VALUES "));
                    stbSQL.Append(clsFacil.MontarQuery("( "));
                    stbSQL.Append(clsFacil.MontarQuery("  @dtr_referencia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @nom_componente, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @val_componente, "));
                    stbSQL.Append(clsFacil.MontarQuery("  getdate() "));
                    stbSQL.Append(clsFacil.MontarQuery(") "));
                    strSQL_aux = stbSQL.ToString();

                    // Montando parametros
                    clsDados.LimparParametro();
                    clsFacil.AdicionarParametro(ref strSQL_aux, "20" + CTeOS.cteOSProc.protCTe.InfProt.chCTe.Substring(2, 4), "@dtr_referencia", SqlDbType.Int);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTeOS.cteOSProc.protCTe.InfProt.chCTe, "@cod_chave_acesso", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTeOS.cteOSProc.CTeOS.infCte.vPrest.Comp[Lcont].xNome, "@nom_componente", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, Convert.ToString(CTeOS.cteOSProc.CTeOS.infCte.vPrest.Comp[Lcont].vComp), "@val_componente", SqlDbType.Decimal);

                    strSQL += strSQL_aux;
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

        public string InserirXML_detalhe_xml_conhecimento_transporte_eletronico_outro_servico_gtve(Common.SerializableClasses.CTe.ClsCTeOS_v4.proc CTeOS)
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
                for (Lcont = 0; Lcont <= CTeOS.cteOSProc.CTeOS.infCte.infCTeNorm.infGTVe.Count - 1; Lcont++)
                {
                    strSQL_aux = string.Empty;
                    stbSQL.Clear();
                    // Montando query a ser executada
                    stbSQL.Append(clsFacil.MontarQuery("INSERT INTO cte.detalhe_xml_conhecimento_transporte_eletronico_outro_servico_gtve "));
                    stbSQL.Append(clsFacil.MontarQuery("( "));
                    stbSQL.Append(clsFacil.MontarQuery("  dtr_referencia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso, "));
                    stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso_guia_transporte_valor_eletronico, "));
                    stbSQL.Append(clsFacil.MontarQuery("  dtc_atualizacao "));
                    stbSQL.Append(clsFacil.MontarQuery(") "));
                    stbSQL.Append(clsFacil.MontarQuery("VALUES "));
                    stbSQL.Append(clsFacil.MontarQuery("( "));
                    stbSQL.Append(clsFacil.MontarQuery("  @dtr_referencia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso_guia_transporte_valor_eletronico, "));
                    stbSQL.Append(clsFacil.MontarQuery("  getdate() "));
                    stbSQL.Append(clsFacil.MontarQuery(") "));
                    strSQL_aux = stbSQL.ToString();

                    // Montando parametros
                    clsDados.LimparParametro();
                    clsFacil.AdicionarParametro(ref strSQL_aux, "20" + CTeOS.cteOSProc.protCTe.InfProt.chCTe.Substring(2, 4), "@dtr_referencia", SqlDbType.Int);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTeOS.cteOSProc.protCTe.InfProt.chCTe, "@cod_chave_acesso", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTeOS.cteOSProc.CTeOS.infCte.infCTeNorm.infGTVe[Lcont].chCte, "@cod_chave_acesso_guia_transporte_valor_eletronico", SqlDbType.VarChar);
                    strSQL += strSQL_aux;
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

        public string InserirXML_detalhe_xml_conhecimento_transporte_eletronico_outro_servico_componente_valor_gtve(Common.SerializableClasses.CTe.ClsCTeOS_v4.proc CTeOS)
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
                for (i = 0; i <= CTeOS.cteOSProc.CTeOS.infCte.infCTeNorm.infGTVe.Count - 1; i++)
                {
                    int j = 0;
                    for (j = 0; j <= CTeOS.cteOSProc.CTeOS.infCte.infCTeNorm.infGTVe[i].CompinfGTVe.Count - 1; j++)
                    {
                        strSQL_aux = string.Empty;
                        stbSQL.Clear();
                        // Montando query a ser executada
                        stbSQL.Append(clsFacil.MontarQuery("INSERT INTO cte.detalhe_xml_conhecimento_transporte_eletronico_outro_servico_componente_valor_gtve "));
                        stbSQL.Append(clsFacil.MontarQuery("( "));
                        stbSQL.Append(clsFacil.MontarQuery("  dtr_referencia, "));
                        stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso, "));
                        stbSQL.Append(clsFacil.MontarQuery("  cod_tipo_componente, "));
                        stbSQL.Append(clsFacil.MontarQuery("  val_componente, "));
                        stbSQL.Append(clsFacil.MontarQuery("  nom_componente, "));
                        stbSQL.Append(clsFacil.MontarQuery("  dtc_atualizacao, "));
                        stbSQL.Append(clsFacil.MontarQuery("  seq_detalhe_xml_conhecimento_transporte_eletronico_outro_servico_componente_valor_gtve "));
                        stbSQL.Append(clsFacil.MontarQuery(") "));
                        stbSQL.Append(clsFacil.MontarQuery("VALUES "));
                        stbSQL.Append(clsFacil.MontarQuery("( "));
                        stbSQL.Append(clsFacil.MontarQuery("  @dtr_referencia, "));
                        stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso, "));
                        stbSQL.Append(clsFacil.MontarQuery("  @cod_tipo_componente, "));
                        stbSQL.Append(clsFacil.MontarQuery("  @val_componente, "));
                        stbSQL.Append(clsFacil.MontarQuery("  @nom_componente, "));
                        stbSQL.Append(clsFacil.MontarQuery("  getdate(), "));
                        stbSQL.Append(clsFacil.MontarQuery("  @seq_detalhe_xml_conhecimento_transporte_eletronico_outro_servico_componente_valor_gtve "));
                        stbSQL.Append(clsFacil.MontarQuery(") "));
                        strSQL_aux = stbSQL.ToString();

                        // Montando parametros
                        clsDados.LimparParametro();
                        clsFacil.AdicionarParametro(ref strSQL_aux, "20" + CTeOS.cteOSProc.protCTe.InfProt.chCTe.Substring(2, 4), "@dtr_referencia", SqlDbType.Int);
                        clsFacil.AdicionarParametro(ref strSQL_aux, CTeOS.cteOSProc.protCTe.InfProt.chCTe, "@cod_chave_acesso", SqlDbType.VarChar);
                        clsFacil.AdicionarParametro(ref strSQL_aux, Convert.ToString(j + 1), "@seq_detalhe_xml_conhecimento_transporte_eletronico_outro_servico_componente_valor_gtve", SqlDbType.Int);
                        clsFacil.AdicionarParametro(ref strSQL_aux, CTeOS.cteOSProc.CTeOS.infCte.infCTeNorm.infGTVe[i].CompinfGTVe[j].tpComp, "@cod_tipo_componente", SqlDbType.SmallInt);
                        clsFacil.AdicionarParametro(ref strSQL_aux, Convert.ToString(CTeOS.cteOSProc.CTeOS.infCte.infCTeNorm.infGTVe[i].CompinfGTVe[j].vComp), "@val_componente", SqlDbType.Decimal);
                        clsFacil.AdicionarParametro(ref strSQL_aux, CTeOS.cteOSProc.CTeOS.infCte.infCTeNorm.infGTVe[i].CompinfGTVe[j].xComp, "@nom_componente", SqlDbType.VarChar);

                        strSQL += strSQL_aux;
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

        public string InserirXML_detalhe_xml_conhecimento_transporte_eletronico_outro_servico_documento_referenciado(Common.SerializableClasses.CTe.ClsCTeOS_v4.proc CTeOS)
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
                for (Lcont = 0; Lcont <= CTeOS.cteOSProc.CTeOS.infCte.infCTeNorm.infDocRef.Count - 1; Lcont++)
                {
                    strSQL_aux = string.Empty;
                    stbSQL.Clear();
                    // Montando query a ser executada
                    stbSQL.Append(clsFacil.MontarQuery("INSERT INTO cte.detalhe_xml_conhecimento_transporte_eletronico_outro_servico_documento_referenciado "));
                    stbSQL.Append(clsFacil.MontarQuery("( "));
                    stbSQL.Append(clsFacil.MontarQuery("  dtr_referencia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso, "));
                    stbSQL.Append(clsFacil.MontarQuery("  num_documento, "));
                    stbSQL.Append(clsFacil.MontarQuery("  num_serie, "));
                    stbSQL.Append(clsFacil.MontarQuery("  num_subserie, "));
                    stbSQL.Append(clsFacil.MontarQuery("  dtc_emissao, "));
                    stbSQL.Append(clsFacil.MontarQuery("  val_transportado, "));
                    stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso_bilhete_passagem_excesso_bagagem, "));
                    stbSQL.Append(clsFacil.MontarQuery("  dtc_atualizacao "));
                    stbSQL.Append(clsFacil.MontarQuery(") "));
                    stbSQL.Append(clsFacil.MontarQuery("VALUES "));
                    stbSQL.Append(clsFacil.MontarQuery("( "));
                    stbSQL.Append(clsFacil.MontarQuery("  @dtr_referencia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @num_documento, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @num_serie, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @num_subserie, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @dtc_emissao, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @val_transportado, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso_bilhete_passagem_excesso_bagagem, "));
                    stbSQL.Append(clsFacil.MontarQuery("  getdate() "));
                    stbSQL.Append(clsFacil.MontarQuery(") "));
                    strSQL_aux = stbSQL.ToString();

                    // Montando parametros
                    clsDados.LimparParametro();
                    clsFacil.AdicionarParametro(ref strSQL_aux, "20" + CTeOS.cteOSProc.protCTe.InfProt.chCTe.Substring(2, 4), "@dtr_referencia", SqlDbType.Int);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTeOS.cteOSProc.protCTe.InfProt.chCTe, "@cod_chave_acesso", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTeOS.cteOSProc.CTeOS.infCte.infCTeNorm.infDocRef[Lcont].nDoc, "@num_documento", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTeOS.cteOSProc.CTeOS.infCte.infCTeNorm.infDocRef[Lcont].serie, "@num_serie", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTeOS.cteOSProc.CTeOS.infCte.infCTeNorm.infDocRef[Lcont].subserie, "@num_subserie", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTeOS.cteOSProc.CTeOS.infCte.infCTeNorm.infDocRef[Lcont].dEmi, "@dtc_emissao", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, Convert.ToString(CTeOS.cteOSProc.CTeOS.infCte.infCTeNorm.infDocRef[Lcont].vDoc), "@val_transportado", SqlDbType.Decimal);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTeOS.cteOSProc.CTeOS.infCte.infCTeNorm.infDocRef[Lcont].chBPe, "@cod_chave_acesso_bilhete_passagem_excesso_bagagem", SqlDbType.VarChar);
                    strSQL += strSQL_aux;
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

        public string InserirXML_detalhe_xml_conhecimento_transporte_eletronico_outro_servico_duplicata(Common.SerializableClasses.CTe.ClsCTeOS_v4.proc CTeOS)
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
                for (Lcont = 0; Lcont <= CTeOS.cteOSProc.CTeOS.infCte.infCTeNorm.cobr.dup.Count - 1; Lcont++)
                {
                    strSQL_aux = string.Empty;
                    stbSQL.Clear();
                    // Montando query a ser executada
                    stbSQL.Append(clsFacil.MontarQuery("INSERT INTO cte.detalhe_xml_conhecimento_transporte_eletronico_outro_servico_duplicata "));
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
                    clsFacil.AdicionarParametro(ref strSQL_aux, "20" + CTeOS.cteOSProc.protCTe.InfProt.chCTe.Substring(2, 4), "@dtr_referencia", SqlDbType.Int);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTeOS.cteOSProc.protCTe.InfProt.chCTe, "@cod_chave_acesso", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTeOS.cteOSProc.CTeOS.infCte.infCTeNorm.cobr.dup[Lcont].nDup, "@num_duplicata", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTeOS.cteOSProc.CTeOS.infCte.infCTeNorm.cobr.dup[Lcont].dVenc, "@dtc_vencimento", SqlDbType.DateTime);
                    clsFacil.AdicionarParametro(ref strSQL_aux, Convert.ToString(CTeOS.cteOSProc.CTeOS.infCte.infCTeNorm.cobr.dup[Lcont].vDup), "@val_duplicata", SqlDbType.Decimal);
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

        public string InserirXML_detalhe_xml_conhecimento_transporte_eletronico_outro_servico_percurso(Common.SerializableClasses.CTe.ClsCTeOS_v4.proc CTeOS)
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
                for (Lcont = 0; Lcont <= CTeOS.cteOSProc.CTeOS.infCte.ide.infPercurso.Count - 1; Lcont++)
                {
                    strSQL_aux = string.Empty;
                    stbSQL.Clear();
                    // Montando query a ser executada
                    stbSQL.Append(clsFacil.MontarQuery("INSERT INTO cte.detalhe_xml_conhecimento_transporte_eletronico_outro_servico_percurso "));
                    stbSQL.Append(clsFacil.MontarQuery("( "));
                    stbSQL.Append(clsFacil.MontarQuery("  dtr_referencia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso, "));
                    stbSQL.Append(clsFacil.MontarQuery("  seq_detalhe_xml_conhecimento_transporte_eletronico_outro_servico_percurso, "));
                    stbSQL.Append(clsFacil.MontarQuery("  sig_unid_federacao_percurso_prestacao, "));
                    stbSQL.Append(clsFacil.MontarQuery("  dtc_atualizacao "));
                    stbSQL.Append(clsFacil.MontarQuery(") "));
                    stbSQL.Append(clsFacil.MontarQuery("VALUES "));
                    stbSQL.Append(clsFacil.MontarQuery("( "));
                    stbSQL.Append(clsFacil.MontarQuery("  @dtr_referencia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @seq_detalhe_xml_conhecimento_transporte_eletronico_outro_servico_percurso, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @sig_unid_federacao_percurso_prestacao, "));
                    stbSQL.Append(clsFacil.MontarQuery("  getdate() "));
                    stbSQL.Append(clsFacil.MontarQuery(") "));
                    strSQL_aux = stbSQL.ToString();

                    // Montando parametros
                    clsDados.LimparParametro();
                    clsFacil.AdicionarParametro(ref strSQL_aux, "20" + CTeOS.cteOSProc.protCTe.InfProt.chCTe.Substring(2, 4), "@dtr_referencia", SqlDbType.Int);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTeOS.cteOSProc.protCTe.InfProt.chCTe, "@cod_chave_acesso", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, Convert.ToString(Lcont + 1), "@seq_detalhe_xml_conhecimento_transporte_eletronico_outro_servico_percurso", SqlDbType.Int);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTeOS.cteOSProc.CTeOS.infCte.ide.infPercurso[Lcont].UFPer, "@sig_unid_federacao_percurso_prestacao", SqlDbType.VarChar);

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

        public string InserirXML_detalhe_xml_conhecimento_transporte_eletronico_outro_servico_seguro_carga(Common.SerializableClasses.CTe.ClsCTeOS_v4.proc CTeOS)
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
                for (Lcont = 0; Lcont <= CTeOS.cteOSProc.CTeOS.infCte.infCTeNorm.seg.Count - 1; Lcont++)
                {
                    strSQL_aux = string.Empty;
                    stbSQL.Clear();
                    // Montando query a ser executada
                    stbSQL.Append(clsFacil.MontarQuery("INSERT INTO cte.detalhe_xml_conhecimento_transporte_eletronico_outro_servico_seguro_carga "));
                    stbSQL.Append(clsFacil.MontarQuery("( "));
                    stbSQL.Append(clsFacil.MontarQuery("  dtr_referencia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso, "));
                    stbSQL.Append(clsFacil.MontarQuery("  cod_responsavel_seguro, "));
                    stbSQL.Append(clsFacil.MontarQuery("  nom_seguradora, "));
                    stbSQL.Append(clsFacil.MontarQuery("  num_apolice, "));
                    stbSQL.Append(clsFacil.MontarQuery("  dtc_atualizacao "));
                    stbSQL.Append(clsFacil.MontarQuery(") "));
                    stbSQL.Append(clsFacil.MontarQuery("VALUES "));
                    stbSQL.Append(clsFacil.MontarQuery("( "));
                    stbSQL.Append(clsFacil.MontarQuery("  @dtr_referencia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @cod_responsavel_seguro, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @nom_seguradora, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @num_apolice, "));
                    stbSQL.Append(clsFacil.MontarQuery("  getdate() "));
                    stbSQL.Append(clsFacil.MontarQuery(") "));
                    strSQL_aux = stbSQL.ToString();

                    // Montando parametros
                    clsDados.LimparParametro();
                    clsFacil.AdicionarParametro(ref strSQL_aux, "20" + CTeOS.cteOSProc.protCTe.InfProt.chCTe.Substring(2, 4), "@dtr_referencia", SqlDbType.Int);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTeOS.cteOSProc.protCTe.InfProt.chCTe, "@cod_chave_acesso", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTeOS.cteOSProc.CTeOS.infCte.infCTeNorm.seg[Lcont].respSeg, "@cod_responsavel_seguro", SqlDbType.SmallInt);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTeOS.cteOSProc.CTeOS.infCte.infCTeNorm.seg[Lcont].xSeg, "@nom_seguradora", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTeOS.cteOSProc.CTeOS.infCte.infCTeNorm.seg[Lcont].nApol, "@num_apolice", SqlDbType.VarChar);
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

        public string InserirXML_detalhe_xml_conhecimento_transporte_eletronico_outro_servico_modal_rodoviario(Common.SerializableClasses.CTe.ClsCTeOS_v4.proc CTeOS)
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
                stbSQL.Append(clsFacil.MontarQuery("INSERT INTO cte.detalhe_xml_conhecimento_transporte_eletronico_outro_servico_modal_rodoviario "));
                stbSQL.Append(clsFacil.MontarQuery("( "));
                stbSQL.Append(clsFacil.MontarQuery("  dtr_referencia, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso, "));                                
                stbSQL.Append(clsFacil.MontarQuery("  num_termo_autorizacao_fretamento_emitente, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_registro_estadual_emitente, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_placa, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_renavam, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_cnpj_cpf_base_proprietario, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_cnpj_cpf_filial_proprietario, "));
                stbSQL.Append(clsFacil.MontarQuery("  dig_cnpj_cpf_proprietario, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_termo_autorizacao_fretamento, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_registro_estadual, "));
                stbSQL.Append(clsFacil.MontarQuery("  nom_razao_social_propietario, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_insc_estad_propietario, "));
                stbSQL.Append(clsFacil.MontarQuery("  sig_unid_federacao_proprietario, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_tipo_proprietario, "));
                stbSQL.Append(clsFacil.MontarQuery("  sig_unid_federacao_licenciamento, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_tipo_fretamento, "));
                stbSQL.Append(clsFacil.MontarQuery("  dtc_viajem, "));
                stbSQL.Append(clsFacil.MontarQuery("  des_versao_modal, "));
                stbSQL.Append(clsFacil.MontarQuery("  dtc_atualizacao "));
                stbSQL.Append(clsFacil.MontarQuery(") "));
                stbSQL.Append(clsFacil.MontarQuery("VALUES "));
                stbSQL.Append(clsFacil.MontarQuery("( "));
                stbSQL.Append(clsFacil.MontarQuery("  @dtr_referencia, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso, "));                                
                stbSQL.Append(clsFacil.MontarQuery("  @num_termo_autorizacao_fretamento_emitente, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_registro_estadual_emitente, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_placa, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_renavam, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_cnpj_cpf_base_proprietario, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_cnpj_cpf_filial_proprietario, "));
                stbSQL.Append(clsFacil.MontarQuery("  @dig_cnpj_cpf_proprietario, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_termo_autorizacao_fretamento, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_registro_estadual, "));
                stbSQL.Append(clsFacil.MontarQuery("  @nom_razao_social_propietario, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_insc_estad_propietario, "));
                stbSQL.Append(clsFacil.MontarQuery("  @sig_unid_federacao_proprietario, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_tipo_proprietario, "));
                stbSQL.Append(clsFacil.MontarQuery("  @sig_unid_federacao_licenciamento, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_tipo_fretamento, "));
                stbSQL.Append(clsFacil.MontarQuery("  @dtc_viajem, "));
                stbSQL.Append(clsFacil.MontarQuery("  @des_versao_modal, "));
                stbSQL.Append(clsFacil.MontarQuery("  getdate() "));
                stbSQL.Append(clsFacil.MontarQuery(") "));
                string strSQL = stbSQL.ToString();

                // Montando parametros
                clsDados.LimparParametro();
                clsFacil.AdicionarParametro(ref strSQL, "20" + CTeOS.cteOSProc.protCTe.InfProt.chCTe.Substring(2, 4), "@dtr_referencia", SqlDbType.Int);
                clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.protCTe.InfProt.chCTe, "@cod_chave_acesso", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCte.infCTeNorm.infModal.versaoModal, "@des_versao_modal", SqlDbType.VarChar);                
                clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCte.infCTeNorm.infModal.rodoOS.TAF, "@num_termo_autorizacao_fretamento_emitente", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCte.infCTeNorm.infModal.rodoOS.NroRegEstadual, "@num_registro_estadual_emitente", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCte.infCTeNorm.infModal.rodoOS.veic.placa, "@num_placa", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCte.infCTeNorm.infModal.rodoOS.veic.RENAVAM, "@num_renavam", SqlDbType.VarChar);

                if (CTeOS.cteOSProc.CTeOS.infCte.infCTeNorm.infModal.rodoOS.veic.prop.CPF != null && CTeOS.cteOSProc.CTeOS.infCte.infCTeNorm.infModal.rodoOS.veic.prop.CPF != string.Empty)
                {
                    clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCte.infCTeNorm.infModal.rodoOS.veic.prop.CPF.Substring(0, 9), "@num_cnpj_cpf_base_tomador", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, "0", "@num_cnpj_cpf_filial_tomador", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCte.infCTeNorm.infModal.rodoOS.veic.prop.CPF.Substring(9, 2), "@dig_cnpj_cpf_tomador", SqlDbType.SmallInt);
                }
                if (CTeOS.cteOSProc.CTeOS.infCte.infCTeNorm.infModal.rodoOS.veic.prop.CNPJ != null && CTeOS.cteOSProc.CTeOS.infCte.infCTeNorm.infModal.rodoOS.veic.prop.CNPJ != string.Empty)
                {
                    clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCte.infCTeNorm.infModal.rodoOS.veic.prop.CNPJ.Substring(0, 8), "@num_cnpj_cpf_base_tomador", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCte.infCTeNorm.infModal.rodoOS.veic.prop.CNPJ.Substring(8, 4), "@num_cnpj_cpf_filial_tomador", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCte.infCTeNorm.infModal.rodoOS.veic.prop.CNPJ.Substring(12, 2), "@dig_cnpj_cpf_tomador", SqlDbType.SmallInt);
                }

                if ((CTeOS.cteOSProc.CTeOS.infCte.infCTeNorm.infModal.rodoOS.veic.prop.CPF == null && CTeOS.cteOSProc.CTeOS.infCte.infCTeNorm.infModal.rodoOS.veic.prop.CNPJ == null) || (CTeOS.cteOSProc.CTeOS.infCte.infCTeNorm.infModal.rodoOS.veic.prop.CPF == null && CTeOS.cteOSProc.CTeOS.infCte.infCTeNorm.infModal.rodoOS.veic.prop.CNPJ == string.Empty) || (CTeOS.cteOSProc.CTeOS.infCte.infCTeNorm.infModal.rodoOS.veic.prop.CPF == string.Empty && CTeOS.cteOSProc.CTeOS.infCte.infCTeNorm.infModal.rodoOS.veic.prop.CNPJ == null))
                { 
                    clsFacil.AdicionarParametro(ref strSQL, string.Empty, "@num_cnpj_cpf_base_proprietario", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, string.Empty, "@num_cnpj_cpf_filial_proprietario", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, string.Empty, "@dig_cnpj_cpf_proprietario", SqlDbType.SmallInt);
                }

                clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCte.infCTeNorm.infModal.rodoOS.veic.prop.TAF, "@num_termo_autorizacao_fretamento", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCte.infCTeNorm.infModal.rodoOS.veic.prop.NroRegEstadual, "@num_registro_estadual", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCte.infCTeNorm.infModal.rodoOS.veic.prop.xNome, "@nom_razao_social_propietario", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCte.infCTeNorm.infModal.rodoOS.veic.prop.IE, "@num_insc_estad_propietario", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCte.infCTeNorm.infModal.rodoOS.veic.prop.UF, "@sig_unid_federacao_proprietario", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCte.infCTeNorm.infModal.rodoOS.veic.prop.tpProp, "@cod_tipo_proprietario", SqlDbType.SmallInt);
                clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCte.infCTeNorm.infModal.rodoOS.veic.UF, "@sig_unid_federacao_licenciamento", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCte.infCTeNorm.infModal.rodoOS.infFretamento.tpFretamento, "@cod_tipo_fretamento", SqlDbType.SmallInt);
                clsFacil.AdicionarParametro(ref strSQL, CTeOS.cteOSProc.CTeOS.infCte.infCTeNorm.infModal.rodoOS.infFretamento.dhViagem, "@dtc_viajem", SqlDbType.DateTime);

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

        public string InserirXML(Common.SerializableClasses.CTe.ClsCTeOS_v4.proc CTeOS)
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
