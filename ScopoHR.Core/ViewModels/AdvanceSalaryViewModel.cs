using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Core.ViewModels
{
    public class AdvanceSalaryViewModel
    {
        public string CardNo { get; set; }
        public double Advance { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public Nullable<DateTime> AdvanceTaken { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? LastModified { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
