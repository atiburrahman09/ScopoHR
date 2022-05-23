using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Domain.Models
{
    public class License:BaseEntity
    {
        public int LicenseID { get; set; }
        public string LicenseNo { get; set; }
        public string LicenseName { get; set; }
        public Nullable<DateTime> RenewedDate { get; set; }
        public int LicenseValidity { get; set; }
        public string Remarks { get; set; }
        public Nullable<DateTime> ExpiryDate { get; set; }
        public decimal? LicenseFee { get; set; }
        public decimal? Tips { get; set; }
        public decimal? BudgetAmount { get; set; }
        public Nullable<DateTime> BudgetDate { get; set; }
    }
}
