using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Core.ViewModels
{
   public class DesignationViewModel
    {
        public int DesignationID { get; set; }
        [Required]
        public string DesignationName { get; set; }
        public string DesignationNameBangla { get; set; }

        [Required]
        public int DepartmentID { get; set; }
        public string  DepartmentName { get; set; }
        public bool IsOverTimeApplicable { get; set; }
        public string ModifiedBy { get; set; }

    }
}
