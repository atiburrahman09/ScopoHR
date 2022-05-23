using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Core.ViewModels
{
    public class WeeklyReportViewModel
    {
        public DateTime AttendanceDate { get; set; }
        public string CardNo { get; set; }
        public string Floor { get; set; }
        public string Section { get; set; }
        public string EmployeeName { get; set; }
        public string Designation { get; set; }
        public string Shift { get; set; }
        public string EntryTime { get; set; }
        public string ExitTime { get; set; }
        public double? WorkingHours { get; set; }
        public double? TwoHoursOT { get; set; }
        public double? ExtraOT { get; set; }
        public double? NightOT { get; set; }
        public string TotalHoursString { get; set; }

        public double? TotalHours { get; set; }
        public int? TypeOfHours { get; set; }
        public int? NoOfWorkers { get; set; }
        public int? Hours { get; set; }
    }
}
