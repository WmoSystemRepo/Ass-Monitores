using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Common.SerializableClasses.CTe

{
    //PL_CTe_400_NT2024.002_1.04
    public class xmlEventoCTe_v400

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

            [XmlElement(ElementName = "procEventoCTe", Namespace = "http://www.portalfiscal.inf.br/cte")]
            public procEventoCTe procEventoCTe { get; set; }

            [XmlElement(ElementName = "evento")]
            public evento evento { get; set; }

        }

        #region "procEventoCTe"

        [Serializable]
        [XmlRoot(ElementName = "procEventoCTe", Namespace = "http://www.portalfiscal.inf.br/cte")]
        public class procEventoCTe

        {

            [XmlElement(ElementName = "eventoCTe", Namespace = "http://www.portalfiscal.inf.br/cte")]

            public eventoCTe eventoCTe { get; set; }

            [XmlElement(ElementName = "retEventoCTe", Namespace = "http://www.portalfiscal.inf.br/cte")]

            public retEventoCTe retEventoCTe { get; set; }

            [XmlAttribute(AttributeName = "versao")]

            public string versao { get; set; }

            [XmlAttribute(AttributeName = "ipTransmissor")]
            public string ipTransmissor { get; set; }

            [XmlAttribute(AttributeName = "nPortaCon")]
            public string nPortaCon { get; set; }



        }
      
        #region eventoCTe

        [XmlRoot(ElementName = "eventoCTe", Namespace = "http://www.portalfiscal.inf.br/cte")]
        public class eventoCTe

        {
            [XmlAttribute(AttributeName = "xmlns")]
            public string xmlns { get; set; }

            [XmlAttribute(AttributeName = "versao")]
            public string versao { get; set; }
        
            [XmlElement(ElementName = "infEvento")]
            public infEvento infEvento { get; set; }

            [XmlElement(ElementName = "Signature", Namespace = "http://www.w3.org/2000/09/xmldsig#")]
            public Signature Signature { get; set; }

        }

        #region infEvento
        [Serializable]
        
        public class infEvento

        {

            public infEvento()
            {
                infSolicNFF = new infSolicNFF();
                infPAA = new infPAA();
            }

            [XmlAttribute(AttributeName = "Id")]
            public string Id { get; set; }

            public string cOrgao { get; set; }

            public string tpAmb { get; set; }

            public string CNPJ { get; set; }
            public string CPF { get; set; }

            public string chCTe { get; set; }

            public string dhEvento { get; set; }

            public string tpEvento { get; set; }

            public string nSeqEvento { get; set; }
                       
            public infSolicNFF infSolicNFF { get; set; }

            public infPAA infPAA { get; set; }


        }


        #region infSolicNFF

        [Serializable]
        public class infSolicNFF

        {

            public string xSolic { get; set; }


        }

        #endregion

        #region infPAA

        [Serializable]
        public class infPAA

        {
            public infPAA()
            {
                PAASignature = new PAASignature();
            }


            public string CNPJPAA { get; set; }

            public PAASignature PAASignature { get; set; }

        }

        #region PAASignature

        [Serializable]
        public class PAASignature

        {

            public string SignatureValue { get; set; }

            public string RSAKeyValue { get; set; }


        }

        #endregion

        #endregion

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


        #endregion

        #region "retEventoCTe"
        [Serializable]
        [XmlRoot(ElementName = "retEventoCTe", Namespace = "http://www.portalfiscal.inf.br/cte")]

        public class retEventoCTe

        {

            [XmlAttribute(AttributeName = "xmlns")]
            public string xmlns { get; set; }
            [XmlAttribute(AttributeName = "versao")]
            public string versao { get; set; }

            [XmlElement(ElementName = "infEvento", Namespace = "http://www.portalfiscal.inf.br/cte")]
            public retInfEvento retInfEvento { get; set; }



        }


        [XmlRoot(ElementName = "infEvento", Namespace = "http://www.portalfiscal.inf.br/cte")]
        public class retInfEvento

        {

            public string TpAmb { get; set; }

            public string verAplic { get; set; }

            public string cOrgao { get; set; }

            public string cStat { get; set; }

            public string xMotivo { get; set; }

            public string chCTe { get; set; }

            public string tpEvento { get; set; }

            public string xEvento { get; set; }

            public string nSeqEvento { get; set; }

            public string dhRegEvento { get; set; }

            public string nProt { get; set; }

            [XmlAttribute(AttributeName = "Id")]
            public string Id { get; set; }



        }

        #endregion

        #endregion

        #region "evento"

        [XmlRoot(ElementName = "evento", Namespace = "http://www.portalfiscal.inf.br/cte")]
        public class evento

        {
            
            public string cOrgao { get; set; }

            public string chCTe { get; set; }

            public string CNPJ { get; set; }

            public string dhEvento { get; set; }

            public string tpEvento { get; set; }
            
            public string nSeqEvento { get; set; }

            public string dhRecbto { get; set; }

            public string nProt { get; set; }


        }

        #endregion

    }

}