var module = angular.module('app', []);

module.controller('FixedPayout', function ($scope, $http) {
    $scope.processing = false;
    $scope.processed = false;
    $scope.processcount = 0;
    $scope.totalpayout = 0;
    $scope.searchtxt = "";

    $scope.todayRecords = function () {
        $http.get("/Home/TodayPendingInvestmentReturnprocess").then(function (response) {
            $scope.pendingrecs = response.data.ToProcess;
            $scope.totalpayout = response.data.TotalPayout;
        })
    }
    $scope.todayRecords();

    $scope.process = function () {
        $scope.processing = true;
        $http.post("/Home/CalculateDailyPayout").then(function (response) {
            angular.forEach($scope.pendingrecs, function (value, index) {
                value.Status = "Paid";
            })
            $scope.processing = false;
            $scope.processed = true;
            $scope.processcount = response.data.Postings;
        })
    }
})