scopoAppControllers.controller('locationAttendanceCtrl', ['$scope', 'locationAttendanceService', 'alertify', function ($scope, locationAttendanceService, alertify) {

    $scope.at = {};

    $scope.initial = function () {
        $scope.getBusLocation();
        //$scope.getRecentEmployees();
        $scope.getAllEmployees();
    }
    $scope.busList = [];
    $scope.getBusLocation = function () {
        locationAttendanceService.getBusLocation().then(function (res) {
            $scope.busList = res.data.data;
        }, function (err) {
            handleHttpError(err);
        });
    };

    $scope.empList = [];
    $scope.getAllEmployees = function () {
        locationAttendanceService.getAllEmployees().then(function (res) {
            $scope.empList = res.data.data;
        }, function (err) {
            handleHttpError(err);
        });
    };
    //$scope.empList = [];
    //$scope.getRecentEmployees = function () {
    //    locationAttendanceService.getRecentEmployees().then(function (res) {
    //        $scope.empList = res.data.data;
    //    }, function (err) {
    //        handleHttpError(err);
    //    });
    //};

    // searching
    //$scope.getEmployeeByKeyword = function (val) {
    //    console.log("in change");
    //    if (val.length < 3) {
    //        return;
    //    }
    //    locationAttendanceService.getEmployeeDropDownByKeyword(val).then(function (res) {
    //        $scope.empList= res.data;
    //    });
    //};

    $scope.saveInformation = function () {       
        if ($scope.updateAttendanceForm.$valid) {
            locationAttendanceService.saveInformation($scope.at).then(function (res) {
                    alertify.success("Attendance data updated.");
                    $scope.at = {};
                    $scope.updateAttendanceForm.$setPristine();
                    $scope.updateAttendanceForm.$setUntouched();
                }, function (err) {
                    handleHttpError(err);
                });          
        }
        else {
            alertify.error("Please fill the required fields.");
        }
    }

    $scope.getEmpList = function (locationID) {
        locationAttendanceService.getEmpList(locationID).then(function (res) {
            $scope.at.empList = res.data;
        });
    }


    $scope.saveList = function () {
        console.log($scope.at);
        
        locationAttendanceService.saveList($scope.at).then(function (res) {
            alertify.success("Employee list data updated.");
            $scope.at = {};
            $scope.updateAttendanceForm.$setPristine();
            $scope.updateAttendanceForm.$setUntouched();
        }, function (err) {
            handleHttpError(err);
        });
    }
   

    function bindDate(id, model) {
        console.log(id, model);
        $scope[model][id] = $('#' + id).val();
    }
    $scope.bindDate = bindDate;




}]);
