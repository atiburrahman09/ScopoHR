scopoAppServices.service('leaveApprovalService', function ($http) {
    this.getApplications = function (applicationStatus, pageNo) {
        return $http.get('/LeaveManagement/LeaveApproval/GetApplications/', {
            params: { applicationStatus: applicationStatus, pageNo: pageNo }
        });
    }

    this.updateApplications = function (appList) {
        return $http.post('/LeaveManagement/LeaveApproval/UpdateApplications/', appList);
    }

    this.getLeaveMapping = function(employeeID){
        return $http.get('/LeaveManagement/LeaveApproval/GetLeaveMapping/', {params: {employeeID: employeeID}});
    }

    this.approaveApplications = function (List) {
        return $http.post('/LeaveManagement/LeaveApproval/ApproveApplications/', List);
    }
    
});