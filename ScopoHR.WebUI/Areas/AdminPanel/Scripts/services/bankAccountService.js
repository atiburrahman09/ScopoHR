scopoAppServices.service('bankAccountService', function ($http) {

    let serviceBase = "/AdminPanel/BankAccount/";

    
    this.saveAccount = (account) => {
        return $http.post(serviceBase + "SaveAccount", account);
    }
    this.getAllBankAccount = function () {
        return $http.get(serviceBase + "GetAllBankAccount");
    }

    this.getEmployeeDropDownByKeyword = function (inputString) {
        return $http.get(serviceBase + "GetEmployeeDropDownByKeyword", { params: { inputString: inputString } });
    }
    this.getEmployeeeBankAccountDetailsById = function (EmployeeID) {
        return $http.get(serviceBase + "GetEmployeeeBankAccountDetailsById", { params: { EmployeeID: EmployeeID } });
    }

    this.IsUniqueAccount = function (EmployeeID,Bank) {
        return $http.get(serviceBase + "IsUniqueAccount", { params: { EmployeeID: EmployeeID,Bank:Bank } });
    }
      
});