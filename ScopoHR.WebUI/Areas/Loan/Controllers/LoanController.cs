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

namespace ScopoHR.WebUI.Areas.Loan.Controllers
{
    [Authorize(Roles = AppRoles.SuperUser + ", " + AppRoles.Admin + "," + AppRoles.EmployeeSetup + "," + AppRoles.AdvanceSalary)]
    public class LoanController : Controller
    {
        private LoanService loanService;
        private EmployeeService employeeService;
        // GET: EmployeeManagement/AdvanceSalary
        public LoanController(LoanService loanService, EmployeeService employeeService)
        {
            this.loanService = loanService;
            this.employeeService = employeeService;
        }
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetLoanByEmployeeID(int employeeID)
        {
            try
            {
                List<LoanVIewModel> loanVM = loanService.GetLoanByEmployeeID(employeeID);
                return Json(loanVM, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetLoanDetailsByLoanID(int loanID)
        {
            try
            {
                List<LoanDetailsViewModel> loanDetailsVM = loanService.GetLoanDetailsByLoanID(loanID);
                return Json(loanDetailsVM, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult SaveInformation(LoanVIewModel loanVM)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Invalid Data Submitted!", JsonRequestBehavior.AllowGet);
            }
            try
            {

                loanService.SaveLoan(loanVM, User.Identity.Name);
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