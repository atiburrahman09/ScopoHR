scopoAppControllers.controller('salaryIncrementCtrl', ['$scope', 'salaryIncrementService', 'alertify', function ($scope, salaryIncrementService, alertify) {

    $scope.employeeIncrement = {};
    $scope.increment = {};
    $scope.incrementList = [];
    $scope.list = [];
    $scope.productionFloorList = [];
    $scope.workingShiftList = [];
    var prevIndex = null;
    var currentIndex = null;
    $scope.employeeList = [];
    $scope.employeeIncrementData = [];

    $scope.employeeType = [
          { EmployeeTypeName: 'Worker', EmployeeTypeID: 1 },
          { EmployeeTypeName: 'Staff', EmployeeTypeID: 2 }
    ];
    

    $scope.init = function () {
        $scope.getRecentEmployees();
        $scope.GetProductionFloor();
        $scope.getWorkingShift();
        //$scope.increment = {};
    }

    $scope.GetProductionFloor = function () {
        salaryIncrementService.getAllProductionFloorLine().then(function (res) {
            console.log(res.data);
            $scope.productionFloorList = res.data;
        }, function (err) {
            handleHttpError(err);
        })
    };
    $scope.getWorkingShift = function () {
        salaryIncrementService.getWorkingShift().then(function (res) {
            $scope.workingShiftList = res.data;
        }, function (err) {
            handleHttpError(err);
        })
    };

    $scope.getSalaryIncrement = function ()
    {
        salaryIncrementService.getSalaryIncrement($scope.increment).then(function (res) {
            $scope.incrementList = formatDate(res.data, "YYYY-MM-DD", ["JoiningDate", "PreviousIncrementDate"]);
            console.log(res.data.length);
        }, function (err) {
            handleHttpError(err);
        })
    }

 
    $scope.saveInformation = function () {
        $scope.employeeIncrement.EmployeeID = $scope.employeeID;
        console.log($scope.employeeIncrement);
        if ($scope.employeeIncrement.EmployeeID !== null) {
            if ($scope.salaryIncrementForm.$valid)
            {
                salaryIncrementService.saveInformation($scope.employeeIncrement).then(function (res) {
                    alertify.success(res.data);
                    $scope.employeeIncrement = {};
                    $scope.salaryIncrementForm.$setPristine();
                    $scope.salaryIncrementForm.$setUntouched();
                    $scope.getEmployeeeSalaryIncrementDetailsById($scope.employeeID);
                    
                }, function (err) {
                    handleHttpError(err);
                });
            }
            else {
                alertify.error("Please fill required information's.");
            }
           
        }
        else {
            alertify.error("Please select employee");
        }
    }

    // searching
    $scope.getEmployeeByKeyword = function (val) {
        if (val.length < 3) {
            return;
        }
        return salaryIncrementService.getEmployeeDropDownByKeyword(val).then(function (res) {
            return res.data;
        });
    };

    $scope.getRecentEmployees = function () {
        salaryIncrementService.getRecentEmployees().then(function (res) {
            $scope.employeeList = res.data;
        }, function (err) {
            handleHttpError(err);
        })
    }
    // emplyee selection
    $scope.employeeSelected = function (e) {
        //var index = indexOfObjectInArray($scope.employeeList, 'EmployeeName', e.EmployeeName);
        //currentIndex = index;
        //$scope.employeeList[index].selected = true;
        //if (prevIndex != null && prevIndex != index) {
        //    $scope.employeeList[prevIndex].selected = false;
        //}
        //prevIndex = index;
        //$scope.employeeID = $scope.employeeList[index].EmployeeID;
        //$scope.cardNo = $scope.employeeList[index].EmployeeCardNo;
        console.log(e);
        $scope.employeeID = e.EmployeeID;
        $scope.cardNo = e.CardNo;
        $scope.getEmployeeeSalaryIncrementDetailsById($scope.employeeID);
    };

    $scope.getEmployeeeSalaryIncrementDetailsById = function (id) {
        //console.log(id);
        salaryIncrementService.getEmployeeeSalaryIncrementDetailsById(id).then(function (res) {
            console.log(res.data);
            console.log(id);
            if (res.data.length > 0)
            {
                $scope.employeeIncrementData = formatDate(res.data, "MM/DD/YYYY", ["IncrementDate","LastModified"]);
            }
            else {
                $scope.employeeIncrementData = [];
            }
            
            
        }, function (err) { handleHttpError(err); });
    }


    $scope.saveSalaryIncrement = function ()
    {
        for (var i = 0; i < $scope.incrementList.length; i++)
        {
            if ($scope.incrementList[i].Selected) {
                $scope.list.push($scope.incrementList[i]);
            }
        }
        //angular.forEach($scope.incrementList, function (item) {
        //    //item.Selected = $scope.selectedAll;.
            
        //});

        salaryIncrementService.saveSalaryIncrement($scope.list).then(function (res) {
            alertify.success("Data Successfully saved.");
            $scope.list = [];
            $scope.incrementList = [];
            $scope.increment = {};
            $scope.selectedAll = false;
            $scope.salaryIncrementForm.$setPristine();
            $scope.salaryIncrementForm.$setUntouched();

        }, function (err) {
            handleHttpError(err);
        })


        //console.log($scope.list);
        //console.log($scope.list.length);
    }






    //getting index of selected employee
    function indexOfObjectInArray(array, property, value) {
        for (var i = 0; i < array.length; i++) {
            if (array[i][property] === value) {
                return i;
            }
        }
        return -1;
    }

    $scope.bind = function bind(id, model) {
        $scope[model][id] = $('#' + id).val();
        //$scope.increment.IncrementDate = $('#' + id).val();
        //console.log($scope[model][id]);
    };

    function bindDate(id, model) {
        console.log(id, model);
        $scope[model][id] = $('#' + id).val();
    }
    $scope.bindDate = bindDate;

    $scope.bindDateWithIndex = function bindDate(id, model, index) {
        $scope.incrementList[index][id] = $('#' + id).val();
    };


    $scope.selectAll = function () {
        angular.forEach($scope.incrementList, function (item) {
            item.Selected = $scope.selectedAll;
        });
    };

    $scope.checkIfAllSelected = function () {
        $scope.selectedAll = $scope.incrementList.every(function (item) {
            return item.Selected === true
        })
    };


}]);
