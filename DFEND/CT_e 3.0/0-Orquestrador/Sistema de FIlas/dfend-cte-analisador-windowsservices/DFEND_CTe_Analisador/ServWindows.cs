using System;
using System.Diagnostics;
using System.ServiceProcess;
using System.Timers;

namespace DFe
{
    public partial class ServWindows : ServiceBase
    {
        #region " Main "

        static void Main()
        {
            if (Debugger.IsAttached)
            {
                ServWindows serviceTeste = new ServWindows();
                serviceTeste.StartDebug(new string[2]);
                System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);
            }
            else
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                    new ServWindows()
                };
                ServiceBase.Run(ServicesToRun);
            }
        }

        public void StartDebug(string[] args)
        {
            OnStart(args);
        }

        #endregion

        #region " Variaveis "

        // Classes utilizadas
        Timer tmrCronometro = null;

        #endregion

        #region " Construtores "

        public ServWindows()
        {
            InitializeComponent();
        }

        #endregion

        #region " OnStart "

        protected override void OnStart(string[] args)
        {
            try
            {
                // Iniciando o cronometro
                tmrCronometro = new Timer(1000);
                tmrCronometro.Elapsed += new ElapsedEventHandler(this.OnElapsedEvent);
                tmrCronometro.AutoReset = true;
                tmrCronometro.Enabled = true;
                tmrCronometro.Start();
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry(ex.Message, EventLogEntryType.Error);
            }
        }

        #endregion

        #region " OnStop "

        protected override void OnStop()
        {
            try
            {
                // Finalizando cronometro
                tmrCronometro.Stop();
                tmrCronometro.Enabled = false;
                tmrCronometro.Dispose();
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry(ex.Message, EventLogEntryType.Error);
            }
        }

        #endregion

        #region " OnPause "

        protected override void OnPause()
        {
            try
            {
                // Parando cronometro
                tmrCronometro.Enabled = false;
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry(ex.Message, EventLogEntryType.Error);
            }
        }

        #endregion

        #region " OnContinue "

        protected override void OnContinue()
        {
            try
            {
                // Reiniciando cronometro
                tmrCronometro.Enabled = true;
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry(ex.Message, EventLogEntryType.Error);
            }
        }

        #endregion

        #region " OnElapsedEvent "

        protected void OnElapsedEvent(object obj, ElapsedEventArgs e)
        {
            try
            {
                // Parando cronometro
                tmrCronometro.Enabled = false;

                // Preparando processos a serem rodados
                Threads objThread = new Threads();
                objThread.StartPooledThread();
                objThread = null;
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry(ex.Message, EventLogEntryType.Error);
            }
            finally
            {
                tmrCronometro = null;
            }
        }

        #endregion
    }
}
