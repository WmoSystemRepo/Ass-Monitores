using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using DFe;

namespace Receptor.DevHost
{
    /// <summary>
    /// Host POC do Monitor — NÃO faz parte do Windows Service original.
    /// Reutiliza ServWindows.StartDebug() já existente no DFEND_CTe_Receptor
    /// (o mesmo caminho usado quando o depurador está anexado), sem alterar o fonte original.
    ///
    /// Captura Debug.WriteLine do Receptor (ex.: conexao iniciando, config banco OK)
    /// em monitor-live.log para o BFF exibir o fluxo em tempo real no painel.
    /// </summary>
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var livePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "monitor-live.log");
            InstallLiveTrace(livePath);

            WriteLive("DEVHOST", "Receptor.DevHost iniciando (host POC do monitor)");
            WriteLive("BOOTSTRAP", "Carregando serviço e configurações…");

            var service = new ServWindows();
            service.StartDebug(args ?? new string[0]);

            WriteLive("BOOTSTRAP", "StartDebug concluído — workers em loop (aguarde Intervalo entre ciclos)");
            Thread.Sleep(Timeout.Infinite);
        }

        private static void InstallLiveTrace(string livePath)
        {
            try
            {
                // Novo arquivo a cada start — evita lixo de execuções antigas
                File.WriteAllText(livePath, string.Empty, Encoding.UTF8);

                // Debug.WriteLine do DFEND_CTe_Receptor (Threads.cs) cai aqui
                var listener = new TextWriterTraceListener(livePath, "MonitorLive")
                {
                    TraceOutputOptions = TraceOptions.DateTime
                };
                Debug.Listeners.Add(listener);
                Debug.AutoFlush = true;
                Trace.Listeners.Add(listener);
                Trace.AutoFlush = true;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Não foi possível abrir monitor-live.log: " + ex.Message);
            }
        }

        private static void WriteLive(string step, string message)
        {
            Debug.WriteLine(string.Format(
                "[{0:yyyy-MM-dd HH:mm:ss.fff}] [{1}] {2}",
                DateTime.Now,
                step,
                message));
        }
    }
}
