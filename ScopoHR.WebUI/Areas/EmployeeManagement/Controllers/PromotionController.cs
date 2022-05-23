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
    [Authorize(Roles = AppRoles.SuperUser + ", " + AppRoles.Admin + "," + AppRoles.Promotion)]
    public class PromotionController : Controller
    {
        private PromotionService promotionService;
        private EmployeeService employeeService;
        private DesignationService designationService;
        private DepartmentService departmentService;
        public PromotionController(PromotionService promotionService, EmployeeService employeeService, DesignationService designationService, DepartmentService departmentService)
        {
            this.promotionService = promotionService;
            this.employeeService = employeeService;
            this.designationService = designationService;
            this.departmentService = departmentService;
        }
        // GET: EmployeeManagement/Promotion
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetAllDepartments()
        {
            try
            {
                return Json(departmentService.GetAll(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetDesignationListByDepartment(int DepartmentID)
        {
            try
            {
                var designationList = designationService.GetAllDesignationByDepartment(DepartmentID);
                return Json(designationList, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetEmployeeePromotionDetailsById(int EmployeeID)
        {
            try
            {
                List<PromotionViewModel> sIVM = promotionService.GetEmployeeePromotionDetailsById(EmployeeID);
                return Json(sIVM, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult SaveInformation(PromotionViewModel PromotionVM)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Invalid Data Submitted!", JsonRequestBehavior.AllowGet);
            }
            try
            {

                promotionService.SaveEmployeePromotion(PromotionVM, User.Identity.Name);
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

        public JsonResult GetDesignationList(int EmployeeID)
        {
            return Json(designationService.GetDesignationByEmployeeID(EmployeeID), JsonRequestBehavior.AllowGet);
        }
    }
}