LANBackupApp.controller("MainController",
["$scope", "$location", "BackupJobsService",
function ($scope, $location, BackupJobsService) {

    $scope.backupJobs = [];
    $scope.isDisabled = true;
    $scope.selectedJobId = null;
    $scope.setSelected = function (id) {
        $scope.selectedJobId = id;
        $scope.isDisabled = false;
    };

    function init() {
        loadAllJobs();
    };
    function loadAllJobs() {
        BackupJobsService.getBackupJobs().then(
    function (results) {
        $scope.backupJobs = results.data;
    });
    }
    $scope.createBackupJob = function () {
        $location.path('/create');
    };

    $scope.editBackupJob = function () {
        $location.path('/edit/' + $scope.selectedJobId)
    };

    $scope.enableBackupJob = function (id) {
        BackupJobsService.enableBackupJob($scope.selectedJobId);
        loadAllJobs();
    };

    $scope.disableBackupJob = function () {
        BackupJobsService.disableBackupJob($scope.selectedJobId);
        loadAllJobs();
    };

    $scope.deleteBackupJob = function () {
        BackupJobsService.deleteBackupJob($scope.selectedJobId);
        loadAllJobs();
    };

    $scope.scheduleJobs = function () {       
        BackupJobsService.scheduleJobs($scope.backupJobs);
        loadAllJobs();
    };

    init();

}]);