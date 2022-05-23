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

namespace ScopoHR.WebUI.Areas.EmployeeManagement.Controllers
{
    [Authorize(Roles = AppRoles.SuperUser + ", " + AppRoles.Admin + "," + AppRoles.SalaryCalculation)]
    public class SalaryController : Controller
    {
        private EmployeeService employeeService;
        private SalaryService salaryService;
        private DepartmentService departmentService;
        private ProductionService productionService;

        public SalaryController(EmployeeService _employeeService, SalaryService _salaryService, DepartmentService _departmentService, ProductionService _productionService)
        {
            this.employeeService = _employeeService;
            this.salaryService = _salaryService;
            this.departmentService = _departmentService;
            this.productionService = _productionService;

        }
        // GET: EmployeeManagement/Salary
        public ActionResult Index()
        {
            return View();
        }



        public ActionResult GoHome()
        {
            return View();
        }
       
        public JsonResult GetMonth()
        {
            try
            {
                List<MonthViewModel> monthList = salaryService.GetMonth();
                return Json(new { data = monthList }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message,JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult GetYear()
        {
            try
            {
                List<YearViewModel> yearList = salaryService.GetYear();
                return Json(new { data = yearList }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult GetSalaryByMonthYear(int MonthId, string YearId, int DepartmentID , string FloorID)
        {
            try
            {
                var id = UserHelper.Instance.Get().BranchId;
                List<SalarySheetViewModel> salaryList = salaryService.GetSalaryByMonthYear(MonthId , YearId , DepartmentID, FloorID , id);
                return Json(new { data = salaryList }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult GetAllDepartments()
        {
            try
            {
                List<DepartmentViewModel> deparmentList = departmentService.GetAll();
                return Json(new { data = deparmentList }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult GetAllProductionFloor()
        {
            try
            {
                List<ProductionFloorLineViewModel> productionFList = productionService.GetProductionFloorList();
                return Json(productionFList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
    }
}