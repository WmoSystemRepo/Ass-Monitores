using System.Xml;

namespace DFe
{
    class DocCTeInut
    {
        #region " Variaveis "

        // Classes utilizadas
        private Facilitador clsFacil;

        #endregion

        #region " Propriedades "

        public string XML { get; set; }
        public string XMLProc { get; set; }
        public string XMLEnvio { get; set; }
        public string XMLRetorno { get; set; }
        public string Chave { get; set; }
        public string NSU { get; set; }
        public string Versao { get; set; }
        public string Schema { get; set; }
        public string Protocolo { get; set; }
        public string Modelo { get; set; }
        public string CodUF { get; set; }
        public string Ano { get; set; }
        public string Serie { get; set; }
        public string FaixaInicial { get; set; }
        public string FaixaFinal { get; set; }
        public string CNPJ { get; set; }
        public string CPF { get; set; }
        public string CNPJCPFBase { get; set; }
        public string CNPJCPFFilial { get; set; }
        public string CNPJCPFDigito { get; set; }
        public string Justificativa { get; set; }
        public string DataAutorizacao { get; set; }
        public string DataConexao { get; set; }
        public string IP { get; set; }
        public string Porta { get; set; }
        public string Status { get; set; }
        public string Motivo { get; set; }

        #endregion

        #region " Construtores "

        public DocCTeInut(Facilitador clsFacilPar, XmlDocument xmlDocumento)
        {
            // Inicializando classes
            clsFacil = clsFacilPar;

            // Inicializando propriedades
            this.InicializarPropriedades();

            // Preenchendo propriedades
            this.PreencherPropriedades(xmlDocumento);
        }

        public DocCTeInut(Facilitador clsFacilPar, XmlNode xmlNo)
        {
            // Inicializando classes
            clsFacil = clsFacilPar;

            // Montando o XML
            XmlDocument xmlDocumento = new XmlDocument();
            xmlDocumento.LoadXml(xmlNo.OuterXml);

            // Inicializando propriedades
            this.InicializarPropriedades();

            // Preenchendo propriedades
            this.PreencherPropriedades(xmlDocumento);
        }

        public DocCTeInut(Facilitador clsFacilPar, string strXML)
        {
            // Inicializando classes
            clsFacil = clsFacilPar;

            // Montando o XML
            XmlDocument xmlDocumento = new XmlDocument();
            xmlDocumento.LoadXml(strXML);

            // Inicializando propriedades
            this.InicializarPropriedades();

            // Preenchendo propriedades
            this.PreencherPropriedades(xmlDocumento);
        }

        #endregion

        #region " InicializarPropriedades "

        private void InicializarPropriedades()
        {
            // Inicializando propriedades
            this.XML = string.Empty;
            this.XMLProc = string.Empty;
            this.XMLEnvio = string.Empty;
            this.XMLRetorno = string.Empty;
            this.Chave = string.Empty;
            this.NSU = string.Empty;
            this.Versao = string.Empty;
            this.Schema = string.Empty;
            this.Protocolo = string.Empty;
            this.Modelo = string.Empty;
            this.CodUF = string.Empty;
            this.Ano = string.Empty;
            this.Serie = string.Empty;
            this.FaixaInicial = string.Empty;
            this.FaixaFinal = string.Empty;
            this.CNPJ = string.Empty;
            this.CPF = string.Empty;
            this.CNPJCPFBase = string.Empty;
            this.CNPJCPFFilial = string.Empty;
            this.CNPJCPFDigito = string.Empty;
            this.Justificativa = string.Empty;
            this.DataAutorizacao = string.Empty;
            this.DataConexao = string.Empty;
            this.IP = string.Empty;
            this.Porta = string.Empty;
            this.Status = string.Empty;
            this.Motivo = string.Empty;
        }

        #endregion

        #region " PreencherPropriedades "

