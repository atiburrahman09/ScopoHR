using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Domain.Models
{
   public class YearMapping : BaseEntity
    {
        public int YearMappingID { get; set; }
        public int Year { get; set; }
        public bool IsOpen { get; set; }
    }
}
