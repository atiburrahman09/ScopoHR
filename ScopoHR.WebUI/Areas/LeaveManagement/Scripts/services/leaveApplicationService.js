scopoAppServices.service('leaveAppliactionService', function ($http) {

    this.getAllApps = function () {
        return $http.get('/LeaveManagement/LeaveApplications/GetAll');
    }
    
    this.createApp = function (app)
    {
        return $http.post('/LeaveManagement/LeaveApplications/CreateApp', app);
    }

    this.getLeaveTypes = function () {
        return $http.get("/LeaveManagement/LeaveApplications/GetLeaveTypes");
    };

    this.updateApp = function (app) {
        return $http.post("/LeaveManagement/LeaveApplications/Update", app);
    };

    this.getEmployeeDropDownByKeyword = function (inputString) {
        return $http.get("/LeaveManagement/LeaveApplications/GetEmployeeDropDownByKeyword", { params: { inputString: inputString } });
    }
    this.deleteApplication = function (appID) {
        return $http.get("/LeaveManagement/LeaveApplications/DeleteApplication", { params: { appID: appID } });
    }
    this.getAllAppsByEmployeeCardNo = function (cardNo) {
        return $http.get('/LeaveManagement/LeaveApplications/GetAllAppsByEmployeeCardNo', { params: { cardNo: cardNo } });
    }

    this.duplicateLeaveCheck = function (employeeID,fromDate,toDate) {
        return $http.get('/LeaveManagement/LeaveApplications/DuplicateLeaveCheck', { params: { employeeID: employeeID,fromDate:fromDate,toDate:toDate } });
    }


    
});