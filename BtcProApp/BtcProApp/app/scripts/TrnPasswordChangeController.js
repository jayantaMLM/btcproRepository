var module = angular.module('app', []);

module.controller('TrnPasswordChange', function ($scope, $http) {
    $scope.isExists = true;
    $scope.oldpassword = "";
    $scope.newpassword = "";
    $scope.confirmpassword = "";
    $scope.oldpwdErrMsg = "";
    $scope.newpwdErrMsg = "";
    $scope.confirmpwdErrMsg = "";
    $scope.trxpassword = "";

    $http.get("/Home/TransactionPasswordExists").then(function (response) {
        $scope.isExists = response.data.isEXISTS;
    });

    $scope.checkOldPwd = function () {
        $http.get("/Home/TransactionPasswordMatch?OldPassword=" + $scope.oldpassword).then(function (response) {
            $scope.isOK = response.data.isOK;
            if (!$scope.isOK) { $scope.oldpwdErrMsg = "Old Transaction password is incorrect."; }
            else { $scope.oldpwdErrMsg = "";}
        });
    }

    $scope.checkNewPwd = function () {
        if ($scope.oldpassword == $scope.newpassword) { $scope.newpwdErrMsg = "Old and New Transaction password cannot be same."; }
        else { $scope.newpwdErrMsg = ""; }
    }
    
    $scope.checkConfirmPwd = function () {
        if ($scope.newpassword != $scope.confirmpassword) { $scope.confirmpwdErrMsg = "New password and it's confirmation password must be same."; }
        else { $scope.confirmpwdErrMsg = ""; }
    }

    $scope.submit = function () {
        if ($scope.oldpwdErrMsg == "" && $scope.newpwdErrMsg == "" && $scope.confirmpwdErrMsg=="") {
            $http.post("/Home/UpdateTransactionPassword?TxPassword=" + $scope.newpassword).then(function (response) {
                if (response.data.Success) {
                   alert("Transaction Password updated successfully.")
                } else {
                    alert("Update failed!!! Reset again");
                }
            })
        }
    }

    $scope.updateTrnPwd = function (pwd) {
        debugger;
        if (pwd == null || pwd == '') {
            alert("Transaction password cannot be blank");
        } else {
            $http.post("/Home/UpdateTransactionPassword?TxPassword=" + pwd).then(function (response) {
                if (response.data.Success) {
                    $scope.trnpasswordexists = true;
                    $scope.isExists = true;
                    $scope.oldpassword = "";
                    $scope.newpassword = "";
                    $scope.confirmpassword = "";
                } else {
                    alert("Update failed!!! Reset again");
                }

            })
        }
    }
})