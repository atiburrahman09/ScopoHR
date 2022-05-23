scopoAppControllers.controller('salaryCalculationController', ['$scope', 'salaryCalculationService', function ($scope, salaryCalculationService) {
    $scope.salary = {};

    $scope.init = function () {
        $scope.workShifts = [
         { ShiftName: 'Day', ShiftId: 1 },
         { ShiftName: 'Night', ShiftId: 2 }
        ];
        $scope.employeeType = [
           { EmployeeTypeName: 'Worker', EmployeeTypeID: 1 },
           { EmployeeTypeName: 'Staff', EmployeeTypeID: 2 }
        ];
    }

    $scope.calculateSalary = function () {
        if ($scope.salaryCalculationForm.$valid) {

            salaryCalculationService.generateSalary($scope.salary).then(function (res) {
                var type;
                var shift;
                if ($scope.salary.EmployeeType == 1) {
                    type = "Worker";
                }
                else {
                    type = "Staff";
                }
                if ($scope.salary.ShiftId == 1) {
                    shift = "Day";
                }
                else {
                    shift = "Night";
                }
                alertify.success("Salary generated for " + shift + " Shift and for " + type + ".");
            }, function (err) {
                handleHttpError(err);
            });
        }
        else {
            alertify.error("Invalid form submission. Please select year and month.");
        }
    };

    $scope.bindDate = function bindDate(id, model) {
        $scope[model][id] = $('#' + id).val();
    };
}]);