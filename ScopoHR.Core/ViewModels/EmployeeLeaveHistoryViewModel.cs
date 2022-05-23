using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Core.ViewModels
{
    public class EmployeeLeaveHistoryViewModel
    {
        public string CardNo { get; set; }
        public string EmployeeName { get; set; }
        public string DesignationName { get; set; }
        public int? MedicalLeave { get; set; }
        public int? CasualLeave { get; set; }
        public int? MaternityLeave { get; set; }
        public int? compensatoryleave { get; set; }
        public int? earnedleave { get; set; }
        public int? WithoutPay { get; set; }
        public Nullable<DateTime> JoinDate { get; set; }
        public string LeaveTypeName { get; set; }
        public Nullable<DateTime> FromDate { get; set; }
        public Nullable<DateTime> ToDate{ get; set; }
        public int? LeaveDays { get; set; }
        public Nullable<DateTime> ApplicationDate { get; set; }
        public int? SL { get; set; }
        public int? SH { get; set; }
        public int? LeaveDaysCumm { get; set; }
        public int? YearlyTotal { get; set; }
        public string Section { get; set; }
        public int Code { get; set; }
        public string Floor { get; set; }
        public int Total { get; set; }
        public int TicketNo { get; set; }

    }
}
