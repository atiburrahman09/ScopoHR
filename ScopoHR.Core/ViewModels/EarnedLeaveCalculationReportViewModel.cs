using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Core.ViewModels
{
    public class EarnedLeaveCalculationReportViewModel
    {
        public string CardNo { get; set; }
        public string EmployeeName { get; set; }
        public string DesignationName { get; set; }
        public Nullable<DateTime> DueDate { get; set; }
        public Nullable<DateTime> JoinDate { get; set; }
        public double? TotalAttendance { get; set; }
        public double? PvsELBalance { get; set; }
        public double? ELGathered { get; set; }
        public double? Relished { get; set; }
        public decimal? Salary { get; set; }

    }
}
