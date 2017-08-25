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
module.controller('MemberTransfer', function ($scope, $http, $location) {
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
        if (arr) {
            $scope.Arr = arr;
            if ($scope.searchText == '') { $scope.Arr = $scope.transfers;}
            $scope.recordCount = $scope.Arr.length;
            $scope.changeBoundary();
            $scope.calculateTotals($scope.Arr);
        }
    }
    //

    $scope.amount = 0;
    $scope.username = "";
    $scope.wallet = '2';
    $scope.walletBalance = 0;
    $scope.errormessage = "";
    $scope.trnpassword = "";
    $scope.trnpasswordIsOk = false;
    $scope.trnpasswordexists = false;

    $http.get("/Home/IsUserMember").then(function (response) {
        $scope.isExists = response.data.Found;
        $scope.trnpasswordexists = response.data.TrnPasswordExists;
    })

    $scope.getbalance = function () {
        $http.get("/Home/MyWalletBalance?WalletId=2").then(function (data) {
            $scope.walletBalance = data.data.Balance;
        })
    }
    $scope.Transaction = function () {
        var ans=confirm("Are you sure?");
        if (ans) {
            $http.post("/Home/LedgerPostingMember?Username=" + $scope.username + "&WalletType=" + $scope.wallet + "&Amount=" + $scope.amount).then(function (response) {
                if (response.data.Success) {
                    $http.post("/Home/NotifyAdminAboutBalanceTransfer?ToUsername=" + $scope.username + "&Amount=" + $scope.amount);
                    $scope.username = "";
                    $scope.amount = 0;
                    $scope.errormessage = "";
                    $scope.trnpassword = "";
                    $scope.trnpasswordIsOk = false;
                    $scope.getbalance();
                    $scope.GetHistory();
                }
            })
        }
    }

    $scope.getbalance();

    $scope.GetHistory = function () {
        $http.get("/Home/MemberTransferHistory").then(function (response) {
            $scope.transfers = response.data.Transfers;
            if ($scope.transfers.length > 0) {
                var calculatedAmt = 0;
                angular.forEach($scope.transfers, function (value, index) {
                    calculatedAmt = calculatedAmt + value.Deposit - value.Withdraw;
                    value.Amount = calculatedAmt;
                })
                $scope.recordCount = $scope.transfers.length;
                $scope.changeBoundary();
                $scope.calculateTotals($scope.transfers);
            }
        })
    }

    $scope.goToPurchase = function () {
        var path = $location.path("/Home/PackagesShop");
        var abspath = path.$$absUrl;
        var modifiedpath = abspath.replace("/Home/Transfers#!", "");
        window.location = modifiedpath;
    }
    
    $scope.GetHistory();

    $scope.checkUsername = function () {
        $http.get("/Home/IsUserNameExist1?UserName=" + $scope.username).then(function (data) {
            $scope.isUsernameFound1 = data.data.Found;
            if (!$scope.isUsernameFound1) {
                $scope.username = "";
                $scope.errormessage = "Username not found.";

            } else {
                $scope.errormessage = "";
            }
        })
    }

    $scope.updateTrnPwd = function () {
        if ($scope.trxpassword == null || $scope.trxpassword=='') {
            alert("Transaction password cannot be blank");
        } else {
            $http.post("/Home/UpdateTransactionPassword?TxPassword=" + $scope.trxpassword).then(function (response) {
                if (response.data.Success) {
                    $scope.trnpasswordexists = true;
                } else {
                    alert("Update failed!!! Reset again");
                }

            })
        }
    }

    $scope.IsTrnPasswordOK = function () {
        $http.get("/Home/MatchTrnPassword?TxPassword=" + $scope.trnpassword).then(function (response) {
            if (response.data.Success) {
                $scope.trnpasswordIsOk = true;
            } else {
                $scope.trnpasswordIsOk = false;
            }
        })
    }
})