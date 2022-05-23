scopoAppServices.service('homeService', function ($http) {



    this.getDashboardData = function () {
        return $http.get("/Home/GetDashboardData");
    }
    

});