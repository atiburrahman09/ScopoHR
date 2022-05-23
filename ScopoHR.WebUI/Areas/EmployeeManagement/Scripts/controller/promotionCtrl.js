scopoAppControllers.controller('promotionCtrl', ['$scope', 'promotionService', 'alertify', function ($scope, promotionService, alertify) {

    $scope.employeePromotion = {};
    var prevIndex = null;
    var currentIndex = null;
    $scope.employeeList = [];
    $scope.employeePromotionData = [];

    $scope.init = function () {
        $scope.getRecentEmployees();
        $scope.getAllDepartments();
        //$scope.getDesignationList()
        //$scope.increment = {};
    }
    $scope.departmentList = [];
    $scope.getAllDepartments = function () {
        promotionService.getAllDepartments().then(function (res) {
            $scope.departmentList = res.data;
        }, function (err) {
            handleHttpError(err);
        });
    };
    $scope.getDesignationList = function (departmentID) {
        if (departmentID)
            promotionService.getDesignationList(departmentID).then(function (res) {
                $scope.designationList = res.data;
            },
            function (err) {
                handleHttpError(err);
            });
    }

    //$scope.getDesignationList = function (employeeID) {
    //    promotionService.getDesignationList(employeeID).then(function (res) {
    //            $scope.designationList = res.data;
    //        },
    //        function (err) {
    //            handleHttpError(err);
    //        });
    //}
    $scope.saveInformation = function () {
        $scope.employeePromotion.EmployeeID = $scope.employeeID;
        if ($scope.employeePromotion.EmployeeID !== null) {
            if ($scope.promotionForm.$valid) {
                promotionService.saveInformation($scope.employeePromotion).then(function (res) {
                    alertify.success(res.data);
                    $scope.employeePromotion = {};
                    $scope.promotionForm.$setPristine();
                    $scope.promotionForm.$setUntouched();
                    $scope.getEmployeeePromotionDetailsById($scope.employeeID);

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
        return promotionService.getEmployeeDropDownByKeyword(val).then(function (res) {
            return res.data;
        });
    };

    $scope.getRecentEmployees = function () {
        promotionService.getRecentEmployees().then(function (res) {
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
        $scope.getEmployeeePromotionDetailsById($scope.employeeID);
        //$scope.getDesignationList($scope.employeeID);
    };

    $scope.getEmployeeePromotionDetailsById = function (id) {
        //console.log(id);
        promotionService.getEmployeeePromotionDetailsById(id).then(function (res) {
            console.log(res.data);
            console.log(id);
            if (res.data.length > 0) {
                $scope.employeePromotionData = formatDate(res.data, "MM/DD/YYYY", ["PromotionDate"]);
            }
            else {
                $scope.employeePromotionData = [];
            }


        }, function (err) { handleHttpError(err); });
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

  


}]);
