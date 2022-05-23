using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Domain.Models
{
    public class Sys_Grace:BaseEntity
    {
        [Key]
        public int GraceID { get; set; }
        public int BeforeInTimeGrace { get; set; }
        public int AfterInTimeGrace { get; set; }
        public int BeforeOutTimeGrace { get; set; }
        public int AfterOutTimeGrace { get; set; }
        public bool IsApplicable { get; set; }
    }
}
