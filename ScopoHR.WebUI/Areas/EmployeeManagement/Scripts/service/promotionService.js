scopoAppServices.service('promotionService', function ($http) {

    let serviceBase = "/EmployeeManagement/Promotion/";


    this.saveInformation = function (info) {
        return $http.post(serviceBase + "SaveInformation", info);
    };

    this.getEmployeeePromotionDetailsById = function (EmployeeID) {
        return $http.get(serviceBase + "GetEmployeeePromotionDetailsById", { params: { EmployeeID: EmployeeID } });
    }

    this.getEmployeeDropDownByKeyword = function (inputString) {
        return $http.get(serviceBase + "GetEmployeeDropDownByKeyword", { params: { inputString: inputString } });
    };


    this.getRecentEmployees = function () {
        return $http.get(serviceBase + "GetRecentEmployees");
    }

  
    this.savePromotion = function (info) {
        return $http.post(serviceBase + "SavePromotion", info);
    };

    //this.getDesignationList = function (employeeID) {
    //    return $http.get(serviceBase + "GetDesignationList", { params: {EmployeeID : employeeID}});
    //}


    this.getDesignationList = function (DepartmentID) {
        return $http.get("/EmployeeManagement/Promotion/GetDesignationListByDepartment", { params: { DepartmentID: DepartmentID } });
    };


    this.getAllDepartments = function () {
        return $http.get("/EmployeeManagement/Promotion/GetAllDepartments");
    }
});