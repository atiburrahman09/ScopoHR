using ScopoHR.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NtitasCommon.Core.Interfaces
{
    /// <summary>
    /// IUserHelper interface to be inherited by UserHelper
    /// </summary>
    public interface IUserHelper
    {
        /// <summary>
        /// Returns instance of UserCacheViewModel current user. Reads from cache or creates cache if empty. If not logged in returns null
        /// </summary>
        /// <returns>UserCacheViewModel when logged in or null if not</returns>
        UserCacheViewModel Get();

        /// <summary>
        /// Returns instance of UserCacheViewModel current user. Reads from cache or creates cache if empty. If not logged in returns null
        /// </summary>
        /// <returns>UserCacheViewModel when logged in or null if not</returns>
        UserCacheViewModel Get(string name);

        /// <summary>
        /// Invalidated the user cache for the specified <paramref name="username"/>
        /// </summary>
        /// <param name="username">user name</param>
        void InvalidateCache(string username);

        /// <summary>
        /// Returns the username of the logged-in user
        /// </summary>
        /// <returns> Returns the username of the logged-in user </returns>
        string LoggedinUsername();

        /// <summary>
        /// Returns the users pagersize cache value or default value from web.config if not set for the user
        /// </summary>
        int PagerSize { get; }
    }
}
