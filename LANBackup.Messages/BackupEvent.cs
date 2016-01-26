
using LANBackup.Domain;
using NServiceBus;
using System;

namespace LANBackup.Messages
{
    public class BackupEvent : IEvent
    {
        public Guid Id { get; set; }
        public Guid BackupJobId { get; set; }
        public JobStatus Status { get; set; }
        public string Message { get; set; }
    }
}
