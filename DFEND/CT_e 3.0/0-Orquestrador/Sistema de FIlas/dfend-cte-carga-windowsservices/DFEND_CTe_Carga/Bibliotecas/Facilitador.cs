using System;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Security.Cryptography.X509Certificates;

namespace DFe
{
    class Facilitador
    {
        #region " ObterDataReferencia "

        public string ObterDataReferencia(string strChaveAcesso)
        {
            // Obtendo a data de referencia de uma chave de acesso
            return ("20" + strChaveAcesso.Substring(2, 2) + strChaveAcesso.Substring(4, 2));
        }

        #endregion

        #region " ObterCPFBase "

        public string ObterCPFBase(string strCPF)
        {
            // Destrinchando CPF
            strCPF = strCPF.PadLeft(11, '0');
            return strCPF.Substring(0, (strCPF.Length - 2));
        }

        #endregion

        #region " ObterCPFFilial "

        public string ObterCPFFilial(string strCPF)
        {
            // Destrinchando CPF
            return "0";
        }

        #endregion

        #region " ObterCPFDigito "

        public string ObterCPFDigito(string strCPF)
        {
            // Destrinchando CPF
            strCPF = strCPF.PadLeft(11, '0');
            return strCPF.Substring((strCPF.Length - 2), 2);
        }

        #endregion

        #region " ObterCNPJBase "

        public string ObterCNPJBase(string strCNPJ)
        {
            // Destrinchando CNPJ
            strCNPJ = strCNPJ.PadLeft(14, '0');
            return strCNPJ.Substring(0, (strCNPJ.Length - 6));
        }

        #endregion

        #region " ObterCNPJFilial "

        public string ObterCNPJFilial(string strCNPJ)
        {
            // Destrinchando CNPJ
            strCNPJ = strCNPJ.PadLeft(14, '0');
            return strCNPJ.Substring((strCNPJ.Length - 6), 4);
        }

        #endregion

        #region " ObterCNPJDigito "

        public string ObterCNPJDigito(string strCNPJ)
        {
            // Destrinchando CNPJ
            strCNPJ = strCNPJ.PadLeft(14, '0');
            return strCNPJ.Substring((strCNPJ.Length - 2), 2);
        }

        #endregion

        #region " ObterCNPJCPFBase "

        public string ObterCNPJCPFBase(int intTipo, string strCNPJ)
        {
            // Obtendo o CNPJ Base
            string strCNPJBase = string.Empty;

            if (strCNPJ != string.Empty)
            {
                // Retirando caracteres
                strCNPJ = strCNPJ.Replace(".", "").Replace("/", "").Replace("-", "");

                // Verificando se nao e passaporte
                if (intTipo != 2)
                {
                    // Verificando se e CNPJ ou CPF
                    if (strCNPJ.Length == 14)
                    {
                        strCNPJBase = strCNPJ.Substring(0, 8);
                    }
                    else if (strCNPJ.Length == 11)
                    {
                        strCNPJBase = strCNPJ.Substring(0, 9);
                    }
                }
            }

            return strCNPJBase;
        }

        #endregion

        #region " ObterCNPJCPFFilial "

        public string ObterCNPJCPFFilial(int intTipo, string strCNPJ)
        {
            // Obtendo o CNPJ Filial
            string strCNPJFilial = string.Empty;

            if (strCNPJ != string.Empty)
            {
                // Retirando caracteres
                strCNPJ = strCNPJ.Replace(".", "").Replace("/", "").Replace("-", "");

                // Verificando se nao e passaporte
                if (intTipo != 2)
                {
                    // Verificando se e CNPJ ou CPF
                    if (strCNPJ.Length == 14)
                    {
                        strCNPJFilial = strCNPJ.Substring(8, 4);
                    }
                    else if (strCNPJ.Length == 11)
                    {
                        strCNPJFilial = "0";
                    }
                }
            }

            return strCNPJFilial;
        }

        #endregion

        #region " ObterCNPJCPFDigito "

        public string ObterCNPJCPFDigito(int intTipo, string strCNPJ)
        {
            // Obtendo o CNPJ Digito
            string strCNPJDigito = string.Empty;

            if (strCNPJ != string.Empty)
            {
                // Retirando caracteres
                strCNPJ = strCNPJ.Replace(".", "").Replace("/", "").Replace("-", "");

                // Verificando se nao e passaporte
                if (intTipo != 2)
                {
                    // Verificando se e CNPJ ou CPF
                    if (strCNPJ.Length == 14)
                    {
                        strCNPJDigito = strCNPJ.Substring(12, 2);
                    }
                    else if (strCNPJ.Length == 11)
                    {
                        strCNPJDigito = strCNPJ.Substring(9, 2);
                    }
                }
            }

            return strCNPJDigito;
        }

