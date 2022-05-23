scopoAppServices.service('salaryIncrementService', function ($http) {

    let serviceBase = "/EmployeeManagement/IncrementSalary/";


    this.saveInformation = function (info) {
        return $http.post(serviceBase+"SaveInformation", info);
    };

    this.getEmployeeeSalaryIncrementDetailsById = function (EmployeeID) {
        return $http.get(serviceBase + "GetEmployeeeSalaryIncrementDetailsById", { params: { EmployeeID: EmployeeID } });
    }

    this.getEmployeeDropDownByKeyword = function (inputString) {
        return $http.get(serviceBase+"GetEmployeeDropDownByKeyword", { params: { inputString: inputString } });
    };


    this.getRecentEmployees = function () {
        return $http.get(serviceBase + "GetRecentEmployees");
    }

    this.getAllProductionFloorLine = function () {
        return $http.get("/EmployeeManagement/IncrementSalary/GetAllProductionFloor");
    }
    this.getWorkingShift = function () {
        return $http.get("/EmployeeManagement/IncrementSalary/GetWorkingShift");
    }

    this.getSalaryIncrement = function (increment) {
        return $http.post("/EmployeeManagement/IncrementSalary/GetSalaryIncrement",increment);
    }
    this.saveSalaryIncrement = function (info) {
        return $http.post(serviceBase + "SaveSalaryIncrement", info);
    };
});