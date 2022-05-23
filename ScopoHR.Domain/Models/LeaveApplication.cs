using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Domain.Models
{
    public class LeaveApplication : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LeaveApplicationID { get; set; }
        public int LeaveTypeID { get; set; }
        public int EmployeeID { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int Status { get; set; }
        public string ApproavedBy { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public DateTime ApplicationDate { get; set; }
        public string ReasonOfLeave { get; set; }
        public DateTime? SubstituteDate { get; set; }
    }
}