        private void PreencherPropriedades(XmlDocument xmlDocumento)
        {
            try
            {
                // Obtendo XML's
                XmlElement xmlProc = xmlDocumento.DocumentElement;
                XmlElement xmlInt = (XmlElement)xmlDocumento.GetElementsByTagName(Constante.EsqCTeInutilizacaoProc)[0];
                XmlElement xmlEnv = (XmlElement)xmlInt.GetElementsByTagName(Constante.EsqCTeInutilizacaoEnv)[0];
                XmlElement xmlRet = (XmlElement)xmlInt.GetElementsByTagName(Constante.EsqCTeInutilizacaoRet)[0];

                // Obtendo elementos do XML
                XmlElement infInutRet = (XmlElement)xmlRet.GetElementsByTagName("infInut")[0];
                XmlElement infInutEnv = (XmlElement)xmlEnv.GetElementsByTagName("infInut")[0];

                // Obtendo atributos do XML
                this.XML = xmlDocumento.OuterXml;
                this.XMLProc = xmlProc.OuterXml;
                this.XMLEnvio = xmlEnv.OuterXml;
                this.XMLRetorno = xmlRet.OuterXml;
                this.Versao = clsFacil.ObterAtributoXML(xmlRet.Attributes["versao"]);
                this.NSU = clsFacil.ObterAtributoXML(xmlProc.Attributes["NSU"]);
                this.Schema = clsFacil.ObterAtributoXML(xmlProc.Attributes["schema"]);
                this.IP = clsFacil.ObterAtributoXML(xmlInt.Attributes["ipTransmissor"]);
                this.Porta = clsFacil.ObterAtributoXML(xmlInt.Attributes["nPortaCon"]);
                this.DataConexao = clsFacil.ObterAtributoXML(xmlInt.Attributes["dhConexao"]);

                // Preenchendo propiedades com dados do XML
                this.PreencherPropriedadesRetorno(infInutRet);
                this.PreencherPropriedadesEnvio(infInutEnv);
                this.PreencherPropriedadesCNPJCPF();
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region " PreencherPropriedadesRetorno "

        private void PreencherPropriedadesRetorno(XmlElement infInutRet)
        {
            // Obtendo dados do retorno no XML
            if (infInutRet != null)
            {
                this.Protocolo = clsFacil.ObterItemXML(infInutRet["nProt"]);
                this.Status = clsFacil.ObterItemXML(infInutRet["cStat"]);
                this.Motivo = clsFacil.ObterItemXML(infInutRet["xMotivo"]);
                this.DataAutorizacao = clsFacil.ObterItemXML(infInutRet["dhRecbto"]);
            }
        }

        #endregion

        #region " PreencherPropriedadesEnvio "

        private void PreencherPropriedadesEnvio(XmlElement infInutEnv)
        {
            // Obtendo dados do envio no XML
            if (infInutEnv != null)
            {
                this.Modelo = clsFacil.ObterItemXML(infInutEnv["mod"]);
                this.CodUF = clsFacil.ObterItemXML(infInutEnv["cUF"]);
                this.Ano = clsFacil.ObterItemXML(infInutEnv["ano"]);
                this.Serie = clsFacil.ObterItemXML(infInutEnv["serie"]);
                this.FaixaInicial = clsFacil.ObterItemXML(infInutEnv["nCTIni"]);
                this.FaixaFinal = clsFacil.ObterItemXML(infInutEnv["nCTFin"]);
                this.CNPJ = clsFacil.ObterItemXML(infInutEnv["CNPJ"]);
                this.CPF = clsFacil.ObterItemXML(infInutEnv["CPF"]);
                this.Justificativa = clsFacil.ObterItemXML(infInutEnv["xJust"]);

                // Verificando se o Ano tem 4 digitos
                if ((this.Ano != string.Empty) && (this.Ano.Length != 4))
                {
                    this.Ano = (2000 + System.Convert.ToInt32(this.Ano)).ToString();
                }
            }
        }

        #endregion

        #region " PreencherPropriedadesCNPJCPF "

        private void PreencherPropriedadesCNPJCPF()
        {
            // Destrinchando CNPJ/CPF do emitente
            if (this.CNPJ != string.Empty)
            {
                this.CNPJCPFBase = clsFacil.ObterCNPJBase(this.CNPJ);
                this.CNPJCPFFilial = clsFacil.ObterCNPJFilial(this.CNPJ);
                this.CNPJCPFDigito = clsFacil.ObterCNPJDigito(this.CNPJ);
            }
            else if (this.CPF != string.Empty)
            {
                this.CNPJCPFBase = clsFacil.ObterCPFBase(this.CPF);
                this.CNPJCPFFilial = clsFacil.ObterCPFFilial(this.CPF);
                this.CNPJCPFDigito = clsFacil.ObterCPFDigito(this.CPF);
            }

            this.Chave = this.Ano + ";" + this.Serie + ";" + this.FaixaInicial + ";" + this.FaixaFinal + ";" + this.CNPJCPFBase + ";" + this.CNPJCPFFilial + ";" + this.CNPJCPFDigito;
        }

        #endregion
    }
}