
scopoAppServices.service('jobCircularService', function ($http) {
    
    this.getAllJobs = function () {
        return $http.get('/Recruitment/JobCircular/GetAll');
    }

    this.createJob = function (job) {
        return $http.post('/Recruitment/JobCircular/CreateJob', job);
    }

   

    this.editJob = function (job) {
        return $http.post('/Recruitment/JobCircular/Edit', job);
    }

    this.deleteJob = function (job) {
        return $http.post('/Recruitment/JobCircular/Delete', job);
    }


});