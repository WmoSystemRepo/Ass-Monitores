using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Common.SerializableClasses.CTe
{
    public class ClsXmlGTVeNT202402
    {

        [Serializable]
        [XmlRoot(ElementName = "proc", Namespace = "http://www.portalfiscal.inf.br/cte")]
        public class proc
        {
            [XmlAttribute(AttributeName = "schema")]

            public string schema { get; set; }

            [XmlAttribute(AttributeName = "NSUSVD")]

            public string NSUSVD { get; set; }

            [XmlAttribute(AttributeName = "NSUAut")]

            public string NSUAut { get; set; }

            [XmlElement(ElementName = "GTVeProc", Namespace = "http://www.portalfiscal.inf.br/cte")]
            public GTVeProc GTVeProc { get; set; }

            //[XmlElement(ElementName = "CTe")]
            //public CTe CTe { get; set; }

        }

        [Serializable]
        [XmlRoot(ElementName = "GTVeProc", Namespace = "http://www.portalfiscal.inf.br/cte")]
        public class GTVeProc

        {

            [XmlElement(ElementName = "GTVe", Namespace = "http://www.portalfiscal.inf.br/cte")]

            public GTVe GTVe { get; set; }

            [XmlElement(ElementName = "protCTe", Namespace = "http://www.portalfiscal.inf.br/cte")]

            public protCTe protCTe { get; set; }

            [XmlAttribute(AttributeName = "versao")]

            public string versao { get; set; }

        }

        [XmlRoot(ElementName = "GTVe", Namespace = "http://www.portalfiscal.inf.br/cte")]
        public class GTVe

        {
            [XmlElement(ElementName = "infCte", Namespace = "http://www.portalfiscal.inf.br/cte")]
            public infCte infCte { get; set; }

            [XmlElement(ElementName = "infCTeSupl", Namespace = "http://www.portalfiscal.inf.br/cte")]
            public infCTeSupl infCTeSupl { get; set; }

            [XmlElement(ElementName = "Signature", Namespace = "http://www.w3.org/2000/09/xmldsig#")]
            public Signature Signature { get; set; }

            [XmlAttribute(AttributeName = "versao")]

            public string versao { get; set; }

        }

        #region infCTe
        [Serializable]

        public class infCte

        {

            public infCte()
            {
                ide = new ide();
                compl = new compl();
                emit = new emit();
                rem = new rem();
                dest = new dest();
                origem = new origem();
                destino = new destino();
                detGTV = new detGTV();
                autXML = new List<autXML>();
                infRespTec = new infRespTec();
            }

            public ide ide { get; set; }

            public compl compl { get; set; }

            public emit emit { get; set; }

            public rem rem { get; set; }

            public dest dest { get; set; }

            public origem origem { get; set; }

            public destino destino { get; set; }

            public detGTV detGTV { get; set; }

            [XmlElement(ElementName = "autXML")]
            public List<autXML> autXML { get; set; }
            
            public infRespTec infRespTec { get; set; }
            
            [XmlAttribute(AttributeName = "versao")]
            public string versao { get; set; }

            [XmlAttribute(AttributeName = "Id")]
            public string Id { get; set; }


        }

        #region ide
        [Serializable]

        public class ide

        {
            public ide()
            {
                toma = new toma();
                tomaTerceiro = new tomaTerceiro();
            }

            public string cUF { get; set; }

            public string cCT { get; set; }

            public string CFOP { get; set; }

            public string natOp { get; set; }

            public string mod { get; set; }

            public string serie { get; set; }

            public string nCT { get; set; }

            public string dhEmi { get; set; }

            public string tpImp { get; set; }

            public string tpEmis { get; set; }

            public string cDV { get; set; }

            public string tpAmb { get; set; }

            public string tpCTe { get; set; }

            public string verProc { get; set; }

            public string cMunEnv { get; set; }

            public string xMunEnv { get; set; }

            public string UFEnv { get; set; }

            public string modal { get; set; }

            public string tpServ { get; set; }

            public string indIEToma { get; set; }

            public string dhSaidaOrig { get; set; }

            public string dhChegadaDest { get; set; }

            public toma toma { get; set; }

            public tomaTerceiro tomaTerceiro { get; set; }

            public string dhCont { get; set; }

            public string xJust { get; set; }

        }

        [Serializable]

        public class toma

        {
            [XmlElement(ElementName = "toma")]
            public string strToma { get; set; }

        }


        [Serializable]

        public class tomaTerceiro
        {
            public tomaTerceiro()
            {
                enderToma = new enderToma();
            }


            public string toma { get; set; }


            public string CNPJ { get; set; }


            public string CPF { get; set; }


            public string IE { get; set; }


            public string xNome { get; set; }


            public string xFant { get; set; }

            public string fone { get; set; }

            public enderToma enderToma { get; set; }

            public string email { get; set; }

        }


        [Serializable]
        public class enderToma
        {


            public string xLgr { get; set; }



            public string nro { get; set; }



            public string xCpl { get; set; }



            public string xBairro { get; set; }



            public string cMun { get; set; }



            public string xMun { get; set; }



            public string CEP { get; set; }



            public string UF { get; set; }



            public string cPais { get; set; }



            public string xPais { get; set; }

            public string email { get; set; }

        }

        #endregion

        #region compl

        [Serializable]

        public class compl

        {

            public compl()
            {
                ObsCont = new List<ObsCont>();
                ObsFisco = new List<ObsFisco>();
            }

            public string xCaracAd { get; set; }

            public string xCaracSer { get; set; }

            public string xEmi { get; set; }

            public string xObs { get; set; }

            [XmlElement(ElementName = "ObsCont")]
            public List<ObsCont> ObsCont { get; set; }

            [XmlElement(ElementName = "ObsFisco")]
            public List<ObsFisco> ObsFisco { get; set; }

        }


        #region ObsCont

        [Serializable]

        public class ObsCont

        {

            [XmlAttribute(AttributeName = "xCampo")]
            public string xCampo { get; set; }

            public string xTexto { get; set; }

        }

        #endregion

        #region ObsFisco

        [Serializable]

        public class ObsFisco

        {

            [XmlAttribute(AttributeName = "xCampo")]
            public string xCampo { get; set; }

            public string xTexto { get; set; }
        }

        #endregion

        #endregion

        #region emit
        [Serializable]

        public class emit

        {

            public string CNPJ { get; set; }

            public string CPF { get; set; }

            public string IE { get; set; }

            public string IEST { get; set; }

            public string xNome { get; set; }

            public string xFant { get; set; }

            public enderEmit enderEmit { get; set; }


        }

        #region enderEmit
        [Serializable]

        public class enderEmit

        {



            public string xLgr { get; set; }



            public string nro { get; set; }


            public string xCpl { get; set; }



            public string xBairro { get; set; }



            public string cMun { get; set; }



            public string xMun { get; set; }



            public string CEP { get; set; }



            public string UF { get; set; }



            public string fone { get; set; }

        }

        #endregion

        #endregion

        #region rem

        [Serializable]

        public class rem

        {
            public rem()
            {
                enderReme = new enderReme();
            }

            public string CNPJ { get; set; }

            public string CPF { get; set; }

            public string IE { get; set; }

            public string xNome { get; set; }

            public string xFant { get; set; }

            public string fone { get; set; }

            public enderReme enderReme { get; set; }

            public string email { get; set; }

        }

        #region enderReme
        [Serializable]

        public class enderReme

        {

            public string xLgr { get; set; }


            public string nro { get; set; }


            public string xCpl { get; set; }


            public string xBairro { get; set; }


            public string cMun { get; set; }


            public string xMun { get; set; }


            public string CEP { get; set; }


            public string UF { get; set; }


            public string cPais { get; set; }


            public string xPais { get; set; }

            public string email { get; set; }

        }

        #endregion

        #endregion

        #region dest
        [Serializable]

        public class dest

        {
            public dest()
            {
                enderDest = new enderDest();
            }

            public string CNPJ { get; set; }

            public string CPF { get; set; }

            public string IE { get; set; }

            public string xNome { get; set; }

            public string fone { get; set; }

            public string ISUF { get; set; }

            public enderDest enderDest { get; set; }

            public string email { get; set; }

        }

        #region enderDest
        [Serializable]

        public class enderDest

        {

            public string xLgr { get; set; }

            public string nro { get; set; }

            public string xCpl { get; set; }


            public string xBairro { get; set; }


            public string cMun { get; set; }


            public string xMun { get; set; }


            public string CEP { get; set; }


            public string UF { get; set; }


            public string cPais { get; set; }

            public string xPais { get; set; }

        }

        #endregion

        #endregion

        #region "origem"

        [Serializable]
        public class origem
        {

            public string xLgr { get; set; }



            public string nro { get; set; }


            public string xCpl { get; set; }



            public string xBairro { get; set; }



            public string cMun { get; set; }



            public string xMun { get; set; }



            public string CEP { get; set; }



            public string UF { get; set; }



            public string fone { get; set; }

        }



        #endregion

        #region "destino"

        [Serializable]
        public class destino
        {

            public string xLgr { get; set; }



            public string nro { get; set; }


            public string xCpl { get; set; }



            public string xBairro { get; set; }



            public string cMun { get; set; }



            public string xMun { get; set; }



            public string CEP { get; set; }



            public string UF { get; set; }



            public string fone { get; set; }

        }



        #endregion

        [Serializable]
        public class detGTV
        {
            public detGTV()
            {
                infEspecie = new List<infEspecie>();
                infVeiculo = new List<infVeiculo>();
            }

            [XmlElement(ElementName = "infEspecie")]            
            public List<infEspecie> infEspecie { get; set; }
            public decimal? qCarga { get; set; }
            [XmlElement(ElementName = "infVeiculo")]
            public List<infVeiculo> infVeiculo { get; set; }

        }

        [Serializable]
        public class infEspecie
        {
            public string tpEspecie { get; set; }
            public decimal? vEspecie { get; set; }
            public string tpNumerario { get; set; }
            public string xMoedaEstr { get; set; }
        }

        [Serializable]
        public class infVeiculo
        {
            public string placa { get; set; }
            public string UF { get; set; }
            public string RNTRC { get; set; }
        }

        #region autXML
        [Serializable]
        public class autXML

        {

            public string CNPJ { get; set; }

            public string CPF { get; set; }

        }

        #endregion

        #region infRespTec

        [Serializable]
        public class infRespTec

        {

            public string CNPJ { get; set; }

            public string xContato { get; set; }

            public string email { get; set; }

            public string fone { get; set; }

            public string idCSRT { get; set; }

            public string hashCSRT { get; set; }

        }

        #endregion

        #endregion

        #region infCTeSupl

        [Serializable]
        public class infCTeSupl
        {
            public string qrCodCTe { get; set; }
        }

        #endregion

        #region Signature

        [XmlRoot(ElementName = "CanonicalizationMethod", Namespace = "http://www.w3.org/2000/09/xmldsig#")]

        public class CanonicalizationMethod

        {

            [XmlAttribute(AttributeName = "Algorithm")]

            public string Algorithm { get; set; }

        }



        [XmlRoot(ElementName = "SignatureMethod", Namespace = "http://www.w3.org/2000/09/xmldsig#")]

        public class SignatureMethod

        {

            [XmlAttribute(AttributeName = "Algorithm")]

            public string Algorithm { get; set; }

        }



        [XmlRoot(ElementName = "Transform", Namespace = "http://www.w3.org/2000/09/xmldsig#")]

        public class Transform

        {

            [XmlAttribute(AttributeName = "Algorithm")]

            public string Algorithm { get; set; }

        }



        [XmlRoot(ElementName = "Transforms", Namespace = "http://www.w3.org/2000/09/xmldsig#")]

        public class Transforms

        {

            [XmlElement(ElementName = "Transform", Namespace = "http://www.w3.org/2000/09/xmldsig#")]

            public List<Transform> Transform { get; set; }

        }



        [XmlRoot(ElementName = "DigestMethod", Namespace = "http://www.w3.org/2000/09/xmldsig#")]

        public class DigestMethod

        {

            [XmlAttribute(AttributeName = "Algorithm")]

            public string Algorithm { get; set; }

        }



        [XmlRoot(ElementName = "Reference", Namespace = "http://www.w3.org/2000/09/xmldsig#")]

        public class Reference

        {

            [XmlElement(ElementName = "Transforms", Namespace = "http://www.w3.org/2000/09/xmldsig#")]

            public Transforms Transforms { get; set; }

            [XmlElement(ElementName = "DigestMethod", Namespace = "http://www.w3.org/2000/09/xmldsig#")]

            public DigestMethod DigestMethod { get; set; }

            [XmlElement(ElementName = "DigestValue", Namespace = "http://www.w3.org/2000/09/xmldsig#")]

            public string DigestValue { get; set; }

            [XmlAttribute(AttributeName = "URI")]

            public string URI { get; set; }

        }



        [XmlRoot(ElementName = "SignedInfo", Namespace = "http://www.w3.org/2000/09/xmldsig#")]

        public class SignedInfo

        {

            [XmlElement(ElementName = "CanonicalizationMethod", Namespace = "http://www.w3.org/2000/09/xmldsig#")]

            public CanonicalizationMethod CanonicalizationMethod { get; set; }

            [XmlElement(ElementName = "SignatureMethod", Namespace = "http://www.w3.org/2000/09/xmldsig#")]

            public SignatureMethod SignatureMethod { get; set; }

            [XmlElement(ElementName = "Reference", Namespace = "http://www.w3.org/2000/09/xmldsig#")]

            public Reference Reference { get; set; }

        }



        [XmlRoot(ElementName = "X509Data", Namespace = "http://www.w3.org/2000/09/xmldsig#")]

        public class X509Data

        {

            [XmlElement(ElementName = "X509Certificate", Namespace = "http://www.w3.org/2000/09/xmldsig#")]

            public string X509Certificate { get; set; }

        }



        [XmlRoot(ElementName = "KeyInfo", Namespace = "http://www.w3.org/2000/09/xmldsig#")]

        public class KeyInfo

        {

            [XmlElement(ElementName = "X509Data", Namespace = "http://www.w3.org/2000/09/xmldsig#")]

            public X509Data X509Data { get; set; }

        }




        [XmlRoot(ElementName = "Signature", Namespace = "http://www.w3.org/2000/09/xmldsig#")]

        public class Signature

        {

            [XmlElement(ElementName = "SignedInfo", Namespace = "http://www.w3.org/2000/09/xmldsig#")]

            public SignedInfo SignedInfo { get; set; }

            [XmlElement(ElementName = "SignatureValue", Namespace = "http://www.w3.org/2000/09/xmldsig#")]

            public string SignatureValue { get; set; }

            [XmlElement(ElementName = "KeyInfo", Namespace = "http://www.w3.org/2000/09/xmldsig#")]

            public KeyInfo KeyInfo { get; set; }

            [XmlAttribute(AttributeName = "xmlns")]

            public string Xmlns { get; set; }

        }

        #endregion


        [Serializable]
        [XmlRoot(ElementName = "protCTe", Namespace = "http://www.portalfiscal.inf.br/cte")]

        public class protCTe

        {

            [XmlElement(ElementName = "infProt", Namespace = "http://www.portalfiscal.inf.br/cte")]
            public InfProt InfProt { get; set; }

            [XmlAttribute(AttributeName = "versao")]
            public string Versao { get; set; }

        }

        [XmlRoot(ElementName = "infProt", Namespace = "http://www.portalfiscal.inf.br/cte")]

        public class InfProt

        {

            [XmlElement(ElementName = "tpAmb", Namespace = "http://www.portalfiscal.inf.br/cte")]

            public string TpAmb { get; set; }

            [XmlElement(ElementName = "verAplic", Namespace = "http://www.portalfiscal.inf.br/cte")]

            public string VerAplic { get; set; }

            [XmlElement(ElementName = "chCTe", Namespace = "http://www.portalfiscal.inf.br/cte")]

            public string chCTe { get; set; }

            [XmlElement(ElementName = "dhRecbto", Namespace = "http://www.portalfiscal.inf.br/cte")]

            public string dhRecbto { get; set; }

            [XmlElement(ElementName = "nProt", Namespace = "http://www.portalfiscal.inf.br/cte")]

            public string nProt { get; set; }

            [XmlElement(ElementName = "digVal", Namespace = "http://www.portalfiscal.inf.br/cte")]

            public string digVal { get; set; }

            [XmlElement(ElementName = "cStat", Namespace = "http://www.portalfiscal.inf.br/cte")]

            public string cStat { get; set; }

            [XmlElement(ElementName = "xMotivo", Namespace = "http://www.portalfiscal.inf.br/cte")]

            public string xMotivo { get; set; }

        }

    }
}
