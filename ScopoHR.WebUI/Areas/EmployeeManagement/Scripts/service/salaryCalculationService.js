scopoAppServices.service('salaryCalculationService', function ($http) {
    this.generateSalary = function (salaryVM) {
        return $http.post('/EmployeeManagement/SalaryCalculation/CalculateSalary', salaryVM);
    };
});