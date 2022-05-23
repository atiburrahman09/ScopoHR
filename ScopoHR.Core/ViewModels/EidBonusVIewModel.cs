using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Core.ViewModels
{
    public class EidBonusVIewModel
    {
        public string EmployeeName { get; set; }
        public string CardNo { get; set; }
        public string Designation { get; set; }
        public Nullable<DateTime> JoinDate { get; set; }
        public string Floor { get; set; }
        public decimal Gross { get; set; }
        public decimal BasicSalary { get; set; }
        public string AccountNo { get; set; }
        public string Bank { get; set; }
    }
}
