
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

namespace ScopoHR.WebUI.Areas.AdminPanel.Controllers
{
    //[Authorize(Roles = AppRoles.SuperUser + ", " + AppRoles.Admin + "," + AppRoles.Settings)]
    public class GeneralSettingsController : Controller
    {

        private DepartmentService departmentService;
        private LeaveTypeService leaveTypeService;
        private SalaryTypeService salaryTypeService;
        private OfficeTimingService officeTimingService;
        private DesignationService designationService;
        private ProductionService productionService;
        private WorkingShiftsService workingShiftsService;
        private Sys_AttendanceBonusService bonusService;
        private Sys_GraceService graceService;



        public GeneralSettingsController(DepartmentService _departmentService,
                                        DesignationService _designationService,
                                        LeaveTypeService _leaveTypeService,
                                        SalaryTypeService _salaryTypeService,
                                        OfficeTimingService _officeTimingService,
                                        ProductionService _productionService,
                                        WorkingShiftsService _workingShiftsService,
                                        Sys_GraceService graceService,
                                        Sys_AttendanceBonusService bonusService
                                        )
        {
            this.departmentService = _departmentService;
            this.designationService = _designationService;
            this.leaveTypeService = _leaveTypeService;
            this.salaryTypeService = _salaryTypeService;
            this.officeTimingService = _officeTimingService;
            this.productionService = _productionService;
            this.workingShiftsService = _workingShiftsService;
            this.bonusService = bonusService;
            this.graceService = graceService;
        }


        // GET: Settings/Home
        public ActionResult Index()
        {
            return View();
        }


        //Department

        public JsonResult CreateDepartment(DepartmentViewModel departmentVM)
        {
            var id = UserHelper.Instance.Get().BranchId;
            if (ModelState.IsValid)
            {
                if (!departmentService.IsUnique(departmentVM, id))
                {
                    Department dep = departmentService.Create(departmentVM, id, User.Identity.Name);
                    return Json(dep.DepartmentID);
                }
                return Json(true);

            }
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json("Modelstate is not valid");
        }

        public JsonResult UpdateDepartment(DepartmentViewModel departmentVM)
        {
            var id = UserHelper.Instance.Get().BranchId;
            if (ModelState.IsValid)
            {
                if (!departmentService.IsUnique(departmentVM, id))
                {
                    departmentService.Update(departmentVM, id, User.Identity.Name);
                    return Json(new { });
                }
                return Json(true);
            }
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json("Modelstate is not valid");
        }

        public JsonResult GetAllDepartments()
        {
            var all = departmentService.GetAll();
            return Json(all, JsonRequestBehavior.AllowGet);
        }

        //designation

