var module = angular.module('app', []);
module.controller('Dashboard', function ($scope, $http, $location,$sce) {
    $scope.data = [];
    var dataPie = [{
        label: "Left",
        data: 10
    }, {
        label: "Right",
        data: 5
    }];

    //$.plot($(".sm-pie"), dataPie, {
    //    series: {
    //        pie: {
    //            innerRadius: 0.7,
    //            show: true,
    //            stroke: {
    //                width: 0.1,
    //                color: '#ffffff'
    //            }
    //        }

    //    },

    //    legend: {
    //        show: true
    //    },
    //    grid: {
    //        hoverable: true,
    //        clickable: true
    //    },

    //    colors: ["#ffdf7c", "#b2def7", "#efb3e6"]
    //});

    $scope.list = true;

    $scope.listClick = function () {
        $scope.list = !$scope.list;
    }
    $http.get("/Home/Dashboarddata").then(function (response) {
        $scope.data = response.data.DashboardDataModel;
        $scope.plutopkg = parseInt($scope.data.PlutoPurchasePc);
        dataPie[0].data = $scope.data.MyLeftCount;
        dataPie[1].data = $scope.data.MyRightCount;

        $http.get("/Home/MyWalletBalance?WalletId=1").then(function (data) {
            $scope.data.CashWalletBalance = data.data.Balance;
        })
        $http.get("/Home/MyWalletBalance?WalletId=2").then(function (data) {
            $scope.data.ReserveWalletBalance = data.data.Balance;
        })
    })

    $scope.CashWallet = function () {
        var path = $location.path("/Home/CashWallet");
        var abspath = path.$$absUrl;
        var modifiedpath = abspath.replace("/Home/Index#", "");
        window.location = modifiedpath;
    }
    $scope.ReserveWallet = function () {
        var path = $location.path("/Home/ReserveWallet");
        var abspath = path.$$absUrl;
        var modifiedpath = abspath.replace("/Home/Index#", "");
        window.location = modifiedpath;
    }
    $scope.ReturnWallet = function () {
        var path = $location.path("/Home/ReturnWallet");
        var abspath = path.$$absUrl;
        var modifiedpath = abspath.replace("/Home/Index#", "");
        window.location = modifiedpath;
    }
    $scope.TeamMembers = function () {
        var path = $location.path("/Home/TeamMembers");
        var abspath = path.$$absUrl;
        var modifiedpath = abspath.replace("/Home/Index#", "");
        window.location = modifiedpath;
    }
    $scope.MyPurchase = function () {
        var path = $location.path("/Home/MyPurchase");
        var abspath = path.$$absUrl;
        var modifiedpath = abspath.replace("/Home/Index#", "");
        window.location = modifiedpath;
    }
    $scope.MyRepurchase = function () {
        var path = $location.path("/Home/WithdrawalRequests");
        var abspath = path.$$absUrl;
        var modifiedpath = abspath.replace("/Home/Index#", "");
        window.location = modifiedpath;
    }

    $scope.getAllNews = function () {
        $http.get("/api/NewsReports").then(function (data) {
            debugger;
            $scope.allReports = data.data;
        })
    }
    $scope.getAllNews();

    $scope.showreport = function (report) {
        $scope.list = false;
        $scope.selectedreport = report;
        $scope.trustedSelectedReport = $sce.trustAsHtml($scope.selectedreport.NewsItemBody);
    }

})