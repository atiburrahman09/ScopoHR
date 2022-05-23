scopoAppServices.service('mailService', function ($http) {



    this.saveClient = function () {
        return $http.post("/TaskManagement/Mail/SendMail");
    };


});