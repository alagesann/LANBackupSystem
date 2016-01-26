using LANBackup.DataPersistence;
using LANBackup.Messages;
using NServiceBus;
using System;
using System.Diagnostics;

namespace LANBackup.LoggerService
{
    public class BackupEventHandler : IHandleMessages<BackupEvent>
    {
        static ISchedulerRepository schedulerRepository;

        public BackupEventHandler()
        {
            schedulerRepository = new SchedulerRepository();
        }

        public void Handle(BackupEvent  backupEvent)
        {
            var log = new System.Diagnostics.EventLog();
            log.Source = "LAN Backup Logger Service";
            log.Log = "Application";
            log.WriteEntry("recieved BackupEvent.", EventLogEntryType.Information);
            try {
                schedulerRepository.InsertLog(new Domain.BackupLog
                {
                    Id = backupEvent.Id,
                    BackupJobId = backupEvent.BackupJobId,
                    Message = backupEvent.Message
                });
            }
            catch(Exception ex)
            {
                log.WriteEntry("Exception:."+ex.InnerException.Message, EventLogEntryType.Error);
                log.WriteEntry("Exception:." + ex.InnerException.InnerException.Message, EventLogEntryType.Error);
            }
            log.WriteEntry("finished updating database.", EventLogEntryType.Information);
        }
    }
}
