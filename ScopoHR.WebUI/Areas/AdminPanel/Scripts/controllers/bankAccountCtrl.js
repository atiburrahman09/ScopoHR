scopoAppControllers.controller('bankAccountCtrl', ['$scope', 'bankAccountService', 'alertify', function ($scope, bankAccountService, alertify) {
    // vars
    $scope.account = {};
    var prevIndex = null;

    $scope.employeeList = [];
    $scope.accountList = [];

    $scope.bankList = [
        { BankName: 'Exim', BankName: 'Exim' },
        { BankName: 'Brac', BankName: 'Brac' }
    ];
    $scope.companyList = [
       { Company: 'Arunima', Company: 'Arunima' },
       { Company: 'DMC', Company: 'DMC' },
       { Company: 'BDC', Company: 'BDC' }
    ];

    $scope.init = function (name) {
        $scope.getAllBankAccount();
    }

    $scope.getAllBankAccount = function () {
        bankAccountService.getAllBankAccount().then(function (res) {
            $scope.accountList = res.data;
        }, function (err) {
            handleHttpError(err);
        })
    };
    // searching
    $scope.getEmployeeByKeyword = function (val) {
        if (val.length < 3) {
            return;
        }
        return bankAccountService.getEmployeeDropDownByKeyword(val).then(function (res) {
            return res.data;
        });
    };

    function employeeSelect(item, model, label) {
        $scope.account.CardNo = item.CardNo;
        $scope.account.EmployeeID = item.EmployeeID;
    }
    $scope.employeeSelect = employeeSelect;


    // emplyee selection
    $scope.selected = function (a) {

        var index = indexOfObjectInArray($scope.accountList, 'EmployeeID', a.EmployeeID);

        $scope.accountList[index].selected = true;
        if (prevIndex !== null && prevIndex !== index) {
            $scope.accountList[prevIndex].selected = false;
        }
        prevIndex = index;
        $scope.employeeID = $scope.accountList[index].EmployeeID;

        // load all necessary data from the server
        getEmployeeeBankAccountDetailsById($scope.employeeID);
    };


    // Get Employee Salary mapping after detail has been loaded
    let getEmployeeeBankAccountDetailsById = (id) => {
        bankAccountService.getEmployeeeBankAccountDetailsById(id).then(res=> {
            $scope.account = angular.copy(res.data);
            console.log($scope.account);
            console.log(res.data);
        }, err=> { handleHttpError(err); });
    }


    $scope.saveAccount = function (account) {
        if (!$scope.bankAccountForm.$valid) {
            alertify.error("Fill the require fields");
            return;
        }
        if (account.ID > 0) {
            bankAccountService.saveAccount(account).then(function (res) {
                alertify.success("Data Successfully Saved");
                $scope.account = {};
                $scope.bankAccountForm.$setPristine();
                $scope.bankAccountForm.$setUntouched();
            }, function (err) { handleHttpError(err) });
        }
        else {
            bankAccountService.IsUniqueAccount(account.EmployeeID,account.BankName).then(function (res) {
                if (res.status === 206)
                    alertify.error(res.data);
                else {
                    bankAccountService.saveAccount(account).then(function (res) {
                        alertify.success("Data Successfully Saved");
                        $scope.account = {};
                        $scope.bankAccountForm.$setPristine();
                        $scope.bankAccountForm.$setUntouched();
                    }, function (err) { handleHttpError(err) });
                }
            })
        }

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

}]);
