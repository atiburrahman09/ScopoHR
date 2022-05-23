using Microsoft.Build.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Core.ViewModels
{
    public class BankSalaryViewModel
    {
        public int ID { get; set; }
        [Required]
        public int EmployeeID { get; set; }
        [Required]
        public string CardNo { get; set; }
        [Required]
        public string EmployeeName { get; set; }
        public string Designation { get; set; }
        [Required]
        public string AccountNo { get; set; }
        [Required]
        public string BankName { get; set; }
        [Required]
        public string Company { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<DateTime> LastModified { get; set; }
        public bool IsDeleted { get; set; }
    }
}
