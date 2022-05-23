using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Domain.Models
{
   public class ProductionFloorLine : BaseEntity
    {
        public int ProductionFloorLineID { get; set; }
        public string Floor { get; set; }
        public string Line { get; set; }
        public int BranchID { get; set; }
    }
}
