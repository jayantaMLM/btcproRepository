var module = angular.module('app',[]);

module.controller('ReferralLink', function ($scope, $http, $location) {
    $scope.position = "L";
    $scope.isMember = false;

    $http.get("/Home/IsUserMember").then(function (response) {
        if (response.data.Found) { $scope.isMember = true;}
    })
    $http.get("/Home/Membername").then(function (data) {
        $scope.user = data.data.CurrentUser;
    });
    $scope.path = $location.host();
    $scope.port = $location.port();

    $scope.setPosition = function () {
        $http.get("/Home/SetWorkingLeg?leg=" + $scope.position).then(function(data){
           
        })
    }

    $scope.getPosition = function () {
        debugger;
        $http.get("/Home/GetWorkingLeg?user=").then(function (data) {
            $scope.position = data.data.Position;
            if ($scope.position == null) {
                $scope.position = "L";
            }
        })
    }
    $scope.getPosition();


})
