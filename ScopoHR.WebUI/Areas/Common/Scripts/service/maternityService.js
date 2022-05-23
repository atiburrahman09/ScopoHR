scopoAppServices.service('maternityService', function ($http) {


    let serviceBase = "/Common/Maternity/";

    this.saveMaternity = function (maternity) {
        return $http.post("/Common/Maternity/SaveMaternity/", maternity);
    }

    this.getAllMaternities = function () {
        return $http.get("/Common/Maternity/GetAllMaternities/");
    }


    this.getEmployeeeMaternityDetailsById = function (EmployeeID) {
        return $http.get(serviceBase + "GetEmployeeeMaternityDetailsById", { params: { EmployeeID: EmployeeID } });
    }

    this.getEmployeeDropDownByKeyword = function (inputString) {
        return $http.get(serviceBase + "GetEmployeeDropDownByKeyword", { params: { inputString: inputString } });
    };


    this.getRecentEmployees = function () {
        return $http.get(serviceBase + "GetRecentEmployees");
    }
});