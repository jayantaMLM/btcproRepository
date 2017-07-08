var module = angular.module('app', []);
module.filter('myLimitTo', function () {
    return function (input, limit, begin) {
        if (!input) {
            return null;
        } else {
            return input.slice(begin, begin + limit);
        }
    };
});
module.controller('FixedIncomeWallet', function ($scope, $http) {
    // Pagination control related
    $scope.recordCount = 0;
    $scope.currentPage = 1;
    $scope.startindex = 0;
    $scope.pageBoundary = 1;
    $scope.pageChanged = function (pageno) {
        $scope.startindex = (pageno * $scope.maxSize) - $scope.maxSize;
    };
    $scope.maxSize = 5;
    $scope.bigTotalItems = 10000;
    $scope.bigCurrentPage = 1;

    $scope.$watch('maxSize', function () {
        $scope.currentPage = 1;
        var pagecount = ($scope.recordCount / $scope.maxSize);
        var decimalpart = pagecount % 1;
        $scope.pageBoundary = Math.floor(pagecount);  //integer part from pagecount
        if (decimalpart > 0) {
            $scope.pageBoundary = $scope.pageBoundary + 1;
        }
        if ($scope.pageBoundary == 0) { $scope.pageBoundary = $scope.pageBoundary + 1; }
        $scope.startindex = ($scope.currentPage * $scope.maxSize) - $scope.maxSize;
    })

    $scope.changeBoundary = function () {
        $scope.currentPage = 1;
        var pagecount = ($scope.recordCount / $scope.maxSize);
        var decimalpart = pagecount % 1;
        $scope.pageBoundary = Math.floor(pagecount);  //integer part from pagecount
        if (decimalpart > 0) {
            $scope.pageBoundary = $scope.pageBoundary + 1;
        }
        if ($scope.pageBoundary == 0) { $scope.pageBoundary = $scope.pageBoundary + 1; }
        $scope.startindex = ($scope.currentPage * $scope.maxSize) - $scope.maxSize;
    }
    //
    $scope.calculateTotals = function (arr) {
        debugger;
        $scope.totalDeposit = 0;
        $scope.totalWithdraw = 0;
        angular.forEach(arr, function (value, index) {
            $scope.totalDeposit = $scope.totalDeposit + value.Deposit;
            $scope.totalWithdraw = $scope.totalWithdraw + value.Withdraw;
        })
    }
    $scope.filterChanged = function (arr) {
        debugger;
        if (arr) {
            $scope.Arr = arr;
            if ($scope.searchText == '') { $scope.Arr = $scope.FixedWalletledger; }
            $scope.recordCount = $scope.Arr.length;
            $scope.changeBoundary();
            $scope.calculateTotals($scope.Arr);
        }
    }
    //

    $scope.getCurrentFixedIncome = function () {
        $scope.totalFixedWalletBalance = 0;

        $http.get('/Home/MyFixedIncomeWallet').then(function (response) {
            $scope.FixedWalletledger = response.data.FixedWallet;

            if ($scope.FixedWalletledger.length > 0) {
                angular.forEach($scope.FixedWalletledger, function (value, index) {
                    value.Balance = $scope.totalFixedWalletBalance + value.Deposit - value.Withdraw;
                    $scope.totalFixedWalletBalance = value.Balance;
                })
            }
            $scope.recordCount = $scope.FixedWalletledger.length;
            $scope.changeBoundary();
            $scope.calculateTotals($scope.FixedWalletledger);
        })
    }
    $scope.getCurrentFixedIncome();
})