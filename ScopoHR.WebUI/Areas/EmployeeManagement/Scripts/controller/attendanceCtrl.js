scopoAppControllers.controller('attendanceCtrl', ['$scope', 'attendanceService', 'alertify', 'Helper', '$filter',
     function ($scope, attendanceService, alertify, Helper, $filter) {


         $scope.attendanceList = [];
         var departmentList = [];
         $scope.productionFloorList = [];
         $scope.fileCSV = {};
         $scope.searchAt = { AbsentOnly: false };
         $scope.forms = {};
         $scope.divAttendance = false;
         $scope.at = {};
         $scope.hasDailyAttandance = false;

         $scope.initial = function () {
             $scope.csvType = 'attendanceData';
             $scope.GetAllDepartments();
             $scope.GetProductionFloor();
             getDocumentCategories();


         }


         $scope.attendanceStatus = [
             { id: 'A', name: 'Absent' },
             { id: 'P', name: 'Present' },
             { id: 'D', name: 'Delay' },
             { id: 'ED', name: 'Extended Delay' },
             { id: 'HD', name: 'Holiday' },
             { id: 'UP', name: 'Unpaid Leave' },
             { id: 'PL', name: 'Paid Leave' },
         ];
         $scope.attendanceType = [
             { id: 'I', name: 'InTime' },
             { id: 'O', name: 'OutTime' },
             { id: 'B', name: 'Both' }
         ];

         // file upload

         let getDocumentCategories = () => {
             Helper.getDocumentCategories().then(res=> {
                 console.log(res.data);
                 $scope.categories = res.data;

             }, err=> { handleHttpError(err); });
         }
         $scope.getTarget = () => {
             return Helper.getFileUploadTarget();
         }

         $scope.GetProductionFloor = function () {
             attendanceService.getAllProductionFloor().then(function (res) {
                 $scope.productionFloorList = res.data;
                 console.log(res.data);
             }, function (err) {
                 handleHttpError(err);
             });
         };

         $scope.GetAllDepartments = function () {
             attendanceService.getAllDepartments().then(function (res) {
                 $scope.departmentList = res.data.data;
             }, function (err) {
                 handleHttpError(err);
             });
         };


         //function toTime(timeString) {
         //    var timeTokens = timeString.split(':');
         //    console.log(timeTokens[0]);
         //    if (timeTokens[0] >= 24 || timeTokens[0] >= 60)
         //        return "Invalid Date";
         //    return new Date(2018, 1, 1, timeTokens[0], timeTokens[1]);
         //}
         // Save attendance
         $scope.updateDailyAttendance = function () {
             if ($scope.updateAttendanceForm.$valid) {
                 $scope.attendanceList[0].InOutTime = $scope.searchAt.FromDate;
                 attendanceService.saveAttendance($scope.attendanceList[0]).then(function (res) {
                     alertify.success(res.data);
                 }, function (err) {
                     handleHttpError(err);
                 });
                 //if ($scope.attendanceList[0].OutTime == "") {
                 //    if (toTime($scope.attendanceList[0].InTime) !== "Invalid Date") {
                 //        $scope.attendanceList[0].InOutTime = $scope.searchAt.FromDate;
                 //        attendanceService.saveAttendance($scope.attendanceList[0]).then(function (res) {
                 //            alertify.success(res.data);
                 //        }, function (err) {
                 //            handleHttpError(err);
                 //        });
                 //    }
                 //    else {
                 //        alertify.error("Invalid time format.");
                 //    }
                 //}
                 //else {
                 //    if (toTime($scope.attendanceList[0].OutTime) !== "Invalid Date") {
                 //        $scope.attendanceList[0].InOutTime = $scope.searchAt.FromDate;
                 //        attendanceService.saveAttendance($scope.attendanceList[0]).then(function (res) {
                 //            alertify.success(res.data);
                 //        }, function (err) {
                 //            handleHttpError(err);
                 //        });
                 //    }
                 //    else {
                 //        alertify.error("Invalid time format.");
                 //    }
                 //}
             }

             else {
                 alertify.error("Please fill the required information's");
             }
            



         }

         //$scope.createAttendance = function () {
         //    attendanceService.saveAttendance().then(function (msg) {
         //        alertify.success(msg.data);
         //    }, function (err) {
         //        handleHttpError(err);
         //    });
         //}

         $scope.getButtonName = function () {
             if (moment(new Date()).format("HH") < 13)
                 return "Get In!";
             return "Get Out!"
         }

         $scope.uploadSuccess = function (message) {
             alertify.success("Data Successfully Uploaded!!!!");
         };

         $scope.uploadError = (function (message, file) {
             alertify.alert('Employee Attendance Failed?')
                 .then(function () {
                     location.reload();
                 });
         });

         function resetForm() {
             $scope.$broadcast('angucomplete-alt:clearInput', 'employeeCardNo');
             $scope.at = {};
             $scope.forms.inOutTimeForm.$setPristine();
             $scope.forms.inOutTimeForm.$setUntouched();
         }

         $scope.Cancel = function () {
             $scope.divAttendance = false;
         };

         $scope.OpenDiv = function () {
             $scope.divAttendance = true;
             $scope.user = null;
         }


         $scope.uploadItem = function (d, fileCategory) {
             d.defaults.query.category = fileCategory;
             d.defaults.query.employeeId = 0;
             console.log(d.defaults.query);
             d.upload();
         };

         $scope.checkExtension = function (d) {
             if (d.getExtension() == 'csv' || d.getExtension() == 'txt') {
                 $scope.status = true;
                 return true;
             }
             else {
                 alertify.error("Please Upload CSV File");
                 return false;
             }
         };
         $scope.attCount = 0;
         $scope.pageNo = -1;
         function getDailyAttendance(search) {
             //if ($scope.pageNo < 0)
             //    $scope.pageNo = 0;
             console.log("test");
             attendanceService.getDailyAttendance(search).then(function (res) {
                 var result = formatDate([res.data], "YYYY-MM-DD hh:mm A", ["InOutTime", "InTimeDate", "InTime", "OutTimeDate", "OutTime"])[0];
                 console.log(result);
                 if (result.AttendanceID > 0) {
                     $scope.attendanceList.push(result);
                 }
                 else {
                     $scope.attendanceList = [];
                 }

                 //$scope.attCount = res.data.count;
             }, function (err) { handleHttpError(err) });
         }

         $scope.getNext = function (search) {
             if (!search.ToDate || !search.FromDate)
                 return;
             $scope.pageNo += 1;
             getDailyAttendance(search);
         }

         $scope.getPrev = function (search) {
             if (!search.ToDate || !search.FromDate)
                 return;
             $scope.pageNo -= 1;
             getDailyAttendance(search);
         }

         $scope.getDailyAttendance = getDailyAttendance;
         $scope.searchAttendance = function (search) {
             $scope.attendanceList = [];
             if ($scope.forms.searchAttandenceForm.$valid) {
                 attendanceService.getDailyAttendance(search).then(function (res) {
                     console.log(res.data);
                     var result = formatDate([res.data], "YYYY-MM-DD hh:mm A", ["InOutTime", "InTimeDate", "InTime", "OutTimeDate", "OutTime"])[0];

                     if (result.CardNo != null && result.HasAttendance != 0) {
                         $scope.attendanceList.push(result);

                         $scope.attendanceList[0].InTimeDate = moment($scope.attendanceList[0].InTimeDate).format('YYYY-MM-DD'); //$filter('date')($scope.attendanceList[0].InTimeDate, 'yyyy-MM-dd');
                         $scope.attendanceList[0].InTime = moment($scope.attendanceList[0].InTime).format('HH:mm');
                         $scope.attendanceList[0].OutTimeDate = moment($scope.attendanceList[0].OutTimeDate).format('YYYY-MM-DD');

                         if (moment($scope.attendanceList[0].OutTime).format('HH:mm') == "Invalid date") {
                             console.log("In If");
                             $scope.attendanceList[0].OutTime = "";
                             //$scope.attendanceList[0].OutTime = moment($scope.attendanceList[0].OutTime).format('HH:mm');
                         }
                         else {
                             $scope.attendanceList[0].OutTime = moment($scope.attendanceList[0].OutTime).format('HH:mm');
                         }

                         //if ($scope.attendanceList[0].OutTime = "Invalid date")
                         //{
                         //    $scope.attendanceList[0].OutTime = "";
                         //}
                         //else {
                         //    $scope.attendanceList[0].OutTimeDate = "";
                         //    $scope.attendanceList[0].OutTime = "";
                         //}

                         //$scope.hasDailyAttandance = true;
                     }
                     else {
                         //alertify.alert("No data found.");
                         $scope.attendanceList.push(result);
                         $scope.attendanceList[0].InTimeDate = moment($scope.attendanceList[0].InTimeDate).format('YYYY-MM-DD');
                         $scope.attendanceList[0].OutTimeDate = moment($scope.attendanceList[0].OutTimeDate).format('YYYY-MM-DD');

                     }
                 }, function (err) { handleHttpError(err) });
             }
             else {
                 alertify.error("Please Fill Up The Informations!!!");
             }
         };

         $scope.deleteAttendance = function (data) {
             console.log("In Delete");
             attendanceService.deleteAttendance(data[0]).then(function (res) {
                 alertify.success("Attendance information deleted.");
                 $scope.attendanceList = [];
             }, function (err) { handleHttpError(err) })
         }


         $scope.bindDate = function (id, model) {
             $scope[model][id] = $('#' + id).val();
             console.log($('#' + id).val());
         };
         $scope.bindDateWithIndex = function bindDate(id, model, index) {
             $scope.attendanceList[index][id] = $('#' + id).val();
         };

         $scope.getEmployeeByKeyword = function (val) {
             if (val.length < 3) {
                 return;
             }
             return attendanceService.getEmployeeDropDownByKeyword(val).then(function (res) {
                 return res.data;
             });
         };

         $scope.typeaheadOnSelect = function (item, model, label) {
             $scope.at.EmployeeName = item.EmployeeName;
             $scope.at.CardNo = item.CardNo;
         };

         $scope.enableFileBrowser = function (id) {
             $('#' + id).click();
         }

         $scope.Reset = function () {
             $scope.at = {};
         };




     }]);
