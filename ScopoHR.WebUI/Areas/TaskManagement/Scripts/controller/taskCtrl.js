scopoAppControllers.controller('taskCtrl', ['$scope', 'taskService', 'alertify', function ($scope, taskService, alertify) {
    var today = moment(new Date()).format("YYYY/MM/DD");

    $scope.taskList = [];
    $scope.projectList = [];
    $scope.selected = "";
    $scope.tID = "";
    var prevIndex = null;
    var prevTask = null;

    $scope.counts = {};
    $scope.menus = {
        current: '',
        setCurrent: function (menu) {
            this.current = menu;
        }
    }
    $scope.task = {};

    var taskFilters = {
        allTask: function (task) {
            return task.Status != 2;
        },
        today: function (task) {
            if (!task.ActualStartDate) { return false;}
            return task.Status < 2 && (task.ActualStartDate <= today && task.PlannedEndDate >= today);
        },
        overdue: function (task) {
            return task.Status < 2 && task.PlannedEndDate < today;
        },
        pHigh: function (task) {
            return task.Status < 2 && task.Priority == 2;
        },
        pMedium: function (task) {
            return task.Status < 2 && task.Priority == 1;
        },
        pLow: function (task) {
            return task.Status < 2 && task.Priority == 0;
        },
        completed: function (task) {
            return task.Status == 2;
        }
    }

    $scope.taskFilter = function (task) {
        return taskFilters[$scope.menus.current](task);
    }

    $scope.priorities = [
        { Value: 0, Text: 'Low' },
        { Value: 1, Text: 'Medium' },
        { Value: 2, Text: 'High' },
    ];

    $scope.statusList = [
        { Value: 0, Text: 'Not Started' },
        { Value: 1, Text: 'Started' },
        { Value: 2, Text: 'Completed' },
    ];

    $scope.init = function () {
        getTaskCounts();
        $scope.getAllTaskList();
        $scope.menus.current = 'allTask';
        $scope.getAllProjectList();
    }

    $scope.getAllProjectList = function () {
        taskService.getAllProjectList()
            .then(function (res) {
                $scope.projectList = res.data;
            }, function (err) {
                handleHttpError(err);
            });
    };

    $scope.getAllTaskList = function () {
        taskService.getAllTaskList()
            .then(function (res) {                
                $scope.taskList = formatDate(res.data);
                console.log($scope.taskList);
            }, function (err) {
                handleHttpError(err);
            });
    };
  
    $scope.saveTask = function (task) {        
        if (!$scope.taskForm.$valid) {
            return;
        }
        task.ActualManHour = Math.abs(task.ActualManHour);
        task.PlannedManHour = Math.abs(task.PlannedManHour);
        taskService.saveTask(task).then(function (res) {
            //$scope.getAllTaskList();
            console.log(res);
            if (res.status === 201) {
                $scope.taskList.push(formatDate([res.data.Data])[0]);
            } else {
                $scope.taskList[prevIndex] = formatDate([res.data.Data])[0];
            }            
            getTaskCounts();
            alertify.success(res.data.Message);
            $scope.cancel();
        },
             function (err) {
                 handleHttpError(err);
                 $scope.task = $scope.old_task;
             });
    };

    function bindDate(id, model) {
        console.log(id, model);
        $scope[model][id] = $('#' + id).val();
    }
    $scope.bindDate = bindDate;

    function selectTask(task, index) {        
        if (prevTask) {
            prevTask.selected = false;
        }
        task.selected = true;
        prevTask = task;
        $scope.task = angular.copy(task);
        $scope.old_task = task;
        prevIndex = indexOfObjectInArray($scope.taskList, "TaskID", task.TaskID);
        $scope.edit = true;
    }
    $scope.selectTask = selectTask;

    $scope.cancel = function () {
        $scope.edit = false;
        $scope.task = {};
        $scope.taskForm.$setPristine();
        $scope.taskForm.$setUntouched();
        if (prevTask) {
            prevTask.selected = false;
        }
    };

    function getTaskCounts() {
        taskService.getTaskCounts().then(function (res) {            
            $scope.counts = res.data;
        }, function (err) {
            handleHttpError(err);
        });
    }

    function navigateTo(menu) {
        $scope.menus.setCurrent(menu);
    }
    $scope.navigateTo = navigateTo;

    function addNewTask() {        
        $scope.cancel();        
    }
    $scope.addNewTask = addNewTask;

    $scope.getEmployeeByKeyword = function (val) {
        if (val.length < 3) {
            return;
        }
        return taskService.getEmployeeDropDownByKeyword(val).then(function (res) {
            return res.data;
        });
    };

    $scope.typeaheadOnSelect = function(item, model, label) {        
        $scope.task.AssigneeName = item.EmployeeName;
        $scope.task.AssigneeID = item.EmployeeID;        
    }     
}]);