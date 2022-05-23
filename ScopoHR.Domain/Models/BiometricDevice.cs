using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Domain.Models
{
    public class BiometricDevice : BaseEntity
    {
        public int Id { get; set; }
        public string IpAddress { get; set; }
        public int Port { get; set; }
        public int MachineNumber { get; set; }
        public DateTime InServiceSince { get; set; }
        public DateTime? LastSync { get; set; }
    }
}
