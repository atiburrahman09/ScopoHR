using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Domain.Models
{
    public class PublishNotice : BaseEntity
    {
        [Key]
        public int PublishID { get; set; }
        public int NoticeID { get; set; }
        public int EmployeeID { get; set; }
        public string NoticeTitle { get; set; }
        public string NoticeDetails { get; set; }

        public virtual Notice Notice { get; set; }
        public virtual Employee Employee { get; set; }
    }
}
