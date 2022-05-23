using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Core.ViewModels
{
    public class AttendanceSummaryViewModel
    {
        public int AbsentDays { get; set; }
        public int TotalPresent { get; set; }
        public int NoLine { get; set; }
        public int Leave { get; set; }
        public int Late { get; set; }
        public int GrandTotal { get; set; }
        public int Male { get; set; }
        public int Female { get; set; }
        public string Section { get; set; }
        public int Code { get; set; }
    }
}
