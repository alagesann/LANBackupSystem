using LANBackup.Messages;
using NServiceBus;
using System;

namespace LANBackup.ClientService
{
    public class BackupStatusNotifier
    {
        IBus bus;
        PerformBackupCommand command;
        public BackupStatusNotifier(IBus bus, PerformBackupCommand command)
        {
            this.bus = bus;
            this.command = command;
        }

        public void NotifyStart()
        {
            bus.Publish<JobStatusChangedEvent>(e =>
            {             
                e.BackupJobId = command.Id;
                e.Status = Domain.JobStatus.Started;             
            });
            var message = string.Format("Starting Backup from path: {0}, to path: {0}", command.SourcePath, command.DestinationPath);
            bus.Publish<BackupEvent>(e =>
            {
                e.Id = Guid.NewGuid();
                e.BackupJobId = command.Id;
                e.Status = Domain.JobStatus.Started;
                e.Message = message;
            });
        }

        public void NotifyEnd()
        {
            bus.Publish<JobStatusChangedEvent>(e =>
            {
                e.BackupJobId = command.Id;
                e.Status = Domain.JobStatus.Completed;
            });
            var message = string.Format("Completed Backup");
            bus.Publish<BackupEvent>(e =>
            {
                e.Id = Guid.NewGuid();
                e.BackupJobId = command.Id;
                e.Status = Domain.JobStatus.Completed;
                e.Message = message;
            });
        }

        public void NotifyInfo(string message)
        {
            bus.Publish<BackupEvent>(e =>
            {
                e.Id = Guid.NewGuid();
                e.BackupJobId = command.Id;
                e.Status = Domain.JobStatus.Started;
                e.Message = message;
            });
        }
        public void NotifyError(string message)
        {
            bus.Publish<BackupEvent>(e =>
            {
                e.Id = Guid.NewGuid();
                e.BackupJobId = command.Id;
                e.Status = Domain.JobStatus.Error;
                e.Message = message;
            });

            bus.Publish<JobStatusChangedEvent>(e =>
            {
                e.BackupJobId = command.Id;
                e.Status = Domain.JobStatus.Error;
            });
        }
    }
}
