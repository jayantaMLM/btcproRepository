var module = angular.module('app', []);

module.controller('Purchase', function ($scope, $http) {
    $scope.showInvoice = false;
    $scope.showIllustration = false;
    $scope.invObj = {};
    $scope.batchno = "";

    $http.get("/Home/MyPurchases").then(function (purdata) {
        $scope.purchases = purdata.data.Purchases;
    })

    $http.get("/Home/MyAddress").then(function (retdata) {
        $scope.address = retdata.data.Address;
    })

    $scope.selectedInvoice = function (purObj) {
        $scope.invObj = purObj;
        $scope.showInvoice = true;
    }

    $scope.getIllustration = function (purobj) {
        
        $http.get('/Home/GetFixedIncomeIllustration?Guid='+purobj.ReferenceNo).then(function (response) {
            $scope.ledger = response.data.FixedIncomeArray;
            if ($scope.ledger.length > 0) {
                var calculatedAmt = 0;
                angular.forEach($scope.ledger, function (value, index) {
                    calculatedAmt = calculatedAmt + value.Amount;
                    value.Total = calculatedAmt;
                })
            }
            $scope.showIllustration = true;
        })
    }
})