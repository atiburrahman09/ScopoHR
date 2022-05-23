scopoAppServices.service('yearService', function ($http) {


    this.getYear = function () {
        return $http.get("/HRPolicy/Year/GetYear");
    };

    this.closeYear = function (year) {
        console.log(year);
        return $http.get("/HRPolicy/Year/CloseYear", { params: {year : year}});
    };
});