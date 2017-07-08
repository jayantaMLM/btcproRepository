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
module.controller('MemberWithdraw', function ($scope, $http, $location) {
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
    //transaction password related code--------------------------------------------------------------------------------------


    $scope.trnpasswordIsOk = false;
    $scope.trnpasswordexists = false;

    $http.get("/Home/IsUserMember").then(function (response) {
        $scope.isExists = response.data.Found;
        $scope.trnpasswordexists = response.data.TrnPasswordExists;
    })

    $scope.updateTrnPwd = function () {
        if ($scope.trxpassword == null || $scope.trxpassword == '') {
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

    //-------------------------------------------------------------------------transaction password related code end

    $scope.calculateTotals = function (arr) {
        debugger;
        $scope.total_Amount = 0;
        $scope.total_Payable = 0;
        $scope.total_AdministrativeChg = 0;

        angular.forEach(arr, function (value, index) {
            $scope.total_Amount = $scope.total_Amount + value.Amount;
            $scope.total_Payable = $scope.total_Payable + value.Payable;
            $scope.total_AdministrativeChg = $scope.total_AdministrativeChg + value.AdministrativeChg;
        })
    }
    $scope.filterChanged = function (arr) {
        debugger;
        if (arr) {
            $scope.Arr = arr;
            if ($scope.searchText == '') { $scope.Arr = $scope.transfers; }
            $scope.recordCount = $scope.Arr.length;
            $scope.changeBoundary();
            $scope.calculateTotals($scope.Arr);
        }
    }
    //
    $scope.amount = 0;
    $scope.cashwalletBalance = 0;
    $scope.fixedwalletBalance = 0;
    $scope.wallet = "1";
    $scope.errormessage = "";
    $scope.totalrequested = 0;
    $scope.totalpayable = 0;
    $scope.adminchange = 0;

    $http.get("/Home/IsUserMember").then(function (response) {
        $scope.isExists = response.data.Found;
    })
    $http.get("/Home/IsMemberACNOpresent").then(function (response) {
        $scope.isExistsAcNo = response.data.Found;
    })

    $scope.getbalance = function () {
        $http.get("/Home/MyWalletBalance?WalletId=1").then(function (data) {
            $scope.cashwalletBalance = data.data.Balance;
        })
        $http.get("/Home/MyWalletBalance?WalletId=3").then(function (data) {
            $scope.fixedwalletBalance = data.data.Balance;
        })
    }

    $scope.amountisValid = function () {
        debugger;
        if ($scope.wallet == "1") {
            if ($scope.amount > $scope.cashwalletBalance) {
                return true;
            } else {
                return false;
            }
        }
        if ($scope.wallet == "3") {
            if ($scope.amount > $scope.fixedwalletBalance) {
                return true;
            } else {
                return false;
            }
        }
    }

    $scope.Transaction = function () {
        var ans = confirm("Are you sure?");
        if (ans) {
            $http.post("/Home/WithdrawPostingMember?WalletType=" + $scope.wallet + "&Amount=" + $scope.amount).then(function (response) {
                debugger;
                if (response.data.Success) {
                    $scope.getbalance();
                    $scope.GetHistory();
                    $http.post("/Home/SendMyWithdrawalRequestemail?Username=&amount=" + $scope.amount + "&status=").then(function () {
                        $scope.amount = 0;
                    })
                }
            })
        }
    }

    $scope.getbalance();

    $scope.GetHistory = function () {
        $scope.totalrequested = 0;
        $scope.totalpayable = 0;
        $scope.adminchange = 0;
        $http.get("/Home/MemberWithdrawalHistory").then(function (response) {
            $scope.transfers = response.data.Transfers;
            $scope.recordCount = $scope.transfers.length;
            $scope.changeBoundary();
            $scope.calculateTotals($scope.transfers);
        })
    }

    $scope.GetHistory();
   
    $scope.cancelOrder = function (Id) {
        var ans = confirm("Sure you want to cancel it?");
        if (ans) {
            $http.get("/Home/CancelOrder?Id=" + Id + "&remarks=Cancelled by Member"+"&comment=").then(function (response) {
                if (response.data.Success) {
                    $scope.getbalance();
                    $scope.GetHistory();
                    alert("Request successfully cancelled.");
                } else {
                    alert("Request cancellation failure!!!")
                }
            })
        }
    }

    $scope.TakeMeToAccountPage = function () {
        var path = $location.path("/Home/AccountStatus");
        var abspath = path.$$absUrl;
        var modifiedpath = abspath.replace("/Home/WithdrawalRequests#!", "");
        window.location = modifiedpath;
    }
})