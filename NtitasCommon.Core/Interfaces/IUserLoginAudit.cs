using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NtitasCommon.Core.Interfaces
{
    public interface IUserLoginAudit
    {
        int Id { get; set; }        
        string UserId { get; set; }
        DateTime LoggedInTime { get; set; }
        DateTime LoggedOutTime { get; set; }
    }
}
