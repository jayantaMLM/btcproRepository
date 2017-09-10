var module = angular.module('app', ['ui.bootstrap']);

module.filter('myLimitTo', function () {
    return function (input, limit, begin) {
        if (!input) {
            return null;
        } else {
            return input.slice(begin, begin + limit);
        }
    };
});

module.controller('Payout', function ($scope, $http, $window, $uibModal) {
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

    $scope.requests = [];
    $scope.selectAll = false;
    $scope.totalRequested = 0;
    $scope.totalPaid = 0;
    $scope.totalCharge = 0;
    $scope.w_totalRequested = 0;
    $scope.w_totalPaid = 0;
    $scope.w_totalCharge = 0;
    $scope.searchText = "";
    $scope.oldComment = "";
    $scope.fromDate = new Date();
    $scope.toDate = new Date();

    $scope.getdata = function () {
        $http.get("/api/WithdrawalRequests").then(function (response) {
            debugger;
            $scope.requests = response.data;
            $scope.calculateTotals_W($scope.requests);
        })
        $http.get("/api/WithdrawalRequests?mode=").then(function (response) {
            $scope.requests_history = response.data;
            $scope.recordCount = $scope.requests_history.length;
            $scope.changeBoundary();
            $scope.calculateTotals($scope.requests_history);
        })
    }

    $scope.calculateTotals_W = function (arr) {
        $scope.w_totalRequested = 0;
        $scope.w_totalPaid = 0;
        $scope.w_totalCharge = 0;
        angular.forEach(arr, function (value, index) {
            $scope.w_totalRequested = $scope.w_totalRequested + value.Amount;
            $scope.w_totalPaid = $scope.w_totalPaid + value.PaidOutAmount;
            $scope.w_totalCharge = $scope.w_totalCharge + value.ServiceCharge;
        })
    }

    $scope.calculateTotals = function (arr) {
        $scope.totalRequested = 0;
        $scope.totalPaid = 0;
        $scope.totalCharge = 0;
        angular.forEach(arr, function (value, index) {
            $scope.totalRequested = $scope.totalRequested + value.Amount;
            $scope.totalPaid = $scope.totalPaid + value.PaidOutAmount;
            $scope.totalCharge = $scope.totalCharge + value.ServiceCharge;
        })
    }

    $scope.filterChanged = function (arr) {
        $scope.Arr = arr;
        if ($scope.searchText == '') { $scope.Arr = $scope.requests_history; }
        $scope.recordCount = $scope.Arr.length;
        $scope.changeBoundary();
        $scope.calculateTotals($scope.Arr);
    }

    $scope.getdata();

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

    //---------------------------------------Modal dialog-------------------------------------------------//
    $scope.modalPay = function (RegistrationId, Id, Amount) {
        var modalInstance = $uibModal.open({
            templateUrl: 'Paycomment.html',
            controller: 'ModalInstanceCtrl',
            backdrop: 'static',
            scope: $scope,
            resolve: {
                objParameters: function () {
                    return {
                        RegId: RegistrationId,
                        UniqId: Id,
                        Amt: Amount,
                        Remarks: ""
                    };
                }
            }
        });
    }
    $scope.modalCancel = function (RegistrationId, Id, Amount) {
        var modalInstance = $uibModal.open({
            templateUrl: 'Cancelcomment.html',
            controller: 'ModalInstanceCtrl',
            backdrop: 'static',
            scope: $scope,
            resolve: {
                objParameters: function () {
                    return {
                        RegId: RegistrationId,
                        UniqId: Id,
                        Amt: Amount,
                        Remarks: ""
                    };
                }

            }
        });
    }
    $scope.modalupdateComment = function (RegistrationId, Id, Amount, Comment) {
        var modalInstance = $uibModal.open({
            templateUrl: 'Updatecomment.html',
            controller: 'ModalInstanceCtrl',
            backdrop: 'static',
            scope: $scope,
            resolve: {
                objParameters: function () {
                    return {
                        RegId: RegistrationId,
                        UniqId: Id,
                        Amt: Amount,
                        Remarks: Comment
                    };
                }
            }
        });
    }

    //---------------------------------------------------------------------------------------------------//

})

module.controller('ModalInstanceCtrl', function ($scope, $controller, $uibModalInstance, $http, $q, objParameters) {
    $scope.RegId = objParameters.RegId;
    $scope.Id = objParameters.UniqId;
    $scope.Amount = objParameters.Amt;
    $scope.Comment = "";
    $scope.headerText = "Comment";
    $scope.OldComment = objParameters.Remarks;

    //cancel button click
    $scope.cancel = function () {
        $uibModalInstance.close();
    }

    //close button click
    $scope.close = function () {
        $uibModalInstance.close();
    }

    //save Pay approval
    $scope.paySave = function () {
        $http.get("/Home/UpdatePaymentRequest?id=" + $scope.Id + "&comment=" + $scope.Comment).then(function (data) {
            $scope.getdata();
            $uibModalInstance.close();
            $http.post("/Home/SendMyWithdrawalConfirmationemail?RegistrationId=" + $scope.RegId + "&amount=" + $scope.Amount + "&status="+$scope.Comment).then(function () {

            })
        })
    }

    //cancel Pay
    $scope.abortSave = function () {
        $http.get("/Home/CancelOrder?Id=" + $scope.Id + "&remarks=Cancelled by Admin" + "&comment=" + $scope.Comment).then(function (response) {
            if (response.data.Success) {
                $scope.getdata();
                $uibModalInstance.close();
            } else {
                alert("Request cancellation failure!!!")
            }
            $http.post("/Home/SendMyWithdrawalCancellationemail?RegistrationId=" + $scope.RegId + "&amount=" + $scope.Amount + "&status="+$scope.Comment).then(function () {

            })
        })
    }

    //update Comment
    $scope.updateCommentSave = function () {
        $http.get("/Home/UpdatePaymentRequestComment?id=" + $scope.Id + "&comment=" + $scope.OldComment).then(function (data) {
            $scope.getdata();
            $uibModalInstance.close();
            //$http.post("/Home/SendMyWithdrawalCommentUpdateemail?RegistrationId=" + $scope.RegId + "&amount=" + $scope.Amount + "&status=" + $scope.Comment).then(function () {

            //})
        })
    }
});
