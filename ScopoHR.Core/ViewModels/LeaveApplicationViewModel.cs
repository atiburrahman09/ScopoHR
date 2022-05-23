using ScopoHR.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Core.ViewModels
{
    public class LeaveApplicationViewModel
    {
        public int LeaveApplicationID { get;  set; }
        public int EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string Designation { get; set; }
        public string CardNo { get; set; }
        public int LeaveTypeID { get; set; }
        public string LeaveTypeName { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int TotalDays { get; set; }
        public LeaveApplicationStatus Status { get; set; }
        public string ApproavedBy { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public DateTime ApplicationDate { get; set; }
        public string ReasonOfLeave { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? SubstituteDate { get; set; }

    }
}
