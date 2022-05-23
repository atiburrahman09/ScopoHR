scopoAppControllers.controller(
    'changeCardNoCtrl', ['$scope', 'changeCardNoService', 'alertify',
    function ($scope, changeCardNoService, alertify
        ) {
        // vars
        $scope.employee = {};
        var prevIndex = null;
        $scope.employeeList = [];
        $scope.init = function () {
            getRecentEmployees();
        }


        // searching
        $scope.getEmployeeByKeyword = function (val) {
            if (val.length < 3) {
                return;
            }
            return changeCardNoService.getEmployeeDropDownByKeyword(val).then(function (res) {
                return res.data;
            });
        };

        $scope.advanceEmployeeSearch = function (val) {
            if (val.length < 3) {
                if (val.length == 0)
                    getRecentEmployees();
                return;
            }
            changeCardNoService.getEmployeeDropDownByKeyword(val).then(function (res) {
                $scope.isRecent = false;
                $scope.employeeList = res.data;
            });
        };

        let getRecentEmployees = () => {
            changeCardNoService.getRecentEmployees().then((res) => {
                $scope.isRecent = true;
                $scope.employeeList = res.data;
                //$scope.employeeList[0].selected = true;
                //$scope.employeeSelected(0);
            }, (err) => {
                handleError(err);
            })
        }

        $scope.employeeSelected = function (e) {
            var index = indexOfObjectInArray($scope.employeeList, 'EmployeeName', e.EmployeeName);

            $scope.employeeList[index].selected = true;
            if (prevIndex != null && prevIndex != index) {
                $scope.employeeList[prevIndex].selected = false;
            }
            prevIndex = index;
            $scope.employeeID = $scope.employeeList[index].EmployeeID;
            $scope.employee.OldCardNo = $scope.employeeList[index].CardNo;

        };

        $scope.changeCardNo = function () {
           
            if ($scope.changeCardNoForm.$valid) {
                changeCardNoService.isUniqueCardNo($scope.employee.NewCardNo).then(function (res) {
                    if (res.status === 206) {
                        alertify.error(res.data);
                    }
                    else {
                        changeCardNoService.changeCardNo($scope.employee).then(function (res) {
                            console.log(res);
                            if (res.data == true) {
                                $scope.resetForm();
                                alertify.success("Card no successfully modified.");
                            }
                        }, function (err) {
                            handleHttpError(err);
                        })
                    }
                });
            }
            else {
                alertify.error("Please fill the required information's.");
            }
           
        };


        $scope.resetForm = function ()
        {
            $scope.employee = {};
            $scope.changeCardNoForm.$setPristine();
            $scope.changeCardNoForm.$setUntouched();
            if (prevIndex != null) {
                $scope.employeeList[prevIndex].selected = false;
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

    }]);
