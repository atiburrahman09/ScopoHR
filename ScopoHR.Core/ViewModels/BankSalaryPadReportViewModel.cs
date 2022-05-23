using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Core.ViewModels
{
    public class BankSalaryPadReportViewModel
    {
        public string CardNo { get; set; }
        public string EmployeeName { get; set; }
        public string AccountNo { get; set; }
        public string DesignationName { get; set; }
        public decimal? Gross { get; set; }
        public decimal? TotalPay { get; set; }
        public decimal? Deduction { get; set; }
        public string BankName { get; set; }
        public string Company { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public decimal? Cash { get; set; }

    }
}
