scopoAppServices.service('employeeService', function ($http) {

    let serviceBase = "/EmployeeManagement/Employees/";

    this.assignSalary = function (salaryMappingVM) {        
        return $http.post("/EmployeeManagement/Employees/AssignSalary/", salaryMappingVM);
    };

    this.saveEmployee = function (employee) {
        return $http.post("/EmployeeManagement/Employees/SaveEmployee", employee);
    };
    
    this.getEmployeeeDetailsById = function (EmployeeID) {
        return $http.get("/EmployeeManagement/Employees/GetEmployeeInfoByID", { params: { EmployeeID: EmployeeID } });
    }
    
    this.getAssignSalaryInfo = function (EmployeeID) {
        return $http.get("/EmployeeManagement/Employees/GetAssignSalaryInfoByID", { params: { EmployeeID: EmployeeID } });
    }    

    this.getAllProductionFloor = function () {
        return $http.get("/EmployeeManagement/Employees/GetAllProductionFloor");
    }

    this.getAllDepartments = function () {
        return $http.get("/EmployeeManagement/Employees/GetAllDepartments");
    }

    this.createSalaryMapping = function (salaryMappingList) {        
        return $http.post("/EmployeeManagement/Employees/createSalaryMapping", salaryMappingList);
    }

    this.getEmployeeDropDownByKeyword = function (inputString) {
        return $http.get("/EmployeeManagement/Employees/GetEmployeeDropDownByKeyword", { params: { inputString: inputString } });
    };

    this.getDesignationList = function (DepartmentID) {
        return $http.get("/EmployeeManagement/Employees/GetDesignationListByDepartment", { params: { DepartmentID: DepartmentID } });
    };

    this.isUniqueCardNo = function (cardNo) {        
        return $http.get("/EmployeeManagement/Employees/IsUniqueCardNo/", { params: { cardNo: cardNo } });
    }
    
    this.getLeaveMapping = (empId) => {
        return $http.get(serviceBase + "GetLeaveMappingByEmployeeId", { params: { id: empId } });
    }

    this.saveLeaveMapping = (mapping) => {
        return $http.post(serviceBase + "SaveLeaveMapping", mapping);
    }

    this.getRecentEmployees = () => {
        return $http.get(serviceBase + "GetRecentEmployees");
    }

    this.getEmployeeDocumentsById = (id) => {
        return $http.get(serviceBase + "GetEmployeeDocumentsById?id="+id);
    }

    this.getSectionList = function () {
        return $http.get("/EmployeeManagement/Employees/GetSectionList");
    }
});