scopoAppControllers.controller(
    'employeeCtrl', ['$scope', 'employeeService', 'alertify', 'Helper',
    function ($scope, employeeService, alertify, Helper
        ) {
        // vars
        $scope.employee = {};
        var prevIndex = null;
        $scope.basicSalary = 0;
        $scope.leaveMappings = [];
        $scope.employeeList = [];
        $scope.sectionList = [];
        $scope.documentLocations = {};
        $scope.workShifts = [
            { ShiftName: 'Day', ShiftId: 1 },
            { ShiftName: 'Night', ShiftId: 2 },
            { ShiftName: 'SG-A', ShiftId: 3 },
            { ShiftName: 'SG-B', ShiftId: 4 },
            { ShiftName: 'SG-C', ShiftId: 5 },
            { ShiftName: 'SG-D', ShiftId: 6 },
        ];
        $scope.employeeType = [
           { EmployeeTypeName: 'Worker', EmployeeTypeID: 1 },
           { EmployeeTypeName: 'Staff', EmployeeTypeID: 2 }
        ];
        $scope.userName = 'test';
        $scope.init = function (name) {
            $scope.userName = name;//angular.element('userName');

            $scope.cardNoDisabled = false;
            //$scope.employee.IsActive = false;
            $scope.getProductionFloors();
            $scope.getAllDepartments();
            getRecentEmployees();
            getDocumentLocations();
            getDocumentCategories();
            $scope.getSectionList();
        }
        // dropdowns
        $scope.salaryGrades = [
            { name: 'Grade 1', id: 1 },
            { name: 'Grade 2', id: 2 },
            { name: 'Grade 3', id: 3 },
            { name: 'Grade 4', id: 4 },
            { name: 'Grade 5', id: 5 },
            { name: 'Grade 6', id: 6 },
            { name: 'Grade 7', id: 7 },
        ];

        $scope.maritalStatus = [
            { name: 'Married', id: 1 },
            { name: 'Unmarried', id: 2 },
            { name: 'Divorced', id: 3 }
        ];

        $scope.genders = [
            { name: 'Male', id: 1 },
            { name: 'Female', id: 2 },
            { name: 'Not Specified', id: 3 }
        ];

        $scope.bloodGroups = [
            { name: "A+", id: "A+" },
            { name: "A-", id: "A-" },
            { name: "B+", id: "B+" },
            { name: "B-", id: "B-" },
            { name: "AB+", id: "AB+" },
            { name: "AB-", id: "AB-" },
            { name: "O+", id: "O+" },
            { name: "O-", id: "O-" }
        ];

        $scope.getSectionList = function () {
            employeeService.getSectionList().then(function (res) {
                $scope.sectionList = res.data;
            }, function (err) { handleHttpError(err); });
        };
        // image placeholder
        $scope.placeholderImage = Helper.PlaceholderImage;

        let getDocumentLocations = () => {
            Helper.getDocumentLocations().then(res=> {
                $scope.documentLocations = res.data;
            }, err=> { handleHttpError(err); });
        }

        let getDocumentCategories = () => {
            Helper.getDocumentCategories().then(res=> {
                $scope.categories = res.data;
            }, err=> { handleHttpError(err); });
        }

        let getLocation = (category) => {
            console.log(category);
            for (let c in $scope.categories) {
                if ($scope.categories[c] === category) {
                    return $scope.documentLocations[c];
                }
            }
        }

        // file uploads         
        $scope.getTarget = () => {
            return Helper.getFileUploadTarget();
        }

        $scope.removeFile = (index, files) => {
            files.splice(index, 1);
        }

        $scope.fileAdded = (document, fileCategory) => {
            setTimeout(() => {
                //uploadDocument(document);            
                document.defaults.query.category = fileCategory;
                document.defaults.query.employeeId = $scope.employee.EmployeeID;
                document.upload();
            }, 1000);
        }

        let uploadDocument = (doc) => {
            doc.upload();
        }

        $scope.uploadSuccess = function (msg, func) {
            alertify.success(msg);
            if (func) {
                func();
            }
        }

        function EmployeeDocuments() {
            this.ProfilePhoto = null,
            this.NomineePhoto = null,
            this.FingerPrint = null
        }




        // Get documents
        let getEmployeeDocumentsById = (id) => {
            $scope.empDocs = new EmployeeDocuments();
            employeeService.getEmployeeDocumentsById(id).then(res=> {
                for(let doc of res.data) {
                    switch (doc.Category) {
                        case $scope.categories.ProfilePhoto:
                            console.log('profile photo ', $scope.empDocs.ProfilePhoto);
                            $scope.empDocs.ProfilePhoto = doc;
                            console.log('profile photo ', $scope.empDocs.ProfilePhoto);
                            break;
                        case $scope.categories.NomineePhoto:
                            $scope.empDocs.NomineePhoto = doc;
                            break;
                        case $scope.categories.FingerPrint:
                            $scope.empDocs.FingerPrint = doc;
                            break;
                    }
                }
            }, err=> { handleHttpError(err); });
        }
        $scope.getEmployeeDocumentsById = getEmployeeDocumentsById;

        // new navigation
        $scope.contexts = {
            current: null,
        }

        $scope.navigateTo = (contx, callback) => {
            console.log($scope.userName);
            $scope.contexts.current = contx;
            if (callback) {
                callback();
            }
        }
        // new Employee
        let saveNewEmployee = (employee) => {
            if (employee.EmployeeID > 0) {
                employeeService.saveEmployee($scope.employee).then(function (res) {
                    alertify.success(res.data);
                    resetForm();
                    getRecentEmployees();
                }, function (err) {
                    handleHttpError(err);
                });
            }
            else {
                employeeService.isUniqueCardNo($scope.employee.CardNo).then(function (res) {
                    if (res.status === 206) {
                        alertify.error(res.data);
                    }
                    else {
                        //$scope.employee.IsActive = true;
                        employeeService.saveEmployee($scope.employee).then(function (res) {
                            alertify.success(res.data);
                            resetForm();
                            getRecentEmployees();
                        }, function (err) {
                            handleHttpError(err);
                        });
                    }
                });
            }


        }

        //   actions
        let actions = {
            empNew() {
                saveNewEmployee($scope.employee);
            },
            empDetail() {
                //alertify.error('Not implemented!');
                //return;

                saveNewEmployee($scope.employee);
            },
            empSalary() {
                let mapping = mapSalary($scope.salaryTypeList, $scope.employee.EmployeeID);
                saveSalaryMapping(mapping);
            },
            empBasic() {
                saveNewEmployee($scope.employee);
            },
            empLeave() {
                saveLeaveMapping($scope.leaveMappings);
            }
        }
        // form submission
        $scope.forms = {};
        $scope.handleFormSubmisstion = () => {
            if (!$scope.forms.employeeForm.$valid) {
                return;
            }
            actions[$scope.contexts.current]();
        }

        // salary mapping & automatic salary calculatin 
        $scope.salaryTypes = {
            Basic: 1,
            OverTime: 2,
            AttBonus: 3,
            MedAllow: 4,
            FoodAllow: 5,
            HouseRent: 6,
            Conveyance: 7
        }

        $scope.calculateSalary = (gross) => {
            console.log("In salary");
            console.log($scope.salaryTypeList);
            for(let s of $scope.salaryTypeList) {
                switch (s.SalaryTypeID) {
                    case $scope.salaryTypes.Basic:
                        s.Amount = Math.round(parseFloat((gross - 1100) / 1.4));
                        basic = s.Amount;
                        $scope.basicSalary = s.Amount;
                        console.log($scope.basicSalary);
                        break;
                    case $scope.salaryTypes.OverTime:
                        break;
                    case $scope.salaryTypes.AttBonus:
                        s.Amount = 500;
                        break;
                    case $scope.salaryTypes.MedAllow:
                        s.Amount = 250;
                        break;
                    case $scope.salaryTypes.FoodAllow:
                        s.Amount = 650;
                        break;
                    case $scope.salaryTypes.HouseRent:
                        s.Amount = Math.round(parseFloat(((gross - 1100) / 1.4) * 0.4));
                        break;
                    case $scope.salaryTypes.Conveyance:
                        s.Amount = 200;
                        break;
                    default:
                        break;
                }
            }
        }

        let getSalaryMapping = (empId) => {
            $scope.grossSalary = 0;
            employeeService.getAssignSalaryInfo(empId).then(function (res) {
                $scope.salaryTypeList = res.data;
                $scope.grossSalary = 0;
                for(let s of $scope.salaryTypeList) {
                    $scope.grossSalary += s.Amount;
                    $scope.salaryShow = $scope.grossSalary;
                }
            }, function (err) {
                handleHttpError(err);
            });
        };

        let mapSalary = (typeList, id) => {
            let salaryMapping = {
                EmployeeID: id,
                SalaryTypeAmountList: []
            };

            for(t of typeList) {
                salaryMapping.SalaryTypeAmountList.push(t);
            }
            return salaryMapping;
        }

        let saveSalaryMapping = (mapping) => {
            employeeService.createSalaryMapping(mapping).then(function (res) {
                console.log(res);
                if (res.data === true) {
                    alertify.success("Salary assign successful.");
                }
                else {
                    alertify.error(res.data.Message);

                }

            }, function (err) {
                handleHttpError(err);
            });
        }

        // leave mapping
        let getLeaveMapping = (empId) => {
            employeeService.getLeaveMapping(empId).then((res) => {
                $scope.leaveMappings = res.data;
                console.log(res.data);
            }, (err) => {
                handleHttpError(err);
            });
        }

        let saveLeaveMapping = (mapping) => {
            employeeService.saveLeaveMapping(mapping).then((res) => {
                if (res.data === true) {
                    alertify.success(res.data);
                }
                else {
                    alertify.error(res.data);
                }

            }, (err) => {
                handleHttpError(err);
            });
        }

        // reset form
        let resetForm = () => {

            $scope.employee = {};
            $scope.employee.IsActive = true;
            $scope.cardNoDisabled = false;
            $scope.forms.employeeForm.$setPristine();
            $scope.forms.employeeForm.$setUntouched();
            if (prevIndex !== null) {
                $scope.employeeList[prevIndex].selected = false;
            }
        }
        $scope.resetForm = resetForm;

        // searching
        $scope.getEmployeeByKeyword = function (val) {
            if (val.length < 3) {
                return;
            }
            return employeeService.getEmployeeDropDownByKeyword(val).then(function (res) {
                return res.data;
            });
        };

        $scope.advanceEmployeeSearch = function (val) {
            if (val.length < 3) {
                if (val.length === 0)
                    getRecentEmployees();
                return;
            }
            employeeService.getEmployeeDropDownByKeyword(val).then(function (res) {
                $scope.isRecent = false;
                $scope.employeeList = res.data;
            });
        };

        function reportToOnSelect(item, model, label) {
            $scope.employee.ReportToName = item.EmployeeName;
            $scope.employee.ReportToID = item.EmployeeID;
        }
        $scope.reportToOnSelect = reportToOnSelect;

        function referenceOnSelect(item, model, label) {
            $scope.employee.ReferenceName = item.EmployeeName;
            $scope.employee.ReferenceID = item.EmployeeID;
        }
        $scope.referenceOnSelect = referenceOnSelect;

        let getRecentEmployees = () => {
            employeeService.getRecentEmployees().then((res) => {
                $scope.isRecent = true;
                $scope.employeeList = res.data;
                //$scope.employeeList[0].selected = true;
                //$scope.employeeSelected(0);
            }, (err) => {
                handleError(err);
            })
        }

        //new
        $scope.newEmployee = () => {
            resetForm();
            $scope.leaveMappings = [];
            $scope.salaryTypeList = [];
        };

        //date picker date bind with model
        $scope.bindDate = function bindDate(id, model) {
            $scope[model][id] = $('#' + id).val();
        };

        // emplyee selection
        $scope.employeeSelected = function (e) {
            $scope.cardNoDisabled = true;
            var index = indexOfObjectInArray($scope.employeeList, 'EmployeeName', e.EmployeeName);
            if (!$scope.contexts.current || $scope.contexts.current === 'empNew')
                $scope.contexts.current = 'empBasic';
            $scope.employeeList[index].selected = true;
            if (prevIndex !== null && prevIndex !== index) {
                $scope.employeeList[prevIndex].selected = false;
            }
            prevIndex = index;
            $scope.employeeID = $scope.employeeList[index].EmployeeID;
            // load all necessary data from the server
            getEmployeeeDetailsById($scope.employeeID);
            getEmployeeDocumentsById($scope.employeeID);
            getSalaryMapping($scope.employeeID);
            getLeaveMapping($scope.employeeID);
        };


        // Get Employee Salary mapping after detail has been loaded
        let getEmployeeeDetailsById = (id) => {
            employeeService.getEmployeeeDetailsById(id).then(res=> {
                $scope.employee = angular.copy(Helper.FormatDate([res.data], "MM/DD/YYYY", ["JoinDate", "DateOfBirth"])[0]);
                $scope.activeChecked = $scope.employee.IsActive;
                $scope.getDesignationList($scope.employee.DepartmentID);
                console.log(res.data);
            }, err=> { handleHttpError(err); });
        }

        //Load all production floors
        $scope.floorLineSelected = true;
        $scope.productionFloorList = [];
        $scope.getProductionFloors = function () {
            employeeService.getAllProductionFloor().then(function (res) {
                $scope.productionFloorList = res.data;
            }, function (err) {
                handleHttperror(err);
            });
        };

        //load all departments
        //$scope.departmentSelected = true;
        $scope.departmentList = [];
        $scope.getAllDepartments = function () {
            employeeService.getAllDepartments().then(function (res) {
                $scope.departmentList = res.data;
            }, function (err) {
                handleHttpError(err);
            });
        };

        //load all designation on department    
        $scope.getDesignationList = function (departmentID) {
            if (departmentID)
                employeeService.getDesignationList(departmentID).then(function (res) {
                    $scope.designationList = res.data;
                },
                function (err) {
                    handleHttpError(err);
                });
        }

        // cardno validation
        $scope.isUniqueCardNo = function (cardNo) {
            if (!cardNo || cardNo.length < 4)
                return;
            employeeService.isUniqueCardNo(cardNo).then(function (res) {
                if (res.status === 206) {
                    alertify.error(res.data);
                }
            }, function (err) {
                handleHttpError(err);
            })
        }

        // file broser
        $scope.enableFileBrowser = function (id) {
            $('#' + id).click();
        }

        //getting index of selected employee
        function indexOfObjectInArray(array, property, value) {
            for (var i = 0; i < array.length; i++) {
                if (array[i][property] === value) {
                    return i;
                }
            }
            return -1;
        }

    }]);
