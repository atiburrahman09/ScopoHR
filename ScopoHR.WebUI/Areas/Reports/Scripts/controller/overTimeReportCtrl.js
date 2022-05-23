scopoAppControllers.controller('overTimeReportCtrl', ['$scope', 'overTimeReportService', 'alertify', function ($scope, overTimeReportService, alertify) {

    // $scope.dailyATReportList = [];
    $scope.productionFloorList = [];


    $scope.initial = function () {
        $scope.GetProductionFloor();
    }

    $scope.dReport = {};

    $scope.getOverTimeReport = function () {
        console.log($scope.dReport);
        //console.log("test");
        overTimeReportService.getOverTimeReport($scope.dReport).then(function (res) {
           
            $location.path = res.data;
            
            //$scope.dailyATReportList = formatDate(res.data, "YYYY-MM-DD HH:mm", ["InTime", "OutTime"]);

        }, function (err) {
            handleHttpError(err);
        });
    };

    $scope.GetProductionFloor = function () {
        overTimeReportService.getAllProductionFloorLine().then(function (res) {
            $scope.productionFloorList = res.data;
        }, function (err) {
            handleHttpError(err);
        })
    };

    function bindDate(id, model) {
        console.log(id, model);
        $scope[model][id] = $('#' + id).val();
    }
    $scope.bindDate = bindDate;

}]);