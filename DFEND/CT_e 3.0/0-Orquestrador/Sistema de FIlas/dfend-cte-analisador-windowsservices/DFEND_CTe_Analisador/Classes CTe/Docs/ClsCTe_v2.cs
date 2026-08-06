using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Common.SerializableClasses.CTe

{
    //Ultima atualização: PL_CTe_200a_NT2015.004
    public class ClsCTe_v2
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

            [XmlElement(ElementName = "cteProc", Namespace = "http://www.portalfiscal.inf.br/cte")]
            public CteProc CteProc { get; set; }

            //[XmlElement(ElementName = "CTe")]
            //public evento evento { get; set; }

        }

        [Serializable]
        [XmlRoot(ElementName = "cteProc", Namespace = "http://www.portalfiscal.inf.br/cte")]
        public class CteProc
        {

            [XmlElement(ElementName = "CTe", Namespace = "http://www.portalfiscal.inf.br/cte")]

            public CTe CTe { get; set; }

            [XmlElement(ElementName = "protCTe", Namespace = "http://www.portalfiscal.inf.br/cte")]

            public protCTe protCTe { get; set; }

            [XmlAttribute(AttributeName = "versao")]

            public string versao { get; set; }

            [XmlAttribute(AttributeName = "xmlns")]

            public string xmlns { get; set; }

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

        #region CTe

        [XmlRoot(ElementName = "CTe", Namespace = "http://www.portalfiscal.inf.br/cte")]
        public class CTe

        {
            [XmlElement(ElementName = "infCte", Namespace = "http://www.portalfiscal.inf.br/cte")]
            public infCte infCte { get; set; }

            [XmlElement(ElementName = "Signature", Namespace = "http://www.w3.org/2000/09/xmldsig#")]
            public Signature Signature { get; set; }

            [XmlAttribute(AttributeName = "xmlns")]

            public string xmlns { get; set; }

        }

        #endregion

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
                exped = new exped();
                receb = new receb();
                dest = new dest();
                vPrest = new vPrest();
                imp = new imp();
                infCTeNorm = new infCTeNorm();
                infCteComp = new infCteComp();
                infCteAnu = new infCteAnu();
                autXML = new List<autXML>();

            }



            public ide ide { get; set; }



            public compl compl { get; set; }



            public emit emit { get; set; }



            public rem rem { get; set; }



            public exped exped { get; set; }



            public receb receb { get; set; }



            public dest dest { get; set; }



            public vPrest vPrest { get; set; }



            public imp imp { get; set; }



            public infCTeNorm infCTeNorm { get; set; }


            public infCteComp infCteComp { get; set; }

            public infCteAnu infCteAnu { get; set; }

            [XmlElement(ElementName = "autXML")]
            public List<autXML> autXML { get; set; }


            [XmlAttribute(AttributeName = "versao")]
            public string versao { get; set; }

            [XmlAttribute(AttributeName = "Id")]
            public string Id { get; set; }


        }

        #endregion

        #region ide
        [Serializable]

        public class ide

        {
            public ide()
            {
                toma03 = new toma03();
                toma4 = new toma4();
            }

            public string cUF { get; set; }

            public string cCT { get; set; }

            public string CFOP { get; set; }

            public string natOp { get; set; }

            public string forPag { get; set; }

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

            public string refCTE { get; set; }

            public string cMunEnv { get; set; }

            public string xMunEnv { get; set; }

            public string UFEnv { get; set; }

            public string modal { get; set; }

            public string tpServ { get; set; }

            public string cMunIni { get; set; }

            public string xMunIni { get; set; }

            public string UFIni { get; set; }

            public string cMunFim { get; set; }

            public string xMunFim { get; set; }

            public string UFFim { get; set; }

            public string retira { get; set; }

            public string xDetRetira { get; set; }


            public toma03 toma03 { get; set; }

            public toma4 toma4 { get; set; }

            public string dhCont { get; set; }

            public string xJust { get; set; }

        }

        [Serializable]

        public class toma03

        {

            public string toma { get; set; }

        }


        [Serializable]

        public class toma4
        {
            public toma4()
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
                fluxo = new fluxo();
                Entrega = new Entrega();
            }

            public string xCaracAd { get; set; }



            public string xCaracSer { get; set; }



            public string xEmi { get; set; }



            public fluxo fluxo { get; set; }



            public Entrega Entrega { get; set; }



            public string origCalc { get; set; }



            public string destCalc { get; set; }



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

        #region Entrega
        [Serializable]

        public class Entrega

        {
            public Entrega()
            {
                semData = new semData();
                comData = new comData();
                noPeriodo = new noPeriodo();
                semHora = new semHora();
                comHora = new comHora();
                noInter = new noInter();
            }

            public semData semData { get; set; }

            public comData comData { get; set; }

            public noPeriodo noPeriodo { get; set; }

            public semHora semHora { get; set; }

            public comHora comHora { get; set; }

            public noInter noInter { get; set; }

        }


        [Serializable]

        public class semData

        {

            public string tpPer { get; set; }

        }


        [Serializable]

        public class comData

        {

            public string tpPer { get; set; }

            public string dProg { get; set; }

        }



        [Serializable]

        public class noPeriodo

        {

            public string tpPer { get; set; }

            public string dIni { get; set; }

            public string dFim { get; set; }

        }



        [Serializable]

        public class semHora

        {

            public string tpHor { get; set; }

        }



        [Serializable]

        public class comHora

        {

            public string tpHor { get; set; }

            public string hProg { get; set; }

        }



        [Serializable]

        public class noInter

        {

            public string tpHor { get; set; }


            public string hIni { get; set; }


            public string hFim { get; set; }

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

        }

        #endregion

        #endregion

        #region exped

        [Serializable]

        public class exped

        {
            public exped()
            {
                enderExped = new enderExped();
            }

            public string CNPJ { get; set; }

            public string CPF { get; set; }

            public string IE { get; set; }

            public string xNome { get; set; }

            public string fone { get; set; }

            public enderExped enderExped { get; set; }


            public string email { get; set; }

        }

        [Serializable]

        public class enderExped

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

        #region receb

        [Serializable]

        public class receb

        {
            public receb()
            {
                enderReceb = new enderReceb();
            }

            public string CNPJ { get; set; }


            public string CPF { get; set; }


            public string IE { get; set; }


            public string xNome { get; set; }


            public string fone { get; set; }

            public enderReceb enderReceb { get; set; }


            public string email { get; set; }

        }

        #region enderReceb
        [Serializable]

        public class enderReceb

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

        #region vPrest

        [Serializable]

        public class vPrest

        {

            public vPrest()
            {
                comp = new List<comp>();
            }

            public decimal? vTPrest { get; set; }


            public decimal? vRec { get; set; }

            [XmlElement(ElementName = "Comp")]
            public List<comp> comp { get; set; }

        }

        #region comp
        [Serializable]

        public class comp

        {

            public string xNome { get; set; }

            public string vComp { get; set; }

        }

        #endregion

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


        }


        [Serializable]

        public class ICMS45

        {

            public string CST { get; set; }

        }



        [Serializable]

        public class ICMS60

        {


            public string CST { get; set; }


            public decimal? vBCSTRet { get; set; }


            public decimal? vICMSSTRet { get; set; }


            public decimal? pICMSSTRet { get; set; }


            public decimal? vCred { get; set; }

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


        }



        [Serializable]

        public class ICMSOutraUF

        {

            public string CST { get; set; }


            public decimal? pRedBCOutraUF { get; set; }


            public decimal? vBCOutraUF { get; set; }


            public decimal? pICMSOutraUF { get; set; }


            public decimal? vICMSOutraUF { get; set; }


        }

        [Serializable]

        public class ICMSSN

        {
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

        #region infCTeNorm


        [Serializable]

        public class infCTeNorm

        {
            public infCTeNorm()
            {
                infCarga = new infCarga();
                infDoc = new infDoc();
                docAnt = new docAnt();
                seg = new List<seg>();
                infModal = new infModal();
                peri = new List<peri>();
                veicNovos = new List<veicNovos>();
                cobr = new cobr();
                infCTeSub = new infCTeSub();

            }

            public infCarga infCarga { get; set; }

            public infDoc infDoc { get; set; }

            public docAnt docAnt { get; set; }

            [XmlElement(ElementName = "seg")]
            public List<seg> seg { get; set; }

            public infModal infModal { get; set; }

            [XmlElement(ElementName = "peri")]
            public List<peri> peri { get; set; }

            [XmlElement(ElementName = "veicNovos")]
            public List<veicNovos> veicNovos { get; set; }

            public cobr cobr { get; set; }

            public infCTeSub infCTeSub { get; set; }


        }

        #region infCarga

        [Serializable]
        public class infCarga

        {
            public infCarga()
            {
                infQ = new List<infQ>();
            }

            public decimal? vCarga { get; set; }

            public string proPred { get; set; }

            public string xOutCat { get; set; }

            [XmlElement(ElementName = "infQ")]
            public List<infQ> infQ { get; set; }

        }


        [Serializable]

        public class infQ

        {

            public string cUnid { get; set; }

            public string tpMed { get; set; }

            public decimal? qCarga { get; set; }

        }


        #endregion

        #region infDoc

        [Serializable]
        public class infDoc

        {
            public infDoc()
            {
                infNF = new List<infNF>();
                infNFe = new List<infNFe>();
                infOutros = new List<infOutros>();
            }


            [XmlElement(ElementName = "infNF")]
            public List<infNF> infNF { get; set; }


            [XmlElement(ElementName = "infNFe")]
            public List<infNFe> infNFe { get; set; }


            [XmlElement(ElementName = "infOutros")]
            public List<infOutros> infOutros { get; set; }

        }

        #region infNF


        [Serializable]
        public class infNF

        {
            public infNF()
            {
                infUnidCarga = new List<infUnidCarga>();
                infUnidTransp = new List<infUnidTransp>();
            }

            public string nRoma { get; set; }

            public string nPed { get; set; }


            public string mod { get; set; }


            public string serie { get; set; }


            public string nDoc { get; set; }


            public string dEmi { get; set; }


            public decimal? vBC { get; set; }


            public decimal? vICMS { get; set; }


            public decimal? vBCST { get; set; }


            public decimal? vST { get; set; }


            public decimal? vProd { get; set; }


            public decimal? vNF { get; set; }


            public string nCFOP { get; set; }


            public decimal? nPeso { get; set; }


            public string PIN { get; set; }


            public string dPrev { get; set; }


            [XmlElement(ElementName = "infUnidCarga")]
            public List<infUnidCarga> infUnidCarga { get; set; }


            [XmlElement(ElementName = "infUnidTransp")]
            public List<infUnidTransp> infUnidTransp { get; set; }



        }
        #endregion


        #region infNFe

        [Serializable]
        public class infNFe

        {

            public infNFe()
            {
                infUnidCarga = new List<infUnidCarga>();
                infUnidTransp = new List<infUnidTransp>();
            }

            public string chave { get; set; }

            public string PIN { get; set; }

            public string dPrev { get; set; }


            [XmlElement(ElementName = "infUnidCarga")]
            public List<infUnidCarga> infUnidCarga { get; set; }


            [XmlElement(ElementName = "infUnidTransp")]
            public List<infUnidTransp> infUnidTransp { get; set; }

        }

        #endregion


        #region infOutros

        [Serializable]
        public class infOutros

        {
            public infOutros()
            {
                infUnidCarga = new List<infUnidCarga>();
                infUnidTransp = new List<infUnidTransp>();
            }

            public string tpDoc { get; set; }

            public string descOutros { get; set; }



            public string nDoc { get; set; }



            public string dEmi { get; set; }



            public decimal? vDocFisc { get; set; }



            public string dPrev { get; set; }


            [XmlElement(ElementName = "infUnidCarga")]
            public List<infUnidCarga> infUnidCarga { get; set; }


            [XmlElement(ElementName = "infUnidTransp")]
            public List<infUnidTransp> infUnidTransp { get; set; }

        }


        #endregion


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

        #region docAnt

        [Serializable]
        public class docAnt

        {
            public docAnt()
            {
                emiDocAnt = new List<emiDocAnt>();
            }

            [XmlElement(ElementName = "emiDocAnt")]
            public List<emiDocAnt> emiDocAnt { get; set; }

        }

        [Serializable]

        public class emiDocAnt

        {
            public emiDocAnt()
            {
                idDocAnt = new List<idDocAnt>();
            }


            public string CNPJ { get; set; }


            public string CPF { get; set; }


            public string IE { get; set; }


            public string UF { get; set; }

            public string xNome { get; set; }

            [XmlElement(ElementName = "idDocAnt")]
            public List<idDocAnt> idDocAnt { get; set; }

        }


        [Serializable]
        public class idDocAnt

        {
            public idDocAnt()
            {
                idDocAntPap = new List<idDocAntPap>();
                idDocAntEle = new List<idDocAntEle>();
            }

            [XmlElement(ElementName = "idDocAntPap")]
            public List<idDocAntPap> idDocAntPap { get; set; }

            [XmlElement(ElementName = "idDocAntEle")]
            public List<idDocAntEle> idDocAntEle { get; set; }


        }

        [Serializable]
        public class idDocAntPap

        {

            public string tpDoc { get; set; }

            public string serie { get; set; }

            public string subser { get; set; }

            public string nDoc { get; set; }

            public string dEmi { get; set; }

        }


        [Serializable]
        public class idDocAntEle

        {

            public string chave { get; set; }

        }



        #endregion

        #region "seg"

        [Serializable]
        public class seg
        {


            public string respSeg { get; set; }

            public string xSeg { get; set; }

            public string nApol { get; set; }

            public string nAver { get; set; }

            public decimal? vCarga { get; set; }

        }

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
                valePed = new List<valePed>();
                veic = new List<veic>();
                lacRodo = new List<lacRodo>();
                moto = new List<moto>();
            }

            public string RNTRC { get; set; }
            public string dPrev { get; set; }
            public string lota { get; set; }
            public string CIOT { get; set; }
            [XmlElement(ElementName = "occ")]
            public List<occ> occ { get; set; }
            [XmlElement(ElementName = "valePed")]
            public List<valePed> valePed { get; set; }
            [XmlElement(ElementName = "veic")]
            public List<veic> veic { get; set; }
            [XmlElement(ElementName = "lacRodo")]
            public List<lacRodo> lacRodo { get; set; }
            [XmlElement(ElementName = "moto")]
            public List<moto> moto { get; set; }

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

            public occ()
            {
                emiOcc = new emiOcc();
            }

            public string serie { get; set; }


            public string nOcc { get; set; }


            public string dEmi { get; set; }


            public emiOcc emiOcc { get; set; }

        }

        [Serializable]
        public class valePed
        {
            public string CNPJForn { get; set; }
            public string nCompra { get; set; }
            public string CNPJPg { get; set; }
            public decimal? vValePed { get; set; }
        }

        [Serializable]
        public class veic
        {
            public veic()
            {
                prop = new prop();
            }

            public string cInt { get; set; }
            public string RENAVAM { get; set; }
            public string placa { get; set; }
            public string tara { get; set; }
            public string capKG { get; set; }
            public string capM3 { get; set; }
            public string tpProp { get; set; }
            public string tpVeic { get; set; }
            public string tpRod { get; set; }
            public string tpCar { get; set; }
            public string UF { get; set; }
            public prop prop { get; set; }

        }

        [Serializable]
        public class prop
        {
            public string CPF { get; set; }
            public string CNPJ { get; set; }
            public string RNTRC { get; set; }
            public string xNome { get; set; }
            public string IE { get; set; }
            public string UF { get; set; }
            public string tpProp { get; set; }
        }

        [Serializable]
        public class lacRodo
        {
            public string nLacre { get; set; }
        }

        [Serializable]
        public class moto
        {
            public string xNome { get; set; }
            public string CPF { get; set; }
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
            }
            public string nMinu { get; set; }

            public string nOCA { get; set; }

            public string dPrevAereo { get; set; }
            public string xLAgEmi { get; set; }
            public string IdT { get; set; }

            public natCarga natCarga { get; set; }

            public tarifa tarifa { get; set; }

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

            public string cIMP { get; set; }
        }


        [Serializable]
        public class tarifa

        {

            public string CL { get; set; }

            public string cTar { get; set; }

            public decimal? vTar { get; set; }

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

            public string nBooking { get; set; }
            public string nCtrl { get; set; }

            public string xNavio { get; set; }

            [XmlElement(ElementName = "balsa")]
            public List<balsa> balsa { get; set; }

            public string nViag { get; set; }


            public string direc { get; set; }

            public string prtEmb { get; set; }

            public string prtTrans { get; set; }

            public string prtDest { get; set; }

            public string tpNav { get; set; }

            public string irin { get; set; }

            [XmlElement(ElementName = "detCont")]
            public List<detCont> detCont { get; set; }



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
                ferroEnv = new List<ferroEnv>();
                detVag = new List<detVag>();
            }
            public string tpTraf { get; set; }

            public trafMut trafMut { get; set; }

            public string fluxo { get; set; }

            public string idTrem { get; set; }

            public decimal? vFrete { get; set; }

            [XmlElement(ElementName = "ferroEnv")]
            public List<ferroEnv> ferroEnv { get; set; }

            [XmlElement(ElementName = "detVag")]
            public List<detVag> detVag { get; set; }

        }


        [Serializable]
        public class trafMut
        {
            public string respFat { get; set; }

            public string ferrEmi { get; set; }
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
        public class detVag
        {

            public string nVag { get; set; }
            public decimal? cap { get; set; }
            public string tpVag { get; set; }
            public decimal? pesoR { get; set; }
            public decimal? pesoBC { get; set; }
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

            public string COTM { get; set; }

            public string indNegociavel { get; set; }

        }

        #endregion

        #endregion

        #region "peri"

        [Serializable]
        public class peri

        {

            public string nONU { get; set; }

            public string xNomeAE { get; set; }

            public string xClaRisco { get; set; }

            public string grEmb { get; set; }

            public string qTotProd { get; set; }

            public string qVolTipo { get; set; }

            public string pontoFulgor { get; set; }

        }

        #endregion

        #region veicNovos

        [Serializable]
        public class veicNovos

        {
            public string chassi { get; set; }


            public string cCor { get; set; }


            public string xCor { get; set; }


            public string cMod { get; set; }


            public decimal? vUnit { get; set; }


            public decimal? vFrete { get; set; }

        }



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
        public class infCTeSub
        {
            public infCTeSub()
            {
                tomaICMS = new tomaICMS();
                tomaNaoICMS = new tomaNaoICMS();
            }

            public string chCte { get; set; }
            public tomaICMS tomaICMS { get; set; }
            public tomaNaoICMS tomaNaoICMS { get; set; }

        }

        [Serializable]
        public class tomaICMS
        {
            public tomaICMS()
            {
                refNF = new refNF();
            }
            public string refNFe { get; set; }

            public refNF refNF { get; set; }
            public string refCte { get; set; }
        }

        [Serializable]
        public class refNF

        {

            public string CNPJ { get; set; }

            public string CPF { get; set; }

            public string mod { get; set; }

            public string serie { get; set; }

            public string subserie { get; set; }

            public string nro { get; set; }

            public string valor { get; set; }

            public string dEmi { get; set; }

        }

        [Serializable]
        public class tomaNaoICMS
        {

            public string refCteAnu { get; set; }

        }


        #endregion

        #endregion

        #region infCTeComp

        [Serializable]
        public class infCteComp

        {

            public string chave { get; set; }

        }

        #endregion

        #region infCteAnu

        [Serializable]
        public class infCteAnu

        {

            public string chCTe { get; set; }
            public string dEmi { get; set; }

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