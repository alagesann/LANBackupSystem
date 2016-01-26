
LANBackupApp.factory('BackupJobsService',
    ["$http",
    function ($http) {
        var baseUrl = 'http://localhost/lanbackup.adminportal/api';
        var getBackupJobs = function () {
            return $http.get(baseUrl+"/backupjobs");
        };

        var getBackupJob = function (id) {
            return $http.get(baseUrl+"/backupjobs/"+id);
        };

        var createBackupJob = function (newjob) {
            return $http.post(baseUrl+"/backupjobs", JSON.stringify(newjob));
        };

        var editBackupJob = function (editJob) {
            return $http.put(baseUrl+"/backupjobs", JSON.stringify(editJob));
        };

        var enableBackupJob = function (id) {
            return $http.put(baseUrl + "/backupjobs/enable/"+id);
        };

        var disableBackupJob = function (id) {
            return $http.put(baseUrl + "/backupjobs/disable/"+ id);
        };

        var deleteBackupJob = function (id) {
            return $http.delete(baseUrl + "/backupjobs/" + id);
        };

        var scheduleJobs = function (jobs) {
            return $http.post(baseUrl + "/scheduler", JSON.stringify(jobs));
        };
        function removeHashKey(jobs) {
            var cleanedJobs = JSON.stringify(jobs, function (key, value) {
                if (key === "$$hashKey") {
                    return undefined;
                }
                return value;
            });
            return cleanedJobs;
        }
        return {
            createBackupJob: createBackupJob,
            editBackupJob: editBackupJob,
            getBackupJob: getBackupJob,
            getBackupJobs: getBackupJobs,
            enableBackupJob: enableBackupJob,
            disableBackupJob: disableBackupJob,
            deleteBackupJob: deleteBackupJob,
            scheduleJobs: scheduleJobs
        };
    }]);