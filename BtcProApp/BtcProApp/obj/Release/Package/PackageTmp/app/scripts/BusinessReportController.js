var module = angular.module('app', []);

module.controller('BusinessReport', function ($scope, $http) {
    $scope.calculatedAmt = 0;
    $scope.sponsorTotalIncome = 0;
    $scope.generationTotalIncome = 0;
    $scope.modifiedGenerationLedger = [];

    $scope.getCurrentFixedIncome = function () {
        $http.get('/Home/GetMyCurrentFixedIncome').then(function (response) {
            $scope.ledger = response.data.FixedIncomeArray;
            if ($scope.ledger.length > 0) {
                $scope. calculatedAmt = 0;
                angular.forEach($scope.ledger, function (value, index) {
                    $scope.calculatedAmt = $scope.calculatedAmt + value.Amount;
                    value.Total = $scope.calculatedAmt;
                })
            }
        })
    }
    $scope.getCurrentFixedIncome();

    $scope.getCurrentBinaryIncome = function () {
        $http.get('/Home/GetMyCurrentBinaryIncome').then(function (response) {
            $scope.binaryledger = response.data.BIincomeArray;
            $scope.ledgertotals = response.data.TotalsArray;
        })
    }
    $scope.getCurrentBinaryIncome();

    $scope.getCurrentSponsorIncome = function () {
        $http.get('/Home/GetMyCurrentSponsorIncome').then(function (response) {
            debugger;
            $scope.sponsorledger = response.data.SpIincomeArray;
            $scope.sponsorTotalIncome = 0;
            angular.forEach($scope.sponsorledger,function(value,index){
                $scope.sponsorTotalIncome = $scope.sponsorTotalIncome + value.WalletAmount;
            })
        })
    }
    $scope.getCurrentSponsorIncome();

    $scope.user = "";
    $scope.getCurrentGenerationIncome = function () {
        $http.get('/Home/GetMyCurrentGenerationIncome?member=' + $scope.user).then(function (response) {
            $scope.generationledger = response.data.GnIincomeArray;
            $scope.generationTotalIncome = 0;
            angular.forEach($scope.generationledger, function (value, index) {
                $scope.generationTotalIncome = $scope.generationTotalIncome + +value.WalletAmount;
                if (value.Total > 0) {
                    $scope.modifiedGenerationLedger.push(value);
                }
            })
        })
    }
    $scope.getCurrentGenerationIncome();
})