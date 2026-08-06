using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Common.SerializableClasses.CTe

{
    public class ClsCTeOS_v4
    {
        [XmlRoot(ElementName = "proc", Namespace = "http://www.portalfiscal.inf.br/cte")]
        public class proc
        {
            [XmlAttribute(AttributeName = "schema")]
            public string schema { get; set; }

            [XmlAttribute(AttributeName = "NSUSVD")]
            public string NSUSVD { get; set; }

            [XmlAttribute(AttributeName = "NSUAut")]
            public string NSUAut { get; set; }

            [XmlElement(ElementName = "cteOSProc", Namespace = "http://www.portalfiscal.inf.br/cte")]
            public cteOSProc cteOSProc { get; set; }
        }

        [Serializable]
        [XmlRoot(ElementName = "cteOSProc", Namespace = "http://www.portalfiscal.inf.br/cte")]
        public class cteOSProc
        {
            [XmlAttribute(AttributeName = "versao")]
            public string versao { get; set; }

            [XmlElement(ElementName = "CTeOS", Namespace = "http://www.portalfiscal.inf.br/cte")]
            public CTeOS CTeOS { get; set; }


            [XmlElement(ElementName = "protCTe", Namespace = "http://www.portalfiscal.inf.br/cte")]

            public protCTe protCTe { get; set; }


        }

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

        [Serializable]
        public class CTeOS
        {
            [XmlElement(ElementName = "infCte")]
            public infCte infCte { get; set; }

            [XmlElement(ElementName = "infCTeSupl", Namespace = "http://www.portalfiscal.inf.br/cte")]
            public infCTeSupl infCTeSupl { get; set; }

            [XmlElement(ElementName = "Signature", Namespace = "http://www.w3.org/2000/09/xmldsig#")]
            public Signature Signature { get; set; }
        }
        
        [Serializable]
        public class infCte
        {

            public infCte()
            {
                ide = new ide();
                compl = new compl();
                emit = new emit();
                toma = new toma();
                vPrest = new vPrest();
                imp = new imp();
                infCTeNorm = new infCTeNorm();
                infCteComp = new List<infCteComp>();
                autXML = new List<autXML>();
                infRespTec = new infRespTec();
            }

            [XmlAttribute(AttributeName = "versao")]
            public string versao { get; set; }

            [XmlAttribute(AttributeName = "id")]
            public string id { get; set; }
            public ide ide { get; set; }
            public compl compl { get; set; }

            public emit emit { get; set; }

            public toma toma { get; set; }

            public vPrest vPrest { get; set; }

            public imp imp { get; set; }

            public infCTeNorm infCTeNorm { get; set; }

            [XmlElement(ElementName = "infCteComp")]
            public List<infCteComp> infCteComp { get; set; }

            [XmlElement(ElementName = "autXML")]
            public List<autXML> autXML { get; set; }

            public infRespTec infRespTec { get; set; }
        }

        #region "ide"

        [Serializable]
        public class ide
        {

            public ide()
            {
                infPercurso = new List<infPercurso>();
            }

            [XmlElement(ElementName = "cUF")]
            public string cUF { get; set; }

            [XmlElement(ElementName = "cCT")]
            public string cCT { get; set; }

            [XmlElement(ElementName = "CFOP")]
            public string CFOP { get; set; }

            [XmlElement(ElementName = "natOp")]
            public string natOp { get; set; }

            [XmlElement(ElementName = "mod")]
            public string mod { get; set; }

            [XmlElement(ElementName = "serie")]
            public string serie { get; set; }

            [XmlElement(ElementName = "nCT")]
            public string nCT { get; set; }

            [XmlElement(ElementName = "dhEmi")]
            public string dhEmi { get; set; }

            [XmlElement(ElementName = "tpImp")]
            public string tpImp { get; set; }

            [XmlElement(ElementName = "tpEmis")]
            public string tpEmis { get; set; }

            [XmlElement(ElementName = "cDV")]
            public string cDV { get; set; }

            [XmlElement(ElementName = "tpAmb")]
            public string tpAmb { get; set; }

            [XmlElement(ElementName = "tpCTe")]
            public string tpCTe { get; set; }

            [XmlElement(ElementName = "procEmi")]
            public string procEmi { get; set; }

            [XmlElement(ElementName = "verProc")]
            public string verProc { get; set; }

            [XmlElement(ElementName = "cMunEnv")]
            public string cMunEnv { get; set; }

            [XmlElement(ElementName = "xMunEnv")]
            public string xMunEnv { get; set; }

            [XmlElement(ElementName = "UFEnv")]
            public string UFEnv { get; set; }

            [XmlElement(ElementName = "modal")]
            public string modal { get; set; }

            [XmlElement(ElementName = "tpServ")]
            public string tpServ { get; set; }

            [XmlElement(ElementName = "indIEToma")]
            public string indIEToma { get; set; }

            [XmlElement(ElementName = "cMunIni")]
            public string cMunIni { get; set; }

            [XmlElement(ElementName = "xMunIni")]
            public string xMunIni { get; set; }

            [XmlElement(ElementName = "UFIni")]
            public string UFIni { get; set; }

            [XmlElement(ElementName = "cMunFim")]
            public string cMunFim { get; set; }

            [XmlElement(ElementName = "xMunFim")]
            public string xMunFim { get; set; }

            [XmlElement(ElementName = "UFFim")]
            public string UFFim { get; set; }

            [XmlElement(ElementName = "infPercurso")]
            public List<infPercurso> infPercurso { get; set; }

            [XmlElement(ElementName = "dhCont")]
            public string dhCont { get; set; }

            [XmlElement(ElementName = "xJust")]
            public string xJust { get; set; }
        }

        [Serializable]
        public class infPercurso
        {
            [XmlElement(ElementName = "UFPer")]
            public string UFPer { get; set; }
        }

        #endregion

        #region "compl"

        [Serializable]
        public class compl
        {

            public compl()
            {
                ObsCont = new List<ObsCont>();
                ObsFisco = new List<ObsFisco>();
            }

            [XmlElement(ElementName = "xCaracAd")]
            public string xCaracAd { get; set; }

            [XmlElement(ElementName = "xCaracSer")]
            public string xCaracSer { get; set; }

            [XmlElement(ElementName = "xEmi")]
            public string xEmi { get; set; }

            [XmlElement(ElementName = "xObs")]
            public string xObs { get; set; }

            [XmlElement(ElementName = "ObsCont")]
            public List<ObsCont> ObsCont { get; set; }

            [XmlElement(ElementName = "ObsFisco")]
            public List<ObsFisco> ObsFisco { get; set; }
        }

        [Serializable]
        public class ObsCont

        {

            [XmlAttribute(AttributeName = "xCampo")]
            public string xCampo { get; set; }

            public string xTexto { get; set; }

        }

        [Serializable]
        public class ObsFisco

        {

            [XmlAttribute(AttributeName = "xCampo")]
            public string xCampo { get; set; }

            public string xTexto { get; set; }
        }

        #endregion

        #region "emit"

        [Serializable]
        public class emit
        {

            [XmlElement(ElementName = "CNPJ")]
            public string CNPJ { get; set; }

            [XmlElement(ElementName = "IE")]
            public string IE { get; set; }

            [XmlElement(ElementName = "IEST")]
            public string IEST { get; set; }

            [XmlElement(ElementName = "xNome")]
            public string xNome { get; set; }

            [XmlElement(ElementName = "xFant")]
            public string xFant { get; set; }

            [XmlElement(ElementName = "enderEmit")]
            public enderEmit enderEmit { get; set; }

            [XmlElement(ElementName = "CRT")]
            public string CRT { get; set; }
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

        #region "toma"

        [Serializable]
        public class toma
        {
            public toma()
            {
                enderToma = new enderToma();
            }

            [XmlElement(ElementName = "CNPJ")]
            public string CNPJ { get; set; }

            [XmlElement(ElementName = "CPF")]
            public string CPF { get; set; }

            [XmlElement(ElementName = "IE")]
            public string IE { get; set; }

            [XmlElement(ElementName = "xNome")]
            public string xNome { get; set; }

            [XmlElement(ElementName = "xFant")]
            public string xFant { get; set; }

            [XmlElement(ElementName = "fone")]
            public string fone { get; set; }

            [XmlElement(ElementName = "enderToma")]
            public enderToma enderToma { get; set; }

            [XmlElement(ElementName = "email")]
            public string email { get; set; }
        }

        #region "enderToma"

        [Serializable]
        public class enderToma
        {
            [XmlElement(ElementName = "xLgr")]
            public string xLgr { get; set; }

            [XmlElement(ElementName = "nro")]
            public string nro { get; set; }

            [XmlElement(ElementName = "xCpl")]
            public string xCpl { get; set; }

            [XmlElement(ElementName = "xBairro")]
            public string xBairro { get; set; }

            [XmlElement(ElementName = "cMun")]
            public string cMun { get; set; }

            [XmlElement(ElementName = "xMun")]
            public string xMun { get; set; }

            [XmlElement(ElementName = "CEP")]
            public string CEP { get; set; }

            [XmlElement(ElementName = "UF")]
            public string UF { get; set; }

            [XmlElement(ElementName = "cPais")]
            public string cPais { get; set; }

            [XmlElement(ElementName = "xPais")]
            public string xPais { get; set; }
        }

        #endregion

        #endregion

        #region "vPrest"

        [Serializable]
        public class vPrest
        {
            public vPrest()
            {
                Comp = new List<Comp>();
            }

            [XmlElement(ElementName = "vTPrest")]
            public decimal? vTPrest { get; set; }

            [XmlElement(ElementName = "vRec")]
            public decimal? vRec { get; set; }

            [XmlElement(ElementName = "Comp")]
            public List<Comp> Comp { get; set; }
        }

        #region "Comp"

        [Serializable]
        public class Comp
        {
            [XmlElement(ElementName = "xNome")]
            public string xNome { get; set; }

            [XmlElement(ElementName = "vComp")]
            public decimal? vComp { get; set; }

        }

        #endregion

        #endregion

        #region "imp"

        [Serializable]
        public class imp
        {

            public imp()
            {
                ICMS = new ICMS();
                ICMSUFFim = new ICMSUFFim();
                infTribFed = new infTribFed();

            }

            [XmlElement(ElementName = "ICMS")]
            public ICMS ICMS { get; set; }

            [XmlElement(ElementName = "vTotTrib")]
            public decimal? vTotTrib { get; set; }

            [XmlElement(ElementName = "infAdFisco")]
            public string infAdFisco { get; set; }

            [XmlElement(ElementName = "ICMSUFFim")]
            public ICMSUFFim ICMSUFFim { get; set; }

            [XmlElement(ElementName = "infTribFed")]
            public infTribFed infTribFed { get; set; }
        }

        #region ICMS
        [Serializable]

        public class ICMS

        {
            public ICMS()
            {
                ICMS00 = new ICMS00();
                ICMS20 = new ICMS20();
                ICMS45 = new ICMS45();
                ICMS90 = new ICMS90();
                ICMSOutraUF = new ICMSOutraUF();
                ICMSSN = new ICMSSN();
            }

            public ICMS00 ICMS00 { get; set; }

            public ICMS20 ICMS20 { get; set; }

            public ICMS45 ICMS45 { get; set; }

            public ICMS90 ICMS90 { get; set; }

            public ICMSOutraUF ICMSOutraUF { get; set; }

            public ICMSSN ICMSSN { get; set; }

        }



        [Serializable]

        public class ICMS00

        {

            public string CST { get; set; }

            public decimal? vBC { get; set; }

            public decimal? pICMS { get; set; }

            public decimal? vICMS { get; set; }

        }



        [Serializable]

        public class ICMS20

        {

            public string CST { get; set; }


            public decimal? pRedBC { get; set; }


            public decimal? vBC { get; set; }


            public decimal? pICMS { get; set; }


            public decimal? vICMS { get; set; }

            public decimal? vICMSDeson { get; set; }

            public string cBenef { get; set; }

        }



        [Serializable]

        public class ICMS45

        {

            public string CST { get; set; }

            public decimal? vICMSDeson { get; set; }

            public string cBenef { get; set; }

        }


        [Serializable]

        public class ICMS90

        {
            public string CST { get; set; }


            public decimal? pRedBC { get; set; }


            public decimal? vBC { get; set; }


            public decimal? pICMS { get; set; }


            public decimal? vICMS { get; set; }


            public decimal? vCred { get; set; }

            public decimal? vICMSDeson { get; set; }

            public string cBenef { get; set; }

        }



        [Serializable]

        public class ICMSOutraUF

        {

            public string CST { get; set; }


            public decimal? pRedBCOutraUF { get; set; }


            public decimal? vBCOutraUF { get; set; }


            public decimal? pICMSOutraUF { get; set; }


            public decimal? vICMSOutraUF { get; set; }

            public decimal? vICMSDeson { get; set; }

            public string cBenef { get; set; }

        }



        [Serializable]

        public class ICMSSN

        {


            public string CST { get; set; }



            public string indSN { get; set; }

        }

        #endregion

        [Serializable]
        public class ICMSUFFim
        {
            [XmlElement(ElementName = "vBCUFFim")]
            public decimal? vBCUFFim { get; set; }

            [XmlElement(ElementName = "pFCPUFFim")]
            public decimal? pFCPUFFim { get; set; }

            [XmlElement(ElementName = "pICMSUFFim")]
            public decimal? pICMSUFFim { get; set; }

            [XmlElement(ElementName = "pICMSInter")]
            public decimal? pICMSInter { get; set; }

            [XmlElement(ElementName = "vFCPUFFim")]
            public decimal? vFCPUFFim { get; set; }

            [XmlElement(ElementName = "vICMSUFFim")]
            public decimal? vICMSUFFim { get; set; }

            [XmlElement(ElementName = "vICMSUFIni")]
            public decimal? vICMSUFIni { get; set; }
        }

        [Serializable]
        public class infTribFed
        {
            [XmlElement(ElementName = "vPIS")]
            public decimal? vPIS { get; set; }

            [XmlElement(ElementName = "vCOFINS")]
            public decimal? vCOFINS { get; set; }

            [XmlElement(ElementName = "vIR")]
            public decimal? vIR { get; set; }

            [XmlElement(ElementName = "vINSS")]
            public decimal? vINSS { get; set; }

            [XmlElement(ElementName = "vCSLL")]
            public decimal? vCSLL { get; set; }
        }

        #endregion

        #region "infCTeNorm"

        [Serializable]
        public class infCTeNorm
        {

            public infCTeNorm()
            {

                infServico = new infServico();
                infDocRef = new List<infDocRef>();
                seg = new List<seg>();
                infModal = new infModal();
                infCteSub = new infCteSub();
                cobr = new cobr();
                infGTVe = new List<infGTVe>();
            }

            [XmlElement(ElementName = "infServico")]
            public infServico infServico { get; set; }

            [XmlElement(ElementName = "infDocRef")]
            public List<infDocRef> infDocRef { get; set; }

            [XmlElement(ElementName = "seg")]
            public List<seg> seg { get; set; }

            [XmlElement(ElementName = "infModal")]
            public infModal infModal { get; set; }

            [XmlElement(ElementName = "infCteSub")]
            public infCteSub infCteSub { get; set; }

            [XmlElement(ElementName = "refCTeCanc")]
            public string refCTeCanc { get; set; }

            [XmlElement(ElementName = "cobr")]
            public cobr cobr { get; set; }

            [XmlElement(ElementName = "infGTVe")]
            public List<infGTVe> infGTVe { get; set; }
        }
        
        #endregion

        #region "infServico"
        [Serializable]
        public class infServico
        {
            public infServico()
            {
                infQ = new infQ();
            }

            [XmlElement(ElementName = "xDescServ")]
            public string xDescServ { get; set; }

            [XmlElement(ElementName = "infQ")]
            public infQ infQ { get; set; }
        }

        #region "infQ"

        [Serializable]
        public class infQ
        {
            [XmlElement(ElementName = "qCarga")]
            public decimal? qCarga { get; set; }
        }

        #endregion

        #endregion

        #region "infDocRef"

        [Serializable]
        public class infDocRef
        {
            [XmlElement(ElementName = "nDoc")]
            public string nDoc { get; set; }

            [XmlElement(ElementName = "serie")]
            public string serie { get; set; }

            [XmlElement(ElementName = "subserie")]
            public string subserie { get; set; }

            [XmlElement(ElementName = "dEmi")]
            public string dEmi { get; set; }

            [XmlElement(ElementName = "vDoc")]
            public decimal? vDoc { get; set; }

            [XmlElement(ElementName = "chBPe")]
            public string chBPe { get; set; }
        }

        #endregion
        
        #region "seg"

        [Serializable]
        public class seg
        {
            [XmlElement(ElementName = "respSeg")]
            public string respSeg { get; set; }

            [XmlElement(ElementName = "xSeg")]
            public string xSeg { get; set; }

            [XmlElement(ElementName = "nApol")]
            public string nApol { get; set; }
        }

        #endregion

        #region infModal
        [Serializable]
        public class infModal
        {

            public infModal()
            {
                rodoOS = new rodoOS();              
            }

            public rodoOS rodoOS { get; set; }

            [XmlAttribute(AttributeName = "versaoModal")]
            public string versaoModal { get; set; }

        }

        #region rodo

        [Serializable]
        public class rodoOS

        {
            public rodoOS()
            {
                veic = new veic();
                infFretamento = new infFretamento();
            }

            public string TAF { get; set; }

            public string NroRegEstadual { get; set; }

            [XmlElement(ElementName = "veic")]
            public veic veic { get; set; }

            [XmlElement(ElementName = "infFretamento")]
            public infFretamento infFretamento { get; set; }

        }

        [Serializable]
        public class veic
        {
            public veic()
            {
                prop = new prop();
               
            }
            public string placa { get; set; }

            public string RENAVAM { get; set; }

            public prop prop { get; set; }

            public string UF { get; set; }
        }

        public class prop
        {
            public string CPF { get; set; }
            public string CNPJ { get; set; }            
            public string TAF { get; set; }
            public string NroRegEstadual { get; set; }
            public string xNome { get; set; }
            public string IE { get; set; }
            public string UF { get; set; }
            public string tpProp { get; set; }
        }

        [Serializable]
        public class infFretamento
        {

            public string tpFretamento { get; set; }

            public string dhViagem { get; set; }

        }

        #endregion

        #endregion

        #region "infCteSub"

        [Serializable]
        public class infCteSub
        {
            [XmlElement(ElementName = "chCte")]
            public string chCte { get; set; }
        }

        #endregion

        #region "cobr"

        [Serializable]
        public class cobr
        {
            public cobr()
            {

                fat = new fat();
                dup = new List<dup>();
            }

            [XmlElement(ElementName = "fat")]
            public fat fat { get; set; }

            [XmlElement(ElementName = "dup")]
            public List<dup> dup { get; set; }
        }

        #region "fat"

        [Serializable]
        public class fat
        {
            [XmlElement(ElementName = "nFat")]
            public string nFat { get; set; }

            [XmlElement(ElementName = "vOrig")]
            public decimal? vOrig { get; set; }

            [XmlElement(ElementName = "vDesc")]
            public decimal? vDesc { get; set; }

            [XmlElement(ElementName = "vLiq")]
            public decimal? vLiq { get; set; }
        }

        #endregion

        #region "dup"

        [Serializable]
        public class dup
        {
            [XmlElement(ElementName = "nDup")]
            public string nDup { get; set; }

            [XmlElement(ElementName = "dVenc")]
            public string dVenc { get; set; }

            [XmlElement(ElementName = "vDup")]
            public decimal? vDup { get; set; }
        }

        #endregion

        #endregion

        #region "infGTVe"

        [Serializable]
        public class infGTVe
        {
            public infGTVe()
            {
                CompinfGTVe = new List<CompinfGTVe>();
            }

            [XmlElement(ElementName = "chCTe")]
            public string chCte { get; set; }

            [XmlElement(ElementName = "Comp")]
            public List<CompinfGTVe> CompinfGTVe { get; set; }
        }


        [Serializable]
        public class CompinfGTVe
        {
            public string tpComp { get; set; }
            public decimal? vComp { get; set; }
            public string xComp { get; set; }
        }

        #endregion

        #region "infCteComp"

        [Serializable]
        public class infCteComp
        {
            [XmlElement(ElementName = "chCTe")]
            public string chCTe { get; set; }
        }


        #endregion

        #region "autXML"

        [Serializable]
        public class autXML
        {
            [XmlElement(ElementName = "CNPJ")]
            public string CNPJ { get; set; }

            [XmlElement(ElementName = "CPF")]
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


    }
}
