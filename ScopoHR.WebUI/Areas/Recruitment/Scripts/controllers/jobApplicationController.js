scopoAppControllers.controller('jobApplicationController', ['$scope', 'jobApplicationService', function ($scope, jobApplicationService) {

    $scope.jobList = [];
    $scope.job = {};
    $scope.appliedJob = {};
   
    
    $scope.init = function () {
        $scope.getAllJobs();
    }

    $scope.getAllJobs=function() {
        jobApplicationService.getAllJobs().then(function (res) {
            $scope.jobList = formatDate(res.data, "MM/DD/YYYY", ["CreatedDate", "DueDate"]);
            $scope.jobSelected(0);
        }, function (err) {
            handleHttpError(err);
        })
    }

    $scope.lastSelected = 0;
    $scope.jobSelected = function (index) {
        $scope.job = $scope.jobList[index];
        if ($scope.jobList[$scope.lastSelected])
            $scope.jobList[$scope.lastSelected].selected = false;
        if ($scope.jobList[index])
            $scope.jobList[index].selected = true;
        $scope.lastSelected = index;
    }

    $scope.applyJob = function (msg)
    {
        if ($scope.jobApplicationForm.$valid) {

            $scope.appliedJob.CandidateResume = (JSON.parse(msg)).Guid;
            $scope.appliedJob.JobCircularId = $scope.job.JobCircularId;

            jobApplicationService.createApply($scope.appliedJob).then(function (res) {
                $scope.job = {};
                $scope.appliedJob = {};
                alertify.success("Successfully applied for the job.");
            }, function (err) {
                handleHttpError(err);
            });
        }
        else {
            alertify.error("Please check your inputs.");
        }

    }

    //upload cv
    $scope.InsertItems = function (d) {
        if(Object.keys($scope.appliedJob).length !=3){
            alertify.error("Please fill up the required fields first.");
        }
        else {
            d.upload();
        }
        
    }

}]);

