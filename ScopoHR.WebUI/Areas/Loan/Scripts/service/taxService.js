scopoAppServices.service('taxService', function ($http) {

    let serviceBase = "/Loan/Tax/";


    this.saveInformation = function (info) {
        return $http.post(serviceBase+"SaveInformation", info);
    };

    this.getTaxByEmployeeID = function (employeeID) {
        return $http.get(serviceBase + "GetTaxByEmployeeID", { params: { employeeID: employeeID } });
    }


    this.getEmployeeDropDownByKeyword = function (inputString) {
        return $http.get(serviceBase+"GetEmployeeDropDownByKeyword", { params: { inputString: inputString } });
    };


    this.getRecentEmployees = function () {
        return $http.get(serviceBase + "GetRecentEmployees");
    }
});