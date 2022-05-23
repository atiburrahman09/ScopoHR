using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Core.ViewModels
{
    public class SalarySummaryViewModel
    {
        public string Floor { get; set; }
        public decimal? PayableAmount { get; set; }
        public decimal? OTAmount { get; set; }
        public int TotalPD { get; set; }
        public decimal? TotalSalary { get; set; }
    }
}
