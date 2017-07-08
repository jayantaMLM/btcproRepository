var module = angular.module('app', []);

module.filter("TypeFilter", function () {
    return function (charStatus) {
        debugger;
        var fullname = "";
        var char = "";
        if (charStatus != null) {
            char = charStatus.toString();
        }
        switch (char) {
            case 'A':
                fullname = "Address Proof";
                break;
            case 'I':
                fullname = "Identity Proof";
                break;
            case 'N':
                fullname = "Income Proof";
                break;
             default:
                fullname = "";
                break;
        }
        return fullname;
    }
})

module.controller('KycUpload', function ($scope, $http, $location, $sce) {
    $scope.document_attachments = [];
    $scope.isAttachmentUploadingA = false;
    $scope.fileInterName = "";
    $scope.fileId = 0;
    $scope.isIframe = false;
    $scope.disableSubmit = false;
    $scope.currentRecordId = 0;

    $http.get("/api/KYCDocuments").then(function (response) {
        $scope.mykycs = response.data;
    })

    var KYCObj = {
        Id: 0,
        RegistrationId: 0,
        DocumentType: "",
        DocumentName: "",
        Comments: "",
        LibraryId: 0,
        LibraryFilename: ""
    }

    //upload KYC document
    $scope.uploadLibraryDocumentA = function (files) {
        debugger;
        if (files[0].size > 4000000) {
            alert("Maximum 4MB document size allowed!");
            $("#documentA").val("");
            return false;
        }
        var filename = files[0].name;
        filename = filename.toLowerCase();
        if (filename.includes(".jpg") || filename.includes(".jpeg") || filename.includes(".bmp") || filename.includes(".png") ||
            filename.includes(".pdf")) {
            $scope.isAttachmentUploadingA = true;
            var fd = new FormData();
            //Take the first selected file
            fd.append("file", files[0]);

            $http.post("/api/Library/Resource?module=Images&moduleId=" + $scope.currentRecordId + "&subModule=KYC&subModuleId=0", fd, {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).then(function (data) {
                debugger;
                $scope.fileInterName = data.data[0].fileInternalName;
                $scope.fileId = data.data[0].fileId;
                $scope.isAttachmentUploadingA = false;
                if (!$scope.document_attachments.uploadresults) {
                    $scope.document_attachments["uploadresults"] = [];
                    $scope.document_attachments.uploadresults.push(data[0]);
                }
                else {
                    $scope.document_attachments.uploadresults.push(data[0]);
                }
                alert("Attachment successfully uploaded!");
            }
            )
        } else {
            alert("Valid file types are: jpg/jpeg/bmp/png/pdf");
            $("#documentA").val("");
        };

    };

    //delete KYC attachment
    $scope.deleteAttachmentA = function (id) {
        var ans = confirm("Are you sure you want to delete?");
        if (ans) {
            $http.delete("/api/Library/Resource/" + id).success(function (data) {
                var indx = 0;
                angular.forEach($scope.document_attachments.uploadresults, function (value, index) {
                    if (value.fileId == id) { indx = index; }
                })
                $scope.document_attachments.uploadresults.splice(indx, 1);
                toaster.pop('success', 'Success', "Attachment successfully deleted!");
            });
        }
    }
  
    //view document
    $scope.showimage = function (Id) {
        //uniquename, filename, filetype, filepath
        debugger;
        $scope.currentRecordId = Id;
        $http.get('/api/Library/Index?module=Images&moduleId=' + $scope.currentRecordId + "&subModule=KYC&subModuleId=0").then(function (data4) {
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

    $scope.submitTicket = function () {
        KYCObj.Id = 0;
        KYCObj.RegistrationId = 0;
        KYCObj.DocumentType = $scope.documentType;
        KYCObj.DocumentName = $scope.documentName;
        KYCObj.Comments = $scope.documentComments;
        KYCObj.LibraryId = $scope.fileId;
        KYCObj.LibraryFilename = $scope.fileInterName;

        $http.post("/api/KYCDocuments", KYCObj).then(function (response) {
            debugger;
            $scope.documentType = "";
            $scope.documentName = "";
            $scope.documentComments = "";
            $scope.fileInterName = "";
            $scope.fileId = 0;
            $("#documentA").val("");
            alert("KYC document submitted successfully");
            $scope.disableSubmit = false;
            $http.get("/api/KYCDocuments").then(function (response) {
                debugger;
                $scope.mykycs = response.data;
            })
        })
    }
})