scopoAppServices.service('licenseService', function ($http) {

    this.saveLicense = function (license) {
        return $http.post("/Common/License/SaveLicense/", license);
    }

    this.getAllLicenses = function () {
        return $http.get("/Common/License/GetAllLicenses/");
    }
});