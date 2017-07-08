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
module.controller('AdminTransfer', function ($scope, $http) {
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
        $scope.totalDeposit = 0;
        $scope.totalWithdraw = 0;
        angular.forEach(arr, function (value, index) {
            $scope.totalDeposit = $scope.totalDeposit + value.Deposit;
            $scope.totalWithdraw = $scope.totalWithdraw + value.Withdraw;
        })
    }
    $scope.filterChanged = function (arr) {
        $scope.Arr = arr;
        if ($scope.searchText == '') { $scope.Arr = $scope.transfers; }
        $scope.recordCount = $scope.Arr.length;
        $scope.changeBoundary();
        $scope.calculateTotals($scope.Arr);
    }
    //

    $scope.amount = 0;
    $scope.username = "";
    $scope.dr_or_cr = "D";
    $scope.wallet = '2';
    $scope.errormessage = "";
    $scope.name = "";
    $scope.email = "";
    $scope.countrycode = "";
    $scope.idfound = false;
    $scope.comment = "";

    //getdata
    $http.get("/Home/AdminTransferLedger?Username=").then(function (response) {
        $scope.transfers = response.data.Ledger;
        $scope.recordCount = $scope.transfers.length;
        $scope.changeBoundary();
        $scope.calculateTotals($scope.transfers);
    })

    //ledger posting
    $scope.Transaction = function () {
        var ans = confirm("Are you sure?");
        if (ans) {
            $http.post("LedgerPosting?Username=" + $scope.username + "&DrCr=" + $scope.dr_or_cr + "&WalletType=" + $scope.wallet + "&Amount=" + $scope.amount + "&Comment=" + $scope.comment).then(function (response) {
                $http.get("/Home/AdminTransferLedger?Username=" + $scope.username).then(function (response) {
                    $scope.transfers = response.data.Ledger;
                    $http.post("/Home/SendPostingUpdateemail?Username=" + $scope.username + "&DrCr=" + $scope.dr_or_cr + "&WalletType=" + $scope.wallet + "&Amount=" + $scope.amount + "&Comment=" + $scope.comment).then(function () {
                        $scope.username = "";
                        $scope.amount = 0;
                        $scope.idfound = false;
                        $scope.name = "";
                        $scope.email = "";
                        $scope.countrycode = "";
                        $scope.comment = "";
                    })
                })
            })
        }
    }

    //check withdraw/deposit amount legibility
    $scope.isNotExcessWithdraw = function () {
        if ($scope.dr_or_cr == "D") {
            return false;
        }
        if ($scope.dr_or_cr == "W") {
            if ($scope.wallet == 1) {
                if ($scope.CashwalletBalance >= $scope.amount) { return false; } else { return true; }
            }
            if ($scope.wallet == 2) {
                if ($scope.ReservewalletBalance >= $scope.amount) { return false; } else { return true; }
            }
            if ($scope.wallet == 3) {
                if ($scope.InvestmentReturnwalletBalance >= $scope.amount) { return false; } else { return true; }
            }
            if ($scope.wallet == 3) {
                if ($scope.FrozenwalletBalance >= $scope.amount) { return false; } else { return true; }
            }
        }
    }

    //verify user Id
    $scope.checkUsername = function () {
        $http.get("/Home/IsUserNameExist1?UserName=" + $scope.username).then(function (data) {
            $scope.isUsernameFound1 = data.data.Found;
            if (!$scope.isUsernameFound1) {
                $scope.username = "";
                $scope.errormessage = "Username not found.";
                $scope.idfound = false;
                $scope.name = "";
                $scope.email = "";
                $scope.countrycode = "";
                $scope.transfers = [];
            } else {
                $scope.errormessage = "";
                $scope.idfound = true;
                $scope.name = data.data.Name;
                $scope.email = data.data.Email;
                $scope.countrycode = data.data.Countrycode;
                $http.get("/Home/AdminTransferLedger?Username=" + $scope.username).then(function (response) {
                    $scope.transfers = response.data.Ledger;
                })
                $http.get("/Home/UserWalletBalance?username=" + $scope.username).then(function (data) {
                    debugger;
                    $scope.CashwalletBalance = data.data.Cash_Balance;
                    $scope.ReservewalletBalance = data.data.Reserve_Balance;
                    $scope.InvestmentReturnwalletBalance = data.data.Invreturn_Balance;
                    $scope.FrozenwalletBalance = data.data.Frozen_Balance;
                })
            }
        })
    }
})