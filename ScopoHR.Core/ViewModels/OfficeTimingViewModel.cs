using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Core.ViewModels
{
   public class OfficeTimingViewModel
    {
        public int OfficeTimingId { get; set; }
        public int BranchID { get; set; }

        [Required]
        public DateTime InTime { get; set; }

        [Required]
        public DateTime OutTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
