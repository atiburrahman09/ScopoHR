using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Core.ViewModels
{
    public class SalaryIncrementViewModel
    {
        public int SalaryIncrementID { get; set; }
        public int EmployeeID { get; set; }
        public string CardNo { get; set; }
        public string EmployeeName { get; set; }
        public DateTime IncrementDate { get; set; }
        public decimal IncrementAmount { get; set; }
        public DateTime LastModified { get; set; }
        public string ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}
