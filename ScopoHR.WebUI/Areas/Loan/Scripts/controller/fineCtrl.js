scopoAppControllers.controller('fineCtrl', ['$scope', 'fineService', 'alertify', function ($scope, fineService, alertify) {

    $scope.fine = {};
    var prevIndex = null;
    var currentIndex = null;
    $scope.employeeList = [];
    $scope.fineList = [];

    


    $scope.init = function () {
    }

 
    $scope.saveInformation = function () {
        $scope.fine.EmployeeID = $scope.employeeID;
        console.log($scope.fine);
       
        if ($scope.fine.EmployeeID != null) {
            if ($scope.fineForm.$valid)
            {
                fineService.saveInformation($scope.fine).then(function (res) {
                    console.log(res);
                    alertify.success(res.data);
                    $scope.fine = {};
                    $scope.fineForm.$setPristine();
                    $scope.fineForm.$setUntouched();
                    $scope.getFineByEmployeeID($scope.employeeID);
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
        return fineService.getEmployeeDropDownByKeyword(val).then(function (res) {
            return res.data;
        });
    };

    $scope.getRecentEmployees = function () {
        fineService.getRecentEmployees().then(function (res) {
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
        $scope.getFineByEmployeeID($scope.employeeID);
    };

    //$scope.taxSelected = function (t) {
    //    console.log(t);
    //    var index = indexOfObjectInArray($scope.taxList, 'TaxID', t.TaxID);
    //    $scope.taxList[index].selected = true;
    //    if (prevIndex != null && prevIndex != index) {
    //        $scope.taxList[prevIndex].selected = false;
    //    }
    //    prevIndex = index;

    //    $scope.tax = angular.copy(t);
    //};


    $scope.getFineByEmployeeID = function (ID) {
        //console.log(id);
        fineService.getFineByEmployeeID(ID).then(function (res) {
            console.log(res.data);
            console.log(ID);
            if (res.data.length > 0)
            {
                $scope.fineList = formatDate(res.data, "MM/DD/YYYY", ["Date","LastModified"])
            }
            else {
                $scope.fineList = [];
            }
            
            
        }, function (err) { handleHttpError(err); });
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

    $scope.bind = function bind(id, model) {
        $scope[model][id] = $('#' + id).val();
    };


}]);
