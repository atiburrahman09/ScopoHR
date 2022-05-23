scopoAppControllers.controller('noticeAssignCtrl', ['$scope', '$timeout', 'noticeAssignService', 'alertify', function ($scope, $timeout, noticeAssignService, alertify) {
  
    var noticeList = [];
    var userList = [];
    var selectionHandle = { previousIndex: null, currentIndex: null };
    $scope.selected = "";
    $scope.selectedUser = "";
    $scope.userArray = [];
    $scope.nID = "";
    var prevIndex = null;
    var prevIndexUser = null;
    $scope.emptyUser = true;
    $scope.noticeDetails = {};

    var uID = "";

    $scope.init = function (editor) {
        $scope.getAllNoticeList();
        $scope.getAllRecentUserList();

        $scope.people = [
      { label: 'EmployeeName' },
      { label: 'CardNo' },
      { label: 'Designation' },
      { label: 'MobileNo' },
      { label: 'Department' },
      { label: 'FathersName' },
      { label: 'PermanentAddress' },
      { label: 'PresentAddress' },
      { label: 'SpouseName' }
        ]
      

        $scope.ck = CKEDITOR.instances;
        CKEDITOR.on('instanceReady', function (ev) {
            if ($scope.iframeElement == undefined) {
                $scope.iframeElement = $scope.ck.editor1.document.$.defaultView.frameElement;
            }
        });
    }

    var getAllNoticeList = function () {
        noticeAssignService.getAllNoticeList()
            .then(function (res) {
                $scope.noticeList = res.data.data;
            }, function (err) {
                handleHttpError(err);
            })
    };
    var getAllRecentUserList = function () {
        noticeAssignService.getAllRecentUserList()
            .then(function (res) {
                $scope.userList = res.data;
                if($scope.userList.length == 0)
                {
                    $scope.emptyUser = false;
                }
                else {
                    $scope.emptyUser = true;
                }
            }, function (err) {
                handleHttpError(err);
            })
    };
    $scope.getAllNoticeList = getAllNoticeList;
    $scope.getAllRecentUserList = getAllRecentUserList;
    $scope.selected = function (index) {
       
        $scope.nID = $scope.noticeList[index].NoticeID;
        $scope.noticeInformation = $scope.getNoticeDetails($scope.nID);

        $scope.noticeList[index].selected = true;
        if (prevIndex != null) {
            $scope.noticeList[prevIndex].selected = false;
        }

        prevIndex = index;
       
    };

    $scope.selectedUser = function (userIndex) {
        uID = $scope.userList[userIndex].EmployeeID;
         if ($scope.userList[userIndex].selected)
         {
             $scope.userList[userIndex].selected = false;
             $scope.userArray.splice($scope.userArray.indexOf(uID), 1);
         }
         else {
             $scope.userList[userIndex].selected = true;
             $scope.userArray.push(uID);
         }
        
         prevIndexUser = userIndex;
    };
    $scope.getNoticeDetails = function (nID) {
        console.log(nID);
        if (nID != null)
        {
            noticeAssignService.getNoticeDetails(nID)
                .then(function (res) {
                    $scope.noticeDetails = res.data.data;
                }, function (err) {
                    handleHttpError(err);
                })
        }
    }

    $scope.publishNotice = function (nID, userArray) {
        //console.log($scope.userArray);
        //console.log($scope.nID);
        //noticeAssignService.publishNotice({ NoticeID: nID, EmployeeList: userArray }).then(function (res) {
        //    console.log(res.data);
        //    alertify.success("Notice Successfully Assigned to the selected Users!!!!");
        //    ClearFields();
        //},function(err)
        //{
        //    handleHttpError(err);
        //});
    };

    $scope.createUpdateNotice = function (noticeDetails) {
        console.log(noticeDetails.NoticeDetail);
        if ($scope.noticeForm.$valid) {
            noticeAssignService.createUpdate(noticeDetails)
                       .then(function (res) {
                           $scope.getAllNoticeList();
                           alertify.success(res.data);
                           $scope.noticeForm.$setPristine();
                           $scope.noticeForm.$setUntouched();
                           ClearFields();
                       }, function (err) {
                           handleHttpError(err);
                       });
        }
        else {
            alertify.error("Please fill up the required fields.");
        }

    };

    function ClearFields() {
        $scope.noticeDetails = {};
    };

    $scope.newMode=function()
    {
        $scope.noticeDetails = "";        
    }


    }]);