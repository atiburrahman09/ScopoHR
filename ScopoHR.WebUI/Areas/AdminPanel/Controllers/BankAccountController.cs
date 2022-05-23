using ScopoHR.Core.Helpers;
using ScopoHR.Core.Services;
using ScopoHR.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ScopoHR.WebUI.Areas.AdminPanel.Controllers
{
    public class BankAccountController : Controller
    {
        private EmployeeService empService;
        private BankAccountService bankService;

        public BankAccountController(BankAccountService bankService, EmployeeService empService)
        {
            this.bankService = bankService;
            this.empService = empService;
        }
        // GET: AdminPanel/BankAccount
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetAllBankAccount()
        {
            try
            {
                var data = bankService.GetAllBankAccount();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(e.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult getEmployeeeBankAccountDetailsById(int EmployeeID)
        {
            try
            {
                var data = bankService.GetEmployeeeBankAccountDetailsById(EmployeeID);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(e.Message, JsonRequestBehavior.AllowGet);
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
                return Json(empService.GetEmployeeDropDownByKeyword(inputString, id), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult SaveAccount(BankSalaryViewModel bsVM)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Invalid Data Submitted!");
            }
            try
            {
                bsVM.ModifiedBy = User.Identity.Name;
                bankService.Save(bsVM);
                return Json("Data successfully saved!");
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message);
            }
        }

        public JsonResult IsUniqueAccount(int EmployeeID,string Bank)
        {
            try
            {
                if (!bankService.IsUniqueAccount(EmployeeID,Bank))
                {
                    return Json("Account is unique!", JsonRequestBehavior.AllowGet);
                }
                Response.StatusCode = (int)HttpStatusCode.PartialContent;
                return Json("Account already exists!", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
    }
}