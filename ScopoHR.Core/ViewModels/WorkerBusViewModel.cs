using ScopoHR.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Core.ViewModels
{
    public class WorkerBusViewModel:BaseEntity
    {
        public int? WorkerBusID { get; set; }
        public string LocationName { get; set; }
        public DateTime? InTimeDate { get; set; }
        public DateTime? InTime { get; set; }
        public int LocationID { get; set; }
        public List<string> empList { get; set; }

    }
}
