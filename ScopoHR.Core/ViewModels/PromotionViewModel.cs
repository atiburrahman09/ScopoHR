using Microsoft.Build.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Core.ViewModels
{
    public class PromotionViewModel
    {
        public int PromotionID { get; set; }
        [Required]
        public int EmployeeID { get; set; }
        [Required]
        public string CardNo { get; set; }
        [Required]
        public Nullable<DateTime> PromotionDate { get; set; }
        public string Remarks { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? LastModified { get; set; }
        public bool? IsDeleted { get; set; }
        public string EmployeeName { get; set; }
        public int NewDesignationID { get; set; }
        public int PreviousDesignationID { get; set; }
        public string NewDesignation { get; set; }
        public string PreviousDesignation { get; set; }
        public int DepartmentID { get; set; }
    }
}