        #endregion

        #region " ObterItem "

        public object ObterItem(string strElemento)
        {
            // Verificando se existe o item
            if (strElemento != string.Empty)
            {
                return strElemento;
            }
            else
            {
                return DBNull.Value;
            }
        }

        #endregion

        #region " ObterItemQuery "

        public string ObterItemQuery(string strElemento)
        {
            // Verificando se existe o item
            if (strElemento.Trim() != string.Empty)
            {
                return "'" + strElemento.Trim().Replace("'", "''") + "'";
            }
            else
            {
                return "null";
            }
        }

        #endregion

        #region " ObterItemData "

        public string ObterItemData(string strElemento)
        {
            // Verificando se existe o item
            if (strElemento != string.Empty)
            {
                // Verificando se contem o fuso
                if ((strElemento.Length == 25))
                {
                    strElemento = strElemento.Remove(19);
                }

                // Retirando o separador "T"
                strElemento = strElemento.Replace("T", " ");

                // Convertendo para o formato padrao
                strElemento = Convert.ToDateTime(strElemento).ToString("yyyy-MM-dd HH:mm:ss");
            }

            return strElemento;
        }

        #endregion

        #region " ObterItemDigestValue "

        public string ObterItemDigestValue(byte[] bytElemento)
        {
            // Inicializando variavel
            string strDigVal = string.Empty;

            // Verificando se existe o item
            if ((bytElemento.Length > 0))
            {
                foreach (byte objByte in bytElemento)
                {
                    strDigVal = (strDigVal + objByte.ToString());
                }
            }

            return strDigVal;
        }

        #endregion

        #region " ObterItemXML "

        public string ObterItemXML(XmlElement xmlElemento)
        {
            // Verificando se existe o item
            if (xmlElemento != null)
            {
                return xmlElemento.InnerText.Trim();
            }
            else
            {
                return string.Empty;
            }
        }

        #endregion

        #region " ObterElementoX "

        public string ObterElementoX(object objElemento)
        {
            // Verificando se existe o item
            if (objElemento != null)
            {
                return ((XElement)objElemento).Value;
            }
            else
            {
                return string.Empty;
            }
        }

        #endregion

        #region " ObterAtributoX "

        public string ObterAtributoX(object objElemento)
        {
            // Verificando se existe o item
            if (objElemento != null)
            {
                return ((XAttribute)objElemento).Value;
            }
            else
            {
                return string.Empty;
            }
        }

        #endregion

        #region " ObterAtributoXML "

        public string ObterAtributoXML(object objElemento)
        {
            // Verificando se existe o item
            if (objElemento != null)
            {
                return ((XmlAttribute)objElemento).Value;
            }
            else
            {
                return string.Empty;
            }
        }

        #endregion

        #region " ObterParteChave "

        public string ObterParteChave(string strChave, short intParte)
        {
            // Desmontando chave
            char[] chrSeparador = new char[] { ';' };
            return strChave.Split(chrSeparador)[intParte].ToString();
        }

        #endregion

        #region " ObterCertificado "

        public X509Certificate ObterCertificado(string strNomeOuCaminhoCertificado)
        {
            // Compatibilidade: se ainda for caminho de arquivo existente, lê do disco
            if (File.Exists(strNomeOuCaminhoCertificado))
            {
                if (!strNomeOuCaminhoCertificado.EndsWith(".pfx", StringComparison.OrdinalIgnoreCase))
                {
                    return X509Certificate.CreateFromCertFile(strNomeOuCaminhoCertificado);
                }

                return new X509Certificate2(strNomeOuCaminhoCertificado);
            }

            // Lê do repositório Pessoal do Windows (CurrentUser, depois LocalMachine)
            string strDisponiveis;
            X509Certificate2 objCertificado = this.BuscarCertificadoNoStore(StoreLocation.CurrentUser, strNomeOuCaminhoCertificado, out strDisponiveis);
            if (objCertificado == null)
            {
                string strDisponiveisLM;
                objCertificado = this.BuscarCertificadoNoStore(StoreLocation.LocalMachine, strNomeOuCaminhoCertificado, out strDisponiveisLM);
                if (!string.IsNullOrEmpty(strDisponiveisLM))
                {
                    strDisponiveis = strDisponiveis + " | LocalMachine: " + strDisponiveisLM;
                }
            }

            if (objCertificado == null)
            {
                throw new Exception(
                    "Certificado digital nao encontrado. Config='" + strNomeOuCaminhoCertificado + "'. " +
                    "Certificados no store: " + (string.IsNullOrEmpty(strDisponiveis) ? "(nenhum)" : strDisponiveis));
            }

            return objCertificado;
        }

