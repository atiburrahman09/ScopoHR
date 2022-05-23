using ScopoHR.Core.Helpers;
using ScopoHR.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScopoHR.Core.ViewModels;

namespace ScopoHR.Core.Services
{
    public class SalaryCalculationService
    {
        UnitOfWork unitOfWork;
        // Constructor 
        public SalaryCalculationService(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public void GenerateSalary(SalaryCalculationViewModel salaryVM,int branchId)
        {
            var query = $"EXEC CalculateSalary '" + salaryVM.FromDate + "','" + salaryVM.ToDate + "','" + " " + "','" + branchId + "','" + salaryVM.ShiftId + "','" + salaryVM.CardNo + "','"+salaryVM.EmployeeType+"'";
            unitOfWork.attendanceRepository.RawQuery(query);
        }
    }
}
