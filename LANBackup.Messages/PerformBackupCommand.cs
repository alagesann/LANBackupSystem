using LANBackup.Domain;
using NServiceBus;
using System;

namespace LANBackup.Messages
{
    public class PerformBackupCommand : ICommand
    {
        public Guid Id { get; set; }      
        public string SourcePath { get; set; }
        public string DestinationPath { get; set; }
        public User SourceCredential { get; set; }
        public User DestinationCredential { get; set; }
    } 
}
