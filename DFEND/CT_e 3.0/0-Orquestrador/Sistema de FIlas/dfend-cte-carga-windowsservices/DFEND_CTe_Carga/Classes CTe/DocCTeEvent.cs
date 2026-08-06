using System.Xml;

namespace DFe
{
    class DocCTeEvent
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
        public string DataReferencia { get; set; }
        public string Chave { get; set; }
        public string ChaveAcesso { get; set; }
        public string ChaveAcessoCodUF { get; set; }
        public string TipoEvento { get; set; }
        public string SeqEvento { get; set; }
        public string DescEvento { get; set; }
        public string NSU { get; set; }
        public string Versao { get; set; }
        public string VersaoEvento { get; set; }
        public string Schema { get; set; }
        public string Protocolo { get; set; }
        public string Modelo { get; set; }
        public string CodUF { get; set; }
        public string CNPJ { get; set; }
        public string CPF { get; set; }
        public string CNPJCPFBase { get; set; }
        public string CNPJCPFFilial { get; set; }
        public string CNPJCPFDigito { get; set; }
        public string TipoAutor { get; set; }
        public string OrgaoAutor { get; set; }
        public string OrgaoReceptor { get; set; }
        public string DataEvento { get; set; }
        public string DataAutorizacao { get; set; }
        public string DataConexao { get; set; }
        public string IP { get; set; }
        public string Porta { get; set; }
        public string Status { get; set; }
        public string Motivo { get; set; }

        #endregion

        #region " Construtores "

        public DocCTeEvent(Facilitador clsFacilPar, XmlDocument xmlDocumento)
        {
            // Inicializando classes
            clsFacil = clsFacilPar;

            // Inicializando propriedades
            this.InicializarPropriedades();

            // Preenchendo propriedades
            this.PreencherPropriedades(xmlDocumento);
        }

        public DocCTeEvent(Facilitador clsFacilPar, XmlNode xmlNo)
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

        public DocCTeEvent(Facilitador clsFacilPar, string strXML)
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
            this.DataReferencia = string.Empty;
            this.Chave = string.Empty;
            this.ChaveAcesso = string.Empty;
            this.ChaveAcessoCodUF = string.Empty;
            this.TipoEvento = string.Empty;
            this.SeqEvento = string.Empty;
            this.DescEvento = string.Empty;
            this.NSU = string.Empty;
            this.Versao = string.Empty;
            this.VersaoEvento = string.Empty;
            this.Schema = string.Empty;
            this.Protocolo = string.Empty;
            this.Modelo = string.Empty;
            this.CodUF = string.Empty;
            this.CNPJ = string.Empty;
            this.CPF = string.Empty;
            this.CNPJCPFBase = string.Empty;
            this.CNPJCPFFilial = string.Empty;
            this.CNPJCPFDigito = string.Empty;
            this.TipoAutor = string.Empty;
            this.OrgaoAutor = string.Empty;
            this.OrgaoReceptor = string.Empty;
            this.DataEvento = string.Empty;
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
                XmlElement xmlInt = (XmlElement)xmlDocumento.GetElementsByTagName(Constante.EsqCTeEventoProc)[0];
                XmlElement xmlEnv = (XmlElement)xmlInt.GetElementsByTagName(Constante.EsqCTeEventoEnv)[0];
                XmlElement xmlRet = (XmlElement)xmlInt.GetElementsByTagName(Constante.EsqCTeEventoRet)[0];

                if (xmlEnv == null)
                {
                    xmlEnv = xmlProc;
                }

                // Obtendo elementos do XML
                XmlElement infEventoRet = (XmlElement)xmlRet.GetElementsByTagName("infEvento")[0];
                XmlElement infEventoEnv = (XmlElement)xmlEnv.GetElementsByTagName("infEvento")[0];
                XmlElement detEvento = (XmlElement)xmlEnv.GetElementsByTagName("detEvento")[0];

