using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Core.ViewModels
{
    public class LoanDetailsViewModel
    {
        public int LoanDetailsID { get; set; }
        public int LoanID { get; set; }
        public decimal RepaymentAmount { get; set; }
        public int RepaymentMonth { get; set; }
        public DateTime RepaymentDate { get; set; }
    }
}
