using LANBackup.Domain;
using System;

namespace LANBackup.DataPersistence
{
    public interface ISchedulerRepository
    {
        void UpdateBackupJobStatus(Guid backupJobId, JobStatus status);
        void InsertLog(BackupLog log);
    }
}
