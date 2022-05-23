scopoAppServices.service('publishNoticeService', function ($http) {


    this.getAllRecentUserList = function () {
        return $http.get("/Notice/Notice/GetAllRecentUserList");
    }

    this.publishNotice = function (publish) {
        return $http.post("/Notice/Notice/PublishNotice", publish);
    }

});