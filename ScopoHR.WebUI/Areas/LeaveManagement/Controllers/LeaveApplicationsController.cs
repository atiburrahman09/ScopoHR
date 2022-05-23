using ScopoHR.Core.Common;
using ScopoHR.Core.Helpers;
using ScopoHR.Core.Services;
using ScopoHR.Core.ViewModels;
using ScopoHR.Domain.Models;
using ScopoHR.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ScopoHR.WebUI.Areas.LeaveManagement.Controllers
{
    //[Authorize(Roles = AppRoles.SuperUser + ", " + AppRoles.Admin+ "," + AppRoles.LeaveApplication)]
    public class LeaveApplicationsController : Controller
    {
        private LeaveApplicationService leaveApplicatinService;
        private LeaveTypeService leaveTypeService;
        private EmployeeService employeeService;
        private LeaveMappingService leaveMappingService;
        private SalaryService salaryService;
        private MaternityService maternityService;

        public LeaveApplicationsController(
            LeaveApplicationService leaveApplicationService,
            LeaveTypeService leaveTypeService,
            LeaveMappingService leaveMappingService,
            EmployeeService employeeService,
            SalaryService salaryService,
            MaternityService maternityService
            )
        {
            this.leaveMappingService = leaveMappingService;
            this.leaveApplicatinService = leaveApplicationService;
            this.employeeService = employeeService;
            this.leaveTypeService = leaveTypeService;
            this.salaryService = salaryService;
            this.maternityService = maternityService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetAll()
        {
            var branchId = UserHelper.Instance.Get().BranchId;
            try
            {
                return Json(leaveApplicatinService.GetAll(branchId, User.Identity.Name), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult CreateApp(LeaveApplicationViewModel LeaveAppVM)
        {
            if (ModelState.IsValid)
            {
                var leaveApplicationID=leaveApplicatinService.Create(LeaveAppVM, User.Identity.Name);
                if (LeaveAppVM.LeaveTypeID == 4)
                {
                    SalarySummaryViewModel mVM = salaryService.GetSalaryData(LeaveAppVM.EmployeeID,LeaveAppVM.FromDate);
                    maternityService.CreateMaternityFromLeave(mVM.TotalPD,mVM.TotalSalary,LeaveAppVM.FromDate,LeaveAppVM.TotalDays,LeaveAppVM.EmployeeID, User.Identity.Name, leaveApplicationID);

                }
                return Json(LeaveAppVM);
            }
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json(false);
        }

        public JsonResult GetLeaveTypes()
        {
            List<LeaveTypeViewModel> leaveTypeList = leaveTypeService.GetAll();
            return Json(leaveTypeList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Update(LeaveApplicationViewModel leaveAppVM)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Invalid data submitted!");
            }

            try
            {
                leaveApplicatinService.Update(leaveAppVM, User.Identity.Name);
                return Json("Update successfully!");
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message);
            }
        }

        public JsonResult GetEmployeeDropDownByKeyword(string inputString)
        {
            if (String.IsNullOrEmpty(inputString))
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Please enter employee name or card no.", JsonRequestBehavior.AllowGet);
            }

            try
            {
                var id = UserHelper.Instance.Get().BranchId;
                return Json(employeeService.GetEmployeeDropDownByKeyword(inputString, id), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult DeleteApplication(int appID)
        {
            if (appID < 1)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Please select leave application first.", JsonRequestBehavior.AllowGet);
            }

            try
            {
                
                return Json(leaveApplicatinService.DeleteApplication(appID), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetAllAppsByEmployeeCardNo(string cardNo)
        {
            var branchId = UserHelper.Instance.Get().BranchId;
            try
            {
                return Json(leaveApplicatinService.GetAllAppsByEmployeeCardNo(branchId, User.Identity.Name, cardNo), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult DuplicateLeaveCheck(int employeeID,DateTime fromDate,DateTime toDate)
        {
            
            try
            {
                return Json(leaveApplicatinService.DuplicateLeaveCheck(employeeID, fromDate, toDate), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
    }
}