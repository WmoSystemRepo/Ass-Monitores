using System;
using System.Text;
using System.Xml;

namespace DFe
{
    class NegCTe
    {
        #region " Variaveis "

        // Classes utilizadas
        private Facilitador clsFacil;

        #endregion

        #region " Construtores "

        public NegCTe(Facilitador clsFacilPar)
        {
            // Inicializando classes
            clsFacil = clsFacilPar;
        }

        #endregion

        #region " MontarXMLDistribuicao "

        public XmlDocument MontarXMLDistribuicao(string strNSU, short intTipoConsulta, short intTipoRetorno, short intTipoAmbiente, string strVersaoDados, string strUF)
        {
            // Classes e variaveis utilizadas
            StringBuilder stbXML = new StringBuilder();
            XmlDocument xmlRetorno = new XmlDocument();

            // Montando o XML de envio
            stbXML.Append(Constante.CabecalhoXML);
            stbXML.Append("<distCTeSVD versao=\"" + strVersaoDados + "\" xmlns =\"" + Constante.NamespacePadraoCTe + "\">");
            stbXML.Append(clsFacil.MontarTagXML("tpAmb", intTipoAmbiente, false, Constante.TipoFormatData.Nenhuma));
            stbXML.Append(clsFacil.MontarTagXML("cOrgao", strUF, false, Constante.TipoFormatData.Nenhuma));
            stbXML.Append(clsFacil.MontarTagXML("indDFe", intTipoConsulta, false, Constante.TipoFormatData.Nenhuma));
            stbXML.Append(clsFacil.MontarTagXML("indRetXML", intTipoRetorno, false, Constante.TipoFormatData.Nenhuma));
            stbXML.Append(clsFacil.MontarTagXML("ultNSU", strNSU.PadLeft(15, '0'), false, Constante.TipoFormatData.Nenhuma));
            stbXML.Append("</distCTeSVD>");

            // Retornando XML gerado
            xmlRetorno.PreserveWhitespace = true;
            xmlRetorno.LoadXml(stbXML.ToString());
            return xmlRetorno;
        }

        #endregion

        #region " MontarXMLConsulta "

        public XmlDocument MontarXMLConsulta(string strChaveAcesso, short intTipoAmbiente, string strTipoConsulta, string strVersaoDados)
        {
            // Classes e variaveis utilizadas
            StringBuilder stbXML = new StringBuilder();
            XmlDocument xmlRetorno = new XmlDocument();

            // Montando o XML de envio
            stbXML.Append(Constante.CabecalhoXML);
            stbXML.Append("<cteConsultaDFe versao=\"" + strVersaoDados + "\" xmlns =\"" + Constante.NamespacePadraoCTe + "\">");
            stbXML.Append(clsFacil.MontarTagXML("tpAmb", intTipoAmbiente, false, Constante.TipoFormatData.Nenhuma));
            stbXML.Append(clsFacil.MontarTagXML("xServ", strTipoConsulta, false, Constante.TipoFormatData.Nenhuma));
            stbXML.Append(clsFacil.MontarTagXML("chCTe", strChaveAcesso, false, Constante.TipoFormatData.Nenhuma));
            stbXML.Append("</cteConsultaDFe>");

            // Retornando XML gerado
            xmlRetorno.PreserveWhitespace = true;
            xmlRetorno.LoadXml(stbXML.ToString());
            return xmlRetorno;
        }

        #endregion

        #region " MontarXMLProc "

        public XmlDocument MontarXMLProc(string strXML, string strEsquema, string strNSU)
        {
            // Classes e variaveis utilizadas
            StringBuilder stbXML = new StringBuilder();
            XmlDocument xmlRetorno = new XmlDocument();

            // Montando o XML de envio
            stbXML.Append("<proc schema=\"" + strEsquema + "\" NSU =\"" + strNSU + "\">");
            stbXML.Append(strXML);
            stbXML.Append("</proc>");

            // Retornando XML gerado
            xmlRetorno.PreserveWhitespace = true;
            xmlRetorno.LoadXml(stbXML.ToString());
            return xmlRetorno;
        }

        #endregion

        #region " MontarXMLLote "

        public XmlDocument MontarXMLLote(string strXMLPedido, string strXMLResposta, string strVersao, string strEsquema, string strNSU, string strIP)
        {
            // Classes e variaveis utilizadas
            StringBuilder stbXML = new StringBuilder();
            XmlDocument xmlRetorno = new XmlDocument();

            // Montando o XML de envio
            stbXML.Append("<" + Constante.EsqCTeRetSVD + ">");
            stbXML.Append("<proc schema=\"" + strEsquema + "\" NSU =\"" + strNSU + "\">");

            // Verificando qual o esquema
            if (strEsquema.StartsWith(Constante.EsqCTeAutorizacaoSchema))
            {
                stbXML.Append("<" + Constante.EsqCTeAutorizacaoProc + " versao=\"" + strVersao + "\" xmlns =\"" + Constante.NamespacePadraoCTe + "\">"); //+ "\" ipTransmissor =\"" + strIP + "\">");
                stbXML.Append(strXMLPedido);
                stbXML.Append(strXMLResposta);
                stbXML.Append("</" + Constante.EsqCTeAutorizacaoProc + ">");
            }
            else if (strEsquema.StartsWith(Constante.EsqCTeEventoSchema))
            {
                stbXML.Append("<" + Constante.EsqCTeEventoProc + " versao=\"" + strVersao + "\" xmlns =\"" + Constante.NamespacePadraoCTe + "\">"); //+ "\" ipTransmissor =\"" + strIP + "\">");
                stbXML.Append(strXMLPedido);
                stbXML.Append(strXMLResposta);
                stbXML.Append("</" + Constante.EsqCTeEventoProc + ">");
            }

            stbXML.Append("</proc>");
            stbXML.Append("</" + Constante.EsqCTeRetSVD + ">");

            // Retornando XML gerado
            xmlRetorno.PreserveWhitespace = true;
            xmlRetorno.LoadXml(stbXML.ToString());
            return xmlRetorno;
        }

        #endregion
    }
}