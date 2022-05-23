using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Core.ViewModels
{
    public class LeaveTypeViewModel
    {
        public int LeaveTypeID { get; set; }
        [Required]
        public string LeaveTypeName { get; set; }
        [Required]
        public int LeaveDays { get; set; }
        public bool IsForward { get; set; }
        public bool IsPayableToNextYear { get; set; }
        public string ModifiedBy { get; set; }
    }
}
