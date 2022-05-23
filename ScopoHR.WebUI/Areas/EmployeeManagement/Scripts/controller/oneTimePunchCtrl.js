scopoAppControllers.controller('oneTimePunchCtrl', ['$scope', 'oneTimePunchService', 'alertify', 'Helper', '$filter',
     function ($scope, oneTimePunchService, alertify, Helper, $filter) {


         $scope.oneTimePunchList = [];
         $scope.productionFloorList = [];
         $scope.shiftList = [];
         $scope.searchAt = {};
         var isValidTime=0;
         $scope.initial = function () {
             isValidTime = 0;
             $scope.GetProductionFloor();
             $scope.GetShift();
         }


         $scope.GetProductionFloor = function () {
             oneTimePunchService.getAllProductionFloor().then(function (res) {
                 $scope.productionFloorList = res.data;
             }, function (err) {
                 handleHttpError(err);
             });
         };
         $scope.GetShift = function () {
             oneTimePunchService.getShift().then(function (res) {
                 $scope.shiftList = res.data;
             }, function (err) {
                 handleHttpError(err);
             });
         };

         //function toTime(timeString) {
         //    var timeTokens = timeString.split(':');
         //    console.log(timeTokens[0]);
         //    if (timeTokens[0] >= 24 || timeTokens[0] >=60)
         //        return "Invalid Date";
         //    return new Date(2018, 1, 1, timeTokens[0], timeTokens[1]);
         //}
         //// update attendance
         $scope.updateOneTimePunch = function () {
             console.log($scope.updateOneTimePunchForm.$valid);
             if (!$scope.updateOneTimePunchForm.$valid) {
                 alertify.error("Invalid data submitted");
                 return;
             }
             $scope.oneTimePunchList[0].InOutTime = $scope.searchAt.FromDate;
             oneTimePunchService.updateAttendance($scope.oneTimePunchList).then(function (res) {
                 alertify.success(res.data);
                 $scope.oneTimePunchList = [];
                 $scope.searchAt = {};
                 $scope.updateOneTimePunchForm.$setPristine();
                 $scope.updateOneTimePunchForm.$setUntouched();
             }, function (err) {
                 handleHttpError(err);
             });
             //for (var i = 0; i < $scope.oneTimePunchList.length; i++)
             //{
             //    if (toTime($scope.oneTimePunchList[i].InTime) !== "Invalid Date" && toTime($scope.oneTimePunchList[i].OutTime) !== "Invalid Date") {
             //        $scope.oneTimePunchList[0].InOutTime = $scope.searchAt.FromDate;
             //        oneTimePunchService.updateAttendance($scope.oneTimePunchList).then(function (res) {
             //            alertify.success(res.data);
             //            $scope.oneTimePunchList = [];
             //            $scope.searchAt = {};
             //            $scope.updateOneTimePunchForm.$setPristine();
             //            $scope.updateOneTimePunchForm.$setUntouched();
             //        }, function (err) {
             //            handleHttpError(err);
             //        });
             //    }
             //    else {
             //        isValidTime++;
             //        return alertify.error("Invalid time format");
             //    }
             //}
            
         }

         //$scope.getDailyAttendance = getDailyAttendance;
         $scope.searchAttendance = function (search) {
             $scope.oneTimePunchList = [];
             if ($scope.forms.searchAttandenceForm.$valid) {
                 oneTimePunchService.getDailyAttendance(search).then(function (res) {
                     console.log(res.data);
                     var result = formatDate(res.data, "YYYY-MM-DD", ["InOutTime", "InTimeDate","OutTimeDate"]);
                     console.log(result);
                     if (result.length > 0) {
                         $scope.oneTimePunchList = result;
                         console.log($scope.oneTimePunchList);
                     }
                     else {
                         alertify.alert("No data found.");
                         //$scope.oneTimePunchList.push(result);
                     }
                 }, function (err) { handleHttpError(err) });
             }
             else {
                 alertify.error("Please Fill Up The Informations!!!");
             }
         };
    


         $scope.bindDate = function (id, model) {
             $scope[model][id] = $('#' + id).val();
             console.log($('#' + id).val());
         };

         $scope.bindDateWithIndex = function bindDate(id, model, index) {
             $scope.oneTimePunchList[index][id] = $('#' + id).val();
         };
     }]);
