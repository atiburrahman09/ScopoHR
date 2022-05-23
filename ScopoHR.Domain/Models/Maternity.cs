using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Domain.Models
{
    public class Maternity:BaseEntity
    {
        public int MaternityID { get; set; }
        public string CardNo { get; set; }
        public int EmployeeID { get; set; }
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
        public DateTime? Appx_DelivaryDate { get; set; }
    }
}
