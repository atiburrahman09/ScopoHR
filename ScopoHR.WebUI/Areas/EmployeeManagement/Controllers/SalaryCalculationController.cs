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
    public class SalaryCalculationController : Controller
    {
        private UnitOfWork unitOfwork;
        private SalaryCalculationService salaryCalculationService;

        public SalaryCalculationController(UnitOfWork unitOfwork,SalaryCalculationService salaryCalculationService)
        {
            this.unitOfwork = unitOfwork;
            this.salaryCalculationService = salaryCalculationService;
        }


        // GET: EmployeeManagement/SalaryCalculation
        public ActionResult Index()
        {
            return View();
        }
        
        public JsonResult CalculateSalary(SalaryCalculationViewModel salaryVM)
        {
            try
            {                
                int branchId = UserHelper.Instance.Get().BranchId;
                salaryCalculationService.GenerateSalary(salaryVM,branchId);
                return Json("Salary Generated!", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }            
        }
    }
}