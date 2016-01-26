using LANBackup.DataPersistence;
using LANBackup.Domain;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace LANBackup.AdminPortal.Controllers
{
    public class BackupJobsController : ApiController
    {
        IBackupJobRepository backupJobRepo = new BackupJobRepository();
        ISchedulerRepository schedulerRepo = new SchedulerRepository();
        public IEnumerable<BackupJob> Get()
        {
            return backupJobRepo.GetAllBackupJobs();
        }      

        public BackupJob Get(Guid id)
        {
            return backupJobRepo.GetBackupJobById(id);
        }

        public void Post([FromBody]BackupJob job)
        {
            // Must ensure to secure transport from browser to server through ssl.
            // Currently we store Base64 based password in db. 
            // STORING PASSWORD IN DB IS BAD AND THE APPROACH NEEDED TO BE IMPROVED. BEST IS AVOID STORING IT ANYWHERE.
            job.Id = Guid.NewGuid();
            job.SourceUser.Password = PasswordEncryption.Encrypt(job.SourceUser.Password); 
            job.DestinationUser.Password = PasswordEncryption.Encrypt(job.DestinationUser.Password);  
            backupJobRepo.CreateBackupJob(job);
        }

        [HttpPut]
        [System.Web.Http.AcceptVerbs("PUT")]
        public void Put([FromBody]BackupJob job)
        {
            backupJobRepo.EditBackupJob(job);
        }
      
        [HttpDelete]
        [System.Web.Http.AcceptVerbs("Delete")]
        public void Delete(Guid id)
        {
            backupJobRepo.DeleteBackupJob(id);
        }

        [Route("api/backupjobs/enable")]
        [HttpPut]
        [System.Web.Http.AcceptVerbs("PUT")]
        public void Enable(Guid id)
        {
            backupJobRepo.EnableBackupJob(id, true);
        }
        [Route("api/backupjobs/disable")]
        [HttpPut]
        [System.Web.Http.AcceptVerbs("PUT")]
        public void Disable(Guid id)
        {
            backupJobRepo.EnableBackupJob(id, false);
        }       
    }
}