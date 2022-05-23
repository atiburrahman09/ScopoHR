scopoAppControllers.controller('userController', ['$scope', 'userService', function ($scope, userService) {
    var prevUser = null;
    $scope.user = {};
    $scope.users = [];
    $scope.roles = [];
    $scope.branchDropDown = [];


    $scope.init = function () {
        // get all app users 
        getAllUsers();
        // get all roles
        getAllRoles();
        getBranchDropDown();
    }

    function getBranchDropDown() {
        userService.getBranchDropDown().then(function (res) {
            $scope.branchDropDown = res.data;
            //console.log(res.data);
        }, function (err) {
            handleHttpError(err);
        });
    }


    function validatePassword() {
        if ($scope.user.Password != $scope.user.ConfirmPassword) {
            //console.log($scope.userForm.confirmPassword);
            $scope.userForm.confirmPassword.$valid = false;
        }
    }
    $scope.validatePassword = validatePassword;

    function handleFormSubmission() {
        if (!$scope.userForm.$valid) {
            return;
        }
        if ($scope.new) {
            userService.CheckUserExits($scope.user.UserName)
                .then(function (res) {
                    if(res.data == "False")
                    {
                        //console.log("User Not Exits");
                        createUser($scope.user);
                    }
                    else {
                        alertify.error("User Already Assigned!!!!");
                    }
                }, function (err) {
                    handleHttpError(err);
                });
            
        } else {
            //console.log("In Else Method");
            updateUser($scope.user);
        }        
    }
    $scope.handleFormSubmission = handleFormSubmission;

    function newUser() {
        $scope.new = true;
        $scope.edit = false;
        $scope.user = {};
        $scope.resetForm();
    }
    $scope.newUser = newUser;


    function createUser(user) {
        if (user.Password == user.ConfirmPassword)
        {
            console.log(user);
            userService.createUser(user).then(function (res) {
                alertify.success('User Created!');
                $scope.resetForm();
                getAllUsers();
            })
      .catch(function (err) {
          alertify.error(err.data);
      });
        }
        else {
            alertify.error("Password Does Not Match!!!!");
        }      
    }

    function updateUser(user) {
        userService.updateUser(user).then(function (res) {
            alertify.success(res.data);
        }, function (err) {
            handleHttpError(err);
        });
    }
    
    function getUserBranches(userID) {
        userService.getUserBranches(userID).then(function (res) {
            $scope.user.BranchIDs = res.data;
        }, function (err) {
            handleHttpError(err);
        });
    }
    
    function getUserRoles(userID) {
        userService.getUserRoles(userID).then(function (res) {
            console.log(res.data);
            $scope.user.Roles = res.data;            
        }, function (err) {
            handleHttpError(err);
        });
    }

    function getAllUsers() {
        userService.getAllUsers().then(function (res) {
            console.log(res.data);
            $scope.users = formatDate(res.data, null, ["LastLoginDate"]);            
        }, function (err) {
            handleHttpError(err);
        });
    }

    function getAllRoles () {
        userService.getAllRoles().then(function (res) {
            $scope.roles = res.data;
            
        });
    }

    function selectUser(user) {
        $scope.new = false;
        $scope.edit = true;
        if (prevUser != null) {
            prevUser.selected = false;
        }
        user.selected = true;
        prevUser = user;
    
        //console.log(user);
        $scope.user = angular.copy(user);
        getUserBranches(user.UserName);
        getUserRoles(user.UserName);
    }
    $scope.selectUser = selectUser;


    $scope.resetForm = function () {
        $scope.user = {};
        //console.log($scope.user.selected);
        //console.log("reset test");
        $scope.user.selected = null;
        $scope.userForm.$setPristine();
        $scope.userForm.$setUntouched();
    }
    


    $scope.getEmployeeByKeyword = function (val) {
        if (val.length < 3) {
            return;
        }
        return userService.getEmployeeDropDownByKeyword(val).then(function (res) {
            return res.data;
        });
    };

    $scope.typeaheadOnSelect = function (item, model, label) {
        $scope.user.EmployeeName = item.EmployeeName;
        $scope.user.UserName = item.CardNo;
    }
}]);