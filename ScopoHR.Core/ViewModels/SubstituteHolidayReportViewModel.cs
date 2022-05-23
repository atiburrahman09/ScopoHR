using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Core.ViewModels
{
    public class SubstituteHolidayReportViewModel
    {
        public string EmployeeName { get; set; }
        public string CardNo { get; set; }
        public string Designation { get; set; }
        public int? TicketNo { get; set; }
        public DateTime? SubstituteHolidayDate { get; set; }
        public DateTime? WorkingDate { get; set; }
        public string Remarks { get; set; }
    }
}
