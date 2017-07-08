var module = angular.module('app', []);
module.controller('MemberArea', function ($scope, $http, $location, $window) {
    $scope.username = "";
    $scope.password = "";

    $scope.openWindow = function () {
        alert("Opening a new window");
        $window.open("/Home/openMember", "_blank");
    }
})