        private X509Certificate2 BuscarCertificadoNoStore(StoreLocation local, string strNome, out string strListaSubjects)
        {
            strListaSubjects = string.Empty;
            X509Store store = new X509Store(StoreName.My, local);
            try
            {
                store.Open(OpenFlags.ReadOnly);

                System.Text.StringBuilder stbLista = new System.Text.StringBuilder();
                X509Certificate2 objPrimeiroMatch = null;

                foreach (X509Certificate2 cert in store.Certificates)
                {
                    if (cert.Subject != null && cert.Subject.Length > 0)
                    {
                        if (stbLista.Length > 0)
                        {
                            stbLista.Append(" ; ");
                        }
                        stbLista.Append(cert.Subject);
                    }

                    if (cert.Subject != null &&
                        cert.Subject.IndexOf(strNome, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        try
                        {
                            if (cert.HasPrivateKey)
                            {
                                strListaSubjects = stbLista.ToString();
                                return cert;
                            }
                        }
                        catch
                        {
                            strListaSubjects = stbLista.ToString();
                            return cert;
                        }

                        if (objPrimeiroMatch == null)
                        {
                            objPrimeiroMatch = cert;
                        }
                    }
                }

                strListaSubjects = stbLista.ToString();
                return objPrimeiroMatch;
            }
            catch
            {
                return null;
            }
            finally
            {
                store.Close();
            }
        }

        #endregion

        #region " ObterEsquemaCTe "

        public string ObterEsquemaCTe(XmlElement xmlDocumento)
        {
            // Classes e variaveis utilizadas
            XmlElement xmlInt = xmlDocumento;

            // Obtendo o schema de um CT-e
            string strSchema = this.ObterAtributoXML(xmlDocumento.Attributes["schema"]);
            if (strSchema == string.Empty)
            {
                xmlInt = (XmlElement)xmlDocumento.GetElementsByTagName(Constante.EsqGTVeAutorizacaoProc)[0];
                if (xmlInt != null)
                {
                    strSchema = Constante.EsqGTVeAutorizacaoSchema;
                }
                else
                {
                    xmlInt = (XmlElement)xmlDocumento.GetElementsByTagName(Constante.EsqCTeOSAutorizacaoProc)[0];
                    if (xmlInt != null)
                    {
                        strSchema = Constante.EsqCTeOSAutorizacaoSchema;
                    }
                    else
                    {
                        xmlInt = (XmlElement)xmlDocumento.GetElementsByTagName(Constante.EsqCTeSimpAutorizacaoProc)[0];
                        if (xmlInt != null)
                        {
                            strSchema = Constante.EsqCTeSimpAutorizacaoSchema;
                        }
                        else
                        {
                            xmlInt = (XmlElement)xmlDocumento.GetElementsByTagName(Constante.EsqCTeSimpAutorizacaoProc2)[0];
                            if (xmlInt != null)
                            {
                                strSchema = Constante.EsqCTeSimpAutorizacaoSchema;
                            }
                            else
                            {
                                xmlInt = (XmlElement)xmlDocumento.GetElementsByTagName(Constante.EsqCTeAutorizacaoProc)[0];
                                if (xmlInt != null)
                                {
                                    strSchema = Constante.EsqCTeAutorizacaoSchema;
                                }
                            }
                        }
                    }
                }
                strSchema = this.MontarEsquema(strSchema, this.ObterAtributoXML(xmlInt.Attributes["versao"]));
            }

            // Retornando o nome do esquema
            return strSchema;
        }

        #endregion

        #region " ObterHintConsulta "

        public string ObterHintConsulta(int intTipoConsultaNL)
        {
            // Classes e variaveis utilizadas
            string strRetorno = "NOLOCK";

            // Verificando se vai usar NOLOCK ou READPAST
            if (intTipoConsultaNL == 0)
            {
                strRetorno = "READPAST";
            }

            return strRetorno;
        }

        #endregion

        #region " ObterNSUArquivo "

        public string ObterNSUArquivo()
        {
            // Classes e variaveis utilizadas
            string strNSU = string.Empty;

            // Formando o nome do arquivo
            string strArquivo = Environment.CurrentDirectory + "\\NSU.txt";

            // Verificando se o arquivo existe
            if (File.Exists(strArquivo))
            {
                // Lendo o NSU do arquivo existente
                FileStream objFileStream = new FileStream(strArquivo, FileMode.Open, FileAccess.Read);
                StreamReader objStreamReader = new StreamReader(objFileStream, UTF8Encoding.Default);
                strNSU = objStreamReader.ReadLine();
                objStreamReader.Close();
                objFileStream.Close();
            }
            else
            {
                // Criando o arquivo com NSU 0
                StreamWriter objStreamWriter = new StreamWriter(File.Create(strArquivo));
                strNSU = "000000000000000";
                objStreamWriter.WriteLine(strNSU);
                objStreamWriter.Close();
            }

            return strNSU;
        }

        #endregion

        #region " AtualizarNSUArquivo "

        public void AtualizarNSUArquivo(string strNSU)
        {
            // Formando o nome do arquivo
            string strArquivo = Environment.CurrentDirectory + "\\NSU.txt";
            strNSU = ("000000000000000" + strNSU).Substring(strNSU.Length, 15);

            // Verificando se o arquivo existe
            if (File.Exists(strArquivo))
            {
                // Gravando o NSU no arquivo existente
                StreamWriter objStreamWriter = new StreamWriter(strArquivo);
                objStreamWriter.WriteLine(strNSU);
                objStreamWriter.Close();
            }
            else
            {
                // Criando o arquivo com NSU
                StreamWriter objStreamWriter = new StreamWriter(File.Create(strArquivo));
                objStreamWriter.WriteLine(strNSU);
                objStreamWriter.Close();
            }
        }

        #endregion

        #region " AdicionarParametro "

        public void AdicionarParametro(ref string strQuery, string strValorParametro, string strNomeParametro, SqlDbType objTipoParametro)
        {
            // Verificando se existe a query
            if (strQuery != string.Empty)
            {
                // Verificando se o valor e nulo
                if ((strValorParametro == string.Empty) || (strValorParametro == null))
                {
                    strQuery = strQuery.Replace((strNomeParametro + ", "), "null, ").Replace((strNomeParametro + ")"), "null)").Replace((strNomeParametro + " "), "null ");
                }
                else
                {
                    // Verificando qual o tipo do parametro
                    if (objTipoParametro == SqlDbType.Structured)
                    {
                        strQuery = strQuery.Replace(strNomeParametro, strValorParametro);
                    }
                    else
                    {
                        // Verificando qual o tipo do parametro
                        if ((objTipoParametro == SqlDbType.Char) || (objTipoParametro == SqlDbType.VarChar))
                        {
                            strValorParametro = ("\'" + strValorParametro.Replace("\'", "\'\'") + "\'");
                        }
                        else if ((objTipoParametro == SqlDbType.DateTime) || (objTipoParametro == SqlDbType.SmallDateTime) || (objTipoParametro == SqlDbType.Date))
                        {
                            strValorParametro = ("\'" + this.ObterItemData(strValorParametro) + "\'");
                        }
                        else if (objTipoParametro == SqlDbType.Decimal)
                        {
                            strValorParametro = ("\'" + strValorParametro.Replace(",", ".") + "\'");
                        }
                        else if (objTipoParametro == SqlDbType.VarBinary)
                        {
                            strValorParametro = ("convert(varbinary(max)," + "\'" + strValorParametro.Replace("\'", "\'\'") + "\')");
                        }

                        // Adicionando valor a query
                        strQuery = strQuery.Replace((strNomeParametro + ", "), (strValorParametro + ", ")).Replace((strNomeParametro + ")"), (strValorParametro + ")")).Replace((strNomeParametro + " "), (strValorParametro + " "));
                    }
                }
            }
        }

        #endregion

        #region " MontarCNPJCPF "

        public string MontarCNPJCPF(string strCNPJBase, string strCNPJFilial, string strCNPJDigito)
        {
            // Classes e variaveis utilizadas
            string strRetorno = string.Empty;

            // Verificando se existe o item
            if ((strCNPJBase != string.Empty) || (strCNPJDigito != string.Empty))
            {
                // Verificando se existe a filial
                if ((strCNPJFilial != string.Empty) && (strCNPJFilial != "0"))
                {
                    // Montando CNPJ
                    strRetorno = strCNPJBase.PadLeft(8, '0') + strCNPJFilial.PadLeft(4, '0') + strCNPJDigito.PadRight(2, '0');
                }
                else
                {
                    // Montando CPF
                    strRetorno = strCNPJBase.PadLeft(9, '0') + strCNPJDigito.PadRight(2, '0');
                }
            }

            return strRetorno;
        }

        #endregion

        #region " MontarTagXML "

        public string MontarTagXML(string strTag, object objElemento, bool bolTexto, Constante.TipoFormatData tipData)
        {
            // Classes e variaveis utilizadas
            string strRetorno = string.Empty;

            // Verificando se existe o item
            if ((objElemento != null) && (objElemento != DBNull.Value))
            {
                // Verificando se o item esta vazio
                if (objElemento.ToString() != string.Empty)
                {
                    // Verificando se deve formatar como texto
                    if (!bolTexto)
                    {
                        strRetorno = "<" + strTag + ">" + this.FormatarData(objElemento, tipData) + "</" + strTag + ">";
                    }
                    else
                    {
                        strRetorno = "<" + strTag + ">" + "<![CDATA[" + objElemento.ToString() + "]]>" + "</" + strTag + ">";
                    }
                }
            }

            return strRetorno;
        }

        #endregion

        #region " MontarEsquema "

        public string MontarEsquema(string strEsquema, string strVersao)
        {
            // Montando o nome do esquema
            return (strEsquema + "_v" + strVersao.Trim() + ".xsd");
        }

        #endregion

        #region " MontarQuery "

        public string MontarQuery(string strQuery)
        {
            return strQuery + Environment.NewLine;
        }

        #endregion

        #region " MontarLogInicializacao "

        public string MontarLogInicializacao()
        {
            // Montando o log de inicializacao
            return (Constante.MsgServicoIniciado + this.FormatarVersao(Assembly.GetEntryAssembly().GetName()) + " ; " + Environment.MachineName.ToUpper());
        }

        #endregion

        #region " FormatarCPFBase "

        public string FormatarCPFBase(string strCPF)
        {
            // Retornando CPF formatado
            return strCPF.PadLeft(9, '0');
        }

        #endregion

        #region " FormatarCNPJBase "

        public string FormatarCNPJBase(string strCNPJ)
        {
            // Retornando CNPJ formatado
            return strCNPJ.PadLeft(8, '0');
        }

        #endregion

        #region " FormatarCNPJFilial "

        public string FormatarCNPJFilial(string strCNPJ)
        {
            // Retornando CNPJ formatado
            return strCNPJ.PadLeft(4, '0');
        }

        #endregion

        #region " FormatarVersao "

        public string FormatarVersao(AssemblyName objVersao)
        {
            // Montando nome da versao
            string strRetorno = objVersao.Name + "_" + objVersao.Version.Major.ToString().PadLeft(2, '0') + "." + objVersao.Version.Minor.ToString().PadLeft(2, '0') + "." + objVersao.Version.Build.ToString().PadLeft(2, '0');

            // Retornando versao formatada
            return strRetorno;
        }

        #endregion

        #region " FormatarVersao "

        public string FormatarVersao(AssemblyName objVersao, string strNomeAplicacao)
        {
            // Montando nome da versao
            string strRetorno = strNomeAplicacao + "_" + objVersao.Version.Major.ToString().PadLeft(2, '0') + "." + objVersao.Version.Minor.ToString().PadLeft(2, '0') + "." + objVersao.Version.Build.ToString().PadLeft(2, '0');

            // Retornando versao formatada
            return strRetorno;
        }

        #endregion

        #region " FormatarData "

        public string FormatarData(object objElemento, Constante.TipoFormatData tipData)
        {
            // Classes e variaveis utilizadas
            string strRetorno = string.Empty;

            // Verificando se existe o item
            if ((objElemento != null) && (objElemento != DBNull.Value))
            {
                // Verificando se o item esta vazio
                if (objElemento.ToString() != string.Empty)
                {
                    // Verificando se deve formatar como data
                    if (tipData == Constante.TipoFormatData.Nenhuma)
                    {
                        strRetorno = objElemento.ToString();
                    }
                    else if (tipData == Constante.TipoFormatData.Data)
                    {
                        strRetorno = Convert.ToDateTime(objElemento.ToString()).ToString("yyyy-MM-dd");
                    }
                    else if (tipData == Constante.TipoFormatData.DataHora)
                    {
                        strRetorno = Convert.ToDateTime(objElemento.ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    else if (tipData == Constante.TipoFormatData.DataHoraFuso)
                    {
                        strRetorno = Convert.ToDateTime(objElemento.ToString()).ToString("yyyy-MM-dd HH:mm:ss") + Constante.FusoHorario;
                    }
                    else if (tipData == Constante.TipoFormatData.DataHoraT)
                    {
                        strRetorno = Convert.ToDateTime(objElemento.ToString()).ToString("yyyy-MM-ddTHH:mm:ss");
                    }
                    else if (tipData == Constante.TipoFormatData.DataHoraFusoT)
                    {
                        strRetorno = Convert.ToDateTime(objElemento.ToString()).ToString("yyyy-MM-ddTHH:mm:ss") + Constante.FusoHorario;
                    }
                }
            }

            return strRetorno;
        }

        #endregion

        #region " CompactarTexto "

        public string CompactarTexto(string strEntrada)
        {
            // Classes e variaveis utilizadas
            MemoryStream objCompactado = new MemoryStream();
            GZipStream objGZip = new GZipStream(objCompactado, CompressionMode.Compress, true);

            // Montando informacao compactada
            byte[] bytEntrada = Encoding.UTF8.GetBytes(strEntrada);
            objGZip.Write(bytEntrada, 0, bytEntrada.Length);

            // Retornando informacao compactada
            objGZip.Close();
            return Convert.ToBase64String(objCompactado.ToArray());
        }

        #endregion

        #region " DescompactarTexto "

        public string DescompactarTexto(string strEntrada)
        {
            // Classes e variaveis utilizadas
            MemoryStream objCompactado = new MemoryStream();
            MemoryStream objDescompactado = new MemoryStream();

            // Inicializando variaveis
            byte[] bytEntrada = Convert.FromBase64String(strEntrada);
            byte[] bytBuffer = new byte[] { 63 };
            int intLido = -1;

            // Montando informacao compactada 
            objCompactado.Write(bytEntrada, 0, bytEntrada.Length);
            objCompactado.Position = 0;
            GZipStream objGZip = new GZipStream(objCompactado, CompressionMode.Decompress, true);
            intLido = objGZip.Read(bytBuffer, 0, bytBuffer.Length);

            // Montando informacao descompactada 
            while ((intLido > 0))
            {
                objDescompactado.Write(bytBuffer, 0, intLido);
                intLido = objGZip.Read(bytBuffer, 0, bytBuffer.Length);
            }

            // Retornando informacao descompactada
            objGZip.Close();
            string strRetorno = Encoding.UTF8.GetString(objDescompactado.ToArray());
            return strRetorno;
        }

        #endregion

        #region " DescompactarProc "

        public XmlDocument DescompactarProc(XmlNode xmlProc)
        {
            // Montando XML
            string strXML = xmlProc.OuterXml;
            XmlDocument xmlDescompactado = new XmlDocument();
            xmlDescompactado.LoadXml(xmlProc.OuterXml);
            XmlElement xmlComp = (XmlElement)xmlDescompactado.GetElementsByTagName("procComp")[0];

            // Verificando se o retorno esta compactado
            if (xmlComp != null)
            {
                // Substituindo resposta compactada
                string strDescompactado = this.DescompactarTexto(xmlProc["procComp"].InnerText);
                string strInicial = strXML.Substring(0, strXML.IndexOf("<procComp>"));
                string strFinal = strXML.Substring(strXML.IndexOf("</procComp>") + 11);
                strDescompactado = strDescompactado.Replace(Constante.CabecalhoXML, string.Empty);
                strDescompactado = strDescompactado.Replace(Constante.CabecalhoXML2, string.Empty);
                strDescompactado = strDescompactado.Replace(Constante.CabecalhoXML3, string.Empty);
                strDescompactado = strDescompactado.Replace(Constante.CabecalhoXML4, string.Empty);
                strDescompactado = strDescompactado.Replace(Constante.CabecalhoXML5, string.Empty);
                strDescompactado = strDescompactado.Replace(Constante.CabecalhoXML6, string.Empty);
                strDescompactado = strDescompactado.Replace(Constante.CabecalhoXML7, string.Empty);
                strDescompactado = strDescompactado.Replace(Constante.CabecalhoXML8, string.Empty);
                strDescompactado = strDescompactado.Replace(Constante.CabecalhoXML9, string.Empty);
                strDescompactado = strDescompactado.Replace(Constante.CabecalhoXML10, string.Empty);
                xmlDescompactado.LoadXml(strDescompactado);
                xmlDescompactado.LoadXml(strInicial + xmlDescompactado.LastChild.OuterXml + strFinal);
            }
            else
            {
                // Levantando excecao
                throw new Exception("Lote não compactado");
            }

            return xmlDescompactado;
        }

        #endregion

        #region " VerificarExistenciaItemLista "

        public bool VerificarExistenciaItemLista(string strListaPrincipal, string strListaItens, char strSeparador)
        {
            bool bolRetorno = false;

            // Verificando se existe algum Item da lista de Itens na lista Principal
            foreach (string strItem in strListaItens.Split(new char[] { strSeparador }))
            {
                if ((strItem != string.Empty) && (strListaPrincipal.Contains(strItem)))
                {
                    bolRetorno = true;
                    break;
                }
            }

            return bolRetorno;
        }

        #endregion

        #region " ValidarItemConfigArquivo "

        public object ValidarItemConfigArquivo(object objElemento, string strItem)
        {
            // Verificando se existe o item
            if ((objElemento == null) || (objElemento == DBNull.Value))
            {
                throw new FormatException("Configuração não cadastrada no arquivo config: ' " + strItem + "'");
            }

            return objElemento;
        }

        #endregion

        #region " ValidarItemConfigBanco "

        public object ValidarItemConfigBanco(object objElemento, string strItem)
        {
            // Verificando se existe o item
            if ((objElemento == null) || (objElemento == DBNull.Value) || (objElemento.ToString() == string.Empty))
            {
                throw new FormatException("Configuração não cadastrada no banco de dados: ' " + strItem + "'");
            }

            return objElemento;
        }

        #endregion

        #region " ValidarNumero "

        public bool ValidarNumero(string strItem)
        {
            if (!string.IsNullOrWhiteSpace(strItem))
            {
                bool bolRetorno = true;

                foreach (char carater in strItem)
                {
                    bolRetorno = bolRetorno && char.IsDigit(carater);
                }

                return bolRetorno;
            }

            return false;
        }

        #endregion

        #region " ValidarCPF "

        public bool ValidarCPF(string strCPF)
        {
            try
            {
                // Retirando espaços
                strCPF = strCPF.Trim();

                // Verificando se não é vazio
                if (string.IsNullOrWhiteSpace(strCPF))
                {
                    return false;
                }

                // Verificando quantidade de dígitos
                if (strCPF.Length != 11)
                {
                    return false;
                }

                // Variáveis para array de dígitos
                int intIndice1;
                int intIndice2;
                string[] strArrayNumeros = new string[] { "11111111111", "22222222222", "33333333333", "44444444444", "55555555555", "66666666666", "77777777777", "88888888888", "99999999999" };

                // Verificando se o valor está no array
                var loopTo = strArrayNumeros.Length - 1;
                for (intIndice1 = 0; intIndice1 <= loopTo; intIndice1++)
                {
                    if (strArrayNumeros[intIndice1].Equals(strCPF))
                    {
                        return false;
                    }
                }

                // Variáveis para verificação dos dígitos
                int intNumero1;
                int intNumero2;

                // Calculando os dígito do CPF
                for (intIndice1 = 0; intIndice1 <= 1; intIndice1++)
                {
                    intNumero1 = 0;

                    var loopTo1 = 8 + intIndice1;
                    for (intIndice2 = 0; intIndice2 <= loopTo1; intIndice2++)
                        intNumero1 = (int)Math.Round(intNumero1 + Convert.ToDouble(strCPF.Substring(intIndice2, 1)) * (10 + intIndice1 - intIndice2));

                    intNumero2 = (int)Math.Round(11 - (intNumero1 - Convert.ToDouble(intNumero1 / 11) * 11));

                    if (intNumero2 == 10 || intNumero2 == 11)
                        intNumero2 = 0;

                    if (intNumero2 != Convert.ToDouble(strCPF.Substring(9 + intIndice1, 1)))
                    {
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                // Levantando exceção
                throw new Exception("ValidarCPF: " + ex.Message);
            }
        }

        #endregion

        #region " ValidarCNPJ "

        public bool ValidarCNPJ(string strCNPJ)
        {
            try
            {
                // Retirando espaços do CNPJ
                strCNPJ = strCNPJ.Trim();

                // Verificando quantidade de dígitos
                if (strCNPJ.Length != 14)
                {
                    return false;
                }

                // Verificando se é numérico
                if (!this.ValidarNumero(strCNPJ))
                {
                    return false;
                }

                // Variáveis para array de dígitos do CNPJ
                int[] intNumeros = new int[14];
                int intIndice;

                // Montando um array com os dígitos do CNPJ
                int loopTo = intNumeros.Length - 1;
                for (intIndice = 0; intIndice <= loopTo; intIndice++)
                    intNumeros[intIndice] = Convert.ToInt32(strCNPJ.Substring(intIndice, 1));

                // Calculando primeiro dígito verificador
                int intSoma;
                intSoma = intNumeros[0] * 5 + intNumeros[1] * 4 + intNumeros[2] * 3 + intNumeros[3] * 2 + intNumeros[4] * 9 + intNumeros[5] * 8 + intNumeros[6] * 7 + intNumeros[7] * 6 + intNumeros[8] * 5 + intNumeros[9] * 4 + intNumeros[10] * 3 + intNumeros[11] * 2;
                intSoma = (int)Math.Round(intSoma - 11 * Convert.ToDouble(intSoma / 11));

                // Obtendo o primeiro dígito verificador
                int intPrimeiroDigito;
                if (intSoma < 2)
                {
                    intPrimeiroDigito = 0;
                }
                else
                {
                    intPrimeiroDigito = 11 - intSoma;
                }

                // Verificando se o primeiro digito está correto
                if (intPrimeiroDigito == intNumeros[12])
                {
                    // Calculando segundo dígito verificador
                    int intSegundoDigito;
                    intSoma = intNumeros[0] * 6 + intNumeros[1] * 5 + intNumeros[2] * 4 + intNumeros[3] * 3 + intNumeros[4] * 2 + intNumeros[5] * 9 + intNumeros[6] * 8 + intNumeros[7] * 7 + intNumeros[8] * 6 + intNumeros[9] * 5 + intNumeros[10] * 4 + intNumeros[11] * 3 + intNumeros[12] * 2;
                    intSoma = (int)Math.Round(intSoma - 11 * Convert.ToDouble(intSoma / 11));

                    // Obtendo o segundo dígito verificador
                    if (intSoma < 2)
                    {
                        intSegundoDigito = 0;
                    }
                    else
                    {
                        intSegundoDigito = 11 - intSoma;
                    }

                    // Verificando se o segundo digito está correto
                    if (intSegundoDigito == intNumeros[13])
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                // Levantando exceção
                throw new ArgumentException("ValidarCNPJ: " + ex.Message);
            }
        }

        #endregion

        #region " ValidarCNPJAlfa "

        public bool ValidarCNPJAlfa(string strCNPJ)
        {
            try
            {
                // Classes e variaveis utilizadas
                Regex regCNPJ = new Regex("^([A-Z\\d]){12}(\\d){2}$", RegexOptions.Compiled);
                Regex regCaracteresMascara = new Regex("[./-]", RegexOptions.Compiled);
                Regex regCaracteresNaoPermitidos = new Regex("[^A-Z\\d./-]", RegexOptions.IgnoreCase | RegexOptions.Compiled);

                // Verificando se o CNPJ e valido
                if (!regCaracteresNaoPermitidos.IsMatch(strCNPJ))
                {
                    // Inicializando variaveis
                    int intTamanhoCNPJSemDV = 12;
                    string strCNPJZerado = "00000000000000";
                    string strCNPJSemMascara = regCaracteresMascara.Replace(strCNPJ, "");

                    if ((regCNPJ.IsMatch(strCNPJSemMascara)) && (strCNPJSemMascara != strCNPJZerado))
                    {
                        string strDVInformado = strCNPJSemMascara.Substring(intTamanhoCNPJSemDV);
                        string strDVCalculado = this.CalcularDigitoVerificadorCNPJAlfa(strCNPJSemMascara.Substring(0, intTamanhoCNPJSemDV));

                        return strDVInformado == strDVCalculado;
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                // Levantando exceção
                throw new ArgumentException("ValidarCNPJAlfa: " + ex.Message);
            }
        }

        #endregion

        #region " CalcularDigitoVerificadorCNPJAlfa "

        public string CalcularDigitoVerificadorCNPJAlfa(string strCNPJ)
        {
            // Classes e variaveis utilizadas
            Regex regCNPJSemDV = new Regex("^([A-Z\\d]){12}$", RegexOptions.Compiled);
            Regex regCaracteresMascara = new Regex("[./-]", RegexOptions.Compiled);
            Regex regCaracteresNaoPermitidos = new Regex("[^A-Z\\d./-]", RegexOptions.IgnoreCase | RegexOptions.Compiled);

            // Verificando se o CNPJ e valido
            if (!regCaracteresNaoPermitidos.IsMatch(strCNPJ))
            {
                // Inicializando variaveis
                int intTamanhoCNPJSemDV = 12;
                int intSomaDV1 = 0;
                int intSomaDV2 = 0;
                int[] intPesosDV = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
                string strCNPJZerado = "00000000000000";
                string strCNPJSemMascara = regCaracteresMascara.Replace(strCNPJ, "");

                if ((regCNPJSemDV.IsMatch(strCNPJSemMascara)) && (strCNPJSemMascara != strCNPJZerado.Substring(0, intTamanhoCNPJSemDV)))
                {

                    for (int i = 0; i < intTamanhoCNPJSemDV; i++)
                    {
                        int intAsciiDigito = strCNPJSemMascara[i] - 0;
                        intSomaDV1 += intAsciiDigito * intPesosDV[i + 1];
                        intSomaDV2 += intAsciiDigito * intPesosDV[i];
                    }

                    int intDV1 = (intSomaDV1 % 11) < 2 ? 0 : 11 - (intSomaDV1 % 11);
                    intSomaDV2 += intDV1 * intPesosDV[intTamanhoCNPJSemDV];
                    int intDV2 = (intSomaDV2 % 11) < 2 ? 0 : 11 - (intSomaDV2 % 11);

                    return intDV1.ToString() + intDV2.ToString();
                }
            }

            throw new Exception("Não é possível calcular o dígito verificador (DV), pois o CNPJ fornecido é inválido");
        }

        #endregion
    }
}
