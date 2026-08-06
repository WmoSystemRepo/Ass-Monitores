using System;
using System.Data;
using System.Text;

namespace DFe
{
    class ClsInserirGTVe

    {
       

        #region "Insert GTVe"

        public string InserirXML_detalhe_xml_guia_transporte_valor_eletronico_autorizado(Common.SerializableClasses.CTe.ClsXmlGTVeNT202402.proc GTVe)
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
                stbSQL.Append(clsFacil.MontarQuery("INSERT INTO cte.detalhe_xml_guia_transporte_valor_eletronico_autorizado "));
                stbSQL.Append(clsFacil.MontarQuery("( "));
                stbSQL.Append(clsFacil.MontarQuery("  dtr_referencia, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_gerado_emitente, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_unid_federacao_emitente, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_cfop, "));
                stbSQL.Append(clsFacil.MontarQuery("  des_natureza_operacao, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_serie_guia_transporte_valor_eletronico, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_guia_transporte_valor_eletronico, "));
                stbSQL.Append(clsFacil.MontarQuery("  dtc_emissao, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_tipo_modal_transporte_documento_fiscal_eletronico, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_tipo_formato_impressao_documento, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_tipo_forma_emissao_documento_fiscal_eletronico, "));
                stbSQL.Append(clsFacil.MontarQuery("  dig_chave_acesso, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_tipo_ambiente, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_tipo_guia_transporte_valor_eletronico, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_versao_processo_emissao, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_municipio_envio_guia_transporte_valor_eletronico, "));
                stbSQL.Append(clsFacil.MontarQuery("  nom_municipio_envio_guia_transporte_valor_eletronico, "));
                stbSQL.Append(clsFacil.MontarQuery("  sig_unid_federacao_envio_guia_transporte_valor_eletronico, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_tipo_servico_conhecimento_transporte_eletronico, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_insc_estad_tomador, "));
                stbSQL.Append(clsFacil.MontarQuery("  dtc_saida_origem, "));
                stbSQL.Append(clsFacil.MontarQuery("  dtc_chegada_destino, "));                
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
                stbSQL.Append(clsFacil.MontarQuery("  dtc_entrada_contingencia, "));
                stbSQL.Append(clsFacil.MontarQuery("  des_justificativa_entrada_contingencia, "));
                stbSQL.Append(clsFacil.MontarQuery("  des_caracteristica_adicional_transporte, "));
                stbSQL.Append(clsFacil.MontarQuery("  des_caracteristica_adicional_servico, "));
                stbSQL.Append(clsFacil.MontarQuery("  nom_funcionario_emissor, "));
                stbSQL.Append(clsFacil.MontarQuery("  des_observacao_geral, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_cnpj_cpf_base_emitente, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_cnpj_cpf_filial_emitente, "));
                stbSQL.Append(clsFacil.MontarQuery("  dig_cnpj_cpf_emitente, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_insc_estad_emitente, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_insc_estad_st_emitente, "));
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
                stbSQL.Append(clsFacil.MontarQuery("  num_cnpj_cpf_base_destinatário, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_cnpj_cpf_filial_destinatário, "));
                stbSQL.Append(clsFacil.MontarQuery("  dig_cnpj_cpf_destinatário, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_insc_estad_destinatário, "));
                stbSQL.Append(clsFacil.MontarQuery("  nom_razao_social_destinatário, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_telefone_destinatário, "));                
                stbSQL.Append(clsFacil.MontarQuery("  des_logradouro_destinatário, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_endereco_destinatário, "));
                stbSQL.Append(clsFacil.MontarQuery("  des_compl_endereco_destinatário, "));
                stbSQL.Append(clsFacil.MontarQuery("  des_bairro_destinatário, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_municipio_ibge_destinatário, "));
                stbSQL.Append(clsFacil.MontarQuery("  nom_municipio_ibge_destinatário, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_cep_destinatário, "));
                stbSQL.Append(clsFacil.MontarQuery("  sig_unid_federacao_destinatário, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_pais_bacen_destinatário, "));
                stbSQL.Append(clsFacil.MontarQuery("  nom_pais_bacen_destinatário, "));
                stbSQL.Append(clsFacil.MontarQuery("  nom_email_destinatário, "));
                stbSQL.Append(clsFacil.MontarQuery("  des_logradouro_origem, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_endereco_origem, "));
                stbSQL.Append(clsFacil.MontarQuery("  des_compl_endereco_origem, "));
                stbSQL.Append(clsFacil.MontarQuery("  des_bairro_origem, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_municipio_ibge_origem, "));
                stbSQL.Append(clsFacil.MontarQuery("  nom_municipio_ibge_origem, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_cep_origem, "));
                stbSQL.Append(clsFacil.MontarQuery("  sig_unid_federacao_origem, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_telefone_origem, "));
                stbSQL.Append(clsFacil.MontarQuery("  des_logradouro_destino, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_endereco_destino, "));
                stbSQL.Append(clsFacil.MontarQuery("  des_compl_endereco_destino, "));
                stbSQL.Append(clsFacil.MontarQuery("  des_bairro_destino, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_municipio_ibge_destino, "));
                stbSQL.Append(clsFacil.MontarQuery("  nom_municipio_ibge_destino, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_cep_destino, "));
                stbSQL.Append(clsFacil.MontarQuery("  sig_unid_federacao_destino, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_telefone_destino, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_cnpj_cpf_base_responsavel_tecnico, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_cnpj_cpf_filial_responsavel_tecnico, "));
                stbSQL.Append(clsFacil.MontarQuery("  dig_cnpj_cpf_responsavel_tecnico, "));
                stbSQL.Append(clsFacil.MontarQuery("  nom_pessoa_ser_contatada, "));
                stbSQL.Append(clsFacil.MontarQuery("  nom_email_responsavel_tecnico, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_telefone_responsavel_tecnico, "));
                stbSQL.Append(clsFacil.MontarQuery("  sig_identificador_hash_codigo_seguraca_responsavel_tecnico, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_hash_csrt, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_tipo_modelo_documento_fiscal, "));
                stbSQL.Append(clsFacil.MontarQuery("  dtc_atualizacao, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_tipo_servico_conhecimento_transporte_eletronico_tomador, "));
                stbSQL.Append(clsFacil.MontarQuery("  num_insc_suframa_destinatario "));
                stbSQL.Append(clsFacil.MontarQuery(") "));
                stbSQL.Append(clsFacil.MontarQuery("VALUES "));
                stbSQL.Append(clsFacil.MontarQuery("( "));
                stbSQL.Append(clsFacil.MontarQuery("  @dtr_referencia, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_gerado_emitente, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_unid_federacao_emitente, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_cfop, "));
                stbSQL.Append(clsFacil.MontarQuery("  @des_natureza_operacao, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_serie_guia_transporte_valor_eletronico, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_guia_transporte_valor_eletronico, "));
                stbSQL.Append(clsFacil.MontarQuery("  @dtc_emissao, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_tipo_modal_transporte_documento_fiscal_eletronico, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_tipo_formato_impressao_documento, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_tipo_forma_emissao_documento_fiscal_eletronico, "));
                stbSQL.Append(clsFacil.MontarQuery("  @dig_chave_acesso, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_tipo_ambiente, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_tipo_guia_transporte_valor_eletronico, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_versao_processo_emissao, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_municipio_envio_guia_transporte_valor_eletronico, "));
                stbSQL.Append(clsFacil.MontarQuery("  @nom_municipio_envio_guia_transporte_valor_eletronico, "));
                stbSQL.Append(clsFacil.MontarQuery("  @sig_unid_federacao_envio_guia_transporte_valor_eletronico, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_tipo_servico_conhecimento_transporte_eletronico, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_insc_estad_tomador, "));
                stbSQL.Append(clsFacil.MontarQuery("  @dtc_saida_origem, "));
                stbSQL.Append(clsFacil.MontarQuery("  @dtc_chegada_destino, "));                
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
                stbSQL.Append(clsFacil.MontarQuery("  @dtc_entrada_contingencia, "));
                stbSQL.Append(clsFacil.MontarQuery("  @des_justificativa_entrada_contingencia, "));
                stbSQL.Append(clsFacil.MontarQuery("  @des_caracteristica_adicional_transporte, "));
                stbSQL.Append(clsFacil.MontarQuery("  @des_caracteristica_adicional_servico, "));
                stbSQL.Append(clsFacil.MontarQuery("  @nom_funcionario_emissor, "));
                stbSQL.Append(clsFacil.MontarQuery("  @des_observacao_geral, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_cnpj_cpf_base_emitente, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_cnpj_cpf_filial_emitente, "));
                stbSQL.Append(clsFacil.MontarQuery("  @dig_cnpj_cpf_emitente, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_insc_estad_emitente, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_insc_estad_st_emitente, "));
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
                stbSQL.Append(clsFacil.MontarQuery("  @num_cnpj_cpf_base_destinatário, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_cnpj_cpf_filial_destinatário, "));
                stbSQL.Append(clsFacil.MontarQuery("  @dig_cnpj_cpf_destinatário, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_insc_estad_destinatário, "));
                stbSQL.Append(clsFacil.MontarQuery("  @nom_razao_social_destinatário, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_telefone_destinatário, "));                
                stbSQL.Append(clsFacil.MontarQuery("  @des_logradouro_destinatário, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_endereco_destinatário, "));
                stbSQL.Append(clsFacil.MontarQuery("  @des_compl_endereco_destinatário, "));
                stbSQL.Append(clsFacil.MontarQuery("  @des_bairro_destinatário, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_municipio_ibge_destinatário, "));
                stbSQL.Append(clsFacil.MontarQuery("  @nom_municipio_ibge_destinatário, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_cep_destinatário, "));
                stbSQL.Append(clsFacil.MontarQuery("  @sig_unid_federacao_destinatário, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_pais_bacen_destinatário, "));
                stbSQL.Append(clsFacil.MontarQuery("  @nom_pais_bacen_destinatário, "));
                stbSQL.Append(clsFacil.MontarQuery("  @nom_email_destinatário, "));
                stbSQL.Append(clsFacil.MontarQuery("  @des_logradouro_origem, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_endereco_origem, "));
                stbSQL.Append(clsFacil.MontarQuery("  @des_compl_endereco_origem, "));
                stbSQL.Append(clsFacil.MontarQuery("  @des_bairro_origem, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_municipio_ibge_origem, "));
                stbSQL.Append(clsFacil.MontarQuery("  @nom_municipio_ibge_origem, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_cep_origem, "));
                stbSQL.Append(clsFacil.MontarQuery("  @sig_unid_federacao_origem, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_telefone_origem, "));
                stbSQL.Append(clsFacil.MontarQuery("  @des_logradouro_destino, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_endereco_destino, "));
                stbSQL.Append(clsFacil.MontarQuery("  @des_compl_endereco_destino, "));
                stbSQL.Append(clsFacil.MontarQuery("  @des_bairro_destino, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_municipio_ibge_destino, "));
                stbSQL.Append(clsFacil.MontarQuery("  @nom_municipio_ibge_destino, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_cep_destino, "));
                stbSQL.Append(clsFacil.MontarQuery("  @sig_unid_federacao_destino, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_telefone_destino, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_cnpj_cpf_base_responsavel_tecnico, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_cnpj_cpf_filial_responsavel_tecnico, "));
                stbSQL.Append(clsFacil.MontarQuery("  @dig_cnpj_cpf_responsavel_tecnico, "));
                stbSQL.Append(clsFacil.MontarQuery("  @nom_pessoa_ser_contatada, "));
                stbSQL.Append(clsFacil.MontarQuery("  @nom_email_responsavel_tecnico, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_telefone_responsavel_tecnico, "));
                stbSQL.Append(clsFacil.MontarQuery("  @sig_identificador_hash_codigo_seguraca_responsavel_tecnico, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_hash_csrt, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_tipo_modelo_documento_fiscal, "));
                stbSQL.Append(clsFacil.MontarQuery("  getdate(), "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_tipo_servico_conhecimento_transporte_eletronico_tomador, "));
                stbSQL.Append(clsFacil.MontarQuery("  @num_insc_suframa_destinatario "));
                stbSQL.Append(clsFacil.MontarQuery(") "));
                string strSQL = stbSQL.ToString();

                // Montando parametros
                clsDados.LimparParametro();
                clsFacil.AdicionarParametro(ref strSQL, "20" + GTVe.GTVeProc.protCTe.InfProt.chCTe.Substring(2, 4), "@dtr_referencia", SqlDbType.Int);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.protCTe.InfProt.chCTe, "@cod_chave_acesso", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.ide.cCT, "@num_gerado_emitente", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.ide.cUF, "@cod_unid_federacao_emitente", SqlDbType.Int);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.ide.CFOP, "@num_cfop", SqlDbType.SmallInt);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.ide.natOp, "@des_natureza_operacao", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.ide.serie, "@num_serie_guia_transporte_valor_eletronico", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.ide.nCT, "@num_guia_transporte_valor_eletronico", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.ide.dhEmi, "@dtc_emissao", SqlDbType.DateTime);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.ide.modal, "@cod_tipo_modal_transporte_documento_fiscal_eletronico", SqlDbType.SmallInt);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.ide.tpImp, "@cod_tipo_formato_impressao_documento", SqlDbType.SmallInt);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.ide.tpEmis, "@cod_tipo_forma_emissao_documento_fiscal_eletronico", SqlDbType.SmallInt);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.ide.cDV, "@dig_chave_acesso", SqlDbType.SmallInt);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.ide.tpAmb, "@cod_tipo_ambiente", SqlDbType.SmallInt);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.ide.tpCTe, "@cod_tipo_guia_transporte_valor_eletronico", SqlDbType.SmallInt);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.ide.verProc, "@num_versao_processo_emissao", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.ide.cMunEnv, "@cod_municipio_envio_guia_transporte_valor_eletronico", SqlDbType.Int);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.ide.xMunEnv, "@nom_municipio_envio_guia_transporte_valor_eletronico", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.ide.UFEnv, "@sig_unid_federacao_envio_guia_transporte_valor_eletronico", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.ide.tpServ, "@cod_tipo_servico_conhecimento_transporte_eletronico", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.ide.indIEToma, "@cod_insc_estad_tomador", SqlDbType.SmallInt);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.ide.dhSaidaOrig, "@dtc_saida_origem", SqlDbType.DateTime);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.ide.dhChegadaDest, "@dtc_chegada_destino", SqlDbType.DateTime);

                clsFacil.AdicionarParametro(ref strSQL, string.Empty, "@num_cnpj_cpf_base_tomador", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, string.Empty, "@num_cnpj_cpf_filial_tomador", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, string.Empty, "@dig_cnpj_cpf_tomador", SqlDbType.SmallInt);
                if (GTVe.GTVeProc.GTVe.infCte.ide.tomaTerceiro.CPF != null && GTVe.GTVeProc.GTVe.infCte.ide.tomaTerceiro.CPF != string.Empty)
                {
                    clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.ide.tomaTerceiro.CPF.Substring(0, 9), "@num_cnpj_cpf_base_tomador", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, "0", "@num_cnpj_cpf_filial_tomador", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.ide.tomaTerceiro.CPF.Substring(9, 2), "@dig_cnpj_cpf_tomador", SqlDbType.SmallInt);
                }
                if (GTVe.GTVeProc.GTVe.infCte.ide.tomaTerceiro.CNPJ != null && GTVe.GTVeProc.GTVe.infCte.ide.tomaTerceiro.CNPJ != string.Empty)
                {
                    clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.ide.tomaTerceiro.CNPJ.Substring(0, 8), "@num_cnpj_cpf_base_tomador", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.ide.tomaTerceiro.CNPJ.Substring(8, 4), "@num_cnpj_cpf_filial_tomador", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.ide.tomaTerceiro.CNPJ.Substring(12, 2), "@dig_cnpj_cpf_tomador", SqlDbType.SmallInt);
                }

                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.ide.tomaTerceiro.IE, "@num_insc_estad_tomador", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.ide.tomaTerceiro.xNome, "@nom_razao_social_tomador", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.ide.tomaTerceiro.xFant, "@nom_fantasia_tomador", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.ide.tomaTerceiro.fone, "@num_telefone_tomador", SqlDbType.BigInt);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.ide.tomaTerceiro.enderToma.xLgr, "@des_logradouro_tomador", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.ide.tomaTerceiro.enderToma.nro, "@num_endereco_tomador", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.ide.tomaTerceiro.enderToma.xCpl, "@des_compl_endereco_tomador", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.ide.tomaTerceiro.enderToma.xBairro, "@des_bairro_tomador", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.ide.tomaTerceiro.enderToma.cMun, "@cod_municipio_ibge_tomador", SqlDbType.Int);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.ide.tomaTerceiro.enderToma.xMun, "@nom_municipio_ibge_tomador", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.ide.tomaTerceiro.enderToma.CEP, "@num_cep_tomador", SqlDbType.Int);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.ide.tomaTerceiro.enderToma.UF, "@sig_unid_federacao_tomador", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.ide.tomaTerceiro.enderToma.cPais, "@cod_pais_bacen_tomador", SqlDbType.SmallInt);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.ide.tomaTerceiro.enderToma.xPais, "@nom_pais_bacen_tomador", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.ide.tomaTerceiro.enderToma.email, "@nom_email_tomador", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.ide.dhCont, "@dtc_entrada_contingencia", SqlDbType.DateTime);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.ide.xJust, "@des_justificativa_entrada_contingencia", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.compl.xCaracAd, "@des_caracteristica_adicional_transporte", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.compl.xCaracSer, "@des_caracteristica_adicional_servico", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.compl.xEmi, "@nom_funcionario_emissor", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.compl.xObs, "@des_observacao_geral", SqlDbType.VarChar);

                if (GTVe.GTVeProc.GTVe.infCte.emit.CNPJ != null && GTVe.GTVeProc.GTVe.infCte.emit.CNPJ != string.Empty)
                {
                    clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.emit.CNPJ.Substring(0, 8), "@num_cnpj_cpf_base_emitente", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.emit.CNPJ.Substring(8, 4), "@num_cnpj_cpf_filial_emitente", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.emit.CNPJ.Substring(12, 2), "@dig_cnpj_cpf_emitente", SqlDbType.SmallInt);
                }
                else
                {
                    clsFacil.AdicionarParametro(ref strSQL, string.Empty, "@num_cnpj_cpf_base_emitente", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, string.Empty, "@num_cnpj_cpf_filial_emitente", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, string.Empty, "@dig_cnpj_cpf_emitente", SqlDbType.SmallInt);
                }

                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.emit.IE, "@num_insc_estad_emitente", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.emit.IEST, "@num_insc_estad_st_emitente", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.emit.xNome, "@nom_razao_social_emitente", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.emit.xFant, "@nom_fantasia_emitente", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.emit.enderEmit.xLgr, "@des_logradouro_emitente", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.emit.enderEmit.nro, "@num_endereco_emitente", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.emit.enderEmit.xCpl, "@des_compl_endereco_emitente", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.emit.enderEmit.xBairro, "@des_bairro_emitente", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.emit.enderEmit.cMun, "@cod_municipio_ibge_emitente", SqlDbType.Int);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.emit.enderEmit.xMun, "@nom_municipio_ibge_emitente", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.emit.enderEmit.CEP, "@num_cep_emitente", SqlDbType.Int);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.emit.enderEmit.UF, "@sig_unid_federacao_emitente", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.emit.enderEmit.fone, "@num_telefone_emitente", SqlDbType.BigInt);
                                
                if (GTVe.GTVeProc.GTVe.infCte.rem.CPF != null && GTVe.GTVeProc.GTVe.infCte.rem.CPF != string.Empty)
                {
                    clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.rem.CPF.Substring(0, 9), "@num_cnpj_cpf_base_remetente", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, "0", "@num_cnpj_cpf_filial_remetente", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.rem.CPF.Substring(9, 2), "@dig_cnpj_cpf_remetente", SqlDbType.SmallInt);
                }
                if (GTVe.GTVeProc.GTVe.infCte.rem.CNPJ != null && GTVe.GTVeProc.GTVe.infCte.rem.CNPJ != string.Empty)
                {
                    clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.rem.CNPJ.Substring(0, 8), "@num_cnpj_cpf_base_remetente", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.rem.CNPJ.Substring(8, 4), "@num_cnpj_cpf_filial_remetente", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.rem.CNPJ.Substring(12, 2), "@dig_cnpj_cpf_remetente", SqlDbType.SmallInt);
                }
                if ((GTVe.GTVeProc.GTVe.infCte.rem.CPF == null && GTVe.GTVeProc.GTVe.infCte.rem.CPF == null) || (GTVe.GTVeProc.GTVe.infCte.rem.CPF == null && GTVe.GTVeProc.GTVe.infCte.rem.CPF == string.Empty) || (GTVe.GTVeProc.GTVe.infCte.rem.CPF == string.Empty && GTVe.GTVeProc.GTVe.infCte.rem.CPF == null))
                {
                    clsFacil.AdicionarParametro(ref strSQL, string.Empty, "@num_cnpj_cpf_base_remetente", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, string.Empty, "@num_cnpj_cpf_filial_remetente", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, string.Empty, "@dig_cnpj_cpf_remetente", SqlDbType.SmallInt);
                }

                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.rem.IE, "@num_insc_estad_remetente", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.rem.xNome, "@nom_razao_social_remetente", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.rem.xFant, "@nom_fantasia_remetente", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.rem.fone, "@num_telefone_remetente", SqlDbType.BigInt);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.rem.enderReme.xLgr, "@des_logradouro_remetente", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.rem.enderReme.nro, "@num_endereco_remetente", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.rem.enderReme.xCpl, "@des_compl_endereco_remetente", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.rem.enderReme.xBairro, "@des_bairro_remetente", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.rem.enderReme.cMun, "@cod_municipio_ibge_remetente", SqlDbType.Int);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.rem.enderReme.xMun, "@nom_municipio_ibge_remetente", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.rem.enderReme.CEP, "@num_cep_remetente", SqlDbType.Int);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.rem.enderReme.UF, "@sig_unid_federacao_remetente", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.rem.enderReme.cPais, "@cod_pais_bacen_remetente", SqlDbType.SmallInt);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.rem.enderReme.xPais, "@nom_pais_bacen_remetente", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.rem.enderReme.email, "@nom_email_remetente", SqlDbType.VarChar);
                                
                if (GTVe.GTVeProc.GTVe.infCte.rem.CPF != null && GTVe.GTVeProc.GTVe.infCte.rem.CPF != string.Empty)
                {
                    clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.rem.CPF.Substring(0, 9), "@num_cnpj_cpf_base_destinatário", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, "0", "@num_cnpj_cpf_filial_destinatário", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.rem.CPF.Substring(9, 2), "@dig_cnpj_cpf_destinatário", SqlDbType.SmallInt);
                }
                if (GTVe.GTVeProc.GTVe.infCte.rem.CNPJ != null && GTVe.GTVeProc.GTVe.infCte.rem.CNPJ != string.Empty)
                {
                    clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.rem.CNPJ.Substring(0, 8), "@num_cnpj_cpf_base_destinatário", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.rem.CNPJ.Substring(8, 4), "@num_cnpj_cpf_filial_destinatário", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.rem.CNPJ.Substring(12, 2), "@dig_cnpj_cpf_destinatário", SqlDbType.SmallInt);
                }
                if ((GTVe.GTVeProc.GTVe.infCte.rem.CPF == null && GTVe.GTVeProc.GTVe.infCte.rem.CPF == null) || (GTVe.GTVeProc.GTVe.infCte.rem.CPF == null && GTVe.GTVeProc.GTVe.infCte.rem.CPF == string.Empty) || (GTVe.GTVeProc.GTVe.infCte.rem.CPF == string.Empty  && GTVe.GTVeProc.GTVe.infCte.rem.CPF == null))
                {
                    clsFacil.AdicionarParametro(ref strSQL, string.Empty, "@num_cnpj_cpf_base_destinatário", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, string.Empty, "@num_cnpj_cpf_filial_destinatário", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, string.Empty, "@dig_cnpj_cpf_destinatário", SqlDbType.SmallInt);
                }
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.dest.IE, "@num_insc_estad_destinatário", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.dest.xNome, "@nom_razao_social_destinatário", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.dest.fone, "@num_telefone_destinatário", SqlDbType.BigInt);                
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.dest.enderDest.xLgr, "@des_logradouro_destinatário", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.dest.enderDest.nro, "@num_endereco_destinatário", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.dest.enderDest.xCpl, "@des_compl_endereco_destinatário", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.dest.enderDest.xBairro, "@des_bairro_destinatário", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.dest.enderDest.cMun, "@cod_municipio_ibge_destinatário", SqlDbType.Int);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.dest.enderDest.xMun, "@nom_municipio_ibge_destinatário", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.dest.enderDest.CEP, "@num_cep_destinatário", SqlDbType.Int);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.dest.enderDest.UF, "@sig_unid_federacao_destinatário", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.dest.enderDest.cPais, "@cod_pais_bacen_destinatário", SqlDbType.SmallInt);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.dest.enderDest.xPais, "@nom_pais_bacen_destinatário", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.dest.email, "@nom_email_destinatário", SqlDbType.VarChar);

                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.origem.xLgr, "@des_logradouro_origem", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.origem.nro, "@num_endereco_origem", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.origem.xCpl, "@des_compl_endereco_origem", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.origem.xBairro, "@des_bairro_origem", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.origem.cMun, "@cod_municipio_ibge_origem", SqlDbType.Int);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.origem.xMun, "@nom_municipio_ibge_origem", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.origem.CEP, "@num_cep_origem", SqlDbType.Int);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.origem.UF, "@sig_unid_federacao_origem", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.origem.fone, "@num_telefone_origem", SqlDbType.BigInt);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.destino.xLgr, "@des_logradouro_destino", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.destino.nro, "@num_endereco_destino", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.destino.xCpl, "@des_compl_endereco_destino", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.destino.xBairro, "@des_bairro_destino", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.destino.cMun, "@cod_municipio_ibge_destino", SqlDbType.Int);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.destino.xMun, "@nom_municipio_ibge_destino", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.destino.CEP, "@num_cep_destino", SqlDbType.Int);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.destino.UF, "@sig_unid_federacao_destino", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.destino.fone, "@num_telefone_destino", SqlDbType.BigInt);

                
                if (GTVe.GTVeProc.GTVe.infCte.infRespTec.CNPJ != null && GTVe.GTVeProc.GTVe.infCte.infRespTec.CNPJ != string.Empty )
                {
                    clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.infRespTec.CNPJ.Substring(0, 8), "@num_cnpj_cpf_base_responsavel_tecnico", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.infRespTec.CNPJ.Substring(8, 4), "@num_cnpj_cpf_filial_responsavel_tecnico", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.infRespTec.CNPJ.Substring(12, 2), "@dig_cnpj_cpf_responsavel_tecnico", SqlDbType.SmallInt);
                }
                else
                {
                    clsFacil.AdicionarParametro(ref strSQL, string.Empty, "@num_cnpj_cpf_base_responsavel_tecnico", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, string.Empty, "@num_cnpj_cpf_filial_responsavel_tecnico", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL, string.Empty, "@dig_cnpj_cpf_responsavel_tecnico", SqlDbType.SmallInt);
                }
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.infRespTec.xContato, "@nom_pessoa_ser_contatada", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.infRespTec.email, "@nom_email_responsavel_tecnico", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.infRespTec.fone, "@num_telefone_responsavel_tecnico", SqlDbType.BigInt);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.infRespTec.idCSRT, "@sig_identificador_hash_codigo_seguraca_responsavel_tecnico", SqlDbType.SmallInt);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.infRespTec.hashCSRT, "@cod_hash_csrt", SqlDbType.VarChar);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.ide.mod, "@cod_tipo_modelo_documento_fiscal", SqlDbType.SmallInt);
                if (GTVe.GTVeProc.GTVe.infCte.ide.toma.strToma != null)
                {
                    clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.ide.toma.strToma, "@cod_tipo_servico_conhecimento_transporte_eletronico_tomador", SqlDbType.VarChar);
                }
                if (GTVe.GTVeProc.GTVe.infCte.ide.tomaTerceiro.toma != null)
                {
                    clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.ide.tomaTerceiro.toma, "@cod_tipo_servico_conhecimento_transporte_eletronico_tomador", SqlDbType.VarChar);
                }
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.GTVe.infCte.dest.ISUF, "@num_insc_suframa_destinatario", SqlDbType.VarChar);


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

        public string InserirXML_detalhe_xml_guia_transporte_valor_eletronico_autorizado_download(Common.SerializableClasses.CTe.ClsXmlGTVeNT202402.proc GTVe)
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
                for (Lcont = 0; Lcont <= GTVe.GTVeProc.GTVe.infCte.autXML.Count - 1; Lcont++)
                {
                    strSQL_aux = string.Empty;
                    stbSQL.Clear();
                    // Montando query a ser executada

                    // Montando query a ser executada
                    stbSQL.Append(clsFacil.MontarQuery("INSERT INTO cte.detalhe_xml_guia_transporte_valor_eletronico_autorizado_download "));
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
                    clsFacil.AdicionarParametro(ref strSQL_aux, "20" + GTVe.GTVeProc.protCTe.InfProt.chCTe.Substring(2, 4), "@dtr_referencia", SqlDbType.Int);
                    clsFacil.AdicionarParametro(ref strSQL_aux, GTVe.GTVeProc.protCTe.InfProt.chCTe, "@cod_chave_acesso", SqlDbType.VarChar);

                    if (GTVe.GTVeProc.GTVe.infCte.autXML[Lcont].CPF != null)
                    {
                        clsFacil.AdicionarParametro(ref strSQL_aux, GTVe.GTVeProc.GTVe.infCte.autXML[Lcont].CPF.Substring(0, 9), "@num_cnpj_cpf_base_autorizado", SqlDbType.VarChar);
                        clsFacil.AdicionarParametro(ref strSQL_aux, "0", "@num_cnpj_cpf_filial_autorizado", SqlDbType.VarChar);
                        clsFacil.AdicionarParametro(ref strSQL_aux, GTVe.GTVeProc.GTVe.infCte.autXML[Lcont].CPF.Substring(9, 2), "@dig_cnpj_cpf_autorizado", SqlDbType.SmallInt);
                    }
                    if (GTVe.GTVeProc.GTVe.infCte.autXML[Lcont].CNPJ != null)
                    {
                        clsFacil.AdicionarParametro(ref strSQL_aux, GTVe.GTVeProc.GTVe.infCte.autXML[Lcont].CNPJ.Substring(0, 8), "@num_cnpj_cpf_base_autorizado", SqlDbType.VarChar);
                        clsFacil.AdicionarParametro(ref strSQL_aux, GTVe.GTVeProc.GTVe.infCte.autXML[Lcont].CNPJ.Substring(8, 4), "@num_cnpj_cpf_filial_autorizado", SqlDbType.VarChar);
                        clsFacil.AdicionarParametro(ref strSQL_aux, GTVe.GTVeProc.GTVe.infCte.autXML[Lcont].CNPJ.Substring(12, 2), "@dig_cnpj_cpf_autorizado", SqlDbType.SmallInt);
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

        public string InserirXML_detalhe_xml_guia_transporte_valor_eletronico_info_contribuinte(Common.SerializableClasses.CTe.ClsXmlGTVeNT202402.proc GTVe)
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
                for (Lcont = 0; Lcont <= GTVe.GTVeProc.GTVe.infCte.compl.ObsCont.Count - 1; Lcont++)
                {
                    strSQL_aux = string.Empty;
                    stbSQL.Clear();
                    // Montando query a ser executada
                    stbSQL.Append(clsFacil.MontarQuery("INSERT INTO cte.detalhe_xml_guia_transporte_valor_eletronico_info_contribuinte "));
                    stbSQL.Append(clsFacil.MontarQuery("( "));
                    stbSQL.Append(clsFacil.MontarQuery("  dtr_referencia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso, "));
                    stbSQL.Append(clsFacil.MontarQuery("  seq_detalhe_xml_guia_transporte_valor_eletronico_info_contribuinte, "));
                    stbSQL.Append(clsFacil.MontarQuery("  nom_campo_livre_contribuinte, "));
                    stbSQL.Append(clsFacil.MontarQuery("  des_campo_livre_contribuinte, "));
                    stbSQL.Append(clsFacil.MontarQuery("  dtc_atualizacao "));
                    stbSQL.Append(clsFacil.MontarQuery(") "));
                    stbSQL.Append(clsFacil.MontarQuery("VALUES "));
                    stbSQL.Append(clsFacil.MontarQuery("( "));
                    stbSQL.Append(clsFacil.MontarQuery("  @dtr_referencia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @seq_detalhe_xml_guia_transporte_valor_eletronico_info_contribuinte, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @nom_campo_livre_contribuinte, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @des_campo_livre_contribuinte, "));
                    stbSQL.Append(clsFacil.MontarQuery("  getdate() "));
                    stbSQL.Append(clsFacil.MontarQuery(") "));
                    strSQL_aux = stbSQL.ToString();

                    // Montando parametros
                    clsDados.LimparParametro();
                    clsFacil.AdicionarParametro(ref strSQL_aux, "20" + GTVe.GTVeProc.protCTe.InfProt.chCTe.Substring(2, 4), "@dtr_referencia", SqlDbType.Int);
                    clsFacil.AdicionarParametro(ref strSQL_aux, GTVe.GTVeProc.protCTe.InfProt.chCTe, "@cod_chave_acesso", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, Convert.ToString(Lcont + 1), "@seq_detalhe_xml_guia_transporte_valor_eletronico_info_contribuinte", SqlDbType.SmallInt);
                    clsFacil.AdicionarParametro(ref strSQL_aux, GTVe.GTVeProc.GTVe.infCte.compl.ObsCont[Lcont].xCampo, "@nom_campo_livre_contribuinte", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, GTVe.GTVeProc.GTVe.infCte.compl.ObsCont[Lcont].xTexto, "@des_campo_livre_contribuinte", SqlDbType.VarChar);
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

        public string InserirXML_detalhe_xml_guia_transporte_valor_eletronico_info_fisco(Common.SerializableClasses.CTe.ClsXmlGTVeNT202402.proc GTVe)
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
                for (Lcont = 0; Lcont <= GTVe.GTVeProc.GTVe.infCte.compl.ObsFisco.Count - 1; Lcont++)
                {
                    strSQL_aux = string.Empty;
                    stbSQL.Clear();
                    // Montando query a ser executada
                    stbSQL.Append(clsFacil.MontarQuery("INSERT INTO cte.detalhe_xml_guia_transporte_valor_eletronico_info_fisco "));
                    stbSQL.Append(clsFacil.MontarQuery("( "));
                    stbSQL.Append(clsFacil.MontarQuery("  dtr_referencia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso, "));
                    stbSQL.Append(clsFacil.MontarQuery("  seq_detalhe_xml_guia_transporte_valor_eletronico_info_fisco, "));
                    stbSQL.Append(clsFacil.MontarQuery("  nom_campo_livre_fisco, "));
                    stbSQL.Append(clsFacil.MontarQuery("  des_campo_livre_fisco, "));
                    stbSQL.Append(clsFacil.MontarQuery("  dtc_atualizacao "));
                    stbSQL.Append(clsFacil.MontarQuery(") "));
                    stbSQL.Append(clsFacil.MontarQuery("VALUES "));
                    stbSQL.Append(clsFacil.MontarQuery("( "));
                    stbSQL.Append(clsFacil.MontarQuery("  @dtr_referencia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @seq_detalhe_xml_guia_transporte_valor_eletronico_info_fisco, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @nom_campo_livre_fisco, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @des_campo_livre_fisco, "));
                    stbSQL.Append(clsFacil.MontarQuery("  getdate() "));
                    stbSQL.Append(clsFacil.MontarQuery(") "));
                    strSQL_aux = stbSQL.ToString();

                    // Montando parametros
                    clsDados.LimparParametro();
                    clsFacil.AdicionarParametro(ref strSQL_aux, "20" + GTVe.GTVeProc.protCTe.InfProt.chCTe.Substring(2, 4), "@dtr_referencia", SqlDbType.Int);
                    clsFacil.AdicionarParametro(ref strSQL_aux, GTVe.GTVeProc.protCTe.InfProt.chCTe, "@cod_chave_acesso", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, Convert.ToString(Lcont + 1), "@seq_detalhe_xml_guia_transporte_valor_eletronico_info_fisco", SqlDbType.SmallInt);
                    clsFacil.AdicionarParametro(ref strSQL_aux, GTVe.GTVeProc.GTVe.infCte.compl.ObsFisco[Lcont].xCampo, "@nom_campo_livre_fisco", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, GTVe.GTVeProc.GTVe.infCte.compl.ObsFisco[Lcont].xTexto, "@des_campo_livre_fisco", SqlDbType.VarChar);

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

        public string InserirXML_detalhe_xml_guia_transporte_valor_eletronico_grupo_informacao_detalhada(Common.SerializableClasses.CTe.ClsXmlGTVeNT202402.proc GTVe)
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
                stbSQL.Append(clsFacil.MontarQuery("INSERT INTO cte.detalhe_xml_guia_transporte_valor_eletronico_grupo_informacao_detalhada "));
                stbSQL.Append(clsFacil.MontarQuery("( "));
                stbSQL.Append(clsFacil.MontarQuery("  dtr_referencia, "));
                stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso, "));                
                stbSQL.Append(clsFacil.MontarQuery("  qtd_volume, "));
                stbSQL.Append(clsFacil.MontarQuery("  dtc_atualizacao "));
                stbSQL.Append(clsFacil.MontarQuery(") "));
                stbSQL.Append(clsFacil.MontarQuery("VALUES "));
                stbSQL.Append(clsFacil.MontarQuery("( "));
                stbSQL.Append(clsFacil.MontarQuery("  @dtr_referencia, "));
                stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso, "));                
                stbSQL.Append(clsFacil.MontarQuery("  @qtd_volume, "));
                stbSQL.Append(clsFacil.MontarQuery("  getdate() "));
                stbSQL.Append(clsFacil.MontarQuery(") "));
                string strSQL = stbSQL.ToString();

                // Montando parametros
                clsDados.LimparParametro();
                clsFacil.AdicionarParametro(ref strSQL, "20" + GTVe.GTVeProc.protCTe.InfProt.chCTe.Substring(2, 4), "@dtr_referencia", SqlDbType.Int);
                clsFacil.AdicionarParametro(ref strSQL, GTVe.GTVeProc.protCTe.InfProt.chCTe, "@cod_chave_acesso", SqlDbType.VarChar);                
                clsFacil.AdicionarParametro(ref strSQL, Convert.ToString(GTVe.GTVeProc.GTVe.infCte.detGTV.qCarga), "@qtd_volume", SqlDbType.Decimal);

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

        public string InserirXML_detalhe_xml_guia_transporte_valor_eletronico_especie_transportada(Common.SerializableClasses.CTe.ClsXmlGTVeNT202402.proc GTVe)
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
                for (Lcont = 0; Lcont <= GTVe.GTVeProc.GTVe.infCte.detGTV.infEspecie.Count - 1; Lcont++)
                {
                    strSQL_aux = string.Empty;
                    stbSQL.Clear();
                    // Montando query a ser executada
                    stbSQL.Append(clsFacil.MontarQuery("INSERT INTO cte.detalhe_xml_guia_transporte_valor_eletronico_especie_transportada "));
                    stbSQL.Append(clsFacil.MontarQuery("( "));
                    stbSQL.Append(clsFacil.MontarQuery("  dtr_referencia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso, "));
                    stbSQL.Append(clsFacil.MontarQuery("  seq_detalhe_xml_guia_transporte_valor_eletronico_especie_transportada, "));
                    stbSQL.Append(clsFacil.MontarQuery("  cod_tipo_especie, "));
                    stbSQL.Append(clsFacil.MontarQuery("  val_especie, "));
                    stbSQL.Append(clsFacil.MontarQuery("  cod_tipo_nacionalidade_numerario, "));
                    stbSQL.Append(clsFacil.MontarQuery("  nom_moeda, "));
                    stbSQL.Append(clsFacil.MontarQuery("  dtc_atualizacao "));
                    stbSQL.Append(clsFacil.MontarQuery(") "));
                    stbSQL.Append(clsFacil.MontarQuery("VALUES "));
                    stbSQL.Append(clsFacil.MontarQuery("( "));
                    stbSQL.Append(clsFacil.MontarQuery("  @dtr_referencia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @seq_detalhe_xml_guia_transporte_valor_eletronico_especie_transportada, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @cod_tipo_especie, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @val_especie, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @cod_tipo_nacionalidade_numerario, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @nom_moeda, "));
                    stbSQL.Append(clsFacil.MontarQuery("  getdate() "));
                    stbSQL.Append(clsFacil.MontarQuery(") "));
                    strSQL_aux = stbSQL.ToString();

                    // Montando parametros
                    clsDados.LimparParametro();
                    clsFacil.AdicionarParametro(ref strSQL_aux, "20" + GTVe.GTVeProc.protCTe.InfProt.chCTe.Substring(2, 4), "@dtr_referencia", SqlDbType.Int);
                    clsFacil.AdicionarParametro(ref strSQL_aux, GTVe.GTVeProc.protCTe.InfProt.chCTe, "@cod_chave_acesso", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, Convert.ToString(Lcont + 1), "@seq_detalhe_xml_guia_transporte_valor_eletronico_especie_transportada", SqlDbType.SmallInt);
                    clsFacil.AdicionarParametro(ref strSQL_aux, GTVe.GTVeProc.GTVe.infCte.detGTV.infEspecie[Lcont].tpEspecie, "@cod_tipo_especie", SqlDbType.SmallInt);
                    clsFacil.AdicionarParametro(ref strSQL_aux, Convert.ToString(GTVe.GTVeProc.GTVe.infCte.detGTV.infEspecie[Lcont].vEspecie), "@val_especie", SqlDbType.Decimal);
                    clsFacil.AdicionarParametro(ref strSQL_aux, GTVe.GTVeProc.GTVe.infCte.detGTV.infEspecie[Lcont].tpNumerario, "@cod_tipo_nacionalidade_numerario", SqlDbType.SmallInt);
                    clsFacil.AdicionarParametro(ref strSQL_aux, GTVe.GTVeProc.GTVe.infCte.detGTV.infEspecie[Lcont].xMoedaEstr, "@nom_moeda", SqlDbType.VarChar);
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

        public string InserirXML_detalhe_xml_guia_transporte_valor_eletronico_veiculo(Common.SerializableClasses.CTe.ClsXmlGTVeNT202402.proc GTVe)
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
                for (Lcont = 0; Lcont <= GTVe.GTVeProc.GTVe.infCte.detGTV.infVeiculo.Count - 1; Lcont++)
                {
                    strSQL_aux = string.Empty;
                    stbSQL.Clear();
                    // Montando query a ser executada
                    stbSQL.Append(clsFacil.MontarQuery("INSERT INTO cte.detalhe_xml_guia_transporte_valor_eletronico_veiculo "));
                    stbSQL.Append(clsFacil.MontarQuery("( "));
                    stbSQL.Append(clsFacil.MontarQuery("  dtr_referencia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  cod_chave_acesso, "));
                    stbSQL.Append(clsFacil.MontarQuery("  seq_detalhe_xml_guia_transporte_valor_eletronico_veiculo, "));
                    stbSQL.Append(clsFacil.MontarQuery("  num_placa, "));
                    stbSQL.Append(clsFacil.MontarQuery("  sig_unid_federacao_licenciamento, "));
                    stbSQL.Append(clsFacil.MontarQuery("  num_rntrc_transportador, "));
                    stbSQL.Append(clsFacil.MontarQuery("  dtc_atualizacao "));
                    stbSQL.Append(clsFacil.MontarQuery(") "));
                    stbSQL.Append(clsFacil.MontarQuery("VALUES "));
                    stbSQL.Append(clsFacil.MontarQuery("( "));
                    stbSQL.Append(clsFacil.MontarQuery("  @dtr_referencia, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @cod_chave_acesso, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @seq_detalhe_xml_guia_transporte_valor_eletronico_veiculo, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @num_placa, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @sig_unid_federacao_licenciamento, "));
                    stbSQL.Append(clsFacil.MontarQuery("  @num_rntrc_transportador, "));
                    stbSQL.Append(clsFacil.MontarQuery("  getdate() "));
                    stbSQL.Append(clsFacil.MontarQuery(") "));
                    strSQL_aux = stbSQL.ToString();

                    // Montando parametros
                    clsDados.LimparParametro();
                    clsFacil.AdicionarParametro(ref strSQL_aux, "20" + GTVe.GTVeProc.protCTe.InfProt.chCTe.Substring(2, 4), "@dtr_referencia", SqlDbType.Int);
                    clsFacil.AdicionarParametro(ref strSQL_aux, GTVe.GTVeProc.protCTe.InfProt.chCTe, "@cod_chave_acesso", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, Convert.ToString(Lcont + 1), "@seq_detalhe_xml_guia_transporte_valor_eletronico_veiculo", SqlDbType.SmallInt);
                    clsFacil.AdicionarParametro(ref strSQL_aux, GTVe.GTVeProc.GTVe.infCte.detGTV.infVeiculo[Lcont].placa, "@num_placa", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, GTVe.GTVeProc.GTVe.infCte.detGTV.infVeiculo[Lcont].UF, "@sig_unid_federacao_licenciamento", SqlDbType.VarChar);
                    clsFacil.AdicionarParametro(ref strSQL_aux, GTVe.GTVeProc.GTVe.infCte.detGTV.infVeiculo[Lcont].RNTRC, "@num_rntrc_transportador", SqlDbType.VarChar);
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

        public string InserirXML(Common.SerializableClasses.CTe.ClsXmlGTVeNT202402.proc GTVe)
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
