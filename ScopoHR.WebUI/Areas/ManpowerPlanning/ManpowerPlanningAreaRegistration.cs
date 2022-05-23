using System.Web.Mvc;

namespace ScopoHR.WebUI.Areas.ManpowerPlanning
{
    public class ManpowerPlanningAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "ManpowerPlanning";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "ManpowerPlanning_default",
                "ManpowerPlanning/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}