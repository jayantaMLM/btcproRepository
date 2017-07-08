var module = angular.module('app',[]);

module.controller('MemberBlockUnblock', function ($scope, $http, $location) {
    $scope.username = "";
    $scope.status = "";
    $scope.blockunblock = "";
    $scope.isUsernameFound = false;
    $scope.isBlockedUnblocked = false;
    $scope.errormessage = "";
    $scope.showBlockUnblock = false;

    $scope.checkLoginUsername = function () {
        if ($scope.username != "") {
            $http.get("/Home/IsUserNameExists2?UserName=" + $scope.username).then(function (data) {
                $scope.isUsernameFound = data.data.Found;
                $scope.isBlockedUnblocked = data.data.BlockStatus;
                if ($scope.isUsernameFound == false) {
                    $scope.errormessage = "Username not found";
                    $scope.status = "";
                    $scope.showBlockUnblock = false;
                } else {
                    $scope.showBlockUnblock = true;
                    $scope.errormessage = "";
                    if ($scope.isBlockedUnblocked) {
                        $scope.status = "BLOCKED";
                        $scope.blockunblock = "True";
                    } else {
                        $scope.status = "UN-BLOCKED";
                        $scope.blockunblock = "False";
                    }
                }
            })
        }
    }

    $scope.updateStatus = function () {
        debugger;
        $http.get("/Home/IsUserNameExists2Update?UserName=" + $scope.username + "&BlockStatus=" + $scope.blockunblock).then(function (data) {
            $scope.isUsernameFound = data.data.Found;
            $scope.isBlockedUnblocked = data.data.BlockStatus;
            if ($scope.isUsernameFound == false) {
                $scope.errormessage = "Username not found";
                $scope.status = "";
                $scope.showBlockUnblock = false;
            } else {
                $scope.showBlockUnblock = true;
                $scope.errormessage = "";
                if ($scope.isBlockedUnblocked) {
                    $scope.status = "BLOCKED";
                    $scope.blockunblock = "True";
                } else {
                    $scope.status = "UN-BLOCKED";
                    $scope.blockunblock = "False";
                }
            }
        })
    }
})
