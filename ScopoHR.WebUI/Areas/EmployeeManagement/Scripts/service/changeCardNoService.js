scopoAppServices.service('changeCardNoService', function ($http) {

    let serviceBase = "/EmployeeManagement/Employees/";

   
    this.getEmployeeeDetailsById = function (EmployeeID) {
        return $http.get("/EmployeeManagement/Employees/GetEmployeeInfoByID", { params: { EmployeeID: EmployeeID } });
    }

   

    this.getEmployeeDropDownByKeyword = function (inputString) {
        return $http.get("/EmployeeManagement/Employees/GetEmployeeDropDownByKeyword", { params: { inputString: inputString } });
    };

    this.isUniqueCardNo = function (cardNo) {
        return $http.get("/EmployeeManagement/Employees/IsUniqueCardNo/", { params: { cardNo: cardNo } });
    }
  
    this.getRecentEmployees = () => {
        return $http.get(serviceBase + "GetRecentEmployees");
    }

    this.getEmployeeDocumentsById = (id) => {
        return $http.get(serviceBase + "GetEmployeeDocumentsById?id=" + id);
    }

    this.changeCardNo = function (employee) {
        return $http.post("/EmployeeManagement/Employees/ModifyCardNo/", employee);
    }
});