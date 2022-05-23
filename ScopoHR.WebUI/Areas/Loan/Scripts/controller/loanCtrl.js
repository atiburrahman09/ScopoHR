scopoAppControllers.controller('loanCtrl', ['$scope', 'loanService', 'alertify', function ($scope, loanService, alertify) {

    $scope.loan = {};
    $scope.loanDetails = {};
    var prevIndex = null;
    var currentIndex = null;
    $scope.employeeList = [];
    $scope.loanDetailsList = [];
    $scope.loanList = [];
    $scope.loanDetailsDiv = false;

    
    $scope.monthList = [
           { MonthId: '1', MonthName: 'January' },
           { MonthId: '2', MonthName: 'February' },
           { MonthId: '3', MonthName: 'March' },
           { MonthId: '4', MonthName: 'April' },
           { MonthId: '5', MonthName: 'May' },
           { MonthId: '6', MonthName: 'June' },
           { MonthId: '7', MonthName: 'July' },
           { MonthId: '8', MonthName: 'August' },
           { MonthId: '9', MonthName: 'September' },
           { MonthId: '10', MonthName: 'October' },
           { MonthId: '11', MonthName: 'November' },
           { MonthId: '12', MonthName: 'December' },
    ];
    

    $scope.init = function () {
        //$scope.getRecentEmployees();
        //$scope.increment = {};
    }

 
    $scope.saveInformation = function () {
        $scope.loan.EmployeeID = $scope.employeeID;
        console.log($scope.loan);
       
        if ($scope.loan.EmployeeID != null) {
            if ($scope.loanForm.$valid)
            {
                loanService.saveInformation($scope.loan).then(function (res) {
                    alertify.success(res.data);
                    $scope.loan = {};
                    $scope.loanForm.$setPristine();
                    $scope.loanForm.$setUntouched();
                    $scope.getloanByEmployeeID($scope.employeeID);
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
        return loanService.getEmployeeDropDownByKeyword(val).then(function (res) {
            return res.data;
        });
    };

    $scope.getRecentEmployees = function () {
        loanService.getRecentEmployees().then(function (res) {
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
        $scope.getloanByEmployeeID($scope.employeeID);
    };

    $scope.loanSelected = function (l) {
        console.log(l);
        var index = indexOfObjectInArray($scope.loanList, 'LoanID', l.LoanID);
        $scope.loanList[index].selected = true;
        if (prevIndex != null && prevIndex != index) {
            $scope.loanList[prevIndex].selected = false;
        }
        prevIndex = index;
        $scope.getloanDetailsByLoanID(l.LoanID);
    };

    $scope.getloanDetailsByLoanID = function (loanID) {
        console.log(loanID);
        loanService.getloanDetailsByLoanID(loanID).then(function (res) {
            console.log(res.data);
            console.log(loanID);
            if (res.data.length > 0) {
                $scope.loanDetailsList = formatDate(res.data, "MM/DD/YYYY", ["RepaymentDate"]);
                $scope.loanDetailsDiv = true;
            }
            else {
                $scope.loanDetailsList = [];
                $scope.loanDetailsDiv = false;
            }


        }, function (err) { handleHttpError(err); });
    }

    $scope.getloanByEmployeeID = function (ID) {
        //console.log(id);
        loanService.getloanByEmployeeID(ID).then(function (res) {
            console.log(res.data);
            console.log(ID);
            if (res.data.length > 0)
            {
                $scope.loanList = formatDate(res.data, "MM/DD/YYYY", ["DisbursementDate", "LastModified"]);
            }
            else {
                $scope.loanList = [];
            }
            
            
        }, function (err) { handleHttpError(err); });
    }



    //$scope.generateLoanDetails = function (loan)
    //{
    //    console.log(loan);
    //    var loanAmount=loan.LoanAmount;
    //    var repaymentMonth = loan.StartsFrom;

    //    var repaymentDate = new Date("01-" + loan.StartsFrom + "-" + new Date(loan.DisbursementDate).getFullYear()).toString("MM/DD/YYYY");
    //    console.log(repaymentDate);

    //    for (var i = 0; i < loan.Duration; i++) {

    //        $scope.loanDetails = {
    //            LoanID: loan.LoanID,
    //            RepaymentAmount: Math.round(loan.LoanAmount / loan.Duration, 0),
    //            RepaymentMonth: repaymentMonth,
    //            RepaymentDate: repaymentDate
    //        };
    //        $scope.loanDetailsList.push(formatDate([$scope.loanDetails], "MM/DD/YYYY", ["RepaymentDate"])[0]);

    //        console.log($scope.loanDetails);

    //        loanAmount =loanAmount - Math.round(loan.LoanAmount / loan.Duration, 0);
    //        repaymentMonth++;
    //        repaymentDate = moment(repaymentDate).add(1, 'day').format("MM/DD/YYYY"); //moment(repaymentDate, "MM/DD/YYYY").add(1, 'months');
    //        console.log(repaymentDate);
    //    }
      
    //}

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
