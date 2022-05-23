using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Core.ViewModels
{
    public class Sys_AttendanceBonusViewModel
    {
        public int AttendanceBonusID { get; set; }
        public decimal AttendanceBonus { get; set; }
        public bool IsApplicable { get; set; }
    }
}
