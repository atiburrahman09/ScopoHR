using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Core.ViewModels
{
    public class ComperativeSummaryReportViewModel
    {
        public string Section { get; set; }
        public int Month { get; set; }
        public decimal? Bonus { get; set; }
        public decimal? Hours { get; set; }
        public decimal? Salary_Wages { get; set; }
        public decimal? OTAmount { get; set; }
        public decimal? TotalPay { get; set; }
        public int TotalPerson { get; set; }
        public string Code { get; set; }
        public int Year { get; set; }
    }
}
