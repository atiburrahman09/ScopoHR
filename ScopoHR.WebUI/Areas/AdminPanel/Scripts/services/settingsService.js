scopoAppServices.service('settingsService', function ($http) {
    const BASE_URI = '/AdminPanel/GeneralSettings/'
    //department

    this.createDepartment = function (departmentObj) {
        return $http.post(BASE_URI + 'CreateDepartment', departmentObj);
    };

    this.allDepartments = function () {
        return $http.get(BASE_URI + 'GetAllDepartments');
    };

    this.updateDepartment = function (departmentObj) {
        return $http.post(BASE_URI + 'UpdateDepartment', departmentObj);
    };

    //designation
    this.createDesignation = function (designationObj) {
        return $http.post(BASE_URI + 'CreateDesignation', designationObj);
    };

    this.allDesignations = function () {
        return $http.get(BASE_URI + 'GetAllDesignations');
    };

    this.updateDesignation = function (designationObj) {
        return $http.post(BASE_URI + 'UpdateDesignation', designationObj);
    };

    //salary type
    this.createSalaryType = function (salaryTypeObj) {
        return $http.post(BASE_URI + 'CreateSalaryType', salaryTypeObj);
    };

    this.allSalaryTypes = function () {
        return $http.get(BASE_URI + 'GetAllSalaryTypes');
    };

    this.updateSalaryType = function (salaryTypeObj) {
        return $http.post(BASE_URI + 'UpdateSalaryType', salaryTypeObj);
    };


    //leave type
    this.createLeaveType = function (leaveTypeObj) {
        return $http.post(BASE_URI + 'CreateLeaveType', leaveTypeObj);
    };

    this.allLeaveTypes = function () {
        return $http.get(BASE_URI + 'GetAllLeaveTypes');
    };

    this.updateLeaveType = function (leaveTypeObj) {
        return $http.post(BASE_URI + 'UpdateLeaveType', leaveTypeObj);
    };

    //office time
    this.createOfficeTime = function (officeTimingObj) {
        return $http.post(BASE_URI + 'CreateOfficeTiming', officeTimingObj);
    };

    this.updateOfficeTime = function (officeTimingObj) {
        return $http.post(BASE_URI + 'UpdateOfficeTiming', officeTimingObj);
    };

    this.lastOfficeTime = function () {
        return $http.get(BASE_URI + 'GetOfficeTime');
    };

    //Production
    this.createFloorLine = function (productionObj) {
        return $http.post(BASE_URI + 'CreateProduction', productionObj);
    };

    this.allFloorLines = function () {
        return $http.get(BASE_URI + 'GetAllFloorLines');
    };

    this.updateFloorLine = function (productionObj) {
        return $http.post(BASE_URI + 'UpdateProduction', productionObj);
    };

    this.getAllShifts = function () {        
        return $http.get(BASE_URI + 'GetAllShifts');
    }

    this.saveWorkShift = function (workShift) {
        if (workShift.Id) {
            return $http.post(BASE_URI + 'UpdateWorkShift', workShift);
        }
        return $http.post(BASE_URI + 'CreateWorkShift', workShift);
    }

    this.getGraceData = function () {
        return $http.get(BASE_URI + 'GetGraceData');
    }
    this.saveGrace = function (grace) {
        if (grace.GraceID) {
            return $http.post(BASE_URI + 'UpdateGrace', grace);
        }
        return $http.post(BASE_URI + 'CreateGrace', grace);
    }

    this.saveAttendanceBonus = function (attendanceBonus) {
        if (attendanceBonus.AttendanceBonusID) {
            return $http.post(BASE_URI + 'UpdateAttendanceBonus', attendanceBonus);
        }
        return $http.post(BASE_URI + 'CreateAttendanceBonus', attendanceBonus);
    }
    this.getAttendanceBonusData = function () {
        return $http.get(BASE_URI + 'GetAttendanceBonusData');
    }
});