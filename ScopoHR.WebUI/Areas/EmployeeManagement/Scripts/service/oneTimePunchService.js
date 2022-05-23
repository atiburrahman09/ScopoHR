scopoAppServices.service('oneTimePunchService', function ($http) {

    var serviceBase = "/EmployeeManagement/OneTimePunch/";

    // Update Attendance 
    this.updateAttendance = function (attendance) {
        return $http.post(serviceBase + "SaveAttendance", attendance);
    }

    
    this.getAllProductionFloor = function () {
        return $http.get(serviceBase + "GetAllProductionFloor");
    }

    this.getDailyAttendance = function (search) {
        return $http.post(serviceBase + "SearchAttendance", search);
    }

    this.getShift = function () {
        return $http.post(serviceBase + "GetShift");
    }

   
});