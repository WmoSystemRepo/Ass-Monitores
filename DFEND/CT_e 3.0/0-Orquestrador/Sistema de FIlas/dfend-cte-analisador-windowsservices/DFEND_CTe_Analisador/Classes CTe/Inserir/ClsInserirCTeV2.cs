using System;
using System.Data;
using System.Text;

namespace DFe
{
    class ClsInserirCTeV2
    {

        #region "Insert CTe"

        public string InserirXML_detalhe_xml_conhecimento_transporte_eletronico_autorizado(Common.SerializableClasses.CTe.ClsCTe_v2.proc CTe)
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
                stbSQL.Append(clsFacil.MontarQuery("INSERT INTO cte.detalhe_xml_conhecimento_transporte_eletronico_autorizado "));
                stbSQL.Append(clsFacil.MontarQuery("( "));
                stbSQL.Append(clsFacil.MontarQuery("  dtr_referencia, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_unid_federacao_ibge, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_cfop, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_natureza_operacao, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_modelo, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_serie, "));
                stbSQL.Append(clsFacil.MontarQuery("  dtc_emissao, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_tipo_formato_impressao_conhecimento_transporte_eletronico, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_tipo_forma_emissao_conhecimento_transporte_eletronico, "));
                stbSQL.Append(clsFacil.MontarQuery("  dig_chave_acesso, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_tipo_ambiente, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_tipo_conhecimento_transporte_eletronico, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_versao_processo_emissao_conhecimento_transporte_eletronico, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_municipio_ibge_envio, "));
                stbSQL.Append(clsFacil.MontarQuery("  des_municipio_envio, "));
                stbSQL.Append(clsFacil.MontarQuery("  sig_unid_federacao_envio, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_tipo_servico_conhecimento_transporte_eletronico, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_municipio_ibge_inicio_prestacao, "));
                stbSQL.Append(clsFacil.MontarQuery("  sig_unid_federacao_inicio_prestacao, "));
                stbSQL.Append(clsFacil.MontarQuery("  des_municipio_inicio_prestacao, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_municipio_ibge_termino_prestacao, "));
                stbSQL.Append(clsFacil.MontarQuery("  des_municipio_fim_prestacao, "));
                stbSQL.Append(clsFacil.MontarQuery("  sig_unid_federacao_fim_prestacao, "));
                stbSQL.Append(clsFacil.MontarQuery("  ind_retira_recebedor, "));
                stbSQL.Append(clsFacil.MontarQuery("  des_retira_recebedor, "));
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
                stbSQL.Append(clsFacil.MontarQuery("  cod_tipo_servico_conhecimento_transporte_eletronico_tomador, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_cep_tomador, "));
                stbSQL.Append(clsFacil.MontarQuery("  sig_unid_federacao_tomador, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_pais_bacen_tomador, "));
                stbSQL.Append(clsFacil.MontarQuery("  nom_pais_bacen_tomador, "));
                stbSQL.Append(clsFacil.MontarQuery("  nom_email_tomador, "));
                stbSQL.Append(clsFacil.MontarQuery("  dtc_entrada_contingencia, "));
                stbSQL.Append(clsFacil.MontarQuery("  des_justificativa_entrada_contingencia, "));
                stbSQL.Append(clsFacil.MontarQuery("  des_caracteristica_adicional_transporte, "));
                stbSQL.Append(clsFacil.MontarQuery("  des_caracteristica_adicional_servico, "));
                stbSQL.Append(clsFacil.MontarQuery("  des_funcionario_emissor, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_tipo_data_programada, "));
                stbSQL.Append(clsFacil.MontarQuery("  dtc_entrega_programada, "));
                stbSQL.Append(clsFacil.MontarQuery("  dtc_periodo_definido_inicio, "));
                stbSQL.Append(clsFacil.MontarQuery("  dtc_periodo_definido_fim, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_tipo_hora_programada, "));
                stbSQL.Append(clsFacil.MontarQuery("  des_hora_programada, "));
                stbSQL.Append(clsFacil.MontarQuery("  des_hora_definido_inicio, "));
                stbSQL.Append(clsFacil.MontarQuery("  des_hora_definido_fim, "));
                stbSQL.Append(clsFacil.MontarQuery("  des_municipio_origem_calculo_frete, "));
                stbSQL.Append(clsFacil.MontarQuery("  des_municipio_destino_calculo_frete, "));
                stbSQL.Append(clsFacil.MontarQuery("  des_observacao_geral, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_cnpj_cpf_base_emitente, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_cnpj_cpf_filial_emitente, "));
                stbSQL.Append(clsFacil.MontarQuery("  dig_cnpj_cpf_emitente, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_insc_estad_emitente, "));
                stbSQL.Append(clsFacil.MontarQuery("  nom_razao_social_emitente, "));
                stbSQL.Append(clsFacil.MontarQuery("  nom_fantasia_emitente, "));
                stbSQL.Append(clsFacil.MontarQuery("  des_logradouro_emitente, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_endereco_emitente, "));
                stbSQL.Append(clsFacil.MontarQuery("  des_compl_endereco_emitente, "));
                stbSQL.Append(clsFacil.MontarQuery("  des_bairro_emitente, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_municipio_ibge_emitente, "));
                stbSQL.Append(clsFacil.MontarQuery("  nom_municipio_ibge_emitente, "));
                stbSQL.Append(clsFacil.MontarQuery("  sig_unid_federacao_emitente, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_telefone_emitente, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_cnpj_cpf_base_remetente, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_cnpj_cpf_filial_remetente, "));
                stbSQL.Append(clsFacil.MontarQuery("  dig_cnpj_cpf_remetente, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_insc_estad_remetente, "));
                stbSQL.Append(clsFacil.MontarQuery("  nom_razao_social_remetente, "));
                stbSQL.Append(clsFacil.MontarQuery("  nom_fantasia_remetente, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_telefone_remetente, "));
                stbSQL.Append(clsFacil.MontarQuery("  des_logradouro_remetente, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_endereco_remetente, "));
                stbSQL.Append(clsFacil.MontarQuery("  des_compl_endereco_remetente, "));
                stbSQL.Append(clsFacil.MontarQuery("  des_bairro_remetente, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_municipio_ibge_remetente, "));
                stbSQL.Append(clsFacil.MontarQuery("  nom_municipio_ibge_remetente, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_cep_remetente, "));
                stbSQL.Append(clsFacil.MontarQuery("  sig_unid_federacao_remetente, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_pais_bacen_remetente, "));
                stbSQL.Append(clsFacil.MontarQuery("  nom_pais_bacen_remetente, "));
                stbSQL.Append(clsFacil.MontarQuery("  nom_email_remetente, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_cnpj_base_expedidor, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_cnpj_filial_expedidor, "));
                stbSQL.Append(clsFacil.MontarQuery("  dig_cnpj_expedidor, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_insc_estad_expedidor, "));
                stbSQL.Append(clsFacil.MontarQuery("  nom_razao_social_expedidor, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_telefone_expedidor, "));
                stbSQL.Append(clsFacil.MontarQuery("  des_logradouro_expedidor, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_endereco_expedidor, "));
                stbSQL.Append(clsFacil.MontarQuery("  des_compl_endereco_expedidor, "));
                stbSQL.Append(clsFacil.MontarQuery("  des_bairro_expedidor, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_municipio_ibge_expedidor, "));
                stbSQL.Append(clsFacil.MontarQuery("  nom_municipio_ibge_expedidor, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_cep_expedidor, "));
                stbSQL.Append(clsFacil.MontarQuery("  sig_unid_federacao_expedidor, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_pais_bacen_expedidor, "));
                stbSQL.Append(clsFacil.MontarQuery("  nom_pais_bacen_expedidor, "));
                stbSQL.Append(clsFacil.MontarQuery("  nom_email_expedidor, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_cnpj_base_recebedor, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_cnpj_filial_recebedor, "));
                stbSQL.Append(clsFacil.MontarQuery("  dig_cnpj_recebedor, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_insc_estad_recebedor, "));
                stbSQL.Append(clsFacil.MontarQuery("  nom_razao_social_recebedor, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_telefone_recebedor, "));
                stbSQL.Append(clsFacil.MontarQuery("  des_logradouro_recebedor, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_endereco_recebedor, "));
                stbSQL.Append(clsFacil.MontarQuery("  des_compl_endereco_recebedor, "));
                stbSQL.Append(clsFacil.MontarQuery("  des_bairro_recebedor, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_municipio_ibge_recebedor, "));
                stbSQL.Append(clsFacil.MontarQuery("  nom_municipio_ibge_recebedor, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_cep_recebedor, "));
                stbSQL.Append(clsFacil.MontarQuery("  sig_unid_federacao_recebedor, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_pais_bacen_recebedor, "));
                stbSQL.Append(clsFacil.MontarQuery("  nom_pais_bacen_recebedor, "));
                stbSQL.Append(clsFacil.MontarQuery("  nom_email_recebedor, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_cnpj_cpf_base_destinatario, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_cnpj_cpf_filial_destinatario, "));
                stbSQL.Append(clsFacil.MontarQuery("  dig_cnpj_cpf_destinatario, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_insc_estad_destinatario, "));
                stbSQL.Append(clsFacil.MontarQuery("  nom_razao_social_destinatario, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_telefone_destinatario, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_insc_suframa_destinatario, "));
                stbSQL.Append(clsFacil.MontarQuery("  des_logradouro_destinatario, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_endereco_destinatario, "));
                stbSQL.Append(clsFacil.MontarQuery("  des_compl_endereco_destinatario, "));
                stbSQL.Append(clsFacil.MontarQuery("  des_bairro_destinatario, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_municipio_ibge_destinatario, "));
                stbSQL.Append(clsFacil.MontarQuery("  nom_municipio_ibge_destinatario, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_cep_destinatario, "));
                stbSQL.Append(clsFacil.MontarQuery("  sig_unid_federacao_destinatario, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_pais_bacen_destinatario, "));
                stbSQL.Append(clsFacil.MontarQuery("  nom_pais_bacen_destinatario, "));
                stbSQL.Append(clsFacil.MontarQuery("  nom_email_destinatario, "));
                stbSQL.Append(clsFacil.MontarQuery("  val_total_prestacao_servico, "));
                stbSQL.Append(clsFacil.MontarQuery("  val_prestacao_servico_receber, "));
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
                stbSQL.Append(clsFacil.MontarQuery("  des_produto_predominante, "));
                stbSQL.Append(clsFacil.MontarQuery("  des_outra_caracteristica_carga, "));
                stbSQL.Append(clsFacil.MontarQuery("  des_versao_leiaute_modal, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso_documento_substituido, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_fatura, "));
                stbSQL.Append(clsFacil.MontarQuery("  val_total_fatura, "));
                stbSQL.Append(clsFacil.MontarQuery("  val_total_desconto_fatura, "));
                stbSQL.Append(clsFacil.MontarQuery("  val_total_liquido_fatura, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_tipo_classificacao_tributaria, "));
                stbSQL.Append(clsFacil.MontarQuery("  dtc_atualizacao, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_tipo_processo_emissao_conhecimento_transporte_eletronico, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_cep_emitente, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_tipo_documento_transporte_eletronico_transportado, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_sequencial_unico, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_sequencial_unico_autorizacao, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_tipo_modal_transporte_documento_fiscal_eletronico "));
                stbSQL.Append(clsFacil.MontarQuery(") "));
                stbSQL.Append(clsFacil.MontarQuery("VALUES "));
                stbSQL.Append(clsFacil.MontarQuery("( "));
                stbSQL.Append(clsFacil.MontarQuery("  @dtr_referencia, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_unid_federacao_ibge, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_cfop, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_natureza_operacao, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_modelo, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_serie, "));
                stbSQL.Append(clsFacil.MontarQuery("  @dtc_emissao, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_tipo_formato_impressao_conhecimento_transporte_eletronico, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_tipo_forma_emissao_conhecimento_transporte_eletronico, "));
                stbSQL.Append(clsFacil.MontarQuery("  @dig_chave_acesso, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_tipo_ambiente, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_tipo_conhecimento_transporte_eletronico, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_versao_processo_emissao_conhecimento_transporte_eletronico, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_municipio_ibge_envio, "));
                stbSQL.Append(clsFacil.MontarQuery("  @des_municipio_envio, "));
                stbSQL.Append(clsFacil.MontarQuery("  @sig_unid_federacao_envio, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_tipo_servico_conhecimento_transporte_eletronico, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_municipio_ibge_inicio_prestacao, "));
                stbSQL.Append(clsFacil.MontarQuery("  @sig_unid_federacao_inicio_prestacao, "));
                stbSQL.Append(clsFacil.MontarQuery("  @des_municipio_inicio_prestacao, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_municipio_ibge_termino_prestacao, "));
                stbSQL.Append(clsFacil.MontarQuery("  @des_municipio_fim_prestacao, "));
                stbSQL.Append(clsFacil.MontarQuery("  @sig_unid_federacao_fim_prestacao, "));
                stbSQL.Append(clsFacil.MontarQuery("  @ind_retira_recebedor, "));
                stbSQL.Append(clsFacil.MontarQuery("  @des_retira_recebedor, "));
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
                stbSQL.Append(clsFacil.MontarQuery("  @cod_tipo_servico_conhecimento_transporte_eletronico_tomador, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_cep_tomador, "));
                stbSQL.Append(clsFacil.MontarQuery("  @sig_unid_federacao_tomador, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_pais_bacen_tomador, "));
                stbSQL.Append(clsFacil.MontarQuery("  @nom_pais_bacen_tomador, "));
                stbSQL.Append(clsFacil.MontarQuery("  @nom_email_tomador, "));
                stbSQL.Append(clsFacil.MontarQuery("  @dtc_entrada_contingencia, "));
                stbSQL.Append(clsFacil.MontarQuery("  @des_justificativa_entrada_contingencia, "));
                stbSQL.Append(clsFacil.MontarQuery("  @des_caracteristica_adicional_transporte, "));
                stbSQL.Append(clsFacil.MontarQuery("  @des_caracteristica_adicional_servico, "));
                stbSQL.Append(clsFacil.MontarQuery("  @des_funcionario_emissor, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_tipo_data_programada, "));
                stbSQL.Append(clsFacil.MontarQuery("  @dtc_entrega_programada, "));
                stbSQL.Append(clsFacil.MontarQuery("  @dtc_periodo_definido_inicio, "));
                stbSQL.Append(clsFacil.MontarQuery("  @dtc_periodo_definido_fim, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_tipo_hora_programada, "));
                stbSQL.Append(clsFacil.MontarQuery("  @des_hora_programada, "));
                stbSQL.Append(clsFacil.MontarQuery("  @des_hora_definido_inicio, "));
                stbSQL.Append(clsFacil.MontarQuery("  @des_hora_definido_fim, "));
                stbSQL.Append(clsFacil.MontarQuery("  @des_municipio_origem_calculo_frete, "));
                stbSQL.Append(clsFacil.MontarQuery("  @des_municipio_destino_calculo_frete, "));
                stbSQL.Append(clsFacil.MontarQuery("  @des_observacao_geral, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_cnpj_cpf_base_emitente, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_cnpj_cpf_filial_emitente, "));
                stbSQL.Append(clsFacil.MontarQuery("  @dig_cnpj_cpf_emitente, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_insc_estad_emitente, "));
                stbSQL.Append(clsFacil.MontarQuery("  @nom_razao_social_emitente, "));
                stbSQL.Append(clsFacil.MontarQuery("  @nom_fantasia_emitente, "));
                stbSQL.Append(clsFacil.MontarQuery("  @des_logradouro_emitente, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_endereco_emitente, "));
                stbSQL.Append(clsFacil.MontarQuery("  @des_compl_endereco_emitente, "));
                stbSQL.Append(clsFacil.MontarQuery("  @des_bairro_emitente, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_municipio_ibge_emitente, "));
                stbSQL.Append(clsFacil.MontarQuery("  @nom_municipio_ibge_emitente, "));
                stbSQL.Append(clsFacil.MontarQuery("  @sig_unid_federacao_emitente, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_telefone_emitente, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_cnpj_cpf_base_remetente, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_cnpj_cpf_filial_remetente, "));
                stbSQL.Append(clsFacil.MontarQuery("  @dig_cnpj_cpf_remetente, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_insc_estad_remetente, "));
                stbSQL.Append(clsFacil.MontarQuery("  @nom_razao_social_remetente, "));
                stbSQL.Append(clsFacil.MontarQuery("  @nom_fantasia_remetente, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_telefone_remetente, "));
                stbSQL.Append(clsFacil.MontarQuery("  @des_logradouro_remetente, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_endereco_remetente, "));
                stbSQL.Append(clsFacil.MontarQuery("  @des_compl_endereco_remetente, "));
                stbSQL.Append(clsFacil.MontarQuery("  @des_bairro_remetente, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_municipio_ibge_remetente, "));
                stbSQL.Append(clsFacil.MontarQuery("  @nom_municipio_ibge_remetente, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_cep_remetente, "));
                stbSQL.Append(clsFacil.MontarQuery("  @sig_unid_federacao_remetente, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_pais_bacen_remetente, "));
                stbSQL.Append(clsFacil.MontarQuery("  @nom_pais_bacen_remetente, "));
                stbSQL.Append(clsFacil.MontarQuery("  @nom_email_remetente, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_cnpj_base_expedidor, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_cnpj_filial_expedidor, "));
                stbSQL.Append(clsFacil.MontarQuery("  @dig_cnpj_expedidor, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_insc_estad_expedidor, "));
                stbSQL.Append(clsFacil.MontarQuery("  @nom_razao_social_expedidor, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_telefone_expedidor, "));
                stbSQL.Append(clsFacil.MontarQuery("  @des_logradouro_expedidor, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_endereco_expedidor, "));
                stbSQL.Append(clsFacil.MontarQuery("  @des_compl_endereco_expedidor, "));
                stbSQL.Append(clsFacil.MontarQuery("  @des_bairro_expedidor, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_municipio_ibge_expedidor, "));
                stbSQL.Append(clsFacil.MontarQuery("  @nom_municipio_ibge_expedidor, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_cep_expedidor, "));
                stbSQL.Append(clsFacil.MontarQuery("  @sig_unid_federacao_expedidor, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_pais_bacen_expedidor, "));
                stbSQL.Append(clsFacil.MontarQuery("  @nom_pais_bacen_expedidor, "));
                stbSQL.Append(clsFacil.MontarQuery("  @nom_email_expedidor, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_cnpj_base_recebedor, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_cnpj_filial_recebedor, "));
                stbSQL.Append(clsFacil.MontarQuery("  @dig_cnpj_recebedor, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_insc_estad_recebedor, "));
                stbSQL.Append(clsFacil.MontarQuery("  @nom_razao_social_recebedor, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_telefone_recebedor, "));
                stbSQL.Append(clsFacil.MontarQuery("  @des_logradouro_recebedor, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_endereco_recebedor, "));
                stbSQL.Append(clsFacil.MontarQuery("  @des_compl_endereco_recebedor, "));
                stbSQL.Append(clsFacil.MontarQuery("  @des_bairro_recebedor, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_municipio_ibge_recebedor, "));
                stbSQL.Append(clsFacil.MontarQuery("  @nom_municipio_ibge_recebedor, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_cep_recebedor, "));
                stbSQL.Append(clsFacil.MontarQuery("  @sig_unid_federacao_recebedor, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_pais_bacen_recebedor, "));
                stbSQL.Append(clsFacil.MontarQuery("  @nom_pais_bacen_recebedor, "));
                stbSQL.Append(clsFacil.MontarQuery("  @nom_email_recebedor, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_cnpj_cpf_base_destinatario, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_cnpj_cpf_filial_destinatario, "));
                stbSQL.Append(clsFacil.MontarQuery("  @dig_cnpj_cpf_destinatario, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_insc_estad_destinatario, "));
                stbSQL.Append(clsFacil.MontarQuery("  @nom_razao_social_destinatario, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_telefone_destinatario, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_insc_suframa_destinatario, "));
                stbSQL.Append(clsFacil.MontarQuery("  @des_logradouro_destinatario, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_endereco_destinatario, "));
                stbSQL.Append(clsFacil.MontarQuery("  @des_compl_endereco_destinatario, "));
                stbSQL.Append(clsFacil.MontarQuery("  @des_bairro_destinatario, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_municipio_ibge_destinatario, "));
                stbSQL.Append(clsFacil.MontarQuery("  @nom_municipio_ibge_destinatario, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_cep_destinatario, "));
                stbSQL.Append(clsFacil.MontarQuery("  @sig_unid_federacao_destinatario, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_pais_bacen_destinatario, "));
                stbSQL.Append(clsFacil.MontarQuery("  @nom_pais_bacen_destinatario, "));
                stbSQL.Append(clsFacil.MontarQuery("  @nom_email_destinatario, "));
                stbSQL.Append(clsFacil.MontarQuery("  @val_total_prestacao_servico, "));
                stbSQL.Append(clsFacil.MontarQuery("  @val_prestacao_servico_receber, "));
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
                stbSQL.Append(clsFacil.MontarQuery("  @des_produto_predominante, "));
                stbSQL.Append(clsFacil.MontarQuery("  @des_outra_caracteristica_carga, "));
                stbSQL.Append(clsFacil.MontarQuery("  @des_versao_leiaute_modal, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso_documento_substituido, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_fatura, "));
                stbSQL.Append(clsFacil.MontarQuery("  @val_total_fatura, "));
                stbSQL.Append(clsFacil.MontarQuery("  @val_total_desconto_fatura, "));
                stbSQL.Append(clsFacil.MontarQuery("  @val_total_liquido_fatura, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_tipo_classificacao_tributaria, "));
                stbSQL.Append(clsFacil.MontarQuery("  getdate(), "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_tipo_processo_emissao_conhecimento_transporte_eletronico, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_cep_emitente, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_tipo_documento_transporte_eletronico_transportado, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_sequencial_unico, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_sequencial_unico_autorizacao, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_tipo_modal_transporte_documento_fiscal_eletronico "));
                stbSQL.Append(clsFacil.MontarQuery(") "));
                string strSQL = stbSQL.ToString();

                // Montando parametros
                clsDados.LimparParametro();
                clsFacil.AdicionarParametro(ref strSQL, "20" + CTe.CteProc.protCTe.InfProt.chCTe.Substring(2, 4), "@dtr_referencia", SqlDbType.Int);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.protCTe.InfProt.chCTe, "@cod_chave_acesso", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.ide.cUF, "@cod_unid_federacao_ibge", SqlDbType.SmallInt);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.ide.CFOP, "@num_cfop", SqlDbType.Int);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.ide.natOp, "@cod_natureza_operacao", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.ide.mod, "@cod_modelo", SqlDbType.SmallInt);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.ide.serie, "@num_serie", SqlDbType.SmallInt);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.ide.dhEmi, "@dtc_emissao", SqlDbType.DateTime);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.ide.tpImp, "@cod_tipo_formato_impressao_conhecimento_transporte_eletronico", SqlDbType.SmallInt);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.ide.tpEmis, "@cod_tipo_forma_emissao_conhecimento_transporte_eletronico", SqlDbType.SmallInt);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.ide.cDV, "@dig_chave_acesso", SqlDbType.SmallInt);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.ide.tpAmb, "@cod_tipo_ambiente", SqlDbType.SmallInt);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.ide.tpCTe, "@cod_tipo_conhecimento_transporte_eletronico", SqlDbType.SmallInt);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.ide.verProc, "@num_versao_processo_emissao_conhecimento_transporte_eletronico", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.ide.cMunEnv, "@cod_municipio_ibge_envio", SqlDbType.Int);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.ide.xMunEnv, "@des_municipio_envio", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.ide.UFEnv, "@sig_unid_federacao_envio", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.ide.tpServ, "@cod_tipo_servico_conhecimento_transporte_eletronico", SqlDbType.SmallInt);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.ide.cMunIni, "@cod_municipio_ibge_inicio_prestacao", SqlDbType.Int);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.ide.UFIni, "@sig_unid_federacao_inicio_prestacao", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.ide.xMunIni, "@des_municipio_inicio_prestacao", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.ide.cMunFim, "@cod_municipio_ibge_termino_prestacao", SqlDbType.Int);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.ide.xMunFim, "@des_municipio_fim_prestacao", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.ide.UFFim, "@sig_unid_federacao_fim_prestacao", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.ide.retira, "@ind_retira_recebedor", SqlDbType.SmallInt);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.ide.xDetRetira, "@des_retira_recebedor", SqlDbType.VarChar);


                if (CTe.CteProc.CTe.infCte.ide.toma03.toma != null)
                {
                    clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.ide.toma03.toma, "@cod_tipo_servico_conhecimento_transporte_eletronico_tomador", SqlDbType.SmallInt);
                }
                if (CTe.CteProc.CTe.infCte.ide.toma4.toma != null)
                {
                    clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.ide.toma4.toma, "@cod_tipo_servico_conhecimento_transporte_eletronico_tomador", SqlDbType.SmallInt);
                }

                if (CTe.CteProc.CTe.infCte.ide.toma4.CPF != null && CTe.CteProc.CTe.infCte.ide.toma4.CPF != string.Empty)
                {
                    clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.ide.toma4.CPF.Substring(0, 9), "@num_cnpj_cpf_base_tomador", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, "0", "@num_cnpj_cpf_filial_tomador", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.ide.toma4.CPF.Substring(9, 2), "@dig_cnpj_cpf_tomador", SqlDbType.SmallInt);
                }
                if (CTe.CteProc.CTe.infCte.ide.toma4.CNPJ != null && CTe.CteProc.CTe.infCte.ide.toma4.CNPJ != string.Empty)
                {
                    clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.ide.toma4.CNPJ.Substring(0, 8), "@num_cnpj_cpf_base_tomador", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.ide.toma4.CNPJ.Substring(8, 4), "@num_cnpj_cpf_filial_tomador", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.ide.toma4.CNPJ.Substring(12, 2), "@dig_cnpj_cpf_tomador", SqlDbType.SmallInt);
                }
                if ((CTe.CteProc.CTe.infCte.ide.toma4.CPF == null && CTe.CteProc.CTe.infCte.ide.toma4.CNPJ == null) || (CTe.CteProc.CTe.infCte.ide.toma4.CPF == null && CTe.CteProc.CTe.infCte.ide.toma4.CNPJ == string.Empty) || (CTe.CteProc.CTe.infCte.ide.toma4.CPF == string.Empty && CTe.CteProc.CTe.infCte.ide.toma4.CNPJ == null))
                {
                    clsFacil.AdicionarParametro(ref strSQL, string.Empty, "@num_cnpj_cpf_base_tomador", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, string.Empty, "@num_cnpj_cpf_filial_tomador", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, string.Empty, "@dig_cnpj_cpf_tomador", SqlDbType.SmallInt);
                }

                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.ide.toma4.IE, "@num_insc_estad_tomador", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.ide.toma4.xNome, "@nom_razao_social_tomador", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.ide.toma4.xFant, "@nom_fantasia_tomador", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.ide.toma4.fone, "@num_telefone_tomador", SqlDbType.BigInt);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.ide.toma4.enderToma.xLgr, "@des_logradouro_tomador", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.ide.toma4.enderToma.nro, "@num_endereco_tomador", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.ide.toma4.enderToma.xCpl, "@des_compl_endereco_tomador", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.ide.toma4.enderToma.xBairro, "@des_bairro_tomador", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.ide.toma4.enderToma.cMun, "@cod_municipio_ibge_tomador", SqlDbType.Int);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.ide.toma4.enderToma.xMun, "@nom_municipio_ibge_tomador", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.ide.toma4.enderToma.CEP, "@num_cep_tomador", SqlDbType.BigInt);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.ide.toma4.enderToma.UF, "@sig_unid_federacao_tomador", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.ide.toma4.enderToma.cPais, "@cod_pais_bacen_tomador", SqlDbType.SmallInt);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.ide.toma4.enderToma.xPais, "@nom_pais_bacen_tomador", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.ide.toma4.email, "@nom_email_tomador", SqlDbType.VarChar);

                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.ide.dhCont, "@dtc_entrada_contingencia", SqlDbType.DateTime);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.ide.xJust, "@des_justificativa_entrada_contingencia", SqlDbType.VarChar);

                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.compl.xCaracAd, "@des_caracteristica_adicional_transporte", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.compl.xCaracSer, "@des_caracteristica_adicional_servico", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.compl.xEmi, "@des_funcionario_emissor", SqlDbType.VarChar);

                string tpPer = string.Empty;
                if (CTe.CteProc.CTe.infCte.compl.Entrega.semData.tpPer != null)
                {
                    tpPer = CTe.CteProc.CTe.infCte.compl.Entrega.semData.tpPer;
                }
                if (CTe.CteProc.CTe.infCte.compl.Entrega.comData.tpPer != null)
                {
                    tpPer = CTe.CteProc.CTe.infCte.compl.Entrega.comData.tpPer;
                }
                if (CTe.CteProc.CTe.infCte.compl.Entrega.noPeriodo.tpPer != null)
                {
                    tpPer = CTe.CteProc.CTe.infCte.compl.Entrega.noPeriodo.tpPer;
                }
                clsFacil.AdicionarParametro(ref strSQL, tpPer, "@cod_tipo_data_programada", SqlDbType.SmallInt);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.compl.Entrega.comData.dProg, "@dtc_entrega_programada", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.compl.Entrega.noPeriodo.dIni, "@dtc_periodo_definido_inicio", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.compl.Entrega.noPeriodo.dFim, "@dtc_periodo_definido_fim", SqlDbType.VarChar);

                string tpHor = string.Empty;
                if (CTe.CteProc.CTe.infCte.compl.Entrega.semHora.tpHor != null)
                {
                    tpHor = CTe.CteProc.CTe.infCte.compl.Entrega.semHora.tpHor;
                }
                if (CTe.CteProc.CTe.infCte.compl.Entrega.comHora.tpHor != null)
                {
                    tpHor = CTe.CteProc.CTe.infCte.compl.Entrega.comHora.tpHor;
                }
                if (CTe.CteProc.CTe.infCte.compl.Entrega.noInter.tpHor != null)
                {
                    tpHor = CTe.CteProc.CTe.infCte.compl.Entrega.noInter.tpHor;
                }
                clsFacil.AdicionarParametro(ref strSQL, tpHor, "@cod_tipo_hora_programada", SqlDbType.SmallInt);

                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.compl.Entrega.comHora.hProg, "@des_hora_programada", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.compl.Entrega.noInter.hIni, "@des_hora_definido_inicio", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.compl.Entrega.noInter.hFim, "@des_hora_definido_fim", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.compl.origCalc, "@des_municipio_origem_calculo_frete", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.compl.destCalc, "@des_municipio_destino_calculo_frete", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.compl.xObs, "@des_observacao_geral", SqlDbType.VarChar);

                if (CTe.CteProc.CTe.infCte.emit.CPF != null && CTe.CteProc.CTe.infCte.emit.CPF != string.Empty)
                {
                    clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.emit.CPF.Substring(0, 9), "@num_cnpj_cpf_base_emitente", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, "0", "@num_cnpj_cpf_filial_emitente", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.emit.CPF.Substring(9, 2), "@dig_cnpj_cpf_emitente", SqlDbType.SmallInt);
                }
                if (CTe.CteProc.CTe.infCte.emit.CNPJ != null && CTe.CteProc.CTe.infCte.emit.CNPJ != string.Empty)
                {
                    clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.emit.CNPJ.Substring(0, 8), "@num_cnpj_cpf_base_emitente", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.emit.CNPJ.Substring(8, 4), "@num_cnpj_cpf_filial_emitente", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.emit.CNPJ.Substring(12, 2), "@dig_cnpj_cpf_emitente", SqlDbType.SmallInt);
                }
                if ((CTe.CteProc.CTe.infCte.emit.CPF == null && CTe.CteProc.CTe.infCte.emit.CNPJ == null) || (CTe.CteProc.CTe.infCte.emit.CPF == null && CTe.CteProc.CTe.infCte.emit.CNPJ == string.Empty) || (CTe.CteProc.CTe.infCte.emit.CPF == string.Empty && CTe.CteProc.CTe.infCte.emit.CNPJ == null))
                {
                    clsFacil.AdicionarParametro(ref strSQL, string.Empty, "@num_cnpj_cpf_base_emitente", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, string.Empty, "@num_cnpj_cpf_filial_emitente", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, string.Empty, "@dig_cnpj_cpf_emitente", SqlDbType.SmallInt);
                }
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.emit.IE, "@num_insc_estad_emitente", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.emit.xNome, "@nom_razao_social_emitente", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.emit.xFant, "@nom_fantasia_emitente", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.emit.enderEmit.xLgr, "@des_logradouro_emitente", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.emit.enderEmit.nro, "@num_endereco_emitente", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.emit.enderEmit.xCpl, "@des_compl_endereco_emitente", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.emit.enderEmit.xBairro, "@des_bairro_emitente", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.emit.enderEmit.cMun, "@cod_municipio_ibge_emitente", SqlDbType.Int);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.emit.enderEmit.xMun, "@nom_municipio_ibge_emitente", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.ide.modal, "@cod_tipo_modal_transporte_documento_fiscal_eletronico", SqlDbType.SmallInt);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.emit.enderEmit.CEP, "@num_cep_emitente", SqlDbType.BigInt);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.emit.enderEmit.UF, "@sig_unid_federacao_emitente", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.emit.enderEmit.fone, "@num_telefone_emitente", SqlDbType.BigInt);

                if (CTe.CteProc.CTe.infCte.rem.CPF != null && CTe.CteProc.CTe.infCte.rem.CPF != string.Empty)
                {
                    clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.rem.CPF.Substring(0, 9), "@num_cnpj_cpf_base_remetente", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, "0", "@num_cnpj_cpf_filial_remetente", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.rem.CPF.Substring(9, 2), "@dig_cnpj_cpf_remetente", SqlDbType.SmallInt);
                }
                if (CTe.CteProc.CTe.infCte.rem.CNPJ != null && CTe.CteProc.CTe.infCte.rem.CNPJ != string.Empty)
                {
                    clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.rem.CNPJ.Substring(0, 8), "@num_cnpj_cpf_base_remetente", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.rem.CNPJ.Substring(8, 4), "@num_cnpj_cpf_filial_remetente", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.rem.CNPJ.Substring(12, 2), "@dig_cnpj_cpf_remetente", SqlDbType.SmallInt);
                }
                if ((CTe.CteProc.CTe.infCte.rem.CPF == null && CTe.CteProc.CTe.infCte.rem.CNPJ == null) || (CTe.CteProc.CTe.infCte.rem.CPF == null && CTe.CteProc.CTe.infCte.rem.CNPJ == string.Empty) || (CTe.CteProc.CTe.infCte.rem.CPF == string.Empty && CTe.CteProc.CTe.infCte.rem.CNPJ == null))
                {
                    clsFacil.AdicionarParametro(ref strSQL, string.Empty, "@num_cnpj_cpf_base_remetente", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, string.Empty, "@num_cnpj_cpf_filial_remetente", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, string.Empty, "@dig_cnpj_cpf_remetente", SqlDbType.SmallInt);
                }
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.rem.IE, "@num_insc_estad_remetente", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.rem.xNome, "@nom_razao_social_remetente", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.rem.xFant, "@nom_fantasia_remetente", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.rem.fone, "@num_telefone_remetente", SqlDbType.BigInt);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.rem.enderReme.xLgr, "@des_logradouro_remetente", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.rem.enderReme.nro, "@num_endereco_remetente", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.rem.enderReme.xCpl, "@des_compl_endereco_remetente", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.rem.enderReme.xBairro, "@des_bairro_remetente", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.rem.enderReme.cMun, "@cod_municipio_ibge_remetente", SqlDbType.Int);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.rem.enderReme.xMun, "@nom_municipio_ibge_remetente", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.rem.enderReme.CEP, "@num_cep_remetente", SqlDbType.BigInt);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.rem.enderReme.UF, "@sig_unid_federacao_remetente", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.rem.enderReme.cPais, "@cod_pais_bacen_remetente", SqlDbType.SmallInt);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.rem.enderReme.xPais, "@nom_pais_bacen_remetente", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.rem.email, "@nom_email_remetente", SqlDbType.VarChar);

                if (CTe.CteProc.CTe.infCte.exped.CPF != null && CTe.CteProc.CTe.infCte.exped.CPF != string.Empty)
                {
                    clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.exped.CPF.Substring(0, 9), "@num_cnpj_base_expedidor", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, "0", "@num_cnpj_filial_expedidor", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.exped.CPF.Substring(9, 2), "@dig_cnpj_expedidor", SqlDbType.SmallInt);
                }
                if (CTe.CteProc.CTe.infCte.exped.CNPJ != null && CTe.CteProc.CTe.infCte.exped.CNPJ != string.Empty)
                {
                    clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.exped.CNPJ.Substring(0, 8), "@num_cnpj_base_expedidor", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.exped.CNPJ.Substring(8, 4), "@num_cnpj_filial_expedidor", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.exped.CNPJ.Substring(12, 2), "@dig_cnpj_expedidor", SqlDbType.SmallInt);
                }
                if ((CTe.CteProc.CTe.infCte.exped.CPF == null && CTe.CteProc.CTe.infCte.exped.CNPJ == null) || (CTe.CteProc.CTe.infCte.exped.CPF == null && CTe.CteProc.CTe.infCte.exped.CNPJ == string.Empty) || (CTe.CteProc.CTe.infCte.exped.CPF == string.Empty && CTe.CteProc.CTe.infCte.exped.CNPJ == null))
                {
                    clsFacil.AdicionarParametro(ref strSQL, string.Empty, "@num_cnpj_base_expedidor", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, string.Empty, "@num_cnpj_filial_expedidor", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, string.Empty, "@dig_cnpj_expedidor", SqlDbType.SmallInt);
                }
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.exped.IE, "@num_insc_estad_expedidor", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.exped.xNome, "@nom_razao_social_expedidor", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.exped.fone, "@num_telefone_expedidor", SqlDbType.BigInt);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.exped.enderExped.xLgr, "@des_logradouro_expedidor", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.exped.enderExped.nro, "@num_endereco_expedidor", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.exped.enderExped.xCpl, "@des_compl_endereco_expedidor", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.exped.enderExped.xBairro, "@des_bairro_expedidor", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.exped.enderExped.cMun, "@cod_municipio_ibge_expedidor", SqlDbType.Int);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.exped.enderExped.xMun, "@nom_municipio_ibge_expedidor", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.exped.enderExped.CEP, "@num_cep_expedidor", SqlDbType.BigInt);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.exped.enderExped.UF, "@sig_unid_federacao_expedidor", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.exped.enderExped.cPais, "@cod_pais_bacen_expedidor", SqlDbType.SmallInt);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.exped.enderExped.xPais, "@nom_pais_bacen_expedidor", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.exped.email, "@nom_email_expedidor", SqlDbType.VarChar);

                if (CTe.CteProc.CTe.infCte.receb.CPF != null && CTe.CteProc.CTe.infCte.receb.CPF != string.Empty)
                {
                    clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.receb.CPF.Substring(0, 9), "@num_cnpj_base_recebedor", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, "0", "@num_cnpj_filial_recebedor", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.receb.CPF.Substring(9, 2), "@dig_cnpj_recebedor", SqlDbType.SmallInt);
                }
                if (CTe.CteProc.CTe.infCte.receb.CNPJ != null && CTe.CteProc.CTe.infCte.receb.CNPJ != string.Empty)
                {
                    clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.receb.CNPJ.Substring(0, 8), "@num_cnpj_base_recebedor", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.receb.CNPJ.Substring(8, 4), "@num_cnpj_filial_recebedor", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.receb.CNPJ.Substring(12, 2), "@dig_cnpj_recebedor", SqlDbType.SmallInt);
                }
                if ((CTe.CteProc.CTe.infCte.receb.CPF == null && CTe.CteProc.CTe.infCte.receb.CNPJ == null) || (CTe.CteProc.CTe.infCte.receb.CPF == null && CTe.CteProc.CTe.infCte.receb.CNPJ == string.Empty) || (CTe.CteProc.CTe.infCte.receb.CPF == string.Empty && CTe.CteProc.CTe.infCte.receb.CNPJ == null))
                {
                    clsFacil.AdicionarParametro(ref strSQL, string.Empty, "@num_cnpj_base_recebedor", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, string.Empty, "@num_cnpj_filial_recebedor", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, string.Empty, "@dig_cnpj_recebedor", SqlDbType.SmallInt);
                }
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.receb.IE, "@num_insc_estad_recebedor", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.receb.xNome, "@nom_razao_social_recebedor", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.receb.fone, "@num_telefone_recebedor", SqlDbType.BigInt);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.receb.enderReceb.xLgr, "@des_logradouro_recebedor", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.receb.enderReceb.nro, "@num_endereco_recebedor", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.receb.enderReceb.xCpl, "@des_compl_endereco_recebedor", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.receb.enderReceb.xBairro, "@des_bairro_recebedor", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.receb.enderReceb.cMun, "@cod_municipio_ibge_recebedor", SqlDbType.Int);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.receb.enderReceb.xMun, "@nom_municipio_ibge_recebedor", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.receb.enderReceb.CEP, "@num_cep_recebedor", SqlDbType.BigInt);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.receb.enderReceb.UF, "@sig_unid_federacao_recebedor", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.receb.enderReceb.cPais, "@cod_pais_bacen_recebedor", SqlDbType.SmallInt);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.receb.enderReceb.xPais, "@nom_pais_bacen_recebedor", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.receb.email, "@nom_email_recebedor", SqlDbType.VarChar);

                if (CTe.CteProc.CTe.infCte.dest.CPF != null && CTe.CteProc.CTe.infCte.dest.CPF != string.Empty)
                {
                    clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.dest.CPF.Substring(0, 9), "@num_cnpj_cpf_base_destinatario", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, "0", "@num_cnpj_cpf_filial_destinatario", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.dest.CPF.Substring(9, 2), "@dig_cnpj_cpf_destinatario", SqlDbType.SmallInt);
                }
                if (CTe.CteProc.CTe.infCte.dest.CNPJ != null && CTe.CteProc.CTe.infCte.dest.CNPJ != string.Empty)
                {
                    clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.dest.CNPJ.Substring(0, 8), "@num_cnpj_cpf_base_destinatario", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.dest.CNPJ.Substring(8, 4), "@num_cnpj_cpf_filial_destinatario", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.dest.CNPJ.Substring(12, 2), "@dig_cnpj_cpf_destinatario", SqlDbType.SmallInt);
                }
                if ((CTe.CteProc.CTe.infCte.dest.CPF == null && CTe.CteProc.CTe.infCte.dest.CNPJ == null) || (CTe.CteProc.CTe.infCte.dest.CPF == null && CTe.CteProc.CTe.infCte.dest.CNPJ == string.Empty) || (CTe.CteProc.CTe.infCte.dest.CPF == string.Empty && CTe.CteProc.CTe.infCte.dest.CNPJ == null))
                {
                    clsFacil.AdicionarParametro(ref strSQL, string.Empty, "@num_cnpj_cpf_base_destinatario", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, string.Empty, "@num_cnpj_cpf_filial_destinatario", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, string.Empty, "@dig_cnpj_cpf_destinatario", SqlDbType.SmallInt);
                }
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.dest.IE, "@num_insc_estad_destinatario", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.dest.xNome, "@nom_razao_social_destinatario", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.dest.fone, "@num_telefone_destinatario", SqlDbType.BigInt);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.dest.ISUF, "@num_insc_suframa_destinatario", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.dest.enderDest.xLgr, "@des_logradouro_destinatario", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.dest.enderDest.nro, "@num_endereco_destinatario", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.dest.enderDest.xCpl, "@des_compl_endereco_destinatario", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.dest.enderDest.xBairro, "@des_bairro_destinatario", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.dest.enderDest.cMun, "@cod_municipio_ibge_destinatario", SqlDbType.Int);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.dest.enderDest.cMun, "@nom_municipio_ibge_destinatario", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.dest.enderDest.CEP, "@num_cep_destinatario", SqlDbType.BigInt);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.dest.enderDest.UF, "@sig_unid_federacao_destinatario", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.dest.enderDest.cPais, "@cod_pais_bacen_destinatario", SqlDbType.SmallInt);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.dest.enderDest.xPais, "@nom_pais_bacen_destinatario", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.dest.email, "@nom_email_destinatario", SqlDbType.VarChar);

                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTe.CteProc.CTe.infCte.vPrest.vTPrest), "@val_total_prestacao_servico", SqlDbType.Decimal);
                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTe.CteProc.CTe.infCte.vPrest.vRec), "@val_prestacao_servico_receber", SqlDbType.Decimal);
                //ICMS
                string CST = string.Empty, vBC = string.Empty, pICMS = string.Empty, vICMS = string.Empty, pRedBC = string.Empty;
                string vBCSTRet = string.Empty, vICMSSTRet = string.Empty, pICMSSTRet = string.Empty, vCred = string.Empty, pRedBCOutraUF = string.Empty;
                string vBCOutraUF = string.Empty, pICMSOutraUF = string.Empty, vICMSOutraUF = string.Empty; ;
                if (CTe.CteProc.CTe.infCte.imp.ICMS.ICMS00.CST != null)
                {
                    CST = CTe.CteProc.CTe.infCte.imp.ICMS.ICMS00.CST;
                }
                if (CTe.CteProc.CTe.infCte.imp.ICMS.ICMS20.CST != null)
                {
                    CST = CTe.CteProc.CTe.infCte.imp.ICMS.ICMS20.CST;
                }
                if (CTe.CteProc.CTe.infCte.imp.ICMS.ICMS45.CST != null)
                {
                    CST = CTe.CteProc.CTe.infCte.imp.ICMS.ICMS45.CST;
                }
                if (CTe.CteProc.CTe.infCte.imp.ICMS.ICMS60.CST != null)
                {
                    CST = CTe.CteProc.CTe.infCte.imp.ICMS.ICMS60.CST;
                }
                if (CTe.CteProc.CTe.infCte.imp.ICMS.ICMS90.CST != null)
                {
                    CST = CTe.CteProc.CTe.infCte.imp.ICMS.ICMS90.CST;
                }
                if (CTe.CteProc.CTe.infCte.imp.ICMS.ICMSOutraUF.CST != null)
                {
                    CST = CTe.CteProc.CTe.infCte.imp.ICMS.ICMSOutraUF.CST;
                }

                clsFacil.AdicionarParametro(ref strSQL, CST, "@cod_tipo_classificacao_tributaria", SqlDbType.SmallInt);
                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTe.CteProc.CTe.infCte.imp.ICMS.ICMS00.vBC), "@val_base_calculo", SqlDbType.Decimal);
                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTe.CteProc.CTe.infCte.imp.ICMS.ICMS00.pICMS), "@val_aliquota", SqlDbType.Decimal);
                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTe.CteProc.CTe.infCte.imp.ICMS.ICMS00.vICMS), "@val_icms", SqlDbType.Decimal);
                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTe.CteProc.CTe.infCte.imp.ICMS.ICMS20.pRedBC), "@prc_base_calculo_reducao", SqlDbType.Decimal);
                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTe.CteProc.CTe.infCte.imp.ICMS.ICMS20.vBC), "@val_base_calculo_reducao", SqlDbType.Decimal);
                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTe.CteProc.CTe.infCte.imp.ICMS.ICMS20.pICMS), "@prc_aliquota_icms_reducao", SqlDbType.Decimal);
                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTe.CteProc.CTe.infCte.imp.ICMS.ICMS20.vICMS), "@val_aliquota_icms", SqlDbType.Decimal);
                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTe.CteProc.CTe.infCte.imp.ICMS.ICMS60.vBCSTRet), "@val_base_calculo_st_retido", SqlDbType.Decimal);
                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTe.CteProc.CTe.infCte.imp.ICMS.ICMS60.vICMSSTRet), "@val_st_retido", SqlDbType.Decimal);
                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTe.CteProc.CTe.infCte.imp.ICMS.ICMS60.pICMSSTRet), "@prc_aliquota_icms_st_retido", SqlDbType.Decimal);
                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTe.CteProc.CTe.infCte.imp.ICMS.ICMS60.vCred), "@val_st_credito_outorgado", SqlDbType.Decimal);

                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTe.CteProc.CTe.infCte.imp.ICMS.ICMS90.pRedBC), "@per_reducao_base_calculo_outros", SqlDbType.Decimal);
                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTe.CteProc.CTe.infCte.imp.ICMS.ICMS90.vBC), "@val_base_calculo_outros", SqlDbType.Decimal);
                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTe.CteProc.CTe.infCte.imp.ICMS.ICMS90.pICMS), "@prc_aliquota_icms_outros", SqlDbType.Decimal);
                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTe.CteProc.CTe.infCte.imp.ICMS.ICMS90.vICMS), "@val_aliquota_icms_outros", SqlDbType.Decimal);
                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTe.CteProc.CTe.infCte.imp.ICMS.ICMS90.vCred), "@val_credito_outorgado_outros", SqlDbType.Decimal);

                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTe.CteProc.CTe.infCte.imp.ICMS.ICMSOutraUF.pRedBCOutraUF), "@per_reducao_base_calculo_outra_uf", SqlDbType.Decimal);
                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTe.CteProc.CTe.infCte.imp.ICMS.ICMSOutraUF.vBCOutraUF), "@val_base_calculo_outra_uf", SqlDbType.Decimal);
                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTe.CteProc.CTe.infCte.imp.ICMS.ICMSOutraUF.pICMSOutraUF), "@prc_aliquota_icms_outra_uf", SqlDbType.Decimal);
                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTe.CteProc.CTe.infCte.imp.ICMS.ICMSOutraUF.vICMSOutraUF), "@val_icms_outra_uf", SqlDbType.Decimal);

                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.imp.ICMS.ICMSSN.indSN, "@ind_simples_nacional", SqlDbType.SmallInt);
                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTe.CteProc.CTe.infCte.imp.vTotTrib), "@val_total_tributos", SqlDbType.Decimal);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.imp.InfAdFisco, "@des_informacao_adicional_fisco", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTe.CteProc.CTe.infCte.imp.ICMSUFFim.vBCUFFim), "@val_base_calculo_uf_termino", SqlDbType.Decimal);
                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTe.CteProc.CTe.infCte.imp.ICMSUFFim.pFCPUFFim), "@prc_base_calculo_icms_fundo_combate_pobreza_uf_termino", SqlDbType.Decimal);
                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTe.CteProc.CTe.infCte.imp.ICMSUFFim.pICMSUFFim), "@prc_aliquota_interna_icms_uf_termino", SqlDbType.Decimal);
                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTe.CteProc.CTe.infCte.imp.ICMSUFFim.pICMSInter), "@prc_aliquota_interestadual_uf_envolvida", SqlDbType.Decimal);
                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTe.CteProc.CTe.infCte.imp.ICMSUFFim.vFCPUFFim), "@val_icms_fundo_combate_pobreza_uf_termino", SqlDbType.Decimal);
                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTe.CteProc.CTe.infCte.imp.ICMSUFFim.vICMSUFFim), "@val_icms_partilha_uf_termino", SqlDbType.Decimal);
                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTe.CteProc.CTe.infCte.imp.ICMSUFFim.vICMSUFIni), "@val_icms_partilha_uf_inicio", SqlDbType.Decimal);
                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTe.CteProc.CTe.infCte.infCTeNorm.infCarga.vCarga), "@val_total_carga", SqlDbType.Decimal);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.infCTeNorm.infCarga.proPred, "@des_produto_predominante", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.infCTeNorm.infCarga.xOutCat, "@des_outra_caracteristica_carga", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.infCTeNorm.infModal.versaoModal, "@des_versao_leiaute_modal", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.infCTeNorm.infCTeSub.chCte, "@cod_chave_acesso_documento_substituido", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.infCTeNorm.cobr.fat.nFat, "@num_fatura", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTe.CteProc.CTe.infCte.infCTeNorm.cobr.fat.vOrig), "@val_total_fatura", SqlDbType.Decimal);
                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTe.CteProc.CTe.infCte.infCTeNorm.cobr.fat.vDesc), "@val_total_desconto_fatura", SqlDbType.Decimal);
                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTe.CteProc.CTe.infCte.infCTeNorm.cobr.fat.vLiq), "@val_total_liquido_fatura", SqlDbType.Decimal);

                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.ide.procEmi, "@cod_tipo_processo_emissao_conhecimento_transporte_eletronico", SqlDbType.SmallInt);

                clsFacil.AdicionarParametro(ref strSQL, string.Empty, "@cod_tipo_documento_transporte_eletronico_transportado", SqlDbType.SmallInt);

                if (CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infNF.Count > 0)
                {
                    clsFacil.AdicionarParametro(ref strSQL, "1", "@cod_tipo_documento_transporte_eletronico_transportado", SqlDbType.SmallInt);
                }
                if (CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infNFe.Count > 0)
                {
                    clsFacil.AdicionarParametro(ref strSQL, "2", "@cod_tipo_documento_transporte_eletronico_transportado", SqlDbType.SmallInt);
                }
                if (CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infOutros.Count > 0)
                {
                    clsFacil.AdicionarParametro(ref strSQL, "3", "@cod_tipo_documento_transporte_eletronico_transportado", SqlDbType.SmallInt);
                }

                clsFacil.AdicionarParametro(ref strSQL, CTe.NSUSVD, "@num_sequencial_unico", SqlDbType.BigInt);
                clsFacil.AdicionarParametro(ref strSQL, CTe.NSUAut, "@num_sequencial_unico_autorizacao", SqlDbType.BigInt);

                strRetorno = strSQL;

            }
            catch (Exception ex)
            {
                throw new ArgumentException($"[{strMetodo}]: " + ex.Message);
            }
            finally
            {
                stbSQL = null;
                clsFacil = null;
            }

            return strRetorno;
        }

        public string InserirXML_detalhe_xml_conhecimento_transporte_eletronico_autorizado_download(Common.SerializableClasses.CTe.ClsCTe_v2.proc CTe)
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
                for (Lcont = 0; Lcont <= CTe.CteProc.CTe.infCte.autXML.Count - 1; Lcont++)
                {
                    strSQL_aux = string.Empty;
                    stbSQL.Clear();
                    // Montando query a ser executada
                    stbSQL.Append(clsFacil.MontarQuery("INSERT INTO cte.detalhe_xml_conhecimento_transporte_eletronico_autorizado_download "));
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
                    clsFacil.AdicionarParametro(ref strSQL_aux, "20" + CTe.CteProc.protCTe.InfProt.chCTe.Substring(2, 4), "@dtr_referencia", SqlDbType.Int);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.protCTe.InfProt.chCTe, "@cod_chave_acesso", SqlDbType.VarChar);

                    if (CTe.CteProc.CTe.infCte.autXML[Lcont].CPF != null && CTe.CteProc.CTe.infCte.autXML[Lcont].CPF != string.Empty)
                    {
                        clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.autXML[Lcont].CPF.Substring(0, 9), "@num_cnpj_cpf_base_autorizado", SqlDbType.VarChar);
                        clsFacil.AdicionarParametro(ref strSQL_aux, "0", "@num_cnpj_cpf_filial_autorizado", SqlDbType.VarChar);
                        clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.autXML[Lcont].CPF.Substring(9, 2), "@dig_cnpj_cpf_autorizado", SqlDbType.SmallInt);
                    }
                    if (CTe.CteProc.CTe.infCte.autXML[Lcont].CNPJ != null && CTe.CteProc.CTe.infCte.autXML[Lcont].CNPJ != string.Empty)
                    {
                        clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.autXML[Lcont].CNPJ.Substring(0, 8), "@num_cnpj_cpf_base_autorizado", SqlDbType.VarChar);
                        clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.autXML[Lcont].CNPJ.Substring(8, 4), "@num_cnpj_cpf_filial_autorizado", SqlDbType.VarChar);
                        clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.autXML[Lcont].CNPJ.Substring(12, 2), "@dig_cnpj_cpf_autorizado", SqlDbType.SmallInt);
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
                clsFacil = null;
            }

            return strRetorno;
        }

        public string InserirXML_detalhe_xml_conhecimento_transporte_eletronico_fluxo_carga(Common.SerializableClasses.CTe.ClsCTe_v2.proc CTe)
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
                for (Lcont = 0; Lcont <= CTe.CteProc.CTe.infCte.compl.fluxo.pass.Count - 1; Lcont++)
                {
                    strSQL_aux = string.Empty;
                    stbSQL.Clear();
                    // Montando query a ser executada
                    stbSQL.Append(clsFacil.MontarQuery("INSERT INTO cte.detalhe_xml_conhecimento_transporte_eletronico_fluxo_carga "));
                    stbSQL.Append(clsFacil.MontarQuery("( "));
                    stbSQL.Append(clsFacil.MontarQuery("  dtr_referencia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso, "));
                    stbSQL.Append(clsFacil.MontarQuery("  seq_detalhe_xml_conhecimento_transporte_eletronico_fluxo_carga, "));
                    stbSQL.Append(clsFacil.MontarQuery("  cod_interno_origem, "));
                    stbSQL.Append(clsFacil.MontarQuery("  cod_interno_passagem, "));
                    stbSQL.Append(clsFacil.MontarQuery("  cod_interno_destino, "));
                    stbSQL.Append(clsFacil.MontarQuery("  cod_rota_entrega, "));
                    stbSQL.Append(clsFacil.MontarQuery("  dtc_atualizacao "));
                    stbSQL.Append(clsFacil.MontarQuery(") "));
                    stbSQL.Append(clsFacil.MontarQuery("VALUES "));
                    stbSQL.Append(clsFacil.MontarQuery("( "));
                    stbSQL.Append(clsFacil.MontarQuery("  @dtr_referencia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @seq_detalhe_xml_conhecimento_transporte_eletronico_fluxo_carga, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @cod_interno_origem, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @cod_interno_passagem, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @cod_interno_destino, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @cod_rota_entrega, "));
                    stbSQL.Append(clsFacil.MontarQuery("  getdate() "));
                    stbSQL.Append(clsFacil.MontarQuery(") "));
                    strSQL_aux = stbSQL.ToString();

                    // Montando parametros
                    clsDados.LimparParametro();
                    clsFacil.AdicionarParametro(ref strSQL_aux, "20" + CTe.CteProc.protCTe.InfProt.chCTe.Substring(2, 4), "@dtr_referencia", SqlDbType.Int);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.protCTe.InfProt.chCTe, "@cod_chave_acesso", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, Convert.ToString(Lcont + 1), "@seq_detalhe_xml_conhecimento_transporte_eletronico_fluxo_carga", SqlDbType.Int);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.compl.fluxo.xOrig, "@cod_interno_origem", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.compl.fluxo.pass[Lcont].xPass, "@cod_interno_passagem", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.compl.fluxo.xDest, "@cod_interno_destino", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.compl.fluxo.xRota, "@cod_rota_entrega", SqlDbType.VarChar);
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

        public string InserirXML_detalhe_xml_conhecimento_transporte_eletronico_informacao_contribuinte(Common.SerializableClasses.CTe.ClsCTe_v2.proc CTe)
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
                for (Lcont = 0; Lcont <= CTe.CteProc.CTe.infCte.compl.ObsCont.Count - 1; Lcont++)
                {
                    strSQL_aux = string.Empty;
                    stbSQL.Clear();
                    // Montando query a ser executada
                    stbSQL.Append(clsFacil.MontarQuery("INSERT INTO cte.detalhe_xml_conhecimento_transporte_eletronico_informacao_contribuinte "));
                    stbSQL.Append(clsFacil.MontarQuery("( "));
                    stbSQL.Append(clsFacil.MontarQuery("  dtr_referencia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso, "));
                    stbSQL.Append(clsFacil.MontarQuery("  seq_detalhe_xml_conhecimento_transporte_eletronico_informacao_contribuinte, "));
                    stbSQL.Append(clsFacil.MontarQuery("  nom_campo_livre_contribuinte, "));
                    stbSQL.Append(clsFacil.MontarQuery("  des_campo_livre_contribuinte, "));
                    stbSQL.Append(clsFacil.MontarQuery("  dtc_atualizacao "));
                    stbSQL.Append(clsFacil.MontarQuery(") "));
                    stbSQL.Append(clsFacil.MontarQuery("VALUES "));
                    stbSQL.Append(clsFacil.MontarQuery("( "));
                    stbSQL.Append(clsFacil.MontarQuery("  @dtr_referencia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @seq_detalhe_xml_conhecimento_transporte_eletronico_informacao_contribuinte, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @nom_campo_livre_contribuinte, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @des_campo_livre_contribuinte, "));
                    stbSQL.Append(clsFacil.MontarQuery("  getdate() "));
                    stbSQL.Append(clsFacil.MontarQuery(") "));
                    strSQL_aux = stbSQL.ToString();

                    // Montando parametros
                    clsDados.LimparParametro();
                    clsFacil.AdicionarParametro(ref strSQL_aux, "20" + CTe.CteProc.protCTe.InfProt.chCTe.Substring(2, 4), "@dtr_referencia", SqlDbType.Int);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.protCTe.InfProt.chCTe, "@cod_chave_acesso", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, Convert.ToString(Lcont + 1), "@seq_detalhe_xml_conhecimento_transporte_eletronico_informacao_contribuinte", SqlDbType.Int);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.compl.ObsCont[Lcont].xCampo, "@nom_campo_livre_contribuinte", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.compl.ObsCont[Lcont].xTexto, "@des_campo_livre_contribuinte", SqlDbType.VarChar);
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

        public string InserirXML_detalhe_xml_conhecimento_transporte_eletronico_informacao_fisco(Common.SerializableClasses.CTe.ClsCTe_v2.proc CTe)
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
                for (Lcont = 0; Lcont <= CTe.CteProc.CTe.infCte.compl.ObsFisco.Count - 1; Lcont++)
                {
                    strSQL_aux = string.Empty;
                    stbSQL.Clear();
                    // Montando query a ser executada
                    stbSQL.Append(clsFacil.MontarQuery("INSERT INTO cte.detalhe_xml_conhecimento_transporte_eletronico_informacao_fisco "));
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
                    clsFacil.AdicionarParametro(ref strSQL_aux, "20" + CTe.CteProc.protCTe.InfProt.chCTe.Substring(2, 4), "@dtr_referencia", SqlDbType.Int);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.protCTe.InfProt.chCTe, "@cod_chave_acesso", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, Convert.ToString(Lcont + 1), "@seq_detalhe_xml_conhecimento_transporte_eletronico_informacao_fisco", SqlDbType.Int);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.compl.ObsFisco[Lcont].xCampo, "@nom_campo_livre_fisco", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.compl.ObsFisco[Lcont].xTexto, "@des_campo_livre_fisco", SqlDbType.VarChar);
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

        public string InserirXML_detalhe_xml_conhecimento_transporte_eletronico_componente_prestacao(Common.SerializableClasses.CTe.ClsCTe_v2.proc CTe)
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
                for (Lcont = 0; Lcont <= CTe.CteProc.CTe.infCte.vPrest.comp.Count - 1; Lcont++)
                {
                    strSQL_aux = string.Empty;
                    stbSQL.Clear();
                    // Montando query a ser executada
                    stbSQL.Append(clsFacil.MontarQuery("INSERT INTO cte.detalhe_xml_conhecimento_transporte_eletronico_componente_prestacao "));
                    stbSQL.Append(clsFacil.MontarQuery("( "));
                    stbSQL.Append(clsFacil.MontarQuery("  dtr_referencia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso, "));
                    stbSQL.Append(clsFacil.MontarQuery("  seq_detalhe_xml_conhecimento_transporte_eletronico_componente_prestacao, "));
                    stbSQL.Append(clsFacil.MontarQuery("  nom_componente_prestacao, "));
                    stbSQL.Append(clsFacil.MontarQuery("  val_componente_prestacao, "));
                    stbSQL.Append(clsFacil.MontarQuery("  dtc_atualizacao "));
                    stbSQL.Append(clsFacil.MontarQuery(") "));
                    stbSQL.Append(clsFacil.MontarQuery("VALUES "));
                    stbSQL.Append(clsFacil.MontarQuery("( "));
                    stbSQL.Append(clsFacil.MontarQuery("  @dtr_referencia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @seq_detalhe_xml_conhecimento_transporte_eletronico_componente_prestacao, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @nom_componente_prestacao, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @val_componente_prestacao, "));
                    stbSQL.Append(clsFacil.MontarQuery("  getdate() "));
                    stbSQL.Append(clsFacil.MontarQuery(") "));
                    strSQL_aux = stbSQL.ToString();

                    // Montando parametros
                    clsDados.LimparParametro();
                    clsFacil.AdicionarParametro(ref strSQL_aux, "20" + CTe.CteProc.protCTe.InfProt.chCTe.Substring(2, 4), "@dtr_referencia", SqlDbType.Int);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.protCTe.InfProt.chCTe, "@cod_chave_acesso", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, Convert.ToString(Lcont + 1), "@seq_detalhe_xml_conhecimento_transporte_eletronico_componente_prestacao", SqlDbType.Int);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.vPrest.comp[Lcont].xNome, "@nom_componente_prestacao", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.vPrest.comp[Lcont].vComp, "@val_componente_prestacao", SqlDbType.Decimal);
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

        public string InserirXML_detalhe_xml_conhecimento_transporte_eletronico_carga(Common.SerializableClasses.CTe.ClsCTe_v2.proc CTe)
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
                for (Lcont = 0; Lcont <= CTe.CteProc.CTe.infCte.infCTeNorm.infCarga.infQ.Count - 1; Lcont++)
                {
                    strSQL_aux = string.Empty;
                    stbSQL.Clear();
                    // Montando query a ser executada
                    stbSQL.Append(clsFacil.MontarQuery("INSERT INTO cte.detalhe_xml_conhecimento_transporte_eletronico_carga "));
                    stbSQL.Append(clsFacil.MontarQuery("( "));
                    stbSQL.Append(clsFacil.MontarQuery("  dtr_referencia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso, "));
                    stbSQL.Append(clsFacil.MontarQuery("  seq_detalhe_xml_conhecimento_transporte_eletronico_carga, "));
                    stbSQL.Append(clsFacil.MontarQuery("  cod_tipo_unidade_medida_carga, "));
                    stbSQL.Append(clsFacil.MontarQuery("  des_tipo_unidade_medida_carga, "));
                    stbSQL.Append(clsFacil.MontarQuery("  qtd_carga, "));
                    stbSQL.Append(clsFacil.MontarQuery("  dtc_atualizacao "));
                    stbSQL.Append(clsFacil.MontarQuery(") "));
                    stbSQL.Append(clsFacil.MontarQuery("VALUES "));
                    stbSQL.Append(clsFacil.MontarQuery("( "));
                    stbSQL.Append(clsFacil.MontarQuery("  @dtr_referencia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @seq_detalhe_xml_conhecimento_transporte_eletronico_carga, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @cod_tipo_unidade_medida_carga, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @des_tipo_unidade_medida_carga, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @qtd_carga, "));
                    stbSQL.Append(clsFacil.MontarQuery("  getdate() "));
                    stbSQL.Append(clsFacil.MontarQuery(") "));
                    strSQL_aux = stbSQL.ToString();

                    // Montando parametros
                    clsDados.LimparParametro();
                    clsFacil.AdicionarParametro(ref strSQL_aux, "20" + CTe.CteProc.protCTe.InfProt.chCTe.Substring(2, 4), "@dtr_referencia", SqlDbType.Int);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.protCTe.InfProt.chCTe, "@cod_chave_acesso", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, Convert.ToString(Lcont + 1), "@seq_detalhe_xml_conhecimento_transporte_eletronico_carga", SqlDbType.SmallInt);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.infCarga.infQ[Lcont].cUnid, "@cod_tipo_unidade_medida_carga", SqlDbType.SmallInt);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.infCarga.infQ[Lcont].tpMed, "@des_tipo_unidade_medida_carga", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, Convert.ToString(CTe.CteProc.CTe.infCte.infCTeNorm.infCarga.infQ[Lcont].qCarga), "@qtd_carga", SqlDbType.Decimal);
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

        #region "infNF"

        public string InserirXML_detalhe_xml_conhecimento_transporte_eletronico_nota_fiscal(Common.SerializableClasses.CTe.ClsCTe_v2.proc CTe)
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
                for (Lcont = 0; Lcont <= CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infNF.Count - 1; Lcont++)
                {
                    strSQL_aux = string.Empty;
                    stbSQL.Clear();
                    // Montando query a ser executada
                    stbSQL.Append(clsFacil.MontarQuery("INSERT INTO cte.detalhe_xml_conhecimento_transporte_eletronico_nota_fiscal "));
                    stbSQL.Append(clsFacil.MontarQuery("( "));
                    stbSQL.Append(clsFacil.MontarQuery("  dtr_referencia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso, "));
                    stbSQL.Append(clsFacil.MontarQuery("  seq_detalhe_xml_conhecimento_transporte_eletronico_nota_fiscal, "));
                    stbSQL.Append(clsFacil.MontarQuery("  num_romaneio_nota_fiscal, "));
                    stbSQL.Append(clsFacil.MontarQuery("  num_pedido_nota_fiscal, "));
                    stbSQL.Append(clsFacil.MontarQuery("  cod_modelo_nota_fiscal, "));
                    stbSQL.Append(clsFacil.MontarQuery("  num_serie_nota_fiscal, "));
                    stbSQL.Append(clsFacil.MontarQuery("  num_nota_fiscal, "));
                    stbSQL.Append(clsFacil.MontarQuery("  dtc_emissao_nota_fiscal, "));
                    stbSQL.Append(clsFacil.MontarQuery("  val_base_calculo, "));
                    stbSQL.Append(clsFacil.MontarQuery("  val_total_icms_base_calculo, "));
                    stbSQL.Append(clsFacil.MontarQuery("  val_st_base_calculo, "));
                    stbSQL.Append(clsFacil.MontarQuery("  val_total_st, "));
                    stbSQL.Append(clsFacil.MontarQuery("  val_total_produto, "));
                    stbSQL.Append(clsFacil.MontarQuery("  val_total_nota_fiscal, "));
                    stbSQL.Append(clsFacil.MontarQuery("  cod_cfop_predominante, "));
                    stbSQL.Append(clsFacil.MontarQuery("  val_total_peso, "));
                    stbSQL.Append(clsFacil.MontarQuery("  num_pin_suframa, "));
                    stbSQL.Append(clsFacil.MontarQuery("  dtc_previsao_entrega, "));
                    stbSQL.Append(clsFacil.MontarQuery("  dtc_atualizacao "));
                    stbSQL.Append(clsFacil.MontarQuery(") "));
                    stbSQL.Append(clsFacil.MontarQuery("VALUES "));
                    stbSQL.Append(clsFacil.MontarQuery("( "));
                    stbSQL.Append(clsFacil.MontarQuery("  @dtr_referencia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @seq_detalhe_xml_conhecimento_transporte_eletronico_nota_fiscal, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @num_romaneio_nota_fiscal, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @num_pedido_nota_fiscal, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @cod_modelo_nota_fiscal, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @num_serie_nota_fiscal, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @num_nota_fiscal, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @dtc_emissao_nota_fiscal, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @val_base_calculo, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @val_total_icms_base_calculo, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @val_st_base_calculo, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @val_total_st, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @val_total_produto, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @val_total_nota_fiscal, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @cod_cfop_predominante, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @val_total_peso, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @num_pin_suframa, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @dtc_previsao_entrega, "));
                    stbSQL.Append(clsFacil.MontarQuery("  getdate() "));
                    stbSQL.Append(clsFacil.MontarQuery(") "));
                    strSQL_aux = stbSQL.ToString();

                    // Montando parametros
                    clsDados.LimparParametro();
                    clsFacil.AdicionarParametro(ref strSQL_aux, "20" + CTe.CteProc.protCTe.InfProt.chCTe.Substring(2, 4), "@dtr_referencia", SqlDbType.Int);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.protCTe.InfProt.chCTe, "@cod_chave_acesso", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, Convert.ToString(Lcont + 1), "@seq_detalhe_xml_conhecimento_transporte_eletronico_nota_fiscal", SqlDbType.Int);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infNF[Lcont].nRoma, "@num_romaneio_nota_fiscal", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infNF[Lcont].nPed, "@num_pedido_nota_fiscal", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infNF[Lcont].mod, "@cod_modelo_nota_fiscal", SqlDbType.SmallInt);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infNF[Lcont].serie, "@num_serie_nota_fiscal", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infNF[Lcont].nDoc, "@num_nota_fiscal", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, Convert.ToString(CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infNF[Lcont].dEmi), "@dtc_emissao_nota_fiscal", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, Convert.ToString(CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infNF[Lcont].vBC), "@val_base_calculo", SqlDbType.Decimal);
                    clsFacil.AdicionarParametro(ref strSQL_aux, Convert.ToString(CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infNF[Lcont].vICMS), "@val_total_icms_base_calculo", SqlDbType.Decimal);
                    clsFacil.AdicionarParametro(ref strSQL_aux, Convert.ToString(CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infNF[Lcont].vBCST), "@val_st_base_calculo", SqlDbType.Decimal);
                    clsFacil.AdicionarParametro(ref strSQL_aux, Convert.ToString(CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infNF[Lcont].vST), "@val_total_st", SqlDbType.Decimal);
                    clsFacil.AdicionarParametro(ref strSQL_aux, Convert.ToString(CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infNF[Lcont].vProd), "@val_total_produto", SqlDbType.Decimal);
                    clsFacil.AdicionarParametro(ref strSQL_aux, Convert.ToString(CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infNF[Lcont].vNF), "@val_total_nota_fiscal", SqlDbType.Decimal);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infNF[Lcont].nCFOP, "@cod_cfop_predominante", SqlDbType.SmallInt);
                    clsFacil.AdicionarParametro(ref strSQL_aux, Convert.ToString(CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infNF[Lcont].nPeso), "@val_total_peso", SqlDbType.Decimal);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infNF[Lcont].PIN, "@num_pin_suframa", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, Convert.ToString(CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infNF[Lcont].dPrev), "@dtc_previsao_entrega", SqlDbType.VarChar);
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

        public string InserirXML_detalhe_xml_conhecimento_transporte_eletronico_unidade_carga_infNF(Common.SerializableClasses.CTe.ClsCTe_v2.proc CTe)
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
                for (i = 0; i <= CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infNF.Count - 1; i++)
                {
                    int j = 0;
                    for (j = 0; j <= CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infNF[i].infUnidCarga.Count - 1; j++)
                    {
                        strSQL_aux = string.Empty;
                        stbSQL.Clear();

                        // Montando query a ser executada
                        stbSQL.Append(clsFacil.MontarQuery("INSERT INTO cte.detalhe_xml_conhecimento_transporte_eletronico_unidade_carga "));
                        stbSQL.Append(clsFacil.MontarQuery("( "));
                        stbSQL.Append(clsFacil.MontarQuery("  dtr_referencia, "));
                        stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso, "));
                        stbSQL.Append(clsFacil.MontarQuery("  num_identificacao_unidade_carga, "));
                        stbSQL.Append(clsFacil.MontarQuery("  cod_tipo_unidade_carga_documento_fiscal_eletronico, "));
                        stbSQL.Append(clsFacil.MontarQuery("  qtd_rateada_unidade_carga, "));
                        stbSQL.Append(clsFacil.MontarQuery("  dtc_atualizacao "));
                        stbSQL.Append(clsFacil.MontarQuery(") "));
                        stbSQL.Append(clsFacil.MontarQuery("VALUES "));
                        stbSQL.Append(clsFacil.MontarQuery("( "));
                        stbSQL.Append(clsFacil.MontarQuery("  @dtr_referencia, "));
                        stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso, "));
                        stbSQL.Append(clsFacil.MontarQuery("  @num_identificacao_unidade_carga, "));
                        stbSQL.Append(clsFacil.MontarQuery("  @cod_tipo_unidade_carga_documento_fiscal_eletronico, "));
                        stbSQL.Append(clsFacil.MontarQuery("  @qtd_rateada_unidade_carga, "));
                        stbSQL.Append(clsFacil.MontarQuery("  getdate() "));
                        stbSQL.Append(clsFacil.MontarQuery(") "));
                        strSQL_aux = stbSQL.ToString();

                        // Montando parametros
                        clsDados.LimparParametro();
                        clsFacil.AdicionarParametro(ref strSQL_aux, "20" + CTe.CteProc.protCTe.InfProt.chCTe.Substring(2, 4), "@dtr_referencia", SqlDbType.Int);
                        clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.protCTe.InfProt.chCTe, "@cod_chave_acesso", SqlDbType.VarChar);
                        clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infNF[i].infUnidCarga[j].idUnidCarga, "@num_identificacao_unidade_carga", SqlDbType.VarChar);
                        clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infNF[i].infUnidCarga[j].tpUnidCarga, "@cod_tipo_unidade_carga_documento_fiscal_eletronico", SqlDbType.SmallInt);
                        clsFacil.AdicionarParametro(ref strSQL_aux, Convert.ToString(CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infNF[i].infUnidCarga[j].qtdRat), "@qtd_rateada_unidade_carga", SqlDbType.Decimal);
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

        public string InserirXML_detalhe_xml_conhecimento_transporte_eletronico_unidade_carga_lacre_infNF(Common.SerializableClasses.CTe.ClsCTe_v2.proc CTe)
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
                for (i = 0; i <= CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infNF.Count - 1; i++)
                {
                    int j = 0;
                    for (j = 0; j <= CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infNF[i].infUnidCarga.Count - 1; j++)
                    {
                        int k = 0;
                        for (k = 0; k <= CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infNF[i].infUnidCarga[j].lacUnidCarga.Count - 1; k++)
                        {
                            strSQL_aux = string.Empty;
                            stbSQL.Clear();
                            // Montando query a ser executada
                            stbSQL.Append(clsFacil.MontarQuery("INSERT INTO cte.detalhe_xml_conhecimento_transporte_eletronico_unidade_carga_lacre "));
                            stbSQL.Append(clsFacil.MontarQuery("( "));
                            stbSQL.Append(clsFacil.MontarQuery("  dtr_referencia, "));
                            stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso, "));
                            stbSQL.Append(clsFacil.MontarQuery("  num_identificacao_unidade_carga, "));
                            stbSQL.Append(clsFacil.MontarQuery("  num_lacre_unidade_carga, "));
                            stbSQL.Append(clsFacil.MontarQuery("  dtc_atualizacao "));
                            stbSQL.Append(clsFacil.MontarQuery(") "));
                            stbSQL.Append(clsFacil.MontarQuery("VALUES "));
                            stbSQL.Append(clsFacil.MontarQuery("( "));
                            stbSQL.Append(clsFacil.MontarQuery("  @dtr_referencia, "));
                            stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso, "));
                            stbSQL.Append(clsFacil.MontarQuery("  @num_identificacao_unidade_carga, "));
                            stbSQL.Append(clsFacil.MontarQuery("  @num_lacre_unidade_carga, "));
                            stbSQL.Append(clsFacil.MontarQuery("  getdate() "));
                            stbSQL.Append(clsFacil.MontarQuery(") "));
                            strSQL_aux = stbSQL.ToString();

                            // Montando parametros
                            clsDados.LimparParametro();
                            clsFacil.AdicionarParametro(ref strSQL_aux, "20" + CTe.CteProc.protCTe.InfProt.chCTe.Substring(2, 4), "@dtr_referencia", SqlDbType.Int);
                            clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.protCTe.InfProt.chCTe, "@cod_chave_acesso", SqlDbType.VarChar);
                            clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infNF[i].infUnidCarga[j].idUnidCarga, "@num_identificacao_unidade_carga", SqlDbType.VarChar);
                            clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infNF[i].infUnidCarga[j].lacUnidCarga[k].nLacre, "@num_lacre_unidade_carga", SqlDbType.VarChar);
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

        public string InserirXML_detalhe_xml_conhecimento_transporte_eletronico_unidade_transporte_infNF(Common.SerializableClasses.CTe.ClsCTe_v2.proc CTe)
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
                for (i = 0; i <= CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infNF.Count - 1; i++)
                {
                    int j = 0;
                    for (j = 0; j <= CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infNF[i].infUnidTransp.Count - 1; j++)
                    {
                        strSQL_aux = string.Empty;
                        stbSQL.Clear();
                        // Montando query a ser executada
                        stbSQL.Append(clsFacil.MontarQuery("INSERT INTO cte.detalhe_xml_conhecimento_transporte_eletronico_unidade_transporte "));
                        stbSQL.Append(clsFacil.MontarQuery("( "));
                        stbSQL.Append(clsFacil.MontarQuery("  dtr_referencia, "));
                        stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso, "));
                        stbSQL.Append(clsFacil.MontarQuery("  num_identificacao_unidade_transporte, "));
                        stbSQL.Append(clsFacil.MontarQuery("  cod_tipo_unidade_transporte_documento_fiscal_eletronico, "));
                        stbSQL.Append(clsFacil.MontarQuery("  qtd_rateada_unidade_carga, "));
                        stbSQL.Append(clsFacil.MontarQuery("  dtc_atualizacao "));
                        stbSQL.Append(clsFacil.MontarQuery(") "));
                        stbSQL.Append(clsFacil.MontarQuery("VALUES "));
                        stbSQL.Append(clsFacil.MontarQuery("( "));
                        stbSQL.Append(clsFacil.MontarQuery("  @dtr_referencia, "));
                        stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso, "));
                        stbSQL.Append(clsFacil.MontarQuery("  @num_identificacao_unidade_transporte, "));
                        stbSQL.Append(clsFacil.MontarQuery("  @cod_tipo_unidade_transporte_documento_fiscal_eletronico, "));
                        stbSQL.Append(clsFacil.MontarQuery("  @qtd_rateada_unidade_carga, "));
                        stbSQL.Append(clsFacil.MontarQuery("  getdate() "));
                        stbSQL.Append(clsFacil.MontarQuery(") "));
                        strSQL_aux = stbSQL.ToString();

                        // Montando parametros
                        clsDados.LimparParametro();
                        clsFacil.AdicionarParametro(ref strSQL_aux, "20" + CTe.CteProc.protCTe.InfProt.chCTe.Substring(2, 4), "@dtr_referencia", SqlDbType.Int);
                        clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.protCTe.InfProt.chCTe, "@cod_chave_acesso", SqlDbType.VarChar);
                        clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infNF[i].infUnidTransp[j].idUnidTransp, "@num_identificacao_unidade_transporte", SqlDbType.VarChar);
                        clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infNF[i].infUnidTransp[j].tpUnidTransp, "@cod_tipo_unidade_transporte_documento_fiscal_eletronico", SqlDbType.SmallInt);
                        clsFacil.AdicionarParametro(ref strSQL_aux, Convert.ToString(CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infNF[i].infUnidTransp[j].qtdRat), "@qtd_rateada_unidade_carga", SqlDbType.Decimal);
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

        public string InserirXML_detalhe_xml_conhecimento_transporte_eletronico_unidade_transporte_lacre_infNF(Common.SerializableClasses.CTe.ClsCTe_v2.proc CTe)
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
                for (i = 0; i <= CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infNF.Count - 1; i++)
                {
                    int j = 0;
                    for (j = 0; j <= CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infNF[i].infUnidTransp.Count - 1; j++)
                    {
                        int k = 0;
                        for (k = 0; k <= CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infNF[i].infUnidTransp[j].lacUnidTransp.Count - 1; k++)
                        {
                            strSQL_aux = string.Empty;
                            stbSQL.Clear();

                            // Montando query a ser executada
                            stbSQL.Append(clsFacil.MontarQuery("INSERT INTO cte.detalhe_xml_conhecimento_transporte_eletronico_unidade_transporte_lacre "));
                            stbSQL.Append(clsFacil.MontarQuery("( "));
                            stbSQL.Append(clsFacil.MontarQuery("  dtr_referencia, "));
                            stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso, "));
                            stbSQL.Append(clsFacil.MontarQuery("  num_identificacao_unidade_transporte, "));
                            stbSQL.Append(clsFacil.MontarQuery("  num_lacre_unidade_transporte, "));
                            stbSQL.Append(clsFacil.MontarQuery("  dtc_atualizacao "));
                            stbSQL.Append(clsFacil.MontarQuery(") "));
                            stbSQL.Append(clsFacil.MontarQuery("VALUES "));
                            stbSQL.Append(clsFacil.MontarQuery("( "));
                            stbSQL.Append(clsFacil.MontarQuery("  @dtr_referencia, "));
                            stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso, "));
                            stbSQL.Append(clsFacil.MontarQuery("  @num_identificacao_unidade_transporte, "));
                            stbSQL.Append(clsFacil.MontarQuery("  @num_lacre_unidade_transporte, "));
                            stbSQL.Append(clsFacil.MontarQuery("  getdate() "));
                            stbSQL.Append(clsFacil.MontarQuery(") "));
                            strSQL_aux = stbSQL.ToString();

                            // Montando parametros
                            clsDados.LimparParametro();
                            clsFacil.AdicionarParametro(ref strSQL_aux, "20" + CTe.CteProc.protCTe.InfProt.chCTe.Substring(2, 4), "@dtr_referencia", SqlDbType.Int);
                            clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.protCTe.InfProt.chCTe, "@cod_chave_acesso", SqlDbType.VarChar);
                            clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infNF[i].infUnidTransp[j].idUnidTransp, "@num_identificacao_unidade_transporte", SqlDbType.VarChar);
                            clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infNF[i].infUnidTransp[j].lacUnidTransp[k].nLacre, "@num_lacre_unidade_transporte", SqlDbType.VarChar);
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

        public string InserirXML_detalhe_xml_conhecimento_transporte_eletronico_unidade_transporte_unidade_carga_infNF(Common.SerializableClasses.CTe.ClsCTe_v2.proc CTe)
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
                for (i = 0; i <= CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infNF.Count - 1; i++)
                {
                    int j = 0;
                    for (j = 0; j <= CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infNF[i].infUnidTransp.Count - 1; j++)
                    {
                        int k = 0;
                        for (k = 0; k <= CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infNF[i].infUnidTransp[j].infUnidCarga.Count - 1; k++)
                        {
                            strSQL_aux = string.Empty;
                            stbSQL.Clear();

                            // Montando query a ser executada
                            stbSQL.Append(clsFacil.MontarQuery("INSERT INTO cte.detalhe_xml_conhecimento_transporte_eletronico_unidade_transporte_unidade_carga "));
                            stbSQL.Append(clsFacil.MontarQuery("( "));
                            stbSQL.Append(clsFacil.MontarQuery("  dtr_referencia, "));
                            stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso, "));
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
                            stbSQL.Append(clsFacil.MontarQuery("  @num_identificacao_unidade_transporte, "));
                            stbSQL.Append(clsFacil.MontarQuery("  @num_identificacao_unidade_carga, "));
                            stbSQL.Append(clsFacil.MontarQuery("  @cod_tipo_unidade_carga_documento_fiscal_eletronico, "));
                            stbSQL.Append(clsFacil.MontarQuery("  @qtd_rateada_unidade_carga, "));
                            stbSQL.Append(clsFacil.MontarQuery("  getdate() "));
                            stbSQL.Append(clsFacil.MontarQuery(") "));
                            strSQL_aux = stbSQL.ToString();

                            // Montando parametros
                            clsDados.LimparParametro();
                            clsFacil.AdicionarParametro(ref strSQL_aux, "20" + CTe.CteProc.protCTe.InfProt.chCTe.Substring(2, 4), "@dtr_referencia", SqlDbType.Int);
                            clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.protCTe.InfProt.chCTe, "@cod_chave_acesso", SqlDbType.VarChar);
                            clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infNF[i].infUnidTransp[j].idUnidTransp, "@num_identificacao_unidade_transporte", SqlDbType.VarChar);
                            clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infNF[i].infUnidTransp[j].infUnidCarga[k].idUnidCarga, "@num_identificacao_unidade_carga", SqlDbType.VarChar);
                            clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infNF[i].infUnidTransp[j].infUnidCarga[k].tpUnidCarga, "@cod_tipo_unidade_carga_documento_fiscal_eletronico", SqlDbType.SmallInt);
                            clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infNF[i].infUnidTransp[j].infUnidCarga[k].qtdRat), "@qtd_rateada_unidade_carga", SqlDbType.Decimal);

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

        public string InserirXML_detalhe_xml_conhecimento_transporte_eletronico_unidade_transporte_unidade_carga_lacre_infNF(Common.SerializableClasses.CTe.ClsCTe_v2.proc CTe)
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
                for (i = 0; i <= CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infNF.Count - 1; i++)
                {
                    int j = 0;
                    for (j = 0; j <= CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infNF[i].infUnidTransp.Count - 1; j++)
                    {
                        int k = 0;
                        for (k = 0; k <= CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infNF[i].infUnidTransp[j].infUnidCarga.Count - 1; k++)
                        {
                            int l = 0;
                            for (l = 0; l <= CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infNF[i].infUnidTransp[j].infUnidCarga[k].lacUnidCarga.Count - 1; l++)
                            {
                                strSQL_aux = string.Empty;
                                stbSQL.Clear();

                                // Montando query a ser executada
                                stbSQL.Append(clsFacil.MontarQuery("INSERT INTO cte.detalhe_xml_conhecimento_transporte_eletronico_unidade_transporte_unidade_carga_lacre "));
                                stbSQL.Append(clsFacil.MontarQuery("( "));
                                stbSQL.Append(clsFacil.MontarQuery("  dtr_referencia, "));
                                stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso, "));
                                stbSQL.Append(clsFacil.MontarQuery("  num_identificacao_unidade_transporte, "));
                                stbSQL.Append(clsFacil.MontarQuery("  num_identificacao_unidade_carga, "));
                                stbSQL.Append(clsFacil.MontarQuery("  num_lacre_unidade_carga, "));
                                stbSQL.Append(clsFacil.MontarQuery("  dtc_atualizacao "));
                                stbSQL.Append(clsFacil.MontarQuery(") "));
                                stbSQL.Append(clsFacil.MontarQuery("VALUES "));
                                stbSQL.Append(clsFacil.MontarQuery("( "));
                                stbSQL.Append(clsFacil.MontarQuery("  @dtr_referencia, "));
                                stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso, "));
                                stbSQL.Append(clsFacil.MontarQuery("  @num_identificacao_unidade_transporte, "));
                                stbSQL.Append(clsFacil.MontarQuery("  @num_identificacao_unidade_carga, "));
                                stbSQL.Append(clsFacil.MontarQuery("  @num_lacre_unidade_carga, "));
                                stbSQL.Append(clsFacil.MontarQuery("  getdate() "));
                                stbSQL.Append(clsFacil.MontarQuery(") "));
                                strSQL_aux = stbSQL.ToString();

                                // Montando parametros
                                clsDados.LimparParametro();
                                clsFacil.AdicionarParametro(ref strSQL_aux, "20" + CTe.CteProc.protCTe.InfProt.chCTe.Substring(2, 4), "@dtr_referencia", SqlDbType.Int);
                                clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.protCTe.InfProt.chCTe, "@cod_chave_acesso", SqlDbType.VarChar);
                                clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infNF[i].infUnidTransp[j].idUnidTransp, "@num_identificacao_unidade_transporte", SqlDbType.VarChar);
                                clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infNF[i].infUnidTransp[j].infUnidCarga[k].idUnidCarga, "@num_identificacao_unidade_carga", SqlDbType.VarChar);
                                clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infNF[i].infUnidTransp[j].infUnidCarga[k].lacUnidCarga[l].nLacre, "@num_lacre_unidade_carga", SqlDbType.VarChar);

                                strSQL = strSQL + strSQL_aux;
                            }
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

        #endregion

        #region "infNFe"

        public string InserirXML_detalhe_xml_conhecimento_transporte_eletronico_nota_fiscal_eletronica(Common.SerializableClasses.CTe.ClsCTe_v2.proc CTe)
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
                for (Lcont = 0; Lcont <= CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infNFe.Count - 1; Lcont++)
                {
                    strSQL_aux = string.Empty;
                    stbSQL.Clear();
                    // Montando query a ser executada
                    stbSQL.Append(clsFacil.MontarQuery("INSERT INTO cte.detalhe_xml_conhecimento_transporte_eletronico_nota_fiscal_eletronica "));
                    stbSQL.Append(clsFacil.MontarQuery("( "));
                    stbSQL.Append(clsFacil.MontarQuery("  dtr_referencia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso, "));
                    stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso_nota_fiscal_eletronica, "));
                    stbSQL.Append(clsFacil.MontarQuery("  num_pin_suframa, "));
                    stbSQL.Append(clsFacil.MontarQuery("  dtc_previsao_entrega, "));
                    stbSQL.Append(clsFacil.MontarQuery("  dtc_atualizacao "));
                    stbSQL.Append(clsFacil.MontarQuery(") "));
                    stbSQL.Append(clsFacil.MontarQuery("VALUES "));
                    stbSQL.Append(clsFacil.MontarQuery("( "));
                    stbSQL.Append(clsFacil.MontarQuery("  @dtr_referencia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso_nota_fiscal_eletronica, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @num_pin_suframa, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @dtc_previsao_entrega, "));
                    stbSQL.Append(clsFacil.MontarQuery("  getdate() "));
                    stbSQL.Append(clsFacil.MontarQuery(") "));
                    strSQL_aux = stbSQL.ToString();

                    // Montando parametros
                    clsDados.LimparParametro();
                    clsFacil.AdicionarParametro(ref strSQL_aux, "20" + CTe.CteProc.protCTe.InfProt.chCTe.Substring(2, 4), "@dtr_referencia", SqlDbType.Int);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.protCTe.InfProt.chCTe, "@cod_chave_acesso", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infNFe[Lcont].chave, "@cod_chave_acesso_nota_fiscal_eletronica", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infNFe[Lcont].PIN, "@num_pin_suframa", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infNFe[Lcont].dPrev, "@dtc_previsao_entrega", SqlDbType.VarChar);

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

        public string InserirXML_detalhe_xml_conhecimento_transporte_eletronico_unidade_carga_infNFe(Common.SerializableClasses.CTe.ClsCTe_v2.proc CTe)
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
                for (i = 0; i <= CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infNFe.Count - 1; i++)
                {
                    int j = 0;
                    for (j = 0; j <= CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infNFe[i].infUnidCarga.Count - 1; j++)
                    {
                        strSQL_aux = string.Empty;
                        stbSQL.Clear();

                        // Montando query a ser executada
                        stbSQL.Append(clsFacil.MontarQuery("INSERT INTO cte.detalhe_xml_conhecimento_transporte_eletronico_unidade_carga "));
                        stbSQL.Append(clsFacil.MontarQuery("( "));
                        stbSQL.Append(clsFacil.MontarQuery("  dtr_referencia, "));
                        stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso, "));
                        stbSQL.Append(clsFacil.MontarQuery("  num_identificacao_unidade_carga, "));
                        stbSQL.Append(clsFacil.MontarQuery("  cod_tipo_unidade_carga_documento_fiscal_eletronico, "));
                        stbSQL.Append(clsFacil.MontarQuery("  qtd_rateada_unidade_carga, "));
                        stbSQL.Append(clsFacil.MontarQuery("  dtc_atualizacao "));
                        stbSQL.Append(clsFacil.MontarQuery(") "));
                        stbSQL.Append(clsFacil.MontarQuery("VALUES "));
                        stbSQL.Append(clsFacil.MontarQuery("( "));
                        stbSQL.Append(clsFacil.MontarQuery("  @dtr_referencia, "));
                        stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso, "));
                        stbSQL.Append(clsFacil.MontarQuery("  @num_identificacao_unidade_carga, "));
                        stbSQL.Append(clsFacil.MontarQuery("  @cod_tipo_unidade_carga_documento_fiscal_eletronico, "));
                        stbSQL.Append(clsFacil.MontarQuery("  @qtd_rateada_unidade_carga, "));
                        stbSQL.Append(clsFacil.MontarQuery("  getdate() "));
                        stbSQL.Append(clsFacil.MontarQuery(") "));
                        strSQL_aux = stbSQL.ToString();

                        // Montando parametros
                        clsDados.LimparParametro();
                        clsFacil.AdicionarParametro(ref strSQL_aux, "20" + CTe.CteProc.protCTe.InfProt.chCTe.Substring(2, 4), "@dtr_referencia", SqlDbType.Int);
                        clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.protCTe.InfProt.chCTe, "@cod_chave_acesso", SqlDbType.VarChar);
                        clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infNFe[i].infUnidCarga[j].idUnidCarga, "@num_identificacao_unidade_carga", SqlDbType.VarChar);
                        clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infNFe[i].infUnidCarga[j].tpUnidCarga, "@cod_tipo_unidade_carga_documento_fiscal_eletronico", SqlDbType.SmallInt);
                        clsFacil.AdicionarParametro(ref strSQL_aux, Convert.ToString(CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infNFe[i].infUnidCarga[j].qtdRat), "@qtd_rateada_unidade_carga", SqlDbType.Decimal);
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

        public string InserirXML_detalhe_xml_conhecimento_transporte_eletronico_unidade_carga_lacre_infNFe(Common.SerializableClasses.CTe.ClsCTe_v2.proc CTe)
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
                for (i = 0; i <= CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infNFe.Count - 1; i++)
                {
                    int j = 0;
                    for (j = 0; j <= CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infNFe[i].infUnidCarga.Count - 1; j++)
                    {
                        int k = 0;
                        for (k = 0; k <= CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infNFe[i].infUnidCarga[j].lacUnidCarga.Count - 1; k++)
                        {
                            strSQL_aux = string.Empty;
                            stbSQL.Clear();
                            // Montando query a ser executada
                            stbSQL.Append(clsFacil.MontarQuery("INSERT INTO cte.detalhe_xml_conhecimento_transporte_eletronico_unidade_carga_lacre "));
                            stbSQL.Append(clsFacil.MontarQuery("( "));
                            stbSQL.Append(clsFacil.MontarQuery("  dtr_referencia, "));
                            stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso, "));
                            stbSQL.Append(clsFacil.MontarQuery("  num_identificacao_unidade_carga, "));
                            stbSQL.Append(clsFacil.MontarQuery("  num_lacre_unidade_carga, "));
                            stbSQL.Append(clsFacil.MontarQuery("  dtc_atualizacao "));
                            stbSQL.Append(clsFacil.MontarQuery(") "));
                            stbSQL.Append(clsFacil.MontarQuery("VALUES "));
                            stbSQL.Append(clsFacil.MontarQuery("( "));
                            stbSQL.Append(clsFacil.MontarQuery("  @dtr_referencia, "));
                            stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso, "));
                            stbSQL.Append(clsFacil.MontarQuery("  @num_identificacao_unidade_carga, "));
                            stbSQL.Append(clsFacil.MontarQuery("  @num_lacre_unidade_carga, "));
                            stbSQL.Append(clsFacil.MontarQuery("  getdate() "));
                            stbSQL.Append(clsFacil.MontarQuery(") "));
                            strSQL_aux = stbSQL.ToString();

                            // Montando parametros
                            clsDados.LimparParametro();
                            clsFacil.AdicionarParametro(ref strSQL_aux, "20" + CTe.CteProc.protCTe.InfProt.chCTe.Substring(2, 4), "@dtr_referencia", SqlDbType.Int);
                            clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.protCTe.InfProt.chCTe, "@cod_chave_acesso", SqlDbType.VarChar);
                            clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infNFe[i].infUnidCarga[j].idUnidCarga, "@num_identificacao_unidade_carga", SqlDbType.VarChar);
                            clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infNFe[i].infUnidCarga[j].lacUnidCarga[k].nLacre, "@num_lacre_unidade_carga", SqlDbType.VarChar);
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

        public string InserirXML_detalhe_xml_conhecimento_transporte_eletronico_unidade_transporte_infNFe(Common.SerializableClasses.CTe.ClsCTe_v2.proc CTe)
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
                for (i = 0; i <= CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infNFe.Count - 1; i++)
                {
                    int j = 0;
                    for (j = 0; j <= CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infNFe[i].infUnidTransp.Count - 1; j++)
                    {
                        strSQL_aux = string.Empty;
                        stbSQL.Clear();
                        // Montando query a ser executada
                        stbSQL.Append(clsFacil.MontarQuery("INSERT INTO cte.detalhe_xml_conhecimento_transporte_eletronico_unidade_transporte "));
                        stbSQL.Append(clsFacil.MontarQuery("( "));
                        stbSQL.Append(clsFacil.MontarQuery("  dtr_referencia, "));
                        stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso, "));
                        stbSQL.Append(clsFacil.MontarQuery("  num_identificacao_unidade_transporte, "));
                        stbSQL.Append(clsFacil.MontarQuery("  cod_tipo_unidade_transporte_documento_fiscal_eletronico, "));
                        stbSQL.Append(clsFacil.MontarQuery("  qtd_rateada_unidade_carga, "));
                        stbSQL.Append(clsFacil.MontarQuery("  dtc_atualizacao "));
                        stbSQL.Append(clsFacil.MontarQuery(") "));
                        stbSQL.Append(clsFacil.MontarQuery("VALUES "));
                        stbSQL.Append(clsFacil.MontarQuery("( "));
                        stbSQL.Append(clsFacil.MontarQuery("  @dtr_referencia, "));
                        stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso, "));
                        stbSQL.Append(clsFacil.MontarQuery("  @num_identificacao_unidade_transporte, "));
                        stbSQL.Append(clsFacil.MontarQuery("  @cod_tipo_unidade_transporte_documento_fiscal_eletronico, "));
                        stbSQL.Append(clsFacil.MontarQuery("  @qtd_rateada_unidade_carga, "));
                        stbSQL.Append(clsFacil.MontarQuery("  getdate() "));
                        stbSQL.Append(clsFacil.MontarQuery(") "));
                        strSQL_aux = stbSQL.ToString();

                        // Montando parametros
                        clsDados.LimparParametro();
                        clsFacil.AdicionarParametro(ref strSQL_aux, "20" + CTe.CteProc.protCTe.InfProt.chCTe.Substring(2, 4), "@dtr_referencia", SqlDbType.Int);
                        clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.protCTe.InfProt.chCTe, "@cod_chave_acesso", SqlDbType.VarChar);
                        clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infNFe[i].infUnidTransp[j].idUnidTransp, "@num_identificacao_unidade_transporte", SqlDbType.VarChar);
                        clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infNFe[i].infUnidTransp[j].tpUnidTransp, "@cod_tipo_unidade_transporte_documento_fiscal_eletronico", SqlDbType.SmallInt);
                        clsFacil.AdicionarParametro(ref strSQL_aux, Convert.ToString(CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infNFe[i].infUnidTransp[j].qtdRat), "@qtd_rateada_unidade_carga", SqlDbType.Decimal);
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

        public string InserirXML_detalhe_xml_conhecimento_transporte_eletronico_unidade_transporte_lacre_infNFe(Common.SerializableClasses.CTe.ClsCTe_v2.proc CTe)
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
                for (i = 0; i <= CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infNFe.Count - 1; i++)
                {
                    int j = 0;
                    for (j = 0; j <= CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infNFe[i].infUnidTransp.Count - 1; j++)
                    {
                        int k = 0;
                        for (k = 0; k <= CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infNFe[i].infUnidTransp[j].lacUnidTransp.Count - 1; k++)
                        {
                            strSQL_aux = string.Empty;
                            stbSQL.Clear();

                            // Montando query a ser executada
                            stbSQL.Append(clsFacil.MontarQuery("INSERT INTO cte.detalhe_xml_conhecimento_transporte_eletronico_unidade_transporte_lacre "));
                            stbSQL.Append(clsFacil.MontarQuery("( "));
                            stbSQL.Append(clsFacil.MontarQuery("  dtr_referencia, "));
                            stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso, "));
                            stbSQL.Append(clsFacil.MontarQuery("  num_identificacao_unidade_transporte, "));
                            stbSQL.Append(clsFacil.MontarQuery("  num_lacre_unidade_transporte, "));
                            stbSQL.Append(clsFacil.MontarQuery("  dtc_atualizacao "));
                            stbSQL.Append(clsFacil.MontarQuery(") "));
                            stbSQL.Append(clsFacil.MontarQuery("VALUES "));
                            stbSQL.Append(clsFacil.MontarQuery("( "));
                            stbSQL.Append(clsFacil.MontarQuery("  @dtr_referencia, "));
                            stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso, "));
                            stbSQL.Append(clsFacil.MontarQuery("  @num_identificacao_unidade_transporte, "));
                            stbSQL.Append(clsFacil.MontarQuery("  @num_lacre_unidade_transporte, "));
                            stbSQL.Append(clsFacil.MontarQuery("  getdate() "));
                            stbSQL.Append(clsFacil.MontarQuery(") "));
                            strSQL_aux = stbSQL.ToString();

                            // Montando parametros
                            clsDados.LimparParametro();
                            clsFacil.AdicionarParametro(ref strSQL_aux, "20" + CTe.CteProc.protCTe.InfProt.chCTe.Substring(2, 4), "@dtr_referencia", SqlDbType.Int);
                            clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.protCTe.InfProt.chCTe, "@cod_chave_acesso", SqlDbType.VarChar);
                            clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infNFe[i].infUnidTransp[j].idUnidTransp, "@num_identificacao_unidade_transporte", SqlDbType.VarChar);
                            clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infNFe[i].infUnidTransp[j].lacUnidTransp[k].nLacre, "@num_lacre_unidade_transporte", SqlDbType.VarChar);
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

        public string InserirXML_detalhe_xml_conhecimento_transporte_eletronico_unidade_transporte_unidade_carga_infNFe(Common.SerializableClasses.CTe.ClsCTe_v2.proc CTe)
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
                for (i = 0; i <= CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infNFe.Count - 1; i++)
                {
                    int j = 0;
                    for (j = 0; j <= CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infNFe[i].infUnidTransp.Count - 1; j++)
                    {
                        int k = 0;
                        for (k = 0; k <= CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infNFe[i].infUnidTransp[j].infUnidCarga.Count - 1; k++)
                        {
                            strSQL_aux = string.Empty;
                            stbSQL.Clear();

                            // Montando query a ser executada
                            stbSQL.Append(clsFacil.MontarQuery("INSERT INTO cte.detalhe_xml_conhecimento_transporte_eletronico_unidade_transporte_unidade_carga "));
                            stbSQL.Append(clsFacil.MontarQuery("( "));
                            stbSQL.Append(clsFacil.MontarQuery("  dtr_referencia, "));
                            stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso, "));
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
                            stbSQL.Append(clsFacil.MontarQuery("  @num_identificacao_unidade_transporte, "));
                            stbSQL.Append(clsFacil.MontarQuery("  @num_identificacao_unidade_carga, "));
                            stbSQL.Append(clsFacil.MontarQuery("  @cod_tipo_unidade_carga_documento_fiscal_eletronico, "));
                            stbSQL.Append(clsFacil.MontarQuery("  @qtd_rateada_unidade_carga, "));
                            stbSQL.Append(clsFacil.MontarQuery("  getdate() "));
                            stbSQL.Append(clsFacil.MontarQuery(") "));
                            strSQL_aux = stbSQL.ToString();

                            // Montando parametros
                            clsDados.LimparParametro();
                            clsFacil.AdicionarParametro(ref strSQL_aux, "20" + CTe.CteProc.protCTe.InfProt.chCTe.Substring(2, 4), "@dtr_referencia", SqlDbType.Int);
                            clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.protCTe.InfProt.chCTe, "@cod_chave_acesso", SqlDbType.VarChar);
                            clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infNFe[i].infUnidTransp[j].idUnidTransp, "@num_identificacao_unidade_transporte", SqlDbType.VarChar);
                            clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infNFe[i].infUnidTransp[j].infUnidCarga[k].idUnidCarga, "@num_identificacao_unidade_carga", SqlDbType.VarChar);
                            clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infNFe[i].infUnidTransp[j].infUnidCarga[k].tpUnidCarga, "@cod_tipo_unidade_carga_documento_fiscal_eletronico", SqlDbType.SmallInt);
                            clsFacil.AdicionarParametro(ref strSQL_aux, Convert.ToString(CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infNFe[i].infUnidTransp[j].infUnidCarga[k].qtdRat), "@qtd_rateada_unidade_carga", SqlDbType.Decimal);

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

        public string InserirXML_detalhe_xml_conhecimento_transporte_eletronico_unidade_transporte_unidade_carga_lacre_infNFe(Common.SerializableClasses.CTe.ClsCTe_v2.proc CTe)
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
                for (i = 0; i <= CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infNFe.Count - 1; i++)
                {
                    int j = 0;
                    for (j = 0; j <= CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infNFe[i].infUnidTransp.Count - 1; j++)
                    {
                        int k = 0;
                        for (k = 0; k <= CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infNFe[i].infUnidTransp[j].infUnidCarga.Count - 1; k++)
                        {
                            int l = 0;
                            for (l = 0; l <= CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infNFe[i].infUnidTransp[j].infUnidCarga[k].lacUnidCarga.Count - 1; l++)
                            {
                                strSQL_aux = string.Empty;
                                stbSQL.Clear();

                                // Montando query a ser executada
                                stbSQL.Append(clsFacil.MontarQuery("INSERT INTO cte.detalhe_xml_conhecimento_transporte_eletronico_unidade_transporte_unidade_carga_lacre "));
                                stbSQL.Append(clsFacil.MontarQuery("( "));
                                stbSQL.Append(clsFacil.MontarQuery("  dtr_referencia, "));
                                stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso, "));
                                stbSQL.Append(clsFacil.MontarQuery("  num_identificacao_unidade_transporte, "));
                                stbSQL.Append(clsFacil.MontarQuery("  num_identificacao_unidade_carga, "));
                                stbSQL.Append(clsFacil.MontarQuery("  num_lacre_unidade_carga, "));
                                stbSQL.Append(clsFacil.MontarQuery("  dtc_atualizacao "));
                                stbSQL.Append(clsFacil.MontarQuery(") "));
                                stbSQL.Append(clsFacil.MontarQuery("VALUES "));
                                stbSQL.Append(clsFacil.MontarQuery("( "));
                                stbSQL.Append(clsFacil.MontarQuery("  @dtr_referencia, "));
                                stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso, "));
                                stbSQL.Append(clsFacil.MontarQuery("  @num_identificacao_unidade_transporte, "));
                                stbSQL.Append(clsFacil.MontarQuery("  @num_identificacao_unidade_carga, "));
                                stbSQL.Append(clsFacil.MontarQuery("  @num_lacre_unidade_carga, "));
                                stbSQL.Append(clsFacil.MontarQuery("  getdate() "));
                                stbSQL.Append(clsFacil.MontarQuery(") "));
                                strSQL_aux = stbSQL.ToString();

                                // Montando parametros
                                clsDados.LimparParametro();
                                clsFacil.AdicionarParametro(ref strSQL_aux, "20" + CTe.CteProc.protCTe.InfProt.chCTe.Substring(2, 4), "@dtr_referencia", SqlDbType.Int);
                                clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.protCTe.InfProt.chCTe, "@cod_chave_acesso", SqlDbType.VarChar);
                                clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infNFe[i].infUnidTransp[j].idUnidTransp, "@num_identificacao_unidade_transporte", SqlDbType.VarChar);
                                clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infNFe[i].infUnidTransp[j].infUnidCarga[k].idUnidCarga, "@num_identificacao_unidade_carga", SqlDbType.VarChar);
                                clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infNFe[i].infUnidTransp[j].infUnidCarga[k].lacUnidCarga[l].nLacre, "@num_lacre_unidade_carga", SqlDbType.VarChar);

                                strSQL = strSQL + strSQL_aux;
                            }
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

        #endregion

        #region infOutros"

        public string InserirXML_detalhe_xml_conhecimento_transporte_eletronico_outro_documento(Common.SerializableClasses.CTe.ClsCTe_v2.proc CTe)
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
                for (Lcont = 0; Lcont <= CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infNFe.Count - 1; Lcont++)
                {
                    strSQL_aux = string.Empty;
                    stbSQL.Clear();

                    // Montando query a ser executada
                    stbSQL.Append(clsFacil.MontarQuery("INSERT INTO cte.detalhe_xml_conhecimento_transporte_eletronico_outro_documento "));
                    stbSQL.Append(clsFacil.MontarQuery("( "));
                    stbSQL.Append(clsFacil.MontarQuery("  dtr_referencia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso, "));
                    stbSQL.Append(clsFacil.MontarQuery("  seq_detalhe_xml_conhecimento_transporte_eletronico_outro_documento, "));
                    stbSQL.Append(clsFacil.MontarQuery("  cod_tipo_documento_originario, "));
                    stbSQL.Append(clsFacil.MontarQuery("  des_documento, "));
                    stbSQL.Append(clsFacil.MontarQuery("  num_documento, "));
                    stbSQL.Append(clsFacil.MontarQuery("  dtc_emissao, "));
                    stbSQL.Append(clsFacil.MontarQuery("  val_documento, "));
                    stbSQL.Append(clsFacil.MontarQuery("  dtc_previsao_entrega, "));
                    stbSQL.Append(clsFacil.MontarQuery("  dtc_atualizacao "));
                    stbSQL.Append(clsFacil.MontarQuery(") "));
                    stbSQL.Append(clsFacil.MontarQuery("VALUES "));
                    stbSQL.Append(clsFacil.MontarQuery("( "));
                    stbSQL.Append(clsFacil.MontarQuery("  @dtr_referencia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @seq_detalhe_xml_conhecimento_transporte_eletronico_outro_documento, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @cod_tipo_documento_originario, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @des_documento, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @num_documento, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @dtc_emissao, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @val_documento, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @dtc_previsao_entrega, "));
                    stbSQL.Append(clsFacil.MontarQuery("  getdate() "));
                    stbSQL.Append(clsFacil.MontarQuery(") "));

                    strSQL_aux = stbSQL.ToString();

                    // Montando parametros
                    clsDados.LimparParametro();
                    clsFacil.AdicionarParametro(ref strSQL_aux, "20" + CTe.CteProc.protCTe.InfProt.chCTe.Substring(2, 4), "@dtr_referencia", SqlDbType.Int);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.protCTe.InfProt.chCTe, "@cod_chave_acesso", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, Convert.ToString(Lcont + 1), "@seq_detalhe_xml_conhecimento_transporte_eletronico_outro_documento", SqlDbType.SmallInt);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infOutros[Lcont].tpDoc, "@cod_tipo_documento_originario", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infOutros[Lcont].descOutros, "@des_documento", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infOutros[Lcont].nDoc, "@num_documento", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infOutros[Lcont].dEmi, "@dtc_emissao", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, Convert.ToString(CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infOutros[Lcont].vDocFisc), "@val_documento", SqlDbType.Decimal);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infOutros[Lcont].dPrev, "@dtc_previsao_entrega", SqlDbType.VarChar);

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

        public string InserirXML_detalhe_xml_conhecimento_transporte_eletronico_unidade_carga_infOutros(Common.SerializableClasses.CTe.ClsCTe_v2.proc CTe)
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
                for (i = 0; i <= CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infOutros.Count - 1; i++)
                {
                    int j = 0;
                    for (j = 0; j <= CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infOutros[i].infUnidCarga.Count - 1; j++)
                    {
                        strSQL_aux = string.Empty;
                        stbSQL.Clear();

                        // Montando query a ser executada
                        stbSQL.Append(clsFacil.MontarQuery("INSERT INTO cte.detalhe_xml_conhecimento_transporte_eletronico_unidade_carga "));
                        stbSQL.Append(clsFacil.MontarQuery("( "));
                        stbSQL.Append(clsFacil.MontarQuery("  dtr_referencia, "));
                        stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso, "));
                        stbSQL.Append(clsFacil.MontarQuery("  num_identificacao_unidade_carga, "));
                        stbSQL.Append(clsFacil.MontarQuery("  cod_tipo_unidade_carga_documento_fiscal_eletronico, "));
                        stbSQL.Append(clsFacil.MontarQuery("  qtd_rateada_unidade_carga, "));
                        stbSQL.Append(clsFacil.MontarQuery("  dtc_atualizacao "));
                        stbSQL.Append(clsFacil.MontarQuery(") "));
                        stbSQL.Append(clsFacil.MontarQuery("VALUES "));
                        stbSQL.Append(clsFacil.MontarQuery("( "));
                        stbSQL.Append(clsFacil.MontarQuery("  @dtr_referencia, "));
                        stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso, "));
                        stbSQL.Append(clsFacil.MontarQuery("  @num_identificacao_unidade_carga, "));
                        stbSQL.Append(clsFacil.MontarQuery("  @cod_tipo_unidade_carga_documento_fiscal_eletronico, "));
                        stbSQL.Append(clsFacil.MontarQuery("  @qtd_rateada_unidade_carga, "));
                        stbSQL.Append(clsFacil.MontarQuery("  getdate() "));
                        stbSQL.Append(clsFacil.MontarQuery(") "));
                        strSQL_aux = stbSQL.ToString();

                        // Montando parametros
                        clsDados.LimparParametro();
                        clsFacil.AdicionarParametro(ref strSQL_aux, "20" + CTe.CteProc.protCTe.InfProt.chCTe.Substring(2, 4), "@dtr_referencia", SqlDbType.Int);
                        clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.protCTe.InfProt.chCTe, "@cod_chave_acesso", SqlDbType.VarChar);
                        clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infOutros[i].infUnidCarga[j].idUnidCarga, "@num_identificacao_unidade_carga", SqlDbType.VarChar);
                        clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infOutros[i].infUnidCarga[j].tpUnidCarga, "@cod_tipo_unidade_carga_documento_fiscal_eletronico", SqlDbType.SmallInt);
                        clsFacil.AdicionarParametro(ref strSQL_aux, Convert.ToString(CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infOutros[i].infUnidCarga[j].qtdRat), "@qtd_rateada_unidade_carga", SqlDbType.Decimal);
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

        public string InserirXML_detalhe_xml_conhecimento_transporte_eletronico_unidade_carga_lacre_infOutros(Common.SerializableClasses.CTe.ClsCTe_v2.proc CTe)
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
                for (i = 0; i <= CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infOutros.Count - 1; i++)
                {
                    int j = 0;
                    for (j = 0; j <= CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infOutros[i].infUnidCarga.Count - 1; j++)
                    {
                        int k = 0;
                        for (k = 0; k <= CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infOutros[i].infUnidCarga[j].lacUnidCarga.Count - 1; k++)
                        {
                            strSQL_aux = string.Empty;
                            stbSQL.Clear();
                            // Montando query a ser executada
                            stbSQL.Append(clsFacil.MontarQuery("INSERT INTO cte.detalhe_xml_conhecimento_transporte_eletronico_unidade_carga_lacre "));
                            stbSQL.Append(clsFacil.MontarQuery("( "));
                            stbSQL.Append(clsFacil.MontarQuery("  dtr_referencia, "));
                            stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso, "));
                            stbSQL.Append(clsFacil.MontarQuery("  num_identificacao_unidade_carga, "));
                            stbSQL.Append(clsFacil.MontarQuery("  num_lacre_unidade_carga, "));
                            stbSQL.Append(clsFacil.MontarQuery("  dtc_atualizacao "));
                            stbSQL.Append(clsFacil.MontarQuery(") "));
                            stbSQL.Append(clsFacil.MontarQuery("VALUES "));
                            stbSQL.Append(clsFacil.MontarQuery("( "));
                            stbSQL.Append(clsFacil.MontarQuery("  @dtr_referencia, "));
                            stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso, "));
                            stbSQL.Append(clsFacil.MontarQuery("  @num_identificacao_unidade_carga, "));
                            stbSQL.Append(clsFacil.MontarQuery("  @num_lacre_unidade_carga, "));
                            stbSQL.Append(clsFacil.MontarQuery("  getdate() "));
                            stbSQL.Append(clsFacil.MontarQuery(") "));
                            strSQL_aux = stbSQL.ToString();

                            // Montando parametros
                            clsDados.LimparParametro();
                            clsFacil.AdicionarParametro(ref strSQL_aux, "20" + CTe.CteProc.protCTe.InfProt.chCTe.Substring(2, 4), "@dtr_referencia", SqlDbType.Int);
                            clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.protCTe.InfProt.chCTe, "@cod_chave_acesso", SqlDbType.VarChar);
                            clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infOutros[i].infUnidCarga[j].idUnidCarga, "@num_identificacao_unidade_carga", SqlDbType.VarChar);
                            clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infOutros[i].infUnidCarga[j].lacUnidCarga[k].nLacre, "@num_lacre_unidade_carga", SqlDbType.VarChar);
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

        public string InserirXML_detalhe_xml_conhecimento_transporte_eletronico_unidade_transporte_infOutros(Common.SerializableClasses.CTe.ClsCTe_v2.proc CTe)
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
                for (i = 0; i <= CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infOutros.Count - 1; i++)
                {
                    int j = 0;
                    for (j = 0; j <= CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infOutros[i].infUnidTransp.Count - 1; j++)
                    {
                        strSQL_aux = string.Empty;
                        stbSQL.Clear();
                        // Montando query a ser executada
                        stbSQL.Append(clsFacil.MontarQuery("INSERT INTO cte.detalhe_xml_conhecimento_transporte_eletronico_unidade_transporte "));
                        stbSQL.Append(clsFacil.MontarQuery("( "));
                        stbSQL.Append(clsFacil.MontarQuery("  dtr_referencia, "));
                        stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso, "));
                        stbSQL.Append(clsFacil.MontarQuery("  num_identificacao_unidade_transporte, "));
                        stbSQL.Append(clsFacil.MontarQuery("  cod_tipo_unidade_transporte_documento_fiscal_eletronico, "));
                        stbSQL.Append(clsFacil.MontarQuery("  qtd_rateada_unidade_carga, "));
                        stbSQL.Append(clsFacil.MontarQuery("  dtc_atualizacao "));
                        stbSQL.Append(clsFacil.MontarQuery(") "));
                        stbSQL.Append(clsFacil.MontarQuery("VALUES "));
                        stbSQL.Append(clsFacil.MontarQuery("( "));
                        stbSQL.Append(clsFacil.MontarQuery("  @dtr_referencia, "));
                        stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso, "));
                        stbSQL.Append(clsFacil.MontarQuery("  @num_identificacao_unidade_transporte, "));
                        stbSQL.Append(clsFacil.MontarQuery("  @cod_tipo_unidade_transporte_documento_fiscal_eletronico, "));
                        stbSQL.Append(clsFacil.MontarQuery("  @qtd_rateada_unidade_carga, "));
                        stbSQL.Append(clsFacil.MontarQuery("  getdate() "));
                        stbSQL.Append(clsFacil.MontarQuery(") "));
                        strSQL_aux = stbSQL.ToString();

                        // Montando parametros
                        clsDados.LimparParametro();
                        clsFacil.AdicionarParametro(ref strSQL_aux, "20" + CTe.CteProc.protCTe.InfProt.chCTe.Substring(2, 4), "@dtr_referencia", SqlDbType.Int);
                        clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.protCTe.InfProt.chCTe, "@cod_chave_acesso", SqlDbType.VarChar);
                        clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infOutros[i].infUnidTransp[j].idUnidTransp, "@num_identificacao_unidade_transporte", SqlDbType.VarChar);
                        clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infOutros[i].infUnidTransp[j].tpUnidTransp, "@cod_tipo_unidade_transporte_documento_fiscal_eletronico", SqlDbType.SmallInt);
                        clsFacil.AdicionarParametro(ref strSQL_aux, Convert.ToString(CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infOutros[i].infUnidTransp[j].qtdRat), "@qtd_rateada_unidade_carga", SqlDbType.Decimal);
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

        public string InserirXML_detalhe_xml_conhecimento_transporte_eletronico_unidade_transporte_lacre_infOutros(Common.SerializableClasses.CTe.ClsCTe_v2.proc CTe)
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
                for (i = 0; i <= CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infOutros.Count - 1; i++)
                {
                    int j = 0;
                    for (j = 0; j <= CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infOutros[i].infUnidTransp.Count - 1; j++)
                    {
                        int k = 0;
                        for (k = 0; k <= CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infOutros[i].infUnidTransp[j].lacUnidTransp.Count - 1; k++)
                        {
                            strSQL_aux = string.Empty;
                            stbSQL.Clear();

                            // Montando query a ser executada
                            stbSQL.Append(clsFacil.MontarQuery("INSERT INTO cte.detalhe_xml_conhecimento_transporte_eletronico_unidade_transporte_lacre "));
                            stbSQL.Append(clsFacil.MontarQuery("( "));
                            stbSQL.Append(clsFacil.MontarQuery("  dtr_referencia, "));
                            stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso, "));
                            stbSQL.Append(clsFacil.MontarQuery("  num_identificacao_unidade_transporte, "));
                            stbSQL.Append(clsFacil.MontarQuery("  num_lacre_unidade_transporte, "));
                            stbSQL.Append(clsFacil.MontarQuery("  dtc_atualizacao "));
                            stbSQL.Append(clsFacil.MontarQuery(") "));
                            stbSQL.Append(clsFacil.MontarQuery("VALUES "));
                            stbSQL.Append(clsFacil.MontarQuery("( "));
                            stbSQL.Append(clsFacil.MontarQuery("  @dtr_referencia, "));
                            stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso, "));
                            stbSQL.Append(clsFacil.MontarQuery("  @num_identificacao_unidade_transporte, "));
                            stbSQL.Append(clsFacil.MontarQuery("  @num_lacre_unidade_transporte, "));
                            stbSQL.Append(clsFacil.MontarQuery("  getdate() "));
                            stbSQL.Append(clsFacil.MontarQuery(") "));
                            strSQL_aux = stbSQL.ToString();

                            // Montando parametros
                            clsDados.LimparParametro();
                            clsFacil.AdicionarParametro(ref strSQL_aux, "20" + CTe.CteProc.protCTe.InfProt.chCTe.Substring(2, 4), "@dtr_referencia", SqlDbType.Int);
                            clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.protCTe.InfProt.chCTe, "@cod_chave_acesso", SqlDbType.VarChar);
                            clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infOutros[i].infUnidTransp[j].idUnidTransp, "@num_identificacao_unidade_transporte", SqlDbType.VarChar);
                            clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infOutros[i].infUnidTransp[j].lacUnidTransp[k].nLacre, "@num_lacre_unidade_transporte", SqlDbType.VarChar);
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

        public string InserirXML_detalhe_xml_conhecimento_transporte_eletronico_unidade_transporte_unidade_carga_infOutros(Common.SerializableClasses.CTe.ClsCTe_v2.proc CTe)
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
                for (i = 0; i <= CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infOutros.Count - 1; i++)
                {
                    int j = 0;
                    for (j = 0; j <= CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infOutros[i].infUnidTransp.Count - 1; j++)
                    {
                        int k = 0;
                        for (k = 0; k <= CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infOutros[i].infUnidTransp[j].infUnidCarga.Count - 1; k++)
                        {
                            strSQL_aux = string.Empty;
                            stbSQL.Clear();

                            // Montando query a ser executada
                            stbSQL.Append(clsFacil.MontarQuery("INSERT INTO cte.detalhe_xml_conhecimento_transporte_eletronico_unidade_transporte_unidade_carga "));
                            stbSQL.Append(clsFacil.MontarQuery("( "));
                            stbSQL.Append(clsFacil.MontarQuery("  dtr_referencia, "));
                            stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso, "));
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
                            stbSQL.Append(clsFacil.MontarQuery("  @num_identificacao_unidade_transporte, "));
                            stbSQL.Append(clsFacil.MontarQuery("  @num_identificacao_unidade_carga, "));
                            stbSQL.Append(clsFacil.MontarQuery("  @cod_tipo_unidade_carga_documento_fiscal_eletronico, "));
                            stbSQL.Append(clsFacil.MontarQuery("  @qtd_rateada_unidade_carga, "));
                            stbSQL.Append(clsFacil.MontarQuery("  getdate() "));
                            stbSQL.Append(clsFacil.MontarQuery(") "));
                            strSQL_aux = stbSQL.ToString();

                            // Montando parametros
                            clsDados.LimparParametro();
                            clsFacil.AdicionarParametro(ref strSQL_aux, "20" + CTe.CteProc.protCTe.InfProt.chCTe.Substring(2, 4), "@dtr_referencia", SqlDbType.Int);
                            clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.protCTe.InfProt.chCTe, "@cod_chave_acesso", SqlDbType.VarChar);
                            clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infOutros[i].infUnidTransp[j].idUnidTransp, "@num_identificacao_unidade_transporte", SqlDbType.VarChar);
                            clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infOutros[i].infUnidTransp[j].infUnidCarga[k].idUnidCarga, "@num_identificacao_unidade_carga", SqlDbType.VarChar);
                            clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infOutros[i].infUnidTransp[j].infUnidCarga[k].tpUnidCarga, "@cod_tipo_unidade_carga_documento_fiscal_eletronico", SqlDbType.SmallInt);
                            clsFacil.AdicionarParametro(ref strSQL_aux, Convert.ToString(CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infOutros[i].infUnidTransp[j].infUnidCarga[k].qtdRat), "@qtd_rateada_unidade_carga", SqlDbType.Decimal);

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

        public string InserirXML_detalhe_xml_conhecimento_transporte_eletronico_unidade_transporte_unidade_carga_lacre_infOutros(Common.SerializableClasses.CTe.ClsCTe_v2.proc CTe)
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
                for (i = 0; i <= CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infOutros.Count - 1; i++)
                {
                    int j = 0;
                    for (j = 0; j <= CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infOutros[i].infUnidTransp.Count - 1; j++)
                    {
                        int k = 0;
                        for (k = 0; k <= CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infOutros[i].infUnidTransp[j].infUnidCarga.Count - 1; k++)
                        {
                            int l = 0;
                            for (l = 0; l <= CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infOutros[i].infUnidTransp[j].infUnidCarga[k].lacUnidCarga.Count - 1; l++)
                            {
                                strSQL_aux = string.Empty;
                                stbSQL.Clear();

                                // Montando query a ser executada
                                stbSQL.Append(clsFacil.MontarQuery("INSERT INTO cte.detalhe_xml_conhecimento_transporte_eletronico_unidade_transporte_unidade_carga_lacre "));
                                stbSQL.Append(clsFacil.MontarQuery("( "));
                                stbSQL.Append(clsFacil.MontarQuery("  dtr_referencia, "));
                                stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso, "));
                                stbSQL.Append(clsFacil.MontarQuery("  num_identificacao_unidade_transporte, "));
                                stbSQL.Append(clsFacil.MontarQuery("  num_identificacao_unidade_carga, "));
                                stbSQL.Append(clsFacil.MontarQuery("  num_lacre_unidade_carga, "));
                                stbSQL.Append(clsFacil.MontarQuery("  dtc_atualizacao "));
                                stbSQL.Append(clsFacil.MontarQuery(") "));
                                stbSQL.Append(clsFacil.MontarQuery("VALUES "));
                                stbSQL.Append(clsFacil.MontarQuery("( "));
                                stbSQL.Append(clsFacil.MontarQuery("  @dtr_referencia, "));
                                stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso, "));
                                stbSQL.Append(clsFacil.MontarQuery("  @num_identificacao_unidade_transporte, "));
                                stbSQL.Append(clsFacil.MontarQuery("  @num_identificacao_unidade_carga, "));
                                stbSQL.Append(clsFacil.MontarQuery("  @num_lacre_unidade_carga, "));
                                stbSQL.Append(clsFacil.MontarQuery("  getdate() "));
                                stbSQL.Append(clsFacil.MontarQuery(") "));
                                strSQL_aux = stbSQL.ToString();

                                // Montando parametros
                                clsDados.LimparParametro();
                                clsFacil.AdicionarParametro(ref strSQL_aux, "20" + CTe.CteProc.protCTe.InfProt.chCTe.Substring(2, 4), "@dtr_referencia", SqlDbType.Int);
                                clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.protCTe.InfProt.chCTe, "@cod_chave_acesso", SqlDbType.VarChar);
                                clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infOutros[i].infUnidTransp[j].idUnidTransp, "@num_identificacao_unidade_transporte", SqlDbType.VarChar);
                                clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infOutros[i].infUnidTransp[j].infUnidCarga[k].idUnidCarga, "@num_identificacao_unidade_carga", SqlDbType.VarChar);
                                clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.infDoc.infOutros[i].infUnidTransp[j].infUnidCarga[k].lacUnidCarga[l].nLacre, "@num_lacre_unidade_carga", SqlDbType.VarChar);

                                strSQL = strSQL + strSQL_aux;
                            }
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

        #endregion

        public string InserirXML_detalhe_xml_conhecimento_transporte_eletronico_transporte_emissor_anterior(Common.SerializableClasses.CTe.ClsCTe_v2.proc CTe)
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
                for (Lcont = 0; Lcont <= CTe.CteProc.CTe.infCte.infCTeNorm.docAnt.emiDocAnt.Count - 1; Lcont++)
                {
                    strSQL_aux = string.Empty;
                    stbSQL.Clear();
                    stbSQL.Append(clsFacil.MontarQuery("INSERT INTO cte.detalhe_xml_conhecimento_transporte_eletronico_transporte_emissor_anterior "));
                    stbSQL.Append(clsFacil.MontarQuery("( "));
                    stbSQL.Append(clsFacil.MontarQuery("  dtr_referencia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso, "));
                    stbSQL.Append(clsFacil.MontarQuery("  seq_detalhe_xml_conhecimento_transporte_eletronico_transporte_emissor_anterior, "));
                    stbSQL.Append(clsFacil.MontarQuery("  num_cnpj_cpf_base_emissor_anterior, "));
                    stbSQL.Append(clsFacil.MontarQuery("  num_cnpj_cpf_filial_emissor_anterior, "));
                    stbSQL.Append(clsFacil.MontarQuery("  dig_cnpj_cpf_emissor_anterior, "));
                    stbSQL.Append(clsFacil.MontarQuery("  num_insc_estad_emissor_anterior, "));
                    stbSQL.Append(clsFacil.MontarQuery("  sig_unid_federacao_emissor_anterior, "));
                    stbSQL.Append(clsFacil.MontarQuery("  nom_razao_social_emissor_anterior, "));
                    stbSQL.Append(clsFacil.MontarQuery("  dtc_atualizacao "));
                    stbSQL.Append(clsFacil.MontarQuery(") "));
                    stbSQL.Append(clsFacil.MontarQuery("VALUES "));
                    stbSQL.Append(clsFacil.MontarQuery("( "));
                    stbSQL.Append(clsFacil.MontarQuery("  @dtr_referencia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @seq_detalhe_xml_conhecimento_transporte_eletronico_transporte_emissor_anterior, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @num_cnpj_cpf_base_emissor_anterior, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @num_cnpj_cpf_filial_emissor_anterior, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @dig_cnpj_cpf_emissor_anterior, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @num_insc_estad_emissor_anterior, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @sig_unid_federacao_emissor_anterior, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @nom_razao_social_emissor_anterior, "));
                    stbSQL.Append(clsFacil.MontarQuery("  getdate() "));
                    stbSQL.Append(clsFacil.MontarQuery(") "));
                    strSQL_aux = stbSQL.ToString();

                    // Montando parametros
                    clsDados.LimparParametro();
                    clsFacil.AdicionarParametro(ref strSQL_aux, "20" + CTe.CteProc.protCTe.InfProt.chCTe.Substring(2, 4), "@dtr_referencia", SqlDbType.Int);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.protCTe.InfProt.chCTe, "@cod_chave_acesso", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, Convert.ToString(Lcont + 1), "@seq_detalhe_xml_conhecimento_transporte_eletronico_transporte_emissor_anterior", SqlDbType.SmallInt);

                    if (CTe.CteProc.CTe.infCte.infCTeNorm.docAnt.emiDocAnt[Lcont].CNPJ != null && CTe.CteProc.CTe.infCte.infCTeNorm.docAnt.emiDocAnt[Lcont].CNPJ != string.Empty)
                    {
                        clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.docAnt.emiDocAnt[Lcont].CNPJ.Substring(0, 8), "@num_cnpj_cpf_base_emissor_anterior", SqlDbType.VarChar);
                        clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.docAnt.emiDocAnt[Lcont].CNPJ.Substring(8, 4), "@num_cnpj_cpf_filial_emissor_anterior", SqlDbType.VarChar);
                        clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.docAnt.emiDocAnt[Lcont].CNPJ.Substring(12, 2), "@dig_cnpj_cpf_emissor_anterior", SqlDbType.TinyInt);
                    }
                    if (CTe.CteProc.CTe.infCte.infCTeNorm.docAnt.emiDocAnt[Lcont].CPF != null && CTe.CteProc.CTe.infCte.infCTeNorm.docAnt.emiDocAnt[Lcont].CPF != string.Empty)
                    {
                        clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.docAnt.emiDocAnt[Lcont].CPF.Substring(0, 9), "@num_cnpj_cpf_base_emissor_anterior", SqlDbType.VarChar);
                        clsFacil.AdicionarParametro(ref strSQL_aux, "0", "@num_cnpj_cpf_filial_emissor_anterior", SqlDbType.VarChar);
                        clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.docAnt.emiDocAnt[Lcont].CPF.Substring(9, 2), "@dig_cnpj_cpf_emissor_anterior", SqlDbType.TinyInt);
                    }
                    if ((CTe.CteProc.CTe.infCte.infCTeNorm.docAnt.emiDocAnt[Lcont].CNPJ == null && CTe.CteProc.CTe.infCte.infCTeNorm.docAnt.emiDocAnt[Lcont].CPF == null) || (CTe.CteProc.CTe.infCte.infCTeNorm.docAnt.emiDocAnt[Lcont].CNPJ == null && CTe.CteProc.CTe.infCte.infCTeNorm.docAnt.emiDocAnt[Lcont].CPF == string.Empty) || (CTe.CteProc.CTe.infCte.infCTeNorm.docAnt.emiDocAnt[Lcont].CNPJ == string.Empty && CTe.CteProc.CTe.infCte.infCTeNorm.docAnt.emiDocAnt[Lcont].CPF == null))
                    {
                        clsFacil.AdicionarParametro(ref strSQL_aux, string.Empty, "@num_cnpj_cpf_base_emissor_anterior", SqlDbType.VarChar);
                        clsFacil.AdicionarParametro(ref strSQL_aux, string.Empty, "@num_cnpj_cpf_filial_emissor_anterior", SqlDbType.VarChar);
                        clsFacil.AdicionarParametro(ref strSQL_aux, string.Empty, "@dig_cnpj_cpf_emissor_anterior", SqlDbType.TinyInt);
                    }

                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.docAnt.emiDocAnt[Lcont].IE, "@num_insc_estad_emissor_anterior", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.docAnt.emiDocAnt[Lcont].UF, "@sig_unid_federacao_emissor_anterior", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.docAnt.emiDocAnt[Lcont].xNome, "@nom_razao_social_emissor_anterior", SqlDbType.VarChar);
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

        public string InserirXML_detalhe_xml_conhecimento_transporte_eletronico_transporte_anterior_papel(Common.SerializableClasses.CTe.ClsCTe_v2.proc CTe)
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
                for (i = 0; i <= CTe.CteProc.CTe.infCte.infCTeNorm.docAnt.emiDocAnt.Count - 1; i++)
                {
                    int j = 0;
                    for (j = 0; j <= CTe.CteProc.CTe.infCte.infCTeNorm.docAnt.emiDocAnt[i].idDocAnt.Count - 1; j++)
                    {
                        int k = 0;
                        for (k = 0; k <= CTe.CteProc.CTe.infCte.infCTeNorm.docAnt.emiDocAnt[i].idDocAnt[j].idDocAntPap.Count - 1; k++)
                        {
                            strSQL_aux = string.Empty;
                            stbSQL.Clear();
                            stbSQL.Append(clsFacil.MontarQuery("INSERT INTO cte.detalhe_xml_conhecimento_transporte_eletronico_transporte_anterior_papel "));
                            stbSQL.Append(clsFacil.MontarQuery("( "));
                            stbSQL.Append(clsFacil.MontarQuery("  dtr_referencia, "));
                            stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso, "));
                            stbSQL.Append(clsFacil.MontarQuery("  seq_detalhe_xml_conhecimento_transporte_eletronico_transporte_emissor_anterior, "));
                            stbSQL.Append(clsFacil.MontarQuery("  seq_informacao_identificacao_documento_transporte_anterior, "));
                            stbSQL.Append(clsFacil.MontarQuery("  seq_detalhe_xml_conhecimento_transporte_eletronico_transporte_anterior_papel, "));
                            stbSQL.Append(clsFacil.MontarQuery("  num_serie_documento_transporte_anterior_papel, "));
                            stbSQL.Append(clsFacil.MontarQuery("  cod_tipo_documento_transporte_anterior_papel, "));
                            stbSQL.Append(clsFacil.MontarQuery("  num_subserie_documento_transporte_anterior_papel, "));
                            stbSQL.Append(clsFacil.MontarQuery("  num_documento_fiscal_transporte_anterior_papel, "));
                            stbSQL.Append(clsFacil.MontarQuery("  dtc_emissao_documento_transporte_anterior_papel, "));
                            stbSQL.Append(clsFacil.MontarQuery("  dtc_atualizacao "));
                            stbSQL.Append(clsFacil.MontarQuery(") "));
                            stbSQL.Append(clsFacil.MontarQuery("VALUES "));
                            stbSQL.Append(clsFacil.MontarQuery("( "));
                            stbSQL.Append(clsFacil.MontarQuery("  @dtr_referencia, "));
                            stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso, "));
                            stbSQL.Append(clsFacil.MontarQuery("  @seq_detalhe_xml_conhecimento_transporte_eletronico_transporte_emissor_anterior, "));
                            stbSQL.Append(clsFacil.MontarQuery("  @seq_informacao_identificacao_documento_transporte_anterior, "));
                            stbSQL.Append(clsFacil.MontarQuery("  @seq_detalhe_xml_conhecimento_transporte_eletronico_transporte_anterior_papel, "));
                            stbSQL.Append(clsFacil.MontarQuery("  @num_serie_documento_transporte_anterior_papel, "));
                            stbSQL.Append(clsFacil.MontarQuery("  @cod_tipo_documento_transporte_anterior_papel, "));
                            stbSQL.Append(clsFacil.MontarQuery("  @num_subserie_documento_transporte_anterior_papel, "));
                            stbSQL.Append(clsFacil.MontarQuery("  @num_documento_fiscal_transporte_anterior_papel, "));
                            stbSQL.Append(clsFacil.MontarQuery("  @dtc_emissao_documento_transporte_anterior_papel, "));
                            stbSQL.Append(clsFacil.MontarQuery("  getdate() "));
                            stbSQL.Append(clsFacil.MontarQuery(") "));

                            strSQL_aux = stbSQL.ToString();

                            // Montando parametros
                            clsDados.LimparParametro();
                            clsFacil.AdicionarParametro(ref strSQL_aux, "20" + CTe.CteProc.protCTe.InfProt.chCTe.Substring(2, 4), "@dtr_referencia", SqlDbType.Int);
                            clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.protCTe.InfProt.chCTe, "@cod_chave_acesso", SqlDbType.VarChar);

                            clsFacil.AdicionarParametro(ref strSQL_aux, Convert.ToString(i + 1), "@seq_detalhe_xml_conhecimento_transporte_eletronico_transporte_emissor_anterior", SqlDbType.SmallInt);
                            clsFacil.AdicionarParametro(ref strSQL_aux, Convert.ToString(j + 1), "@seq_informacao_identificacao_documento_transporte_anterior", SqlDbType.SmallInt);
                            clsFacil.AdicionarParametro(ref strSQL_aux, Convert.ToString(k + 1), "@seq_detalhe_xml_conhecimento_transporte_eletronico_transporte_anterior_papel", SqlDbType.SmallInt);
                            clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.docAnt.emiDocAnt[i].idDocAnt[j].idDocAntPap[k].serie, "@num_serie_documento_transporte_anterior_papel", SqlDbType.VarChar);
                            clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.docAnt.emiDocAnt[i].idDocAnt[j].idDocAntPap[k].tpDoc, "@cod_tipo_documento_transporte_anterior_papel", SqlDbType.SmallInt);
                            clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.docAnt.emiDocAnt[i].idDocAnt[j].idDocAntPap[k].subser, "@num_subserie_documento_transporte_anterior_papel", SqlDbType.VarChar);
                            clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.docAnt.emiDocAnt[i].idDocAnt[j].idDocAntPap[k].nDoc, "@num_documento_fiscal_transporte_anterior_papel", SqlDbType.VarChar);
                            clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.docAnt.emiDocAnt[i].idDocAnt[j].idDocAntPap[k].dEmi, "@dtc_emissao_documento_transporte_anterior_papel", SqlDbType.VarChar);
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

        public string InserirXML_detalhe_xml_conhecimento_transporte_eletronico_transporte_anterior_eletronico(Common.SerializableClasses.CTe.ClsCTe_v2.proc CTe)
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
                for (i = 0; i <= CTe.CteProc.CTe.infCte.infCTeNorm.docAnt.emiDocAnt.Count - 1; i++)
                {
                    int j = 0;
                    for (j = 0; j <= CTe.CteProc.CTe.infCte.infCTeNorm.docAnt.emiDocAnt[i].idDocAnt.Count - 1; j++)
                    {
                        int k = 0;
                        for (k = 0; k <= CTe.CteProc.CTe.infCte.infCTeNorm.docAnt.emiDocAnt[i].idDocAnt[j].idDocAntEle.Count - 1; k++)
                        {
                            strSQL_aux = string.Empty;
                            stbSQL.Clear();
                            // Montando query a ser executada                            
                            stbSQL.Append(clsFacil.MontarQuery("INSERT INTO cte.detalhe_xml_conhecimento_transporte_eletronico_transporte_anterior_eletronico "));
                            stbSQL.Append(clsFacil.MontarQuery("( "));
                            stbSQL.Append(clsFacil.MontarQuery("  dtr_referencia, "));
                            stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso, "));
                            stbSQL.Append(clsFacil.MontarQuery("  seq_detalhe_xml_conhecimento_transporte_eletronico_transporte_anterior_eletronico, "));
                            stbSQL.Append(clsFacil.MontarQuery("  seq_informacao_identificacao_documento_transporte_anterior, "));
                            stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso_documento_transporte_anterior, "));
                            stbSQL.Append(clsFacil.MontarQuery("  dtc_atualizacao "));
                            stbSQL.Append(clsFacil.MontarQuery(") "));
                            stbSQL.Append(clsFacil.MontarQuery("VALUES "));
                            stbSQL.Append(clsFacil.MontarQuery("( "));
                            stbSQL.Append(clsFacil.MontarQuery("  @dtr_referencia, "));
                            stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso, "));
                            stbSQL.Append(clsFacil.MontarQuery("  @seq_detalhe_xml_conhecimento_transporte_eletronico_transporte_anterior_eletronico, "));
                            stbSQL.Append(clsFacil.MontarQuery("  @seq_informacao_identificacao_documento_transporte_anterior, "));
                            stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso_documento_transporte_anterior, "));
                            stbSQL.Append(clsFacil.MontarQuery("  getdate() "));
                            stbSQL.Append(clsFacil.MontarQuery(") "));
                            strSQL_aux = stbSQL.ToString();

                            // Montando parametros
                            clsDados.LimparParametro();
                            clsFacil.AdicionarParametro(ref strSQL_aux, "20" + CTe.CteProc.protCTe.InfProt.chCTe.Substring(2, 4), "@dtr_referencia", SqlDbType.Int);
                            clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.protCTe.InfProt.chCTe, "@cod_chave_acesso", SqlDbType.VarChar);
                            clsFacil.AdicionarParametro(ref strSQL_aux, Convert.ToString(i + 1), "@seq_detalhe_xml_conhecimento_transporte_eletronico_transporte_anterior_eletronico", SqlDbType.SmallInt);
                            clsFacil.AdicionarParametro(ref strSQL_aux, Convert.ToString(j + 1), "@seq_informacao_identificacao_documento_transporte_anterior", SqlDbType.SmallInt);
                            clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.docAnt.emiDocAnt[i].idDocAnt[j].idDocAntEle[k].chave, "@cod_chave_acesso_documento_transporte_anterior", SqlDbType.VarChar);
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

        public string InserirXML_detalhe_xml_conhecimento_transporte_eletronico_veiculo(Common.SerializableClasses.CTe.ClsCTe_v2.proc CTe)
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
                for (Lcont = 0; Lcont <= CTe.CteProc.CTe.infCte.infCTeNorm.veicNovos.Count - 1; Lcont++)
                {
                    strSQL_aux = string.Empty;
                    stbSQL.Clear();
                    // Montando query a ser executada
                    stbSQL.Append(clsFacil.MontarQuery("INSERT INTO cte.detalhe_xml_conhecimento_transporte_eletronico_veiculo "));
                    stbSQL.Append(clsFacil.MontarQuery("( "));
                    stbSQL.Append(clsFacil.MontarQuery("  dtr_referencia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso, "));
                    stbSQL.Append(clsFacil.MontarQuery("  num_chassi_veiculo_novo, "));
                    stbSQL.Append(clsFacil.MontarQuery("  cod_cor_veiculo_novo, "));
                    stbSQL.Append(clsFacil.MontarQuery("  des_cor_veiculo_novo, "));
                    stbSQL.Append(clsFacil.MontarQuery("  cod_modelo_veiculo_novo, "));
                    stbSQL.Append(clsFacil.MontarQuery("  val_veiculo_novo, "));
                    stbSQL.Append(clsFacil.MontarQuery("  val_frete_veiculo_novo, "));
                    stbSQL.Append(clsFacil.MontarQuery("  dtc_atualizacao "));
                    stbSQL.Append(clsFacil.MontarQuery(") "));
                    stbSQL.Append(clsFacil.MontarQuery("VALUES "));
                    stbSQL.Append(clsFacil.MontarQuery("( "));
                    stbSQL.Append(clsFacil.MontarQuery("  @dtr_referencia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @num_chassi_veiculo_novo, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @cod_cor_veiculo_novo, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @des_cor_veiculo_novo, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @cod_modelo_veiculo_novo, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @val_veiculo_novo, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @val_frete_veiculo_novo, "));
                    stbSQL.Append(clsFacil.MontarQuery("  getdate() "));
                    stbSQL.Append(clsFacil.MontarQuery(") "));
                    strSQL_aux = stbSQL.ToString();

                    // Montando parametros
                    clsDados.LimparParametro();
                    clsFacil.AdicionarParametro(ref strSQL_aux, "20" + CTe.CteProc.protCTe.InfProt.chCTe.Substring(2, 4), "@dtr_referencia", SqlDbType.Int);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.protCTe.InfProt.chCTe, "@cod_chave_acesso", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.veicNovos[Lcont].chassi, "@num_chassi_veiculo_novo", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.veicNovos[Lcont].cCor, "@cod_cor_veiculo_novo", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.veicNovos[Lcont].xCor, "@des_cor_veiculo_novo", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.veicNovos[Lcont].cMod, "@cod_modelo_veiculo_novo", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, Convert.ToString(CTe.CteProc.CTe.infCte.infCTeNorm.veicNovos[Lcont].vUnit), "@val_veiculo_novo", SqlDbType.Decimal);
                    clsFacil.AdicionarParametro(ref strSQL_aux, Convert.ToString(CTe.CteProc.CTe.infCte.infCTeNorm.veicNovos[Lcont].vFrete), "@val_frete_veiculo_novo", SqlDbType.Decimal);
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

        public string InserirXML_detalhe_xml_conhecimento_transporte_eletronico_cobranca_duplicata(Common.SerializableClasses.CTe.ClsCTe_v2.proc CTe)
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
                for (Lcont = 0; Lcont <= CTe.CteProc.CTe.infCte.infCTeNorm.cobr.dup.Count - 1; Lcont++)
                {
                    strSQL_aux = string.Empty;
                    stbSQL.Clear();
                    // Montando query a ser executada
                    stbSQL.Append(clsFacil.MontarQuery("INSERT INTO cte.detalhe_xml_conhecimento_transporte_eletronico_cobranca_duplicata "));
                    stbSQL.Append(clsFacil.MontarQuery("( "));
                    stbSQL.Append(clsFacil.MontarQuery("  dtr_referencia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso, "));
                    stbSQL.Append(clsFacil.MontarQuery("  seq_detalhe_xml_conhecimento_transporte_eletronico_cobranca_duplicata, "));
                    stbSQL.Append(clsFacil.MontarQuery("  num_duplicata, "));
                    stbSQL.Append(clsFacil.MontarQuery("  dtc_vencimento_duplicata, "));
                    stbSQL.Append(clsFacil.MontarQuery("  val_total_duplicata, "));
                    stbSQL.Append(clsFacil.MontarQuery("  dtc_atualizacao "));
                    stbSQL.Append(clsFacil.MontarQuery(") "));
                    stbSQL.Append(clsFacil.MontarQuery("VALUES "));
                    stbSQL.Append(clsFacil.MontarQuery("( "));
                    stbSQL.Append(clsFacil.MontarQuery("  @dtr_referencia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @seq_detalhe_xml_conhecimento_transporte_eletronico_cobranca_duplicata, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @num_duplicata, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @dtc_vencimento_duplicata, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @val_total_duplicata, "));
                    stbSQL.Append(clsFacil.MontarQuery("  getdate() "));
                    stbSQL.Append(clsFacil.MontarQuery(") "));
                    strSQL_aux = stbSQL.ToString();

                    // Montando parametros
                    clsDados.LimparParametro();
                    clsFacil.AdicionarParametro(ref strSQL_aux, "20" + CTe.CteProc.protCTe.InfProt.chCTe.Substring(2, 4), "@dtr_referencia", SqlDbType.Int);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.protCTe.InfProt.chCTe, "@cod_chave_acesso", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, Convert.ToString(Lcont + 1), "@seq_detalhe_xml_conhecimento_transporte_eletronico_cobranca_duplicata", SqlDbType.Int);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.cobr.dup[Lcont].nDup, "@num_duplicata", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.cobr.dup[Lcont].dVenc, "@dtc_vencimento_duplicata", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, Convert.ToString(CTe.CteProc.CTe.infCte.infCTeNorm.cobr.dup[Lcont].vDup), "@val_total_duplicata", SqlDbType.Decimal);

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

        public string InserirXML_detalhe_xml_conhecimento_transporte_eletronico_complementado(Common.SerializableClasses.CTe.ClsCTe_v2.proc CTe)
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
                if (CTe.CteProc.CTe.infCte.infCteComp.chave != string.Empty && CTe.CteProc.CTe.infCte.infCteComp.chave != null)
                {
                    strSQL_aux = string.Empty;
                    stbSQL.Clear();
                    // Montando query a ser executada
                    stbSQL.Append(clsFacil.MontarQuery("INSERT INTO cte.detalhe_xml_conhecimento_transporte_eletronico_complementado "));
                    stbSQL.Append(clsFacil.MontarQuery("( "));
                    stbSQL.Append(clsFacil.MontarQuery("  dtr_referencia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso, "));
                    stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso_complementado, "));
                    stbSQL.Append(clsFacil.MontarQuery("  dtc_atualizacao "));
                    stbSQL.Append(clsFacil.MontarQuery(") "));
                    stbSQL.Append(clsFacil.MontarQuery("VALUES "));
                    stbSQL.Append(clsFacil.MontarQuery("( "));
                    stbSQL.Append(clsFacil.MontarQuery("  @dtr_referencia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso_complementado, "));
                    stbSQL.Append(clsFacil.MontarQuery("  getdate() "));
                    stbSQL.Append(clsFacil.MontarQuery(") "));
                    strSQL_aux = stbSQL.ToString();

                    // Montando parametros
                    clsDados.LimparParametro();
                    clsFacil.AdicionarParametro(ref strSQL_aux, "20" + CTe.CteProc.protCTe.InfProt.chCTe.Substring(2, 4), "@dtr_referencia", SqlDbType.Int);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.protCTe.InfProt.chCTe, "@cod_chave_acesso", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCteComp.chave, "@cod_chave_acesso_complementado", SqlDbType.VarChar);
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

        public string InserirXML_detalhe_xml_conhecimento_transporte_eletronico_modal_rodoviario(Common.SerializableClasses.CTe.ClsCTe_v2.proc CTe)
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
                if (CTe.CteProc.CTe.infCte.infCTeNorm.infModal.rodo.occ.Count != 0)
                {
                    for (Lcont = 0; Lcont <= CTe.CteProc.CTe.infCte.infCTeNorm.infModal.rodo.occ.Count - 1; Lcont++)
                    {
                        strSQL_aux = string.Empty;
                        stbSQL.Clear();
                        // Montando query a ser executada
                        stbSQL.Append(clsFacil.MontarQuery("INSERT INTO cte.detalhe_xml_conhecimento_transporte_eletronico_modal_rodoviario "));
                        stbSQL.Append(clsFacil.MontarQuery("( "));
                        stbSQL.Append(clsFacil.MontarQuery("  dtr_referencia, "));
                        stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso, "));
                        stbSQL.Append(clsFacil.MontarQuery("  seq_detalhe_xml_conhecimento_transporte_eletronico_modal_rodoviario, "));
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
                        stbSQL.Append(clsFacil.MontarQuery("  @seq_detalhe_xml_conhecimento_transporte_eletronico_modal_rodoviario, "));
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
                        clsFacil.AdicionarParametro(ref strSQL_aux, "20" + CTe.CteProc.protCTe.InfProt.chCTe.Substring(2, 4), "@dtr_referencia", SqlDbType.Int);
                        clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.protCTe.InfProt.chCTe, "@cod_chave_acesso", SqlDbType.VarChar);
                        clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.infModal.versaoModal, "@des_versao_modal", SqlDbType.VarChar);
                        clsFacil.AdicionarParametro(ref strSQL_aux, Convert.ToString(Lcont + 1), "@seq_detalhe_xml_conhecimento_transporte_eletronico_modal_rodoviario", SqlDbType.SmallInt);
                        clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.infModal.rodo.RNTRC, "@cod_registro_nacional_transporte_carga", SqlDbType.VarChar);
                        clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.infModal.rodo.occ[Lcont].serie, "@num_serie_ordem_coleta", SqlDbType.VarChar);
                        clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.infModal.rodo.occ[Lcont].nOcc, "@num_ordem_coleta", SqlDbType.VarChar);
                        clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.infModal.rodo.occ[Lcont].dEmi, "@dtc_emissao_ordem_coleta", SqlDbType.VarChar);

                        if (CTe.CteProc.CTe.infCte.infCTeNorm.infModal.rodo.occ[Lcont].emiOcc.CNPJ != null && CTe.CteProc.CTe.infCte.infCTeNorm.infModal.rodo.occ[Lcont].emiOcc.CNPJ != string.Empty)
                        {
                            clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.infModal.rodo.occ[Lcont].emiOcc.CNPJ.Substring(0, 8), "@num_cnpj_cpf_base_emissor_ordem_coleta", SqlDbType.VarChar);
                            clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.infModal.rodo.occ[Lcont].emiOcc.CNPJ.Substring(8, 4), "@num_cnpj_cpf_filial_emissor_ordem_coleta", SqlDbType.VarChar);
                            clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.infModal.rodo.occ[Lcont].emiOcc.CNPJ.Substring(12, 2), "@dig_cnpj_cpf_emissor_ordem_coleta", SqlDbType.SmallInt);
                        }
                        else
                        {
                            clsFacil.AdicionarParametro(ref strSQL_aux, string.Empty, "@num_cnpj_cpf_base_emissor_ordem_coleta", SqlDbType.VarChar);
                            clsFacil.AdicionarParametro(ref strSQL_aux, string.Empty, "@num_cnpj_cpf_filial_emissor_ordem_coleta", SqlDbType.VarChar);
                            clsFacil.AdicionarParametro(ref strSQL_aux, string.Empty, "@dig_cnpj_cpf_emissor_ordem_coleta", SqlDbType.SmallInt);
                        }

                        clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.infModal.rodo.occ[Lcont].emiOcc.cInt, "@cod_interno_transportadora_emissor_ordem_coleta", SqlDbType.VarChar);
                        clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.infModal.rodo.occ[Lcont].emiOcc.IE, "@num_insc_estad_emissor_ordem_coleta", SqlDbType.VarChar);
                        clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.infModal.rodo.occ[Lcont].emiOcc.UF, "@sig_unid_federacao_emissor_ordem_coleta", SqlDbType.VarChar);
                        clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.infModal.rodo.occ[Lcont].emiOcc.fone, "@num_telefone_emissor_ordem_coleta", SqlDbType.BigInt);
                        strSQL = strSQL + strSQL_aux;
                    }
                }
                else
                {
                    strSQL_aux = string.Empty;
                    stbSQL.Clear();
                    // Montando query a ser executada
                    stbSQL.Append(clsFacil.MontarQuery("INSERT INTO cte.detalhe_xml_conhecimento_transporte_eletronico_modal_rodoviario "));
                    stbSQL.Append(clsFacil.MontarQuery("( "));
                    stbSQL.Append(clsFacil.MontarQuery("  dtr_referencia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso, "));
                    stbSQL.Append(clsFacil.MontarQuery("  seq_detalhe_xml_conhecimento_transporte_eletronico_modal_rodoviario, "));
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
                    stbSQL.Append(clsFacil.MontarQuery("  @seq_detalhe_xml_conhecimento_transporte_eletronico_modal, "));
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
                    clsFacil.AdicionarParametro(ref strSQL_aux, "20" + CTe.CteProc.protCTe.InfProt.chCTe.Substring(2, 4), "@dtr_referencia", SqlDbType.Int);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.protCTe.InfProt.chCTe, "@cod_chave_acesso", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.infModal.versaoModal, "@des_versao_modal", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, "1", "@seq_detalhe_xml_conhecimento_transporte_eletronico_modal", SqlDbType.SmallInt);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.infModal.rodo.RNTRC, "@cod_registro_nacional_transporte_carga", SqlDbType.VarChar);
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

        public string InserirXML_detalhe_xml_conhecimento_transporte_eletronico_modal_aereo(Common.SerializableClasses.CTe.ClsCTe_v2.proc CTe)
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
                stbSQL.Append(clsFacil.MontarQuery("INSERT INTO cte.detalhe_xml_conhecimento_transporte_eletronico_modal_aereo "));
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
                clsFacil.AdicionarParametro(ref strSQL, "20" + CTe.CteProc.protCTe.InfProt.chCTe.Substring(2, 4), "@dtr_referencia", SqlDbType.Int);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.protCTe.InfProt.chCTe, "@cod_chave_acesso", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.infCTeNorm.infModal.versaoModal, "@des_versao_modal", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.infCTeNorm.infModal.aereo.nMinu, "@num_minuta", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.infCTeNorm.infModal.aereo.nOCA, "@num_operacional_conhecimento_aereo", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.infCTeNorm.infModal.aereo.dPrevAereo, "@dtc_previsao_entrega", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.infCTeNorm.infModal.aereo.tarifa.CL, "@cod_tipo_classe_tarifaria", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.infCTeNorm.infModal.aereo.tarifa.cTar, "@cod_tarifa", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTe.CteProc.CTe.infCte.infCTeNorm.infModal.aereo.tarifa.vTar), "@val_total_tarifa", SqlDbType.Decimal);

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

        public string InserirXML_detalhe_xml_conhecimento_transporte_eletronico_modal_aereo_natureza_carga(Common.SerializableClasses.CTe.ClsCTe_v2.proc CTe)
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
                for (Lcont = 0; Lcont <= CTe.CteProc.CTe.infCte.infCTeNorm.infModal.aereo.natCarga.cInfManu.Count - 1; Lcont++)
                {
                    strSQL_aux = string.Empty;
                    stbSQL.Clear();
                    // Montando query a ser executada
                    stbSQL.Append(clsFacil.MontarQuery("INSERT INTO cte.detalhe_xml_conhecimento_transporte_eletronico_modal_aereo_natureza_carga "));
                    stbSQL.Append(clsFacil.MontarQuery("( "));
                    stbSQL.Append(clsFacil.MontarQuery("  dtr_referencia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso, "));
                    stbSQL.Append(clsFacil.MontarQuery("  seq_detalhe_xml_conhecimento_transporte_eletronico_modal_natureza_carga, "));
                    stbSQL.Append(clsFacil.MontarQuery("  des_dimensao, "));
                    stbSQL.Append(clsFacil.MontarQuery("  cod_tipo_manuseio, "));
                    stbSQL.Append(clsFacil.MontarQuery("  dtc_atualizacao "));
                    stbSQL.Append(clsFacil.MontarQuery(") "));
                    stbSQL.Append(clsFacil.MontarQuery("VALUES "));
                    stbSQL.Append(clsFacil.MontarQuery("( "));
                    stbSQL.Append(clsFacil.MontarQuery("  @dtr_referencia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @seq_detalhe_xml_conhecimento_transporte_eletronico_modal_natureza_carga, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @des_dimensao, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @cod_tipo_manuseio, "));
                    stbSQL.Append(clsFacil.MontarQuery("  getdate() "));
                    stbSQL.Append(clsFacil.MontarQuery(") "));
                    strSQL_aux = stbSQL.ToString();

                    // Montando parametros
                    clsDados.LimparParametro();
                    clsFacil.AdicionarParametro(ref strSQL_aux, "20" + CTe.CteProc.protCTe.InfProt.chCTe.Substring(2, 4), "@dtr_referencia", SqlDbType.Int);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.protCTe.InfProt.chCTe, "@cod_chave_acesso", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, Convert.ToString(Lcont + 1), "@seq_detalhe_xml_conhecimento_transporte_eletronico_modal_natureza_carga", SqlDbType.SmallInt);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.infModal.aereo.natCarga.xDime, "@des_dimensao", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.infModal.aereo.natCarga.cInfManu[Lcont], "@cod_tipo_manuseio", SqlDbType.SmallInt);
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

        public string InserirXML_detalhe_xml_conhecimento_transporte_eletronico_modal_multimodal(Common.SerializableClasses.CTe.ClsCTe_v2.proc CTe)
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
                stbSQL.Append(clsFacil.MontarQuery("INSERT INTO cte.detalhe_xml_conhecimento_transporte_eletronico_modal_multimodal "));
                stbSQL.Append(clsFacil.MontarQuery("( "));
                stbSQL.Append(clsFacil.MontarQuery("  dtr_referencia, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso, "));
                stbSQL.Append(clsFacil.MontarQuery("  des_versao_modal, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_certificado_operador, "));
                stbSQL.Append(clsFacil.MontarQuery("  ind_negociavel, "));
                stbSQL.Append(clsFacil.MontarQuery("  dtc_atualizacao "));
                stbSQL.Append(clsFacil.MontarQuery(") "));
                stbSQL.Append(clsFacil.MontarQuery("VALUES "));
                stbSQL.Append(clsFacil.MontarQuery("( "));
                stbSQL.Append(clsFacil.MontarQuery("  @dtr_referencia, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso, "));
                stbSQL.Append(clsFacil.MontarQuery("  @des_versao_modal, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_certificado_operador, "));
                stbSQL.Append(clsFacil.MontarQuery("  @ind_negociavel, "));
                stbSQL.Append(clsFacil.MontarQuery("  getdate() "));
                stbSQL.Append(clsFacil.MontarQuery(") "));
                string strSQL = stbSQL.ToString();

                // Montando parametros
                clsDados.LimparParametro();
                clsFacil.AdicionarParametro(ref strSQL, "20" + CTe.CteProc.protCTe.InfProt.chCTe.Substring(2, 4), "@dtr_referencia", SqlDbType.Int);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.protCTe.InfProt.chCTe, "@cod_chave_acesso", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.infCTeNorm.infModal.versaoModal, "@des_versao_modal", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.infCTeNorm.infModal.multimodal.COTM, "@num_certificado_operador", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.infCTeNorm.infModal.multimodal.indNegociavel, "@ind_negociavel", SqlDbType.SmallInt);

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

        public string InserirXML_detalhe_xml_conhecimento_transporte_eletronico_modal_ferroviario(Common.SerializableClasses.CTe.ClsCTe_v2.proc CTe)
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
                stbSQL.Append(clsFacil.MontarQuery("INSERT INTO cte.detalhe_xml_conhecimento_transporte_eletronico_modal_ferroviario "));
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
                clsFacil.AdicionarParametro(ref strSQL, "20" + CTe.CteProc.protCTe.InfProt.chCTe.Substring(2, 4), "@dtr_referencia", SqlDbType.Int);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.protCTe.InfProt.chCTe, "@cod_chave_acesso", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.infCTeNorm.infModal.versaoModal, "@des_versao_modal", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.infCTeNorm.infModal.ferrov.tpTraf, "@cod_tipo_trafego", SqlDbType.SmallInt);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.infCTeNorm.infModal.ferrov.trafMut.respFat, "@cod_responsavel_faturamento", SqlDbType.SmallInt);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.infCTeNorm.infModal.ferrov.trafMut.ferrEmi, "@cod_ferrovia_emitente", SqlDbType.SmallInt);
                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTe.CteProc.CTe.infCte.infCTeNorm.infModal.ferrov.vFrete), "@val_total_frete_trafego_mutuo", SqlDbType.Decimal);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.infCTeNorm.infModal.ferrov.idTrem, "@cod_chave_acesso_cte_ferrovia", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.infCTeNorm.infModal.ferrov.fluxo, "@num_fluxo_ferroviario", SqlDbType.VarChar);

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

        public string InserirXML_detalhe_xml_conhecimento_transporte_eletronico_modal_ferroviario_ferrovia(Common.SerializableClasses.CTe.ClsCTe_v2.proc CTe)
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
                for (Lcont = 0; Lcont <= CTe.CteProc.CTe.infCte.infCTeNorm.infModal.ferrov.ferroEnv.Count - 1; Lcont++)
                {
                    strSQL_aux = string.Empty;
                    stbSQL.Clear();
                    // Montando query a ser executada
                    stbSQL.Append(clsFacil.MontarQuery("INSERT INTO cte.detalhe_xml_conhecimento_transporte_eletronico_modal_ferroviario_ferrovia "));
                    stbSQL.Append(clsFacil.MontarQuery("( "));
                    stbSQL.Append(clsFacil.MontarQuery("  dtr_referencia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso, "));
                    stbSQL.Append(clsFacil.MontarQuery("  seq_detalhe_xml_conhecimento_transporte_eletronico_modal_ferroviario_ferrovia, "));
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
                    stbSQL.Append(clsFacil.MontarQuery("  @seq_detalhe_xml_conhecimento_transporte_eletronico_modal_ferroviario_ferrovia, "));
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
                    clsFacil.AdicionarParametro(ref strSQL_aux, "20" + CTe.CteProc.protCTe.InfProt.chCTe.Substring(2, 4), "@dtr_referencia", SqlDbType.Int);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.protCTe.InfProt.chCTe, "@cod_chave_acesso", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, Convert.ToString(Lcont + 1), "@seq_detalhe_xml_conhecimento_transporte_eletronico_modal_ferroviario_ferrovia", SqlDbType.SmallInt);

                    if (CTe.CteProc.CTe.infCte.infCTeNorm.infModal.ferrov.ferroEnv[Lcont].CNPJ != null && CTe.CteProc.CTe.infCte.infCTeNorm.infModal.ferrov.ferroEnv[Lcont].CNPJ != string.Empty)
                    {
                        clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.infModal.ferrov.ferroEnv[Lcont].CNPJ.Substring(0, 8), "@num_cnpj_cpf_base_ferrovia", SqlDbType.VarChar);
                        clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.infModal.ferrov.ferroEnv[Lcont].CNPJ.Substring(8, 4), "@num_cnpj_cpf_filial_ferrovia", SqlDbType.VarChar);
                        clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.infModal.ferrov.ferroEnv[Lcont].CNPJ.Substring(12, 2), "@dig_cnpj_cpf_ferrovia", SqlDbType.SmallInt);
                    }
                    else
                    {
                        clsFacil.AdicionarParametro(ref strSQL_aux, string.Empty, "@num_cnpj_cpf_base_ferrovia", SqlDbType.VarChar);
                        clsFacil.AdicionarParametro(ref strSQL_aux, string.Empty, "@num_cnpj_cpf_filial_ferrovia", SqlDbType.VarChar);
                        clsFacil.AdicionarParametro(ref strSQL_aux, string.Empty, "@dig_cnpj_cpf_ferrovia", SqlDbType.SmallInt);
                    }

                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.infModal.ferrov.ferroEnv[Lcont].cInt, "@cod_interno_ferrovia", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.infModal.ferrov.ferroEnv[Lcont].IE, "@num_insc_estad_ferrovia", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.infModal.ferrov.ferroEnv[Lcont].xNome, "@nom_razao_social_ferrovia", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.infModal.ferrov.ferroEnv[Lcont].enderFerro.xLgr, "@des_logradouro_ferrovia", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.infModal.ferrov.ferroEnv[Lcont].enderFerro.nro, "@num_endereco_ferrovia", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.infModal.ferrov.ferroEnv[Lcont].enderFerro.xCpl, "@des_compl_endereco_ferrovia", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.infModal.ferrov.ferroEnv[Lcont].enderFerro.xBairro, "@des_bairro_ferrovia", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.infModal.ferrov.ferroEnv[Lcont].enderFerro.cMun, "@cod_municipio_ibge_ferrovia", SqlDbType.Int);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.infModal.ferrov.ferroEnv[Lcont].enderFerro.xMun, "@nom_municipio_ibge_ferrovia", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.infModal.ferrov.ferroEnv[Lcont].enderFerro.CEP, "@num_cep_ferrovia", SqlDbType.Int);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.infModal.ferrov.ferroEnv[Lcont].enderFerro.UF, "@sig_unid_federacao_ferrovia", SqlDbType.VarChar);
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

        public string InserirXML_detalhe_xml_conhecimento_transporte_eletronico_modal_dutoviario(Common.SerializableClasses.CTe.ClsCTe_v2.proc CTe)
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
                stbSQL.Append(clsFacil.MontarQuery("INSERT INTO cte.detalhe_xml_conhecimento_transporte_eletronico_modal_dutoviario "));
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
                clsFacil.AdicionarParametro(ref strSQL, "20" + CTe.CteProc.protCTe.InfProt.chCTe.Substring(2, 4), "@dtr_referencia", SqlDbType.Int);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.protCTe.InfProt.chCTe, "@cod_chave_acesso", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.infCTeNorm.infModal.versaoModal, "@des_versao_modal", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTe.CteProc.CTe.infCte.infCTeNorm.infModal.duto.vTar), "@val_total_tarifa", SqlDbType.Decimal);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.infCTeNorm.infModal.duto.dIni, "@dtc_inicio_prestracao_servico", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.infCTeNorm.infModal.duto.dFim, "@dtc_fim_prestracao_servico", SqlDbType.VarChar);

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

        public string InserirXML_detalhe_xml_conhecimento_transporte_eletronico_modal_aquaviario(Common.SerializableClasses.CTe.ClsCTe_v2.proc CTe)
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
                stbSQL.Append(clsFacil.MontarQuery("INSERT INTO cte.detalhe_xml_conhecimento_transporte_eletronico_modal_aquaviario "));
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
                clsFacil.AdicionarParametro(ref strSQL, "20" + CTe.CteProc.protCTe.InfProt.chCTe.Substring(2, 4), "@dtr_referencia", SqlDbType.Int);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.protCTe.InfProt.chCTe, "@cod_chave_acesso", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.infCTeNorm.infModal.versaoModal, "@des_versao_modal", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.infCTeNorm.infModal.aquav.xNavio, "@num_identificacao_navio", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTe.CteProc.CTe.infCte.infCTeNorm.infModal.aquav.vPrest), "@val_total_base_calculo_prestacao_afrmn", SqlDbType.Decimal);
                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(CTe.CteProc.CTe.infCte.infCTeNorm.infModal.aquav.vAFRMM), "@val_total_adicional_frete_afrmn", SqlDbType.Decimal);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.infCTeNorm.infModal.aquav.nViag, "@num_viagem", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.infCTeNorm.infModal.aquav.direc, "@sig_direcao", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.infCTeNorm.infModal.aquav.irin, "@num_irim", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, CTe.CteProc.CTe.infCte.infCTeNorm.infModal.aquav.tpNav, "@ind_navegacao", SqlDbType.SmallInt);

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

        public string InserirXML_detalhe_xml_conhecimento_transporte_eletronico_modal_aquaviario_balsa(Common.SerializableClasses.CTe.ClsCTe_v2.proc CTe)
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
                for (Lcont = 0; Lcont <= CTe.CteProc.CTe.infCte.infCTeNorm.infModal.aquav.balsa.Count - 1; Lcont++)
                {
                    strSQL_aux = string.Empty;
                    stbSQL.Clear();
                    // Montando query a ser executada
                    stbSQL.Append(clsFacil.MontarQuery("INSERT INTO cte.detalhe_xml_conhecimento_transporte_eletronico_modal_aquaviario_balsa "));
                    stbSQL.Append(clsFacil.MontarQuery("( "));
                    stbSQL.Append(clsFacil.MontarQuery("  dtr_referencia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso, "));
                    stbSQL.Append(clsFacil.MontarQuery("  seq_detalhe_xml_conhecimento_transporte_eletronico_modal_aquaviario_balsa, "));
                    stbSQL.Append(clsFacil.MontarQuery("  num_identificacao_navio, "));
                    stbSQL.Append(clsFacil.MontarQuery("  num_balsa, "));
                    stbSQL.Append(clsFacil.MontarQuery("  dtc_atualizacao "));
                    stbSQL.Append(clsFacil.MontarQuery(") "));
                    stbSQL.Append(clsFacil.MontarQuery("VALUES "));
                    stbSQL.Append(clsFacil.MontarQuery("( "));
                    stbSQL.Append(clsFacil.MontarQuery("  @dtr_referencia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @seq_detalhe_xml_conhecimento_transporte_eletronico_modal_aquaviario_balsa, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @num_identificacao_navio, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @num_balsa, "));
                    stbSQL.Append(clsFacil.MontarQuery("  getdate() "));
                    stbSQL.Append(clsFacil.MontarQuery(") "));
                    strSQL_aux = stbSQL.ToString();

                    // Montando parametros
                    clsDados.LimparParametro();
                    clsFacil.AdicionarParametro(ref strSQL_aux, "20" + CTe.CteProc.protCTe.InfProt.chCTe.Substring(2, 4), "@dtr_referencia", SqlDbType.Int);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.protCTe.InfProt.chCTe, "@cod_chave_acesso", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, Convert.ToString(Lcont + 1), "@seq_detalhe_xml_conhecimento_transporte_eletronico_modal_aquaviario_balsa", SqlDbType.SmallInt);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.infModal.aquav.xNavio, "@num_identificacao_navio", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.infModal.aquav.balsa[Lcont].xBalsa, "@num_balsa", SqlDbType.VarChar);

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

        public string InserirXML_detalhe_xml_conhecimento_transporte_eletronico_modal_aquaviario_conteiner(Common.SerializableClasses.CTe.ClsCTe_v2.proc CTe)
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
                for (Lcont = 0; Lcont <= CTe.CteProc.CTe.infCte.infCTeNorm.infModal.aquav.detCont.Count - 1; Lcont++)
                {
                    strSQL_aux = string.Empty;
                    stbSQL.Clear();
                    // Montando query a ser executada
                    stbSQL.Append(clsFacil.MontarQuery("INSERT INTO cte.detalhe_xml_conhecimento_transporte_eletronico_modal_aquaviario_conteiner "));
                    stbSQL.Append(clsFacil.MontarQuery("( "));
                    stbSQL.Append(clsFacil.MontarQuery("  dtr_referencia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso, "));
                    stbSQL.Append(clsFacil.MontarQuery("  seq_detalhe_xml_conhecimento_transporte_eletronico_modal_aquaviario_container, "));
                    stbSQL.Append(clsFacil.MontarQuery("  num_identificacao_navio, "));
                    stbSQL.Append(clsFacil.MontarQuery("  num_container, "));
                    stbSQL.Append(clsFacil.MontarQuery("  dtc_atualizacao "));
                    stbSQL.Append(clsFacil.MontarQuery(") "));
                    stbSQL.Append(clsFacil.MontarQuery("VALUES "));
                    stbSQL.Append(clsFacil.MontarQuery("( "));
                    stbSQL.Append(clsFacil.MontarQuery("  @dtr_referencia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @seq_detalhe_xml_conhecimento_transporte_eletronico_modal_aquaviario_container, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @num_identificacao_navio, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @num_container, "));
                    stbSQL.Append(clsFacil.MontarQuery("  getdate() "));
                    stbSQL.Append(clsFacil.MontarQuery(") "));
                    strSQL_aux = stbSQL.ToString();

                    // Montando parametros
                    clsDados.LimparParametro();
                    clsFacil.AdicionarParametro(ref strSQL_aux, "20" + CTe.CteProc.protCTe.InfProt.chCTe.Substring(2, 4), "@dtr_referencia", SqlDbType.Int);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.protCTe.InfProt.chCTe, "@cod_chave_acesso", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, Convert.ToString(Lcont + 1), "@seq_detalhe_xml_conhecimento_transporte_eletronico_modal_aquaviario_container", SqlDbType.SmallInt);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.infModal.aquav.xNavio, "@num_identificacao_navio", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.infModal.aquav.detCont[Lcont].nCont, "@num_container", SqlDbType.VarChar);

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

        public string InserirXML_detalhe_xml_conhecimento_transporte_eletronico_modal_aquaviario_conteiner_lacre(Common.SerializableClasses.CTe.ClsCTe_v2.proc CTe)
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
                for (i = 0; i <= CTe.CteProc.CTe.infCte.infCTeNorm.infModal.aquav.detCont.Count - 1; i++)
                {
                    int j = 0;
                    for (j = 0; j <= CTe.CteProc.CTe.infCte.infCTeNorm.infModal.aquav.detCont[i].lacre.Count - 1; j++)
                    {
                        strSQL_aux = string.Empty;
                        stbSQL.Clear();
                        // Montando query a ser executada
                        stbSQL.Append(clsFacil.MontarQuery("INSERT INTO cte.detalhe_xml_conhecimento_transporte_eletronico_modal_aquaviario_conteiner_lacre "));
                        stbSQL.Append(clsFacil.MontarQuery("( "));
                        stbSQL.Append(clsFacil.MontarQuery("  dtr_referencia, "));
                        stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso, "));
                        stbSQL.Append(clsFacil.MontarQuery("  seq_detalhe_xml_conhecimento_transporte_eletronico_modal_aquaviario_container, "));
                        stbSQL.Append(clsFacil.MontarQuery("  seq_detalhe_xml_conhecimento_transporte_eletronico_modal_aquaviario_container_lacre, "));
                        stbSQL.Append(clsFacil.MontarQuery("  num_identificacao_navio, "));
                        stbSQL.Append(clsFacil.MontarQuery("  num_container, "));
                        stbSQL.Append(clsFacil.MontarQuery("  num_lacre, "));
                        stbSQL.Append(clsFacil.MontarQuery("  dtc_atualizacao "));
                        stbSQL.Append(clsFacil.MontarQuery(") "));
                        stbSQL.Append(clsFacil.MontarQuery("VALUES "));
                        stbSQL.Append(clsFacil.MontarQuery("( "));
                        stbSQL.Append(clsFacil.MontarQuery("  @dtr_referencia, "));
                        stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso, "));
                        stbSQL.Append(clsFacil.MontarQuery("  @seq_detalhe_xml_conhecimento_transporte_eletronico_modal_aquaviario_container, "));
                        stbSQL.Append(clsFacil.MontarQuery("  @seq_detalhe_xml_conhecimento_transporte_eletronico_modal_aquaviario_container_lacre, "));
                        stbSQL.Append(clsFacil.MontarQuery("  @num_identificacao_navio, "));
                        stbSQL.Append(clsFacil.MontarQuery("  @num_container, "));
                        stbSQL.Append(clsFacil.MontarQuery("  @num_lacre, "));
                        stbSQL.Append(clsFacil.MontarQuery("  getdate() "));
                        stbSQL.Append(clsFacil.MontarQuery(") "));
                        strSQL_aux = stbSQL.ToString();

                        // Montando parametros
                        clsDados.LimparParametro();
                        clsFacil.AdicionarParametro(ref strSQL_aux, "20" + CTe.CteProc.protCTe.InfProt.chCTe.Substring(2, 4), "@dtr_referencia", SqlDbType.Int);
                        clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.protCTe.InfProt.chCTe, "@cod_chave_acesso", SqlDbType.VarChar);
                        clsFacil.AdicionarParametro(ref strSQL_aux, Convert.ToString(i + 1), "@seq_detalhe_xml_conhecimento_transporte_eletronico_modal_aquaviario_container", SqlDbType.SmallInt);
                        clsFacil.AdicionarParametro(ref strSQL_aux, Convert.ToString(j + 1), "@seq_detalhe_xml_conhecimento_transporte_eletronico_modal_aquaviario_container_lacre", SqlDbType.SmallInt);
                        clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.infModal.aquav.xNavio, "@num_identificacao_navio", SqlDbType.VarChar);
                        clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.infModal.aquav.detCont[i].nCont, "@num_container", SqlDbType.VarChar);
                        clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.infModal.aquav.detCont[i].lacre[j].nLacre, "@num_lacre", SqlDbType.VarChar);
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

        public string InserirXML_detalhe_xml_conhecimento_transporte_eletronico_modal_aquaviario_nota_fiscal(Common.SerializableClasses.CTe.ClsCTe_v2.proc CTe)
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
                for (i = 0; i <= CTe.CteProc.CTe.infCte.infCTeNorm.infModal.aquav.detCont.Count - 1; i++)
                {
                    int j = 0;
                    for (j = 0; j <= CTe.CteProc.CTe.infCte.infCTeNorm.infModal.aquav.detCont[i].infDoc.infNF.Count - 1; j++)
                    {
                        strSQL_aux = string.Empty;
                        stbSQL.Clear();
                        // Montando query a ser executada
                        stbSQL.Append(clsFacil.MontarQuery("INSERT INTO cte.detalhe_xml_conhecimento_transporte_eletronico_modal_aquaviario_nota_fiscal "));
                        stbSQL.Append(clsFacil.MontarQuery("( "));
                        stbSQL.Append(clsFacil.MontarQuery("  dtr_referencia, "));
                        stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso, "));
                        stbSQL.Append(clsFacil.MontarQuery("  seq_detalhe_xml_conhecimento_transporte_eletronico_modal_aquaviario_nota_fiscal, "));
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
                        stbSQL.Append(clsFacil.MontarQuery("  @seq_detalhe_xml_conhecimento_transporte_eletronico_modal_aquaviario_nota_fiscal, "));
                        stbSQL.Append(clsFacil.MontarQuery("  @num_identificacao_navio, "));
                        stbSQL.Append(clsFacil.MontarQuery("  @num_serie_nota_fiscal, "));
                        stbSQL.Append(clsFacil.MontarQuery("  @num_documento_fiscal, "));
                        stbSQL.Append(clsFacil.MontarQuery("  @num_unidade_medida_rateada, "));
                        stbSQL.Append(clsFacil.MontarQuery("  getdate() "));
                        stbSQL.Append(clsFacil.MontarQuery(") "));
                        strSQL_aux = stbSQL.ToString();

                        // Montando parametros
                        clsDados.LimparParametro();
                        clsFacil.AdicionarParametro(ref strSQL_aux, "20" + CTe.CteProc.protCTe.InfProt.chCTe.Substring(2, 4), "@dtr_referencia", SqlDbType.Int);
                        clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.protCTe.InfProt.chCTe, "@cod_chave_acesso", SqlDbType.VarChar);
                        clsFacil.AdicionarParametro(ref strSQL_aux, Convert.ToString(j + 1), "@seq_detalhe_xml_conhecimento_transporte_eletronico_modal_aquaviario_nota_fiscal", SqlDbType.SmallInt);
                        clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.infModal.aquav.xNavio, "@num_identificacao_navio", SqlDbType.VarChar);
                        clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.infModal.aquav.detCont[i].infDoc.infNF[j].serie, "@num_serie_nota_fiscal", SqlDbType.VarChar);
                        clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.infModal.aquav.detCont[i].infDoc.infNF[j].nDoc, "@num_documento_fiscal", SqlDbType.VarChar);
                        clsFacil.AdicionarParametro(ref strSQL_aux, Convert.ToString(CTe.CteProc.CTe.infCte.infCTeNorm.infModal.aquav.detCont[i].infDoc.infNF[j].unidRat), "@num_unidade_medida_rateada", SqlDbType.Decimal);
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

        public string InserirXML_detalhe_xml_conhecimento_transporte_eletronico_modal_aquaviario_nota_fiscal_eletronica(Common.SerializableClasses.CTe.ClsCTe_v2.proc CTe)
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
                for (i = 0; i <= CTe.CteProc.CTe.infCte.infCTeNorm.infModal.aquav.detCont.Count - 1; i++)
                {
                    int j = 0;
                    for (j = 0; j <= CTe.CteProc.CTe.infCte.infCTeNorm.infModal.aquav.detCont[i].infDoc.infNFe.Count - 1; j++)
                    {
                        strSQL_aux = string.Empty;
                        stbSQL.Clear();
                        // Montando query a ser executada
                        stbSQL.Append(clsFacil.MontarQuery("INSERT INTO cte.detalhe_xml_conhecimento_transporte_eletronico_modal_aquaviario_nota_fiscal_eletronica "));
                        stbSQL.Append(clsFacil.MontarQuery("( "));
                        stbSQL.Append(clsFacil.MontarQuery("  dtr_referencia, "));
                        stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso, "));
                        stbSQL.Append(clsFacil.MontarQuery("  seq_detalhe_xml_conhecimento_transporte_eletronico_modal_aquaviario_nota_fiscal_eletronica, "));
                        stbSQL.Append(clsFacil.MontarQuery("  num_identificacao_navio, "));
                        stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso_nfe, "));
                        stbSQL.Append(clsFacil.MontarQuery("  val_unidade_medida, "));
                        stbSQL.Append(clsFacil.MontarQuery("  dtc_atualizacao "));
                        stbSQL.Append(clsFacil.MontarQuery(") "));
                        stbSQL.Append(clsFacil.MontarQuery("VALUES "));
                        stbSQL.Append(clsFacil.MontarQuery("( "));
                        stbSQL.Append(clsFacil.MontarQuery("  @dtr_referencia, "));
                        stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso, "));
                        stbSQL.Append(clsFacil.MontarQuery("  @seq_detalhe_xml_conhecimento_transporte_eletronico_modal_aquaviario_nota_fiscal_eletronica, "));
                        stbSQL.Append(clsFacil.MontarQuery("  @num_identificacao_navio, "));
                        stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso_nfe, "));
                        stbSQL.Append(clsFacil.MontarQuery("  @val_unidade_medida, "));
                        stbSQL.Append(clsFacil.MontarQuery("  getdate() "));
                        stbSQL.Append(clsFacil.MontarQuery(") "));
                        strSQL_aux = stbSQL.ToString();

                        // Montando parametros
                        clsDados.LimparParametro();
                        clsFacil.AdicionarParametro(ref strSQL_aux, "20" + CTe.CteProc.protCTe.InfProt.chCTe.Substring(2, 4), "@dtr_referencia", SqlDbType.Int);
                        clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.protCTe.InfProt.chCTe, "@cod_chave_acesso", SqlDbType.VarChar);
                        clsFacil.AdicionarParametro(ref strSQL_aux, Convert.ToString(j + 1), "@seq_detalhe_xml_conhecimento_transporte_eletronico_modal_aquaviario_nota_fiscal_eletronica", SqlDbType.SmallInt);
                        clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.infModal.aquav.xNavio, "@num_identificacao_navio", SqlDbType.VarChar);
                        clsFacil.AdicionarParametro(ref strSQL_aux, CTe.CteProc.CTe.infCte.infCTeNorm.infModal.aquav.detCont[i].infDoc.infNFe[j].chave, "@cod_chave_acesso_nfe", SqlDbType.VarChar);
                        clsFacil.AdicionarParametro(ref strSQL_aux, Convert.ToString(CTe.CteProc.CTe.infCte.infCTeNorm.infModal.aquav.detCont[i].infDoc.infNFe[j].unidRat), "@val_unidade_medida", SqlDbType.Decimal);
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

        #endregion


    }
}