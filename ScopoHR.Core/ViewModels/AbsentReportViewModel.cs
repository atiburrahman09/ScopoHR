using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Core.ViewModels
{
    public class AttendanceReportViewModel
    {
        public string CardNo { get; set; }
        public string EmployeeName { get; set; }
        public string DesignationName { get; set; }
        public string Remarks { get; set; }
        public DateTime Date { get; set; }
        public DateTime InTime { get; set; }
        public DateTime OutTime { get; set; }
        public string Status { get; set; }
        public int OverTime { get; set; }
        public string Floor { get; set; }
    }
}
