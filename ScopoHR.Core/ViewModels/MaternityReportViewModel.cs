using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Core.ViewModels
{
    public class MaternityReportViewModel
    {
        public string CardNo { get; set; }
        public string Designation { get; set; }
        public string EmployeeName { get; set; }
        public DateTime JoiningDate { get; set; }
        public decimal BasicSalary { get; set; }
        public decimal GrossSalary { get; set; }
        public int PresentDays { get; set; }
        public int TotalHolidays { get; set; }
        public int TWD { get; set; }
        public DateTime? Appx_DelivaryDate { get; set; }
        public DateTime? ReJoiningDate { get; set; }
        public decimal? Bonus { get; set; }
        public decimal TotalPay { get; set; }
        public int AbsentDays { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int RowNum { get; set; }

    }
}
