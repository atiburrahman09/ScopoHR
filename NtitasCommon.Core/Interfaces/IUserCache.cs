using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NtitasCommon.Core.Interfaces
{
    /// <summary>
    /// IUserCache interface to be implemented by user cache view model
    /// </summary>
    public interface IUserCache
    {
        string UserId { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        DateTime LoggedInTime { get; set; }

    }
}
