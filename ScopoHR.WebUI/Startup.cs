using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using System.Web.Http;
using Hangfire;
using ScopoHR.Core.Helpers;
using System.Web;
using ScopoHR.WebUI.Helpers;

[assembly: OwinStartup(typeof(ScopoHR.WebUI.Startup))]

namespace ScopoHR.WebUI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
                );
            app.UseWebApi(config);
            
            ConfigureAuth(app);
        }
    }
}
