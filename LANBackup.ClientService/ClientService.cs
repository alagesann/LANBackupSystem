using System;
using System.ServiceProcess;
using NServiceBus;
using LANBackup.Messages;

namespace LANBackup.ClientService
{
    class ClientService : ServiceBase
    {
        IBus bus;
        public ClientService()
        {
            this.ServiceName = "LAN Backup Client Service";
            this.EventLog.Log = "Application";
           
            this.CanPauseAndContinue = true;
            this.CanShutdown = true;
            this.CanStop = true;
        }

        static void Main()
        {
            using (ClientService service = new ClientService())
            {
                Run(service);
            }
        }

        protected override void OnStart(string[] args)
        {
            BusConfiguration busConfiguration = new BusConfiguration();

            busConfiguration.EndpointName("LANBackupSystem.ClientService");
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
