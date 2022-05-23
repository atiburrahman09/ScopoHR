using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Core.ViewModels
{
    public class SalaryTypeViewModel
    {
        public int SalaryTypeID { get; set; }
        [Required]
        public string SalaryTypeName { get; set; }
    }
}
