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
    [Authorize(Roles = AppRoles.SuperUser + ", " + AppRoles.Admin + "," + AppRoles.SalaryIncrement)]
    public class IncrementSalaryController : Controller
    {

        private ProductionService productionService;
        private OfficeTimingService officeTimingService;
        private SalaryIncrementService salaryIncrementService;
        private EmployeeService employeeService;

        public IncrementSalaryController(OfficeTimingService officeTimingService, ProductionService _productionService, SalaryIncrementService salaryIncrementService, EmployeeService employeeService)
        {
            this.officeTimingService = officeTimingService;
            this.productionService = _productionService;
            this.salaryIncrementService = salaryIncrementService;
            this.employeeService = employeeService;
        }


        // GET: EmployeeManagement/IncrementSalary
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetAllProductionFloor()
        {
            var productionFList = productionService.GetAll();
            return Json(productionFList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetWorkingShift()
        {
            try
            {
                List<DropDownViewModel> workingShiftList = officeTimingService.GetWorkingShiftsDropDown();
                return Json(workingShiftList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetSalaryIncrement(ReportFilteringViewModel siVM)
        {
            try
            {
                List<IncrementReportViewModel> incrementData = salaryIncrementService.GetSalaryIncrement(siVM, UserHelper.Instance.Get().BranchId);
                return Json(incrementData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult SaveSalaryIncrement(List<IncrementReportViewModel> SIVM)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Invalid Data Submitted!", JsonRequestBehavior.AllowGet);
            }
            try
            {

                salaryIncrementService.SaveSalaryIncrement(SIVM, User.Identity.Name);
                return Json("Successfully created!", JsonRequestBehavior.AllowGet);


            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }



        public ActionResult SalaryIncrement()
        {
            return View();
        }

        public JsonResult GetEmployeeeSalaryIncrementDetailsById(int EmployeeID)
        {
            try
            {
                List<SalaryIncrementViewModel> sIVM = salaryIncrementService.GetEmployeeeSalaryIncrementDetailsById(EmployeeID);
                return Json(sIVM, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult SaveInformation(SalaryIncrementViewModel SIVM)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Invalid Data Submitted!", JsonRequestBehavior.AllowGet);
            }
            try
            {

                salaryIncrementService.SaveEmployeeSalaryIncrement(SIVM, User.Identity.Name);
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