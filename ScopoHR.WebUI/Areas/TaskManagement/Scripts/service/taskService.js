scopoAppServices.service('taskService', function ($http) {

    this.getAllTaskList = function () {
        return $http.get("/TaskManagement/Tasks/GetAllTasks");
    };

    this.saveTask = function (task) {
        return $http.post("/TaskManagement/Tasks/SaveTask", task);
    };

    this.getTaskCounts = function () {
        return $http.get('/TaskManagement/Tasks/GetTaskCounts');
    };

    this.getEmployeeDropDownByKeyword = function (inputString) {
        return $http.get("/TaskManagement/Tasks/GetEmployeeDropDownByKeyword", { params: { inputString: inputString } });
    };

    this.getAllProjectList = function () {
        return $http.get("/TaskManagement/Tasks/GetAllProjectList");
    };
});