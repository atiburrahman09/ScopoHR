using ScopoHR.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScopoHR.Core.ViewModels;
using System.Data.SqlClient;

namespace ScopoHR.Core.Services
{
    public class SalaryService
    {
        private UnitOfWork unitOfWork;
        public SalaryService(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        

        public List<MonthViewModel> GetMonth()
        {
            List<MonthViewModel> month = new List<MonthViewModel>();

            month.Add(new MonthViewModel { MonthId = 1, MonthName = "January" });
            month.Add(new MonthViewModel { MonthId = 2, MonthName = "February" });
            month.Add(new MonthViewModel { MonthId = 3, MonthName = "March" });
            month.Add(new MonthViewModel { MonthId = 4, MonthName = "April" });
            month.Add(new MonthViewModel { MonthId = 5, MonthName = "May" });
            month.Add(new MonthViewModel { MonthId = 6, MonthName = "June" });
            month.Add(new MonthViewModel { MonthId = 7, MonthName = "July" });
            month.Add(new MonthViewModel { MonthId = 8, MonthName = "August" });
            month.Add(new MonthViewModel { MonthId = 9, MonthName = "Septembar" });
            month.Add(new MonthViewModel { MonthId = 10, MonthName = "Octobar" });
            month.Add(new MonthViewModel { MonthId = 11, MonthName = "Novembar" });
            month.Add(new MonthViewModel { MonthId = 12, MonthName = "Decembar" });

            return month;

        }

        public List<YearViewModel> GetYear()
        {
            List<YearViewModel> year = new List<YearViewModel>();

            year.Add(new YearViewModel { YearId = Convert.ToString(DateTime.Now.Year + 1), YearName = Convert.ToString(DateTime.Now.Year + 1) });
            year.Add(new YearViewModel { YearId = Convert.ToString(DateTime.Now.Year), YearName = Convert.ToString(DateTime.Now.Year) });
            year.Add(new YearViewModel { YearId = Convert.ToString(DateTime.Now.Year - 1), YearName = Convert.ToString(DateTime.Now.Year - 1) });
            year.Add(new YearViewModel { YearId = Convert.ToString(DateTime.Now.Year - 2), YearName = Convert.ToString(DateTime.Now.Year - 2) });
            year.Add(new YearViewModel { YearId = Convert.ToString(DateTime.Now.Year - 3), YearName = Convert.ToString(DateTime.Now.Year - 3) });
            year.Add(new YearViewModel { YearId = Convert.ToString(DateTime.Now.Year - 4), YearName = Convert.ToString(DateTime.Now.Year - 4) });
            year.Add(new YearViewModel { YearId = Convert.ToString(DateTime.Now.Year - 5), YearName = Convert.ToString(DateTime.Now.Year - 5) });

            return year;
        }

        public SalarySummaryViewModel GetSalaryData(int employeeID, DateTime fromDate)
        {
            SalarySummaryViewModel sVM = new SalarySummaryViewModel();

            int firstMonth =fromDate.AddMonths(-4).Month;
            int lastMonth = fromDate.AddMonths(-1).Month;

            var res = (from s in unitOfWork.MonthlySalaryRepository.Get()
                       where s.EmployeeId == employeeID && s.Month <= lastMonth  && s.Month >= firstMonth 
                       select s).ToList();            

            sVM.TotalPD= res.Sum(x => Int32.Parse(x.PD));
            sVM.TotalSalary= res.Sum(x => x.TotalPay) + res.Sum(x=>x.Advance);

            return sVM;
        }

        public List<SalarySheetViewModel> GetSalaryByMonthYear(int monthId, string yearId, int departmentID , string Floor , int BranchId)
        {
            List<SalarySheetViewModel> salarySheetList = unitOfWork.SalaryTypeRepository.SelectQuery<SalarySheetViewModel>("EXEC GetSalarySheet '" + monthId +"','"+ yearId +"' ,'"+ departmentID+"','"+ Floor +"','" + BranchId + "'");
            return salarySheetList;
        }
    }
}
