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
    public class TaxController : Controller
    {

        private TaxService taxService;
        private EmployeeService employeeService;

        public TaxController(TaxService taxService, EmployeeService employeeService)
        {
            this.taxService = taxService;
            this.employeeService = employeeService;
        }
        // GET: Loan/Tax
        public ActionResult Index()
        {
            return View();
        }


        public JsonResult GetTaxByEmployeeID(int employeeID)
        {
            try
            {
                List<TaxViewModel> taxVM = taxService.GetTaxByEmployeeID(employeeID);
                return Json(taxVM, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult SaveInformation(TaxViewModel taxVM)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Invalid Data Submitted!", JsonRequestBehavior.AllowGet);
            }
            try
            {
                if (taxVM.TaxID > 0)
                {
                    if (!taxService.isExists(taxVM))
                    {
                        taxService.Update(taxVM, User.Identity.Name);
                        return Json("Successfully updated!", JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { res = false, MSG = "Duplicate Data!" }, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    if (!taxService.isExists(taxVM))
                    {
                        taxService.SaveTax(taxVM, User.Identity.Name);
                        return Json("Successfully created!", JsonRequestBehavior.AllowGet);
                    }
                    return Json(new {res=false, MSG = "Duplicate Data!"}, JsonRequestBehavior.AllowGet);
                }
               


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