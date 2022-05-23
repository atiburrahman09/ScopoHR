using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Domain.Models
{
    public class Tax : BaseEntity
    {
        public int TaxID { get; set; }
        public int EmployeeID { get; set; }
        public decimal Amount { get; set; }
        public int Year { get; set; }
    }
}
