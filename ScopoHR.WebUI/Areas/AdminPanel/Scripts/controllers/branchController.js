scopoAppControllers.controller('branchController', ['$scope', 'branchService', function ($scope, branchService) {
    var prevBranch = null;

    $scope.init = function () {
        getBranchDropDown();
    }
    
    // data containers
    $scope.branches = [];
    
    // instances
    $scope.branch = {};

    function saveBranch(branch) {
        if ($scope.branchForm.$submitted && !$scope.branchForm.$valid) {
            console.log('invalid form');
            return;
        }
       
        branchService.saveBranch(branch).then(function (res) {
            alertify.success(res.data);
            $scope.branch = {};
            resetForm();
            getBranchDropDown();
        }, function (err) {
            handleHttpError(err);
        });
    }
    $scope.saveBranch = saveBranch;

    function getBranchDropDown() {
        branchService.getBranchDropDown().then(function (res) {
            $scope.branches = res.data;
        }, function (err) {
            handleHttpError(err);
        });
    }

    function selectBranch(branch) {
        if (prevBranch != null) {
            prevBranch.selected = false;
        }
        branch.selected = true;
        prevBranch = branch;
        getBranchByID(branch.Value);
    }
    $scope.selectBranch = selectBranch;

    function getBranchByID(branchID) {
        branchService.getBranchByID(branchID).then(function (res) {
            $scope.branch = res.data;
        }, function (err) {
            handleHttpError(err);
        });
    }

    function resetForm() {
        $scope.branch = {};
        $scope.branchForm.$setPristine();
        $scope.branchForm.$setUntouched();
    }
    $scope.resetForm = resetForm;
}]);