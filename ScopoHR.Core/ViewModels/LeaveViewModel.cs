using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Core.ViewModels
{
   public  class LeaveViewModel
    {
        public int EmployeeID { get; set; }
        public int LeaveMappingID { get; set; }
        public int LeaveTypeID { get; set; }
        public string LeaveTypeName { get; set; }
        public int LeaveDaysTotalByType { get; set; }
        public int LeaveDays { get; set; }
        public int LeaveTaken { get; set; }
        public int YearMappingID { get; set; }
        public int TotalLeaveByType { get; set; }
    }
}
