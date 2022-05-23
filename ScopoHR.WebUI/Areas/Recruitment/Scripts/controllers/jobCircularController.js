scopoAppControllers.controller('jobCircularController', ['$scope', 'jobCircularService', function ($scope, jobCircularService) {

    $scope.jobList = [];
    $scope.job = {};
    $scope.mode = "create";

    $scope.init = function () {
        console.log('controller init works');
        getAllJobs();
    }


    function createJob() {

        if ($scope.jobForm.$valid) {

            //console.log($scope.job);

            if ($scope.mode === "update") { 
                jobCircularService.editJob($scope.job).then(function (res) {
                    console.log('Job updated!');
                    $scope.job = {};
                    //console.log(res);
                    getAllJobs();
                }, function (err) {
                    console.log('Error', err);
                });


            }
            else {


                jobCircularService.createJob($scope.job).then(function (res) {
                    //console.log('Job Created!');
                    //console.log(res);
                    getAllJobs();
                }, function (err) {
                    console.log('Error', err);
                });
            }
        }

        console.log($scope.jobForm);
    }

    


    function getAllJobs() {
        jobCircularService.getAllJobs().then(function (res) {
            console.log(res);
           // $scope.jobList=res.data;
           $scope.jobList = formatDate(res.data, "MM/DD/YYYY", ["CreatedDate", "DueDate"]);
        }, function (err) {
            console.log('Error: ',err);
            })
    }

    function jobSelected(index)
    {
        $scope.mode = "update";
        console.log("job selected func");
        //console.log($scope.jobList[index]);
        console.log($scope.jobList[index].CreatedDate);
        //console.log(Object.getPrototypeOf($scope.jobList[index].CreatedDate));
        console.log($scope.jobList[index]);
        $scope.job = $scope.jobList[index];
        
    }


    function DeleteJob(job)
    {
        console.log("in delete job func");
        console.log(job);
        jobCircularService.deleteJob(job).then(function (res) {
            console.log('Job deleted!');
            $scope.job = {};
            //console.log(res);
            getAllJobs();
        }, function (err) {
            console.log('Error', err);
        });
        
    }

    function newMode()
    {
       
        $scope.mode = "create";
        $scope.job = {};
    }


    $scope.DeleteJob = DeleteJob;
    $scope.getAllJobs = getAllJobs;
    $scope.createJob = createJob;
    $scope.jobSelected = jobSelected;
    $scope.newMode = newMode();
}]);