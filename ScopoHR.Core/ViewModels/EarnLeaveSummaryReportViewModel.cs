using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Core.ViewModels
{
    public class EarnLeaveSummaryReportViewModel
    {
        public string Floor { get; set; }
        public string EmployeeType { get; set; }
        public int Person { get; set; }
        public double Amount { get; set; }
        public DateTime Date { get; set; }
    }
}