        public JsonResult CreateDesignation(DesignationViewModel designationVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (!designationService.IsExists(designationVM))
                    {
                        Designation des = designationService.Create(designationVM);
                        return Json(des.DepartmentID);
                    }
                    return Json(true);

                }
                catch (Exception ex)
                {
                    return Json("Error occured");
                }
            }
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json("Modelstate is not valid");
        }

        public JsonResult UpdateDesignation(DesignationViewModel designationVM)
        {
            if (ModelState.IsValid)
            {
                if (!designationService.IsExists(designationVM))
                {
                    designationService.Update(designationVM);
                    return Json(new { });
                }
                return Json(true);
            }
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json("Modelstate is not valid");
        }

        public JsonResult GetAllDesignations()
        {
            var all = designationService.GetAll();
            return Json(all, JsonRequestBehavior.AllowGet);
        }

        //Salary type
        public JsonResult CreateSalaryType(SalaryTypeViewModel salaryTypeVM)
        {
            if (ModelState.IsValid)
            {
                if (!salaryTypeService.IsExists(salaryTypeVM))
                {
                    SalaryType st = salaryTypeService.Create(salaryTypeVM, User.Identity.Name);
                    return Json(st.SalaryTypeID);
                }
                return Json(true);

            }
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json("Modelstate is not valid");
        }

        public JsonResult GetAllSalaryTypes()
        {
            var all = salaryTypeService.GetAll();
            return Json(all, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateSalaryType(SalaryTypeViewModel salaryTypeVM)
        {
            if (ModelState.IsValid)
            {
                if (!salaryTypeService.IsExists(salaryTypeVM))
                {
                    salaryTypeService.Update(salaryTypeVM, User.Identity.Name);
                    return Json(new { });
                }
                return Json(true);
            }
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json("Modelstate is not valid");
        }

        //leave type
        public JsonResult CreateLeaveType(LeaveTypeViewModel leaveTypeVM)
        {
            if (ModelState.IsValid)
            {
                if (!leaveTypeService.IsExists(leaveTypeVM))
                {
                    leaveTypeVM.ModifiedBy = User.Identity.Name;
                    LeaveType lt = leaveTypeService.Create(leaveTypeVM);
                    return Json(lt.LeaveTypeID);
                }
                return Json(true);

            }
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json("Modelstate is not valid");
        }

        public JsonResult GetAllLeaveTypes()
        {
            var all = leaveTypeService.GetAll();
            return Json(all, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateLeaveType(LeaveTypeViewModel leaveTypeVM)
        {
            if (ModelState.IsValid)
            {
                if (!leaveTypeService.IsExists(leaveTypeVM))
                {
                    leaveTypeVM.ModifiedBy = User.Identity.Name;
                    leaveTypeService.Update(leaveTypeVM);
                    return Json(new { });
                }
                return Json(true);

            }
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json("Modelstate is not valid");
        }

        //office time
        public JsonResult CreateOfficeTiming(OfficeTimingViewModel officeTimingVM)
        {
            if (ModelState.IsValid)
            {

                var id = UserHelper.Instance.Get().BranchId;
                officeTimingVM.BranchID = id;

                try
                {
                    OfficeTiming ot = officeTimingService.Create(officeTimingVM, User.Identity.Name);
                    return Json(ot);
                }
                catch (Exception ex)
                {
                    return Json("Error occured.");
                }

            }
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json("Modelstate is not valid");
        }

        public JsonResult UpdateOfficeTiming(OfficeTimingViewModel officeTimingVM)
        {
            if (ModelState.IsValid)
            {
                officeTimingService.Update(officeTimingVM, User.Identity.Name);
                return Json(new { });
            }
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json("Modelstate is not valid");
        }

        public JsonResult GetOfficeTime(OfficeTimingViewModel officeTimingVM)
        {
            OfficeTimingViewModel lastOfficeTime = officeTimingService.GetLast();
            return Json(lastOfficeTime, JsonRequestBehavior.AllowGet);
        }

        //Production
        public JsonResult CreateProduction(ProductionFloorLineViewModel productionVM)
        {
            if (ModelState.IsValid)
            {
                if (!productionService.IsProductionExists(productionVM))
                {
                    productionVM.ModifiedBy = User.Identity.Name;
                    ProductionFloorLine fl = productionService.Create(productionVM);
                    return Json(fl.ProductionFloorLineID);
                }
                else
                {
                    return Json(true);
                }

            }
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json("Modelstate is not valid");
        }

        public JsonResult GetAllFloorLines()
        {
            var all = productionService.GetAll();
            return Json(all, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateProduction(ProductionFloorLineViewModel productionVM)
        {
            if (ModelState.IsValid)
            {
                if (!productionService.IsProductionExists(productionVM))
                {
                    productionVM.ModifiedBy = User.Identity.Name;
                    productionService.Update(productionVM);
                    return Json(new { });
                }
                return Json(true);
            }
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json("Modelstate is not valid");
        }

        public JsonResult GetAllShifts()
        {
            var data = workingShiftsService.GetAll();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateWorkShift(WorkingShiftViewModel model)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                var err = ModelState.
                    SelectMany(x => x.Value.Errors.Select(y => y.ErrorMessage))
                    .ToList();
                return Json(err, JsonRequestBehavior.AllowGet);
            }
            workingShiftsService.Update(model);
            return Json("Updated successfully.", JsonRequestBehavior.AllowGet);
        }

        public JsonResult CreateWorkShift(WorkingShiftViewModel model)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                var err = ModelState.
                    SelectMany(x => x.Value.Errors.Select(y => y.ErrorMessage))
                    .ToList();
                return Json(err, JsonRequestBehavior.AllowGet);
            }
            workingShiftsService.Create(model);
            return Json("Created successfully.", JsonRequestBehavior.AllowGet);
        }


        public JsonResult CreateGrace(Sys_GraceViewModel model)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                var err = ModelState.
                    SelectMany(x => x.Value.Errors.Select(y => y.ErrorMessage))
                    .ToList();
                return Json(err, JsonRequestBehavior.AllowGet);
            }
            graceService.CreateGrace(model, User.Identity.Name);
            return Json("Created successfully.", JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateGrace(Sys_GraceViewModel model)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                var err = ModelState.
                    SelectMany(x => x.Value.Errors.Select(y => y.ErrorMessage))
                    .ToList();
                return Json(err, JsonRequestBehavior.AllowGet);
            }
            graceService.UpdateGrace(model,User.Identity.Name);
            return Json("Updated successfully.", JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetGraceData()
        {
            var data = graceService.GetGraceData();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CreateAttendanceBonus(Sys_AttendanceBonusViewModel model)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                var err = ModelState.
                    SelectMany(x => x.Value.Errors.Select(y => y.ErrorMessage))
                    .ToList();
                return Json(err, JsonRequestBehavior.AllowGet);
            }
            bonusService.CreateAttendanceBonus(model, User.Identity.Name);
            return Json("Created successfully.", JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateAttendanceBonus(Sys_AttendanceBonusViewModel model)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                var err = ModelState.
                    SelectMany(x => x.Value.Errors.Select(y => y.ErrorMessage))
                    .ToList();
                return Json(err, JsonRequestBehavior.AllowGet);
            }
            bonusService.UpdateAttendanceBonus(model, User.Identity.Name);
            return Json("Updated successfully.", JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAttendanceBonusData()
        {
            var data = bonusService.GetAttendanceBonusData();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}