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
    [Authorize(Roles = AppRoles.SuperUser + ", " + AppRoles.Admin + "," + AppRoles.OneTimeEntryUpdate)]
    public class OneTimePunchController : Controller
    {
        private OneTimePunchService oneTimePunchService;
        private ProductionService productionService;
        private AttendanceService attendanceService;
        private OfficeTimingService officeTimingService;

        public OneTimePunchController(OneTimePunchService oneTimePunchService, ProductionService productionService, AttendanceService attendanceService, OfficeTimingService officeTimingService)
        {
            this.oneTimePunchService = oneTimePunchService;
            this.productionService = productionService;
            this.officeTimingService = officeTimingService;
        }
        // GET: EmployeeManagement/OneTimePunch
        public ActionResult Index()
        {
            return View();
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

        public JsonResult GetShift()
        {
            try
            {
                List<DropDownViewModel> ShiftList = officeTimingService.GetWorkingShiftsDropDown();
                return Json(ShiftList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult SearchAttendance(SearchAttendanceViewModel searchVM)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Invalid data submitted");
            }

            try
            {
                var id = UserHelper.Instance.Get().BranchId;
                //var list = attendanceService.GetDailyAttendance(searchVM, id, Page.Size, Page.Size * pageNo, out count);
                var res = oneTimePunchService.GetDailyAttendance(searchVM, id);
               
                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult SaveAttendance(List<AttendanceViewModel> attendance)
        {
            try
            {
                //attendance.Remarks = "Missing Data Update";
                oneTimePunchService.Update(attendance, User.Identity.Name);
                return Json("Time added!", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

    }
}