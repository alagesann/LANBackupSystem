using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceProcess;
using NServiceBus;

namespace LANBackup.LoggerService
{
    class LoggerService : ServiceBase
    {
        IBus bus;
        public LoggerService()
        {
            this.ServiceName = "LAN Backup Logger Service";           
           
            this.EventLog.Source = this.ServiceName;
            this.EventLog.Log = "Application";

            this.CanPauseAndContinue = true;
            this.CanShutdown = true;
            this.CanStop = true;
        }

        static void Main()
        {
            using (LoggerService service = new LoggerService())
            {
                Run(service);
            }
        }

        protected override void OnStart(string[] args)
        {
            BusConfiguration busConfiguration = new BusConfiguration();

            busConfiguration.EndpointName("LANBackupSystem.LoggerService");
            busConfiguration.UseSerialization<JsonSerializer>();
            busConfiguration.UsePersistence<InMemoryPersistence>();
            busConfiguration.EnableInstallers();
            bus = Bus.Create(busConfiguration).Start();
        }

        protected override void OnStop()
        {
            if (bus != null)
            {
                bus.Dispose();
            }
        }
    }
}
