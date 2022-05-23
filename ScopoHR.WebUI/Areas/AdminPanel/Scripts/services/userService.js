scopoAppServices.service('userService', function ($http) {
    
    this.getAllUsers = function () {
        return $http.get('/AdminPanel/UserManagement/GetAllApplicationUsers');
    }

    this.getAllRoles = function () {
        return $http.get('/AdminPanel/UserManagement/GetAllRoles');
    }

    this.createUser = function (appUserVM) {
        return $http.post('/AdminPanel/UserManagement/CreateUser/', appUserVM);
    }

    this.getBranchDropDown = function () {
        return $http.get('/AdminPanel/UserManagement/GetBranchDropDown/');
    }

    this.getUserBranches = function (userName) {
        return $http.get('/AdminPanel/UserManagement/GetUserBranches', { params: { userName: userName } });
    }

    this.getUserRoles = function (userName) {
        return $http.get('/AdminPanel/UserManagement/GetUserRoles', { params: { userName: userName } });
    }

    this.updateUser = function (user) {
        return $http.post('/AdminPanel/UserManagement/UpdateUser', user);
    }

    this.CheckUserExits = function (userName) {
        return $http.get('/AdminPanel/UserManagement/CheckUserExits', { params: { userName: userName } });
    }

    this.getEmployeeDropDownByKeyword = function (inputString) {
        return $http.get("/AdminPanel/UserManagement/GetEmployeeDropDownByKeyword", { params: { inputString: inputString } });
    };
});