scopoAppServices.service('salarySheetService', function ($http) {

    this.getYear = function () {
        return $http.get("/EmployeeManagement/Salary/GetYear");
    };
    this.getMonth = function () {
        return $http.get("/EmployeeManagement/Salary/GetMonth");
    };

    this.getSalaryByMonthYear = function (salary) {
        return $http.get("/EmployeeManagement/Salary/GetSalaryByMonthYear", { params: { MonthId: salary.MonthId, YearId: salary.YearId , DepartmentID : salary.DepartmentID , FloorID : salary.FloorID} });
    };

    this.getAllDepartments = function () {
        return $http.get("/EmployeeManagement/Salary/GetAllDepartments");
    };

    this.getAllProductionFloor = function () {
        return $http.get("/EmployeeManagement/Salary/GetAllProductionFloor");
    }


});