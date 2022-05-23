scopoAppServices.service('overTimeReportService', function ($http) {


    this.getOverTimeReport = function (dReport) {
        return $http.post("/Reports/OverTimeReport/GetOverTimeReport", dReport);
    };


    this.getAllProductionFloorLine = function () {
        return $http.get("/Reports/OverTimeReport/GetAllProductionFloor");
    }


});