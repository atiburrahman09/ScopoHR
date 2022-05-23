using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Domain.Models
{
    public class AdvanceSalary:BaseEntity
    {
        public int AdvanceSalaryId { get; set; }
        public string CardNo { get; set; }
        public double Advance { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public Nullable<DateTime> AdvanceTaken { get; set; }
     
    }
}
