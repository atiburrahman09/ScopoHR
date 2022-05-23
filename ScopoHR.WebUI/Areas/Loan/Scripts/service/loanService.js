scopoAppServices.service('loanService', function ($http) {

    let serviceBase = "/Loan/Loan/";


    this.saveInformation = function (info) {
        return $http.post(serviceBase+"SaveInformation", info);
    };

    this.getloanByEmployeeID = function (employeeID) {
        return $http.get(serviceBase + "GetLoanByEmployeeID", { params: { employeeID: employeeID } });
    }

    this.getloanDetailsByLoanID = function (loanID) {
        console.log(loanID);
        return $http.get(serviceBase + "GetLoanDetailsByLoanID", { params: { loanID: loanID } });
    }

    this.getEmployeeDropDownByKeyword = function (inputString) {
        return $http.get(serviceBase+"GetEmployeeDropDownByKeyword", { params: { inputString: inputString } });
    };


    this.getRecentEmployees = function () {
        return $http.get(serviceBase + "GetRecentEmployees");
    }
});