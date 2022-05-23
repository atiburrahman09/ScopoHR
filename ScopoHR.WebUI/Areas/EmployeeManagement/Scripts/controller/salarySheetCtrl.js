scopoAppControllers.controller('salarySheetCtrl', ['$scope', 'salarySheetService', 'alertify', function ($scope, salarySheetService, alertify) {

    $scope.salaryList = [];
    $scope.yearList = [];
    $scope.monthList = [];
    $scope.departmentList = [];
    $scope.productionLineList = [];
    $scope.salary = {
        MonthId: 0,
        YearId: "",
        DepartmentID : 0,
        FloorID : ""
    }


    $scope.initial = function () {
        //$scope.GetAllSalary();
        $scope.GetAllDepartments();
        $scope.GetProductionFloor();
        $scope.GetYear();
        $scope.GetMonth();
    }

    $scope.GetProductionFloor = function () {

        salarySheetService.getAllProductionFloor().then(function (res) {
            $scope.productionFloorList = res.data;
        }, function (err) {
            handleHttpError(err);
        })


    };

    $scope.GetAllDepartments = function () {
        salarySheetService.getAllDepartments().then(function (res) {
            $scope.departmentList = res.data.data;
        }, function (err) {
            handleHttpError(err);
        });
    };

    $scope.GetYear = function () {
        salarySheetService.getYear().then(function (res) {
            $scope.yearList = res.data.data;
            //console.log($scope.salaryList);
        },
        function (err) {
            handleHttpError(err);
        });

    };

    $scope.GetMonth = function () {
        salarySheetService.getMonth().then(function (res) {
            $scope.monthList = res.data.data;
            //console.log($scope.salaryList);
        },
        function (err) {
            handleHttpError(err);
        });

    };


    $scope.searchSalary = function (salary) {
        if ($scope.salaryForm.$valid) {
            if ($scope.salary.DepartmentID == null)
            {
                $scope.salary.DepartmentID = 0;
            }
            salarySheetService.getSalaryByMonthYear(salary).then(function (res) {
                if (res.data.data.length > 0)
                {
                    $scope.salaryList = res.data.data;
                }
                else {
                    $scope.salaryList = res.data.data;
                    alertify.error("No Data Found");
                }
            },
            function (err) {
                handleHttpError(err);
            });
        }
        else {
            alertify.error("Please Fill Up The Informations!!!");
        }

    };
}]);