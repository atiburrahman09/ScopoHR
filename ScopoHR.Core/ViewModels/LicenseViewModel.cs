using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Core.ViewModels
{
    public class LicenseViewModel
    {
        public int LicenseID { get; set; }
        [Required]
        public string LicenseNo { get; set; }
        [Required]
        public string LicenseName { get; set; }
        [Required]
        public Nullable<DateTime> RenewedDate { get; set; }
        public Nullable<DateTime> ExpiryDate { get; set; }
        public int LicenseValidity { get; set; }
        public string Remarks { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? LastModified { get; set; }
        public bool? IsDeleted { get; set; }

        public decimal? LicenseFee { get; set; }
        public decimal? Tips { get; set; }
        public decimal? BudgetAmount { get; set; }
        public Nullable<DateTime> BudgetDate { get; set; }
    }
}
