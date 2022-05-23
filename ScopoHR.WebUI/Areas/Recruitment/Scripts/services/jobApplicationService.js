scopoAppServices.service('jobApplicationService', function ($http) {
    
    this.createApply = function (job) {
        return $http.post('/Recruitment/JobApplication/Create', job);
    };

    this.getAllJobs = function () {
        return $http.get('/Recruitment/JobApplication/GetAllJobs');
    }
});