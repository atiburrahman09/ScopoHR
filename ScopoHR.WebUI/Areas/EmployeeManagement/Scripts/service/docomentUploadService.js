scopoAppServices.service('docomentUploadService', function ($http) {

    var serviceBase = "/EmployeeManagement/DocumentUpload/";

    this.getEmployeeDropDownByKeyword = function (inputString) {
        return $http.get("GetEmployeeDropDownByKeyword", { params: { inputString: inputString } });
    }

    this.getDocumentTypesDropdown = function () {
        return $http.get("GetDocumentTypes");
    }
   
});