
LANBackupApp.controller('BackupJobsController',
    ["$scope", "$window", "$routeParams", "BackupJobsService",
    function BackupJobsController($scope, $window, $routeParams, BackupJobsService) {

        if ($routeParams.id)
            BackupJobsService.getBackupJob($routeParams.id).then(
                function (result) {
                    $scope.editableJob = result.data;
                },
                  function (results) {
                      $scope.hasFormError = true;
                      $scope.formErrors = results.statusText;
                  }
                );
        else {
            $scope.job = {
                id: 0, status: 0, isEnabled: 1, sourcePath: '', destinationPath: '', sourceUser: { userId: '', password: '', domain: '' },
                 destinationUser: { userId: '', password: '', domain: '' }, client: {clientId: '', isAvailable: 1}
            };
            $scope.editableJob = angular.copy($scope.job)
        }       

        $scope.submitForm = function () {

            $scope.$broadcast('show-errors-event');

            if ($scope.jobForm.$invalid)
                return;

            if ($scope.editableJob.id == 0) {
                BackupJobsService.createBackupJob($scope.editableJob).then(
                    function (results) {
                        $scope.job = angular.copy($scope.editableJob);
                        $window.history.back();
                    },
                    function (results) {
                        $scope.hasFormError = true;
                        $scope.formErrors = results.statusText;
                    });
            }
            else {
                BackupJobsService.editBackupJob($scope.editableJob).then(
                    function (results) {
                        $scope.job = angular.copy($scope.editableJob);
                        $window.history.back();
                    },
                    function (results) {
                        $scope.hasFormError = true;
                        $scope.formErrors = results.statusText;
                    });
            }
        };

        $scope.cancelForm = function () {
            $window.history.back();
        };

        $scope.resetForm = function () {
            $scope.$broadcast('hide-errors-event');
        }

    }]);


