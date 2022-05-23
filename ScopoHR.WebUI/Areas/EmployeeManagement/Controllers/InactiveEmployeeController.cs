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
    [Authorize(Roles = AppRoles.SuperUser + ", " + AppRoles.Admin + "," + AppRoles.InActiveEmployee)]
    public class InactiveEmployeeController : Controller
    {
        private EmployeeService employeeService;

        public InactiveEmployeeController (EmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        // GET: EmployeeManagement/InactiveEmployee
        public ActionResult Index()
        {
            return View();
        }


        public JsonResult getInActiveEmployeeeDetailsById(int EmployeeID)
        {
            try
            {
                InActiveEmployeeViewModel inactiveEmployeeInfo = employeeService.getInActiveEmployeeeDetailsById(EmployeeID);
                return Json(inactiveEmployeeInfo, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
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

        public JsonResult GetRecentInActiveEmployees()
        {
            return Json(employeeService.GetRecentInActiveEmployees(UserHelper.Instance.Get().BranchId), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetRecentEmployees()
        {
            return Json(employeeService.GetRecentEmployees(UserHelper.Instance.Get().BranchId), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveInformation(InActiveEmployeeViewModel info)
        {
            var id = UserHelper.Instance.Get().BranchId;
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Invalid Data Submitted!");
            }
            try
            {
                if (info.ID == 0)
                {
                    employeeService.SaveInactiveEmployeeInfo(info, id, User.Identity.Name);
                    return Json("Successfully created!");
                }
                else
                {
                    info.ModifiedBy = User.Identity.Name;
                    employeeService.UpdateInactiveEmployeeInfo(info, id, User.Identity.Name);
                    return Json("Succeessfully updated!");
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message);
            }
        }
    }
}