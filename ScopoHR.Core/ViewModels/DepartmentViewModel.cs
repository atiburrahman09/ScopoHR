using ScopoHR.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Core.ViewModels
{
    public class DepartmentViewModel
    {
        public int DepartmentID { get; set; }
        [Required]
        public string DepartmentName { get; set; }
        public string DepartmentNameBangla { get; set; }
    }

    
}
