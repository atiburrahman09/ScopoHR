using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Domain.Models
{
    public class WorkerBus
    {
        public int WorkerBusID { get; set; }
        public string LocationName { get; set; }
        public string CardNo { get; set; }
        public int LocationID { get; set; }
    }
}
