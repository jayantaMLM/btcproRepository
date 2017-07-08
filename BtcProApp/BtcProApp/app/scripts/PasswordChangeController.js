var module = angular.module('app', []);

module.controller('PasswordChange', function ($scope, $http) {
    $scope.oldpassword = "";
    $scope.newpassword = "";
    $scope.confirmpassword = "";
    $scope.oldpwdErrMsg = "";
    $scope.newpwdErrMsg = "";
    $scope.confirmpwdErrMsg = "";

    var ResetPasswordModel = {
        UserName: "",
        Password: "",
        ConfirmPassword: "",
        Code: ""
    }

    $scope.checkOldPwd = function () {
        $http.get("/Home/PasswordMatch?OldPassword=" + $scope.oldpassword).then(function (response) {
            $scope.isOK = response.data.isOK;
            if (!$scope.isOK) { $scope.oldpwdErrMsg = "Old password is incorrect."; }
            else { $scope.oldpwdErrMsg = ""; }
        });
    }

    $scope.checkNewPwd = function () {
        if ($scope.oldpassword == $scope.newpassword) { $scope.newpwdErrMsg = "Old and New password cannot be same."; }
        else { $scope.newpwdErrMsg = ""; }
    }

    $scope.checkConfirmPwd = function () {
        if ($scope.newpassword != $scope.confirmpassword) { $scope.confirmpwdErrMsg = "New password and it's confirmation password must be same."; }
        else { $scope.confirmpwdErrMsg = ""; }
    }

    $scope.submit = function () {
        if ($scope.oldpwdErrMsg == "" && $scope.newpwdErrMsg == "" && $scope.confirmpwdErrMsg == "") {
            $http.get("/Home/RegistrationDetail").then(function (response) {
                $scope.member = response.data.Member;
                $scope.member.Password = $scope.newpassword;

                ResetPasswordModel.UserName = "";
                ResetPasswordModel.Password = $scope.oldpassword;
                ResetPasswordModel.ConfirmPassword = $scope.newpassword;
                ResetPasswordModel.Code = "";
                $http.post("/Account/ResetMyPassword", ResetPasswordModel).then(function (response) {
                    if (response.data.OK) {
                        $http.get("/Home/ResetPasswordInRegistration?OldPassword=" + $scope.newpassword).then(function () {
                            alert("Password saved successfully.");
                        })
                    } else {
                        alert("Password could not be saved.");
                    }
                })
            })
        }
    }
})