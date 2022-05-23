using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Domain.Models
{
    public class InactiveEmployee
    {
        public int ID { get; set; }
        public int EmployeeID { get; set; }
        public int InActiveType { get; set; }
        public string Reason { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ApplicableDate { get; set; }
    }
}
