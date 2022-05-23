scopoAppControllers.controller('maternityCtrl', ['$scope', 'maternityService', 'alertify', function ($scope, maternityService, alertify) {
    var currentIndex = null;
    var previousIndex = null;
    $scope.maternityList = [];
    $scope.employeeList = [];
    $scope.maternity = {};
    $scope.employeeMaternityData = [];

    $scope.init = function () {
        $scope.getRecentEmployees();
    }

    // searching
    $scope.getEmployeeByKeyword = function (val) {
        if (val.length < 3) {
            return;
        }
        return maternityService.getEmployeeDropDownByKeyword(val).then(function (res) {
            return res.data;
        });
    };

    $scope.getRecentEmployees =function () {
        maternityService.getRecentEmployees().then(function (res) {
            $scope.employeeList = res.data;
        }, function (err) {
            handleHttpError(err);
        })
    }
    // emplyee selection
    $scope.employeeSelected = function (e) {
        console.log(e);
        $scope.employeeID = e.EmployeeID;
        $scope.cardNo = e.CardNo;
        $scope.getEmployeeeMaternityDetailsById($scope.employeeID);
    };

    $scope.getEmployeeeMaternityDetailsById = function (id) {
        //console.log(id);
        maternityService.getEmployeeeMaternityDetailsById(id).then(function (res) {
            console.log(res.data);
            console.log(id);
            if (res.data.length > 0) {
                $scope.employeeMaternityData = formatDate(res.data, "MM/DD/YYYY", ["FirstInstallmentDate", "FirstPaymentDate", "FirstRequisitionDate", "FirstReceivedDate", "MaternityLeaveDate"
                , "SecondInstallmentDate", "SecondPaymentDate", "SecondRequisitionDate", "SecondReceivedDate","Appx_DelivaryDate"]);
            }
            else {
                $scope.employeeMaternityData = [];
            }


        }, function (err) { handleHttpError(err); });
    }
   

    function saveMaternity() {
        $scope.maternity.EmployeeID = $scope.employeeID;
        if ($scope.employeeID == null || $scope.employeeID == undefined) {
            alertify.error("Please select employee.");
        }
        else {
            if ($scope.maternityForm.$valid) {
                maternityService.saveMaternity($scope.maternity).then(function (res) {
                    console.log(res);
                    if (res.data.ErrorCode == false) {
                        alertify.error(res.data.Message);
                    } else {
                        alertify.success(res.data.Message);
                        $scope.getEmployeeeMaternityDetailsById($scope.employeeID);
                        resetForm();
                    }

                });
            }
        }
    }

    $scope.lastSelectedMaternity = 0;
    $scope.selectedMaternity = function selectedMaternity(index) {
        console.log(index);

        $scope.employeeMaternityData[$scope.lastSelectedMaternity].selected = false;
        $scope.lastSelectedMaternity = index;
        $scope.employeeMaternityData[$scope.lastSelectedMaternity].selected = true;
        angular.copy($scope.employeeMaternityData[$scope.lastSelectedMaternity], $scope.maternity);
    }

   


    function resetForm() {
        if (currentIndex != null) {
            currentIndex = null;
            previousIndex = null;
        }
        $scope.maternity = {};
        $scope.maternityForm.$setPristine();
        $scope.maternityForm.$setUntouched();
    }

    function indexOfObjectInArray(array, property, value) {
        for (var i = 0; i < array.length; i++) {
            if (array[i][property] == value) {
                return i;
            }
        }
        return -1;
    }


    $scope.saveMaternity = saveMaternity;

    $scope.resetForm = resetForm;

    $scope.bind = function bind(id, model) {
        $scope[model][id] = $('#' + id).val();
    };
}])