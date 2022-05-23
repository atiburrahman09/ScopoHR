scopoAppControllers.controller('yearCtrl', ['$scope', 'yearService', 'alertify', function ($scope, yearService, alertify) {

   
    $scope.init = function () {
        $scope.getYear();
    };

    $scope.getYear = function () {
        yearService.getYear()
       .then(function (res) {
           $scope.yearData = res.data;
           if ($scope.yearData == '') {
               var yearOnly = parseInt(new Date().getFullYear());
               $scope.year = yearOnly;
           }
           else {
              
               $scope.year = (parseInt($scope.yearData.Year));
           }

       }, function (err) {
           handleHttpError(err);
       });
    };


    $scope.closeYear = function (yearData) {
        if ($scope.yearForm.$valid) {
            
            yearService.closeYear(yearData)
                .then(function (res) {
                    $scope.getYear();
                alertify.success(res.data);
            }, function (err) {
                handleHttpError(err);
            });
        }
        else
            return;
    };
}]);