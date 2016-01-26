using LANBackup.DataPersistence;
using LANBackup.Messages;
using NServiceBus;
using System.Diagnostics;

namespace LANBackup.LoggerService
{
    public class JobStatusChangedEventHandler : IHandleMessages<JobStatusChangedEvent>
    {
        static  ISchedulerRepository schedulerRepository;
        public JobStatusChangedEventHandler()
        {
            schedulerRepository = new SchedulerRepository();
        }

        public void Handle(JobStatusChangedEvent jobEvent)
        {
            var log = new System.Diagnostics.EventLog();
            log.Source = "LAN Backup Logger Service";
            log.Log = "Application";
            log.WriteEntry("recieved JobStatusChangedEvent.", EventLogEntryType.Information);
            schedulerRepository.UpdateBackupJobStatus(jobEvent.BackupJobId, jobEvent.Status);
            log.WriteEntry("finished updating database.", EventLogEntryType.Information);
        }
    }
}
