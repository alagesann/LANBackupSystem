using LANBackup.Domain;
using System;
using System.Linq;

namespace LANBackup.DataPersistence
{
   public  class SchedulerRepository : ISchedulerRepository
    {
        public void InsertLog(BackupLog log)
        {
            using (var ctx = new LANBackupContext())
            {
                var dbLog = new BACKUPLOG { ID = log.Id, BACKUPJOBID = log.BackupJobId, MESSAGE = log.Message };
                ctx.BACKUPLOGs.Add(dbLog);
                ctx.SaveChanges();
            }
        }

        public void ScheduleJob(Guid backupJobId, string clientId)
        {
            //using (var ctx = new LANBackupContext1())
            //{
            //    var job = ctx.BackupJobs.FirstOrDefault(p => p.Id == backupJobId);
            //    if (job != null)
            //    {
            //        var client = new Client { ClientId = clientId };
            //        job.Client=client;
            //        ctx.SaveChanges();
            //    }
            //}
        }

        public void UpdateBackupJobStatus(Guid backupJobId, JobStatus status)
        {
            using (var ctx = new LANBackupContext())
            {
                var job = ctx.BACKUPJOBS.FirstOrDefault(p => p.ID == backupJobId);
                if (job != null)
                {
                    job.STATUS = (int) status;
                    ctx.SaveChanges();
                }
            }
        }

        public void UpdateClientAvailabilityStatus(Client client)
        {
            //using (var ctx = new LANBackupContext1())
            //{
            //    var dbClient = ctx.Clients.FirstOrDefault(p => p.ClientId == client.ClientId);
            //    if (dbClient == null)
            //        ctx.Clients.Add(client);
            //    else dbClient.isAvailable = client.isAvailable;
            //    ctx.SaveChanges();
            //}
        }      
    }
}
