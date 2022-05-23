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

namespace ScopoHR.WebUI.Areas.LeaveManagement.Controllers
{
    //[Authorize(Roles = AppRoles.SuperUser + ", " + AppRoles.LeaveApprover + "," + AppRoles.Admin)]
    public class LeaveApprovalController : Controller
    {
        
        private LeaveApplicationService leaveApplicationService;
        private LeaveMappingService leaveMappingService;
        private AttendanceService attendanceService;

        public LeaveApprovalController(
            LeaveApplicationService leaveApplicationService, 
            LeaveMappingService leaveMappingService,
            AttendanceService attendanceService            
            )
        {
            this.leaveApplicationService = leaveApplicationService;
            this.leaveMappingService = leaveMappingService;
            this.attendanceService = attendanceService;
        }
        // GET: LeaveManagement/LeaveApproval
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetApplications(LeaveApplicationStatus applicationStatus, int pageNo)
        {
            var branchId = UserHelper.Instance.Get().BranchId;
            var skip = Page.Size * pageNo;
            try
            {
                int count = 0;
                var apps = leaveApplicationService.GetAll(applicationStatus, branchId, Page.Size, skip, out count);
                return Json(new { apps = apps, count = count}, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult UpdateApplications(List<LeaveApplicationViewModel> appList)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.NotAcceptable;
                return Json(false);
            }

            try
            {
                leaveApplicationService.Update(appList, User.Identity.Name);
                return Json(true);
            }
            catch(Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message);
            }
        }
        
        public JsonResult GetLeaveMapping(int employeeID)
        {
            try
            {
                var mapping = leaveMappingService.GetLeaveMapping(employeeID);
                return Json(mapping, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult ApproveApplications(List<LeaveApplicationViewModel> List)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.NotAcceptable;
                return Json(false);
            }

            try
            {
                var approaved =  leaveMappingService.UpdateLeaveMapping(List, User.Identity.Name);
                if(approaved.Count > 0)
                {
                    leaveApplicationService.Update(approaved, User.Identity.Name);
                    // Update daily attendances
                    attendanceService.UpdateDailyAttendance(approaved);
                }

                if(List.Count - approaved.Count > 0)
                {
                    Response.StatusCode = (int)HttpStatusCode.PartialContent;
                    return Json((List.Count - approaved.Count).ToString() + " of them exceeds allocated leave, therefor not approaved!");
                }
                return Json(List.Count.ToString() + " application approaved!");
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message);
            }
        }
    }
}