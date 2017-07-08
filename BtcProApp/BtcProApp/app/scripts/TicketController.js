var module = angular.module('app', []);

module.filter("TypeFilter", function () {
    return function (charStatus) {
        var fullname = "";
        var char = "";
        if (charStatus != null) {
            char = charStatus.toString();
        }
        switch (char) {
            case 'A':
                fullname = "Business building queries";
                break;
            case 'B':
                fullname = "Bitcoin deposit in Account";
                break;
            case 'C':
                fullname = "Financial";
                break;
            case 'D':
                fullname = "Founder upgrade";
                break;
            case 'E':
                fullname = "From support email";
                break;
            case 'F':
                fullname = "General";
                break;
            case 'G':
                fullname = "Personal information error";
                break;
            case 'H':
                fullname = "Product queries";
                break;
            case 'I':
                fullname = "Website error";
                break;
            default:
                fullname = "";
                break;
        }
        return fullname;
    }
})

module.controller('Ticket', function ($scope, $http, $location,$sce) {
    //$scope.document_attachments.uploadresults = [];
    $scope.isAttachmentUploadingA = false;
    $scope.test = "Message";
    $scope.currentRecordId = 0;
    $scope.ticketType = "";
    $scope.subjectName = "";
    $scope.documentComments = "";
    $scope.ticketPriority = "";
    $scope.fileInterName = "";
    $scope.Filename = "";
    $scope.disableSubmit = false;
    $scope.fileId = 0;
   
    $http.get("/api/Tickets").then(function (response) {
        $scope.mytickets = response.data;
    })

    var TicketObj = {
        Id: 0,
        RegistrationId: 0,
        TicketCategory: "",
        Subject: "",
        Message: "",
        Priority: "",
        LibraryId: 0,
        LibraryFilename:""
    }

    //upload Ticket document
    $scope.uploadLibraryDocumentA = function (files) {
        if (files[0].size > 4000000) {
            alert("Maximum 4MB document size allowed!");
            $("#documentA").val("");
            return false;
        }
        $scope.isAttachmentUploadingA = true;
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
                $scope.isAttachmentUploadingA = false;
            }
            )
        } else {
            alert("Valid file types are: jpg/jpeg/bmp/png/pdf/txt/doc/docx/xls/xlsx/ppt/pptx");
            $("#documentA").val("");
        };

    };

    //delete KYC attachment
    $scope.deleteAttachmentA = function (id) {
        var ans = confirm("Are you sure you want to delete?");
        if (ans) {
            $http.delete("/api/Library/Resource/" + id).then(function (data) {
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
        $scope.currentRecordId = Id;
        $http.get('/api/Library/Index?module=Images&moduleId=' + $scope.currentRecordId + "&subModule=Attachments&subModuleId=0").then(function (data4) {
            if (data4.data.uploadresults.length>0) {
                $scope.document_attachments = data4.data.uploadresults;
                $scope.documentID = data4.data.uploadresults[0].fileInternalName;
                $scope.documentNAME = data4.data.uploadresults[0].fileName;
                $scope.documentFILETYPE = data4.data.uploadresults[0].fileType;
                $scope.fullFilePath = data4.data.uploadresults[0].filePath + $scope.documentID;
                $scope.viewerFullFilePath = "<iframe src='https://docs.google.com/viewer?url=" + $scope.fullFilePath+ "&embedded=true&chrome=false&dov=1' style='width:100%;height:750px' frameborder='0'></iframe>";
                $scope.TrustedviewerFullFilePath = $sce.trustAsHtml($scope.viewerFullFilePath);
                $scope.isIframe = true;
            } else {
                $scope.document_attachments = [];
            }
        })
       
    }

    $scope.submitTicket = function () {
        TicketObj.Id = 0;
        TicketObj.RegistrationId = 0;
        TicketObj.TicketCategory = $scope.ticketType;
        TicketObj.Subject = $scope.subjectName;
        TicketObj.Message = $scope.documentComments;
        TicketObj.Priority = $scope.ticketPriority;
        TicketObj.LibraryId = $scope.fileId;
        TicketObj.LibraryFilename = $scope.fileInterName;

        $http.post("/api/Tickets", TicketObj).then(function (response) {
            $scope.Filename = "";
            $scope.subjectName = "";
            $scope.documentComments = "";
            $scope.fileInterName = "";
            $scope.fileId = 0;
            $("#documentA").val("");
            alert("Ticket submitted successfully");
            $scope.disableSubmit = false;
            $http.get("/api/Tickets").then(function (response) {
                $scope.mytickets = response.data;
            })
        })
    }
})

module.controller('AccountStatus', function ($scope, $http, $location, $sce) {
    //transaction password related code--------------------------------------------------------------------------------------

   
    $scope.trnpasswordIsOk = false;
    $scope.trnpasswordexists = false;

    $http.get("/Home/IsUserMember").then(function (response) {
        $scope.isExists = response.data.Found;
        $scope.trnpasswordexists = response.data.TrnPasswordExists;
    })

    $scope.updateTrnPwd = function () {
        if ($scope.trxpassword == null || $scope.trxpassword == '') {
            alert("Transaction password cannot be blank");
        } else {
            $http.post("/Home/UpdateTransactionPassword?TxPassword=" + $scope.trxpassword).then(function (response) {
                if (response.data.Success) {
                    $scope.trnpasswordexists = true;
                } else {
                    alert("Update failed!!! Reset again");
                }

            })
        }
    }

    $scope.IsTrnPasswordOK = function () {
        $http.get("/Home/MatchTrnPassword?TxPassword=" + $scope.trnpassword).then(function (response) {
            if (response.data.Success) {
                $scope.trnpasswordIsOk = true;
            } else {
                $scope.trnpasswordIsOk = false;
            }
        })
    }

    //-------------------------------------------------------------------------transaction password related code end

    $scope.BitCoinAcNo = "";
    $http.get("/Home/GetMyAccountNo").then(function (response) {
        $scope.BitCoinAcNo = response.data.WalletAc;
        $scope.trnpassword = "";
    })

    $scope.UpdateAcno = function () {
        $http.get("/Home/MyAccountNo?WalletId=" + $scope.BitCoinAcNo).then(function (data) {
            if (data.data.Success) {
                alert("Updated successfully");
            }
        })
    }
})

module.controller('TicketSupport', function ($scope, $http, $location, $sce) {
    
    $scope.getTickets = function () {
        $http.get("/Home/ActiveSupportTickets").then(function (response) {
            debugger;
            $scope.mytickets = response.data.Tickets;
        })
    }
   
    //view document
    $scope.showimage = function (Id) {
        //uniquename, filename, filetype, filepath
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
    $scope.notifyuser = function (ticket) {
        $http.post("/Home/TicketFeedback", ticket).then(function (response) {
            alert("Feedback sent!");
        })
    }
    $scope.closeticket = function (ticket) {
        var ans = confirm("Sure you want to close it?");
        if (ans) {
            $http.post("/Home/TicketClose", ticket).then(function (response) {
                $scope.getTickets();
                alert("Ticket closed!");
            })
        }
    }

    $scope.getTickets();
})