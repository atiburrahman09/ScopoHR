scopoAppControllers.controller('databaseAdminCtrl', function ($scope, $http) {
    
    var events = {
        syncIn: function (shiftId) {
            $http.get('/AdminPanel/DatabaseAdministration/SyncIn?shiftId='+shiftId).then(res=> {
                alertify.success(res.data);
            }, err=> { handleHttpError(err)})
        },
        syncOut: function (shiftId) {
            $http.get('/AdminPanel/DatabaseAdministration/SyncOut?shiftId=' + shiftId).then(res=> {
                alertify.success(res.data);
            }, err=> { handleHttpError(err) })
        }
    }

    $scope.syncNow = function (event, shiftId) {
        events[event](shiftId);
    }
});