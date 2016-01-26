
using LANBackup.Domain;
using NServiceBus;
using System;

namespace LANBackup.Messages
{
    public class JobStatusChangedEvent : IEvent
    {
        public Guid BackupJobId { get; set; }
        public JobStatus Status { get; set; }
    }
}
