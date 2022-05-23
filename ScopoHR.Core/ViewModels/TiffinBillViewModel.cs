using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Core.ViewModels
{
    public class TiffinBillViewModel
    {
        public string  CardNo { get; set; }
        public string EmployeeName { get; set; }
        public string Designation { get; set; }
        public Nullable<DateTime> JoinDate { get; set; }
        public int TiffinDays { get; set; }
        public int TiffinRate { get; set; }
        public int PaymentAmount { get; set; }

    }
}
