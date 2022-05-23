using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Domain.Models
{
    public class Notice : BaseEntity
    {
        public int NoticeID { get; set; }
        public string NoticeTitle { get; set; }
        
        [Column(TypeName = "ntext")]
        public string NoticeDetail { get; set; }

        public DateTime? NoticeDate { get; set; }
        public int BranchID { get; set; }

        public ICollection<Employee> Employees;
        public ICollection<Notice> Notices;
    }
}
