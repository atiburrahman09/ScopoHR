scopoAppControllers.controller('mailController', ['$scope', 'mailService', 'alertify', function ($scope, mailService, alertify) {


    $scope.init = function () {
        //$scope.getAllClientList();
    }


    $scope.saveClicked = function () {
        mailService.saveClient().then(function (res) {
           
        }, function (err) {
            handleHttpError(err);
        });

    };




}]);

