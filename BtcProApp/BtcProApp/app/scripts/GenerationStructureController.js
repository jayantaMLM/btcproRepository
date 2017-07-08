var module = angular.module('app', ['ngAnimate', 'ngSanitize', 'ui.bootstrap']);

module.controller('GenerationTree', function ($scope, $http, $sce, $rootScope) {
    $scope.dynamicPopover = {
        content: 'Hello, World!',
        templateUrl: 'myPopoverTemplate.html',
        title: 'Title'
    };

    $scope.placement = {
        options: [
          'top',
          'top-left',
          'top-right',
          'bottom',
          'bottom-left',
          'bottom-right',
          'left',
          'left-top',
          'left-bottom',
          'right',
          'right-top',
          'right-bottom'
        ],
        selected: 'top'
    };

    $scope.htmlPopover = $sce.trustAsHtml('<b style="color: red">I can</b> have <div class="label label-success">HTML</div> content');


    $scope.searchname = "";
    $scope.level = 5;
    $scope.loading = true;
    $scope.search = function () {
        $scope.loading = true;
        $http.get("GetUnilevelTeamByTree?Member=" + $scope.searchname + "&levels=" + $scope.level + "&Id=null").then(function (data) {
            debugger;
            $scope.treehtml = data.data.Members;
            $scope.trustedHtml = $sce.trustAsHtml($scope.treehtml);
            $scope.loading = false;
        })
    }
    $scope.search();

    $scope.clicksearch = function (Id) {
        $scope.loading = true;
        var memname = "";
        $http.get("GetUnilevelTeamByTree?Member=" + memname + "&levels=" + $scope.level + "&Id=" + Id).then(function (data) {
            $scope.loading = true;
            $scope.treehtml = data.data.Members;
            $scope.trustedHtml = $sce.trustAsHtml($scope.treehtml);
            $scope.loading = false;
        })
    }
})