using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Domain.Models
{
    public class Sys_AttendanceBonus : BaseEntity
    {
        [Key]
        public int AttendanceBonusID { get; set; }
        public decimal AttendanceBonus { get; set; }
        public bool IsApplicable { get; set; }
    }
}
