using ScopoHR.Core.Common;
using ScopoHR.Core.Helpers;
using ScopoHR.Core.Services;
using ScopoHR.WebUI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ScopoHR.WebUI.Areas.AdminPanel.Controllers
{
    [Authorize(Roles = AppRoles.SuperUser + ", " + AppRoles.Admin)]
    public class DatabaseAdministrationController : Controller
    {
        private OleDbHelper oleDbHelper;
        private AttendanceService attendanceService;
        public DatabaseAdministrationController(AttendanceService attService)
        {
            attendanceService = attService;
            oleDbHelper = new OleDbHelper();
        }

        // GET: AdminPanel/DatabaseAdministration
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult SyncIn(int shiftId)
        {
            try
            {
                oleDbHelper.PullData(DateTime.Now);
                attendanceService.SyncInTime(UserHelper.Instance.Get().BranchId, DateTime.Now, shiftId.ToString(), User.Identity.Name);
                return Json("Successfully Updated!", JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult SyncOut(int shiftId)
        {
            try
            {
                oleDbHelper.PullData(DateTime.Now);
                attendanceService.SyncOutTime(UserHelper.Instance.Get().BranchId, DateTime.Now, shiftId.ToString(), User.Identity.Name);
                return Json("Successfully Updated!", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

    }
}