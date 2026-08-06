using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Common.SerializableClasses.CTe
{
    public class xmlCTeSimp_v400
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

            [XmlElement(ElementName = "cteSimpProc", Namespace = "http://www.portalfiscal.inf.br/cte")]
            public CteSimpProc CteSimpProc { get; set; }
        }

        [Serializable]
        [XmlRoot(ElementName = "cteSimpProc", Namespace = "http://www.portalfiscal.inf.br/cte")]
        public class CteSimpProc
        {
            [XmlElement(ElementName = "CTeSimp", Namespace = "http://www.portalfiscal.inf.br/cte")]
            public CTeSimp CTeSimp { get; set; }

            [XmlElement(ElementName = "protCTe", Namespace = "http://www.portalfiscal.inf.br/cte")]
            public protCTe protCTe { get; set; }

            [XmlAttribute(AttributeName = "versao")]
            public string versao { get; set; }

            [XmlAttribute(AttributeName = "xmlns")]
            public string xmlns { get; set; }
        }

        #region "CTeSimp"

        [XmlRoot(ElementName = "CTeSimp", Namespace = "http://www.portalfiscal.inf.br/cte")]
        public class CTeSimp
        {
            [XmlElement(ElementName = "infCte", Namespace = "http://www.portalfiscal.inf.br/cte")]
            public infCte infCte { get; set; }

            [XmlElement(ElementName = "infCTeSupl", Namespace = "http://www.portalfiscal.inf.br/cte")]
            public infCTeSupl infCTeSupl { get; set; }

            [XmlElement(ElementName = "Signature", Namespace = "http://www.w3.org/2000/09/xmldsig#")]
            public Signature Signature { get; set; }

            [XmlAttribute(AttributeName = "xmlns")]
            public string xmlns { get; set; }
        }

        #region "infCTe"

        [Serializable]
        public class infCte
        {
            public infCte()
            {
                ide = new ide();
                compl = new compl();
                emit = new emit();
                toma = new toma();
                infCarga = new infCarga();
                det = new List<det>();
                infModal = new infModal();
                cobr = new cobr();
                infCteSub = new infCteSub();
                imp = new imp();
                total = new total();
                autXML = new List<autXML>();
                infRespTec = new infRespTec();
                infSolicNFF = new infSolicNFF();
                infPAA = new infPAA();
            }

            public ide ide { get; set; }

            public compl compl { get; set; }

            public emit emit { get; set; }

            public toma toma { get; set; }

            public infCarga infCarga { get; set; }

            [XmlElement(ElementName = "det")]
            public List<det> det { get; set; }

            public infModal infModal { get; set; }

            public cobr cobr { get; set; }

            public infCteSub infCteSub { get; set; }

            public imp imp { get; set; }

            public total total { get; set; }

            [XmlElement(ElementName = "autXML")]
            public List<autXML> autXML { get; set; }

            public infRespTec infRespTec { get; set; }

            public infSolicNFF infSolicNFF { get; set; }

            public infPAA infPAA { get; set; }

            [XmlAttribute(AttributeName = "versao")]
            public string versao { get; set; }

            [XmlAttribute(AttributeName = "Id")]
            public string Id { get; set; }
        }

        #region ide

        [Serializable]
        public class ide
        {
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

            public string procEmi { get; set; }

            public string verProc { get; set; }

            public string cMunEnv { get; set; }

            public string xMunEnv { get; set; }

            public string UFEnv { get; set; }

            public string modal { get; set; }

            public string tpServ { get; set; }

            public string UFIni { get; set; }

            public string UFFim { get; set; }

            public string retira { get; set; }

            public string xDetRetira { get; set; }

            public string dhCont { get; set; }

            public string xJust { get; set; }
        }

        #endregion

        #region compl

        [Serializable]
        public class compl
        {
            public compl()
            {
                fluxo = new fluxo();
                ObsCont = new List<ObsCont>();
                ObsFisco = new List<ObsFisco>();
            }

            public string xCaracAd { get; set; }

            public string xCaracSer { get; set; }

            public fluxo fluxo { get; set; }

            public string xObs { get; set; }

            [XmlElement(ElementName = "ObsCont")]
            public List<ObsCont> ObsCont { get; set; }

            [XmlElement(ElementName = "ObsFisco")]
            public List<ObsFisco> ObsFisco { get; set; }
        }

        #region fluxo

        [Serializable]
        public class fluxo
        {
            public fluxo()
            {
                pass = new List<pass>();
            }

            public string xOrig { get; set; }

            [XmlElement(ElementName = "pass")]
            public List<pass> pass { get; set; }

            public string xDest { get; set; }

            public string xRota { get; set; }
        }

        [Serializable]
        public class pass
        {
            public string xPass { get; set; }
        }

        #endregion

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

            [XmlElement(ElementName = "toma")]
            public string tomaStr { get; set; }

            public string indIEToma { get; set; }

            public string CNPJ { get; set; }

            public string CPF { get; set; }

            public string IE { get; set; }

            public string xNome { get; set; }

            public string ISUF { get; set; }

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
        }

        #endregion

        #region "infCarga"

        [Serializable]
        public class infCarga
        {
            public infCarga()
            {
                infQ = new infQ();
            }

            public decimal? vCarga { get; set; }

            public string proPred { get; set; }

            public string xOutCat { get; set; }

            public infQ infQ { get; set; }

            public decimal? vCargaAverb { get; set; }
        }

        #region "infQ"

        [Serializable]
        public class infQ
        {
            public string cUnid { get; set; }

            public string tpMed { get; set; }

            public decimal? qCarga { get; set; }
        }

        #endregion

        #endregion

        #region "det"

        [Serializable]
        public class det
        {
            public det()
            {
                Comp = new List<Comp>();
                infNFe = new List<infNFe>();
                infDocAnt = new List<infDocAnt>();
            }

            public string cMunIni { get; set; }

            public string xMunIni { get; set; }

            public string cMunFim { get; set; }

            public string xMunFim { get; set; }

            public decimal? vPrest { get; set; }

            public decimal? vRec { get; set; }


            [XmlElement(ElementName = "Comp")]
            public List<Comp> Comp { get; set; }

            [XmlElement(ElementName = "infNFe")]
            public List<infNFe> infNFe { get; set; }

            [XmlElement(ElementName = "infDocAnt")]
            public List<infDocAnt> infDocAnt { get; set; }

            [XmlAttribute(AttributeName = "nItem")]
            public string nItem { get; set; }
        }

        #region "Comp"

        [Serializable]
        public class Comp
        {
            public string xNome { get; set; }

            public decimal? vComp { get; set; }
        }

        #endregion

        #region "infNFe"

        [Serializable]
        public class infNFe
        {
            public infNFe()
            {
                infUnidCarga = new List<infUnidCarga>();
                infUnidTransp = new List<infUnidTransp>();
            }

            public string chNFe { get; set; }

            public string PIN { get; set; }

            public string dPrev { get; set; }

            [XmlElement(ElementName = "infUnidCarga")]
            public List<infUnidCarga> infUnidCarga { get; set; }

            [XmlElement(ElementName = "infUnidTransp")]
            public List<infUnidTransp> infUnidTransp { get; set; }
        }

        #region infUnidCarga

        [Serializable]
        public class infUnidCarga
        {
            public infUnidCarga()
            {
                lacUnidCarga = new List<lacUnidCarga>();
            }

            public string tpUnidCarga { get; set; }

            public string idUnidCarga { get; set; }

            [XmlElement(ElementName = "lacUnidCarga")]
            public List<lacUnidCarga> lacUnidCarga { get; set; }

            public decimal? qtdRat { get; set; }
        }

        [Serializable]
        public class lacUnidCarga
        {
            public string nLacre { get; set; }
        }

        #endregion

        #region infUnidTransp

        [Serializable]
        public class infUnidTransp
        {
            public infUnidTransp()
            {
                lacUnidTransp = new List<lacUnidTransp>();
                infUnidCarga = new List<infUnidCarga>();
            }

            public string tpUnidTransp { get; set; }

            public string idUnidTransp { get; set; }

            [XmlElement(ElementName = "lacUnidTransp")]
            public List<lacUnidTransp> lacUnidTransp { get; set; }

            [XmlElement(ElementName = "infUnidCarga")]
            public List<infUnidCarga> infUnidCarga { get; set; }

            public decimal? qtdRat { get; set; }
        }

        [Serializable]
        public class lacUnidTransp
        {
            public string nLacre { get; set; }
        }

        #endregion

        #endregion

        #region "infDocAnt"

        [Serializable]
        public class infDocAnt
        {
            public infDocAnt()
            {
                infNFeTranspParcial = new List<infNFeTranspParcial>();
            }

            public string chCTe { get; set; }

            public string tpPrest { get; set; }

            [XmlElement(ElementName = "infNFeTranspParcial")]
            public List<infNFeTranspParcial> infNFeTranspParcial { get; set; }
        }

        [Serializable]
        public class infNFeTranspParcial
        {
            public string chNFe { get; set; }
        }

        #endregion

        #endregion

        #region infModal

        [Serializable]
        public class infModal
        {
            public infModal()
            {
                rodo = new rodo();
                aereo = new aereo();
                aquav = new aquav();
                ferrov = new ferrov();
                duto = new duto();
                multimodal = new multimodal();
            }

            public rodo rodo { get; set; }

            public aereo aereo { get; set; }

            public aquav aquav { get; set; }

            public ferrov ferrov { get; set; }

            public duto duto { get; set; }

            public multimodal multimodal { get; set; }

            [XmlAttribute(AttributeName = "versaoModal")]
            public string versaoModal { get; set; }
        }

        #region rodo

        [Serializable]
        public class rodo
        {
            public rodo()
            {
                occ = new List<occ>();
            }

            public string RNTRC { get; set; }

            [XmlElement(ElementName = "occ")]
            public List<occ> occ { get; set; }
        }

        [Serializable]
        public class emiOcc
        {
            public string CNPJ { get; set; }

            public string cInt { get; set; }

            public string IE { get; set; }

            public string UF { get; set; }

            public string fone { get; set; }
        }

        [Serializable]
        public class occ
        {
            public string serie { get; set; }

            public string nOcc { get; set; }

            public string dEmi { get; set; }

            public emiOcc emiOcc { get; set; }
        }

        #endregion

        #region aereo

        [Serializable]
        public class aereo
        {
            public aereo()
            {
                natCarga = new natCarga();
                tarifa = new tarifa();
                peri = new List<peri>();
            }

            public string nMinu { get; set; }

            public string nOCA { get; set; }

            public string dPrevAereo { get; set; }

            public natCarga natCarga { get; set; }

            public tarifa tarifa { get; set; }

            [XmlElement(ElementName = "peri")]
            public List<peri> peri { get; set; }
        }

        [Serializable]
        public class natCarga
        {
            public natCarga()
            {
                cInfManu = new List<string>();
            }

            public string xDime { get; set; }

            [XmlElement(ElementName = "cInfManu")]
            public List<string> cInfManu { get; set; }
        }

        [Serializable]
        public class tarifa
        {
            public string CL { get; set; }

            public string cTar { get; set; }

            public decimal? vTar { get; set; }
        }

        [Serializable]
        public class peri
        {
            public peri()
            {
                infTotAP = new infTotAP();
            }

            public string nONU { get; set; }

            public string qTotEmb { get; set; }

            public infTotAP infTotAP { get; set; }
        }

        [Serializable]
        public class infTotAP
        {
            public decimal? qTotProd { get; set; }

            public string uniAP { get; set; }
        }

        #endregion

        #region aquav

        [Serializable]
        public class aquav
        {
            public aquav()
            {
                balsa = new List<balsa>();
                detCont = new List<detCont>();
            }

            public decimal? vPrest { get; set; }

            public decimal? vAFRMM { get; set; }

            public string xNavio { get; set; }

            [XmlElement(ElementName = "balsa")]
            public List<balsa> balsa { get; set; }

            public string nViag { get; set; }

            public string direc { get; set; }

            public string irin { get; set; }

            [XmlElement(ElementName = "detCont")]
            public List<detCont> detCont { get; set; }

            public string tpNav { get; set; }
        }

        [Serializable]
        public class balsa
        {
            public string xBalsa { get; set; }
        }

        [Serializable]
        public class detCont
        {
            public detCont()
            {
                lacre = new List<lacre>();
                infDoc = new infDocaquav();
            }

            public string nCont { get; set; }

            [XmlElement(ElementName = "lacre")]
            public List<lacre> lacre { get; set; }

            [XmlElement(ElementName = "infDoc")]
            public infDocaquav infDoc { get; set; }
        }

        [Serializable]
        public class lacre
        {
            public string nLacre { get; set; }
        }

        [Serializable]
        public class infDocaquav
        {
            public infDocaquav()
            {
                infNF = new List<infNFaquav>();
                infNFe = new List<infNFeaquav>();
            }

            [XmlElement(ElementName = "infNF")]
            public List<infNFaquav> infNF { get; set; }

            [XmlElement(ElementName = "infNFe")]
            public List<infNFeaquav> infNFe { get; set; }
        }

        [Serializable]
        public class infNFaquav
        {
            public string serie { get; set; }

            public string nDoc { get; set; }

            public decimal? unidRat { get; set; }
        }

        [Serializable]
        public class infNFeaquav
        {
            public string chave { get; set; }

            public decimal? unidRat { get; set; }
        }

        #endregion

        #region ferrov

        [Serializable]
        public class ferrov

        {
            public ferrov()
            {
                trafMut = new trafMut();
            }
            public string tpTraf { get; set; }

            public trafMut trafMut { get; set; }

            public string fluxo { get; set; }

        }


        [Serializable]
        public class trafMut

        {
            public trafMut()
            {
                ferroEnv = new List<ferroEnv>();
            }
            public string respFat { get; set; }

            public string ferrEmi { get; set; }

            public decimal? vFrete { get; set; }

            public string chCTeFerroOrigem { get; set; }

            [XmlElement(ElementName = "ferroEnv")]
            public List<ferroEnv> ferroEnv { get; set; }

            public string fluxo { get; set; }
        }


        [Serializable]
        public class ferroEnv
        {
            public ferroEnv()
            {
                enderFerro = new enderFerro();
            }

            public string CNPJ { get; set; }

            public string cInt { get; set; }

            public string IE { get; set; }

            public string xNome { get; set; }

            public enderFerro enderFerro { get; set; }

        }


        [Serializable]
        public class enderFerro
        {

            public string xLgr { get; set; }

            public string nro { get; set; }

            public string xCpl { get; set; }

            public string xBairro { get; set; }

            public string cMun { get; set; }

            public string xMun { get; set; }

            public string CEP { get; set; }

            public string UF { get; set; }

        }



        #endregion

        #region duto

        [Serializable]
        public class duto
        {
            public decimal? vTar { get; set; }

            public string dIni { get; set; }

            public string dFim { get; set; }
        }

        #endregion

        #region multimodal

        [Serializable]
        public class multimodal
        {
            public multimodal()
            {
                seg = new seg();
            }

            public string COTM { get; set; }

            public string indNegociavel { get; set; }

            public seg seg { get; set; }

        }


        [Serializable]
        public class seg
        {
            public seg()
            {
                infSeg = new infSeg();
            }

            public infSeg infSeg { get; set; }

            public string nApol { get; set; }

            public string nAver { get; set; }

        }


        [Serializable]
        public class infSeg
        {
            public string xSeg { get; set; }

            public string CNPJ { get; set; }
        }



        #endregion

        #endregion

        #region cobr

        [Serializable]
        public class cobr
        {
            public cobr()
            {
                fat = new fat();
                dup = new List<dup>();
            }

            public fat fat { get; set; }

            [XmlElement(ElementName = "dup")]
            public List<dup> dup { get; set; }
        }


        [Serializable]
        public class fat
        {
            public string nFat { get; set; }

            public decimal? vOrig { get; set; }

            public decimal? vDesc { get; set; }

            public decimal? vLiq { get; set; }
        }

        [Serializable]
        public class dup
        {
            public string nDup { get; set; }

            public string dVenc { get; set; }

            public decimal? vDup { get; set; }
        }

        #endregion

        #region infCTeSub

        [Serializable]
        public class infCteSub
        {
            public string chCte { get; set; }

            public string indAlteraToma { get; set; }
        }

        #endregion

        #region imp

        [Serializable]
        public class imp
        {
            public imp()
            {
                ICMS = new ICMS();
                ICMSUFFim = new ICMSUFFim();
            }

            public ICMS ICMS { get; set; }

            public decimal? vTotTrib { get; set; }

            public string InfAdFisco { get; set; }

            public ICMSUFFim ICMSUFFim { get; set; }
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
                ICMS60 = new ICMS60();
                ICMS90 = new ICMS90();
                ICMSOutraUF = new ICMSOutraUF();
                ICMSSN = new ICMSSN();
            }

            public ICMS00 ICMS00 { get; set; }

            public ICMS20 ICMS20 { get; set; }

            public ICMS45 ICMS45 { get; set; }

            public ICMS60 ICMS60 { get; set; }

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
        public class ICMS60
        {
            public string CST { get; set; }

            public decimal? vBCSTRet { get; set; }

            public decimal? vICMSSTRet { get; set; }

            public decimal? pICMSSTRet { get; set; }

            public decimal? vCred { get; set; }

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

        #region ICMSUFFim

        [Serializable]
        public class ICMSUFFim
        {
            public decimal? vBCUFFim { get; set; }

            public decimal? pFCPUFFim { get; set; }

            public decimal? pICMSUFFim { get; set; }

            public decimal? pICMSInter { get; set; }

            public decimal? vFCPUFFim { get; set; }

            public decimal? vICMSUFFim { get; set; }

            public decimal? vICMSUFIni { get; set; }
        }

        #endregion

        #endregion

        #region "total"

        [Serializable]
        public class total
        {
            public decimal? vTPrest { get; set; }

            public decimal? vTRec { get; set; }
        }

        #endregion

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

        #endregion

        #region PAASignature

        [Serializable]
        public class PAASignature
        {
            public string SignatureValue { get; set; }

            public string RSAKeyValue { get; set; }
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
