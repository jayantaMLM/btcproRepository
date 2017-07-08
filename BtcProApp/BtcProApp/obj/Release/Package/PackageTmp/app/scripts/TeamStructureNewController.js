var module = angular.module('app', ['ngMaterial', 'ngMessages']);

module.controller('TeamTreeNew', function ($scope, $http) {
    $scope.showTree = true;
    $scope.selectedOption = "P";
    $scope.payamount = 99;
    $scope.errmsg = "";
    $scope.minPay = 20;
    $scope.maxPay = 99;
    $scope.step1 = false;
    $scope.step2 = false;

    $scope.isValid = true;
    $scope.isRegistered = false;
    $scope.isComplete = false;
    $scope.isUsernameFound = false;
    $scope.isUsernameFound1 = false;
    $scope.isEmailIdFound = false;
    $scope.register = false;
    $scope.referrerId = 0;
    $scope.showSubmit = true;
    $scope.showEmail = false;
    $scope.binarypos = "";
    $scope.currentusername = "";
    $scope.uplineId = 0;
    $scope.packageId = 1;

    $scope.registerModel = {
        ReferrerName: "",
        FullName: "",
        EmailId: "",
        UserName: "",
        Password: "",
        ConfirmPassword: "",
        CreatedDate: new Date,
        ReferrerId: 0,
        BinaryPosition: "",
        CountryCode: ""
    }
    $scope.errorModel = {
        ReferrerName: "",
        FullName: "",
        EmailId: "",
        UserName: "",
        Password: "",
        ConfirmPassword: ""
    }

    $scope.getbalance = function () {
        debugger;
        $http.get("/Home/MyWalletBalance?WalletId=2").then(function (data) {
            $scope.walletBalance = data.data.Balance;
        })
        $http.get("/Home/CurrentUserName").then(function (data) {
            $scope.currentusername = data.data.CurrentUser;
            $scope.referrerId = data.data.UserId;
        })
    }

    $scope.getbalance();

    $scope.payamount_validate = function () {
        debugger;
        if ($scope.payamount < $scope.minPay || $scope.payamount > $scope.maxPay) {
            $scope.errmsg = "Amount must be between " + $scope.minPay + "$ - " + $scope.maxPay + "$";
        }
        else {
            $scope.errmsg = "";
        }
    }

    $scope.payamount_click = function () {
        if ($scope.selectedOption == "P") {
            $scope.minPay = 20;
            $scope.maxPay = 99;
            $scope.payamount = $scope.maxPay;
            $scope.packageId = 1;
        } else if ($scope.selectedOption == "J") {
            $scope.minPay = 100;
            $scope.maxPay = 2999;
            $scope.payamount = $scope.maxPay;
            $scope.packageId = 2;
        } else if ($scope.selectedOption == "E") {
            $scope.minPay = 3000;
            $scope.maxPay = 5000;
            $scope.payamount = $scope.maxPay;
            $scope.packageId = 3;
        } else if ($scope.selectedOption == "M") {
            $scope.minPay = 10000;
            $scope.maxPay = 10000;
            $scope.payamount = $scope.maxPay;
            $scope.packageId = 4;
        } else {
            $scope.minPay = 0;
            $scope.maxPay = 0;
            $scope.payamount = $scope.maxPay;
        }
    }

    $scope.CheckBalance = function () {
        if ($scope.walletBalance < $scope.payamount) {
            $scope.errmsg1 = " Insufficient wallet balance to pay.";
        } else {
            $scope.errmsg1 = "";
        }
    }

    $scope.CHECKReferrername = function () {
        debugger;
        $scope.isValid = false;
        if ($scope.ReferrerName != "") {
            $http.get("/Home/IsUserNameExist?UserName=" + $scope.registerModel.ReferrerName).then(function (data) {
                $scope.isUsernameFound1 = data.data.Found;
                if (!$scope.isUsernameFound1) {
                    $scope.errorModel.ReferrerName = "Incorrect Sponsor Username.";
                    $scope.isValid = false;
                    //this.Registration.ReferrerName.$setValidity("errReferrerField", false);
                } else {
                    $scope.errorModel.ReferrerName = "";
                    $scope.isValid = true;
                    $scope.registerModel.ReferrerId = data.data.ReferrerId;
                    $scope.registerModel.BinaryPosition = data.data.BinaryPos;
                }
            })
        }
    }

    $scope.VALIDATION1 = function () {
        debugger;
        $scope.errorModel.ReferrerName = "";
        if ($scope.ReferrerName == "") {
            $scope.errorModel.ReferrerName = "Cannot be left blank.";
            $scope.isValid = false;
            this.Registration.ReferrerName.$setValidity("errReferrerField", false);
        } else {
            this.Registration.ReferrerName.$setValidity("errReferrerField", true);
        }
    }

    $scope.validation2 = function () {
        $scope.errorModel.FullName = "";
        if ($scope.registerModel.FullName == "") {
            $scope.errorModel.FullName = "Cannot be left blank.";
            $scope.isValid = false;
            this.Registration.Fullname.$setValidity("errFullnameField", false);
        } else {
            this.Registration.Fullname.$setValidity("errFullnameField", true);
        }
        if (this.Registration.Fullname.$error.pattern) {
            $scope.errorModel.FullName = "Name can have only Alphabets and Spaces";
            $scope.isValid = false;
        }
    }

    $scope.validation3 = function () {
        $scope.errorModel.EmailId = "";
        if ($scope.registerModel.EmailId == "") {
            $scope.errorModel.EmailId = "Cannot be left blank.";
            $scope.isValid = false;
            this.Registration.Email.$setValidity("errEmailField", false);
        } else {
            this.Registration.Email.$setValidity("errEmailField", true);
        }
        if (this.Registration.Email.$error.email) {
            $scope.errorModel.EmailId = "Enter valid email id";
            $scope.isValid = false;
        }
    }

    $scope.validation4 = function () {
        $scope.errorModel.UserName = "";
        if ($scope.registerModel.UserName == "") {
            $scope.errorModel.UserName = "Cannot be left blank.";
            $scope.isValid = false;
            this.Registration.Username.$setValidity("errUsernameField", false);
        } else {
            this.Registration.Username.$setValidity("errUsernameField", true);
        }
        if (this.Registration.Username.$error.minlength) {
            $scope.errorModel.UserName = "Username should be minimum 6 characters";
            $scope.isValid = false;
        }
        if (this.Registration.Username.$error.maxlength) {
            $scope.errorModel.UserName = "Username cannot be more than 30 characters";
            $scope.isValid = false;
        }
        if (this.Registration.Username.$error.pattern) {
            $scope.errorModel.UserName = "Username can be only alphabets/numbers or alphabets+numbers";
            $scope.isValid = false;
        }
    }

    $scope.validation5 = function () {
        $scope.errorModel.Password = "";
        if ($scope.registerModel.Password == "") {
            $scope.errorModel.Password = "Cannot be left blank.";
            $scope.isValid = false;
            this.Registration.Password.$setValidity("errPasswordField", false);
        } else {
            this.Registration.Password.$setValidity("errPasswordField", true);
        }
        if (this.Registration.Password.$error.minlength) {
            $scope.errorModel.Password = "Password should be minimum 6 characters";
            $scope.isValid = false;
        }
        if (this.Registration.Password.$error.maxlength) {
            $scope.errorModel.Password = "Password cannot be more than 14 characters";
            $scope.isValid = false;
        }
        if (this.Registration.Password.$error.pattern) {
            $scope.errorModel.Password = "Password can be only alphabets/numbers or alphabets+numbers";
            $scope.isValid = false;
        }
    }

    $scope.validation6 = function () {
        $scope.errorModel.ConfirmPassword = "";
        if ($scope.registerModel.Password != $scope.registerModel.ConfirmPassword) {
            $scope.errorModel.ConfirmPassword = "Password and confirm password should match";
            $scope.isValid = false;
        } else {
            $scope.errorModel.ConfirmPassword = "";
        }

        if ($scope.registerModel.ConfirmPassword == "") {
            $scope.errorModel.ConfirmPassword = "Cannot be left blank.";
            $scope.isValid = false;
            this.Registration.Password2.$setValidity("errPassword2Field", false);
        } else {
            this.Registration.Password2.$setValidity("errPassword2Field", true);
        }
    }

    $scope.checkUsername = function () {
        $scope.isValid = false;
        if ($scope.registerModel.UserName != "") {
            $http.get("/Home/IsUserNameExist1?UserName=" + $scope.registerModel.UserName).then(function (data) {
                $scope.isUsernameFound1 = data.data.Found;
                if ($scope.isUsernameFound1) {
                    $scope.errorModel.UserName = "Username already exists.";
                    $scope.isValid = false;
                    //this.Registration.UserName.$setValidity("errUsernameField", false);
                } else {
                    $scope.errorModel.UserName = "";
                    $scope.isValid = true;
                }
            })
        }
    }

    $scope.buy = function () {
        $scope.step1 = false;
        $scope.step2 = true;
    }

    $scope.register = function () {
        debugger;
        $scope.errorModel.FullName = "";
        if ($scope.registerModel.FullName == "") {
            $scope.errorModel.FullName = "Cannot be left blank.";
            $scope.isValid = false;
            this.Registration.Fullname.$setValidity("errFullnameField", false);
        } else {
            this.Registration.Fullname.$setValidity("errFullnameField", true);
        }
        if (this.Registration.Fullname.$error.pattern) {
            $scope.errorModel.FullName = "Name can have only Alphabets and Spaces";
            $scope.isValid = false;
        }
        $scope.errorModel.EmailId = "";
        if ($scope.registerModel.EmailId == "") {
            $scope.errorModel.EmailId = "Cannot be left blank.";
            $scope.isValid = false;
            this.Registration.Email.$setValidity("errEmailField", false);
        } else {
            this.Registration.Email.$setValidity("errEmailField", true);
        }
        if (this.Registration.Email.$error.email) {
            $scope.errorModel.EmailId = "Enter valid email id";
            $scope.isValid = false;
        }
        $scope.errorModel.UserName = "";
        if ($scope.registerModel.UserName == "") {
            $scope.errorModel.UserName = "Cannot be left blank.";
            $scope.isValid = false;
            this.Registration.Username.$setValidity("errUsernameField", false);
        } else {
            this.Registration.Username.$setValidity("errUsernameField", true);
        }
        if (this.Registration.Username.$error.minlength) {
            $scope.errorModel.UserName = "Username should be minimum 6 characters";
            $scope.isValid = false;
        }
        if (this.Registration.Username.$error.maxlength) {
            $scope.errorModel.UserName = "Username cannot be more than 30 characters";
            $scope.isValid = false;
        }
        if (this.Registration.Username.$error.pattern) {
            $scope.errorModel.UserName = "Username can be only alphabets/numbers or alphabets+numbers";
            $scope.isValid = false;
        }
        debugger;
        $scope.errorModel.Password = "";
        if ($scope.registerModel.Password == "") {
            $scope.errorModel.Password = "Cannot be left blank.";
            $scope.isValid = false;
            this.Registration.Password.$setValidity("errPasswordField", false);
        } else {
            this.Registration.Password.$setValidity("errPasswordField", true);
        }
        if (this.Registration.Password.$error.minlength) {
            $scope.errorModel.Password = "Password should be minimum 6 characters";
            $scope.isValid = false;
        }
        if (this.Registration.Password.$error.maxlength) {
            $scope.errorModel.Password = "Password cannot be more than 14 characters";
            $scope.isValid = false;
        }
        if (this.Registration.Password.$error.pattern) {
            $scope.errorModel.Password = "Password can be only alphabets/numbers or alphabets+numbers";
            $scope.isValid = false;
        }
        $scope.errorModel.ConfirmPassword = "";
        if ($scope.registerModel.Password != $scope.registerModel.ConfirmPassword) {
            $scope.errorModel.ConfirmPassword = "Password and confirm password should match";
            $scope.isValid = false;
        } else {
            $scope.errorModel.ConfirmPassword = "";
        }

        if ($scope.registerModel.ConfirmPassword == "") {
            $scope.errorModel.ConfirmPassword = "Cannot be left blank.";
            $scope.isValid = false;
            this.Registration.Password2.$setValidity("errPassword2Field", false);
        } else {
            this.Registration.Password2.$setValidity("errPassword2Field", true);
        }

        if ($scope.registerModel.UserName != "") {
            $http.get("/Home/IsUserNameExist1?UserName=" + $scope.registerModel.UserName).then(function (data) {
                $scope.isUsernameFound1 = data.data.Found;
                if ($scope.isUsernameFound1) {
                    $scope.errorModel.UserName = "Username already exists.";
                    $scope.isValid = false;
                } else {
                    $scope.errorModel.UserName = "";

                    if ($scope.errorModel.FullName == '' && $scope.errorModel.EmailId == '' && $scope.errorModel.UserName == '' && $scope.errorModel.Password == '' && $scope.errorModel.ConfirmPassword == '') {
                        $scope.isValid = true;
                    } else {
                        $scope.isValid = false;
                    }

                    if ($scope.isValid) {
                        $scope.isComplete = true;
                        $scope.showProceed = false;
                    } else {
                    }
                }
            })
        }

        $scope.registration = function () {
            debugger;
            if ($scope.currentusername != "superadmin") {
                $scope.registerModel.ReferrerId = $scope.referrerId;
                $scope.registerModel.ReferrerName = $scope.currentusername;
                $scope.registerModel.BinaryPosition = $scope.binarypos;
            }
            
            var data = $scope.registerModel;
            //Add new user to AspNetUer table and send email
            $http.post('/Account/Adduser?emailId=' + $scope.registerModel.EmailId + "&username=" + $scope.registerModel.UserName + "&password=" + $scope.registerModel.Password).then(function (data) {
                if (data.data.flag == 'Success') {
                    //add new entry to registration table
                    $http.post('/api/Registers', $scope.registerModel).then(function (data) {
                        $scope.isRegistered = true;
                        $scope.showEmail = true;
                        $http.post('/Home/SendMyRegistrationEmail?Username=' + $scope.registerModel.UserName).then(function (response) {
                            if (response.data.status == 'Success') {
                                $scope.showEmail = false;
                            }
                        })

                        $http.post("/Home/LedgerPostingMember?Username=" + $scope.registerModel.UserName + "&WalletType=2&Amount=" + $scope.payamount).then(function (response) {
                            if (response.data.Success) {
                            }
                        })
                        $http.post("AutoPurchase?username="+$scope.registerModel.UserName+"&UplineId="+$scope.uplineId+"&packageId=" + $scope.packageId + "&investmentAmt=" + $scope.payamount).then(function (data) {
                            debugger;
                            if (data.data.Success == "TRUE") {
                                $http.get("MyAddress").then(function (retdata) {
                                    $scope.address = retdata.data.Address;
                                    $scope.loader = false;
                                })

                            }
                        })
                    })
                }
            })
        }
    }
})