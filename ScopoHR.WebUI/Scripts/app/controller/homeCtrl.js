scopoAppControllers.controller('homeCtrl', ['$scope', 'homeService', function ($scope, homeService) {
      
    $scope.init = function () {
        $scope.getDashboardData();
    }


    $scope.getDashboardData = function () {
        homeService.getDashboardData()
            .then(function (res) {
                $scope.data = res.data.data;
            }, function (err) {
                handleHttpError(err);
            })

    }
}]);