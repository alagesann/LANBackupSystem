using LANBackup.Messages;
using NServiceBus;
using NServiceBus.Logging;
using System;

namespace LANBackup.AdminConsole
{
    public class BackupEventHandler : IHandleMessages<BackupEvent>
    {
        public void Handle(BackupEvent  backupEvent)
        {
            System.Console.WriteLine("Log: job: {0}, message: {1}", backupEvent.BackupJobId, backupEvent.Message);
        }
    }
}
