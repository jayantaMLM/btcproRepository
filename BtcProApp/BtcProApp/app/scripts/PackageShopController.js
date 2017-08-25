var module = angular.module('app', []);
module.controller('PackageShop', function ($scope, $http, $sce, $interval) {
    $scope.isProductSelected = false;
    $scope.selectedProduct = "";
    $scope.minPay = 0;
    $scope.maxPay = 0;
    $scope.priceToPay = "";
    $scope.payamount = 0;
    $scope.errmsg = "";
    $scope.errmsg1 = "";
    $scope.proceed = false;
    $scope.walletBalance = 0;
    $scope.packageId = 0;
    $scope.loader = false;
    $scope.loaderBTC = false;
    $scope.loaderLTC = false;
    $scope.loaderETH = false;
    $scope.loaderDSC = false;
    $scope.showInvoice = false;
    $scope.address = {};
    $scope.today = new Date();

    $scope.hidePurchaseBtn = true;  //Repurchase
    $scope.upgradeOption = false;
    $scope.hidePurchaseBtn_Pluto = false;
    $scope.hidePurchaseBtn_Jupiter = false;
    $scope.hidePurchaseBtn_Earth = false;
    $scope.hidePurchaseBtn_Amazing = false;
    $scope.hidePurchaseBtn_OctaCore = false;

    $scope.showBitCoinInfo = false;
    //$scope.FullQRImagePath = "/Content/qrgen.png";
    $scope.FullQRImagePath = "";
    $scope.TrustedviewerFullFilePath = $sce.trustAsHtml($scope.FullQRImagePath);

    $scope.getbalance = function () {
        $http.get("MyWalletBalance?WalletId=2").then(function (data) {
            $scope.walletBalance = data.data.Balance;
            $scope.CheckBalance();
        })
        $http.get("/Home/MyPurchases").then(function (purdata) {
            debugger;
            $scope.purchases = purdata.data.Purchases;
            if ($scope.purchases.length == 0) {
                $scope.hidePurchaseBtn = false;

                $scope.upgradeOption = false;
                $scope.hidePurchaseBtn_Pluto = false;
                $scope.hidePurchaseBtn_Jupiter = false;
                $scope.hidePurchaseBtn_Earth = false;
                $scope.hidePurchaseBtn_Amazing = false;
                $scope.hidePurchaseBtn_OctaCore = false;
            } else {
                $scope.filteredpurchases = [];
                angular.forEach($scope.purchases, function (value, index) {
                    if (value.PackageId != 5) { $scope.filteredpurchases.push(value);}
                })
            }


            if ($scope.filteredpurchases.length > 0 && $scope.filteredpurchases[$scope.filteredpurchases.length - 1].PackageId == 1) {
                $scope.hidePurchaseBtn = true;

                $scope.upgradeOption = true;
                $scope.hidePurchaseBtn_Pluto = true;
                $scope.hidePurchaseBtn_Jupiter = false;
                $scope.hidePurchaseBtn_Earth = false;
                $scope.hidePurchaseBtn_Amazing = false;
                $scope.hidePurchaseBtn_OctaCore = false;
            }
            if ($scope.filteredpurchases.length > 0 && $scope.filteredpurchases[$scope.filteredpurchases.length - 1].PackageId == 2) {
                $scope.hidePurchaseBtn = true;

                $scope.upgradeOption = true;
                $scope.hidePurchaseBtn_Pluto = true;
                $scope.hidePurchaseBtn_Jupiter = true;
                $scope.hidePurchaseBtn_Earth = false;
                $scope.hidePurchaseBtn_Amazing = false;
                $scope.hidePurchaseBtn_OctaCore = false;
            }
            if ($scope.filteredpurchases.length > 0 && $scope.filteredpurchases[$scope.filteredpurchases.length - 1].PackageId == 3) {
                $scope.hidePurchaseBtn = true;

                $scope.upgradeOption = true;
                $scope.hidePurchaseBtn_Pluto = true;
                $scope.hidePurchaseBtn_Jupiter = true;
                $scope.hidePurchaseBtn_Earth = true;
                $scope.hidePurchaseBtn_Amazing = false;
                $scope.hidePurchaseBtn_OctaCore = false;
            }
        })
    }
    
    $scope.getbalance();

    $scope.payamount_validate = function () {
        if ($scope.payamount < $scope.minPay || $scope.payamount > $scope.maxPay) {
            $scope.errmsg = "Amount must be between " + $scope.minPay + "$ - " + $scope.maxPay + "$";
        }
        else {
            $scope.errmsg = "";
        }
    }

    $scope.CheckBalance = function () {
        if ($scope.walletBalance < $scope.payamount) {
            $scope.errmsg1 = " Insufficient wallet balance.";
        } else {
            $scope.errmsg1 = "";
        }
    }

    $scope.newPurchase = function () {
        $scope.loader = true;
        $http.post("MyNewPurchase?packageId=" + $scope.packageId + "&investmentAmt=" + $scope.payamount).then(function (data) {
            if (data.data.Success == "TRUE") {
                $http.post("/Home/NotifyAdminAboutPackagePurchase?PackageId=" + $scope.packageId + "&Amount=" + $scope.payamount);
                $http.get("MyAddress").then(function (retdata) {
                    $scope.address = retdata.data.Address;
                    $scope.loader = false;
                })
               
            }
        })
    }

    var updateQRCode = function () {
        $scope.TrustedviewerFullFilePath = $sce.trustAsHtml($scope.FullQRImagePath);
        $scope.$apply();
    };

    $scope.bitCoinPurchase = function (cointype) {
        $http.post("PayCryptoCurrency?packageId=" + $scope.packageId + "&UplineId=0&investmentAmt=" + $scope.payamount+"&cointype="+cointype).then(function (response) {
            $scope.objTransaction = response.data.objTransaction;
            $scope.FullQRImagePath = $scope.objTransaction.Qrcode_url;
            $scope.TrustedviewerFullFilePath = $sce.trustAsHtml($scope.FullQRImagePath);
            $scope.proceed = false;
            $scope.showBitCoinInfo = true;
            $scope.loaderBTC = false;
            $scope.loaderLTC = false;
            $scope.loaderETH = false;
            $scope.loaderDSC = false;
            $interval(updateQRCode, 1000);
        })
    }
})