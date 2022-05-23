scopoAppServices.service('dailyATReportService', function ($http) {


    this.getDailyReport = function (dReport) {
        return $http.post("/Reports/DailyAttendance/DailyReportByDateFloor", dReport);
    };

  
    this.getAllProductionFloorLine = function () {
        return $http.get("/Reports/DailyAttendance/GetAllProductionFloor");
    }
    this.getWorkingShift = function () {
        return $http.get("/Reports/DailyAttendance/GetWorkingShift");
    }


});