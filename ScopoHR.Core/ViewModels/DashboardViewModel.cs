using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Core.ViewModels
{
    public class DashboardViewModel
    {
        public int TotalEmployee { get; set; }
        public int NewJoin { get; set; }
        public int DropOut { get; set; }
        public int LeaveApplied { get; set; }
    }
}
