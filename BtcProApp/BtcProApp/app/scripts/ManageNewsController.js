var module = angular.module('app', []);

module.controller('ManageNews', function ($scope, $http, $location, $sce) {
    $scope.isAdding = false;
    $scope.isEditing = false;

    $scope.isAttachmentUploadingA = false;
    $scope.currentRecordId = 0;

    $scope.NewsModel = {
        Id: 0,
        NewsItemTitle:"",
        NewsItemBody:"",
        ImageFileName:"",
        NewsAuthor: "",
        Add: false,
        Edit:false,
    }

    $scope.objNewsModel = {};
    
    $scope.getAllNews = function () {
        $http.get("/api/NewsReports").then(function (data) {
            debugger;
            $scope.allReports = data.data;
        })
    }
    $scope.getAllNews();

    //new button clicked
    $scope.addNew = function () {
        angular.copy($scope.NewsModel, $scope.objNewsModel);
        $scope.isAdding = true;
        $scope.isEditing = false;
    }

    $scope.uploadLibraryDocumentA = function (files) {
        debugger;
        if (files[0].size > 4000000) {
            alert("Maximum 4MB image size allowed!");
            $("#documentA").val("");
            return false;
        }
        var filename = files[0].name;
        filename = filename.toLowerCase();
        if (filename.includes(".jpg") || filename.includes(".jpeg") || filename.includes(".bmp") || filename.includes(".png") ||
            filename.includes(".pdf") || filename.includes(".txt") || filename.includes(".doc") || filename.includes(".docx") || filename.includes(".xls") ||
            filename.includes(".xlsx") || filename.includes(".ppt") || filename.includes(".pptx")) {
            $scope.isAttachmentUploadingA = true;
            var fd = new FormData();
            //Take the first selected file
            fd.append("file", files[0]);

            $http.post("/api/Library/Resource?module=Images&moduleId=" + $scope.currentRecordId + "&subModule=Attachments&subModuleId=0", fd, {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).then(function (data) {
                $scope.fileInterName = data.data[0].fileInternalName;
                $scope.objNewsModel.ImageFileName = $scope.fileInterName;
                //$("#documentA").val("");
                $scope.isAttachmentUploadingA = false;
               
                alert("Image successfully uploaded!");
            }
            )
        } else {
            alert("Valid file types are: jpg/jpeg/bmp/png/pdf/txt/doc/docx/xls/xlsx/ppt/pptx");
            $("#documentA").val("");
        };

    };
   
    //view document
    $scope.showimage = function (Id) {
        //uniquename, filename, filetype, filepath
        debugger;
        $scope.currentRecordId = Id;
        $http.get('/api/Library/Index?module=Images&moduleId=' + $scope.currentRecordId + "&subModule=Attachments&subModuleId=0").then(function (data4) {
            debugger;
            if (data4.data.uploadresults.length > 0) {
                $scope.document_attachments = data4.data.uploadresults;
                debugger;
                $scope.documentID = data4.data.uploadresults[0].fileInternalName;
                $scope.documentNAME = data4.data.uploadresults[0].fileName;
                $scope.documentFILETYPE = data4.data.uploadresults[0].fileType;
                $scope.fullFilePath = data4.data.uploadresults[0].filePath + $scope.documentID;
                $scope.viewerFullFilePath = "<iframe src='https://docs.google.com/viewer?url=" + $scope.fullFilePath + "&embedded=true&chrome=false&dov=1' style='width:100%;height:750px' frameborder='0'></iframe>";
                $scope.TrustedviewerFullFilePath = $sce.trustAsHtml($scope.viewerFullFilePath);
                $scope.isIframe = true;
            } else {
                $scope.document_attachments = [];
            }
        })

    }

    $scope.insert = function () {
        $http.post("/api/NewsReports", $scope.objNewsModel).then(function (response) {
            alert("Successfully inserted!!!");
            $scope.isAdding = false;
            $scope.isEditing = false;
        })
    }

    $scope.delete = function (id) {
        var ans = confirm("Sure you want to delete the news?");
        if (ans) {
            $http.delete("/api/NewsReports/" + id).then(function (response) {
                $scope.getAllNews();
            })
        }
    }

    $scope.edit = function (report) {
        angular.copy($scope.NewsModel, $scope.objNewsModel);
        $scope.objNewsModel = report;
        $scope.isAdding = false;
        $scope.isEditing = true;
    }

    $scope.update = function () {
        $http.post("/Home/NewsReportsEdit", $scope.objNewsModel).then(function (response) {
            $scope.getAllNews();
            $scope.isAdding = false;
            $scope.isEditing = false;
        })
    }
})
