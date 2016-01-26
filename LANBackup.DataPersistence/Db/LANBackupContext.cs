using LANBackup.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LANBackup.DataPersistence.Db
{
  public class LANBackupContext: DbContext
    {
        public LANBackupContext(): base("LANBackupContext") 
        {
        }
        public DbSet<BackupJob> BackupJobs { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<BackupLog> BackupLogs { get; set; }

    }
}
