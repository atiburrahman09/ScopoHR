using NtitasCommon.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Core.ViewModels
{
    public class UserLoginAuditViewModel : IUserLoginAudit
    {
        public int Id { get; set; }
        public int BranchId { get; set; }
        public string UserId { get; set; }
        public DateTime LoggedInTime { get; set; }
        public DateTime LoggedOutTime { get; set; }
    }
}
