scopoAppServices.service('medicineService', function ($http) {

    this.saveMedicine = function (medicine) {
        return $http.post("/Common/Medicine/SaveMedicine/", medicine);
    }

    this.getAllMedicines = function () {
        return $http.get("/Common/Medicine/GetAllMedicines/");
    }

    this.isUnique = function (medicine) {
        return $http.post("/Common/Medicine/IsUnique/", medicine);
    }
});