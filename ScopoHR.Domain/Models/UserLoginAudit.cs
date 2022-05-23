using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Domain.Models
{
    public class UserLoginAudit
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public int BranchId { get; set; }
        public DateTime LoggedInTime { get; set; }
        public DateTime? LoggedOutTime { get; set; }
        public virtual Branch Branch { get; set; }
    }
}
