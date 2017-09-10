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
        $scope.totalDeposit_ETH = 0;
        $scope.totalWithdraw_ETH = 0;
        angular.forEach(arr, function (value, index) {
            if (value.Currency=='USD'){
                $scope.totalDeposit = $scope.totalDeposit + value.Deposit;
                $scope.totalWithdraw = $scope.totalWithdraw + value.Withdraw;
            }
            if (value.Currency == 'ETH') {
                $scope.totalDeposit_ETH = $scope.totalDeposit_ETH + value.Deposit;
                $scope.totalWithdraw_ETH = $scope.totalWithdraw_ETH + value.Withdraw;
            }
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
    $scope.currency = 'USD';

    //getdata
    $http.get("/Home/AdminTransferLedger?Username=").then(function (response) {
        $scope.transfers = response.data.Ledger;
        $scope.recordCount = $scope.transfers.length;
        $scope.changeBoundary();
        $scope.calculateTotals($scope.transfers);
    })

    $scope.fetch = function () {
        $http.get("/Home/WithdrawalRequestDetailsInDateRange?FromDate=" + $scope.fromDate.toISOString() + "&ToDate=" + $scope.toDate.toISOString()).then(function (response) {
            $scope.daterangeReportData = response.data.data;
            $scope.requestedsum = 0;
            $scope.paidsum = 0;
            $scope.balanceamt = 0;
            angular.forEach($scope.daterangeReportData, function (value, index) {
                $scope.requestedsum = $scope.requestedsum + value.requestedamount;
                $scope.paidsum = $scope.paidsum + value.paidamount;
                $scope.balanceamt = $scope.balanceamt + value.balance;
            })
        })
    }

    //ledger posting
    $scope.Transaction = function () {
        var ans = confirm("Are you sure?");
        if (ans) {
            debugger;
            if ($scope.wallet == 1 || $scope.wallet == 2 || $scope.wallet == 3) {
                $scope.currency = 'USD';
            }
            if ($scope.wallet==4 || $scope.wallet==5 || $scope.wallet==6) {
                $scope.currency = 'ETH';
            }
            $http.post("LedgerPosting?Username=" + $scope.username + "&DrCr=" + $scope.dr_or_cr + "&WalletType=" + $scope.wallet + "&Amount=" + $scope.amount + "&Comment=" + $scope.comment + "&Currency="+ $scope.currency).then(function (response) {
                $http.get("/Home/AdminTransferLedger?Username=" + $scope.username).then(function (response) {
                    $scope.transfers = response.data.Ledger;
                    $scope.filterChanged($scope.transfers);
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
            if ($scope.wallet == 4) {
                if ($scope.EthereumwalletBalance >= $scope.amount) { return false; } else { return true; }
            }
            if ($scope.wallet == 5) {
                if ($scope.CashEthereumwalletBalance >= $scope.amount) { return false; } else { return true; }
            }
        }
    }

    $scope.fetch = function () {
        $http.get("/Home/BalanceTransferDetailsInDateRange?FromDate=" + $scope.fromDate.toISOString() + "&ToDate=" + $scope.toDate.toISOString()).then(function (response) {
            $scope.daterangeReportData = response.data.data;
            $scope.usdtotaltransferredT = 0;
            $scope.usdtotalrealisedT = 0;
            $scope.ethereumtotaltransferredT = 0;
            $scope.ethereumtotalrealizedT = 0;
            angular.forEach($scope.daterangeReportData, function (value, index) {
                $scope.usdtotaltransferredT = $scope.usdtotaltransferredT + value.usdtotaltransferred;
                $scope.usdtotalrealisedT = $scope.usdtotalrealisedT + value.usdtotalrealised;
                $scope.ethereumtotaltransferredT = $scope.ethereumtotaltransferredT + value.ethereumtotaltransferred;
                $scope.ethereumtotalrealizedT = $scope.ethereumtotalrealizedT + value.ethereumtotalrealized;
            })
        })
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
                    $scope.EthereumwalletBalance = data.data.Ethereum_Balance;
                    $scope.CashEthereumwalletBalance = data.data.CashEthereum_Balance;
                })
            }
        })
    }


})