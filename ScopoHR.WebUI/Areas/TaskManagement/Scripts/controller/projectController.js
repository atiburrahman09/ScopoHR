scopoAppControllers.controller('projectController', ['$scope', 'projectService', 'alertify', function ($scope, projectService, alertify) {

    $scope.projectList = [];
    $scope.clientList = [];
    $scope.selected = "";
    $scope.projectID = "";
    var prevIndex = null;
    var prevProject = null;
    $scope.project = {};

    

    $scope.statusList = [
        { Value: 0, Text: 'Not Confirmed' },
        { Value: 1, Text: 'Confirmed' },
        { Value: 2, Text: 'Started' },
        { Value: 3, Text: 'Completed' },
    ];

    $scope.init = function () {
        $scope.getAllProjectList();
        $scope.getAllClientList();

    }

    $scope.getAllProjectList = function () {
        projectService.getAllProjectList()
            .then(function (res) {
                $scope.projectList = res.data;
            }, function (err) {
                handleHttpError(err);
            });
    };

    $scope.getAllClientList = function () {
        projectService.getAllClientList()
            .then(function (res) {
                $scope.clientList = res.data;
            }, function (err) {
                handleHttpError(err);
            });
    };

    $scope.saveClicked = function () {
        if (!$scope.projectSetupForm.$valid) {
            console.log("Please fill the required fields.");
            return;
        }
        projectService.saveProject($scope.project).then(function (res) {
          
            if (res.data.Message == "Project already exists.")
            {
                alertify.error(res.data.Message);
            }
            else {
                $scope.getAllProjectList();
                alertify.success(res.data.Message);
                $scope.resetForm();
            }
         
        },
             function (err) {
                 handleHttpError(err);
             });
    };

    function bindDate(id, model) {
        console.log(id, model);
        $scope[model][id] = $('#' + id).val();
    }
    $scope.bindDate = bindDate;

    function projectSelected(project, index) {

        if (prevProject) {
            prevProject.selected = false;
        }
        project.selected = true;
        prevProject = project;
        $scope.project = angular.copy(project);
        $scope.old_project = project;
        prevIndex = indexOfObjectInArray($scope.projectList, "ProjectID", project.ProjectID);


        projectService.getProjectDetailByID($scope.projectList[index].ProjectID)
           .then(function (res) {
               $scope.project = formatDate([res.data], "MM/DD/YYYY", ["DelivaryDate"])[0];
           }, function (err) {
               handleHttpError(err);
           });

        
    }
    $scope.projectSelected = projectSelected;

    $scope.resetForm = function () {
        $scope.project = {};
        $scope.projectSetupForm.$setPristine();
        $scope.projectSetupForm.$setUntouched();
        if (prevProject) {
            prevProject.selected = false;
        }
    };

}]);

