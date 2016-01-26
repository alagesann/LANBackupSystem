using LANBackup.Domain;
using LANBackup.Messages;
using NServiceBus;
using System.Collections.Generic;
using System.Web.Http;
using System.Linq;
using System;

namespace LANBackup.AdminPortal.Controllers
{
    public class SchedulerController : ApiController
    {
        IBus bus;
        public SchedulerController(IBus bus)
        {
            this.bus = bus;
        }
        public void Post([FromBody]IEnumerable<BackupJob> jobs)
        {
            jobs.ToList().ForEach(job =>
            {              
                var performBackupCommand = new PerformBackupCommand();
                performBackupCommand.Id = job.Id;
                performBackupCommand.SourcePath = job.SourcePath;
                performBackupCommand.DestinationPath = job.DestinationPath;
                performBackupCommand.SourceCredential = new User { UserId = job.SourceUser.UserId, Password = job.SourceUser.Password, Domain = "ds" };
                performBackupCommand.DestinationCredential = new User { UserId = job.DestinationUser.UserId, Password = job.DestinationUser.Password, Domain = "ds" };
                bus.Send("LANBackupSystem.ClientService", performBackupCommand);
            });
        }             
    }
}