using ScopoHR.Core.Helpers;
using ScopoHR.Core.Services;
using ScopoHR.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ScopoHR.WebUI.Areas.Common.Controllers
{
    public class MaternityController : Controller
    {
        private MaternityService mtService;
        private EmployeeService employeeService;
        public MaternityController(MaternityService mtService, EmployeeService employeeService)
        {
            this.mtService = mtService;
            this.employeeService = employeeService;
        }
        // GET: Common/Maternity
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult SaveMaternity(MaternityViewModel mtVM)
        {

            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Invalid Data Submitted!");
            }
            try
            {
                if (mtVM.MaternityID > 0)
                {

                    mtService.UpdateMaternity(mtVM, User.Identity.Name);
                    Response.StatusCode = (int)HttpStatusCode.Created;
                    return Json(new { Message = "Maternity updated." });
                }
                else
                {
                    mtService.CreateMaternity(mtVM, User.Identity.Name);
                    Response.StatusCode = (int)HttpStatusCode.Created;
                    return Json(new { Message = "Maternity created." });

                }



            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message);
            }
        }
        public JsonResult GetEmployeeeMaternityDetailsById(int EmployeeID)
        {
            try
            {
                return Json(mtService.GetEmployeeeMaternityDetailsById(EmployeeID), JsonRequestBehavior.AllowGet);
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

        public JsonResult GetRecentEmployees()
        {
            return Json(employeeService.GetRecentEmployees(UserHelper.Instance.Get().BranchId), JsonRequestBehavior.AllowGet);
        }


    }
}