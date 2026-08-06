using System;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Security.Cryptography.X509Certificates;

namespace DFe
{
    class Facilitador
    {
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
                return xmlElemento.InnerText;
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

        #region " ObterParteChave "

        public string ObterParteChave(string strChave, short intParte)
        {
            // Desmontando chave
            char[] chrSeparador = new char[] { ';' };
            return strChave.Split(chrSeparador)[intParte].ToString();
        }

        #endregion

        #region " ObterCertificado "

        public X509Certificate ObterCertificado(string strCaminhoCertificado)
        {
            // Classes e variaveis utilizadas
            X509Certificate objCertificado;

            // Verificando qual a extensão do certificado digital
            if (!strCaminhoCertificado.EndsWith(".pfx"))
            {
                objCertificado = X509Certificate.CreateFromCertFile(strCaminhoCertificado);
            }
            else
            {
                X509Certificate objCertificadoPrivado = X509Certificate.CreateFromCertFile(strCaminhoCertificado);
                objCertificado = new X509Certificate();
                objCertificado.Import(objCertificadoPrivado.GetPublicKey());
            }

            return objCertificado;
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

        #region " ObterDataReferencia "

        public string ObterDataReferencia(string strChaveAcesso)
        {
            // Obtendo a data de referencia de uma chave de acesso
            return ("20" + strChaveAcesso.Substring(2, 2) + strChaveAcesso.Substring(4, 2));
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
                            strValorParametro = strValorParametro.Replace("\n", "");
                            strValorParametro = ("\'" + strValorParametro.Replace("\'", "\'\'") + "\'");
                        }
                        else if ((objTipoParametro == SqlDbType.DateTime) || (objTipoParametro == SqlDbType.SmallDateTime))
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

        public string MontarTagXML(object objElemento, string strTag, bool bolTexto, Constante.TipoFormatData tipData)
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

        #region " MontarQuery "

        public string MontarQuery(string strQuery)
        {
            return strQuery + Environment.NewLine;
        }

        #endregion

        #region " MontarEsquema "

        public string MontarEsquema(string strEsquema, string strVersao)
        {
            // Montando o nome do esquema
            return (strEsquema + "_v" + strVersao.Trim() + ".xsd");
        }

        #endregion

        #region " ValidarItemConfigArquivo "

        public object ValidarItemConfigArquivo(object objElemento, string strItem)
        {
            // Verificando se existe o item
            if ((objElemento == null) || (objElemento == DBNull.Value) || (objElemento.ToString() == string.Empty))
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
    }
}
