scopoAppControllers.controller('securityGuardCtrl', function ($scope, securityGuardService, Helper) {
   
    $scope.shiftA = [];
    $scope.shiftB = [];
    $scope.shiftC = [];
    $scope.shiftD = [];

    $scope.placesOfDuty = [
        'Main Security Office',
        'Main Gate Security Office',
        'Main Gate',
        'Mobile Duty',
        'Load / Unload Area',
        'Emergency Gate',
        'Back side of Building',
        'Building Roof',
        'Stair Male',
        'Stair Female',
        'Accessories Store',
        'Finish Goods Store (Gate No-01)',
        'Finish Goods Store (Gate No-02)',
        'Finish Goods Store (Gate No-03)',
        'Finish Goods Store (Gate No-04)',
        'Packing Area 5th Floor (Gate No-01)',
        'Packing Area 5th Floor (Gate No-02)',
        'Packing Area 5th Floor (Gate No-03)',
        'Packing Area 5th Floor (Gate No-04)',
        'Packing Area 5th Floor (Gate No-05)',
        'Packing Area 5th Floor (Gate No-06)',
        'Packing Area 6th Floor (Gate No-01)',
        'Packing Area 6th Floor (Gate No-02)',
        'Packing Area 7th Floor (Gate No-01)',
        'Packing Area 7th Floor (Gate No-02)',
        'Packing Area 7th Floor (Gate No-03)',
        'Packing Area 7th Floor (Gate No-04)',
        'Packing Area 7th Floor (Gate No-05)',
        'Packing Area 7th Floor (Gate No-06)',
        '8th Floor Office (Gate No-01)',
        '8th Floor Office (Gate No-02)',
        'BDC Store (Gate No-01)',
        'BDC Store (Gate No-02)',
        'Central Fabric Store(Gate No-01)',
        'Central Fabric Store(Gate No-02)',
        'Main Gate-01',
        'Main Gate-02',
        'Inner Gate',
        'In Front of Central Febric Store',
        'Back Side of Central Febric Store'
    ];


    $scope.init = function () {
        getAllGuards();
    }


    function getAllGuards() {
        securityGuardService.getAllGuards().then(res=> {
            $scope.guardsList = res.data;
        }, err=> { console.log(err) });
    }

    $scope.addItem = function (list, shiftId) {
        if (!$scope.rosterDate)
        {
            alertify.alert('Please pick a date for roster!');
            return;
        }
        list.push({
            EmployeeID: null,
            ShiftId: shiftId,
            WorkingDate: $scope.rosterDate
        });
        console.log(list);
    }

    $scope.removeItem = function(list, index){
        var x = list.splice(index, 1);      
    }

    $scope.previousDate = function () {
        if (!$scope.rosterDate) {
            return false;
        }
        return $scope.rosterDate.toLocaleDateString() < new Date().toLocaleDateString();
    }

    $scope.saveRoster = function () {
        if (!$scope.sgForm.$valid) {
            alertify.alert('invalid form submission');
        }

        //if ($scope.rosterDate.toLocaleDateString() < new Date().toLocaleDateString())
        //{
        //    alertify.alert('You cannot change previous roster!');
        //    return;
        //}
        

        if (hasDuplicates($scope.shiftA)) {
            alertify.error('Shift A contains duplicates!');
            return;
        }

        if (hasDuplicates($scope.shiftB)) {
            alertify.error('Shift B contains duplicates!');
            return;
        }

        if (hasDuplicates($scope.shiftC)) {
            alertify.error('Shift C contains duplicates!');
            return;
        }

        if (hasDuplicates($scope.shiftD)) {
            alertify.error('Shift D contains duplicates!');
            return;
        }

        let list = [];        
        list = list.concat($scope.shiftA, $scope.shiftB, $scope.shiftC, $scope.shiftD);
        console.log(list);

        if (list.length < 1) {
            return;
        }
    
        securityGuardService.saveRoster(list).then(res=> {
            alertify.success(res.data);
        }, err=> { handleHttpError(err); });
        
    }


    function hasDuplicates(list) {
        var items = list.map(x=> x.EmployeeID);
        return items.some((x, y) => items.indexOf(x) != y);
    }

    $scope.setDetail = function (list, index, cardNo) {
        if (!cardNo) {
            list[index].DesignationName = '';
            list[index].EmployeeName = '';
            return;
        }
        var i = indexOfObjectInArray($scope.guardsList, 'CardNo', cardNo);
        list[index].DesignationName = $scope.guardsList[i].DesignationName;
        list[index].EmployeeName = $scope.guardsList[i].EmployeeName;
        list[index].EmployeeID = $scope.guardsList[i].EmployeeID;
        
    }

    $scope.searchRoster = function () {
        if (!$scope.rosterDate)
        {
            alertify.alert('Please pick a date for roster!');
            return;
        }     
        search($scope.rosterDate);
    }
       

    $scope.searchNext = function () {
        let newDate = $scope.rosterDate.setDate($scope.rosterDate.getDate() + 1);
        $scope.rosterDate = new Date(newDate);
        search($scope.rosterDate);

    }

    $scope.searchPrev = function () {
        let newDate = $scope.rosterDate.setDate($scope.rosterDate.getDate() - 1);
        $scope.rosterDate = new Date(newDate);
        search($scope.rosterDate);
    }

    function search(date) {
        securityGuardService.searchRoster(date).then(res=> {
            console.log(res.data);
            if (res.status === 204) {
                alertify.alert("No data found!");
                $scope.shiftA = [];
                $scope.shiftB = [];
                $scope.shiftC = [];
                $scope.shiftD = [];                
            }
            else {
                $scope.shiftA = Helper.FormatDate(res.data.shiftA);
                $scope.shiftB = Helper.FormatDate(res.data.shiftB);
                $scope.shiftC = Helper.FormatDate(res.data.shiftC);
                $scope.shiftD = Helper.FormatDate(res.data.shiftD);
            }
        }, err=> { handleHttpError(err); });
    }
});