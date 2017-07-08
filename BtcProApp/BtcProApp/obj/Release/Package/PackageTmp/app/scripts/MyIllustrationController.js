var module = angular.module('app', []);

module.controller('Illustration', function ($scope, $http) {
   
    $scope.getIllustration = function () {
        $http.get('GetFixedIncomeIllustration').then(function (response) {
            $scope.ledger = response.data.FixedIncomeArray;
            if ($scope.ledger.length > 0) {
                var calculatedAmt = 0;
                angular.forEach($scope.ledger, function (value, index) {
                    calculatedAmt = calculatedAmt + value.Amount;
                    value.Total = calculatedAmt;
                })
            }
        })
    }
    $scope.getIllustration();
})