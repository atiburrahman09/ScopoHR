using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Domain.Models
{
    public abstract class BaseAttendance : BaseEntity
    {
        [Key]
        public virtual int Id { get; set; }
        public virtual string CardNo { get; set; }
    }

    public class Attendance : BaseAttendance
    {   
        public DateTime InOutTime { get; set; }
        public string Remarks { get; set; }
    }
    
    public class DailyAttendance : BaseAttendance
    {
        public int EmployeeId { get; set; }
        public DateTime Date { get; set; }
        public DateTime? InTime { get; set; }
        public DateTime? OutTime { get; set; }        
        public string Status { get; set; }
        public short? OverTime { get; set; }
        public string Remarks { get; set; }
    }

    public class SpecialAttendance : BaseAttendance
    {
        public DateTime InOutTime { get; set; }
        public string Remarks { get; set; }
    }
}
