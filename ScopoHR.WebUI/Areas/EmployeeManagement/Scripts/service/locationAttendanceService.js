scopoAppServices.service('locationAttendanceService', function ($http) {

    let serviceBase = "/EmployeeManagement/Attendance/";


    this.saveInformation = function (at) {
        return $http.post(serviceBase + "SaveInformation", at);
    };   

    this.getBusLocation = function () {
        return $http.get(serviceBase + "GetBusLocation");
    }

    this.getRecentEmployees = function () {
        return $http.get(serviceBase + "GetRecentEmployees");
    }

    this.getEmployeeDropDownByKeyword = function (val) {
        return $http.get(serviceBase + "GetEmployeeDropDownByKeyword", { params: {val:val}});
    }

    this.getAllEmployees = function () {
        return $http.get(serviceBase + "GetAllEmployees");
    }

    this.getEmpList = function (locationID) {
        return $http.get(serviceBase + "GetEmpList", { params: {locationID:locationID}});
    }

    this.saveList = function (at) {
        return $http.post(serviceBase + "SaveList", at);
    };
});