scopoAppControllers.controller('publishNoticeCtrl', ['$scope', 'publishNoticeService', 'alertify', function ($scope, publishNoticeService, alertify) {

    var userList = [];
    var selectionHandle = { previousIndex: null, currentIndex: null };
    $scope.selected = "";
    $scope.selectedUser = "";
    $scope.userArray = [];
    var prevIndex = null;
    var prevIndexUser = null;
    $scope.emptyUser = true;

    var uID = "";

    $scope.init = function () {
        $scope.getAllRecentUserList();
    }

   
    var getAllRecentUserList = function () {
        noticeAssignService.getAllRecentUserList()
            .then(function (res) {
                $scope.userList = res.data;
                if ($scope.userList.length == 0) {
                    $scope.emptyUser = false;
                }
                else {
                    $scope.emptyUser = true;
                }
            }, function (err) {
                handleHttpError(err);
            })
    };
    $scope.getAllRecentUserList = getAllRecentUserList;
  
    $scope.selectedUser = function (userIndex) {
        uID = $scope.userList[userIndex].EmployeeID;
        if ($scope.userList[userIndex].selected) {
            $scope.userList[userIndex].selected = false;
            $scope.userArray.splice($scope.userArray.indexOf(uID), 1);
        }
        else {
            $scope.userList[userIndex].selected = true;
            $scope.userArray.push(uID);
        }

        prevIndexUser = userIndex;
    };
 

    $scope.publishNotice = function (nID, userArray) {
        console.log($scope.userArray);
        console.log($scope.nID);
        noticeAssignService.publishNotice({ NoticeID: nID, EmployeeList: userArray }).then(function (res) {
            console.log(res.data);
            alertify.success("Notice Successfully Assigned to the selected Users!!!!");
            ClearFields();
        }, function (err) {
            handleHttpError(err);
        });
    };


}]);