scopoAppServices.service('branchService', function ($http) {
    var serviceBase = '/AdminPanel/Branch/';
    this.saveBranch = function(data){
        return $http.post(serviceBase + 'SaveBranch', data);
    }

    this.getBranchDropDown = function () {
        return $http.get(serviceBase + 'GetBranchDropDown');
    }

    this.getBranchByID = function (id) {
        return $http.get(serviceBase + 'GetBranchByID', { params: { id: id } });
    }
});