                // Obtendo atributos do XML
                this.XML = xmlDocumento.OuterXml;
                this.XMLProc = xmlProc.OuterXml;
                this.XMLEnvio = xmlEnv.OuterXml;
                this.XMLRetorno = xmlRet.OuterXml;
                this.Versao = clsFacil.ObterAtributoXML(xmlRet.Attributes["versao"]);
                this.NSU = clsFacil.ObterAtributoXML(xmlProc.Attributes["NSUSVD"]);
                this.Schema = clsFacil.ObterAtributoXML(xmlProc.Attributes["schema"]);
                this.IP = clsFacil.ObterAtributoXML(xmlInt.Attributes["ipTransmissor"]);
                this.Porta = clsFacil.ObterAtributoXML(xmlInt.Attributes["nPortaCon"]);
                this.DataConexao = clsFacil.ObterAtributoXML(xmlInt.Attributes["dhConexao"]);

                // Preenchendo propiedades com dados do XML
                this.PreencherPropriedadesRetorno(infEventoRet);
                this.PreencherPropriedadesEnvio(infEventoEnv);
                this.PreencherPropriedadesDetalhe(detEvento);
                this.PreencherPropriedadesCNPJCPF();

                // Obtendo o codigo da UF
                this.ChaveAcessoCodUF = this.ChaveAcesso.Substring(0, 2);
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region " PreencherPropriedadesRetorno "

        private void PreencherPropriedadesRetorno(XmlElement infEventoRet)
        {
            // Obtendo dados do retorno no XML
            if (infEventoRet != null)
            {
                this.DescEvento = clsFacil.ObterItemXML(infEventoRet["xEvento"]);
                this.Protocolo = clsFacil.ObterItemXML(infEventoRet["nProt"]);
                this.OrgaoReceptor = clsFacil.ObterItemXML(infEventoRet["cOrgao"]);
                this.Status = clsFacil.ObterItemXML(infEventoRet["cStat"]);
                this.Motivo = clsFacil.ObterItemXML(infEventoRet["xMotivo"]);
                this.DataAutorizacao = clsFacil.ObterItemXML(infEventoRet["dhRegEvento"]);
            }
        }

        #endregion

        #region " PreencherPropriedadesEnvio "

        private void PreencherPropriedadesEnvio(XmlElement infEventoEnv)
        {
            // Obtendo dados do envio no XML
            if (infEventoEnv != null)
            {
                this.ChaveAcesso = clsFacil.ObterItemXML(infEventoEnv["chCTe"]);
                this.TipoEvento = clsFacil.ObterItemXML(infEventoEnv["tpEvento"]);
                this.SeqEvento = clsFacil.ObterItemXML(infEventoEnv["nSeqEvento"]);
                this.VersaoEvento = clsFacil.ObterItemXML(infEventoEnv["verEvento"]);
                this.DataEvento = clsFacil.ObterItemXML(infEventoEnv["dhEvento"]);
                this.OrgaoAutor = clsFacil.ObterItemXML(infEventoEnv["cOrgao"]);
                this.CNPJ = clsFacil.ObterItemXML(infEventoEnv["CNPJ"]);
                this.CPF = clsFacil.ObterItemXML(infEventoEnv["CPF"]);
                this.Modelo = this.ChaveAcesso.Substring(20, 2);
                this.CodUF = this.ChaveAcesso.Substring(0, 2);
                this.DataReferencia = clsFacil.ObterDataReferencia(this.ChaveAcesso);
                this.Chave = this.ChaveAcesso + ";" + this.SeqEvento + ";" + this.TipoEvento;
            }
        }

        #endregion

        #region " PreencherPropriedadesDetalhe "

        private void PreencherPropriedadesDetalhe(XmlElement detEvento)
        {
            // Obtendo dados do envio no XML
            if (detEvento != null)
            {
                //this.DescEvento = clsFacil.ObterItemXML(detEvento["descEvento"]);
                this.TipoAutor = clsFacil.ObterItemXML(detEvento["tpAutor"]);
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
        }

        #endregion
    }
}