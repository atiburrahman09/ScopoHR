scopoAppServices.service('prescriptionService', function ($http) {



    this.getRecentPrescriptionList = function () {
        return $http.get("/Common/Prescription/GetRecentPrescriptionList");
    }
    this.getEmployeeDropDownByKeyword = function (inputString) {
        return $http.get("/Common/Prescription/GetEmployeeDropDownByKeyword", { params: { inputString: inputString } });
    }

    this.createUpdate = function (prescription) {
        return $http.post("/Common/Prescription/CreateUpdateNotice", prescription);
    }

    this.getPrescriptionDropDownByKeyword = function (inputString) {
        return $http.get("/Common/Prescription/GetPrescriptionDropDownByKeyword", { params: { inputString: inputString } });
    };


});