using ScopoHR.Core.Helpers;
using ScopoHR.Core.Services;
using ScopoHR.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ScopoHR.WebUI.Areas.Loan.Controllers
{
    public class FineController : Controller
    {
        private EmployeeService employeeService;
        private FineService fineService;
        public FineController(EmployeeService employeeService, FineService fineService)
        {
            this.employeeService = employeeService;
            this.fineService = fineService;
        }
        // GET: Loan/Fine
        public ActionResult Index()
        {
            return View();
        }


        public JsonResult GetFineByEmployeeID(int employeeID)
        {
            try
            {
                List<FineViewModel> taxVM = fineService.GetFineByEmployeeID(employeeID);
                return Json(taxVM, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult SaveInformation(FineViewModel fineVM)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Invalid Data Submitted!", JsonRequestBehavior.AllowGet);
            }
            try
            {
                fineService.SaveFine(fineVM, User.Identity.Name);
                return Json("Successfully created!", JsonRequestBehavior.AllowGet);
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