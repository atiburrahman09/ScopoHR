
using NtitasCommon.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Core.ViewModels
{
    public class UserCacheViewModel : IUserCache
    {
        public string UserId { get; set; }
        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime LoggedInTime { get; set; }
    }
}
