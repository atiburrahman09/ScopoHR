scopoAppControllers.controller('dropOutEmployeeListCtrl', ['$scope', 'dropOutEmployeeListService', '$filter', 'alertify', '$window', function ($scope, dropOutEmployeeListService, $filter, alertify, $window) {

    $scope.dropOutList = [];
    $scope.noticeList = [];
    $scope.productionFloorList = [];
    $scope.workingShiftList = [];
    $scope.days = [{ day: 3 }, { day: 7 }, { day: 10 }];

    $scope.initial = function () {
        $scope.GetProductionFloor();
        $scope.getWorkingShift();
        $scope.divSize = 12;
        $scope.noticeDiv = false;
        $scope.printNoticeDiv = false;
    }

    $scope.dReport = {};
    $scope.GetProductionFloor = function () {
        console.log("Test");
        dropOutEmployeeListService.getAllProductionFloorLine().then(function (res) {
            $scope.productionFloorList = res.data;
            console.log($scope.productionFloorList);
        }, function (err) {
            handleHttpError(err);
        })
    };
    $scope.getWorkingShift = function () {
        dropOutEmployeeListService.getWorkingShift().then(function (res) {
            $scope.workingShiftList = res.data;
            console.log($scope.workingShiftList);
        }, function (err) {
            handleHttpError(err);
        })
    };



    $scope.getDropOutList = function () {
        if ($scope.droOutForm.$valid) {
            dropOutEmployeeListService.getDropOutList($scope.dReport).then(function (res) {
                if (res.data.length > 0) {
                    $scope.dropOutList = formatDate(res.data, "YYYY-MM-DD", ["JoinDate", "AbsentFrom", "FirstRemindarDate", "SecondRemindarDate", "ThirdRemindarDate"]);
                }
                else {
                    alertify.alert("No data found");
                    $scope.dropOutList = [];
                }


            }, function (err) {
                handleHttpError(err);
            });
        }
        else {
            alertify.error("Date is required!!!.");
        }
        //console.log("test");

    };

    $scope.selectNotice = function (data, index) {
        $scope.dropOutEmployeeData = data;
        $scope.divSize = 9;
        $scope.noticeDIv = true;
        $scope.getAllNotice();

    }

    $scope.getAllNotice = function () {
        dropOutEmployeeListService.getAllNotice().then(function (res) {
            $scope.noticeList = res.data.data;
        }, function (err) {
            handleHttpError(err);
        })
    }
    $scope.sendNotice = function (data) {
        if ($scope.noticeForm.$valid) {
            $scope.dropOutEmployeeData.NoticeID = $scope.NoticeID;
            $scope.dropOutEmployeeData.NoticeDetails = $scope.NoticeDetail;
            for (var i = 0; i < $scope.noticeList.length; i++)
            {
                if ($scope.NoticeID == $scope.noticeList[i].NoticeID)
                {
                    $scope.dropOutEmployeeData.NoticeTitle = $scope.noticeList[i].NoticeTitle;
                }
            }
            console.log($scope.dropOutEmployeeData);
            
            dropOutEmployeeListService.sendNotice($scope.dropOutEmployeeData).then(function (res) {
                
                alertify.success("Notice send to employee successful.");
                //$scope.noticeDIv = false;
                //$scope.printNoticeDiv = false;
                //$scope.divSize = 12;
                //console.log($window.location);
                //$window.location.href =$window.location.origin + '/Notice/Notice/PrintNotice';
                //$location.path = ($window.location.origin + '/Notice/Notice/PrintNotice')

                
            }, function (err) {
                console.log(err);
            });


        }
        else {
            alertify.error("Please fill the required information's");
        }

    }


    $scope.getNoticeDetails = function (noticeID) {
        for (var i = 0; i < $scope.noticeList.length; i++) {
            console.log($scope.dropOutEmployeeData);
            if (noticeID == $scope.noticeList[i].NoticeID) {
                $scope.NoticeDetail = $scope.noticeList[i].NoticeDetail;
                if ($scope.NoticeDetail.indexOf('@EmployeeName') !== -1) {
                    //str.replace(new RegExp('hello', 'g'), 'hi');

                    $scope.NoticeDetail = $scope.NoticeDetail.replace(new RegExp('@EmployeeName', 'g'), $scope.dropOutEmployeeData.EmployeeName);
                }
                if ($scope.NoticeDetail.indexOf('@CardNo') !== -1) {
                    
                    $scope.NoticeDetail = $scope.NoticeDetail.replace("@CardNo", $scope.dropOutEmployeeData.CardNo);
                }
                if ($scope.NoticeDetail.indexOf('@Designation') !== -1) {
                    
                    $scope.NoticeDetail = $scope.NoticeDetail.replace("@Designation", $scope.dropOutEmployeeData.Designation);
                }
                if ($scope.NoticeDetail.indexOf('@Department') !== -1) {
                    if ($scope.dropOutEmployeeData.DepartmentName == null) {
                        $scope.NoticeDetail = $scope.NoticeDetail.replace("@Department", "");
                    }
                    else {
                        $scope.NoticeDetail = $scope.NoticeDetail.replace("@Department", $scope.dropOutEmployeeData.DepartmentName);
                    }

                }
                if ($scope.NoticeDetail.indexOf('@MobileNo') !== -1) {
                    if ($scope.dropOutEmployeeData.MobileNo==null)
                    {
                        $scope.NoticeDetail = $scope.NoticeDetail.replace("@MobileNo", "");
                    }
                    else {
                        $scope.NoticeDetail = $scope.NoticeDetail.replace("@MobileNo", $scope.dropOutEmployeeData.MobileNo);
                    }
                   
                }
                if ($scope.NoticeDetail.indexOf('@FathersName') !== -1) {
                    if ($scope.dropOutEmployeeData.FatherName == null) {                       
                        $scope.NoticeDetail = $scope.NoticeDetail.replace(new RegExp('@FathersName', 'g'), "");
                    }
                    else {
                        $scope.NoticeDetail = $scope.NoticeDetail.replace(new RegExp('@FathersName', 'g'), $scope.dropOutEmployeeData.FatherName);
                    }

                }
                if ($scope.NoticeDetail.indexOf('@PermanentAddress') !== -1) {
                    if ($scope.dropOutEmployeeData.PermanentAddress == null) {
                        $scope.NoticeDetail = $scope.NoticeDetail.replace(new RegExp('@PermanentAddress', 'g'), "");
                    }
                    else {
                        $scope.NoticeDetail = $scope.NoticeDetail.replace(new RegExp('@PermanentAddress', 'g'), $scope.dropOutEmployeeData.PermanentAddress);
                    }

                }
                if ($scope.NoticeDetail.indexOf('@PresentAddress') !== -1) {
                    if ($scope.dropOutEmployeeData.PresentAddress == null) {
                        $scope.NoticeDetail = $scope.NoticeDetail.replace(new RegExp('@PresentAddress', 'g'), "");
                    }
                    else {
                        $scope.NoticeDetail = $scope.NoticeDetail.replace(new RegExp('@PresentAddress', 'g'), $scope.dropOutEmployeeData.PresentAddress);
                    }

                }
                if ($scope.NoticeDetail.indexOf('@SpouseName') !== -1) {
                    if ($scope.dropOutEmployeeData.SpouseName == null) {
                        $scope.NoticeDetail = $scope.NoticeDetail.replace(new RegExp('@SpouseName', 'g'), "");
                    }
                    else {
                        $scope.NoticeDetail = $scope.NoticeDetail.replace(new RegExp('@SpouseName', 'g'), $scope.dropOutEmployeeData.SpouseName);
                    }

                }

               
    
            }
        }

        //$scope.NoticeDetails = notice.NoticeDetails;
    };

    $scope.download = function () {
        if ($scope.dropOutList.length > 0) {


            alasql('SELECT CardNo, EmployeeName,Designation, JoinDate, AbsentFrom,AbsentDays,Remarks INTO XLSX("' + new Date().toLocaleDateString() + ' DropOutReport.xlsx",{sheetid:"Data", headers:true}) FROM ?', [$scope.dropOutList]);
        }
        else {
            alertify.error("Can not Download Empty File");
        }

    };



    function bindDate(id, model) {
        console.log(id, model);
        $scope[model][id] = $('#' + id).val();
    }
    $scope.bindDate = bindDate;

    $scope.dateOf = function (utcDateStr) {
        var date = new Date(utcDateStr);
        var hours = date.getHours() > 12 ? date.getHours() - 12 : date.getHours();
        var am_pm = date.getHours() >= 12 ? "PM" : "AM";
        hours = hours < 10 ? "0" + hours : hours;
        var minutes = date.getMinutes() < 10 ? "0" + date.getMinutes() : date.getMinutes();
        var seconds = date.getSeconds() < 10 ? "0" + date.getSeconds() : date.getSeconds();
        time = hours + ":" + minutes + ":" + seconds + " " + am_pm;
        console.log(time)
        return new Date(time);
    }

    $scope.back = function ()
    {
        $scope.printNoticeDiv = false;
        $scope.noticeDIv = true;

    }

    $scope.printPreview = function ()
    {
        $scope.printNoticeDiv = true;
        $scope.noticeDIv = false;
    }

    $scope.print = function (notice)
    {
        var printContents = notice;
        var popupWin = $window.open('', '_blank', 'width=300,height=300');
        popupWin.document.open();
        popupWin.document.write('<html><head><link rel="stylesheet" type="text/css" href="~/Content/Style.css" /></head><body onload="window.print()">' + printContents + '</body></html>');
        popupWin.document.close();
    }

}]);