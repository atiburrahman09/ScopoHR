using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Domain.Models
{
    public class Gender : BaseEntity
    {
        public int GenderID { get; set; }
        public string GenderName { get; set; }
    }
}
