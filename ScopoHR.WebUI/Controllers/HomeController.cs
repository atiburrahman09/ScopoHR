using ScopoHR.Core.Services;
using ScopoHR.Web.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ScopoHR.WebUI.Controllers
{
    [Authorize]    
    public class HomeController : Controller
    {
        private EmployeeService employeeService;
        public HomeController(EmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetDashboardData()
        {
            try
            {
                var data = employeeService.GetDashboardData();

                return Json(new { data = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message);
            }
        }
    }
}