var module = angular.module('app',[]);

module.controller('UploadWallet', function ($scope, $http) {

    $http.get("/Home/GetWalletAccount").then(function (response) {
        $scope.wallets = response.data.Stats[0];
    })

})
