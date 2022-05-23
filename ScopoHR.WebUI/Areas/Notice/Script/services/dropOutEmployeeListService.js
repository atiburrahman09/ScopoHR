scopoAppServices.service('dropOutEmployeeListService', function ($http) {


    this.getDropOutList = function (dReport) {
        return $http.post("/Notice/Notice/GetDropOutListReport", dReport);
    };

    this.sendNotice = function (data) {
        return $http.post("/Notice/Notice/SendNotice", data)
    }

    this.getAllNotice = function () {
        return $http.get("/Notice/Notice/GetAllNotice");
    };

    this.getAllProductionFloorLine = function () {
        return $http.get("/Notice/Notice/GetAllProductionFloorLine");
    }
    this.getWorkingShift = function () {
        return $http.get("/Notice/Notice/GetWorkingShift");
    }





});