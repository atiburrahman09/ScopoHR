scopoAppControllers.controller('licenseCtrl', ['$scope', 'licenseService', 'alertify', function ($scope, licenseService, alertify) {
    var currentIndex = null;
    var previousIndex = null;
    $scope.licenseList = [];
    $scope.license = {};

    $scope.init = function () {
        $scope.getAllLicenses();
    }


    $scope.getAllLicenses = function () {
        licenseService.getAllLicenses().then(function (res) {
            $scope.licenseList = formatDate(res.data, "MM/DD/YYYY", ["RenewedDate","ExpiryDate","BudgetDate"]);
        });
    }
   

    function saveLicense() {
        if ($scope.licenseForm.$valid) {
            licenseService.saveLicense($scope.license).then(function (res) {
                console.log(res);
                if (res.data.ErrorCode == false) {
                    alertify.error(res.data.Message);
                } else {
                    alertify.success(res.data.Message);
                    $scope.getAllLicenses();
                    resetForm();
                }

            });
        }
    }

    $scope.selectedLicense = function selectedLicense(license) {
        console.log("in select");
        var index = indexOfObjectInArray($scope.licenseList, 'LicenseID', license.LicenseID);

        if (index == currentIndex) {
            $scope.licenseList[currentIndex].selected = true;
        } else {
            previousIndex = currentIndex;
            currentIndex = index;
            $scope.licenseList[currentIndex].selected = true;
            $scope.license = angular.copy($scope.licenseList[currentIndex]);

            if (previousIndex != null) {
                $scope.licenseList[previousIndex].selected = false;
            }
            //getAllJobClass();
        }
    }

    $scope.getExpiryDate = function getExpiryDate(licenseValidity)
    {
        var expiryDate = moment($scope.license.RenewedDate).add(licenseValidity, 'months');
        console.log(expiryDate);
        $scope.license.ExpiryDate = moment(expiryDate).format('MM/DD/YYYY');
        console.log($scope.license);
    }



    function resetForm() {
        if (currentIndex != null) {
            currentIndex = null;
            previousIndex = null;
        }
        $scope.license = {};
        $scope.licenseForm.$setPristine();
        $scope.licenseForm.$setUntouched();
    }

    function indexOfObjectInArray(array, property, value) {
        for (var i = 0; i < array.length; i++) {
            if (array[i][property] == value) {
                return i;
            }
        }
        return -1;
    }


    $scope.saveLicense = saveLicense;
    //$scope.selectedlicense = selectedlicense;
    //$scope.getAllTrainingCurve = getAllTrainingCurve;

    $scope.resetForm = resetForm;

    $scope.bind = function bind(id, model) {
        $scope[model][id] = $('#' + id).val();
        //$scope.increment.IncrementDate = $('#' + id).val();
        //console.log($scope[model][id]);
    };
}])