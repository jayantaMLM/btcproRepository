var module = angular.module('app', ['ngMaterial', 'ngMessages', 'ui.bootstrap']);
module.controller('Register', function ($scope, $http, $location, $mdDialog, $uibModal) {
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
    $scope.CountryCode = "";
    $scope.ReferrerName = "";

    $scope.SponsorIsCorrect = "";

    $scope.registerModel = {
        ReferrerName: "",
        SponsorName:"",
        FullName: "",
        EmailId: "",
        UserName: "",
        Password: "",
        ConfirmPassword: "",
        CreatedDate: new Date,
        ReferrerId: 0,
        BinaryPosition: "L",
        CountryCode: ""
    }

    $scope.loginModel = {
        UserName: "",
        Password: "",
    }

    $scope.loginErrModel = {
        UserName: "",
        Password: "",
    }

    $scope.errorModel = {
        ReferrerName: "",
        FullName: "",
        EmailId: "",
        UserName: "",
        Password: "",
        ConfirmPassword: ""
    }

    //$scope.getCountryCode = function () {
    //    $http.get("https://ipinfo.io").then(function (response) {
    //        debugger;
    //        $scope.CountryCode = response.data.country;
    //    })
    //}

    $scope.checkReferrername = function () {
        $scope.isValid = false;
        //if ($scope.ReferrerName != "") {
        //    $http.get("/Home/IsUserNameExist?UserName=" + $scope.ReferrerName).then(function (data) {
        //        $scope.isUsernameFound1 = data.data.Found;
        //        if (!$scope.isUsernameFound1) {
        //            $scope.errorModel.ReferrerName = "Incorrect Sponsor Username.";
        //            $scope.isValid = false;
        //            //this.Registration.ReferrerName.$setValidity("errReferrerField", false);
        //        } else {
        //            $scope.errorModel.ReferrerName = "";
        //            $scope.isValid = true;
        //        }
        //    })
        //}
        if ($scope.registerModel.ReferrerName != "") {
            $http.get("/Home/IsUserNameExist?UserName=" + $scope.registerModel.ReferrerName).then(function (data) {
                $scope.isUsernameFound1 = data.data.Found;
                if (!$scope.isUsernameFound1) {
                    $scope.errorModel.ReferrerName = "Incorrect Sponsor Username.";
                    $scope.isValid = false;
                } else {
                    $scope.errorModel.ReferrerName = "";
                    $scope.registerModel.SponsorName = data.data.Sponsorname;
                    $scope.isValid = true;
                }
            })
        }
    }

    $scope.checkEmailId = function () {
        $scope.isValid = false;
        if ($scope.registerModel.EmailId != "") {
            //$http.get("/Home/IsEmailPresent?EmailId=" + $scope.registerModel.EmailId).then(function (data) {
            //    //$scope.isEmailIdFound = data.data.Found;
            //    $scope.isEmailIdFound = false;
            //    if ($scope.isEmailIdFound) {
            //        $scope.errorModel.EmailId = "Email Id already exists.";
            //        $scope.isValid = false;
            //        //this.Registration.EmailId.$setValidity("errEmailField", false);
            //    } else {
            //        $scope.errorModel.EmailId = "";
            //        $scope.isValid = true;
            //    }
            //})
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
                    $scope.SponsorIsCorrect = "N";
                    //this.Registration.UserName.$setValidity("errUsernameField", false);
                } else {
                    $scope.errorModel.UserName = "";
                    $scope.isValid = true;
                    $scope.SponsorIsCorrect = "Y";
                }
            })
        }
    }

    $scope.validations = function () {
        $scope.isValid = true;
        $scope.validation1();
        $scope.validation2();
        $scope.validation3();
        $scope.validation4();
        $scope.validation5();
        $scope.validation6();
        $scope.checkReferrername();
        $scope.checkEmailId();
        $scope.checkUsername();
        $scope.register();
    }

    $scope.register = function () {
        //$scope.validations();
        debugger;
        //$scope.errorModel.ReferrerName = "";
        //if ($scope.ReferrerName == "") {
        //    $scope.errorModel.ReferrerName = "Cannot be left blank.";
        //    $scope.isValid = false;
        //    this.Registration.ReferrerName.$setValidity("errReferrerField", false);
        //} else {
        //    this.Registration.ReferrerName.$setValidity("errReferrerField", true);
        //}
        $scope.errorModel.ReferrerName = "";
        if ($scope.registerModel.ReferrerName == "") {
            $scope.errorModel.ReferrerName = "Cannot be left blank.";
            $scope.SponsorIsCorrect = "N";
            $scope.isValid = false;
            this.Registration.ReferrerName.$setValidity("errReferrerField", false);
        } else {
            this.Registration.ReferrerName.$setValidity("errReferrerField", true);
            $scope.SponsorIsCorrect = "Y";
        }
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
        //if ($scope.ReferrerName != "") {
        //    $http.get("/Home/IsUserNameExist?UserName=" + $scope.ReferrerName).then(function (data) {
        if ($scope.registerModel.ReferrerName != "") {
            $http.get("/Home/IsUserNameExist?UserName=" + $scope.registerModel.ReferrerName).then(function (data) {
                $scope.isUsernameFound1 = data.data.Found;
                $scope.registerModel.ReferrerId = data.data.ReferrerId;
                if (!$scope.isUsernameFound1) {
                    $scope.errorModel.ReferrerName = "Incorrect Sponsor User Id.";
                    $scope.isValid = false;
                } else {
                    $scope.errorModel.ReferrerName = "";
                    $scope.isValid = true;
                    if ($scope.registerModel.EmailId != "") {
                        $http.get("/Home/IsEmailPresent?EmailId=" + $scope.registerModel.EmailId).then(function (data) {
                            //$scope.isEmailIdFound = data.data.Found;
                            $scope.isEmailIdFound = false;
                            if ($scope.isEmailIdFound) {
                                $scope.errorModel.EmailId = "Email Id already exists.";
                                $scope.isValid = false;
                            } else {
                                $scope.errorModel.EmailId = "";
                                $scope.isValid = true;
                                if ($scope.registerModel.UserName != "") {
                                    $http.get("/Home/IsUserNameExist1?UserName=" + $scope.registerModel.UserName).then(function (data) {
                                        $scope.isUsernameFound1 = data.data.Found;
                                        if ($scope.isUsernameFound1) {
                                            $scope.errorModel.UserName = "Username already exists.";
                                            $scope.isValid = false;
                                        } else {
                                            $scope.errorModel.UserName = "";
                                            $scope.isValid = true;
                                            //Get country location
                                            //$http.get("http://api.wipmania.com/json").then(function (response) {
                                            //    debugger;
                                            //    $scope.errorModel.CountryCode = response.data.country_code;
                                            //})
                                            if ($scope.isValid) {
                                                //Add new user to AspNetUer table and send email
                                                //$http.post('/Account/Adduser?emailId=' + $scope.registerModel.EmailId + "&username=" + $scope.registerModel.UserName + "&password=" + $scope.registerModel.Password).then(function (data) {
                                                //    if (data.data.flag == 'Success') {
                                                //        //add new entry to registration table
                                                //        $http.post('/api/Registers', $scope.registerModel).then(function (data) {
                                                //            //if (data.data.id > 0) {
                                                //            debugger;
                                                //                $scope.isRegistered = true;
                                                //            //}
                                                //        })
                                                //    }
                                                //})
                                                $scope.isComplete = true;
                                                $scope.showProceed = false;
                                            } else {

                                            }
                                        }
                                    })
                                }
                            }
                        })
                    }
                }
            })
        }



    }

    $scope.registration = function () {
        $http.get("https://ipinfo.io").then(function (response) {
            $scope.CountryCode = response.data.country;
            $scope.registerModel.CountryCode = $scope.CountryCode;
            //Add new user to AspNetUer table and send email
            $http.post('/Account/Adduser?emailId=' + $scope.registerModel.EmailId + "&username=" + $scope.registerModel.UserName + "&password=" + $scope.registerModel.Password).then(function (data) {
                if (data.data.flag == 'Success') {
                    //add new entry to registration table
                    $scope.registerModel.CountryCode = $scope.CountryCode;
                    //$scope.registerModel.ReferrerName = $scope.ReferrerName;
                    $http.get("/Home/GetWorkingLeg?user=" + $scope.registerModel.ReferrerName).then(function (data) {
                        $scope.registerModel.BinaryPosition = data.data.Position;
                        if (data.data.Position == null) {
                            $scope.registerModel.BinaryPosition = "L";
                        }
                        $http.post('/api/Registers', $scope.registerModel).then(function (data) {
                            $scope.isRegistered = true;
                            $scope.showEmail = true;
                            $http.post('/Home/SendMyRegistrationEmail?Username=' + $scope.registerModel.UserName).then(function (response) {
                                if (response.data.status == 'Success') {
                                    $scope.showEmail = false;
                                }
                            })
                        })

                    })

                }
            })
        })
    }

    $scope.validation1 = function () {
        //$scope.errorModel.ReferrerName = "";
        //if ($scope.ReferrerName == "") {
        //    $scope.errorModel.ReferrerName = "Cannot be left blank.";
        //    $scope.isValid = false;
        //    this.Registration.ReferrerName.$setValidity("errReferrerField", false);
        //} else {
        //    this.Registration.ReferrerName.$setValidity("errReferrerField", true);
        //}
        $scope.errorModel.ReferrerName = "";
        if ($scope.registerModel.ReferrerName == "") {
            $scope.errorModel.ReferrerName = "Cannot be left blank.";
            $scope.isValid = false;
            $scope.SponsorIsCorrect = "N";
            this.Registration.ReferrerName.$setValidity("errEmailField", false);
        } else {
            this.Registration.ReferrerName.$setValidity("errEmailField", true);
            $scope.SponsorIsCorrect = "Y";
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

    $scope.checkLoginUsername = function () {
        $scope.isValid = false;
        if ($scope.loginModel.UserName != "") {
            $http.get("/Home/IsUserNameExist1?UserName=" + $scope.loginModel.UserName).then(function (data) {
                $scope.isUsernameFound2 = data.data.Found;
                if ($scope.isUsernameFound2) {
                    $scope.loginErrModel.UserName = "";
                    $scope.isValid = true;
                } else {
                    $scope.loginErrModel.UserName = "Username does not exist.";
                    $scope.isValid = false;
                }
            })
        }
    }

    $scope.sendForLogin = function () {
        $http.post("/Account/UserLogin?Username=" + $scope.loginModel.UserName + "&Password=" + $scope.loginModel.Password).then(function (resp) {
            if (resp.data.Status) {
                $scope.loginErrModel.Password = "";
                var path = $location.path("Home/Index");
                var abspath = path.$$absUrl;
                var modifiedpath = abspath.replace("/Home/Login#", "");
                window.location = modifiedpath;
            } else {
                $scope.loginErrModel.Password = "Invalid Password";
            }
        })
    }

    $scope.fullname = "";
    $scope.phoneno = "";
    $scope.emailid = "";
    $scope.country = "";
    $scope.subject = "";
    $scope.message = "";

    $scope.SendMail = function () {
        $http.post("/Home/SendContactUsEMail?fullname=" + $scope.fullname + "&phone=" + $scope.phoneno + "&country=" + $scope.country + "&mailfrom=" + $scope.emailid + "&mail_to=&mail_cc=&subj=" + $scope.subject + "&desc=" + $scope.message).then(function (response) {
            alert("Mail delivered successfully!");
            $scope.fullname = "";
            $scope.phoneno = "";
            $scope.emailid = "";
            $scope.country = "";
            $scope.subject = "";
            $scope.message = "";
        })

    }

    //---------------------------------------Modal dialog start-----------------------------------------------//
    $scope.showpopup = function () {
        var modalInstance = $uibModal.open({
            templateUrl: '../ForgetPassword.html',
            controller: 'ModalInstanceCtrl',
            backdrop: 'static',
            scope: $scope,
            resolve: {
                objParameters: function () {
                    return {
                       
                    };
                }
            }
        });
    }

    //---------------------------------------Modal dialog end-------------------------------------------------//

})

module.controller('ModalInstanceCtrl', function ($scope, $controller, $uibModalInstance, $http, $q, objParameters) {

    //close button click
    $scope.close = function () {
        $uibModalInstance.close();
    }
    $scope.headerText = objParameters.Week + " Forget Password";


    $scope.forgetUsername = "";
    $scope.forgetEmailid = "";
    $scope.forgetStatus = false;
    $scope.sending = false;
    $scope.forgetMessage = "Input correct username and registered email id for self verification to receive forgotten password in your registered email id.";
    $scope.showMessage = "Input correct username and registered email id for self verification to receive forgotten password in your registered email id.";

    $scope.checkIdentity = function () {
        $scope.sending = true;
        $http.post("/Home/CheckIdentity?username=" + $scope.forgetUsername + "&emailId=" + $scope.forgetEmailid).then(function (response) {
            $scope.showMessage = response.data.Message;
            if (response.data.Status) {
                $scope.forgetStatus = true;
            } else {
                $scope.forgetStatus = false;
                $scope.sending = false;
            }
        })
    }
})

