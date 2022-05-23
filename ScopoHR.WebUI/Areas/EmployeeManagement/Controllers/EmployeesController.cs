using ScopoHR.Core.Services;
using ScopoHR.Core.ViewModels;
using ScopoHR.Domain.Repositories;
using ScopoHR.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using ScopoHR.Core.Helpers;
using ScopoHR.Core.Common;
using System.Configuration;
using ScopoHR.WebUI.Helpers;

namespace ScopoHR.WebUI.Areas.EmployeeManagement.Controllers
{
    //[Authorize(Roles = AppRoles.SuperUser + ", " + AppRoles.Admin + ","+AppRoles.EmployeeSetup + "," + AppRoles.ChangeEmployeeCardNo + "," + AppRoles.SalaryIncrement)]
    public class EmployeesController : Controller
    {
        private EmployeeService employeeService;
        private DepartmentService departmentService;
        private DesignationService designationService;
        private SalaryMappingService salaryMappingService;
        private SalaryTypeService salaryTypeService;
        private LeaveMappingService leaveMappingService;
        private LeaveTypeService leaveTypeService;
        private ProductionService productionService;
        private YearService yearMappingService;
        private DocumentService documentService;

        public EmployeesController(
                EmployeeService _employeeService, 
                DepartmentService _departmentService, 
                DesignationService _designationService,
                SalaryMappingService _salaryMappingService, 
                SalaryTypeService _salaryTypeService, 
                LeaveMappingService _leaveMappingService, 
                LeaveTypeService _leaveTypeService, 
                ProductionService _productionService,
                YearService _yearMappingService,
                DocumentService _documentService)
        {
            this.employeeService = _employeeService;
            this.departmentService = _departmentService;
            this.designationService = _designationService;
            this.salaryMappingService = _salaryMappingService;
            this.salaryTypeService = _salaryTypeService;
            this.leaveMappingService = _leaveMappingService;
            this.leaveTypeService = _leaveTypeService;
            this.productionService = _productionService;
            this.yearMappingService = _yearMappingService;
            this.documentService = _documentService;

        }

        public ActionResult Index()
        {
            ViewBag.userName = User.Identity.Name;
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
        
        [HttpPost]
        public JsonResult AssignSalary(SalaryMappingViewModel salaryMappingVM)
        {

            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Model State is not valid!");
            }

            try
            {
                salaryMappingService.SaveMapping(salaryMappingVM , User.Identity.Name);
                return Json(true);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message);
            }


        }
        
        [HttpPost]
        public JsonResult SaveEmployee(EmployeeViewModel employeeVM)
        {
            var id = UserHelper.Instance.Get().BranchId;
            var year = yearMappingService.GetYear();
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Invalid Data Submitted!");
            }
            if(year == null)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Year is not opened yeat!");
            }
            try
            {
                if (employeeVM.EmployeeID == 0)
                {                 
                    var empId = employeeService.Create(employeeVM, id, User.Identity.Name);                    
                    leaveMappingService.CreateLeaveMapping(empId, employeeVM.GenderID, year.YearMappingID);
                    return Json("Successfully created!");
                }
                else
                {
                    employeeVM.ModifiedBy = User.Identity.Name;
                    employeeService.Update(employeeVM, id, User.Identity.Name);
                    return Json("Succeessfully updated!");
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message);
            }
        }
                
        public JsonResult GetEmployeeInfoByID(int EmployeeID)
        {
            try
            {
                EmployeeViewModel employeeInfo = employeeService.GetByID(EmployeeID);
                return Json(employeeInfo, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetAssignSalaryInfoByID(int EmployeeID)
        {
            try
            {
                List<SalaryTypeAmountViewModel> salaryInfo = salaryMappingService.GetSalaryMapping(EmployeeID);
                return Json(salaryInfo, JsonRequestBehavior.AllowGet);
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
                var allProductionFloors = productionService.GetAll();
                return Json(allProductionFloors, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        
        public JsonResult createSalaryMapping(SalaryMappingViewModel salaryMappingVM)
        {
            try
            {
                if (!salaryMappingService.IsSalaryMappingExists(salaryMappingVM.EmployeeID))
                {
                    var createSalaryMapping = salaryMappingService.SaveMapping(salaryMappingVM, User.Identity.Name);
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                if (User.Identity.Name=="1701103")
                {
                    var createSalaryMapping = salaryMappingService.SaveMapping(salaryMappingVM, User.Identity.Name);
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                return Json(new {Message="You are not allowed to update salary" }, JsonRequestBehavior.AllowGet);

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

        public JsonResult IsUniqueCardNo(string cardNo)
        {
            if(String.IsNullOrEmpty(cardNo))
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Please enter a valid card no!", JsonRequestBehavior.AllowGet);
            }
            try
            {
                if (!employeeService.IsValidCardNo(cardNo))
                {
                    return Json("Card no is unique!", JsonRequestBehavior.AllowGet);
                }
                Response.StatusCode = (int)HttpStatusCode.PartialContent;
                return Json("Card no already exists!", JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetLeaveMappingByEmployeeId(int id)
        {
            if(id < 1)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Please select an employee!", JsonRequestBehavior.AllowGet);
            }

            try
            {
                return Json(leaveMappingService.GetLeaveMappingByEmployeeId(id), JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult SaveLeaveMapping(List<LeaveDaysViewModel> mapping)
        {
            if(!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Invalid data submitted!");
            }

            try
            {
                if(User.Identity.Name == "1701103")
                {
                    leaveMappingService.UpdateLeaveAllocation(mapping);
                    return Json(true);
                }
                else
                {
                    return Json(false);
                }
               
            }
            catch(Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message);
            }
        }

        public JsonResult GetRecentEmployees()
        {
            return Json(employeeService.GetRecentEmployees(UserHelper.Instance.Get().BranchId), JsonRequestBehavior.AllowGet);
        }    
       
        public JsonResult GetEmployeeDocumentsById(int id)
        {
            var documents = documentService.GetEmployeeDocumentsById(id);
            var locations = DocumentHelper.GetLocations();
            foreach(var doc in documents)
            {                
                doc.Url = locations[doc.Category] + doc.UniqueIdentifier;                
            }
            return Json(documents, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ChangeCardNo()
        {
            return View();
        }
        [HttpPost]
        public JsonResult ModifyCardNo(ChangeCardNoViewModel changeCardNoVM)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Invalid data submitted!");
            }

            try
            {
                changeCardNoVM.LastModified = DateTime.Now;
                changeCardNoVM.ModifiedBy = User.Identity.Name;
                employeeService.ModifyCardNo(changeCardNoVM);
                return Json(true,JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message);
            }
        }

        public JsonResult GetSectionList()
        {
            try
            {
                List<SectionViewModel> list = employeeService.GetAllSectins();
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }



    }
}
