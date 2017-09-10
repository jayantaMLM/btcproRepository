var module = angular.module('app', []);

module.controller('BusinessStats', function ($scope, $http) {

    $http.get("/Home/GetBusinessStats").then(function (response) {
        $scope.stats = response.data.Stats[0];
    })

    $http.get("/Home/GetWalletAccount").then(function (response) {
        $scope.wallets = response.data.Stats[0];
    })

    $scope.save = function () {
        $http.post("/Home/PostBusinessStats", $scope.stats).then(function (response) {
            alert("Data saved successfully!");
        })
    }

    $scope.saveWallets = function () {
        $http.post("/Home/PostWalletAccount", $scope.wallets).then(function (response) {
            alert("Data saved successfully!");
        })
    }
})
