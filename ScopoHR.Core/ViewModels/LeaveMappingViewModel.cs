using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Core.ViewModels
{
    public class LeaveMappingViewModel
    {
        public int EmployeeID { get; set; }
        public List<LeaveDaysViewModel> LeaveDaysList { get; set; }
    }

    public class LeaveDaysViewModel
    {
        public int LeaveMappingId { get; set; }
        public int LeaveTypeID { get; set; }
        public string LeaveTypeName { get; set; }
        public int LeaveDays { get; set; }
        public int LeaveTaken { get; set; }
        public int YearMappingID { get; set; }
    }    
}
