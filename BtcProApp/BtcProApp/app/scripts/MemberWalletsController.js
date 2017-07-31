var module = angular.module('app', []);
module.controller('MemberWallet', function ($scope, $http, $location, $window) {
    $http.get("/Home/MemberAllWallets").then(function (response) {
        $scope.members = response.data.Wallets;
    })
   
})