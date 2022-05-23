scopoAppControllers.controller('prescriptionCtrl', ['$scope', 'prescriptionService', 'alertify', function ($scope, prescriptionService, alertify) {
  
    var prescriptionList = [];
    $scope.selected = "";
    var prevIndex = null;
    var prevIndexUser = null;
    $scope.prescription = {};

    $scope.init = function (editor) {
        $scope.getRecentPrescriptionList();
        

        $scope.ck = CKEDITOR.instances;
        CKEDITOR.on('instanceReady', function (ev) {
            if ($scope.iframeElement == undefined) {
                $scope.iframeElement = $scope.ck.editor1.document.$.defaultView.frameElement;
            }
        });
    }

    $scope.getEmployeeByKeyword = function (val) {
        console.log(val);
        if (val.length < 3) {
            return;
        }
        return prescriptionService.getEmployeeDropDownByKeyword(val).then(function (res) {
            return res.data;
        });
    };

    function typeaheadOnSelect(item, model, label) {
        $scope.prescription.EmployeeID = item.EmployeeID;
        $scope.prescription.EmployeeName = item.EmployeeName;
    }

    $scope.typeaheadOnSelect = typeaheadOnSelect;
    $scope.getRecentPrescriptionList = function () {
        prescriptionService.getRecentPrescriptionList()
            .then(function (res) {
                $scope.prescriptionList = res.data.data;
            }, function (err) {
                handleHttpError(err);
            })
    };
    $scope.selected = function (p, index) {
        angular.copy(p, $scope.prescription);
        $scope.CardNo = $scope.prescription.CardNo;
        $scope.prescriptionList[index].selected = true;
        if (prevIndex != null) {
            $scope.prescriptionList[prevIndex].selected = false;
        }
        prevIndex = index;       
    };


    $scope.createUpdateNotice = function (prescription) {
        if ($scope.prescriptionForm.$valid) {
            prescription.PrescribedDate = moment(new Date()).format("YYYY-MM-DD");
            prescriptionService.createUpdate(prescription)
                       .then(function (res) {
                           $scope.getRecentPrescriptionList();
                           alertify.success(res.data);                          
                           ClearFields();
                       }, function (err) {
                           handleHttpError(err);
                       });
        }
        else {
            alertify.error("Please fill up the required fields.");
        }

    };

    $scope.advancePrescriptionSearch = function (val) {
        if (val.length < 3) {
            if (val.length === 0)
                $scope.getRecentPrescriptionList();
            return;
        }
        prescriptionService.getPrescriptionDropDownByKeyword(val).then(function (res) {
            $scope.prescriptionList = res.data;
        });
    };


    function ClearFields() {
        $scope.prescriptionForm.$setPristine();
        $scope.prescriptionForm.$setUntouched();
        $scope.prescription = {};
        $scope.CardNo = "";
        
    };

    $scope.newMode=function()
    {
        $scope.prescription = {};
        $scope.CardNo = "";
    }


    }]);