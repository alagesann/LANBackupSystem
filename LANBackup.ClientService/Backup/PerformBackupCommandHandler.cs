using LANBackup.Messages;
using NServiceBus;
using NServiceBus.Logging;
using System;

namespace LANBackup.ClientService
{
    public class PerformBackupCommandHandler : IHandleMessages<PerformBackupCommand>
    {      
        IBus bus;

        public PerformBackupCommandHandler(IBus bus)
        {
            this.bus = bus;
        }
        public void Handle(PerformBackupCommand command)
        {
            try
            {               
                var backupper = new Backupper(bus,command);
                backupper.DoBackup();
            }
            catch(Exception ex)
            {
                bus.Publish<BackupEvent>(e =>
                {
                    e.Id = Guid.NewGuid();
                    e.BackupJobId = command.Id;
                    e.Status = Domain.JobStatus.Error;
                    e.Message = "exception:" + ex.InnerException.Message;
                });
            }
        }
    }
}
