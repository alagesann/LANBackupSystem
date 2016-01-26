using LANBackup.Messages;
using NServiceBus;
using System;

namespace LANBackup.AdminConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.EndpointName("LANBackup.AdminConsole");
            busConfiguration.UseSerialization<JsonSerializer>();
            busConfiguration.UsePersistence<InMemoryPersistence>();
            busConfiguration.EnableInstallers();
            using (IBus bus = Bus.Create(busConfiguration).Start())
            {
                Start(bus);
            }
        }
        static void Start(IBus bus)
        {           
            Console.WriteLine("Press 'Enter' to publish a message.To exit, Ctrl + C");
            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey();
                var performBackupCommand = new PerformBackupCommand();
                performBackupCommand.Id = Guid.NewGuid();
                performBackupCommand.SourcePath = @"\\192.168.0.143\ToShare\source";
                performBackupCommand.DestinationPath = @"\\192.168.0.143\ToShare\dest";
                performBackupCommand.SourceCredential = new Domain.User { UserId = "palania", Password = "Ancha$2909", Domain = "ds" };
                performBackupCommand.DestinationCredential = new Domain.User { UserId = "palania", Password = "Ancha$2909", Domain = "ds" };
                bus.Send("LANBackupSystem.ClientService", performBackupCommand);
                Console.WriteLine("Command sent");
            }
        }
    }
}
