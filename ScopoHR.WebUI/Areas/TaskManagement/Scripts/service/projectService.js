scopoAppServices.service('projectService', function ($http) {

    this.getAllProjectList = function () {
        return $http.get("/TaskManagement/Project/GetAllProjectList");
    };

    this.getAllClientList = function () {
        return $http.get("/TaskManagement/Project/GetAllClientList");
    };

    this.saveProject = function (project) {
        return $http.post("/TaskManagement/Project/SaveProject", project);
    };


    this.getProjectDetailByID = function (projectID) {
        return $http.get("/TaskManagement/Project/GetProjectDetailByID", { params: { projectID: projectID } });
    };
});