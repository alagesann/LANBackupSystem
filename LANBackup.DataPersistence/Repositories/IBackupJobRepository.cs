using LANBackup.Domain;
using System;
using System.Collections.Generic;

namespace LANBackup.DataPersistence
{
    public interface IBackupJobRepository
    {
        IEnumerable<BackupJob> GetAllBackupJobs();
        BackupJob GetBackupJobById(Guid Id);
        void CreateBackupJob(BackupJob job);
        void EditBackupJob(BackupJob job);      
        void DeleteBackupJob(Guid Id);
        void EnableBackupJob(Guid Id, bool isEnable);
    }
}
