scopoAppControllers.controller('dailyATReportCtrl', ['$scope', 'dailyATReportService', 'alertify', function ($scope, dailyATReportService, alertify) {

    $scope.dailyATReportList = [];
    $scope.productionFloorList = [];
    $scope.workingShiftList = [];
    $scope.initial = function () {
        $scope.GetProductionFloor();
        $scope.getWorkingShift();
    }

    $scope.dReport = {};

    $scope.getDailyATReport = function () {
        if ($scope.dailyATReportForm.$valid) {
            dailyATReportService.getDailyReport($scope.dReport).then(function (res) {
                console.log(res);
                if (res.data.length > 0) {
                    $scope.dailyATReportList = formatDate(res.data, "YYYY-MM-DD", ["Date"]);
                }
                else {
                    alertify.alert("No data found");
                    $scope.dailyATReportList = [];
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

    $scope.download = function () {
        if ($scope.dailyATReportList.length > 0) {


            alasql('SELECT CardNo, EmployeeName,Date, InTime, OutTime,LunchTime,TotalMinutes INTO XLSX("' + new Date().toLocaleDateString() + ' DailyAttendanceReport.xlsx",{sheetid:"Data", headers:true}) FROM ?', [$scope.dailyATReportList]);
        }
        else {
            alertify.error("Can not Download Empty File");
        }

    };

    $scope.downloadPDF = function () {
        if ($scope.dailyATReportList.length > 0) {

            // var doc = new jsPDF("p", "pt", "letter"),
            //source = $("#data")[0],
            //margins = {
            //    top: 10,
            //    bottom: 10,
            //    left: 10
            //};
            //     doc.fromHTML(
            //       source, // HTML string or DOM elem ref.
            //       margins.left, // x coord
            //       margins.top,
            //       {
            //           // y coord
            //           width: margins.width // max width of content on PDF
            //       },
            //       function (dispose) {
            //           // dispose: object with X, Y of the last line add to the PDF
            //           //          this allow the insertion of new lines after html
            //           doc.save("Test.pdf");
            //       },
            //       margins
            //     );

            //var margins = { top: 100, bottom: 60, left: 100, width: 522 };
            //var doc = new jsPDF('p', 'pt', 'letter');
            //doc.addHTML($('#reportTable')[0], margins.left, margins.top, { width: margins.width }, function () { doc.save(new Date().toLocaleDateString() + 'DailyAttendanceReport.pdf'); }, margins);


            var margins = { top: 10, bottom: 10, left: 10, width: 522 };
            var pdf = new jsPDF('p', 'pt', 'a4'); // basic create pdf
            pdf.internal.scaleFactor = 1.75; // play with this value

            pdf.addHTML(document.getElementById('reportTable'), {
                pagesplit: true, retina: true, background: '#fff', margin: { left: 10, right: 10, bottom: 10 }, 
            }, function () { // addHtml with automatic pageSplit
               
                pdf.save(new Date().toLocaleDateString() + 'DailyAttendanceReport.pdf');
            });

            
        }
        else {
            alertify.error("Can not Download Empty File");
        }
    }

    $scope.GetProductionFloor = function () {
        dailyATReportService.getAllProductionFloorLine().then(function (res) {
            $scope.productionFloorList = res.data;
        }, function (err) {
            handleHttpError(err);
        })
    };
    $scope.getWorkingShift = function () {
        dailyATReportService.getWorkingShift().then(function (res) {
            $scope.workingShiftList = res.data;
        }, function (err) {
            handleHttpError(err);
        })
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

}]);