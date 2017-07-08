var module = angular.module('app', []);

module.controller('Team', function ($scope, $http) {
    $scope.team = [];
    $scope.loading = true;
    $http.get("/Home/GetTeam?RegistrationId=null").then(function (teamdata) {
        $scope.team = teamdata.data.Members;
        $scope.loading = false;
    })
    $http.get("/Home/GetMyFreeMembers").then(function (teamdata) {
        debugger;
        $scope.freeteam = teamdata.data.FreeMembers;
        $scope.loading = false;
    })
})