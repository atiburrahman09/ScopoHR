using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Core.ViewModels
{
    public class IncrementReportViewModel
    {
        public string CardNo { get; set; }
        public int EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string Designation { get; set; }
        public Nullable<DateTime> JoiningDate { get; set; }
        public decimal? Gross { get; set; }
        public decimal? IncrementAmount { get; set; }
        public string Remarks { get; set; }
        public decimal? PreviousIncrementAmount { get; set; }
        public Nullable<DateTime> PreviousIncrementDate { get; set; }
        public Nullable<DateTime> IncrementDate { get; set; }
    }
}
