using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Core.ViewModels
{
    public class SecurityGuardRosterViewModel
    {
        public int Id { get; set; }
        public int EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string DesignationName { get; set; }
        public string CardNo { get; set; }
        public string ShiftId { get; set; }
        public string PlaceOfDuty { get; set; }
        public string Remarks { get; set; }
        public DateTime WorkingDate { get; set; }

    }
}
