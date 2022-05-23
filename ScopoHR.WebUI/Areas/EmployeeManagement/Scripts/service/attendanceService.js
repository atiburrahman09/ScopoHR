scopoAppServices.service('attendanceService', function ($http) {

    var serviceBase = "/EmployeeManagement/Attendance/";


    //get All Attendances
    this.getAttendances = function () {
        return $http.get( serviceBase + "GetAllAttendanceList");
    };

    // get Attendance by AttendanceID
    this.getAttendance = function (AtID) {
        return $http.get("/EmployeeManagement/Attendance/GetAttendanceByID", { params: { AttendanceID: AtID } });
    }

    // Update Attendance 
    this.updateAttendance = function (attendance) {
        return $http.post("/EmployeeManagement/Attendance/UpdateAttendance", attendance);
    }

    this.saveAttendance = function (attendance) {
        return $http.post("/EmployeeManagement/Attendance/SaveAttendance/", attendance);
    }

    this.getAllDepartments = function () {
        return $http.get("/EmployeeManagement/Attendance/GetAllDepartments");
    };

    this.getAllEmployees = function () {
        return $http.get("/EmployeeManagement/Attendance/GetAllEmployees");
    };
    this.getAllProductionFloor = function () {
        return $http.get("/EmployeeManagement/Attendance/GetAllProductionFloor");
    }

    this.getEmployeeDropDownByKeyword = function (inputString) {
        return $http.get("/EmployeeManagement/Attendance/GetEmployeeDropDownByKeyword", { params: { inputString: inputString } });
    }
  
    this.updateDailyAttendance = function (attList) {
        return $http.post("/EmployeeManagement/Attendance/UpdateDailyAttendance", attList);
    }

    this.getDailyAttendance = function (search) {
        return $http.post("/EmployeeManagement/Attendance/SearchAttendance", search);
    }

    this.getEmployeeInfo = function (cardNo) {
        return $http.get("/EmployeeManagement/Attendance/GetEmployeeInfo", { params: { cardNo: cardNo } });
    };

    this.deleteAttendance = function (attendance) {
        return $http.post("/EmployeeManagement/Attendance/DeleteAttendance", attendance);
    }
});