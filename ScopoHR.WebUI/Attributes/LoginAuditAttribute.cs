using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using NtitasCommon.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ScopoHR.Web.Attributes
{
    public class LoginAuditAttribute : AuthorizeAttribute
    {
        private IUserLoginAuditService _loginAuditService
        {
            get
            {
                return DependencyResolver.Current.GetService<IUserLoginAuditService>();
            }
        }

        private IUserHelper _userHelper
        {
            get
            {
                return DependencyResolver.Current.GetService<IUserHelper>();
            }
        }
        
        private void signOut(HttpContextBase context)
        {
            context.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var isAuthorized = base.AuthorizeCore(httpContext);
            if (!isAuthorized)
            {
                signOut(httpContext);
                return false;
            }

            if (_userHelper.Get() == null)
            {
                signOut(httpContext);
                return false;
            }                

            if (_loginAuditService.GetCurrentLoggedIn(httpContext.User.Identity.Name) == null)
            {
                signOut(httpContext);
                return false;
            }
            return true;
        }
    }
}