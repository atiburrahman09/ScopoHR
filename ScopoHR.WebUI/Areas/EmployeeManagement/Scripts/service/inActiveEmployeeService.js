scopoAppServices.service('inActiveEmployeeService', function ($http) {

    let serviceBase = "/EmployeeManagement/InActiveEmployee/";


    this.saveInformation = function (info) {
        return $http.post("/EmployeeManagement/InActiveEmployee/SaveInformation", info);
    };

    this.getInActiveEmployeeeDetailsById = function (EmployeeID) {
        return $http.get("/EmployeeManagement/InActiveEmployee/GetInActiveEmployeeeDetailsById", { params: { EmployeeID: EmployeeID } });
    }

    this.getEmployeeDropDownByKeyword = function (inputString) {
        return $http.get("/EmployeeManagement/InActiveEmployee/GetEmployeeDropDownByKeyword", { params: { inputString: inputString } });
    };

   
    this.getRecentEmployees = function () {
        return $http.get(serviceBase + "GetRecentEmployees");
    }
});