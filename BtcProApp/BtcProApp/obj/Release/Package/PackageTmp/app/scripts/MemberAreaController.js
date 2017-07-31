var module = angular.module('app', []);
module.controller('Members', function ($scope, $http, $location, $window) {
    $http.get("/Home/MembersList").then(function (response) {
        $scope.members = response.data.MemberList;
    })
    $http.get("/Home/GetCountries").then(function (response) {
        $scope.countries = response.data.Countries;
    })

    $scope.resetCountry = function (Id, Index) {
        debugger;
        var countrycd = "";
        angular.forEach($scope.members, function (value, index) {
            if (value.Id == Id) { countrycd = value.CountryCode;}
        })
        //alert(Id + " " + Index + " " + countrycd);
        $http.post("/Home/ResetCountry?Id=" + Id + "&CountryCode=" + countrycd).then(function () {
            alert("Country updated!!!")
        })
    }
   
})