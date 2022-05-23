scopoAppControllers.controller('settingsController', ['$scope', 'settingsService', function ($scope, settingsService) {

    $scope.init = function () {
        $scope.show = 1;
        $scope.allDepartments();
        $scope.allDesignations();
        $scope.allSalaryTypes();
        $scope.allLeaveTypes();
        $scope.allFloorLines();
        $scope.getAttendanceBonusData();
        $scope.getGraceData();
        getAllShifts();
        $scope.workShift = new WorkShift();
    };

    //new
    $scope.newClicked = function () {
        $scope.mode = "create";
        if ($scope.lastSelectedDepartment) {
            $scope.departmentList[$scope.lastSelectedDepartment].selected = false;
            $scope.department = {};
        }
        if ($scope.lastSelectedDesignation) {
            $scope.designationList[$scope.lastSelectedDesignation].selected = false;
            $scope.designation = {};
        }

        if ($scope.lastSelectedSalaryType) {
            $scope.salaryTypeList[$scope.lastSelectedSalaryType].selected = false;
            $scope.salaryType = {};
        }

        if ($scope.lastSelectedLeaveType) {
            $scope.leaveTypeList[$scope.lastSelectedLeaveType].selected = false;
            $scope.leaveType = {};
        }
        if ($scope.lastSelectedFloorLine) {
            $scope.floorLineList[$scope.lastSelectedFloorLine].selected = false;
            $scope.floorLine = {};
        }
        if ($scope.prevShIndex) {
            $scope.shiftList[$scope.prevShIndex].selected = false;
            $scope.workShift = new WorkShift();
        }

    };

    //department
    $scope.departmentList = [];
    $scope.department = {};
    $scope.lastSelectedDepartment = 0;
    $scope.allDepartments = function () {
        settingsService.allDepartments().then(function (res) {
            $scope.departmentList = res.data;
            if ($scope.departmentList[$scope.lastSelectedDepartment])
                $scope.departmentList[$scope.lastSelectedDepartment].selected = true;
            angular.copy($scope.departmentList[$scope.lastSelectedDepartment], $scope.department);
            $scope.mode = 'update';
        }, function (err) {
            console.log('Error', err);
        });
    };

    $scope.departmentSelected = function (index) {
        $scope.mode = "update";
        $scope.departmentList[$scope.lastSelectedDepartment].selected = false;
        $scope.lastSelectedDepartment = index;
        $scope.departmentList[$scope.lastSelectedDepartment].selected = true;
        angular.copy($scope.departmentList[$scope.lastSelectedDepartment], $scope.department);
    };


    $scope.addDepartment = function () {
        if ($scope.departmentForm.$valid) {
            if ($scope.department.DepartmentID) {
                settingsService.updateDepartment($scope.department).then(function (res) {
                    if (res.data == true) {
                        alertify.error("Failed. Same department already exists.");
                    }
                    else {
                        $scope.departmentList[$scope.lastSelectedDepartment] = $scope.department;
                        alertify.success("Department update successful.");
                    }

                    $scope.departmentForm.$setPristine();
                    $scope.departmentForm.$setUntouched();
                    $scope.newClicked();
                }, function (err) {
                    handleHttpError(err);
                });
            }
            else {
                settingsService.createDepartment($scope.department).then(function (res) {
                    if (res.data == true) {
                        alertify.error("Failed. Same department already Exists.");
                    }
                    else {
                        $scope.department.DepartmentID = res.data;
                        $scope.departmentList.push($scope.department);
                        alertify.success("Department creation successful.");
                    }

                    $scope.departmentForm.$setPristine();
                    $scope.departmentForm.$setUntouched();
                    $scope.newClicked();

                }, function (err) {
                    handleHttpError(err);
                });
            }
        }
        else {
            alertify.error("Invalid form submission. Please check your inputs.");
        }
    }

    //designation

    $scope.designationList = [];
    $scope.designation = {};
    $scope.lastSelectedDesignation = 0;
    $scope.allDesignations = function () {
        settingsService.allDesignations().then(function (res) {
            $scope.designationList = res.data;
            if ($scope.designationList[$scope.lastSelectedDesignation])
                $scope.designationList[$scope.lastSelectedDesignation].selected = true;
            angular.copy($scope.designationList[$scope.lastSelectedDesignation], $scope.designation);
            $scope.mode = 'update';
        }, function (err) {
            console.log('Error', err);
        });
    };

    $scope.designationSelected = function (index) {
        $scope.mode = "update";
        $scope.designationList[$scope.lastSelectedDesignation].selected = false;
        $scope.lastSelectedDesignation = index;
        $scope.designationList[$scope.lastSelectedDesignation].selected = true;
        angular.copy($scope.designationList[$scope.lastSelectedDesignation], $scope.designation);
    };

    $scope.addDesignation = function () {
        if ($scope.designationForm.$valid) {
            if ($scope.designation.DesignationID) {
                settingsService.updateDesignation($scope.designation).then(function (res) {
                    if (res.data == true) {
                        alertify.error("Failed. Same designation already exists.");
                    }
                    else {
                        $scope.designationList[$scope.lastSelectedDesignation] = $scope.designation;
                        alertify.success("Designation update successful.");
                    }

                    $scope.designationForm.$setPristine();
                    $scope.designationForm.$setUntouched();
                    $scope.newClicked();
                }, function (err) {
                    handleHttpError(err);
                });
            }
            else {
                settingsService.createDesignation($scope.designation).then(function (res) {
                    if (res.data == true) {
                        alertify.error("Failed. Same designation already exists.");
                    }
                    else {
                        $scope.designation.DesignationID = res.data;
                        $scope.designationList.push($scope.designation);
                        alertify.success("Designation creation successful.");
                    }

                    $scope.designationForm.$setPristine();
                    $scope.designationForm.$setUntouched();
                    $scope.newClicked();

                }, function (err) {
                    handleHttpError(err);
                });
            }
        }
        else {
            alertify.error("Invalid form submission. Please check your inputs.");
        }
    }

    //salary type

    $scope.salaryTypeList = [];
    $scope.salaryType = {};
    $scope.lastSelectedSalaryType = 0;
    $scope.allSalaryTypes = function () {
        settingsService.allSalaryTypes().then(function (res) {
            $scope.salaryTypeList = res.data;
            $scope.salaryTypeList[$scope.lastSelectedSalaryType].selected = true;
            angular.copy($scope.salaryTypeList[$scope.lastSelectedSalaryType], $scope.salaryType);
            $scope.mode = 'update';
        }, function (err) {
            console.log('Error', err);
        });
    };

    $scope.salaryTypeSelected = function (index) {
        $scope.mode = "update";
        $scope.salaryTypeList[$scope.lastSelectedSalaryType].selected = false;
        $scope.lastSelectedSalaryType = index;
        $scope.salaryTypeList[$scope.lastSelectedSalaryType].selected = true;
        angular.copy($scope.salaryTypeList[$scope.lastSelectedSalaryType], $scope.salaryType);
    };

    $scope.addSalaryType = function () {
        if ($scope.salaryTypeForm.$valid) {
            if ($scope.salaryType.SalaryTypeID) {
                settingsService.updateSalaryType($scope.salaryType).then(function (res) {
                    if (res.data == true) {
                        alertify.error("Failed. Same salary type already exists.");
                    }
                    else {
                        $scope.salaryTypeList[$scope.lastSelectedSalaryType] = $scope.salaryType;
                        alertify.success("SalaryType update successful.");
                    }

                    $scope.salaryTypeForm.$setPristine();
                    $scope.salaryTypeForm.$setUntouched();
                    $scope.newClicked();
                }, function (err) {
                    handleHttpError(err);
                });
            }
            else {
                settingsService.createSalaryType($scope.salaryType).then(function (res) {
                    if (res.data == true) {
                        alertify.error("Failed. Same salary type already exists.");
                    }
                    else {
                        $scope.salaryType.SalaryTypeID = res.data;
                        $scope.salaryTypeList.push($scope.salaryType);
                        alertify.success("Salary type creation successful.");
                    }

                    $scope.salaryTypeForm.$setPristine();
                    $scope.salaryTypeForm.$setUntouched();
                    $scope.newClicked();

                }, function (err) {
                    handleHttpError(err);
                });
            }
        }
        else {
            alertify.error("Invalid form submission. Please check your inputs.");
        }
    }
    //Leave type
    $scope.leaveTypeList = [];
    $scope.leaveType = {};
    $scope.lastSelectedLeaveType = 0;
    $scope.allLeaveTypes = function () {
        settingsService.allLeaveTypes().then(function (res) {
            $scope.leaveTypeList = res.data;
            if ($scope.lastSelectedLeaveType)
                $scope.leaveTypeList[$scope.lastSelectedLeaveType].selected = true;
            angular.copy($scope.leaveTypeList[$scope.lastSelectedLeaveType], $scope.leaveType);
            $scope.mode = 'update';
        }, function (err) {
            console.log('Error', err);
        });
    };

    $scope.leaveTypeSelected = function (index) {
        $scope.mode = "update";
        $scope.leaveTypeList[$scope.lastSelectedLeaveType].selected = false;
        $scope.lastSelectedLeaveType = index;
        $scope.leaveTypeList[$scope.lastSelectedLeaveType].selected = true;
        angular.copy($scope.leaveTypeList[$scope.lastSelectedLeaveType], $scope.leaveType);
    };

    $scope.addLeaveType = function () {
        if ($scope.leaveTypeForm.$valid) {
            if ($scope.leaveType.LeaveTypeID) {
                settingsService.updateLeaveType($scope.leaveType).then(function (res) {
                    if (res.data == true) {
                        alertify.error("Failed. Same leave type already exists.");
                    }
                    else {
                        $scope.leaveTypeList[$scope.lastSelectedLeaveType] = $scope.leaveType;
                        alertify.success("Leave type update successful.");
                    }
                    $scope.leaveTypeForm.$setPristine();
                    $scope.leaveTypeForm.$setUntouched();
                    $scope.newClicked();
                }, function (err) {
                    handleHttpError(err);
                });
            }
            else {
                settingsService.createLeaveType($scope.leaveType).then(function (res) {
                    if (res.data == true) {
                        alertify.error("Failed. Same leave type already exists.");
                    }
                    else {
                        $scope.leaveType.LeaveTypeID = res.data;
                        $scope.leaveTypeList.push($scope.leaveType);
                        alertify.success("Leave type creation successful.");
                    }

                    $scope.leaveTypeForm.$setPristine();
                    $scope.leaveTypeForm.$setUntouched();
                    $scope.newClicked();
                }, function (err) {
                    handleHttpError(err);
                });
            }
        }
        else {
            alertify.error("Invalid form submission. Please check your inputs.");
        }
    }

    //Production floor line

    $scope.floorLineList = [];
    $scope.floorLine = {};
    $scope.lastSelectedFloorLine = 0;
    $scope.allFloorLines = function () {
        settingsService.allFloorLines().then(function (res) {
            $scope.floorLineList = res.data;
            if ($scope.floorLineList[$scope.lastSelectedFloorLine])
                $scope.floorLineList[$scope.lastSelectedFloorLine].selected = true;
            angular.copy($scope.floorLineList[$scope.lastSelectedFloorLine], $scope.floorLine);
            $scope.mode = 'update';
        }, function (err) {
            console.log('Error', err);
        });
    };

    $scope.floorLineSelected = function (index) {
        $scope.mode = "update";
        $scope.floorLineList[$scope.lastSelectedFloorLine].selected = false;
        $scope.lastSelectedFloorLine = index;
        $scope.floorLineList[$scope.lastSelectedFloorLine].selected = true;
        angular.copy($scope.floorLineList[$scope.lastSelectedFloorLine], $scope.floorLine);
    };

    $scope.addFloorLine = function () {
        if ($scope.floorLineForm.$valid) {
            if ($scope.floorLine.ProductionFloorLineID) {
                settingsService.updateFloorLine($scope.floorLine).then(function (res) {
                    if (res.data == true) {
                        alertify.error("Failed. Same floor-line already exists.");
                    }
                    else {
                        $scope.floorLineList[$scope.lastSelectedFloorLine] = $scope.floorLine;
                        alertify.success("Floor-line update successful.");
                    }

                    $scope.floorLineForm.$setPristine();
                    $scope.floorLineForm.$setUntouched();
                    $scope.newClicked();
                }, function (err) {
                    handleHttpError(err);
                });
            }
            else {
                settingsService.createFloorLine($scope.floorLine).then(function (res) {
                    if (res.data == true) {
                        alertify.error("Failed. Same floor-line already exists.");
                    }
                    else {
                        $scope.floorLine.ProductionFloorLineID = res.data;
                        $scope.floorLineList.push($scope.floorLine);
                        alertify.success("Floor-line creation successful.");
                    }

                    $scope.floorLineForm.$setPristine();
                    $scope.floorLineForm.$setUntouched();
                    $scope.newClicked();

                }, function (err) {
                    handleHttpError(err);
                });
            }
        }
        else {
            alertify.error("Invalid form submission. Please check your inputs.");
        }
    }


    //office time



    function WorkShift() {
        this.Name = '';
        this.InTime = '';
        this.OutTime = '';
        this.Id = 0;
    }

    $scope.saveWorkShift = function () {
        if (!$scope.officeForm.$valid) {
            return;
        }
        settingsService.saveWorkShift($scope.workShift).then(function (res) {
            getAllShifts();
            alertify.success(res.data);
            $scope.workShift = new WorkShift();
        });
    }
    function getAllShifts() {
        $scope.shiftList = [];
        settingsService.getAllShifts().then((res) => {
            res.data.forEach(x => {
                console.log
                    (x);
                $scope.shiftList.push({
                    Id: x.Id,
                    Name: x.Name,
                    InTime: new Date(parseInt(x.InTime.substr(6, 13))),
                    OutTime: new Date(parseInt(x.OutTime.substr(6, 13))),
                });
            });
            console.log($scope.shiftList);
        });

    }
    // office time

    $scope.prevShIndex;
    $scope.shiftSelected = function (index, sh, prevSh, prevIndex) {
        $scope.mode = "update";               
        if (index != prevIndex) {
            sh.selected = true;
            
            if (prevSh) {
                prevSh.selected = false;
            }
            $scope.prevShIndex = index;
            $scope.prevSh = sh;
            angular.copy(sh, $scope.workShift);
             
        }

        //$scope.departmentList[$scope.lastSelectedDepartment].selected = false;
        //$scope.lastSelectedDepartment = index;
        //$scope.departmentList[$scope.lastSelectedDepartment].selected = true;
        //angular.copy($scope.departmentList[$scope.lastSelectedDepartment], $scope.department);
    };
    $scope.defaultSalaryTypes = function (item) {
        if (!item)
            return;
        let range = [1, 2, 3];
        if (range.indexOf(item) > -1)
            return true
        return false;
    }


    // Grace

    $scope.saveGrace = function () {
        if (!$scope.graceForm.$valid) {
            return;
        }
        settingsService.saveGrace($scope.grace).then(function (res) {
            alertify.success(res.data);
        });
    }

    $scope.getGraceData = function () {
        settingsService.getGraceData().then(function (res) {
            $scope.grace = res.data;
        });
    }
    // Attendance Bonus
    $scope.saveAttendanceBonus = function () {
        if (!$scope.attendanceBonusForm.$valid) {
            return;
        }
        settingsService.saveAttendanceBonus($scope.attendanceBonus).then(function (res) {
            alertify.success(res.data);
        });
    }

    $scope.getAttendanceBonusData = function () {
        settingsService.getAttendanceBonusData().then(function (res) {
            $scope.attendanceBonus = res.data;
        });
    }


}]);