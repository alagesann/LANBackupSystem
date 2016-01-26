using LANBackup.Domain;
using System.Linq;
using System;
using System.Collections.Generic;

namespace LANBackup.DataPersistence
{
   public class BackupJobRepository : IBackupJobRepository
    {
        public void CreateBackupJob(BackupJob job)
        {
            CreateUser(job.SourceUser);
            CreateUser(job.DestinationUser);
            CreateClient(job.Client);
            using (var ctx = new LANBackupContext())
            {
                var dbJob = MapToDbBackupJob(job);
                ctx.BACKUPJOBS.Add(dbJob);                
                ctx.SaveChanges();
            }
        }
        private BACKUPJOB MapToDbBackupJob (BackupJob job)
        {
            return new BACKUPJOB
            {
                ID = job.Id,
                STATUS = (int)job.Status,
                ISENABLED = job.IsEnabled,
                SOURCE_PATH = job.SourcePath,
                DESTINATION_PATH = job.DestinationPath,
                SOURCE_USERID = job.SourceUser.UserId,
                DESTINATION_USERID = job.DestinationUser.UserId,
                CLIENT_ID = job.Client.ClientId
            };
        }
        private void CreateUser(User user)
        {
            using (var ctx = new LANBackupContext())
            {
                var dbuser=ctx.USERS.FirstOrDefault(p => p.USERID == user.UserId);
                if (dbuser != null)
                {
                    dbuser.PASSWORD = user.Password;
                    dbuser.DOMAIN = user.Domain;
                    ctx.SaveChanges();
                }
                else
                {
                    dbuser = new USER { USERID = user.UserId, PASSWORD = user.Password, DOMAIN = user.Domain };
                    ctx.USERS.Add(dbuser);
                    ctx.SaveChanges();
                }
                          
            }
        }
        private User GetUser(string userId)
        {
            using (var ctx = new LANBackupContext())
            {
                var dbuser = ctx.USERS.FirstOrDefault(p => p.USERID == userId);
                return new User { UserId = dbuser.USERID, Password = dbuser.PASSWORD, Domain=dbuser.DOMAIN };          
            }
        }
        private void CreateClient(Client client)
        {
            using (var ctx = new LANBackupContext())
            {
                var dbclient = ctx.CLIENTs.FirstOrDefault(p => p.CLIENTID == client.ClientId);
                if (dbclient != null)
                {
                    dbclient.ISAVAILABLE = client.isAvailable;
                    ctx.SaveChanges();
                }

                else
                {
                    dbclient = new CLIENT { CLIENTID = client.ClientId, ISAVAILABLE = client.isAvailable };
                    ctx.CLIENTs.Add(dbclient);
                    ctx.SaveChanges();
                }
            }
        }
        private Client GetClient(string clientId)
        {
            using (var ctx = new LANBackupContext())
            {
                var dbclient = ctx.CLIENTs.FirstOrDefault(p => p.CLIENTID == clientId);
                return new Client { ClientId = dbclient.CLIENTID, isAvailable = dbclient.ISAVAILABLE };
            }
        }
        public void DeleteBackupJob(Guid Id)
        {
            using (var ctx = new LANBackupContext())
            {
                var job = ctx.BACKUPJOBS.FirstOrDefault(p => p.ID == Id);
                var log = ctx.BACKUPLOGs.FirstOrDefault(p => p.BACKUPJOBID == job.ID);
                if (log != null) throw new Exception("Deleting already run job is not allowed");
                if (job != null)
                {
                    ctx.BACKUPJOBS.Remove(job);
                    ctx.SaveChanges();
                }
            }
        }

        public void EditBackupJob(BackupJob job)
        {
            using (var ctx = new LANBackupContext())
            {
                var editJob = ctx.BACKUPJOBS.FirstOrDefault(p => p.ID == job.Id);
                var log = ctx.BACKUPLOGs.FirstOrDefault(p => p.ID == job.Id);
                if (log != null) throw new Exception("Editing on already run job is not Allowed");
                if (editJob != null)
                {
                    var dbJob = MapToDbBackupJob(job);
                    ctx.BACKUPJOBS.Remove(editJob);
                    ctx.BACKUPJOBS.Add(dbJob);
                    ctx.SaveChanges();
                }
            }
        }

        public IEnumerable<BackupJob> GetAllBackupJobs()
        {
            var backupJobs = new List<BackupJob>();
            using (var ctx = new LANBackupContext())
            {
                ctx.BACKUPJOBS
                    .ToList().ForEach(p => backupJobs.Add(new BackupJob
                    {
                        Id = p.ID,
                        Status = (JobStatus) p.STATUS,
                        SourcePath = p.SOURCE_PATH,
                        DestinationPath = p.DESTINATION_PATH,
                        IsEnabled = p.ISENABLED,
                        Client = GetClient(p.CLIENT_ID),
                        SourceUser = GetUser(p.SOURCE_USERID),
                        DestinationUser = GetUser(p.DESTINATION_USERID)
                    }));
                return backupJobs;            
            }
        }
        
        public BackupJob GetBackupJobById(Guid Id)
        {           
            using (var ctx = new LANBackupContext())
            {
                var backupJob=ctx.BACKUPJOBS.FirstOrDefault(p => p.ID == Id);
                return new BackupJob
                {
                    Id = backupJob.ID,
                    Status = (JobStatus)backupJob.STATUS,
                    SourcePath = backupJob.SOURCE_PATH,
                    DestinationPath = backupJob.DESTINATION_PATH,
                    IsEnabled = backupJob.ISENABLED,
                    Client = GetClient(backupJob.CLIENT_ID),
                    SourceUser = GetUser(backupJob.SOURCE_USERID),
                    DestinationUser = GetUser(backupJob.DESTINATION_USERID)
                };               
            }
        }

       private BackupJob GetBackupJob(BACKUPJOB DbBackupJob)
        {
            return new BackupJob
            {
                Id = DbBackupJob.ID,
                Status = (JobStatus)DbBackupJob.STATUS,
                IsEnabled = DbBackupJob.ISENABLED,
                SourcePath = DbBackupJob.SOURCE_PATH,
                DestinationPath = DbBackupJob.DESTINATION_PATH,
            };
        }
        public void EnableBackupJob(Guid Id, bool isEnable)
        {
            using (var ctx = new LANBackupContext())
            {
                var job = ctx.BACKUPJOBS.FirstOrDefault(p => p.ID == Id);
                if (job != null)
                {
                    job.ISENABLED = isEnable;
                    ctx.SaveChanges();
                }
            }
        }
    }
}
