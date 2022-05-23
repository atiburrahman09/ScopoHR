using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Core.ViewModels
{
    public class ProductionFloorLineViewModel 
    {
        public int ProductionFloorLineID { get; set; }
        public string Floor { get; set; }
        public string Line { get; set; }
        public string ModifiedBy { get; set; }
    }
}
