using LANBackup.Messages;
using LANBackup.Domain;
using NServiceBus;

namespace LANBackup.LoggerService
{
    public class JobStatusChangedEventHandler : IHandleMessages<JobStatusChangedEvent>
    { 
        public void Handle(JobStatusChangedEvent jobStatus)
        {            
            System.Console.WriteLine("Status: job: {0}, status: {1}",jobStatus.BackupJobId, jobStatus.Status);
        }
    }
}
