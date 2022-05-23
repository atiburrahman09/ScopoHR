scopoAppControllers.controller('medicineCtrl', ['$scope', 'medicineService', 'alertify', function ($scope, medicineService, alertify) {
    var currentIndex = null;
    var previousIndex = null;
    $scope.medicineList = [];
    $scope.medicine = {};

    $scope.init = function () {
        $scope.getAllMedicines();
    }


    $scope.getAllMedicines = function () {
        medicineService.getAllMedicines().then(function (res) {
            $scope.medicineList = res.data;
        });
    }


    function saveMedicine() {
        if ($scope.medicineForm.$valid) {
            medicineService.isUnique($scope.medicine).then(function (res) {
                if (res.data) {
                    medicineService.saveMedicine($scope.medicine).then(function (res) {
                        console.log(res);
                        if (res.data.ErrorCode == false) {
                            alertify.error(res.data.Message);
                        } else {
                            alertify.success(res.data.Message);
                            $scope.getAllMedicines();
                            resetForm();
                        }

                    });
                }
                else {
                    alertify.error("Duplicate Entry.");
                }
            })

        }
    }

    $scope.selectedMedicine = function selectedLicense(medicine) {
        var index = indexOfObjectInArray($scope.medicineList, 'MedicineID', medicine.MedicineID);

        if (index == currentIndex) {
            $scope.medicineList[currentIndex].selected = true;
        } else {
            previousIndex = currentIndex;
            currentIndex = index;
            $scope.medicineList[currentIndex].selected = true;
            $scope.medicine = angular.copy($scope.medicineList[currentIndex]);

            if (previousIndex != null) {
                $scope.medicineList[previousIndex].selected = false;
            }
        }
    }

    function resetForm() {
        if (currentIndex != null) {
            currentIndex = null;
            previousIndex = null;
        }
        $scope.medicine = {};
        $scope.medicineForm.$setPristine();
        $scope.medicineForm.$setUntouched();
    }

    function indexOfObjectInArray(array, property, value) {
        for (var i = 0; i < array.length; i++) {
            if (array[i][property] == value) {
                return i;
            }
        }
        return -1;
    }


    $scope.saveMedicine = saveMedicine;
    $scope.resetForm = resetForm;

}])