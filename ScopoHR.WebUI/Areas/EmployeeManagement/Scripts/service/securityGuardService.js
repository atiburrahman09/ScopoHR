scopoAppServices.service('securityGuardService', function ($http) {
    // get all security guard
    this.getAllGuards = function () {
        return $http.get('/EmployeeManagement/SecurityGuard/GetAllGuards');
    }

    this.saveRoster = function (list) {        
        return $http.post('/EmployeeManagement/SecurityGuard/SaveRoster', list);
    }

    this.searchRoster = function (date) {
        console.log(date);
        
        return $http.post('/EmployeeManagement/SecurityGuard/SearchRoster', JSON.stringify({ FromDate: date }));
    }
    
});