scopoAppControllers.controller('clientController', ['$scope', 'clientService', 'alertify', function ($scope, clientService, alertify) {


    $scope.clientList = [];
    $scope.selected = "";
    $scope.clientID = "";
    var prevIndex = null;
    var prevClient = null;
    $scope.client = {};



    $scope.init = function () {
        $scope.getAllClientList();
    }

    $scope.getAllClientList = function () {
        clientService.getAllClientList()
            .then(function (res) {
                $scope.clientList = res.data;
            }, function (err) {
                handleHttpError(err);
            });
    };

    $scope.saveClicked = function () {
        if (!$scope.clientSetupForm.$valid) {
            console.log("Please fill the required fields.");
            return;
        }
        clientService.saveClient($scope.client).then(function (res) {
            if (res.data.Message == "Client Already Exists") {
                alertify.error(res.data.Message);
            }
            else {
                $scope.getAllClientList();
                alertify.success(res.data.Message);
                $scope.resetForm();
            }

        }, function (err) {
            handleHttpError(err);
        });

    };



    function clientSelected(client, index) {

        if (prevClient) {
            prevClient.selected = false;
        }
        client.selected = true;
        prevClient = client;
        $scope.client = angular.copy(client);
        $scope.old_client = client;
        prevIndex = indexOfObjectInArray($scope.clientList, "ClientID", client.ClientID);


        clientService.getClientDetailByID($scope.clientList[index].ClientID)
           .then(function (res) {
               $scope.client = res.data;
           }, function (err) {
               handleHttpError(err);
           });


    }
    $scope.clientSelected = clientSelected;

    $scope.resetForm = function () {
        $scope.client = {};
        $scope.clientSetupForm.$setPristine();
        $scope.clientSetupForm.$setUntouched();
        if (prevClient) {
            prevClient.selected = false;
        }
    };

}]);

