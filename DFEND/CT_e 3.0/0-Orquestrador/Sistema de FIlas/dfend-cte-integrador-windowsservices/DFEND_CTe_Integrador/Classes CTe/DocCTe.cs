using System.Xml;

namespace DFe
{
    class DocCTe
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
        public string NSU { get; set; }
        public string Versao { get; set; }
        public string Schema { get; set; }
        public string Protocolo { get; set; }
        public string Modelo { get; set; }
        public string Serie { get; set; }
        public string Numero { get; set; }
        public string DigitoVerificador { get; set; }
        public string Codigo { get; set; }
        public string UFCod { get; set; }
        public string UFEnv { get; set; }
        public string MunicipioEnv { get; set; }
        public string MunicipioEnvCod { get; set; }
        public string UFIni { get; set; }
        public string MunicipioIni { get; set; }
        public string MunicipioIniCod { get; set; }
        public string UFFim { get; set; }
        public string MunicipioFim { get; set; }
        public string MunicipioFimCod { get; set; }
        public string CFOP { get; set; }
        public string Modal { get; set; }
        public string NatOperacao { get; set; }
        public string TipoCTe { get; set; }
        public string TipoImpressao { get; set; }
        public string TipoEmissao { get; set; }
        public string TipoProcEmissao { get; set; }
        public string TipoServico { get; set; }
        public string TipoGlobalizado { get; set; }
        public string TipoFormaPagto { get; set; }
        public string VersaoAplic { get; set; }
        public string JustContingencia { get; set; }
        public string DataContingencia { get; set; }
        public string TomaIndicador { get; set; }
        public string TomaCod { get; set; }
        public string TomaCNPJ { get; set; }
        public string TomaCPF { get; set; }
        public string TomaCNPJCPFBase { get; set; }
        public string TomaCNPJCPFFilial { get; set; }
        public string TomaCNPJCPFDigito { get; set; }
        public string TomaIE { get; set; }
        public string TomaNome { get; set; }
        public string TomaFantasia { get; set; }
        public string TomaEndUF { get; set; }
        public string TomaEndCEP { get; set; }
        public string TomaEndNumero { get; set; }
        public string TomaEndLogradouro { get; set; }
        public string TomaEndComplemento { get; set; }
        public string TomaEndBairro { get; set; }
        public string TomaEndMunicipio { get; set; }
        public string TomaEndMunicipioCod { get; set; }
        public string TomaEndPais { get; set; }
        public string TomaEndPaisCod { get; set; }
        public string TomaFone { get; set; }
        public string TomaEmail { get; set; }
        public string EmitCNPJ { get; set; }
        public string EmitCPF { get; set; }
        public string EmitCNPJCPFBase { get; set; }
        public string EmitCNPJCPFFilial { get; set; }
        public string EmitCNPJCPFDigito { get; set; }
        public string EmitIE { get; set; }
        public string EmitIEST { get; set; }
        public string EmitNome { get; set; }
        public string EmitFantasia { get; set; }
        public string EmitEndUF { get; set; }
        public string EmitEndCEP { get; set; }
        public string EmitEndNumero { get; set; }
        public string EmitEndLogradouro { get; set; }
        public string EmitEndComplemento { get; set; }
        public string EmitEndBairro { get; set; }
        public string EmitEndMunicipio { get; set; }
        public string EmitEndMunicipioCod { get; set; }
        public string EmitEndPais { get; set; }
        public string EmitEndPaisCod { get; set; }
        public string EmitFone { get; set; }
        public string RemeCNPJ { get; set; }
        public string RemeCPF { get; set; }
        public string RemeCNPJCPFBase { get; set; }
        public string RemeCNPJCPFFilial { get; set; }
        public string RemeCNPJCPFDigito { get; set; }
        public string RemeIE { get; set; }
        public string RemeNome { get; set; }
        public string RemeFantasia { get; set; }
        public string RemeEndUF { get; set; }
        public string RemeEndCEP { get; set; }
        public string RemeEndNumero { get; set; }
        public string RemeEndLogradouro { get; set; }
        public string RemeEndComplemento { get; set; }
        public string RemeEndBairro { get; set; }
        public string RemeEndMunicipio { get; set; }
        public string RemeEndMunicipioCod { get; set; }
        public string RemeEndPais { get; set; }
        public string RemeEndPaisCod { get; set; }
        public string RemeFone { get; set; }
        public string RemeEmail { get; set; }
        public string ExpeCNPJ { get; set; }
        public string ExpeCPF { get; set; }
        public string ExpeCNPJCPFBase { get; set; }
        public string ExpeCNPJCPFFilial { get; set; }
        public string ExpeCNPJCPFDigito { get; set; }
        public string ExpeIE { get; set; }
        public string ExpeNome { get; set; }
        public string ExpeEndUF { get; set; }
        public string ExpeEndCEP { get; set; }
        public string ExpeEndNumero { get; set; }
        public string ExpeEndLogradouro { get; set; }
        public string ExpeEndComplemento { get; set; }
        public string ExpeEndBairro { get; set; }
        public string ExpeEndMunicipio { get; set; }
        public string ExpeEndMunicipioCod { get; set; }
        public string ExpeEndPais { get; set; }
        public string ExpeEndPaisCod { get; set; }
        public string ExpeFone { get; set; }
        public string ExpeEmail { get; set; }
        public string ReceCNPJ { get; set; }
        public string ReceCPF { get; set; }
        public string ReceCNPJCPFBase { get; set; }
        public string ReceCNPJCPFFilial { get; set; }
        public string ReceCNPJCPFDigito { get; set; }
        public string ReceIE { get; set; }
        public string ReceNome { get; set; }
        public string ReceEndUF { get; set; }
        public string ReceEndCEP { get; set; }
        public string ReceEndNumero { get; set; }
        public string ReceEndLogradouro { get; set; }
        public string ReceEndComplemento { get; set; }
        public string ReceEndBairro { get; set; }
        public string ReceEndMunicipio { get; set; }
        public string ReceEndMunicipioCod { get; set; }
        public string ReceEndPais { get; set; }
        public string ReceEndPaisCod { get; set; }
        public string ReceFone { get; set; }
        public string ReceEmail { get; set; }
        public string DestCNPJ { get; set; }
        public string DestCPF { get; set; }
        public string DestCNPJCPFBase { get; set; }
        public string DestCNPJCPFFilial { get; set; }
        public string DestCNPJCPFDigito { get; set; }
        public string DestIE { get; set; }
        public string DestNome { get; set; }
        public string DestEndUF { get; set; }
        public string DestEndCEP { get; set; }
        public string DestEndNumero { get; set; }
        public string DestEndLogradouro { get; set; }
        public string DestEndComplemento { get; set; }
        public string DestEndBairro { get; set; }
        public string DestEndMunicipio { get; set; }
        public string DestEndMunicipioCod { get; set; }
        public string DestEndPais { get; set; }
        public string DestEndPaisCod { get; set; }
        public string DestFone { get; set; }
        public string DestEmail { get; set; }
        public string ValorTotal { get; set; }
        public string ValorReceber { get; set; }
        public string ImpostoCST { get; set; }
        public string ImpostoICMS { get; set; }
        public string ImpostoValorICMS { get; set; }
        public string ImpostoValorICMSPerc { get; set; }
        public string ImpostoValorBC { get; set; }
        public string ImpostoValorBCPerc { get; set; }
        public string ImpostoValorCredito { get; set; }
        public string ImpostoValorTotal { get; set; }
        public string DigestValue { get; set; }
        public string DataEmissao { get; set; }
        public string DataAutorizacao { get; set; }
        public string DataConexao { get; set; }
        public string IP { get; set; }
        public string Porta { get; set; }
        public string Status { get; set; }
        public string Motivo { get; set; }
        public string QtdeNFes { get; set; }

