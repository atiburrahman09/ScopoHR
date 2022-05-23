using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Domain.Models
{
    public class SalaryMapping : BaseEntity
    {
        public int SalaryMappingID { get; set; }
        public int EmployeeID { get; set; }
        public int SalaryTypeID { get; set; }
        public decimal Amount { get; set; }
    }
}
