using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Core.ViewModels
{
    public class EmployeeFinalBillReportViewModel
    {
        public string EmployeeName { get; set; }
        public string DesignationName { get; set; }
        public string DepartmentName { get; set; }
        public string CardNo { get; set; }
        public DateTime JoinDate { get; set; }
        public DateTime ApplicableDate { get; set; }
        public string TotalDays { get; set; }
        public int CurrentYearTotalPresentDays { get; set; }
        public decimal GrossWage { get; set; }
        public decimal Basic { get; set; }
        public int TotalYear { get; set; }
        public int YearCondition { get; set; }
        public string CurrentYearTotalMonth { get; set; }
    }
}
