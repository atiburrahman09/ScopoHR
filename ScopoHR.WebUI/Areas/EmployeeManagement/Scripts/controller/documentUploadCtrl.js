scopoAppControllers.controller('documentUploadCtrl', ['$scope', 'docomentUploadService', 'alertify', 'Helper', '$filter',
    function ($scope, docomentUploadService, alertify, Helper, $filter) {

         $scope.doc = {

         };

         $scope.init = function () {
             getDocumentTypesDropdown();
             $scope.docTypes = null;
         }

         function getDocumentTypesDropdown() {
             docomentUploadService.getDocumentTypesDropdown().then(function (res) {
                 console.log(res.data);
                 $scope.docTypes = res.data;
             });
         }



         $scope.getEmployeeByKeyword = function (val) {
             if (val.length < 3) {
                 return;
             }
             return docomentUploadService.getEmployeeDropDownByKeyword(val).then(function (res) {
                 return res.data;
             });
         };

         function typeaheadOnSelect(item, model, label) {
             $scope.doc.EmployeeID = item.EmployeeID;
             $scope.doc.EmployeeName = item.EmployeeName;
             console.log(model);
         }
         $scope.typeaheadOnSelect = typeaheadOnSelect;


         $scope.uploadSuccess = function (message) {
             alertify.success("Data Successfully Uploaded!!!!");
         };

         $scope.uploadError = (function (message, file) {
             alertify.alert('Employee Attendance Failed?')
                 .then(function () {
                     location.reload();
                 });
         });
         

         $scope.getTarget = () => {
             return Helper.getFileUploadTarget();
         }

         $scope.uploadItem = function (d, fileCategory) {
             

             //d.defaults.query.category = fileCategory;
             d.defaults.query.employeeId = 0;
             //console.log(d.defaults.query);
             console.log('docment : ', $scope.doc);
             d.defaults.query.category = $scope.doc.Type;
             d.defaults.query.employeeId = $scope.doc.EmployeeID;
             d.defaults.query.cardNo = $scope.doc.Employee.CardNo;
             d.upload();
         };

         $scope.checkExtension = function (d) {
             if (d.getExtension() == 'png' ||
                 d.getExtension() == 'pdf' ||
                 d.getExtension() == 'jpg') {
                 $scope.status = true;
                 return true;
             }
             else {
                 alertify.error("Invalid file extension.");
                 return false;
             }
         };
        
         $scope.enableFileBrowser = function (id) {
             $('#' + id).click();
         }
         
     }]);
