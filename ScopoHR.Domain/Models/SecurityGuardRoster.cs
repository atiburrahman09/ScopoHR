using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Domain.Models
{
    public class SecurityGuardRoster : BaseEntity
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string ShiftId { get; set; }
        public string PlaceOfDuty { get; set; }
        public DateTime WorkingDate { get; set; }
        public string Remarks { get; set; }
    }
}