        #endregion

        #region " Construtores "

        public DocCTe(Facilitador clsFacilPar, XmlDocument xmlDocumento)
        {
            // Inicializando classes
            clsFacil = clsFacilPar;

            // Inicializando propriedades
            this.InicializarPropriedades();

            // Preenchendo propriedades
            this.PreencherPropriedades(xmlDocumento);
        }

        public DocCTe(Facilitador clsFacilPar, XmlNode xmlNo)
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

        public DocCTe(Facilitador clsFacilPar, string strXML)
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
            this.NSU = string.Empty;
            this.Versao = string.Empty;
            this.Schema = string.Empty;
            this.Protocolo = string.Empty;
            this.Modelo = string.Empty;
            this.Serie = string.Empty;
            this.Numero = string.Empty;
            this.DigitoVerificador = string.Empty;
            this.Codigo = string.Empty;
            this.UFCod = string.Empty;
            this.UFEnv = string.Empty;
            this.MunicipioEnv = string.Empty;
            this.MunicipioEnvCod = string.Empty;
            this.UFIni = string.Empty;
            this.MunicipioIni = string.Empty;
            this.MunicipioIniCod = string.Empty;
            this.UFFim = string.Empty;
            this.MunicipioFim = string.Empty;
            this.MunicipioFimCod = string.Empty;
            this.CFOP = string.Empty;
            this.Modal = string.Empty;
            this.NatOperacao = string.Empty;
            this.TipoCTe = string.Empty;
            this.TipoImpressao = string.Empty;
            this.TipoEmissao = string.Empty;
            this.TipoProcEmissao = string.Empty;
            this.TipoServico = string.Empty;
            this.TipoGlobalizado = string.Empty;
            this.TipoFormaPagto = string.Empty;
            this.VersaoAplic = string.Empty;
            this.JustContingencia = string.Empty;
            this.DataContingencia = string.Empty;
            this.TomaIndicador = string.Empty;
            this.TomaCod = string.Empty;
            this.TomaCNPJ = string.Empty;
            this.TomaCPF = string.Empty;
            this.TomaCNPJCPFBase = string.Empty;
            this.TomaCNPJCPFFilial = string.Empty;
            this.TomaCNPJCPFDigito = string.Empty;
            this.TomaIE = string.Empty;
            this.TomaNome = string.Empty;
            this.TomaFantasia = string.Empty;
            this.TomaEndUF = string.Empty;
            this.TomaEndCEP = string.Empty;
            this.TomaEndNumero = string.Empty;
            this.TomaEndLogradouro = string.Empty;
            this.TomaEndComplemento = string.Empty;
            this.TomaEndBairro = string.Empty;
            this.TomaEndMunicipio = string.Empty;
            this.TomaEndMunicipioCod = string.Empty;
            this.TomaEndPais = string.Empty;
            this.TomaEndPaisCod = string.Empty;
            this.TomaFone = string.Empty;
            this.TomaEmail = string.Empty;
            this.EmitCNPJ = string.Empty;
            this.EmitCPF = string.Empty;
            this.EmitCNPJCPFBase = string.Empty;
            this.EmitCNPJCPFFilial = string.Empty;
            this.EmitCNPJCPFDigito = string.Empty;
            this.EmitIE = string.Empty;
            this.EmitIEST = string.Empty;
            this.EmitNome = string.Empty;
            this.EmitFantasia = string.Empty;
            this.EmitEndUF = string.Empty;
            this.EmitEndCEP = string.Empty;
            this.EmitEndNumero = string.Empty;
            this.EmitEndLogradouro = string.Empty;
            this.EmitEndComplemento = string.Empty;
            this.EmitEndBairro = string.Empty;
            this.EmitEndMunicipio = string.Empty;
            this.EmitEndMunicipioCod = string.Empty;
            this.EmitEndPais = string.Empty;
            this.EmitEndPaisCod = string.Empty;
            this.EmitFone = string.Empty;
            this.RemeCNPJ = string.Empty;
            this.RemeCPF = string.Empty;
            this.RemeCNPJCPFBase = string.Empty;
            this.RemeCNPJCPFFilial = string.Empty;
            this.RemeCNPJCPFDigito = string.Empty;
            this.RemeIE = string.Empty;
            this.RemeNome = string.Empty;
            this.RemeFantasia = string.Empty;
            this.RemeEndUF = string.Empty;
            this.RemeEndCEP = string.Empty;
            this.RemeEndNumero = string.Empty;
            this.RemeEndLogradouro = string.Empty;
            this.RemeEndComplemento = string.Empty;
            this.RemeEndBairro = string.Empty;
            this.RemeEndMunicipio = string.Empty;
            this.RemeEndMunicipioCod = string.Empty;
            this.RemeEndPais = string.Empty;
            this.RemeEndPaisCod = string.Empty;
            this.RemeFone = string.Empty;
            this.RemeEmail = string.Empty;
            this.ExpeCNPJ = string.Empty;
            this.ExpeCPF = string.Empty;
            this.ExpeCNPJCPFBase = string.Empty;
            this.ExpeCNPJCPFFilial = string.Empty;
            this.ExpeCNPJCPFDigito = string.Empty;
            this.ExpeIE = string.Empty;
            this.ExpeNome = string.Empty;
            this.ExpeEndUF = string.Empty;
            this.ExpeEndCEP = string.Empty;
            this.ExpeEndNumero = string.Empty;
            this.ExpeEndLogradouro = string.Empty;
            this.ExpeEndComplemento = string.Empty;
            this.ExpeEndBairro = string.Empty;
            this.ExpeEndMunicipio = string.Empty;
            this.ExpeEndMunicipioCod = string.Empty;
            this.ExpeEndPais = string.Empty;
            this.ExpeEndPaisCod = string.Empty;
            this.ExpeFone = string.Empty;
            this.ExpeEmail = string.Empty;
            this.ReceCNPJ = string.Empty;
            this.ReceCPF = string.Empty;
            this.ReceCNPJCPFBase = string.Empty;
            this.ReceCNPJCPFFilial = string.Empty;
            this.ReceCNPJCPFDigito = string.Empty;
            this.ReceIE = string.Empty;
            this.ReceNome = string.Empty;
            this.ReceEndUF = string.Empty;
            this.ReceEndCEP = string.Empty;
            this.ReceEndNumero = string.Empty;
            this.ReceEndLogradouro = string.Empty;
            this.ReceEndComplemento = string.Empty;
            this.ReceEndBairro = string.Empty;
            this.ReceEndMunicipio = string.Empty;
            this.ReceEndMunicipioCod = string.Empty;
            this.ReceEndPais = string.Empty;
            this.ReceEndPaisCod = string.Empty;
            this.ReceFone = string.Empty;
            this.ReceEmail = string.Empty;
            this.DestCNPJ = string.Empty;
            this.DestCPF = string.Empty;
            this.DestCNPJCPFBase = string.Empty;
            this.DestCNPJCPFFilial = string.Empty;
            this.DestCNPJCPFDigito = string.Empty;
            this.DestIE = string.Empty;
            this.DestNome = string.Empty;
            this.DestEndUF = string.Empty;
            this.DestEndCEP = string.Empty;
            this.DestEndNumero = string.Empty;
            this.DestEndLogradouro = string.Empty;
            this.DestEndComplemento = string.Empty;
            this.DestEndBairro = string.Empty;
            this.DestEndMunicipio = string.Empty;
            this.DestEndMunicipioCod = string.Empty;
            this.DestEndPais = string.Empty;
            this.DestEndPaisCod = string.Empty;
            this.DestFone = string.Empty;
            this.DestEmail = string.Empty;
            this.ValorTotal = string.Empty;
            this.ValorReceber = string.Empty;
            this.ImpostoCST = string.Empty;
            this.ImpostoICMS = string.Empty;
            this.ImpostoValorICMS = string.Empty;
            this.ImpostoValorICMSPerc = string.Empty;
            this.ImpostoValorBC = string.Empty;
            this.ImpostoValorBCPerc = string.Empty;
            this.ImpostoValorCredito = string.Empty;
            this.ImpostoValorTotal = string.Empty;
            this.DigestValue = string.Empty;
            this.DataEmissao = string.Empty;
            this.DataAutorizacao = string.Empty;
            this.DataConexao = string.Empty;
            this.IP = string.Empty;
            this.Porta = string.Empty;
            this.Status = string.Empty;
            this.Motivo = string.Empty;
            this.QtdeNFes = "0";
        }

        #endregion

        #region " PreencherPropriedades "

        private void PreencherPropriedades(XmlDocument xmlDocumento)
        {
            try
            {
                // Obtendo XML's
                XmlElement xmlProc = xmlDocumento.DocumentElement;
                XmlElement xmlInt = xmlDocumento.DocumentElement;
                XmlElement xmlEnv = xmlDocumento.DocumentElement;
                XmlElement xmlRet = xmlDocumento.DocumentElement;

                // Verificando qual o schema
                this.Schema = clsFacil.ObterEsquemaCTe(xmlProc);
                if (this.Schema.StartsWith(Constante.EsqGTVeAutorizacaoSchema))
                {
                    xmlInt = (XmlElement)xmlDocumento.GetElementsByTagName(Constante.EsqGTVeAutorizacaoProc)[0];
                    xmlEnv = (XmlElement)xmlInt.GetElementsByTagName(Constante.EsqGTVeAutorizacaoEnv)[0];
                    xmlRet = (XmlElement)xmlInt.GetElementsByTagName(Constante.EsqGTVeAutorizacaoRet)[0];
                }
                else if (this.Schema.StartsWith(Constante.EsqCTeOSAutorizacaoSchema))
                {
                    xmlInt = (XmlElement)xmlDocumento.GetElementsByTagName(Constante.EsqCTeOSAutorizacaoProc)[0];
                    xmlEnv = (XmlElement)xmlInt.GetElementsByTagName(Constante.EsqCTeOSAutorizacaoEnv)[0];
                    xmlRet = (XmlElement)xmlInt.GetElementsByTagName(Constante.EsqCTeOSAutorizacaoRet)[0];
                }
                else if (this.Schema.StartsWith(Constante.EsqCTeSimpAutorizacaoSchema))
                {
                    xmlInt = (XmlElement)xmlDocumento.GetElementsByTagName(Constante.EsqCTeSimpAutorizacaoProc)[0];
                    if (xmlInt == null)
                    {
                        xmlInt = (XmlElement)xmlDocumento.GetElementsByTagName(Constante.EsqCTeSimpAutorizacaoProc2)[0];
                    }
                    xmlEnv = (XmlElement)xmlInt.GetElementsByTagName(Constante.EsqCTeSimpAutorizacaoEnv)[0];
                    xmlRet = (XmlElement)xmlInt.GetElementsByTagName(Constante.EsqCTeSimpAutorizacaoRet)[0];
                }
                else if (this.Schema.StartsWith(Constante.EsqCTeAutorizacaoSchema))
                {
                    xmlInt = (XmlElement)xmlDocumento.GetElementsByTagName(Constante.EsqCTeAutorizacaoProc)[0];
                    xmlEnv = (XmlElement)xmlInt.GetElementsByTagName(Constante.EsqCTeAutorizacaoEnv)[0];
                    xmlRet = (XmlElement)xmlInt.GetElementsByTagName(Constante.EsqCTeAutorizacaoRet)[0];
                }

                // Obtendo elementos do XML
                XmlElement infProt = (XmlElement)xmlRet.GetElementsByTagName("infProt")[0];
                XmlElement infCte = (XmlElement)xmlEnv.GetElementsByTagName("infCte")[0];
                XmlElement ide = (XmlElement)infCte.GetElementsByTagName("ide")[0];
                XmlElement emit = (XmlElement)infCte.GetElementsByTagName("emit")[0];
                XmlElement rem = (XmlElement)infCte.GetElementsByTagName("rem")[0];
                XmlElement toma = (XmlElement)infCte.GetElementsByTagName("toma")[0];
                XmlElement exped = (XmlElement)infCte.GetElementsByTagName("exped")[0];
                XmlElement receb = (XmlElement)infCte.GetElementsByTagName("receb")[0];
                XmlElement dest = (XmlElement)infCte.GetElementsByTagName("dest")[0];
                XmlElement vPrest = (XmlElement)infCte.GetElementsByTagName("vPrest")[0];
                XmlElement total = (XmlElement)infCte.GetElementsByTagName("total")[0];
                XmlElement imp = (XmlElement)infCte.GetElementsByTagName("imp")[0];
                XmlElement infCTeNorm = (XmlElement)infCte.GetElementsByTagName("infCTeNorm")[0];

                // Preenchendo propiedades com atributos do XML
                this.XML = xmlDocumento.OuterXml;
                this.XMLProc = xmlProc.OuterXml;
                this.XMLEnvio = xmlEnv.OuterXml;
                this.XMLRetorno = xmlRet.OuterXml;
                this.Versao = clsFacil.ObterAtributoXML(xmlRet.Attributes["versao"]);
                this.NSU = clsFacil.ObterAtributoXML(xmlProc.Attributes["NSUSVD"]);
                this.IP = clsFacil.ObterAtributoXML(xmlInt.Attributes["ipTransmissor"]);
                this.Porta = clsFacil.ObterAtributoXML(xmlInt.Attributes["nPortaCon"]);
                this.DataConexao = clsFacil.ObterAtributoXML(xmlInt.Attributes["dhConexao"]);

                // Preenchendo propiedades com dados do XML
                this.PreencherPropriedadesRetorno(infProt);
                this.PreencherPropriedadesIdentificacao(ide);
                this.PreencherPropriedadesEmitente(emit);
                this.PreencherPropriedadesRemetente(rem);
                this.PreencherPropriedadesTomador(toma);
                this.PreencherPropriedadesExpedidor(exped);
                this.PreencherPropriedadesRecebedor(receb);
                this.PreencherPropriedadesDestinatario(dest);
                this.PreencherPropriedadesPrestacao(vPrest);
                this.PreencherPropriedadesTotal(total);
                this.PreencherPropriedadesImposto(imp);
                this.PreencherPropriedadesCTeNormal(infCTeNorm);
                this.PreencherPropriedadesCNPJCPF();

                if (this.DataAutorizacao == string.Empty)
                {
                    this.DataAutorizacao = clsFacil.ObterItemXML(xmlDocumento["proc"]["CTe"]["dhRecbto"]);
                }

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

        private void PreencherPropriedadesRetorno(XmlElement infProt)
        {
            // Obtendo dados do retorno no XML
            if (infProt != null)
            {
                this.ChaveAcesso = clsFacil.ObterItemXML(infProt["chCTe"]);
                this.Protocolo = clsFacil.ObterItemXML(infProt["nProt"]);
                this.Status = clsFacil.ObterItemXML(infProt["cStat"]);
                this.Motivo = clsFacil.ObterItemXML(infProt["xMotivo"]);
                this.DigestValue = clsFacil.ObterItemXML(infProt["digVal"]);
                this.DataAutorizacao = clsFacil.ObterItemXML(infProt["dhRecbto"]);
                this.DataReferencia = clsFacil.ObterDataReferencia(this.ChaveAcesso);
                this.Chave = this.DataReferencia + ";" + this.ChaveAcesso;
            }
        }

        #endregion

        #region " PreencherPropriedadesIdentificacao "

        private void PreencherPropriedadesIdentificacao(XmlElement ide)
        {
            // Obtendo dados de identificacao no XML
            if (ide != null)
            {
                this.Modelo = clsFacil.ObterItemXML(ide["mod"]);
                this.Serie = clsFacil.ObterItemXML(ide["serie"]);
                this.Numero = clsFacil.ObterItemXML(ide["nCT"]);
                this.DigitoVerificador = clsFacil.ObterItemXML(ide["cDV"]);
                this.Codigo = clsFacil.ObterItemXML(ide["cCT"]);
                this.UFCod = clsFacil.ObterItemXML(ide["cUF"]);
                this.UFEnv = clsFacil.ObterItemXML(ide["UFEnv"]);
                this.MunicipioEnv = clsFacil.ObterItemXML(ide["xMunEnv"]);
                this.MunicipioEnvCod = clsFacil.ObterItemXML(ide["cMunEnv"]);
                this.UFIni = clsFacil.ObterItemXML(ide["UFIni"]);
                this.MunicipioIni = clsFacil.ObterItemXML(ide["xMunIni"]);
                this.MunicipioIniCod = clsFacil.ObterItemXML(ide["cMunIni"]);
                this.UFFim = clsFacil.ObterItemXML(ide["UFFim"]);
                this.MunicipioFim = clsFacil.ObterItemXML(ide["xMunFim"]);
                this.MunicipioFimCod = clsFacil.ObterItemXML(ide["cMunFim"]);
                this.CFOP = clsFacil.ObterItemXML(ide["CFOP"]);
                this.Modal = clsFacil.ObterItemXML(ide["modal"]);
                this.NatOperacao = clsFacil.ObterItemXML(ide["natOp"]);
                this.TipoCTe = clsFacil.ObterItemXML(ide["tpCTe"]);
                this.TipoImpressao = clsFacil.ObterItemXML(ide["tpImp"]);
                this.TipoEmissao = clsFacil.ObterItemXML(ide["tpEmis"]);
                this.TipoProcEmissao = clsFacil.ObterItemXML(ide["procEmi"]);
                this.TipoServico = clsFacil.ObterItemXML(ide["tpServ"]);
                this.TipoGlobalizado = clsFacil.ObterItemXML(ide["indGlobalizado"]);
                this.TipoFormaPagto = clsFacil.ObterItemXML(ide["forPag"]);
                this.VersaoAplic = clsFacil.ObterItemXML(ide["verProc"]);
                this.JustContingencia = clsFacil.ObterItemXML(ide["xJust"]);
                this.DataContingencia = clsFacil.ObterItemXML(ide["dhCont"]);
                this.DataEmissao = clsFacil.ObterItemXML(ide["dhEmi"]);
                if (this.DataEmissao == string.Empty)
                {
                    this.DataEmissao = clsFacil.ObterItemXML(ide["dEmi"]);
                }

                // Obtendo dados do tomador no XML
                this.TomaIndicador = clsFacil.ObterItemXML(ide["indIEToma"]);
                XmlElement toma03 = ide["toma03"];
                if (toma03 != null)
                {
                    this.TomaCod = clsFacil.ObterItemXML(toma03["toma"]);
                }
                XmlElement toma3 = ide["toma3"];
                if (toma3 != null)
                {
                    this.TomaCod = clsFacil.ObterItemXML(toma3["toma"]);
                }
                XmlElement toma4 = ide["toma4"];
                if (toma4 != null)
                {
                    this.TomaCod = clsFacil.ObterItemXML(toma4["toma"]);
                    this.TomaCNPJ = clsFacil.ObterItemXML(toma4["CNPJ"]);
                    this.TomaCPF = clsFacil.ObterItemXML(toma4["CPF"]);
                    this.TomaIE = clsFacil.ObterItemXML(toma4["IE"]);
                    this.TomaNome = clsFacil.ObterItemXML(toma4["xNome"]);
                    this.TomaFantasia = clsFacil.ObterItemXML(toma4["xFant"]);
                    this.TomaFone = clsFacil.ObterItemXML(toma4["fone"]);
                    this.TomaEmail = clsFacil.ObterItemXML(toma4["email"]);

                    XmlElement enderToma = toma4["enderToma"];
                    if (enderToma != null)
                    {
                        this.TomaEndUF = clsFacil.ObterItemXML(enderToma["UF"]);
                        this.TomaEndCEP = clsFacil.ObterItemXML(enderToma["CEP"]);
                        this.TomaEndNumero = clsFacil.ObterItemXML(enderToma["nro"]);
                        this.TomaEndLogradouro = clsFacil.ObterItemXML(enderToma["xLgr"]);
                        this.TomaEndComplemento = clsFacil.ObterItemXML(enderToma["xCpl"]);
                        this.TomaEndBairro = clsFacil.ObterItemXML(enderToma["xBairro"]);
                        this.TomaEndMunicipio = clsFacil.ObterItemXML(enderToma["xMun"]);
                        this.TomaEndMunicipioCod = clsFacil.ObterItemXML(enderToma["cMun"]);
                        this.TomaEndPais = clsFacil.ObterItemXML(enderToma["xPais"]);
                        this.TomaEndPaisCod = clsFacil.ObterItemXML(enderToma["cPais"]);
                    }
                }
            }
        }

        #endregion

        #region " PreencherPropriedadesEmitente "

        private void PreencherPropriedadesEmitente(XmlElement emit)
        {
            // Obtendo dados do emitente no XML
            if (emit != null)
            {
                this.EmitCNPJ = clsFacil.ObterItemXML(emit["CNPJ"]);
                this.EmitCPF = clsFacil.ObterItemXML(emit["CPF"]);
                this.EmitIE = clsFacil.ObterItemXML(emit["IE"]);
                this.EmitIEST = clsFacil.ObterItemXML(emit["IEST"]);
                this.EmitNome = clsFacil.ObterItemXML(emit["xNome"]);
                this.EmitFantasia = clsFacil.ObterItemXML(emit["xFant"]);

                XmlElement enderEmit = emit["enderEmit"];
                if (enderEmit != null)
                {
                    this.EmitEndUF = clsFacil.ObterItemXML(enderEmit["UF"]);
                    this.EmitEndCEP = clsFacil.ObterItemXML(enderEmit["CEP"]);
                    this.EmitEndNumero = clsFacil.ObterItemXML(enderEmit["nro"]);
                    this.EmitEndLogradouro = clsFacil.ObterItemXML(enderEmit["xLgr"]);
                    this.EmitEndComplemento = clsFacil.ObterItemXML(enderEmit["xCpl"]);
                    this.EmitEndBairro = clsFacil.ObterItemXML(enderEmit["xBairro"]);
                    this.EmitEndMunicipio = clsFacil.ObterItemXML(enderEmit["xMun"]);
                    this.EmitEndMunicipioCod = clsFacil.ObterItemXML(enderEmit["cMun"]);
                    this.EmitEndPais = clsFacil.ObterItemXML(enderEmit["xPais"]);
                    this.EmitEndPaisCod = clsFacil.ObterItemXML(enderEmit["cPais"]);
                    this.EmitFone = clsFacil.ObterItemXML(enderEmit["fone"]);
                }
            }
        }

        #endregion

        #region " PreencherPropriedadesRemetente "

        private void PreencherPropriedadesRemetente(XmlElement rem)
        {
            // Obtendo dados do remetente no XML
            if (rem != null)
            {
                this.RemeCNPJ = clsFacil.ObterItemXML(rem["CNPJ"]);
                this.RemeCPF = clsFacil.ObterItemXML(rem["CPF"]);
                this.RemeIE = clsFacil.ObterItemXML(rem["IE"]);
                this.RemeNome = clsFacil.ObterItemXML(rem["xNome"]);
                this.RemeFantasia = clsFacil.ObterItemXML(rem["xFant"]);
                this.RemeFone = clsFacil.ObterItemXML(rem["fone"]);
                this.RemeEmail = clsFacil.ObterItemXML(rem["email"]);

                XmlElement enderReme = rem["enderReme"];
                if (enderReme != null)
                {
                    this.RemeEndUF = clsFacil.ObterItemXML(enderReme["UF"]);
                    this.RemeEndCEP = clsFacil.ObterItemXML(enderReme["CEP"]);
                    this.RemeEndNumero = clsFacil.ObterItemXML(enderReme["nro"]);
                    this.RemeEndLogradouro = clsFacil.ObterItemXML(enderReme["xLgr"]);
                    this.RemeEndComplemento = clsFacil.ObterItemXML(enderReme["xCpl"]);
                    this.RemeEndBairro = clsFacil.ObterItemXML(enderReme["xBairro"]);
                    this.RemeEndMunicipio = clsFacil.ObterItemXML(enderReme["xMun"]);
                    this.RemeEndMunicipioCod = clsFacil.ObterItemXML(enderReme["cMun"]);
                    this.RemeEndPais = clsFacil.ObterItemXML(enderReme["xPais"]);
                    this.RemeEndPaisCod = clsFacil.ObterItemXML(enderReme["cPais"]);
                }
            }
        }

        #endregion

        #region " PreencherPropriedadesTomador "

        private void PreencherPropriedadesTomador(XmlElement toma)
        {
            // Obtendo dados do remetente no XML
            if ((toma != null) && (this.TomaCod == string.Empty))
            {
                this.TomaCNPJ = clsFacil.ObterItemXML(toma["CNPJ"]);
                this.TomaCPF = clsFacil.ObterItemXML(toma["CPF"]);
                this.TomaIE = clsFacil.ObterItemXML(toma["IE"]);
                this.TomaNome = clsFacil.ObterItemXML(toma["xNome"]);
                this.TomaFantasia = clsFacil.ObterItemXML(toma["xFant"]);
                this.TomaFone = clsFacil.ObterItemXML(toma["fone"]);
                this.TomaEmail = clsFacil.ObterItemXML(toma["email"]);
                this.TomaCod = clsFacil.ObterItemXML(toma["toma"]);
                this.TomaIndicador = clsFacil.ObterItemXML(toma["indIEToma"]);

                XmlElement enderToma = toma["enderToma"];
                if (enderToma != null)
                {
                    this.TomaEndUF = clsFacil.ObterItemXML(enderToma["UF"]);
                    this.TomaEndCEP = clsFacil.ObterItemXML(enderToma["CEP"]);
                    this.TomaEndNumero = clsFacil.ObterItemXML(enderToma["nro"]);
                    this.TomaEndLogradouro = clsFacil.ObterItemXML(enderToma["xLgr"]);
                    this.TomaEndComplemento = clsFacil.ObterItemXML(enderToma["xCpl"]);
                    this.TomaEndBairro = clsFacil.ObterItemXML(enderToma["xBairro"]);
                    this.TomaEndMunicipio = clsFacil.ObterItemXML(enderToma["xMun"]);
                    this.TomaEndMunicipioCod = clsFacil.ObterItemXML(enderToma["cMun"]);
                    this.TomaEndPais = clsFacil.ObterItemXML(enderToma["xPais"]);
                    this.TomaEndPaisCod = clsFacil.ObterItemXML(enderToma["cPais"]);
                }
            }
        }

        #endregion

        #region " PreencherPropriedadesExpedidor "

        private void PreencherPropriedadesExpedidor(XmlElement exped)
        {
            // Obtendo dados do expedidor no XML
            if (exped != null)
            {
                this.ExpeCNPJ = clsFacil.ObterItemXML(exped["CNPJ"]);
                this.ExpeCPF = clsFacil.ObterItemXML(exped["CPF"]);
                this.ExpeIE = clsFacil.ObterItemXML(exped["IE"]);
                this.ExpeNome = clsFacil.ObterItemXML(exped["xNome"]);
                this.ExpeFone = clsFacil.ObterItemXML(exped["fone"]);
                this.ExpeEmail = clsFacil.ObterItemXML(exped["email"]);

                XmlElement enderExped = exped["enderExped"];
                if (enderExped != null)
                {
                    this.ExpeEndUF = clsFacil.ObterItemXML(enderExped["UF"]);
                    this.ExpeEndCEP = clsFacil.ObterItemXML(enderExped["CEP"]);
                    this.ExpeEndNumero = clsFacil.ObterItemXML(enderExped["nro"]);
                    this.ExpeEndLogradouro = clsFacil.ObterItemXML(enderExped["xLgr"]);
                    this.ExpeEndComplemento = clsFacil.ObterItemXML(enderExped["xCpl"]);
                    this.ExpeEndBairro = clsFacil.ObterItemXML(enderExped["xBairro"]);
                    this.ExpeEndMunicipio = clsFacil.ObterItemXML(enderExped["xMun"]);
                    this.ExpeEndMunicipioCod = clsFacil.ObterItemXML(enderExped["cMun"]);
                    this.ExpeEndPais = clsFacil.ObterItemXML(enderExped["xPais"]);
                    this.ExpeEndPaisCod = clsFacil.ObterItemXML(enderExped["cPais"]);
                }
            }
        }

        #endregion

        #region " PreencherPropriedadesRecebedor "

        private void PreencherPropriedadesRecebedor(XmlElement receb)
        {
            // Obtendo dados do recebedor no XML
            if (receb != null)
            {
                this.ReceCNPJ = clsFacil.ObterItemXML(receb["CNPJ"]);
                this.ReceCPF = clsFacil.ObterItemXML(receb["CPF"]);
                this.ReceIE = clsFacil.ObterItemXML(receb["IE"]);
                this.ReceNome = clsFacil.ObterItemXML(receb["xNome"]);
                this.ReceFone = clsFacil.ObterItemXML(receb["fone"]);
                this.ReceEmail = clsFacil.ObterItemXML(receb["email"]);

                XmlElement enderReceb = receb["enderReceb"];
                if (enderReceb != null)
                {
                    this.ReceEndUF = clsFacil.ObterItemXML(enderReceb["UF"]);
                    this.ReceEndCEP = clsFacil.ObterItemXML(enderReceb["CEP"]);
                    this.ReceEndNumero = clsFacil.ObterItemXML(enderReceb["nro"]);
                    this.ReceEndLogradouro = clsFacil.ObterItemXML(enderReceb["xLgr"]);
                    this.ReceEndComplemento = clsFacil.ObterItemXML(enderReceb["xCpl"]);
                    this.ReceEndBairro = clsFacil.ObterItemXML(enderReceb["xBairro"]);
                    this.ReceEndMunicipio = clsFacil.ObterItemXML(enderReceb["xMun"]);
                    this.ReceEndMunicipioCod = clsFacil.ObterItemXML(enderReceb["cMun"]);
                    this.ReceEndPais = clsFacil.ObterItemXML(enderReceb["xPais"]);
                    this.ReceEndPaisCod = clsFacil.ObterItemXML(enderReceb["cPais"]);
                }
            }
        }

        #endregion

        #region " PreencherPropriedadesDestinatario "

        private void PreencherPropriedadesDestinatario(XmlElement dest)
        {
            // Obtendo dados do destinatario no XML
            if (dest != null)
            {
                this.DestCNPJ = clsFacil.ObterItemXML(dest["CNPJ"]);
                this.DestCPF = clsFacil.ObterItemXML(dest["CPF"]);
                this.DestIE = clsFacil.ObterItemXML(dest["IE"]);
                this.DestNome = clsFacil.ObterItemXML(dest["xNome"]);
                this.DestFone = clsFacil.ObterItemXML(dest["fone"]);
                this.DestEmail = clsFacil.ObterItemXML(dest["email"]);

                XmlElement enderDest = dest["enderDest"];
                if (enderDest != null)
                {
                    this.DestEndUF = clsFacil.ObterItemXML(enderDest["UF"]);
                    this.DestEndCEP = clsFacil.ObterItemXML(enderDest["CEP"]);
                    this.DestEndNumero = clsFacil.ObterItemXML(enderDest["nro"]);
                    this.DestEndLogradouro = clsFacil.ObterItemXML(enderDest["xLgr"]);
                    this.DestEndComplemento = clsFacil.ObterItemXML(enderDest["xCpl"]);
                    this.DestEndBairro = clsFacil.ObterItemXML(enderDest["xBairro"]);
                    this.DestEndMunicipio = clsFacil.ObterItemXML(enderDest["xMun"]);
                    this.DestEndMunicipioCod = clsFacil.ObterItemXML(enderDest["cMun"]);
                    this.DestEndPais = clsFacil.ObterItemXML(enderDest["xPais"]);
                    this.DestEndPaisCod = clsFacil.ObterItemXML(enderDest["cPais"]);
                }
            }
        }

        #endregion

        #region " PreencherPropriedadesPrestacao "

        private void PreencherPropriedadesPrestacao(XmlElement vPrest)
        {
            // Obtendo dados do valor da prestacao no XML
            if (vPrest != null)
            {
                this.ValorTotal = clsFacil.ObterItemXML(vPrest["vTPrest"]);
                this.ValorReceber = clsFacil.ObterItemXML(vPrest["vRec"]);
            }
        }

        #endregion

        #region " PreencherPropriedadesTotal "

        private void PreencherPropriedadesTotal(XmlElement total)
        {
            // Obtendo dados do valor da prestacao no XML
            if (total != null)
            {
                this.ValorTotal = clsFacil.ObterItemXML(total["vTPrest"]);
                this.ValorReceber = clsFacil.ObterItemXML(total["vTRec"]);
            }
        }

        #endregion

        #region " PreencherPropriedadesImposto "

        private void PreencherPropriedadesImposto(XmlElement imp)
        {
            // Obtendo dados do imposto no XML
            if (imp != null)
            {
                this.ImpostoValorTotal = clsFacil.ObterItemXML(imp["vTotTrib"]);

                XmlElement ICMS = imp["ICMS"];
                if (ICMS != null)
                {
                    XmlElement ICMS00 = ICMS["ICMS00"];
                    if (ICMS00 != null)
                    {
                        this.ImpostoICMS = ICMS00.Name;
                        this.ImpostoCST = clsFacil.ObterItemXML(ICMS00["CST"]);
                        this.ImpostoValorICMS = clsFacil.ObterItemXML(ICMS00["vICMS"]);
                        this.ImpostoValorICMSPerc = clsFacil.ObterItemXML(ICMS00["pICMS"]);
                        this.ImpostoValorBC = clsFacil.ObterItemXML(ICMS00["vBC"]);
                    }
                    XmlElement ICMS20 = ICMS["ICMS20"];
                    if (ICMS20 != null)
                    {
                        this.ImpostoICMS = ICMS20.Name;
                        this.ImpostoCST = clsFacil.ObterItemXML(ICMS20["CST"]);
                        this.ImpostoValorICMS = clsFacil.ObterItemXML(ICMS20["vICMS"]);
                        this.ImpostoValorICMSPerc = clsFacil.ObterItemXML(ICMS20["pICMS"]);
                        this.ImpostoValorBC = clsFacil.ObterItemXML(ICMS20["vBC"]);
                        this.ImpostoValorBCPerc = clsFacil.ObterItemXML(ICMS20["pRedBC"]);
                    }
                    XmlElement ICMS45 = ICMS["ICMS45"];
                    if (ICMS45 != null)
                    {
                        this.ImpostoICMS = ICMS45.Name;
                        this.ImpostoCST = clsFacil.ObterItemXML(ICMS45["CST"]);
                    }
                    XmlElement ICMS60 = ICMS["ICMS60"];
                    if (ICMS60 != null)
                    {
                        this.ImpostoICMS = ICMS60.Name;
                        this.ImpostoCST = clsFacil.ObterItemXML(ICMS60["CST"]);
                        this.ImpostoValorICMS = clsFacil.ObterItemXML(ICMS60["vICMSSTRet"]);
                        this.ImpostoValorICMSPerc = clsFacil.ObterItemXML(ICMS60["pICMSSTRet"]);
                        this.ImpostoValorBC = clsFacil.ObterItemXML(ICMS60["vBCSTRet"]);
                        this.ImpostoValorCredito = clsFacil.ObterItemXML(ICMS60["vCred"]);
                    }
                    XmlElement ICMS90 = ICMS["ICMS90"];
                    if (ICMS90 != null)
                    {
                        this.ImpostoICMS = ICMS90.Name;
                        this.ImpostoCST = clsFacil.ObterItemXML(ICMS90["CST"]);
                        this.ImpostoValorICMS = clsFacil.ObterItemXML(ICMS90["vICMS"]);
                        this.ImpostoValorICMSPerc = clsFacil.ObterItemXML(ICMS90["pICMS"]);
                        this.ImpostoValorBC = clsFacil.ObterItemXML(ICMS90["vBC"]);
                        this.ImpostoValorBCPerc = clsFacil.ObterItemXML(ICMS90["pRedBC"]);
                        this.ImpostoValorCredito = clsFacil.ObterItemXML(ICMS90["vCred"]);
                    }
                    XmlElement ICMSOutraUF = ICMS["ICMSOutraUF"];
                    if (ICMSOutraUF != null)
                    {
                        this.ImpostoICMS = ICMSOutraUF.Name;
                        this.ImpostoCST = clsFacil.ObterItemXML(ICMSOutraUF["CST"]);
                        this.ImpostoValorICMS = clsFacil.ObterItemXML(ICMSOutraUF["vICMSOutraUF"]);
                        this.ImpostoValorICMSPerc = clsFacil.ObterItemXML(ICMSOutraUF["pICMSOutraUF"]);
                        this.ImpostoValorBC = clsFacil.ObterItemXML(ICMSOutraUF["vBCOutraUF"]);
                        this.ImpostoValorBCPerc = clsFacil.ObterItemXML(ICMSOutraUF["pRedBCOutraUF"]);
                    }
                    XmlElement ICMSSN = ICMS["ICMSSN"];
                    if (ICMSSN != null)
                    {
                        this.ImpostoICMS = ICMSSN.Name;
                        this.ImpostoCST = clsFacil.ObterItemXML(ICMSSN["CST"]);
                    }
                }
            }
        }

        #endregion

        #region " PreencherPropriedadesCTeNormal "

        private void PreencherPropriedadesCTeNormal(XmlElement infCTeNorm)
        {
            // Obtendo dados do imposto no XML
            if (infCTeNorm != null)
            {
                this.QtdeNFes = infCTeNorm.GetElementsByTagName("infNFe").Count.ToString();
            }
        }

        #endregion

        #region " PreencherPropriedadesCNPJCPF "

        private void PreencherPropriedadesCNPJCPF()
        {
            // Destrinchando CNPJ/CPF do tomador
            if (this.TomaCNPJ != string.Empty)
            {
                this.TomaCNPJCPFBase = clsFacil.ObterCNPJBase(this.TomaCNPJ);
                this.TomaCNPJCPFFilial = clsFacil.ObterCNPJFilial(this.TomaCNPJ);
                this.TomaCNPJCPFDigito = clsFacil.ObterCNPJDigito(this.TomaCNPJ);
            }
            else if (this.TomaCPF != string.Empty)
            {
                this.TomaCNPJCPFBase = clsFacil.ObterCPFBase(this.TomaCPF);
                this.TomaCNPJCPFFilial = clsFacil.ObterCPFFilial(this.TomaCPF);
                this.TomaCNPJCPFDigito = clsFacil.ObterCPFDigito(this.TomaCPF);
            }

            // Destrinchando CNPJ/CPF do emitente
            if (this.EmitCNPJ != string.Empty)
            {
                this.EmitCNPJCPFBase = clsFacil.ObterCNPJBase(this.EmitCNPJ);
                this.EmitCNPJCPFFilial = clsFacil.ObterCNPJFilial(this.EmitCNPJ);
                this.EmitCNPJCPFDigito = clsFacil.ObterCNPJDigito(this.EmitCNPJ);
            }
            else if (this.EmitCPF != string.Empty)
            {
                this.EmitCNPJCPFBase = clsFacil.ObterCPFBase(this.EmitCPF);
                this.EmitCNPJCPFFilial = clsFacil.ObterCPFFilial(this.EmitCPF);
                this.EmitCNPJCPFDigito = clsFacil.ObterCPFDigito(this.EmitCPF);
            }

            // Destrinchando CNPJ/CPF do remetente
            if (this.RemeCNPJ != string.Empty)
            {
                this.RemeCNPJCPFBase = clsFacil.ObterCNPJBase(this.RemeCNPJ);
                this.RemeCNPJCPFFilial = clsFacil.ObterCNPJFilial(this.RemeCNPJ);
                this.RemeCNPJCPFDigito = clsFacil.ObterCNPJDigito(this.RemeCNPJ);
            }
            else if (this.RemeCPF != string.Empty)
            {
                this.RemeCNPJCPFBase = clsFacil.ObterCPFBase(this.RemeCPF);
                this.RemeCNPJCPFFilial = clsFacil.ObterCPFFilial(this.RemeCPF);
                this.RemeCNPJCPFDigito = clsFacil.ObterCPFDigito(this.RemeCPF);
            }

            // Destrinchando CNPJ/CPF do expedidor
            if (this.ExpeCNPJ != string.Empty)
            {
                this.ExpeCNPJCPFBase = clsFacil.ObterCNPJBase(this.ExpeCNPJ);
                this.ExpeCNPJCPFFilial = clsFacil.ObterCNPJFilial(this.ExpeCNPJ);
                this.ExpeCNPJCPFDigito = clsFacil.ObterCNPJDigito(this.ExpeCNPJ);
            }
            else if (this.ExpeCPF != string.Empty)
            {
                this.ExpeCNPJCPFBase = clsFacil.ObterCPFBase(this.ExpeCPF);
                this.ExpeCNPJCPFFilial = clsFacil.ObterCPFFilial(this.ExpeCPF);
                this.ExpeCNPJCPFDigito = clsFacil.ObterCPFDigito(this.ExpeCPF);
            }

            // Destrinchando CNPJ/CPF do recebedor
            if (this.ReceCNPJ != string.Empty)
            {
                this.ReceCNPJCPFBase = clsFacil.ObterCNPJBase(this.ReceCNPJ);
                this.ReceCNPJCPFFilial = clsFacil.ObterCNPJFilial(this.ReceCNPJ);
                this.ReceCNPJCPFDigito = clsFacil.ObterCNPJDigito(this.ReceCNPJ);
            }
            else if (this.ReceCPF != string.Empty)
            {
                this.ReceCNPJCPFBase = clsFacil.ObterCPFBase(this.ReceCPF);
                this.ReceCNPJCPFFilial = clsFacil.ObterCPFFilial(this.ReceCPF);
                this.ReceCNPJCPFDigito = clsFacil.ObterCPFDigito(this.ReceCPF);
            }

            // Destrinchando CNPJ/CPF do destinatario
            if (this.DestCNPJ != string.Empty)
            {
                this.DestCNPJCPFBase = clsFacil.ObterCNPJBase(this.DestCNPJ);
                this.DestCNPJCPFFilial = clsFacil.ObterCNPJFilial(this.DestCNPJ);
                this.DestCNPJCPFDigito = clsFacil.ObterCNPJDigito(this.DestCNPJ);
            }
            else if (this.DestCPF != string.Empty)
            {
                this.DestCNPJCPFBase = clsFacil.ObterCPFBase(this.DestCPF);
                this.DestCNPJCPFFilial = clsFacil.ObterCPFFilial(this.DestCPF);
                this.DestCNPJCPFDigito = clsFacil.ObterCPFDigito(this.DestCPF);
            }
        }

        #endregion
    }
}