using NtitasCommon.Core.Common;
using NtitasCommon.Core.Interfaces;
using ScopoHR.Core.ViewModels;
using System;
using System.Collections.Concurrent;
using System.Web;

namespace ScopoHR.Core.Helpers
{
   
    /// <summary>
    /// Class to get UserCacheModel
    /// </summary>
    public class UserHelper : IUserHelper
    {
        public static IUserHelper Instance { get; set; }

        /// <summary>
        /// Cache key for the user dictionary
        /// </summary>
        private const string USER_CACHE_DICTIONARY = "USER_CACHE_DICTIONARY";

        private static readonly String pagersize = "pagersize";

        /// <summary>
        /// Object is used as a lock when the user cache concurrent dictionary needs to created
        /// </summary>
        private static readonly object GET_DICTIONARY_LOCK = new object();

        private ICookieAccessor _cookieAccessor;
        private IUserLoginAuditService _userLoginAuditService;
        private IConfig _config;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="cookieAccessor"></param>
        /// <param name="userProfileService"></param>
        public UserHelper(ICookieAccessor cookieAccessor, IUserLoginAuditService userLoginAuditService, IConfig config)
        {
            _cookieAccessor = cookieAccessor;
            _userLoginAuditService = userLoginAuditService;
            _config = config;
        }

        /// <summary>
        /// Returns dictionary from http cache - if not found creates a new one
        /// </summary>
        /// <returns> returns dictionary from http cache - if not found creates a new one </returns>
        private ConcurrentDictionary<string, UserCacheViewModel> GetDictionary()
        {
            try
            {
                if (System.Web.HttpContext.Current.Cache[USER_CACHE_DICTIONARY] == null)
                {
                    lock (GET_DICTIONARY_LOCK)
                    {
                        if (HttpContext.Current.Cache[USER_CACHE_DICTIONARY] == null)
                        {
                            HttpContext.Current.Cache[USER_CACHE_DICTIONARY] = new ConcurrentDictionary<string, UserCacheViewModel>();
                        }
                    }
                }

                return (ConcurrentDictionary<string, UserCacheViewModel>)HttpContext.Current.Cache[USER_CACHE_DICTIONARY];
            }
            catch (Exception ex)
            {
                // TODO
                // Should be Log here for debuging in production
                return null;
            }
        }

        /// <summary>
        /// Returns instance of UserCacheModel current user. Reads from cache or creates cache if empty. If not logged in returns null
        /// </summary>
        /// <returns>UserCacheModel when logged in or null if not</returns>
        public virtual UserCacheViewModel Get()
        {
            if (!HttpContext.Current.Request.IsAuthenticated)
                return null;

            return Get(HttpContext.Current.User.Identity.Name);
        }

        /// <summary>
        /// Returns the username of the logged-in user
        /// </summary>
        /// <returns> Returns the username of the logged-in user </returns>
        public virtual string LoggedinUsername()
        {
            return HttpContext.Current.User.Identity.IsAuthenticated ? HttpContext.Current.User.Identity.Name : null;
        }

        /// <summary>
        /// Returns instance of UserCacheModel for specified username. Reads from cache or creates cache if empty
        /// </summary>
        /// <param name="userName">Username to retrieve UserCacheModel, defaults to current username</param>
        /// <returns>UserCacheModel instance based on username</returns>
        public virtual UserCacheViewModel Get(string userName)
        {
            try
            {
                ConcurrentDictionary<string, UserCacheViewModel> userCache = GetDictionary();
                UserCacheViewModel user = null;

                if (!userCache.TryGetValue(userName, out user))
                {

                    user = _userLoginAuditService.GetUserCache(userName.ToLower());

                    if (user == null)
                        throw new Exception("UserCacheModel tried to access username that doesn't exist: " + userName);

                    userCache.TryAdd(userName, user);
                }

                return user;
            }
            catch (Exception ex)
            {
                // TODO
                // Should be Log here for debuging in production
                return null;
            }
        }

        /// <summary>
        /// Invalidated the user cache for the specified <paramref name="username"/>
        /// </summary>
        /// <param name="username">user name</param>
        public virtual void InvalidateCache(string username)
        {
            if (String.IsNullOrWhiteSpace(username))
                return;

            try
            {
                ConcurrentDictionary<string, UserCacheViewModel> userCache = GetDictionary();
                UserCacheViewModel removedUser = null;

                userCache.TryRemove(username, out removedUser);
            }
            catch (Exception ex)
            {
                // TODO
                // Should be Log here for debuging in production
            }
        }

        /// <summary>
        /// Returns the users pagersize cache value or default value from web.config if not set for the user
        /// </summary>
        public virtual int PagerSize
        {
            get
            {

                if (_cookieAccessor.requestCookies[pagersize] != null)
                {
                    return int.Parse(_cookieAccessor.requestCookies[pagersize].Value);
                }
                else
                {
                    HttpCookie cookie = new HttpCookie(pagersize, _config.DefaultPageSize.ToString());
                    cookie.Path = "/";

                    if (_cookieAccessor.responseCookies != null)
                        _cookieAccessor.responseCookies.Add(cookie);

                    return _config.DefaultPageSize;
                }
            }
        }
    }
}
