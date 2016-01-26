using System;
namespace LANBackup.Domain
{
    public class BackupJob
    {
        public Guid Id { get; set; }
        public JobStatus Status { get; set; }       
        public bool IsEnabled { get; set; }      
        public string SourcePath { get; set; }      
        public string DestinationPath { get; set; }       
        public User SourceUser { get; set; }      
        public User DestinationUser { get; set; }      
        public Client Client { get; set; }
    }
}