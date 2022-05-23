using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Domain.Models
{
    public class Promotion:BaseEntity
    {
        public int PromotionID { get; set; }
        public int EmployeeID { get; set; }
        public string CardNo { get; set; }
        public Nullable<DateTime> PromotionDate { get; set; }
        public string Remarks { get; set; }
        public int NewDesignationID { get; set; }
        public int PreviousDesignationID { get; set; }
    }
}
