
var LANBackupApp = angular.module('LANBackupApp', ["ngRoute", "ui.bootstrap"]);

LANBackupApp.config(["$routeProvider", "$locationProvider",
    function ($routeProvider, $locationProvider) {
    $routeProvider
        .when("/main", {
            templateUrl: "app/Main.html",
            controller: "MainController"
        })
        .when("/create", {
            templateUrl: "app/BackupJobsTemplate.html",
            controller: "BackupJobsController"
        })
         .when("/edit/:id", {
             templateUrl: "app/BackupJobsTemplate.html",
             controller: "BackupJobsController"
         })
        .otherwise({
            redirectTo: "/main"
        });
    //$locationProvider.html5Mode(true);
}]);