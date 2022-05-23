scopoAppControllers.controller('taxCtrl', ['$scope', 'taxService', 'alertify', function ($scope, taxService, alertify) {

    $scope.tax = {};
    var prevIndex = null;
    var currentIndex = null;
    $scope.employeeList = [];
    $scope.taxList = [];

    
    $scope.yearList = [
           { YearId: 2017, Year: '2017' },
           { YearId: 2018, Year: '2018' },
           { YearId: 2019, Year: '2019' },
           { YearId: 2020, Year: '2020' },
           { YearId: 2021, Year: '2021' },
           { YearId: 2022, Year: '2022' },
           { YearId: 2023, Year: '2023' },
           { YearId: 2024, Year: '2024' },
           { YearId: 2025, Year: '2025' },
    ];
    

    $scope.init = function () {
    }

 
    $scope.saveInformation = function () {
        $scope.tax.EmployeeID = $scope.employeeID;
        console.log($scope.tax);
       
        if ($scope.tax.EmployeeID != null) {
            if ($scope.taxForm.$valid)
            {
                taxService.saveInformation($scope.tax).then(function (res) {
                    console.log(res);
                    if (res.data.res == false) {
                        alertify.error(res.data.MSG);
                    }
                    else {
                        alertify.success(res.data);
                    }

                    
                    $scope.tax = {};
                    $scope.taxForm.$setPristine();
                    $scope.taxForm.$setUntouched();
                    $scope.getTaxByEmployeeID($scope.employeeID);
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
        return taxService.getEmployeeDropDownByKeyword(val).then(function (res) {
            return res.data;
        });
    };

    $scope.getRecentEmployees = function () {
        taxService.getRecentEmployees().then(function (res) {
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
        $scope.getTaxByEmployeeID($scope.employeeID);
    };

    $scope.taxSelected = function (t) {
        console.log(t);
        var index = indexOfObjectInArray($scope.taxList, 'TaxID', t.TaxID);
        $scope.taxList[index].selected = true;
        if (prevIndex != null && prevIndex != index) {
            $scope.taxList[prevIndex].selected = false;
        }
        prevIndex = index;

        $scope.tax = angular.copy(t);
        $scope.tax.YearlyAmount = $scope.tax.Amount * 12;
    };

    $scope.getMonthlyAmount = function (amount)
    {
        $scope.tax.Amount = Math.round(amount / 12, 0);
    }


    $scope.getTaxByEmployeeID = function (ID) {
        //console.log(id);
        taxService.getTaxByEmployeeID(ID).then(function (res) {
            console.log(res.data);
            console.log(ID);
            if (res.data.length > 0)
            {
                $scope.taxList = formatDate(res.data, "MM/DD/YYYY", ["LastModified"]);

            }
            else {
                $scope.taxList = [];
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
        //$scope.increment.IncrementDate = $('#' + id).val();
        //console.log($scope[model][id]);
    };


}]);
