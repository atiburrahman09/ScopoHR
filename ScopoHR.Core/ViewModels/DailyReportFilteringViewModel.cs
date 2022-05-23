using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Core.ViewModels
{
    public class DailyReportFilteringViewModel
    {
        public DateTime FromDate { get; set; }
        public DateTime Date { get; set; }
        public DateTime ToDate { get; set; }
        public int DepartmentID { get; set; }
        public string FloorID { get; set; }
        public string LineID { get; set; }
        public string CardNo { get; set; }
        public int? ShiftId { get; set; }
    }
}
