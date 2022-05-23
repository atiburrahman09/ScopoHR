using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Domain.Models
{
    public class SalaryIncrement : BaseEntity
    {
        public int SalaryIncrementID { get; set; }
        public int EmployeeID { get; set; }
        public DateTime IncrementDate { get; set; }
        public decimal IncrementAmount { get; set; }
    }
}
