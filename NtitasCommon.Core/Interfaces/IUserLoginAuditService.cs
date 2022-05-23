using ScopoHR.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NtitasCommon.Core.Interfaces
{
    public interface IUserLoginAuditService
    {
        void insert(string username, int branchId);
        void clearAllLogin(string username, int branchId = 0);
        UserLoginAuditViewModel GetCurrentLoggedIn(string username);
        UserCacheViewModel GetUserCache(string username);
    }
}
