scopoAppServices.service('noticeAssignService', function ($http) {



    this.getAllNoticeList = function () {
        return $http.get("/Notice/Notice/GetAllNoticeList");
    }
    this.getAllRecentUserList = function () {
        return $http.get("/Notice/Notice/GetAllRecentUserList");
    }

    this.getNoticeDetails = function (nID) {
        return $http.get("/Notice/Notice/GetAllNoticeDetailByID", { params: { nID: nID } });
    }

    this.publishNotice = function (publish) {
        return $http.post("/Notice/Notice/PublishNotice", publish);
    }

    this.createUpdate = function (notice) {
        return $http.post("/Notice/Notice/CreateUpdateNotice", notice);
    }


});