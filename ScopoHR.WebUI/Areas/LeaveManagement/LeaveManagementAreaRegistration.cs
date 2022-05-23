using System.Web.Mvc;

namespace ScopoHR.WebUI.Areas.LeaveManagement
{
    public class LeaveManagementAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "LeaveManagement";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "LeaveManagement_default",
                "LeaveManagement/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}