scopoAppServices.service('fineService', function ($http) {

    let serviceBase = "/Loan/Fine/";


    this.saveInformation = function (info) {
        return $http.post(serviceBase+"SaveInformation", info);
    };

    this.getFineByEmployeeID = function (employeeID) {
        return $http.get(serviceBase + "GetFineByEmployeeID", { params: { employeeID: employeeID } });
    }


    this.getEmployeeDropDownByKeyword = function (inputString) {
        return $http.get(serviceBase+"GetEmployeeDropDownByKeyword", { params: { inputString: inputString } });
    };


    this.getRecentEmployees = function () {
        return $http.get(serviceBase + "GetRecentEmployees");
    }
});