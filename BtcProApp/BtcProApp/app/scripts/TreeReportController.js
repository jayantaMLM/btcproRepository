var module = angular.module('app', []);
module.controller('Test', function ($scope, $http, $sce) {

    $scope.searchname = "Raj1-1";
    $scope.level = 2;

    $scope.search = function () {
        $http.get('GetTeamByName?Member=' + $scope.searchname).then(function (data) {
            debugger;
            $scope.treedata = data.data.Members;
        })
        $http.get('GetTeamByTree?Member=' + $scope.searchname + "&levels="+$scope.level).then(function (data) {
            debugger;
            $scope.treehtml = data.data.Members;
            $scope.trustedHtml = $sce.trustAsHtml($scope.treehtml);
        })
    }

    $scope.search();

    ////////////////////////////////////////////////////////////////////////////

    //Fetch permissions of selected role
    $scope.fetchteam = function () {

        // Get the dictionary elements list
        $http.get('GetTeamByNameFirstLevel?Member=' + $scope.searchname).then(function (data) {
            if (data.data.Members.length == 0) {
                $scope.nodata = true;
            } else {
                $scope.nodata = false;
                $scope.tree = [];
                $scope.index = 0;
                for (var ii = 0; ii < data.data.Members.length; ii++) {
                    $scope.index = $scope.index + 1;
                    var newrecord = { level: 0, index: $scope.index, parentindex: 0, id: data.data.Members[ii].Id, isloaded: false, isexpanded: false, haschildren: true, show: true, description: data.data.Members[ii].Username, shortname: data.data.Members[ii].Username, nodes: [], ismouseover1: false, ismouseover2: false, iserror: false, errormessage: "", isborder1: "", isborder2: "", ishighlighted: false }
                    $scope.tree.push(newrecord);
                }

                //loop through to load entire tree
                //------------------------------------------
                angular.forEach($scope.tree, function (value, index) {
                    $scope.preshowhideChildren(value, false);
                })
                //------------------------------------------

                $scope.nodescount = $scope.tree.length;
            }
        })
    }

    // Check if children are loaded in DOM..if (false) then load from table else load from DOM
    $scope.preshowhideChildren = function (data, boolstatus) {
        if (!data.isloaded) {
            $http.get('GetTeamByIdNextLevel?Id=' + data.id).then(function (data1) {
                if (data1.data.Members.length == 0) { data.haschildren = false; }
                angular.forEach(data1.data.Members, function (value, index) {
                    $scope.count = $scope.count + data1.length;
                    var boolchildcount = false;
                    if (value.Level > 0)
                        boolchildcount = true;   //used as a substitution of ChildCount
                    var newrecord = {
                        level: data.level + 1,
                        index: $scope.index,
                        parentindex: data.index,
                        id: value.Id,
                        isloaded: false,
                        isexpanded: true,
                        haschildren: boolchildcount,
                        show: true,
                        description: value.Username,
                        shortname: value.Username,
                        nodes: [],
                        ismouseover1: false,
                        ismouseover2: false,
                        iserror: false,
                        errormessage: "",
                        isborder1: "",
                        isborder2: "",
                        ishighlighted: false
                    }
                    data.nodes.push(newrecord);
                    $scope.index = $scope.index + 1;

                    //load further down
                    $scope.preshowhideChildren(newrecord, true);
                })
            })
        }
        data.isloaded = true;
        $scope.showhideChildren(data.nodes, boolstatus);
    }

    // Recursive iteration through children
    $scope.showhideChildren = function (data, boolstatus) {
        angular.forEach(data, function (value, index) {
            value.show = boolstatus;
            $scope.children = [];
            $scope.children = value.nodes;
            if (value.nodes.length > 0 && value.isexpanded) {
                $scope.showhideChildren($scope.children, boolstatus);
            }
        })
    }

    ///////////////////////////////////////////////////////////////////////////

    $scope.fetchteam();
});

