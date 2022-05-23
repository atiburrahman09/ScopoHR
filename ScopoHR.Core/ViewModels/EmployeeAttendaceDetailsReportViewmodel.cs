using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Core.ViewModels
{
    public class EmployeeAttendaceDetailsReportViewmodel
    {
        public string Day { get; set; }
        public DateTime? Date { get; set; }
        public string InTime { get; set; }
        public string OutTime { get; set; }
        public int? WorkingHours { get; set; }
        public int? OTMinutes { get; set; }
        public string Status { get; set; }
        public int LunchTime { get; set; }
        public string CardNo { get; set; }
        public string EmployeeName { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string DesignationName { get; set; }


    }
}
