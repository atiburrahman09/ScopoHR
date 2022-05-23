scopoAppControllers.controller('leaveApprovalController', ['$scope', 'leaveApprovalService', function ($scope, leaveApplicationService) {
    $scope.isAllSelected = false;
    $scope.appList = [];
    $scope.leaveMapping = [];
    $scope.pageNo = -1;
    $scope.leaveApplicationStatus = {
        pending: 0,
        approaved: 1,
        rejected: 2,
        toString: function (status) {
            for (i in this) {
                if (this[i] === status) {
                    return i.toString();
                }
            }
        }
    };

    $scope.menus = {
        pending: true,
        approaved: false,
        rejected: false
    };
    $scope.currentMenu = '';
    $scope.init = function () {
        $scope.navigateTo('pending');
    }

    let currentAppStatus = '';
    $scope.totalCount = 0;
    $scope.getNext = () => {
        if ($scope.totalCount < $scope.pageNo * 20 + 1) {
            return;
        }
        $scope.pageNo += 1;              
        getApplications(currentAppStatus);
    }

    $scope.getPrev = () => {
        $scope.pageNo -= 1;                
        getApplications(currentAppStatus);
    }

    let getApplications = (applicationStatus) => {
        if ($scope.pageNo < 0)
            $scope.pageNo = 0;
        leaveApplicationService.getApplications(applicationStatus, $scope.pageNo).then(function (res) {
            console.log(res.data.apps);
            $scope.appList = formatDate(res.data.apps, "DD-MMM-YY", ["ApplicationDate", "FromDate", "ToDate", "ApprovalDate"]);
            $scope.totalCount = res.data.count;
        });
    };


    function changeApplicationStatus(status, list) {
        for (var i in list) {
            list[i].Status = status;
        }
        // update
        leaveApplicationService.updateApplications(list).then(function (res) {
            navigateTo($scope.leaveApplicationStatus.toString(status));
        });
    }

    function getShortenedReasonOfLeave(string, length) {
        if (string) {
            return string.substr(0, 15);
        }
        return "";
    }

    function getSelectedApplications(list) {
        var selectedApps = []
        for (var i in list) {
            if (list[i].selected) {
                selectedApps.push(list[i]);
            }
        }
        return selectedApps;
    }

    function approaveApplications() {

        if ($scope.menus.approaved) {
            return;
        }

        var list = getSelectedApplications(angular.copy($scope.appList));
        if (list.length < 1) {
            alertify.error('Nothing is selected!');
            return;
        }

        leaveApplicationService.approaveApplications(list).then(function (res) {
            //changeApplicationStatus($scope.leaveApplicationStatus.approaved, list);
            if (res.status != 200) {
                alertify.error(res.data);
            } else {
                alertify.success(res.data);
            }
            if ($scope.menus.pending) {
                getApplications($scope.leaveApplicationStatus.pending);
            } else {
                getApplications($scope.leaveApplicationStatus.rejected);
            }
        }, function (err) {
            handleHttpError(err);
        });
    }

    function rejectApplications() {
        var list = getSelectedApplications(angular.copy($scope.appList));
        if (list.length < 1) {
            alertify.error('Nothing is selected!');
            return;
        }
        changeApplicationStatus($scope.leaveApplicationStatus.rejected, list);
    }

    function selectApplication(index, list) {
        list[index].selected = !list[index].selected;
    }

    function navigateTo(folder) {                
        $scope.menus.current = folder;
        currentAppStatus = $scope.leaveApplicationStatus[folder];
        for (var i in $scope.menus) {
            if (folder === i) {
                //hightlight selected folder
                $scope.menus[i] = true;
                //I made menu name and leave application status same name so i can call this way
                getApplications($scope.leaveApplicationStatus[i], $scope.pageNo);
                //Empty the leave mapping
                $scope.leaveMapping = [];
            } else {
                $scope.menus[i] = false;
            }
        }
    }

    function selectAllApplications(isAllSelected, list) {
        $scope.isAllSelected = !isAllSelected;
        if (!isAllSelected) {
            for (var i in list) {
                list[i].selected = true;
            }
        } else {
            for (var i in list) {
                list[i].selected = false;
            }
        }
    }

    function getLeaveMapping(employeeID) {
        if (!employeeID) {
            alertify.error('Please try again!');
            return;
        }

        leaveApplicationService.getLeaveMapping(employeeID).then(function (res) {
            if (res.data.length < 1) {
                alertify.error('Info not available!');
                return;
            }
            $scope.leaveMapping = res.data;
        });
    }

    $scope.selectApplication = selectApplication;
    $scope.selectAllApplications = selectAllApplications;
    $scope.navigateTo = navigateTo;
    $scope.approaveApplications = approaveApplications;
    $scope.rejectApplications = rejectApplications;
    $scope.getShortenedReasonOfLeave = getShortenedReasonOfLeave;
    $scope.getLeaveMapping = getLeaveMapping;
}]);