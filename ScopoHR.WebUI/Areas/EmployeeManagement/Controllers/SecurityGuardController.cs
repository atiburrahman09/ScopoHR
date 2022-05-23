using ScopoHR.Core.Common;
using ScopoHR.Core.Helpers;
using ScopoHR.Core.Services;
using ScopoHR.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ScopoHR.WebUI.Areas.EmployeeManagement.Controllers
{
    //[Authorize(Roles = AppRoles.SuperUser + ", " + AppRoles.Admin + "," + AppRoles.Security)]
    public class SecurityGuardController : Controller
    {
        private EmployeeService employeeService;
        private SecurityGuardRosterService sgRosterService;
        public SecurityGuardController(EmployeeService employeeService, SecurityGuardRosterService sgRosterService)
        {
            this.employeeService = employeeService;
            this.sgRosterService = sgRosterService;
        }
        // GET: EmployeeManagement/SecurityGuard
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetAllGuards()
        {
            var branchId = UserHelper.Instance.Get().BranchId;
            var data = employeeService.GetAllGuards(branchId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveRoster(List<SecurityGuardRosterViewModel> roster)
        {
            if (!ModelState.IsValid)
            {
                var errList = ModelState.Values.SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage)
                                        .AsEnumerable();
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(errList);
            }
            try
            {
                sgRosterService.SaveRoster(roster, User.Identity.Name);
                return Json("Successfully Saved!");
            }
            catch(Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message);
            }
            
        }


        public JsonResult SearchRoster(SearchAttendanceViewModel search)
        {
            try
            {
                var list = sgRosterService.SearchRoster(Convert.ToDateTime(search.FromDate));
                if (list != null && list.Count() > 0)
                {
                    var shiftA = list.Where(x => x.ShiftId == "3").AsEnumerable();
                    var shiftB = list.Where(x => x.ShiftId == "4").AsEnumerable();
                    var shiftC = list.Where(x => x.ShiftId == "5").AsEnumerable();
                    var shiftD = list.Where(x => x.ShiftId == "6").AsEnumerable();
                    return Json(new { shiftA = shiftA, shiftB = shiftB, shiftC = shiftC, shiftD = shiftD }, JsonRequestBehavior.AllowGet);
                }
                Response.StatusCode = (int)HttpStatusCode.NoContent;
                return Json("No data found!", JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
    }
}