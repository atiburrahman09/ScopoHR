using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Core.ViewModels
{
    public class LeaveApprovalReportViewModel
    {
        public string EmployeeName{ get; set; }
        public string Designation { get; set; }
        public string CardNo { get; set; }
        public string Department { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public int LeaveDays { get; set; }
        public DateTime? JoinDate { get; set; }
    }
}
