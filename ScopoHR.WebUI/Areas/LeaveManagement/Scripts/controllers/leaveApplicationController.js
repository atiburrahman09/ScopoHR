scopoAppControllers.controller('leaveApplicationController', ['$scope', 'leaveAppliactionService', function ($scope, leaveAppliactionService) {

    $scope.appList = [];
    $scope.lastSelected = 0;
    $scope.pageNo = -1;
    $scope.app = {        
    };

    var dateFormat = "MM/DD/YYYY";

    $scope.init = function () {
        $scope.mode = "new";
        $scope.app = {};
        getAllApps($scope.pageNo);
        getLeaveTypes();
        $scope.app.ApplicationDate = moment(new Date()).format(dateFormat);
    }
    
    $scope.getEmployeeByKeyword = function (val) {
        if (val.length < 3) {
            return;
        }
        return leaveAppliactionService.getEmployeeDropDownByKeyword(val).then(function (res) {
            return res.data;
        });
    };

    // 
    function getAllApps() {
        leaveAppliactionService.getAllApps().then(function (res) {
            angular.forEach(res.data, function (value, key) {
                res.data.TotalDays = getTotalDays(value.FromDate, value.ToDate);
            });
            $scope.appList = formatDate(res.data, dateFormat, ["FromDate", "ToDate", "ApprovalDate", "ApplicationDate","SubstituteDate"]);
            console.log(res.data);
        }, function (err) {
            console.log('Error: ', err);
        })
    }

    function typeaheadOnSelect(item, model, label) {        
        $scope.app.EmployeeID = item.EmployeeID;
        $scope.app.EmployeeName = item.EmployeeName;        
    }
    $scope.typeaheadOnSelect = typeaheadOnSelect;


    function getLeaveTypes() {
        leaveAppliactionService.getLeaveTypes().then(function (res) {
            $scope.LeaveTypeList = res.data;
        }, function (err) {
            console.log(err);
        });
    }

    function appSelected(index) {
        console.log(index);
        $scope.appList[$scope.lastSelected].selected = false;
        $scope.app = angular.copy($scope.appList[index]);
        $scope.app.ApplicationDate = moment(new Date($scope.app.ApplicationDate)).format(dateFormat);
        $scope.appList[index].selected = true;        
        $scope.lastSelected = index;
        $scope.mode = "update";
    }

    function resetForm() {        
        //$scope.$broadcast('angucomplete-alt:clearInput', 'employeeCardNo');
        $scope.mode = "new";
        if ($scope.appList.length) {
            $scope.appList[$scope.lastSelected].selected = false;
        }
        $scope.employeeSetupForm.$setPristine();
        $scope.employeeSetupForm.$setUntouched();
        $scope.app = {
            ApplicationDate: moment(new Date()).format(dateFormat),
            EmployeeID: $scope.app.EmployeeID,
            EmployeeName: $scope.app.EmployeeName
        };
    }
    $scope.resetForm = resetForm;

    function saveClicked() {
        if ($scope.employeeSetupForm.$valid) {
            if (!$scope.app.EmployeeID) {
                alertify.error('Please select employee name.');
                return;
            }

            if ($scope.app != {} && $scope.mode == "new") {
                if (!($scope.app.TotalDays > 0)) {
                    alertify.error("Invalid number of days. From date has to be less than to date.");
                }
                else {
                    leaveAppliactionService.duplicateLeaveCheck($scope.app.EmployeeID,$scope.app.FromDate,$scope.app.ToDate).then(function (res) {
                        if (res.data) {
                            alertify.error("Duplicate Leave Entry.");
                        }
                        else {
                            leaveAppliactionService.createApp($scope.app).then(function (res) {
                                alertify.success("Leave application successfully created.");
                                getAllApps();
                                resetForm();
                            }, function (err) {
                                handleHttpError(err);
                            });
                        }
                    })
                   
                }
            }
            else {
                leaveAppliactionService.updateApp($scope.app).then(function (res) {
                    alertify.success(res.data);
                    getAllApps();
                    resetForm();
                }, function (err) {
                    handleHttpError(err);
                });
            }
        }
        else {
            alertify.error("Invalid form submission. Please fill up the required fields.");
        }
    }
    
    function bindDate(id, model, functionCall) {            
        $scope[model][id] = $('#' + id).val();
        if (functionCall) {
            functionCall();
        }
    }

    $scope.bindDate = bindDate;
    $scope.appSelected = appSelected;
    $scope.saveClicked = saveClicked;

    function getTotalDays(from, to) {
        return getTimeDifference(from, to, 'd') + 1;
    }

    function validateToDate() {        
        if ($scope.app.FromDate != "") {
            if ($scope.app.ToDate == "") {
                alertify.error("To date can't be empty.");
            } else {
                var total = getTotalDays($scope.app.FromDate, $scope.app.ToDate);
                if(total < 0){
                    alertify.error("Invalid date range.");
                    return;
                }
                $scope.app.TotalDays = total;
            }
        }
        else {
            alertify.error("From date can't be empty.");
        }
    }
    $scope.validateToDate = validateToDate;

    $scope.delete=function()
    {
        if($scope.app.LeaveApplicationID > 0)
        {
            leaveAppliactionService.deleteApplication($scope.app.LeaveApplicationID).then(function (res) {
                alertify.success("Leave application successfully deleted");
            }, function (err) { handleHttpError(err); });
        }
        else {
            alertify.error("Please select a leave application");
        }
    }

    $scope.advanceEmployeeSearch = function (val) {
        console.log(val)
        if (val.length <= 5) {
            return;
        }
        else {
            leaveAppliactionService.getAllAppsByEmployeeCardNo(val).then(function (res) {
                angular.forEach(res.data, function (value, key) {
                    res.data.TotalDays = getTotalDays(value.FromDate, value.ToDate);
                });
                $scope.appList = formatDate(res.data, dateFormat, ["FromDate", "ToDate", "ApprovalDate", "ApplicationDate","SubstituteDate"]);
                console.log(res.data);
            }, function (err) {
                console.log('Error: ', err);
            })
        }
    };
}]);