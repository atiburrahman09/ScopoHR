using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace NtitasCommon.Core.Interfaces
{
    /// <summary>
    /// Class that accesses the cookie collection from current request HttpContext
    /// </summary>
    public interface ICookieAccessor
    {
        /// <summary>
        /// Returns cookie collection from HttpContext.Current.Request.Cookies
        /// </summary>
        HttpCookieCollection requestCookies { get; }

        /// <summary>
        /// Returns cookie collection from HttpContext.Current.Reponse.Cookies
        /// </summary>
        HttpCookieCollection responseCookies { get; }

        /// <summary>
        /// Remove a cookie from the HttpContext.Current.Response.Cookies collection
        /// </summary>
        /// <param name="cookieName"> name of the cookie to remove </param>
        void ClearCookie(string cookieName);

        /// <summary>
        /// Updates the value of an existing cookie or creates a new one
        /// </summary>
        /// <param name="cookieName"> name of the cookie to update </param>
        /// <param name="value"> value to store in the cookie </param>
        void UpdateCookie(string cookieName, object value);
    }
}
