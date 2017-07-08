var module = angular.module('app', []);

module.controller('Contactus', function ($scope, $http) {
    $scope.fullname = "";
    $scope.phoneno = "";
    $scope.emailid = "";
    $scope.country = "";
    $scope.subject = "";
    $scope.message = "";

    $scope.SendMail = function () {
        $http.post("/Home/SendContactUsEMail?fullname="+$scope.fullname+"&phone="+$scope.phoneno+"&country="+$scope.country+"&mailfrom=" + $scope.emailid + "&mail_to=&mail_cc=&subj=" + $scope.subject + "&desc=" + $scope.message).then(function (response) {
            alert("Mail delivered successfully!");
            $scope.fullname = "";
            $scope.phoneno = "";
            $scope.emailid = "";
            $scope.country = "";
            $scope.subject = "";
            $scope.message = "";
        })

    }
})
