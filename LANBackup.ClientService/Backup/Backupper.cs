using LANBackup.Domain;
using LANBackup.Messages;
using Microsoft.Synchronization;
using Microsoft.Synchronization.Files;
using NServiceBus;
using System;
using System.Net;

namespace LANBackup.ClientService
{
    class Backupper
    {
        BackupStatusNotifier notifier;
        PerformBackupCommand command;
        public Backupper(IBus bus,PerformBackupCommand command)
        {
            this.notifier = new BackupStatusNotifier(bus,command);
            this.command = command;
        }

       public void DoBackup()
        {
            notifier.NotifyStart();
            var readCredential = new NetworkCredential(command.SourceCredential.UserId, PasswordEncryption.Decrypt(command.SourceCredential.Password), command.SourceCredential.Domain);
            var writeCredential = new NetworkCredential(command.DestinationCredential.UserId, PasswordEncryption.Decrypt(command.DestinationCredential.Password), command.DestinationCredential.Domain);
            using (new NetworkPermission(command.SourcePath, readCredential))
            using (new NetworkPermission(command.DestinationPath, writeCredential))
            {
                var filter = new FileSyncScopeFilter();
                SyncFileSystemReplicasOneWay(command.SourcePath, command.DestinationPath, filter, FileSyncOptions.None);
            }
            notifier.NotifyEnd();
        }

        private void SyncFileSystemReplicasOneWay(
                string sourceReplicaRootPath, string destinationReplicaRootPath,
                FileSyncScopeFilter filter, FileSyncOptions options)
        {
            FileSyncProvider sourceProvider = null;
            FileSyncProvider destinationProvider = null;

            try
            {
                sourceProvider = new FileSyncProvider(
                    sourceReplicaRootPath, filter, options);

                destinationProvider = new FileSyncProvider(
                    destinationReplicaRootPath, filter, options);
                destinationProvider.AppliedChange +=  new EventHandler<AppliedChangeEventArgs>(OnAppliedChange);
                destinationProvider.SkippedChange += new EventHandler<SkippedChangeEventArgs>(OnSkippedChange);
                destinationProvider.CopyingFile += new EventHandler<CopyingFileEventArgs>(OnCopyingFile);
                SyncOrchestrator agent = new SyncOrchestrator();

                agent.LocalProvider = sourceProvider;
                agent.RemoteProvider = destinationProvider;
                agent.Direction = SyncDirectionOrder.Upload; 

                agent.Synchronize();
                notifier.NotifyInfo("Backup in progress..");
            }
            catch(Exception ex)
            {
                notifier.NotifyError(ex.Message);
            }
            finally
            {                
                if (sourceProvider != null) sourceProvider.Dispose();
                if (destinationProvider != null) destinationProvider.Dispose();
            }
        }

        private void OnCopyingFile(object sender, CopyingFileEventArgs e)
        {
            var message = string.Format("File: {0}, Percent Completed: {1}", e.FilePath, e.PercentCopied);
            notifier.NotifyInfo(message);
        }

        private void OnSkippedChange(object sender, SkippedChangeEventArgs e)
        {
            var message = string.Format("Skiping File: {0}, Reason: {1}", e.CurrentFilePath, e.SkipReason);
            notifier.NotifyInfo(message);
        }

        private void OnAppliedChange(object sender, AppliedChangeEventArgs e)
        {
            var message = string.Format("Applied Change, old: {0}, new: {1}", e.OldFilePath, e.NewFilePath);
            notifier.NotifyInfo(message);
        }
    }
}
