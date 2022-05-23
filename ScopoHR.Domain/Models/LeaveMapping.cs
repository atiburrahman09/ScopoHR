using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Domain.Models
{
    public class LeaveMapping : BaseEntity
    {
        public int LeaveMappingID { get; set; }
        public int EmployeeID { get; set; }
        public int LeaveTypeID { get; set; }
        public int LeaveDays { get; set; }
        public int LeaveTaken { get; set; }
        public int YearMappingID { get; set; }
        [ForeignKey("YearMappingID")]
        public virtual YearMapping Yearmappings { get; set; }
    }
}
