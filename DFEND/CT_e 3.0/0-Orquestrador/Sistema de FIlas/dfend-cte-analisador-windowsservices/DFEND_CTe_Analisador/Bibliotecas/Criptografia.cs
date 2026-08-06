using System;
using System.Collections;
using System.Security.Cryptography;
using System.Text;

namespace DFe
{
    class Criptografia
    {
        #region " Variaveis "

        public const int intTamanho = 1024;
        public const string strChavePublica = "<RSAKeyValue><Modulus>sk6FDisGDL+lymNzN+QYQtPFCvz3oDdqVQh1qo97I/voaLBKtL7/BTuc9Ey3xWKJSiEWsBxywnHInEDnsX9og0wnul5cSoN9Ex7mByfNODuiquRADNBztjfMzczMzoZALPm6bAjHPoHMBoxh/HMwllC6lfXjE37CjvEiQ5RMAmk=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
        public const string strChavePrivada = "<RSAKeyValue><Modulus>sk6FDisGDL+lymNzN+QYQtPFCvz3oDdqVQh1qo97I/voaLBKtL7/BTuc9Ey3xWKJSiEWsBxywnHInEDnsX9og0wnul5cSoN9Ex7mByfNODuiquRADNBztjfMzczMzoZALPm6bAjHPoHMBoxh/HMwllC6lfXjE37CjvEiQ5RMAmk=</Modulus><Exponent>AQAB</Exponent><P>7diDMIfJ6r8VA5tcOSrt6dmy7vTlq/eNCPWB4hfNdPZ1YhF/nbjoaXZeh/ETJjwFiqpwx4f8RMOu3O9uTqEFzQ==</P><Q>v+qegq7VtAtdcLSKCEqJou4+UfuZ8hFHum0rdAM1VuKjLXMTqR6ofRU1Hnb45zzC2AvxU+AjUYfZUf+WNPKTDQ==</Q><DP>Hq1vdmH9TDbrOfTe90yvNRtsNeAGg6DBYydvYj8Zs/1Z9JU3NZu09m1bEgOpKaRSPqlvNH04r5HhywNi4edo/Q==</DP><DQ>tuloVelIaKcEUazgefKAp7cl0oFYQixSDs6JwbJlHgA3wdOSA3fi4gRackS2Cb4RflQzl9tiDgzKBarxgFlJ6Q==</DQ><InverseQ>vDU/wf7dESLiTngdPMWV3mp7aDGaYS5zb5AFqt2FVraDj5RZF3jSzh2Ljec+7jZCYyib9jEiPYaC+Kx8gYhL5w==</InverseQ><D>bbT4Es6P61ojZNIpuxLBhqSLkQYyScauyuDonOepCWveBEOrw2fcutiB6qIlU/IbrJuNPRBT36VonBMnO0O8BjxxgqmTBIGwEkDDHDz9cxViiGOd9680qcEZ5x2AqWvoLlxnAxXYJUXnsoojbTD4lWDD/y+HBPlb+rGUHFJqVLE=</D></RSAKeyValue>";

        #endregion

        #region " Encriptar "

        public string Encriptar(string strTexto)
        {
            // Definindo o tamanho da chave
            RSACryptoServiceProvider rsaCriptografia = new RSACryptoServiceProvider(intTamanho);

            // Inserindo a chave publica
            rsaCriptografia.FromXmlString(strChavePublica);

            // Declarando objetos utilizados
            StringBuilder stbRetorno = new StringBuilder();
            byte[] bytEntrada = Encoding.UTF32.GetBytes(strTexto);
            byte[] bytEncriptados = null;
            byte[] bytTemp = null;

            // Calculando tamanhos
            int intTamanhoChave = intTamanho / 8;
            int intTamanhoMax = intTamanhoChave - 42;
            int intTamanhoDado = bytEntrada.Length;
            int intInteracoes = intTamanhoDado / intTamanhoMax;

            // Efetuando criptografia
            for (int i = 0; i <= intInteracoes; i++)
            {
                bytTemp = new byte[(intTamanhoDado - intTamanhoMax * i > intTamanhoMax) ? intTamanhoMax : intTamanhoDado - intTamanhoMax * i];
                Buffer.BlockCopy(bytEntrada, intTamanhoMax * i, bytTemp, 0, bytTemp.Length);
                bytEncriptados = rsaCriptografia.Encrypt(bytTemp, true);
                Array.Reverse(bytEncriptados);
                stbRetorno.Append(Convert.ToBase64String(bytEncriptados));
            }

            // Retornando texto encriptado
            return stbRetorno.ToString();
        }

        #endregion

        #region " Decriptar "

        public string Decriptar(string strTexto)
        {
            // Definindo o tamanho da chave
            RSACryptoServiceProvider rsaCriptografia = new RSACryptoServiceProvider(intTamanho);

            // Inserindo a chave privada
            rsaCriptografia.FromXmlString(strChavePrivada);

            // Declarando objetos utilizados
            ArrayList arrRetorno = new ArrayList();

            // Calculando tamanhos
            int intTamanhoChave = intTamanho / 8;
            int intTamanhoBloco = (intTamanhoChave % 3 != 0) ? ((intTamanhoChave / 3) * 4) + 4 : (intTamanhoChave / 3) * 4;
            int intInteracoes = strTexto.Length / intTamanhoBloco;

            // Efetuando descriptografia
            for (int i = 0; i < intInteracoes; i++)
            {
                byte[] bytEncriptados = Convert.FromBase64String(strTexto.Substring(intTamanhoBloco * i, intTamanhoBloco));
                Array.Reverse(bytEncriptados);
                arrRetorno.AddRange(rsaCriptografia.Decrypt(bytEncriptados, true));
            }

            // Retornando texto desencriptado
            return Encoding.UTF32.GetString(arrRetorno.ToArray(Type.GetType("System.Byte")) as byte[]);
        }

        #endregion
    }
}
