using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Domain.Models
{
    public class Fine : BaseEntity
    {
        public int FineID { get; set; }
        public Nullable<DateTime> Date { get; set; }
        public decimal Amount { get; set; }
        public int EmployeeID { get; set; }
    }
}
