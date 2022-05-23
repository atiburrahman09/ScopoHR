using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Core.ViewModels
{
    public class TaxViewModel
    {
        public int TaxID { get; set; }
        [Required]
        public int EmployeeID { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public int Year { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? LastModified { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
