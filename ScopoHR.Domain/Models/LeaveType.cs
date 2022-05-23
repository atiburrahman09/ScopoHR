using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Domain.Models
{
    public class LeaveType : BaseEntity
    {
        public int LeaveTypeID { get; set; } 
        public string LeaveTypeName { get; set; }
        public int LeaveDays { get; set; }
        public bool IsForward { get; set; }
        public bool IsPayable { get; set; }
    }
}
