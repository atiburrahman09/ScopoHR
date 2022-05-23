using Microsoft.Build.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Core.ViewModels
{
    public class MaternityViewModel
    {
        public int MaternityID { get; set; }
        [Required]
        public string CardNo { get; set; }
        [Required]
        public int EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public int LeaveApplicationID { get; set; }

        public Nullable<DateTime> FirstInstallmentDate { get; set; }
        public decimal? FirstInstallmentAmount { get; set; }
        public Nullable<DateTime> FirstPaymentDate { get; set; }
        public Nullable<DateTime> FirstRequisitionDate { get; set; }
        public decimal? FirstRequisitionAmount { get; set; }
        public Nullable<DateTime> FirstReceivedDate { get; set; }
        public Nullable<DateTime> MaternityLeaveDate { get; set; }
        public int? MaternityDuration { get; set; }

        public Nullable<DateTime> SecondInstallmentDate { get; set; }
        public decimal? SecondInstallmentAmount { get; set; }
        public Nullable<DateTime> SecondPaymentDate { get; set; }
        public Nullable<DateTime> SecondRequisitionDate { get; set; }
        public decimal? SecondRequisitionAmount { get; set; }
        public Nullable<DateTime> SecondReceivedDate { get; set; }

        public string ModifiedBy { get; set; }
        public DateTime? LastModified { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public DateTime? Appx_DelivaryDate { get; set; }
    }
}
