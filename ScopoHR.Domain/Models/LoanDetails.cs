using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Domain.Models
{
    public class LoanDetails
    {
        public int LoanDetailsID { get; set; }
        public int LoanID { get; set; }
        public decimal RepaymentAmount { get; set; }
        public DateTime RepaymentDate { get; set; }
    }
}
