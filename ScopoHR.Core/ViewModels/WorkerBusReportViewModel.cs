using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Core.ViewModels
{
    public class WorkerBusReportViewModel
    {
        public string CardNo { get; set; }
        public string EmployeeName { get; set; }
        public string DesignationName { get; set; }
        public DateTime? InTime { get; set; }
        public string LocationName { get; set; }
    }
}
