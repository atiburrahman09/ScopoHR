scopoAppControllers.controller('inActiveEmployeeCtrl', ['$scope', 'inActiveEmployeeService', 'alertify', function ($scope, inActiveEmployeeService, alertify) {

    $scope.inactiveEmployee = {};
    var prevIndex = null;
    var currentIndex = null;
    $scope.employeeList = [];
    $scope.inactivetypes = [{ name: 'Dismissed', InActiveType: 1 }, { name: 'Resigned', InActiveType: 2 }, { name: 'Drop Out', InActiveType: 3 }];
    $scope.reasonTypes = [{ name: 'Illness'}, { name: 'Family Crisis'}, { name: 'Unavoidable Reason'},
    { name: 'Personal Problem'}, { name: 'Unauthorised Absent'}, { name: 'Insufficient Salary'}];

    $scope.init = function () {
        getRecentEmployees();
        $scope.dismissedReason = true;
        $scope.dropOutReason = false;
    }

    $scope.saveInformation = function () {
        console.log($scope.inactiveEmployee);
        //return;
        if ($scope.inActiveEmployeeForm.$valid) {
            //$scope.inactiveEmployee.EmployeeID = $scope.employeeID;
            inActiveEmployeeService.saveInformation($scope.inactiveEmployee).then(function (res) {
                alertify.success(res.data);
                $scope.inactiveEmployee = {};
                $scope.inActiveEmployeeForm.$setPristine();
                $scope.inActiveEmployeeForm.$setUntouched();
            }, function (err) {
                handleHttpError(err);
            });
        }
        else {
            alertify.error("Please fill the required information's.");
        }
    }


    let getRecentEmployees = () => {
        inActiveEmployeeService.getRecentEmployees().then((res) => {
            $scope.isRecent = true;
            $scope.employeeList = res.data;
            //$scope.employeeList[0].selected = true;
            //$scope.employeeSelected(0);
        }, (err) => {
            handleError(err);
        })
    }

    $scope.getEmployeeByKeyword = function (val) {
        if (val.length < 3) {
            return;
        }
        return inActiveEmployeeService.getEmployeeDropDownByKeyword(val).then(function (res) {
            return res.data;
        });
    };

    $scope.advanceEmployeeSearch = function (val) {
        if (val.length < 3) {
            if (val.length == 0)
                getRecentEmployees();
            return;
        }
        inActiveEmployeeService.getEmployeeDropDownByKeyword(val).then(function (res) {
            $scope.employeeList = res.data;
        });
    };

    function employeeSelect(item, model, label) {
        //$scope.employee.ReportToName = item.EmployeeName;
        $scope.getInActiveEmployeeeDetailsById(item);
    }
    $scope.employeeSelect = employeeSelect;


    $scope.getInActiveEmployeeeDetailsById = function (item) {
        inActiveEmployeeService.getInActiveEmployeeeDetailsById(item.EmployeeID).then(function (res) {
            console.log(res.data.length);
            if (res.data.length == 0) {
                $scope.inactiveEmployee.EmployeeID = item.EmployeeID;
            }
            else {

                $scope.inactiveEmployee = formatDate([res.data], "MM/DD/YYYY", ["ApplicableDate"])[0];//res.data;
                $scope.inactiveEmployee.CardNo = item.CardNo + " : " + item.EmployeeName;

                if ($scope.inactiveEmployee.InActiveType == 1) {
                    $scope.dismissedReason = true;
                    $scope.dropOutReason = false;
                }
                else {
                    $scope.dismissedReason = false;
                    $scope.dropOutReason = true;
                }
            }

        }, function (err) { handleHttpError(err); });
    }
    $scope.getReasonDiv = function (type) {
        console.log(type);
        if (type == 1) {
            $scope.dismissedReason = true;
            $scope.dropOutReason = false;
        }
        else {
            $scope.dismissedReason = false;
            $scope.dropOutReason = true;
        }
    }
    //getting index of selected employee
    function indexOfObjectInArray(array, property, value) {
        for (var i = 0; i < array.length; i++) {
            if (array[i][property] == value) {
                return i;
            }
        }
        return -1;
    }

    //date picker date bind with model
    $scope.bindDate = function bindDate(id, model) {
        $scope[model][id] = $('#' + id).val();
    };

}]);
