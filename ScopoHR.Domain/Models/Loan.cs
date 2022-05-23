using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Domain.Models
{
    public class Loan : BaseEntity
    {
        public int LoanID { get; set; }        
        public decimal LoanAmount { get; set; }
        public Nullable<DateTime> DisbursementDate { get; set; }
        public int EmployeeID { get; set; }          
        public int Duration { get; set; }
        public int StartsFrom { get; set; }
    }
}
