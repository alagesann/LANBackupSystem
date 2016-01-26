using System;

namespace LANBackup.Domain
{
   public  class BackupLog
    {
        public Guid Id { get; set; }   
        public Guid BackupJobId { get; set; }       
        public string Message { get; set; }
    }
}
